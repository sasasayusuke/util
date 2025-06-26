<template>
  <BaseLayout :type="1" />
</template>

<script>
import BaseLayout from './BaseLayout.vue'

export default BaseLayout.extend({
  components: {
    BaseLayout,
  },
  // TODO: statusCodeがないと表示エラーが出たのでdefaultを設定しました。
  // statusCodeを受け取れるようになったらdefaultを必要に応じて修正してください。
  props: {
    error: {
      type: Object,
      default: () => ({ statusCode: 404 }),
    },
  },
  data() {
    return {
      pageNotFound: '404 Not Found',
      otherError: 'An error occurred',
    }
  },
  head() {
    const title =
      this.error.statusCode === 404 ? this.pageNotFound : this.otherError
    return {
      title,
    }
  },
})
</script>

<style lang="scss" scoped>
h1 {
  font-size: 20px;
}
</style>
