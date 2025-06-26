<template>
  <RootTemplate width="1200" class="t-man-hour pt-8 pb-10">
    <!-- ヘッダ -->
    <ListInPageHeader
      :level="1"
      class="type2 pb-4"
      :back-to-list="`/man-hour/supporter/${$route.params.year}/${$route.params.month}`"
    >
      {{ pageName }}
    </ListInPageHeader>
    <!-- 詳細コンテナ -->
    <ManHourContainer
      :is-loading="isLoading.manHourBySupporterUserId"
      :man-hour-by-supporter-user-id="manHourBySupporterUserId"
    />
  </RootTemplate>
</template>

<script lang="ts">
import BaseComponent, { PropType } from '~/common/BaseComponent'
import RootTemplate from '~/components/common/bases/RootTemplate.vue'
import ListInPageHeader from '~/components/common/organisms/ListInPageHeader.vue'
import ManHourContainer from '~/components/man-hour/organisms/ManHourContainer.vue'
import ManHourModal from '~/components/man-hour/organisms/ManHourModal.vue'
import { Button } from '~/components/common/atoms/index'
import Modal from '~/components/common/molecules/Modal.vue'
export interface isLoading {
  manHourBySupporterUserId: boolean
}
export default BaseComponent.extend({
  name: 'MonthlyManHourBySupporterDetail',
  components: {
    RootTemplate,
    ListInPageHeader,
    ManHourContainer,
    ManHourModal,
    Button,
    Modal,
  },
  props: {
    /** 支援者単位での支援工数情報 */
    manHourBySupporterUserId: {
      type: Object,
    },
    /** 読み込み中判定 */
    isLoading: {
      type: Object as PropType<isLoading>,
    },
  },
  computed: {
    /**
     * ページ名(支援者名)を返す
     * @return 支援者名
     */
    pageName(): string {
      return this.manHourBySupporterUserId.supporterName
    },
  },
  data() {
    return {
      id: '',
      isModalOpen: false,
    }
  },
  created() {
    this.id = this.$route.params.id
  },
})
</script>
