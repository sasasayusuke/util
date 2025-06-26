<!--
  使用するときは、
  <CommonMenuList v-slot="{ on }">
    <Button icon v-on="on">
      <v-icon>mdi-dots-vertical</v-icon>
    </Button>
  </CommonMenuList>

  といった形で、アクティベーターを包んで
  - v-slot="{ on }"
  - v-on="on"
  の記述を足すこと
  -->

<template>
  <v-menu class="m-menu-list" bottom :offset-y="true">
    <template #activator="slotData">
      <slot v-bind="slotData" />
    </template>
    <v-card>
      <!-- <Sheet
        class="d-flex pa-3 font-size-small font-weight-bold"
        color="#F0F0F0"
      >
        {{ $store.state.me.name }}
        （{{ $store.state.me.role.join('／') }}）
      </Sheet> -->
      <Sheet
        class="d-flex pa-3 font-size-small font-weight-bold"
        color="#F0F0F0"
      >
        {{ $store.state.me.name }}
        <!-- 顧客以外のロールを氏名のあとに明示 -->
        <template v-if="$store.state.me.role !== 'customer'">
          （{{ translatedRole }}）
        </template>
      </Sheet>
      <v-list class="m-menu-list__list">
        <template v-for="(item, index) in items">
          <v-list-item
            :key="index"
            :to="item.to"
            :target="item.otherPage ? '_blank' : '_self'"
            :href="item.href"
            :ripple="false"
            @click="!item.otherPage ? item.to || item.onClick(item.name) : null"
          >
            <v-list-item-content>
              <v-list-item-title>
                {{ item.label }}
                <Icon v-if="item.otherPage === true" size="15" class="pl-5"
                  >icon-org-blank</Icon
                >
              </v-list-item-title>
            </v-list-item-content>
          </v-list-item>
        </template>
      </v-list>
    </v-card>
  </v-menu>
</template>

<script lang="ts">
import { Button, Icon, Sheet } from '../atoms/index'
import BaseComponent from '~/common/BaseComponent'

export default BaseComponent.extend({
  components: {
    Button,
    Icon,
    Sheet,
  },
  props: {
    items: {
      type: Array,
      required: true,
      default() {
        return [
          {
            /*
              label: 'ラベルのサンプル',
              name: 'sample',
              onClick() {},
              */
          },
        ]
      },
    },
  },
  computed: {
    // ロールを日本語に変換
    translatedRole(): string {
      const rolesList: any = this.$t('common.role')
      // @ts-ignore
      const currentRole = this.$store.state.me.role

      return rolesList[currentRole]
    },
  },
})
</script>

<style lang="scss" scoped>
.m-menu-list__list {
  @include list-item;
  .v-list-item__title {
    font-size: 0.875rem;
    display: flex;
    justify-content: space-between;
    align-items: center;
  }
}
</style>
