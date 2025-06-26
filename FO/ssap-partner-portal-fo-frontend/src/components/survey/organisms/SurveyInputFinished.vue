<template>
  <Sheet class="py-8 px-0 my-0 mx-auto" width="840px">
    <p class="pa-0 font-size-large font-weight-bold" align="center">
      {{ finishText1 }}<br />
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
    /** アンケート回答後締めのテキストその１ */
    finishText1: {
      type: String,
      default: '',
    },
    /** アンケート回答後締めのテキストその２ */
    finishText2: {
      type: String,
      default: '',
    },
  },
  data() {
    return {
      title: 'test',
    }
  },
})
</script>
