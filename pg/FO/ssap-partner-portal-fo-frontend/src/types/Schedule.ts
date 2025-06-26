//アンケート送信スケジュール型定義
export interface ISurveyProjectSchedule {
  scheduleGroupId: string
  surveyId: string
  sendDate: string
  surveyName: string
  surveyLimitDate: string
  isResendAnonymousSurvey: boolean
  version: number
}

//支援日スケジュール型定義
export interface ISupportProjectSchedule {
  yearMonth: string
  supportDate: string
  supportStartTime: string
  supportEndTime: string
  karteId: string
  isAccessibleKarteDetail: boolean
  lastUpdateDatetime: string
  updateUser: string
  version: number
  completed: boolean
}

//GetSurveySchedulesByIdレスポンス型定義
export interface IGetSurveySchedulesByIdResponse {
  projectId: string
  projectSchedules: ISurveyProjectSchedule[]
  surveyGroupId: string
}

//GetSupportSchedulesByIdレスポンス型定義
export interface IGetSupportSchedulesByIdResponse {
  projectId: string
  projectSchedules: ISupportProjectSchedule[]
}

//UpdateSupportSchedulesByIdAndDateリクエストパラメタ型定義
export interface IUpdateSupportSchedulesByIdAndDateRequestParams {
  karteId: string
  version: number
}

//UpdateSupportSchedulesByIdAndDateリクエスト型定義
export interface IUpdateSupportSchedulesByIdAndDateRequest {
  supportDate: string
  supportStartTime: string
  supportEndTime: string
}

//UpdateSupportSchedulesByIdAndDateレスポンスのprojectSchedules型定義
export interface IUpdateProjectSchedules {
  yearMonth: string
  scheduleId: string
  scheduleGroupId: string
  supportDate: string
  supportStartTime: string
  supportEndTime: string
  karteId: string
  version: number
  createId: string
  createAt: string
  updateId: string
  updateAt: string
}

//UpdateSupportSchedulesByIdAndDateレスポンス型定義
export interface IUpdateSupportSchedulesByIdAndDateResponse {
  projectId: string
  projectSchedules: IUpdateProjectSchedules
}

//CreateScheduleリクエスト型定義
export interface ICreateSupportScheduleRequest {
  timing: string
  supportDate: string
  startTime: string
  endTime: string
}

//CreateScheduleレスポンスのprojectSchedules型定義
// export interface ICreateSchedulesItems {
//   scheduleId: string
//   scheduleGroupId: string
//   day: string
//   type: string
//   completed: string
//   supportInfo: Object
//   surveyInfo: Object
// }

//CreateScheduleレスポンス型定義
export interface ICreateSupportSchedulesResponse {
  message: string
}

//CreateSurveySchedulesリクエスト型定義
export interface ICreateSurveySchedulesRequest {
  surveyType: string
  timing: string
  requestDate: number
  limitDate: number
}

//CreateSurveySchedulesレスポンス型定義
export interface ICreateSurveySchedulesResponse {
  message: string
}

//UpdateSurveySchedulesByIdAndDateリクエストパラメタ型定義
export interface IUpdateSurveySchedulesByIdAndDateRequestParams {
  surveyId: string
  version: number
}

//UpdateSurveySchedulesByIdAndDateリクエスト型定義
export interface IUpdateSurveySchedulesByIdAndDateRequest {
  sendDate: string
  surveyLimitDate: number
}

//UpdateSurveySchedulesByIdAndDateレスポンス型定義
export interface IUpdateSurveySchedulesByIdAndDateResponse {
  message: string
}

export interface IBulkDateInfo {
  requestDate: number
  limitDate: number
}

//BulkCreateSurveySchedulesRequestリクエスト型定義
export interface IBulkCreateSurveySchedulesRequest {
  service: Object
  completion: Object
}

//BulkCreateSurveySchedulesResponseレスポンス型定義
export interface IBulkCreateSurveySchedulesResponse {
  message: string
}

//BulkUpdateSurveySchedulesRequestリクエスト型定義
export interface IBulkUpdateSurveySchedulesRequest {
  service: Object
  completion: Object
}

//BulkUpdateSurveySchedulesRequestレスポンス型定義
export interface IBulkUpdateSurveySchedulesResponse {
  message: string
}

//DeleteSchedulesByIdAndDateリクエストパラメタ型定義
export interface IDeleteSchedulesByIdAndDateRequestParams {
  karteId: string
  version: number
}

//DeleteSchedulesByIdAndDateレスポンス型定義
export interface IDeleteSchedulesByIdAndDateResponse {
  message: string
}
