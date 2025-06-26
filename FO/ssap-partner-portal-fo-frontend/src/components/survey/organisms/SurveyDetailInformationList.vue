<template>
  <Sheet v-if="survey.id" class="px-8 pt-5 o-survey-info" :class="className">
    <template v-if="type === 'detail'">
      <v-sheet
        v-if="survey.surveyType === 'pp'"
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
          <ItemInformation
            cols="4"
            :title="$t('survey.row.actualSurveyResponseDatetime')"
            :text="survey.actualSurveyResponseDatetime"
          />
        </v-row>
      </v-sheet>
      <v-sheet
        v-else-if="survey.surveyType !== 'pp'"
        class="px-1 py-2 o-survey-info__body"
      >
        <v-row class="py-0 px-2 ma-0">
          <ItemInformation
            cols="4"
            :title="$t('survey.row.company')"
            :text="survey.surveyCompany"
          />
          <ItemInformation
            cols="4"
            :title="$t('survey.row.projectName')"
            :text="survey.projectName"
            :link="!isAnonymous ? `/project/${survey.projectId}` : ''"
          />
          <ItemInformation
            cols="4"
            :title="$t('survey.row.customerName')"
            :text="survey.answerUserName"
          />
        </v-row>
        <v-expansion-panel-content>
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
            <ItemInformation
              cols="4"
              :title="$t('survey.row.actualSurveyResponseDatetime')"
              :text="survey.actualSurveyResponseDatetime"
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
    /** 匿名アンケートか否か */
    isAnonymous: {
      type: Boolean,
      default: false,
    },
  },
  data() {
    return {
      role: meStore.role,
    }
  },
  computed: {
    /**
     * タイプよってクラス名を変更
     * @return タイプが「detail」以外は空の文字列を返す
     * */
    className() {
      return this.type === 'detail' ? 'o-survey-info--detail' : ''
    },
  },
})
</script>
<style lang="scss" scoped>
.o-survey-info {
  background-color: $c-white !important;
  &__head {
    color: $c-black;
  }
  &__body {
    background-color: #f0f0f0;
  }
}
.o-survey-info--detail {
  padding-bottom: 0 !important;
  padding-top: 20px;
}
</style>
