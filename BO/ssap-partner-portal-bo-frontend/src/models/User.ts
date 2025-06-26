import {
  IUserListItem,
  IGetUsersRequest,
  IGetUsersResponse,
  // GET_USERS_REQUEST_SORT,
  ISuggestUsersRequest,
  SUGGEST_USERS_REQUEST_SORT,
  ISuggestUser,
  ICreateUserRequest,
  ICreateUserResponse,
  IUpdateUserByIdRequest,
  IUpdateUserByIdResponse,
  IUserDetail,
  IGetUserByIdResponse,
  IPatchUserStatusRequest,
  IPatchUserStatusResponse,
  ISupporterOrganization,
} from '@/types/User'
import { Api } from '~/plugins/api'

const $api = new Api()

export class UserListItem implements IUserListItem {
  public id = ''
  public name = ''
  public email = ''
  public role = ''
  public customerId = ''
  public customerName = ''
  public job = ''
  public company = ''
  public supporterOrganizations = []
  public organizationName = ''
  public isInputManHour = true
  public projectIds: string[] = []
  public agreed = true
  public lastLoginAt = ''
  public disabled = false
  public createId = ''
  public createUserName = ''
  public createAt = ''
  public updateId = ''
  public updateUserName = ''
  public updateAt = ''
  public version = 0
}

export class GetUsersRequest implements IGetUsersRequest {
  public name?: string
  public email?: string
  public sort?: string
  public role?: string
  public limit?: number = 20
  public offsetPage?: number = 1
}

export class GetUsersResponse implements IGetUsersResponse {
  public offsetPage = 0
  public total = 0
  public users: UserListItem[] = []
}

export async function GetUsers(request: GetUsersRequest) {
  return await $api.get<GetUsersResponse>(`/users`, request)
}

export class SuggestUsersRequest implements ISuggestUsersRequest {
  public role = 'sales'
  public disabled = true
  public sort = SUGGEST_USERS_REQUEST_SORT.NAME_ASC
}

export class SuggestUser implements ISuggestUser {
  public id = ''
  public name = ''
  public email = ''
  public customerName = ''
}

export class SuggestUsersResponse extends Array<SuggestUser> {}

export async function SuggestUsers(
  request: SuggestUsersRequest = new SuggestUsersRequest()
) {
  return await $api.get<SuggestUsersResponse>('/users/suggest', request)
}

export class CreateUserRequest implements ICreateUserRequest {
  name = ''
  email = ''
  role = ''
  customerId?: string
  job?: string
  company?: string
  solverCorporationId?: string
  supporterOrganizations?: []
  isInputManHour?: boolean = true
  organizationName?: string = ''
}
export class CreateUserResponse implements ICreateUserResponse {
  id = ''
  name = ''
  email = ''
  role = ''
  customerId = ''
  customerName = ''
  job = ''
  company = ''
  solverCorporationId = ''
  supporterOrganizations = []
  organizationName = ''
  isInputManHour = true
  projectIds = []
  agreed = false
  lastLoginAt = ''
  disabled = false
  createId = ''
  createUserName = ''
  createAt = ''
  updateId = ''
  updateUserName = ''
  updateAt = ''
  version = 0
}

export async function CreateUser(request: CreateUserRequest) {
  return await $api.post<CreateUserResponse>(`/users`, request)
}

export class PatchUserStatusRequest implements IPatchUserStatusRequest {
  public id = ''
  public version = 0
  public enable = false
}

export async function PatchUser(request: PatchUserStatusRequest) {
  return await $api.patch<PatchUserStatusResponse>(
    `/users/${request.id}?version=${request.version}&enable=${request.enable}`
  )
}

export class PatchUserStatusResponse implements IPatchUserStatusResponse {
  public id = ''
  public supporterOrganizations = []
  public organizationName = ''
  public disabled = false
  public version = 1
}

export class GetUserByIdResponse implements IGetUserByIdResponse {
  public id = ''
  public name = ''
  public email = ''
  public role = ''
  public customerId = ''
  public customerName = ''
  public job = ''
  public company = ''
  public solverCorporationId = ''
  public supporterOrganizations = []
  public organizationName = ''
  public isInputManHour = true
  public projectIds: string[] = []
  public agreed = true
  public lastLoginAt = ''
  public disabled = false
  public createId = ''
  public createUserName = ''
  public createAt = ''
  public updateId = ''
  public updateUserName = ''
  public updateAt = ''
  public version = 0
}

export async function GetUserById(id: string) {
  return await $api.get<GetUserByIdResponse>(`/users/${id}`)
}

export class UpdateUserByIdRequest implements IUpdateUserByIdRequest {
  name = ''
  job = ''
  company = ''
  solverCorporationId = ''
  supporterOrganizations: ISupporterOrganization[] = []
  organizationName = ''
  isInputManHour = true
}

export class UpdateUserByIdResponse implements IUpdateUserByIdResponse {
  id = ''
  name = ''
  email = ''
  role = ''
  customerId = ''
  customerName = ''
  job = ''
  company = ''
  solverCorporationId = ''
  supporterOrganizations = []
  organizationName = ''
  isInputManHour = true
  projectIds = []
  agreed = false
  lastLoginAt = ''
  disabled = false
  createId = ''
  createUserName = ''
  createAt = ''
  updateId = ''
  updateUserName = ''
  updateAt = ''
  version = 0
}

export async function UpdateUserById(
  id: string,
  version: number,
  request: UpdateUserByIdRequest
) {
  return await $api.put<UpdateUserByIdResponse>(
    `/users/${id}?version=${version}`,
    request
  )
}
export class UserDetail implements IUserDetail {
  public id = ''
  public name = ''
  public email = ''
  public role = ''
  public customerId = ''
  public customerName = ''
  public job = ''
  public company = ''
  public solverCorporationId = ''
  public supporterOrganizations = []
  public organizationName = ''
  public isInputManHour = true
  public projectIds = []
  public agreed = true
  public lastLoginAt = ''
  public disabled = false
  public createId = ''
  public createUserName = ''
  public createAt = ''
  public updateId = ''
  public updateUserName = ''
  public updateAt = ''
  public version = 0
}
