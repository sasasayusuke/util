export interface ICustomerListItem {
  id: string
  name: string
  category: string
  salesforceUpdateAt: string
  updateAt: string
}

interface IGetCustomersRequestSortDefine {
  NAME_ASC: string
  NAME_DESC: string
  SALESFORCE_UPDATE_AT_ASC: string
  SALESFORCE_UPDATE_AT_DESC: string
  UPDATE_AT_ASC: string
  UPDATE_AT_DESC: string
  [key: string]: string
}

export const GET_CUSTOMERS_REQUEST_SORT: IGetCustomersRequestSortDefine = {
  NAME_ASC: 'name:asc',
  NAME_DESC: 'name:desc',
  SALESFORCE_UPDATE_AT_ASC: 'salesforceUpdateAt:asc',
  SALESFORCE_UPDATE_AT_DESC: 'salesforceUpdateAt:desc',
  UPDATE_AT_ASC: 'updateAt:asc',
  UPDATE_AT_DESC: 'updateAt:desc',
}

export interface IGetCustomersRequest {
  name: string
  sort: string
  limit: number
  offsetPage: number
}

export interface IGetCustomersResponse {
  offsetPage: number
  total: number
  customers: ICustomerListItem[]
}

interface ISuggestCustomersSortDefine {
  NAME_ASC: string
  [key: string]: string
}

export const SUGGEST_CUSTOMERS_REQUEST_SORT: ISuggestCustomersSortDefine = {
  NAME_ASC: 'name:asc',
}

export interface ISuggestCustomersRequest {
  sort: string
}

export interface ISuggestCustomer {
  id: string
  name: string
}

export interface ICustomerDetail {
  id: string
  name: string
  category: string
  salesforceCustomerId: string
  salesforceUpdateAt: string
  createId: string
  createUserName: string
  createAt: string
  updateId: string
  updateUserName: string
  updateAt: string
  version: number
}

export interface IGetCustomerByIdResponse extends ICustomerDetail {}
