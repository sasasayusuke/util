<template>
  <div class="o-man-hour-sum px-4 py-3 ma-0">
    <div class="d-flex flex-row justify-end">
      <!-- 新規計 -->
      <div v-if="$scopedSlots.new" class="mr-8">
        {{ $t('man-hour.pages.supporter.userId.manHourSum.new') }}
        <span><slot name="new"></slot>h</span>
      </div>
      <!-- 継続計 -->
      <div v-if="$scopedSlots.continuation" class="mr-8">
        {{ $t('man-hour.pages.supporter.userId.manHourSum.continuation') }}
        <span><slot name="continuation"></slot>h</span>
      </div>
      <!-- 合計 -->
      <div v-if="$scopedSlots.sum">
        {{ $t('man-hour.pages.supporter.userId.manHourSum.sum') }}
        <span><slot name="sum"></slot>h</span>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import BasePage from '~/common/BasePage'
export default BasePage.extend({
  name: 'ManHourSum',
})
</script>

<style lang="scss" scoped>
.o-man-hour-sum span {
  color: $c-primary-dark;
  font-size: 1.25rem;
  font-weight: bold;
}
</style>
