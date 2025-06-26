<template>
  <main class="detail-tabs-container">
    <div class="tabs-group">
      <!-- 基本情報 -->
      <v-btn
        :color="selected === 'fundamental' ? 'btn_primary' : 'btn_negative'"
        x-large
        depressed
        height="40"
        width="200"
        class="tabs-group__btn"
        :class="
          selected === 'fundamental'
            ? 'white--text font-weight-bold'
            : 'btn_primary--text font-weight-bold inactive__btn'
        "
        @click="$emit('click', 'fundamental')"
      >
        {{ $t('master-karte.pages.detail.container.tabs.fundamental') }}
      </v-btn>

      <!-- 案件結果・実績情報 -->
      <v-btn
        v-show="isCurrentProgram"
        :color="selected === 'result' ? 'btn_primary' : 'btn_negative'"
        x-large
        depressed
        height="40"
        width="200"
        class="tabs-group__btn"
        :class="
          selected === 'result'
            ? 'white--text font-weight-bold'
            : 'btn_primary--text font-weight-bold inactive__btn'
        "
        @click="$emit('click', 'result')"
      >
        {{ $t('master-karte.pages.detail.container.tabs.result') }}
      </v-btn>

      <!-- 企業・部署情報 -->
      <v-btn
        v-show="isCurrentProgram"
        :color="selected === 'client' ? 'btn_primary' : 'btn_negative'"
        x-large
        depressed
        height="40"
        width="200"
        class="tabs-group__btn"
        :class="
          selected === 'client'
            ? 'white--text font-weight-bold'
            : 'btn_primary--text font-weight-bold inactive__btn'
        "
        @click="$emit('click', 'client')"
      >
        {{ $t('master-karte.pages.detail.container.tabs.companyDepartment') }}
      </v-btn>

      <!-- その他チェック事項 -->
      <v-btn
        :color="selected === 'others' ? 'btn_primary' : 'btn_negative'"
        x-large
        depressed
        height="40"
        width="200"
        class="tabs-group__btn"
        :class="
          selected === 'others'
            ? 'white--text font-weight-bold'
            : 'btn_primary--text font-weight-bold inactive__btn'
        "
        @click="$emit('click', 'others')"
      >
        {{ $t('master-karte.pages.detail.container.tabs.others') }}
      </v-btn>
    </div>
  </main>
</template>

<script lang="ts">
import BasePage from '~/common/BasePage'
import { Button } from '~/components/common/atoms'
import GroupTabs from '~/components/common/molecules/GroupTabs.vue'
import DetailContainer from '~/components/common/organisms/DetailContainer.vue'

export default BasePage.extend({
  name: 'MasterKarteListNpfProjectId',
  middleware: ['roleCheck'],
  components: { DetailContainer, GroupTabs, Button },
  head() {
    return {
      title: this.$t('master-karte.pages.detail.name') as string,
    }
  },
  props: {
    selected: {
      type: String,
      default: 'fundamental',
    },
    isCurrentProgram: {
      type: Boolean,
      default: true,
    },
  },
})
</script>

<style lang="scss" scoped>
.detail-tabs-container {
  margin-top: 50px;
  display: flex;
  justify-content: center;
}
.tabs-group {
  padding: 5px;
  background-color: #f0f0f0 !important;
  .tabs-group__btn {
    font-size: 14px;
  }
  .inactive__btn {
    background-color: transparent !important;
  }
}
</style>
