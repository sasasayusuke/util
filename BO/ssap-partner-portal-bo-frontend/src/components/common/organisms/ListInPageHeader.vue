<template>
  <CommonInPageHeader class="o-ListInPageHeader">
    <slot name="default" />
    <span
      v-if="noteHead === 'required'"
      class="ml-10 m-heading__title__required"
      ><Required /><span class="m-heading__title__required__text">{{
        $t('common.label.required2')
      }}</span></span
    >
    <!-- latestDateが与えられていれば、日時を作成する -->
    <template v-if="latestDate" #date>
      <v-row>
        <v-col class="m-last-update__title">
          {{ $t('common.lastUpdate.total') }}
        </v-col>
        <v-col class="m-last-update__data" cols="auto">
          {{ formatDate(new Date(latestDate), 'Y/MM/DD hh:mm') }}
        </v-col>
      </v-row>
    </template>
    <!-- createPageRouteが与えられていれば、新規作成ボタンを作成する -->
    <template v-if="createPageRoute" #button>
      <Button style-set="small-primary" width="96" :to="createPageRoute">
        {{ $t('common.button.createNew') }}
      </Button>
    </template>
    <!-- csvOutputが与えられていれば、csv出力ボタンを作成する -->
    <template v-if="csvOutput" #button-csv>
      <Button
        style-set="small-primary"
        width="96"
        outlined
        :disabled="csvButtonDisabled"
        @click="buttonAction0"
      >
        {{ $t('common.button.csv') }}
      </Button>
    </template>
    <!-- backToListが与えられていれば、一覧へ戻るボタンを作成する -->
    <template v-if="backToList" #button-back>
      <Button style-set="small-tertiary" width="96" outlined :to="backToList">
        {{ $t('common.button.backToList') }}
      </Button>
    </template>
    <template v-if="monthChange" #button-month>
      <Button
        outlined
        style-set="large-tertiary"
        width="96"
        class="mr-8"
        @click="buttonAction1"
      >
        {{ $t('common.button.thisMonth') }}
      </Button>
      <Button
        style-set="large-primary"
        class="px-2"
        width="auto"
        @click="buttonAction2"
      >
        <span class="d-flex justify-space-between">
          <Icon class="mr-2" size="14" color="white">icon-org-arrow-left</Icon>
          {{ $t('common.button.lastMonth') }}
          <Icon class="ml-2" size="14" color="transparent"
            >icon-org-arrow-right</Icon
          >
        </span>
      </Button>
      <Button
        style-set="large-primary"
        class="px-2"
        width="auto"
        :disabled="nextMonthDisabled"
        @click="buttonAction3"
      >
        <span class="d-flex justify-space-between">
          <Icon class="mr-2" size="14" color="transparent"
            >icon-org-arrow-left</Icon
          >
          {{ $t('common.button.nextMonth') }}
          <Icon class="ml-2" size="14" color="white">icon-org-arrow-right</Icon>
        </span>
      </Button>
    </template>
  </CommonInPageHeader>
</template>

<script lang="ts">
import { Button, Icon, Required } from '../atoms/index'
import CommonInPageHeader from './CommonInPageHeader.vue'
import BaseComponent from '~/common/BaseComponent'

export default BaseComponent.extend({
  components: {
    Button,
    Icon,
    Required,
    CommonInPageHeader,
  },
  props: {
    createPageRoute: {
      type: [String, Object],
      default: '',
    },
    csvOutput: {
      type: Boolean,
    },
    backToList: {
      type: [Boolean, String],
      default: '',
    },
    monthChange: {
      type: Boolean,
    },
    latestDate: {
      type: [Boolean, String],
      default: '',
    },
    noteHead: {
      type: String,
      default: '',
    },
    csvButtonDisabled: {
      type: Boolean,
      default: false,
    },
    nextMonthDisabled: {
      type: Boolean,
      default: false,
    },
  },
  methods: {
    buttonAction0() {
      this.$emit('buttonAction0')
    },
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

<style lang="scss" scoped>
.m-heading__title__required {
  @include fontSize('xsmall');
  font-weight: normal;
}
.m-heading__title__required__text {
  color: $c-black-60;
}
</style>
<style lang="scss">
.o-ListInPageHeader {
  .a-button.theme--light.v-btn.v-btn--disabled .v-icon {
    color: $c-white !important;
  }
}
</style>
