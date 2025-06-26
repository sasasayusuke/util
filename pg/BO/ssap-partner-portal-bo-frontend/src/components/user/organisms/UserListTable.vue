<template>
  <CommonDataTable
    :headers="userHeaders"
    :items="formattedUser"
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
import { UserListItem } from '~/models/User'
import BaseComponent from '~/common/BaseComponent'
import { formatDateStr } from '~/utils/common-functions'
import type { PropType } from '~/common/BaseComponent'

export default BaseComponent.extend({
  name: 'UserListTable',
  components: {
    CommonDataTable,
  },
  props: {
    /** GetUsers APIレスポンスのuserプロパティ */
    users: {
      type: Array as PropType<UserListItem[]>,
      required: true,
    },
    /** 一般ユーザーの総数 */
    total: {
      type: Number,
      required: true,
    },
    /** ページ番号 */
    offsetPage: {
      type: Number,
      required: true,
    },
    /** ページ毎の件数 */
    limit: {
      type: Number,
      required: true,
    },
    isLoading: {
      type: Boolean,
      required: true,
    },
  },
  data(): {
    userHeaders: IDataTableHeader[]
  } {
    return {
      userHeaders: [
        {
          text: this.$t('user.row.name.name'),
          align: 'start',
          value: 'name',
          sortable: false,
          width: '192px',
          maxLength: 13,
          link: {
            prefix: '/user/',
            value: 'id',
          },
        },
        {
          text: this.$t('user.row.email.name'),
          value: 'email',
          sortable: false,
          maxLength: 18,
          width: '165px',
        },
        {
          text: this.$t('user.row.company.name'),
          value: 'company',
          sortable: false,
          maxLength: 16,
          width: '245px',
        },
        {
          text: this.$t('user.row.organizationName.name'),
          value: 'organizationName',
          sortable: false,
          maxLength: 7,
          width: '132px',
        },
        {
          text: this.$t('user.row.supporterOrganizations.name'),
          value: 'supporterOrganizations',
          sortable: false,
          maxLength: 7,
          width: '132px',
        },
        {
          text: this.$t('user.row.role.name'),
          value: 'role',
          sortable: false,
          maxLength: 14,
          width: '150px',
        },
        {
          text: this.$t('user.row.lastLoginAt.name'),
          value: 'lastLoginAt',
          width: '137px',
        },
      ],
    }
  },
  methods: {
    /** 英語のロールをja.jsonを利用して日本語に変換 */
    rolesToString(roleName: string) {
      const rolesLabels = this.$t('user.row.role.radio.labels') as string
      const rolesList = this.$t('user.row.role.radio.values') as string
      for (let i = 0; i < rolesList.length; i++) {
        if (rolesList[i] === roleName) {
          return rolesLabels[i]
        }
      }
    },
  },
  computed: {
    // 表の表示形式を揃えるための変換は、ListTable organismで行う
    // (上流で元データを加工しないように)
    /** GetUsers APIで取得した一般ユーザー一覧データを加工 */
    formattedUser(): UserListItem[] {
      const rolesLabels = this.$t('user.row.role.radio.labels') as string
      const rolesList = this.$t('user.row.role.radio.values') as string

      // 日時のフォーマット
      return this.users.map((user) => {
        if (user.lastLoginAt) {
          // 登録後未ログインユーザーは最終ログイン日時を空欄にする
          if (user.lastLoginAt === '2000-01-01T00:00:00.000+09:00') {
            user.lastLoginAt = ''
          } else {
            user.lastLoginAt = formatDateStr(user.lastLoginAt, 'Y/MM/dd HH:mm')
          }
        }

        // 所属会社項目の制御
        user.company =
          user.role === 'customer' ? user.customerName : user.company

        // 部署項目の制御
        user.organizationName =
          user.role === 'supporter' ||
          user.role === 'supporter_mgr' ||
          user.role === 'business_mgr'
            ? ''
            : user.organizationName

        // 支援者組織項目の制御
        const supporterOrganizations: any = []

        user.role === 'supporter' ||
        user.role === 'supporter_mgr' ||
        user.role === 'business_mgr'
          ? user.supporterOrganizations.forEach(
              (supporterOrganization: any) => {
                supporterOrganizations.push(supporterOrganization.name)
              }
            )
          : (user.supporterOrganizations = [])
        user.supporterOrganizations = supporterOrganizations.join(',')

        //システムロール名前を和名に
        for (let i = 0; i < rolesList.length; i++) {
          if (rolesList[i] === user.role) {
            user.role = rolesLabels[i]
          }
        }
        // HACK: 2ページ目以降「(無効)」が連続して表示されるため、暫定対応として前方一致確認処理を追加
        // TODO: 原因の特定し、修正を実施する
        // ユーザー名のフォーマット
        if (
          user.disabled &&
          user.name.indexOf(this.$t('user.row.name.invalid') as string) !== 0
        ) {
          // 無効ユーザー且つユーザー名に（無効）が付いていない場合
          user.name = this.$t('user.row.name.invalid') + user.name
        }

        return user
      })
    },
  },
})
</script>
