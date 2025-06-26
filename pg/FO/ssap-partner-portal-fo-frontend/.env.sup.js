module.exports = {
  NODE_ENV: 'production',
  VUEX_STORAGE_KEY: 'sup_nuxt_app',
  LANDSCAPE: 'sup',
  API_BASE_URL: 'https://sup.partner-portal.sony-sap-dev.com/api',
  SSAP_SITE_URL: 'https://portal.major.ssap-dev.com/',
  STARTDASH_SITE_URL: 'https://portal.major.ssap-dev.com/startdash/',
  STARTDASH_OFFICE_SITE_URL: 'https://sup.rs.startdash-dev.com/',
  APP_URL: 'https://sup.partner-portal.sony-sap-dev.com',

  APP_TITLE: 'Partner Portal(sup)',

  S3_BUCKET_REGION: 'ap-northeast-1',
  S3_UPLOAD_BUCKET_NAME: 'partnerportal-sup-common-upload',

  // デバッグ用設定
  DEBUG: false,
  CONSOLE_LOGGING: true, // consoleログ出力
  CONSOLE_LOG_LEVEL: 'info',

  /* ================================================== */
  /*  AWS Amplify Auth                                  */
  /* ================================================== */
  // フェデレーション
  AWS_FEDERATION_PROVIDER: 'SSAP-SAML-IdP-dev',
  // 認証情報
  AWS_AMPLIFY_AUTH: {
    identityPoolId: 'ap-northeast-1:32bf55bd-9cb3-4718-87eb-3a8c73001cb1',
    region: 'ap-northeast-1',
    userPoolId: 'ap-northeast-1_xBPEWRyR2',
    userPoolWebClientId: '2lkmv8gkis1qjmonb69k884gjj', // partnerportal-sup-frontoffice-cognito-client
    oauth: {
      domain: 'dev-fo-partner-portal.auth.ap-northeast-1.amazoncognito.com',
      scope: ['openid'],
      redirectSignIn: 'https://sup.partner-portal.sony-sap-dev.com/home',
      redirectSignOut: 'https://sup.partner-portal.sony-sap-dev.com/',
      responseType: 'code',
    },
    cookieStorage: {
      domain: 'sup.partner-portal.sony-sap-dev.com',
      path: '/',
      expires: 1,
      secure: false,
    },
  },
}
