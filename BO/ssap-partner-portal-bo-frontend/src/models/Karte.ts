import {
  IKarteListItem,
  IGetKartenRequest,
  IDocuments,
  IDeliverables,
  IGetKarteByIdResponse,
  ILocation,
} from '@/types/Karte'
import { Api } from '~/plugins/api'

const $api = new Api()

export class KarteListItem implements IKarteListItem {
  karteId = ''
  date = ''
  startTime = ''
  endTime = ''
  updateUser = ''
}

export class GetKartenRequest implements IGetKartenRequest {
  projectId = ''
}

export class Documents implements IDocuments {
  fileName = ''
  path = ''
}

export class Deliverables implements IDeliverables {
  fileName = ''
  path = ''
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
  detail = ''
  customers = ''
  supportTeam = ''
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
