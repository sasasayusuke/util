<!-- 個人ソルバー詳細・変更画面ページ -->

<template>
  <TemplateSolverIdDetail
    :solver="getSolverByIdResponse"
    :tsi-areas="getSelectItemsTsiAreasResponse"
    :issue-map50="getSelectItemsResponse"
    :is-editing="isEditing"
    :is-loading="isLoading.updateSolver"
    :is-delete-modal-open="isDeleteModalOpen"
    :is-loading-temporary="isLoading.temporary"
    :is-snackbar-open="isSnackbarOpen"
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
    @temporarySaveAction="temporarySave"
    @updateSnackBarOpen="updateSnackBarOpen"
  />
</template>

<script lang="ts">
import TemplateSolverIdDetail from '~/components/solver/templates/SolverIdDetail.vue'
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
  temporary: boolean
}

export default BasePage.extend({
  name: 'SolverDetail',
  components: {
    TemplateSolverIdDetail,
  },
  middleware: ['roleCheck'],
  head() {
    return {
      title: this.$t('solver.pages.solverDetail.name') as string,
    }
  },
  data(): {
    /** GetSelectItems APIのレスポンス */
    getSelectItemsResponse: GetSelectItemsResponse
    /** GetSolverBy APIのレスポンス */
    getSolverByIdResponse: GetSolverByIdResponse
    /** 東証33業種経験/対応可能領域プルダウンアイテム */
    getSelectItemsTsiAreasResponse: GetSelectItemsResponse
    /** 個人ソルバーの入力データ（申請用） */
    solverCandidateData: GetSolverByIdResponse
    /** 個人ソルバーの入力データ（参照用） */
    solverInputData: any
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
    /** スナックバー表示フラグ */
    isSnackbarOpen: boolean
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
      /** 個人ソルバーの入力データ（申請用） */
      solverCandidateData: new GetSolverByIdResponse(),
      /** 個人ソルバーの入力データ（参照用） */
      solverInputData: {
        ...new GetSolverByIdResponse(),
        solverApplicationId: '',
        solverApplicationName: '',
      },
      projectId: '',
      /** 案件名 */
      projectName: '',
      /** ローディング中か否か */
      isLoading: {
        selectItem: false,
        solver: false,
        updateSolver: false,
        deleteSolver: false,
        temporary: false,
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
      isSnackbarOpen: false,
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
      const id = this.$route.params.solverId

      /**
       * ソルバー情報取得APIを叩く
       * @param リクエストパラメータ
       */
      await GetSolverById(id)
        .then((res) => {
          if (!this.isAllowAccess) {
            this.$router.push('/404')
          }
          this.getSolverByIdResponse = res.data
          this.solverCandidateData = JSON.parse(JSON.stringify(res.data))
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

    /** UpdateSolverById API: ソルバー候補1人の情報を更新する */
    async updateSolverById(
      updateData: IUpdateSolverRequest,
      isTemporary: boolean = false
    ): Promise<boolean> {
      let result = false
      if (isTemporary) {
        this.isLoading.temporary = true
      } else {
        this.isLoading.updateSolver = true
      }

      this.clearErrorBar()

      /**
       * ソルバー情報更新APIを叩く
       * @param リクエストパラメータ
       */
      await UpdateSolverById(
        this.$route.params.solverId,
        this.getSolverByIdResponse.version,
        updateData
      )
        .then(async () => {
          if (!this.isAllowAccess) {
            this.$router.push('/404')
          }
          await this.deleteDiffFiles(updateData)
          this.isEditing = isTemporary
          result = true
        })
        .catch((error) => {
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
          result = false
        })
      if (isTemporary) {
        this.isLoading.temporary = false
      } else {
        this.isLoading.updateSolver = false
      }
      return result
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
        this.$route.params.solverId,
        this.getSolverByIdResponse.version
      )
        .then(() => {
          if (!this.isAllowAccess) {
            this.$router.push('/404')
          }
          const corporationId = this.getSolverByIdResponse.corporateId
          this.$router.push(`/solver/list/${corporationId}`)
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
    // 一時保存
    temporarySave() {
      this.saveSolverInfo(true)
    },
    // 「変更する」ボタン押下時の処理
    onClickEdit() {
      this.isEditing = true
    },

    // 「保存する」ボタン押下時の処理
    onClickSave() {
      this.saveSolverInfo()
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
        this.saveSolverInfo()
      } else {
        this.isEditing = true
      }
    },

    // コンテナヘッダー「一覧へ戻る」・「キャンセル」ボタン押下時の処理
    onClickNegative() {
      if (this.isEditing) {
        this.onClickCancel()
      } else {
        const corporationId = this.getSolverByIdResponse.corporateId
        this.$router.push(`/solver/list/${corporationId}`)
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
        ...this.solverInputData,
        ...newValue,
      }
      this.solverInputData = mergedInputData
    },

    // 個人ソルバー情報を保存する処理
    async saveSolverInfo(isTemporary: boolean = false) {
      if (isTemporary) {
        // 一時保存
        this.isLoading.temporary = true
        this.isSnackbarOpen = false
        this.fileRefresh = false
      } else {
        // 前回最終更新日時から20秒経過していない場合、エラーを表示させ更新できないようにする
        if (
          Date.now() - new Date(this.getSolverByIdResponse.updateAt).getTime() <
          20000
        ) {
          this.showErrorBarWithScrollPageTop(this.$t('msg.error.inProgress'))
          this.isLoading.updateSolver = false
          return
        }
        this.isLoading.updateSolver = true
      }
      // 添付ファイルをS3へアップロード
      const results = await this.fileUpload(
        S3_KEY_TEMPLATE_SOLVER.SOLVER_FILES,
        true
      )
      if (results.length > 0) {
        this.solverInputData.resume =
          results.length > 0
            ? [...this.solverInputData.resume, ...results]
            : [...this.solverInputData.resume]
      } else {
        this.solverInputData.resume = [...this.solverInputData.resume]
      }

      // 個人ソルバー画像をS3へアップロード
      if (this.solverInputData.image === undefined) {
        this.solverInputData.facePhoto = null
      } else {
        const imageResults = await this.fileUpload(
          S3_KEY_TEMPLATE_SOLVER.SOLVER_PHOTOS,
          false
        )
        if (imageResults.length > 0) {
          this.solverInputData.facePhoto = imageResults[0]
        }
      }

      let result = false
      if (
        this.getSolverByIdResponse.registrationStatus === 'temporary_saving'
      ) {
        const temporarySaving = this.temporarySavingUpdateData(isTemporary)
        result = await this.updateSolverById(temporarySaving, isTemporary)
      } else {
        const updateData = this.generateUpdateData()
        result = await this.updateSolverById(updateData)
      }
      if (isTemporary) {
        this.isLoading.temporary = false
        this.isSnackbarOpen = true
        this.fileRefresh = true
        // 一時保存後はインプットデータをリセットし、データを再取得
        this.solverInputData = null
        await this.getSolverById()
      } else {
        this.isLoading.updateSolver = false
      }
      if (result && !isTemporary) {
        this.refresh()
      }
    },

    // 一時保存の個人ソルバー情報更新用のデータを生成する処理
    temporarySavingUpdateData(isTemporary: boolean = false) {
      const facePhoto = () => {
        if (
          this.solverInputData.facePhoto &&
          this.solverInputData.facePhoto.path
        ) {
          return this.solverInputData.facePhoto
        } else {
          return null
        }
      }
      const temporarySaving = {
        name: this.solverInputData.name,
        nameKana: this.solverInputData.nameKana,
        mode: isTemporary ? 'temporary_save_solver' : 'update_solver',
        solverApplicationId: this.solverInputData.solverApplicationId,
        solverApplicationName: this.solverInputData.solverApplicationName,
        title: this.solverInputData.title,
        email: this.solverInputData.email,
        phone: this.solverInputData.phone,
        issueMap50: this.solverInputData.issueMap50,
        corporateId: this.solverInputData.corporateId,
        sex: this.solverInputData.sex,
        birthDay: this.solverInputData.birthDay
          ? this.solverInputData.birthDay
          : DEFAULT_BIRTHDAY,
        operatingStatus: this.solverInputData.operatingStatus,
        facePhoto: facePhoto(),
        resume: this.solverInputData.resume,
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
        isSolver: true,
        registrationStatus: isTemporary
          ? this.getSolverByIdResponse.registrationStatus
          : 'saved',
      }
      return temporarySaving
    },

    // 個人ソルバー情報更新用のデータを生成する処理
    generateUpdateData() {
      const updateData = {
        name: this.solverInputData.name,
        nameKana: this.solverInputData.nameKana,
        mode: 'update_solver',
        title: this.solverInputData.title,
        email: this.solverInputData.email,
        phone: this.solverInputData.phone,
        issueMap50: this.solverInputData.issueMap50,
        corporateId: this.solverInputData.corporateId,
        sex: this.solverInputData.sex,
        birthDay: this.solverInputData.birthDay
          ? this.solverInputData.birthDay
          : DEFAULT_BIRTHDAY,
        operatingStatus: this.solverInputData.operatingStatus,
        facePhoto: this.solverInputData.facePhoto,
        resume: this.solverInputData.resume,
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
        isSolver: true,
        registrationStatus: this.solverInputData.registrationStatus,
        deleteSolverApplicationIds:
          this.solverInputData.deleteSolverApplicationIds || [],
      }
      return updateData
    },

    // 個人ソルバー画像・添付ファイルをS3にアップロードする処理
    async fileUpload(template: string, isFile: boolean) {
      const results: { fileName: string; path: string }[] = []
      const promises: any[] = []
      if (
        isFile &&
        this.solverInputData.files &&
        this.solverInputData.files.length > 0
      ) {
        this.solverInputData.files.forEach((file: IFile) => {
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
              this.solverInputData.corporateId,
              this.solverInputData.fileKeyId,
              extension
            )
          )
        })
      } else if (
        !isFile &&
        this.solverInputData.image &&
        this.solverInputData.image.name
      ) {
        const result = {
          fileName: this.solverInputData.image.name,
          path: '',
        }
        const extension = this.solverInputData.image.name.split('.').pop()
        results.push(result)
        promises.push(
          uploadFileSolver(
            template,
            this.solverInputData.image,
            this.solverInputData.corporateId,
            this.solverInputData.fileKeyId,
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
        this.solverInputData.resume &&
        this.solverInputData.resume.length > 0
      ) {
        this.solverInputData.resume.forEach(
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
        this.solverInputData.facePhoto &&
        this.solverInputData.facePhoto.fileName
      ) {
        deleteFile(this.solverInputData.facePhoto.path)
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

    // 再度リロード
    refresh() {
      window.location.reload()
    },

    // スナックバーフラグ更新
    updateSnackBarOpen(isSnackbarOpen: boolean) {
      this.isSnackbarOpen = isSnackbarOpen
    },
  },
})
</script>
