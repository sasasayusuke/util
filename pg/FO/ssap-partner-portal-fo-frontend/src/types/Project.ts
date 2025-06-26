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
  updateAt: string
}

export const ENUM_GET_PROJECTS_REQUEST_SORT = {
  NAME_ASC: 'name:asc',
  CUSTOMER_NAME_ASC: 'customerName:asc',
  SUPPORT_DATE_FROM_DESC: 'supportDateFrom:desc',
} as const

export type GetProjectsRequestSort =
  typeof ENUM_GET_PROJECTS_REQUEST_SORT[keyof typeof ENUM_GET_PROJECTS_REQUEST_SORT]

export const ENUM_PROJECTS_STATUS = {
  PERFORMED: 'performed',
  PLAN: 'plan',
  PROGRESS: 'progress',
} as const

export type GetProjectsStatus =
  typeof ENUM_PROJECTS_STATUS[keyof typeof ENUM_PROJECTS_STATUS]

interface ISuggestProjectSortDefine {
  NAME_ASC: string
  [key: string]: string
}

export const SUGGEST_PROJECTS_REQUEST_SORT: ISuggestProjectSortDefine = {
  NAME_ASC: 'name:asc',
}

export interface ISuggestProjectsRequest {
  customerId?: string
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
  createNew: boolean
  mainSalesUserId: string
  mainSalesUserName: string
  phase: string
  customerSuccess: string
  supportDateFrom: string
  supportDateTo: string
  profit: IProfit
  totalContractTime: number
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
  isSolverProject: boolean
  dedicatedSurveyUserEmail: string
  dedicatedSurveyUserName: string
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

export const ENUM_UPDATE_PROJECT_RESPONSE_ERROR = {
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
