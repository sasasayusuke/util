import {
  getCurrentDate,
  getFiscalYearStart,
  getFiscalYearEnd,
  toYearMonthInt,
} from '~/utils/common-functions'
import {
  IGetSurveysRequest,
  ISurveyListItem,
  IGetSurveysResponse,
  IMainSupporterUser,
  ISupporterUser,
  ITermSummaryResultService,
  ITermSummaryResultCompletion,
  ITermSummaryResultServiceAndCompletion,
  ITermSummaryResultQuick,
  ITermSummaryResultPp,
  ITermSummaryResultItem,
  ISurveySummaryService,
  ISurveySummaryCompletionContinuation,
  ISurveySummaryCompletion,
  ISurveySummaryKarte,
  ISurveySummaryQuick,
  ISurveySummaryPp,
  IGetSurveySummaryAllRequest,
  IGetSurveySummaryAllResponse,
  ICompletionContinuation,
  ISupporterOrganizationsTermSummaryResultItem,
  ISupporterOrganizationsItem,
  ISupporterOrganizationSurveysItem,
  IGetSurveySummarySupporterOrganizationsRequest,
  IGetSurveySummarySupporterOrganizationsResponse,
  IExportSurveysRequest,
  IExportSurveysResponse,
  ISummaryReportItem,
  IGetSurveySummaryReportRequest,
  IGetSurveySummaryReportResponse,
  IAnswerInfo,
  IGetSurveyByIdResponse,
  IUpdateSurveyByIdRequest,
  ISurveyListSummaryItem,
  ISurveyListSummaryTotalItem,
  ISurveyListSummaryTotalOuota,
  ISurveyListSummaryTotalHalf,
  ISurveyListSummaryTotal,
  ISurveyListSummary,
  IGetSurveyPlansRequest,
  ISurveyPlanListItem,
  IGetSurveyPlansResponse,
} from '@/types/Survey'
import { Api } from '~/plugins/api'
export { ENUM_GET_SURVEY_TYPE } from '@/types/Survey'

const $api = new Api()

export class MainSupporterUser implements IMainSupporterUser {
  id = ''
  name = ''
  organizationId = ''
  organizationName = ''
}

export class SupporterUser implements ISupporterUser {
  public id = ''
  public name = ''
  public organizationId = ''
  public organizationName = ''
}

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
  public supporterUsers: ISupporterUser[] = []
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

export class GetSurveysRequest implements IGetSurveysRequest {}

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

export class GetSurveysResponse implements IGetSurveysResponse {
  summary = new SurveyListSummary()
  surveys: SurveyListItem[] = []
}

export async function GetSurveys(request: GetSurveysRequest) {
  return await $api.get<GetSurveysResponse>(`/surveys`, request)
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
  public masterKarteSatisfactionAverage = 0
  public totalReceive = 0
}

export class TermSummaryResultItem implements ITermSummaryResultItem {
  public service = new TermSummaryResultService()
  public completion = new TermSummaryResultCompletion()
  public serviceAndCompletion = new TermSummaryResultServiceAndCompletion()
  public quick = new TermSummaryResultQuick()
  public pp = new TermSummaryResultPp()
}

export class SurveySummaryService implements ISurveySummaryService {
  public projectCount = 0
  public sendCount = 0
  public receiveCount = 0
  public receivePercent = 0
  public satisfactionSummary = 0
  public satisfactionAverage = 0;

  readonly [key: string]: any
}

export class SurveySummaryCompletionContinuation
  implements ISurveySummaryCompletionContinuation
{
  public positiveCount = 0
  public negativeCount = 0
  public positivePercent = 0
}

export class SurveySummaryCompletion implements ISurveySummaryCompletion {
  public projectCount = 0
  public sendCount = 0
  public receiveCount = 0
  public receivePercent = 0
  public satisfactionSummary = 0
  public satisfactionAverage = 0
  public continuation = new SurveySummaryCompletionContinuation()
  public recommendedSummary = 0
  public recommendedAverage = 0
  public salesSummary = 0
  public salesAverage = 0;

  readonly [key: string]: any
}

export class SurveySummaryKarte implements ISurveySummaryKarte {
  public projectCount = 0
  public karteCount = 0
  public usePercent = 0;

  readonly [key: string]: any
}

export class SurveySummaryQuick implements ISurveySummaryQuick {
  public sendCount = 0
  public receiveCount = 0
  public receivePercent = 0;

  readonly [key: string]: any
}

export class SurveySummaryPp implements ISurveySummaryPp {
  public surveySatisfactionSummary = 0
  public surveySatisfactionAverage = 0
  public manHourSatisfactionSummary = 0
  public manHourSatisfactionAverage = 0
  public karteSatisfactionSummary = 0
  public karteSatisfactionAverage = 0
  public masterKarteSatisfactionSummary = 0
  public masterKarteSatisfactionAverage = 0
  public sendCount = 0
  public receiveCount = 0
  public receivePercent = 0;

  readonly [key: string]: any
}

export class SurveySummaryItem {
  summaryMonth = ''
  service = new SurveySummaryService()
  completion = new SurveySummaryCompletion()
  karte = new SurveySummaryKarte()
  quick = new SurveySummaryQuick()
  pp = new SurveySummaryPp();

  readonly [key: string]: any
}

export class GetSurveySummaryAllResponse
  implements IGetSurveySummaryAllResponse
{
  public termSummaryResult: TermSummaryResultItem = new TermSummaryResultItem()
  public surveys: SurveySummaryItem[] = []
}

export class GetSurveySummaryAllRequest implements IGetSurveySummaryAllRequest {
  yearMonthFrom: number
  yearMonthTo: number

  constructor() {
    const now = getCurrentDate()

    const fiscalyearStart = getFiscalYearStart(now)
    this.yearMonthFrom = toYearMonthInt(fiscalyearStart)

    const fiscalyearEnd = getFiscalYearEnd(now)
    this.yearMonthTo = toYearMonthInt(fiscalyearEnd)
  }
}

export async function GetSurveySummaryAll(request: GetSurveySummaryAllRequest) {
  return await $api.get<GetSurveySummaryAllResponse>(
    `/surveys/summary/all`,
    request
  )
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

export class SupporterOrganizationsItem implements ISupporterOrganizationsItem {
  public supporterOrganizationName = ''
  public serviceSatisfactionAverage = 0
  public serviceReceive = 0
  public completionSatisfactionAverage = 0
  public completionContinuationPercent = 0
  public completionRecommendedAverage = 0
  public completionReceive = 0
  public totalSatisfactionAverage = 0
  public totalReceive = 0
}

export class SupporterOrganizationSurveysItem
  implements ISupporterOrganizationSurveysItem
{
  public summaryMonth = ''
  public supporterOrganizations: SupporterOrganizationsItem[] = []
}

export class GetSurveySummarySupporterOrganizationsResponse
  implements IGetSurveySummarySupporterOrganizationsResponse
{
  public termSummaryResult: SupporterOrganizationsTermSummaryResultItem[] = []
  public surveys: SupporterOrganizationSurveysItem[] = []
}

export class GetSurveySummarySupporterOrganizationsRequest
  implements IGetSurveySummarySupporterOrganizationsRequest
{
  yearMonthFrom: number
  yearMonthTo: number

  constructor() {
    const now = getCurrentDate()

    const fiscalyearStart = getFiscalYearStart(now)
    this.yearMonthFrom = toYearMonthInt(fiscalyearStart)

    const fiscalyearEnd = getFiscalYearEnd(now)
    this.yearMonthTo = toYearMonthInt(fiscalyearEnd)
  }
}

export async function GetSurveySummarySupporterOrganizations(
  request: GetSurveySummarySupporterOrganizationsRequest
) {
  return await $api.get<GetSurveySummarySupporterOrganizationsResponse>(
    `/surveys/summary/supporter-organizations`,
    request
  )
}

export class ExportSurveysRequest implements IExportSurveysRequest {
  public mode = ''
}

export class ExportSurveysResponse implements IExportSurveysResponse {
  public url = ''
}

export async function ExportSurveys(request: ExportSurveysRequest) {
  return await $api.get<ExportSurveysResponse>(`/surveys/export`, request)
}

export class AnswerInfo implements IAnswerInfo {
  public id = ''
  public answer = ''
  public point = 0
  public choiceIds: Array<string> = []
  public summaryType = 'normal'
  public otherInput = ''
}

export class GetSurveyByIdResponse implements IGetSurveyByIdResponse {
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
  public supporterUsers = []
  public salesUserId = ''
  public salesUserName = ''
  public serviceTypeId = ''
  public serviceTypeName = ''
  public answerUserId = ''
  public answerUserName = ''
  public customerId = ''
  public customerName = ''
  public company = ''
  public answers: AnswerInfo[] = []
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
  public updateUserName = ''
  public updateAt = ''
  public version = 0
}

export async function GetSurveyById(id: string) {
  return await $api.get<GetSurveyByIdResponse>(`/surveys/${id}`)
}

export class UpdateSurveyByIdRequest implements IUpdateSurveyByIdRequest {
  public summaryMonth = ''
  public isNotSummary = false
  public isSolverProject = false
}

export async function UpdateSurveyById(
  id: string,
  version: number,
  request: UpdateSurveyByIdRequest
) {
  return await $api.put<string>(`/surveys/${id}?version=${version}`, request)
}

export class SummaryReportItem implements ISummaryReportItem {
  public plan = 0
  public collect = 0
  public percent = 0
  public summaryPlan = 0
  public summaryCollect = 0
  public summaryPercent = 0
}

export class GetSurveySummaryReportRequest
  implements IGetSurveySummaryReportRequest {}

export class GetSurveySummaryReportResponse
  implements IGetSurveySummaryReportResponse
{
  public summaryMonth = ''
  public service = new SummaryReportItem()
  public completion = new SummaryReportItem()
  public serviceAndCompletion = new SummaryReportItem()
  public pp = new SummaryReportItem()
}

export async function GetSurveySummaryReport(
  request: GetSurveySummaryReportRequest
) {
  return await $api.get<GetSurveySummaryReportResponse>(
    `/surveys/summary/reports`,
    request
  )
}

export class GetSurveyPlansRequest implements IGetSurveyPlansRequest {}

export class SurveyPlanListItem implements ISurveyPlanListItem {
  public projectId = ''
  public projectName = ''
  public id = ''
  public surveyType = ''
  public customerSuccess = ''
  public supporterOrganizationId = ''
  public supporterOrganizationName = ''
  public supportDateFrom = ''
  public supportDateTo = ''
  public mainSupporterUser = new MainSupporterUser()
  public supporterUsers = []
  public salesUserId = ''
  public salesUserName = ''
  public serviceTypeId = ''
  public serviceTypeName = ''
  public surveyUserType = ''
  public surveyUserName = ''
  public surveyUserEmail = ''
  public customerId = ''
  public customerName = ''
  public summaryMonth = ''
  public planSurveyRequestDatetime = ''
  public actualSurveyRequestDatetime = ''
  public planSurveyResponseDatetime = ''
  public actualSurveyResponseDatetime = ''
  public phase = ''
  public isCountManHour = false
  public unansweredSurveysNumber = 0
  public thisMonthType = ''
  public noSendReason = ''
}

export class GetSurveyPlansResponse implements IGetSurveyPlansResponse {
  surveys: SurveyPlanListItem[] = []
}

export async function GetSurveyPlans(request: GetSurveyPlansRequest) {
  return await $api.get<GetSurveyPlansResponse>(`/surveys/plans`, request)
}
