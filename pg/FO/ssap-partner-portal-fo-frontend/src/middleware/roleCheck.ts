import {
  Api,
  UnauthorizedError,
  ForbiddenError,
  InternalServerError,
} from '~/plugins/api'
import { Logger } from '~/plugins/logger'
import {
  meStore,
  redirectStore,
  fullScreenLoadingStore,
  persistedErrorBarStore,
} from '~/store'
import { GetUserByMineResponse } from '~/models/User'
import { hasRole, getAllowedRoles } from '~/utils/role-authorizer'
import { signOut } from '~/utils/common-functions'
import { AuthLogin } from '~/models/Auth'

const $api = new Api() // loggerプラグインを読み込み
const $logger = new Logger() // loggerプラグインを読み込み

async function fetchMe() {
  await $api
    .get<GetUserByMineResponse>(`/users/me`)
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
  i18n,
}: {
  redirect: any
  route: any
  i18n: any
}) {
  const allowedRoles = getAllowedRoles(route.path)
  try {
    await fetchMe()
    await AuthLogin()
  } catch (error: any) {
    // システムエラー時 (リダイレクトは無効とする)
    if (error instanceof InternalServerError) {
      $logger.warn('sign out(system error)')
      // ローディングをホールド
      fullScreenLoadingStore.setHold()
      signOut().finally(() => {
        window.location.href = `/`
      })
      // トップページで表示するエラー内容をセット
      persistedErrorBarStore.setMessage(
        i18n.t('top.pages.index.errorMessages.systemError')
      )
      return
    }

    // ユーザー無効時またはPartnerPortalアカウントが存在しない (リダイレクトは無効とする)
    if (error instanceof ForbiddenError) {
      $logger.warn('sign out(invalid user)')
      // ローディングをホールド
      fullScreenLoadingStore.setHold()
      signOut().finally(() => {
        window.location.href = `/`
      })
      // トップページで表示するエラー内容をセット
      persistedErrorBarStore.setMessage(
        i18n.t('top.pages.index.errorMessages.userNotFound')
      )
      return
    }

    // ログインされていないorタイムアウト時 (リダイレクトは有効とする)
    if (error instanceof UnauthorizedError) {
      $logger.warn('sign out(Unauthorized)')
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

    throw error
  }

  if (!meStore.agreed) {
    if (!(route.path === '/user-policy')) {
      return redirect('/user-policy')
    }
  }

  if (!allowedRoles) {
    return false
  }

  if (!hasRole(allowedRoles)) {
    return redirect('/403')
  }
}
