/**
 * 基底クラス(BaseComponent, BasePage)の基底クラス
 * TODO: warning "export 'FormModel' was not found in './BaseVueClass'""の対処
 */

import Vue from 'vue'
import { errorBarStore } from '~/store'
import { signOut, getCurrentDate, adjustEmail } from '~/utils/common-functions'
import {
  UnauthorizedError,
  ForbiddenError,
  InternalServerError,
} from '~/plugins/api'
import { getAllowedRoles } from '~/utils/role-authorizer'

export type { PropType } from 'vue'

export default Vue.extend({
  data() {
    return {}
  },

  methods: {
    formatDate(date: Date, format: string = 'Y/MM/DD hh:mm'): string {
      format = format.replace(/Y/g, date.getFullYear().toString())
      format = format.replace(/MM/g, ('0' + (date.getMonth() + 1)).slice(-2))
      format = format.replace(/M/g, (date.getMonth() + 1).toString())
      format = format.replace(/DD/g, ('0' + date.getDate()).slice(-2))
      format = format.replace(/D/g, date.getDate().toString())
      format = format.replace(/hh/g, ('0' + date.getHours()).slice(-2))
      format = format.replace(/h/g, date.getHours().toString())
      format = format.replace(/mm/g, ('0' + date.getMinutes()).slice(-2))
      format = format.replace(/m/g, date.getMinutes().toString())
      return format
    },
    formatDateYMD(date: Date, format: string = 'Y/M/D') {
      format = format.replace(/Y/g, date.getFullYear().toString())
      format = format.replace(/MM/g, ('0' + (date.getMonth() + 1)).slice(-2))
      format = format.replace(/M/g, (date.getMonth() + 1).toString())
      format = format.replace(/DD/g, ('0' + date.getDate()).slice(-2))
      format = format.replace(/D/g, date.getDate().toString())
      return format
    },
    min2hour(time: number) {
      const hour = Math.floor(time / 60)
      const min = time % 60
      return `${hour < 10 ? `0${hour}` : hour}:${min === 0 ? '00' : min}`
    },
    setScrollHint(message: string = ''): void {
      Vue.prototype.$setScrollHint(message)
    },
    scrollPageTop() {
      window.scroll({ top: 0, behavior: 'smooth' })
    },
    clearErrorBar() {
      errorBarStore.clear()
    },
    showErrorBar(errorMessage: any) {
      errorBarStore.setMessage(errorMessage as string)
    },
    showErrorBarWithScrollPageTop(errorMessage: any) {
      this.showErrorBar(errorMessage)
      this.scrollPageTop()
    },
    apiErrorHandle(error: Error) {
      if (error instanceof UnauthorizedError) {
        signOut()
        return
      }

      let destination = ''

      if (error instanceof ForbiddenError) {
        destination = '/403'
      } else if (error instanceof InternalServerError) {
        destination = '/500'
      } else {
        this.$logger.error(
          'axiosレスポンス取得時に想定外のエラーが発生しました'
        )
        throw error
      }

      this.$router.push(destination)
    },
    displayLoading(promises: Promise<any>[], isModal: boolean = false) {
      if (isModal === true) {
        this.$nuxt.$emit('modalLoading')
      } else {
        this.$nuxt.$emit('componentLoading')
      }
      return Promise.all(
        promises.map((p) => {
          return new Promise((resolve) => {
            p.then((value) => {
              resolve([value, null])
            }).catch((err) => {
              resolve([null, err])
            })
          })
        })
      ).then(() => {
        if (isModal === true) {
          this.$nuxt.$emit('modalLoaded')
        } else {
          this.$nuxt.$emit('componentLoaded')
        }
      })
    },
    forwardToUrl(defaultUrl: string): string {
      let sourceUrl: string = ''
      const allowedRoles = getAllowedRoles(this.$route.path)
      // 不正なリンク防止のためにロールに登録されているもののみ指定可能とする。
      if (allowedRoles && allowedRoles.length && allowedRoles.length > 0) {
        sourceUrl = encodeURIComponent(this.$route.fullPath)
      }
      if (sourceUrl !== '') {
        return `${defaultUrl}?source_url=${sourceUrl}`
      } else {
        return defaultUrl
      }
    },
    backToUrl(defaultUrl: string): string {
      let sourceUrl: string = ''
      if (this.$route.query.source_url && this.$route.query.source_url !== '') {
        const encodedSourceUrl = String(this.$route.query.source_url)
        const decodedSourceUrl = decodeURIComponent(encodedSourceUrl)
        const decodedSourcePath = decodedSourceUrl.split('?')[0]
        const allowedRoles = getAllowedRoles(decodedSourcePath)
        // 不正なリンク防止のためにロールに登録されているもののみ指定可能とする。
        if (allowedRoles && allowedRoles.length && allowedRoles.length > 0) {
          sourceUrl = decodedSourceUrl
        }
      }
      if (sourceUrl !== '') {
        return sourceUrl
      } else {
        return defaultUrl
      }
    },
    getCurrentDate,
    adjustEmail,
  },
})

// export type { PropType } from 'vue'
