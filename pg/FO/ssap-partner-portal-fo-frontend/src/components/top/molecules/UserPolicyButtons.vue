<template>
  <v-container fluid pa-0>
    <v-row no-gutters align="center" justify="center" class="mb-9">
      <v-col cols="auto"
        ><Checkbox
          id="agree"
          v-model="localParam.agreed"
          name="agree"
          class="mt-0 pt-0"
          hide-details
      /></v-col>
      <v-col cols="auto">
        <label for="agree">
          <TextLink to="/terms" target="_blank" style-set="noIcon">{{
            $t('top.pages.user_policy.checkbox.link')
          }}</TextLink>
          {{ $t('top.pages.user_policy.checkbox.text') }}
          <span class="font-size-xsmall m-user-policy__require-text">{{
            $t('common.label.required')
          }}</span>
        </label>
      </v-col>
    </v-row>
    <v-row no-gutters align="center" justify="center">
      <v-col cols="auto">
        <Button
          style-set="xlarge-primary"
          width="400"
          :disabled="!localParam.agreed"
          :loading="isLoading"
          @click="buttonAction"
        >
          {{ $t('common.button.user_policy') }}
        </Button>
      </v-col>
    </v-row>
  </v-container>
</template>

<script lang="ts">
import { Checkbox, Button, TextLink, Required } from '../../common/atoms/index'
import CommonDetailRows from '../../common/molecules/CommonDetailRows.vue'
import type { PropType } from '~/common/BaseComponent'

// LocalUser型はPatchUserByMineRequest型と同じ
import {
  PatchUserByMineRequest,
  PatchUserByMineRequest as LocalUser,
} from '~/models/User'
export { LocalUser }

export default CommonDetailRows.extend({
  components: {
    Checkbox,
    Button,
    TextLink,
    Required,
  },
  props: {
    /**
     * ユーザーの利用規約同意情報
     */
    user: {
      type: Object as PropType<PatchUserByMineRequest>,
      required: true,
    },
    isLoading: {
      type: Boolean,
      required: true,
    },
  },
  data() {
    return {
      localParam: Object.assign(new LocalUser(), this.user),
    }
  },
  methods: {
    buttonAction() {
      this.$emit('click:positive')
    },
  },
})
</script>
<style lang="scss">
.m-user-policy__require-text {
  color: $c-red;
}
</style>
