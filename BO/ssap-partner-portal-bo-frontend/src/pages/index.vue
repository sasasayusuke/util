<template>
  <TemplateTop />
</template>

<script lang="ts">
import BasePage from '~/common/BasePage'
import TemplateTop from '~/components/top/templates/Top.vue'

export default BasePage.extend({
  name: 'Home',
  layout: 'BeforeLogin',
  components: {
    TemplateTop,
  },
  data() {
    return {
      img: 'top-bg',
    }
  },
  created() {
    console.log('mode is ' + process.env.NODE_ENV)
  },
})
</script>
