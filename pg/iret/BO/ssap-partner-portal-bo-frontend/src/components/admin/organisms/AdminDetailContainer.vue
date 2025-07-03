<template>
  <DetailContainer
    :title="title"
    :is-editing="isEditing"
    :is-valid="isValidWithChange"
    :is-loading-button="isLoadingButton"
    hx="h1"
    @click:positive="$emit('click:positive', localAdmin)"
    @click:negative="$emit('click:negative')"
  >
    <AdminDetailRows
      v-model="isValid"
      :admin="admin"
      :is-editing="isEditing"
      :is-creating="isCreating"
      :is-loading="isLoading"
      @update="update('localAdmin', $event)"
    />
  </DetailContainer>
</template>

<script lang="ts">
import type { LocaleMessages } from 'vue-i18n/types'
import { PropType } from '~/common/BaseComponent'
import AdminDetailRows, {
  LocalAdmin,
} from '~/components/admin/molecules/AdminDetailRows.vue'
import CommonDetailContainer from '~/components/common/organisms/CommonDetailContainer.vue'
import DetailContainer from '~/components/common/organisms/DetailContainer.vue'
import { GetAdminByIdResponse } from '~/models/Admin'

export { LocalAdmin }

export default CommonDetailContainer.extend({
  components: {
    AdminDetailRows,
    DetailContainer,
  },
  props: {
    /**
     * Back Officeにログイン可能な管理ユーザー
     */
    admin: {
      type: Object as PropType<GetAdminByIdResponse>,
    },
    /** 管理ユーザー情報を登録/更新中か */
    isLoadingButton: {
      type: Boolean,
      default: false,
    },
  },
  data() {
    return {
      localAdmin: new LocalAdmin(),
    }
  },
  computed: {
    /**
     * タイトルを返す
     * @returns タイトル
     */
    title(): string | LocaleMessages {
      return this.$t('admin.pages.' + this.mode + '.name')
    },
  },
})
</script>
