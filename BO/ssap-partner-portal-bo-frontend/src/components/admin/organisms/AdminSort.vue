<template>
  <CommonSort @sort="$emit('sort')" @clear="$emit('clear')">
    <AdminSortInput
      :params="params"
      :is-loading="isLoading"
      v-on="$listeners"
    />
  </CommonSort>
</template>

<script lang="ts">
import AdminSortInput from '~/components/admin/molecules/AdminSortInput.vue'
import BaseComponent from '~/common/BaseComponent'
import CommonSort from '~/components/common/organisms/CommonSort.vue'

export class AdminSearchParams {
  name = ''
  email = ''
}

export default BaseComponent.extend({
  name: 'AdminSort',
  components: {
    CommonSort,
    AdminSortInput,
  },
  props: {
    /**
     * 管理者ユーザー一覧画面でソートを行うための条件
     */
    params: {
      type: Object,
      required: true,
    },
    /**
     * ローディング中か否かを示す
     */
    isLoading: {
      type: Boolean,
      required: true,
    },
  },
})
</script>
,
