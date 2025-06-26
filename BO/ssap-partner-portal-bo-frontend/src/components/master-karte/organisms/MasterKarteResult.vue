<template>
  <div>
    <MasterKarteDetailSection
      :title="
        $t('master-karte.pages.detail.container.result.projectResultTitle')
      "
    >
      <!-- カスタマーサクセス結果（達成／未達） -->
      <MasterKarteDetailRow
        :label="
          $t(
            'master-karte.pages.detail.container.result.customerSuccessResult.name'
          )
        "
        icon="void"
      >
        <template #isNotEditing>
          <RadioGroup
            v-show="result.customerSuccessResult"
            v-model="result.customerSuccessResult"
            :labels="
              $t(
                'master-karte.pages.detail.container.result.customerSuccessResult.radio'
              ).labels
            "
            :values="
              $t(
                'master-karte.pages.detail.container.result.customerSuccessResult.radio'
              ).values
            "
            is-no-editing
            horizontal
          />
        </template>
      </MasterKarteDetailRow>
      <!-- カスタマーサクセス達成/未達要因 -->
      <MasterKarteDetailRow
        :label="
          $t(
            'master-karte.pages.detail.container.result.customerSuccessResultFactor'
          )
        "
        :text="result.customerSuccessResultFactor"
        icon="void"
      >
      </MasterKarteDetailRow>

      <!-- 支援で生じた課題 -->
      <MasterKarteDetailRow
        :label="$t('master-karte.pages.detail.container.result.supportIssue')"
        :text="result.supportIssue"
        icon="void"
      >
      </MasterKarteDetailRow>

      <!-- 解決できた要因/解決できなかった要因 -->
      <MasterKarteDetailRow
        :label="
          $t('master-karte.pages.detail.container.result.supportSuccessFactor')
        "
        :text="result.supportSuccessFactor"
        icon="void"
      >
      </MasterKarteDetailRow>

      <!-- 顧客評価に対する自己評価 -->
      <MasterKarteDetailRow
        :label="
          $t('master-karte.pages.detail.container.result.surveySsapAssessment')
        "
        :text="result.surveySsapAssessment"
        icon="void"
      >
      </MasterKarteDetailRow>

      <!-- 次期支援内容 -->
      <MasterKarteDetailRow
        :label="
          $t('master-karte.pages.detail.container.result.nextSupportContent')
        "
        :text="result.nextSupportContent"
        icon="void"
      >
      </MasterKarteDetailRow>

      <!-- 終了時アンケート -->
      <MasterKarteDetailRow
        :subtitle="
          $t('master-karte.pages.detail.container.result.completedSurvey')
        "
        :label="
          $t('master-karte.pages.detail.container.result.completedSurveyDetail')
        "
        :survey-id="masterKarteProject.currentProgram.result.surveyId"
        icon="void"
      >
      </MasterKarteDetailRow>
      <MasterKarteDetailRow
        :label="
          $t('master-karte.pages.detail.container.result.completedSurveyReview')
        "
        :radios="result.satisfactionEvaluation"
        icon="void"
      >
      </MasterKarteDetailRow>
    </MasterKarteDetailSection>
  </div>
</template>

<script lang="ts">
import DetailContainer from '~/components/common/organisms/DetailContainer.vue'
import CommonDetailContainer from '~/components/common/organisms/CommonDetailContainer.vue'
import MasterKarteDetailRow from '~/components/master-karte/molecules/MasterKarteDetailRow.vue'
import type { PropType } from '~/common/BaseComponent'
import { Result, GetMasterKarteByIdResponse } from '~/types/MasterKarte'
import { RadioGroup } from '~/components/common/atoms/index'

export default CommonDetailContainer.extend({
  components: {
    DetailContainer,
    MasterKarteDetailRow,
    RadioGroup,
  },
  props: {
    masterKarteProject: {
      type: Object as PropType<GetMasterKarteByIdResponse>,
    },
    isLoading: {
      type: Boolean,
      default: false,
    },
  },
  data() {
    return {}
  },
  computed: {
    result(): Result {
      return this.masterKarteProject.currentProgram.result
    },
  },
})
</script>

<style></style>
