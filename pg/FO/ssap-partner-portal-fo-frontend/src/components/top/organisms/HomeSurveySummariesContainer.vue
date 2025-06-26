<script lang="ts">
import HomeSurveySummariesCard from '../molecules/HomeSurveySummariesGraph.vue'
import CommonContainer from '~/components/common/organisms/CommonContainer.vue'
import BaseComponent from '~/common/BaseComponent'

export default BaseComponent.extend({
  components: {
    HomeSurveySummariesCard,
    CommonContainer,
  },
  props: {
    surveySummary: {
      type: Object,
    },
  },
})
</script>
