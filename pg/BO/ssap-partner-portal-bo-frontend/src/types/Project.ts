export interface IProfit {
  monthly: number[]
  quarterly: number[]
  half: number[]
  year: number
}

export interface IGross {
  monthly: number[]
  quarterly: number[]
  half: number[]
  year: number
}

export interface ISalesforceMainCustomer {
  name: string
  email: string
  organizationName: string
}

export interface IProjectListItem {
  id: string
  customerId: string
  salesforceUpdateAt: string
  name: string
  customerName: string
  serviceType: string
  serviceTypeName: string
  createNew: boolean
  mainSalesUserId: string
  mainSalesUserName: string
  phase: string
  customerSuccess: string
  supportDateFrom: string
  supportDateTo: string
  totalContractTime: number
  mainCustomerUserId: string
  mainCustomerUserName: string
  salesforceMainCustomer: ISalesforceMainCustomer[]
  customerUsers: { key: string }[]
  mainSupporterUserId: string
  mainSupporterUserName: string
  supporterOrganizationId: string
  supporterOrganizationName: string
  salesforceMainSupporterUserName: string
  supporterUsers: { key: string }[]
  salesforceSupporterUserNames: string[]
  isCountManHour: boolean
  isKarteRemind: boolean
  isMasterKarteRemind: boolean
  contractType: string
  isSecret: boolean
  isSolverProject: boolean
  updateAt: string
}

export interface IGetProjectsRequest {
  status: string
  fromYearMonth: number
  toYearMonth: number
  customerId: string
  mainSalesUserId: string
  supporterOrganizationId: string
  serviceTypeId: string
  sort: string
  offsetPage: number
  limit: number
}

export interface IGetProjectsResponse {
  offsetPage: number
  total: number
  projects: IProjectListItem[]
}

interface IGetProjectSortDefine {
  NAME_ASC: string
  [key: string]: string
}

export const GET_PROJECTS_REQUEST_SORT: IGetProjectSortDefine = {
  NAME_ASC: 'name:asc',
}
interface ISuggestProjectSortDefine {
  NAME_ASC: string
  [key: string]: string
}

export const SUGGEST_PROJECTS_REQUEST_SORT: ISuggestProjectSortDefine = {
  NAME_ASC: 'name:asc',
}

export interface ISuggestProjectsRequest {
  customerId?: string
  serviceType: string
  sort: string
}

export interface ISuggestProjectResponse {
  id: string
  name: string
  displayName: string
}

export interface ICustomerUser {
  id: string
  name: string
}

export interface ISupporterUser {
  id: string
  name: string
}

export interface IGetProjectByIdResponse {
  id: string
  customerId: string
  salesforceCustomerId: string
  salesforceOpportunityId: string
  salesforceUpdateAt: string
  name: string
  customerName: string
  serviceType: string
  serviceTypeName: string
  createNew: boolean | null
  mainSalesUserId: string
  mainSalesUserName: string
  phase: string
  customerSuccess: string
  supportDateFrom: string
  supportDateTo: string
  profit: IProfit
  totalContractTime?: number
  mainCustomerUserId: string
  mainCustomerUserName: string
  salesforceMainCustomer: ISalesforceMainCustomer
  customerUsers: ICustomerUser[]
  serviceManagerName: string
  mainSupporterUserId: string
  mainSupporterUserName: string
  supporterOrganizationId: string
  supporterOrganizationName: string
  salesforceMainSupporterUserName: string
  supporterUsers: ISupporterUser[]
  salesforceSupporterUserNames: string[]
  isCountManHour: boolean
  isKarteRemind: boolean
  isMasterKarteRemind: boolean
  contractType: string
  isSecret: boolean
  dedicatedSurveyUserEmail: string
  dedicatedSurveyUserName: string
  surveyPassword: string
  isSurveyEmailToSalesforceMainCustomer: boolean | null
  createId: string
  createUserName: string
  createAt: string
  updateId: string
  updateUserName: string
  updateAt: string
  version: number
}

export interface ICreateProjectRequest {
  customerId: string
  name: string
  customerName: string
  serviceType: string
  createNew: boolean
  mainSalesUserId: string
  phase: string
  customerSuccess: string
  supportDateFrom: string
  supportDateTo: string
  profit: IProfit[]
  totalContractTime: number
  mainCustomerUserId: string
  salesforceMainCustomer: ISalesforceMainCustomer[]
  customerUsers: { key: string }[]
  mainSupporterUserId: string
  supporterOrganizationId: string
  salesforceMainSupporterUserName: string
  supporterUsers: { key: string }[]
  salesforceSupporterUserNames: string[]
  isCountManHour: boolean
  isKarteRemind: boolean
  isMasterKarteRemind: boolean
  contractType: string
  isSecret: boolean
  dedicatedSurveyUserEmail: string
  dedicatedSurveyUserName: string
  surveyPassword: string
  isSurveyEmailToSalesforceMainCustomer: boolean | null
}

export interface ICreateProjectResponse {
  id: string
  customerId: string
  salesforceCustomerId: string
  salesforceOpportunityId: string
  salesforceUpdateAt: string
  name: string
  customerName: string
  serviceType: string
  serviceTypeName: string
  createNew: boolean
  mainSalesUserId: string
  mainSalesUserName: string
  phase: string
  customerSuccess: string
  supportDateFrom: string
  supportDateTo: string
  profit: IProfit[]
  totalContractTime: number
  mainCustomerUserId: string
  mainCustomerUserName: string
  salesforceMainCustomer: ISalesforceMainCustomer[]
  customerUsers: { key: string }[]
  mainSupporterUserId: string
  mainSupporterUserName: string
  supporterOrganizationId: string
  supporterOrganizationName: string
  salesforceMainSupporterUserName: string
  supporterUsers: { key: string }[]
  salesforceSupporterUserNames: string[]
  isCountManHour: boolean
  isKarteRemind: boolean
  isMasterKarteRemind: boolean
  contractType: string
  isSecret: boolean
  dedicatedSurveyUserEmail: string
  dedicatedSurveyUserName: string
  surveyPassword: string
  isSurveyEmailToSalesforceMainCustomer: boolean | null
  createId: string
  createUserName: string
  createAt: string
  updateId: string
  updateUserName: string
  updateAt: string
  version: number
}

export interface IUpdateProjectRequestParam {
  id: string
  version: number
}

export interface IUpdateProjectRequest {
  customerId: string
  name: string
  customerName: string
  serviceType: string
  createNew: boolean
  mainSalesUserId: string
  customerSuccess: string
  supportDateFrom: string
  supportDateTo: string
  profit: IProfit[]
  totalContractTime: number
  mainCustomerUserId: string
  salesforceMainCustomer: ISalesforceMainCustomer[]
  customerUsers: { key: string }[]
  mainSupporterUserId: string
  supporterOrganizationId: string
  supporterUsers: { key: string }[]
  isCountManHour: boolean
  isKarteRemind: boolean
  isMasterKarteRemind: boolean
  contractType: string
  isSecret: boolean
  dedicatedSurveyUserEmail: string
  dedicatedSurveyUserName: string
  surveyPassword: string
  isSurveyEmailToSalesforceMainCustomer: boolean | null
}

export interface IUpdateProjectResponse {
  id: string
  customerId: string
  salesforceCustomerId: string
  salesforceOpportunityId: string
  salesforceUpdateAt: string
  name: string
  customerName: string
  serviceType: string
  serviceTypeName: string
  createNew: boolean
  mainSalesUserId: string
  mainSalesUserName: string
  phase: string
  customerSuccess: string
  supportDateFrom: string
  supportDateTo: string
  profit: IProfit[]
  totalContractTime: number
  mainCustomerUserId: string
  mainCustomerUserName: string
  salesforceMainCustomer: ISalesforceMainCustomer[]
  dedicatedSurveyUserEmail: string
  dedicatedSurveyUserName: string
  surveyPassword: string
  isSurveyEmailToSalesforceMainCustomer: boolean | null
  customerUsers: { key: string }[]
  mainSupporterUserId: string
  mainSupporterUserName: string
  supporterOrganizationId: string
  supporterOrganizationName: string
  salesforceMainSupporterUserName: string
  supporterUsers: { key: string }[]
  salesforceSupporterUserNames: string[]
  isCountManHour: boolean
  isKarteRemind: boolean
  isMasterKarteRemind: boolean
  contractType: string
  isSecret: boolean
  createId: string
  createUserName: string
  createAt: string
  updateId: string
  updateUserName: string
  updateAt: string
  version: number
}

export const ENUM_CREATE_OR_UPDATE_PROJECT_RESPONSE_ERROR = {
  DISABLED_MAINSALES: 'disabled mainSales is set',
  DISABLED_MAINCUSTOMER: 'disabled mainCustomer is set',
  DISABLED_CUSTOMER: 'disabled customer is set',
  DISABLED_MAINSUPPORTER: 'disabled mainSupporter is set',
  DISABLED_SUPPORTER: 'disabled supporter is set',
} as const

export interface IDeleteProjectRequestParam {
  id: string
  version: number
}

export interface IDeleteProjectResponse {
  message: string
}

export const ENUM_IMPORT_PROJECTS_MODE = {
  CHECK: 'check',
  EXECUTE: 'execute',
} as const

export type importProjectsMode =
  typeof ENUM_IMPORT_PROJECTS_MODE[keyof typeof ENUM_IMPORT_PROJECTS_MODE]

export const ENUM_IMPORT_PROJECTS_RESULT = {
  OK: 'ok',
  NG: 'ng',
  DONE: 'done',
  ERROR: 'error',
} as const

export type importProjectsResult =
  typeof ENUM_IMPORT_PROJECTS_RESULT[keyof typeof ENUM_IMPORT_PROJECTS_RESULT]

interface IImportedProject {
  salesforceOpportunityId: string
  customerId: string
  salesforceUpdateAt: string
  name: string
  customerName: string
  serviceType: string
  salesforceUsePackage: boolean
  createNew: boolean
  salesforceViaPr: boolean
  salesforceMainCustomer: ISalesforceMainCustomer[]
  mainSalesUserName: string
  phase: string
  customerSuccess: string
  supportDateFrom: string
  supportDateTo: string
  profit: IProfit[]
  gross: IGross[]
  salesforceMainSupporterUserName: string
  supporterUsers: string[]
  totalContractTime: number
  errorMessage: string
}

export class ImportedProject implements IImportedProject {
  salesforceOpportunityId = ''
  customerId = ''
  salesforceUpdateAt = ''
  name = ''
  customerName = ''
  serviceType = ''
  salesforceUsePackage = true
  createNew = true
  salesforceViaPr = true
  salesforceMainCustomer = []
  mainSalesUserName = ''
  phase = ''
  customerSuccess = ''
  supportDateFrom = ''
  supportDateTo = ''
  profit = []
  gross = []
  salesforceMainSupporterUserName = ''
  supporterUsers = []
  totalContractTime = 0
  errorMessage = ''
}

export interface IImportProjectsRequest {
  file: string
  mode: importProjectsMode
}

export interface IImportProjectsResponse {
  mode: importProjectsMode
  result: importProjectsResult
  projects: ImportedProject[]
}

export interface IMember {
  supporterName: string
  role: string
  isConfirm: boolean
}

export interface IOrganization {
  supporterOrganizationName: string
  members: IMember[]
}

export interface IGetMonthlyProjectsRequest {
  year: number
  month: number
}

export interface IMonthlyProjectHeader {
  supporterOrganizationId: string
  supporterOrganizationName: string
}
export interface IGetMonthlyProjectResponse {
  serviceTypeName: string
  name: string
  contractType: string
  mainSalesUserName: string
  Organizations: IOrganization[]
  supportDateFrom: string
  supportDateTo: string
  totalContractTime: number
  thisMonthContractTime: number
  totalProfit: number
  thisMonthProfit: number
}

export interface IGetMonthlyProjectsResponse {
  details: IGetMonthlyProjectResponse[]
  header: IMonthlyProjectHeader[]
}
