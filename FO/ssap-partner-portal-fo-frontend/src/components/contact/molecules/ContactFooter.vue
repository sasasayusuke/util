<template>
  <v-row class="m-ContactFooter" no-gutters justify="center">
    <v-col>
      <v-divider class="mt-10 mb-8" />
      <Title class="m-ContactFooter__title" style-set="contactSub">
        {{ $t('contact.pages.index.footer.title') }}
      </Title>
      <p class="m-ContactFooter__text-1">
        {{ $t('contact.pages.index.footer.text1') }}
      </p>
      <p class="m-ContactFooter__text-2">
        {{ $t('contact.pages.index.footer.text2') }}
      </p>
      <div class="d-flex justify-center pt-7">
        <Button to="/" style-set="large-tertiary" outlined width="160">{{
          $t('common.button.disagree')
        }}</Button>
        <Button
          href="https://sonygroup.my.salesforce-sites.com/sony/partnerportal_contact1"
          style-set="large-primary"
          class="ml-2"
          width="160"
          >{{ $t('common.button.agree') }}</Button
        >
      </div>
    </v-col>
  </v-row>
</template>

<script lang="ts">
import BaseComponent from '~/common/BaseComponent'
import { Title, Button } from '~/components/common/atoms'

export default BaseComponent.extend({
  name: '',
  components: {
    Title,
    Button,
  },
})
</script>

<style lang="scss" scoped>
.m-ContactFooter__title {
  text-align: center;
}
.m-ContactFooter__text-1 {
  text-align: center;
  margin: 12px 0 0 0;
  @include fontSize('small');
}
.m-ContactFooter__text-2 {
  text-align: center;
  color: $c-red;
  margin: 12px 0 0 0;
  @include fontSize('small');
}
</style>
