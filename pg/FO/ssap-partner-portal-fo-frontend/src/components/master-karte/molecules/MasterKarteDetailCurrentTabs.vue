<template>
  <main>
    <GroupButton class="mb-10" style="width: 40%; margin: 0 auto">
      <!-- 当期支援 -->
      <Button
        style-set="group-large"
        :outlined="!isCurrentProgram"
        width="50%"
        @click="$emit('clickTab', true)"
      >
        {{ $t('master-karte.pages.detail.main-tabs.current') }}
      </Button>

      <!-- 次期支援 -->
      <!-- 次期支援ボタン活性 -->
      <Button
        v-show="nextProgramReadRoles"
        style-set="group-large"
        :outlined="isCurrentProgram"
        width="50%"
        @click="$emit('clickTab', false)"
      >
        {{ $t('master-karte.pages.detail.main-tabs.next') }}
      </Button>

      <!-- 次期支援ボタン非活性 -->
      <Button
        v-show="!nextProgramReadRoles"
        style-set="group-large"
        :outlined="isCurrentProgram"
        disabled
        style="
          background: #8f8f8f !important;
          color: #fff !important;
          border: none;
        "
        width="50%"
        @click="$emit('clickTab', false)"
      >
        {{ $t('master-karte.pages.detail.main-tabs.next') }}
      </Button>
    </GroupButton>
  </main>
</template>

<script lang="ts">
import BasePage from '~/common/BasePage'
import { Button } from '~/components/common/atoms'
import GroupButton from '~/components/common/molecules/GroupButton.vue'
import DetailContainer from '~/components/common/organisms/DetailContainer.vue'

export default BasePage.extend({
  name: 'MasterKarteListNpfProjectId',
  middleware: ['roleCheck'],
  components: { DetailContainer, GroupButton, Button },
  head() {
    return {
      title: this.$t('master-karte.pages.index.name') as string,
    }
  },
  props: {
    isCurrentProgram: {
      type: Boolean,
      default: true,
    },
    masterKarteProject: {
      type: Object,
    },
    nextProgramReadRoles: {
      type: Boolean,
      default: false,
    },
  },
})
</script>
