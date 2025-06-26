<template>
  <CommonSort @sort="$emit('sort')" @clear="$emit('clear')">
    <PPSurveySortInput :param="param" v-on="$listeners" @change="onChange" />
  </CommonSort>
</template>

<script lang="ts">
import PPSurveySortInput, {
  SurveySearchParam,
} from '../molecules/PPSurveySortInput.vue'
import BaseComponent from '~/common/BaseComponent'
import CommonSort from '~/components/common/organisms/CommonSort.vue'

export { SurveySearchParam }

export default BaseComponent.extend({
  components: {
    CommonSort,
    PPSurveySortInput,
  },
  props: {
    /** リクエストパラメーター */
    param: {
      type: SurveySearchParam,
      required: true,
    },
  },
  methods: {
    /** 並び替え条件の入力変更 */
    onChange(localData: SurveySearchParam) {
      this.$emit('update', 'isRefined', localData.isRefined)
      this.$emit('update', 'from', localData.from)
      this.$emit('update', 'to', localData.to)
    },
  },
})
</script>
