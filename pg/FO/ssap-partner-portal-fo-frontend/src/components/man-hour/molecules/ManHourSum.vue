<template>
  <div class="o-man-hour-sum px-4 py-3 ma-0">
    <div class="d-flex flex-row justify-end">
      <div v-if="$scopedSlots.new" class="mr-8">
        {{ $t('man-hour.tables.sums.new')
        }}<span><slot name="new"></slot>h</span>
      </div>
      <div v-if="$scopedSlots.continuation" class="mr-8">
        {{ $t('man-hour.tables.sums.continuation')
        }}<span><slot name="continuation"></slot>h</span>
      </div>
      <div v-if="$scopedSlots.sum">
        {{ $t('man-hour.tables.sums.sum')
        }}<span><slot name="sum"></slot>h</span>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import BasePage from '~/common/BasePage'
export default BasePage.extend({
  components: {},
})
</script>

<style lang="scss" scoped>
.o-man-hour-sum span {
  color: $c-primary-dark;
  font-size: 1.25rem;
  font-weight: bold;
}
</style>
