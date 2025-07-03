<template>
  <v-container v-if="$scopedSlots.footerButton" :class="className">
    <v-row class="pa-0 ma-0" justify="center">
      <v-col v-if="isProject" cols="auto">
        <Button
          v-if="isEditing"
          style-set="large-tertiary"
          outlined
          width="160"
          :disabled="isDisabledButton2"
          @click="buttonAction2"
        >
          {{ $t('common.button.cancel') }}
        </Button>
        <Button
          style-set="large-primary"
          width="160"
          class="ml-4"
          :disabled="isDisabledButton1 || isValid !== true"
          :loading="isLoadingButton"
          @click="buttonAction1"
        >
          {{
            isEditing ? $t('common.button.save2') : $t('common.button.edit2')
          }}
        </Button>
      </v-col>
      <v-col v-else-if="isSolverCorporation" cols="auto">
        <Button
          v-if="!isEditing"
          outlined
          width="160"
          style-set="large-tertiary"
          @click="buttonAction3"
        >
          {{ $t('common.button.solverTop') }}
        </Button>
        <Button
          v-if="isEditing"
          style-set="large-tertiary"
          outlined
          width="160"
          :disabled="isDisabledButton2"
          @click="buttonAction2"
        >
          {{ $t('common.button.cancel') }}
        </Button>
        <Button
          style-set="large-primary"
          width="160"
          class="ml-4"
          :disabled="isDisabledButton1 || isValid !== true"
          :loading="isLoadingButton"
          @click="buttonAction1"
        >
          {{
            isEditing ? $t('common.button.save2') : $t('common.button.edit2')
          }}
        </Button>
      </v-col>
      <v-col v-else cols="auto">
        <slot name="footerButton" />
      </v-col>
    </v-row>
  </v-container>
  <v-container v-else :class="className">
    <template v-if="isKarte">
      <v-row
        v-if="isEditing"
        class="pa-0 ma-0 is-karte-footer"
        justify="center"
      >
        <Button outlined style-set="large-tertiary" @click="buttonAction2">
          {{ $t('common.button.backToList') }}
        </Button>
        <Button
          style-set="large-primary"
          class="ml-2"
          :disabled="isDisabledButton1 || isValid !== true"
          @click="buttonAction1"
        >
          {{ $t('common.button.save2') }}
        </Button>
      </v-row>
      <v-row v-else class="pa-0 ma-0 is-karte-footer" justify="center">
        <Sheet class="ml-8">
          <Button outlined style-set="large-tertiary" @click="buttonAction2">
            {{ $t('common.button.back') }}
          </Button>
        </Sheet>
      </v-row>
    </template>
    <v-row
      v-else-if="!isManHour || !isProject"
      class="pa-0 ma-0"
      justify="center"
    >
      <v-col v-if="isManHour" cols="auto">
        <Button
          outlined
          style-set="large-tertiary"
          width="160"
          :disabled="isDisabledButton2"
          @click="buttonAction2"
        >
          {{ $t('common.button.backToList') }}
        </Button>
      </v-col>
    </v-row>
  </v-container>
</template>

<script lang="ts">
import { Button, Select, Checkbox } from '../atoms/index'
import BaseComponent from '~/common/BaseComponent'

export default BaseComponent.extend({
  components: {
    Button,
    Select,
    Checkbox,
  },
  data() {
    return {
      draft: this.$t('karte.pages.list.header.select[0]'),
    }
  },
  props: {
    isEditing: {
      type: Boolean,
      required: true,
    },
    isKarte: {
      type: Boolean,
    },
    isManHour: {
      type: Boolean,
      default: false,
    },
    isProject: {
      type: Boolean,
      default: false,
    },
    isSolverCorporation: {
      type: Boolean,
      default: false,
    },
    isValid: {
      type: Boolean,
      default: false,
    },
    isHideButton1: {
      type: Boolean,
    },
    isHideButton2: {
      type: Boolean,
    },
    isDisabledButton1: {
      type: Boolean,
    },
    isDisabledButton2: {
      type: Boolean,
    },
    isLoadingButton: {
      type: Boolean,
      default: false,
    },
  },
  computed: {
    className() {
      return this.isManHour ? 'py-6' : 'py-10'
    },
  },
  methods: {
    buttonAction1() {
      this.$emit('buttonAction1')
    },
    buttonAction2() {
      this.$emit('buttonAction2')
    },
    buttonAction3() {
      this.$emit('buttonAction3')
    },
  },
})
</script>

<style lang="scss">
.is-karte-footer {
  align-items: center;
  .a-checkbox {
    border-left: 1px solid $c-gray-line;
    margin-top: 0;
    padding: 10px 0;
    .v-input__slot {
      .v-label {
        @include fontSize($size: 'small');
        color: $c-black;
      }
    }
  }
  .a-select {
    width: 100px;
    flex: 0 1 auto;
  }
  .a-button {
    width: 160px;
  }
}
</style>

<style lang="scss" scoped></style>
