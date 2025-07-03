<template>
  <CommonContainer
    :title="title"
    :is-editing="isEditing"
    :is-register="isRegister"
    hx="h1"
    is-hide-button1
    is-hide-header-button
    @buttonAction1="buttonAction1"
    @buttonAction2="buttonAction2"
  >
    <Sheet class="pa-8 pb-0">
      <Title hx="h2" style-set="detail">
        <Icon color="btn_primary" size="24" class="mt-n1"
          >mdi-checkbox-marked-circle</Icon
        >
        {{ $t('customer.pages.import.confirm.completed') }}
      </Title>
      <div class="o-common-data-table o-import-table is-scroll is-import">
        <CustomerImportTable :customers="customers" :is-loading="isLoading" />
      </div>
    </Sheet>
  </CommonContainer>
</template>

<script lang="ts">
import CustomerImportTable from './CustomerImportTable.vue'
import BaseComponent from '~/common/BaseComponent'
import type { PropType } from '~/common/BaseComponent'
import { ImportedCustomer } from '@/types/Customer'

import CommonContainer from '~/components/common/organisms/CommonContainer.vue'

import { Sheet, Title, Icon } from '~/components/common/atoms/index'

export default BaseComponent.extend({
  components: {
    CommonContainer,
    Sheet,
    Title,
    Icon,
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
      isRegister: false,
    }
  },
  methods: {
    buttonAction1() {},
    /**
     * /customer/listに遷移する
     */
    buttonAction2() {
      this.$router.push('/customer/list')
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
