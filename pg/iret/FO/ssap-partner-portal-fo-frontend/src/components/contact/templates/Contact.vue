<template>
  <RootTemPlate>
    <ContactContainer />
  </RootTemPlate>
</template>

<script lang="ts">
import BaseComponent from '~/common/BaseComponent'
import RootTemPlate from '~/components/common/bases/RootTemplate.vue'
import ContactContainer from '~/components/contact/organisms/ContactContainer.vue'

export default BaseComponent.extend({
  name: 'ContactTemplate',
  components: {
    RootTemPlate,
    ContactContainer,
  },
  props: {},
  data() {
    return {}
  },
  methods: {},
})
</script>
