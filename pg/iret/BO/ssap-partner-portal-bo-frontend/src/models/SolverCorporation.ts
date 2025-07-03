import { Api } from '~/plugins/api'
import {
  ISuggestSolverCorporationsRequest,
  ENUM_SUGGEST_SOLVER_CORPORATIONS_REQUEST_SORT,
  ISuggestSolverCorporation,
} from '@/types/SolverCorporation'

const $api = new Api()

export class SuggestSolverCorporationsRequest
  implements ISuggestSolverCorporationsRequest
{
  sort = ENUM_SUGGEST_SOLVER_CORPORATIONS_REQUEST_SORT.NAME_ASC
}

export class SuggestSolverCorporation implements ISuggestSolverCorporation {
  id = ''
  name = ''
}

export class SuggestSolverCorporationsResponse extends Array<SuggestSolverCorporation> {}

export async function SuggestSolverCorporations(
  request: SuggestSolverCorporationsRequest = new SuggestSolverCorporationsRequest()
) {
  return await $api.get<SuggestSolverCorporationsResponse>(
    '/solvers/corporations/suggest',
    request
  )
}
