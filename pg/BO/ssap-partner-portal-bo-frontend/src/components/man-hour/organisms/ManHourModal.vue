<template>
  <!-- 数値編集 -->
  <ModalEdit
    :title="$t('man-hour.modal.title')"
    :item="manHourBySupporterUserId"
    small-modal
  >
    <div class="d-flex align-center justify-center pt-4 pb-2">
      <Sheet width="80">
        <TextField
          v-model.number="localNumber"
          type="number"
          number
          outlined
          dense
          :max-digits="6"
          :positive-number-digits="3"
          :decimal-number-digits="2"
          @change="inputHours"
        />
      </Sheet>
      <span class="pl-2">h</span>
    </div>
    <template #foot>
      <!-- キャンセルボタン -->
      <Button
        outlined
        style-set="large-tertiary"
        width="160"
        @click="$emit('closeModal')"
      >
        {{ $t('common.button.cancel') }}
      </Button>
      <!-- 保存ボタン -->
      <Button
        class="ml-2"
        style-set="large-primary"
        width="160"
        :disabled="isDisabled"
        @click="update"
      >
        {{ $t('common.button.save2') }}
      </Button>
    </template>
  </ModalEdit>
</template>

<script lang="ts">
import BaseComponent from '~/common/BaseComponent'
import { Button, TextField, Sheet } from '~/components/common/atoms/index'
import ModalEdit from '~/components/common/molecules/ModalEdit.vue'
import {
  UpdateManHourBySupporterUserIdRequest,
  UpdateManHourBySupporterUserId,
} from '~/models/ManHour'

export default BaseComponent.extend({
  name: 'ManHourModal',
  components: {
    Button,
    TextField,
    Sheet,
    ModalEdit,
  },
  props: {
    /** 支援者単位での支援工数情報 */
    manHourBySupporterUserId: {
      type: Object,
    },
    /** コピーされた支援者単位での支援工数情報 */
    localManHourBySupporterUserId: {
      type: Object,
    },
    /** コピーされた表示項目で絞られた支援者単位での支援工数情報 */
    localManHourData: {
      type: Object,
    },
    /** 表示項目 */
    type: {
      type: String,
      default: '',
    },
    /** 何行目か */
    index: {
      type: [String, Number],
    },
    /** 実績工数 */
    number: {
      type: Number,
    },
  },
  data() {
    return {
      localNumber: this.number,
      version: 0,
    }
  },
  mounted() {
    this.version = parseInt(this.localManHourBySupporterUserId.version)
  },
  computed: {
    /**
     * バリデーションチェック
     * @returns 条件に合わない場合はtrueを返す
     */
    isDisabled(): boolean {
      //文字の長さを判定
      const length = String(this.localNumber).length > 6
      //正の数を判定
      const positive = Math.sign(this.localNumber) === -1
      //整数が3桁以内か判定
      const maxNumber = this.localNumber >= 1000
      //小数点が2桁か判定
      const decPart = String(this.localNumber).split('.')
      if (decPart[1]) {
        const decPartLength = decPart[1].length > 3
        return length || positive || maxNumber || decPartLength
      } else {
        return length || positive || maxNumber
      }
    },
  },
  methods: {
    /** 入力された数値を格納 */
    inputHours(elm: number) {
      this.localNumber = elm
    },
    /**
     * UpdateManHourBySupporterUserIdAPIを叩いて、支援者単位で支援工数を取得
     * @param {number} year ルートパラメータから取得した年
     * @param {number} month ルートパラメータから取得した月
     * @param {string} supporterUserId ルートパラメータから取得した支援者ユーザーID
     * @param {number} version ロックキー（楽観ロック制御）
     * @param {UpdateManHourBySupporterUserIdRequest} request UpdateManHourBySupporterUserIdAPIのリクエストパラメータ
     */
    async update() {
      this.clearErrorBar()
      const supporterUserId = this.$route.params.userId
      const version = this.version
      const year = parseInt(this.$route.params.year)
      const month = parseInt(this.$route.params.month)
      if (
        this.type === 'directSupportManHours' ||
        this.type === 'preSupportManHours' ||
        this.type === 'salesSupportManHours'
      ) {
        this.localManHourData.items[this.index].inputManHour = this.localNumber
      } else {
        this.localManHourData[this.index] = this.localNumber
      }
      this.localManHourBySupporterUserId[this.type] = this.localManHourData
      const request = new UpdateManHourBySupporterUserIdRequest()
      request.supporterUserName =
        this.localManHourBySupporterUserId.supporterUserName
      request.supporterOrganizationId =
        this.localManHourBySupporterUserId.supporterOrganizationId
      request.supporterOrganizationName =
        this.localManHourBySupporterUserId.supporterOrganizationName
      request.directSupportManHours =
        this.localManHourBySupporterUserId.directSupportManHours
      request.preSupportManHours =
        this.localManHourBySupporterUserId.preSupportManHours
      request.salesSupportManHours =
        this.localManHourBySupporterUserId.salesSupportManHours
      request.ssapManHours = this.localManHourBySupporterUserId.ssapManHours
      request.holidaysManHours =
        this.localManHourBySupporterUserId.holidaysManHours
      request.isConfirm = this.localManHourBySupporterUserId.isConfirm
      await UpdateManHourBySupporterUserId(
        supporterUserId,
        version,
        year,
        month,
        request
      )
        .then(() => {
          location.reload()
        })
        .catch(() => {
          this.showErrorBarWithScrollPageTop(this.$t('msg.error.default'))
          this.$emit('closeModal')
        })
    },
  },
})
</script>

<style lang="scss">
.o-manhour-modal-title {
  font-size: 1.125rem;
  font-weight: bold;
}
</style>
