<template>
  <Sheet class="m-page-header elevation-4">
    <v-layout v-if="category" class="m-page-header__layout">
      <v-flex align-self-center="true">
        <compnent :is="checkH1" class="m-page-header__title">
          {{ pageHeaderTitle }}
        </compnent>
        <p
          v-if="pageHeaderSubTitle"
          class="m-page-header__name font-size-normal mb-0 txt-limit"
        >
          {{ pageHeaderSubTitle }}
        </p>
      </v-flex>
      <v-flex class="d-flex justify-end" align-self-center="true">
        <template v-if="checkKarteDir()">
          <Button
            :to="`/karte/list/${$store.state['current-page-data'].ppProjectId}`"
            class="m-page-header__button"
            :class="{
              'is-current': $route.path.startsWith('/karte'),
            }"
            style-set="page-header"
          >
            {{ $t('karte.pages.list.subName') }}
          </Button>
          <Button
            :to="`/master-karte/${$store.state['current-page-data'].npfProjectId}`"
            class="m-page-header__button"
            :class="{
              'is-current': $route.path.startsWith('/master-karte'),
            }"
            style-set="page-header"
          >
            {{ $t('master-karte.pages.index.master-karte') }}
          </Button>
          <Button style-set="page-header" :to="toMasterKarteList">
            <Icon size="20" class="mr-3">icon-org-return</Icon>
            {{ $t('common.button.backToList3') }}
          </Button>
        </template>
        <template v-else-if="checkMasterKarteDir()">
          <div v-if="isAccessibleKarten()">
            <Button
              :to="`/karte/list/${$store.state['current-page-data'].ppProjectId}`"
              class="m-page-header__button"
              :class="{
                'is-current': $route.path.startsWith('/karte'),
              }"
              style-set="page-header"
            >
              {{ $t('karte.pages.list.subName') }}
            </Button>
          </div>
          <Button
            :to="`/master-karte/${$store.state['current-page-data'].npfProjectId}`"
            class="m-page-header__button"
            :class="{
              'is-current': $route.path.startsWith('/master-karte'),
            }"
            style-set="page-header"
          >
            {{ $t('master-karte.pages.index.master-karte') }}
          </Button>
          <Button style-set="page-header" :to="toMasterKarteList">
            <Icon size="20" class="mr-3">icon-org-return</Icon>
            {{ $t('common.button.backToList3') }}
          </Button>
        </template>
        <template v-else>
          <template v-for="(item, index) in list[category].buttons">
            <template v-if="hasRole(getAllowedRoles(item.link))">
              <Button
                :key="index"
                :to="item.link"
                class="m-page-header__button"
                :class="{ 'is-current': checkCurrent(item.link) }"
                style-set="page-header"
              >
                {{ item.name }}
              </Button>
            </template>
          </template>
        </template>
      </v-flex>
    </v-layout>
  </Sheet>
</template>

<script lang="ts">
import { format } from 'date-fns'
import { getCurrentDate } from '~/utils/common-functions'
import BaseComponent from '~/common/BaseComponent'
import { Button, Icon, Sheet } from '~/components/common/atoms/index'
import { currentPageDataStore, masterKarteListUrlStore, meStore } from '~/store'
import { hasRole, getAllowedRoles, ADMIN_ROLE } from '~/utils/role-authorizer'
import { ENUM_ADMIN_ROLE } from '~/types/Admin'
import { SupporterOrganization } from '@/models/Admin'
import { GetUsers, GetUsersRequest, UserListItem } from '~/models/User'
import {
  GetMasterKarteById,
  GetMasterKarteByIdResponseClass,
} from '~/models/MasterKarte'

export default BaseComponent.extend({
  components: {
    Button,
    Icon,
    Sheet,
  },
  created() {
    this.category = this.$route.path.replace(/^\//, '').split('/')[0]
    // アンケート事務局ロールの場合「アンケート予実績」は[forecast-survey-ops]に遷移
    if (hasRole([ADMIN_ROLE.SURVEY_OPS])) {
      this.list.survey.buttons[3].link = '/survey/report/forecast-survey-ops'
    }
  },
  watch: {
    $route() {
      this.category = this.$route.path.replace(/^\//, '').split('/')[0]
      if (this.checkMasterKarteDir()) {
        this.getMasterKarteById()
        this.getUserInfo()
      }
    },
  },
  computed: {
    /**
     * @return ページタイトル
     */
    pageHeaderTitle(): string {
      // VuexのppProjectIdを"個別カルテ・スケジュール"タブのリンクに追加
      if (this.$store.state['current-page-data'].ppProjectId) {
        this.list[
          'master-karte'
        ].buttons[0].link = `/karte/list/${this.$store.state['current-page-data'].ppProjectId}`
      }

      // VuexのnpfProjectIdを"マスターカルテ"タブのリンクに追加
      if (this.$store.state['current-page-data'].npfProjectId) {
        this.list[
          'master-karte'
        ].buttons[1].link = `/master-karte/${this.$store.state['current-page-data'].npfProjectId}`
      }
      if (
        (this.checkKarteDir() || this.checkMasterKarteDir()) &&
        this.$store.state['current-page-data'].projectName
      ) {
        this.pageHeaderSubTitle =
          this.$store.state['current-page-data'].customerName
        return this.$store.state['current-page-data'].projectName || ''
      } else {
        currentPageDataStore.clear()
        this.pageHeaderSubTitle = ''
        return this.list[this.category].categoryName
      }
    },
    /**
     * ルーティング別 タイトルのタグを変更
     * @return h1 | p
     */
    checkH1() {
      const list = this.h1List as string[]
      const path = this.$route.path.replace(/^\//, '')
      return list.includes(path) ? 'h1' : 'p'
    },
    /**
     * マスターカルテ一覧ページへ戻る際に、検索条件を引き継ぐ
     */
    toMasterKarteList() {
      const masterKarteListParams = masterKarteListUrlStore.params
        ? new URLSearchParams(
            JSON.parse(masterKarteListUrlStore.params)
          ).toString()
        : ''

      return `/karte/list?${masterKarteListParams}`
    },
  },
  methods: {
    /**
     * 各ルーティングページの処理
     * @params 各パスのリンク
     * @return 真偽値 & 正規表現 & null
     */
    hasRole,
    getAllowedRoles,
    checkCurrent(url: string) {
      const path = this.$route.path
      const category = this.category
      const current = this.list[category].detailCurrent
      const base = this.list[category].base
      const yyyymm = '[0-9]{4}/[0-9]+'
      const detail = '[0-9a-z]{8}-([0-9a-z]{4}-){3}[0-9a-z]{12}'
      const surveyResult = new RegExp(`^/${category}/${detail}`)
      if (url.match(/^\/survey/)) {
        const regMaster = new RegExp(`^/${base || category}/master`)
        if (path.match(regMaster)) {
          return url.match(/^\/survey\/master/)
        } else if (path.match(surveyResult)) {
          return url === current
        } else {
          return path === url
        }
      } else if (url.match(/^\/master-karte/)) {
        return true
      } else if (url.match(/^\/man-hour/)) {
        const regSupporter = new RegExp(
          `^/${base || category}/supporter/${yyyymm}`
        )
        const regReport = new RegExp(`^/${base || category}/report/${yyyymm}`)
        const regReportProject = new RegExp(
          `^/${base || category}/report/${yyyymm}/project`
        )
        const regAlertConfig = new RegExp(
          `^/${base || category}/alert/config/?`
        )
        const regAlertDetail = new RegExp(
          `^/${base || category}/alert/${yyyymm}/${detail}`
        )
        if (path.match(regSupporter)) {
          return url.match(/^\/man-hour\/supporter/)
        } else if (path.match(regReport) && !path.match(/\/project/)) {
          return url.match(/^\/man-hour\/report/) && !url.match(/\/project/)
        } else if (path.match(regReportProject)) {
          return url.match(/^\/man-hour\/report/) && url.match(/\/project/)
        } else if (path.match(regAlertConfig) || path.match(regAlertDetail)) {
          return url.match(/^\/man-hour\/alert\/list/)
        } else if (path.match(/^\/man-hour$/) || path.match(/^\/man-hour\/$/)) {
          return url.match(/^\/man-hour$/)
        } else {
          return path === url
        }
      } else if (url.match(/^\/karte/)) {
        return path.match(/^\/karte\/list\/?/)
      } else if (url.match(/^\/master/)) {
        const regDetail = new RegExp(
          `^/${category}/master_service_type/${detail}`
        )
        const regDetailAll = new RegExp(`^/${category}/all/${detail}`)
        const regDetailId = new RegExp(`^/${category}/${detail}`)
        const regDetailSupporterOrganizat = new RegExp(
          `^/${category}/master_supporter_organization/${detail}`
        )

        const list = '/master/list'
        if (
          path.match(regDetail) ||
          path.match(regDetailAll) ||
          path.match(regDetailId) ||
          path.match(regDetailSupporterOrganizat)
        ) {
          return url.match(new RegExp(list))
        } else {
          return path === url
        }
      } else {
        const regDetail = new RegExp(`^/${base || category}/${detail}`)
        const regImport = new RegExp(`^/${base || category}/import`)
        if (path.match(regImport)) {
          return url.match(regImport)
        } else {
          return path.match(regDetail) ? current === url : path === url
        }
      }
    },
    /**
     * 個別カルテ・スケジュールタブを表示させるか否か
     * @return 表示させるのであればtrue
     */
    isAccessibleKarten(): boolean {
      if (
        meStore.roles.includes(ENUM_ADMIN_ROLE.SYSTEM_ADMIN) ||
        meStore.roles.includes(ENUM_ADMIN_ROLE.SALES_MGR) ||
        meStore.roles.includes(ENUM_ADMIN_ROLE.BUSINESS_MGR)
      ) {
        // システム管理者、営業責任者,事業者責任者
        return true
      } else if (
        meStore.roles.length === 1 &&
        meStore.roles.includes(ENUM_ADMIN_ROLE.SUPPORTER_MGR)
      ) {
        // 支援者責任者
        if (
          !this.masterKarteProject.supporterOrganizationId &&
          this.countCalledGetMasterKarteByIdApi === 0
        ) {
          this.getMasterKarteById()
        } else if (this.masterKarteProject.supporterOrganizationId === null) {
          // マスターカルテ詳細情報の支援者組織IDがnullの場合
          return false
        } else if (this.checkMasterKarteDir()) {
          for (const i in meStore.supporterOrganizations) {
            const supporterOrganization: SupporterOrganization =
              meStore.supporterOrganizations[i]
            if (
              supporterOrganization.id &&
              supporterOrganization.id ===
                this.masterKarteProject.supporterOrganizationId
            ) {
              // 所属課案件
              return true
            }
          }
        }
      } else if (
        meStore.roles.length === 1 &&
        meStore.roles.includes(ENUM_ADMIN_ROLE.SALES)
      ) {
        // 営業担当者
        if (
          this.userInfo.projectIds.includes(this.masterKarteProject.ppProjectId)
        ) {
          // 担当案件
          return true
        }
      }
      return false
    },
    /**
     * 現在のルーティングが/karteか判定
     * @return カルテであればtrue
     */
    checkKarteDir() {
      const path = this.$route.path
      const detail = '/karte/[0-9a-z]{8}-([0-9a-z]{4}-){3}[0-9a-z]{12}'
      const list = '/karte/list/[0-9a-z]{8}-([0-9a-z]{4}-){3}[0-9a-z]{12}'
      return path.match(new RegExp(list)) || path.match(new RegExp(detail))
    },
    /**
     * 現在のルーティングが/master-karte/{_npfId}か判定
     * @return カルテであればtrue
     */
    checkMasterKarteDir() {
      const path = this.$route.path
      const list = '/master-karte/.*'
      return path.match(new RegExp(list))
    },
    /**
     * マスターカルテ詳細情報取得APIから案件情報を取得
     */
    async getMasterKarteById() {
      const npfProjectId = this.$route.params.npfProjectId
      await GetMasterKarteById(npfProjectId)
        .then((res: any) => {
          this.masterKarteProject = res.data
          this.countCalledGetMasterKarteByIdApi += 1
        })
        .catch((error) => {
          this.apiErrorHandle(error)
        })
    },
    /**
     * GetUsersを叩き、管理ユーザーと一致する一般ユーザー情報を取得
     */
    async getUserInfo() {
      if (meStore.roles.includes(ENUM_ADMIN_ROLE.SALES)) {
        const request = new GetUsersRequest()
        request.email = meStore.email
        await GetUsers(request)
          .then((res) => {
            this.userInfo = res.data.users[0]
          })
          .catch((error) => {
            this.apiErrorHandle(error)
          })
      }
    },
  },
  data() {
    const now = getCurrentDate()
    const year = format(now, 'yyyy')
    const month = format(now, 'MM')

    return {
      category: '',
      pageHeaderSubTitle: '',
      masterKarteProject: new GetMasterKarteByIdResponseClass(),
      userInfo: new UserListItem(),
      countCalledGetMasterKarteByIdApi: 0,
      list: {
        admin: {
          categoryName: this.$t('admin.group_info.name'),
          buttons: [
            { name: this.$t('admin.pages.index.name'), link: '/admin/list' },
            {
              name: this.$t('admin.pages.create.name'),
              link: '/admin/create',
            },
          ],
          detailCurrent: '/admin/list',
        },
        user: {
          categoryName: this.$t('user.group_info.name'),
          buttons: [
            { name: this.$t('user.pages.index.name'), link: '/user/list' },
            { name: this.$t('user.pages.create.name'), link: '/user/create' },
          ],
          detailCurrent: '/user/list',
        },
        master: {
          categoryName: this.$t('master.group_info.name'),
          buttons: [
            {
              name: this.$t('master.pages.index.name'),
              link: '/master/list',
            },
            {
              name: this.$t('master.pages.create.name'),
              link: '/master/create',
            },
          ],
          base: 'master/master_service_type',
          detailCurrent: '/master/list',
        },
        customer: {
          categoryName: this.$t('customer.group_info.name'),
          buttons: [
            {
              name: this.$t('customer.pages.index.name'),
              link: '/customer/list',
            },
            {
              name: this.$t('customer.pages.create.name'),
              link: '/customer/create',
            },
          ],
          detailCurrent: '/customer/list',
        },
        karte: {
          categoryName: this.$t('karte.group_info.name'),
          buttons: [
            {
              name: this.$t('karte.pages.index.name'),
              link: '/karte/list',
            },
          ],
          detailCurrent: '/karte/list',
        },
        project: {
          categoryName: this.$t('project.group_info.name'),
          buttons: [
            {
              name: this.$t('project.pages.index.name'),
              link: '/project/list',
            },
            {
              name: this.$t('project.pages.create.name'),
              link: '/project/create',
            },
          ],
          detailCurrent: '/project/list',
        },
        'man-hour': {
          categoryName: this.$t('man-hour.group_info.name'),
          customerName: '',
          buttons: [
            {
              name: this.$t('man-hour.pages.index.name'),
              link: '/man-hour',
            },
            {
              name: this.$t('man-hour.pages.supporter.index.name'),
              link: `/man-hour/supporter/${year}/${month}`,
            },
            {
              name: this.$t('man-hour.pages.report.index.name'),
              link: `/man-hour/report/${year}/${month}`,
            },
            {
              name: this.$t('man-hour.pages.report.project.name'),
              link: `/man-hour/report/${year}/${month}/project`,
            },
            {
              name: this.$t('man-hour.pages.alert.list.name'),
              link: '/man-hour/alert/list',
            },
          ],
        },
        survey: {
          categoryName: this.$t('survey.group_info.name'),
          buttons: [
            {
              name: this.$t('survey.pages.index.name'),
              link: '/survey',
            },
            {
              name: this.$t('survey.pages.result.name'),
              link: '/survey/list',
            },
            {
              name: this.$t('survey.pages.report.name'),
              link: '/survey/report/monthly',
            },
            {
              name: this.$t('survey.pages.forecast.name'),
              link: '/survey/report/forecast',
            },
            {
              name: this.$t('survey.pages.model.name'),
              link: '/survey/master/list',
            },
          ],
          detailCurrent: '/survey/list',
        },
        'master-karte': {
          categoryName: this.$t('master-karte.group_info.name'),
          buttons: [
            {
              name: this.$t('karte.pages.list.subName'),
              link: `/karte/list/${this.$store.state['current-page-data'].ppProjectId}`,
            },
            {
              name: this.$t('master-karte.pages.index.master-karte'),
              link: `/master-karte/${this.$store.state['current-page-data'].npfProjectId}`,
            },
          ],
          detailCurrent: '/master-survey/list',
        },
      } as any,
      h1List: ['survey'],
    }
  },
})
</script>

<style lang="scss" scoped>
.m-page-header {
  height: 60px;
  background-color: $c-white !important;
}
.m-page-header__layout {
  max-width: 1200px;
  margin: 0 auto;
}
.m-page-header__title {
  font-size: 1.125rem;
  font-weight: bold;
  margin: 0;
}
.m-page-header__button {
  position: relative;
  font-weight: normal !important;
  &::before {
    display: none !important;
  }
  &:hover,
  &:focus,
  &.is-current {
    color: $c-primary-dark !important;
    font-weight: bold !important;
    &::after {
      background-color: $c-primary-dark;
    }
  }
  &::after {
    content: '';
    display: block !important;
    width: 100%;
    height: 6px;
    background-color: $c-white;
    position: absolute;
    left: 0;
    bottom: 0;
    transition-duration: 0.2s;
  }
}
// 取引先名のテキストが長い場合は、テキストを省略する
.txt-limit {
  width: 700px;
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
}
</style>
