import { format } from 'date-fns'
import {
  GetMasterKarteByIdResponse,
  FundamentalInformation,
  SatisfactionEvaluation,
  CompanyDepartment,
  CurrentProgram,
  NextProgram,
  Result,
  UsageHistory,
  GetSelectBoxResponse,
  Others,
  MasterKarten,
  SearchParams,
  DetailSearchParams,
} from '~/types/MasterKarte'
import { getCurrentDate } from '~/utils/common-functions'

import { Api } from '~/plugins/api'

const $api = new Api()

export async function GetMasterKarten(request: any) {
  return await $api.get<any>(`/master-karten`, request)
}

export async function GetMasterKartenSelectBox() {
  return await $api.get<any>(`/master-karten/select-box`)
}

export class MasterKarte implements MasterKarten {
  npfProjectId = ''
  ppProjectId = ''
  serviceName = ''
  project = ''
  client = ''
  supportDateFrom = ''
  supportDateTo = ''
  isAccessibleKarten = false
  isAccessibleMasterKarten = false
}
export class MasterKarteListSearchParams implements SearchParams {
  customerId = ''
  supportDateFrom = format(getCurrentDate(), 'yyyy/MM')
  supportDateTo = ''
  all = false
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
  supplementHumanResourceToSap = ''
  currentCustomerProfile = ''
  wantAcquireCustomerProfile = ''
  requiredPartner = ''
  ourStrength = ''
  aspiration = ''
  usageHistory = [new UsageHistoryClass()]
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

export class OthersClass implements Others {
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
  surveyId = ''
  satisfactionEvaluation = [new SatisfactionEvaluationClass()]
  isDisclosure = false
}

export class CurrentProgramClass implements CurrentProgram {
  id = ''
  isDraft = false
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
  isDraft = false
  version = 0
  isCustomerPublic = false
  fundamentalInformation = new FundamentalInformationClass()
  others = new OthersClass()
  lastUpdateBy = ''
  lastUpdateDatetime = ''
}

export class GetMasterKarteByIdResponseClass
  implements GetMasterKarteByIdResponse
{
  npfProjectId = ''
  ppProjectId = ''
  supporterOrganizationId = ''
  service = ''
  project = ''
  client = ''
  supportDateFrom = ''
  supportDateTo = ''
  lastUpdateDatetime = ''
  lastUpdateBy = ''
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

export class UpdateCurrentProgramClass {
  id = ''
  version: number = 0
  result = new ResultClass()
}

export class UpdateNextProgramClass {
  id = ''
  version: number = 0
  fundamentalInformation = new FundamentalInformationClass()
  others = new OthersClass()
  isCustomerPublic = false
}

export class UpdateMasterKarteResponseClass {
  ppProjectId = ''
  npfProjectId = ''
  currentProgram = new UpdateCurrentProgramClass()
  nextProgram = new UpdateNextProgramClass()
  isNotifyUpdateMasterKarte = false
}

export async function UpdateMasterKarteById(
  masterKarteId: string,
  isCurrentProgram: boolean,
  param: UpdateMasterKarteResponseClass
) {
  return await $api.put(
    `/master-karten/${masterKarteId}?isCurrentProgram=${
      isCurrentProgram ? 'true' : 'false'
    }`,
    param
  )
}

export async function CreateMasterKarte(isCurrentProgram: boolean, param: any) {
  return await $api.post(
    `/master-karten?isCurrentProgram=${isCurrentProgram ? 'true' : 'false'}`,
    param
  )
}

export async function GetNpfProjectId(ppProjectId: string) {
  return await $api.get<any>(`/master-karten/npf-id/${ppProjectId}`)
}

export async function GetMasterKarteSelectBox() {
  return await $api.get<GetSelectBoxResponse>('/master-karten/select-box')
}

export class SelectBoxClass {
  label = ''
  value = ''
}

export class GetSelectBoxResponseClass {
  name = ''
  items = new SelectBoxClass()
}
