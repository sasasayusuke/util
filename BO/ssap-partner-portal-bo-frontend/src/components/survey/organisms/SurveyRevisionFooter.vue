<template>
  <div class="m-common-anchor">
    <Sheet v-if="!isDraft" class="d-flex justify-center py-10" width="100%">
      <Button
        style-set="large-tertiary"
        outlined
        width="160"
        @click="$emit('click:back')"
      >
        {{ $t('common.button.back') }}
      </Button>
    </Sheet>
    <template v-else>
      <Sheet
        class="d-flex justify-end pt-6 o-survey-revision-add-button"
        width="100%"
      >
        <Button style-set="add" @click="$emit('click:add')">
          <Icon class="mr-1" size="20" color="btn_primary">mdi-plus</Icon>
          {{ $t('common.button.addQuestion') }}
        </Button>
      </Sheet>
      <Sheet class="d-flex justify-center pt-6" width="100%">
        <Button
          style-set="large-tertiary"
          outlined
          width="160"
          class="mr-4"
          @click="$emit('click:cancel')"
        >
          {{ $t('common.button.cancel') }}
        </Button>
        <Button
          style-set="large-primary"
          width="160"
          :disabled="isValid !== true"
          @click="$emit('click:save')"
        >
          {{ $t('common.button.save') }}
        </Button>
      </Sheet>
    </template>
  </div>
</template>

<script lang="ts">
import { Icon, Button, Sheet } from '~/components/common/atoms/index'
import BaseComponent from '~/common/BaseComponent'

export default BaseComponent.extend({
  components: {
    Icon,
    Button,
    Sheet,
  },
  computed: {},
  props: {
    /** 下書きであるか */
    isDraft: {
      type: Boolean,
      default: false,
    },
    /** バリデーション状態 */
    isValid: {
      type: Boolean,
    },
  },
})
</script>
