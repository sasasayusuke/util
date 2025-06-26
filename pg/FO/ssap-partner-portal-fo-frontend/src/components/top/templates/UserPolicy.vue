<template>
  <RootTemplate fill-height>
    <UserPolicyCard
      :user="user"
      :is-editing="isEditing"
      :is-loading="isLoading"
      @click:positive="onClickPositive"
    />
  </RootTemplate>
</template>

<script lang="ts">
import UserPolicyCard, {
  LocalUser,
} from '../../top/organisms/UserPolicyCard.vue'
import CommonDetail from '~/components/common/templates/CommonDetailTemplate.vue'
import { PropType } from '~/common/BaseComponent'
import RootTemplate from '~/components/common/bases/RootTemplate.vue'

import { PatchUserByMine, PatchUserByMineRequest } from '~/models/User'
import { meStore } from '~/store'
import { ENUM_USER_ROLE } from '~/types/User'

export default CommonDetail.extend({
  components: {
    UserPolicyCard,
    RootTemplate,
  },
  props: {
    /**
     * ユーザーの利用規約同意情報
     */
    user: {
      type: Object as PropType<PatchUserByMineRequest>,
      required: true,
    },
  },
  data() {
    return {
      isEditing: true,
    }
  },
  methods: {
    /**
     * 利用規約同意情報の更新
     */
    update(localUser: LocalUser) {
      this.clearErrorBar()
      this.isLoading = true

      const request = new PatchUserByMineRequest()

      Object.assign(request, localUser)

      PatchUserByMine(request)
        .then(() => {
          this.isLoading = false
          if (
            meStore.role === ENUM_USER_ROLE.APT ||
            meStore.role === ENUM_USER_ROLE.SOLVER_STAFF
          ) {
            this.$router.push(`/solver/menu`)
          } else {
            this.$router.push(`/home`)
          }
        })
        .catch(() => {
          this.isLoading = false
          this.showErrorBarWithScrollPageTop(this.$t('msg.error.default'))
        })
    },
  },
})
</script>
