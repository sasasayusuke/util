<template>
  <v-row justify="center" class="px-8 py-0 ma-0 o-manhour-footer">
    <Sheet color="#EBF7ED" rounded width="100%" class="py-6">
      <p class="text-center a-manhour-total--foot">
        {{ title }} : <span>{{ total }}h</span>
      </p>
    </Sheet>
  </v-row>
</template>

<script lang="ts">
import BaseComponent from '~/common/BaseComponent'
import { Sheet } from '~/components/common/atoms/index'

export default BaseComponent.extend({
  components: {
    Sheet,
  },
  props: {
    isEditing: {
      type: Boolean,
      default: false,
    },
    title: {
      type: String,
      default: '',
    },
    isConfirm: {
      type: Boolean,
      default: false,
    },
    total: {
      type: Number,
      default: 0,
    },
  },
  data() {
    return {}
  },
  methods: {
    buttonAction(actionType: number = 0) {
      this.$emit('buttonAction', actionType)
    },
  },
})
</script>

<style lang="scss" scoped>
.a-manhour-total--foot {
  font-weight: bold;
  font-size: 1.25rem;
  margin-bottom: 0;
  span {
    font-size: 1.5rem;
    color: $c-primary-dark;
  }
}
</style>
