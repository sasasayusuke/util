/**
 * Mixin
 * TODO: typescriptだとmixinに定義したプロパティを参照できなくてエラーが出るのでなんとかしたい
 * https://nuxtjs.org/ja/docs/directory-structure/plugins/#%E3%82%B0%E3%83%AD%E3%83%BC%E3%83%90%E3%83%AB%E3%83%9F%E3%83%83%E3%82%AF%E3%82%B9%E3%82%A4%E3%83%B3
 */
import BaseVueClass from '~/common/BaseVueClass'

interface MixinData {
  foo: string
}

export const Mixin = BaseVueClass.extend({
  data(): MixinData {
    return {
      foo: 'Mixin in Nuxt.js',
    }
  },
  methods: {
    // sample
    hello(bar: String): void {
      this.$logger.info(`Hello dear ${bar}`)
    },
  },
})
