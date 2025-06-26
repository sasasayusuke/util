<template>
  <RootTemplate>
    <SurveyMasterDetailContainer
      :title="title"
      :survey-master="surveyMaster"
      is-editing
      is-creating
      :is-loading="isLoading"
      :is-loading-button="isLoadingButton"
      @click:positive="onClickPositive"
      @click:negative="onClickNegative"
    />
  </RootTemplate>
</template>

<script lang="ts">
import SurveyMasterDetailContainer, {
  LocalSurveyMaster,
} from '../organisms/SurveyMasterDetailContainer.vue'
import CommonCreate from '~/components/common/templates/CommonCreateTemplate.vue'
import { PropType } from '~/common/BaseComponent'
import RootTemplate from '~/components/common/bases/RootTemplate.vue'

import {
  CreateSurveyMastersRequest,
  CreateSurveyMasters,
} from '~/models/Master'

export { LocalSurveyMaster }

export default CommonCreate.extend({
  components: {
    SurveyMasterDetailContainer,
    RootTemplate,
  },
  props: {
    /** アンケートマスター */
    surveyMaster: {
      type: Object as PropType<CreateSurveyMastersRequest>,
      required: true,
    },
  },
  data() {
    return {
      title: this.$t('survey.pages.create.name'),
      listPagePath: '/survey/master/list',
      detailPagePrefix: '/survey/master/',
      isLoadingButton: false,
    }
  },
  methods: {
    /**
     * 入力したアンケートマスターを新規作成
     * @param localSurveyMaster 入力したアンケートマスター情報
     */
    create(localSurveyMaster: LocalSurveyMaster) {
      this.isLoading = true
      this.isLoadingButton = true

      const request = new CreateSurveyMastersRequest()
      Object.assign(request, localSurveyMaster)

      CreateSurveyMasters(request)
        .then((res) => {
          const id = res.data.id
          this.toDetailPage(id)
        })
        .catch(() => {
          this.showErrorBarWithScrollPageTop(this.$t('msg.error.default'))
        })
        .finally(() => {
          this.isLoading = false
          this.isLoadingButton = false
        })
    },
  },
})
</script>
