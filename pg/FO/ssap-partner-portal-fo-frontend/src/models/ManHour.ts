import {
  IGetManHourByMineRequest,
  IDirectSupportManHourItem,
  IDirectSupportManHours,
  IPreSupportManHourItem,
  IPreSupportManHours,
  ISalesSupportManHourItem,
  ISalesSupportManHours,
  ISsapManHours,
  IHolidaysManHours,
  ISummarySupportManHours,
  IGetManHourByMineResponse,
  IGetSummaryProjectSupporterManHourStatusRequest,
  IGetSummaryProjectSupporterManHourStatusResponse,
  IUpdateManHourByMineRequest,
} from '@/types/ManHour'

import { Api } from '~/plugins/api'

const $api = new Api()

export class GetManHourByMineRequest implements IGetManHourByMineRequest {
  public year = 2000
  public month = 1
}

export class DirectSupportManHourItem implements IDirectSupportManHourItem {
  public projectId = ''
  public projectName = ''
  public role = ''
  public serviceType = ''
  public kartenManHour = 0
  public inputManHour = 0
}

export class DirectSupportManHours implements IDirectSupportManHours {
  public items: DirectSupportManHourItem[] = []
  public memo = ''
}

export class PreSupportManHourItem implements IPreSupportManHourItem {
  public projectId = ''
  public projectName = ''
  public role = ''
  public serviceType = ''
  public inputManHour = 0
}

export class PreSupportManHours implements IPreSupportManHours {
  public items: PreSupportManHourItem[] = []
  public memo = ''
}

export class SalesSupportManHourItem implements ISalesSupportManHourItem {
  public projectName = ''
  public customerId = ''
  public customerName = ''
  public type = ''
  public inputManHour = 0
}

export class SalesSupportManHours implements ISalesSupportManHours {
  public items: SalesSupportManHourItem[] = []
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

export class GetManHourByMineResponse implements IGetManHourByMineResponse {
  public yearMonth = ''
  public supporterUserId = ''
  public supporterUserName = ''
  public supporterOrganizationId = ''
  public supporterOrganizationName = ''
  public directSupportManHours = new DirectSupportManHours()
  public preSupportManHours = new PreSupportManHours()
  public salesSupportManHours = new SalesSupportManHours()
  public ssapManHours = new SsapManHours()
  public holidaysManHours = new HolidaysManHours()
  public summaryManHour = new SummarySupportManHours()
  public isConfirm = false
  public createId = ''
  public createUserName = ''
  public createAt = ''
  public updateId = ''
  public updateUserName = ''
  public updateAt = ''
  public version = 1
}

export async function GetManHourByMine(request: GetManHourByMineRequest) {
  return await $api.get<GetManHourByMineResponse>(
    `/man-hours/me?year=${request.year}&month=${request.month}`
  )
}

export class GetSummaryProjectSupporterManHourStatusRequest
  implements IGetSummaryProjectSupporterManHourStatusRequest
{
  supporterUserId = ''
}

export class GetSummaryProjectSupporterManHourStatusResponse
  implements IGetSummaryProjectSupporterManHourStatusResponse
{
  projectId = ''
  projectName = ''
  customerId = ''
  customerName = ''
  supporterOrganizationId = ''
  serviceType = ''
  supportDateFrom = ''
  supportDateTo = ''
  totalContractTime = 0
  thisMonthContractTime = 0
  summarySupporterDirectSupportManHour = 0
  thisMonthSupporterDirectSupportManHour = 0
  summaryDirectSupportManHour = 0
  thisMonthDirectSupportManHour = 0
  summaryDirectSupportManHourLimit = 0
  thisMonthDirectSupportManHourLimit = 0
  summarySupporterPreSupportManHour = 0
  thisMonthSupporterPreSupportManHour = 0
  summaryPreSupportManHour = 0
  thisMonthPreSupportManHour = 0
  summaryPreSupportManHourLimit = 0
  thisMonthPreSupportManHourLimit = 0
}

export async function GetSummaryProjectSupporterManHourStatus(
  id: string,
  request: GetSummaryProjectSupporterManHourStatusRequest
) {
  return await $api.get<GetSummaryProjectSupporterManHourStatusResponse>(
    `/man-hours/summary/project/${id}/supporter`,
    request
  )
}

export async function UpdateManHourByMine(
  year: string,
  month: string,
  version: number,
  request: UpdateManHourByMineRequest
) {
  return await $api.put(
    `/man-hours/me?year=${year}&month=${month}&version=${version}`,
    request
  )
}

export class UpdateManHourByMineRequest implements IUpdateManHourByMineRequest {
  public supporterUserName = ''
  public supporterOrganizationId = ''
  public supporterOrganizationName = ''
  public directSupportManHours: IDirectSupportManHours =
    new DirectSupportManHours()

  public preSupportManHours: IPreSupportManHours = new PreSupportManHours()

  public salesSupportManHours: ISalesSupportManHours =
    new SalesSupportManHours()

  public ssapManHours: ISsapManHours = new SsapManHours()

  public holidaysManHours: IHolidaysManHours = new HolidaysManHours()

  public isConfirm = true
}
