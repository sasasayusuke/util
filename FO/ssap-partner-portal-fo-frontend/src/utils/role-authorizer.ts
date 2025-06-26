import { meStore } from '~/store'
import { Logger } from '~/plugins/logger'
import type { UserRole as UserRoleStr } from '@/types/User'
import { ENUM_USER_ROLE } from '@/types/User'

export type { UserRoleStr }
export { ENUM_USER_ROLE as USER_ROLE }

const $logger = new Logger() // loggerプラグインを読み込み

export class UserRole extends String {
  // 引数として渡された権限のうち、一つでも持っているかどうかを返す
  hasRole(roles: UserRoleStr[] = []): boolean {
    return roles.includes(this.valueOf() as UserRoleStr)
  }
}

export function hasRole(roles: UserRoleStr[]) {
  const userRole = new UserRole(meStore.role)
  return userRole.hasRole(roles)
}

export const USER_ROLES_SET: { [key: string]: UserRoleStr[] } = {
  all: [
    ENUM_USER_ROLE.CUSTOMER,
    ENUM_USER_ROLE.SUPPORTER,
    ENUM_USER_ROLE.SALES,
    ENUM_USER_ROLE.SUPPORTER_MGR,
    ENUM_USER_ROLE.SALES_MGR,
    ENUM_USER_ROLE.BUSINESS_MGR,
  ],
  notCustomer: [
    ENUM_USER_ROLE.SUPPORTER,
    ENUM_USER_ROLE.SALES,
    ENUM_USER_ROLE.SUPPORTER_MGR,
    ENUM_USER_ROLE.SALES_MGR,
    ENUM_USER_ROLE.BUSINESS_MGR,
  ],
  supporters: [ENUM_USER_ROLE.SUPPORTER, ENUM_USER_ROLE.SUPPORTER_MGR],
  supportersAndSales: [
    ENUM_USER_ROLE.SUPPORTER,
    ENUM_USER_ROLE.SUPPORTER_MGR,
    ENUM_USER_ROLE.SALES,
    ENUM_USER_ROLE.SALES_MGR,
    ENUM_USER_ROLE.BUSINESS_MGR,
  ],
  supportersAndBusinessMgr: [
    ENUM_USER_ROLE.SUPPORTER,
    ENUM_USER_ROLE.SUPPORTER_MGR,
    ENUM_USER_ROLE.BUSINESS_MGR,
  ],
  managers: [ENUM_USER_ROLE.SUPPORTER_MGR, ENUM_USER_ROLE.SALES_MGR],
  solver: [ENUM_USER_ROLE.APT, ENUM_USER_ROLE.SOLVER_STAFF],
  allIncludingSolver: [
    ENUM_USER_ROLE.CUSTOMER,
    ENUM_USER_ROLE.SUPPORTER,
    ENUM_USER_ROLE.SALES,
    ENUM_USER_ROLE.SUPPORTER_MGR,
    ENUM_USER_ROLE.SALES_MGR,
    ENUM_USER_ROLE.BUSINESS_MGR,
    ENUM_USER_ROLE.APT,
    ENUM_USER_ROLE.SOLVER_STAFF,
  ],
}

export const allowedRolesDefine: {
  [key: string]: UserRoleStr[]
} = {
  '/home': USER_ROLES_SET.allIncludingSolver,
  '/project/list': USER_ROLES_SET.all,
  '/project/_projectId': USER_ROLES_SET.notCustomer,
  '/project/list/me': USER_ROLES_SET.supportersAndSales,
  '/karte/list/_projectId': USER_ROLES_SET.all,
  '/karte/create': USER_ROLES_SET.supportersAndBusinessMgr,
  '/karte/_karteId': USER_ROLES_SET.all,
  '/man-hour': USER_ROLES_SET.supporters,
  '/man-hour/_year/_month': USER_ROLES_SET.supporters,
  '/customer/list': USER_ROLES_SET.notCustomer,
  '/customer/_customerId': USER_ROLES_SET.notCustomer,
  '/survey/admin/list': USER_ROLES_SET.notCustomer,
  '/survey/pp/list': USER_ROLES_SET.notCustomer,
  '/survey/pp/_surveyId': USER_ROLES_SET.notCustomer,
  '/survey/list/_projectId': USER_ROLES_SET.all,
  '/survey/_surveyId': USER_ROLES_SET.all,
  '/user-policy': USER_ROLES_SET.allIncludingSolver,
  '/anonymous-survey/auth': USER_ROLES_SET.all,
  '/anonymous-survey/_surveyId': USER_ROLES_SET.all,
  '/satisfaction-survey/_surveyId': USER_ROLES_SET.all,
  '/master-karte/_npfProjectId': USER_ROLES_SET.all,
  '/solver/utilization-rate/_solveCorporationId': USER_ROLES_SET.solver,
  '/solver/candidate/application': USER_ROLES_SET.solver,
  '/solver/menu': USER_ROLES_SET.solver,
  '/solver/list/_solverCorporationId': USER_ROLES_SET.solver,
  '/solver/candidate/list/_solverCorporationId': USER_ROLES_SET.solver,
  '/solver/corporation/_solver_corporation_id': USER_ROLES_SET.solver,
  '/solver/candidate/_solverId': USER_ROLES_SET.solver,
  '/solver/application': USER_ROLES_SET.solver,
  '/solver/_solverId': USER_ROLES_SET.solver,
  '/solver/candidate/certification/_solver_id': USER_ROLES_SET.solver,
}

// パスからパスパラメータを正規表現化したものを作る
// 例: "/admin/_adminId" -> "^/admin/[^/]+$"
function pathToReg(path: string) {
  if (path === '/man-hour/_year/_month') {
    // 月別支援工数管理画面の場合
    // eslint-disable-next-line prefer-regex-literals
    return new RegExp('^/man-hour?[^/]+&[^/]+$')
  } else {
    const replacedKey = path.replaceAll(/_[^/]+/g, '[^/]+')
    return new RegExp(`^${replacedKey}$`)
  }
}

// 対象のページにおいてアクセスが許可されているロール群を返す
export function getAllowedRoles(
  targetPath: string,
  printsLog = false
): UserRoleStr[] {
  const paths = Object.keys(allowedRolesDefine)

  for (let i = 0; i < paths.length; i++) {
    const path = paths[i]
    const reg = pathToReg(path)

    if (targetPath.match(reg)) {
      if (printsLog) {
        $logger.info(
          'this route is limited',
          targetPath,
          allowedRolesDefine[path]
        )
      }
      return allowedRolesDefine[path]
    }
  }

  if (printsLog) {
    $logger.info('this route is not limited', targetPath)
  }

  return []
}
