<template>
  <Sheet
    class="o-ContactContainer mx-auto mt-7"
    elevation="3"
    rounded
    color="white"
    width="900"
  >
    <v-container fluid px-8 py-10>
      <ContactHeader />
      <ContactBody />
      <ContactFooter />
    </v-container>
  </Sheet>
</template>

<script lang="ts">
import BaseComponent from '~/common/BaseComponent'
import ContactHeader from '~/components/contact/molecules/ContactHeader.vue'
import ContactBody from '~/components/contact/molecules/ContactBody.vue'
import ContactFooter from '~/components/contact/molecules/ContactFooter.vue'
import { Sheet, Title, Button } from '~/components/common/atoms'

export default BaseComponent.extend({
  name: 'ContactContainer',
  components: {
    Sheet,
    Title,
    Button,
    ContactHeader,
    ContactBody,
    ContactFooter,
  },
})
</script>
