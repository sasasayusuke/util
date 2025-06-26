<template>
  <DetailContainer
    :title="title"
    :is-editing="isEditing"
    :is-valid="isValidWithChange"
    :is-hide-button2="!isEditing"
    :is-loading-button="isLoadingButton"
    :is-disabled-edit-button="isDisabledEditButton"
    hx="h1"
    @click:positive="$emit('click:positive', localProject)"
    @click:negative="$emit('click:negative')"
  >
    <ProjectDetailRows
      v-model="isValid"
      :project="project"
      :service-types="serviceTypes"
      :suggest-users="suggestUsers"
      :suggest-sales-users="suggestSalesUsers"
      :suggest-customers="suggestCustomers"
      :suggest-supporter-users="suggestSupporterUsers"
      :suggest-main-supporter-users="suggestMainSupporterUsers"
      :supporter-organizations="supporterOrganizations"
      :is-editing="isEditing"
      :is-creating="isCreating"
      :is-loading="isLoading"
      @update="update('localProject', $event)"
    />
    <template #button>
      <slot name="button" />
    </template>
  </DetailContainer>
</template>

<script lang="ts">
import { PropType } from '~/common/BaseComponent'
import ProjectDetailRows, {
  LocalProject,
} from '~/components/project/molecules/ProjectDetailRows.vue'
import DetailContainer from '~/components/common/organisms/DetailContainer.vue'
import CommonDetailContainer from '~/components/common/organisms/CommonDetailContainer.vue'
import { GetProjectByIdResponse } from '~/models/Project'
import { GetServiceTypesResponse } from '~/models/Master'
import { isLoading } from '~/components/customer/templates/CustomerList.vue'
import { ENUM_ADMIN_ROLE } from '~/types/Admin'
import { meStore } from '~/store'

export { LocalProject }

export default CommonDetailContainer.extend({
  components: {
    DetailContainer,
    ProjectDetailRows,
  },
  props: {
    /** 選択した案件詳細 */
    project: {
      type: Object as PropType<GetProjectByIdResponse>,
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
    /** 取引先のサジェスト用営業情報 */
    suggestSalesUsers: {
      type: Array,
      required: true,
    },
    /** 取引先のサジェスト用支援者情報 */
    suggestSupporterUsers: {
      type: Array,
      required: true,
    },
    /** 取引先のサジェスト用支援者責任者情報 */
    suggestMainSupporterUsers: {
      type: Array,
      required: true,
    },
    /** 取引先のサジェスト用顧客情報 */
    suggestCustomers: {
      type: Array,
      required: true,
    },
    /** 支援者組織一覧 */
    supporterOrganizations: {
      type: Array,
      required: true,
    },
    /** 読み込み中か */
    isLoading: {
      type: Object as PropType<isLoading>,
      required: true,
    },
    /** 案件保存中か */
    isLoadingButton: {
      type: Boolean,
      default: false,
    },
  },
  data() {
    return {
      /** バリデーション確認結果 */
      isValid: true,
      /** 編集中の案件内容 */
      localProject: new LocalProject(),
    }
  },
  computed: {
    /**
     * 表示タイトル
     * @returns 表示タイトル文字列
     */
    title(): string {
      return this.$t('project.pages.' + this.mode + '.name') as string
    },
    /**
     * 支援者責任者が編集不可の案件の場合、「変更する」ボタンを非活性にする
     */
    isDisabledEditButton() {
      const project = this.project
      if (
        meStore.roles.length === 1 &&
        meStore.roles.includes(ENUM_ADMIN_ROLE.SUPPORTER_MGR)
      ) {
        const isOrganizationIdInclude = meStore.supporterOrganizations.some(
          function (supporterOrganization) {
            return supporterOrganization.id === project.supporterOrganizationId
          }
        )
        if (this.isCreating || isOrganizationIdInclude || !project.isSecret) {
          // 新規案件個別登録/所属課案件/その他公開案件
          return false
        } else {
          return true
        }
      } else {
        return false
      }
    },
  },
})
</script>
