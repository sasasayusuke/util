<template>
  <CommonDataTable
    :headers="projectHeaders"
    :items="formattedProject"
    :total="total"
    :offset-page="offsetPage"
    :is-loading="isLoading"
    :shows-pagination="false"
    :short-text="true"
    link-prefix="/project"
    v-on="$listeners"
  >
    <template #[`item.id`]="{ item }">
      <div class="d-flex justify-end">
        <nuxt-link :to="`/karte/list/${item.id}`">{{
          $t('project.pages.index.table.link.karte')
        }}</nuxt-link>
        <span class="mx-3">|</span>
        <nuxt-link v-if="linkToProjectDetail" :to="`/project/${item.id}`">{{
          $t('project.pages.index.table.link.project')
        }}</nuxt-link>
        <span v-if="linkToProjectDetail" class="mx-3">|</span>
        <nuxt-link :to="`/survey/list/${item.id}`">{{
          $t('project.pages.index.table.link.survey')
        }}</nuxt-link>
      </div>
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
import { TextLink } from '~/components/common/atoms/index'
import { ProjectListItem } from '~/models/Project'

export default BaseComponent.extend({
  name: 'ProjectListTable',
  components: {
    CommonDataTable,
    TextLink,
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
    /** ロード中か */
    isLoading: {
      type: Boolean,
      required: true,
    },
    /** 案件詳細へ遷移できるか(顧客は不可) */
    linkToProjectDetail: {
      type: Boolean,
      required: true,
    },
  },
  data(): { projectHeaders: IDataTableHeader[] } {
    return {
      projectHeaders: [
        {
          text: this.$t('project.pages.index.table.header.name'),
          value: 'name',
          sortable: false,
          maxLength: 24,
        },
        {
          text: this.$t('project.pages.index.table.header.customerName'),
          value: 'customerName',
          sortable: false,
          maxLength: 16,
        },
        {
          text: this.$t('project.pages.index.table.header.supportPeriod'),
          value: 'supportDateFrom',
          sortable: false,
        },
        { text: '', value: 'id', sortable: false },
      ],
    }
  },
  computed: {
    /** 案件一覧の各案件の支援期間を修正 */
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
