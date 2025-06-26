<script lang="ts">
import BaseComponent from '~/common/BaseComponent'

export default BaseComponent.extend({
  data() {
    return {
      headerPageName: '',
      isEditing: false,
      isLoading: false,
      isModalOpen: false,
      isDeletable: true,
      listPagePath: '',
    }
  },
  methods: {
    onClickPositive($event: any) {
      this.clearErrorBar()
      if (!this.isEditing) {
        this.isEditing = true
      } else {
        this.update($event)
      }
    },
    onClickNegative() {
      this.clearErrorBar()
      if (this.isEditing) {
        this.isEditing = false
      } else {
        this.toListPage()
      }
    },
    toListPage() {
      this.clearErrorBar()
      this.$router.push(this.listPagePath)
    },
    refresh() {
      this.clearErrorBar()
      this.$emit('refresh')
    },
    // 要オーバーライド
    update(_localParam: any) {
      /*
      this.isLoading = true

      const request = new UpdateByIdRequest()
      Object.assign(request, localParam)

      const id = localCustomer.id
      const version = localCustomer.version

      UpdateById(id, version, request)
        .then(this.updateThen)
        .finally(() => {
          this.isLoading = false
        })
      */
    },
    updateThen() {
      this.refresh()
      this.isEditing = false
    },
    // 要オーバーライド
    // deleteという名前の関数は作れない
    deleteRecord() {
      /*
      this.isLoading = true

      const id = this.customer.id
      const version = this.customer.version

      DeleteById(id, version)
        .then(() => {
          this.toListPage()
        })
        .catch((error) => {
          if (error.response.status === 400) {
            this.isDeletable = false
          } else {
            throw error
          }
        })
        .finally(() => {
          this.isLoading = false
        })
      */
    },
  },
})
</script>
