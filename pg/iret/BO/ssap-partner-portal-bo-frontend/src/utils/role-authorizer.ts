import { meStore } from '~/store'
import { Logger } from '~/plugins/logger'
import { ENUM_ADMIN_ROLE as ADMIN_ROLE, AdminRole } from '@/types/Admin'

export { ADMIN_ROLE, AdminRole } from '@/types/Admin'

const $logger = new Logger() // loggerプラグインを読み込み

export class AdminRoles extends Array<string> {
  constructor(roles: string[] = []) {
    super()
    roles.forEach((role) => {
      Object.keys(ADMIN_ROLE).forEach((key: string) => {
        if (role === ADMIN_ROLE[key]) {
          this.push(role)
        }
      })
    })
  }

  // 引数として渡された権限のうち、一つでも持っているかどうかを返す
  hasRole(roles: string | string[]): boolean {
    let rtn: boolean = false

    if (typeof roles === 'string') {
      // 単一の型が渡されたとき
      return this.includes(roles)
    } else {
      // 型の配列が渡されたとき
      roles.forEach((role: string) => {
        if (this.includes(role)) {
          rtn = true
        }
      })
    }

    return rtn
  }
}

export function hasRole(roles: string | string[]) {
  const adminRoles = new AdminRoles(meStore.roles)
  return adminRoles.hasRole(roles)
}

export const ROLES_SET: { [key: string]: AdminRole[] } = {
  all: [
    ADMIN_ROLE.SALES,
    ADMIN_ROLE.SUPPORTER_MGR,
    ADMIN_ROLE.SALES_MGR,
    ADMIN_ROLE.SURVEY_OPS,
    ADMIN_ROLE.MAN_HOUR_OPS,
    ADMIN_ROLE.SYSTEM_ADMIN,
    ADMIN_ROLE.BUSINESS_MGR,
  ],
  onlySystemAdmin: [ADMIN_ROLE.SYSTEM_ADMIN],
  notManHourOps: [
    ADMIN_ROLE.SALES,
    ADMIN_ROLE.SUPPORTER_MGR,
    ADMIN_ROLE.SALES_MGR,
    ADMIN_ROLE.SURVEY_OPS,
    ADMIN_ROLE.SYSTEM_ADMIN,
    ADMIN_ROLE.BUSINESS_MGR,
  ],
  surveyOps: [ADMIN_ROLE.SURVEY_OPS, ADMIN_ROLE.SYSTEM_ADMIN],
  manHourOps: [ADMIN_ROLE.MAN_HOUR_OPS, ADMIN_ROLE.SYSTEM_ADMIN],
  managerAndManHourOps: [
    ADMIN_ROLE.SUPPORTER_MGR,
    ADMIN_ROLE.SALES_MGR,
    ADMIN_ROLE.MAN_HOUR_OPS,
    ADMIN_ROLE.SYSTEM_ADMIN,
    ADMIN_ROLE.BUSINESS_MGR,
  ],
  opsAndSalesMng: [
    ADMIN_ROLE.SALES_MGR,
    ADMIN_ROLE.SURVEY_OPS,
    ADMIN_ROLE.MAN_HOUR_OPS,
    ADMIN_ROLE.SYSTEM_ADMIN,
    ADMIN_ROLE.BUSINESS_MGR,
  ],
  notManHourOpsAndNotSurveyOps: [
    ADMIN_ROLE.SALES,
    ADMIN_ROLE.SUPPORTER_MGR,
    ADMIN_ROLE.SALES_MGR,
    ADMIN_ROLE.SYSTEM_ADMIN,
    ADMIN_ROLE.BUSINESS_MGR,
  ],
}

export const allowedRolesDefine: {
  [key: string]: AdminRole[]
} = {
  '/home': ROLES_SET.all,

  '/customer/list': ROLES_SET.all,
  '/customer/create': ROLES_SET.all,
  '/customer/_customerId': ROLES_SET.all,

  '/project/list': ROLES_SET.all,
  '/project/create': ROLES_SET.all,
  '/project/_projectId': ROLES_SET.all,

  '/survey': ROLES_SET.notManHourOps,
  '/survey/list': ROLES_SET.notManHourOps,
  '/survey/_surveyId': ROLES_SET.notManHourOps,
  '/survey/report/monthly': ROLES_SET.notManHourOps,
  // v1.0ドロップ '/survey/report/projects': ROLES_SET.notManHourOps,
  '/survey/report/forecast': ROLES_SET.notManHourOps,
  '/survey/report/forecast-survey-ops': ROLES_SET.notManHourOps,
  '/survey/master/list': ROLES_SET.notManHourOps,
  '/survey/master/create': ROLES_SET.surveyOps,
  '/survey/master/_surveyMasterId': ROLES_SET.notManHourOps,
  '/survey/master/_surveyMasterId/_revision': ROLES_SET.notManHourOps,

  '/man-hour': ROLES_SET.managerAndManHourOps,
  '/man-hour/report/_year/_month': ROLES_SET.managerAndManHourOps,
  '/man-hour/report/_year/_month/project': ROLES_SET.managerAndManHourOps,
  '/man-hour/supporter/_year/_month': ROLES_SET.managerAndManHourOps,
  '/man-hour/supporter/_year/_month/_userId': ROLES_SET.managerAndManHourOps,
  '/man-hour/alert/list': ROLES_SET.managerAndManHourOps,
  '/man-hour/alert/_year/_month/_projectId': ROLES_SET.managerAndManHourOps,
  '/man-hour/alert/config': ROLES_SET.manHourOps,

  '/karte/list': ROLES_SET.all,
  '/karte/list/_projectId': ROLES_SET.notManHourOpsAndNotSurveyOps,
  '/karte/_karteId': ROLES_SET.notManHourOpsAndNotSurveyOps,

  '/admin/list': ROLES_SET.onlySystemAdmin,
  '/admin/create': ROLES_SET.onlySystemAdmin,
  '/admin/_adminId': ROLES_SET.onlySystemAdmin,

  '/user/list': ROLES_SET.all,
  '/user/create': ROLES_SET.onlySystemAdmin,
  '/user/_userId': ROLES_SET.all,

  '/master/list': ROLES_SET.opsAndSalesMng,
  '/master/_dataType/_masterId': ROLES_SET.opsAndSalesMng,
  '/master/create': ROLES_SET.opsAndSalesMng,
  '/master-karte/list/_npfProjectId': ROLES_SET.all,
  '/master-karte/_npfProjectId': ROLES_SET.all,
}

// パスからパスパラメータを正規表現化したものを作る
// 例: "/admin/_adminId" -> "^/admin/[^/]+$"
function pathToReg(path: string) {
  const replacedKey = path.replaceAll(/_[^/]+/g, '[^/]+')
  return new RegExp(`^${replacedKey}$`)
}

// 対象のページにおいてアクセスが許可されているロール群を返す
export function getAllowedRoles(targetPath: string): AdminRole[] {
  const paths = Object.keys(allowedRolesDefine)

  for (let i = 0; i < paths.length; i++) {
    const path = paths[i]
    const reg = pathToReg(path)

    if (targetPath.match(reg)) {
      $logger.info(
        'this route is limited',
        targetPath,
        allowedRolesDefine[path]
      )
      return allowedRolesDefine[path]
    }
  }

  $logger.info('this route is not limited', targetPath)

  return []
}
