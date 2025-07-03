<template>
  <div class="m-karter-aside-check-box">
    <Title style-set="aside">
      {{ title }}
    </Title>
    <Checkbox
      v-for="(user, index) in users"
      :key="index"
      v-model="ids"
      class="mr-7 mt-2"
      :label="user.name"
      :value="user.id"
      hide-details
      :disabled="!ids.includes(user.id)"
      is-no-editing
    />
  </div>
</template>

<script lang="ts">
import { Title, Checkbox } from '~/components/common/atoms/index'
import BaseComponent from '~/common/BaseComponent'

export default BaseComponent.extend({
  name: 'KarteAsideCheckBox',
  components: {
    Title,
    Checkbox,
  },
  props: {
    /** 「出席支援者 出席お客様メンバー」 */
    title: {
      type: String,
      required: true,
    },
    /** 出席したお客様メンバー */
    users: {
      type: Array,
      required: true,
    },
    ids: {
      type: Array,
      required: true,
    },
  },
})
</script>
