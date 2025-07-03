<template>
  <Sheet class="py-10 px-0 my-0 mx-auto" width="840px">
    <p class="pa-0 mb-0 font-size-xlarge font-weight-bold" align="center">
      {{ finishText1 }}
    </p>
    <p class="pa-0 mb-0 mt-4" align="center">
      {{ finishText2 }}
    </p>
  </Sheet>
</template>

<script lang="ts">
import BaseComponent from '~/common/BaseComponent'
import { Sheet } from '~/components/common/atoms/index'

export default BaseComponent.extend({
  components: {
    Sheet,
  },
  props: {
    /** 申請完了後締めのテキストその１ */
    finishText1: {
      type: String,
      default: '',
    },
    /** 申請完了後締めのテキストその２ */
    finishText2: {
      type: String,
      default: '',
    },
  },
})
</script>
