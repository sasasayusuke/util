module.exports = {
  NODE_ENV: 'local',
  VUEX_STORAGE_KEY: 'nuxt_app',
  LANDSCAPE: 'dev',
  API_BASE_URL: 'https://dev.partner-portal.inhouse-sony-startup-acceleration-program.com/api',
  SSAP_SITE_URL: 'https://portal.major.ssap-dev.com/',
  STARTDASH_SITE_URL: 'https://portal.major.ssap-dev.com/startdash/',
  STARTDASH_OFFICE_SITE_URL: 'https://rs.startdash-dev.com/',
  APP_URL: 'https://nuxt.partner-portal.inhouse-sony-startup-acceleration-program.com',

  APP_TITLE: 'Partner Portal(nuxt)',

  S3_BUCKET_REGION: 'ap-northeast-1',
  S3_UPLOAD_BUCKET_NAME: 'partnerportal-dev-common-upload',

  // デバッグ用設定
  DEBUG: true,
  CONSOLE_LOGGING: true, // consoleログ出力
  CONSOLE_LOG_LEVEL: 'debug',
  STORAGE_CURRENT_DATE_KEY: 'local_ssappp_current_time',

  /* ================================================== */
  /*  AWS Amplify Auth                                  */
  /* ================================================== */
  // フェデレーション
  AWS_FEDERATION_PROVIDER: 'SSAP-SAML-IdP-dev',
  // 認証情報
  AWS_AMPLIFY_AUTH: {
    region: 'ap-northeast-1',
    identityPoolId: 'ap-northeast-1:29154dda-3aa7-4f73-ad7c-2407d4d53503',
    userPoolId: 'ap-northeast-1_xBPEWRyR2',
    userPoolWebClientId: '7lvo58vfbun3duahpauql8srmj', // partnerportal-dev-frontoffice-cognito-client
    oauth: {
      domain: 'dev-fo-partner-portal.auth.ap-northeast-1.amazoncognito.com',
      scope: ['openid'],
      redirectSignIn: 'https://nuxt.partner-portal.inhouse-sony-startup-acceleration-program.com/home',
      redirectSignOut: 'https://nuxt.partner-portal.inhouse-sony-startup-acceleration-program.com/',
      responseType: 'code',
    },
    cookieStorage: {
      domain: 'nuxt.partner-portal.inhouse-sony-startup-acceleration-program.com',
      path: '/',
      expires: 1,
      secure: false,
    },
  },
}
