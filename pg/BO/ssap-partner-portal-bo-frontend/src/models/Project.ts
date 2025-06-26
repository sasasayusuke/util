import { format } from 'date-fns'
import { getCurrentDate } from '~/utils/common-functions'
import {
  IProfit,
  ISalesforceMainCustomer,
  IProjectListItem,
  IGetProjectByIdResponse,
  IGetProjectsRequest,
  GET_PROJECTS_REQUEST_SORT,
  IGetProjectsResponse,
  ICreateProjectRequest,
  ICreateProjectResponse,
  SUGGEST_PROJECTS_REQUEST_SORT,
  ISuggestProjectsRequest,
  ISuggestProjectResponse,
  IGross,
  IImportProjectsRequest,
  IImportProjectsResponse,
  importProjectsMode,
  importProjectsResult,
  ImportedProject,
  IUpdateProjectRequest,
  IUpdateProjectResponse,
  IDeleteProjectRequestParam,
  IDeleteProjectResponse,
  ICustomerUser,
  ISupporterUser,
  IGetMonthlyProjectsRequest,
  IMonthlyProjectHeader,
  IGetMonthlyProjectResponse,
  IGetMonthlyProjectsResponse,
} from '@/types/Project'
import { Api } from '~/plugins/api'
export {
  ENUM_IMPORT_PROJECTS_MODE,
  ENUM_IMPORT_PROJECTS_RESULT,
} from '@/types/Project'

const $api = new Api()

const ENUM_EXCLUDE_SANITIZE_REQUEST_KEYS: string[] = [
  'dedicatedSurveyUserName',
  'dedicatedSurveyUserEmail',
]

export class Profit implements IProfit {
  public monthly = []
  public quarterly = []
  public half = []
  public year = 0
}

export class Gross implements IGross {
  public monthly = []
  public quarterly = []
  public half = []
  public year = 0
}

export class SalesforceMainCustomer implements ISalesforceMainCustomer {
  public name = ''
  public email = ''
  public organizationName = ''
}

export class ProjectListItem implements IProjectListItem {
  public id = ''
  public customerId = ''
  public salesforceUpdateAt = ''
  public name = ''
  public customerName = ''
  public serviceType = ''
  public serviceTypeName = ''
  public createNew = true
  public mainSalesUserId = ''
  public mainSalesUserName = ''
  public phase = ''
  public customerSuccess = ''
  public supportDateFrom = ''
  public supportDateTo = ''
  public totalContractTime = 0
  public mainCustomerUserId = ''
  public mainCustomerUserName = ''
  public salesforceMainCustomer: SalesforceMainCustomer[] = []
  public customerUsers = []
  public mainSupporterUserId = ''
  public mainSupporterUserName = ''
  public supporterOrganizationId = ''
  public supporterOrganizationName = ''
  public salesforceMainSupporterUserName = ''
  public supporterUsers = []
  public salesforceSupporterUserNames = []
  public isCountManHour = true
  public isKarteRemind = true
  public isMasterKarteRemind = true
  public contractType = ''
  public isSecret = true
  public isSolverProject = false
  public updateAt = ''
}

export class GetProjectsRequest implements IGetProjectsRequest {
  public status = 'during'
  public fromYearMonth = parseInt(format(getCurrentDate(), 'yyyyMM'))
  public toYearMonth = 999912 //最大年月
  public customerId = ''
  public mainSalesUserId = ''
  public supporterOrganizationId = ''
  public serviceTypeId = ''
  public sort = GET_PROJECTS_REQUEST_SORT.NAME_ASC
  public offsetPage = 1
  public limit = 20
  public isKarteUsageProjectListOfSales = false
}

export class GetProjectsResponse implements IGetProjectsResponse {
  offsetPage = 0
  total = 0
  projects: ProjectListItem[] = []
}

export async function GetProjects(request: GetProjectsRequest) {
  return await $api.get<GetProjectsResponse>(`/projects`, request)
}

export class SuggestProjectsRequest implements ISuggestProjectsRequest {
  public customerId = ''
  public serviceType = ''
  public sort = SUGGEST_PROJECTS_REQUEST_SORT.NAME_ASC
}

export class SuggestProject implements ISuggestProjectResponse {
  public id = ''
  public name = ''
  public displayName = ''
}

export class CustomerUser implements ICustomerUser {
  public id = ''
  public name = ''
}

export class SupporterUser implements ISupporterUser {
  public id = ''
  public name = ''
}

export class SuggestProjectsResponse extends Array<SuggestProject> {}

export class GetProjectByIdResponse implements IGetProjectByIdResponse {
  public id = ''
  public customerId = ''
  public salesforceCustomerId = ''
  public salesforceOpportunityId = ''
  public salesforceUpdateAt = ''
  public name = ''
  public customerName = ''
  public serviceType = ''
  public serviceTypeName = ''
  public createNew: null | boolean = null
  public mainSalesUserId = ''
  public mainSalesUserName = ''
  public phase = ''
  public customerSuccess = ''
  public supportDateFrom = ''
  public supportDateTo = ''
  public profit = {} as Profit
  public totalContractTime?: number
  public mainCustomerUserId = ''
  public mainCustomerUserName = ''
  public salesforceMainCustomer = new SalesforceMainCustomer()
  public customerUsers: CustomerUser[] = []
  public serviceManagerName = ''
  public mainSupporterUserId = ''
  public mainSupporterUserName = ''
  public supporterOrganizationId = ''
  public supporterOrganizationName = ''
  public salesforceMainSupporterUserName = ''
  public supporterUsers: SupporterUser[] = []
  public salesforceSupporterUserNames: string[] = []
  public isCountManHour = true
  public isKarteRemind = true
  public isMasterKarteRemind = true
  public contractType = '無償'
  public isSecret = true
  public isSolverProject = false
  public dedicatedSurveyUserEmail = ''
  public dedicatedSurveyUserName = ''
  public surveyPassword = ''
  public isSurveyEmailToSalesforceMainCustomer: null | boolean = true
  public createId = ''
  public createUserName = ''
  public createAt = ''
  public updateId = ''
  public updateUserName = ''
  public updateAt = ''
  public version = 0
}

export async function GetProjectById(id: string) {
  return await $api.get<GetProjectByIdResponse>(`/projects/${id}`)
}

export class CreateProjectRequest implements ICreateProjectRequest {
  public customerId = ''
  public name = ''
  public customerName = ''
  public serviceType = ''
  public createNew = true
  public mainSalesUserId = ''
  public phase = ''
  public customerSuccess = ''
  public supportDateFrom = ''
  public supportDateTo = ''
  public profit: Profit[] = []
  public totalContractTime = 0
  public mainCustomerUserId = ''
  public salesforceMainCustomer: SalesforceMainCustomer[] = []
  public customerUsers = []
  public mainSupporterUserId = ''
  public supporterOrganizationId = ''
  public salesforceMainSupporterUserName = ''
  public supporterUsers = []
  public salesforceSupporterUserNames = []
  public isCountManHour = true
  public isKarteRemind = true
  public isMasterKarteRemind = true
  public contractType = ''
  public isSecret = true
  public dedicatedSurveyUserEmail = ''
  public dedicatedSurveyUserName = ''
  public surveyPassword = ''
  public isSurveyEmailToSalesforceMainCustomer: null | boolean = null
}

export class CreateProjectResponse implements ICreateProjectResponse {
  public id = ''
  public customerId = ''
  public salesforceCustomerId = ''
  public salesforceOpportunityId = ''
  public salesforceUpdateAt = ''
  public name = ''
  public customerName = ''
  public serviceType = ''
  public serviceTypeName = ''
  public createNew = true
  public mainSalesUserId = ''
  public mainSalesUserName = ''
  public phase = ''
  public customerSuccess = ''
  public supportDateFrom = ''
  public supportDateTo = ''
  public profit: Profit[] = []
  public totalContractTime = 0
  public mainCustomerUserId = ''
  public mainCustomerUserName = ''
  public salesforceMainCustomer: SalesforceMainCustomer[] = []
  public customerUsers = []
  public mainSupporterUserId = ''
  public mainSupporterUserName = ''
  public supporterOrganizationId = ''
  public supporterOrganizationName = ''
  public salesforceMainSupporterUserName = ''
  public supporterUsers = []
  public salesforceSupporterUserNames = []
  public isCountManHour = true
  public isKarteRemind = true
  public isMasterKarteRemind = true
  public contractType = ''
  public isSecret = true
  public dedicatedSurveyUserEmail = ''
  public dedicatedSurveyUserName = ''
  public surveyPassword = ''
  public isSurveyEmailToSalesforceMainCustomer: null | boolean = null
  public createId = ''
  public createUserName = ''
  public createAt = ''
  public updateId = ''
  public updateUserName = ''
  public updateAt = ''
  public version = 0
}

export async function CreateProject(request: CreateProjectRequest) {
  return await $api.post<CreateProjectResponse>(`/projects`, request)
}

export class UpdateProjectRequestParam {
  public id = ''
  public version = 0
}

export class UpdateProjectRequest implements IUpdateProjectRequest {
  public customerId = ''
  public name = ''
  public customerName = ''
  public serviceType = ''
  public createNew = true
  public mainSalesUserId = ''
  public customerSuccess = ''
  public supportDateFrom = ''
  public supportDateTo = ''
  public profit: Profit[] = []
  public totalContractTime = 0
  public mainCustomerUserId = ''
  public salesforceMainCustomer: SalesforceMainCustomer[] = []
  public customerUsers = []
  public mainSupporterUserId = ''
  public supporterOrganizationId = ''
  public supporterUsers = []
  public isCountManHour = true
  public isKarteRemind = true
  public isMasterKarteRemind = true
  public contractType = ''
  public isSecret = true
  public dedicatedSurveyUserEmail = ''
  public dedicatedSurveyUserName = ''
  public surveyPassword = ''
  public isSurveyEmailToSalesforceMainCustomer: null | boolean = null
}

export class UpdateProjectResponse implements IUpdateProjectResponse {
  public id = ''
  public customerId = ''
  public salesforceCustomerId = ''
  public salesforceOpportunityId = ''
  public salesforceUpdateAt = ''
  public name = ''
  public customerName = ''
  public serviceType = ''
  public serviceTypeName = ''
  public createNew = true
  public mainSalesUserId = ''
  public mainSalesUserName = ''
  public phase = ''
  public customerSuccess = ''
  public supportDateFrom = ''
  public supportDateTo = ''
  public profit: Profit[] = []
  public totalContractTime = 0
  public mainCustomerUserId = ''
  public mainCustomerUserName = ''
  public salesforceMainCustomer: SalesforceMainCustomer[] = []
  public dedicatedSurveyUserEmail = ''
  public dedicatedSurveyUserName = ''
  public surveyPassword = ''
  public isSurveyEmailToSalesforceMainCustomer: null | boolean = null
  public customerUsers = []
  public mainSupporterUserId = ''
  public mainSupporterUserName = ''
  public supporterOrganizationId = ''
  public supporterOrganizationName = ''
  public salesforceMainSupporterUserName = ''
  public supporterUsers = []
  public salesforceSupporterUserNames = []
  public isCountManHour = true
  public isKarteRemind = true
  public isMasterKarteRemind = true
  public contractType = ''
  public isSecret = true
  public createId = ''
  public createUserName = ''
  public createAt = ''
  public updateId = ''
  public updateUserName = ''
  public updateAt = ''
  public version = 0
}

export async function UpdateProjectById(
  id: string,
  version: number,
  request: UpdateProjectRequest
) {
  return await $api.put<UpdateProjectResponse>(
    `/projects/${id}?version=${version}`,
    request
  )
}

export class DeleteProjectRequestParam implements IDeleteProjectRequestParam {
  public id = ''
  public version = 0
}

export class DeleteProjectResponse implements IDeleteProjectResponse {
  public message = ''
}

export async function DeleteProjectById(id: string, version: number) {
  return await $api.delete<DeleteProjectResponse>(
    `/projects/${id}?version=${version}`
  )
}

export class ImportProjectsRequest implements IImportProjectsRequest {
  file = 'https://'
  mode = '' as importProjectsMode
}

export class ImportProjectsResponse implements IImportProjectsResponse {
  mode = '' as importProjectsMode
  result = '' as importProjectsResult
  projects = [] as ImportedProject[]
}

export function ImportProjects(
  request: ImportProjectsRequest = new ImportProjectsRequest()
) {
  return $api.post<ImportProjectsResponse>('/projects/import', request)
}

export async function GetSuggestProjects(
  request: SuggestProjectsRequest = new SuggestProjectsRequest()
) {
  return await $api.get<SuggestProjectsResponse>('/projects/suggest', request)
}

export class GetMonthlyProjectsRequest implements IGetMonthlyProjectsRequest {
  public year = parseInt(format(getCurrentDate(), 'yyyy'))
  public month = parseInt(format(getCurrentDate(), 'MM'))
}

export class MonthlyProjectHeader implements IMonthlyProjectHeader {
  public supporterOrganizationId = ''
  public supporterOrganizationName = ''
}
export class GetMonthlyProjectResponse implements IGetMonthlyProjectResponse {
  public serviceTypeName = ''
  public name = ''
  public contractType = ''
  public mainSalesUserName = ''
  public Organizations = []
  public supportDateFrom = ''
  public supportDateTo = ''
  public totalContractTime = 0
  public thisMonthContractTime = 0
  public totalProfit = 0
  public thisMonthProfit = 0
}
export class GetMonthlyProjectsResponse implements IGetMonthlyProjectsResponse {
  public details = [] as GetMonthlyProjectResponse[]
  public header = [] as MonthlyProjectHeader[]
}

export async function GetMonthlyProjects(year: number, month: number) {
  return await $api.get<GetMonthlyProjectsResponse>(
    `/projects/summary/${year}/${month}`
  )
}
