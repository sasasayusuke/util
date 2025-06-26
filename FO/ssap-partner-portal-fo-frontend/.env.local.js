module.exports = {
  NODE_ENV: 'local',
  VUEX_STORAGE_KEY: 'local_nuxt_app',
  LANDSCAPE: 'dev',
  API_BASE_URL: 'https://api.dev.partner-portal.inhouse-sony-startup-acceleration-program.com/api',
  SSAP_SITE_URL: 'https://portal.major.ssap-dev.com/',
  STARTDASH_SITE_URL: 'https://portal.major.ssap-dev.com/startdash/',
  STARTDASH_OFFICE_SITE_URL: 'https://rs.startdash-dev.com/',
  APP_URL: 'https://dev.partner-portal.inhouse-sony-startup-acceleration-program.com',

  APP_TITLE: 'Partner Portal(local)',

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
      redirectSignIn: 'http://localhost:3000/home',
      redirectSignOut: 'http://localhost:3000/',
      responseType: 'code',
    },
    cookieStorage: {
      domain: 'localhost',
      path: '/',
      expires: 1,
      secure: false,
    },
  },
}
