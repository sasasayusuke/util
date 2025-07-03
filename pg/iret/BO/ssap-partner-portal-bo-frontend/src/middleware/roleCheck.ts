import { Api, UnauthorizedError } from '~/plugins/api'
import { Logger } from '~/plugins/logger'
import { meStore, redirectStore, fullScreenLoadingStore } from '~/store'
import { GetAdminByMineResponse } from '~/models/Admin'
import { signOut } from '~/utils/common-functions'
import { hasRole, getAllowedRoles } from '~/utils/role-authorizer'

const $api = new Api() // loggerプラグインを読み込み
const $logger = new Logger() // loggerプラグインを読み込み

// TODO: modelsのgetAdminByMine関数を使う形に書き換える
async function fetchMe() {
  await $api
    .get<GetAdminByMineResponse>(`/admins/me`)
    .then((res) => {
      $logger.info(res.data)

      // storeに格納
      meStore.setResponse(res.data)
    })
    .catch((error) => {
      throw error
    })
}

export default async function ({
  redirect,
  route,
}: {
  redirect: any
  route: any
}) {
  const allowedRoles = getAllowedRoles(route.path)
  try {
    await fetchMe()
  } catch (error: any) {
    if (error instanceof UnauthorizedError) {
      if (allowedRoles && allowedRoles.length && allowedRoles.length > 0) {
        const encodedRedirectUrl: string = encodeURIComponent(
          `${route.fullPath}`
        )
        // ローディングをホールド
        fullScreenLoadingStore.setHold()
        // セッションが存在する場合はsignOutメソッドで強制的に/以下に遷移するのでリダイレクトURLを保持
        redirectStore.setBufferUrl(encodedRedirectUrl)
      }
      signOut().finally(() => {
        // セッションが完全に存在しない場合は明示的にGet値を付与してリダイレクトする
        if (redirectStore.bufferUrl) {
          window.location.href = `/?redirect_url=${redirectStore.bufferUrl}`
        } else {
          window.location.href = `/`
        }
      })
      return
    }
  }

  if (!allowedRoles) {
    return false
  }

  if (!hasRole(allowedRoles)) {
    return redirect('/403')
  }
}
