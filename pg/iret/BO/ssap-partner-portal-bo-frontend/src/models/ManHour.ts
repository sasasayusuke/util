import { format } from 'date-fns'

import { getCurrentDate } from '~/utils/common-functions'
import {
  IGetSummaryManHourTypeResponse,
  IManHour,
  IGetSummaryManHourTypeRequest,
  IGetSummaryServiceTypesManHoursRequest,
  IGetSummaryServiceTypesManHoursResponse,
  IGetSummarySupporterOrganizationsManHoursRequest,
  IGetSummarySupporterOrganizationsManHourResponse,
  IProject,
  IGetSummaryProjectManHourAlertsRequest,
  IGetSupporterOrganizationsRequest,
  ISummaryManHour,
  IContractTime,
  ISummarySupporterManHour,
  IGetSummarySupporterManHoursRequest,
  IGetSummarySupporterManHoursResponse,
  IServiceTypeItem,
  IGetServiceTypesRequest,
  ISupporterOrganization,
  ISummaryManHourTypeHeader,
  IgetSummaryProjectManHourAlertsResponse,
  ISummaryProjectManHourAlertsProjectItems,
  IGetSummaryProjectManHourAlertDetailResponse,
  IManHours,
  IDirectSupportManHoursItem,
  IDirectSupportManHours,
  IPreSupportManHoursItem,
  IPreSupportManHours,
  ISalesSupportManHoursItem,
  ISalesSupportManHours,
  ISsapManHours,
  IHolidaysManHours,
  ISummarySupportManHours,
  IGetManHourBySupporterUserIdResponse,
  IGetManHourBySupporterUserIdRequest,
  IUpdateManHourRequest,
  IUpdateManHourResponse,
  IUpdateManHourRequestParams,
  IUpdateManHourBySupporterUserIdRequest,
} from '@/types/ManHour'
import { Api } from '~/plugins/api'

const $api = new Api()

export class SummaryManHourTypeHeader implements ISummaryManHourTypeHeader {
  supporterOrganizationManHours = []
  supporterOrganizationTotal = []
}
export class ManHourType implements IManHour {
  manHourTypeName = ''
  subName = ''
  serviceTypeName = ''
  roleName = ''
  supporterOrganizationTotal = []
  supporterOrganizationManHours = []
}

export class GetSummaryManHourTypeResponse
  implements IGetSummaryManHourTypeResponse
{
  yearMonth = ''
  header = [] as ISummaryManHourTypeHeader[]
  manHours = [] as ManHourType[]
}

export class Project implements IProject {
  projectId = ''
  projectName = ''
  customerId = ''
  contractType = ''
  thisMonthDirectSupportManHourMain = 0
  thisMonthDirectSupportManHourSub = 0
  thisMonthPreSupportManHour = 0
  thisMonthContractTime = 0
  totalProcessYPercent = 0
}

export class GetSummaryServiceTypesManHoursRequest
  implements IGetSummaryServiceTypesManHoursRequest
{
  year = parseInt(format(getCurrentDate(), 'yyyy'))
  month = parseInt(format(getCurrentDate(), 'MM'))
}

export class GetSummaryServiceTypesManHoursResponse
  implements IGetSummaryServiceTypesManHoursResponse
{
  serviceTypeId = ''
  serviceTypeName = ''
  directSupportManHourFactor = 0
  projects = [] as Project[]
}

export async function GetSummaryServiceTypesManHours(
  request: GetSummaryServiceTypesManHoursRequest
) {
  return await $api.get<GetSummaryServiceTypesManHoursResponse[]>(
    `man-hours/summary/service-types`,
    request
  )
}

export class GetSummarySupporterOrganizationsManHoursRequest
  implements IGetSummarySupporterOrganizationsManHoursRequest
{
  year = parseInt(format(getCurrentDate(), 'yyyy'))
  month = parseInt(format(getCurrentDate(), 'MM'))
  supporterOrganizationId?: string
}

export class GetSummarySupporterOrganizationsManHourResponse
  implements IGetSummarySupporterOrganizationsManHourResponse
{
  supporterOrganizationId = ''
  supporterOrganizationName = ''
  annualSales = 0
  monthlySales = 0
  monthlyProjectPrice = 0
  monthlyContractTime = 0
  monthlyWorkTime = 0
  monthlyWorkTimeRate = 0
  monthlySupporters = 0
  monthlyManHour = 0
  monthlyOccupancyRate = 0
  monthlyOccupancyTotalTime = 0
  monthlyOccupancyTotalRate = 0
  updateAt = ''
}

export async function GetSummarySupporterOrganizationsManHours(
  request: GetSummarySupporterOrganizationsManHoursRequest
) {
  return await $api.get<GetSummarySupporterOrganizationsManHourResponse[]>(
    `man-hours/summary/supporter-organizations`,
    request
  )
}

export class GetSummaryManHourTypeRequest
  implements IGetSummaryManHourTypeRequest
{
  year = parseInt(format(getCurrentDate(), 'yyyy'))
  month = parseInt(format(getCurrentDate(), 'MM'))
}

export async function GetSummaryManHourType(
  request: GetSummaryManHourTypeRequest
) {
  return await $api.get<GetSummaryManHourTypeResponse>(
    `/man-hours/summary/type`,
    request
  )
}

export class GetSummaryProjectManHourAlertsRequest
  implements IGetSummaryProjectManHourAlertsRequest
{
  public year = 0
  public month = 0
  public supporterOrganizationId? = ''
  public serviceTypeId? = ''
}

export async function GetSummaryProjectManHourAlerts(
  request: GetSummaryProjectManHourAlertsRequest
) {
  return await $api.get<GetSummaryProjectManHourAlertsResponse>(
    `/man-hours/summary/project-man-hour-alerts`,
    request
  )
}

export class SummaryManHour implements ISummaryManHour {
  public direct = 0
  public pre = 0
  public sales = 0
  public ssap = 0
  public others = 0
  public total = 0
}

export class ContractTime implements IContractTime {
  public producer = 0
  public accelerator = 0
  public total = 0
}

export class SummarySupporterManHour implements ISummarySupporterManHour {
  public supporterOrganizationId = ''
  public supporterOrganizationName = ''
  public supporterId = ''
  public supporterName = ''
  public summaryManHour = [] as SummaryManHour[]
  public contractTime = [] as ContractTime[]
  public isConfirm = true
}

export class GetSummarySupporterManHoursRequest
  implements IGetSummarySupporterManHoursRequest
{
  year = parseInt(format(getCurrentDate(), 'yyyy'))
  month = parseInt(format(getCurrentDate(), 'MM'))
}

export class GetSummarySupporterManHoursResponse
  implements IGetSummarySupporterManHoursResponse
{
  yearMonth = ''
  manHours = [] as SummarySupporterManHour[]
}

export async function GetSummarySupporterManHours(
  request: GetSummarySupporterManHoursRequest
) {
  return await $api.get<GetSummarySupporterManHoursResponse>(
    `/man-hours/summary/supporter`,
    request
  )
}

//本来Masterから取得
export class SupporterOrganization implements ISupporterOrganization {
  public id = ''
  public name = ''
  public shortName = ''
}

export class GetSupporterOrganizationsRequest
  implements IGetSupporterOrganizationsRequest
{
  public year = parseInt(format(getCurrentDate(), 'yyyy'))
  public month = parseInt(format(getCurrentDate(), 'MM'))
}

export class GetSupporterOrganizationsResponse extends Array<SupporterOrganization> {}

export async function GetSupporterOrganizations(
  request: GetSupporterOrganizationsRequest
) {
  return await $api.get<GetSupporterOrganizationsResponse>(
    `/masters/supporter-organizations`,
    request
  )
}
export class ServiceTypeItem implements IServiceTypeItem {
  public id = ''
  public name = ''
}

export class ServiceTypeRequest implements IGetServiceTypesRequest {
  public year = parseInt(format(getCurrentDate(), 'yyyy'))
  public month = parseInt(format(getCurrentDate(), 'MM'))
}

export class GetServiceTypesResponse extends Array<ServiceTypeItem> {}

export async function GetServiceTypes(request: IGetServiceTypesRequest) {
  return await $api.get<GetServiceTypesResponse>(
    `/masters/service-types`,
    request
  )
}

export class GetSummaryProjectManHourAlertsResponse
  implements IgetSummaryProjectManHourAlertsResponse
{
  summaryThisMonthContractTime = 0
  projects = [] as ISummaryProjectManHourAlertsProjectItems[]
}

export class ManHourResponse implements IManHours {
  supporterUserId = ''
  supporterUserName = ''
  role = ''
  thisMonthSupporterDirectSupportManHours = 0
  thisMonthSupporterPreSupportManHours = 0
  summarySupporterDirectSupportManHours = 0
  summarySupporterPreSupportManHours = 0
}

export class GetSummaryProjectManHourAlertDetailResponse
  implements IGetSummaryProjectManHourAlertDetailResponse
{
  projectId = ''
  projectName = ''
  customerId = ''
  customerName = ''
  manHours: ManHourResponse[] = []
}

export class GetSummaryProjectManHourAlertDetailRequest {
  public year: number = 0
  public month: number = 0
  public projectId: string = ''
}

export async function getSummaryProjectManHourAlertDetail(
  request: GetSummaryProjectManHourAlertDetailRequest
) {
  return await $api.get<GetSummaryProjectManHourAlertDetailResponse>(
    `/man-hours/summary/project-man-hour-alert`,
    request
  )
}

export class DirectSupportManHoursItem implements IDirectSupportManHoursItem {
  public projectId = ''
  public projectName = ''
  public role = ''
  public serviceType = ''
  public kartenManHour = 0
  public inputManHour = 0
}
export class DirectSupportManHours implements IDirectSupportManHours {
  public items = [] as DirectSupportManHoursItem[]
  public memo = ''
}
export class PreSupportManHoursItem implements IPreSupportManHoursItem {
  public projectId = ''
  public projectName = ''
  public role = ''
  public serviceType = ''
  public inputManHour = 0
}
export class PreSupportManHours implements IPreSupportManHours {
  public items = [] as PreSupportManHoursItem[]
  public memo = ''
}

export class SalesSupportManHoursItem implements ISalesSupportManHoursItem {
  public projectName = ''
  public customerId = ''
  public customerName = ''
  public type = ''
  public inputManHour = 0
}
export class SalesSupportManHours implements ISalesSupportManHours {
  public items = [] as SalesSupportManHoursItem[]
  public memo = ''
}

export class SsapManHours implements ISsapManHours {
  public meeting = 0
  public study = 0
  public learning = 0
  public newService = 0
  public startdash = 0
  public improvement = 0
  public ssap = 0
  public qc = 0
  public accounting = 0
  public management = 0
  public officeWork = 0
  public others = 0
  public memo = ''
}

export class HolidaysManHours implements IHolidaysManHours {
  public paidHoliday = 0
  public holiday = 0
  public private = 0
  public others = 0
  public departmentOthers = 0
  public memo = ''
}

export class SummarySupportManHours implements ISummarySupportManHours {
  public direct = 0
  public pre = 0
  public sales = 0
  public ssap = 0
  public others = 0
  public total = 0
}

export class GetManHourBySupporterUserIdResponse
  implements IGetManHourBySupporterUserIdResponse
{
  public yearMonth = ''
  public supporterUserId = ''
  public supporterUserName = ''
  public supporterOrganizationId = ''
  public supporterOrganizationName = ''
  public directSupportManHours = [] as DirectSupportManHours[]
  public preSupportManHours = [] as PreSupportManHours[]
  public salesSupportManHours = [] as SalesSupportManHours[]
  public ssapManHours = {} as SsapManHours
  public holidaysManHours = {} as HolidaysManHours
  public summarySupportManHours = {} as SummarySupportManHours
  public isConfirm = true
  public createId = ''
  public createUserName = ''
  public createAt = ''
  public updateId = ''
  public updateUserName = ''
  public updateAt = ''
  public version = 0
}

export class GetManHourBySupporterUserIdRequest
  implements IGetManHourBySupporterUserIdRequest
{
  public year = parseInt(format(getCurrentDate(), 'yyyy'))
  public month = parseInt(format(getCurrentDate(), 'MM'))
}

export async function GetManHourBySupporterUserId(
  supporterUserId: string,
  request: GetManHourBySupporterUserIdRequest
) {
  return await $api.get<GetManHourBySupporterUserIdResponse>(
    `/man-hours/${supporterUserId}`,
    request
  )
}

export class UpdateManHourRequestParams implements IUpdateManHourRequestParams {
  public id = ''
  public version = 0
  public year = parseInt(format(getCurrentDate(), 'yyyy'))
  public month = parseInt(format(getCurrentDate(), 'MM'))
}

export class UpdateManHourRequest implements IUpdateManHourRequest {
  public supporterUserName = ''
  public supporterOrganizationId = ''
  public supporterOrganizationName = ''
  public directSupportManHours = [] as DirectSupportManHours[]
  public preSupportManHours = [] as PreSupportManHours[]
  public salesSupportManHours = [] as SalesSupportManHours[]
  public ssapManHours = {} as SsapManHours
  public holidaysManHours = {} as HolidaysManHours
  public summarySupportManHours = {} as SummarySupportManHours
  public isConfirm = true
}

export class UpdateManHourResponse implements IUpdateManHourResponse {
  public yearMonth = ''
  public supporterUserId = ''
  public supporterUserName = ''
  public supporterOrganizationId = ''
  public supporterOrganizationName = ''
  public directSupportManHours = [] as DirectSupportManHours[]
  public preSupportManHours = [] as PreSupportManHours[]
  public salesSupportManHours = [] as SalesSupportManHours[]
  public ssapManHours = {} as SsapManHours
  public holidaysManHours = {} as HolidaysManHours
  public summarySupportManHours = {} as SummarySupportManHours
  public isConfirm = true
  public createId = ''
  public createUserName = ''
  public createAt = ''
  public updateId = ''
  public updateUserName = ''
  public updateAt = ''
  public version = 0
}

export class UpdateManHourBySupporterUserIdRequest
  implements IUpdateManHourBySupporterUserIdRequest
{
  public supporterUserName = ''
  public supporterOrganizationId = ''
  public supporterOrganizationName = ''
  public directSupportManHours = [] as DirectSupportManHours[]
  public preSupportManHours = [] as PreSupportManHours[]
  public salesSupportManHours = [] as SalesSupportManHours[]
  public ssapManHours = {} as SsapManHours
  public holidaysManHours = {} as HolidaysManHours
  public isConfirm = true
}

export async function UpdateManHourBySupporterUserId(
  supporterUserId: string,
  version: number,
  year: number,
  month: number,
  request: UpdateManHourBySupporterUserIdRequest
) {
  return await $api.put<UpdateManHourResponse>(
    `/man-hours/${supporterUserId}?version=${version}&year=${year}&month=${month}`,
    request
  )
}
