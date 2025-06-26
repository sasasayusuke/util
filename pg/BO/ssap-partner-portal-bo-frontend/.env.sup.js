module.exports = {
  NODE_ENV: 'production',
  VUEX_STORAGE_KEY: 'sup_bo_nuxt_app',
  LANDSCAPE: 'sup',
  API_BASE_URL: 'https://bo-app.sup.partner-portal.sony-sap-dev.com/api',
  SSAP_SITE_URL: 'https://portal.major.ssap-dev.com/',
  APP_URL: 'https://bo-app.sup.partner-portal.sony-sap-dev.com',

  APP_TITLE: 'Partner Portal Back Office(sup)',

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
    region: 'ap-northeast-1',
    identityPoolId: 'ap-northeast-1:683e2889-b7d5-4d85-b0dd-978a8fbdfc09',
    userPoolId: 'ap-northeast-1_DXkTGUJJm',
    userPoolWebClientId: '3gosrrs357bt5je73k35vgiiu4',
    oauth: {
      domain: 'dev-bo-partner-portal.auth.ap-northeast-1.amazoncognito.com',
      scope: ['openid'],
      redirectSignIn: 'https://bo-app.sup.partner-portal.sony-sap-dev.com/otp',
      redirectSignOut: 'https://bo-app.sup.partner-portal.sony-sap-dev.com/',
      responseType: 'code',
    },
    cookieStorage: {
      domain: 'bo-app.sup.partner-portal.sony-sap-dev.com',
      path: '/',
      expires: 1,
      secure: false,
    },
  },
}
