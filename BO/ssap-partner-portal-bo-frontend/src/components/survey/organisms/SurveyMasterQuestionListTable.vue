<template>
  <QuestionTable
    :headers="surveyHeaders"
    :survey-masters="surveyMasters"
    is-hide-pagination
    link-prefix="/survey/master/"
    class="pa-6"
    @click:refer="emitRefer"
    @click:edit="emitEdit"
    @click:delete="emitCopy"
    @click:draft="emitDraft"
  >
  </QuestionTable>
</template>

<script lang="ts">
import type { IDataTableHeader } from '../../common/organisms/CommonDataTable.vue'
import QuestionTable from '~/components/survey/organisms/QuestionTable.vue'
import { SurveyMasterListItem } from '~/models/Master'
import BaseComponent from '~/common/BaseComponent'
import type { PropType } from '~/common/BaseComponent'

export default BaseComponent.extend({
  components: {
    QuestionTable,
  },
  props: {
    /** アンケートマスター一覧 */
    surveyMasters: {
      type: Array as PropType<SurveyMasterListItem[]>,
      required: true,
    },
  },
  data(): { surveyHeaders: IDataTableHeader[] } {
    return {
      surveyHeaders: [
        {
          text: this.$t('survey.pages.masterDetail.version.table.status'),
          align: 'start',
          value: 'status',
          sortable: false,
          width: '80px',
        },
        {
          text: this.$t('survey.pages.masterDetail.version.table.version'),
          value: 'revision',
          sortable: false,
          width: '180px',
        },
        {
          text: this.$t('survey.pages.masterDetail.version.table.versionMemo'),
          value: 'memo',
          sortable: false,
          width: '440px',
        },
        {
          text: this.$t(
            'survey.pages.masterDetail.version.table.questionAmount'
          ),
          value: 'questionCount',
          sortable: false,
          width: '80px',
        },
        {
          text: this.$t('survey.pages.masterDetail.version.table.updateAt'),
          value: 'updateAt',
          sortable: false,
          width: '166px',
        },
        {
          text: '',
          value: 'actions',
          align: 'end',
          sortable: false,
          width: '24px',
        },
      ],
    }
  },
  methods: {
    /**
     * アンケートひな形設問詳細を表示
     * @param id アンケートマスターID
     * @param revision リビジョン番号
     */
    emitRefer(id: String, revision: number) {
      this.$emit('click:refer', id, revision)
    },
    /**
     * アンケートひな形設問詳細を編集
     * @param id アンケートマスターID
     * @param revision リビジョン番号
     */
    emitEdit(id: String, revision: number) {
      this.$emit('click:edit', id, revision)
    },
    /**
     * 指定したアンケートマスターを運用中に変更
     * @param id アンケートマスターID
     * @param revision リビジョン番号
     */
    emitDraft(id: String, revision: number) {
      this.$emit('click:draft', id, revision)
    },
    /**
     * 指定したアンケートマスターをコピー作成
     * @param id アンケートマスターID
     * @param originRevision リビジョン番号
     */
    emitCopy(id: String, originRevision: number) {
      this.$emit('click:delete', id, originRevision)
    },
  },
})
</script>
