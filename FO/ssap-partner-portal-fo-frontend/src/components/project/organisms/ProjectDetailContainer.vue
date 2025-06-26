<template>
  <DetailContainer
    :title="title"
    :is-editing="isEditing"
    :note-head="noteHead"
    :is-valid="isValidWithChange"
    :is-disabled-button1="!isEditable"
    :is-hide-button2="!isEditing"
    :is-project="true"
    :is-loading-button="isLoadingButton"
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
    <template #footerButton>
      <slot name="footerButton" />
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
import { meStore } from '~/store'

export { LocalProject }

export default CommonDetailContainer.extend({
  name: 'ProjectDetailContainer',
  components: {
    DetailContainer,
    ProjectDetailRows,
  },
  watch: {
    isValidWithChange(newValue: boolean) {
      this.$emit('validWithChange', newValue)
    },
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
    /** 取引先のサジェスト用情報 */
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
    isLoading: {
      type: Object as PropType<isLoading>,
      required: true,
    },
    /** 'required' */
    noteHead: {
      type: String,
      default: '',
    },
    /** 案件更新中か否か */
    isLoadingButton: {
      type: Boolean,
      default: false,
    },
  },
  data() {
    return {
      isValid: true,
      /** 編集中の案件内容 */
      localProject: new LocalProject(),
    }
  },
  computed: {
    title() {
      return this.$t('project.pages.' + this.mode + '.name')
    },
    /** 支援者・支援者責任者・営業ロールの所属案件のみ案件変更ができるチェック処理
     *  事業者責任者はすべての案件の変更が可能
     */
    isEditable(): boolean {
      const projectsIds = this.$store.state.me.projectIds

      if (meStore.role === 'business_mgr') {
        return true
      } else if (meStore.role === 'sales_mgr') {
        return false
      } else if (!projectsIds) {
        return false
      } else if (
        meStore.role === 'supporter' ||
        meStore.role === 'supporter_mgr' ||
        meStore.role === 'sales'
      ) {
        const id = this.$route.params.projectId
        const projectsIds = this.$store.state.me.projectIds
        return projectsIds.includes(id)
      } else {
        return false
      }
    },
  },
})
</script>
