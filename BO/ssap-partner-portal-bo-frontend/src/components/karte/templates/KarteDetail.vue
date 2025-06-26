<template>
  <RootTemplate>
    <KarteDetailContainer
      :project="project"
      :karte="karte"
      :surveys="surveys"
      @click:negative="onClickNegative"
    />
  </RootTemplate>
</template>

<script lang="ts">
import { Chip } from '~/components/common/atoms'
import KarteDetailContainer from '~/components/karte/organisms/KarteDetailContainer.vue'
import CommonDetail from '~/components/common/templates/CommonDetailTemplate.vue'
import { PropType } from '~/common/BaseComponent'
import RootTemplate from '~/components/common/bases/RootTemplate.vue'
import { GetProjectByIdResponse } from '~/models/Project'
import { GetKarteByIdResponse } from '~/models/Karte'
import { SurveyListItem } from '~/models/Survey'

export interface isLoading {
  project: boolean
  karte: boolean
  surveys: boolean
}

export default CommonDetail.extend({
  name: 'KarteDetail',
  components: {
    RootTemplate,
    KarteDetailContainer,
    Chip,
  },
  props: {
    /**
     * GetProjectById APIのレスポンス
     **/
    project: {
      type: Object as PropType<GetProjectByIdResponse>,
      required: true,
    },
    /**
     * GetKarteById APIのレスポンス
     */
    karte: {
      type: Object as PropType<GetKarteByIdResponse>,
      required: true,
    },
    /**
     * GetSurveys APIのレスポンス
     */
    surveys: {
      type: Array as PropType<SurveyListItem[]>,
      required: true,
    },
  },
  methods: {
    /**
     * 案件別トップ(カルテ一覧)に遷移
     */
    onClickNegative() {
      this.$router.push('/karte/list/' + this.project.id)
    },
  },
})
</script>
