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
      <Button
        style-set="group-large"
        :outlined="isCurrentProgram"
        width="50%"
        @click="$emit('clickTab', false)"
      >
        {{ $t('master-karte.pages.detail.main-tabs.next') }}
      </Button>
    </GroupButton>
  </main>
</template>

<script lang="ts">
import { PropType } from '~/common/BaseComponent'
import BasePage from '~/common/BasePage'
import { Button } from '~/components/common/atoms'
import GroupButton from '~/components/common/molecules/GroupButton.vue'
import DetailContainer from '~/components/common/organisms/DetailContainer.vue'
import { GetMasterKarteByIdResponseClass } from '~/models/MasterKarte'
import { GetProjectByIdResponse } from '~/models/Project'
import { UserListItem } from '~/models/User'

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
    masterKarteProject: {
      type: Object as PropType<GetMasterKarteByIdResponseClass>,
    },
    isCurrentProgram: {
      type: Boolean,
      default: true,
    },
    project: {
      type: Object as PropType<GetProjectByIdResponse>,
    },
    userInfo: {
      type: Object as PropType<UserListItem>,
    },
  },
})
</script>
