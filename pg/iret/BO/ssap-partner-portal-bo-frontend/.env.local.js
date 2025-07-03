module.exports = {
  NODE_ENV: 'local',
  VUEX_STORAGE_KEY: 'local_bo_nuxt_app',
  LANDSCAPE: 'dev',
  API_BASE_URL: 'https://bo-api.dev.partner-portal.inhouse-sony-startup-acceleration-program.com/api',
  SSAP_SITE_URL: 'https://portal.major.ssap-dev.com/',
  APP_URL: 'https://bo-app.dev.partner-portal.inhouse-sony-startup-acceleration-program.com',

  APP_TITLE: 'Partner Portal Back Office(local)',

  S3_BUCKET_REGION: 'ap-northeast-1',
  S3_UPLOAD_BUCKET_NAME: 'partnerportal-dev-common-upload',

  // デバッグ用設定
  DEBUG: true,
  CONSOLE_LOGGING: true, // consoleログ出力
  CONSOLE_LOG_LEVEL: 'debug',
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
      redirectSignIn: 'http://localhost:3000/otp',
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
