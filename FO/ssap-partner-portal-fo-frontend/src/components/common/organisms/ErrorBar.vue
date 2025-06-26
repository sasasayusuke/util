<template>
  <Alert
    :value="showsErrorBar"
    style-set="error"
    class="mt-n2 mb-6"
    @input="errorBarChecked"
  >
    <!-- eslint-disable-next-line vue/no-v-html -->
    <span v-html="$sanitize(errorText)"></span>
  </Alert>
</template>

<script lang="ts">
import BaseComponent from '~/common/BaseComponent'
import { Alert } from '~/components/common/atoms/index'

import { errorBarStore } from '~/store'

export default BaseComponent.extend({
  components: {
    Alert,
  },
  computed: {
    errorText(): string {
      return errorBarStore.message
    },
    showsErrorBar(): boolean {
      return !errorBarStore.checked
    },
  },
  mounted() {
    // 下記のような記述でエラーメッセージをセット
    // errorBarStore.setMessage('sample error message')
  },
  methods: {
    errorBarChecked() {
      errorBarStore.setCheckedTrue()
    },
  },
})
</script>
<style>
.v-alert__content span a {
  color: #d53030;
}
</style>
