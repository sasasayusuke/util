const environment = process.env.ENV_KEY || 'local'
const envSet = require(`./.env.${environment}.js`)

export default {
  srcDir: './src',

  // Disable server-side rendering: https://go.nuxtjs.dev/ssr-mode
  ssr: false,

  // Target: https://go.nuxtjs.dev/config-target
  target: 'static',

  // Global page headers: https://go.nuxtjs.dev/config-head
  head: {
    titleTemplate: '%s | ' + envSet.APP_TITLE,
    title: envSet.APP_TITLE,
    htmlAttrs: {
      lang: 'ja',
    },
    meta: [
      { charset: 'utf-8' },
      { name: 'viewport', content: 'width=device-width, initial-scale=1' },
      { hid: 'description', name: 'description', content: envSet.APP_TITLE },
      { name: 'format-detection', content: 'telephone=no' },
    ],
    link: [{ rel: 'icon', type: 'image/x-icon', href: '/favicon.ico' }],
  },

  styleResources: {
    scss: [
      '~/assets/sass/_config.scss',
      '~/assets/sass/_mixin.scss',
      '~/assets/sass/_scroll-hint.scss',
    ],
  },
  // Global CSS: https://go.nuxtjs.dev/config-css
  css: ['~/assets/sass/style.scss', '~/assets/icomoon/style.css'],

  // Plugins to run before rendering page: https://go.nuxtjs.dev/config-plugins
  plugins: [
    { src: '~/plugins/amplify-init.ts', ssr: false },
    { src: '~/plugins/logger.ts', ssr: false },
    { src: '~/plugins/api.ts', ssr: false },
    { src: '~/plugins/persistedstate.js', ssr: false },
    { src: '~/plugins/sanitize-html', ssr: false },
    { src: '~/plugins/scroll-hint.ts', ssr: false },
  ],

  // Auto import components: https://go.nuxtjs.dev/config-components
  components: true,

  // Modules for dev and build (recommended): https://go.nuxtjs.dev/config-modules
  buildModules: [
    // https://go.nuxtjs.dev/typescript
    '@nuxtjs/eslint-module',
    '@nuxt/typescript-build',
    '@nuxtjs/vuetify',
    '@nuxtjs/style-resources',
  ],

  // Modules: https://go.nuxtjs.dev/config-modules
  modules: [
    // https://go.nuxtjs.dev/axios
    '@nuxtjs/axios',
    // https://i18n.nuxtjs.org/
    '@nuxtjs/i18n',
  ],

  // vue.config property: https://nuxtjs.org/ja/docs/configuration-glossary/configuration-vue-config/
  vue: {
    config: {
      productionTip: envSet.DEBUG,
      devtools: envSet.DEBUG,
      silent: !envSet.DEBUG,
      performance: envSet.DEBUG,
    },
  },

  // Axios module configuration: https://go.nuxtjs.dev/config-axios
  axios: {},

  // i18n module configuration: https://i18n.nuxtjs.org/basic-usage
  i18n: {
    locales: [
      // { code: 'en', name: 'English', iso: 'en-US', file: 'en.json' },
      { code: 'ja', name: 'Japanese', iso: 'ja_JP', file: 'ja.json' },
    ],
    defaultLocale: 'ja',
    lazy: true,
    langDir: 'lang/',
    vueI18nLoader: true,
  },

  // Vuetify module configuration: https://github.com/nuxt-community/vuetify-module
  vuetify: {
    customVariables: ['~/assets/sass/_vuetify-variables.scss'],
    theme: {
      themes: {
        light: {
          primary: '#1867c0',
          error: '#d53030',
          btn_primary: '#008a19',
          btn_secondary: '#1867c0',
          btn_tertiary: '#666',
          btn_error: '#d53030',
          btn_disabled: '#ccc',
        },
      },
    },
  },

  // Build Configuration: https://go.nuxtjs.dev/config-build
  build: {
    extend(config: any, ctx: any) {
      if (ctx.isDev && ctx.isClient) {
        config.devtool = 'inline-cheap-module-source-map'
      }
    },
  },

  // Router Configuration: https://nuxtjs.org/docs/configuration-glossary/configuration-router/
  router: {
    extendRoutes(routes: any) {
      // Sample page debug only
      if (!envSet.DEBUG) {
        routes.map((route: any) => {
          if (route.path.startsWith('/sample')) {
            route.redirect = '/'
          }
          return route
        })
      }
    },
  },
  env: envSet,
}
