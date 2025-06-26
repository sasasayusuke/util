module.exports = {
  NODE_ENV: 'production',
  VUEX_STORAGE_KEY: 'bo_nuxt_app',
  LANDSCAPE: 'prd',
  API_BASE_URL:
    'https://bo-app.partner-portal.sony-startup-acceleration-program.com/api',
  SSAP_SITE_URL: 'https://sony-startup-acceleration-program.com/',
  APP_URL:
    'https://bo-app.partner-portal.sony-startup-acceleration-program.com',

  APP_TITLE: 'Partner Portal Back Office',

  S3_BUCKET_REGION: 'ap-northeast-1',
  S3_UPLOAD_BUCKET_NAME: 'partnerportal-prd-common-upload',

  // デバッグ用設定
  DEBUG: false,
  CONSOLE_LOGGING: false, // consoleログ出力
  CONSOLE_LOG_LEVEL: 'error',

  /* ================================================== */
  /*  AWS Amplify Auth                                  */
  /* ================================================== */
  // フェデレーション
  AWS_FEDERATION_PROVIDER: 'SSAP-SAML-IdP-prod',
  // 認証情報
  AWS_AMPLIFY_AUTH: {
    region: 'ap-northeast-1',
    identityPoolId: 'ap-northeast-1:d70dbfc1-70a5-4394-8c46-4ff4efb66f37',
    userPoolId: 'ap-northeast-1_XhzkFoiSr',
    userPoolWebClientId: '5k198ceqruma5rd2gen3te6gto',
    oauth: {
      domain: 'bo-partner-portal.auth.ap-northeast-1.amazoncognito.com',
      scope: ['openid'],
      redirectSignIn:
        'https://bo-app.partner-portal.sony-startup-acceleration-program.com/otp',
      redirectSignOut:
        'https://bo-app.partner-portal.sony-startup-acceleration-program.com/',
      responseType: 'code',
    },
    cookieStorage: {
      domain: 'bo-app.partner-portal.sony-startup-acceleration-program.com',
      path: '/',
      expires: 1,
      secure: false,
    },
  },
}
