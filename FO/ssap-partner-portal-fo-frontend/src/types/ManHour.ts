export interface IGetManHourByMineRequest {
  year: number
  month: number
}

export interface IDirectSupportManHourItem {
  projectId: string
  projectName: string
  role: string
  serviceType: string
  kartenManHour: number
  inputManHour: number
}

export interface IDirectSupportManHours {
  items: IDirectSupportManHourItem[]
  memo: string
}

export interface IPreSupportManHourItem {
  projectId: string
  projectName: string
  role: string
  serviceType: string
  inputManHour: number
}

export interface IPreSupportManHours {
  items: IPreSupportManHourItem[]
  memo: string
}

export interface ISalesSupportManHourItem {
  projectName: string
  customerId: string
  customerName: string
  type: string
  inputManHour: number
}

export interface ISalesSupportManHours {
  items: ISalesSupportManHourItem[]
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

export interface IGetManHourByMineResponse {
  yearMonth: string
  supporterUserId: string
  supporterUserName: string
  supporterOrganizationId: string
  supporterOrganizationName: string
  directSupportManHours: IDirectSupportManHours
  preSupportManHours: IPreSupportManHours
  salesSupportManHours: ISalesSupportManHours
  ssapManHours: ISsapManHours
  holidaysManHours: IHolidaysManHours
  summaryManHour: ISummarySupportManHours
  isConfirm: boolean
  createId: string
  createUserName: string
  createAt: string
  updateId: string
  updateUserName: string
  updateAt: string
  version: number
}

export interface IGetSummaryProjectSupporterManHourStatusRequest {
  supporterUserId: string
  summaryMonth?: number
}

export interface IGetSummaryProjectSupporterManHourStatusResponse {
  projectId: string
  projectName: string
  customerId: string
  customerName: string
  supporterOrganizationId: string
  serviceType: string
  supportDateFrom: string
  supportDateTo: string
  totalContractTime: number
  thisMonthContractTime: number
  summarySupporterDirectSupportManHour: number
  thisMonthSupporterDirectSupportManHour: number
  summaryDirectSupportManHour: number
  thisMonthDirectSupportManHour: number
  summaryDirectSupportManHourLimit: number
  thisMonthDirectSupportManHourLimit: number
  summarySupporterPreSupportManHour: number
  thisMonthSupporterPreSupportManHour: number
  summaryPreSupportManHour: number
  thisMonthPreSupportManHour: number
  summaryPreSupportManHourLimit: number
  thisMonthPreSupportManHourLimit: number
}

export interface IUpdateManHourByMineRequest {
  supporterUserName: string
  supporterOrganizationId: string
  supporterOrganizationName: string
  directSupportManHours: IDirectSupportManHours
  preSupportManHours: IPreSupportManHours
  salesSupportManHours: ISalesSupportManHours
  ssapManHours: ISsapManHours
  holidaysManHours: IHolidaysManHours
  isConfirm: boolean
}
