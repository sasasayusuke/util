export interface ISupporterOrganizationTotal {
  supporterOrganizationName: string
  manHour: number
}

export interface ISupporter {
  name: string
  manHour: number
}

export interface ISupporterOrganizationManHours {
  supporterOrganizationName: string
  supporters: ISupporter[]
}

export interface ISummaryManHourTypeHeader {
  supporterOrganizationManHours: ISupporterOrganizationManHours[]
  supporterOrganizationTotal: ISupporterOrganizationTotal[]
}

export interface IManHour {
  manHourTypeName: string
  subName: string
  serviceTypeName: string
  roleName: string
  supporterOrganizationTotal: ISupporterOrganizationTotal[]
  supporterOrganizationManHours: ISupporterOrganizationManHours[]
}

export interface IGetSummaryManHourTypeRequest {
  year: number
  month: number
}

export interface IGetSummaryManHourTypeResponse {
  yearMonth: string
  header: ISummaryManHourTypeHeader[]
  manHours: IManHour[]
}

export interface ISummaryManHour {
  direct: number
  pre: number
  sales: number
  ssap: number
  others: number
  total: number
}

export interface IContractTime {
  producer: number
  accelerator: number
  total: number
}
export interface ISummarySupporterManHour {
  supporterOrganizationId: string
  supporterOrganizationName: string
  supporterId: string
  supporterName: string
  summaryManHour: ISummaryManHour[]
  contractTime: IContractTime[]
  isConfirm: boolean
}
export interface IGetSummarySupporterManHoursRequest {
  year: number
  month: number
}

export interface IGetSummarySupporterManHoursResponse {
  yearMonth: string
  manHours: ISummarySupporterManHour[]
}

export interface IProject {
  projectId: string
  projectName: string
  customerId: string
  contractType: string
  thisMonthDirectSupportManHourMain: number
  thisMonthDirectSupportManHourSub: number
  thisMonthPreSupportManHour: number
  thisMonthContractTime: number
  totalProcessYPercent: number
}

export interface IGetSummaryServiceTypesManHoursResponse {
  serviceTypeId: string
  serviceTypeName: string
  directSupportManHourFactor: number
  projects: IProject[]
}

export interface IGetSummaryServiceTypesManHoursRequest {
  year: number
  month: number
}

export interface IGetSummarySupporterOrganizationsManHourResponse {
  supporterOrganizationId: string
  supporterOrganizationName: string
  annualSales: number
  monthlySales: number
  monthlyProjectPrice: number
  monthlyContractTime: number
  monthlyWorkTime: number
  monthlyWorkTimeRate: number
  monthlySupporters: number
  monthlyManHour: number
  monthlyOccupancyRate: number
  monthlyOccupancyTotalTime: number
  monthlyOccupancyTotalRate: number
  updateAt: string
}
export interface IGetSummarySupporterOrganizationsManHoursRequest {
  year: number
  month: number
  supporterOrganizationId?: string
}

export interface IMainSupporterUser {
  id: string
  name: string
}

export interface ISupporterUser {
  id: string
  name: string
}

export interface IAlertProject {
  projectId: string
  projectName: string
  customerId: string
  customerName: string
  supporterOrganizationId: string
  supporterOrganizationName: string
  serviceType: string
  serviceTypeName: string
  supportDateFrom: string
  supportDateTo: string
  totalContractTime: number
  thisMonthContractTime: number
  totalProfit: number
  thisMonthProfit: number
  mainSupporterUser: IMainSupporterUser
  supporterUsers: ISupporterUser[]
  summaryDirectSupportManHour: number
  summaryPreSupportManHour: number
  thisMonthDirectSupportManHour: number
  thisMonthPreSupportManHour: number
  summaryTheoreticalDirectSupportManHour: number
  summaryTheoreticalPreSupportManHour: number
  thisMonthTheoreticalDirectSupportManHour: number
  thisMonthTheoreticalPreSupportManHour: number
}

export interface IGetSummaryProjectManHourAlertsResponse {
  summaryThisMonthContractTime: number
  projects: IAlertProject[]
}

export interface IAlertManHour {
  supporterUserId: 'string'
  supporterUserName: 'string'
  role: 'string'
  thisMonthSupporterDirectSupportManHours: 0
  thisMonthSupporterPreSupportManHours: 0
  summarySupporterDirectSupportManHours: 0
  summarySupporterPreSupportManHours: 0
}

export interface IGetSummaryProjectManHourAlertRequest {
  year: number
  month: number
  serviceTypeId: string
}

export interface IGetSummaryProjectManHourAlertResponse {
  projectId: string
  projectName: string
  customerId: string
  customerName: string
  manHours: IAlertManHour[]
}

export interface ISupporterOrganization {
  id: string
  name: string
  shortName: string
}

export interface IGetSupporterOrganizationsRequest {
  year: number
  month: number
}
export interface IGetSupporterOrganizationsResponse {
  supporterOrganizations: ISupporterOrganization[]
}

export interface IServiceTypeItem {
  id: string
  name: string
}

export interface IGetServiceTypesRequest {
  year: number
  month: number
}

export interface IGetServiceTypesResponse {
  serviceTypes: IServiceTypeItem[]
}

export interface ISupporterUsers {
  id: string
  name: string
}

export interface ISummaryProjectManHourAlertsProjectItems {
  projectId: string
  projectName: string
  customerId: string
  customerName: string
  supporterOrganizationId: string
  supporterOrganizationName: string
  serviceType: string
  serviceTypeName: string
  supportDateFrom: string
  supportDateTo: string
  totalContractTime: string
  thisMonthContractTime: string
  totalProfit: string
  thisMonthProfit: string
  mainSupporterUser: IMainSupporterUser
  supporterUsers: ISupporterUsers[]
  summaryDirectSupportManHour: string
  summaryPreSupportManHour: string
  thisMonthDirectSupportManHour: number
  thisMonthPreSupportManHour: number
  summaryTheoreticalDirectSupportManHour: number
  summaryTheoreticalPreSupportManHour: number
  thisMonthTheoreticalDirectSupportManHour: number
  thisMonthTheoreticalPreSupportManHour: number
}

export interface IgetSummaryProjectManHourAlertsResponse {
  summaryThisMonthContractTime: number
  projects: ISummaryProjectManHourAlertsProjectItems[]
}

export interface IGetSummaryProjectManHourAlertsRequest {
  year: number
  month: number
  supporterOrganizationId?: string
  serviceTypeId?: string
}

export interface IManHours {
  supporterUserId: string
  supporterUserName: string
  role: string
  thisMonthSupporterDirectSupportManHours: number
  thisMonthSupporterPreSupportManHours: number
  summarySupporterDirectSupportManHours: number
  summarySupporterPreSupportManHours: number
}

export interface IGetSummaryProjectManHourAlertDetailResponse {
  projectId: string
  projectName: string
  customerId: string
  customerName: string
  manHours: IManHours[]
}

export interface IDirectSupportManHoursItem {
  projectId: string
  projectName: string
  role: string
  serviceType: string
  kartenManHour: number
  inputManHour: number
}
export interface IDirectSupportManHours {
  items: IDirectSupportManHoursItem[]
  memo: string
}

export interface IPreSupportManHoursItem {
  projectId: string
  projectName: string
  role: string
  serviceType: string
  inputManHour: number
}

export interface IPreSupportManHours {
  items: IPreSupportManHoursItem[]
  memo: string
}

export interface ISalesSupportManHoursItem {
  projectName: string
  customerId: string
  customerName: string
  type: string
  inputManHour: number
}

export interface ISalesSupportManHours {
  items: ISalesSupportManHoursItem[]
  memo: string
}

export interface ISsapManHours {
  meeting: number
  study: number
  learning: number
  newService: number
  startdash: number
  improvement: number
  ssap: number
  qc: number
  accounting: number
  management: number
  officeWork: number
  others: number
  memo: string
}

export interface IHolidaysManHours {
  paidHoliday: number
  holiday: number
  private: number
  others: number
  departmentOthers: number
  memo: string
}

export interface ISummarySupportManHours {
  direct: number
  pre: number
  sales: number
  ssap: number
  others: number
  total: number
}

export interface IGetManHourBySupporterUserIdRequest {
  year: number
  month: number
}

export interface IUpdateManHourRequestParams {
  id: string
  version: number
  year: number
  month: number
}
export interface IGetManHourBySupporterUserIdResponse {
  yearMonth: string
  supporterUserId: string
  supporterUserName: string
  supporterOrganizationId: string
  supporterOrganizationName: string
  directSupportManHours: IDirectSupportManHours[]
  preSupportManHours: IPreSupportManHours[]
  salesSupportManHours: ISalesSupportManHours[]
  ssapManHours: ISsapManHours
  holidaysManHours: IHolidaysManHours
  summarySupportManHours: ISummarySupportManHours
  isConfirm: boolean
  createId: string
  createUserName: string
  createAt: string
  updateId: string
  updateUserName: string
  updateAt: string
  version: number
}

export interface IUpdateManHourRequest {
  supporterUserName: string
  supporterOrganizationId: string
  supporterOrganizationName: string
  directSupportManHours: IDirectSupportManHours[]
  preSupportManHours: IPreSupportManHours[]
  salesSupportManHours: ISalesSupportManHours[]
  ssapManHours: ISsapManHours
  holidaysManHours: IHolidaysManHours
  summarySupportManHours: ISummarySupportManHours
  isConfirm: boolean
}

export interface IUpdateManHourResponse {
  yearMonth: string
  supporterUserId: string
  supporterUserName: string
  supporterOrganizationId: string
  supporterOrganizationName: string
  directSupportManHours: IDirectSupportManHours[]
  preSupportManHours: IPreSupportManHours[]
  salesSupportManHours: ISalesSupportManHours[]
  ssapManHours: ISsapManHours
  holidaysManHours: IHolidaysManHours
  summarySupportManHours: ISummarySupportManHours
  isConfirm: boolean
  createId: string
  createUserName: string
  createAt: string
  updateId: string
  updateUserName: string
  updateAt: string
  version: number
}

export interface IUpdateManHourBySupporterUserIdRequest {
  supporterUserName: string
  supporterOrganizationId: string
  supporterOrganizationName: string
  directSupportManHours: IDirectSupportManHours[]
  preSupportManHours: IPreSupportManHours[]
  salesSupportManHours: ISalesSupportManHours[]
  ssapManHours: ISsapManHours
  holidaysManHours: IHolidaysManHours
  isConfirm: boolean
}

export interface IFormattedMonthlyProjectOrganizationMember {
  isConfirm: true
  role: string
  supporterName: string
}

export interface IFormattedMonthlyProjectOrganization {
  supporterOrganizationId: string
  supporterOrganizationName: string
  members: IFormattedMonthlyProjectOrganizationMember[]
}

export interface IFormattedMonthlyProject {
  /**
   * supporterOrganizationId+'Total':number (支援者組織の合計人数)
   * supporterOrganizationId+'Producer':string (支援者組織のプロデューサ名),
   * supporterOrganizationId+'Accelerator':string (支援者組織のアクセラレータ名)
   * となるためやむなく [key: string]: unknown
   */
  [key: string]: unknown
  serviceTypeName: string
  name: string
  contractType: string
  mainSalesUserName: string
  organizations: IFormattedMonthlyProjectOrganization[]
  supportDateFrom: string
  supportDateTo: string
  totalContractTime: number
  thisMonthContractTime: string
  totalProfit: number
  thisMonthProfit: string
}

export const ENUM_FORMATTED_PROJECT = {
  INIT: 0,
  EXPECTATION: 1,
  EXCESS: 2,
} as const
