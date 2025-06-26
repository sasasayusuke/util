<template>
  <Img
    :src="require('~/assets/img/logo-home.png')"
    width="323"
    height="80"
    alt="Sony Acceleration Platform Partner Portal"
    class="mx-auto"
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
