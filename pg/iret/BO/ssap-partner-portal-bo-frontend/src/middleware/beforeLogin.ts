import { Api } from '~/plugins/api'
import { Logger } from '~/plugins/logger'

const $api = new Api() // loggerプラグインを読み込み
const $logger = new Logger() // loggerプラグインを読み込み

export default async function ({ redirect }: { redirect: any }) {
  try {
    await $api.getJWT()
  } catch (error: any) {
    $logger.info(error)
    return redirect('/403')
  }

  return false
}
