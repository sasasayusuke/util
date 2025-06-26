<template>
  <GroupButton class="mb-10">
    <Button
      style-set="group-xlarge"
      :outlined="!(surveyType === 'service')"
      width="25%"
      @click="$emit('click', 'service')"
    >
      {{ $t('survey.pages.admin.list.service.name') }}
    </Button>
    <Button
      style-set="group-xlarge"
      :outlined="!(surveyType === 'completion')"
      width="25%"
      @click="$emit('click', 'completion')"
    >
      {{ $t('survey.pages.admin.list.completion.name') }}
    </Button>
    <Button
      style-set="group-xlarge"
      :outlined="!(surveyType === 'service_and_completion')"
      width="25%"
      @click="$emit('click', 'service_and_completion')"
    >
      <span class="is-pre-wrap">{{
        $t('survey.pages.admin.list.serviceAndcompletion.name')
      }}</span>
    </Button>
    <Button
      style-set="group-xlarge"
      :outlined="!(surveyType === 'quick')"
      width="25%"
      @click="$emit('click', 'quick')"
    >
      {{ $t('survey.pages.admin.list.quick.name') }}
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
    /** アンケートタイプ */
    surveyType: {
      type: String,
      required: true,
    },
  },
})
</script>
