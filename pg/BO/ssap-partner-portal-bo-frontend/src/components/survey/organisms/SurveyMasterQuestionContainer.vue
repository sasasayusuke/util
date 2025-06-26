<template>
  <ScheduleContainer
    v-if="!isEditing"
    :title="title"
    :is-editing="isEditing"
    is-hide-button1
    is-hide-button2
    @click:bulkEdit="$emit('click:bulkEdit')"
    @click:add="$emit('click:add')"
  >
    <SurveyMasterQuestionListTable
      :survey-masters="surveyMasters"
      @click:refer="emitRefer"
      @click:edit="emitEdit"
      @click:delete="emitCopy"
      @click:draft="emitDraft"
    />
  </ScheduleContainer>
</template>

<script lang="ts">
import CommonDetailContainer from '~/components/common/organisms/CommonDetailContainer.vue'
import SurveyMasterQuestionListTable from '~/components/survey/organisms/SurveyMasterQuestionListTable.vue'
import ScheduleContainer from '~/components/common/organisms/ScheduleContainer.vue'

export default CommonDetailContainer.extend({
  components: {
    ScheduleContainer,
    SurveyMasterQuestionListTable,
  },
  props: {
    /** 表示タイトル */
    title: {
      type: String,
    },
    /** 読み込み中か */
    isEditing: {
      type: Boolean,
      required: true,
    },
    /** アンケートマスター一覧 */
    surveyMasters: {
      type: Array,
      required: true,
    },
  },
  computed: {
    getSurveySchedules() {},
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
