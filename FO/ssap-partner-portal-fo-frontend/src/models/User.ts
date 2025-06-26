import {
  ISupporterOrganization,
  UserRole,
  IGetUserByMineResponse,
  IPatchUserByMineRequest,
  IPatchUserByMineResponse,
  ISuggestUsersRequest,
  SUGGEST_USERS_REQUEST_SORT,
  ISuggestUser,
  ICreateUserRequest,
  ICreateUserResponse,
  IUserListItem,
  IGetUsersRequest,
  IGetUsersResponse,
  IGetUserByIdResponse,
  IUpdateUserByIdRequestParams,
  IUpdateUserByIdRequest,
  IUpdateUserByIdResponse,
} from '@/types/User'

import { Api } from '~/plugins/api'

const $api = new Api()

export class SupporterOrganization implements ISupporterOrganization {
  public id = ''
  public name = ''
}

export class GetUserByMineResponse implements IGetUserByMineResponse {
  public id = ''
  public name = ''
  public email = ''
  public role = '' as UserRole
  public customerId = ''
  public customerName = ''
  public job = ''
  public company = ''
  public solverCorporationId = ''
  public supporterOrganizations = []
  public organizationName = ''
  public isInputManHour = false
  public projectIds = []
  public agreed = false
  public lastLoginAt = ''
  public disabled = false
  public totalNotifications = 0
  public showNotifications = false
}

export class PatchUserByMineRequest implements IPatchUserByMineRequest {
  public agreed = false
}

export class PatchUserByMineResponse implements IPatchUserByMineResponse {}

export async function PatchUserByMine(request: PatchUserByMineRequest) {
  return await $api.patch<PatchUserByMineResponse>(`/users/me`, request)
}

export class SuggestUsersRequest implements ISuggestUsersRequest {
  public role = ''
  public disable = true
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
  public name = ''
  public email = ''
  public role = ''
  public customerId = ''
  public job = ''
  public organizationName = ''
  public projectId = ''
}

export class CreateUserResponse implements ICreateUserResponse {
  public id = ''
  public name = ''
  public email = ''
  public role = ''
  public customerId = ''
  public job = ''
  public organizationName = ''
  public projectIds = []
  public disabled = false
  public version = 0
}

export async function CreateUser(
  request: CreateUserRequest = new CreateUserRequest()
) {
  return await $api.post<CreateUserResponse>('/users', request)
}

export class UserListItem implements IUserListItem {
  public id = ''
  public name = ''
  public email = ''
  public role = ''
  public customerId = ''
  public customerName = ''
  public job = ''
  public company = ''
  public supporterOrganizations: SupporterOrganization[] = []
  public organizationName = ''
  public isInputManHour = true
  public projectIds = []
  public agreed = true
  public lastLoginAt = ''
  public disabled = true
}

export class GetUsersRequest implements IGetUsersRequest {
  public email?: string
  public name?: string
  public role?: string
}

export class GetUsersResponse implements IGetUsersResponse {
  public total = 0
  public users: UserListItem[] = []
}

export async function GetUsers(request: IGetUsersRequest) {
  return await $api.get<GetUsersResponse>('/users', request)
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
  public supporterOrganizations: SupporterOrganization[] = []
  public organizationName = ''
  public isInputManHour = true
  public projectIds = []
  public agreed = true
  public lastLoginAt = ''
  public disabled = true
  public createId = ''
  public createAt = ''
  public updateId = ''
  public updateAt = ''
  public version = 0
}

export async function GetUserById(id: string) {
  return await $api.get<GetUserByIdResponse>(`/users/${id}`)
}

export class UpdateUserByIdRequestParams
  implements IUpdateUserByIdRequestParams
{
  public version = 0
  public id = ''
}

export class UpdateUserByIdRequest implements IUpdateUserByIdRequest {
  public name = ''
  public job = ''
  public company = ''
  public organizationName = ''
}

export class UpdateUserByIdResponse implements IUpdateUserByIdResponse {
  public id = ''
  public name = ''
  public email = ''
  public role = ''
  public customerId = ''
  public customerName = ''
  public job = ''
  public organizationName = ''
  public version = 0
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
