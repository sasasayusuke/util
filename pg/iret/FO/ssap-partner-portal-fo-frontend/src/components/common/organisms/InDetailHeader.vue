<template>
  <Sheet
    class="m-heading d-flex justify-space-between align-center px-8"
    height="60"
  >
    <compnent :is="tag" class="m-heading__title">
      <slot name="default" />
      <span class="m-heading__title__term"><slot name="term" /></span>
    </compnent>
    <div class="m-heading__ui d-flex align-center">
      <p v-if="$scopedSlots.date" class="m-heading__date font-size-xsmall">
        <slot name="date" />
      </p>
      <div
        v-if="
          !isProject &&
          !isSolver &&
          !isSolverDetail &&
          !isDetail &&
          !isSolverCorporation &&
          !isSolverApplication
        "
        class="m-heading__button is-karte"
      >
        <slot name="button">
          <Button outlined style-set="small-tertiary" @click="buttonAction2">
            {{ $t('common.button.backToList') }}
          </Button>
        </slot>
      </div>
      <div
        v-if="(isSolverCorporation || isSolverDetail) && !isEditing"
        class="m-heading__button is-karte"
      >
        <slot v-if="isSolverCorporation" name="button">
          <Button outlined style-set="small-tertiary" @click="buttonAction3">
            {{ $t('common.button.solverTop') }}
          </Button>
        </slot>
        <slot v-else-if="isSolverDetail" name="button">
          <Button outlined style-set="small-tertiary" @click="buttonAction2">
            {{ $t('common.button.backToList') }}
          </Button>
        </slot>
      </div>
      <div v-if="isSolverApplication" class="m-heading__button is-karte">
        <slot v-if="isEditing" name="button">
          <Button
            v-if="!isHideButton3"
            outlined
            width="90"
            style-set="small-tertiary"
            class="mr-2"
            @click="buttonAction6"
          >
            {{ $t('common.button.cancel') }}
          </Button>
          <Button
            v-if="isHideButton3"
            outlined
            width="90"
            style-set="small-tertiary"
            class="mr-2"
            @click="buttonAction1"
          >
            {{ $t('common.button.backToList') }}
          </Button>
          <Button
            outlined
            width="90"
            style-set="small-primary"
            class="mr-2"
            :loading="isLoadingButton2"
            :disabled="isDisabledButton3"
            @click="buttonAction2"
          >
            {{ $t('common.button.temporarySave') }}
          </Button>
          <Button
            :disabled="isDisabledButton1"
            :loading="isLoadingButton2"
            style-set="small-primary"
            @click="buttonAction3"
          >
            {{ $t('common.button.toConfirm') }}
          </Button>
        </slot>
        <slot v-if="!isEditing" name="button">
          <Button
            outlined
            style-set="small-tertiary"
            class="mr-2"
            @click="buttonAction4"
          >
            {{ $t('common.button.backToInput') }}
          </Button>
          <Button
            width="90"
            style-set="small-primary"
            class="mr-2"
            :loading="isLoadingButton"
            @click="buttonAction5"
          >
            {{ $t('common.button.apply') }}
          </Button>
        </slot>
      </div>
      <Button
        v-if="!isHideButton2 && isEditing"
        style-set="detailHeaderNegative"
        :disabled="isDisabledButton2"
        @click="buttonAction2"
      >
        {{ $t('common.button.cancel') }}
      </Button>
      <Button
        v-if="isTemporarySave && isEditing"
        outlined
        width="90"
        style-set="small-primary"
        class="ml-2"
        :loading="isLoadingButton2"
        :disabled="isDisabledButton3"
        @click="temporarySaveAction"
      >
        {{ $t('common.button.temporarySave') }}
      </Button>
      <Button
        v-if="!isHideButton1"
        class="ml-2"
        style-set="detailHeaderPositive"
        :disabled="
          isDisabledButton1 ||
          isValid !== true ||
          isUpdating ||
          !isAttendeesValid
        "
        :loading="isLoadingButton"
        @click="buttonAction1"
      >
        {{
          isEditing
            ? isTemporarySave
              ? $t('common.button.apply')
              : $t('common.button.save2')
            : $t('common.button.edit2')
        }}
      </Button>
      <Button
        v-if="isSolver"
        style-set="detailHeaderError"
        outlined
        @click="buttonAction2"
      >
        {{ $t('common.button.delete2') }}
      </Button>
    </div>
  </Sheet>
</template>

<script lang="ts">
import { Button, Card, Sheet, Checkbox, Select } from '../atoms/index'
import BaseComponent from '~/common/BaseComponent'

export default BaseComponent.extend({
  components: {
    Button,
    Card,
    Sheet,
    Checkbox,
    Select,
  },
  data() {
    return {
      draft: this.$t('karte.pages.list.header.select[0]'),
    }
  },
  computed: {
    tag() {
      return `${this.hx}`
    },
  },
  props: {
    hx: {
      type: String,
      default: 'h2',
    },
    isEditing: {
      type: Boolean,
      required: true,
    },
    isKarte: {
      type: Boolean,
    },
    isProject: {
      type: Boolean,
      default: false,
    },
    isSolverApplication: {
      type: Boolean,
      default: false,
    },
    isInvalid: {
      type: Boolean,
      default: false,
    },
    isValid: {
      type: Boolean,
      default: false,
    },
    isUpdating: {
      type: Boolean,
      default: false,
    },
    isAttendeesValid: {
      type: Boolean,
      default: true,
    },
    isHideButton1: {
      type: Boolean,
    },
    isHideButton2: {
      type: Boolean,
    },
    isHideButton3: {
      type: Boolean,
    },
    isDisabledButton1: {
      type: Boolean,
    },
    isDisabledButton2: {
      type: Boolean,
    },
    isDisabledButton3: {
      type: Boolean,
    },
    isLoadingButton: {
      type: Boolean,
      default: false,
    },
    isLoadingButton2: {
      type: Boolean,
      default: false,
    },
    isSolver: {
      type: Boolean,
      default: false,
    },
    isSolverDetail: {
      type: Boolean,
      default: false,
    },
    isSolverCorporation: {
      type: Boolean,
      default: false,
    },
    isDetail: {
      type: Boolean,
      default: false,
    },
    isTemporarySave: {
      type: Boolean,
      default: false,
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
    buttonAction4() {
      this.$emit('buttonAction4')
    },
    buttonAction5() {
      this.$emit('buttonAction5')
    },
    buttonAction6() {
      this.$emit('buttonAction6')
    },
    temporarySaveAction() {
      this.$emit('temporarySaveAction')
    },
  },
})
</script>

<style lang="scss" scoped>
.m-heading {
  border-bottom: 4px solid $c-primary-dark !important;
}
.m-heading__title {
  font-size: 1.25rem;
  font-weight: bold;
}
.m-heading__title__term {
  @include fontSize('normal');
}
.m-heading__date {
  margin: 0;
  color: $c-black-60;
}
</style>

<style lang="scss">
.private .v-select__selections {
  color: #308eef !important;
}
.m-heading__button {
  display: flex;
  align-items: center;
  .a-checkbox {
    margin-top: 0;
    .v-input__slot {
      .v-label {
        @include fontSize($size: 'small');
        color: $c-black;
      }
    }
  }
  .a-select {
    width: 100px;
  }
  // &.is-karte {
  //   display: flex;
  //   align-items: center;
  //   .a-checkbox {
  //     border-left: 1px solid $c-gray-line;
  //     margin-top: 0;
  //     .v-input__slot {
  //       .v-label {
  //         @include fontSize($size: 'small');
  //         color: $c-black;
  //       }
  //     }
  //   }
  //   .a-select {
  //     width: 100px;
  //   }
  // }
}
</style>
