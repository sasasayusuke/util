export const ENUM_GET_SOLVER_CORPORATION_REQUEST_SORT = {
  NAME_ASC: 'name:asc',
} as const

export type GetSolverCorporationRequestSort =
  typeof ENUM_GET_SOLVER_CORPORATION_REQUEST_SORT[keyof typeof ENUM_GET_SOLVER_CORPORATION_REQUEST_SORT]

// 従業員数・資本金・売上
export interface IValueAndMemo {
  value: number
  memo: string
}

// 住所
export interface IAddress {
  postalCode: string
  state: string
  city: string
  street: string
  building: string
}

// 法人ソルバー画像
export interface ICorporatePhoto {
  fileName: string
  path: string
}

// 会社案内資料
export interface ICorporateInfoDocument {
  fileName: string
  path: string
}

// 主担当
export interface IMainCharge {
  name: string
  kana: string
  title: string
  email: string
  department: string
  phone: string
}

// 副担当
export interface IDeputyCharge {
  name: string
  kana: string
  title: string
  email: string
  department: string
  phone: string
}

// その他担当
export interface IOtherCharge {
  name: string
  title: string
  email: string
  department: string
  phone: string
}

export interface IGetSolverCorporationByIdResponse {
  id: string
  name: string
  companyAbbreviation: string
  industry: string
  established: string
  managementTeam: string
  employee?: {
    value: number
    memo?: string
  }
  capital?: {
    value: number
    memo?: string
  }
  earnings?: {
    value: number
    memo?: string
  }
  listingExchange: string
  businessContent: string
  address: {
    postalCode: string
    state: string
    city: string
    street: string
    building: string
  }
  corporatePhoto?: {
    fileName: string
    path: string
  }
  corporateInfoDocument?: {
    fileName: string
    path: string
  }[]
  issueMap50?: string[]
  vision: string
  mission: string
  mainCharge: {
    name: string
    kana: string
    title: string
    email: string
    department: string
    phone: string
  }
  deputyCharge?: {
    name: string
    kana: string
    title: string
    email: string
    department: string
    phone: string
  }
  otherCharge?: {
    name: string
    title: string
    email: string
    department: string
    phone: string
  }
  registeredSolverIds?: string[]
  notes?: string
  disabled?: boolean
  createId: string
  createUserName?: string
  createAt: string
  priceAndOperatingRateUpdateAt?: string
  priceAndOperatingRateUpdateBy?: string
  updateId?: string
  updateUserName?: string
  updateAt?: string
  version?: number
  utilizationRateVersion?: number
}

export interface IGetSolverCorporationsResponse {
  solverCorporations: {
    id: string
    name: string
    updateAt?: string
  }[]
}

export interface IUpdateSolverCorporationByIdRequest {
  name: string
  companyAbbreviation: string
  industry: string
  established: string
  managementTeam: string
  employee: IValueAndMemo
  capital: IValueAndMemo
  earnings: IValueAndMemo
  listingExchange: string
  businessContent: string
  address: IAddress
  corporatePhoto?: ICorporatePhoto
  corporateInfoDocument: ICorporateInfoDocument[]
  issueMap50?: string[]
  mission: string
  mainCharge: IMainCharge
  deputyCharge: IDeputyCharge
  otherCharge: IOtherCharge
  notes: string
}

export interface IUpdateSolverCorporationByIdResponse {
  id: string
  name: string
  companyAbbreviation: string
  industry: string
  established: string
  managementTeam: string
  employee: IValueAndMemo
  capital: IValueAndMemo
  earnings: IValueAndMemo
  listingExchange: string
  businessContent: string
  address: IAddress
  corporatePhoto: ICorporatePhoto
  corporateInfoDocument: ICorporateInfoDocument[]
  issueMap50?: string[]
  mission: string
  mainCharge: IMainCharge
  deputyCharge: IDeputyCharge
  otherCharge: IOtherCharge
  notes: string
  disabled?: boolean
  createId: string
  createUserName?: string
  createAt: string
  priceAndOperatingRateUpdateAt?: string
  priceAndOperatingRateUpdateBy?: string
  updateId?: string
  updateUserName?: string
  updateAt?: string
  version?: number
}
export interface ISolverCorporationInfo {
  name: string
  companyAbbreviation: string
  industry: string
  established: string
  managementTeam: string
  employee: IValueAndMemo
  capital: IValueAndMemo
  earnings: IValueAndMemo
  listingExchange: string
  businessContent: string
  address: IAddress
  corporatePhoto: ICorporatePhoto
  corporateInfoDocument: ICorporateInfoDocument[]
  issueMap50?: string[]
  mission: string
  mainCharge: IMainCharge
  deputyCharge: IDeputyCharge
  otherCharge: IOtherCharge
  notes: string
}
