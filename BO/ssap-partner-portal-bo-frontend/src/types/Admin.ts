export interface ISupporterOrganization {
  id: string
  name: string
  shortName: string
}
export interface IGetSupporterOrganizations {
  supporterOrganizations: ISupporterOrganization[]
}

export interface IAdmin {
  id: string
  name: string
  email: string
  roles: string[]
  company: string
  job: string
  supporterOrganizations: ISupporterOrganization[]
  organizationName: string
  lastLoginAt: string
  disabled: boolean
  createId: string
  createUserName: string
  createAt: string
  updateId: string
  updateUserName: string
  updateAt: string
  version: number
}
export interface IAdminListItem {
  id: string
  name: string
  email: string
  roles: string[]
  company: string
  job: string
  supporterOrganizations: ISupporterOrganization[]
  organizationName: string
  lastLoginAt: string
  disabled: boolean
}

export interface IAdminRoleDefine {
  SALES: string
  SUPPORTER_MGR: string
  SALES_MGR: string
  SURVEY_OPS: string
  MAN_HOUR_OPS: string
  SYSTEM_ADMIN: string
  BUSINESS_MGR: string
  [key: string]: string
}

// TODO: ENUM_ADMIN_ROLEに統一する
export const ADMIN_ROLE: IAdminRoleDefine = {
  SALES: 'sales',
  SUPPORTER_MGR: 'supporter_mgr',
  SALES_MGR: 'sales_mgr',
  SURVEY_OPS: 'survey_ops',
  MAN_HOUR_OPS: 'man_hour_ops',
  SYSTEM_ADMIN: 'system_admin',
  BUSINESS_MGR: 'business_mgr',
} as const

export const ENUM_ADMIN_ROLE = ADMIN_ROLE

export type AdminRole = typeof ENUM_ADMIN_ROLE[keyof typeof ENUM_ADMIN_ROLE]

export interface IGetAdmins {
  total: number
  admins: IAdminListItem[]
}
interface IGetAdminsRequestSortDefine {
  LAST_LOGIN_AT_ASC: string
  LAST_LOGIN_AT_DESC: string
  // [key: string]: string
}
export const GET_ADMIN_REQUEST_SORT: IGetAdminsRequestSortDefine = {
  LAST_LOGIN_AT_ASC: 'last_login_at:asc',
  LAST_LOGIN_AT_DESC: 'last_login_at:desc',
}
export interface IGetAdminsRequest {
  email?: string
  name?: string
  sort: string
  limit: number
  offsetPage: number
}
export interface IGetAdminsResponse {
  offsetPage: number
  total: number
  admins: IAdminListItem[]
}

export interface IGetAdminByIdRequest {
  id: string
}

export interface IGetAdminByMineRequest {
  // No Parameters
}

export interface IGetAdminByMineResponse extends IAdmin {
  totalNotifications: number
}

export interface ICreateAdminRequest {
  name: string
  email: string
  roles: string[]
  company: string
  job: string
  supporterOrganizationId: ISupporterOrganization[]
  organizationName: string
  disabled: boolean
}
export interface ICreateAdminResponse {
  id: string
  name: string
  email: string
  roles: string[]
  company: string
  job: string
  supporterOrganizationId: ISupporterOrganization[]
  organizationName: string
  disabled: boolean
}

export const ENUM_CREATE_ADMIN_RESPONSE_ERROR = {
  ALREADY_REGISTERED: 'email address is already registered.',
} as const

export interface IUpdateAdminByIdRequest {
  name: string
  roles: string[]
  company: string
  job: string
  supporterOrganizationId: string[]
  organizationName: string
  disabled: boolean
}
export interface IUpdateAdminByIdResponse {
  name: string
  email: string
  roles: string[]
  company: string
  job: string
  supporterOrganizationId: ISupporterOrganization[]
  organizationName: string
  disabled: boolean
}

export interface IPatchAdminStatusByIdRequest {
  id: string
  version: number
  enable: boolean
}

export interface IPatchAdminStatusByIdResponse {
  id: string
  name: string
  email: string
  roles: string[]
  company: string
  job: string
  supporterOrganizations: ISupporterOrganization[]
  organizationName: string
  lastLoginAt: string
  disabled: boolean
  createId: string
  createUserName: string
  createAt: string
  updateId: string
  updateUserName: string
  updateAt: string
  version: number
}
