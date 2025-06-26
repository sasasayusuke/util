<template>
  <CommonDataTable
    :headers="adminHeaders"
    :items="formattedAdmin"
    :total="total"
    :limit="limit"
    :is-loading="isLoading"
    :short-text="true"
    :shows-pagination="false"
    disable-pagination
    v-on="$listeners"
  >
  </CommonDataTable>
</template>

<script lang="ts">
import CommonDataTable from '../../common/organisms/CommonDataTable.vue'
import type { IDataTableHeader } from '../../common/organisms/CommonDataTable.vue'
import BaseComponent from '~/common/BaseComponent'
import type { PropType } from '~/common/BaseComponent'
import { AdminListItem } from '~/models/Admin'
import { formatDateStr } from '~/utils/common-functions'

class FormattedAdmin extends AdminListItem {
  organizations: string = ''
}

export default BaseComponent.extend({
  name: 'AdminListTable',
  components: {
    CommonDataTable,
  },
  props: {
    /**
     * Back Officeにログイン可能な管理ユーザーの初期値
     */
    admins: {
      type: Array as PropType<AdminListItem[]>,
      required: true,
    },
    /**
     * 全ての管理者ユーザーの件数
     */
    total: {
      type: Number,
      required: true,
    },
    /**
     * 一ページに表示される管理者ユーザーの件数
     */
    limit: {
      type: Number,
      required: true,
    },
    isLoading: {
      type: Boolean,
      required: true,
    },
  },
  data(): { adminHeaders: IDataTableHeader[] } {
    return {
      adminHeaders: [
        {
          text: this.$t('admin.row.name.name'),
          align: 'start',
          value: 'name',
          maxLength: 12,
          sortable: false,
          link: {
            prefix: '/admin/',
            value: 'id',
          },
          width: '192px',
        },
        {
          text: this.$t('admin.row.email.name'),
          value: 'email',
          maxLength: 18,
          sortable: false,
          width: '160px',
        },
        {
          text: this.$t('admin.row.company.name'),
          value: 'company',
          maxLength: 16,
          sortable: false,
          width: '232px',
        },
        {
          text: this.$t('admin.row.organizationName.name'),
          value: 'organizations',
          maxLength: 7,
          sortable: false,
          width: '132px',
        },
        {
          text: this.$t('admin.row.job.name'),
          value: 'job',
          maxLength: 7,
          sortable: false,
          width: '132px',
        },
        {
          text: this.$t('admin.row.roles.name'),
          value: 'roles',
          maxLength: 14,
          sortable: false,
          width: '213px',
        },
        {
          text: this.$t('admin.row.lastLoginAt.name'),
          value: 'lastLoginAt',
          width: '137px',
        },
      ],
    }
  },
  methods: {},

  computed: {
    // 表の表示形式を揃えるための変換は、ListTable organismで行う
    // (上流で元データを加工しないように)
    /**
     * テーブルで表示する用に、管理者ユーザー一覧のデータを変更する
     */
    formattedAdmin(): AdminListItem[] {
      const rolesList = this.$t('admin.row.roles.list')
      return this.admins.map((admin) => {
        const rtn = Object.assign(new FormattedAdmin(), admin)
        if (admin.lastLoginAt) {
          // 登録後未ログイン管理ユーザーは最終ログイン日時を空欄にする
          if (admin.lastLoginAt === '2000-01-01T00:00:00.000+09:00') {
            rtn.lastLoginAt = ''
          } else {
            rtn.lastLoginAt = formatDateStr(admin.lastLoginAt, 'Y/MM/dd HH:mm')
          }
        }
        let rolesString = ''
        admin.roles.forEach((elm: any) => {
          // 下記の二つの処理を一度に行うと、undefinedエラーが発生するためこのように分けて実装
          // @ts-ignore
          const convertedArray = rolesList.filter(
            (item: any) => item.value === elm
          )
          const roleConvertJa = convertedArray.map((el: any) => el.name)
          rolesString = rolesString
            ? `${rolesString}/${roleConvertJa}`
            : `${roleConvertJa}`
          // @ts-ignore
          rtn.roles = rolesString
        })
        rtn.name =
          admin.disabled === true
            ? this.$t('admin.row.name.invalid') + admin.name
            : admin.name

        // 登録時のロールが"支援者責任者"の場合は管理者一覧に複数部署表示
        if (admin.roles.includes('supporter_mgr')) {
          rtn.organizations = admin.supporterOrganizations
            .map((elm) => {
              return elm.name
            })
            .join(', ')
        } else {
          rtn.organizations = admin.organizationName
        }
        return rtn
      })
    },
  },
})
</script>

<style lang="scss">
.o-admin-list-table {
  margin-top: 60px;
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
            &:hover {
              background: $c-primary-8 !important;
              a {
                color: $c-primary-over !important;
              }
            }
            td {
              font-size: 0.75rem;
            }
          }
        }
      }
    }
    .v-data-footer {
      &:first-child {
        margin-bottom: 16px;
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
    margin: 0 0 16px;
  }
  .v-data-footer__icons-before,
  .v-data-footer__icons-after {
    .v-btn {
      display: none;
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
.o-admin-list-table__number {
  margin: 0 !important;
}
.o-admin-list-table__header {
  display: flex;
  justify-content: space-between;
  align-items: flex-end;
  margin-bottom: 16px;
}
.o-admin-list-table__footer {
  display: flex;
  justify-content: flex-end;
  margin-top: 16px;
}
.o-admin-table__link {
  color: $c-primary-dark !important;
  font-weight: bold;
}
</style>
