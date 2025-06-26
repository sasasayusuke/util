import {
  IGetSurveySchedulesByIdResponse,
  IGetSupportSchedulesByIdResponse,
  IUpdateSupportSchedulesByIdAndDateRequestParams,
  IUpdateSupportSchedulesByIdAndDateRequest,
  IUpdateSupportSchedulesByIdAndDateResponse,
  ICreateSupportScheduleRequest,
  ICreateSupportSchedulesResponse,
  ICreateSurveySchedulesResponse,
  ICreateSurveySchedulesRequest,
  IDeleteSchedulesByIdAndDateRequestParams,
  IDeleteSchedulesByIdAndDateResponse,
  IUpdateSurveySchedulesByIdAndDateRequestParams,
  IUpdateSurveySchedulesByIdAndDateRequest,
  IUpdateSurveySchedulesByIdAndDateResponse,
  IBulkCreateSurveySchedulesRequest,
  IBulkCreateSurveySchedulesResponse,
  IBulkUpdateSurveySchedulesRequest,
  IBulkUpdateSurveySchedulesResponse,
  ISurveyProjectSchedule,
  ISupportProjectSchedule,
  // ICreateSchedulesItems,
  IUpdateProjectSchedules,
  IBulkDateInfo,
} from '@/types/Schedule'

import { Api } from '~/plugins/api'

const $api = new Api()

export class SurveyProjectSchedule implements ISurveyProjectSchedule {
  public scheduleGroupId = ''
  public surveyId = ''
  public sendDate = ''
  public surveyName = ''
  public surveyLimitDate = ''
  public isResendAnonymousSurvey = true
  public version = 0
}

export class SupportProjectSchedule implements ISupportProjectSchedule {
  public yearMonth = ''
  public supportDate = ''
  public supportStartTime = ''
  public supportEndTime = ''
  public karteId = ''
  public isAccessibleKarteDetail = true
  public lastUpdateDatetime = ''
  public updateUser = ''
  public version = 0
  public completed = true
}

export class GetSurveySchedulesByIdResponse
  implements IGetSurveySchedulesByIdResponse
{
  public projectId = ''
  public projectSchedules: SurveyProjectSchedule[] = []
  public surveyGroupId = ''
}

export class GetSupportSchedulesByIdResponse
  implements IGetSupportSchedulesByIdResponse
{
  public projectId = ''
  public projectSchedules: SupportProjectSchedule[] = []
}

export class UpdateSupportSchedulesByIdAndDateRequestParams
  implements IUpdateSupportSchedulesByIdAndDateRequestParams
{
  public karteId = ''
  public version = 0
}

export class UpdateSupportSchedulesByIdAndDateRequest
  implements IUpdateSupportSchedulesByIdAndDateRequest
{
  public supportDate = ''
  public supportStartTime = ''
  public supportEndTime = ''
}

export class UpdateProjectSchedules implements IUpdateProjectSchedules {
  public yearMonth = ''
  public scheduleId = ''
  public scheduleGroupId = ''
  public supportDate = ''
  public supportStartTime = ''
  public supportEndTime = ''
  public karteId = ''
  public version = 0
  public createId = ''
  public createAt = ''
  public updateId = ''
  public updateAt = ''
}

export class UpdateSupportSchedulesByIdAndDateResponse
  implements IUpdateSupportSchedulesByIdAndDateResponse
{
  public projectId = ''
  public projectSchedules = new UpdateProjectSchedules()
}

export class CreateSupportScheduleRequest
  implements ICreateSupportScheduleRequest
{
  public timing = ''
  public supportDate = ''
  public startTime = ''
  public endTime = ''
}

export class CreateSupportScheduleResponse
  implements ICreateSupportSchedulesResponse
{
  public message = ''
}

export class CreateSurveySchedulesRequest
  implements ICreateSurveySchedulesRequest
{
  public surveyMasterId = ''
  public surveyType = ''
  public timing = ''
  public requestDate = 0
  public limitDate = 0
}

export class CreateSurveySchedulesResponse
  implements ICreateSurveySchedulesResponse
{
  public message = ''
}

export class UpdateSurveySchedulesByIdAndDateRequestParams
  implements IUpdateSurveySchedulesByIdAndDateRequestParams
{
  public surveyId = ''
  public version = 0
}

export class UpdateSurveySchedulesByIdAndDateRequest
  implements IUpdateSurveySchedulesByIdAndDateRequest
{
  public sendDate = ''
  public surveyLimitDate = 0
}

export class UpdateSurveySchedulesByIdAndDateResponse
  implements IUpdateSurveySchedulesByIdAndDateResponse
{
  public message = ''
}

export class BulkDateInfo implements IBulkDateInfo {
  public requestDate = 0
  public limitDate = 0
}

export class BulkCreateSurveySchedulesRequest
  implements IBulkCreateSurveySchedulesRequest
{
  public service = BulkDateInfo
  public completion = BulkDateInfo
}

export class BulkCreateSurveySchedulesResponse
  implements IBulkCreateSurveySchedulesResponse
{
  public message = ''
}

export class BulkUpdateSurveySchedulesRequest
  implements IBulkUpdateSurveySchedulesRequest
{
  public service = BulkDateInfo
  public completion = BulkDateInfo
}

export class BulkUpdateSurveySchedulesResponse
  implements IBulkUpdateSurveySchedulesResponse
{
  public message = ''
}

export class DeleteSupportSchedulesByIdAndDateRequestParams
  implements IDeleteSchedulesByIdAndDateRequestParams
{
  public karteId = ''
  public version = 0
}

export class DeleteSupportSchedulesByIdAndDateResponse
  implements IDeleteSchedulesByIdAndDateResponse
{
  public message = ''
}

export class DeleteSurveySchedulesByIdAndDateResponse
  implements IDeleteSchedulesByIdAndDateResponse
{
  public message = ''
}

//GetSurveySchedulesById実行関数
export async function GetSurveySchedulesById(id: string) {
  return await $api.get<GetSurveySchedulesByIdResponse>(
    `/schedules/survey/${id}`
  )
}

//GetSupportSchedulesById実行関数
export async function GetSupportSchedulesById(id: string) {
  return await $api.get<GetSupportSchedulesByIdResponse>(
    `/schedules/support/${id}`
  )
}

//UpdateSupportSchedulesByIdAndDate実行関数
export async function UpdateSupportSchedulesByIdAndDate(
  karteId: string,
  version: string,
  request: UpdateSupportSchedulesByIdAndDateRequest
) {
  return await $api.put<UpdateSupportSchedulesByIdAndDateResponse>(
    `/schedules/support?karteId=${karteId}&version=${version}`,
    request
  )
}

//UpdateSurveySchedulesByIdAndDate実行関数
export async function UpdateSurveySchedulesByIdAndDate(
  surveyId: string,
  version: string,
  request: UpdateSurveySchedulesByIdAndDateRequest
) {
  return await $api.put<UpdateSurveySchedulesByIdAndDateResponse>(
    `schedules/survey?surveyId=${surveyId}&version=${version}`,
    request
  )
}

//BulkSurveySchedules実行関数
export async function BulkCreateSurveySchedules(
  id: string,
  request: BulkCreateSurveySchedulesRequest
) {
  return await $api.post<BulkCreateSurveySchedulesResponse>(
    `schedules/survey/bulk/${id}`,
    request
  )
}

//BulkUpdateSurveySchedules実行関数
export async function BulkUpdateSurveySchedules(
  id: string,
  request: BulkUpdateSurveySchedulesRequest
) {
  return await $api.put<BulkUpdateSurveySchedulesResponse>(
    `schedules/survey/bulk/${id}`,
    request
  )
}

//CreateSurveySchedules実行関数
export async function CreateSurveySchedules(
  id: string,
  request: CreateSurveySchedulesRequest
) {
  return await $api.post<CreateSurveySchedulesResponse>(
    `/schedules/survey/${id}`,
    request
  )
}

//CreateSupportSchedules実行関数
export async function CreateSupportSchedules(
  id: string,
  request: CreateSupportScheduleRequest
) {
  return await $api.post<CreateSupportScheduleResponse>(
    `/schedules/support/${id}`,
    request
  )
}

//DeleteSupportSchedulesByIdAndDate実行関数
export async function DeleteSupportSchedulesByIdAndDate(
  karteId: string,
  version: number
) {
  return await $api.delete<DeleteSupportSchedulesByIdAndDateResponse>(
    `/schedules/support?karteId=${karteId}&version=${version}`
  )
}

//DeleteSurveySchedulesByIdAndDate実行関数
export async function DeleteSurveySchedulesByIdAndDate(
  surveyId: string,
  version: number
) {
  return await $api.delete<DeleteSurveySchedulesByIdAndDateResponse>(
    `/schedules/survey?surveyId=${surveyId}&version=${version}`
  )
}
