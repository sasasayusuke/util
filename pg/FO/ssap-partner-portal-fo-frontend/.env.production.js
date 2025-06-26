module.exports = {
  NODE_ENV: 'production',
  VUEX_STORAGE_KEY: 'nuxt_app',
  LANDSCAPE: 'prd',
  API_BASE_URL:
    'https://partner-portal.sony-startup-acceleration-program.com/api',
  SSAP_SITE_URL: 'https://sony-startup-acceleration-program.com/',
  STARTDASH_SITE_URL:
    'https://sony-startup-acceleration-program.com/startdash/',
  STARTDASH_OFFICE_SITE_URL:
    'https://office.startdash.sony-startup-acceleration-program.com/',
  APP_URL: 'https://partner-portal.sony-startup-acceleration-program.com',

  APP_TITLE: 'Partner Portal',

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
    identityPoolId: 'ap-northeast-1:90eee30f-14e7-4a2a-b3dc-1d17ec38a910',
    userPoolId: 'ap-northeast-1_kp3VmUNvJ',
    userPoolWebClientId: 'h732casi9vm9sop0pii76a1oq', // partnerportal-prd-frontoffice-cognito-client
    oauth: {
      domain: 'fo-partner-portal.auth.ap-northeast-1.amazoncognito.com',
      scope: ['openid'],
      redirectSignIn:
        'https://partner-portal.sony-startup-acceleration-program.com/home',
      redirectSignOut:
        'https://partner-portal.sony-startup-acceleration-program.com/',
      responseType: 'code',
    },
    cookieStorage: {
      domain: 'partner-portal.sony-startup-acceleration-program.com',
      path: '/',
      expires: 1,
      secure: true,
    },
  },
}
