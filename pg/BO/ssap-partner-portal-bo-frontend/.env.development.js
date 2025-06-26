module.exports = {
  NODE_ENV: 'development',
  VUEX_STORAGE_KEY: 'dev_bo_nuxt_app',
  LANDSCAPE: 'dev',
  API_BASE_URL: 'https://bo-app.dev.partner-portal.inhouse-sony-startup-acceleration-program.com/api',
  SSAP_SITE_URL: 'https://portal.major.ssap-dev.com/',
  APP_URL: 'https://bo-app.dev.partner-portal.inhouse-sony-startup-acceleration-program.com',

  APP_TITLE: 'Partner Portal Back Office(dev)',

  S3_BUCKET_REGION: 'ap-northeast-1',
  S3_UPLOAD_BUCKET_NAME: 'partnerportal-dev-common-upload',

  // デバッグ用設定
  DEBUG: true,
  CONSOLE_LOGGING: true, // consoleログ出力
  CONSOLE_LOG_LEVEL: 'info',
  /* ================================================== */
  /*  AWS Amplify Auth                                  */
  /* ================================================== */
  // フェデレーション
  AWS_FEDERATION_PROVIDER: 'SSAP-SAML-IdP-dev',
  // 認証情報
  AWS_AMPLIFY_AUTH: {
    identityPoolId: 'ap-northeast-1:b9fdedbe-f41d-4c2c-acd8-682a7badc2ba',
    region: 'ap-northeast-1',
    identityPoolId: 'ap-northeast-1:b9fdedbe-f41d-4c2c-acd8-682a7badc2ba',
    userPoolId: 'ap-northeast-1_DXkTGUJJm',
    userPoolWebClientId: '5mcqu3bn9bkvn58ojhog9l7amt',
    oauth: {
      domain: 'dev-bo-partner-portal.auth.ap-northeast-1.amazoncognito.com',
      scope: ['openid'],
      redirectSignIn: 'https://bo-app.dev.partner-portal.inhouse-sony-startup-acceleration-program.com/otp',
      redirectSignOut: 'https://bo-app.dev.partner-portal.inhouse-sony-startup-acceleration-program.com/',
      responseType: 'code',
    },
    cookieStorage: {
      domain: 'bo-app.dev.partner-portal.inhouse-sony-startup-acceleration-program.com',
      path: '/',
      expires: 1,
      secure: false,
    },
  },
}
