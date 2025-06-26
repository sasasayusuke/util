export const ENUM_GET_SOLVERS_REQUEST_SORT = {
  CREATE_AT_DESC: 'create_at:desc',
  PRICE_AND_OPERATING_RATE_UPDATE_AT_DESC:
    'price_and_operating_rate_update_at:desc',
} as const

export type GetSolversRequestSort =
  typeof ENUM_GET_SOLVERS_REQUEST_SORT[keyof typeof ENUM_GET_SOLVERS_REQUEST_SORT]

export interface IDocuments {
  fileName: string
  path: string
}

// ソルバー種別
export const ENUM_SOLVER_TYPE = {
  ALL: 'all',
  SOLVER_CANDIDATE: 'solver_candidate',
  SOLVER: 'solver',
} as const

// 性別
export const ENUM_SEX_TYPE = {
  ALL: 'all',
  MAN: 'man',
  WOMAN: 'woman',
  NOT_SET: 'not_set',
} as const

// ソルバー候補の個人ソルバー申請状況
export const ENUM_CERTIFICATION_STATUS = {
  ALL: 'all',
  BEFORE: 'before',
  DURING: 'during',
  NOT_SET: 'not_set',
} as const

// 稼働状況
export const ENUM_OPERATING_STATUS = {
  ALL: 'all',
  NOT_WORKING: 'not_working',
  WORKING: 'working',
  INACTIVE: 'inactive',
} as const

// 登録状態
export const ENUM_REGISTRATION_STATUS = {
  NEW: 'new',
  TEMPORARY_SAVING: 'temporary_saving',
  SAVED: 'saved',
  CERTIFICATED: 'certificated',
} as const

export interface IFormattedSolver {
  id: string
  name: string
  sex?: string
  age?: number
  birthDay?: string
  specializedThemes?: string
  operatingStatus?: string
  providedOperatingRate?: number
  providedOperatingRateNext?: number
  operationProspectsMonthAfterNext?: string
  pricePerPersonMonth?: {
    value1: number
    value2: number
  }
  hourlyRate?: {
    value1: number
    value2: number
  }
  isCertificateApproval?: boolean
  isTemporarySaving?: boolean
  priceAndOperatingRateUpdateAt?: string
}

export interface ISolver {
  id: string
  name: string
  corporateId?: string
  sex?: string
  age?: number
  birthDay?: string
  specializedThemes?: string
  operatingStatus?: string
  providedOperatingRate?: number
  providedOperatingRateNext?: number
  operationProspectsMonthAfterNext?: string
  pricePerPersonMonth?: number
  pricePerPersonMonthLower?: number
  hourlyRate?: number
  hourlyRateLower?: number
  registrationStatus?: string
  priceAndOperatingRateUpdateAt?: string
  version?: number
  isSolver?: boolean
}

export interface IGetSolversResponse {
  offsetPage: number
  total: number
  solvers: ISolver[]
}

export interface IUpdateUtilizationRate {
  id: string
  providedOperatingRate: number
  providedOperatingRateNext: number
  pricePerPersonMonth: number
  pricePerPersonMonthLower: number
  hourlyRate: number
  hourlyRateLower: number
}
export interface IUpdateUtilizationRateRequest {
  utilizationRate: {
    id: string
    providedOperatingRate?: number
    providedOperatingRateNext?: number
    operationProspectsMonthAfterNext?: string
    pricePerPersonMonth?: number
    pricePerPersonMonthLower?: number
    hourlyRate?: number
    hourlyRateLower?: number
  }[]
}

export interface IUpdateUtilizationRateResponse {
  message: string
}

export interface IFileContent {
  fileName: string
  path: string
}

export interface ISolverApplicationContent {
  id: string
  name: string
}

export interface IGetSolverByIdResponse {
  id: string
  name: string
  nameKana?: string
  solverApplications?: ISolverApplicationContent[]
  title?: string
  email?: string
  phone?: string
  issueMap50: string[]
  corporateId?: string
  birthDay: string
  sex: string
  operatingStatus?: string
  facePhoto?: {
    fileName: string
    path: string
  }
  resume?: IFileContent[]
  academicBackground?: string
  workHistory: string
  isConsultingFirm: boolean
  specializedThemes: string
  mainAchievements: string
  providedOperatingRate?: number
  providedOperatingRateNext?: number
  operationProspectsMonthAfterNext?: string
  pricePerPersonMonth?: number
  pricePerPersonMonthLower?: number
  hourlyRate?: number
  hourlyRateLower?: number
  englishLevel?: string
  tsiAreas?: string[]
  screening1?: {
    evaluation: boolean
    evidence: string
  }
  screening2?: {
    evaluation: boolean
    evidence: string
  }
  screening3?: {
    evaluation: boolean
    evidence: string
  }
  screening4?: {
    evaluation: boolean
    evidence: string
  }
  screening5?: {
    evaluation: boolean
    evidence: string
  }
  screening6?: {
    evaluation: boolean
    evidence: string
  }
  screening7?: {
    evaluation: boolean
    evidence: string
  }
  screening8?: {
    evaluation: boolean
    evidence: string
  }
  criteria1?: string
  criteria2?: string
  criteria3?: string
  criteria4?: string
  criteria5?: string
  criteria6?: string
  criteria7?: string
  criteria8?: string
  notes?: string
  isSolver: boolean
  registrationStatus: string
  createId: string
  createUserName?: string
  createAt: string
  priceAndOperatingRateUpdateAt?: string
  priceAndOperatingRateUpdateBy?: string
  updateId?: string
  updateUserName?: string
  updateAt?: string
  version?: number
  fileKeyId?: string
}

export interface ISolverIFile {
  fileName: string
  path: string
}
export interface ISolverInfo {
  id?: string
  mode: string
  isRegisteredSolver: boolean
  name: string
  nameKana?: string
  solverApplicationId?: string
  solverApplicationName?: string
  title?: string
  email?: string
  phone?: string
  issueMap50?: string[]
  corporateId?: string
  sex: string
  birthDay: string
  operatingStatus?: string
  facePhoto?: ISolverIFile
  resume?: ISolverIFile[]
  academicBackground?: string
  workHistory: string
  isConsultingFirm: boolean
  specializedThemes: string
  mainAchievements: string
  providedOperatingRate?: number
  providedOperatingRateNext?: number
  operationProspectsMonthAfterNext?: string
  pricePerPersonMonth?: number
  pricePerPersonMonthLower?: number
  hourlyRate?: number
  hourlyRateLower?: number
  englishLevel?: string
  tsiAreas?: string[]
  screening1?: {
    evaluation: boolean
    evidence: string
  }
  screening2?: {
    evaluation: boolean
    evidence: string
  }
  screening3?: {
    evaluation: boolean
    evidence: string
  }
  screening4?: {
    evaluation: boolean
    evidence: string
  }
  screening5?: {
    evaluation: boolean
    evidence: string
  }
  screening6?: {
    evaluation: boolean
    evidence: string
  }
  screening7?: {
    evaluation: boolean
    evidence: string
  }
  screening8?: {
    evaluation: boolean
    evidence: string
  }
  criteria1?: string
  criteria2?: string
  criteria3?: string
  criteria4?: string
  criteria5?: string
  criteria6?: string
  criteria7?: string
  criteria8?: string
  notes?: string
  registrationStatus: string
  fileKeyId?: string
}

export interface ICreateSolverRequest {
  solversInfo: ISolverInfo[]
}

export interface IUpdateSolverRequest
  extends Omit<ISolverInfo, 'id' | 'isRegisteredSolver'> {
  corporateId: string
  isSolver: boolean
  deleteSolverApplicationIds?: string[]
}
