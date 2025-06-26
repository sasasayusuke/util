<template>
  <RootTemplate>
    <UserPolicyTextContainer />
  </RootTemplate>
</template>

<script lang="ts">
import UserPolicyTextContainer from '../../top/organisms/UserPolicyTextContainer.vue'
import BaseComponent from '~/common/BaseComponent'
import RootTemplate from '~/components/common/bases/RootTemplate.vue'

export default BaseComponent.extend({
  components: {
    UserPolicyTextContainer,
    RootTemplate,
  },
})
</script>
