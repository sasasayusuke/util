<template>
  <div class="pa-0 ma-0 o-survey-qa" :class="{ 'is-disclosure': isDisclosure }">
    <!-- 設問 -->
    <v-row
      class="px-4 py-4 o-survey-qa__question justify-space-between align-center"
    >
      <div class="d-flex pa-0 ma-0 justify-start" :class="className2">
        <div v-if="isQuestionNumber" class="mr-4">
          {{ $t('survey.pages.common.question') + currentPage
          }}<span v-if="type === 1"> : </span>
        </div>
        <div class="o-survey-qa__question__desc" :class="className">
          <span>{{ question.description }}</span>
        </div>
      </div>
      <template v-if="!isDisclosure">
        <Chip v-if="isRequired" x-small style-set="required">{{
          $t('survey.pages.common.required')
        }}</Chip>
        <Chip v-else-if="isAny" x-small style-set="any">{{
          $t('survey.pages.common.any')
        }}</Chip>
      </template>
    </v-row>

    <Sheet
      class="px-4 pt-4 mx-0 mb-0"
      :class="type === 1 ? 'pb-8 mt-0' : 'pb-4 mt-6'"
    >
      <template v-if="question.format === 'radio'">
        <!-- radio -->
        <div
          v-for="(choice, index) in question.choices"
          :key="index"
          class="o-option-group"
        >
          <h4 v-if="choice.description">{{ choice.description }}</h4>
          <v-row class="ma-0 pa-0">
            <RadioGroup
              v-model="answerValue"
              class="a-radio--survey"
              :labels="choice.options"
              :values="choice.optionValues"
              horizontal
              hide-details
              type="2"
              :readonly="readonly"
              @change="$emit('change', $event)"
            />
          </v-row>
        </div>
      </template>

      <template v-if="question.format === 'checkbox'">
        <!-- checkbox -->
        <div
          v-for="(choice, index) in question.choices"
          :key="index"
          class="o-option-group"
        >
          <h4 v-if="choice.description">{{ choice.description }}</h4>
          <v-row class="pa-0 ma-0">
            <v-col
              v-for="(item, index_opt) in choice.group"
              :key="index_opt"
              :cols="question.isEnd ? 12 : 3"
              class="pa-0 ma-0"
            >
              <Checkbox
                v-model="answerValues"
                class="mr-6 mt-0 mb-2 a-checkbox--survey"
                :label="item.title"
                :value="item.id"
                hide-details
                :readonly="readonly"
                type="2"
                @change="$emit('change', $event)"
              />
            </v-col>
          </v-row>
        </div>
      </template>

      <template v-if="question.format === 'textarea'">
        <!-- textarea -->
        <Sheet v-if="readonly" style-set="no-editing" class="mt-3">
          <Paragraph
            v-for="(text, p_index) in brText(answerValue)"
            :key="p_index"
            style-set="small"
            class="mb-0"
          >
            {{ text }}
          </Paragraph>
        </Sheet>
        <Textarea
          v-else
          v-model="answerValue"
          outlined
          :placeholder="$t('common.placeholder.textarea')"
          :readonly="readonly"
          @input="$emit('change', $event)"
        />
      </template>

      <template v-if="question.otherDescription">
        <h4 :class="className3">{{ question.otherDescription }}</h4>
        <!-- textarea -->
        <Sheet v-if="readonly" style-set="no-editing" class="mt-3">
          <Paragraph
            v-for="(text, p_index) in brText(otherAnswerValue)"
            :key="p_index"
            style-set="small"
            class="mb-0"
          >
            {{ text }}
          </Paragraph>
        </Sheet>
        <Textarea
          v-else
          v-model="otherAnswerValue"
          outlined
          :placeholder="$t('common.placeholder.textarea')"
          :readonly="readonly"
          @input="$emit('change-other', $event)"
        />
      </template>
    </Sheet>
  </div>
</template>

<script lang="ts">
import BaseComponent from '~/common/BaseComponent'
import {
  Sheet,
  RadioGroup,
  Checkbox,
  Textarea,
  Chip,
  Paragraph,
} from '~/components/common/atoms/index'

interface DataType {
  answerValue: string
  answerValues: string[]
  otherAnswerValue: string
  test: string
}
export default BaseComponent.extend({
  components: {
    Sheet,
    RadioGroup,
    Checkbox,
    Textarea,
    Chip,
    Paragraph,
  },
  props: {
    /** 設問 */
    question: {
      type: Object,
      default: {},
    },
    /** 回答 */
    answer: {
      type: String,
      default: '',
    },
    /** 回答 */
    otherAnswer: {
      type: String,
      default: '',
    },
    /** 現在のページ */
    currentPage: {
      type: Number,
      default: 1,
    },
    type: {
      type: Number,
      default: 0,
    },
    /** 読み込み専用の有無 */
    readonly: {
      type: Boolean,
      default: false,
    },
    /** 開示の有無 */
    isDisclosure: {
      type: Boolean,
      default: false,
    },
    /** アンケートタイプ */
    surveyType: {
      type: String,
      default: '',
    },
    /** 満足度評価のみ回答アンケートか */
    isSatisfactionSurvey: {
      type: Boolean,
      default: false,
    },
  },
  watch: {
    currentPage() {
      this.initialize()
    },
  },
  data(): DataType {
    return {
      answerValue: this.answer,
      answerValues: [],
      otherAnswerValue: this.otherAnswer,
      test: 'aaaaaaa\naaaaaaa\naaaaaaa\naaaaaaa\naaaaaaa\naaaaaaa',
    }
  },
  mounted() {
    this.initialize()
  },
  computed: {
    /**
     * @return 設問が回答済みの場合「isEnd」を返す
     */
    className(): string {
      return this.question.isEnd ? 'isEnd' : ''
    },
    /**
     * @return typeが1の場合「isDetail」を返す
     */
    className2(): string {
      return this.type === 1 ? 'isDetail' : ''
    },
    /**
     * @return クラス名
     */
    className3(): string {
      return this.type === 1 ? 'noboard pb-2 pt-4' : 'noboard pb-2 pt-10'
    },
    /**
     * @return 「設問が回答済み」又は「満足度評価のみ回答アンケート」の場合、falseを返す
     */
    isQuestionNumber(): boolean {
      if (this.question.isEnd) {
        return false
      } else if (this.isSatisfactionSurvey) {
        return false
      } else {
        return true
      }
    },
    /**
     * @return 必須の場合true
     */
    isRequired(): boolean {
      if (this.question.required) {
        if (
          this.surveyType !== 'pp' &&
          this.question.isEnd &&
          this.type !== 1
        ) {
          return false
        } else {
          return true
        }
      }
      return false
    },
    /**
     * @return 必須の場合はtrue
     */
    isAny(): boolean {
      if (!this.question.required) {
        if (
          this.surveyType !== 'pp' &&
          this.question.isEnd &&
          this.type !== 1
        ) {
          return false
        } else {
          return true
        }
      }
      return false
    },
  },
  methods: {
    /**
     * 改行(n)のエスケープシーケンスの場合、区切って配列化
     * @param text 文章
     */
    brText(text: string) {
      return text.split('\n')
    },
    /**
     * 入力方法によって回答の入力を変更する
     */
    initialize() {
      if (
        this.question.format === 'radio' ||
        this.question.format === 'textarea'
      ) {
        this.answerValue = this.answer
      } else if (this.question.format === 'checkbox') {
        this.answerValues = this.answer ? this.answer.split(',') : []
      }
      this.otherAnswerValue = this.otherAnswer
    },
  },
})
</script>

<style lang="scss" scoped>
.o-survey-qa {
  .isDetail {
    max-width: calc(100% - 80px);
    & > div:first-child {
      min-width: 56px;
    }
  }
  &__question {
    background-color: #f0f0f0;
    margin: 0;
    font-size: 1.125rem !important;
    font-weight: bold;
    &__desc {
      max-width: 960px;
      padding: 0 !important;
      margin: 0 !important;
      font-weight: bold;
      font-size: 1.125rem !important;
      flex: 1;
      span {
        white-space: pre-wrap;
      }
      &.isEnd {
        max-width: 100%;
      }
    }
  }
  &.is-disclosure {
    .isDetail {
      max-width: 100%;
    }
  }
}
h4 {
  font-size: 1rem;
  padding-left: 16px;
  margin-bottom: 16px;
  position: relative;
  &:before {
    content: '';
    position: absolute;
    left: 0;
    top: 0;
    height: 100%;
    width: 6px;
    background-color: $c-primary-dark;
  }
  &.noboard {
    padding-left: 0;
    &:before {
      content: none;
    }
  }
}
.o-option-group {
  &:nth-child(n + 2) {
    padding-top: 40px;
  }
}
.v-chip--label {
  border-radius: 2px !important;
}
</style>
<style lang="scss">
.a-checkbox--survey {
  .v-label {
    color: rgba(0, 0, 0, 1);
  }
  &.v-input--is-readonly {
    .v-input--selection-controls__input {
      pointer-events: none;
    }
    .v-label {
      color: $c-gray-line-dark;
    }
    .v-icon.mdi-checkbox-blank-outline {
      color: $c-gray-line-dark;
    }
    &.v-input--is-label-active {
      .v-label {
        color: $c-secondary;
        font-weight: bold;
      }
    }
  }
}
.a-radio--survey {
  &.v-input--is-readonly {
    .v-radio {
      .v-input--selection-controls__input {
        pointer-events: none;
      }
      .a-radio__text {
        color: $c-gray-line-dark;
      }
      .v-icon.mdi-radiobox-blank {
        color: $c-gray-line-dark;
      }
      &.v-item--active {
        .a-radio__text {
          color: $c-secondary;
          font-weight: bold;
        }
      }
    }
  }
}
</style>
