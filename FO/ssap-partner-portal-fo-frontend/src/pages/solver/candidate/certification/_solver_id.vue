<!-- 個人ソルバー登録申請画面ページ -->

<template>
  <TemplateSolverApplication
    :issue-map50="getSelectItemsResponse"
    :tsi-areas="getSelectItemsTsiAreasResponse"
    :is-editing="isEditing"
    :is-finished="isFinished"
    :is-loading="isLoading.updateSolver"
    :is-loading-temporary="isLoading.updateSolverTemporary"
    :solver-id="getSolverByIdResponse.id"
    :solver="getSolverByIdResponse"
    :is-snackbar-open="isSnackbarOpen"
    :is-display-application-project="false"
    :is-registration-application="true"
    :file-refresh="fileRefresh"
    @click:confirm="toConfirm"
    @click:backToInput="toInput"
    @click:apply="finishApplication"
    @click:saveTemporary="saveTemporary"
    @inputForm="keepInputData"
  />
</template>

<script lang="ts">
import { ManagedUpload } from 'aws-sdk/clients/s3'
import TemplateSolverApplication from '~/components/solver/templates/SolverApplication.vue'
import BasePage from '~/common/BasePage'
import {
  GetSolverById,
  GetSolverByIdResponse,
  UpdateSolverById,
  UpdateSolverRequest,
  FileContent,
} from '~/models/Solver'
import { hasRole } from '~/utils/role-authorizer'
import { ENUM_USER_ROLE } from '~/types/User'
import { IUpdateSolverRequest, ISolverIFile } from '~/types/Solver'
import { solverCorporationStore } from '~/store'
import { GetSelectItems, GetSelectItemsResponse } from '~/models/Master'
import {
  IFile,
  S3_KEY_TEMPLATE_SOLVER,
  uploadFileSolver,
  generateKeySolver,
} from '~/utils/upload'
import { deleteFile } from '~/utils/delete'
import { DEFAULT_BIRTHDAY } from '~/const'

export interface isLoading {
  selectItem: boolean
  updateSolver: boolean
  updateSolverTemporary: boolean
  tsiAreas: boolean
  solver: boolean
}

export default BasePage.extend({
  name: 'SolverIndividualApplication',
  components: {
    TemplateSolverApplication,
  },
  middleware: ['roleCheck'],
  head() {
    return {
      title: this.$t('solver.pages.candidate.certification.name') as string,
    }
  },
  data(): {
    /** GetSelectItems APIのレスポンス */
    getSelectItemsResponse: GetSelectItemsResponse
    /** 東証33業種経験/対応可能領域プルダウンアイテム */
    getSelectItemsTsiAreasResponse: GetSelectItemsResponse
    /** GetSolverById APIのレスポンス */
    getSolverByIdResponse: GetSolverByIdResponse
    /** 個人ソルバーの入力データ（申請用） */
    solver: IUpdateSolverRequest
    /** 個人ソルバーの入力データ（参照用） */
    solverInputData: any
    /** 登録済の添付ファイル */
    registeredFiles: any
    /** 法人内ID */
    corporateId: string
    /** ローディング中か否か */
    isLoading: isLoading
    /** 編集中か否か */
    isEditing: boolean
    /** 申請が完了したか否か */
    isFinished: boolean
    /** スナックバーを表示するか否か */
    isSnackbarOpen: boolean
    /** 添付ファイルのリフレッシュ */
    fileRefresh: boolean
    /** 更新前データ */
    beforeSolverData: GetSolverByIdResponse
  } {
    return {
      /** GetSelectItems APIのレスポンス */
      getSelectItemsResponse: new GetSelectItemsResponse(),
      /** 東証33業種経験/対応可能領域プルダウンアイテム */
      getSelectItemsTsiAreasResponse: new GetSelectItemsResponse(),
      /** GetSolverById APIのレスポンス */
      getSolverByIdResponse: new GetSolverByIdResponse(),
      /** 個人ソルバーの入力データ（申請用） */
      solver: new UpdateSolverRequest(),
      /** 個人ソルバーの入力データ（参照用） */
      solverInputData: {
        ...new GetSolverByIdResponse(),
        solverApplicationId: '',
        solverApplicationName: '',
      },
      registeredFiles: [],
      /** 法人内ID */
      corporateId: solverCorporationStore.id,
      /** ローディング中か否か */
      isLoading: {
        selectItem: false,
        updateSolver: false,
        updateSolverTemporary: false,
        tsiAreas: false,
        solver: false,
      },
      /** 編集中か否か */
      isEditing: true,
      /** 申請が完了したか否か */
      isFinished: false,
      /** スナックバーを表示するか否か */
      isSnackbarOpen: false,
      /** スナックバーを表示するか否か */
      fileRefresh: false,
      /** 更新前データ */
      beforeSolverData: new GetSolverByIdResponse(),
    }
  },
  async mounted() {
    await this.displayLoading([
      this.getSelectItems('issue_map50'),
      this.getSelectItems('industry_segment'),
    ])

    await this.displayLoading([this.getSolverById()])
  },
  computed: {
    /** アライアンス担当と法人ソルバー担当のみが画面表示可能 */
    isAllowAccess() {
      return hasRole([ENUM_USER_ROLE.APT, ENUM_USER_ROLE.SOLVER_STAFF])
    },
  },
  methods: {
    /** GetSelectItems API: 課題マップ50/東証33業種経験/対応可能領域の選択肢を取得する */
    async getSelectItems(dataTypeValue: string) {
      this.isLoading.selectItem = true
      const request = { dataType: dataTypeValue }

      /**
       * 選択肢取得APIを叩く
       * @param リクエストパラメータ
       */
      await GetSelectItems(request)
        .then((res) => {
          if (!this.isAllowAccess) {
            this.$router.push('/404')
          }
          if (dataTypeValue === 'issue_map50') {
            this.getSelectItemsResponse = res.data
          } else {
            this.getSelectItemsTsiAreasResponse = res.data
          }
          this.isLoading.selectItem = false
        })
        .catch((error) => {
          this.isLoading.selectItem = false
          if (error.response && error.response.status === 404) {
            this.$router.push('/404')
          } else {
            this.apiErrorHandle(error)
          }
        })
    },

    /** UpdateSolverById API: ソルバー候補1人の情報を更新する */
    async updateSolverById(isApplication: boolean) {
      this.clearErrorBar()
      // ファイルアップロード
      const fileUploadResult: ISolverIFile[] = await this.fileUpload()

      const updateData: IUpdateSolverRequest = this.generateApplyData(
        isApplication,
        fileUploadResult
      )

      /**
       * ソルバー情報更新APIを叩く
       * @param リクエストパラメータ
       */
      await UpdateSolverById(
        this.$route.params.solver_id,
        this.getSolverByIdResponse.version,
        updateData
      )
        .then(async () => {
          if (!this.isAllowAccess) {
            this.$router.push('/404')
          }
          await this.deleteDiffFiles(updateData)
          if (isApplication) {
            this.isFinished = true
          } else {
            this.displayLoading([this.getSolverById()])
            this.isSnackbarOpen = true
          }
        })
        .catch((error) => {
          this.$set(this.isLoading, 'updateSolver', false)
          this.$set(this.isLoading, 'updateSolverTemporary', false)
          if (error.response && error.response.status === 404) {
            this.$router.push('/404')
          } else if (error.response && error.response.status === 409) {
            this.showErrorBar(
              this.$t('solver.pages.utilizationRate.errorMessage.conflict')
            )
            this.apiErrorHandle(error)
          } else {
            this.showErrorBar(
              this.$t('solver.pages.utilizationRate.errorMessage.conflict')
            )
            this.apiErrorHandle(error)
          }
        })
    },

    // ファイルアップロード
    async fileUpload(): Promise<ISolverIFile[]> {
      const promises: any[] = []
      const solverIFiles: ISolverIFile[] = []
      const data = this.solverInputData
      if (data.image && data.image.name) {
        solverIFiles.push({
          fileName: data.image.name,
          path: this.createFileKey(data.image, data.fileKeyId, true),
        })
        promises.push(this.createUploadFunc(data.image, data.fileKeyId, true))
      }
      if (data.files) {
        data.files.forEach((file: IFile) => {
          solverIFiles.push({
            fileName: file.name,
            path: this.createFileKey(file, data.fileKeyId, false),
          })
          promises.push(this.createUploadFunc(file, data.fileKeyId, false))
        })
      }
      const results = await Promise.all(promises)
      const keys = results.map((d) => d.key || d.Key)
      return solverIFiles.filter((file: ISolverIFile) =>
        keys.includes(file.path)
      )
    },

    // 個人ソルバー申請用のデータを生成する処理
    generateApplyData(
      isApplication: boolean,
      fileUploadResult: ISolverIFile[]
    ): IUpdateSolverRequest {
      let registrationStatusValue = ''
      if (isApplication) {
        registrationStatusValue = 'saved'
      } else {
        registrationStatusValue = 'temporary_saving'
      }

      const uploadedFile: ISolverIFile[] = fileUploadResult.filter(
        (file: ISolverIFile) =>
          file.path.includes(this.solverInputData.fileKeyId)
      )
      // 個人画像設定
      const facePhotoList: ISolverIFile[] = uploadedFile.filter(
        (file: ISolverIFile) => file.path.includes('photos')
      )
      const facePhoto: ISolverIFile | undefined =
        facePhotoList && facePhotoList.length > 0
          ? facePhotoList[0]
          : this.solverInputData.facePhoto &&
            this.solverInputData.facePhoto.fileName
          ? this.solverInputData.facePhoto
          : undefined
      // 添付ファイル設定
      const resume: ISolverIFile[] = this.solverInputData.resume.concat(
        uploadedFile.filter((file: ISolverIFile) =>
          file.path.includes('resumes')
        )
      )

      const applyData: IUpdateSolverRequest = {
        mode: 'register_solver',
        name: this.solverInputData.name,
        nameKana: this.solverInputData.nameKana,
        solverApplicationId: this.solverInputData.solverApplicationId,
        solverApplicationName: this.solverInputData.solverApplicationName,
        title: this.solverInputData.title,
        email: this.solverInputData.email,
        phone: this.solverInputData.phone,
        issueMap50: this.solverInputData.issueMap50,
        corporateId: this.corporateId,
        sex: this.solverInputData.sex,
        birthDay: this.solverInputData.birthDay
          ? this.solverInputData.birthDay
          : DEFAULT_BIRTHDAY,
        operatingStatus: this.solverInputData.operatingStatus,
        facePhoto,
        resume,
        academicBackground: this.solverInputData.academicBackground,
        workHistory: this.solverInputData.workHistory,
        isConsultingFirm: this.solverInputData.isConsultingFirm,
        specializedThemes: this.solverInputData.specializedThemes,
        mainAchievements: this.solverInputData.mainAchievements,
        providedOperatingRate: this.solverInputData.providedOperatingRate,
        providedOperatingRateNext:
          this.solverInputData.providedOperatingRateNext,
        operationProspectsMonthAfterNext:
          this.solverInputData.operationProspectsMonthAfterNext,
        pricePerPersonMonth: this.solverInputData.pricePerPersonMonth,
        pricePerPersonMonthLower: this.solverInputData.pricePerPersonMonthLower,
        hourlyRate: this.solverInputData.hourlyRate,
        hourlyRateLower: this.solverInputData.hourlyRateLower,
        englishLevel: this.solverInputData.englishLevel,
        tsiAreas: this.solverInputData.tsiAreas,
        screening1: this.solverInputData.screening1,
        screening2: this.solverInputData.screening2,
        screening3: this.solverInputData.screening3,
        screening4: this.solverInputData.screening4,
        screening5: this.solverInputData.screening5,
        screening6: this.solverInputData.screening6,
        screening7: this.solverInputData.screening7,
        screening8: this.solverInputData.screening8,
        criteria1: this.solverInputData.criteria1,
        criteria2: this.solverInputData.criteria2,
        criteria3: this.solverInputData.criteria3,
        criteria4: this.solverInputData.criteria4,
        criteria5: this.solverInputData.criteria5,
        criteria6: this.solverInputData.criteria6,
        criteria7: this.solverInputData.criteria7,
        criteria8: this.solverInputData.criteria8,
        notes: this.solverInputData.notes,
        isSolver: false,
        registrationStatus: registrationStatusValue,
      }

      // 空のデータを削除
      ;(Object.keys(applyData) as (keyof IUpdateSolverRequest)[]).forEach(
        (key) => {
          if (
            applyData[key] === '' ||
            typeof applyData[key] === 'undefined' ||
            (Array.isArray(applyData[key]) &&
              (applyData[key] as any[]).length === 0)
          ) {
            delete applyData[key]
          }
        }
      )

      applyData.facePhoto = facePhoto
      applyData.resume = applyData.resume ? applyData.resume : []
      return applyData
    },

    // 非同期でアップロード用の関数を作成
    createUploadFunc(
      file: IFile,
      fileKeyId: string,
      isPhoto: boolean
    ): Promise<ManagedUpload.SendData> {
      const extension = file.name.split('.').pop()
      return uploadFileSolver(
        isPhoto
          ? S3_KEY_TEMPLATE_SOLVER.SOLVER_PHOTOS
          : S3_KEY_TEMPLATE_SOLVER.SOLVER_FILES,
        file,
        this.corporateId,
        fileKeyId,
        extension
      )
    },

    // S3キーを作成
    createFileKey(file: IFile, fileKeyId: string, isPhoto: boolean) {
      const extension = file.name.split('.').pop()
      return generateKeySolver(
        isPhoto
          ? S3_KEY_TEMPLATE_SOLVER.SOLVER_PHOTOS
          : S3_KEY_TEMPLATE_SOLVER.SOLVER_FILES,
        this.corporateId,
        fileKeyId,
        file.name.replace(`.${extension}`, ''),
        file.uploadDatetime,
        extension
      )
    },

    /** GetSolverById API: 個人ソルバー1人の情報を取得する */
    async getSolverById() {
      this.isLoading.solver = true
      const id = this.$route.params.solver_id

      /**
       * ソルバー情報取得APIを叩く
       * @param リクエストパラメータ
       */
      await GetSolverById(id)
        .then((res) => {
          if (!this.isAllowAccess) {
            this.$router.push('/404')
          }
          const responseData = new GetSolverByIdResponse()
          Object.keys(res.data).forEach((key) => {
            if ((res.data as any)[key] !== null) {
              ;(responseData as any)[key] = (res.data as any)[key]
            }
          })
          this.getSolverByIdResponse = responseData
          this.solverInputData = responseData
          this.beforeSolverData = JSON.parse(JSON.stringify(res.data))
          this.registeredFiles = [...responseData.resume]
          this.isLoading.solver = false
        })
        .catch((error) => {
          this.isLoading.solver = false
          if (error.response && error.response.status === 404) {
            this.$router.push('/404')
          } else {
            this.apiErrorHandle(error)
          }
        })
    },

    // 「確認画面に進む」ボタン押下時の処理
    toConfirm() {
      this.clearErrorBar()
      this.isEditing = false
    },

    // 「入力画面に戻る」ボタン押下時の処理
    toInput() {
      this.isEditing = true
    },

    // 一時保存をする処理
    async saveTemporary() {
      this.fileRefresh = false
      this.isSnackbarOpen = false
      this.isLoading.updateSolverTemporary = true
      await this.updateSolverById(false)
      this.isLoading.updateSolverTemporary = false
      this.fileRefresh = true
    },

    // 個人ソルバーを申請する処理
    async finishApplication() {
      this.isLoading.updateSolver = true
      await this.updateSolverById(true)
      this.isLoading.updateSolver = false
    },

    // formの入力値を保持する処理
    keepInputData(newValue: any) {
      this.solverInputData = { ...this.solverInputData, ...newValue }
    },

    // 登録済みのファイルと登録するファイルの差分があればS3から削除する
    async deleteDiffFiles(updateData: IUpdateSolverRequest) {
      // 更新前の添付ファイル
      const beforeResume: FileContent[] = this.beforeSolverData.resume || []
      // 更新前の個人ソルバー画像
      const beforeFacePhoto: FileContent | null =
        this.beforeSolverData.facePhoto || null

      // 更新後の添付ファイル
      const updateResumePath: string[] = (updateData.resume || []).map(
        (d) => d.path
      )
      // 更新後の個人ソルバー画像
      const updateFacePhoto: FileContent | null = updateData.facePhoto || null

      // 削除対象の添付ファイルパス
      const deletFilePaths: string[] = []
      beforeResume.forEach((d) => {
        if (!updateResumePath.includes(d.path)) {
          deletFilePaths.push(d.path)
        }
      })
      // 削除対象の個人ソルバー画像
      if (beforeFacePhoto !== null && updateFacePhoto === null) {
        deletFilePaths.push(beforeFacePhoto.path)
      } else if (
        beforeFacePhoto !== null &&
        updateFacePhoto !== null &&
        beforeFacePhoto.path !== updateFacePhoto.path
      ) {
        deletFilePaths.push(beforeFacePhoto.path)
      }

      await Promise.all(
        deletFilePaths.map(async (p) => {
          await deleteFile(p)
          this.$logger.info('Resume on S3 has been deleted')
        })
      )
    },
  },
})
</script>
