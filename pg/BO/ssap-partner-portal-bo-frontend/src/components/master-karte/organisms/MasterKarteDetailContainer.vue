<template>
  <DetailContainer
    :title="title"
    hx="h1"
    is-hide-button1
    is-hide-button2
    show-current-program
    :is-editing="false"
    :is-current-program="isCurrentProgram"
    :is-loading="isLoading"
    @clickCheckbox="clickCheckbox"
  >
    <!-- 最終更新者・更新日時 -->
    <v-row class="last-update">
      <LastUpdate
        :user="
          isCurrentProgram
            ? masterKarteProject.currentProgram.lastUpdateBy
            : masterKarteProject.nextProgram.lastUpdateBy
        "
        :date="
          isCurrentProgram
            ? masterKarteProject.currentProgram.lastUpdateDatetime
            : masterKarteProject.nextProgram.lastUpdateDatetime
        "
        :show-term="false"
      />
    </v-row>
    <!-- 5種のタブ -->
    <MasterKarteDetailTabs
      :selected="selected"
      :is-current-program="isCurrentProgram"
      @click="onClickTab"
    />
    <MasterKarteDetailContent
      :selected="selected"
      :is-current-program="isCurrentProgram"
      :master-karte-project="masterKarteProject"
      :show-current-program="showCurrentProgram"
    />
  </DetailContainer>
</template>

<script lang="ts">
import MasterKarteDetailContent from './MasterKarteDetailContent.vue'
import DetailContainer from '~/components/common/organisms/DetailContainer.vue'
import CommonDetailContainer from '~/components/common/organisms/CommonDetailContainer.vue'
import LastUpdate from '~/components/common/molecules/LastUpdate.vue'
import { GetMasterKarteByIdResponse } from '~/types/MasterKarte'
import type { PropType } from '~/common/BaseComponent'

export default CommonDetailContainer.extend({
  components: {
    DetailContainer,
    LastUpdate,
    MasterKarteDetailContent,
  },
  props: {
    isCurrentProgram: {
      type: Boolean,
      default: true,
    },
    masterKarteProject: {
      type: Object as PropType<GetMasterKarteByIdResponse>,
    },
    isLoading: {
      type: Boolean,
      default: false,
    },
  },
  data() {
    return {
      selected: 'fundamental',
      showCurrentProgram: true,
    }
  },
  methods: {
    onClickTab(tab: string) {
      this.selected = tab
    },
    clickCheckbox(showCurrentProgram: boolean) {
      this.showCurrentProgram = showCurrentProgram
    },
  },
  watch: {
    /**
     * isCurrentProgramが変更されたら、基本情報を選択状態にする
     * (個別カルテ一覧選択状態で次期支援をクリックした場合などに対応)
     */
    isCurrentProgram: {
      handler() {
        this.selected = 'fundamental'
      },
      immediate: true,
    },
  },
  computed: {
    title() {
      return this.isCurrentProgram
        ? this.$t('master-karte.pages.detail.titles.current')
        : this.$t('master-karte.pages.detail.titles.next')
    },
  },
})
</script>

<style>
.last-update {
  margin-right: 5rem;
  margin-top: 1rem;
}
</style>
