<template>
  <CommonDataTable
    :headers="projectListMeHeaders"
    :items="projects"
    :offset-page="offsetPage"
    :page="offsetPage"
    :total="total"
    :limit="limit"
    :is-loading="isLoading"
    v-on="$listeners"
  >
    <template #[`item.name`]="{ item }">
      <nuxt-link
        :to="{ path: '/survey/admin/list', query: { projectId: item.id } }"
      >
        {{ item.name }}
      </nuxt-link>
    </template>
    <template #[`item.supportDateFrom`]="{ item }">
      {{ item.supportDateFrom }} 〜 {{ item.supportDateTo }}
    </template>
  </CommonDataTable>
</template>

<script lang="ts">
import CommonDataTable, {
  IDataTableHeader,
} from '~/components/common/organisms/CommonDataTable.vue'

import BaseComponent, { PropType } from '~/common/BaseComponent'
import { ProjectListItem } from '~/models/Project'

export default BaseComponent.extend({
  name: '',
  components: {
    CommonDataTable,
  },
  props: {
    /** 案件一覧 */
    projects: {
      type: Array as PropType<ProjectListItem[]>,
      required: true,
    },
    /** 案件一覧総件数 */
    total: {
      type: Number,
      required: true,
    },
    /** ページ数 */
    offsetPage: {
      type: Number,
      required: true,
    },
    /** ページに表示される担当案件数 */
    limit: {
      type: Number,
      required: true,
    },
    /** ロード中か */
    isLoading: {
      type: Boolean,
      required: true,
    },
  },

  data(): { projectListMeHeaders: IDataTableHeader[] } {
    return {
      /** 担当案件画面ヘッダー情報 */
      projectListMeHeaders: [
        {
          text: this.$t('project.pages.me.table.header.name'),
          value: 'name',
          sortable: false,
          maxLength: 33,
          link: {
            prefix: '/project/',
            value: 'id',
          },
        },
        {
          text: this.$t('project.pages.me.table.header.customerName'),
          value: 'customerName',
          sortable: false,
          maxLength: 33,
        },
        {
          text: this.$t('project.pages.me.table.header.serviceTypeName'),
          value: 'serviceTypeName',
          sortable: false,
        },
        {
          text: this.$t('project.pages.me.table.header.supportPeriod'),
          value: 'supportDateFrom',
          sortable: false,
        },
      ],
    }
  },
  computed: {
    formattedProject() {
      return this.projects.map((project) => {
        const rtn = Object.assign(new ProjectListItem(), project, {
          supportDatePeriod: '',
        })
        if (project.supportDateFrom && project.supportDateTo) {
          rtn.supportDatePeriod = `${project.supportDateFrom}〜${project.supportDateTo}`
        }

        return rtn
      })
    },
  },
})
</script>
