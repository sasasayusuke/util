import {
  ICustomerListItem,
  IGetCustomersRequest,
  IGetCustomersResponse,
  GET_CUSTOMERS_REQUEST_SORT,
  ISuggestCustomersRequest,
  SUGGEST_CUSTOMERS_REQUEST_SORT,
  ISuggestCustomer,
  ICustomerDetail,
  IGetCustomerByIdResponse,
} from '@/types/Customer'

import { Api } from '~/plugins/api'

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
  sort = GET_CUSTOMERS_REQUEST_SORT.NAME_ASC
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
  sort = SUGGEST_CUSTOMERS_REQUEST_SORT.NAME_ASC
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

export class CustomerDetail implements ICustomerDetail {
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
