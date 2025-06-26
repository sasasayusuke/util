export const ENUM_USER_ROLE = {
  CUSTOMER: 'customer',
  SUPPORTER: 'supporter',
  SALES: 'sales',
  SUPPORTER_MGR: 'supporter_mgr',
  SALES_MGR: 'sales_mgr',
} as const

export type UserRole = typeof ENUM_USER_ROLE[keyof typeof ENUM_USER_ROLE]

export interface ISupporterOrganization {
  id: string
}

export interface IProjectIds {
  id: string
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

interface IGetUsersRequestSortDefine {
  NAME_ASC: string
  NAME_DESC: string
  LAST_LOGIN_AT_ASC: string
  LAST_LOGIN_AT_DESC: string
  UPDATE_AT_ASC: string
  UPDATE_AT_DESC: string
  [key: string]: string
}

export const GET_USERS_REQUEST_SORT: IGetUsersRequestSortDefine = {
  NAME_ASC: 'name:asc',
  NAME_DESC: 'name:desc',
  LAST_LOGIN_AT_ASC: 'last_login_at:asc',
  LAST_LOGIN_AT_DESC: 'last_login_at:desc',
  UPDATE_AT_ASC: 'updateAt:asc',
  UPDATE_AT_DESC: 'updateAt:desc',
}

export interface IGetUsersRequest {
  name?: string
  email?: string
  sort?: string
  role?: string
  limit?: number
  offsetPage?: number
}

export interface IGetUsersResponse {
  offsetPage: number
  total: number
  users: IUserListItem[]
}

export interface IPatchUserStatusRequest {
  id: string
  version: number
  enable: boolean
}

export interface IPatchUserStatusResponse {
  id: string
  supporterOrganizations: ISupporterOrganization[]
  organizationName: string
  disabled: boolean
  version: number
}

interface ISuggestUsersSortDefine {
  NAME_ASC: string
  EMAIL_ASC: string
  [key: string]: string
}

export const SUGGEST_USERS_REQUEST_SORT: ISuggestUsersSortDefine = {
  NAME_ASC: 'name:asc',
  EMAIL_ASC: 'email:asc',
}

export interface ISuggestUsersRequest {
  role: string
  disabled: boolean
  sort: string
}

export interface ISuggestUser {
  id: string
  name: string
}

export interface IUserDetail {
  id: string
  name: string
  email: string
  role: string
  customerId: string
  customerName: string
  job: string
  company: string
  solverCorporationId: string
  supporterOrganizations: ISupporterOrganization[]
  organizationName: string
  isInputManHour: boolean
  projectIds: string[]
  agreed: boolean
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

export interface ICreateUserRequest {
  name: string
  email: string
  role: string
  customerId?: string
  job?: string
  company?: string
  solverCorporationId?: string
  supporterOrganizations?: ISupporterOrganization[]
  isInputManHour?: boolean
  organizationName?: string
}

export interface ICreateUserResponse extends IUserDetail {}

export const ENUM_CREATE_USER_RESPONSE_ERROR = {
  ALREADY_REGISTERED: 'email address is already registered.',
} as const

export interface IGetUserByIdResponse extends IUserDetail {}

export interface IUpdateUserByIdRequest {
  name: string
  job?: string
  company?: string
  solverCorporationId?: string
  supporterOrganizations?: ISupporterOrganization[]
  organizationName?: string
  isInputManHour?: boolean
}

export interface IUpdateUserByIdResponse extends IUserDetail {}

export interface IDeleteUserByIdResponse extends IUserDetail {}
