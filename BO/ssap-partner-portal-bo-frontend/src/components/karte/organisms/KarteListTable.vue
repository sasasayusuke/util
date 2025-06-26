<template>
  <CommonDataTable
    :headers="karteHeaders"
    :items="formattedProjects"
    :total="total"
    :offset-page="offsetPage"
    :page="offsetPage"
    :max-page="Math.ceil(total / limit)"
    :limit="limit"
    :is-loading="isLoading"
    v-on="$listeners"
  >
    <template #[`item.name`]="{ item }">
      <nuxt-link
        class="o-common-data-table__link"
        :to="`/karte/list/${item.id}`"
        tabindex="0"
      >
        <OverflowTooltip :text="item.name" :max="32"
      /></nuxt-link>
    </template>
    <template #[`item.customerName`]="{ item }">
      <OverflowTooltip :text="item.customerName" :max="32" />
    </template>
  </CommonDataTable>
</template>
<script lang="ts">
import { format } from 'date-fns'
import CommonDataTable from '~/components/common/organisms/CommonDataTable.vue'
import type { IDataTableHeader } from '~/components/common/organisms/CommonDataTable.vue'
import { ProjectListItem } from '~/models/Project'
import BaseComponent from '~/common/BaseComponent'
import OverflowTooltip from '~/components/common/molecules/OverflowTooltip.vue'
import type { PropType } from '~/common/BaseComponent'

export default BaseComponent.extend({
  name: 'KarteListTable',
  components: {
    CommonDataTable,
    OverflowTooltip,
  },
  props: {
    /**
     * GetProjects APIのレスポンス
     */
    projects: {
      type: Array as PropType<ProjectListItem[]>,
      required: true,
    },
    /**
     * 合計件数
     */
    total: {
      type: Number,
      required: true,
    },
    /**
     * 表示するページ
     */
    offsetPage: {
      type: Number,
      required: true,
    },
    /**
     * 一ページ当たりの件数
     */
    limit: {
      type: Number,
      required: true,
      default: -1,
    },
    /**
     * ページがリロード中か否か
     */
    isLoading: {
      type: Boolean,
      required: true,
    },
  },
  data(): { karteHeaders: IDataTableHeader[] } {
    return {
      karteHeaders: [
        {
          text: this.$t('karte.pages.index.header.serviceTypeName'),
          align: 'start',
          value: 'serviceTypeName',
          sortable: false,
          width: '117px',
        },
        {
          text: this.$t('karte.pages.index.header.name'),
          align: 'start',
          value: 'name',
          sortable: false,
          width: '375px',
          maxLength: 45,
          link: {
            prefix: '/karte/list/',
            value: 'id',
          },
        },
        {
          text: this.$t('karte.pages.index.header.customerName'),
          value: 'customerName',
          maxLength: 45,
          sortable: false,
          width: '375px',
        },
        {
          text: this.$t('karte.pages.index.header.supportDateFrom'),
          value: 'supportDateFrom',
          sortable: false,
          width: '180px',
        },
        {
          text: this.$t('karte.pages.index.header.updateAt'),
          value: 'updateAt',
          sortable: false,
          width: '150px',
        },
      ],
    }
  },
  computed: {
    // 表の表示形式を揃えるための変換は、ListTable organismで行う
    // (上流で元データを加工しないように)
    /**
     * 支援期間を「支援開始日〜支援終了日」という形に修正。最終更新日時のフォーマットを変更
     */
    formattedProjects() {
      return this.projects.map((project) => {
        const rtn = Object.assign(new ProjectListItem(), project)
        rtn.supportDateFrom =
          project.supportDateFrom + ' ～ ' + project.supportDateTo
        rtn.updateAt = format(new Date(project.updateAt), 'yyyy/MM/dd HH:mm')
        return rtn
      })
    },
  },
})
</script>
