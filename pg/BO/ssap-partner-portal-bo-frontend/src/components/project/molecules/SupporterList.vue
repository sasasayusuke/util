<template>
  <div class="m-suppoter-list">
    <AddableRows
      :row-objects="supporters"
      :min-rows="minRows"
      :max-rows="maxRows"
      :is-addable="true"
      :category-name="$t('project.pages.sub_list_item.supporter.accelerator')"
      @add="addRow"
      @remove="removeRow"
    >
      <template #staticRows>
        <tr>
          <th>{{ $t('project.pages.sub_list_item.supporter.producer') }}</th>
          <td>
            {{ mainSupporter.name }}
          </td>
          <td class="m-addable-rows__edit">
            <Button
              v-if="mainSupporter.name"
              style-set="small-primary"
              outlined
              width="96"
              @click="update('edit_producer', mainSupporter.id)"
            >
              {{ $t('common.button.update') }}
            </Button>
            <Button
              v-else
              style-set="small-primary"
              outlined
              width="96"
              @click="addProducer"
            >
              {{ $t('common.button.register2') }}
            </Button>
          </td>
          <td v-if="mainSupporter.name">
            <div class="ml-2">
              <Button
                class="m-suppoter-list__delete-button"
                style-set="icon-rotate-45"
                @click="resetProducer"
              >
                <Icon size="28">icon-org-button-plus-outline</Icon>
              </Button>
            </div>
          </td>
        </tr>
      </template>
      <template #default="slotProps">
        <td class="m-addable-rows__edit">
          <template v-if="supporters && supporters.length">
            <Button
              style-set="small-primary"
              outlined
              width="96"
              @click="
                update(
                  'edit_accelerator',
                  supporters[slotProps.index].id,
                  slotProps.index
                )
              "
            >
              {{ $t('common.button.update') }}
            </Button>
          </template>
          <template v-else>
            <Button
              style-set="small-primary"
              outlined
              width="96"
              @click="addRow"
            >
              {{ $t('common.button.register2') }}
            </Button>
          </template>
        </td>
      </template>
    </AddableRows>
    <template v-if="isShowModal">
      <ProjectCreateSupporterModal
        v-if="modalType === 'create'"
        :title="modalTitle"
        :assigned-users="assignedSupporters"
        :event-type="eventType"
        :main-supporter-info="mainSupporter"
        is-editing
        @click:negative="close"
        @click:positive="save"
        @saveSupporterCreate="saveSupporterCreate"
      />
      <ProjectEditSupporterModal
        v-else
        :title="modalTitle"
        :assigned-users="supporters"
        :selected-user-id="selected"
        :event-type="eventType"
        is-editing
        @click:negative="close"
        @click:positive="save"
        @saveSupporterEdit="saveSupporterEdit"
      />
    </template>
  </div>
</template>

<script lang="ts">
import { Button, Icon } from '../../common/atoms/index'
import AddableRows from '../../common/molecules/AddableRows.vue'
import ProjectCreateSupporterModal from '~/components/project/molecules/ProjectCreateSuppoterModal.vue'
import ProjectEditSupporterModal from '~/components/project/molecules/ProjectEditSuppoterModal.vue'
import BaseComponent from '~/common/BaseComponent'

export default BaseComponent.extend({
  components: {
    Button,
    Icon,
    AddableRows,
    ProjectEditSupporterModal,
    ProjectCreateSupporterModal,
  },
  props: {
    /** プロデューサー */
    mainSupporter: {
      type: Object,
      required: true,
    },
    /** アクセラレーター */
    supporters: {
      type: Array,
      required: true,
    },
    /** 新規作成か */
    isCreating: {
      type: Boolean,
    },
    /** 支援者組織 */
    supporterOrganizations: {
      type: Array,
      required: true,
    },
  },
  computed: {
    /**
     * モーダルに表示するタイトルを返す
     * @returns モーダルの表示タイトル文字列
     */
    modalTitle() {
      const eventType = this.eventType as string
      return this.$t(`project.pages.edit.supporter.title.${eventType}`)
    },
    /**
     * モーダルに表示する支援者/支援者責任者情報を返す
     * @returns モーダルの表示ユーザー情報
     */
    modalUser() {
      let user
      if (this.eventType.match(/producer/)) {
        user = this.mainSupporter as any
      } else if (this.eventType.match(/accelerator/)) {
        user = this.supporters as any
      }
      return user
    },
    /**
     * 取引先および取引先担当者を配列で返す
     * @returns 取引先および取引先担当者情報配列
     */
    assignedSupporters() {
      const assignedCustomers = this.supporters.slice()
      assignedCustomers.push(this.mainSupporter)
      return assignedCustomers
    },
  },
  data() {
    return {
      minRows: 0,
      maxRows: 50,
      eventType: 'producer',
      isEditing: false,
      isShowModal: false,
      isHidePositive: true,
      modalType: 'create',
      selected: '',
      editIndex: 0,
    }
  },
  methods: {
    /**
     * アクセラレーター一覧の選択行を削除
     * @param index 選択された行インデックス番号
     */
    removeRow(index: number) {
      this.supporters.splice(index, 1)
    },
    /** アクセラレーターの行を追加 */
    addRow() {
      this.eventType = 'add_accelerator'
      this.isEditing = true
      this.isHidePositive = false
      this.modalType = 'create'
      this.isShowModal = true
    },
    /** プロデューサーの行を追加 */
    addProducer() {
      this.eventType = 'add_producer'
      this.isEditing = true
      this.isHidePositive = false
      this.modalType = 'create'
      this.isShowModal = true
    },
    /**
     * モーダル画面を開く
     * @param type 編集対象判定文字列
     * @param index 選択された行インデックス番号
     */
    update(type: string, id: string, index: number) {
      this.selected = id
      this.editIndex = index
      this.eventType = type
      this.isEditing = false
      this.isHidePositive = true
      this.modalType = 'edit'
      this.isShowModal = true
    },
    /**
     * 支援者モーダルの変更を保存
     * @param supporterInfo 入力した支援者情報
     */
    saveSupporterEdit(supporterInfo: object) {
      this.$emit(
        'saveSupporterEdit',
        supporterInfo,
        this.eventType,
        this.selected
      )
      this.isShowModal = false
    },
    /**
     * 支援者モーダルの作成を保存
     * @param supporterInfo 入力した支援者情報
     */
    saveSupporterCreate(supporterInfo: object) {
      this.$emit('saveSupporterCreate', supporterInfo, this.eventType)
      this.isShowModal = false
    },
    /** プロデューサーの行を削除 */
    resetProducer() {
      this.$emit('resetMain', 'supporter')
    },
    /** モーダルをクローズ */
    close() {
      this.isShowModal = false
    },
  },
})
</script>

<style lang="scss" scoped>
.m-suppoter-list__delete-button {
  transform-origin: center center;
  transform: rotate(45deg);
}
</style>
