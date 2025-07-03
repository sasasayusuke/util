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
      <v-list-item v-if="item !== 'draft'" @click="onRefer">
        {{ $t('common.button.reference') }}
      </v-list-item>
      <v-list-item
        v-if="item === 'draft' && isSurveyOpsOrSystemAdmin"
        @click="onUpdate"
      >
        {{ $t('common.button.update') }}
      </v-list-item>
      <v-list-item
        v-if="item === 'draft' && isSurveyOpsOrSystemAdmin"
        @click="onDraft"
      >
        {{ $t('common.button.updateOperation') }}
      </v-list-item>
      <v-list-item
        v-if="showsCopyOptions && isSurveyOpsOrSystemAdmin"
        @click="onDelete"
      >
        {{ $t('common.button.copyAndCreateNew') }}
      </v-list-item>
    </v-list>
    <v-list
      v-else
      class="m-table-actions__body"
      :class="[showResend ? 'is-resend' : '']"
    >
      <v-list-item v-if="showUpdate" @click="onUpdate">
        {{ $t('common.button.update') }}
      </v-list-item>
      <v-list-item v-if="showDelete" @click="onDelete">
        {{ $t('common.button.delete') }}
      </v-list-item>
      <v-list-item v-if="showResend" @click="onResend">
        {{ $t('common.button.ResendAnonymousSurvey') }}
      </v-list-item>
    </v-list>
  </v-menu>
</template>
<script lang="ts">
import WrapperComponent, { AttributeSet } from '../bases/WrapperComponent'
import { Icon, Button } from '~/components/common/atoms/index'
import { hasRole } from '~/utils/role-authorizer'
import { ENUM_ADMIN_ROLE } from '~/types/Admin'

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
    showUpdate: {
      type: Boolean,
      default: true,
    },
    showDelete: {
      type: Boolean,
      default: true,
    },
    showsCopyOptions: {
      type: Boolean,
      default: true,
    },
    showResend: {
      type: Boolean,
      default: false,
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
    onDraft() {
      this.$emit('click:draft')
    },
    onRefer() {
      this.$emit('click:refer')
    },
    onResend() {
      this.$emit('click:resend')
    },
  },
  computed: {
    isSurveyOpsOrSystemAdmin() {
      return hasRole([ENUM_ADMIN_ROLE.SURVEY_OPS, ENUM_ADMIN_ROLE.SYSTEM_ADMIN])
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
  &.is-resend {
    &.v-list {
      @include list-item;
      width: 200px;
    }
  }
}
</style>
