<template>
  <CommonSort @sort="$emit('sort')" @clear="$emit('clear')">
    <UserSortInput :param="param" :is-loading="isLoading" v-on="$listeners" />
  </CommonSort>
</template>

<script lang="ts">
import UserSortInput from '../molecules/UserSortInput.vue'
import BaseComponent from '~/common/BaseComponent'
import CommonSort from '~/components/common/organisms/CommonSort.vue'

export class UserSearchParam {
  name = ''
}

export default BaseComponent.extend({
  name: 'UserSort',
  components: {
    CommonSort,
    UserSortInput,
  },
  props: {
    /** ソートの条件 */
    param: {
      type: Object,
      required: true,
    },
    isLoading: {
      type: Boolean,
      required: true,
    },
  },
})
</script>
