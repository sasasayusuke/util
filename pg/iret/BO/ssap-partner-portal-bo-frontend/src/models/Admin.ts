import {
  IAdmin,
  IAdminListItem,
  IGetAdminsRequest,
  IGetAdminsResponse,
  IGetAdminByMineRequest,
  IGetAdminByMineResponse,
  ICreateAdminRequest,
  ICreateAdminResponse,
  IUpdateAdminByIdRequest,
  IUpdateAdminByIdResponse,
  ISupporterOrganization,
  IPatchAdminStatusByIdRequest,
  IPatchAdminStatusByIdResponse,
  GET_ADMIN_REQUEST_SORT,
} from '@/types/Admin'
import { Api } from '~/plugins/api'

const $api = new Api()

export class GetAdminByIdResponse implements IAdmin {
  public id = ''
  public name = ''
  public email = ''
  public roles = [] as string[]
  public company = ''
  public job = ''
  public supporterOrganizations = [] as ISupporterOrganization[]
  public organizationName = ''
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
export class AdminListItem implements IAdminListItem {
  public id = ''
  public name = ''
  public email = ''
  public roles = [] as string[]
  public company = ''
  public job = ''
  public supporterOrganizations = [] as ISupporterOrganization[]
  public organizationName = ''
  public lastLoginAt = ''
  public disabled = false
}

export class GetAdminsResponse implements IGetAdminsResponse {
  public offsetPage = 0
  public total = 0
  public admins: AdminListItem[] = []
}
export class GetAdminsRequest implements IGetAdminsRequest {
  public name?: string
  public email?: string
  public limit = 20
  public sort = GET_ADMIN_REQUEST_SORT.LAST_LOGIN_AT_DESC
  public offsetPage = 1
}
export class GetAdminByIdRequest implements IGetAdminByMineRequest {
  public id = ''
}

export class GetAdminByMineResponse implements IGetAdminByMineResponse {
  public id = ''
  public name = ''
  public email = ''
  public roles = []
  public company = ''
  public job = ''
  public supporterOrganizations = []
  public organizationName = ''
  public lastLoginAt = ''
  public disabled = false
  public createId = ''
  public createUserName = ''
  public createAt = ''
  public updateId = ''
  public updateUserName = ''
  public updateAt = ''
  public version = 0
  public totalNotifications = 0
  public showNotifications = false
}
export class CreateAdminRequest implements ICreateAdminRequest {
  public name = ''
  public email = ''
  public roles = []
  public company = ''
  public job = ''
  public supporterOrganizationId = []
  public organizationName = ''
  public disabled = false
}
export class CreateAdminResponse implements ICreateAdminResponse {
  public id = ''
  public name = ''
  public email = ''
  public roles = []
  public company = ''
  public job = ''
  public supporterOrganizationId = []
  public organizationName = ''
  public disabled = false
}
export class CreateLocalAdmin {
  public name: string = ''
  public email: string = ''
  public roles: string[] = []
  public company: string = ''
  public job: string = ''
  public supporterOrganizationId: string[] = []
  public organizationName: string = ''
  public disabled: boolean = false
  public id: string = ''
  public version: number = 0
}
export class SupporterOrganization {
  id: string = ''
  name: string = ''
}
export class supporterOrganizationWithShortName extends SupporterOrganization {
  shortName: string = ''
}
export class UpdateAdminByIdRequest implements IUpdateAdminByIdRequest {
  public name = ''
  public email: string = ''
  public roles: string[] = []
  public company = ''
  public job = ''
  public supporterOrganizationId: string[] = []
  public organizationName = ''
  public disabled = false
}
export class UpdateAdminByIdResponse implements IUpdateAdminByIdResponse {
  public name = ''
  public email = ''
  public roles = []
  public company = ''
  public job = ''
  public supporterOrganizationId = []
  public organizationName = ''
  public disabled = false
}
export class PatchAdminStatusByIdRequest
  implements IPatchAdminStatusByIdRequest
{
  public id = ''
  public version = 0
  public enable = false
}

export class PatchAdminStatusByIdResponse
  implements IPatchAdminStatusByIdResponse
{
  public id = ''
  public name = ''
  public email = ''
  public roles = [] as string[]
  public company = ''
  public job = ''
  public supporterOrganizations = [] as ISupporterOrganization[]
  public organizationName = ''
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

export async function GetAdmins(request: GetAdminsRequest) {
  return await $api.get<GetAdminsResponse>(`/admins`, request)
}
export async function GetAdminById(id: string) {
  return await $api.get<IAdmin>(`/admins/${id}`)
}
export async function CreateAdmin(request: CreateAdminRequest) {
  return await $api.post<CreateAdminResponse>(`/admins`, request)
}

export async function UpdateAdminById(
  id: string,
  version: number,
  params: UpdateAdminByIdRequest
) {
  return await $api.put<UpdateAdminByIdResponse, UpdateAdminByIdRequest>(
    `/admins/${id}?version=${version}`,
    params
  )
}
export async function PatchAdminStatusById(
  request: PatchAdminStatusByIdRequest
) {
  return await $api.patch<PatchAdminStatusByIdResponse>(
    `/admins/${request.id}?version=${request.version}&enable=${request.enable}`
  )
}

export async function getAdminByMine() {
  return await $api.get<GetAdminByMineResponse>(`/admins/me`)
}
