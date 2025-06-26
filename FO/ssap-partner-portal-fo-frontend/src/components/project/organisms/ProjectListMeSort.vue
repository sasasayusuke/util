<template>
  <CommonSort
    :title="$t('project.pages.me.sort_input.display_period2')"
    @sort="$emit('sort')"
    @clear="$emit('clear')"
  >
    <ProjectListMeSortInput
      :param="param"
      :project-types="projectTypes"
      v-on="$listeners"
    />
  </CommonSort>
</template>

<script lang="ts">
import { format } from 'date-fns'
import ProjectListMeSortInput from '../../project/molecules/ProjectListMeSortInput.vue'
import { getCurrentDate } from '~/utils/common-functions'
import BaseComponent from '~/common/BaseComponent'
import CommonSort from '~/components/common/organisms/CommonSort.vue'

// 種別 選択肢
const PROJECT_TYPES = ['main_project', 'all_projects']

export class ProjectMeSearchParam {
  public fromYearMonth = format(getCurrentDate(), 'yyyy/MM')
  public toYearMonth = ''
  public all = false
  public allAssigned = false
}

export default BaseComponent.extend({
  name: 'ProjectListMeSort',
  components: {
    CommonSort,
    ProjectListMeSortInput,
  },
  props: {
    /** 絞り込みパラメータ */
    param: {
      type: Object,
      required: true,
    },
  },
  data() {
    return {
      /** 下記全て恐らく利用されていない */
      displayPeriodFrom: '',
      displayPeriodTo: '',
      assignerType: '',
    }
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
