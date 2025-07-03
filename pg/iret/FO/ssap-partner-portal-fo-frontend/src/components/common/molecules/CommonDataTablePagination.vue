<template>
  <v-container class="m-table-pagination" fluid pa-0 mx-0>
    <v-row no-gutters align="end" justify="end">
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
        <span v-if="additionalText" class="ml-10">
          {{ additionalText }}
        </span>
      </v-col>
      <v-col v-if="showsPagination" cols="auto">
        <Button
          style-set="pagination"
          :aria-label="$t('common.button.prev')"
          :disabled="isLoading || offsetPage === 1"
          @click="$emit('click:prev')"
        >
          <Icon style-set="primary" size="30">mdi-chevron-left</Icon>
        </Button>
        <Button
          style-set="pagination"
          :aria-label="$t('common.button.next')"
          class="ml-2"
          :disabled="isLoading || offsetPage === lastPage || total === 0"
          @click="$emit('click:next')"
        >
          <Icon style-set="primary" size="30">mdi-chevron-right</Icon>
        </Button>
      </v-col>
      <v-col v-if="isCsvOutput" cols="auto">
        <Button style-set="small-primary" width="96" outlined>
          {{ $t('common.button.csv') }}
        </Button>
      </v-col>
    </v-row>
  </v-container>
</template>

<script lang="ts">
import { Button, Icon } from '~/components/common/atoms/index'
import BaseComponent from '~/common/BaseComponent'

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
      //required: true,
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
    additionalText: {
      type: String,
      default: '',
    },
  },
  computed: {
    pageText() {
      //if (this.isLoading) {
      //  return ''
      //}
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
      const total = this.total
      if (this.total === 0) {
        return this.$t('common.table.total', {
          total,
        })
      } else {
        return this.$t('common.table.pageText', {
          pageStart,
          pageStop,
          itemsLength,
        })
      }
    },
    lastPage() {
      return Math.ceil(this.total / this.itemsPerPage)
    },
    shortPageText() {
      // ja: 全{total}件
      const text = this.$t('common.table.shortPageText', {
        itemsLength: this.total,
      })
      return text
    },
    contractTime() {
      const text = this.$t('common.table.contractTime', {
        contractTime: this.totalContractTime,
      })
      return text
    },
  },
})
</script>
