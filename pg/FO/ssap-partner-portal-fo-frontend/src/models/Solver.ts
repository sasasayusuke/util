import { Api } from '~/plugins/api'
import {
  ENUM_GET_SOLVERS_REQUEST_SORT,
  GetSolversRequestSort,
  ICreateSolverRequest,
  IDocuments,
  IFileContent,
  IGetSolverByIdResponse,
  IGetSolversResponse,
  ISolver,
  ISolverApplicationContent,
  ISolverInfo,
  IUpdateSolverRequest,
  IUpdateUtilizationRateRequest,
} from '~/types/Solver'

const $api = new Api()

export class Documents implements IDocuments {
  fileName = ''
  path = ''
}

export class GetSolversRequest {
  id?: string
  solverType?: string
  name?: string
  sex?: string
  certificationStatus?: boolean
  operatingStatus?: string
  sort?: GetSolversRequestSort =
    ENUM_GET_SOLVERS_REQUEST_SORT.PRICE_AND_OPERATING_RATE_UPDATE_AT_DESC

  offsetPage?: number = 1
  limit?: number = 10
}

export class SolverListItem {
  public id = ''
  public name = ''
  public corporateId = ''
  public sex = ''
  public age: number | string = 0
  public birthDay = ''
  public specializedThemes = ''
  public operatingStatus = ''
  public providedOperatingRate: any = 0
  public providedOperatingRateNext: any = 0
  public pricePerPersonMonth: any = 0
  public pricePerPersonMonthLower: any = 0
  public hourlyRate: any = 0
  public hourlyRateLower: any = 0
  public registrationStatus = ''
  public priceAndOperatingRateUpdateAt = ''
  // ソルバー候補一覧・個人ソルバー一覧のボタン用
  public buttonInfo: { label: string; to: string } | null = null
}

export class Solver implements ISolver {
  public id = ''
  public name = ''
  public corporateId = ''
  public sex = ''
  public age = 0
  public birthDay = ''
  public specializedThemes = ''
  public operatingStatus = ''
  public providedOperatingRate = 0
  public providedOperatingRateNext = 0
  public operationProspectsMonthAfterNext = ''
  public pricePerPersonMonth = 0
  public pricePerPersonMonthLower = 0
  public hourlyRate = 0
  public hourlyRateLower = 0
  public registrationStatus = ''
  public priceAndOperatingRateUpdateAt = ''
  public version = 0
}

export class GetSolversResponse implements IGetSolversResponse {
  public offsetPage = 0
  public total = 0
  public solvers = []
}

export async function GetSolvers(request: GetSolversRequest) {
  return await $api.get<GetSolversResponse>(`/solvers`, request)
}

export class UpdateUtilizationRateRequest
  implements IUpdateUtilizationRateRequest
{
  public utilizationRate = [
    {
      id: '',
      name: '',
      providedOperatingRate: 0,
      providedOperatingRateNext: 0,
      operationProspectsMonthAfterNext: '',
      pricePerPersonMonth: 0,
      pricePerPersonMonthLower: 0,
      hourlyRate: 0,
      hourlyRateLower: 0,
    },
  ]
}

export class UpdateUtilizationRateResponse {
  public deleted: string[] = []
}

export async function UpdateUtilizationRate(
  solverCorporationId: string,
  version: number,
  request: UpdateUtilizationRateRequest
) {
  return await $api.put<UpdateUtilizationRateResponse>(
    `/solvers/utilization-rate/${solverCorporationId}?version=${version}`,
    request
  )
}

export class FileContent implements IFileContent {
  public fileName = ''
  public path = ''
}

export class SolverApplicationContent implements ISolverApplicationContent {
  public id = ''
  public name = ''
  public projectCode = ''
}

export class GetSolverByIdResponse implements IGetSolverByIdResponse {
  public id = ''
  public name = ''
  public nameKana = ''
  public solverApplications: SolverApplicationContent[] = []

  public title = ''
  public email = ''
  public phone = ''
  public issueMap50: string[] = []

  public corporateId = ''
  public birthDay = ''
  public sex = 'not_set'
  public operatingStatus = 'not_working'
  public facePhoto = {
    fileName: '',
    path: '',
  }

  public resume: FileContent[] = []

  public academicBackground = ''
  public workHistory = ''
  public isConsultingFirm = false
  public specializedThemes = ''
  public mainAchievements = ''
  public providedOperatingRate = 0
  public providedOperatingRateNext = 0
  public operationProspectsMonthAfterNext = ''
  public pricePerPersonMonth: any = 0
  public pricePerPersonMonthLower: any = 0
  public hourlyRate: any = 0
  public hourlyRateLower: any = 0
  public englishLevel = 'reading_and_writing'
  public tsiAreas: string[] = []
  public screening1 = {
    evaluation: false,
    evidence: '',
  }

  public screening2 = {
    evaluation: false,
    evidence: '',
  }

  public screening3 = {
    evaluation: false,
    evidence: '',
  }

  public screening4 = {
    evaluation: false,
    evidence: '',
  }

  public screening5 = {
    evaluation: false,
    evidence: '',
  }

  public screening6 = {
    evaluation: false,
    evidence: '',
  }

  public screening7 = {
    evaluation: false,
    evidence: '',
  }

  public screening8 = {
    evaluation: false,
    evidence: '',
  }

  public criteria1 = ''
  public criteria2 = ''
  public criteria3 = ''
  public criteria4 = ''
  public criteria5 = ''
  public criteria6 = ''
  public criteria7 = ''
  public criteria8 = ''
  public notes = ''
  public isSolver = false
  public registrationStatus = ''
  public createId = ''
  public createUserName = ''
  public createAt = ''
  public priceAndOperatingRateUpdateAt = ''
  public priceAndOperatingRateUpdateBy = ''
  public updateId = ''
  public updateUserName = ''
  public updateAt = ''
  public version = 0
  public fileKeyId = ''
}

export async function GetSolverById(id: string) {
  return await $api.get<GetSolverByIdResponse>(`/solvers/${id}`)
}

export class SolverInfo implements ISolverInfo {
  public name = ''
  public nameKana = ''

  public title = ''
  public email = ''
  public phone = ''
  public issueMap50: string[] = []

  public corporateId = ''
  public birthDay = ''
  public sex = 'not_set'
  public operatingStatus = 'not_working'
  public facePhoto = new FileContent()

  public resume: FileContent[] = []

  public academicBackground = ''
  public workHistory = ''
  public isConsultingFirm = false
  public specializedThemes = ''
  public mainAchievements = ''
  public providedOperatingRate = 0
  public providedOperatingRateNext = 0
  public operationProspectsMonthAfterNext = ''
  public pricePerPersonMonth: any = 0
  public pricePerPersonMonthLower: any = 0
  public hourlyRate: any = 0
  public hourlyRateLower: any = 0
  public englishLevel = 'reading_and_writing'
  public tsiAreas: string[] = []
  public screening1 = {
    evaluation: false,
    evidence: '',
  }

  public screening2 = {
    evaluation: false,
    evidence: '',
  }

  public screening3 = {
    evaluation: false,
    evidence: '',
  }

  public screening4 = {
    evaluation: false,
    evidence: '',
  }

  public screening5 = {
    evaluation: false,
    evidence: '',
  }

  public screening6 = {
    evaluation: false,
    evidence: '',
  }

  public screening7 = {
    evaluation: false,
    evidence: '',
  }

  public screening8 = {
    evaluation: false,
    evidence: '',
  }

  public criteria1 = ''
  public criteria2 = ''
  public criteria3 = ''
  public criteria4 = ''
  public criteria5 = ''
  public criteria6 = ''
  public criteria7 = ''
  public criteria8 = ''
  public notes = ''
  public registrationStatus = ''
  public fileKeyId = ''
  public mode = ''
  public isRegisteredSolver = false
  public index: number = 0
}

export class CreateSolverRequest implements ICreateSolverRequest {
  public solversInfo = [
    {
      mode: '',
      isRegisteredSolver: false,
      name: '',
      sex: 'not_set',
      birthDay: '',
      workHistory: '',
      isConsultingFirm: false,
      specializedThemes: '',
      mainAchievements: '',
      registrationStatus: '',
    },
  ]
}

export class CreateSolverResponse {
  public message = ''
}

export async function CreateSolver(request: CreateSolverRequest) {
  return await $api.post<CreateSolverResponse>(`/solvers`, request)
}

export async function PatchSolverStatusById(id: string, version: number) {
  return await $api.patch(`/solvers/${id}?version=${version}`)
}

export class UpdateSolverRequest implements IUpdateSolverRequest {
  public mode = ''
  public name = ''
  public corporateId = ''
  public sex = 'not_set'
  public birthDay = ''
  public workHistory = ''
  public isConsultingFirm = false
  public specializedThemes = ''
  public mainAchievements = ''
  public isSolver = false
  public registrationStatus = ''
}

export class UpdateSolverResponse {
  public message = ''
}

export async function UpdateSolverById(
  id: string,
  version: number,
  request: UpdateSolverRequest
) {
  return await $api.put<UpdateSolverResponse>(
    `/solvers/${id}?version=${version}`,
    request
  )
}

export class DeleteSolverResponse {
  public message = ''
}

export async function DeleteSolverById(id: string, version: number) {
  return await $api.delete<DeleteSolverResponse>(
    `/solvers/${id}?version=${version}`
  )
}
