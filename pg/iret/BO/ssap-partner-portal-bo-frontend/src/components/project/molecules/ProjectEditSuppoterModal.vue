<template>
  <ModalEdit :title="title">
    <v-form
      v-model="isValid"
      @input="$listeners['input']"
      @submit.prevent="onSubmit"
    >
      <CommonDetailRow
        :label="$t('project.pages.edit.supporter.row.role.label')"
        :is-editing="false"
        :value="localUser.role"
        cols="5"
      >
      </CommonDetailRow>
      <CommonDetailRow
        :label="$t('project.pages.edit.supporter.row.name.label')"
        :is-editing="false"
        :value="localUser.name"
        cols="5"
      >
      </CommonDetailRow>
      <!-- メールアドレス -->
      <CommonDetailRow
        :label="$t('project.pages.edit.supporter.row.email.label')"
        :is-editing="isEditing"
        :value="localUser.email"
        cols="5"
        required
      >
        <template v-if="isEditing">
          <Sheet width="300">
            <AutoComplete
              v-model="localUser.email"
              required
              :items="supporterOrMgr"
              item-text="label"
              :max-length="256"
              item-value="value"
              style-set="outlined"
              :placeholder="$t('common.placeholder.autoComplete')"
              :additional-rules="[duplicationUserCheck]"
              @change="checkExistUser"
            />
          </Sheet>
        </template>
      </CommonDetailRow>
      <CommonDetailRow
        :label="$t('project.pages.edit.supporter.row.company.label')"
        :is-editing="false"
        :value="localUser.company"
        cols="5"
      >
      </CommonDetailRow>
      <CommonDetailRow
        :label="$t('project.pages.edit.supporter.row.organization.label')"
        :is-editing="false"
        :value="localUser.organizationName"
        cols="5"
      >
      </CommonDetailRow>
    </v-form>
    <template #foot>
      <Button
        outlined
        style-set="large-tertiary"
        width="160"
        @click="$emit('click:negative')"
      >
        {{ $t('common.button.cancel') }}
      </Button>
      <Button
        class="ml-2"
        style-set="large-primary"
        width="160"
        :disabled="isValid !== true || disabled === true"
        @click="save"
      >
        {{ $t('common.button.save2') }}
      </Button>
    </template>
  </ModalEdit>
</template>

<script lang="ts">
import ModalEdit from '~/components/common/molecules/ModalEdit.vue'
import {
  Button,
  Sheet,
  Paragraph,
  TextField,
  ErrorText,
  AutoComplete,
} from '~/components/common/atoms/index'
import CommonDetailRow from '~/components/common/molecules/CommonDetailRow.vue'
import BaseComponent from '~/common/BaseComponent'

import {
  SuggestUsersRequest,
  SuggestUsers,
  SuggestUsersResponse,
  UpdateUserByIdResponse as LocalUser,
  GetUsers,
  GetUsersRequest,
  GetUserById,
} from '~/models/User'
export { LocalUser }

export default BaseComponent.extend({
  components: {
    ModalEdit,
    Sheet,
    Button,
    Paragraph,
    TextField,
    CommonDetailRow,
    ErrorText,
    AutoComplete,
  },
  props: {
    /** 表示タイトル */
    title: {
      type: String,
      required: true,
    },
    /** アサインされているユーザー */
    assignedUsers: {
      type: [Object, Array],
      required: true,
    },
    /** ログインユーザーid */
    selectedUserId: {
      type: String,
    },
    /** 編集対象判定文字列 */
    eventType: {
      type: String,
    },
    /** 編集中か */
    isEditing: {
      type: Boolean,
      default: false,
    },
  },
  data(): {
    selectedUserInfo: {
      [key: string]: any
    }
    localUser: LocalUser
    userId: string
    disabled: boolean
    alertSupporter: boolean
    emailErrorMessage: string
    isValid: boolean
    suggestedUsers: SuggestUsersResponse
    suggestedUsersArray: string[]
  } {
    return {
      selectedUserInfo: {},
      localUser: new LocalUser(),
      userId: '',
      disabled: false,
      alertSupporter: false,
      emailErrorMessage: '',
      isValid: false,
      suggestedUsers: new SuggestUsersResponse(),
      suggestedUsersArray: [],
    }
  },
  mounted() {
    this.displayLoading([this.setUser(), this.suggestUsers()], true)
  },
  computed: {
    /**
     * メールアドレスのサジェストを画面上に表示
     * @returns メールアドレスのサジェスト情報配列
     */
    supporterOrMgr() {
      const supporterOrMgrList: object[] = []
      const supporterOrMgrInfo = { label: '', value: '' }

      this.suggestedUsers.forEach((suggestedUser: any) => {
        supporterOrMgrInfo.label = suggestedUser.email
        supporterOrMgrInfo.value = suggestedUser.id

        const newElm = Object.assign({}, supporterOrMgrInfo)
        supporterOrMgrList.push(newElm)
      })

      return supporterOrMgrList
    },
  },
  methods: {
    /** 支援者ユーザの情報を取得 */
    async setUser() {
      if (this.selectedUserId) {
        await GetUserById(this.selectedUserId).then((res) => {
          this.selectedUserInfo = res.data
        })
        this.localUser.role = this.$t(
          'common.role.' + this.selectedUserInfo.role
        ) as string
        this.localUser.id = this.selectedUserInfo.id
        this.localUser.name = this.selectedUserInfo.name
        this.localUser.customerName = this.selectedUserInfo.customerName
        if (this.selectedUserInfo.role.match(/supporter/)) {
          this.localUser.organizationName =
            this.formattedSupporterOrganizations(
              this.selectedUserInfo.supporterOrganizations
            )
        } else {
          this.localUser.organizationName =
            this.selectedUserInfo.organizationName
        }
        this.localUser.email = this.selectedUserInfo.id
        this.localUser.company = this.selectedUserInfo.company
      }
    },
    /**
     * ユーザーのサジェスト候補を取得
     * @param params ユーザーのサジェスト情報取得リクエストオブジェクト
     */
    async suggestUsers(
      params: SuggestUsersRequest = new SuggestUsersRequest()
    ) {
      // ロールを限定して、サジェスト候補をDBから取得
      params.role = 'supporter_or_mgr'
      params.disabled = false

      await SuggestUsers(params).then((res) => {
        this.suggestedUsers = res.data
      })
    },
    /**
     * 選択中のユーザーがこれまでに選択したアクセラレーターかプロデューサーと重複していないかチェック
     * @param selectedId 選択中のユーザーID文字列
     * @returns 警告文字列または重複チェック判定真偽値
     */
    duplicationUserCheck(selectedId: string): string | boolean {
      const ids: string[] = []
      this.assignedUsers.forEach((user: any) => {
        if (user.id) {
          ids.push(user.id)
        }
      })
      // 入力値が既に同画面上で登録されている
      if (ids.includes(selectedId) && this.selectedUserInfo.id !== selectedId) {
        return this.$t(
          'project.pages.edit.supporter.errorMessages.registeredSupporter'
        ) as string
      }
      return true
    },
    /**
     * IDを利用し、選択中のユーザー情報のメールアドレスからユーザの存在チェック
     * @param id ユーザーID文字列
     */
    async checkExistUser(id: string) {
      let email = ''

      // idを元にemailを取得
      this.suggestedUsers.forEach((suggestedUser) => {
        if (suggestedUser.id === id) {
          email = suggestedUser.email
        }
      })

      // Eメールの値が空なら問い合わせをしない
      if (email) {
        // リクエストを作成
        const request = new GetUsersRequest()
        request.email = email
        await GetUsers(request).then((res) => {
          const supportersInfo = res.data.users
          for (const i in supportersInfo) {
            const supporterInfo = supportersInfo[i]
            if (request.email !== supporterInfo.email) {
              continue
            }
            this.localUser.id = supporterInfo.id
            this.localUser.name = supporterInfo.name
            this.localUser.customerName = supporterInfo.customerName
            this.localUser.company = supporterInfo.company
            if (supporterInfo.role.match(/supporter/)) {
              this.localUser.organizationName =
                this.formattedSupporterOrganizations(
                  supporterInfo.supporterOrganizations
                )
            } else {
              this.localUser.organizationName = supporterInfo.organizationName
            }
            this.localUser.role = String(
              this.$t('common.role.' + supporterInfo.role)
            )
            break
          }
        })
      }
    },
    /**
     * 部署名の取得
     * @returns 部署名文字列
     */
    formattedSupporterOrganizations(
      supporterOrganizations: Object[] | string[]
    ): string {
      let organizationNames = ''

      if (
        supporterOrganizations &&
        supporterOrganizations.length &&
        supporterOrganizations.length > 0
      ) {
        supporterOrganizations.forEach((supporterOrganization: any) => {
          organizationNames += supporterOrganization.name + ' / '
        })
      }
      return organizationNames.slice(0, -2)
    },
    /** 編集中の支援者情報を保存 */
    save() {
      const supporterInfo = { id: '', name: '' }
      supporterInfo.id = this.localUser.id
      supporterInfo.name = this.localUser.name
      this.$emit('saveSupporterEdit', supporterInfo)
    },
    /** 編集中の支援者情報を保存、入力に不備がある場合はSubmit時のデフォルトのフォーム動作をキャンセル */
    onSubmit() {
      if (this.isValid !== true || this.disabled === true) {
        return false
      } else {
        this.save()
      }
    },
  },
})
</script>
