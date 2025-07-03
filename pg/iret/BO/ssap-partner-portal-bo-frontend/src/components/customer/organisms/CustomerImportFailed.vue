<template>
  <CommonContainer
    :title="title"
    :is-editing="isEditing"
    :is-register="isRegister"
    hx="h1"
    is-hide-header-button
    is-hide-button1
  >
    <Sheet class="pa-8 pb-0">
      <Title hx="h2" style-set="detail">
        <Icon color="btn_error" size="24" class="mt-n1">mdi-close-circle</Icon>
        {{ $t('project.pages.import.error.lead') }}
      </Title>
      <div v-if="!isAbnormalError" class="o-project-list-table is-import">
        <Paragraph style-set="error-text">
          <strong>
            {{ errors.length }}
            {{ $t('project.pages.import.error.text1') }}
          </strong>
          {{ $t('project.pages.import.error.text2') }}
        </Paragraph>
        <ErrorList :errors="errors" />
      </div>
      <div v-else class="o-project-list-table is-import">
        <ErrorList :errors="errors" />
      </div>
    </Sheet>
    <template #footerButton>
      <Button
        outlined
        style-set="large-tertiary"
        width="160"
        to="/customer/import"
      >
        {{ $t('common.button.back') }}
      </Button>
    </template>
  </CommonContainer>
</template>

<script lang="ts">
import BaseComponent from '~/common/BaseComponent'
import type { PropType } from '~/common/BaseComponent'
import CommonContainer from '~/components/common/organisms/CommonContainer.vue'
import ErrorList from '~/components/common/molecules/ErrorList.vue'
import { ImportedCustomer } from '@/types/Customer'

import {
  Sheet,
  Title,
  Paragraph,
  Icon,
  Button,
} from '~/components/common/atoms/index'

export default BaseComponent.extend({
  created() {
    //一括登録機能は利用不可のため、404画面にリダイレクトする
    this.$router.push('/404')
  },
  name: 'CustomerImportSelectFile',
  components: {
    CommonContainer,
    Sheet,
    Title,
    Paragraph,
    ErrorList,
    Icon,
    Button,
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
     * エラーステータス
     */
    errorStatus: {
      type: Number,
    },
    /**
     * エラー詳細情報
     */
    errorDetail: {
      type: String,
      default: '',
    },
  },
  data() {
    return {
      isEditing: false,
      isRegister: true,
      isAbnormalError: false,
      title: this.$t('project.pages.import.name'),
    }
  },
  computed: {
    /**
     * エラー情報を返す
     * @returns エラー情報
     */
    errors() {
      const rtn: string[] = []
      if (this.errorStatus === 999) {
        this.isAbnormalError = true
        rtn.push(
          this.$t('customer.pages.import.error.errorMessageUnknown') as string
        )
      } else if (this.errorStatus === 400) {
        this.isAbnormalError = true
        if (this.errorDetail === 'File not found.') {
          rtn.push(
            this.$t('customer.pages.import.error.errorFileNotFound') as string
          )
        } else if (this.errorDetail === 'Illegal CSV format.') {
          rtn.push(
            this.$t('customer.pages.import.error.errorIllegalFormat') as string
          )
        } else if (/^Over\s500\slines/.test(this.errorDetail)) {
          rtn.push(
            this.$t('customer.pages.import.error.errorOverLines') as string
          )
        } else if (this.errorDetail === "cp932 codec can't decode.") {
          rtn.push(
            this.$t('customer.pages.import.error.errorIllegalCharset') as string
          )
        }
      } else {
        this.customers.forEach((customer, index) => {
          if (customer.errorMessage && customer.errorMessage.length !== 0) {
            rtn.push(
              this.$t('customer.pages.import.error.errorMessageTemplate', {
                index: index + 2,
                message: customer.errorMessage,
              }) as string
            )
          }
        })
      }
      return rtn
    },
  },
})
</script>

<style lang="scss"></style>
