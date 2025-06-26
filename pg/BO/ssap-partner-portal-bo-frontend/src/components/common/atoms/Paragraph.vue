<template>
  <p class="a-paragraph" v-bind="attributes" v-on="$listeners">
    <slot />
  </p>
</template>

<script lang="ts">
import WrapperComponent, { AttributeSet } from '../bases/WrapperComponent'

/*
  Paragraphの属性のセットを作成し、名前をつける
  セット名はケバブケースで設定(呼び出し元のhtml上で属性値として記述するため)
  defaultは指定しなくても自動で適用される
*/
const ATTRIBUTE_SET: AttributeSet = {
  default: {},
  'confirm-text': {
    class: 'a-paragraph__confirm-text',
  },
  'error-text': {
    class: 'a-paragraph__error-text',
  },
  'modal-text1': {
    class: 'a-paragraph__modal-text--1',
  },
  'modal-text2': {
    class: 'a-paragraph__modal-text--2',
  },
  small: {
    class: 'font-size-small',
  },
  bold: {
    class: 'font-weight-bold',
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
  data() {
    return {
      attributeSet: ATTRIBUTE_SET,
    }
  },
})
</script>

<style lang="scss" scoped>
.a-paragraph {
  & + .a-paragraph {
    margin-top: 1em;
  }
  a {
    color: $c-primary-dark;
  }
}
.a-paragraph__confirm-text {
  font-size: 0.875rem;
  margin-bottom: 8px;
  strong {
    color: $c-primary-dark;
    strong {
      font-size: 1.25rem;
    }
  }
}
.a-paragraph__error-text {
  font-size: 0.875rem;
  margin-bottom: 8px;
  strong {
    color: $c-red;
    strong {
      font-size: 1.25rem;
    }
  }
}
.a-paragraph__modal-text--1 {
  font-size: 1.125rem;
  font-weight: bold;
  white-space: pre-wrap;
  text-align: center;
}
.a-paragraph__modal-text--2 {
  font-size: 1rem;
  margin-bottom: 0;
}
.font-size-small {
  font-size: 0.875rem;
}
</style>
