import { format } from 'date-fns'
import { getCurrentDate } from '~/utils/common-functions'
import {
  IProfit,
  ISalesforceMainCustomer,
  IProjectListItem,
  IGetProjectByIdResponse,
  ENUM_GET_PROJECTS_REQUEST_SORT,
  GetProjectsRequestSort,
  ICreateProjectRequest,
  ICreateProjectResponse,
  SUGGEST_PROJECTS_REQUEST_SORT,
  ISuggestProjectsRequest,
  ISuggestProjectResponse,
  IGross,
  IUpdateProjectRequest,
  IUpdateProjectResponse,
  IDeleteProjectRequestParam,
  IDeleteProjectResponse,
} from '@/types/Project'

import { Api } from '~/plugins/api'

const $api = new Api()

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
  public updateAt = ''
}

export class GetProjectsRequest {
  fromDate?: number
  toDate?: number
  fromYearMonth?: number
  toYearMonth?: number
  customerId?: string
  all?: boolean
  allAssigned?: boolean
  limit?: number
  offset?: number
  sort: GetProjectsRequestSort =
    ENUM_GET_PROJECTS_REQUEST_SORT.CUSTOMER_NAME_ASC
}

export class GetProjectsDateRequest extends GetProjectsRequest {
  toDate = 99991231 //最大年月
  all = false
  allAssigned = false
  limit = 20
  offsetPage = 1
}

export class GetProjectsDateNoLimitRequest extends GetProjectsRequest {
  toDate = 99991231 //最大年月
  all = false
  allAssigned = false
  limit = -1
  offsetPage = 1
}

export class GetProjectsYearMonthRequest extends GetProjectsRequest {
  fromYearMonth = parseInt(format(getCurrentDate(), 'yyyyMM'))
  toYearMonth = 999912 //最大年月
  all = false
  allAssigned = false
  limit = 20
  offsetPage = 1
}

export class GetProjectsYearMonthAllAssignedRequest extends GetProjectsRequest {
  fromYearMonth = parseInt(format(getCurrentDate(), 'yyyyMM'))
  toYearMonth = 999912 //最大年月
  all = false
  allAssigned = false
  limit = 20
  offsetPage = 1
}

export class GetProjectsCustomerRequest extends GetProjectsDateRequest {
  sort: GetProjectsRequestSort = ENUM_GET_PROJECTS_REQUEST_SORT.NAME_ASC
}

export class GetProjectsCustomerNoLimitRequest extends GetProjectsDateNoLimitRequest {
  sort: GetProjectsRequestSort = ENUM_GET_PROJECTS_REQUEST_SORT.NAME_ASC
}

export class GetProjectsResponse {
  offsetPage = 0
  total = 0
  projects: ProjectListItem[] = []
}

export async function GetProjects(request: GetProjectsRequest) {
  return await $api.get<GetProjectsResponse>(`/projects`, request)
}

export class SuggestProjectsRequest implements ISuggestProjectsRequest {
  customerId?: string
  public sort = SUGGEST_PROJECTS_REQUEST_SORT.NAME_ASC
}

export class SuggestProject implements ISuggestProjectResponse {
  public id = ''
  public name = ''
  public displayName = ''
}

export async function SuggestProjects(request: SuggestProjectsRequest) {
  return await $api.get<SuggestProjectsResponse>(`/projects/suggest`, request)
}

export class SimpleUser {
  public id = ''
  public name = ''
}

export class CustomerUser extends SimpleUser {}

export class SupporterUser extends SimpleUser {}

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
  public createNew = true
  public mainSalesUserId = ''
  public mainSalesUserName = ''
  public phase = ''
  public customerSuccess = ''
  public supportDateFrom = ''
  public supportDateTo = ''
  public profit = {} as Profit
  public totalContractTime = 0
  public mainCustomerUserId = ''
  public mainCustomerUserName = ''
  public salesforceMainCustomer = {} as SalesforceMainCustomer
  public customerUsers: CustomerUser[] = []
  public serviceManagerName = ''
  public mainSupporterUserId = ''
  public mainSupporterUserName = ''
  public supporterOrganizationId = ''
  public supporterOrganizationName = ''
  public salesforceMainSupporterUserName = ''
  public supporterUsers: SupporterUser[] = []
  public salesforceSupporterUserNames = []
  public isCountManHour = true
  public isKarteRemind = true
  public isMasterKarteRemind = true
  public contractType = ''
  public isSecret = true
  public isSolverProject = false
  public dedicatedSurveyUserEmail = ''
  public dedicatedSurveyUserName = ''
  public isSurveyEmailToSalesforceMainCustomer: boolean | null = true
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
