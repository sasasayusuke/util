// 法人ソルバーの情報を保持するストア

import { Module, VuexModule, Mutation } from 'vuex-module-decorators'

interface SolverCorporationState {
  id: string
  name: string
  companyAbbreviation: string
  industry: string
  established: string
  managementTeam: string
  employee: {
    value: number
    memo: string
  }
  capital: {
    value: number
    memo: string
  }
  earnings: {
    value: number
    memo: string
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
  corporatePhoto: {
    fileName: string
    path: string
  }
  corporateInfoDocument: {
    fileName: string
    path: string
  }[]
  issueMap50: string[]
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
  deputyCharge: {
    name: string
    kana: string
    title: string
    email: string
    department: string
    phone: string
  }
  otherCharge: {
    name: string
    title: string
    email: string
    department: string
    phone: string
  }
  registeredSolverIds: string[]
  notes: string
  disabled: boolean
  createId: string
  createUserName: string
  createAt: string
  priceAndOperatingRateUpdateAt: string
  priceAndOperatingRateUpdateBy: string
  updateId: string
  updateUserName: string
  updateAt: string
  version: number
}

const sampleSolverCorporationState = {
  id: '',
  name: '',
  companyAbbreviation: '',
  industry: '',
  established: '',
  managementTeam: '',
  employee: {
    value: 0,
    memo: '',
  },
  capital: {
    value: 0,
    memo: '',
  },
  earnings: {
    value: 0,
    memo: '',
  },
  listingExchange: '',
  businessContent: '',
  address: {
    postalCode: '',
    state: '',
    city: '',
    street: '',
    building: '',
  },
  corporatePhoto: {
    fileName: '',
    path: '',
  },
  corporateInfoDocument: [
    {
      fileName: '',
      path: '',
    },
  ],
  issueMap50: [''],
  vision: '',
  mission: '',
  mainCharge: {
    name: '',
    kana: '',
    title: '',
    email: '',
    department: '',
    phone: '',
  },
  deputyCharge: {
    name: '',
    kana: '',
    title: '',
    email: '',
    department: '',
    phone: '',
  },
  otherCharge: {
    name: '',
    title: '',
    email: '',
    department: '',
    phone: '',
  },
  registeredSolverIds: [''],
  notes: '',
  disabled: false,
  createId: '',
  createUserName: '',
  createAt: '',
  priceAndOperatingRateUpdateAt: '',
  priceAndOperatingRateUpdateBy: '',
  updateId: '',
  updateUserName: '',
  updateAt: '',
  version: 0,
}

@Module({
  stateFactory: true,
  namespaced: true,
  name: 'solver-corporation',
})
export default class extends VuexModule implements SolverCorporationState {
  id = ''
  name = ''
  companyAbbreviation = ''
  industry = ''
  established = ''
  managementTeam = ''
  employee = {
    value: 0,
    memo: '',
  }

  capital = {
    value: 0,
    memo: '',
  }

  earnings = {
    value: 0,
    memo: '',
  }

  listingExchange = ''
  businessContent = ''
  address = {
    postalCode: '',
    state: '',
    city: '',
    street: '',
    building: '',
  }

  corporatePhoto = {
    fileName: '',
    path: '',
  }

  corporateInfoDocument = [
    {
      fileName: '',
      path: '',
    },
  ]

  issueMap50 = ['']

  vision = ''
  mission = ''
  mainCharge = {
    name: '',
    kana: '',
    title: '',
    email: '',
    department: '',
    phone: '',
  }

  deputyCharge = {
    name: '',
    kana: '',
    title: '',
    email: '',
    department: '',
    phone: '',
  }

  otherCharge = {
    name: '',
    title: '',
    email: '',
    department: '',
    phone: '',
  }

  registeredSolverIds = ['']
  notes = ''
  disabled = false
  createId = ''
  createUserName = ''
  createAt = ''
  priceAndOperatingRateUpdateAt = ''
  priceAndOperatingRateUpdateBy = ''
  updateId = ''
  updateUserName = ''
  updateAt = ''
  version = 0

  @Mutation
  setResponse(response = sampleSolverCorporationState) {
    this.id = response.id
    this.name = response.name
    this.companyAbbreviation = response.companyAbbreviation
    this.industry = response.industry
    this.established = response.established
    this.managementTeam = response.managementTeam
    this.employee = response.employee
    this.capital = response.earnings
    this.listingExchange = response.listingExchange
    this.businessContent = response.businessContent
    this.address = response.address
    this.corporatePhoto = response.corporatePhoto
    this.corporateInfoDocument = response.corporateInfoDocument
    this.issueMap50 = response.issueMap50
    this.vision = response.vision
    this.mission = response.mission
    this.mainCharge = response.mainCharge
    this.deputyCharge = response.deputyCharge
    this.otherCharge = response.otherCharge
    this.registeredSolverIds = response.registeredSolverIds
    this.notes = response.notes
    this.disabled = response.disabled
    this.createId = response.createId
    this.createUserName = response.createUserName
    this.createAt = response.createAt
    this.priceAndOperatingRateUpdateAt = response.priceAndOperatingRateUpdateAt
    this.priceAndOperatingRateUpdateBy = response.priceAndOperatingRateUpdateBy
    this.updateId = response.updateId
    this.updateUserName = response.updateUserName
    this.updateAt = response.updateAt
    this.version = response.version
  }

  @Mutation
  clear() {
    this.id = ''
    this.name = ''
    this.companyAbbreviation = ''
    this.industry = ''
    this.established = ''
    this.managementTeam = ''
    this.employee = {
      value: 0,
      memo: '',
    }
    this.capital = {
      value: 0,
      memo: '',
    }
    this.earnings = {
      value: 0,
      memo: '',
    }
    this.listingExchange = ''
    this.businessContent = ''
    this.address = {
      postalCode: '',
      state: '',
      city: '',
      street: '',
      building: '',
    }
    this.corporatePhoto = {
      fileName: '',
      path: '',
    }
    this.corporateInfoDocument = [
      {
        fileName: '',
        path: '',
      },
    ]
    this.issueMap50 = ['']
    this.vision = ''
    this.mission = ''
    this.mainCharge = {
      name: '',
      kana: '',
      title: '',
      email: '',
      department: '',
      phone: '',
    }
    this.deputyCharge = {
      name: '',
      kana: '',
      title: '',
      email: '',
      department: '',
      phone: '',
    }
    this.otherCharge = {
      name: '',
      title: '',
      email: '',
      department: '',
      phone: '',
    }
    this.registeredSolverIds = ['']
    this.notes = ''
    this.disabled = false
    this.createId = ''
    this.createUserName = ''
    this.createAt = ''
    this.priceAndOperatingRateUpdateAt = ''
    this.priceAndOperatingRateUpdateBy = ''
    this.updateId = ''
    this.updateUserName = ''
    this.updateAt = ''
    this.version = 0
  }
}
