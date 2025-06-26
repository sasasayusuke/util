import { format } from 'date-fns'
import { getCurrentDate } from '~/utils/common-functions'
import {
  MasterKarten,
  SearchParams,
  DetailSearchParams,
  GetMasterKarteByIdResponse,
  FundamentalInformation,
  SatisfactionEvaluation,
  CompanyDepartment,
  CurrentProgram,
  NextProgram,
  Result,
  UsageHistory,
  GetSelectBoxResponse,
} from '~/types/MasterKarte'

import { Api } from '~/plugins/api'

const $api = new Api()

export class MasterKarte implements MasterKarten {
  npfProjectId = ''
  ppProjectId = ''
  serviceName = ''
  project = ''
  client = ''
  supportDateFrom = ''
  supportDateTo = ''
  isAccessibleKarten = false
}
export class MasterKarteListSearchParams implements SearchParams {
  customerId = ''
  supportDateFrom = format(getCurrentDate(), 'yyyy/MM')
  supportDateTo? = ''
}
export class MasterKarteListDetailSearchParams implements DetailSearchParams {
  isCurrentProgram = true
  category = []
  industrySegment = []
  departmentName = ''
  currentSituation = ''
  issue = ''
  customerSuccess = ''
  lineup = []
  requiredPersonalSkill = ''
  requiredPartner = ''
  strength = ''
}

export class UsageHistoryClass implements UsageHistory {
  npfProjectId = ''
  projectName = ''
  serviceType = ''
}
export class FundamentalInformationClass implements FundamentalInformation {
  presidentPolicy = ''
  kpi = ''
  toBeThreeYears = ''
  currentSituation = ''
  issue = ''
  request = ''
  customerSuccess = ''
  customerSuccessReuse = false
  schedule = ''
  lineup = ''
  supportContents = ''
  requiredPersonalSkill = ''
  requiredPartner = ''
  supplementHumanResourceToSap = ''
  ourStrengths = ''
  currentCustomerProfile = ''
  wantAcquireCustomerProfile = ''
  aspiration = ''
  usageHistory = []
}

export class SatisfactionEvaluationClass implements SatisfactionEvaluation {
  isAnswer = false
  title = ''
}

export class CompanyDepartmentClass implements CompanyDepartment {
  customerName = ''
  customerUrl = ''
  category = ''
  establishment = ''
  employee = 0
  capitalStock = 0
  businessSummary = ''
  industrySegment = ''
  departmentId = ''
  departmentName = ''
}

export class OthersClass {
  mission = ''
  numberOfPeople = ''
  manager = ''
  commercializationSkill = ''
  existPartners = ''
  supportOrder = ''
  existEvaluation = ''
  existAudition = ''
  existIdeation = ''
  existIdeaReview = ''
  budget = ''
  humanResource = ''
  idea = ''
  theme = ''
  client = ''
  clientIssue = ''
  solution = ''
  originality = ''
  mvp = ''
  tam = ''
  sam = ''
  isRightTime = ''
  roadMap = ''
}

export class ResultClass {
  customerSuccessResult = ''
  customerSuccessResultFactor = ''
  supportIssue = ''
  supportSuccessFactor = ''
  surveySsapAssessment = ''
  nextSupportContent = ''
  questionnaire = ''
  satisfactionEvaluation = [new SatisfactionEvaluationClass()]
  surveyId = ''
}

export class CurrentProgramClass implements CurrentProgram {
  id = ''
  version = 0
  fundamentalInformation = new FundamentalInformationClass()
  companyDepartment = new CompanyDepartmentClass()
  result: Result = new ResultClass()
  others = new OthersClass()
  lastUpdateBy = ''
  lastUpdateDatetime = ''
}

export class NextProgramClass implements NextProgram {
  id = ''
  version = 0
  isCustomerPublic: boolean = false
  isDraft = false
  fundamentalInformation = new FundamentalInformationClass()
  others = new OthersClass()
  lastUpdateBy = ''
  lastUpdateDatetime = ''
}

export class GetMasterKarteByIdResponseClass
  implements GetMasterKarteByIdResponse
{
  masterKarteId = ''
  ppProjectId = ''
  supporterOrganizationId = ''
  service = ''
  project = ''
  client = ''
  supportDateFrom = ''
  supportDateTo = ''
  currentProgram = new CurrentProgramClass()
  nextProgram = new NextProgramClass()
}

export class GetMasterKarteByIdRequest {
  version = 1
}

export async function GetMasterKarteById(npfProjectId: string) {
  return await $api.get<GetMasterKarteByIdResponse>(
    `/master-karten/${npfProjectId}`
  )
}

export async function GetMasterKarten(request: any) {
  return await $api.get<any>(`/master-karten`, request)
}

export async function GetMasterKartenSelectBox() {
  return await $api.get<GetSelectBoxResponse>(`/master-karten/select-box`)
}
export async function GetNpfProjectId(ppProjectId: string) {
  return await $api.get<any>(`/master-karten/npf-id/${ppProjectId}`)
}

export class SelectBoxClass {
  label = ''
  value = ''
}

export class GetSelectBoxResponseClass {
  name = ''
  items = new SelectBoxClass()
}
