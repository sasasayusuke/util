<template>
  <Sheet v-if="title || (!title && isSolver)" class="m-page-header elevation-4">
    <v-layout class="m-page-header__inner">
      <v-flex class="m-page-header__info" align-self-center="true">
        <h1 class="m-page-header__title">
          {{ title }}
        </h1>
        <p
          v-if="subTitle"
          class="m-page-header__name font-size-normal mb-0 txt-limit"
        >
          {{ subTitle }}
        </p>
      </v-flex>
      <v-flex class="d-flex justify-end" align-self-center="true">
        <div v-for="(button, index) in headerItems.buttons" :key="index">
          <PageHeaderButton
            :button="button"
            class="m-page-header__button"
            :class="{ 'is-current': isCurrent(button) }"
          >
          </PageHeaderButton>
        </div>

        <template v-if="headerItems.backTo">
          <PageHeaderButton
            :button="headerItems.backTo"
            class="m-page-header__button--back"
          >
            <div>
              <Icon size="19" class="mt-n1">icon-org-return</Icon>
            </div>
            <span class="m-alert-label--blue ma-0 pl-2">
              {{ $t(headerItems.backTo.label) }}
            </span>
          </PageHeaderButton>
        </template>
      </v-flex>
    </v-layout>
  </Sheet>
</template>

<script lang="ts">
import PageHeaderButton, { IButton } from './PageHeaderButton.vue'
import BaseComponent from '~/common/BaseComponent'
import { Img, Icon, Sheet } from '~/components/common/atoms/index'
import { ENUM_USER_ROLE } from '~/types/User'

interface headerItem {
  title?: string
  subTitle?: string
  buttons: IButton[]
  backTo?: IButton
}

/*
  上から順に先頭一致検索するため、/project/list/meは/projectより先に書く必要がある
  記述順に気を付けること
*/
const headerItemDefine: { [path: string]: headerItem } = {
  '/karte': {
    title: '_projectName',
    subTitle: '_customerName',
    buttons: [
      {
        label: 'karte.pages.tab.karte',
        to: '/karte/list/_projectId',
        isCurrent: true,
      },
      {
        label: 'master-karte.pages.header.buttons.master-karte',
        to: '/master-karte/_npfProjectId',
      },
      {
        label: 'karte.pages.tab.survey',
        to: '/survey/list/_projectId',
      },
      { label: 'karte.pages.tab.project', to: '/project/_projectId' },
    ],
    backTo: {
      label: 'header.page.menu.toProjects',
      to: '/project/list',
    },
  },
  '/project/list': {
    title: 'survey.group_info.name',
    subTitle: '_myInfo',
    buttons: [
      {
        label: 'header.page.menu.answer',
        to: '/survey/pp/list',
      },
      {
        label: 'header.page.menu.projects',
        to: '/project/list/me',
      },
      {
        label: 'header.page.menu.results',
        to: '/survey/admin/list',
      },
    ],
  },
  '/project': {
    title: '_projectName',
    subTitle: '_customerName',
    buttons: [
      { label: 'project.pages.tab.karte', to: '/karte/list/_projectId' },
      {
        label: 'master-karte.pages.header.buttons.master-karte',
        to: '/master-karte/_npfProjectId',
      },
      {
        label: 'project.pages.tab.survey',
        to: '/survey/list/_projectId',
      },
      {
        label: 'project.pages.tab.project',
        to: '/project/_projectId',
      },
    ],
    backTo: {
      label: 'header.page.menu.toProjects',
      to: '/project/list',
    },
  },
  '/survey/admin/list': {
    title: 'survey.group_info.name',
    subTitle: '_myInfo',
    buttons: [
      {
        label: 'header.page.menu.answer',
        to: '/survey/pp/list',
      },
      {
        label: 'header.page.menu.projects',
        to: '/project/list/me',
      },
      {
        label: 'header.page.menu.results',
        to: '/survey/admin/list',
      },
    ],
  },
  '/survey/pp': {
    title: 'survey.group_info.name',
    subTitle: '_myInfo',
    buttons: [
      {
        label: 'header.page.menu.answer',
        to: 'survey/pp/list',
        isCurrent: true,
      },
      {
        label: 'header.page.menu.projects',
        to: '/project/list/me',
      },
      {
        label: 'header.page.menu.results',
        to: '/survey/admin/list',
      },
    ],
  },
  '/survey': {
    title: '_projectName',
    subTitle: '_customerName',
    buttons: [
      {
        label: 'header.page.menu.karteList',
        to: '/karte/list/_projectId',
      },
      {
        label: 'master-karte.pages.header.buttons.master-karte',
        to: '/master-karte/_npfProjectId',
      },
      {
        label: 'header.page.menu.survey',
        to: '/survey/list/_projectId',
        isCurrent: true,
      },
      {
        label: 'header.page.menu.project',
        to: '/project/_projectId',
      },
    ],
    backTo: {
      label: 'header.page.menu.toProjects',
      to: '/project/list',
    },
  },
  '/anonymous-survey': {
    title: '_projectName',
    subTitle: '_customerName',
    buttons: [],
  },
  '/satisfaction-survey': {
    title: '_projectName',
    subTitle: '_customerName',
    buttons: [],
  },
  '/man-hour/_month/_year': {
    title: 'man-hour.group_info.name',
    buttons: [
      {
        label: 'man-hour.pages.list.name',
        to: '/man-hour/',
      },
      {
        label: 'man-hour.pages.admin.name',
        to: '/man-hour/admin',
      },
    ],
  },
  '/man-hour': {
    title: 'man-hour.group_info.name',
    subTitle: '_myInfo',
    buttons: [
      {
        label: 'man-hour.pages.list.name',
        to: '/man-hour',
      },
      // 担当者別 フェーズ1で落とした箇所
      // {
      //   name: 'man-hour.pages.admin.name'),
      //   link: '/man-hour/admin',
      // },
    ],
  },
  '/master-karte/': {
    title: '_projectName',
    subTitle: '_customerName',
    buttons: [
      {
        label: 'master-karte.pages.header.buttons.karte',
        to: '/karte/list/_projectId',
      },
      {
        label: 'master-karte.pages.header.buttons.master-karte',
        to: '/master-karte/_npfProjectId',
        isCurrent: true,
      },
      {
        label: 'master-karte.pages.header.buttons.survey',
        to: '/survey/list/_projectId',
      },
      {
        label: 'master-karte.pages.header.buttons.project',
        to: '/project/_projectId',
      },
    ],
    backTo: {
      label: 'header.page.menu.toProjects',
      to: '/project/list',
    },
  },
  '/solver': {
    title: '_solverCorporationName',
    buttons: [],
    backTo: {
      label: 'common.button.solverTop2',
      to: '/solver/menu',
    },
  },
  '/user-policy': {
    title: '',
    buttons: [],
  },
}

export default BaseComponent.extend({
  components: {
    Img,
    Icon,
    Sheet,
    PageHeaderButton,
  },

  computed: {
    headerItems(): headerItem {
      const currentPath = this.$route.path

      // role-authorizerの正規表現オブジェクトを使う形に書き換えたほうがよい？
      // startsWithだけで対応が困難なパターンが出たら書き換えてもよい
      const hasYearMonthQuery =
        this.$route.params.year && this.$route.params.month
      if (currentPath.startsWith('/man-hour') && hasYearMonthQuery) {
        return headerItemDefine['/man-hour/_month/_year']
      }

      const keys = Object.keys(headerItemDefine)

      for (let i = 0; i < keys.length; i++) {
        if (currentPath.startsWith(keys[i])) {
          this.$logger.info('header define found: ', headerItemDefine[keys[i]])
          return headerItemDefine[keys[i]]
        }
      }

      this.$logger.info('header define not found: ', headerItemDefine)

      return headerItemDefine['']
    },
    title(): string {
      if (this.headerItems && this.headerItems.title) {
        const title = this.headerItems.title
        this.$logger.info('title: ', this.headerItems.title)
        if (title === '_projectName') {
          return this.$store.state['current-page-data'].projectName
        } else if (title === '_solverCorporationName') {
          return this.$store.state['solver-corporation'].name
            ? `${this.$store.state['solver-corporation'].name}`
            : ''
        }
        return this.$t(title) as string
      } else {
        return ''
      }
    },
    subTitle(): string {
      if (this.headerItems && this.headerItems.subTitle) {
        const subTitle = this.headerItems.subTitle
        if (subTitle === '_myInfo') {
          return this.$store.state.me.company
            ? `${this.$store.state.me.name} ( ${this.$store.state.me.company} )`
            : this.$store.state.me.name
        }

        if (subTitle === '_customerName') {
          return this.$store.state['current-page-data'].customerName || ''
        }
      }
      return ''
    },
    isSolver(): boolean {
      return (
        this.$store.state.me.role === ENUM_USER_ROLE.APT ||
        this.$store.state.me.role === ENUM_USER_ROLE.SOLVER_STAFF
      )
    },
  },
  methods: {
    // TODO: PageHeaderButtonからimportするように書き換える
    isCurrent(button: IButton): boolean {
      const currentPath = this.$route.path
      if (button.isCurrent) {
        return true
      }
      let targetPath = button.to
      targetPath = targetPath.replace('_projectId', '')
      this.$logger.info('logger', currentPath, targetPath)
      return currentPath.startsWith(targetPath)
    },
  },
})
</script>

<style lang="scss" scoped>
.m-page-header {
  z-index: 2;
  position: relative;
  height: 60px;
  background-color: $c-white !important;
  &:before {
    content: '';
    position: absolute;
    width: 100%;
    height: 1px;
    background: $c-gray-line-light;
  }
  &.elevation-4 {
    box-shadow: 0px 2px 4px -1px rgb(0 0 0 / 20%),
      0px 4px 5px 0px rgb(0 0 0 / 14%), 0px 1px 10px -10px rgb(0 0 0 / 12%) !important;
  }
}
.m-page-header__inner {
  max-width: 1200px;
  margin: 0 auto;
}
.m-page-header__info {
  display: flex;
  flex-direction: column;
  justify-content: center;
  height: 60px;
}
.m-page-header__title {
  font-size: 1.125rem;
  font-weight: bold;
}
.m-page-header__text {
  margin-bottom: 0;
}
.m-page-header__button {
  position: relative;
  font-weight: normal !important;
  &::before {
    display: none !important;
  }
  &:hover,
  &:focus {
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
  &.is-current {
    color: $c-primary-dark !important;
    font-weight: bold !important;
    &::after {
      background-color: $c-primary-dark;
    }
  }
}
.m-page-header__button--back {
  .v-icon {
    transition-duration: 0.2s;
    &:hover,
    &:focus {
      caret-color: $c-primary-dark;
    }
  }
}

// 取引先名のテキストが長い場合は、テキストを省略する
.txt-limit {
  width: 500px;
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
}
</style>
