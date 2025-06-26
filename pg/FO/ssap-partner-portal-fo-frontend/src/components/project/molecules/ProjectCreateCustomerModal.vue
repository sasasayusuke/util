<template>
  <ModalEdit
    v-if="!isConfirm"
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
      >
      </CommonDetailRow>
      <!-- ユーザネーム -->
      <CommonDetailRow
        :label="$t('project.pages.edit.customer.row.name.label')"
        :is-editing="isEditing"
        cols="5"
        required
      >
        <Sheet width="300">
          <AutoComplete
            v-model="localCustomer.name"
            style-set="outlined"
            :items="customer"
            :placeholder="$t('common.placeholder.autoComplete')"
            item-text="label"
            item-value="value"
            :warning="warning"
            :is-combobox="true"
            :disabled="disabled.name"
            :max-length="120"
            required
            @change="setUser"
          />
        </Sheet>
      </CommonDetailRow>
      <!-- メールアドレス -->
      <CommonDetailRow
        :label="$t('project.pages.edit.customer.row.email.label')"
        :is-editing="isEditing"
        cols="5"
        required
      >
        <Sheet width="300">
          <TextField
            v-if="!alertCustomer"
            v-model="localCustomer.email"
            role="textbox"
            style-set="outlined"
            :placeholder="
              $t('project.pages.edit.customer.row.email.placeholder')
            "
            :disabled="disabled.email"
            required
            :max-length="256"
            email
            @change="checkExistUser"
          />
          <template v-else>
            <TextField
              v-model="localCustomer.email"
              class="is-error"
              style-set="outlined"
              :placeholder="
                $t('project.pages.edit.customer.row.email.placeholder')
              "
              @change="checkExistUser"
            />
            <ErrorText class="mt-1" style="margin-left: 14px">
              {{ errorText }}
            </ErrorText>
          </template>
        </Sheet>
      </CommonDetailRow>
      <!-- 取引先 -->
      <CommonDetailRow
        :label="$t('project.pages.edit.customer.row.customer.label')"
        :is-editing="false"
        :value="customerName"
        cols="5"
      />
      <!-- 部署 -->
      <CommonDetailRow
        :label="$t('project.pages.edit.customer.row.organization.label')"
        :is-editing="isEditing"
        cols="5"
      >
        <Sheet width="300">
          <TextField
            v-model="localCustomer.organizationName"
            role="textbox"
            style-set="outlined"
            :placeholder="
              $t('project.pages.edit.customer.row.organization.placeholder')
            "
            :disabled="disabled.organizationName"
          />
        </Sheet>
      </CommonDetailRow>
      <!-- 役職 -->
      <CommonDetailRow
        :label="$t('project.pages.edit.customer.row.job.label')"
        :is-editing="isEditing"
        cols="5"
      >
        <Sheet width="300">
          <TextField
            v-model="localCustomer.job"
            role="textbox"
            style-set="outlined"
            :placeholder="$t('project.pages.edit.customer.row.job.placeholder')"
            :disabled="disabled.job"
            :max-length="128"
          />
        </Sheet>
      </CommonDetailRow>
    </v-form>
    <template #foot>
      <!-- 閉じるボタン -->
      <Button
        outlined
        style-set="large-tertiary"
        width="160"
        @click="$emit('click:negative')"
      >
        {{ $t('common.button.cancel') }}
      </Button>
      <!-- 確定ボタン -->
      <Button
        v-if="!isHidePositive"
        class="ml-2"
        style-set="large-primary"
        width="160"
        :disabled="isValid !== true || disabled.button"
        @click="confirm"
      >
        {{ $t('common.button.confirm2') }}
      </Button>
    </template>
  </ModalEdit>
  <Modal v-else>
    <Paragraph style-set="modal-text1">{{
      isAssigned
        ? $t('project.pages.edit.customer.confirm.assign')
        : $t('project.pages.edit.customer.confirm.new')
    }}</Paragraph>
    <Paragraph style-set="modal-text2">{{
      $t('project.pages.edit.customer.confirm.text')
    }}</Paragraph>
    <template #foot>
      <Button outlined style-set="large-tertiary" width="160" @click="cancel">
        {{ $t('common.button.cancel') }}
      </Button>
      <Button
        v-if="!isHidePositive"
        class="ml-2"
        style-set="large-primary"
        width="160"
        @click="save"
      >
        {{ $t('common.button.ok') }}
      </Button>
    </template>
  </Modal>
</template>

<script lang="ts">
import type { LocaleMessages } from 'vue-i18n/types'
import ModalEdit from '~/components/common/molecules/ModalEdit.vue'
import Modal from '~/components/common/molecules/Modal.vue'
import {
  Button,
  Sheet,
  Paragraph,
  AutoComplete,
  TextField,
  ErrorText,
} from '~/components/common/atoms/index'
import CommonDetailRow from '~/components/common/molecules/CommonDetailRow.vue'
import { GetProjectByIdResponse } from '~/models/Project'
import BaseComponent, { PropType } from '~/common/BaseComponent'

import {
  CreateUser,
  CreateUserRequest,
  CreateUserResponse,
  GetUserById,
  GetUsers,
  GetUsersRequest,
} from '~/models/User'
import { ENUM_USER_ROLE } from '~/types/User'

export class LocalCustomer extends CreateUserRequest {
  id: string = ''
}

export default BaseComponent.extend({
  name: 'ProjectCreateCustomerModal',
  components: {
    ModalEdit,
    Modal,
    Sheet,
    Button,
    Paragraph,
    AutoComplete,
    TextField,
    ErrorText,
    CommonDetailRow,
  },
  props: {
    /** 選択した案件詳細 */
    project: {
      type: Object as PropType<GetProjectByIdResponse>,
      required: true,
    },
    /** 編集中の案件情報 */
    localProject: {
      type: Object,
      required: true,
    },
    title: {
      type: String,
      required: true,
    },
    /** サジェストされるユーザー一覧 */
    suggestUsers: {
      type: Array,
      required: true,
    },
    /** アサインされたお客様一覧 */
    assignedCustomers: {
      type: Array,
    },
    /** 取引先 */
    company: {
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
    /** 作成モードか */
    isCreating: {
      type: Boolean,
    },
    /** 確定ボタン/OKボタンの制御 */
    isHidePositive: {
      type: Boolean,
      default: false,
    },
    /** 選択中のお客様ID */
    selectedCustomerId: {
      type: String,
      default: '',
    },
    /** 選択中のお客様名 */
    selectedCustomerName: {
      type: String,
      default: '',
    },
  },
  data(): {
    selectedUserInfo: {
      [key: string]: any
    }
    localCustomer: LocalCustomer
    createdCustomerInfo: CreateUserResponse
    userId: string
    customerName: string
    isConfirm: boolean
    isEditing: boolean
    isAssigned: boolean
    alertCustomer: boolean
    errorText: string
    disabled: {
      name: boolean
      email: boolean
      job: boolean
      organizationName: boolean
      button: boolean
    }
    isValid: boolean
  } {
    return {
      selectedUserInfo: {},
      localCustomer: new LocalCustomer(),
      createdCustomerInfo: new CreateUserResponse(),
      userId: '',
      customerName: this.selectedCustomerName,
      isConfirm: false,
      isEditing: false,
      isAssigned: false,
      alertCustomer: false,
      errorText: '',
      disabled: {
        name: false,
        email: false,
        job: false,
        organizationName: false,
        button: false,
      },
      isValid: false,
    }
  },
  computed: {
    /**
     * 顧客一覧データをフォーマットし、返す
     * @return 顧客一覧
     */
    customer() {
      const customersList: object[] = []
      const customerInfo = { label: '', value: '' }

      this.suggestUsers.forEach((elm: any) => {
        if (elm.customerName) {
          //お客様名（会社名）
          customerInfo.label = elm.name + '（' + elm.customerName + '）'
        } else {
          //お客様名
          customerInfo.label = elm.name
        }
        customerInfo.value = elm.id
        const newElm = Object.assign({}, customerInfo)
        customersList.push(newElm)
      })
      return customersList
    },
    /**
     * ユーザーネームのフォーム直下に出す警告文の出し分け
     */
    warning(): string | LocaleMessages {
      this.disabled.button = false
      const userIds: string[] = []
      this.assignedCustomers.forEach((user: any) => {
        userIds.push(user.id)
      })
      // メールアドレスから入力した際はユーザーネームのフォーム直下に警告文を表示させない
      if (Object.keys(this.selectedUserInfo).length !== 0) {
        // 重複チェック
        if (userIds.includes(this.localCustomer.id)) {
          this.disabled.button = true
          return this.$t('project.pages.edit.customer.errorMessage')
          // 案件の取引先と異なる顧客メンバーかチェック
        } else if (
          this.localCustomer.name &&
          this.localCustomer.customerId &&
          this.selectedCustomerId !== this.localCustomer.customerId
        ) {
          return this.$t('project.pages.create.customer.errorText', {
            customerName: this.selectedUserInfo.customerName,
          })
        }
      }
      return ''
    },
  },
  methods: {
    /** 名前からユーザの存在チェック */
    async setUser(userInfo: string | { label: ''; value: '' }) {
      if (userInfo) {
        if (
          this.errorText ===
            this.$t(
              'project.pages.create.customer.notCustomerRoleErrorMessage'
            ) ||
          this.errorText ===
            this.$t('project.pages.edit.customer.invalidErrorMessage')
        ) {
          this.disabled.button = true
        } else if (
          this.errorText !==
            this.$t(
              'project.pages.create.customer.salesforceMainCustomerDomainMismatch'
            ) &&
          this.errorText !==
            this.$t(
              'project.pages.create.customer.dedicatedSurveyUserDomainMismatch'
            )
        ) {
          // 各判定を初期化
          this.alertCustomer = false
          this.isAssigned = false
          this.errorText = ''
        }
        //既存ユーザの選択時
        if (typeof userInfo === 'object') {
          await GetUserById(userInfo.value).then((res) => {
            this.selectedUserInfo = res.data
            this.userId = userInfo.value
          })
          this.localCustomer.id = this.selectedUserInfo.id
          this.localCustomer.name = this.selectedUserInfo.name
          this.localCustomer.email = this.selectedUserInfo.email
          this.localCustomer.customerId = this.selectedUserInfo.customerId
          this.customerName = this.selectedUserInfo.customerName
          this.localCustomer.job = this.selectedUserInfo.job
          this.localCustomer.organizationName =
            this.selectedUserInfo.organizationName
          this.isAssigned = true
          this.alertCustomer = false
          this.errorText = ''
          this.disabledForms('exceptName')
        }
      } else {
        this.localCustomer = new LocalCustomer()
        this.customerName = this.selectedCustomerName
        this.disabled.name = false
        this.disabled.email = false
        this.disabled.job = false
        this.disabled.organizationName = false
        this.disabled.button = false
        this.isAssigned = false
      }
    },
    /** メールアドレスからユーザの存在チェック */
    async checkExistUser(email: string) {
      // メールアドレス文字列を補正
      email = this.adjustEmail(email)
      this.localCustomer.email = email

      // ユーザー存在フラグ
      let isUserFound = false

      // 各判定を初期化
      this.alertCustomer = false
      this.isAssigned = false
      this.errorText = ''

      // Eメールの値が空なら問い合わせをしない
      if (email) {
        // リクエストを作成
        const request = new GetUsersRequest()
        request.email = email
        await GetUsers(request).then((res) => {
          const usersInfo = res.data.users
          if (usersInfo.length === 0) {
            this.isAssigned = false
          } else {
            for (const i in usersInfo) {
              const userInfo = usersInfo[i]
              isUserFound = true
              // DB登録済みかつお客様ロール以外のメールアドレスが設定された場合
              if (userInfo.role !== ENUM_USER_ROLE.CUSTOMER) {
                this.alertCustomer = true
                this.disabled.button = true
                this.errorText = this.$t(
                  'project.pages.create.customer.notCustomerRoleErrorMessage'
                ) as string
                continue
                // 無効なユーザのメールアドレスが設定された場合
              } else if (
                userInfo.disabled === true ||
                request.email !== userInfo.email
              ) {
                this.alertCustomer = true
                this.disabled.button = true
                this.errorText = this.$t(
                  'project.pages.edit.customer.invalidErrorMessage'
                ) as string
                continue
              }
              this.userId = userInfo.id
              this.localCustomer.id = userInfo.id
              this.localCustomer.name = userInfo.name
              this.localCustomer.email = userInfo.email
              this.localCustomer.customerId = userInfo.customerId
              this.customerName = userInfo.customerName
              this.localCustomer.job = userInfo.job
              this.localCustomer.role = userInfo.role
              // @ts-ignore
              this.localCustomer.organizationName = userInfo.organizationName
            }
          }
        })
      }
      // 既存ユーザのメールアドレス設定時
      if (isUserFound) {
        if (!this.errorText) {
          this.duplicationUserCheck()
          if (this.alertCustomer === false) {
            this.projectCompanyCustomerCheck(
              this.localCustomer.customerId,
              this.customerName
            )
            this.isAssigned = true
            this.disabledForms('exceptEmail')
          }
          if (
            this.alertCustomer === false &&
            (this.localProject.salesforceMainCustomer.email ||
              this.localProject.dedicatedSurveyUserEmail)
          ) {
            this.domainMatchCheck()
            this.isAssigned = true
            this.disabledForms('exceptEmail')
          }
        }
      } else {
        if (
          this.alertCustomer === false &&
          (this.localProject.salesforceMainCustomer.email ||
            this.localProject.dedicatedSurveyUserEmail)
        ) {
          this.domainMatchCheck()
        }
        this.customerName = this.selectedCustomerName
        this.disabled.name = false
        this.disabled.email = false
        this.disabled.job = false
        this.disabled.organizationName = false
        this.disabled.button = false
        this.isAssigned = false
      }
    },
    /** フォーム補完後、手動入力フォーム以外はdisabled */
    disabledForms(type: string) {
      if (type === 'exceptName') {
        this.disabled.email = true
        this.disabled.job = true
        this.disabled.organizationName = true
      } else if (type === 'exceptEmail') {
        this.disabled.name = true
        this.disabled.job = true
        this.disabled.organizationName = true
      }
    },
    cancel() {
      this.isConfirm = false
    },
    /** 重複チェック */
    duplicationUserCheck() {
      this.disabled.button = false
      this.alertCustomer = false
      this.errorText = ''
      // 重複チェック
      const userIds: string[] = []
      this.assignedCustomers.forEach((user: any) => {
        userIds.push(user.id)
      })
      if (userIds.includes(this.localCustomer.id)) {
        this.disabled.button = true
        this.alertCustomer = true
        this.errorText = this.$t(
          'project.pages.edit.customer.errorMessage'
        ) as string
      } else {
        this.disabled.button = false
        this.alertCustomer = false
      }
    },
    /** 案件に登録した取引先と異なる顧客メンバーか確認 */
    projectCompanyCustomerCheck(customerId: string, customerName: string) {
      if (customerId !== this.company.id) {
        this.alertCustomer = true
        this.errorText = this.$t('project.pages.create.customer.errorText', {
          customerName,
        }) as string
      }
    },
    // 入力したメールアドレスのドメインが取引先担当者またはアンケート送信宛先指定に設定されたものと一致しているか確認
    domainMatchCheck() {
      const inputDomain = this.localCustomer.email
        ? this.localCustomer.email.split('@')[1]
        : ''
      const salesforceMainCustomerDomain = this.localProject
        .salesforceMainCustomer.email
        ? this.localProject.salesforceMainCustomer.email.split('@')[1]
        : ''
      const dedicatedSurveyUserDomain = this.localProject
        .dedicatedSurveyUserEmail
        ? this.localProject.dedicatedSurveyUserEmail.split('@')[1]
        : ''

      // 「アンケートに宛先を指定する」がONの場合、取引先担当者（お客様担当者）のアドレスをドメインチェック
      if (
        this.localProject.isSurveyEmailToSalesforceMainCustomer &&
        inputDomain &&
        inputDomain !== dedicatedSurveyUserDomain &&
        salesforceMainCustomerDomain &&
        inputDomain !== salesforceMainCustomerDomain
      ) {
        this.alertCustomer = true
        this.errorText = this.$t(
          'project.pages.create.customer.salesforceMainCustomerDomainMismatch'
        ) as string
        // 「アンケートに宛先を指定する」がOFFの場合、アンケート送信宛先指定のアドレスをドメインチェック
      } else if (
        !this.localProject.isSurveyEmailToSalesforceMainCustomer &&
        inputDomain &&
        dedicatedSurveyUserDomain &&
        inputDomain !== dedicatedSurveyUserDomain
      ) {
        this.alertCustomer = true
        this.errorText = this.$t(
          'project.pages.create.customer.dedicatedSurveyUserDomainMismatch'
        ) as string
      }
    },
    confirm() {
      this.isConfirm = true
    },
    /** お客様メンバー情報を保存 */
    async save() {
      this.clearErrorBar()
      const customerInfo = { id: '', name: '' }
      // 新規ユーザを作成・アサイン
      if (!this.isAssigned) {
        if (!this.selectedCustomerId) {
          this.showErrorBarWithScrollPageTop(
            this.$t('project.pages.create.customer.errors.requiredCustomerId')
          )
          this.$emit('click:negative')
        } else {
          this.localCustomer.role = 'customer'
          this.localCustomer.customerId = this.company.id
          await CreateUser(this.localCustomer)
            .then((res) => {
              this.createdCustomerInfo = res.data
              customerInfo.id = this.createdCustomerInfo.id
              customerInfo.name = this.createdCustomerInfo.name
              this.$emit('saveCustomerCreate', customerInfo)
            })
            .catch(() => {
              this.showErrorBarWithScrollPageTop(this.$t('msg.error.default'))
              this.$emit('click:negative')
            })
        }
        //既存のユーザをアサイン
      } else {
        customerInfo.id = this.userId
        customerInfo.name = this.localCustomer.name
        this.$emit('saveCustomerCreate', customerInfo)
      }
    },
    onSubmit() {
      return false
    },
  },
})
</script>
