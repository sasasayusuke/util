<template>
  <v-row
    class="o-common-detail-rows__unit"
    :class="[disabled ? 'is-disabled' : '']"
  >
    <v-col
      v-if="chip"
      md="auto"
      :align-self="tall ? 'start' : 'center'"
      class="o-common-detail-rows__label d-flex"
    >
      <Chip
        v-if="chip === 'void'"
        style-set="void"
        class="o-common-detail-rows__chip"
        :style="chipSize"
      />
      <Chip v-else style-set="sf" class="o-common-detail-rows__chip">
        {{ chip }}
      </Chip>
    </v-col>
    <v-col
      :cols="cols"
      :align-self="tall ? 'start' : 'center'"
      class="o-common-detail-rows__title"
    >
      <slot name="label">
        {{ label }}
        <Required v-if="required && isEditing" />
        <ToolTips v-if="tooltip"> {{ tooltip }} </ToolTips>
      </slot>
    </v-col>
    <v-col
      :align-self="tall ? 'start' : 'center'"
      class="o-common-detail-rows__data"
    >
      <v-row v-if="isEditing || isDraft" class="px-3 pt-3 pb-3">
        <slot name="default"></slot>
      </v-row>
      <template v-else>
        <slot name="isNotEditing"> {{ value }} </slot>
      </template>
    </v-col>
  </v-row>
</template>

<script lang="ts">
import { ToolTips, Required, Chip } from '../../common/atoms/index'
import BaseComponent from '~/common/BaseComponent'

export default BaseComponent.extend({
  components: {
    ToolTips,
    Required,
    Chip,
  },
  computed: {
    chipSize() {
      return {
        width: `${this.chipWidth}px`,
      }
    },
  },
  props: {
    label: { type: String },
    required: { type: Boolean, default: false },
    disabled: { type: Boolean, default: false },
    isEditing: { type: Boolean, required: true },
    isDraft: { type: Boolean, default: false },
    tooltip: { type: String, default: '' },
    value: { default: '' },
    chip: { type: String, default: '' },
    chipWidth: { type: Number, required: false, default: 33 },
    tall: { type: Boolean, default: false },
    cols: { type: String, default: '3' },
  },
})
</script>

<style lang="scss" scoped>
.o-common-detail-rows__unit {
  border-bottom: 1px solid $c-gray-line;
  margin: 0 !important;
  align-items: center;
  &.is-disabled {
    .o-common-detail-rows__title,
    .o-common-detail-rows__data {
      color: #8f8f8f;
    }
  }
  &.is-noborder {
    border-bottom: 0;
  }
}
.o-common-detail-rows__label {
  padding-right: 0;
}
.o-common-detail-rows__title {
  font-size: 0.875rem;
  font-weight: bold;
  align-items: center;
}
.o-common-detail-rows__data {
  align-items: center;
  table {
    th,
    td {
      padding-bottom: 12px;
    }
    tr {
      &:last-child {
        th,
        td {
          padding-bottom: 0;
        }
      }
    }
    th {
      text-align: left;
      padding-right: 24px;
      padding-left: 0;
      width: 150px;
    }
  }
  .o-common-detail-rows__table__edit {
    padding-left: 24px;
    text-align: left;
  }
}
</style>
