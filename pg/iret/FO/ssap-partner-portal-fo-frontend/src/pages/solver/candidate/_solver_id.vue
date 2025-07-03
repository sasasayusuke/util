<!-- ソルバー候補・変更画面ページ -->

<template>
  <TemplateSolverCandidateDetail
    :solver="getSolverByIdResponse"
    :tsi-areas="getSelectItemsTsiAreasResponse"
    :issue-map50="getSelectItemsResponse"
    :is-editing="isEditing"
    :is-loading="isLoading.updateSolver"
    :is-delete-modal-open="isDeleteModalOpen"
    :file-refresh="fileRefresh"
    @click:edit="onClickEdit"
    @click:save="onClickSave"
    @click:cancel="onClickCancel"
    @click:positive="onClickPositive"
    @click:negative="onClickNegative"
    @click:deleteSolver="deleteSolver"
    @click:closeDelete="closeDeleteModal"
    @click:delete="openDeleteModal"
    @inputForm="keepInputData"
  />
</template>

<script lang="ts">
import TemplateSolverCandidateDetail from '~/components/solver/templates/SolverCandidateDetail.vue'
import BasePage from '~/common/BasePage'
import {
  DeleteSolverById,
  FileContent,
  GetSolverById,
  GetSolverByIdResponse,
  UpdateSolverById,
} from '~/models/Solver'
import { GetSelectItems, GetSelectItemsResponse } from '~/models/Master'
import { IUpdateSolverRequest } from '~/types/Solver'
import { hasRole } from '~/utils/role-authorizer'
import { ENUM_USER_ROLE } from '~/types/User'
import { meStore, solverCorporationStore } from '~/store'
import { IFile, S3_KEY_TEMPLATE_SOLVER, uploadFileSolver } from '~/utils/upload'
import { deleteFile } from '~/utils/delete'
import { DEFAULT_BIRTHDAY } from '~/const'

export interface isLoading {
  selectItem: boolean
  solver: boolean
  updateSolver: boolean
  deleteSolver: boolean
}

export default BasePage.extend({
  name: 'SolverCandidateDetail',
  components: {
    TemplateSolverCandidateDetail,
  },
  middleware: ['roleCheck'],
  head() {
    return {
      title: this.$t('solver.pages.candidate.name') as string,
    }
  },
  data(): {
    /** GetSelectItems APIのレスポンス */
    getSelectItemsResponse: GetSelectItemsResponse
    /** GetSolverBy APIのレスポンス */
    getSolverByIdResponse: GetSolverByIdResponse
    /** 東証33業種経験/対応可能領域プルダウンアイテム */
    getSelectItemsTsiAreasResponse: GetSelectItemsResponse
    /** ソルバー候補の入力データ（申請用） */
    solverCandidateData: GetSolverByIdResponse
    /** ソルバー候補の入力データ（参照用） */
    solverCandidateInputData: any
    /** 案件ID */
    projectId: string
    /** 案件名 */
    projectName: string
    /** ローディング中か否か */
    isLoading: isLoading
    /** 編集中か否か */
    isEditing: boolean
    /** ソルバー削除モーダルが開いているか否か */
    isDeleteModalOpen: boolean
    /** 「一覧へ戻る」ボタン押下時の遷移先URL */
    backUrl: string
    /** 添付ファイルのリフレッシュ */
    fileRefresh: boolean
  } {
    return {
      /** GetSelectItems APIのレスポンス */
      getSelectItemsResponse: new GetSelectItemsResponse(),
      /** GetSolverBy APIのレスポンス */
      getSolverByIdResponse: new GetSolverByIdResponse(),
      /** 東証33業種経験/対応可能領域プルダウンアイテム */
      getSelectItemsTsiAreasResponse: new GetSelectItemsResponse(),
      /** ソルバー候補の入力データ（申請用） */
      solverCandidateData: new GetSolverByIdResponse(),
      /** ソルバー候補の入力データ（参照用） */
      solverCandidateInputData: new GetSolverByIdResponse(),
      projectId: '',
      /** 案件名 */
      projectName: '',
      /** ローディング中か否か */
      isLoading: {
        selectItem: false,
        solver: false,
        updateSolver: false,
        deleteSolver: false,
      },
      /** 編集中か否か */
      isEditing: false,
      /** ソルバー削除モーダルが開いているか否か */
      isDeleteModalOpen: false,
      /** 「一覧へ戻る」ボタン押下時の遷移先URL */
      backUrl:
        meStore.role === ENUM_USER_ROLE.APT
          ? `/solver/candidate/list/${solverCorporationStore.id}`
          : `/solver/candidate/list/${meStore.solverCorporationId}`,
      fileRefresh: false,
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
    /** GetSelectItems API: 課題マップ50・東証33業種経験/対応可能領域の選択肢を取得する */
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
          this.getSolverByIdResponse = this.initializeData(res.data)
          this.solverCandidateData = JSON.parse(
            JSON.stringify(this.initializeData(res.data))
          )
          this.isLoading.solver = false
        })
        .catch((error) => {
          // responseがundefined（データが存在しない）場合
          // または ステータスが404場合に404にリダイレクト
          if (!error.response || error.response.status === 404) {
            this.$router.push('/404')
          } else {
            this.apiErrorHandle(error)
          }
        })
    },

    /** スクリーニング項目とクライテリア項目の初期値を設定 */
    initializeData(data: any) {
      for (let i = 0; i < 8; i++) {
        const index = i + 1
        const criteria = data[`criteria${index}`]
        if (!criteria) {
          data[`criteria${index}`] = ''
        }
        const screening = data[`screening${index}`]
        if (!screening) {
          data[`screening${index}`] = {
            evaluation: false,
            evidence: '',
          }
        }
      }
      return data
    },

    /** UpdateSolverById API: ソルバー候補1人の情報を更新する */
    async updateSolverById(updateData: IUpdateSolverRequest) {
      this.clearErrorBar()

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
          this.displayLoading([this.getSolverById()])
          this.isEditing = false
        })
        .catch((error) => {
          this.isLoading.updateSolver = false
          // responseがundefined（データが存在しない）場合
          // または ステータスが404場合に404にリダイレクト
          if (!error.response || error.response.status === 404) {
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

    /** DeleteSolverById API: 個人ソルバー情報を削除する */
    async deleteSolverById() {
      this.isLoading.deleteSolver = true
      this.clearErrorBar()

      /**
       * ソルバー情報削除APIを叩く
       * @param リクエストパラメータ
       */
      await DeleteSolverById(
        this.$route.params.solver_id,
        this.getSolverByIdResponse.version
      )
        .then(() => {
          if (!this.isAllowAccess) {
            this.$router.push('/404')
          }
          this.$router.push(this.backUrl)
          this.isLoading.deleteSolver = false
        })
        .catch((error) => {
          if (error.response && error.response.status === 404) {
            this.$router.push('/404')
          } else if (error.response && error.response.status === 409) {
            this.showErrorBar(
              this.$t('solver.pages.utilizationRate.errorMessage.conflict')
            )
            this.apiErrorHandle(error)
          } else {
            this.showErrorBar(this.$t('msg.error.default'))
            this.apiErrorHandle(error)
          }
        })
    },

    // 「変更する」ボタン押下時の処理
    onClickEdit() {
      this.isEditing = true
    },

    // 「保存する」ボタン押下時の処理
    onClickSave() {
      this.saveSolverCandidateInfo()
    },

    // 「キャンセル」ボタン押下時の処理
    onClickCancel() {
      this.fileRefresh = false
      this.getSolverByIdResponse = JSON.parse(
        JSON.stringify(this.solverCandidateData)
      )
      this.fileRefresh = true
      this.isEditing = false
    },

    // コンテナヘッダー「変更する」・「保存する」ボタン押下時の処理
    onClickPositive() {
      if (this.isEditing) {
        this.saveSolverCandidateInfo()
      } else {
        this.isEditing = true
      }
    },

    // コンテナヘッダー「一覧へ戻る」・「キャンセル」ボタン押下時の処理
    onClickNegative() {
      if (this.isEditing) {
        this.onClickCancel()
      } else {
        this.$router.push(this.backUrl)
      }
    },

    // ソルバー削除モーダル「削除」ボタンを押下した時の処理
    deleteSolver() {
      this.deleteFiles()
      this.deleteSolverById()
      this.isDeleteModalOpen = false
    },

    // ソルバー削除モーダル「キャンセル」ボタンを押下した時の処理
    closeDeleteModal() {
      this.isDeleteModalOpen = false
    },

    // 「このソルバーを削除する」ボタンを押下した時の処理
    openDeleteModal() {
      this.isDeleteModalOpen = true
    },

    // formの入力値を保持する処理
    keepInputData(newValue: any) {
      const mergedInputData = {
        ...this.solverCandidateInputData,
        ...newValue,
      }
      this.solverCandidateInputData = mergedInputData
    },

    // ソルバー候補情報を保存する処理
    async saveSolverCandidateInfo() {
      this.isLoading.updateSolver = true

      // 前回最終更新日時から20秒経過していない場合、エラーを表示させ更新できないようにする
      if (
        Date.now() - new Date(this.getSolverByIdResponse.updateAt).getTime() <
        20000
      ) {
        this.showErrorBarWithScrollPageTop(this.$t('msg.error.inProgress'))
        this.isLoading.updateSolver = false
        return
      }
      // 添付ファイルをS3へアップロード
      const results = await this.fileUpload(
        S3_KEY_TEMPLATE_SOLVER.SOLVER_FILES,
        true
      )
      if (results.length > 0) {
        this.solverCandidateInputData.resume =
          results.length > 0
            ? [...this.solverCandidateInputData.resume, ...results]
            : [...this.solverCandidateInputData.resume]
      } else {
        this.solverCandidateInputData.resume = [
          ...this.solverCandidateInputData.resume,
        ]
      }

      // 個人ソルバー画像をS3へアップロード
      const imageResults = await this.fileUpload(
        S3_KEY_TEMPLATE_SOLVER.SOLVER_PHOTOS,
        false
      )
      if (imageResults.length > 0) {
        this.solverCandidateInputData.facePhoto = imageResults[0]
      }

      const updateData = this.generateUpdateData()
      await this.updateSolverById(updateData)
      this.isLoading.updateSolver = false
      this.refresh()
    },

    // ソルバー候補情報更新用のデータを生成する処理
    generateUpdateData() {
      const updateData = {
        mode: 'update_candidate',
        name: this.solverCandidateInputData.name,
        nameKana: this.solverCandidateInputData.nameKana,
        title: this.solverCandidateInputData.title,
        email: this.solverCandidateInputData.email,
        phone: this.solverCandidateInputData.phone,
        issueMap50: this.solverCandidateInputData.issueMap50,
        corporateId: this.solverCandidateInputData.corporateId,
        sex: this.solverCandidateInputData.sex,
        birthDay: this.solverCandidateInputData.birthDay
          ? this.solverCandidateInputData.birthDay
          : DEFAULT_BIRTHDAY,
        operatingStatus: this.solverCandidateInputData.operatingStatus,
        facePhoto: this.solverCandidateInputData.facePhoto,
        resume: this.solverCandidateInputData.resume,
        academicBackground: this.solverCandidateInputData.academicBackground,
        workHistory: this.solverCandidateInputData.workHistory,
        isConsultingFirm: this.solverCandidateInputData.isConsultingFirm,
        specializedThemes: this.solverCandidateInputData.specializedThemes,
        mainAchievements: this.solverCandidateInputData.mainAchievements,
        providedOperatingRate:
          this.solverCandidateInputData.providedOperatingRate,
        providedOperatingRateNext:
          this.solverCandidateInputData.providedOperatingRateNext,
        operationProspectsMonthAfterNext:
          this.solverCandidateInputData.operationProspectsMonthAfterNext,
        pricePerPersonMonth:
          this.solverCandidateInputData.pricePerPersonMonth || 0,
        pricePerPersonMonthLower:
          this.solverCandidateInputData.pricePerPersonMonthLower || 0,
        hourlyRate: this.solverCandidateInputData.hourlyRate || 0,
        hourlyRateLower: this.solverCandidateInputData.hourlyRateLower || 0,
        englishLevel: this.solverCandidateInputData.englishLevel,
        tsiAreas: this.solverCandidateInputData.tsiAreas,
        notes: this.solverCandidateInputData.notes,
        isSolver: false,
        deleteSolverApplicationIds:
          this.solverCandidateInputData.deleteSolverApplicationIds || [],
        registrationStatus: this.solverCandidateData.registrationStatus,
        screening1: this.solverCandidateInputData.screening1,
        screening2: this.solverCandidateInputData.screening2,
        screening3: this.solverCandidateInputData.screening3,
        screening4: this.solverCandidateInputData.screening4,
        screening5: this.solverCandidateInputData.screening5,
        screening6: this.solverCandidateInputData.screening6,
        screening7: this.solverCandidateInputData.screening7,
        screening8: this.solverCandidateInputData.screening8,
        criteria1: this.solverCandidateInputData.criteria1,
        criteria2: this.solverCandidateInputData.criteria2,
        criteria3: this.solverCandidateInputData.criteria3,
        criteria4: this.solverCandidateInputData.criteria4,
        criteria5: this.solverCandidateInputData.criteria5,
        criteria6: this.solverCandidateInputData.criteria6,
        criteria7: this.solverCandidateInputData.criteria7,
        criteria8: this.solverCandidateInputData.criteria8,
      }
      return updateData
    },

    // 個人ソルバー画像・添付ファイルをS3にアップロードする処理
    async fileUpload(template: string, isFile: boolean) {
      const results: { fileName: string; path: string }[] = []
      const promises: any[] = []
      if (
        isFile &&
        this.solverCandidateInputData.files &&
        this.solverCandidateInputData.files.length > 0
      ) {
        this.solverCandidateInputData.files.forEach((file: IFile) => {
          const result = {
            fileName: file.name,
            path: '',
          }
          const extension = file.name.split('.').pop()
          results.push(result)
          promises.push(
            uploadFileSolver(
              template,
              file,
              this.solverCandidateInputData.corporateId,
              this.solverCandidateInputData.fileKeyId,
              extension
            )
          )
        })
      } else if (
        !isFile &&
        this.solverCandidateInputData.image &&
        this.solverCandidateInputData.image.name
      ) {
        const result = {
          fileName: this.solverCandidateInputData.image.name,
          path: '',
        }
        const extension = this.solverCandidateInputData.image.name
          .split('.')
          .pop()
        results.push(result)
        promises.push(
          uploadFileSolver(
            template,
            this.solverCandidateInputData.image,
            this.solverCandidateInputData.corporateId,
            this.solverCandidateInputData.fileKeyId,
            extension
          )
        )
      }

      return await Promise.all(promises)
        .then((responses) => {
          responses.forEach((response: any, index: number) => {
            results[index].path = response.Key as string
          })
          return results
        })
        .catch((err) => {
          throw new Error(err)
        })
    },

    // 個人ソルバー画像・添付ファイルをS3から削除する処理
    deleteFiles() {
      // 添付ファイルをS3から削除
      if (
        this.solverCandidateInputData.resume &&
        this.solverCandidateInputData.resume.length > 0
      ) {
        this.solverCandidateInputData.resume.forEach(
          (file: { fileName: string; path: string }) => {
            deleteFile(file.path)
              .then(() => {
                this.$logger.info('Resumes on S3 has been deleted')
              })
              .catch((err) => {
                throw new Error(err)
              })
          }
        )
      }

      // 個人ソルバー画像をS3から削除
      if (
        this.solverCandidateInputData.facePhoto &&
        this.solverCandidateInputData.facePhoto.fileName
      ) {
        deleteFile(this.solverCandidateInputData.facePhoto.path)
          .then(() => {
            this.$logger.info('Face photos on S3 has been deleted')
          })
          .catch((err) => {
            throw new Error(err)
          })
      }
    },
    // 登録済みのファイルと登録するファイルの差分があればS3から削除する
    async deleteDiffFiles(updateData: IUpdateSolverRequest) {
      // 更新前の添付ファイル
      const beforeResume: FileContent[] = this.solverCandidateData.resume || []
      // 更新前の個人ソルバー画像
      const beforeFacePhoto: FileContent | null =
        this.solverCandidateData.facePhoto || null

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
    /**
     * 再度リロード
     */
    refresh() {
      window.location.reload()
    },
  },
})
</script>
