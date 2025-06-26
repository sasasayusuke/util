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
              item-value="value"
              :max-length="256"
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
        :value="userCompany"
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
        {{ $t('common.button.save') }}
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
  SuggestUsers,
  SuggestUsersRequest,
  CreateUserResponse as LocalUser,
  GetUsers,
  GetUsersRequest,
  SuggestUsersResponse,
} from '~/models/User'

export { LocalUser }

export default BaseComponent.extend({
  name: 'ProjectCreateSupporterModal',
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
    title: {
      type: String,
      required: true,
    },
    /** アサインされたお客様一覧 */
    assignedUsers: {
      type: [Object, Array],
      required: true,
    },
    /** プロデューサー情報 */
    mainSupporterInfo: {
      type: Object,
    },
    eventType: {
      type: String,
      required: true,
    },
    /** 編集モードか */
    isEditing: {
      type: Boolean,
      default: false,
    },
  },
  data(): {
    localUser: LocalUser
    disabled: boolean
    emailErrorMessage: string
    userCompany: string
    selectedUserInfo: {
      [key: string]: any
    }
    isValid: boolean
    suggestedUsers: SuggestUsersResponse
    suggestedUsersArray: string[]
  } {
    return {
      localUser: new LocalUser(),
      disabled: false,
      emailErrorMessage: '',
      userCompany: '',
      selectedUserInfo: {},
      isValid: false,
      suggestedUsers: new SuggestUsersResponse(),
      suggestedUsersArray: [],
    }
  },
  mounted() {
    this.displayLoading([this.suggestUsers()], true)
  },
  computed: {
    /** メールアドレスのサジェストを画面上に表示 **/
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
    async suggestUsers(
      params: SuggestUsersRequest = new SuggestUsersRequest()
    ) {
      // ロールを限定して、サジェスト候補をDBから取得
      params.role = 'supporter_or_mgr'
      params.disable = false

      await SuggestUsers(params).then((res) => {
        this.suggestedUsers = res.data
      })
    },
    /** 選択中のユーザーがこれまでに選択したアクセラレーターかプロデューサーと重複していないかチェック **/
    duplicationUserCheck(selectedId: string) {
      const ids: string[] = []
      this.assignedUsers.forEach((user: any) => {
        if (user.id) {
          ids.push(user.id)
        }
      })

      // 入力値が既に同画面上で登録されている
      if (ids.includes(selectedId)) {
        return this.$t(
          'project.pages.edit.supporter.errorMessages.registeredSupporter'
        ) as string
      }
      return true
    },
    /** IDを利用し、選択中のユーザー情報をモーダル内項目に表示 */
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
            this.localUser.customerId = supporterInfo.customerId
            this.localUser.job = supporterInfo.job
            this.userCompany = supporterInfo.company
            this.localUser.role = String(
              this.$t('common.role.' + supporterInfo.role)
            )
            if (supporterInfo.role.match(/supporter || business_mgr/)) {
              // @ts-ignore
              this.localUser.organizationName =
                this.formattedSupporterOrganizations(
                  supporterInfo.supporterOrganizations
                )
            } else {
              // @ts-ignore
              this.localUser.organizationName = supporterInfo.organizationName
            }
            break
          }
        })
      }
    },
    /** 部署名の取得 */
    formattedSupporterOrganizations(
      supporterOrganizations: Object[] | string[]
    ): string {
      let organizationNames = ''

      supporterOrganizations.forEach((supporterOrganization: any) => {
        organizationNames += supporterOrganization.name + ' / '
      })
      return organizationNames.slice(0, -2)
    },
    /** 支援者追加モーダル保存 */
    save() {
      const supporterInfo = { id: '', name: '' }
      supporterInfo.id = this.localUser.id
      supporterInfo.name = this.localUser.name
      this.$emit('saveSupporterCreate', supporterInfo)
    },
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
