<template>
  <CommonDataTable
    :headers="masterKarteHeaders"
    :items="formattedMasterKarteList"
    :is-loading="isLoading"
    :is-scroll="false"
    :shows-pagination="false"
    :shows-header-text="false"
    v-on="$listeners"
  >
    <template #[`item.ppProjectId`]="{ item }">
      <div class="ppProjectId_Block">
        <div v-if="isKarteLinkVisible()">
          <div v-if="item.isAccessibleKarten">
            <nuxt-link
              class="o-common-data-table__link"
              :to="`/karte/list/${item.ppProjectId}`"
              tabindex="0"
            >
              <OverflowTooltip
                :text="
                  $t('master-karte.pages.list.table.items.ppProjectIdText')
                "
                :max="32"
            /></nuxt-link>
          </div>
          <div v-else>
            <OverflowTooltip
              :text="$t('master-karte.pages.list.table.items.ppProjectIdText')"
              :max="32"
            />
          </div>
        </div>
        <div v-if="isKarteLinkVisible()" class="vertical-line"></div>
        <div v-if="item.isAccessibleMasterKarten">
          <nuxt-link
            class="o-common-data-table__link"
            :to="`/master-karte/${item.npfProjectId}`"
            tabindex="1"
          >
            <OverflowTooltip
              :text="$t('master-karte.pages.list.table.items.npfProjectIdText')"
              :max="32"
          /></nuxt-link>
        </div>
        <div v-else>
          <OverflowTooltip
            :text="$t('master-karte.pages.list.table.items.npfProjectIdText')"
            :max="32"
          />
        </div>
      </div>
    </template>
  </CommonDataTable>
</template>
<script lang="ts">
import CommonDataTable from '~/components/common/organisms/CommonDataTable.vue'
import type { IDataTableHeader } from '~/components/common/organisms/CommonDataTable.vue'
import { MasterKarte } from '~/models/MasterKarte'
import BaseComponent from '~/common/BaseComponent'
import OverflowTooltip from '~/components/common/molecules/OverflowTooltip.vue'
import type { PropType } from '~/common/BaseComponent'
import { meStore } from '~/store'

export default BaseComponent.extend({
  name: 'MasterKarteListTable',
  components: {
    CommonDataTable,
    OverflowTooltip,
  },
  props: {
    /**
     * GetProjects APIのレスポンス
     */
    masterKarteList: {
      type: Array as PropType<MasterKarte[]>,
      required: true,
    },
    /**
     * ページがリロード中か否か
     */
    isLoading: {
      type: Boolean,
      required: true,
    },
  },
  data(): { masterKarteHeaders: IDataTableHeader[] } {
    return {
      /** ヘッダー項目名 */
      masterKarteHeaders: [
        {
          text: this.$t('master-karte.pages.list.table.header.serviceName'),
          value: 'serviceName',
          sortable: false,
          maxLength: 12,
        },
        {
          text: this.$t('master-karte.pages.list.table.header.project'),
          value: 'project',
          sortable: false,
          maxLength: 28,
        },
        {
          text: this.$t('master-karte.pages.list.table.header.client'),
          value: 'client',
          maxLength: 24,
          sortable: false,
        },
        {
          text: this.$t('master-karte.pages.list.table.header.supportDateFrom'),
          value: 'supportDateFrom',
          sortable: false,
        },
        {
          text: '',
          value: 'ppProjectId',
          sortable: false,
        },
      ],
    }
  },
  computed: {
    /**
     * 支援期間を「支援開始日〜支援終了日」という形に修正。最終更新日時のフォーマットを変更
     */
    formattedMasterKarteList() {
      return this.masterKarteList.map((masterKarte) => {
        const rtn = Object.assign(new MasterKarte(), masterKarte)
        rtn.supportDateFrom =
          masterKarte.supportDateFrom + ' ～ ' + masterKarte.supportDateTo
        return rtn
      })
    },
  },
  methods: {
    /**
     * アンケート事務局の場合、個別カルテリンクを表示しない
     */
    isKarteLinkVisible(): boolean {
      const rolesLength: number = meStore.roles.length
      const isManHourOps: boolean = meStore.roles.includes('man_hour_ops')
      const isSurveyOps: boolean = meStore.roles.includes('survey_ops')

      // 稼働率調査事務局のみ
      // 個別カルテを表示させない
      if (rolesLength === 1 && isManHourOps) {
        return false
      }

      // アンケート事務局のみ
      // 個別カルテを表示させない
      if (rolesLength === 1 && isSurveyOps) {
        return false
      }

      // 稼働率調査事務局 かつ アンケート事務局のみ
      // 個別カルテを表示させない
      if (rolesLength === 2 && isManHourOps && isSurveyOps) {
        return false
      }

      return true
    },
  },
})
</script>

<style lang="scss" scoped>
.ppProjectId_Block {
  height: 100%;
  display: flex;
  justify-content: flex-end;
  align-items: center;

  .vertical-line {
    width: 1px;
    height: 20px;
    background-color: #bfbfbf;
    margin: 0 15px;
  }
}
</style>
