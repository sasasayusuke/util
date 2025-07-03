<template>
  <img
    src="@/assets/img/logo-home.png"
    width="323"
    height="84"
    alt="Sony Acceleration Platform Partner Portal Back Office"
  />
</template>

<script lang="ts">
import BaseComponent from '~/common/BaseComponent'
import { Img } from '~/components/common/atoms/index'

export default BaseComponent.extend({
  components: {
    Img,
  },
})
</script>
