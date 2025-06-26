import {
  IGetAlertSettingResponse,
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
  IGetSurveyMastersRequest,
  IGetSurveyMastersResponse,
  IServiceTypeItems,
  IGetServiceTypesResponse,
  ISupporterOrganizationItems,
  IGetSupporterOrganizationsResponse,
  ISupporterOrganizationsResponse,
  IGetSurveyMastersByIdResponse,
  ICopySurveyMasterByIdResponse,
  IPatchSurveyMasterRevisionByIdResponse,
  ISurveyMasterChoiceGroupItem,
  ISurveyMasterChoiceItem,
  ISurveyMasterQuestionFlowItem,
  ISurveyMasterQuestionItem,
  ICreateSurveyMastersRequest,
  ICreateSurveyMastersResponse,
  IGetSurveyMasterByIdAndRevisionResponse,
  IUpdateSurveyMasterLatestByIdRequest,
  IUpdateSurveyMasterLatestByIdResponse,
  IGetBatchControlByIdResponse,
  IUpdateAlertSettingsResponse,
  IUpdateAlertSettingsRequest,
  IUpdateSurveyMasterDraftByIdRequest,
  IAttributes,
  IAlertSettingAttributes,
  IDisplaySetting,
  IFactorSetting,
  IDirectSupportManHour,
  IPreSupportManHour,
  GetSurveyMastersIsLatest,
  ENUM_GET_SURVEY_MASTERS_IS_LATEST,
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
  public limit = 20
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

export class Attributes implements IAttributes {
  public info1 = ''
  public info2 = ''
  public info3 = ''
  public info4 = ''
  public info5 = ''
}

export class GetMasterByIdResponse implements IGetMasterByIdResponse {
  public id = ''
  public dataType = ''
  public name = ''
  public value = ''
  public attributes = new Attributes()
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
  public attributes = new Attributes()
  public use = false
}

export class CreateMasterResponse implements ICreateMasterResponse {
  public id = ''
  public dataType = ''
  public name = ''
  public value = ''
  public attributes = new Attributes()
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
  public attributes = new Attributes()
  public order = 1
  public use = true
  public maintenance = true
}

export class UpdateMasterResponse implements IUpdateMasterResponse {
  public dataType = ''
  public name = ''
  public value = ''
  public attributes = new Attributes()
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
export class GetSurveyMastersRequest implements IGetSurveyMastersRequest {
  name = ''
  latest = true
}

export class SurveyMasterListItem {
  public id = ''
  public revision = 0
  public name = ''
  public type = ''
  public timing = ''
  public initSendDaySetting = 0
  public initAnswerLimitDaySetting = 0
  public isDisclosure = false
  public isLatest: GetSurveyMastersIsLatest =
    ENUM_GET_SURVEY_MASTERS_IS_LATEST.FALSE

  public memo = ''
  public status = ''
  public questionCount = 0
  public updateAt = ''
}
export class GetSurveyMastersResponse implements IGetSurveyMastersResponse {
  total = 0
  masters: SurveyMasterListItem[] = []
}

export async function GetSurveyMasters(request: GetSurveyMastersRequest) {
  return await $api.get<GetSurveyMastersResponse>(`/survey-masters`, request)
}

export class ServiceTypeItems implements IServiceTypeItems {
  public id = ''
  public name = ''
}
export class GetServiceTypesResponse implements IGetServiceTypesResponse {
  public serviceTypes: IServiceTypeItems[] = []
}

export async function GetServiceTypes() {
  return await $api.get<GetServiceTypesResponse>(`/masters/service-types`)
}

export class SupporterOrganizationItems implements ISupporterOrganizationItems {
  public id = ''
  public name = ''
  public shortName? = ''
}

export class GetSupporterOrganizationsResponse
  implements IGetSupporterOrganizationsResponse
{
  public supporterOrganizations: ISupporterOrganizationItems[] = []
}

export async function GetSupporterOrganizations() {
  return await $api.get<GetSupporterOrganizationsResponse>(
    `/masters/supporter-organizations`
  )
}

export class SupporterOrganizationsResponse
  implements ISupporterOrganizationsResponse
{
  public id = ''
  public name = ''
  public shortName = ''
}
export class GetSurveyMastersByIdResponse
  implements IGetSurveyMastersByIdResponse
{
  public total = 0
  public masters: SurveyMasterListItem[] = []
}

export class CopySurveyMasterByIdResponse
  implements ICopySurveyMasterByIdResponse
{
  public status = ''
  public revision = 0
  public memo = ''
  public questionsCont = 0
  public updateAt = ''
}

export class PatchSurveyMasterRevisionByIdResponse
  implements IPatchSurveyMasterRevisionByIdResponse
{
  public status = ''
  public revision = 0
  public memo = ''
  public questionsCont = 0
  public updateAt = ''
}

export class SurveyMasterChoiceGroupItem
  implements ISurveyMasterChoiceGroupItem
{
  public id = ''
  public title = ''
  public disabled = false
  public isNew = false
}

export class SurveyMasterChoiceItem implements ISurveyMasterChoiceItem {
  public description = ''
  public group: SurveyMasterChoiceGroupItem[] = []
  public isNew = false
}

export class SurveyMasterQuestionFlowItem
  implements ISurveyMasterQuestionFlowItem
{
  public id = ''
  public conditionId = ''
  public conditionChoiceIds: string[] = []
}

export class SurveyMasterQuestionItem implements ISurveyMasterQuestionItem {
  public id = ''
  public required = false
  public description = ''
  public format = ''
  public summaryType = ''
  public choices: SurveyMasterChoiceItem[] = []
  public otherDescription = ''
  public disabled = false
  public isNew = false
}

export class CreateSurveyMastersRequest implements ICreateSurveyMastersRequest {
  public name = ''
  public type = ''
  public timing = ''
  public initSendDaySetting = 0
  public initAnswerLimitDaySetting = 0
  public isDisclosure = false
  public questions: SurveyMasterQuestionItem[] = []
  public questionFlow: SurveyMasterQuestionFlowItem[] = []
  public memo = ''
}

export class CreateSurveyMastersResponse
  implements ICreateSurveyMastersResponse
{
  public id = ''
  public revision = 0
  public name = ''
  public type = ''
  public timing = ''
  public initSendDaySetting = 0
  public initAnswerLimitDaySetting = 0
  public isDisclosure = false
  public questions: SurveyMasterQuestionItem[] = []
  public questionFlow: SurveyMasterQuestionFlowItem[] = []
  public isLatest = 0
  public memo = ''
  public createId = ''
  public createUserName = ''
  public createAt = ''
  public version = 0
}

export class GetSurveyMasterByIdAndRevisionResponse
  implements IGetSurveyMasterByIdAndRevisionResponse
{
  public id = ''
  public revision = 0
  public name = ''
  public type = ''
  public timing = ''
  public initSendDaySetting = 0
  public initAnswerLimitDaySetting = 0
  public isDisclosure = false
  public questions: SurveyMasterQuestionItem[] = []
  public questionFlow: SurveyMasterQuestionFlowItem[] = []
  public isLatest = 0
  public memo = ''
  public createId = ''
  public createUserName = ''
  public createAt = ''
  public updateId = ''
  public updateUserName = ''
  public updateAt = ''
  public version = 0
}

export class UpdateSurveyMasterLatestByIdRequest
  implements IUpdateSurveyMasterLatestByIdRequest
{
  public name = ''
  public type = ''
  public timing = ''
  public initSendDaySetting = 0
  public initAnswerLimitDaySetting = 0
  public isDisclosure = false
  public memo = ''
}

export class UpdateSurveyMasterLatestByIdResponse
  implements IUpdateSurveyMasterLatestByIdResponse
{
  public id = ''
  public revision = 0
  public name = ''
  public type = ''
  public timing = ''
  public initSendDaySetting = 0
  public initAnswerLimitDaySetting = 0
  public isDisclosure = false
  public isLatest = 0
  public memo = ''
  public updateId = ''
  public updateUserName = ''
  public updateAt = ''
  public version = 0
}

export async function GetSurveyMastersById(id: string) {
  return await $api.get<GetSurveyMastersByIdResponse>(`/survey-masters/${id}`)
}

export async function CopySurveyMasterById(id: string, originRevision: number) {
  return await $api.put(
    `/survey-masters/${id}?originRevision=${originRevision}`
  )
}

export async function PatchSurveyMasterRevisionById(
  id: string,
  version: number
) {
  return await $api.patch(`/survey-masters/${id}?version=${version}`)
}

export async function CreateSurveyMasters(request: CreateSurveyMastersRequest) {
  return await $api.post<CreateSurveyMastersResponse>(
    `/survey-masters`,
    request
  )
}

export async function GetSurveyMasterByIdAndRevision(
  id: string,
  revision: number
) {
  return await $api.get<GetSurveyMasterByIdAndRevisionResponse>(
    `/survey-masters/${id}/${revision}`
  )
}

export async function UpdateSurveyMasterLatestById(
  id: string,
  version: number,
  request: UpdateSurveyMasterLatestByIdRequest
) {
  return await $api.put<UpdateSurveyMasterLatestByIdResponse>(
    `/survey-masters/${id}/latest?version=${version}`,
    request
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

export class FactorSetting implements IFactorSetting {
  serviceTypeId = ''
  directSupportManHour = 0
  directAndPreSupportManHour = 0
  preSupportManHour = 0
  hourlyManHourPrice = 0
  monthlyProfit = 0
}
export class DirectSupportManHour implements IDirectSupportManHour {
  summaryDisplayProject = true
  summaryOverAlert = true
  summaryProspectAlert = true
  thisMonthDisplayProject = true
  thisMonthOverAlert = true
  thisMonthProspectAlert = true
}

export class PreSupportManHour implements IPreSupportManHour {
  summaryDisplayProject = true
  summaryOverAlert = true
  summaryProspectAlert = true
  thisMonthDisplayProject = true
  thisMonthOverAlert = true
  thisMonthProspectAlert = true
}
export class DisplaySetting implements IDisplaySetting {
  directSupportManHour: DirectSupportManHour = new DirectSupportManHour()
  preSupportManHour: PreSupportManHour = new PreSupportManHour()
}
export class AlertSettingAttributes implements IAlertSettingAttributes {
  factorSetting: FactorSetting[] = []
  displaySetting: IDisplaySetting = new DisplaySetting()
}

export class GetAlertSettingsResponse implements IGetAlertSettingResponse {
  id = ''
  name = ''
  attributes: AlertSettingAttributes = new AlertSettingAttributes()
  createId = ''
  createUserName = ''
  createAt = ''
  updateId = ''
  updateUserName = ''
  updateAt = ''
  version = 0
}

export async function GetAlertSettings() {
  return await $api.get<GetAlertSettingsResponse>(`/masters/alert-setting`)
}

export async function UpdateAlertSettings(
  version: number,
  request: UpdateAlertSettingsRequest
) {
  return await $api.put<UpdateAlertSettingsResponse>(
    `/masters/alert-setting?version=${version}`,
    request
  )
}

export class UpdateAlertSettingsRequest implements IUpdateAlertSettingsRequest {
  factorSetting = new FactorSetting()
  displaySetting? = new DisplaySetting()
}

export class UpdateAlertSettingsResponse
  implements IUpdateAlertSettingsResponse
{
  id = ''
  name = ''
  attributes = new AlertSettingAttributes()
  createId = ''
  createUserName = ''
  createAt = ''
  updateId = ''
  updateUserName = ''
  updateAt = ''
  version = 0
}

export class UpdateSurveyMasterDraftByIdRequest
  implements IUpdateSurveyMasterDraftByIdRequest
{
  public name = ''
  public timing = ''
  public initSendDaySetting = 0
  public initAnswerLimitDaySetting = 0
  public isDisclosure = false
  public questions: SurveyMasterQuestionItem[] = []
  public questionFlow: SurveyMasterQuestionFlowItem[] = []
  public memo = ''
}

export async function UpdateSurveyMasterDraftById(
  id: string,
  version: number,
  request: UpdateSurveyMasterDraftByIdRequest
) {
  return await $api.put(
    `/survey-masters/${id}/draft?version=${version}`,
    request
  )
}
