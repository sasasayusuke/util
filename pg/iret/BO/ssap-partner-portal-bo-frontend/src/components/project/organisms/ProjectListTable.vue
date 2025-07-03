<template>
  <CommonDataTable
    :headers="projectHeaders"
    :items="formattedProject"
    :total="total"
    :offset-page="offsetPage"
    :limit="limit"
    :is-loading="isLoading"
    is-scroll
    v-on="$listeners"
  >
  </CommonDataTable>
</template>

<script lang="ts">
import CommonDataTable, {
  IDataTableHeader,
} from '~/components/common/organisms/CommonDataTable.vue'
import BaseComponent from '~/common/BaseComponent'
import { ProjectListItem } from '~/models/Project'
import type { PropType } from '~/common/BaseComponent'
import { formatDateStr } from '~/utils/common-functions'

export default BaseComponent.extend({
  components: {
    CommonDataTable,
  },
  props: {
    /** 案件情報 */
    projects: {
      type: Array as PropType<ProjectListItem[]>,
      required: true,
    },
    /** 全件数 */
    total: {
      type: Number,
      required: true,
    },
    /** 現在のページ */
    offsetPage: {
      type: Number,
      required: true,
    },
    /** 1ページの表示最大件数 */
    limit: {
      type: Number,
      required: true,
    },
    /** 読み込み中か */
    isLoading: {
      type: Boolean,
      required: true,
    },
  },
  data(): { projectHeaders: IDataTableHeader[] } {
    return {
      projectHeaders: [
        {
          text: this.$t('project.pages.index.header.serviceTypeName'),
          value: 'serviceTypeName',
          sortable: false,
          maxLength: 12,
        },
        {
          text: this.$t('project.pages.index.header.name'),
          value: 'name',
          sortable: false,
          maxLength: 12,
          link: {
            prefix: '/project/',
            value: 'id',
          },
        },
        {
          text: this.$t('project.pages.index.header.status'),
          value: 'phase',
          sortable: false,
        },
        {
          text: this.$t('project.pages.index.header.customerName'),
          value: 'customerName',
          sortable: false,
          maxLength: 15,
        },
        {
          text: this.$t('project.pages.index.header.mainSalesUserName'),
          value: 'mainSalesUserName',
          sortable: false,
          maxLength: 6,
        },
        {
          text: this.$t('project.pages.index.header.supporterOrganizationName'),
          value: 'supporterOrganizationName',
          sortable: false,
        },
        {
          text: this.$t('project.pages.index.header.supportDatePeriod'),
          value: 'supportDatePeriod',
          sortable: false,
        },
        {
          text: this.$t('project.pages.index.header.salesforceUpdateAt'),
          value: 'salesforceUpdateAt',
          sortable: false,
        },
        {
          text: this.$t('project.pages.index.header.updateAt'),
          value: 'updateAt',
          sortable: false,
        },
        {
          text: this.$t('project.pages.index.header.isSolverProject'),
          value: 'isSolverProjectText',
          sortable: false,
        },
      ],
    }
  },
  computed: {
    /**
     * 表示用のフォーマットに変換した案件情報を返す
     * @returns フォーマット済み案件情報配列
     */
    formattedProject(): ProjectListItem[] {
      return this.projects.map((project) => {
        const rtn = Object.assign(new ProjectListItem(), project, {
          supportDatePeriod: '',
          isSolverProjectText: '',
        })
        if (project.salesforceUpdateAt) {
          rtn.salesforceUpdateAt = formatDateStr(
            project.salesforceUpdateAt,
            'Y/MM/dd HH:mm'
          )
        }
        if (project.updateAt) {
          rtn.updateAt = formatDateStr(project.updateAt, 'Y/MM/dd HH:mm')
        }

        if (project.supportDateFrom && project.supportDateTo) {
          rtn.supportDatePeriod = `${project.supportDateFrom}〜${project.supportDateTo}`
        }

        if (project.isSolverProject) {
          rtn.isSolverProjectText = this.$t('common.detail.yes') as string
        } else {
          rtn.isSolverProjectText = this.$t('common.detail.no') as string
        }

        return rtn
      })
    },
  },
})
</script>

<style lang="scss"></style>
