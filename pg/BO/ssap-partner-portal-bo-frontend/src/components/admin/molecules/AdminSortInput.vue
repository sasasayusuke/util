<template>
  <Sheet class="d-flex align-center pr-7">
    <p class="font-size-small mb-0 mr-2">
      {{ $t('admin.pages.index.sort_input.name') }}
    </p>
    <Sheet width="240" mr-7>
      <TextField
        :value="params.name"
        :aria-label="$t('admin.pages.index.sort_input.name')"
        outlined
        dense
        @input="$emit('update', 'name', $event)"
      />
    </Sheet>
    <p class="font-size-small mb-0 mr-2 ml-7">
      {{ $t('admin.pages.index.sort_input.mail') }}
    </p>
    <Sheet width="240">
      <TextField
        :value="params.email"
        :aria-label="$t('admin.pages.index.sort_input.mail')"
        outlined
        dense
        @input="$emit('update', 'email', $event)"
      />
    </Sheet>
  </Sheet>
</template>

<script lang="ts">
import BaseComponent from '~/common/BaseComponent'
import { TextField, Sheet } from '~/components/common/atoms'

export default BaseComponent.extend({
  name: 'AdminSortInput',
  components: {
    TextField,
    Sheet,
  },
  props: {
    /**
     * ソートの条件
     */
    params: {
      type: Object,
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
})
</script>
