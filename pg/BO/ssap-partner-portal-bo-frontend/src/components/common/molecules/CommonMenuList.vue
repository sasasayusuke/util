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
      <Sheet
        v-if="isShowState"
        class="d-flex pa-3 font-size-small font-weight-bold"
        color="#F0F0F0"
      >
        {{ $store.state.me.name }}
        （{{ translatedRoles }}）
      </Sheet>
      <v-list class="m-menu-list__list">
        <template v-for="(item, index) in items">
          <v-list-item
            :key="index"
            :to="item.to"
            :href="item.href"
            :target="item.otherPage ? '_blank' : '_self'"
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
    isShowState: {
      type: Boolean,
      default: false,
    },
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
    // ロールを日本語に変換し、複数ロールの場合は「/」で区切る
    translatedRoles() {
      const rolesList = this.$t('admin.row.roles.list')
      const translatedRoles: string[] = []

      this.$store.state.me.roles.forEach((currentRole: string) => {
        // @ts-ignore
        rolesList.forEach((role: any) => {
          if (role.value === currentRole) {
            translatedRoles.push(role.name)
          }
        })
      })
      return translatedRoles.join('／')
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
