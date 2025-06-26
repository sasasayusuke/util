<template>
  <v-container class="m-table-pagination" fluid pa-0 mx-0>
    <v-row no-gutters align="end" justify="end">
      <!-- テキスト -->
      <v-col v-if="showsText">
        <p v-if="!isShortPageText" class="font-size-small my-0">
          {{ pageText }}
        </p>
        <p v-else class="font-size-small my-0">
          {{ shortPageText }}
          <span v-if="totalContractTime || isTotalContractTime" class="ml-12">
            {{ contractTime }}
          </span>
        </p>
      </v-col>
      <!-- ボタン -->
      <v-col v-if="showsPagination" cols="auto">
        <Button
          style-set="pagination"
          :disabled="isLoading || offsetPage === 1"
          :aria-label="$t('common.button.prev')"
          @click="$emit('click:prev')"
        >
          <Icon style-set="primary" size="30">mdi-chevron-left</Icon>
        </Button>
        <Button
          style-set="pagination"
          class="ml-2"
          :disabled="isLoading || offsetPage === lastPage || total === 0"
          :aria-label="$t('common.button.next')"
          @click="$emit('click:next')"
        >
          <Icon style-set="primary" size="30">mdi-chevron-right</Icon>
        </Button>
      </v-col>
      <v-col v-if="isCsvOutput" cols="auto">
        <div class="m-heading__ui d-flex align-center">
          <p v-if="lastCountDate" class="m-heading__date font-size-xsmall">
            {{ $t('common.lastUpdate.total') + lastCountDate }}
          </p>
          <div class="m-heading__button ml-8">
            <Button
              style-set="small-primary"
              width="96"
              outlined
              :disabled="csvButtonDisabled"
              @click="$emit('csvOutput')"
            >
              {{ $t('common.button.csv') }}
            </Button>
          </div>
        </div>
      </v-col>
      <v-col v-if="isSelectRows" cols="auto">
        <div v-if="showSelect" class="m-heading__ui d-flex align-center">
          <div class="m-heading__button ml-5">
            <Button
              style-set="small-primary"
              :disabled="editButtonDisabled"
              @click="$emit('click:open-modal-bulk-edit')"
            >
              {{ $t('common.button.showModalBulkEdit') }}
            </Button>
          </div>
          <div class="m-heading__button ml-5">
            <Button
              style-set="small-error"
              :disabled="editButtonDisabled"
              @click="$emit('click:open-modal-bulk-delete')"
            >
              {{ $t('common.button.showModalBulkDelete') }}
            </Button>
          </div>
          <div class="m-heading__button ml-5">
            <Button
              style-set="small-primary"
              outlined
              :ripple="loadingSelectRows"
              @click="$emit('click:toggle-show-select')"
            >
              {{ $t('common.button.exitModeMultipleSelection') }}
            </Button>
          </div>
        </div>
        <div v-else class="m-heading__ui d-flex align-center">
          <div class="m-heading__button ml-5">
            <Button
              style-set="small-primary"
              width="204"
              outlined
              :disabled="isEnterModeMultipleSelectionDisabled"
              :ripple="loadingSelectRows"
              @click="$emit('click:toggle-show-select')"
            >
              {{ $t('common.button.enterModeMultipleSelection') }}
            </Button>
          </div>
        </div>
      </v-col>
    </v-row>
  </v-container>
</template>

<script lang="ts">
import { Button, Icon } from '~/components/common/atoms/index'
import BaseComponent from '~/common/BaseComponent'
import { hasRole, ADMIN_ROLE } from '~/utils/role-authorizer'

export default BaseComponent.extend({
  components: {
    Button,
    Icon,
  },
  props: {
    offsetPage: {
      type: Number,
      default: 1,
    },
    maxPage: {
      type: Number,
    },
    total: {
      type: Number,
      default: 100,
    },
    totalContractTime: {
      type: Number,
      default: 0,
    },
    itemsPerPage: {
      type: Number,
      // required: true,
    },
    isShortPageText: {
      type: Boolean,
      default: false,
    },
    showsText: {
      type: Boolean,
      default: true,
    },
    showsPagination: {
      type: Boolean,
      default: true,
    },
    isCsvOutput: {
      type: Boolean,
      default: false,
    },
    isHideButton: {
      type: Boolean,
      default: false,
    },
    isLoading: {
      type: Boolean,
      required: true,
    },
    isTotalContractTime: {
      type: Boolean,
      default: false,
    },
    csvButtonDisabled: {
      type: Boolean,
      default: false,
    },
    lastCountDate: {
      type: String,
      default: null,
    },
    editButtonDisabled: {
      type: Boolean,
      default: false,
    },
    showSelect: {
      type: Boolean,
      default: false,
    },
    isSelectRows: {
      type: Boolean,
      default: false,
    },
    isSingleMonth: {
      type: Boolean,
    },
  },
  computed: {
    pageText(): string {
      // TODO: 件数表示が出ていてないのはここでからのテキストをreturnしていたからですが、
      // なんでreturnしていたのか分からないのでコメントアウトにしておきます。
      // if (this.isLoading) {
      //   return ''
      // }
      // ja: {start} 〜 {end}件を表示 / 全{total}件
      const offsetPage = this.offsetPage as number
      const itemsPerPage = this.itemsPerPage as number
      const pageStart = offsetPage * itemsPerPage - itemsPerPage + 1
      const itemsLength = this.total as number
      let pageStop = Math.min(offsetPage * itemsPerPage, itemsLength)
      // 表示数が無限のとき
      if (pageStop < 0) {
        pageStop = itemsLength
      }
      // common.table.totalではtotalという変数しか受け付けないためitemLengthと同じだが、再定義
      const total = this.total
      if (total === 0) {
        return this.$t('common.table.total', {
          total,
        }) as string
      } else {
        return this.$t('common.table.pageText', {
          pageStart,
          pageStop,
          itemsLength,
        }) as string
      }
    },
    lastPage(): number {
      return Math.ceil(this.total / this.itemsPerPage)
    },
    shortPageText(): string {
      // ja: 全{total}件
      const text = this.$t('common.table.displayShort', {
        total: this.total,
      })
      return text as string
    },
    contractTime(): string {
      const text = this.$t('common.table.contractTime', {
        contractTime: this.totalContractTime,
      })
      return text as string
    },
    isEnterModeMultipleSelectionDisabled(): boolean {
      if (!hasRole([ADMIN_ROLE.SYSTEM_ADMIN, ADMIN_ROLE.SURVEY_OPS])) {
        return true
      }

      if (!this.isSingleMonth) {
        return true
      }

      return false
    },
  },
})
</script>

<style lang="scss" scoped>
.m-heading__date {
  margin: 0;
  color: $c-black-60;
}
</style>
