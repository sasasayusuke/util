<template>
  <v-menu v-bind="attributes" :value="menu">
    <template #activator="{ on, attrs }">
      <Button class="px-0" text color="btn_primary" v-on="on">
        {{ name }}
        <Icon class="arrow" v-bind="attrs" size="14">mdi-menu-down</Icon>
      </Button>
    </template>
    <ListGroup :values="listValues" :links="listLinks" />
  </v-menu>
  <!-- <v-menu v-bind="attributes" v-on="$listeners">
    <template #activator="{ on }">
      <Button @onClick="on" />
    </template>
    <v-card>
      <v-list>
        <v-list-item v-for="(values, index) in values" :key="index">
          <v-list-item-content>
            <v-list-item-title>
              <Button :style-set="button.styleSet" @click="button.onClick()">
                {{ values }}
              </Button>
            </v-list-item-title>
          </v-list-item-content>
        </v-list-item>
      </v-list>
    </v-card>
  </v-menu> -->
</template>
<script lang="ts">
import WrapperComponent, { AttributeSet } from '../bases/WrapperComponent'
import { Icon, ListGroup, Button } from '~/components/common/atoms/index'

const ATTRIBUTE_SET: AttributeSet = {
  default: {},
}

export default WrapperComponent.extend({
  model: {
    prop: 'value',
    event: 'input',
  },
  components: { Icon, ListGroup, Button },
  props: {
    name: {
      type: String,
    },
    listValues: {
      type: Array,
    },
    listLinks: {
      type: Array,
    },
    menu: {
      type: Boolean,
      default: false,
    },
    values: {
      type: Array,
      required: true,
    },
    buttons: {
      type: Array,
      default() {
        return []
      },
    },
  },
  data() {
    return {
      attributeSet: ATTRIBUTE_SET,
    }
  },
})
</script>

<style lang="scss" scoped>
.arrow {
  transform-origin: center center;
  transition-duration: 0.2s;
  &[aria-expanded='true'] {
    transform: rotate(-180deg);
  }
}
</style>
