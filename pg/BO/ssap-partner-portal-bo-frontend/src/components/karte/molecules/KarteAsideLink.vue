<template>
  <div class="m-karter-aside-links">
    <Title style-set="aside">
      {{ title }}
    </Title>
    <!-- ファイルダウンロード -->
    <ul v-if="isDownload" class="m-karter-aside-links__list">
      <template v-for="(file, index) in links">
        <li :key="index" class="m-karter-aside-links__item">
          <Icon size="12">icon-org-arrow</Icon>
          <a @click="download(file.path, file.fileName)">
            {{ file.fileName }}
          </a>
        </li>
      </template>
    </ul>
    <!-- アンケート -->
    <ul v-else class="m-karter-aside-links__list">
      <template v-for="(survey, index) in links">
        <li
          v-if="isAllowSurveys && showsSurvey(survey)"
          :key="index"
          class="m-karter-aside-links__item"
        >
          <Icon size="12">icon-org-arrow</Icon>
          <!-- アンケートリンク -->
          <template v-if="isAccessableSurvey(survey)">
            <nuxt-link :to="forwardToUrl(`/survey/${survey.id}`)">
              {{
                replaceSurveyMonth(survey.summaryMonth) +
                ' ' +
                switchSurveyType(survey.surveyType)
              }}
            </nuxt-link>
          </template>
          <template v-else>
            {{
              replaceSurveyMonth(survey.summaryMonth) +
              ' ' +
              switchSurveyType(survey.surveyType)
            }}
          </template>
        </li>
      </template>
    </ul>
  </div>
</template>

<script lang="ts">
import { Title, Icon } from '~/components/common/atoms/index'
import BaseComponent from '~/common/BaseComponent'
import { downloadFile } from '~/utils/download'
import { ENUM_GET_SURVEY_TYPE } from '~/types/Survey'
import { Documents, GetKarteByIdResponse } from '~/models/Karte'
import type { PropType } from '~/common/BaseComponent'
import { GetProjectByIdResponse } from '~/models/Project'
import { SurveyListItem } from '~/models/Survey'
import { hasRole } from '~/utils/role-authorizer'
import { meStore } from '~/utils/store-accessor'
import { ADMIN_ROLE } from '@/types/Admin'

export default BaseComponent.extend({
  name: 'KarteAsideLink',
  components: {
    Title,
    Icon,
  },
  props: {
    title: {
      type: String,
      required: true,
    },
    /** ファイル等が保存されているリンク */
    links: {
      type: Array as PropType<Documents[]>,
      required: true,
    },
    /**
     * 閲覧可能なアンケートか否か
     */
    isAllowSurveys: {
      type: Boolean,
    },
    isDownload: {
      type: Boolean,
      default: false,
    },
    project: {
      type: Object as PropType<GetProjectByIdResponse>,
      required: false,
    },
    karte: {
      type: Object as PropType<GetKarteByIdResponse>,
      required: false,
    },
  },
  methods: {
    /**
     * カルテの起票月がアンケートの集計月まで且つ回答済み且つ公開OKのアンケートのテキストリンクを表示
     * @param survey
     */
    showsSurvey(survey: any) {
      // 各項目の月を取得
      const karteDate = new Date(this.karte.date)
      // 集計月初め
      const summaryMonth = new Date(survey.summaryMonth)

      if (summaryMonth <= karteDate) {
        // 回答済み且つ公開OK
        return survey.isFinished && survey.isDisclosure
      }
    },
    /** ファイルクリック時にファイルをダウンロード */
    download(key: string, fileName: string) {
      downloadFile(key).then((res) => {
        const unit8Array = res.Body as Uint8Array
        const blob = new Blob([unit8Array], { type: res.ContentType })
        const url = (window.URL || window.webkitURL).createObjectURL(blob)
        const link = document.createElement('a')
        link.download = fileName
        link.href = url
        document.body.appendChild(link)
        link.click()
        document.body.removeChild(link)
      })
    },
    /**
     * 関連アンケートに表示する年月を返す
     * @param yearMonth アンケート実施年月
     */
    replaceSurveyMonth(yearMonth: string): string {
      const splittedYearMonth = yearMonth.split('/')
      let splittedMonth = splittedYearMonth[1]
      if (splittedMonth.substring(0, 1) === '0') {
        splittedMonth = splittedMonth.replace('0', '')
      }
      return this.$t('common.format.yearMonth', {
        year: splittedYearMonth[0],
        month: splittedMonth,
      }) as string
    },
    /**
     * 「関連アンケート」で表示するアンケート種別の文字列を返す
     * @param type アンケート種別
     */
    switchSurveyType(type: string): string {
      switch (type) {
        case ENUM_GET_SURVEY_TYPE.SERVICE:
          return this.$t('survey.pages.index.table.service.name') as string
        case ENUM_GET_SURVEY_TYPE.COMPLETION:
          return this.$t('survey.pages.index.table.completion.name') as string
        case ENUM_GET_SURVEY_TYPE.QUICK:
          return this.$t('survey.pages.index.table.quick.name') as string
        case ENUM_GET_SURVEY_TYPE.PP:
          return this.$t('survey.pages.index.table.pp.name') as string
        default:
          return ''
      }
    },
    /**
     * アクセス可能なアンケートかどうか確認する
     * @param survey アンケート
     * @returns boolean アクセス可能なアンケートかどうか
     */
    isAccessableSurvey(survey: SurveyListItem) {
      const isSupporterMgr = hasRole([ADMIN_ROLE.SUPPORTER_MGR])
      if (!isSupporterMgr) {
        return true
      }

      const organizationIds: string[] = []

      if (survey.mainSupporterUser) {
        organizationIds.push(survey.mainSupporterUser.organizationId)
      }
      if (survey.supporterUsers) {
        survey.supporterUsers.forEach((user) => {
          organizationIds.push(user.organizationId)
        })
      }

      const belongsTo = meStore.supporterOrganizations.find((organization) =>
        organizationIds.includes(organization.id)
      )

      return belongsTo
    },
  },
})
</script>

<style lang="scss" scoped>
.m-karter-aside-links__list {
  padding: 0;
  margin: 0;
}
.m-karter-aside-links__item {
  list-style: none;
  margin-top: 12px;
  padding-left: 15px;
  text-indent: -15px;
  a {
    @include fontSize($size: 'small');
    font-weight: bold;
    color: $c-primary-dark;
    transition-duration: 0.2s;
    &:hover {
      color: $c-primary-over;
    }
  }
}
</style>
