<template>
  <DetailContainer
    :title="title"
    :is-editing="isEditing"
    :is-survey="isSurvey"
    :is-valid="isValidWithChange"
    is-hide-footer
    :is-hide-button1="isHideButton1"
    :is-loading-button="isLoadingButton"
    @click:positive="$emit('click:positive', localSurveyMaster)"
    @click:negative="$emit('click:negative')"
  >
    <SurveyMasterDetailRows
      v-model="isValid"
      :survey-master="surveyMaster"
      :is-editing="isEditing"
      :is-creating="isCreating"
      :is-loading="isLoading"
      @update="update('localSurveyMaster', $event)"
    />
  </DetailContainer>
</template>

<script lang="ts">
import { PropType } from '~/common/BaseComponent'
import SurveyMasterDetailRows, {
  LocalSurveyMaster,
} from '~/components/survey/molecules/SurveyMasterDetailRows.vue'
import CommonDetailContainer from '~/components/common/organisms/CommonDetailContainer.vue'
import DetailContainer from '~/components/common/organisms/DetailContainer.vue'
import { SurveyMasterListItem } from '~/models/Master'
import { meStore } from '~/store'

export { LocalSurveyMaster }

export default CommonDetailContainer.extend({
  components: {
    SurveyMasterDetailRows,
    DetailContainer,
  },
  props: {
    /** 表示タイトル */
    title: {
      type: String,
    },
    /** アンケートマスター */
    surveyMaster: {
      type: Object as PropType<SurveyMasterListItem>,
    },
    /** アンケートマスター登録/更新中か */
    isLoadingButton: {
      type: Boolean,
      default: false,
    },
  },
  data() {
    return {
      isSurvey: true,
      localSurveyMaster: new LocalSurveyMaster(),
    }
  },
  computed: {
    /**
     * システム管理者またはアンケート事務局の権限を所持する場合は変更ボタンを表示
     * @returns 判定真偽値
     */
    isHideButton1(): Boolean {
      for (const i in meStore.roles) {
        if (
          meStore.roles[i] === 'system_admin' ||
          meStore.roles[i] === 'survey_ops'
        ) {
          return false
        }
      }
      return true
    },
  },
})
</script>
