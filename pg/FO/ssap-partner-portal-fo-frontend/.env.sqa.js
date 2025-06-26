module.exports = {
  NODE_ENV: 'production',
  VUEX_STORAGE_KEY: 'sqa_nuxt_app',
  LANDSCAPE: 'sqa',
  API_BASE_URL:
    'https://sqa.partner-portal.inhouse-sony-startup-acceleration-program.com/api',
  SSAP_SITE_URL: 'https://portal.major.ssap-dev.com/',
  STARTDASH_SITE_URL: 'https://portal.major.ssap-dev.com/startdash/',
  STARTDASH_OFFICE_SITE_URL: 'https://sqa.rs.startdash-dev.com/',
  APP_URL:
    'https://sqa.partner-portal.inhouse-sony-startup-acceleration-program.com',

  APP_TITLE: 'Partner Portal(sqa)',

  S3_BUCKET_REGION: 'ap-northeast-1',
  S3_UPLOAD_BUCKET_NAME: 'partnerportal-sqa-common-upload',

  // デバッグ用設定
  DEBUG: false,
  CONSOLE_LOGGING: false, // consoleログ出力
  CONSOLE_LOG_LEVEL: 'info',

  /* ================================================== */
  /*  AWS Amplify Auth                                  */
  /* ================================================== */
  // フェデレーション
  AWS_FEDERATION_PROVIDER: 'SSAP-SAML-IdP-dev',
  // 認証情報
  AWS_AMPLIFY_AUTH: {
    identityPoolId: 'ap-northeast-1:fc5f9971-f6eb-4ccb-9e8c-49de2a11b0f3',
    region: 'ap-northeast-1',
    userPoolId: 'ap-northeast-1_xBPEWRyR2',
    userPoolWebClientId: '69c95uptqhtrith2vb172vecmf', // partnerportal-sqa-frontoffice-cognito-client
    oauth: {
      domain: 'dev-fo-partner-portal.auth.ap-northeast-1.amazoncognito.com',
      scope: ['openid'],
      redirectSignIn:
        'https://sqa.partner-portal.inhouse-sony-startup-acceleration-program.com/home',
      redirectSignOut:
        'https://sqa.partner-portal.inhouse-sony-startup-acceleration-program.com/',
      responseType: 'code',
    },
    cookieStorage: {
      domain:
        'sqa.partner-portal.inhouse-sony-startup-acceleration-program.com',
      path: '/',
      expires: 1,
      secure: true,
    },
  },
}
