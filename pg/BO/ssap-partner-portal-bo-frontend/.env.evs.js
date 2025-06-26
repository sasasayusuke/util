module.exports = {
  NODE_ENV: 'production',
  VUEX_STORAGE_KEY: 'evs_bo_nuxt_app',
  LANDSCAPE: 'evs',
  API_BASE_URL: 'https://bo-app.evs.partner-portal.inhouse-sony-startup-acceleration-program.com/api',
  SSAP_SITE_URL: 'https://portal.major.ssap-dev.com/',
  APP_URL: 'https://bo-app.evs.partner-portal.inhouse-sony-startup-acceleration-program.com',

  APP_TITLE: 'Partner Portal Back Office(evs)',

  S3_BUCKET_REGION: 'ap-northeast-1',
  S3_UPLOAD_BUCKET_NAME: 'partnerportal-evs-common-upload',

  // デバッグ用設定
  DEBUG: true,
  CONSOLE_LOGGING: false, // consoleログ出力
  CONSOLE_LOG_LEVEL: 'info',
  /* ================================================== */
  /*  AWS Amplify Auth                                  */
  /* ================================================== */
  // フェデレーション
  AWS_FEDERATION_PROVIDER: 'SSAP-SAML-IdP-dev',
  // 認証情報
  AWS_AMPLIFY_AUTH: {
    region: 'ap-northeast-1',
    identityPoolId: 'ap-northeast-1:a7f70510-8f2b-469f-805e-e0877fe67767',
    userPoolId: 'ap-northeast-1_DXkTGUJJm',
    userPoolWebClientId: '2a2m6lpjdofrnfadfi1ibe2v2p',
    oauth: {
      domain: 'dev-bo-partner-portal.auth.ap-northeast-1.amazoncognito.com',
      scope: ['openid'],
      redirectSignIn: 'https://bo-app.evs.partner-portal.inhouse-sony-startup-acceleration-program.com/otp',
      redirectSignOut: 'https://bo-app.evs.partner-portal.inhouse-sony-startup-acceleration-program.com/',
      responseType: 'code',
    },
    cookieStorage: {
      domain: 'bo-app.evs.partner-portal.inhouse-sony-startup-acceleration-program.com',
      path: '/',
      expires: 1,
      secure: true,
    },
  },
}
