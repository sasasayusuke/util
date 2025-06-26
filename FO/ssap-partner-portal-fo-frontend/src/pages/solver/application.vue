<!-- 新規個人ソルバー申請画面ページ -->

<template>
  <TemplateSolverApplication
    :issue-map50="getSelectItemsResponse"
    :tsi-areas="getSelectItemsTsiAreasResponse"
    :is-editing="isEditing"
    :is-finished="isFinished"
    :is-loading="isLoading.createSolver"
    :is-loading-temporary="isLoading.createSolverTemporary"
    :is-snackbar-open="isSnackbarOpen"
    @click:confirm="toConfirm"
    @click:backToInput="toInput"
    @click:apply="finishApplication"
    @click:saveTemporary="saveTemporary"
    @inputForm="keepInputData"
  />
</template>

<script lang="ts">
import { ManagedUpload } from 'aws-sdk/clients/s3'
import { v4 as uuidv4 } from 'uuid'
import TemplateSolverApplication from '~/components/solver/templates/SolverApplication.vue'
import BasePage from '~/common/BasePage'
import { CreateSolver, GetSolverByIdResponse } from '~/models/Solver'
import { hasRole } from '~/utils/role-authorizer'
import { ENUM_USER_ROLE } from '~/types/User'
import { ISolverInfo, ISolverIFile } from '~/types/Solver'
import { solverCorporationStore } from '~/store'
import { GetSelectItems, GetSelectItemsResponse } from '~/models/Master'
import {
  IFile,
  S3_KEY_TEMPLATE_SOLVER,
  uploadFileSolver,
  generateKeySolver,
} from '~/utils/upload'
import { DEFAULT_BIRTHDAY } from '~/const'

export interface isLoading {
  selectItem: boolean
  createSolver: boolean
  createSolverTemporary: boolean
  tsiAreas: boolean
}

export default BasePage.extend({
  name: 'NewSolverIndividualApplication',
  components: {
    TemplateSolverApplication,
  },
  middleware: ['roleCheck'],
  head() {
    return {
      title: this.$t('solver.pages.application.name') as string,
    }
  },
  data(): {
    /** GetSelectItems APIのレスポンス */
    getSelectItemsResponse: GetSelectItemsResponse
    /** 東証33業種経験/対応可能領域プルダウンアイテム */
    getSelectItemsTsiAreasResponse: GetSelectItemsResponse
    /** 個人ソルバーの入力データ（申請用） */
    solver: ISolverInfo[]
    /** 個人ソルバーの入力データ（参照用） */
    solverInputData: any
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
  } {
    return {
      /** GetSelectItems APIのレスポンス */
      getSelectItemsResponse: new GetSelectItemsResponse(),
      /** 東証33業種経験/対応可能領域プルダウンアイテム */
      getSelectItemsTsiAreasResponse: new GetSelectItemsResponse(),
      /** 個人ソルバーの入力データ（申請用） */
      solver: [],
      /** 個人ソルバーの入力データ（参照用） */
      solverInputData: {
        ...new GetSolverByIdResponse(),
        solverApplicationId: '',
        solverApplicationName: '',
      },
      /** 法人内ID */
      corporateId: solverCorporationStore.id,
      /** ローディング中か否か */
      isLoading: {
        selectItem: false,
        createSolver: false,
        createSolverTemporary: false,
        tsiAreas: false,
      },
      /** 編集中か否か */
      isEditing: true,
      /** 申請が完了したか否か */
      isFinished: false,
      /** スナックバーを表示するか否か */
      isSnackbarOpen: false,
    }
  },
  mounted() {
    this.displayLoading([
      this.getSelectItems('issue_map50'),
      this.getSelectItems('industry_segment'),
    ])
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
          if (error.response && error.response.status === 404) {
            this.$router.push('/404')
          } else {
            this.apiErrorHandle(error)
          }
        })
    },

    /** CreateSolver API: 1人もしくは複数人の個人ソルバー情報を登録する */
    async createSolver(isApplication: boolean) {
      this.clearErrorBar()
      // ファイルアップロード
      const fileUploadResult: ISolverIFile[] = await this.fileUpload()
      // リクエストデータ作成
      const request = {
        solversInfo: this.generateApplyData(isApplication, fileUploadResult),
      }
      /**
       * ソルバー情報登録APIを叩く
       * @param リクエストパラメータ
       */
      await CreateSolver(request)
        .then(() => {
          if (!this.isAllowAccess) {
            this.$router.push('/404')
          }
          if (isApplication) {
            this.$set(this.isLoading, 'createSolver', false)
            this.isFinished = true
          } else {
            // 一時保存の場合
            this.isSnackbarOpen = true
            this.isLoading.createSolverTemporary = true
            // 3秒後に自動で個人ソルバー一覧画面にリダイレクト
            setTimeout(() => {
              this.$router.push(`/solver/list/${solverCorporationStore.id}`)
            }, 3000)
          }
        })
        .catch((error) => {
          this.$set(this.isLoading, 'createSolver', false)
          this.isLoading.createSolverTemporary = false
          if (error.response && error.response.status === 404) {
            this.$router.push('/404')
          } else {
            this.showErrorBar(this.$t('msg.error.default'))
            this.apiErrorHandle(error)
          }
        })
    },

    // ファイルアップロード
    async fileUpload(): Promise<ISolverIFile[]> {
      const promises: any[] = []
      const solverIFiles: ISolverIFile[] = []
      this.solverInputData.fileKeyId = uuidv4()
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
    ): ISolverInfo[] {
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
        facePhotoList && facePhotoList.length > 0 ? facePhotoList[0] : undefined
      // 添付ファイル設定
      const resume: ISolverIFile[] = uploadedFile.filter((file: ISolverIFile) =>
        file.path.includes('resumes')
      )
      const applyData: ISolverInfo = {
        // 入力確認画面で、S3にファイルをアップロードするためにIDを生成しているが、新規作成時はIDをnullでリクエストする。
        // 脆弱性診断で問題があったため対応（https://sonystaqct.backlog.com/view/STADX-1702#comment-490928973）
        // ※ S3のファイルバスのIDと、新規登録ユーザーのIDが異なる
        id: undefined,
        mode: 'create_solver',
        isRegisteredSolver: false,
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
        pricePerPersonMonth: this.solverInputData.pricePerPersonMonth || 0,
        pricePerPersonMonthLower:
          this.solverInputData.pricePerPersonMonthLower || 0,
        hourlyRate: this.solverInputData.hourlyRate || 0,
        hourlyRateLower: this.solverInputData.hourlyRateLower || 0,
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
        registrationStatus: registrationStatusValue,
        fileKeyId: this.solverInputData.fileKeyId,
      }

      // 空のデータを削除
      ;(Object.keys(applyData) as (keyof ISolverInfo)[]).forEach((key) => {
        if (
          applyData[key] === '' ||
          typeof applyData[key] === 'undefined' ||
          (Array.isArray(applyData[key]) &&
            (applyData[key] as any[]).length === 0)
        ) {
          delete applyData[key]
        }
      })

      return [applyData]
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

    // 「確認画面に進む」ボタン押下時の処理
    toConfirm() {
      this.isEditing = false
    },

    // 「入力画面に戻る」ボタン押下時の処理
    toInput() {
      this.isEditing = true
    },

    // 一時保存をする処理
    async saveTemporary() {
      this.isSnackbarOpen = false
      this.isLoading.createSolverTemporary = true
      await this.createSolver(false)
    },

    // 個人ソルバーを申請する処理
    finishApplication() {
      this.$set(this.isLoading, 'createSolver', true)
      this.createSolver(true).then(() => {
        this.$set(this.isLoading, 'createSolver', false)
      })
    },

    // formの入力値を保持する処理
    keepInputData(newValue: any) {
      this.solverInputData = { ...this.solverInputData, ...newValue }
    },
  },
})
</script>
