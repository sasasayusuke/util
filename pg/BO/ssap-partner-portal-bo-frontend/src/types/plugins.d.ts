import { Logger } from '~/plugins/logger'
import { Api } from '~/plugins/api'

// Vueインスタンス用
declare module 'vue/types/vue' {
  interface Vue {
    readonly $logger: Logger
    readonly $api: Api
  }
}

// store用
declare module 'vuex' {
  interface Store<S> {
    readonly $logger: Logger
    readonly $api: Api
  }
}
