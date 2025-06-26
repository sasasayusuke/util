<template>
  <div class="m-customer-list">
    <AddableRows
      :row-objects="customers"
      :max-rows="maxRows"
      :is-addable="true"
      :category-name="$t('project.row.customerUsers.customer')"
      @add="addRow"
      @remove="removeRow"
    >
      <template #staticRows>
        <tr>
          <th>{{ $t('project.row.customerUsers.mainCustomer') }}</th>
          <td>
            {{ mainCustomer.name }}
          </td>
          <td class="m-addable-rows__edit">
            <Button
              v-if="mainCustomer.name"
              style-set="small-primary"
              outlined
              width="96"
              :disabled="isDisabledMainCustomerUserButton"
              @click="edit('main')"
            >
              {{ $t('common.button.update') }}
            </Button>
            <Button
              v-else
              style-set="small-primary"
              outlined
              width="96"
              :disabled="isDisabledMainCustomerUserButton"
              @click="create('main')"
            >
              {{ $t('common.button.register2') }}
            </Button>
          </td>
          <td v-if="mainCustomer.name">
            <div class="ml-2">
              <Button
                style-set="icon-rotate-45"
                :disabled="isDisabledMainCustomerUserButton"
                @click="reset_main"
              >
                <Icon size="28">icon-org-button-plus-outline</Icon>
              </Button>
            </div>
          </td>
        </tr>
      </template>
      <template #default="slotProps">
        <td class="m-addable-rows__edit">
          <Button
            v-if="customers && customers.length"
            style-set="small-primary"
            outlined
            width="96"
            @click="edit('member', slotProps.index)"
          >
            {{ $t('common.button.update') }}
          </Button>
          <Button
            v-else
            style-set="small-primary"
            outlined
            width="96"
            @click="addRow"
          >
            {{ $t('common.button.register2') }}
          </Button>
        </td>
      </template>
    </AddableRows>
    <template v-if="isShowModal">
      <ProjectCreateCustomerModal
        v-if="modalType === 'create'"
        :project="project"
        :local-project="localProject"
        :title="modalTitle"
        :suggest-users="suggestUsers"
        :assigned-customers="assignedCustomers"
        :selected-customer-id="selectedCustomerId"
        :selected-customer-name="selectedCustomerName"
        :company="company"
        :is-editing="isEditing"
        :is-creating="isCreating"
        :event-type="eventType"
        @click:negative="close"
        @saveCustomerCreate="saveCustomerCreate"
      />
      <ProjectEditCustomerModal
        v-else
        :title="modalTitle"
        :user="modalUser"
        :selected="selected"
        :company="company"
        :event-type="eventType"
        is-editing
        @click:negative="close"
        @saveCustomerEdit="saveCustomerEdit"
      />
    </template>
  </div>
</template>

<script lang="ts">
import { Button, Icon } from '../../common/atoms/index'
import AddableRows from '../../common/molecules/AddableRows.vue'
import { GetProjectByIdResponse } from '~/models/Project'
import ProjectEditCustomerModal from '~/components/project/molecules/ProjectEditCustomerModal.vue'
import ProjectCreateCustomerModal from '~/components/project/molecules/ProjectCreateCustomerModal.vue'
import BaseComponent, { PropType } from '~/common/BaseComponent'

export default BaseComponent.extend({
  name: 'CustomerList',
  components: {
    Button,
    Icon,
    AddableRows,
    ProjectEditCustomerModal,
    ProjectCreateCustomerModal,
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
    /** お客様代表 */
    mainCustomer: {
      type: Object,
      required: true,
    },
    /** お客様一覧 */
    customers: {
      type: Array,
      required: true,
    },
    /** 取引先名（お客様名） */
    company: {
      type: Object,
    },
    /** サジェストユーザー一覧 */
    suggestUsers: {
      type: Array,
      required: true,
    },
    /** 作成モードか */
    isCreating: {
      type: Boolean,
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
    /** お客様メンバー一覧 - お客様(代表) のボタン無効化 */
    isDisabledMainCustomerUserButton: {
      type: Boolean,
      default: false,
    },
  },
  computed: {
    /**
     * @returns string モーダルのタイトル(モーダルによって変わる)
     */
    modalTitle() {
      const evenType = this.eventType as string
      const modalType = this.modalType as string
      return modalType === 'create'
        ? this.$t(`project.pages.create.customer.title.${evenType}`)
        : this.$t(`project.pages.edit.customer.title.${evenType}`)
    },
    /**
     * @returns string モーダルのユーザー(モーダルによって変わる)
     */
    modalUser() {
      let user
      if (this.eventType === 'main') {
        user = this.mainCustomer as any
      } else if (this.eventType === 'member') {
        user = this.customers as any
      }
      return user
    },
    /**
     * @returns array お客様一覧 + お客様代表
     */
    assignedCustomers() {
      const assignedCustomers = this.customers.slice()
      assignedCustomers.push(this.mainCustomer)
      return assignedCustomers
    },
  },
  data() {
    return {
      index: 0,
      maxRows: 10,
      eventType: 'producer',
      isEditing: true,
      isShowModal: false,
      isHidePositive: true,
      modalType: 'create',
      selected: 0,
    }
  },
  methods: {
    /** お客様メンバー一覧の選択行を削除 */
    removeRow(index: number) {
      this.$emit('removeRow', 'customer', index)
    },
    /** お客様メンバーを一行追加 */
    addRow() {
      this.eventType = 'register'
      this.isHidePositive = false
      this.modalType = 'create'
      this.isShowModal = true
    },
    /** お客様登録モーダルを開く */
    create(type: string) {
      this.eventType = type
      this.isHidePositive = true
      this.modalType = 'create'
      this.isShowModal = true
    },
    /** お客様編集モーダルを開く */
    edit(type: string, id: number) {
      this.eventType = type
      this.isHidePositive = true
      this.modalType = 'edit'
      this.isShowModal = true
      this.selected = id
    },
    /** お客様編集モーダル保存 */
    saveCustomerEdit(customerInfo: object) {
      this.$emit('saveCustomerEdit', customerInfo, this.eventType)
      this.isShowModal = false
    },
    /** お客様作成モーダル保存 */
    saveCustomerCreate(customerInfo: object) {
      this.$emit('saveCustomerCreate', customerInfo, this.eventType)
      this.isShowModal = false
    },
    reset_main() {
      this.$emit('resetMain', 'customer')
    },
    /** モーダルを閉じる */
    close() {
      this.isShowModal = false
    },
  },
})
</script>
