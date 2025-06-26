<template>
  <div class="m-customer-list">
    <AddableRows
      :row-objects="customers"
      :min-rows="minRows"
      :max-rows="maxRows"
      :is-addable="true"
      :category-name="$t('project.row.customerUsers.customer')"
      @add="addRow"
      @remove="removeRow"
    >
      <template #staticRows>
        <tr class="vertical-top">
          <th>
            {{ $t('project.row.customerUsers.mainCustomer') }}<br />
            <span class="annotation-text">{{
              $t('project.row.customerUsers.annotation')
            }}</span>
          </th>
          <td>
            {{ mainCustomer.name }}
          </td>
          <td class="m-addable-rows__edit">
            <Button
              v-if="mainCustomer.name"
              style-set="small-primary"
              outlined
              width="96"
              @click="edit('main')"
            >
              {{ $t('common.button.update') }}
            </Button>
            <Button
              v-else
              style-set="small-primary"
              outlined
              width="96"
              @click="create('main')"
            >
              {{ $t('common.button.register2') }}
            </Button>
          </td>
          <td v-if="mainCustomer.name">
            <div class="ml-2">
              <Button style-set="icon-rotate-45" @click="resetMain">
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
    //取引先名（お客様名）
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
  },
  computed: {
    /**
     * モーダルに表示するタイトルを返す
     * @returns モーダルの表示タイトル文字列
     */
    modalTitle() {
      const evenType = this.eventType as string
      const modalType = this.modalType as string
      return modalType === 'create'
        ? this.$t(`project.pages.create.customer.title.${evenType}`)
        : this.$t(`project.pages.edit.customer.title.${evenType}`)
    },
    /**
     * モーダルに表示するお客様情報を返す
     * @returns モーダルの表示ユーザー情報
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
     * お客様一覧表情およびお客様代表表情を配列で返す
     * @returns お客様一覧表情およびお客様代表情報配列
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
      minRows: 0,
      maxRows: 50,
      eventType: 'producer',
      isEditing: true,
      isShowModal: false,
      isHidePositive: true,
      modalType: 'create',
      selected: 0,
    }
  },
  methods: {
    /**
     * お客様メンバー一覧の選択行を削除
     * @param index 選択された行インデックス番号
     */
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
    /**
     * お客様登録モーダルを開く
     * @param type 編集対象判定文字列
     */
    create(type: string) {
      this.eventType = type
      this.isHidePositive = true
      this.modalType = 'create'
      this.isShowModal = true
    },
    /**
     * お客様編集モーダルを開く
     * @param type 編集対象判定文字列
     * @param index 選択された行インデックス番号
     */
    edit(type: string, id: number) {
      this.eventType = type
      this.isHidePositive = true
      this.modalType = 'edit'
      this.isShowModal = true
      this.selected = id
    },
    /**
     * お客様編集モーダル保存
     * @param customerInfo 入力したお客様情報
     */
    saveCustomerEdit(customerInfo: object) {
      this.$emit('saveCustomerEdit', customerInfo, this.eventType)
      this.isShowModal = false
    },
    /**
     * お客様作成モーダル保存
     * @param customerInfo 入力したお客様情報
     */
    saveCustomerCreate(customerInfo: object) {
      this.$emit('saveCustomerCreate', customerInfo, this.eventType)
      this.isShowModal = false
    },
    /** お客様(代表)をリセット */
    resetMain() {
      this.$emit('resetMain', 'customer')
    },
    /** モーダルを閉じる */
    close() {
      this.isShowModal = false
    },
  },
})
</script>

<style lang="scss" scoped>
.annotation-text {
  @include fontSize('xsmall');
  font-weight: normal;
  color: $c-black-60;
  white-space: nowrap;
  display: block;
}
.m-customer-list {
  .vertical-top {
    td,
    th {
      vertical-align: top;
      button {
        margin-top: -4px;
      }
    }
  }
}
</style>
