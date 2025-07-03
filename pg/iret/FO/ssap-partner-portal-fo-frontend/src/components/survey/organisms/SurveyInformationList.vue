<template>
  <Sheet
    v-if="survey.id"
    class="pa-8 o-survey-info"
    :class="type === 'input' ? 'pb-0' : ''"
  >
    <v-expansion-panel-content class="v-expansion-panel-content__head">
      <p class="pa-0 mx-0 mt-0 mb-8 font-size-small o-survey-info__head">
        <span>{{ introduction }}</span>
      </p>
    </v-expansion-panel-content>
    <template v-if="type === 'input-before-start' || type === 'input'">
      <v-sheet
        v-if="survey.surveyType === 'pp' && type === 'input-before-start'"
        class="px-1 py-2 o-survey-info__body"
      >
        <v-row class="pa-0 ma-0">
          <ItemInformation
            cols="4"
            :title="$t('survey.row.answerUserName')"
            :text="survey.answerUserName"
          />
          <ItemInformation
            cols="4"
            :title="$t('survey.row.surveyTargetDuration')"
            :text="survey.surveyTargetDuration"
          />
        </v-row>
      </v-sheet>
      <v-sheet
        v-else-if="survey.surveyType !== 'pp'"
        class="px-1 py-2 o-survey-info__body"
      >
        <v-expansion-panel-content class="py-2">
          <v-row class="pa-0 ma-0">
            <ItemInformation
              cols="4"
              :title="$t('survey.row.company')"
              :text="survey.surveyCompany"
            />
            <ItemInformation
              v-if="isCustomer"
              cols="4"
              :title="$t('survey.row.projectName')"
              :text="survey.projectName"
            />
            <ItemInformation
              v-else
              cols="4"
              :title="$t('survey.row.projectName')"
              :text="survey.projectName"
            />
            <ItemInformation
              cols="4"
              :title="$t('survey.row.customerName')"
              :text="survey.answerUserName"
            />
          </v-row>
          <v-row class="pa-0 ma-0">
            <ItemInformation
              cols="4"
              :title="$t('survey.row.mainSupporterUserName')"
              :text="survey.mainSupporterUserName"
            />
            <ItemInformation
              cols="4"
              :title="$t('survey.row.supporters')"
              :text="survey.supportersStr"
            />
            <ItemInformation
              cols="4"
              :title="$t('survey.row.salesUserName')"
              :text="survey.salesUserName"
            />
          </v-row>
          <v-row class="pa-0 ma-0">
            <ItemInformation
              cols="4"
              :title="$t('survey.row.serviceTypeName')"
              :text="survey.serviceTypeName"
            />
            <ItemInformation
              cols="4"
              :title="$t('survey.row.surveyTargetDuration')"
              :text="survey.surveyTargetDuration"
            />
          </v-row>
        </v-expansion-panel-content>
        <v-row class="py-0 ma-0 px-2">
          <ItemInformation
            cols="12"
            :title="$t('survey.row.customerSuccess')"
            :text="survey.customerSuccess"
            :border-bottom="false"
          />
        </v-row>
      </v-sheet>
    </template>
  </Sheet>
</template>

<script>
import BaseComponent from '~/common/BaseComponent'
import { Sheet } from '~/components/common/atoms/index'
import ItemInformation from '~/components/survey/molecules/SurveyInformationItem.vue'
import { meStore } from '~/store'
export default BaseComponent.extend({
  components: {
    Sheet,
    ItemInformation,
    meStore,
  },
  props: {
    type: {
      type: String,
      default: '',
      // input-before-start, input, detail等、使用される画面の名称に応じた文字列でレイアウト出し分け
    },
    /** 案件アンケート */
    survey: {
      type: Object,
      default: {},
    },
    /** 導入のテキスト */
    introduction: {
      type: String,
      default: '',
    },
  },
  computed: {
    /** @return お客様ロールの場合trueを返す */
    isCustomer() {
      if (meStore.role === 'customer') return true
    },
  },
})
</script>
<style lang="scss" scoped>
.o-survey-info {
  &__head {
    color: $c-black;
    span {
      white-space: pre-wrap;
    }
  }
  &__body {
    background-color: #f0f0f0;
    border-radius: 4px;
  }
}
</style>

<style lang="scss">
.o-survey-info {
  .v-expansion-panel-content__wrap {
    padding: 0 8px !important;
  }
}
</style>
