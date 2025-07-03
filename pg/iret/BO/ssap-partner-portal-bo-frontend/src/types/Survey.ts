interface IGetSurveysRequestSortDefine {
  ACTUAL_SURVEY_RESPONSE_DATETIME_DESC: string
  PLAN_SURVEY_RESPONSE_DATETIME_DESC: string
  [key: string]: string
}

export const GET_SURVEYS_REQUEST_SORT: IGetSurveysRequestSortDefine = {
  ACTUAL_SURVEY_RESPONSE_DATETIME_DESC: 'actual_survey_response_datetime:desc',
  PLAN_SURVEY_RESPONSE_DATETIME_DESC: 'plan_survey_response_datetime:desc',
}

export interface IGetSurveysRequest {
  summaryMonthFrom?: number
  summaryMonthTo?: number
  actualSurveyResponseDateFrom?: number
  actualSurveyResponseDateTo?: number
  planSurveyResponseDateFrom?: number
  planSurveyResponseDateTo?: number
  projectId?: string
  customerId?: string
  isFinished?: boolean
  type?: string
  organizationIds?: string
  sort?: string
  limit?: number
  offsetPage?: number
  serviceTypeId?: string
}

export const ENUM_GET_SURVEY_TYPE = {
  SERVICE: 'service',
  COMPLETION: 'completion',
  QUICK: 'quick',
  PP: 'pp',
} as const

export interface IMainSupporterUser {
  id: string
  name: string
  organizationId: string
  organizationName: string
}

export interface ISupporterUser {
  id: string
  name: string
  organizationId: string
  organizationName: string
}

export interface ISurveyListItem {
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
  mainSupporterUser: IMainSupporterUser
  supporterUsers: ISupporterUser[]
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

export interface IGetSurveysResponse {
  summary: ISurveyListSummary
  surveys: ISurveyListItem[]
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
  masterKarteSatisfactionAverage: number
  totalReceive: number
}

export interface ITermSummaryResultItem {
  service: ITermSummaryResultService
  completion: ITermSummaryResultCompletion
  serviceAndCompletion: ITermSummaryResultServiceAndCompletion
  quick: ITermSummaryResultQuick
  pp: ITermSummaryResultPp
}

export interface ISurveySummaryService {
  projectCount: number
  sendCount: number
  receiveCount: number
  receivePercent: number
  satisfactionSummary: number
  satisfactionAverage: number
}

export interface ISurveySummaryCompletionContinuation {
  positiveCount: number
  negativeCount: number
  positivePercent: number
}

export interface ISurveySummaryCompletion {
  projectCount: number
  sendCount: number
  receiveCount: number
  receivePercent: number
  satisfactionSummary: number
  satisfactionAverage: number
  continuation: ISurveySummaryCompletionContinuation
  recommendedSummary: number
  recommendedAverage: number
  salesSummary: number
  salesAverage: number
}

export interface ISurveySummaryKarte {
  projectCount: number
  karteCount: number
  usePercent: number
}

export interface ISurveySummaryQuick {
  sendCount: number
  receiveCount: number
  receivePercent: number
}

export interface ISurveySummaryPp {
  surveySatisfactionSummary: number
  surveySatisfactionAverage: number
  manHourSatisfactionSummary: number
  manHourSatisfactionAverage: number
  karteSatisfactionSummary: number
  karteSatisfactionAverage: number
  masterKarteSatisfactionSummary: number
  masterKarteSatisfactionAverage: number
  sendCount: number
  receiveCount: number
  receivePercent: number
}

export interface ISurveySummaryItem {
  summaryMonth: string
  service: ISurveySummaryService
  completion: ISurveySummaryCompletion
  karte: ISurveySummaryKarte
  quick: ISurveySummaryQuick
  pp: ISurveySummaryPp
}

export interface IGetSurveySummaryAllRequest {
  yearMonthFrom: number
  yearMonthTo: number
}

export interface IGetSurveySummaryAllResponse {
  termSummaryResult: ITermSummaryResultItem
  surveys: ISurveySummaryItem[]
}

//
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

export interface ISupporterOrganizationsItem {
  supporterOrganizationName: string
  serviceSatisfactionAverage: number
  serviceReceive: number
  completionSatisfactionAverage: number
  completionContinuationPercent: number
  completionRecommendedAverage: number
  completionReceive: number
  totalSatisfactionAverage: number
  totalReceive: number
}

export interface ISupporterOrganizationSurveysItem {
  summaryMonth: string
  supporterOrganizations: ISupporterOrganizationsItem[]
}

export interface IGetSurveySummarySupporterOrganizationsRequest {
  yearMonthFrom: number
  yearMonthTo: number
}

export interface IGetSurveySummarySupporterOrganizationsResponse {
  termSummaryResult: ISupporterOrganizationsTermSummaryResultItem[]
  surveys: ISupporterOrganizationSurveysItem[]
}

export interface IExportSurveysRequest {
  summaryMonth?: number
  summaryMonthFrom?: number
  summaryMonthTo?: number
  planSurveyRequestDateFrom?: number
  planSurveyRequestDateTo?: number
  projectId?: string
  customerId?: string
  type?: string
  organizationIds?: string
  mode: string
}

export interface IExportSurveysResponse {
  url: string
}

export interface IAnswerInfo {
  id: string
  answer: string
  point: number
  choiceIds: Array<string>
  summaryType: string
  otherInput: string
}

export interface IGetSurveyByIdResponse {
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
  mainSupporterUser: IMainSupporterUser
  supporterUsers: ISupporterUser[]
  salesUserId: string
  salesUserName: string
  serviceTypeId: string
  serviceTypeName: string
  answerUserId: string
  answerUserName: string
  customerId: string
  customerName: string
  company: string
  answers: IAnswerInfo[]
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
  createUserName: string
  createAt: string
  updateId: string
  updateUserName: string
  updateAt: string
  version: number
}

export interface IUpdateSurveyByIdRequest {
  summaryMonth: string
  isNotSummary: boolean
  isSolverProject: boolean
}

export interface ISummaryReportItem {
  plan: number
  collect: number
  percent: number
  summaryPlan: number
  summaryCollect: number
  summaryPercent: number
}

export interface IGetSurveySummaryReportRequest {
  summaryMonth?: number
}

export interface IGetSurveySummaryReportResponse {
  summaryMonth: string
  service: ISummaryReportItem
  completion: ISummaryReportItem
  serviceAndCompletion: ISummaryReportItem
  pp: ISummaryReportItem
}

export interface IGetSurveyPlansRequest {
  summaryMonthFrom?: number
  summaryMonthTo?: number
  planSurveyResponseDateFrom?: number
  planSurveyResponseDateTo?: number
}

export interface ISurveyPlanListItem {
  projectId: string
  projectName: string
  id: string
  surveyType: string
  customerSuccess: string
  supporterOrganizationId: string
  supporterOrganizationName: string
  supportDateFrom: string
  supportDateTo: string
  mainSupporterUser: IMainSupporterUser
  supporterUsers: ISupporterUser[]
  salesUserId: string
  salesUserName: string
  serviceTypeId: string
  serviceTypeName: string
  surveyUserType: string
  surveyUserName: string
  surveyUserEmail: string
  customerId: string
  customerName: string
  summaryMonth: string
  planSurveyRequestDatetime: string
  actualSurveyRequestDatetime: string
  planSurveyResponseDatetime: string
  actualSurveyResponseDatetime: string
  phase: string
  isCountManHour: boolean
  unansweredSurveysNumber: number
  thisMonthType: string
  noSendReason: string
}
export interface IGetSurveyPlansResponse {
  surveys: ISurveyPlanListItem[]
}

export const ENUM_GET_SURVEY_SERVICE_TYPE_IDX = {
  SATISFACTION: 2,
} as const

export const ENUM_GET_SURVEY_COMPLETION_TYPE_IDX = {
  SATISFACTION: 2,
  CONTINUATION: 3,
  RECOMMENDED: 4,
  SALES: 5,
} as const

export const ENUM_GET_SURVEY_PP_TYPE_IDX = {
  SURVEY_SATISFACTION: 2,
  MAN_HOUR_SATISFACTION: 3,
  KARTE_SATISFACTION: 4,
  MASTER_KARTE_SATISFACTION: 5,
} as const
