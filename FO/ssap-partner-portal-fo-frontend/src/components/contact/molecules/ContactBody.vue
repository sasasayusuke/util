<template>
  <v-row no-gutters>
    <v-col cols="auto">
      <section
        v-for="(item, index) in $t('contact.pages.index.body')"
        :key="index"
        class="mt-10"
      >
        <Title class="o-ContactContainer__sub-title" style-set="contactSub">{{
          item.title
        }}</Title>
        <template v-if="containsUrl(item.text)">
          <p class="o-ContactContainer__text">
            {{ prefix(item.text) }}
            (<nuxt-link
              :to="url(item.text)"
              :href="url(item.text)"
              target="_blank"
              >{{ url(item.text) }}</nuxt-link
            >)
            {{ suffix(item.text) }}
          </p>
        </template>
        <template v-else>
          <p class="o-ContactContainer__text">
            {{ item.text }}
          </p>
        </template>
        <template v-if="item.list">
          <ul class="o-ContactContainer__list">
            <li
              v-for="(listItem, listIndex) in item.list"
              :key="listIndex"
              class="o-ContactContainer__list__item"
            >
              {{ listItem }}
            </li>
          </ul>
        </template>
      </section>
    </v-col>
  </v-row>
</template>

<script lang="ts">
import BaseComponent from '~/common/BaseComponent'
import { Title } from '~/components/common/atoms'

export default BaseComponent.extend({
  name: 'ContactBody',
  components: {
    Title,
  },
  methods: {
    containsUrl(text: string) {
      return text.match(/(http|https):\/\//)
    },
    prefix(text: string) {
      return text.split(/(（|\()/)[0]
    },
    suffix(text: string) {
      return text.split(/(）|\))/).slice(-1)[0]
    },
    url(text: string) {
      const result1 = text.split(/(（|\()/).slice(-1)[0]
      const result2 = result1.split(/(）|\))/)[0]
      return result2
    },
  },
})
</script>

<style lang="scss" scoped>
.o-ContactContainer__text {
  margin: 8px 0 0 0;
  a {
    color: $c-primary-dark;
  }
}
.o-ContactContainer__list {
  margin-top: 0.5em;
  padding: 0;
}
.o-ContactContainer__list__item {
  list-style: none;
  padding-left: 1em;
  text-indent: -1em;
  &::before {
    display: inline-block;
    content: '・';
    width: 1em;
    text-indent: 0;
  }
}
</style>
