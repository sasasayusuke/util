import { Api } from '~/plugins/api'
import {
  ENUM_GET_SOLVER_CORPORATION_REQUEST_SORT,
  IValueAndMemo,
  IAddress,
  ICorporatePhoto,
  ICorporateInfoDocument,
  IMainCharge,
  IDeputyCharge,
  IOtherCharge,
  GetSolverCorporationRequestSort,
  IGetSolverCorporationByIdResponse,
  IGetSolverCorporationsResponse,
  IUpdateSolverCorporationByIdRequest,
  IUpdateSolverCorporationByIdResponse,
  ISolverCorporationInfo,
} from '~/types/SolverCorporation'

const $api = new Api()

// 従業員数・資本金・売上
export class ValueAndMemo implements IValueAndMemo {
  public value = 0
  public memo = ''
}

// 住所
export class Address implements IAddress {
  public postalCode = ''
  public state = ''
  public city = ''
  public street = ''
  public building = ''
}

// 法人ソルバー画像
export class CorporatePhoto implements ICorporatePhoto {
  public fileName = ''
  public path = ''
}

// 会社案内資料
export class CorporateInfoDocument implements ICorporateInfoDocument {
  public fileName = ''
  public path = ''
}

// 主担当
export class MainCharge implements IMainCharge {
  public name = ''
  public kana = ''
  public title = ''
  public email = ''
  public department = ''
  public phone = ''
}

// 副担当
export class DeputyCharge implements IDeputyCharge {
  public name = ''
  public kana = ''
  public title = ''
  public email = ''
  public department = ''
  public phone = ''
}

// その他担当
export class OtherCharge implements IOtherCharge {
  public name = ''
  public title = ''
  public email = ''
  public department = ''
  public phone = ''
}

export class GetSolverCorporationByIdResponse
  implements IGetSolverCorporationByIdResponse
{
  public id = ''
  public name = ''
  public companyAbbreviation = ''
  public industry = ''
  public established = ''
  public managementTeam = ''
  public employee = {
    value: 0,
    memo: '',
  }

  public capital = {
    value: 0,
    memo: '',
  }

  public earnings = {
    value: 0,
    memo: '',
  }

  public listingExchange = ''
  public businessContent = ''
  public address = {
    postalCode: '',
    state: '',
    city: '',
    street: '',
    building: '',
  }

  public corporatePhoto = {
    fileName: '',
    path: '',
  }

  public corporateInfoDocument = []

  public issueMap50 = []

  public vision = ''
  public mission = ''
  public mainCharge = {
    name: '',
    kana: '',
    title: '',
    email: '',
    department: '',
    phone: '',
  }

  public deputyCharge = {
    name: '',
    kana: '',
    title: '',
    email: '',
    department: '',
    phone: '',
  }

  public otherCharge = {
    name: '',
    title: '',
    email: '',
    department: '',
    phone: '',
  }

  public registeredSolverIds = ['']
  public notes = ''
  public disabled = false
  public createId = ''
  public createUserName = ''
  public createAt = ''
  public priceAndOperatingRateUpdateAt = ''
  public priceAndOperatingRateUpdateBy = ''
  public updateId = ''
  public updateUserName = ''
  public updateAt = ''
  public version = 0
  public utilizationRateVersion = 0
}

export async function GetSolverCorporationById(id: string) {
  return await $api.get<GetSolverCorporationByIdResponse>(
    `/solver-corporations/${id}`
  )
}

export class GetSolverCorporationsRequest {
  disabled?: boolean
  sort?: GetSolverCorporationRequestSort =
    ENUM_GET_SOLVER_CORPORATION_REQUEST_SORT.NAME_ASC
}

export class GetSolverCorporationsResponse
  implements IGetSolverCorporationsResponse
{
  public solverCorporations = [
    {
      id: '',
      name: '',
      updateAt: '',
    },
  ]
}

export async function GetSolverCorporations(
  request: GetSolverCorporationsRequest
) {
  return await $api.get<GetSolverCorporationsResponse>(
    '/solver-corporations',
    request
  )
}

export class UpdateSolverCorporationByIdRequest
  implements IUpdateSolverCorporationByIdRequest
{
  public name = ''
  public companyAbbreviation = ''
  public industry = ''
  public established = ''
  public managementTeam = ''
  public employee = new ValueAndMemo()
  public capital = new ValueAndMemo()
  public earnings = new ValueAndMemo()
  public listingExchange = ''
  public businessContent = ''
  public address = new Address()
  public corporatePhoto? = new CorporatePhoto()
  public corporateInfoDocument: CorporateInfoDocument[] = []
  public issueMap50 = []
  public mission = ''
  public mainCharge = new MainCharge()
  public deputyCharge = new DeputyCharge()
  public otherCharge = new OtherCharge()
  public notes = ''
}

export class UpdateSolverCorporationByIdResponse
  implements IUpdateSolverCorporationByIdResponse
{
  public id = ''
  public name = ''
  public companyAbbreviation = ''
  public industry = ''
  public established = ''
  public managementTeam = ''
  public employee = new ValueAndMemo()
  public capital = new ValueAndMemo()
  public earnings = new ValueAndMemo()
  public listingExchange = ''
  public businessContent = ''
  public address = new Address()
  public corporatePhoto = new CorporatePhoto()
  public corporateInfoDocument: CorporateInfoDocument[] = []
  public issueMap50 = []
  public mission = ''
  public mainCharge = new MainCharge()
  public deputyCharge = new DeputyCharge()
  public otherCharge = new OtherCharge()
  public notes = ''
  public disabled = false
  public createId = ''
  public createUserName = ''
  public createAt = ''
  public priceAndOperatingRateUpdateAt = ''
  public priceAndOperatingRateUpdateBy = ''
  public updateId = ''
  public updateUserName = ''
  public updateAt = ''
  public version = 0
}

export class SolverCorporationInfo implements ISolverCorporationInfo {
  public name = ''
  public companyAbbreviation = ''
  public industry = ''
  public established = ''
  public managementTeam = ''
  public employee = new ValueAndMemo()
  public capital = new ValueAndMemo()
  public earnings = new ValueAndMemo()
  public listingExchange = ''
  public businessContent = ''
  public address = new Address()
  public corporatePhoto = new CorporatePhoto()
  public corporateInfoDocument: CorporateInfoDocument[] = []
  public issueMap50 = []
  public mission = ''
  public mainCharge = new MainCharge()
  public deputyCharge = new DeputyCharge()
  public otherCharge = new OtherCharge()
  public notes = ''
}

export async function UpdateSolverCorporationById(
  id: string,
  version: number,
  request: UpdateSolverCorporationByIdRequest
) {
  return await $api.put<UpdateSolverCorporationByIdResponse>(
    `/solver-corporations/${id}?version=${version}`,
    request
  )
}

export async function DeleteSolverCorporationById(id: string, version: number) {
  return await $api.delete(`/solver-corporations/${id}?version=${version}`)
}
