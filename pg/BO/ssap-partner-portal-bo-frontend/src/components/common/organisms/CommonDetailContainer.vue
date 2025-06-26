<script lang="ts">
import BaseComponent from '~/common/BaseComponent'

export default BaseComponent.extend({
  props: {
    isEditing: {
      type: Boolean,
    },
    isCreating: {
      type: Boolean,
    },
    isLoading: {
      type: Boolean,
      required: true,
    },
    isCurrentProgram: {
      type: Boolean,
      required: false,
    },
  },
  data() {
    return {
      isValid: true,
      isFirstLoaded: false,
      isChanged: false,
    }
  },
  methods: {
    update(localParamName: string, localParamObject: any) {
      this.$set(this, localParamName, localParamObject)
      if (this.isFirstLoaded === false) {
        this.isChanged = false
        this.isFirstLoaded = true
      } else {
        this.isChanged = true
      }
    },
  },
  computed: {
    mode() {
      if (this.isCreating) {
        return 'create'
      }

      if (this.isEditing) {
        return 'edit'
      } else {
        return 'detail'
      }
    },
    isValidWithChange(): boolean {
      if (this.isEditing) {
        if (this.isFirstLoaded === false) {
          return false
        } else {
          return this.isChanged ? this.isValid : false
        }
      } else {
        return true
      }
    },
  },
  watch: {
    isEditing(newVal: boolean, oldVal: boolean) {
      if (newVal === true && oldVal === false) {
        this.isFirstLoaded = false
        this.isChanged = false
      }
    },
  },
})
</script>
