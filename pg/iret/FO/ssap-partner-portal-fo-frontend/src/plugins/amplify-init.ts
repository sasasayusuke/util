import { Auth } from 'aws-amplify'

/**
 * Amplify.jsの初期化
 */
export default () => {
  const config = process.env.AWS_AMPLIFY_AUTH
  if (config != null) {
    Auth.configure(config)
  }
}
