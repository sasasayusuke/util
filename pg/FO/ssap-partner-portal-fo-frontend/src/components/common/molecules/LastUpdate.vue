<template>
  <v-container>
    <v-container pa-0 fluid>
      <v-row class="m-last-update" justify="end">
        <v-col v-if="supportTerm">
          <v-row v-if="showTerm">
            <v-col class="m-last-update__term">
              {{ $t('common.lastUpdate.term') }}
            </v-col>
            <v-col class="m-last-update__data">{{ term }}</v-col>
          </v-row>
        </v-col>
        <v-row
          v-if="
            (sourceScreen == 'KarteDetail' ||
              sourceScreen == 'MasterKarteCurrent' ||
              sourceScreen == 'MasterKarteNext') &&
            !isCustomer
          "
          class="m-last-update__scope"
        >
          <v-col align-self="center" class="pr-0">
            <Icon style-set="primary" size="22">mdi-eye</Icon>
          </v-col>
          <v-col align-self="center" class="pl-1">
            <a
              href="#"
              style="color: green"
              @click.prevent="isModalOpen = true"
            >
              {{ $t('common.lastUpdate.scope') }}
            </a>
          </v-col>
        </v-row>
        <v-col class="m-last-update__title">
          {{ $t('common.lastUpdate.user') }}
        </v-col>
        <v-col class="m-last-update__data">
          {{ date && user ? user : 'ー' }}
        </v-col>
        <v-col class="m-last-update__title">
          {{ $t('common.lastUpdate.date') }}
        </v-col>
        <v-col class="m-last-update__data">
          {{
            date && user ? formatDate(new Date(date), 'Y/MM/DD hh:mm') : 'ー'
          }}
        </v-col>
      </v-row>
    </v-container>
    <KarteScopeModal
      :is-modal-open="isModalOpen"
      :source-screen="sourceScreen"
      :is-public="isPublic"
      @closeModal="closeModal"
    />
  </v-container>
</template>

<script lang="ts">
import BaseComponent from '~/common/BaseComponent'
import RootTemplate from '~/components/common/bases/RootTemplate.vue'
import { Button, Sheet, Icon } from '~/components/common/atoms/index'

export default BaseComponent.extend({
  components: {
    RootTemplate,
    Button,
    Sheet,
    Icon,
  },
  props: {
    term: {
      type: String,
    },
    showTerm: {
      type: Boolean,
      default: true,
    },
    user: {
      type: String,
      required: true,
    },
    date: {
      type: String,
      required: true,
    },
    supportTerm: {
      type: Boolean,
    },
    sourceScreen: {
      type: String,
      default: '',
    },
    isPublic: {
      type: Boolean,
      default: true,
    },
    isCustomer: {
      type: Boolean,
      default: true,
    },
  },
  data() {
    return {
      isModalOpen: false,
    }
  },
  methods: {
    /**
     * モーダルを閉じる時の処理
     */
    closeModal() {
      this.isModalOpen = false
    },
  },
})
</script>

<style lang="scss" scoped>
.m-last-update__unit {
  justify-content: flex-end;
}
.m-last-update__term {
  font-size: 0.75rem;
  font-weight: bold;
  flex-grow: 0;
  white-space: nowrap;
}
.m-last-update__title {
  font-size: 0.75rem;
  font-weight: bold;
  flex-grow: 0;
  white-space: nowrap;
}
.m-last-update__data {
  font-size: 0.75rem;
  flex-grow: 0;
  white-space: nowrap;
  padding-left: 0;
}
.m-last-update__scope {
  font-size: 0.75rem;
  flex-grow: 0;
  white-space: nowrap;
  padding-right: 12px;
}
</style>
