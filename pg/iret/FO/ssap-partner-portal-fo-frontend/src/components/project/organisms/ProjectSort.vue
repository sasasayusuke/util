<template>
  <CommonSort class="is-tall" @sort="$emit('sort')" @clear="$emit('clear')">
    <ProjectSortInput
      :param="param"
      :suggest-customers="suggestCustomers"
      :project-types="projectTypes"
      :is-loading="isLoading"
      v-on="$listeners"
    />
  </CommonSort>
</template>

<script lang="ts">
import { format } from 'date-fns'
import { getCurrentDate } from '~/utils/common-functions'
import ProjectSortInput from '~/components/project/molecules/ProjectSortInput.vue'
import BaseComponent from '~/common/BaseComponent'
import CommonSort from '~/components/common/organisms/CommonSort.vue'

// 種別 選択肢
const PROJECT_TYPES = ['main_project', 'all_projects']

export class ProjectSearchParam {
  public dateFrom = format(getCurrentDate(), 'yyyy/MM/dd')
  public dateTo = ''
  public customerId = ''
  public all = false
  public allAssigned = false
}

export default BaseComponent.extend({
  name: 'ProjectListSort',
  components: {
    CommonSort,
    ProjectSortInput,
  },
  props: {
    /** 絞り込みパラメータ */
    param: {
      type: Object,
      required: true,
    },
    /** 一般ユーザーのサジェスト用情報 */
    suggestCustomers: {
      type: Array,
      required: true,
    },
    /** ロード中か */
    isLoading: {
      type: Boolean,
      required: true,
    },
  },
  methods: {
    /** 案件種別を返す */
    projectTypes() {
      const selectItems: object[] = []
      const selectItem = { label: '', value: '' }

      PROJECT_TYPES.forEach((elm) => {
        selectItem.label = this.$t(
          'project.pages.index.sort_input.radio_buttons_items.' + elm
        ) as string
        selectItem.value = elm
        const newElm = Object.assign({}, selectItem)
        selectItems.push(newElm)
      })
      return selectItems
    },
  },
})
</script>
