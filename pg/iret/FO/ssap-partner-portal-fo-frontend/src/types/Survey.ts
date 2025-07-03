export const ENUM_SURVEY_SUMMARY = {
  SATISFACTION: 'satisfaction',
  CONTINUATION: 'continuation',
  RECOMMENDED: 'recommended',
  SALES: 'sales',
  SERVICEOVERALLSATISFACTION: 'serviceOverallSatisfaction',
  COMPLETIONOVERALLSATISFACTION: 'completionOverallSatisfaction',
  EVALUATION: 'evaluation',
} as const

export type SurveySummary =
  typeof ENUM_SURVEY_SUMMARY[keyof typeof ENUM_SURVEY_SUMMARY]

// アンケート一覧検索パラメタ型定義
export interface ISurveySearchParam {
  type: string
  enbFromTo: boolean
  dateFrom: string
  dateTo: string
  limit: number
}

// GetSurveysリクエスト型定義
export interface IGetSurveysRequest {
  summaryMonth?: number
  summaryMonthFrom?: number
  summaryMonthTo?: number
  projectId?: string
  type?: string
  planSurveyResponseDateFrom?: string
  planSurveyResponseDateTo?: string
  isDisclosure?: boolean
}

export interface IMainSupporterUser {
  id: string
  name: string
  organizationId: string
  organizationName: string
}

// 支援者型定義
export interface ISupporterUser {
  id: string
  name: string
  organizationId: string
  organizationName: string
}

// 回答型定義
export interface IAnswerInfo {
  id: string
  answer: string
  point: number
  choiceIds: Array<string>
  summaryType: string
  otherInput: string
}

// 設問の選択肢グループ型定義
export interface IQuestionChoiceGroupInfo {
  id: string
  title: string
  disabled: boolean
  isNew: boolean
}

// 設問の選択肢型定義
export interface IQuestionChoiceInfo {
  description: string
  group: Array<IQuestionChoiceGroupInfo>
  isNew: boolean
}

// 設問型定義
export interface IQuestionInfo {
  id: string
  required: boolean
  description: string
  format: string
  summaryType: string
  choices: Array<IQuestionChoiceInfo>
  otherDescription: string
  disabled: boolean
  isNew: boolean
}

// 出題フロー型定義
export interface IQuestionFlowInfo {
  id: string
  conditionId: string
  conditionChoiceIds: Array<string>
}

/*  アンケート一覧情報型定義
    GetSurveysとGetSurveysByMineどちらの応答も格納できるようにする
    未使用のデータは省略可とする
*/
export interface ISurveyListItem {
  id: string
  surveyMasterId?: string
  surveyRevision?: number
  surveyType: string
  projectId?: string
  projectName: string
  customerSuccess?: string
  supporterOrganizationId?: string
  supporterOrganizationName?: string
  supportDateFrom?: string
  supportDateTo?: string
  mainSupporterUser?: ISupporterUser
  supporterUsers?: Array<ISupporterUser>
  salesUserId?: string
  salesUserName?: string
  serviceTypeId?: string
  serviceTypeName?: string
  answerUserId?: string
  answerUserName: string
  customerId?: string
  customerName: string
  company: string
  summaryMonth?: string
  isNotSummary?: boolean
  planSurveyRequestDatetime?: string
  actualSurveyRequestDatetime: string
  planSurveyResponseDatetime: string
  actualSurveyResponseDatetime: string
  isFinished: boolean
  isDisclosure?: boolean
  isAnswerUserName?: boolean
}

// アンケート詳細型定義
export interface ISurveyInfo {
  id: string
  surveyMasterId: string
  surveyRevision: number
  surveyType: string
  projectId: string
  projectName: string
  customerSuccess: string
  supporterOrganizationId: string
  supporterOrganizationName: string
  supportDateFrom: string
  supportDateTo: string
  mainSupporterUser: ISupporterUser
  supporterUsers: Array<ISupporterUser>
  salesUserId: string
  salesUserName: string
  serviceTypeId: string
  serviceTypeName: string
  answerUserId?: string
  answerUserName?: string
  customerId: string
  customerName: string
  company: string
  answers: Array<IAnswerInfo>
  summaryMonth: string
  isNotSummary: boolean
  isSolverProject: boolean
  planSurveyRequestDatetime: string
  actualSurveyRequestDatetime: string
  planSurveyResponseDatetime: string
  actualSurveyResponseDatetime: string
  isFinished: boolean
  isDisclosure: boolean
  createId: string
  createUserName?: string
  createAt: string
  updateId: string
  updateUserName?: string
  updateAt: string
  version: number
}

// アンケートマスタ型定義
export interface ISurveyMasterInfo {
  id: string
  revision: number
  name: string
  type: string
  timing: string
  initSendDaySetting: number
  initAnswerLimitDaySetting: number
  isDisclosure: boolean
  questions: Array<IQuestionInfo>
  questionFlow: Array<IQuestionFlowInfo>
  isLatest: boolean
  updateId: string
  updateAt: string
  version: number
}

// 満足度評価のみアンケートマスタ型定義
export interface ISurveyMasterSatisfactionInfo {
  id: string
  projectName: string
  customerName: string
  questions: IQuestionInfo
  version: number
}

// 回答入力情報型定義
export interface IInputAnswerInfo {
  answer: string
  otherAnswer: string
  isAnswered: boolean
}

export interface ISurveyListSummaryItem {
  month: string
  satisfactionAverage: number
  receive: number
}

export interface ISurveyListSummaryTotalItem {
  satisfactionAverage: number
  receive: number
}

export interface ISurveyListSummaryTotalOuota {
  quota1: ISurveyListSummaryTotalItem
  quota2: ISurveyListSummaryTotalItem
  quota3: ISurveyListSummaryTotalItem
  quota4: ISurveyListSummaryTotalItem
}
export interface ISurveyListSummaryTotalHalf {
  half1: ISurveyListSummaryTotalItem
  half2: ISurveyListSummaryTotalItem
}

export interface ISurveyListSummaryTotal {
  quota: ISurveyListSummaryTotalOuota
  half: ISurveyListSummaryTotalHalf
  year: ISurveyListSummaryTotalItem
}

export interface ISurveyListSummary {
  monthly: ISurveyListSummaryItem[]
  accumulation: ISurveyListSummaryItem[]
  total: ISurveyListSummaryTotal
}

// GetSurveysレスポンス型定義
export interface IGetSurveysResponse {
  summary: ISurveyListSummary
  surveys: ISurveyListItem[]
}

// UpdateSurveyByIdリクエスト型定義
export interface IUpdateSurveyByIdRequest {
  answers: Array<IAnswerInfo>
  isFinished: boolean
  isDisclosure: boolean
}

export const ENUM_GET_SURVEYS_BY_MINE_TYPE = {
  SERVICE: 'service',
  COMPLETION: 'completion',
  SERVICE_AND_COMPLETION: 'service_and_completion',
  QUICK: 'quick',
  PP: 'pp',
  NON_PP: 'non_pp',
} as const

export type GetSurveysByMineType =
  typeof ENUM_GET_SURVEYS_BY_MINE_TYPE[keyof typeof ENUM_GET_SURVEYS_BY_MINE_TYPE]

export const ENUM_GET_SURVEYS_BY_MINE_SORT = {
  ACTUAL_SURVEY_REQUEST_DATETIME_DESC: 'actual_survey_request_datetime:desc',
  PLAN_SURVEY_RESPONSE_DATETIME_DESC: 'plan_survey_response_datetime:desc',
} as const

export type GetSurveysByMineSort =
  typeof ENUM_GET_SURVEYS_BY_MINE_SORT[keyof typeof ENUM_GET_SURVEYS_BY_MINE_SORT]

export interface IGetSurveysByMineRequest {
  dateFrom?: Number
  dateTo?: Number
  type?: GetSurveysByMineType
  sort: GetSurveysByMineSort
  projectId?: string
  limit?: Number
  isFinished?: boolean
}

class SupporterUser {
  id: string = ''
  name: string = ''
  organizationId: string = ''
  organizationName: string = ''
}

interface IAnswer {
  id: string
  answer: string
  point: number
  choiceIds: string[]
  summaryType: string
  otherInput: string
}

class Answer implements IAnswer {
  id = ''
  answer = ''
  point = 0
  choiceIds = [] as string[]
  summaryType = ''
  otherInput = ''
}

interface ISurvey {
  id: string
  surveyMasterId: string
  surveyRevision: number
  surveyType: string
  projectId: string
  projectName: string
  customerSuccess: string
  supporterOrganizationId: string
  supporterOrganizationName: string
  supportDateFrom: string
  supportDateTo: string
  mainSupporterUser: SupporterUser
  supporterUsers: SupporterUser[]
  salesUserId: string
  salesUserName: string
  serviceTypeId: string
  serviceTypeName: string
  answerUserId: string
  answerUserName: string
  customerId: string
  customerName: string
  company: string
  answers: Answer[]
  summaryMonth: string
  isNotSummary: boolean
  planSurveyRequestDatetime: string
  actualSurveyRequestDatetime: string
  planSurveyResponseDatetime: string
  actualSurveyResponseDatetime: string
  isFinished: boolean
  isDisclosure: boolean
  createId: string
  createUserName: string
  createAt: string
  updateId: string
  updateUserName: string
  updateAt: string
  version: number
}

export class Survey implements ISurvey {
  id = ''
  surveyMasterId = ''
  surveyRevision = 10
  surveyType = ''
  projectId = ''
  projectName = ''
  customerSuccess = ''
  supporterOrganizationId = ''
  supporterOrganizationName = ''
  supportDateFrom = ''
  supportDateTo = ''
  mainSupporterUser = new SupporterUser()
  supporterUsers = [] as SupporterUser[]
  salesUserId = ''
  salesUserName = ''
  serviceTypeId = ''
  serviceTypeName = ''
  answerUserId = ''
  answerUserName = ''
  customerId = ''
  customerName = ''
  company = ''
  answers = [] as Answer[]
  summaryMonth = ''
  isNotSummary = false
  planSurveyRequestDatetime = ''
  actualSurveyRequestDatetime = ''
  planSurveyResponseDatetime = ''
  actualSurveyResponseDatetime = ''
  isFinished = true
  isDisclosure = true
  createId = ''
  createUserName = ''
  createAt = ''
  updateId = ''
  updateUserName = ''
  updateAt = ''
  version = 0
}

export interface IGetSurveysByMineResponse {
  total: number
  surveys: ISurveyListItem[]
}

export interface ICompletionContinuation {
  positiveCount: number
  negativeCount: number
  positivePercent: number
}

export interface ISupporterOrganizationsTermSummaryResultItem {
  supporterOrganizationId: string
  supporterOrganizationName: string
  serviceSatisfactionSummary: number
  serviceSatisfactionAverage: number
  serviceReceive: number
  completionSatisfactionSummary: number
  completionSatisfactionAverage: number
  completionContinuation: ICompletionContinuation
  completionRecommendedSummary: number
  completionRecommendedAverage: number
  completionReceive: number
  totalSatisfactionSummary: number
  totalSatisfactionAverage: number
  totalReceive: number
}

export interface ITermSummaryResultService {
  satisfactionAverage: number
  totalReceive: number
}

export interface ITermSummaryResultCompletion {
  satisfactionAverage: number
  continuationPositivePercent: number
  recommendedAverage: number
  salesAverage: number
  totalReceive: number
}

export interface ITermSummaryResultServiceAndCompletion {
  satisfactionAverage: number
  totalReceive: number
}

export interface ITermSummaryResultQuick {
  totalReceive: number
}

export interface ITermSummaryResultPp {
  surveySatisfactionAverage: number
  manHourSatisfactionAverage: number
  karteSatisfactionAverage: number
  totalReceive: number
}

export interface ITermSummaryResultItem {
  service: ITermSummaryResultService
  completion: ITermSummaryResultCompletion
  serviceAndCompletion: ITermSummaryResultServiceAndCompletion
  quick: ITermSummaryResultQuick
  pp: ITermSummaryResultPp
}

export interface ISupporterOrganizationsItem {
  supporterOrganizationName: string
  serviceSatisfactionAverage: number
  serviceReceive: number
  completionSatisfactionAverage: number
  completionContinuationPercent: number
  completionRecommendedAverage: number
  completionReceive: number
  completionSalesAverage: number
  completionSalesSummary: number
  totalSatisfactionAverage: number
  totalReceive: number
}

export interface ISupporterOrganizationSurveysItem {
  summaryMonth: string
  supporterOrganizations: ISupporterOrganizationsItem[]
}

export interface IGetSurveySummarySupporterOrganizationsByMineRequest {
  yearMonthFrom: number
  yearMonthTo: number
}

export interface IGetSurveySummarySupporterOrganizationsByMineResponse {
  termSummaryResult: ISupporterOrganizationsTermSummaryResultItem[]
  surveys: ISupporterOrganizationSurveysItem[]
}

export interface ISurveySummaryItem {
  summaryMonth: string
  serviceSatisfactionSummary: number
  serviceSatisfactionAverage: number
  serviceReceive: number
  completionSatisfactionSummary: number
  completionSatisfactionAverage: number
  completionContinuation: ICompletionContinuation
  completionRecommendedSummary: number
  completionRecommendedAverage: number
  completionReceive: number
  quickReceive: number
}

export interface IGetSurveySummaryByMineRequest {
  yearMonthFrom: number
  yearMonthTo: number
}

export interface IGetSurveySummaryByMineResponse {
  termSummaryResult: ITermSummaryResultItem
  surveys: ISurveySummaryItem[]
}

export interface ISurveyByMineItem {
  id: string
  surveyMasterId: string
  surveyRevision: number
  surveyType: string
  projectId: string
  projectName: string
  customerSuccess: string
  supporterOrganizationId: string
  supporterOrganizationName: string
  supportDateFrom: string
  supportDateTo: string
  mainSupporterUser: ISupporterUser
  supporterUsers: Array<ISupporterUser>
  salesUserId: string
  salesUserName: string
  serviceTypeId: string
  serviceTypeName: string
  answerUserId: string
  answerUserName: string
  customerId: string
  customerName: string
  company: string
  summaryMonth: string
  isNotSummary: boolean
  planSurveyRequestDatetime: string
  actualSurveyRequestDatetime: string
  planSurveyResponseDatetime: string
  actualSurveyResponseDatetime: string
  isFinished: boolean
  isDisclosure: boolean
}

export interface ICheckSurveyByIdPasswordRequest {
  token: string
  password: string
}

export interface ICheckSurveyByIdPasswordResponse {
  id: string
}

export interface ICheckAndGetSurveyAnonymousByIdRequest {
  token: string
  password: string
}

export interface ICheckAndUpdateSurveyAnonymousByIdRequest {
  token: string
  password: string
  answers: Array<IAnswerInfo>
  isFinished: boolean
  isDisclosure: boolean
}

export interface ICheckAndGetSurveyMasterAnonymousByIdAndRevisionRequest {
  token: string
  password: string
}

export interface IGetSurveyOfSatisfactionByIdRequest {
  token: string
}

export interface IGetSurveyOfSatisfactionByIdResponse {
  surveyId: string
  isFinished: boolean
  surveyMasterId: string
  surveyRevision: number
  projectId: string
  projectName: string
  customerName: string
  version: number
}

export interface IUpdateSurveyOfSatisfactionByIdRequest {
  token: string
  answers: Array<IAnswerInfo>
  isFinished: boolean
  isDisclosure: boolean
}

export interface IGetSurveyMasterSatisfactionByIdAndRevisionRequest {
  token: string
}
