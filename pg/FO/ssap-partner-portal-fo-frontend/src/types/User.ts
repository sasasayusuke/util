export interface ISupporterOrganization {
  id: string
  name: string
}

export interface IProjectId {
  [key: number]: string
}

export interface IGetUserByMineRequest {
  // No Parameters
}

export const ENUM_USER_ROLE = {
  CUSTOMER: 'customer',
  SUPPORTER: 'supporter',
  SALES: 'sales',
  SUPPORTER_MGR: 'supporter_mgr',
  SALES_MGR: 'sales_mgr',
  BUSINESS_MGR: 'business_mgr',
  APT: 'apt',
  SOLVER_STAFF: 'solver_staff',
} as const

export type UserRole = typeof ENUM_USER_ROLE[keyof typeof ENUM_USER_ROLE]

export interface IGetUserByMineResponse {
  id: string
  name: string
  email: string
  role: UserRole
  customerId: string
  customerName: string
  job: string
  company: string
  solverCorporationId: string
  supporterOrganizations: ISupporterOrganization[]
  organizationName: string
  isInputManHour: boolean
  projectIds: IProjectId[]
  agreed: boolean
  lastLoginAt: string
  disabled: boolean
  totalNotifications: number
}

export interface IPatchUserByMineRequest {
  agreed: boolean
}

export interface IPatchUserByMineResponse {
  [key: string]: any
}

interface ISuggestUserSortDefine {
  NAME_ASC: string
  [key: string]: string
}

export const SUGGEST_USERS_REQUEST_SORT: ISuggestUserSortDefine = {
  NAME_ASC: 'name:asc',
}

export interface ISuggestUsersRequest {
  role?: string
  disable?: boolean
  sort?: string
}

export interface ISuggestUser {
  id: string
  name: string
  email: string
  customerName: string
}

export interface ICreateUserRequest {
  name: string
  email: string
  role: string
  customerId: string
  job: string
  organizationName: string
  projectId: string
}

export interface ICreateUserResponse {
  id: string
  name: string
  email: string
  role: string
  customerId: string
  job: string
  organizationName: string
  projectIds: string[]
  disabled: boolean
  version: number
}

export interface IUserListItem {
  id: string
  name: string
  email: string
  role: string
  customerId: string
  customerName: string
  job: string
  company: string
  supporterOrganizations: ISupporterOrganization[]
  organizationName: string
  isInputManHour: boolean
  projectIds: string[]
  agreed: boolean
  lastLoginAt: string
  disabled: boolean
}

export interface IGetUsersRequest {
  email?: string
  name?: string
  role?: string
}

export interface IGetUsersResponse {
  total: number
  users: IUserListItem[]
}

export interface IGetUserByIdResponse {
  id: string
  name: string
  email: string
  role: string
  customerId: string
  customerName: string
  job: string
  company: string
  supporterOrganizations: ISupporterOrganization[]
  organizationName: string
  isInputManHour: boolean
  projectIds: string[]
  agreed: boolean
  lastLoginAt: string
  disabled: boolean
  createId: string
  createAt: string
  updateId: string
  updateAt: string
  version: number
}

export interface IUpdateUserByIdRequestParams {
  version: number
  id: string
}

export interface IUpdateUserByIdRequest {
  name: string
  job: string
  company: string
  organizationName: string
}

export interface IUpdateUserByIdResponse {
  id: string
  name: string
  email: string
  role: string
  customerId: string
  customerName: string
  job: string
  organizationName: string
  version: number
}
