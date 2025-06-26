<template>
  <RootTemplate :class="'fill-height align-start'">
    <div class="home-customer">
      <!-- 参加中の案件 -->
      <HomeProjects :projects="projects" />
      <!-- 最近更新されたカルテ -->
      <HomeKarten :karten="karten" />
      <!-- 未入力のアンケート -->
      <HomeSurveys :surveys="surveys" />
      <!-- 関連ツール -->
      <HomeTools />
    </div>
  </RootTemplate>
</template>

<script lang="ts">
import HomeProjects from '../organisms/HomeProjects.vue'
import HomeKarten from '../organisms/HomeKarten.vue'
import HomeSurveys from '../organisms/HomeSurveys.vue'
import HomeTools from '../organisms/HomeTools.vue'
import BaseComponent from '~/common/BaseComponent'
import type { PropType } from '~/common/BaseComponent'
import RootTemplate from '~/components/common/bases/RootTemplate.vue'
import { KartenLatestListItem } from '~/models/Karte'
import { ProjectListItem } from '~/models/Project'
import { SurveyByMineItem } from '~/models/Survey'

export default BaseComponent.extend({
  components: {
    RootTemplate,
    HomeProjects,
    HomeKarten,
    HomeSurveys,
    HomeTools,
  },
  props: {
    /**
     * 案件情報
     */
    projects: {
      type: Array as PropType<ProjectListItem[]>,
    },
    /**
     * カルテ情報
     */
    karten: {
      type: Array as PropType<KartenLatestListItem[]>,
    },
    /**
     * アンケート情報
     */
    surveys: {
      type: Array as PropType<SurveyByMineItem[]>,
    },
  },
})
</script>

<style lang="scss" scoped>
.home-customer {
  width: 100%;
  padding: 32px 0 80px;
  > section {
    &:nth-of-type(n + 2) {
      margin-top: 36px;
    }
  }
}
</style>
