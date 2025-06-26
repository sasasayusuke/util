<template>
  <RootTemplate class="pt-9">
    <ProjectDetailContainer
      :title="$t('project.pages.create.name')"
      :project="project"
      :users="users.users"
      :service-types="serviceTypes"
      :suggest-users="suggestUsers"
      :suggest-customers="suggestCustomers"
      :suggest-sales-users="suggestSalesUsers"
      :suggest-supporter-users="suggestSupporterUsers"
      :suggest-main-supporter-users="suggestMainSupporterUsers"
      :supporter-organizations="supporterOrganizations.supporterOrganizations"
      is-editing
      is-creating
      :is-loading="isLoading"
      :is-loading-button="isLoadingButton"
      @click:positive="onClickPositive"
      @click:negative="onClickNegative"
    />
  </RootTemplate>
</template>

<script lang="ts">
import ProjectDetailContainer, {
  LocalProject,
} from '../../project/organisms/ProjectDetailContainer.vue'
import RootTemplate from '~/components/common/bases/RootTemplate.vue'
import { PropType } from '~/common/BaseComponent'
import {
  CreateProject,
  CreateProjectRequest,
  GetProjectByIdResponse,
} from '~/models/Project'
import {
  GetServiceTypesResponse,
  GetSupporterOrganizationsResponse,
} from '~/models/Master'
import CommonCreate from '~/components/common/templates/CommonCreateTemplate.vue'
import { GetUsersResponse } from '~/models/User'
import { ENUM_CREATE_OR_UPDATE_PROJECT_RESPONSE_ERROR } from '~/types/Project'

export interface isLoading {
  suggestUsers: boolean
  suggestCustomers: boolean
  serviceTypes: boolean
  suggestSalesUsers: boolean
  supporterOrganization: boolean
}

export default CommonCreate.extend({
  components: {
    RootTemplate,
    ProjectDetailContainer,
  },
  props: {
    /** 作成中の案件詳細 */
    project: {
      type: Object as PropType<GetProjectByIdResponse>,
      required: true,
    },
    users: {
      type: Object as PropType<GetUsersResponse>,
      required: true,
    },
    /** サービス種別 */
    serviceTypes: {
      type: Object as PropType<GetServiceTypesResponse>,
      required: true,
    },
    /** 取引先のサジェスト用情報 */
    suggestUsers: {
      type: Array,
      required: true,
    },
    /** 取引先のサジェスト用顧客情報 */
    suggestCustomers: {
      type: Array,
      required: true,
    },
    /** 取引先のサジェスト用営業情報 */
    suggestSalesUsers: {
      type: Array,
      required: true,
    },
    /** 取引先のサジェスト用支援者情報 */
    suggestSupporterUsers: {
      type: Array,
    },
    /** 取引先のサジェスト用支援者責任者情報 */
    suggestMainSupporterUsers: {
      type: Array,
      required: true,
    },
    /** 支援者組織一覧 */
    supporterOrganizations: {
      type: Object as PropType<GetSupporterOrganizationsResponse>,
      required: true,
    },
    /** 読み込み中か */
    isLoading: {
      type: Object as PropType<isLoading>,
      required: true,
    },
  },
  data() {
    return {
      listPagePath: '/project/list',
      detailPagePrefix: ``,
      isLoading: false,
      isLoadingButton: false,
    }
  },
  methods: {
    /**
     * 編集画面の情報をもとに案件情報を新規作成
     * @param localProject 入力中の案件情報
     */
    create(localProject: LocalProject) {
      this.isLoadingButton = true
      const request = new CreateProjectRequest()
      Object.assign(request, localProject)
      this.detailPagePrefix = `/project/`
      if (!request.customerUsers) {
        request.customerUsers = []
      }
      if (!request.supporterUsers) {
        request.supporterUsers = []
      }
      const customerUsers = request.customerUsers
      const supporterUsers = request.supporterUsers
      request.customerUsers = []
      request.supporterUsers = []
      for (const i in customerUsers) {
        // @ts-ignore
        request.customerUsers.push({ id: customerUsers[i].id })
      }
      for (const i in supporterUsers) {
        // @ts-ignore
        request.supporterUsers.push({ id: supporterUsers[i].id })
      }
      CreateProject(request)
        .then((res) => {
          const id = res.data.id
          this.toDetailPage(id)
        })
        .catch((error) => {
          if (
            error?.response?.data?.detail ===
            ENUM_CREATE_OR_UPDATE_PROJECT_RESPONSE_ERROR.DISABLED_MAINSALES
          ) {
            // 商談所有者
            this.showErrorBarWithScrollPageTop(
              this.$t('project.pages.create.errors.disabledMainSales')
            )
          } else if (
            error?.response?.data?.detail ===
            ENUM_CREATE_OR_UPDATE_PROJECT_RESPONSE_ERROR.DISABLED_MAINCUSTOMER
          ) {
            // お客様（代表）
            this.showErrorBarWithScrollPageTop(
              this.$t('project.pages.create.errors.disabledMainCustomer')
            )
          } else if (
            error?.response?.data?.detail ===
            ENUM_CREATE_OR_UPDATE_PROJECT_RESPONSE_ERROR.DISABLED_CUSTOMER
          ) {
            // お客様
            this.showErrorBarWithScrollPageTop(
              this.$t('project.pages.create.errors.disabledCustomer')
            )
          } else if (
            error?.response?.data?.detail ===
            ENUM_CREATE_OR_UPDATE_PROJECT_RESPONSE_ERROR.DISABLED_MAINSUPPORTER
          ) {
            // プロデューサー
            this.showErrorBarWithScrollPageTop(
              this.$t('project.pages.create.errors.disabledMainSupporter')
            )
          } else if (
            error?.response?.data?.detail ===
            ENUM_CREATE_OR_UPDATE_PROJECT_RESPONSE_ERROR.DISABLED_SUPPORTER
          ) {
            // アクセラレーター
            this.showErrorBarWithScrollPageTop(
              this.$t('project.pages.create.errors.disabledSupporter')
            )
          } else {
            this.showErrorBarWithScrollPageTop(this.$t('msg.error.default'))
          }
        })
        .finally(() => {
          this.isLoadingButton = false
        })
    },
  },
})
</script>
