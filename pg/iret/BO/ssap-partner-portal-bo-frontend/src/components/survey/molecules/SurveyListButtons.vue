<template>
  <GroupButton class="mb-10">
    <Button
      style-set="group-xlarge"
      :outlined="selected !== 'service'"
      width="25%"
      @click="type('service')"
    >
      {{ $t('survey.pages.list.service.name') }}
    </Button>
    <Button
      style-set="group-xlarge"
      :outlined="selected !== 'completion'"
      width="25%"
      @click="type('completion')"
    >
      {{ $t('survey.pages.list.completion.name') }}
    </Button>
    <Button
      style-set="group-xlarge"
      :outlined="selected !== 'service_and_completion'"
      width="25%"
      @click="type('service_and_completion')"
    >
      <span class="is-pre-wrap">{{
        $t('survey.pages.list.serviceAndcompletion.name')
      }}</span>
    </Button>
    <Button
      style-set="group-xlarge"
      :outlined="selected !== 'quick'"
      width="25%"
      @click="type('quick')"
    >
      {{ $t('survey.pages.list.quick.name') }}
    </Button>
  </GroupButton>
</template>

<script lang="ts">
import BaseComponent from '~/common/BaseComponent'
import { Button } from '~/components/common/atoms'
import GroupButton from '~/components/common/molecules/GroupButton.vue'

export default BaseComponent.extend({
  components: {
    Button,
    GroupButton,
  },
  props: {
    /** 選択中のアンケート種別 */
    selected: {
      type: String,
      default: 'service',
    },
  },
  data() {
    return {}
  },
  methods: {
    /**
     * アンケート種別の変更
     * @param type アンケート種別
     */
    type(type: string) {
      this.$emit('type', type)
    },
  },
})
</script>
