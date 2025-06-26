<script lang="ts">
import BaseComponent from '~/common/BaseComponent'

export default BaseComponent.extend({
  data() {
    return {
      isLoading: false,
      detailPagePrefix: '',
      listPagePath: '',
    }
  },
  methods: {
    onClickPositive($event: any) {
      this.create($event)
    },
    onClickNegative() {
      this.toListPage()
    },
    toListPage() {
      this.$router.push(this.listPagePath)
    },
    toDetailPage(id: string) {
      this.$router.push(this.detailPagePrefix + id)
    },
    create(_localParam: any) {
      /*
      this.isLoading = true

      const request = new CreateRequest()
      Object.assign(request, localParam)

      CreateCustomer(request)
        .then((res) => {
          const id = res.data.id
          this.toDetailPage(id)
        })
        .finally(() => {
          this.isLoading = false
        })
      */
    },
  },
})
</script>
