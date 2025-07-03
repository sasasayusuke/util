<template>
  <section>
    <v-container class="pa-0 mb-4">
      <v-flex class="d-flex justify-space-between align-center">
        <h2 class="font-size-large font-weight-bold">
          {{ $t('top.pages.home.header.surveys') }}
        </h2>
      </v-flex>
    </v-container>
    <v-container v-if="omitExpiredSurveys.length" class="pa-0">
      <Card elevation="3" class="m-HomeSurveys__card">
        <SimpleTable>
          <HomeSurveysCard
            v-for="(survey, n) in omitExpiredSurveys"
            :key="n"
            :survey="survey"
          />
        </SimpleTable>
      </Card>
    </v-container>
    <Alert v-else style-set="no_data">
      {{ $t('common.alert.no_data') }}
    </Alert>
  </section>
</template>

<script lang="ts">
import HomeSurveysCard from '../molecules/HomeSurveysCard.vue'
import BaseComponent from '~/common/BaseComponent'
import type { PropType } from '~/common/BaseComponent'
import { Alert, Card, SimpleTable } from '~/components/common/atoms/index'
import { SurveyByMineItem } from '~/models/Survey'

export default BaseComponent.extend({
  components: {
    HomeSurveysCard,
    Alert,
    Card,
    SimpleTable,
  },
  props: {
    /**
     * アンケート情報
     */
    surveys: {
      type: Array as PropType<SurveyByMineItem[]>,
    },
  },
  computed: {
    /**
     * 未回答かつ回答期限日が過ぎていないアンケートのみ取得
     */
    omitExpiredSurveys() {
      const surveys: SurveyByMineItem[] = [] as SurveyByMineItem[]
      for (const i in this.surveys) {
        // アンケート未入力のもののみ表示(回答期限日は問わない)
        if (this.surveys[i].isFinished === false) {
          surveys.push(this.surveys[i])
        }
      }
      surveys.sort(function (a, b) {
        if (a.planSurveyResponseDatetime === null) {
          return 1
        }
        if (b.planSurveyResponseDatetime === null) {
          return -1
        }
        if (a.planSurveyResponseDatetime > b.planSurveyResponseDatetime)
          return -1
        if (b.planSurveyResponseDatetime > a.planSurveyResponseDatetime)
          return 1

        return 0
      })
      return surveys
    },
  },
})
</script>

<style lang="scss">
.m-HomeSurveys__card {
  tbody {
    &:nth-child(n + 2) {
      th,
      td {
        border-top: 1px solid $c-gray-line;
      }
    }
  }
}
</style>
