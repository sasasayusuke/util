<template>
  <section>
    <v-container class="pa-0 mb-4">
      <v-flex class="d-flex justify-space-between align-center">
        <h2 class="font-size-large font-weight-bold">
          {{ $t('top.pages.home.header.questionnaire') }}
        </h2>
        <Button style-set="small-primary" outlined :to="`/survey/pp/list`">{{
          $t('common.button.viewAll')
        }}</Button>
      </v-flex>
    </v-container>
    <v-container v-if="!isNotFilterSurveys" class="pa-0">
      <Card elevation="3">
        <SimpleTable>
          <HomeQuestionnaireCard
            v-for="(survey, n) in surveys"
            :key="n"
            :survey="survey"
          />
        </SimpleTable>
      </Card>
    </v-container>
    <Alert v-if="isNotFilterSurveys" style-set="no_data">
      {{ $t('common.alert.no_data') }}
    </Alert>
  </section>
</template>

<script lang="ts">
import HomeQuestionnaireCard from '../molecules/HomeQuestionnaireCard.vue'
import BaseComponent from '~/common/BaseComponent'
import type { PropType } from '~/common/BaseComponent'
import {
  Button,
  Alert,
  Card,
  SimpleTable,
} from '~/components/common/atoms/index'
import { SurveyByMineItem } from '~/models/Survey'

export default BaseComponent.extend({
  components: {
    HomeQuestionnaireCard,
    Button,
    Alert,
    Card,
    SimpleTable,
  },
  data(): { surveyData: number } {
    return {
      surveyData: 0,
    }
  },
  props: {
    /**
     * 未回答アンケート情報
     */
    surveys: {
      type: Array as PropType<SurveyByMineItem[]>,
    },
  },

  computed: {
    /**
     * @returns PPアンケート表示データが存在しないかどうか
     */
    isNotFilterSurveys(): boolean {
      const filterSurveys = this.surveys.filter((survey) => {
        return !survey.isFinished
      })
      return filterSurveys.length === this.surveyData
    },
  },
})
</script>
