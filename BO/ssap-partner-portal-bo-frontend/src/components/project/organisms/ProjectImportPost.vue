<template>
  <CommonContainer
    :title="title"
    :is-editing="isEditing"
    :is-register="isRegister"
    is-hide-button1
    hx="h1"
    @buttonAction1="buttonAction1"
    @buttonAction2="buttonAction2"
  >
    <Sheet class="pa-8 pb-0">
      <Title hx="h2" style-set="detail">
        <Icon color="btn_primary" size="24" class="mt-n1"
          >mdi-checkbox-marked-circle</Icon
        >
        {{ $t('project.pages.import.confirm.completed') }}
      </Title>
      <div
        class="o-project-list-table o-common-data-table is-scroll is-import o-import-table"
      >
        <v-data-table
          :headers="projectHeaders"
          :items="projects"
          :no-data-text="$t('common.label.noData')"
          :loading-text="$t('common.label.loading')"
        >
          <template #top="{ pagination }">
            <Paragraph style-set="confirm-text">
              {{ $t('project.pages.import.confirm.processed') }}
              <strong
                ><strong>{{ pagination.itemsLength }} </strong
                >{{ $t('project.pages.import.confirm.number') }}</strong
              >
            </Paragraph>
          </template>
          <template #[`footer.page-text`]></template>

          <!-- (アンケート)アンケート記載時のプロジェクト名 -->
          <template #[`header.name`]="{ header }">
            <OverflowTooltip :text="header.text" :max="16" />
          </template>
        </v-data-table>
      </div>
    </Sheet>
  </CommonContainer>
</template>

<script lang="ts">
import BaseComponent from '~/common/BaseComponent'
import CommonContainer from '~/components/common/organisms/CommonContainer.vue'
import OverflowTooltip from '~/components/common/molecules/OverflowTooltip.vue'

import {
  Sheet,
  Title,
  Button,
  FileInput,
  Paragraph,
  Icon,
} from '~/components/common/atoms/index'

export default BaseComponent.extend({
  name: 'ProjectImportSelectFile',
  components: {
    CommonContainer,
    Sheet,
    Title,
    Button,
    FileInput,
    Paragraph,
    OverflowTooltip,
    Icon,
  },
  props: {
    projects: {
      type: Array,
      required: true,
    },
  },
  data() {
    return {
      title: this.$t('project.pages.import.name'),
      isEditing: false,
      isRegister: true,
      projectHeaders: [
        {
          text: '商談ID',
          value: 'id',
          sortable: false,
        },
        {
          text: '取引先ID',
          value: 'customerId',
          sortable: false,
        },
        { text: '最終更新日時', value: 'salesforceUpdateAt', sortable: false },
        { text: '商談名', value: 'salesforceOpportunityId', sortable: false },
        {
          text: '(アンケート)アンケート記載時のプロジェクト名',
          value: 'name',
          sortable: false,
        },
        { text: '取引先名', value: 'customerName', sortable: false },
        { text: 'サービス区分', value: 'serviceTypeName', sortable: false },
        {
          text: 'PKG利用',
          value: 'gross.salesforceUsePackage',
          sortable: false,
        },
        { text: '新規・更新', value: 'createNew', sortable: false },
        {
          text: '前年度で契約し期をまたぐ商談',
          value: 'continued',
          sortable: false,
        },
        { text: 'PR経由', value: 'gross.salesforceViaPr', sortable: false },
        {
          text: '取引先責任者',
          value: 'salesforceMainCustomer.name',
          sortable: false,
        },
        {
          text: '取引先責任者メールアドレス',
          value: 'salesforceMainCustomer.email',
          sortable: false,
        },
        {
          text: '取引先責任者所属部署',
          value: 'salesforceMainCustomer.organizationName',
          sortable: false,
        },
        {
          text: '取引先責任者役職',
          value: 'salesforceMainCustomer.job',
          sortable: false,
        },
        { text: '商談所有者', value: 'mainCustomerUserName', sortable: false },
        { text: '完了予定日', value: 'contractDate', sortable: false },
        { text: 'フェーズ', value: 'phase', sortable: false },
        {
          text: 'カスタマーサクセス',
          value: 'customerSuccess',
          sortable: false,
        },
        { text: '支援開始日', value: 'supportDateFrom', sortable: false },
        { text: '支援終了日', value: 'supportDateTo', sortable: false },
        { text: '04粗利', value: 'profit.monthly[0]', sortable: false },
        { text: '05粗利', value: 'profit.monthly[1]', sortable: false },
        { text: '06粗利', value: 'profit.monthly[2]', sortable: false },
        { text: '07粗利', value: 'profit.monthly[3]', sortable: false },
        { text: '08粗利', value: 'profit.monthly[4]', sortable: false },
        { text: '09粗利', value: 'profit.monthly[5]', sortable: false },
        { text: '10粗利', value: 'profit.monthly[6]', sortable: false },
        { text: '11粗利', value: 'profit.monthly[7]', sortable: false },
        { text: '12粗利', value: 'profit.monthly[8]', sortable: false },
        { text: '01粗利', value: 'profit.monthly[9]', sortable: false },
        { text: '02粗利', value: 'profit.monthly[10]', sortable: false },
        { text: '03粗利', value: 'profit.monthly[11]', sortable: false },
        { text: '1Q粗利', value: 'profit.quarterly[0]', sortable: false },
        { text: '2Q粗利', value: 'profit.quarterly[1]', sortable: false },
        { text: '3Q粗利', value: 'profit.quarterly[2]', sortable: false },
        { text: '4Q粗利', value: 'profit.quarterly[3]', sortable: false },
        { text: '1H粗利', value: 'profit.half[0]', sortable: false },
        { text: '2H粗利', value: 'profit.half[1]', sortable: false },
        { text: 'FY粗利', value: 'profit.year', sortable: false },
        { text: '04売上', value: 'gross.monthly[0]', sortable: false },
        { text: '05売上', value: 'gross.monthly[1]', sortable: false },
        { text: '06売上', value: 'gross.monthly[2]', sortable: false },
        { text: '07売上', value: 'gross.monthly[3]', sortable: false },
        { text: '08売上', value: 'gross.monthly[4]', sortable: false },
        { text: '09売上', value: 'gross.monthly[5]', sortable: false },
        { text: '10売上', value: 'gross.monthly[6]', sortable: false },
        { text: '11売上', value: 'gross.monthly[7]', sortable: false },
        { text: '12売上', value: 'gross.monthly[8]', sortable: false },
        { text: '01売上', value: 'gross.monthly[9]', sortable: false },
        { text: '02売上', value: 'gross.monthly[10]', sortable: false },
        { text: '03売上', value: 'gross.monthly[11]', sortable: false },
        { text: '1Q売上', value: 'gross.quarterly[0]', sortable: false },
        { text: '2Q売上', value: 'gross.quarterly[1]', sortable: false },
        { text: '3Q売上', value: 'gross.quarterly[2]', sortable: false },
        { text: '4Q売上', value: 'gross.quarterly[3]', sortable: false },
        { text: '1H売上', value: 'gross.half[0]', sortable: false },
        { text: '2H売上', value: 'gross.half[1]', sortable: false },
        { text: 'FY売上', value: 'gross.year', sortable: false },
        {
          text: 'プロデューサー(主担当1名)',
          value: 'mainSupporterUserName',
          sortable: false,
        },
        {
          text: 'アクセラレーター(副担当複数名)',
          value: 'supporterUsers',
          sortable: false,
        },
        { text: '延べ契約時間', value: 'totalContractTime', sortable: false },
      ],
    }
  },
  methods: {
    upload() {
      // ユニーク名称でS3にアップロードし、そのファイルパスを入手
      // 確認画面に遷移、パスをパラメータとして送ること
    },
    buttonAction1() {},
    buttonAction2() {
      this.$router.push('/project/list')
    },
  },
  mounted() {
    this.setScrollHint(this.$t('common.table.scrollable') as string)
  },
})
</script>

<style lang="scss">
.o-import-table {
  .v-data-table {
    background-color: $c-white;
    .v-data-table__wrapper {
      table {
        tbody {
          tr {
            &:hover,
            &:focus {
              background: $c-white !important;
            }

            &:nth-child(even) {
              &:hover,
              &:focus {
                background-color: $c-black-table-border !important;
              }
            }
            td {
              font-size: 0.75rem;
              padding: 16px;
            }
          }
        }
      }
    }
  }
}
</style>
