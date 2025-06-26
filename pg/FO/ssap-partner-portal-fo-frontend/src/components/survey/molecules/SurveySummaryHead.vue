<template>
  <div class="d-flex font-size-small">
    <dl class="d-flex">
      <dt class="font-weight-bold">
        {{ $t('survey.terminology.summaryMonth') }}:
      </dt>
      <dd class="ml-2">{{ summaryMonth }}</dd>
    </dl>
    <dl class="d-flex ml-10">
      <dt class="font-size-small font-weight-bold">
        {{ $t('survey.terminology.summaryTarget') }}:
      </dt>
      <dd class="ml-2">{{ isTarget }}</dd>
    </dl>
    <template v-if="surveyType !== 'pp'">
      <dl class="d-flex ml-10">
        <dt class="font-size-small font-weight-bold">
          {{ $t('survey.terminology.isSolverProject') }}:
        </dt>
        <dd class="ml-2">{{ isSolverProject }}</dd>
      </dl>
    </template>
  </div>
</template>

<script lang="ts">
import BaseComponent from '~/common/BaseComponent'

export default BaseComponent.extend({
  props: {
    /** 集計月 */
    summaryMonth: {
      type: String,
      default: '',
    },
    /** 集計対象 */
    isTarget: {
      type: String,
      default: '',
    },
    /** ソルバー担当 */
    isSolverProject: {
      type: String,
      default: '',
    },
    /** アンケートタイプ */
    surveyType: {
      type: String,
      default: '',
    },
  },
})
</script>
