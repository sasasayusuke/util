import {
  IMasterListItem,
  IGetMastersRequest,
  IGetMastersResponse,
  IGetMasterByIdRequest,
  IGetMasterByIdResponse,
  ICreateMasterResponse,
  IUpdateMasterRequestParams,
  IUpdateMasterResponse,
  IUpdateMasterRequest,
  ICreateMasterRequest,
  IServiceTypeItems,
  IGetServiceTypesResponse,
  ISupporterOrganizationItems,
  IGetSupporterOrganizationsResponse,
  ISurveyMasterItem,
  IGetSurveyMastersResponse,
  IGetBatchControlByIdResponse,
  IGetSelectItemsResponse,
  IGetSelectItemsRequest,
} from '@/types/Master'

import { Api } from '~/plugins/api'

const $api = new Api()

export class MasterListItem implements IMasterListItem {
  public id = ''
  public dataType = ''
  public name = ''
  public value = ''
  public use = false
  public maintenance = false
  public createAt = ''
  public updateAt = ''
}

export class GetMastersRequest implements IGetMastersRequest {
  public dataType = 'all'
  public limit = 10
  public offsetPage = 1
}
export class GetMastersResponse implements IGetMastersResponse {
  public offsetPage = 1
  public total = 0
  public masters = []
}

export async function GetMasters(request: GetMastersRequest) {
  return await $api.get<GetMastersResponse>(`/masters`, request)
}

export class GetMasterByIdRequest implements IGetMasterByIdRequest {
  public id = ''
}

export class GetMasterByIdResponse implements IGetMasterByIdResponse {
  public id = ''
  public dataType = ''
  public name = ''
  public value = ''
  public attributes = {}
  public use = true
  public createAt = ''
  public updateAt = ''
  public version = 0
}

export function GetMasterById(id: string) {
  return $api.get<GetMasterByIdResponse>(`/masters/${id}`)
}

export class CreateMasterRequest implements ICreateMasterRequest {
  public dataType = ''
  public name = ''
  public value = ''
  public attributes = {}
  public use = false
}

export class CreateMasterResponse implements ICreateMasterResponse {
  public id = ''
  public dataType = ''
  public name = ''
  public value = ''
  public attributes = {}
  public use = false
}

export async function CreateMaster(request: CreateMasterRequest) {
  return await $api.post<CreateMasterResponse>(`/masters`, request)
}

export class UpdateMasterRequestParams implements IUpdateMasterRequestParams {
  public id = ''
  public version = 0
}

export class UpdateMasterRequest implements IUpdateMasterRequest {
  public name = ''
  public value = ''
  public attributes = {}
  public order = 1
  public use = true
  public maintenance = true
}

export class UpdateMasterResponse implements IUpdateMasterResponse {
  public dataType = ''
  public name = ''
  public value = ''
  public attributes = {}
  public use = false
}

export async function UpdateMasterById(
  id: string,
  version: number,
  request: UpdateMasterRequest
) {
  return await $api.put<UpdateMasterResponse>(
    `/masters/${id}?version=${version}`,
    request
  )
}

// TODO: 名称を単数形に統一する
export class ServiceTypeItems implements IServiceTypeItems {
  public id = ''
  public name = ''
}
export class GetServiceTypesResponse implements IGetServiceTypesResponse {
  public serviceTypes: ServiceTypeItems[] = []
}

export async function GetServiceTypes() {
  return await $api.get<GetServiceTypesResponse>(`/masters/service-types`)
}

// TODO: 名称を単数形に統一する
export class SupporterOrganizationItems implements ISupporterOrganizationItems {
  public id = ''
  public name = ''
  public shortName = ''
}

export class GetSupporterOrganizationsResponse
  implements IGetSupporterOrganizationsResponse
{
  public supporterOrganizations: SupporterOrganizationItems[] = []
}

export async function GetSupporterOrganizations() {
  return await $api.get<GetSupporterOrganizationsResponse>(
    `/masters/supporter-organizations`
  )
}

export class GetBatchControlByIdResponse
  implements IGetBatchControlByIdResponse
{
  public batchEndAt = ''
}

export async function GetBatchControlById(id: string) {
  return await $api.get<GetBatchControlByIdResponse>(
    `/masters/batch-control/${id}`
  )
}

export class SurveyMasterItem implements ISurveyMasterItem {
  public id = ''
  public revision = 0
  public name = ''
  public type = ''
  public timing = ''
  public initSendDaySetting = 0
  public initAnswerLimitDaySetting = 0
  public isDisclosure = true
  public isLatest = true
}
export class GetSurveyMastersResponse implements IGetSurveyMastersResponse {
  public total = 0
  public masters: SurveyMasterItem[] = []
}

export async function GetSurveyMasters() {
  return await $api.get<GetSurveyMastersResponse>(`/survey-masters`)
}

export class GetSelectItemsResponse implements IGetSelectItemsResponse {
  public masters = [
    {
      id: '',
      category: '',
      name: '',
    },
  ]
}

export async function GetSelectItems(request: IGetSelectItemsRequest) {
  return await $api.get<GetSelectItemsResponse>(
    `/masters/select-items`,
    request
  )
}
