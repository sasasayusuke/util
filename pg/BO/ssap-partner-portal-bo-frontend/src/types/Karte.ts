export interface IKarteListItem {
  karteId: string
  date: string
  startTime: string
  endTime: string
  updateUser: string
}

export interface ISupporterIds {
  [key: number]: string
}

export interface ICustomerUserIds {
  [key: number]: string
}

export interface IDocuments {
  fileName: string
  path: string
}

export interface IDeliverables {
  fileName: string
  path: string
}

export interface IGetKartenRequest {
  projectId: string
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

export interface ILocation {
  type: string
  detail: string
}
