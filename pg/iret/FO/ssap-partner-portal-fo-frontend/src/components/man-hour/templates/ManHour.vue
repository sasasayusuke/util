<template>
  <RootTemplate width="1200" class="t-man-hour pb-10 mb-10">
    <InPageHeader v-if="!isEditing" :level="1" class="type2 pb-8">
      {{ pageName }}
    </InPageHeader>
    <ManHourContainer
      :man-hour="manHour"
      :suggest-customers="suggestCustomers"
      :is-editing="isEditing"
      @buttonAction="buttonAction"
      @change="onChange"
    />
    <ManHourModal
      :man-hour="manHour"
      :is-modal-open="isModalOpen"
      @closeModal="closeModal"
      @update="update"
    />
  </RootTemplate>
</template>

<script lang="ts">
import cloneDeep from 'lodash/cloneDeep'
import BaseComponent from '~/common/BaseComponent'
import RootTemplate from '~/components/common/bases/RootTemplate.vue'
import InPageHeader from '~/components/man-hour/organisms/InPageHeader.vue'
import ManHourContainer from '~/components/man-hour/organisms/ManHourContainer.vue'
import ManHourModal from '~/components/man-hour/organisms/ManHourModal.vue'
import { Button } from '~/components/common/atoms/index'
import Modal from '~/components/common/molecules/Modal.vue'
import { getCurrentDate } from '~/utils/common-functions'
import {
  GetManHourByMineResponse,
  UpdateManHourByMine,
  UpdateManHourByMineRequest,
} from '~/models/ManHour'

export default BaseComponent.extend({
  components: {
    RootTemplate,
    InPageHeader,
    ManHourContainer,
    ManHourModal,
    Button,
    Modal,
  },
  props: {
    /**
     * 支援工数データ
     */
    manHour: {
      type: Object,
    },
    /**
     * サジェストする顧客情報
     */
    suggestCustomers: {
      type: Array,
    },
  },
  data() {
    return {
      isEditing: false,
      pageName: this.$t('man-hour.pages.list.name'),
      localManHour: new GetManHourByMineResponse(),
      isModalOpen: false,
    }
  },
  methods: {
    /**
     * ボタン押下時の挙動
     * @param ボタンのアクションタイプ
     */
    buttonAction(actionType: number = 0) {
      // 処理する階層も要検討
      switch (actionType) {
        case 0:
          this.isEditing = false
          this.$emit('refresh')
          break
        case 1:
          this.isEditing = true
          break
        case 2:
          // 編集後の保存の処理
          this.update(false)
          this.isEditing = false
          break
        case 3:
          // 確定確認の modal 表示
          this.localManHour = cloneDeep(this.manHour)
          this.isModalOpen = true
          break
        case 4:
          this.isEditing = true
          break
        default:
          this.isEditing = false
          break
      }
    },
    /**
     * データ更新処理
     * @param 確認済みか否か
     */
    update(isConfirm: boolean) {
      this.clearErrorBar()
      let version = this.manHour.version
      // データが存在しない月はversionがnullのため1を入れる
      if (version === null) {
        version = 1
      }

      //送信前のlocalManHourはデータを持っていないのでqueryから取得
      let year = Number(this.$route.query.year)
      let month = Number(this.$route.query.month)
      //queryに年月が指定されていない場合、当月を取得
      if (!year && !month) {
        year = getCurrentDate().getFullYear()
        month = getCurrentDate().getMonth() + 1
      }

      const request = new UpdateManHourByMineRequest()

      //requestの中にデータ入れるlocalManHour
      // 本来はsupporterUserName だが、APIの戻り値によりsupporterNameを記載する
      // APIの修正が入った場合はrequest.supporterUserNameを修正すること
      request.supporterUserName = this.manHour.supporterName
      request.supporterOrganizationId = this.manHour.supporterOrganizationId
      request.supporterOrganizationName = this.manHour.supporterOrganizationName
      request.directSupportManHours = this.localManHour.directSupportManHours
      request.preSupportManHours = this.localManHour.preSupportManHours
      request.salesSupportManHours = this.localManHour.salesSupportManHours
      request.ssapManHours = this.localManHour.ssapManHours
      request.holidaysManHours = this.localManHour.holidaysManHours
      request.isConfirm = isConfirm

      // @ts-ignore
      UpdateManHourByMine(year, month, version, request)
        .then(() => {
          // 保存成功後処理
          this.$emit('refresh')
        })
        .catch(() => {
          this.showErrorBarWithScrollPageTop(this.$t('msg.error.default'))
        })
    },
    /**
     * 入力値に変更があった場合の処理
     * @param 入力データ
     */
    onChange(data: any) {
      this.localManHour = data
    },
    /**
     * モーダルを閉じる時の処理
     */
    closeModal() {
      this.isModalOpen = false
    },
  },
})
</script>

<style lang="scss" scoped>
.t-man-hour {
  width: 1200px;
}
</style>
<style lang="scss">
.t-man-hour {
  .m-modal__body {
    .a-sheet {
      border-radius: 4px 4px 0 0;
    }
  }
}
</style>
