<template>
  <v-row class="o-UserPolicyTextContainer" justify="center">
    <v-col cols="auto">
      <Sheet
        class="py-12 px-7 mt-7"
        width="900"
        elevation="3"
        rounded
        color="white"
      >
        <v-container fluid pa-0>
          <v-row no-gutters class="mb-10">
            <v-col>
              <h1 class="o-UserPolicyTextContainer__title">
                {{ $t('top.pages.terms.title') }}
              </h1>
            </v-col>
          </v-row>
          <template v-for="(item, index) in $t('top.pages.terms.lead')">
            <v-row :key="index" no-gutters class="mb-2">
              <v-col>
                <p class="o-UserPolicyTextContainer__lead">
                  {{ item }}
                </p>
              </v-col>
            </v-row>
          </template>
          <template v-for="(item, index) in $t('top.pages.terms.article')">
            <v-row :key="index" no-gutters :class="{ 'mt-5': index }">
              <v-col>
                <h2 class="o-UserPolicyTextContainer__sub-title">
                  {{ index + 1 }}. {{ item.title }}
                </h2>
                <p v-if="item.text" class="o-UserPolicyTextContainer__text">
                  {{ item.text }}
                  <a v-if="item.link" :href="item.href" target="_blank">
                    {{ item.link }}
                  </a>
                  <template v-if="item.text2">
                    {{ item.text2 }}
                  </template>
                </p>
                <ol v-if="item.list" class="o-UserPolicyTextContainer__list">
                  <template v-for="(list_item, list_index) in item.list">
                    <li
                      :key="list_index"
                      :class="{
                        'is-long': item.list.length >= 10,
                        'is-2digits': index >= 10,
                      }"
                    >
                      <span>{{ index + 1 }}-{{ list_index + 1 }}.</span
                      >{{ list_item }}
                      <ol
                        v-if="item.childList"
                        class="o-UserPolicyTextContainer__child-list"
                      >
                        <template
                          v-for="(child_list_item, child_list_index) in item
                            .childList[list_index]"
                        >
                          <li
                            :key="child_list_index"
                            :class="{
                              'is-2digits': child_list_index >= 9,
                            }"
                          >
                            <span>({{ child_list_index + 1 }})</span
                            >{{ child_list_item }}
                          </li>
                        </template>
                      </ol>
                    </li>
                  </template>
                </ol>
                <p v-if="item.contact" class="o-UserPolicyTextContainer__text">
                  {{ $t('top.pages.terms.contact') }}<br />
                  <a :href="item.contact" target="_blank">{{ item.contact }}</a>
                </p>
              </v-col>
            </v-row>
          </template>
          <v-row no-gutters class="mt-6">
            <v-col class="font-size-small">
              {{ $t('top.pages.terms.lastUpdate') }}
            </v-col>
          </v-row>
        </v-container>
      </Sheet>
    </v-col>
  </v-row>
</template>

<script lang="ts">
import { Sheet } from '../../common/atoms/index'
import BaseComponent from '~/common/BaseComponent'

export default BaseComponent.extend({
  components: {
    Sheet,
  },
})
</script>

<style lang="scss" scoped>
.o-UserPolicyTextContainer {
  ol {
    list-style: none;
    @include fontSize('small');
  }
}
.o-UserPolicyTextContainer__title {
  text-align: center;
  font-size: 1.5rem;
}
.o-UserPolicyTextContainer__lead {
  @include fontSize('small');
}
.o-UserPolicyTextContainer__sub-title {
  @include fontSize('normal');
  margin-bottom: 12px;
}
.o-UserPolicyTextContainer__text {
  @include fontSize('small');
  a {
    color: $c-primary-dark;
    transition-duration: 0.2s;
    text-decoration: underline;
    &:hover,
    &:focus {
      color: $c-primary-over;
    }
  }
}
.o-UserPolicyTextContainer__list {
  padding-left: 0;
  > li {
    > span {
      display: inline-block;
      width: 2em;
      text-indent: 0;
    }
    text-indent: -2em;
    padding-left: 2em;
    &.is-long {
      > span {
        width: 2.5em;
      }
      text-indent: -2.5em;
      padding-left: 2.5em;
      &.is-2digits {
        text-indent: -3em;
        padding-left: 3em;
        > span {
          width: 3em;
        }
      }
    }
    &.is-2digits {
      text-indent: -2.5em;
      padding-left: 2.5em;
      > span {
        width: 2.5em;
      }
    }
  }
}
.o-UserPolicyTextContainer__child-list {
  padding-left: 0;
  > li {
    > span {
      display: inline-block;
      width: 1.5em;
      text-indent: 0;
    }
    text-indent: -1.5em;
    padding-left: 1.5em;
    &.is-2digits {
      margin-left: -0.5em;
      text-indent: -2em;
      padding-left: 2em;
      > span {
        width: 2em;
      }
    }
  }
}
</style>
