export interface IMasterListItem {
  id: string
  dataType: string
  name: string
  value: string
  use: boolean
  createAt: string
  updateAt: string
}

export interface ISupporterOrganizationsResponse {
  id: string
  name: string
  shortName: string
}

export interface IGetMastersRequest {
  dataType: string
  limit: number
  offsetPage: number
}
export interface IGetMastersResponse {
  offsetPage: number
  total: number
  masters: IMasterListItem[]
}
export interface IGetMasterByIdRequest {
  id: string
}

export interface IAttributes {
  info1: string
  info2: string
  info3: string
  info4: string
  info5: string
}
export interface IGetMasterByIdResponse {
  id: string
  dataType: string
  name: string
  value: string
  attributes: IAttributes
  use: boolean
  createAt: string
  updateAt: string
  version: number
}

export interface ICreateMasterRequest {
  dataType: string
  name: string
  value: string
  attributes: IAttributes
  use: boolean
}

export interface ICreateMasterResponse {
  id: string
  dataType: string
  name: string
  value: string
  attributes: IAttributes
  use: boolean
}

export const ENUM_CREATE_MASTER_RESPONSE_ERROR = {
  ALREADY_EXIST: 'data type and value is already exist.',
} as const

export interface IUpdateMasterRequestParams {
  id: string
  version: number
}

export interface IUpdateMasterRequest {
  name: string
  value: string
  attributes: IAttributes
  order: number
  use: boolean
  maintenance: boolean
}

export interface IUpdateMasterResponse {
  name: string
  value: string
  attributes: IAttributes
  use: boolean
}

export interface ISurveyMasterListItem {
  id: string
  revision: number
  name: string
  type: string
  timing: string
  initSendDaySetting: number
  initAnswerLimitDaySetting: number
  isDisclosure: boolean
  isLatest: number
  memo: string
  status: string
  questionCount: number
  updateAt: string
}

export interface IGetSurveyMastersRequest {
  name: string
  latest: boolean
}

export interface IGetSurveyMastersResponse {
  total: number
  masters: ISurveyMasterListItem[]
}

export interface IServiceTypeItems {
  id: string
  name: string
}

export interface IGetServiceTypesResponse {
  serviceTypes: IServiceTypeItems[]
}

export interface ISupporterOrganizationItems {
  id: string
  name: string
  shortName?: string
}

export interface IGetSupporterOrganizationsResponse {
  supporterOrganizations: ISupporterOrganizationItems[]
}

export interface IGetSurveyMastersByIdResponse {
  total: number
  masters: ISurveyMasterListItem[]
}

export interface ICopySurveyMasterByIdResponse {
  status: string
  revision: number
  memo: string
  questionsCont: number
  updateAt: string
}

export interface IPatchSurveyMasterRevisionByIdResponse {
  status: string
  revision: number
  memo: string
  questionsCont: number
  updateAt: string
}

export interface ISurveyMasterChoiceGroupItem {
  id: string
  title: string
  disabled: boolean
  isNew: boolean
}

export interface ISurveyMasterChoiceItem {
  description: string
  group: ISurveyMasterChoiceGroupItem[]
  isNew: boolean
}

export interface ISurveyMasterQuestionFlowItem {
  id: string
  conditionId: string
  conditionChoiceIds: string[]
}

export interface ISurveyMasterQuestionItem {
  id: string
  required: boolean
  description: string
  format: string
  summaryType: string
  choices: ISurveyMasterChoiceItem[]
  otherDescription: string
  disabled: boolean
  isNew: boolean
}

export interface ICreateSurveyMastersRequest {
  name: string
  type: string
  timing: string
  initSendDaySetting: number
  initAnswerLimitDaySetting: number
  isDisclosure: boolean
  questions: ISurveyMasterQuestionItem[]
  questionFlow: ISurveyMasterQuestionFlowItem[]
  memo: string
}
export const ENUM_GET_SURVEY_MASTERS_IS_LATEST = {
  TRUE: 1,
  FALSE: 0,
} as const

export type GetSurveyMastersIsLatest =
  typeof ENUM_GET_SURVEY_MASTERS_IS_LATEST[keyof typeof ENUM_GET_SURVEY_MASTERS_IS_LATEST]
export interface ICreateSurveyMastersResponse {
  id: string
  revision: number
  name: string
  type: string
  timing: string
  initSendDaySetting: number
  initAnswerLimitDaySetting: number
  isDisclosure: boolean
  questions: ISurveyMasterQuestionItem[]
  questionFlow: ISurveyMasterQuestionFlowItem[]
  isLatest: number
  memo: string
  createId: string
  createUserName: string
  createAt: string
  version: number
}

export interface IGetSurveyMasterByIdAndRevisionResponse {
  id: string
  revision: number
  name: string
  type: string
  timing: string
  initSendDaySetting: number
  initAnswerLimitDaySetting: number
  isDisclosure: boolean
  questions: ISurveyMasterQuestionItem[]
  questionFlow: ISurveyMasterQuestionFlowItem[]
  isLatest: number
  memo: string
  createId: string
  createUserName: string
  createAt: string
  updateId: string
  updateUserName: string
  updateAt: string
  version: number
}

export interface IUpdateSurveyMasterLatestByIdRequest {
  name: string
  type: string
  timing: string
  initSendDaySetting: number
  initAnswerLimitDaySetting: number
  isDisclosure: boolean
  memo: string
}

export interface IUpdateSurveyMasterLatestByIdResponse {
  id: string
  revision: number
  name: string
  type: string
  timing: string
  initSendDaySetting: number
  initAnswerLimitDaySetting: number
  isDisclosure: boolean
  isLatest: number
  memo: string
  updateId: string
  updateUserName: string
  updateAt: string
  version: number
}
export interface IGetBatchControlByIdResponse {
  batchEndAt: string
}

export interface IPreSupportManHour {
  summaryDisplayProject: boolean
  summaryOverAlert: boolean
  summaryProspectAlert: boolean
  thisMonthDisplayProject: boolean
  thisMonthOverAlert: boolean
  thisMonthProspectAlert: boolean
}

export interface IDirectSupportManHour {
  summaryDisplayProject: boolean
  summaryOverAlert: boolean
  summaryProspectAlert: boolean
  thisMonthDisplayProject: boolean
  thisMonthOverAlert: boolean
  thisMonthProspectAlert: boolean
}

export interface IDisplaySetting {
  directSupportManHour: IDirectSupportManHour
  preSupportManHour: IPreSupportManHour
}

export interface IFactorSetting {
  serviceTypeId: string
  directSupportManHour: number
  directAndPreSupportManHour: number
  preSupportManHour: number
  hourlyManHourPrice: number
  monthlyProfit: number
}
export interface IAlertSettingAttributes {
  factorSetting: IFactorSetting[]
  displaySetting: IDisplaySetting
}
export interface IUpdateAlertSettingsRequest {
  factorSetting: IFactorSetting
  displaySetting?: IDisplaySetting
}

export interface IUpdateAlertSettingsResponse {
  id: string
  name: string
  attributes: IAlertSettingAttributes
  createId: string
  createUserName: string
  createAt: string
  updateId: string
  updateUserName: string
  updateAt: string
  version: number
}

export interface IUpdateSurveyMasterDraftByIdRequest {
  name: string
  timing: string
  initSendDaySetting: number
  initAnswerLimitDaySetting: number
  isDisclosure: boolean
  questions: ISurveyMasterQuestionItem[]
  questionFlow: ISurveyMasterQuestionFlowItem[]
  memo: string
}

export interface IGetAlertSettingResponse {
  id: string
  name: string
  attributes: IAlertSettingAttributes
  createId: string
  createUserName: string
  createAt: string
  updateId: string
  updateUserName: string
  updateAt: string
  version: number
}

export const INIT_SENT_DAY_SETTING_TYPE = {
  dayEveryMonth: 1,
  none: 2,
  daysBeforeDeadline: 3,
} as const

// eslint-disable-next-line no-redeclare
export type INIT_SENT_DAY_SETTING_TYPE =
  typeof INIT_SENT_DAY_SETTING_TYPE[keyof typeof INIT_SENT_DAY_SETTING_TYPE]
