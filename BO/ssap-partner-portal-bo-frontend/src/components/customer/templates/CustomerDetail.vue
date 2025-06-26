<template>
  <RootTemplate>
    <CustomerDetailContainer
      :customer="customer"
      :is-editing="isEditing"
      :is-loading="isLoading"
      :is-loading-button="isLoadingButton"
      @click:positive="onClickPositive"
      @click:negative="onClickNegative"
    >
      <template v-if="!isEditing" #button>
        <Button style-set="small-tertiary-96" outlined @click="onClickNegative">
          {{ $t('common.button.backToList') }}
        </Button>
        <Button
          style-set="small-primary-96"
          class="ml-2"
          @click="onClickPositive"
        >
          {{ $t('common.button.edit') }}
        </Button>
      </template>
      <template v-else #button>
        <Button style-set="small-tertiary-96" outlined @click="onClickNegative">
          {{ $t('common.button.cancel') }}
        </Button>
        <Button
          style-set="small-primary-96"
          class="ml-2"
          @click="onClickPositive"
        >
          {{ $t('common.button.save') }}
        </Button>
      </template>
    </CustomerDetailContainer>
    <CustomerInvalidator
      v-if="!isEditing"
      @click:switchValid="isModalOpen = true"
    />
    <!-- モーダル -->
    <template v-if="isModalOpen">
      <template v-if="isDeletable">
        <CustomerInvalidatorModal
          :is-loading="isLoading"
          @click:closeModal="isModalOpen = false"
          @click:delete="deleteRecord"
        />
      </template>
      <template v-else>
        <CustomerDisableModal @click:closeModal="isModalOpen = false" />
      </template>
    </template>
  </RootTemplate>
</template>

<script lang="ts">
import CustomerDetailContainer, {
  LocalCustomer,
} from '../organisms/CustomerDetailContainer.vue'
import { Button } from '~/components/common/atoms/index'
import CommonDetail from '~/components/common/templates/CommonDetailTemplate.vue'
import CustomerInvalidator from '~/components/customer/molecules/CustomerInvalidator.vue'
import CustomerInvalidatorModal from '~/components/customer/molecules/CustomerInvalidatorModal.vue'
import CustomerDisableModal from '~/components/customer/molecules/CustomerDisableModal.vue'
import { PropType } from '~/common/BaseComponent'
import RootTemplate from '~/components/common/bases/RootTemplate.vue'

import {
  UpdateCustomerByIdRequest,
  UpdateCustomerById,
  DeleteCustomerById,
  GetCustomerByIdResponse,
} from '~/models/Customer'

export default CommonDetail.extend({
  components: {
    CustomerDetailContainer,
    CustomerInvalidator,
    CustomerInvalidatorModal,
    CustomerDisableModal,
    RootTemplate,
    Button,
  },
  head() {
    return {
      title: this.isEditing
        ? (this.$t('customer.pages.edit.name') as string)
        : (this.$t('customer.pages.detail.name') as string),
    }
  },
  props: {
    /**
     * IDから取得した取引先情報
     */
    customer: {
      type: Object as PropType<GetCustomerByIdResponse>,
      required: true,
    },
  },
  data() {
    return {
      headerPageName: this.$t('customer.group_info.name'),
      listPagePath: '/customer/list',
      isLoadingButton: false,
    }
  },
  /**
   * 顧客情報をアップデート
   * @param 顧客情報
   */
  methods: {
    update(localCustomer: LocalCustomer) {
      this.clearErrorBar()
      this.isLoading = true
      this.isLoadingButton = true

      const request = new UpdateCustomerByIdRequest()

      Object.assign(request, localCustomer)

      const id = localCustomer.id
      const version = localCustomer.version

      UpdateCustomerById(id, version, request)
        .then(this.updateThen)
        .catch(() => {
          this.showErrorBarWithScrollPageTop(this.$t('msg.error.default'))
        })
        .finally(() => {
          this.isLoading = false
          this.isLoadingButton = false
        })
    },
    /**
     * 顧客情報を削除
     */
    deleteRecord() {
      this.isLoading = true

      const id = this.customer.id
      const version = this.customer.version

      DeleteCustomerById(id, version)
        .then(() => {
          this.toListPage()
        })
        .catch((error) => {
          if (error.response.status === 400) {
            this.isDeletable = false
          } else {
            throw error
          }
        })
        .finally(() => {
          this.isLoading = false
        })
    },
  },
})
</script>
