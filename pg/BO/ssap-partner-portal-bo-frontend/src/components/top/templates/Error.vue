<template>
  <RootTemplate>
    <v-row no-gutters justify="center" class="pt-16 mt-1">
      <v-col cols="auto">
        <ErrorCard :title="title" :description="description" />
      </v-col>
    </v-row>
  </RootTemplate>
</template>

<script lang="ts">
import ErrorCard from '../../top/organisms/ErrorCard.vue'
import BaseComponent from '~/common/BaseComponent'
import RootTemplate from '~/components/common/bases/RootTemplate.vue'

export default BaseComponent.extend({
  components: {
    RootTemplate,
    ErrorCard,
  },
  props: {
    /**
     * タイトル
     */
    title: {
      type: String,
      required: true,
    },
    /**
     * エラー内容
     */
    description: {
      type: String,
      required: true,
    },
  },
})
</script>
