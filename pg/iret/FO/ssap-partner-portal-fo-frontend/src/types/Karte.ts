export const ENUM_MAX_FILE_SIZE = 10737418240

export interface IKarteListItem {
  karteId: string
  date: string
  startTime: string
  endTime: string
  updateUser: string
  isDraft: boolean
  lastUpdateDatetime: string
}

export interface IDocuments {
  fileName: string
  path: string
}

export interface IDeliverables {
  fileName: string
  path: string
}

interface IGetKartenRequestSortDefine {
  NAME_ASC: string
  NAME_DESC: string
  UPDATE_AT_ASC: string
  UPDATE_AT_DESC: string
  [key: string]: string
}

export const GET_KARTEN_REQUEST_SORT: IGetKartenRequestSortDefine = {
  NAME_ASC: 'name:asc',
  NAME_DESC: 'name:desc',
  UPDATE_AT_ASC: 'updateAt:asc',
  UPDATE_AT_DESC: 'updateAt:desc',
}

export interface IGetKartenRequest {
  projectId: string
}

export interface IGetKartenResponse {
  [key: number]: IKarteListItem[]
}

export interface ILocation {
  type: string
  detail: string
}

interface IKarteDetail {
  karteId: string
  projectId: string
  date: string
  startDatetime: string
  startTime: string
  endTime: string
  draftSupporterId: string
  draftSupporterName: string
  draftDatetime: string
  lastUpdateDatetime: string
  startSupportActualTime: string
  endSupportActualTime: string
  manHour: number
  customers: string
  supportTeam: string
  detail: string
  feedback: string
  homework: string
  documents: IDocuments[]
  deliverables: IDeliverables[]
  memo: string
  task: string
  humanResourceNeededForCustomer: string
  companyAndIndustryRecommendedToCustomer: string
  humanResourceNeededForSupport: string
  location: ILocation
  isDraft: boolean
  createId: string
  createUserName: string
  createAt: string
  updateId: string
  updateUserName: string
  updateAt: string
  version: number
}

export interface IGetKarteByIdResponse extends IKarteDetail {}

export interface IUpdateKarteByIdRequest {
  startSupportActualTime: string
  endSupportActualTime: string
  manHour: number
  customers: string
  supportTeam: string
  detail: string
  feedback: string
  homework: string
  documents: IDocuments[]
  deliverables: IDeliverables[]
  memo: string
  task: string
  humanResourceNeededForCustomer: string
  companyAndIndustryRecommendedToCustomer: string
  humanResourceNeededForSupport: string
  isDraft: boolean
  isNotifyUpdateKarte: boolean
  location: ILocation
}

export interface IKarteLatestListItem {
  karteId: string
  projectId: string
  projectName: string
  customerName: string
  date: string
  startTime: string
  endTime: string
  day: string
  lastUpdateDatetime: string | null
  draftSupporterName: string
}
