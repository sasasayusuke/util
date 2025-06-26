module.exports = {
  NODE_ENV: 'production',
  VUEX_STORAGE_KEY: 'evs_nuxt_app',
  LANDSCAPE: 'evs',
  API_BASE_URL: 'https://evs.partner-portal.inhouse-sony-startup-acceleration-program.com/api',
  SSAP_SITE_URL: 'https://portal.major.ssap-dev.com/',
  STARTDASH_SITE_URL: 'https://portal.major.ssap-dev.com/startdash/',
  STARTDASH_OFFICE_SITE_URL: 'https://evs.rs.startdash-dev.com/',
  APP_URL: 'https://evs.partner-portal.inhouse-sony-startup-acceleration-program.com',

  APP_TITLE: 'Partner Portal(evs)',

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
    identityPoolId: 'ap-northeast-1:796a104d-d33c-4f62-aaf6-3fdfcdb79dae',
    userPoolId: 'ap-northeast-1_xBPEWRyR2', // FO
    userPoolWebClientId: 'nct4qj8slarufqtpsteer3g9r', // partnerportal-evs-frontoffice-cognito-client
    oauth: {
      domain: 'dev-fo-partner-portal.auth.ap-northeast-1.amazoncognito.com', // FO
      scope: ['openid'],
      redirectSignIn: 'https://evs.partner-portal.inhouse-sony-startup-acceleration-program.com/home',
      redirectSignOut: 'https://evs.partner-portal.inhouse-sony-startup-acceleration-program.com/',
      responseType: 'code',
    },
    cookieStorage: {
      domain: 'evs.partner-portal.inhouse-sony-startup-acceleration-program.com',
      path: '/',
      expires: 1,
      secure: true,
    },
  },
}
