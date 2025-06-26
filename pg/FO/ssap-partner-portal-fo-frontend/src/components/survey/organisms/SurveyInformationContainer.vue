<template>
  <Sheet elevation="3" rounded width="1200">
    <InContentHeader v-if="!isSatisfaction">
      {{ title }}
      <template v-if="type !== 'detail' && text" #text>{{ text }}</template>
      <template v-if="type === 'input'" #button>
        <Button
          outlined
          style-set="small-tertiary"
          width="160"
          :to="backToListUrl"
        >
          {{ $t('common.button.cancelSurvey') }}
        </Button>
      </template>
      <template v-else-if="type === 'detail' && !isAnonymous" #button>
        <Button
          outlined
          style-set="small-tertiary"
          width="96"
          :to="backToListUrl"
        >
          {{ $t('common.button.backToList') }}
        </Button>
      </template>
    </InContentHeader>

    <div v-if="$scopedSlots.head" class="px-8 pt-6 ma-0">
      <slot name="head" />
      <h3 class="font-size-large pt-12">
        {{ $t('survey.pages.detail.subSection.basicInfo') }}
      </h3>
    </div>

    <v-expansion-panels v-model="isClose" accordion>
      <v-expansion-panel class="o-expansion-panel">
        <slot />
        <!-- Input開始時はまたは詳細ではInitial Close -->
        <template
          v-if="surveyType !== 'pp' && (type === 'input' || type === 'detail')"
        >
          <v-expansion-panel-header>
            <span v-if="isClose === 0">
              {{ $t('common.button.close') }}
            </span>
            <span v-else>
              {{ $t('common.button.openAll') }}
            </span>
          </v-expansion-panel-header>
        </template>
      </v-expansion-panel>
    </v-expansion-panels>

    <div v-if="$scopedSlots.detail" class="o-survey-detail px-8 pt-0 ma-0">
      <h3
        class="font-size-large"
        :class="surveyType === 'pp' ? 'pt-8' : 'pt-0'"
      >
        {{ $t('survey.pages.detail.subSection.answers') }}
      </h3>
      <slot name="detail" />
    </div>

    <InContentFooter v-if="type !== 'input' && !isAnonymousWithFinished">
      <template v-if="type === 'input-before-start'" #button>
        <div class="d-flex justify-center">
          <Button
            style-set="large-primary"
            width="300"
            @click="$emit('click:start')"
          >
            {{ $t('common.button.startSurvey') }}
          </Button>
        </div>
        <div class="d-flex justify-center mt-8">
          <Button
            outlined
            style-set="large-tertiary"
            width="160"
            :to="backToListUrl"
          >
            {{ $t('common.button.cancel') }}
          </Button>
        </div>
      </template>
      <template v-else-if="type === 'finished'" #button>
        <div class="d-flex justify-center">
          <Button style-set="large-primary" width="300" :to="backToListUrl">
            {{ $t('common.button.backToSurveyList') }}
          </Button>
        </div>
      </template>
      <template v-else-if="type === 'detail' && !isAnonymous" #button>
        <div class="d-flex justify-center">
          <Button
            outlined
            style-set="large-tertiary"
            width="160"
            :to="backToListUrl"
          >
            {{ $t('common.button.backToList') }}
          </Button>
        </div>
      </template>
    </InContentFooter>
  </Sheet>
</template>

<script lang="ts">
import BaseComponent from '~/common/BaseComponent'
import { Sheet, Button } from '~/components/common/atoms/index'
import InContentHeader from '~/components/common/organisms/InContentHeader.vue'
import InContentFooter from '~/components/common/organisms/InContentFooter.vue'

export default BaseComponent.extend({
  components: {
    Sheet,
    Button,
    InContentHeader,
    InContentFooter,
  },
  props: {
    /** アンケートタイプ */
    surveyType: {
      type: String,
      default: '',
    },
    /** 案件ID */
    projectId: {
      type: String,
      default: '',
    },
    type: {
      type: String,
      required: true,
      default: '',
    },
    /** 匿名アンケートか否か */
    isAnonymous: {
      type: Boolean,
      default: false,
    },
    /** 満足度評価のみアンケートか否か */
    isSatisfaction: {
      type: Boolean,
      default: false,
    },
    /** タイトル */
    title: {
      type: String,
      required: true,
      default: '',
    },
    /** テキスト */
    text: {
      type: String,
      default: '',
    },
    /** 編集中の有無 */
    isEditing: {
      type: Boolean,
      default: false,
    },
    token: {
      type: String,
      default: '',
    },
  },
  watch: {
    type(newVal) {
      if (
        newVal === 'input' ||
        newVal === 'detail' ||
        newVal === 'input-before-start'
      ) {
        this.isClose = 0
      }
    },
  },
  data() {
    return {
      isClose: this.type === 'detail' ? 1 : 0,
    }
  },
  computed: {
    /**
     * 匿名アンケートかつ入力済みのアンケートか判定
     */
    isAnonymousWithFinished(): boolean {
      return this.isAnonymous && this.type === 'finished'
    },
    /**
     * 戻るボタンを押した際のURLを条件分岐生成
     * @return URL
     * */
    backToListUrl(): string {
      if (this.isAnonymous) {
        return this.backToUrl(`/anonymous-survey/auth?s=${this.token}`)
      } else if (this.surveyType === 'pp') {
        return this.backToUrl(`/survey/pp/list`)
      } else {
        return this.backToUrl(`/survey/list/${this.projectId}`)
      }
    },
  },
})
</script>

<style lang="scss" scoped>
.o-expansion-panel {
  &:before {
    box-shadow: none;
  }
  button {
    text-align: center;
    margin: 0 auto;
    font-size: 0.875rem;
    color: $c-black-80;
    display: flex;
    flex-flow: row-reverse;
    padding: 0;
    transition: 0.2s;
    &:hover,
    &:focus {
      color: $c-primary-dark;
    }
  }
}
.v-expansion-panel-header {
  width: 152px;
}
h3 {
  border-bottom: 2px solid $c-primary-dark;
  padding-bottom: 6px !important;
  color: $c-primary-dark;
}
.container {
  background: $c-white;
  max-width: 1200px;
}
</style>
<style lang="scss">
.o-expansion-panel {
  .v-expansion-panel-header {
    &:hover,
    &:focus {
      .v-expansion-panel-header__icon {
        .v-icon {
          color: $c-primary-dark;
        }
      }
    }
  }
}
.o-survey-detail {
  background-color: $c-white;
}
</style>
