export interface ICustomerListItem {
  id: string
  name: string
  category: string
  salesforceUpdateAt: string
  updateAt: string
}

export const ENUM_GET_CUSTOMERS_REQUEST_SORT = {
  NAME_ASC: 'name:asc',
  NAME_DESC: 'name:desc',
  SALESFORCE_UPDATE_AT_ASC: 'salesforceUpdateAt:asc',
  SALESFORCE_UPDATE_AT_DESC: 'salesforceUpdateAt:desc',
  UPDATE_AT_ASC: 'updateAt:asc',
  UPDATE_AT_DESC: 'updateAt:desc',
} as const

export type GetCustomersRequestSort =
  typeof ENUM_GET_CUSTOMERS_REQUEST_SORT[keyof typeof ENUM_GET_CUSTOMERS_REQUEST_SORT]

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

export const ENUM_SUGGEST_CUSTOMERS_REQUEST_SORT = {
  NAME_ASC: 'name:asc',
} as const

export type SuggestCustomersRequestSort =
  typeof ENUM_SUGGEST_CUSTOMERS_REQUEST_SORT[keyof typeof ENUM_SUGGEST_CUSTOMERS_REQUEST_SORT]

export interface ISuggestCustomersRequest {
  sort: SuggestCustomersRequestSort
}

export interface ISuggestCustomer {
  id: string
  name: string
}

interface ICustomerDetail {
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

export interface ICreateCustomerRequest {
  name: string
  category: string
}

export interface ICreateCustomerResponse extends ICustomerDetail {}

export interface IGetCustomerByIdResponse extends ICustomerDetail {}

export interface IUpdateCustomerByIdRequest extends ICreateCustomerRequest {}

export interface IUpdateCustomerByIdResponse extends ICustomerDetail {}

export interface IDeleteCustomerByIdResponse extends ICustomerDetail {}

export const ENUM_IMPORT_CUSTOMER_MODE = {
  CHECK: 'check',
  EXECUTE: 'execute',
} as const

export type importCustomerMode =
  typeof ENUM_IMPORT_CUSTOMER_MODE[keyof typeof ENUM_IMPORT_CUSTOMER_MODE]

export const ENUM_IMPORT_CUSTOMER_RESULT = {
  OK: 'ok',
  NG: 'ng',
  DONE: 'done',
  ERROR: 'error',
} as const

export type importCustomerResult =
  typeof ENUM_IMPORT_CUSTOMER_RESULT[keyof typeof ENUM_IMPORT_CUSTOMER_RESULT]

interface IImportedCustomer {
  name: string
  category: string
  salesforceCustomerId: string
  salesforceUpdateAt: string
  salesforceTarget: string
  salesforceCreditLimit: string
  salesforceCreditGetMonth: string
  salesforceCreditManager: string
  salesforceCreditNoRetry: string
  salesforcePawsCreditNumber: string
  salesforceCustomerOwner: string
  salesforceCustomerSegment: string
  errorMessage: string
}

export class ImportedCustomer implements IImportedCustomer {
  name = ''
  category = ''
  salesforceCustomerId = ''
  salesforceUpdateAt = ''
  salesforceTarget = ''
  salesforceCreditLimit = ''
  salesforceCreditGetMonth = ''
  salesforceCreditManager = ''
  salesforceCreditNoRetry = ''
  salesforcePawsCreditNumber = ''
  salesforceCustomerOwner = ''
  salesforceCustomerSegment = ''
  errorMessage = ''
}

export interface IImportCustomersRequest {
  file: string
  mode: importCustomerMode
}

export interface IImportCustomersResponse {
  mode: importCustomerMode
  result: importCustomerResult
  customers: ImportedCustomer[]
}
