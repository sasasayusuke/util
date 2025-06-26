export interface MasterKarten {
  npfProjectId: string
  ppProjectId: string
  serviceName: string
  project: string
  client: string
  supportDateFrom: string
  supportDateTo: string
  isAccessibleKarten: boolean
}

export interface GetMasterKarten {
  offsetPage: number
  total: number
  karten: MasterKarten[]
}

export interface MasterKartenSelectBox {
  name: string
  items: {
    value: string
    label: string
  }[]
}

export interface SearchParams {
  customerId: string
  supportDateFrom: string
  supportDateTo?: string
}

export interface DetailSearchParams {
  isCurrentProgram: boolean
  category: string[]
  industrySegment: string[]
  departmentName: string
  currentSituation: string
  issue: string
  customerSuccess: string
  lineup: string[]
  requiredPersonalSkill: string
  requiredPartner: string
  strength: string
}

export interface UsageHistory {
  npfProjectId: string
  projectName: string
  serviceType: string
}

export interface FundamentalInformation {
  presidentPolicy: string
  kpi: string
  toBeThreeYears: string
  currentSituation: string
  issue: string
  request: string
  customerSuccess: string
  customerSuccessReuse: boolean | null
  schedule: string
  lineup: string
  supportContents: string
  requiredPersonalSkill: string
  requiredPartner: string
  supplementHumanResourceToSap: string
  ourStrengths: string
  currentCustomerProfile: string
  wantAcquireCustomerProfile: string
  aspiration: string
  usageHistory: UsageHistory[]
}

export interface SatisfactionEvaluation {
  isAnswer: boolean
  title: string
}

export interface CompanyDepartment {
  customerName: string
  customerUrl: string
  category: string
  establishment: string
  employee: number
  capitalStock: number
  businessSummary: string
  industrySegment: string
  departmentId: string
  departmentName: string
}

export interface Others {
  mission: string
  numberOfPeople: string
  manager: string
  commercializationSkill: string
  existPartners: string
  supportOrder: string
  existEvaluation: string
  existAudition: string
  existIdeation: string
  existIdeaReview: string
  budget: string
  humanResource: string
  idea: string
  theme: string
  client: string
  clientIssue: string
  solution: string
  originality: string
  mvp: string
  tam: string
  sam: string
  isRightTime: string
  roadMap: string
}

export interface Result {
  customerSuccessResult: string
  customerSuccessResultFactor: string
  supportIssue: string
  supportSuccessFactor: string
  surveySsapAssessment: string
  nextSupportContent: string
  questionnaire: string
  satisfactionEvaluation: SatisfactionEvaluation[]
  surveyId: string
}

export interface CurrentProgram {
  id: string
  version: number
  fundamentalInformation: FundamentalInformation
  result: Result
  companyDepartment: CompanyDepartment
  others: Others
  lastUpdateBy: string
  lastUpdateDatetime: string
}

export interface NextProgram {
  id: string
  version: number
  isCustomerPublic: boolean
  fundamentalInformation: FundamentalInformation
  others: Others
  lastUpdateBy: string
  lastUpdateDatetime: string
}

export interface GetMasterKarteByIdResponse {
  masterKarteId: string
  ppProjectId: string
  supporterOrganizationId: string
  service: string
  project: string
  client: string
  supportDateFrom: string
  supportDateTo: string
  currentProgram: CurrentProgram
  nextProgram: NextProgram
}

export interface SelectBox {
  label: string
  value: string
}

export interface GetSelectBoxResponse {
  name: string
  items: SelectBox[]
}
