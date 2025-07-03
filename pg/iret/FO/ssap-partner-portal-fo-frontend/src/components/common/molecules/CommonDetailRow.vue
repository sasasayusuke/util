<template>
  <v-row
    class="o-common-detail-rows__unit"
    :class="[disabled ? 'is-disabled' : '', noborder ? 'is-noborder' : '']"
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
        <div>
          {{ label }}

          <Required v-if="required && isEditing" />
          <ToolTips v-if="tooltip"> {{ tooltip }} </ToolTips>
        </div>
        <div v-if="notes" class="o-common-detail-rows__notes">{{ notes }}</div>
      </slot>
    </v-col>
    <v-col
      :align-self="tall ? 'start' : 'center'"
      class="o-common-detail-rows__data"
    >
      <v-row v-if="isEditing" class="px-3 pt-3 pb-3">
        <slot name="default"></slot>
      </v-row>
      <v-row v-if="!isEditing && component" class="px-3 pt-3 pb-3">
        <slot name="default"></slot>
      </v-row>
      <template v-if="!isEditing && !component">
        <slot name="isNotEditing">
          <!-- eslint-disable vue/no-v-html -->
          <span v-if="!escapeValue" class="pre-line" v-text="value"></span>
          <span v-else>{{ value }}</span>
          <span v-if="text">
            {{ text }}
          </span>
        </slot>
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
    text: { type: String },
    required: { type: Boolean, default: false },
    disabled: { type: Boolean, default: false },
    isEditing: { type: Boolean },
    tooltip: { type: String, default: '' },
    value: { default: '' },
    chip: { type: String, default: '' },
    chipWidth: { type: Number, required: false, default: 33 },
    tall: { type: Boolean, default: false },
    cols: { type: String, default: '3' },
    escapeValue: { type: Boolean, default: true },
    notes: { type: String, default: '' },
    component: { type: Boolean, default: false },
    noborder: { type: Boolean, default: false },
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
.o-common-detail-rows__notes {
  font-size: 0.75rem;
  font-weight: normal;
  white-space: pre-line;
  padding-top: 5px;
}
.o-common-detail-rows__data {
  align-items: center;
  word-break: break-word;
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
  .pre-line {
    white-space: pre-line;
  }
}
</style>
