<template>
  <div class="m-common-anchor">
    <Sheet class="d-flex justify-center pt-10" width="100%">
      <Button
        style-set="large-tertiary"
        outlined
        width="160"
        :to="toProjectList"
      >
        {{ $t('common.button.backToList') }}
      </Button>
    </Sheet>
  </div>
</template>

<script lang="ts">
import { Button, Sheet } from '~/components/common/atoms/index'
import BaseComponent from '~/common/BaseComponent'
import { masterKarteListUrlStore } from '~/store'

export default BaseComponent.extend({
  name: 'ProjectFooter',
  components: {
    Button,
    Sheet,
  },
  computed: {
    /**
     * 案件一覧へのリンク
     */
    toProjectList() {
      const masterKarteListParams = masterKarteListUrlStore.params
        ? new URLSearchParams(
            JSON.parse(masterKarteListUrlStore.params)
          ).toString()
        : ''
      return `/project/list?${masterKarteListParams}`
    },
  },
  props: {},
})
</script>
