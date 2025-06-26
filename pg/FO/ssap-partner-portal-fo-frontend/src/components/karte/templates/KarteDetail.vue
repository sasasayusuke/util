<template>
  <RootTemplate>
    <v-container
      v-if="
        isCustomer === false &&
        isSales === false &&
        isSalesMgr === false &&
        regionalProject === true
      "
      class="pa-0 mb-4"
    >
      <v-flex class="d-flex justify-space-between align-center">
        <h2></h2>
        <Button
          outlined
          style-set="small-tertiary detailHeaderNegative"
          width="120px"
          @click="onClickNegative"
        >
          {{ $t('karte.pages.detail.backToKarteList') }}
        </Button>
      </v-flex>
    </v-container>
    <KarteDetailContainer
      :project="project"
      :karte="karte"
      :surveys="surveys"
      :is-customer="isCustomer"
      :is-sales="isSales"
      :is-sales-mgr="isSalesMgr"
      :regional-project="regionalProject"
      :is-editing="
        isCustomer === false &&
        isSales === false &&
        isSalesMgr === false &&
        regionalProject === true
      "
      :is-updating="isUpdating"
      :back-to-list-button="false"
      :is-loading-button="isLoadingButton"
      @click:positive="onClickPositive"
      @click:negative="onClickNegative"
      @updateDocuments="emitDocuments"
      @updateDeliverables="emitDeliverables"
    />
  </RootTemplate>
</template>

<script lang="ts">
import { Button, Chip } from '~/components/common/atoms/index'
import KarteDetailContainer, {
  LocalKarte,
} from '~/components/karte/organisms/KarteDetailContainer.vue'
import CommonDetail from '~/components/common/templates/CommonDetailTemplate.vue'
import { PropType } from '~/common/BaseComponent'
import RootTemplate from '~/components/common/bases/RootTemplate.vue'
import { GetProjectByIdResponse } from '~/models/Project'
import {
  Documents,
  Deliverables,
  UpdateKarteByIdRequest,
  UpdateKarteById,
  GetKarteByIdResponse,
} from '~/models/Karte'
import { SurveyListItem } from '~/models/Survey'
import { S3_KEY_TEMPLATE, uploadFile } from '~/utils/upload'
import { meStore } from '~/store'
import { ENUM_USER_ROLE } from '~/types/User'
import IndexedDB from '~/utils/indexedDb'
import { Tables } from '~/tables'

export interface isLoading {
  project: boolean
  karte: boolean
  surveys: boolean
}

export default CommonDetail.extend({
  components: {
    RootTemplate,
    KarteDetailContainer,
    Chip,
    Button,
  },
  props: {
    /**
     * プロジェクト情報
     */
    project: {
      type: Object as PropType<GetProjectByIdResponse>,
      required: true,
    },
    /**
     * カルテ情報
     */
    karte: {
      type: Object as PropType<GetKarteByIdResponse>,
      required: true,
    },
    /**
     * アンケート情報
     */
    surveys: {
      type: Array as PropType<SurveyListItem[]>,
      required: true,
    },
    /**
     * 顧客ロールか否か
     */
    isCustomer: {
      type: Boolean,
      default: false,
    },
    /**
     * 営業ロールか否か
     */
    isSales: {
      type: Boolean,
      default: false,
    },
    /**
     * 営業責任者か否か
     */
    isSalesMgr: {
      type: Boolean,
      default: false,
    },
  },
  data() {
    return {
      localDocuments: [],
      localDeliverables: [],
      isUpdating: false,
      regionalProject: false,
      /** 案件更新中か否か */
      isLoadingButton: false,
      indexedDB: new IndexedDB(meStore.id),
    }
  },
  created() {
    //所属案件か否か
    const isRegionalProject = meStore.projectIds?.includes(this.project.id)

    // 支援者・支援者責任者・事業者責任者の場合、非所属案件カルテは詳細画面に遷移
    if (
      (meStore.role === ENUM_USER_ROLE.SUPPORTER ||
        meStore.role === ENUM_USER_ROLE.SUPPORTER_MGR ||
        meStore.role === ENUM_USER_ROLE.BUSINESS_MGR) &&
      isRegionalProject
    ) {
      this.regionalProject = true
    } else if (
      (meStore.role === ENUM_USER_ROLE.SUPPORTER ||
        meStore.role === ENUM_USER_ROLE.SUPPORTER_MGR ||
        meStore.role === ENUM_USER_ROLE.BUSINESS_MGR) &&
      !isRegionalProject
    ) {
      this.regionalProject = false
    }
  },
  methods: {
    /**
     * ページ名
     * @returns ページ名
     */
    pageName() {
      return this.$t('karute.pages.detail.name')
    },
    /**
     * 保存ボタン動作
     * @param カルテ情報
     */
    onClickPositive(localKarte: LocalKarte) {
      this.update(localKarte)
    },
    /**
     * 戻るボタン動作
     */
    onClickNegative() {
      this.$router.push(this.backToUrl('/karte/list/' + this.project.id))
    },
    /**
     * 資料添付情報をセット
     * @param 資料添付情報
     */
    emitDocuments(files: any) {
      this.localDocuments = []
      for (const i in files) {
        this.$set(this.localDocuments, i, files[i])
      }
      this.$emit('updateDocuments', this.localDocuments)
    },
    /**
     * 成果物添付をセット
     * @param 成果物添付情報
     */
    emitDeliverables(files: any) {
      this.localDeliverables = []
      for (const i in files) {
        this.$set(this.localDeliverables, i, files[i])
      }
      this.$emit('updateDeliverables', this.localDeliverables)
    },
    /**
     * カルテ情報更新処理
     * @param カルテ情報
     */
    update(localKarte: LocalKarte) {
      this.isUpdating = true
      this.isLoadingButton = true
      this.clearErrorBar()
      const request = new UpdateKarteByIdRequest()
      Object.assign(request, localKarte)
      const karteId: string = this.$route.params.karteId as string
      const projectId: string = this.karte.projectId as string
      const version: number = this.karte.version

      this.bulkUpload(
        this.localDocuments,
        S3_KEY_TEMPLATE.PROJECT_KARTE_DOCUMENTS,
        projectId,
        karteId
      )
        .then((resultDocuments: Documents[]) => {
          resultDocuments.forEach((resultDocument: Documents) => {
            request.documents.push(resultDocument)
          })
          return this.bulkUpload(
            this.localDeliverables,
            S3_KEY_TEMPLATE.PROJECT_KARTE_DELIVERABLES,
            projectId,
            karteId
          )
            .then((resultDeliverables: Deliverables[]) => {
              resultDeliverables.forEach((resultDeliverable: Deliverables) => {
                request.deliverables.push(resultDeliverable)
              })
              return UpdateKarteById(karteId, version, request)
                .then(() => {
                  this.indexedDB.delete(Tables.karte, karteId)
                  this.$router.push(`/karte/list/${projectId}`)
                })
                .catch(() => {
                  this.isUpdating = false
                  this.showErrorBarWithScrollPageTop(
                    this.$t('msg.error.default')
                  )
                })
            })
            .catch(() => {
              this.isUpdating = false
              this.showErrorBarWithScrollPageTop(
                this.$t(
                  'karte.pages.detail.errorMessages.deliverablesUploadFailed'
                )
              )
            })
        })
        .catch(() => {
          this.isUpdating = false
          this.showErrorBarWithScrollPageTop(
            this.$t('karte.pages.detail.errorMessages.documentsUploadFailed')
          )
        })
        .finally(() => {
          this.isLoadingButton = false
        })
    },
    /**
     * 添付情報バルクアップロード処理
     * @param ファイル情報
     * @param テンプレート名
     * @param プロジェクトID
     * @param カルテID
     * @returns アップロード添付情報
     */
    async bulkUpload(
      files: any,
      template: string,
      projectId: string,
      karteId: string
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
            uploadFile(files[i], template, projectId, karteId, extension)
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
    },
  },
})
</script>
