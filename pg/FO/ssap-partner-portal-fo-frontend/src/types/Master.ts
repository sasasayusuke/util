export interface IMasterListItem {
  id: string
  dataType: string
  name: string
  value: string
  use: boolean
  createAt: string
  updateAt: string
}

export interface IGetMastersRequest {
  dataType: string
  limit: number
  offsetPage: number
}
export interface IGetMastersResponse {
  offsetPage: number
  total: number
  masters: IMasterListItem[]
}
export interface IGetMasterByIdRequest {
  id: string
}

export interface IGetMasterByIdResponse {
  id: string
  dataType: string
  name: string
  value: string
  attributes: object
  use: boolean
  createAt: string
  updateAt: string
  version: number
}

export interface ICreateMasterRequest {
  dataType: string
  name: string
  value: string
  attributes: object
  use: boolean
}

export interface ICreateMasterResponse {
  id: string
  dataType: string
  name: string
  value: string
  attributes: object
  use: boolean
}

export interface IUpdateMasterRequestParams {
  id: string
  version: number
}

export interface IUpdateMasterRequest {
  name: string
  value: string
  attributes: Object
  order: number
  use: boolean
  maintenance: boolean
}

export interface IUpdateMasterResponse {
  name: string
  value: string
  attributes: object
  use: boolean
}

export interface IServiceTypeItems {
  id: string
  name: string
}

export interface IGetServiceTypesResponse {
  serviceTypes: IServiceTypeItems[]
}

export interface ISupporterOrganizationItems {
  id: string
  name: string
  shortName: string
}

export interface IGetSupporterOrganizationsResponse {
  supporterOrganizations: ISupporterOrganizationItems[]
}

export interface IGetBatchControlByIdResponse {
  batchEndAt: string
}

export interface ISurveyMasterItem {
  id: string
  revision: number
  name: string
  type: string
  timing: string
  initSendDaySetting: number
  initAnswerLimitDaySetting: number
  isDisclosure: boolean
  isLatest: boolean
}

export interface IGetSurveyMastersResponse {
  total: number
  masters: ISurveyMasterItem[]
}

export interface IGetSelectItemsRequest {
  dataType: string
}

export interface IGetSelectItemsResponse {
  masters: {
    id: string
    category: string
    name: string
  }[]
}
