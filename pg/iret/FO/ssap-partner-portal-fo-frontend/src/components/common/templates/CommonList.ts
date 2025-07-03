import BaseComponent from '~/common/BaseComponent'
import type { PropType } from '~/common/BaseComponent'

// TODO: コメントアウトした部分を使った形に書き換えること

/*
import { DataOptions } from 'vuetify'

class SearchParam {
  [key: string]: any
}

class Request {
  offsetPage? = 0
  limit? = 10
  sort? = ''
}

class Response {
  total? = 0
  offsetPage? = 0
}

class IsLoading {}

function createVueObject<RQ extends Request = Request, RP extends Response = Response, S extends SearchParam = SearchParam, L extends IsLoading = IsLoading>(
  requestClass: (new () => RQ),
  responseClass: (new () => RP),
  searchParamClass: (new () => S),
  isLoadingClass: (new () => L),
) {
  type Data = {
    apiName: string
    searchParam: S
    lastSearchRequest: RQ

    limit?: number
  }

  return BaseComponent.extend({
    // 型情報の置換のために全てオーバーライドすること
    props: {
      response: {
        type: Object as PropType<RP>,
        required: true,
      },
      isLoading: {
        type: Object as PropType<L>,
        required: true,
      },
    },
    // オーバーライド不要
    data() :Data {
      const rtn: Data = {
        // 要オーバーライド
        apiName: '',

        // オーバーライド不要(createdで自動読み込み)
        searchParam: new searchParamClass(),
        lastSearchRequest: new requestClass(),


      }

      const limit = new requestClass().limit

      if (limit) {
        rtn.limit = limit
      }

      return rtn
    },
    // オーバーライド不要
    computed: {
      total() {
        return this.response.total
      },
      offsetPage() {
        return this.response.offsetPage
      },
    },
    // オーバーライド不要
    methods: {
      get(request: RQ) {
        if (!this.apiName) {
          Error('apiName should be set')
        }
        this.$emit(this.apiName, request)
      },
      updateParam(keyName: keyof S, newVal: any) {
        this.searchParam[keyName] = newVal
      },
      clear() {
        this.searchParam = new searchParamClass()
        this.search()
      },
      search() {
        const request = new requestClass()
        Object.assign(request, this.searchParam)
        this.lastSearchRequest = request
        this.get(request)
      },
      sort(options: DataOptions) {
        const sortKey: string = options.sortBy[0]
        const sortType: string = options.sortDesc[0] ? 'desc' : 'asc'

        this.lastSearchRequest.sort = `${sortKey}:${sortType}`
        this.get(this.lastSearchRequest)
      },
      nextPage() {
        this.lastSearchRequest.offsetPage!++
        this.get(this.lastSearchRequest)
      },
      prevPage() {
        this.lastSearchRequest.offsetPage!--
        this.get(this.lastSearchRequest)
      },
    },
  }
}
*/

class SearchParam {
  [key: string]: string
}

class Request {
  offsetPage = 0
  limit = 10
  sort = ''
}

class Response {
  total = 0
  offsetPage = 0
}

class IsLoading {}

export default BaseComponent.extend({
  // 型情報の置換のために全てオーバーライドすること
  props: {
    response: {
      type: Object as PropType<Response>,
      required: true,
    },
    isLoading: {
      type: Object as PropType<IsLoading>,
      required: true,
    },
  },
  // オーバーライド不要
  created() {
    this.limit = new this.RequestType().limit
    this.lastSearchRequest = new this.RequestType()
    this.searchParam = new this.SearchParamType()
  },
  data() {
    return {
      // 要オーバーライド
      SearchParamType: SearchParam,
      RequestType: Request,
      headerPageName: '',
      pageName: '',
      buttons: [],
      apiName: '',

      // オーバーライド不要(createdで自動読み込み)
      limit: 0,
      searchParam: new SearchParam(),
      lastSearchRequest: new Request(),
    }
  },
  // オーバーライド不要
  computed: {
    total() {
      return this.response.total
    },
    offsetPage() {
      return this.response.offsetPage
    },
  },
  // オーバーライド不要
  methods: {
    updateParam(keyName: string, newVal: any) {
      this.searchParam[keyName] = newVal
    },
    clear() {
      this.searchParam = new this.SearchParamType()
      this.search()
    },
    search() {
      const request = new this.RequestType()
      Object.assign(request, this.searchParam)
      this.lastSearchRequest = request
      this.$emit(this.apiName, request)
    },
    sort(options: any) {
      const request = this.lastSearchRequest

      const sortKey: string = options.sortBy[0]
      const sortType: string = options.sortDesc[0] ? 'desc' : 'asc'
      if (sortKey) {
        request.sort = `${sortKey}:${sortType}`
      }

      this.lastSearchRequest = request
      this.$emit(this.apiName, request)
    },
    nextPage() {
      const request = this.lastSearchRequest
      request.offsetPage++

      this.$emit(this.apiName, request)
    },
    prevPage() {
      const request = this.lastSearchRequest
      request.offsetPage--

      this.$emit(this.apiName, request)
    },
  },
})
