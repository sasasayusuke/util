<template>
  <CommonDataTable
    :headers="masterHeaders"
    :items="formattedMaster"
    :total="total"
    :offset-page="offsetPage"
    :limit="limit"
    :is-loading="isLoading"
    v-on="$listeners"
  >
  </CommonDataTable>
</template>

<script lang="ts">
import CommonDataTable from '../../common/organisms/CommonDataTable.vue'
import type { IDataTableHeader } from '../../common/organisms/CommonDataTable.vue'
import BaseComponent from '~/common/BaseComponent'
import type { PropType } from '~/common/BaseComponent'
import { MasterListItem } from '~/models/Master'
import { formatDateStr } from '~/utils/common-functions'

export default BaseComponent.extend({
  name: 'MasterListTable',
  components: {
    CommonDataTable,
  },
  props: {
    /**
     * マスターメンテナンスの一覧
     */
    masters: {
      type: Array as PropType<MasterListItem[]>,
      required: true,
    },
    /**
     * データの合計
     */
    total: {
      type: Number,
      required: true,
    },
    /**
     * 開始ページ
     */
    offsetPage: {
      type: Number,
      required: true,
    },
    /**
     * 一ページに表示されるマスターメンテナンスの件数
     */
    limit: {
      type: Number,
      required: true,
    },
    /**
     * ローディング中か否か
     */
    isLoading: {
      type: Boolean,
      required: true,
    },
  },
  data(): { masterHeaders: IDataTableHeader[] } {
    return {
      /**
       * テーブルヘッダー
       */
      masterHeaders: [
        {
          text: this.$t('master.pages.index.header.id'),
          align: 'start',
          value: 'id',
          sortable: false,
          width: 160,
          link: {
            prefix: '',
            value: 'id',
          },
        },
        {
          text: this.$t('master.pages.index.header.dataType'),
          value: 'formattedDataType',
          sortable: false,
          width: 200,
          maxLength: 14,
        },
        {
          text: this.$t('master.pages.index.header.name'),
          value: 'masterName',
          sortable: false,
          width: 398,
          maxLength: 30,
        },
        {
          text: this.$t('master.pages.index.header.value'),
          value: 'value',
          sortable: false,
          width: 96,
          maxLength: 5,
        },
        {
          text: this.$t('master.pages.index.header.use'),
          value: 'use',
          sortable: false,
          width: 92,
        },
        {
          text: this.$t('master.pages.index.header.createAt'),
          value: 'createAt',
          sortable: false,
          width: 127,
        },
        {
          text: this.$t('master.pages.index.header.updateAt'),
          value: 'updateAt',
          sortable: false,
          width: 127,
        },
      ],
    }
  },
  computed: {
    /**
     * テーブルで表示する用に、マスターメンテナンス一覧のデータを加工する
     */
    formattedMaster(): Object[] {
      const rtn = this.masters.map((elm: any) => {
        //詳細画面リンク取得
        this.masterHeaders[0].link = {
          prefix: `/master/${elm.dataType}/`,
          value: 'id',
        }

        const copiedElm = Object.assign({}, elm, {
          formattedDataType: '',
          masterName: '',
        })

        //名称
        copiedElm.masterName = copiedElm.name
        delete copiedElm.name

        // 利用フラグ
        if (copiedElm.use === true) {
          copiedElm.use = this.$t('master.row.use.items.enabled') as string
        } else {
          copiedElm.use = this.$t('master.row.use.items.disabled') as string
        }

        //種別
        copiedElm.formattedDataType = this.$t(
          `master.row.dataType.items.${copiedElm.dataType}`
        ) as string

        copiedElm.createAt = formatDateStr(
          copiedElm.createAt,
          this.$t('common.format.date_ymd_hm').toString()
        )
        copiedElm.updateAt = formatDateStr(
          copiedElm.updateAt,
          this.$t('common.format.date_ymd_hm').toString()
        )

        return copiedElm
      })
      return rtn
    },
  },
})
</script>

<style lang="scss">
.o-master-list-table {
  .v-data-table {
    background-color: $c-black-page-bg;
    tr {
      background-color: $c-white;
      td {
        border-bottom: 0 !important;
      }
      &:nth-child(even) {
        background-color: $c-black-table-border;
      }
    }
    .v-data-table-header {
      th,
      td {
        background-color: $c-black-80;
        color: $c-white !important;
      }
      .v-icon {
        color: $c-gray-line-dark !important;
        margin-left: 5px !important;
        opacity: 1;
      }
      th {
        &.active {
          .v-icon {
            color: $c-primary !important;
          }
        }
      }
    }
    .v-data-table__wrapper {
      border-radius: 4px;
      box-shadow: 0px 3px 3px -2px rgb(0 0 0 / 20%),
        0px 3px 4px 0px rgb(0 0 0 / 14%), 0px 1px 8px 0px rgb(0 0 0 / 12%) !important;
      table {
        tbody {
          tr {
            transition-property: background-color;
            transition-duration: 0.2s;
            &:hover,
            &:focus {
              background: $c-primary-8 !important;
              a {
                color: $c-primary-over !important;
              }
            }
            td {
              font-size: 0.75rem;
              padding: 16px;
              white-space: wrap;
            }
          }
        }
      }
    }
    .v-data-footer {
      &:first-child {
        margin-bottom: 16px;
        align-items: flex-end;
      }
      &:last-child {
        margin-top: 16px;
      }
    }
  }
  .v-data-footer {
    width: 100%;
    padding: 0 !important;
    justify-content: flex-end;
    border: 0 !important;
    position: relative;
  }
  .v-data-footer__select {
    display: none !important;
  }
  .v-data-table__wrapper {
    .v-data-footer {
      width: 100%;
    }
  }
  .v-data-footer__pagination {
    font-size: 0.875rem;
    position: absolute;
    left: 0;
    margin: 0;
  }
  .v-data-footer__icons-before,
  .v-data-footer__icons-after {
    .v-btn {
      width: 42px;
      height: 42px;
      box-shadow: 0 3px 6px rgba(#000000, 0.16);
      border-radius: 4px;
      transition-property: background-color;
      transition-duration: 0.2s;
      .v-ripple__container,
      &::before {
        display: none !important;
      }
      &:hover,
      &:focus {
        background-color: $c-primary-8;
      }
    }
    .v-icon {
      width: 30px;
      height: 30px;
      font-size: 30px;
      color: $c-primary-dark !important;
    }
    .v-btn--disabled {
      .v-icon {
        color: #8f8f8f !important;
      }
    }
  }
}
.o-master-list-table__number {
  margin: 0 !important;
}
.o-master-list-table__header {
  display: flex;
  justify-content: space-between;
  align-items: flex-end;
  margin-bottom: 16px;
}
.o-master-list-table__footer {
  display: flex;
  justify-content: flex-end;
  margin-top: 16px;
}
.o-master-table__link {
  color: $c-primary-dark !important;
  font-weight: bold;
}
</style>
