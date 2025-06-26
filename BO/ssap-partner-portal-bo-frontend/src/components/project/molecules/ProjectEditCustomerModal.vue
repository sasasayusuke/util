<template>
  <ModalEdit
    v-if="!isConfirmed"
    :title="title"
    @click-bg="$emit('click:negative')"
  >
    <v-form
      v-model="isValid"
      @input="$listeners['input']"
      @submit.prevent="onSubmit"
    >
      <!-- システムロール -->
      <CommonDetailRow
        :label="$t('project.pages.edit.customer.row.role.label')"
        :is-editing="false"
        :value="$t('project.pages.edit.customer.row.role.value')"
        cols="5"
      />
      <!-- ユーザーネーム -->
      <CommonDetailRow
        :label="$t('project.pages.edit.customer.row.name.label')"
        :is-editing="isEditing"
        required
        :value="localUser.name"
        cols="5"
      >
        <Sheet width="300">
          <TextField
            v-model="localUser.name"
            style-set="outlined"
            required
            :max-length="120"
          />
        </Sheet>
      </CommonDetailRow>
      <!-- メールアドレス -->
      <CommonDetailRow
        :label="$t('project.pages.edit.customer.row.email.label')"
        :is-editing="false"
        required
        :value="userEmail"
        cols="5"
      />
      <!-- 取引先 -->
      <CommonDetailRow
        :label="$t('project.pages.edit.customer.row.customer.label')"
        :is-editing="false"
        :value="localUser.company"
        cols="5"
      />
      <!-- 部署 -->
      <CommonDetailRow
        :label="$t('project.pages.edit.customer.row.organization.label')"
        :is-editing="isEditing"
        :value="localUser.organizationName"
        cols="5"
      >
        <Sheet width="300">
          <TextField
            v-model="localUser.organizationName"
            style-set="outlined"
            :placeholder="
              $t('project.pages.edit.customer.row.organization.placeholder')
            "
            :max-length="80"
          />
        </Sheet>
      </CommonDetailRow>
      <!-- 役職 -->
      <CommonDetailRow
        :label="$t('project.pages.edit.customer.row.job.label')"
        :is-editing="isEditing"
        :value="localUser.job"
        cols="5"
      >
        <Sheet width="300">
          <TextField
            v-model="localUser.job"
            style-set="outlined"
            :placeholder="$t('project.pages.edit.customer.row.job.placeholder')"
            :max-length="128"
          />
        </Sheet>
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
        :disabled="isValid !== true"
        @click="save"
      >
        {{ $t('common.button.save2') }}
      </Button>
    </template>
  </ModalEdit>
</template>

<script lang="ts">
import ModalEdit from '~/components/common/molecules/ModalEdit.vue'
import Modal from '~/components/common/molecules/Modal.vue'
import {
  Button,
  Sheet,
  Paragraph,
  AutoComplete,
  TextField,
  ErrorText,
  Required,
} from '~/components/common/atoms/index'
import CommonDetailRow from '~/components/common/molecules/CommonDetailRow.vue'
import BaseComponent from '~/common/BaseComponent'

import {
  GetUserById,
  UpdateUserById,
  UpdateUserByIdRequest as LocalUser,
} from '~/models/User'
export { LocalUser }

export default BaseComponent.extend({
  components: {
    ModalEdit,
    Modal,
    Sheet,
    Button,
    Paragraph,
    AutoComplete,
    TextField,
    ErrorText,
    Required,
    CommonDetailRow,
  },
  props: {
    /** 表示タイトル */
    title: {
      type: String,
      required: true,
    },
    /** モーダルのユーザー */
    user: {
      type: [Object, Array],
      required: true,
    },
    /** 取引先 */
    company: {
      type: Object,
    },
    /** 編集対象判定文字列 */
    eventType: {
      type: String,
    },
    /** 選択中のお客様メンバー配列インデックス番号 */
    selected: {
      type: Number,
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
    userEmail: string
    customerName: string
    userId: string
    isConfirmed: boolean
    isAssigned: boolean
    isValid: boolean
  } {
    return {
      selectedUserInfo: {},
      localUser: new LocalUser(),
      userEmail: '',
      customerName: '',
      userId: '',
      isConfirmed: false,
      isAssigned: false,
      isValid: false,
    }
  },
  mounted() {
    this.displayLoading([this.setUser()], true)
  },
  methods: {
    /**
     * お客様メンバーID取得
     * @returns お客様メンバーID文字列
     */
    getUserId() {
      if (this.eventType === 'member') {
        return this.user[this.selected].id
      } else {
        return this.user.id
      }
    },
    /** お客様メンバーの情報を取得 */
    async setUser() {
      this.userId = this.getUserId()
      //既存ユーザの選択時
      await GetUserById(this.userId).then((res) => {
        this.selectedUserInfo = res.data
      })
      this.localUser.name = this.selectedUserInfo.name
      this.userEmail = this.selectedUserInfo.email
      this.localUser.company = this.selectedUserInfo.customerName
      this.localUser.job = this.selectedUserInfo.job
      this.localUser.organizationName = this.selectedUserInfo.organizationName

      this.isAssigned = true
    },
    /** 編集をキャンセル */
    cancel() {
      this.isConfirmed = false
    },
    /** お客様メンバー保存 */
    async save() {
      this.clearErrorBar()
      const customerInfo = { id: '', name: '' }
      // ユーザー情報を更新
      await UpdateUserById(
        this.userId,
        this.selectedUserInfo.version,
        this.localUser
      )
        .then(() => {
          customerInfo.name = this.localUser.name
          customerInfo.id = this.userId
          this.$emit('saveCustomerEdit', customerInfo)
        })
        .catch(() => {
          this.showErrorBarWithScrollPageTop(this.$t('msg.error.default'))
          this.$emit('click:negative')
        })
    },
    /** Submit時のデフォルトのフォーム動作をキャンセル */
    onSubmit() {
      return false
    },
  },
})
</script>
