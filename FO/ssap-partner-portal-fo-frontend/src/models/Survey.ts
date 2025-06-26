import {
  getFiscalYearStart,
  getFiscalYearEnd,
  toYearMonthInt,
} from '~/utils/common-functions'
import {
  ISurveySearchParam,
  IGetSurveysByMineRequest,
  IGetSurveysRequest,
  ISurveyListItem,
  IGetSurveysResponse,
  IUpdateSurveyByIdRequest,
  IMainSupporterUser,
  ISupporterUser,
  ISurveyInfo,
  IAnswerInfo,
  ISurveyMasterInfo,
  ISurveyMasterSatisfactionInfo,
  IQuestionChoiceGroupInfo,
  IQuestionChoiceInfo,
  IQuestionInfo,
  IQuestionFlowInfo,
  IGetSurveysByMineResponse,
  IInputAnswerInfo,
  ENUM_GET_SURVEYS_BY_MINE_TYPE,
  ENUM_GET_SURVEYS_BY_MINE_SORT,
  GetSurveysByMineType,
  ICompletionContinuation,
  ISupporterOrganizationsTermSummaryResultItem,
  ITermSummaryResultService,
  ITermSummaryResultCompletion,
  ITermSummaryResultServiceAndCompletion,
  ITermSummaryResultQuick,
  ITermSummaryResultPp,
  ITermSummaryResultItem,
  ISupporterOrganizationsItem,
  ISupporterOrganizationSurveysItem,
  IGetSurveySummarySupporterOrganizationsByMineResponse,
  ISurveySummaryItem,
  IGetSurveySummaryByMineResponse,
  ISurveyByMineItem,
  ISurveyListSummaryItem,
  ISurveyListSummaryTotalItem,
  ISurveyListSummaryTotalOuota,
  ISurveyListSummaryTotalHalf,
  ISurveyListSummaryTotal,
  ISurveyListSummary,
  ICheckSurveyByIdPasswordRequest,
  ICheckSurveyByIdPasswordResponse,
  ICheckAndGetSurveyAnonymousByIdRequest,
  ICheckAndUpdateSurveyAnonymousByIdRequest,
  ICheckAndGetSurveyMasterAnonymousByIdAndRevisionRequest,
  IGetSurveyOfSatisfactionByIdRequest,
  IGetSurveyOfSatisfactionByIdResponse,
  IUpdateSurveyOfSatisfactionByIdRequest,
  IGetSurveyMasterSatisfactionByIdAndRevisionRequest,
} from '@/types/Survey'
import { Api } from '~/plugins/api'

const $api = new Api()
const $noAuthapi = new Api(false)

// アンケート一覧検索パラメタ型実装
export class SurveySearchParam implements ISurveySearchParam {
  public type = ''
  public enbFromTo = false
  public dateFrom = ''
  public dateTo = ''
  public limit = 0
}

// 支援者型実装
export class SupporterUser implements ISupporterUser {
  public id = ''
  public name = ''
  public organizationId = ''
  public organizationName = ''
}

// 回答型実装
export class AnswerInfo implements IAnswerInfo {
  public id = ''
  public answer = ''
  public point = 0
  public choiceIds: Array<string> = []
  public summaryType = 'normal'
  public otherInput = ''
}

// 設問の選択肢グループ型実装
export class QuestionChoiceGroupInfo implements IQuestionChoiceGroupInfo {
  public id = ''
  public title = ''
  public disabled = false
  public isNew = false
}

// 設問の選択肢型定義
export class QuestionChoiceInfo implements IQuestionChoiceInfo {
  public description = ''
  public group: Array<QuestionChoiceGroupInfo> = []
  public isNew = false
  public options: Array<string> = []
  public optionValues: Array<number> = []
}

// 設問型定義
export class QuestionInfo implements IQuestionInfo {
  public id = ''
  public required = false
  public description = ''
  public format = ''
  public summaryType = ''
  public choices: Array<QuestionChoiceInfo> = []
  public otherDescription = ''
  public disabled = false
  public isNew = false
  public isEnd = false
}

// 出題フロー型定義
export class QuestionFlowInfo implements IQuestionFlowInfo {
  public id = ''
  public conditionId = ''
  public conditionChoiceIds: Array<string> = []
  public questionIndex = 0
  public prevIndex = 0
}

export class MainSupporterUser implements IMainSupporterUser {
  id = ''
  name = ''
  organizationId = ''
  organizationName = ''
}

/*  アンケート一覧情報型実装
    使用する値のみ実装
*/
export class SurveyListItem implements ISurveyListItem {
  public id = ''
  public surveyMasterId = ''
  public surveyRevision = 0
  public surveyType = ''
  public projectId = ''
  public projectName = ''
  public customerSuccess = ''
  public supporterOrganizationId = ''
  public supporterOrganizationName = ''
  public supportDateFrom = ''
  public supportDateTo = ''
  public mainSupporterUser = new MainSupporterUser()
  public supporterUsers: SupporterUser[] = []
  public salesUserId = ''
  public salesUserName = ''
  public serviceTypeId = ''
  public serviceTypeName = ''
  public answerUserId = ''
  public answerUserName = ''
  public customerId = ''
  public customerName = ''
  public company = ''
  public summaryMonth = ''
  public isNotSummary = false
  public planSurveyRequestDatetime = ''
  public actualSurveyRequestDatetime = ''
  public planSurveyResponseDatetime = ''
  public actualSurveyResponseDatetime = ''
  public isFinished = false
  public isDisclosure = false
  public isAnswerUserName = false
}

// アンケート詳細型実装
export class SurveyInfo implements ISurveyInfo {
  public id = ''
  public surveyMasterId = ''
  public surveyRevision = 0
  public surveyType = ''
  public projectId = ''
  public projectName = ''
  public customerSuccess = ''
  public supporterOrganizationId = ''
  public supporterOrganizationName = ''
  public supportDateFrom = ''
  public supportDateTo = ''
  public mainSupporterUser = new SupporterUser()
  public supporterUsers: SupporterUser[] = []
  public salesUserId = ''
  public salesUserName = ''
  public serviceTypeId = ''
  public serviceTypeName = ''
  public answerUserId = ''
  public answerUserName = ''
  public customerId = ''
  public customerName = ''
  public company = ''
  public answers: Array<AnswerInfo> = []
  public summaryMonth = ''
  public isNotSummary = false
  public isSolverProject = false
  public planSurveyRequestDatetime = ''
  public actualSurveyRequestDatetime = ''
  public planSurveyResponseDatetime = ''
  public actualSurveyResponseDatetime = ''
  public isFinished = false
  public isDisclosure = false
  public createId = ''
  public createAt = ''
  public updateId = ''
  public updateAt = ''
  public version = 0
  public mainSupporterUserName: string = ''
  public supportersStr: string = ''
  public surveyTargetDuration: string = ''
  public surveyCompany: string = ''
}

// アンケートマスタ型定義
export class SurveyMasterInfo implements ISurveyMasterInfo {
  public id = ''
  public revision = 0
  public name = ''
  public type = ''
  public timing = ''
  public initSendDaySetting = 0
  public initAnswerLimitDaySetting = 0
  public isDisclosure = false
  public questions: Array<QuestionInfo> = []
  public questionFlow: Array<QuestionFlowInfo> = []
  public isLatest = false
  public updateId = ''
  public updateAt = ''
  public version = 0
}

// 満足度評価のみアンケートマスタ型定義
export class SurveyMasterSatisfactionInfo
  implements ISurveyMasterSatisfactionInfo
{
  public id = ''
  public projectName = ''
  public customerName = ''
  public questions = new QuestionInfo()
  public version = 0
}

// 回答入力情報型実装
export class InputAnswerInfo implements IInputAnswerInfo {
  public answer = ''
  public otherAnswer = ''
  public isAnswered = false
}

export class SurveyListSummaryItem implements ISurveyListSummaryItem {
  public month = ''
  public satisfactionAverage = 0
  public receive = 0
}

export class SurveyListSummaryTotalItem implements ISurveyListSummaryTotalItem {
  public satisfactionAverage = 0
  public receive = 0
}

export class SurveyListSummaryTotalOuota
  implements ISurveyListSummaryTotalOuota
{
  public quota1 = new SurveyListSummaryTotalItem()
  public quota2 = new SurveyListSummaryTotalItem()
  public quota3 = new SurveyListSummaryTotalItem()
  public quota4 = new SurveyListSummaryTotalItem()
}
export class SurveyListSummaryTotalHalf implements ISurveyListSummaryTotalHalf {
  public half1 = new SurveyListSummaryTotalItem()
  public half2 = new SurveyListSummaryTotalItem()
}

export class SurveyListSummaryTotal implements ISurveyListSummaryTotal {
  public quota = new SurveyListSummaryTotalOuota()
  public half = new SurveyListSummaryTotalHalf()
  public year = new SurveyListSummaryTotalItem()
}

export class SurveyListSummary implements ISurveyListSummary {
  public monthly: SurveyListSummaryItem[] = []
  public accumulation: SurveyListSummaryItem[] = []
  public total = new SurveyListSummaryTotal()
}

// GetSurveysByMineレスポンス型実装
export class GetSurveysResponse implements IGetSurveysResponse {
  summary = new SurveyListSummary()
  surveys: SurveyListItem[] = []
}

// UpdateSurveyByIdリクエスト型定義レスポンス型実装
export class UpdateSurveyByIdRequest implements IUpdateSurveyByIdRequest {
  public answers: Array<AnswerInfo> = []
  public isFinished = false
  public isDisclosure = false
}

// GetSurveysByMine実行関数
export function GetSurveysByMine(request: IGetSurveysByMineRequest) {
  return $api.get<GetSurveysByMineResponse>(`/surveys/me`, request)
}

export class GetSurveysRequest {
  summaryMonth?: string
  summaryMonthFrom?: string
  summaryMonthTo?: string
  planSurveyRequestDateFrom?: string
  planSurveyRequestDateTo?: string
  projectId?: string
  customerId?: string
  isFinished?: boolean
  isDisclosure?: boolean
  type?: string
  serviceTypeId?: string
  organizationIds?: string
}

// GetSurveys実行関数 TODO: 型の修正
export function GetSurveys(request: IGetSurveysRequest | GetSurveysRequest) {
  return $api.get<GetSurveysResponse>(`/surveys`, request)
}

// GetSurveyById実行関数
export async function GetSurveyById(id: string) {
  return await $api.get<SurveyInfo>(`/surveys/` + id)
}

// GetSurveyMasterByIdAndRevision実行関数
export function GetSurveyMasterByIdAndRevision(id: string, revision: number) {
  return $api.get<SurveyMasterInfo>(`/survey-masters/` + id + `/` + revision)
}

// UpdateSurveyById実行関数
export function PutUpdateSurveyByIdRequest(
  id: string,
  version: number,
  params: UpdateSurveyByIdRequest
) {
  return $api.put<string, UpdateSurveyByIdRequest>(
    `/surveys/` + id + `?version=` + version,
    params
  )
}

export class GetSurveysByMineRequest implements IGetSurveysByMineRequest {
  dateFrom?: Number
  dateTo?: Number
  type?: GetSurveysByMineType
  sort = ENUM_GET_SURVEYS_BY_MINE_SORT.ACTUAL_SURVEY_REQUEST_DATETIME_DESC
  projectId?: string
  limit?: Number
}

export class PPSurveysListRequest extends GetSurveysByMineRequest {
  type = ENUM_GET_SURVEYS_BY_MINE_TYPE.PP
}

export class GetSurveysByMineResponse implements IGetSurveysByMineResponse {
  total = 0
  surveys: SurveyListItem[] = []
}

export function getSurveysByMine(
  request: GetSurveysByMineRequest = new GetSurveysByMineRequest()
) {
  return $api.get<GetSurveysByMineResponse>('/surveys/me', request)
}

export class CompletionContinuation implements ICompletionContinuation {
  public positiveCount = 0
  public negativeCount = 0
  public positivePercent = 0
}

export class SupporterOrganizationsTermSummaryResultItem
  implements ISupporterOrganizationsTermSummaryResultItem
{
  public supporterOrganizationId = ''
  public supporterOrganizationName = ''
  public serviceSatisfactionSummary = 0
  public serviceSatisfactionAverage = 0
  public serviceReceive = 0
  public completionSatisfactionSummary = 0
  public completionSatisfactionAverage = 0
  public completionContinuation = new CompletionContinuation()
  public completionRecommendedSummary = 0
  public completionRecommendedAverage = 0
  public completionReceive = 0
  public totalSatisfactionSummary = 0
  public totalSatisfactionAverage = 0
  public totalReceive = 0
}

export class TermSummaryResultService implements ITermSummaryResultService {
  public satisfactionAverage = 0
  public totalReceive = 0
}

export class TermSummaryResultCompletion
  implements ITermSummaryResultCompletion
{
  public satisfactionAverage = 0
  public continuationPositivePercent = 0
  public recommendedAverage = 0
  public salesAverage = 0
  public totalReceive = 0
}

export class TermSummaryResultServiceAndCompletion
  implements ITermSummaryResultServiceAndCompletion
{
  public satisfactionAverage = 0
  public totalReceive = 0
}

export class TermSummaryResultQuick implements ITermSummaryResultQuick {
  public totalReceive = 0
}

export class TermSummaryResultPp implements ITermSummaryResultPp {
  public surveySatisfactionAverage = 0
  public manHourSatisfactionAverage = 0
  public karteSatisfactionAverage = 0
  public totalReceive = 0
}

export class TermSummaryResultItem implements ITermSummaryResultItem {
  public service = new TermSummaryResultService()
  public completion = new TermSummaryResultCompletion()
  public serviceAndCompletion = new TermSummaryResultServiceAndCompletion()
  public quick = new TermSummaryResultQuick()
  public pp = new TermSummaryResultPp()
}

export class SupporterOrganizationsItem implements ISupporterOrganizationsItem {
  public supporterOrganizationName = ''
  public serviceSatisfactionAverage = 0
  public serviceReceive = 0
  public completionSatisfactionAverage = 0
  public completionContinuationPercent = 0
  public completionRecommendedAverage = 0
  public completionReceive = 0
  public completionSalesAverage = 0
  public completionSalesSummary = 0
  public totalSatisfactionAverage = 0
  public totalReceive = 0
}

export class SupporterOrganizationSurveysItem
  implements ISupporterOrganizationSurveysItem
{
  public summaryMonth = ''
  public supporterOrganizations: SupporterOrganizationsItem[] = []
}

export class GetSurveySummarySupporterOrganizationsByMineResponse
  implements IGetSurveySummarySupporterOrganizationsByMineResponse
{
  public termSummaryResult: SupporterOrganizationsTermSummaryResultItem[] = []
  public surveys: SupporterOrganizationSurveysItem[] = []
}

export class GetSurveySummarySupporterOrganizationsByMineRequest {
  yearMonthFrom: number
  yearMonthTo: number

  constructor() {
    const fiscalyearStart = getFiscalYearStart()
    this.yearMonthFrom = toYearMonthInt(fiscalyearStart)

    const fiscalyearEnd = getFiscalYearEnd()
    this.yearMonthTo = toYearMonthInt(fiscalyearEnd)
  }
}

export async function GetSurveySummarySupporterOrganizationsByMine(
  request: GetSurveySummarySupporterOrganizationsByMineRequest
) {
  return await $api.get<GetSurveySummarySupporterOrganizationsByMineResponse>(
    `/surveys/summary/supporter-organizations/me`,
    request
  )
}

export class SurveySummaryItem implements ISurveySummaryItem {
  public summaryMonth = ''
  public serviceSatisfactionSummary = 0
  public serviceSatisfactionAverage = 0
  public serviceReceive = 0
  public completionSatisfactionSummary = 0
  public completionSatisfactionAverage = 0
  public completionContinuation = new CompletionContinuation()
  public completionRecommendedSummary = 0
  public completionRecommendedAverage = 0
  public completionReceive = 0
  public quickReceive = 0
}

export class GetSurveySummaryByMineResponse
  implements IGetSurveySummaryByMineResponse
{
  public termSummaryResult: TermSummaryResultItem = new TermSummaryResultItem()
  public surveys: SurveySummaryItem[] = []
}

export class GetSurveySummaryByMineRequest {
  yearMonthFrom: number
  yearMonthTo: number

  constructor() {
    const fiscalyearStart = getFiscalYearStart()
    this.yearMonthFrom = toYearMonthInt(fiscalyearStart)

    const fiscalyearEnd = getFiscalYearEnd()
    this.yearMonthTo = toYearMonthInt(fiscalyearEnd)
  }
}

export class SurveyByMineItem implements ISurveyByMineItem {
  public id = ''
  public surveyMasterId = ''
  public surveyRevision = 0
  public surveyType = ''
  public projectId = ''
  public projectName = ''
  public customerSuccess = ''
  public supporterOrganizationId = ''
  public supporterOrganizationName = ''
  public supportDateFrom = ''
  public supportDateTo = ''
  public mainSupporterUser = new SupporterUser()
  public supporterUsers: SupporterUser[] = []
  public salesUserId = ''
  public salesUserName = ''
  public serviceTypeId = ''
  public serviceTypeName = ''
  public answerUserId = ''
  public answerUserName = ''
  public customerId = ''
  public customerName = ''
  public company = ''
  public summaryMonth = ''
  public isNotSummary = false
  public planSurveyRequestDatetime = ''
  public actualSurveyRequestDatetime = ''
  public planSurveyResponseDatetime = ''
  public actualSurveyResponseDatetime = ''
  public isFinished = false
  public isDisclosure = false
}

export async function GetSurveySummaryByMine(
  request: GetSurveySummaryByMineRequest
) {
  return await $api.get<GetSurveySummaryByMineResponse>(
    `/surveys/summary/me`,
    request
  )
}

export class CheckSurveyByIdPasswordRequest
  implements ICheckSurveyByIdPasswordRequest
{
  public token = ''
  public password = ''
}

export class CheckSurveyByIdPasswordResponse
  implements ICheckSurveyByIdPasswordResponse
{
  public id = ''
}

export async function CheckSurveyByIdPassword(
  request: CheckSurveyByIdPasswordRequest
) {
  return await $noAuthapi.post<CheckSurveyByIdPasswordResponse>(
    `/surveys/auth`,
    request
  )
}

// アンケート詳細型実装
export class SurveyAnonymousInfo implements ISurveyInfo {
  public id = ''
  public surveyMasterId = ''
  public surveyRevision = 0
  public surveyType = ''
  public projectId = ''
  public projectName = ''
  public customerSuccess = ''
  public supporterOrganizationId = ''
  public supporterOrganizationName = ''
  public supportDateFrom = ''
  public supportDateTo = ''
  public mainSupporterUser = new SupporterUser()
  public supporterUsers: SupporterUser[] = []
  public salesUserId = ''
  public salesUserName = ''
  public serviceTypeId = ''
  public serviceTypeName = ''
  public customerId = ''
  public customerName = ''
  public company = ''
  public answers: Array<AnswerInfo> = []
  public summaryMonth = ''
  public isNotSummary = false
  public isSolverProject = false
  public planSurveyRequestDatetime = ''
  public actualSurveyRequestDatetime = ''
  public planSurveyResponseDatetime = ''
  public actualSurveyResponseDatetime = ''
  public isFinished = false
  public isDisclosure = false
  public createId = ''
  public createUserName = ''
  public createAt = ''
  public updateId = ''
  public updateAt = ''
  public updateUserName = ''
  public version = 0
  public mainSupporterUserName: string = ''
  public supportersStr: string = ''
  public surveyTargetDuration: string = ''
  public surveyCompany: string = ''
}

export class CheckAndGetSurveyAnonymousByIdRequest
  implements ICheckAndGetSurveyAnonymousByIdRequest
{
  public token = ''
  public password = ''
}

// GetSurveyAnonymousById実行関数
export async function CheckAndGetSurveyAnonymousById(
  id: string,
  request: CheckAndGetSurveyAnonymousByIdRequest
) {
  return await $noAuthapi.post<SurveyAnonymousInfo>(
    `/surveys/anonymous/` + id,
    request
  )
}

// 匿名アンケートマスタ型定義
export class SurveyMasterAnonymousInfo extends SurveyMasterInfo {}

export class CheckAndGetSurveyMasterAnonymousByIdAndRevisionRequest
  implements ICheckAndGetSurveyMasterAnonymousByIdAndRevisionRequest
{
  public token = ''
  public password = ''
}

// CheckAndGetSurveyMasterAnonymousByIdAndRevision実行関数
export function CheckAndGetSurveyMasterAnonymousByIdAndRevision(
  id: string,
  revision: number,
  request: CheckAndGetSurveyMasterAnonymousByIdAndRevisionRequest
) {
  return $noAuthapi.post<SurveyMasterAnonymousInfo>(
    `/survey-masters/anonymous/` + id + `/` + revision,
    request
  )
}

// CheckAndUpdateSurveyAnonymousByIdリクエスト型実装
export class CheckAndUpdateSurveyAnonymousByIdRequest
  implements ICheckAndUpdateSurveyAnonymousByIdRequest
{
  public token = ''
  public password = ''
  public answers: Array<AnswerInfo> = []
  public isFinished = false
  public isDisclosure = false
}

// CheckAndUpdateSurveyAnonymousById実行関数
export function CheckAndUpdateSurveyAnonymousById(
  id: string,
  version: number,
  params: CheckAndUpdateSurveyAnonymousByIdRequest
) {
  return $noAuthapi.put<string, CheckAndUpdateSurveyAnonymousByIdRequest>(
    `/surveys/anonymous/` + id + `?version=` + version,
    params
  )
}

// GetSurveyOfSatisfactionByIdリクエスト型実装
export class GetSurveyOfSatisfactionByIdRequest
  implements IGetSurveyOfSatisfactionByIdRequest
{
  public token = ''
}

export class GetSurveyOfSatisfactionByIdResponse
  implements IGetSurveyOfSatisfactionByIdResponse
{
  public surveyId = ''
  public isFinished = false
  public surveyMasterId = ''
  public surveyRevision = 0
  public projectId = ''
  public projectName = ''
  public customerName = ''
  public version = 0
}

// GetSurveyOfSatisfactionById実行関数
export async function GetSurveyOfSatisfactionById(
  request: GetSurveyOfSatisfactionByIdRequest
) {
  return await $noAuthapi.post<GetSurveyOfSatisfactionByIdResponse>(
    `/surveys/satisfaction`,
    request
  )
}

// UpdateSurveyOfSatisfactionByIdリクエスト型実装
export class UpdateSurveyOfSatisfactionByIdRequest
  implements IUpdateSurveyOfSatisfactionByIdRequest
{
  public token = ''
  public answers: Array<AnswerInfo> = []
  public isFinished = false
  public isDisclosure = false
}

// UpdateSurveyOfSatisfactionById実行関数
export function UpdateSurveyOfSatisfactionById(
  id: string,
  version: number,
  request: UpdateSurveyOfSatisfactionByIdRequest
) {
  return $noAuthapi.put<string, UpdateSurveyOfSatisfactionByIdRequest>(
    `/surveys/satisfaction/` + id + `?version=` + version,
    request
  )
}

// GetSurveyMasterSatisfactionByIdAndRevisionリクエスト型実装
export class GetSurveyMasterSatisfactionByIdAndRevisionRequest
  implements IGetSurveyMasterSatisfactionByIdAndRevisionRequest
{
  public token = ''
}

// GetSurveyMasterSatisfactionByIdAndRevision実行関数
export function GetSurveyMasterSatisfactionByIdAndRevision(
  id: string,
  revision: number,
  request: GetSurveyMasterSatisfactionByIdAndRevisionRequest
) {
  return $noAuthapi.post<SurveyMasterSatisfactionInfo>(
    `/survey-masters/satisfaction/` + id + `/` + revision,
    request
  )
}
