<template>
  <component
    :is="tag"
    class="a-text-link"
    v-bind="attributes"
    v-on="$listeners"
  >
    <Icon size="12" color="#333">icon-org-arrow-right</Icon>
    <span class="a-text-link__text">
      <slot />
    </span>
  </component>
</template>

<script lang="ts">
import WrapperComponent, { AttributeSet } from '../bases/WrapperComponent'
import { Icon } from '~/components/common/atoms/index'
/*
  セット名はケバブケースで設定(呼び出し元のhtml上で属性値として記述するため)
  defaultは指定しなくても自動で適用される
*/
const ATTRIBUTE_SET: AttributeSet = {
  default: {
    class: 'font-size-small font-weight-bold',
  },
  'text-underline': {
    class: 'font-size-small font-weight-bold a-text-link--underline',
  },
}

export default WrapperComponent.extend({
  props: {
    /*
      受け取ったpropsは、丸ごと子コンポーネントに渡される
      また、style-setプロパティを受け取り、ATTRIBUTE_SETに定義した属性群を適用できる
      詳細はWrapperComponent参照
    */
  },
  components: {
    Icon,
  },
  computed: {
    tag() {
      const to = Object.keys(this.attributes).indexOf('to')
      return to !== -1 ? 'router-link' : 'a'
    },
  },
  data() {
    return {
      attributeSet: ATTRIBUTE_SET,
    }
  },
})
</script>

<style lang="scss" scoped>
.a-text-link {
  text-decoration: none;
  color: $c-primary-dark;
  &:hover,
  &:focus {
    .a-text-link__text {
      text-decoration: none;
      color: $c-primary-over;
    }
  }
  &[disabled],
  &.disabled {
    color: $c-disabled;
    pointer-events: none;
    touch-action: none;
    .a-text-link__text {
      text-decoration: none;
    }
  }
  &.a-text-link--underline {
    .a-text-link__text {
      transition-duration: 0.2s;
      text-decoration: underline;
      &:hover,
      &.focus {
        text-decoration: underline;
      }
    }
  }
}
</style>
