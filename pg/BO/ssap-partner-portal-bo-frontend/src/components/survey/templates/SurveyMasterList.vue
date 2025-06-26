<template>
  <RootTemPlate pt-8 pb-10 mb-10>
    <ListInPageHeader :create-page-route="roleCheck">
      {{ pageName }}
    </ListInPageHeader>
    <SurveyMasterListTable
      :survey-masters="response.masters"
      :total="total"
      :is-loading="isLoading"
    />
  </RootTemPlate>
</template>

<script lang="ts">
import ListInPageHeader from '~/components/common/organisms/ListInPageHeader.vue'
import { GetSurveyMastersResponse } from '~/models/Master'
import SurveyMasterListTable from '~/components/survey/organisms/SurveyMasterListTable.vue'
import type { PropType } from '~/common/BaseComponent'
import RootTemPlate from '~/components/common/bases/RootTemplate.vue'
import { Button, Sheet } from '~/components/common/atoms'
import CommonList from '~/components/common/templates/CommonList'
import { meStore } from '~/store'

export default CommonList.extend({
  components: {
    RootTemPlate,
    ListInPageHeader,
    SurveyMasterListTable,
    Button,
    Sheet,
  },
  props: {
    /** アンケートマスター一覧 */
    response: {
      type: Object as PropType<GetSurveyMastersResponse>,
      required: true,
    },
    /** 読み込み中か */
    isLoading: {
      type: Boolean,
      required: true,
    },
  },
  data(): {
    apiName: 'getSurveyMasters'
    headerPageName: string
    pageName: string
    buttons: object[]
  } {
    return {
      apiName: 'getSurveyMasters',
      headerPageName: this.$t('survey.group_info.name') as string,
      pageName: this.$t('survey.pages.masterList.name') as string,
      buttons: [
        {
          name: this.$t('survey.pages.index.name'),
          link: '',
        },
        { name: this.$t('survey.pages.result.name'), link: '' },
        {
          name: this.$t('survey.pages.report.name'),
          link: '',
        },
        {
          name: this.$t('survey.pages.performance.name'),
          link: '',
        },
        {
          name: this.$t('survey.pages.model.name'),
          link: '/survey/master/list',
        },
      ],
    }
  },
  computed: {
    /**
     * システム管理者、またはアンケート事務局のみ新規作成ボタンを表示
     * @returns アンケートマスター新規作成ページのURL
     */
    roleCheck() {
      if (
        meStore.roles.includes('system_admin') ||
        meStore.roles.includes('survey_ops')
      ) {
        return '/survey/master/create'
      } else {
        return ''
      }
    },
  },
})
</script>
