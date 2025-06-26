<script lang="ts">
import BaseComponent from '~/common/BaseComponent'

export default BaseComponent.extend({
  props: {
    isCreating: {
      type: Boolean,
      default: false,
    },
    isEditing: {
      type: Boolean,
      required: true,
    },
  },
  data() {
    return {
      value: '',
      isValid: false,
    }
  },
  methods: {
    update(newValue: any) {
      this.$emit('update', newValue)
    },
    resetLocalParam() {
      // 要オーバーライド
      // 例: this.localParam = Object.assign(new LocalCustomer(), this.customer)
    },
  },
  watch: {
    localParam: {
      deep: true,
      handler(newValue: any) {
        this.update(newValue)
      },
    },
    isEditing(newVal: boolean, oldVal: boolean) {
      if (newVal === true && oldVal === false) {
        this.resetLocalParam()
      }
    },
  },
})
</script>
