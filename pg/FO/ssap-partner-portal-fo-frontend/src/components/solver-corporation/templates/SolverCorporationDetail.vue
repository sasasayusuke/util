<template>
  <RootTemplate>
    <SolverCorporationDetailContainer
      :title="pageName()"
      :solver-corporation="solverCorporation"
      :issue-map50="issueMap50"
      :is-editing="isEditing"
      :is-loading="isSolverCorporationLoading"
      :is-loading-button="isLoadingButton"
      :is-project="true"
      :is-disabled="isDisabled"
      note-head="required"
      @click:positive="update"
      @click:negative="onClickNegative"
      @change:isValid="onChangeIsValid"
      @inputForm="keepInputData"
    />
    <template v-if="!isEditing" #footerButton>
      <Button style-set="large-primary" width="160px" @click="onClickPositive">
        {{ $t('common.button.edit2') }}
      </Button>
    </template>
    <template v-else #footerButton>
      <Button
        style-set="large-tertiary"
        width="160px"
        outlined
        @click="onClickNegative"
      >
        {{ $t('common.button.cancel') }}
      </Button>
      <Button
        style-set="large-primary"
        width="160px"
        class="ml-2"
        :disabled="isDisabled"
        @click="onClickPositive"
      >
        {{ $t('common.button.save2') }}
      </Button>
    </template>
    <SolverCorporationInvalidator
      v-if="!isEditing && isApt"
      @click:switchValid="isModalOpen = true"
    />
    <!-- モーダル -->
    <template v-if="isModalOpen">
      <template v-if="isDeletable">
        <SolverCorporationInvalidatorModal
          :is-loading="isLoadingButton"
          @click:closeModal="isModalOpen = false"
          @click:delete="deleteRecord"
        />
      </template>
      <template v-else>
        <CustomerDisableModal @click:closeModal="isModalOpen = false" />
      </template>
    </template>
  </RootTemplate>
</template>

<script lang="ts">
import SolverCorporationDetailContainer, {
  LocalSolverCorporation,
} from '../../solver-corporation/organisms/SolverCorporationDetailContainer.vue'
import SolverCorporationInvalidator from '../molecules/SolverCorporationInvalidator.vue'
import CommonDetail from '~/components/common/templates/CommonDetailTemplate.vue'
import RootTemplate from '~/components/common/bases/RootTemplate.vue'
import { Button } from '~/components/common/atoms/index'
import { PropType } from '~/common/BaseComponent'
import {
  CorporateInfoDocument,
  GetSolverCorporationByIdResponse,
  UpdateSolverCorporationById,
  UpdateSolverCorporationByIdRequest,
  DeleteSolverCorporationById,
} from '~/models/SolverCorporation'
import { IGetSelectItemsResponse } from '~/types/Master'
import {
  S3_KEY_TEMPLATE_SOLVER_CORPORATION,
  uploadFileSolverCorporation,
} from '~/utils/upload'
import { ENUM_USER_ROLE } from '~/types/User'
import { solverCorporationStore, meStore } from '~/store'
import { deleteFile } from '~/utils/delete'

export interface isLoading {
  selectItem: boolean
  solverCorporation: boolean
}

export default CommonDetail.extend({
  name: 'SolverCorporationDetail',
  components: {
    RootTemplate,
    Button,
    SolverCorporationDetailContainer,
    SolverCorporationInvalidator,
  },
  props: {
    /** 法人ソルバー情報 */
    solverCorporation: {
      type: Object as PropType<GetSolverCorporationByIdResponse>,
      required: true,
    },
    // 課題マップ50プルダウンアイテム
    issueMap50: {
      type: Object as PropType<IGetSelectItemsResponse>,
      required: true,
    },
    /** ローディング中か */
    isSolverCorporationLoading: {
      type: Object as PropType<isLoading>,
      required: true,
    },
    /** 法人情報が初回登録か */
    isCheckCorporationRegistration: {
      type: Boolean,
      required: true,
    },
    beforeSolverCorporation: {
      type: Object as PropType<GetSolverCorporationByIdResponse>,
      required: true,
    },
  },
  /** 編集中かどうかでタイトルを変更 */
  head() {
    const isEditing = this.isEditing as boolean
    const edit = this.$t('solver-corporation.pages.edit.name') as string
    const detail = this.$t('solver-corporation.pages.index.name') as string
    return {
      title: isEditing ? edit : detail,
    }
  },
  data(): {
    id: string
    isEditing: boolean
    isValid: boolean
    isDisabled: boolean
    isModalOpen: boolean
    isValidWithChange: boolean
    isLoadingButton: boolean
    solverCorporationImageInputData: any
    isUpdating: boolean
    ischeckCorporationRegistration: boolean
  } {
    return {
      id: '',
      isEditing: false,
      isValid: false,
      isDisabled: false,
      isModalOpen: false,
      isValidWithChange: false,
      isLoadingButton: false,
      /** 法人ソルバー画像・会社案内資料のデータ */
      solverCorporationImageInputData: new GetSolverCorporationByIdResponse(),
      isUpdating: false,
      /** 法人情報が初回登録か */
      ischeckCorporationRegistration: false,
    }
  },
  created() {
    this.id = this.$route.params.id
  },
  watch: {
    isCheckCorporationRegistration: {
      handler(newVal) {
        this.ischeckCorporationRegistration = newVal
        this.checkCorporationRegistration(this.ischeckCorporationRegistration)
      },
    },
    isEditing(newVal) {
      this.isDisabled = newVal
    },
  },
  methods: {
    /** 法人ソルバーの法人情報が初回登録されているか判定 */
    checkCorporationRegistration(ischeckCorporationRegistration: boolean) {
      if (ischeckCorporationRegistration) {
        this.isEditing = true
      } else {
        this.isEditing = false
      }
    },
    pageName() {
      const isEditing = this.isEditing as boolean
      const edit = this.$t('solver-corporation.pages.edit.name') as string
      const detail = this.$t('solver-corporation.pages.index.name') as string
      return isEditing ? edit : detail
    },
    /** 編集画面の情報をもとに法人ソルバー情報を更新 */
    async update(LocalSolverCorporation: LocalSolverCorporation) {
      this.clearErrorBar()
      if (this.isEditing) {
        // 前回最終更新日時から20秒経過していない場合、エラーを表示させ更新できないようにする
        if (
          Date.now() - new Date(this.solverCorporation.updateAt).getTime() <
          20000
        ) {
          this.showErrorBarWithScrollPageTop(this.$t('msg.error.inProgress'))
          return
        }
        this.isUpdating = true
        this.isLoadingButton = true
        const request = new UpdateSolverCorporationByIdRequest()
        Object.assign(request, LocalSolverCorporation)
        const id = LocalSolverCorporation.id
        const version = LocalSolverCorporation.version

        request.employee = LocalSolverCorporation.employee
        request.capital = LocalSolverCorporation.capital
        request.earnings = LocalSolverCorporation.earnings
        request.address = LocalSolverCorporation.address
        request.mainCharge = LocalSolverCorporation.mainCharge
        request.deputyCharge = LocalSolverCorporation.deputyCharge
        request.otherCharge = LocalSolverCorporation.otherCharge

        // 法人ソルバー画像をS3へアップロード
        const imageResults = await this.fileUpload(
          S3_KEY_TEMPLATE_SOLVER_CORPORATION.CORPORATION_LOGOS,
          LocalSolverCorporation.id
        )
        if (imageResults.length > 0) {
          const filteredImage = imageResults.filter(
            (result: { fileName: string; path: string }) =>
              result.path.includes(this.solverCorporationImageInputData.id)
          )
          request.corporatePhoto = filteredImage[0]
        } else {
          this.solverCorporationImageInputData.facePhoto = undefined
        }
        // 会社案内資料をS3にアップロード
        this.bulkUpload(
          this.solverCorporationImageInputData.files,
          S3_KEY_TEMPLATE_SOLVER_CORPORATION.CORPORATION_DOCUMENTS,
          LocalSolverCorporation.id
        ).then(async (resultDeliverables: CorporateInfoDocument[]) => {
          resultDeliverables.forEach(
            (resultDeliverable: CorporateInfoDocument) => {
              request.corporateInfoDocument.push(resultDeliverable)
            }
          )
          await UpdateSolverCorporationById(id, version, request)
            .then(async () => {
              await this.deleteDiffFiles(request)
              this.isEditing = false
              this.isLoadingButton = false
              window.location.reload()
            })
            .catch((error) => {
              if (error.response && error.response.status === 409) {
                this.isUpdating = false
                this.isLoadingButton = false
                this.showErrorBar(
                  this.$t('solver.pages.utilizationRate.errorMessage.conflict')
                )
                this.apiErrorHandle(error)
              } else {
                this.showErrorBarWithScrollPageTop(this.$t('msg.error.default'))
              }
            })
        })
      } else {
        this.isEditing = !this.isEditing
      }
    },
    onClickPositive() {
      this.isEditing = !this.isEditing
    },
    onClickNegative() {
      this.clearErrorBar()
      // アライアンス担当の場合はソルバーメニューに遷移
      if (meStore.role === ENUM_USER_ROLE.APT) {
        this.$router.push('/solver/menu')
      } else {
        if (this.isEditing) {
          this.isEditing = false
        }
        this.$emit('refresh')
      }
    },
    onClickDelete() {
      this.isModalOpen = true
    },
    // formの入力値を保持する処理
    keepInputData(newValue: any) {
      const mergedInputData = {
        ...this.solverCorporationImageInputData,
        ...newValue,
      }
      this.solverCorporationImageInputData = mergedInputData
    },
    /**
     * 会社案内資料情報バルクアップロード処理
     * @param 会社案内資料情報
     * @param テンプレート名
     * @param 法人ID
     * @returns アップロード会社案内資料情報
     */
    async bulkUpload(
      files: any,
      template: string,
      solverCorporationId: string
    ) {
      const results: any = []
      const promises: any = []

      for (const i in files) {
        if (files[i]) {
          const result: any = {
            fileName: files[i].name,
            path: '',
          }
          const extension = files[i].name.split('.').pop()
          results.push(result)
          promises.push(
            uploadFileSolverCorporation(
              files[i],
              template,
              solverCorporationId,
              extension
            )
          )
        }
      }
      return await Promise.all(promises)
        .then((responses) => {
          responses.forEach((response: any, index: number) => {
            results[index].path = response.Key as String
          })
          return results
        })
        .catch((err) => {
          throw new Error(err)
        })
        .finally(() => {
          this.isLoadingButton = false
        })
    },
    // 法人ソルバー画像をS3にアップロードする処理
    async fileUpload(template: string, solverCorporationId: string) {
      const results: { fileName: string; path: string }[] = []
      const promises: any[] = []

      if (
        this.solverCorporationImageInputData.image &&
        this.solverCorporationImageInputData.image.name
      ) {
        const result = {
          fileName: this.solverCorporationImageInputData.image.name,
          path: '',
        }
        const extension = this.solverCorporationImageInputData.image.name
          .split('.')
          .pop()
        results.push(result)
        promises.push(
          uploadFileSolverCorporation(
            this.solverCorporationImageInputData.image,
            template,
            solverCorporationId,
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
    /**
     * 法人ソルバー情報を削除（無効）
     */
    deleteRecord() {
      this.isLoadingButton = true

      const id = solverCorporationStore.id
      const version = solverCorporationStore.version

      DeleteSolverCorporationById(id, version)
        .then(() => {
          this.toListPage()
          solverCorporationStore.clear()
          this.$router.push(`/solver/menu`)
        })
        .catch((error) => {
          if (error.response.status === 400) {
            this.isDeletable = false
          } else {
            throw error
          }
        })
        .finally(() => {
          this.isLoadingButton = false
        })
    },
    // formのバリデーションが変化した時の処理
    onChangeIsValid(newValue: boolean) {
      this.isDisabled = !newValue
    },
    // 登録済みのファイルと登録するファイルの差分があればS3から削除する
    async deleteDiffFiles(updateData: UpdateSolverCorporationByIdRequest) {
      // 更新前の会社案内資料
      const beforeFile: any[] =
        this.beforeSolverCorporation.corporateInfoDocument || []
      // 更新後の会社案内資料
      const updateFilePath: string[] = (
        updateData.corporateInfoDocument || []
      ).map((d) => d.path)
      // 削除対象の会社案内資料パス
      const deletFilePaths: string[] = []
      beforeFile.forEach((d) => {
        if (!updateFilePath.includes(d.path)) {
          deletFilePaths.push(d.path)
        }
      })
      await Promise.all(
        deletFilePaths.map(async (p) => {
          await deleteFile(p)
          this.$logger.info('Resume on S3 has been deleted')
        })
      )
    },
  },
  computed: {
    /** ログインユーザーがアライアンス担当か */
    isApt() {
      return meStore.role === ENUM_USER_ROLE.APT
    },
  },
})
</script>
