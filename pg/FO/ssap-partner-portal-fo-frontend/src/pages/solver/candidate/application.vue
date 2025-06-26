<!-- 新規ソルバー候補申請画面ページ -->

<template>
  <TemplateSolverDetail
    :solvers="getSolversResponse"
    :solver="getSolverByIdResponse"
    :solver-corporations="getSolverCorporationsResponse"
    :issue-map50="getSelectItemsResponse"
    :tsi-areas="getSelectItemsTsiAreasResponse"
    :is-editing="isEditing"
    :is-finished="isFinished"
    :is-loading="isLoading.createSolver"
    :is-suggest="isSuggest"
    :new-candidate-application="true"
    :on-change-solver-name-func="onChangeSolverName"
    :current-index="currentIndex"
    @click:delete="deleteSolverCandidate"
    @click:confirm="toConfirm"
    @click:backToInput="toInput"
    @click:apply="finishApplication"
    @inputForm="keepInputData"
    @click:displayProjectName="keepProjectInfo"
    @change:solverCorporation="keepCorporateId"
    @incrementIndex="incrementIndex"
  />
</template>

<script lang="ts">
import { ManagedUpload } from 'aws-sdk/clients/s3'
import { v4 as uuidv4 } from 'uuid'
import TemplateSolverDetail from '~/components/solver/templates/SolverDetail.vue'
import BasePage from '~/common/BasePage'
import {
  CreateSolver,
  GetSolverById,
  GetSolverByIdResponse,
  GetSolvers,
  GetSolversResponse,
} from '~/models/Solver'
import { hasRole } from '~/utils/role-authorizer'
import { ENUM_USER_ROLE } from '~/types/User'
import {
  ENUM_GET_SOLVERS_REQUEST_SORT,
  ISolverIFile,
  ISolverInfo,
} from '~/types/Solver'
import { meStore, solverCorporationStore } from '~/store'
import { GetSelectItems, GetSelectItemsResponse } from '~/models/Master'
import {
  GetSolverCorporationById,
  GetSolverCorporationByIdResponse,
  GetSolverCorporations,
  GetSolverCorporationsResponse,
} from '~/models/SolverCorporation'
import {
  generateKeySolver,
  IFile,
  S3_KEY_TEMPLATE_SOLVER,
  uploadFileSolver,
} from '~/utils/upload'
import { DEFAULT_BIRTHDAY } from '~/const'

export interface isLoading {
  selectItem: boolean
  solvers: boolean
  solver: boolean
  solverCorporation: boolean
  createSolver: boolean
  tsiAreas: boolean
}

export default BasePage.extend({
  name: 'NewSolverCandidateDetail',
  components: {
    TemplateSolverDetail,
  },
  middleware: ['roleCheck'],
  head() {
    return {
      title: this.$t('solver.pages.index.name') as string,
    }
  },
  data(): {
    /** GetolverCorporationById APIのレスポンス */
    getSolverCorporationByIdResponse: GetSolverCorporationByIdResponse
    /** GetSolvers APIのレスポンス */
    getSolversResponse: GetSolversResponse
    /** GetSelectItems APIのレスポンス */
    getSelectItemsResponse: GetSelectItemsResponse
    /** GetSolverBy APIのレスポンス */
    getSolverByIdResponse: GetSolverByIdResponse
    /** GetSolverCorporations APIのレスポンス */
    getSolverCorporationsResponse: GetSolverCorporationsResponse
    /** 東証33業種経験/対応可能領域プルダウンアイテム */
    getSelectItemsTsiAreasResponse: GetSelectItemsResponse
    /** ソルバー候補の入力データ（申請用） */
    solverCandidates: ISolverInfo[]
    /** ソルバー候補の入力データ（参照用） */
    solverCandidateInputData: any[]
    /** 案件ID */
    projectId: string
    /** 案件名 */
    projectName: string
    /** 法人内ID */
    corporateId: string
    /** ローディング中か否か */
    isLoading: isLoading
    /** 編集中か否か */
    isEditing: boolean
    /** 申請が完了したか否か */
    isFinished: boolean
    /** 個人ソルバー名がサジェストから選択されたか否か */
    isSuggest: boolean
    /** 追加ソルバーを認識するためのインデックス */
    currentIndex: number
  } {
    return {
      getSolverCorporationByIdResponse: new GetSolverCorporationByIdResponse(),
      /** GetSolvers APIのレスポンス */
      getSolversResponse: new GetSolversResponse(),
      getSelectItemsResponse: new GetSelectItemsResponse(),
      /** GetSolverBy APIのレスポンス */
      getSolverByIdResponse: new GetSolverByIdResponse(),
      /** GetSolverCorporations APIのレスポンス */
      getSolverCorporationsResponse: new GetSolverCorporationsResponse(),
      /** 東証33業種経験/対応可能領域プルダウンアイテム */
      getSelectItemsTsiAreasResponse: new GetSelectItemsResponse(),
      /** ソルバー候補の入力データ（申請用） */
      solverCandidates: [],
      /** ソルバー候補の入力データ（参照用） */
      solverCandidateInputData: [],
      projectId: '',
      /** 案件名 */
      projectName: '',
      /** 法人内ID */
      corporateId: '',
      /** ローディング中か否か */
      isLoading: {
        selectItem: false,
        solvers: false,
        solver: false,
        solverCorporation: false,
        createSolver: false,
        tsiAreas: false,
      },
      /** 編集中か否か */
      isEditing: true,
      /** 申請が完了したか否か */
      isFinished: false,
      /** 個人ソルバー名がサジェストから選択されたか否か */
      isSuggest: true,
      /** 追加ソルバーを認識するためのインデックス */
      currentIndex: 0,
    }
  },
  created() {
    this.corporateId = this.getInitialCorporateId()
  },
  mounted() {
    if (meStore.role === ENUM_USER_ROLE.APT && this.$route.query.id) {
      // アライアンス担当で公式ページから遷移
      this.displayLoading([
        this.getSelectItems('issue_map50'),
        this.getSelectItems('industry_segment'),
        this.getSolverCorporations(),
      ])
    } else if (
      // 法人ソルバーで公式ページから遷移
      meStore.role === ENUM_USER_ROLE.SOLVER_STAFF &&
      this.$route.query.id
    ) {
      this.displayLoading([
        this.getSelectItems('issue_map50'),
        this.getSelectItems('industry_segment'),
        this.getSolvers(meStore.solverCorporationId),
        this.getSolverCorporationById(meStore.solverCorporationId),
      ])
    } else {
      // ソルバーメニューから遷移
      this.displayLoading([
        this.getSelectItems('issue_map50'),
        this.getSelectItems('industry_segment'),
        this.getSolvers(solverCorporationStore.id),
        this.getSolverCorporationById(solverCorporationStore.id),
      ])
    }
  },
  watch: {
    // 初回登録が済んでいない法人の場合は登録モーダルへリダイレクト
    getSolverCorporationByIdResponse: {
      handler(newValue) {
        if (!newValue.updateAt) {
          this.$router.push('/solver/menu')
        }
      },
    },
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

    /** GetSolverCorporationById API: 法人IDから法人ソルバー情報を取得する */
    async getSolverCorporationById(corporationId: string) {
      this.isLoading.solverCorporation = true
      /**
       * 法人ソルバー情報取得APIを叩く
       * @param リクエストパラメータ
       */
      await GetSolverCorporationById(corporationId)
        .then((res) => {
          if (!this.isAllowAccess) {
            this.$router.push('/404')
          }
          this.getSolverCorporationByIdResponse = res.data

          solverCorporationStore.setResponse(res.data)
          this.isLoading.solverCorporation = false
        })
        .catch((error) => {
          if (error.response && error.response.status === 404) {
            this.$router.push('/404')
          } else {
            this.apiErrorHandle(error)
          }
        })
    },

    /** GetSolvers API: 1つの法人に紐づく全ソルバーの一覧情報を取得する */
    async getSolvers(solverCorporationId: string) {
      const request = {
        id: solverCorporationId,
        solverType: 'all',
        sex: 'all',
        operatingStatus: 'all',
        sort: ENUM_GET_SOLVERS_REQUEST_SORT.CREATE_AT_DESC,
      }

      /**
       * ソルバー一覧取得APIを叩く
       * @param リクエストパラメータ
       */
      await GetSolvers(request)
        .then((res) => {
          if (!this.isAllowAccess) {
            this.$router.push('/404')
          }
          this.getSolversResponse = res.data
        })
        .catch((error) => {
          this.isLoading.solvers = false
          if (error.response && error.response.status === 404) {
            this.$router.push('/404')
          } else {
            this.apiErrorHandle(error)
          }
        })
    },

    /** GetSolverById API: 個人ソルバー1人の情報を取得する */
    async getSolverById(
      individualSolverId: string,
      solverIndex: number,
      isNew: boolean
    ): Promise<void | GetSolverByIdResponse> {
      this.isLoading.solver = true

      /**
       * ソルバー情報取得APIを叩く
       * @param リクエストパラメータ
       */
      await GetSolverById(individualSolverId)
        .then((res) => {
          if (!this.isAllowAccess) {
            this.$router.push('/404')
          }
          const solver = { index: solverIndex, ...res.data }
          this.getSolverByIdResponse = solver
          const index = isNew
            ? this.currentIndex
            : this.solverCandidateInputData[solverIndex].index
          this.solverCandidateInputData[solverIndex] = res.data
          this.solverCandidateInputData[solverIndex].name = res.data.name
          this.solverCandidateInputData[solverIndex].isRegisteredSolver = true
          this.solverCandidateInputData[solverIndex].index = index
          this.isLoading.solver = false
        })
        .catch((error) => {
          if (error.response && error.response.status === 404) {
            this.$router.push('/404')
          } else {
            this.apiErrorHandle(error)
          }
        })
    },

    /** GetSolverCorporations API: 法人ソルバーの一覧情報を取得する */
    async getSolverCorporations() {
      this.isLoading.solverCorporation = true
      const request = {
        disabled: false,
      }

      /**
       * 法人ソルバー一覧取得APIを叩く
       * @param リクエストパラメータ
       */
      await GetSolverCorporations(request)
        .then((res) => {
          if (!this.isAllowAccess) {
            this.$router.push('/404')
          }
          // updateAtに値がある法人のみを抽出
          this.getSolverCorporationsResponse.solverCorporations =
            res.data.solverCorporations.filter((solverCorporation) =>
              Boolean(solverCorporation.updateAt)
            )
          this.isLoading.solverCorporation = false
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
    async createSolver() {
      this.clearErrorBar()
      // ファイルアップロード
      const fileUploadResult: ISolverIFile[] = await this.fileUpload()
      // リクエストデータ作成
      const request = {
        solversInfo: this.generateApplyData(fileUploadResult),
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
          this.$set(this.isLoading, 'createSolver', false)
          this.isFinished = true
        })
        .catch((error) => {
          this.$set(this.isLoading, 'createSolver', false)
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
      for (const data of this.solverCandidateInputData) {
        if (!data.isRegisteredSolver) {
          data.fileKeyId = uuidv4()
        } else {
          data.fileKeyId = data.fileKeyId ? data.fileKeyId : data.id
        }
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
      }
      const results = await Promise.all(promises)
      const keys = results.map((d) => d.key || d.Key)
      return solverIFiles.filter((file: ISolverIFile) =>
        keys.includes(file.path)
      )
    },

    // ソルバー候補申請用のデータを生成する処理
    generateApplyData(fileUploadResult: ISolverIFile[]) {
      const applyData: ISolverInfo[] = this.solverCandidateInputData.map(
        (data) => {
          const uploadedFile: ISolverIFile[] = fileUploadResult.filter(
            (file: ISolverIFile) => file.path.includes(data.fileKeyId)
          )
          // 個人画像設定
          const facePhotoList: ISolverIFile[] = uploadedFile.filter(
            (file: ISolverIFile) => file.path.includes('photos')
          )
          const facePhoto: ISolverIFile | undefined =
            facePhotoList && facePhotoList.length > 0
              ? facePhotoList[0]
              : data.facePhoto && data.facePhoto.fileName
              ? data.facePhoto
              : undefined
          // 添付ファイル設定
          const resume: ISolverIFile[] = uploadedFile
            .filter((file: ISolverIFile) => file.path.includes('resumes'))
            .concat(data.resume || [])
          return {
            // 入力確認画面で、S3にファイルをアップロードするためにIDを生成しているが、新規作成時はIDをnullでリクエストする。
            // 脆弱性診断で問題があったため対応（https://sonystaqct.backlog.com/view/STADX-1702#comment-490928973）
            // ※ S3のファイルバスのIDと、新規登録ユーザーのIDが異なる
            id: data.isRegisteredSolver ? data.id : undefined,
            mode: 'create_candidate',
            isRegisteredSolver: data.isRegisteredSolver,
            name: data.name,
            nameKana: data.nameKana,
            solverApplicationId: this.projectId,
            solverApplicationName: this.projectName,
            title: data.title,
            email: data.email,
            phone: data.phone,
            issueMap50: data.issueMap50,
            corporateId: this.corporateId,
            sex: data.sex,
            birthDay: data.birthDay ? data.birthDay : DEFAULT_BIRTHDAY,
            operatingStatus: data.operatingStatus,
            facePhoto,
            resume,
            academicBackground: data.academicBackground,
            workHistory: data.workHistory,
            isConsultingFirm: data.isConsultingFirm,
            specializedThemes: data.specializedThemes,
            mainAchievements: data.mainAchievements,
            providedOperatingRate: data.providedOperatingRate,
            providedOperatingRateNext: data.providedOperatingRateNext,
            operationProspectsMonthAfterNext:
              data.operationProspectsMonthAfterNext,
            pricePerPersonMonth: data.pricePerPersonMonth || 0,
            pricePerPersonMonthLower: data.pricePerPersonMonthLower || 0,
            hourlyRate: data.hourlyRate || 0,
            hourlyRateLower: data.hourlyRateLower || 0,
            englishLevel: data.englishLevel,
            tsiAreas: data.tsiAreas,
            notes: data.notes,
            registrationStatus: 'new',
            fileKeyId: data.fileKeyId,
          }
        }
      )
      // 空のデータを削除
      applyData.forEach((data) => {
        ;(Object.keys(data) as (keyof ISolverInfo)[]).forEach((key) => {
          if (
            data[key] === '' ||
            typeof data[key] === 'undefined' ||
            (Array.isArray(data[key]) && (data[key] as any[]).length === 0)
          ) {
            delete data[key]
          }
        })
      })
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

    // 「確認画面に進む」ボタン押下時の処理
    toConfirm() {
      this.isEditing = false
    },

    // 「入力画面に戻る」ボタン押下時の処理
    toInput() {
      this.isEditing = true
    },

    // ソルバー候補を申請する処理
    finishApplication() {
      this.$set(this.isLoading, 'createSolver', true)
      this.createSolver().then(() => {
        this.$set(this.isLoading, 'createSolver', false)
      })
    },

    getInitialCorporateId() {
      if (meStore.role === ENUM_USER_ROLE.SOLVER_STAFF) {
        return meStore.solverCorporationId
      } else if (meStore.role === ENUM_USER_ROLE.APT) {
        // アライアンス担当で公式サイトからの遷移ではない（クエリパラメータが指定されてない）
        if (!this.$route.query.id) {
          this.$logger.info(solverCorporationStore.id)
          return solverCorporationStore.id
        }
      }
      return ''
    },

    // ソルバー候補の入力値を保持する処理
    keepInputData(newValue: any) {
      if (!this.isSuggest) {
        delete newValue.id
        delete newValue.name
      }
      const targetIndex = this.solverCandidateInputData.findIndex(
        (d) => d.index === newValue.index
      )
      const isNew = targetIndex < 0
      if (isNew) {
        this.solverCandidateInputData = [
          ...this.solverCandidateInputData,
          newValue,
        ]
        this.incrementIndex()
      } else {
        this.solverCandidateInputData[targetIndex] = {
          ...this.solverCandidateInputData[targetIndex],
          ...newValue,
        }
      }
    },

    // ソルバー候補の入力値（案件ID、案件名）を保持する処理
    keepProjectInfo(newValue: { projectId: string; projectName: string }) {
      this.projectId = newValue.projectId
      this.projectName = newValue.projectName
    },

    // ソルバー候補の入力値（法人ID）を保持する処理
    keepCorporateId(newValue: string) {
      this.corporateId = newValue
      this.getSolverCorporationById(newValue)

      // 個人ソルバー名のサジェストアイテムを絞り込み
      this.getSolvers(newValue)
    },

    // 個人ソルバー名を変更した時の処理
    // サジェストで個人ソルバー名を選んだ場合のみ、登録データを取得する
    async onChangeSolverName(newValue: {
      index: number
      solverId: string
      solverName: any
    }) {
      const targetIndex = this.solverCandidateInputData.findIndex(
        (d) => d.index === newValue.index
      )
      const isNew = targetIndex < 0
      if (newValue.solverName && newValue.solverName.text) {
        // サジェストで選択した場合
        this.isSuggest = true
        await this.getSolverById(
          newValue.solverName.value,
          isNew ? this.solverCandidateInputData.length : targetIndex,
          isNew
        )
      } else {
        // 新規入力した場合
        this.isSuggest = false
        this.solverCandidateInputData[
          isNew ? this.solverCandidateInputData.length : targetIndex
        ] = {
          ...this.solverCandidateInputData[targetIndex],
          id: newValue.solverId,
          name: newValue.solverName,
          isRegisteredSolver: false,
          index: isNew ? this.currentIndex : newValue.index,
        }
      }
      if (isNew) {
        this.incrementIndex()
      }
    },

    // ソルバー候補を削除した時の処理
    deleteSolverCandidate(newValue: any) {
      this.solverCandidateInputData = newValue
    },

    // ソルバー追加時にインクリメントする
    incrementIndex() {
      this.currentIndex += 1
    },
  },
})
</script>
