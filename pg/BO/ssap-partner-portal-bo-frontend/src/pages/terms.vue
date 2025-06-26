<template>
  <RootTemPlate class="t-terms" :class="'fill-height align-start'">
    <v-container pt-15>
      <v-layout class="justify-center">
        <v-card tile width="1000px" height="440px">
          <div class="t-terms__body py-8 px-7">
            <h1 class="text-h5 font-weight-bold text-center">
              {{ $t('top.pages.terms.title') }}
            </h1>
            <p class="text-body-2 pt-8 mb-0">
              {{ $t('top.pages.terms.lead') }}
            </p>
            <section
              v-for="(item, index) in $t('top.pages.terms.article')"
              :key="index"
              class="t-terms__section"
            >
              <h2 class="text-h6 font-weight-bold">
                {{ item.title }}
              </h2>
              <v-divider class="t-terms__divider" />
              <p class="text-body-2">
                {{ item.text }}
              </p>
            </section>
          </div>
        </v-card>
      </v-layout>
      <v-layout class="justify-center">
        <Checkbox
          v-model="checkbox"
          class="t-terms__checkbox mt-9"
          label="利用規約に同意する"
          value="利用規約に同意する"
          hide-details
        />
      </v-layout>
      <v-layout class="justify-center" pt-6>
        <Button x-large color="primary" width="400px">
          <!-- {{ $t('common.button.yes') }} -->
          利用規約に同意して開始する
        </Button>
      </v-layout>
    </v-container>
  </RootTemPlate>
</template>

<script lang="ts">
import BasePage from '~/common/BasePage'
import RootTemPlate from '~/components/common/bases/RootTemplate.vue'
import { Button, Checkbox } from '~/components/common/atoms/index'

export default BasePage.extend({
  name: 'Terms',
  components: {
    RootTemPlate,
    Button,
    Checkbox,
  },
  head() {
    return {
      title: this.$t('top.pages.terms.title') as string,
    }
  },
  data() {
    return {
      checkbox: false,
    }
  },
})
</script>

<style lang="scss">
.t-terms__body {
  overflow-y: auto;
  height: 100%;
}
.t-terms__section {
  padding-top: 40px;
}
.t-terms__divider {
  border-width: 2px 0 0 0;
  margin: 5px 0 15px;
}
.t-terms__checkbox {
  .v-label {
    font-size: 18px;
    font-weight: bold;
    color: $c-black;
  }
  .v-input--selection-controls {
    margin-top: 0;
    padding-top: 0;
  }
}
</style>
