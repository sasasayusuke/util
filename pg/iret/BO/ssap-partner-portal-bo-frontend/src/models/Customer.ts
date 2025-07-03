import { Api } from '~/plugins/api'
import {
  ICustomerListItem,
  IGetCustomersRequest,
  IGetCustomersResponse,
  ENUM_GET_CUSTOMERS_REQUEST_SORT,
  ISuggestCustomersRequest,
  ENUM_SUGGEST_CUSTOMERS_REQUEST_SORT,
  ISuggestCustomer,
  ICreateCustomerRequest,
  ICreateCustomerResponse,
  IGetCustomerByIdResponse,
  IUpdateCustomerByIdRequest,
  IUpdateCustomerByIdResponse,
  IDeleteCustomerByIdResponse,
  IImportCustomersRequest,
  IImportCustomersResponse,
  ImportedCustomer,
  importCustomerMode,
  importCustomerResult,
} from '@/types/Customer'
export {
  ENUM_IMPORT_CUSTOMER_MODE,
  ENUM_IMPORT_CUSTOMER_RESULT,
} from '@/types/Customer'

const $api = new Api()

export class CustomerListItem implements ICustomerListItem {
  id = ''
  name = ''
  category = ''
  salesforceUpdateAt = ''
  updateAt = ''
}

export class GetCustomersRequest implements IGetCustomersRequest {
  name = ''
  sort = ENUM_GET_CUSTOMERS_REQUEST_SORT.NAME_ASC
  limit = 20
  offsetPage = 1
}

export class GetCustomersResponse implements IGetCustomersResponse {
  offsetPage = 0
  total = 0
  customers: CustomerListItem[] = []
}

export async function GetCustomers(request: GetCustomersRequest) {
  return await $api.get<GetCustomersResponse>(`/customers`, request)
}

export class SuggestCustomersRequest implements ISuggestCustomersRequest {
  sort = ENUM_SUGGEST_CUSTOMERS_REQUEST_SORT.NAME_ASC
}

export class SuggestCustomer implements ISuggestCustomer {
  id = ''
  name = ''
}

export class SuggestCustomersResponse extends Array<SuggestCustomer> {}

export async function SuggestCustomers(
  request: SuggestCustomersRequest = new SuggestCustomersRequest()
) {
  return await $api.get<SuggestCustomersResponse>('/customers/suggest', request)
}

export class CreateCustomerRequest implements ICreateCustomerRequest {
  name = ''
  category = ''
}

export class CreateCustomerResponse implements ICreateCustomerResponse {
  id = ''
  name = ''
  category = ''
  salesforceCustomerId = ''
  salesforceUpdateAt = ''
  createId = ''
  createUserName = ''
  createAt = ''
  updateId = ''
  updateUserName = ''
  updateAt = ''
  version = 0
}

export async function CreateCustomer(request: CreateCustomerRequest) {
  return await $api.post<CreateCustomerResponse>(`/customers`, request)
}

export class GetCustomerByIdResponse implements IGetCustomerByIdResponse {
  id = ''
  name = ''
  category = ''
  salesforceCustomerId = ''
  salesforceUpdateAt = ''
  createId = ''
  createUserName = ''
  createAt = ''
  updateId = ''
  updateUserName = ''
  updateAt = ''
  version = 0
}

export async function GetCustomerById(id: string) {
  return await $api.get<GetCustomerByIdResponse>(`/customers/${id}`)
}

export class UpdateCustomerByIdRequest implements IUpdateCustomerByIdRequest {
  name = ''
  category = ''
}

export class UpdateCustomerByIdResponse implements IUpdateCustomerByIdResponse {
  id = ''
  name = ''
  category = ''
  salesforceCustomerId = ''
  salesforceUpdateAt = ''
  createId = ''
  createUserName = ''
  createAt = ''
  updateId = ''
  updateUserName = ''
  updateAt = ''
  version = 0
}

export async function UpdateCustomerById(
  id: string,
  version: number,
  request: UpdateCustomerByIdRequest
) {
  return await $api.put<UpdateCustomerByIdResponse>(
    `/customers/${id}?version=${version}`,
    request
  )
}

export class DeleteCustomerByIdResponse implements IDeleteCustomerByIdResponse {
  id = ''
  name = ''
  category = ''
  salesforceCustomerId = ''
  salesforceUpdateAt = ''
  createId = ''
  createUserName = ''
  createAt = ''
  updateId = ''
  updateUserName = ''
  updateAt = ''
  version = 0
}

export async function DeleteCustomerById(id: string, version: number) {
  return await $api.delete<DeleteCustomerByIdResponse>(
    `/customers/${id}?version=${version}`
  )
}

export class ImportCustomersRequest implements IImportCustomersRequest {
  file = 'https://'
  mode = '' as importCustomerMode
}

export class ImportCustomerResponse implements IImportCustomersResponse {
  mode = '' as importCustomerMode
  result = '' as importCustomerResult
  customers = [] as ImportedCustomer[]
}

export function ImportCustomers(
  request: ImportCustomersRequest = new ImportCustomersRequest()
) {
  return $api.post<ImportCustomerResponse>('/customers/import', request)
}
