<template>
  <CommonContainer
    :title="title"
    :is-valid="isValid"
    :is-editing="isEditing"
    :is-register="isRegister"
    hx="h1"
    @buttonAction1="buttonAction1"
    @buttonAction2="buttonAction2"
  >
    <Sheet class="pa-8 pb-0">
      <Title hx="h2" style-set="detail">
        {{ $t('customer.pages.import.confirm.lead') }}
      </Title>
      <p class="font-size-small">
        {{ $t('customer.pages.import.confirm.text') }}
      </p>
      <div class="o-common-data-table is-scroll o-import-table">
        <CustomerImportTable :customers="customers" :is-loading="isLoading" />
      </div>
    </Sheet>
  </CommonContainer>
</template>

<script lang="ts">
import CustomerImportTable from './CustomerImportTable.vue'
import BaseComponent from '~/common/BaseComponent'
import type { PropType } from '~/common/BaseComponent'
import CommonContainer from '~/components/common/organisms/CommonContainer.vue'
import OverflowTooltip from '~/components/common/molecules/OverflowTooltip.vue'
import { ImportedCustomer } from '@/types/Customer'

import {
  Sheet,
  Title,
  Button,
  FileInput,
  Paragraph,
} from '~/components/common/atoms/index'

export default BaseComponent.extend({
  name: 'TemplateCustomerImportConfirm',
  components: {
    CommonContainer,
    Sheet,
    Title,
    Button,
    FileInput,
    Paragraph,
    OverflowTooltip,
    CustomerImportTable,
  },
  props: {
    /**
     * 顧客情報
     */
    customers: {
      type: Array as PropType<ImportedCustomer[]>,
      required: true,
    },
    /**
     * ローディング中か否か
     */
    isLoading: {
      type: Boolean,
      required: true,
    },
  },
  data() {
    return {
      title: this.$t('customer.pages.import.name'),
      isEditing: false,
      isRegister: true,
      isValid: true,
    }
  },
  methods: {
    /**
     * /customer/import/doneに遷移する
     */
    buttonAction1() {
      this.$router.push({
        path: '/customer/import/done',
        query: { objectKey: this.objectKey },
      })
    },
    /**
     * /customer/importに遷移する
     */
    buttonAction2() {
      this.$router.push('/customer/import')
    },
  },
  computed: {
    /**
     * @returns クエリパラメータのobjectKeyを返却
     */
    objectKey(): string {
      return this.$route.query.objectKey as string
    },
  },
})
</script>

<style lang="scss">
.o-import-table {
  .v-data-table {
    background-color: $c-white;
    .v-data-table__wrapper {
      table {
        tbody {
          tr {
            &:hover,
            &:focus {
              background: $c-white !important;
            }

            &:nth-child(even) {
              &:hover,
              &:focus {
                background-color: $c-black-table-border !important;
              }
            }
            td {
              font-size: 0.75rem;
              padding: 16px;
            }
          }
        }
      }
      &::-webkit-scrollbar {
        width: 10px;
        height: 10px;
      }
      &::-webkit-scrollbar-thumb {
        border-radius: 8px;
        --bg-opacity: 1;
        background-color: #808080;
      }
    }
    .v-data-footer {
      display: none;
    }
  }
}
</style>
