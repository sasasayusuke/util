<template>
  <v-menu class="m-table-actions" v-bind="attributes" :value="menu">
    <template #activator="{ on, attrs }">
      <Button
        :aria-label="$t('common.button.openMenu')"
        class="px-0"
        text
        color="#333"
        v-on="on"
      >
        <Icon v-bind="attrs" size="20">mdi-dots-vertical</Icon>
      </Button>
    </template>
    <v-list
      v-if="surveyQuestionTable"
      class="m-table-actions__body is-surveyQuestion"
    >
      <v-list-item @click="onUpdate">
        {{ $t('common.button.updateOrReffer') }}
      </v-list-item>
      <v-list-item v-if="item === 'draft'" @click="onDelete">
        {{ $t('common.button.updateOperation') }}
      </v-list-item>
      <v-list-item @click="onDelete">
        {{ $t('common.button.copyAndCreateNew') }}
      </v-list-item>
    </v-list>
    <v-list v-else class="m-table-actions__body">
      <v-list-item @click="onUpdate">
        {{ $t('common.button.update') }}
      </v-list-item>
      <v-list-item v-if="showDelete" @click="onDelete">
        {{ $t('common.button.delete') }}
      </v-list-item>
    </v-list>
  </v-menu>
</template>
<script lang="ts">
import WrapperComponent, { AttributeSet } from '../bases/WrapperComponent'
import { Icon, Button } from '~/components/common/atoms/index'

const ATTRIBUTE_SET: AttributeSet = {
  default: {},
}

export default WrapperComponent.extend({
  model: {
    prop: 'value',
    event: 'input',
  },
  components: { Icon, Button },
  props: {
    menu: {
      type: Boolean,
      default: false,
    },
    surveyQuestionTable: {
      type: Boolean,
      default: false,
    },
    item: {
      type: String,
    },
    showDelete: {
      type: Boolean,
      default: true,
    },
  },
  data() {
    return {
      attributeSet: ATTRIBUTE_SET,
    }
  },
  methods: {
    onUpdate() {
      this.$emit('click:edit')
    },
    onDelete() {
      this.$emit('click:delete')
    },
  },
})
</script>

<style lang="scss" scoped>
.m-table-actions__body {
  &.v-list {
    @include list-item;
    width: 150px;
  }
  &.is-surveyQuestion {
    &.v-list {
      @include list-item;
      width: 250px;
    }
  }
}
</style>
