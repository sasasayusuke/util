<template>
  <CommonDataTable
    :headers="surveyMasterHeaders"
    :items="formattedSurveyMaster"
    :total="total"
    is-hide-button
    :short-text="true"
    :shows-pagination="false"
    class="mt-10"
    :is-loading="isLoading"
    v-on="$listeners"
  >
  </CommonDataTable>
</template>

<script lang="ts">
import CommonDataTable from '../../common/organisms/CommonDataTable.vue'
import type { IDataTableHeader } from '../../common/organisms/CommonDataTable.vue'
import { SurveyMasterListItem } from '~/models/Master'
import BaseComponent from '~/common/BaseComponent'
import type { PropType } from '~/common/BaseComponent'
import { formatDateStr } from '~/utils/common-functions'

export default BaseComponent.extend({
  components: {
    CommonDataTable,
  },
  props: {
    /** アンケートマスター一覧 */
    surveyMasters: {
      type: Array as PropType<SurveyMasterListItem[]>,
      required: true,
    },
    /** 全件数 */
    total: {
      type: Number,
      required: true,
    },
    /** 読み込み中か */
    isLoading: {
      type: Boolean,
      required: true,
    },
  },
  data(): { surveyMasterHeaders: IDataTableHeader[] } {
    return {
      surveyMasterHeaders: [
        {
          text: this.$t('survey.pages.masterList.header.name'),
          align: 'start',
          sortable: false,
          value: 'name',
          width: '352px',
          maxLength: 26,
          link: {
            prefix: '/survey/master/',
            value: 'id',
          },
        },
        {
          text: this.$t('survey.pages.masterList.header.type'),
          sortable: false,
          value: 'type',
          width: '352px',
          maxLength: 26,
        },
        {
          text: this.$t('survey.pages.masterList.header.timing'),
          sortable: false,
          value: 'timing',
          width: '223px',
        },
        {
          text: this.$t('survey.pages.masterList.header.revision'),
          sortable: false,
          value: 'revision',
          width: '92px',
        },
        {
          text: this.$t('survey.pages.masterList.header.updateAt'),
          sortable: false,
          value: 'updateAt',
          width: '138px',
        },
      ],
    }
  },
  computed: {
    /**
     * 表示用のフォーマットに変換したアンケートマスター配列を返す
     * @returns フォーマット済みアンケートマスター配列
     */
    formattedSurveyMaster(): SurveyMasterListItem[] {
      return this.surveyMasters.map((surveyMaster: SurveyMasterListItem) => {
        const rtn = Object.assign(new SurveyMasterListItem(), surveyMaster)
        rtn.type = this.$t(
          'survey.pages.masterList.row.type.' + rtn.type
        ) as string
        rtn.timing = this.$t(
          'survey.pages.masterList.row.timing.' + rtn.timing
        ) as string
        rtn.updateAt = formatDateStr(surveyMaster.updateAt, 'Y/MM/dd HH:mm')
        return rtn
      })
    },
  },
  methods: {},
})
</script>
