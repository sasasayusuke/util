<template>
  <div class="m-SurveyForecastStatus">
    <v-container py-5 px-8 fluid>
      <v-row no-gutters>
        <v-col cols="auto">{{ terms }}</v-col>
        <v-col class="d-flex justify-end">
          <span>{{ setSend }}</span>
          <span class="ml-14">{{ setReceive }}</span>
        </v-col>
      </v-row>
    </v-container>
  </div>
</template>

<script lang="ts">
import BaseComponent from '~/common/BaseComponent'

export default BaseComponent.extend({
  components: {},
  computed: {
    /**
     * 表示用にフォーマット済みの送信実績文字列を返す
     * @returns フォーマット済み送信実績文字列
     */
    setSend() {
      const text = this.$t('survey.pages.forecast.status.send', {
        plan: this.send.plan.toLocaleString(),
        achievement: this.send.achievement.toLocaleString(),
      })
      return text
    },
    /**
     * 表示用にフォーマット済みの受信実績文字列を返す
     * @returns フォーマット済み受信実績文字列
     */
    setReceive() {
      const text = this.$t('survey.pages.forecast.status.receive', {
        plan: this.receive.plan.toLocaleString(),
        achievement: this.receive.achievement.toLocaleString(),
      })
      return text
    },
  },
  props: {
    /** 年月期間文字列 */
    terms: {
      type: String,
      default: '',
      required: true,
    },
    /** 送信実績文字列 */
    send: {
      type: Object,
      required: true,
    },
    /** 受信実績文字列 */
    receive: {
      type: Object,
      required: true,
    },
    /** 読み込み中か */
    isLoading: {
      type: Boolean,
      required: true,
    },
  },
  data() {
    return {}
  },
  methods: {},
})
</script>

<style lang="scss" scoped>
.m-SurveyForecastStatus {
  border-radius: 4px;
  border: 2px solid #c9c9c9;
  @include fontSize('large');
  font-weight: bold;
  background: $c-white;
}
</style>
