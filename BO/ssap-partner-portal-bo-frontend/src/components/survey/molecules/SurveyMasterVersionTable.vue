<template>
  <DetailContainer
    :title="$t('survey.pages.revision.table.version.title')"
    is-hide-button1
    is-hide-button2
    is-hide-footer
    class="mt-8 pb-10"
    hx="h2"
    :is-editing="false"
  >
    <v-form
      class="o-user-detail-rows no-border pt-4 px-8 pb-0"
      :value="isValid"
      @input="$listeners['input']"
    >
      <!-- バージョン -->
      <CommonDetailRow
        :label="$t('survey.pages.revision.table.version.row.version')"
        :is-editing="false"
      >
        <template #isNotEditing>
          {{ localSurveyMaster.revision }}
          <Chip
            v-if="localSurveyMaster.revision === 0"
            small
            outlined
            style-set="secondary-70"
            class="ml-10"
          >
            {{ $t('common.label.draft') }}
          </Chip>
          <Chip
            v-else-if="localSurveyMaster.isLatest === 1"
            small
            style-set="secondary-70"
            class="ml-10"
          >
            {{ $t('common.label.in-operation') }}
          </Chip>
          <Chip v-else small style-set="tertiary-70" class="ml-10">
            {{ $t('common.label.archive') }}
          </Chip>
        </template>
      </CommonDetailRow>
      <!-- バージョンメモ -->
      <CommonDetailRow
        :label="$t('survey.pages.revision.table.version.row.memo')"
        :value="localSurveyMaster.memo"
        :is-editing="isDraft"
      >
        <Sheet width="100%">
          <Textarea
            v-model="localSurveyMaster.memo"
            style-set="outlined"
            :max-length="500"
            :placeholder="$t('common.placeholder.input')"
            @input="onChange"
          />
        </Sheet>
      </CommonDetailRow>
    </v-form>
  </DetailContainer>
</template>

<script lang="ts">
import CommonDetailRow from '../../common/molecules/CommonDetailRow.vue'
import BaseComponent from '~/common/BaseComponent'
import type { PropType } from '~/common/BaseComponent'
import DetailContainer from '~/components/common/organisms/DetailContainer.vue'
import { Textarea, Sheet, Chip } from '~/components/common/atoms/index'
import { GetSurveyMasterByIdAndRevisionResponse } from '~/models/Master'

export default BaseComponent.extend({
  components: {
    CommonDetailRow,
    DetailContainer,
    Textarea,
    Sheet,
    Chip,
  },
  props: {
    /** アンケートマスター */
    surveyMaster: {
      type: Object as PropType<GetSurveyMasterByIdAndRevisionResponse>,
      required: true,
    },
    /** 下書き状態か */
    isDraft: {
      type: Boolean,
    },
    /** バリデーション状態 */
    isValid: {
      type: Boolean,
    },
  },
  data() {
    return {
      value: '',
      localSurveyMaster: this.surveyMaster,
    }
  },
  methods: {
    /** 他コンポーネントへアンケートひな形詳細の変更を受け渡す */
    onChange(this: any) {
      this.$emit('update', this.localSurveyMaster)
    },
  },
})
</script>
