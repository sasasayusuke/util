export const ENUM_SUGGEST_SOLVER_CORPORATIONS_REQUEST_SORT = {
  NAME_ASC: 'name:asc',
} as const

export type SuggestSolverCorporationsRequestSort =
  typeof ENUM_SUGGEST_SOLVER_CORPORATIONS_REQUEST_SORT[keyof typeof ENUM_SUGGEST_SOLVER_CORPORATIONS_REQUEST_SORT]

export interface ISuggestSolverCorporationsRequest {
  sort: SuggestSolverCorporationsRequestSort
}

export interface ISuggestSolverCorporation {
  id: string
  name: string
}
