<template>
  <TemplateUserPolicy :user="user" />
</template>

<script lang="ts">
import TemplateUserPolicy from '~/components/top/templates/UserPolicy.vue'
import BasePage from '~/common/BasePage'
import { PatchUserByMineRequest } from '~/models/User'
import { meStore } from '~/store'
import { ENUM_USER_ROLE } from '~/types/User'

export default BasePage.extend({
  layout: 'BeforeLoginFillHeight',
  components: {
    TemplateUserPolicy,
  },
  middleware: ['roleCheck'],
  head() {
    return {
      title: this.$t('top.pages.user_policy.title') as string,
    }
  },
  data() {
    return {
      user: new PatchUserByMineRequest(),
    }
  },
  created() {
    if (meStore.agreed === true) {
      if (
        meStore.role === ENUM_USER_ROLE.APT ||
        meStore.role === ENUM_USER_ROLE.SOLVER_STAFF
      ) {
        this.$router.push(`/solver/menu`)
      } else {
        this.$router.push(`/home`)
      }
    }
  },
})
</script>
