<template>
  <!-- <v-card>
    <slot />
    <v-card-actions>
    </v-card-actions>
  </v-card> -->
  <tbody class="m-clickable-card" @click="$router.push(to)">
    <tr>
      <th
        v-if="name"
        class="m-clickable-card__name text-primary-dark font-size-normal"
      >
        {{ name }}
      </th>
      <td v-for="(item, index) in content" :key="index" class="font-size-small">
        {{ item }}
      </td>
      <td v-if="last" class="m-clickable-card__last">
        {{ last }}
      </td>
      <td class="text-right">
        <Icon size="16px">icon-org-arrow-right</Icon>
      </td>
    </tr>
  </tbody>
</template>

<script lang="ts">
import BaseComponent from '~/common/BaseComponent'
import { Icon } from '~/components/common/atoms/index'

export default BaseComponent.extend({
  components: {
    Icon,
  },
  props: {
    to: {
      type: String,
      default: '',
    },
    name: {
      type: String,
    },
    content: {
      type: Array,
    },
    last: {
      type: String,
    },
    surveys: {
      type: Array,
    },
  },
})
</script>

<style lang="scss" scoped>
.m-clickable-card {
  tr {
    transition-property: background-color;
    transition-duration: 0.2s;
    &:hover,
    &:focus {
      background-color: $c-primary-8 !important;
      cursor: pointer;
    }
  }
  th,
  td {
    padding: 0 15px !important;
  }
  &:nth-child(n + 2) {
    th,
    td {
      border-top: 1px solid $c-gray-line;
    }
  }
  td:nth-of-type(-n + 2) {
    width: 20%;
  }
  td:nth-of-type(3) {
    width: 27%;
  }
  td:last-of-type {
    width: 3%;
  }
}
.m-clickable-card__name {
  width: 30%;
}

.m-clickable-card__last {
  @include fontSize('xsmall');
  text-align: left;
  width: 240px;
}
</style>
