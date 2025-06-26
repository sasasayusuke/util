import BaseComponent from '~/common/BaseComponent'
import type { PropType } from '~/common/BaseComponent'

class SearchParam {
  [key: string]: string
}

class Request {
  offsetPage = 0
  limit = 20
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
      const validSearchParam: { [key: string]: any } = {}
      Object.keys(this.searchParam).forEach((key: string) => {
        if (this.searchParam[key] !== '') {
          validSearchParam[key] = this.searchParam[key]
        }
      })
      Object.assign(request, validSearchParam)
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
