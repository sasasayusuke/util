module.exports = {
  NODE_ENV: 'production',
  VUEX_STORAGE_KEY: 'sqa_bo_nuxt_app',
  LANDSCAPE: 'sqa',
  API_BASE_URL:
    'https://bo-app.sqa.partner-portal.inhouse-sony-startup-acceleration-program.com/api',
  SSAP_SITE_URL: 'https://portal.major.ssap-dev.com/',
  APP_URL:
    'https://bo-app.sqa.partner-portal.inhouse-sony-startup-acceleration-program.com',

  APP_TITLE: 'Partner Portal Back Office(sqa)',

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
    region: 'ap-northeast-1',
    identityPoolId: 'ap-northeast-1:33379678-8a3c-484c-927d-3be3724424d2',
    userPoolId: 'ap-northeast-1_DXkTGUJJm',
    userPoolWebClientId: '3461ueo9aj8t7vrktuh9fnqjt8',
    oauth: {
      domain: 'dev-bo-partner-portal.auth.ap-northeast-1.amazoncognito.com',
      scope: ['openid'],
      redirectSignIn:
        'https://bo-app.sqa.partner-portal.inhouse-sony-startup-acceleration-program.com/otp',
      redirectSignOut:
        'https://bo-app.sqa.partner-portal.inhouse-sony-startup-acceleration-program.com/',
      responseType: 'code',
    },
    cookieStorage: {
      domain:
        'bo-app.sqa.partner-portal.inhouse-sony-startup-acceleration-program.com',
      path: '/',
      expires: 1,
      secure: true,
    },
  },
}
