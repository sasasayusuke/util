<template>
  <Sheet class="d-flex align-center pr-7">
    <p class="font-size-small mb-0 mr-2">
      {{ $t('user.pages.index.sort_input.name') }}
    </p>
    <Sheet width="240" mr-7>
      <TextField
        v-model="param.name"
        :aria-label="$t('user.pages.index.sort_input.name')"
        outlined
        dense
        max-length="120"
        style-set="bgWhite"
      />
    </Sheet>
    <p class="font-size-small mb-0 mr-2 ml-7">
      {{ $t('user.pages.index.sort_input.mail') }}
    </p>
    <Sheet width="240">
      <TextField
        v-model="param.email"
        :aria-label="$t('user.pages.index.sort_input.mail')"
        outlined
        dense
        max-length="256"
        style-set="bgWhite"
      />
    </Sheet>
  </Sheet>
</template>

<script lang="ts">
import BaseComponent from '~/common/BaseComponent'
import { TextField, Sheet } from '~/components/common/atoms'

export default BaseComponent.extend({
  name: 'UserSortInput',
  components: {
    TextField,
    Sheet,
  },
  props: {
    /** ソート条件 */
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
