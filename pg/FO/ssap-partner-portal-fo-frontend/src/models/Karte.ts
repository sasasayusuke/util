import {
  IDocuments,
  IDeliverables,
  IKarteListItem,
  IGetKartenRequest,
  IGetKarteByIdResponse,
  IUpdateKarteByIdRequest,
  IKarteLatestListItem,
  ILocation,
} from '@/types/Karte'

import { Api } from '~/plugins/api'

const $api = new Api()

export class Documents implements IDocuments {
  fileName = ''
  path = ''
}

export class Deliverables implements IDeliverables {
  fileName = ''
  path = ''
}

export class KarteListItem implements IKarteListItem {
  karteId = ''
  date = ''
  startTime = ''
  endTime = ''
  updateUser = ''
  isDraft = true
  lastUpdateDatetime = ''
}

export class GetKartenRequest implements IGetKartenRequest {
  projectId = ''
}

export class GetKartenResponse extends Array<KarteListItem> {}

export async function GetKarten(request: GetKartenRequest) {
  return await $api.get<GetKartenResponse>(`/karten`, request)
}

export class Location implements ILocation {
  type = ''
  detail = ''
}

export class GetKarteByIdResponse implements IGetKarteByIdResponse {
  karteId = ''
  projectId = ''
  date = ''
  startDatetime = ''
  startTime = ''
  endTime = ''
  draftSupporterId = ''
  draftSupporterName = ''
  draftDatetime = ''
  lastUpdateDatetime = ''
  startSupportActualTime = ''
  endSupportActualTime = ''
  manHour = 0
  customers = ''
  supportTeam = ''
  detail = ''
  feedback = ''
  homework = ''
  documents: Documents[] = []
  deliverables: Deliverables[] = []
  memo = ''
  task = ''
  humanResourceNeededForCustomer = ''
  companyAndIndustryRecommendedToCustomer = ''
  humanResourceNeededForSupport = ''
  location = new Location()
  isDraft = true
  createId = ''
  createUserName = ''
  createAt = ''
  updateId = ''
  updateUserName = ''
  updateAt = ''
  version = 0
}

export async function GetKarteById(id: string) {
  return await $api.get<GetKarteByIdResponse>(`/karten/${id}`)
}

export class UpdateKarteByIdRequest implements IUpdateKarteByIdRequest {
  karteId = ''
  startSupportActualTime = ''
  endSupportActualTime = ''
  manHour = 0
  customers = ''
  supportTeam = ''
  detail = ''
  feedback = ''
  homework = ''
  documents: Documents[] = []
  deliverables: Deliverables[] = []
  memo = ''
  task = ''
  humanResourceNeededForCustomer = ''
  companyAndIndustryRecommendedToCustomer = ''
  humanResourceNeededForSupport = ''
  isDraft = true
  isNotifyUpdateKarte = false
  location = new Location()
}

export async function UpdateKarteById(
  id: string,
  version: number,
  request: UpdateKarteByIdRequest
) {
  return await $api.put(`/karten/${id}?version=${version}`, request)
}

export class KartenLatestListItem implements IKarteLatestListItem {
  karteId = ''
  projectId = ''
  projectName = ''
  customerName = ''
  date = ''
  startTime = ''
  endTime = ''
  day = ''
  lastUpdateDatetime: string | null = null
  draftSupporterName = ''
}

export async function GetKartenLatest() {
  return await $api.get<KartenLatestListItem[]>(`/karten/latest`)
}
