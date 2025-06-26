import BaseComponent from '~/common/BaseComponent'

export interface AttributeSet {
  default: { [key: string]: any }
  [key: string]: { [key: string]: any }
}

function splitStr(str: string, divider: string = ' ') {
  return str.split(divider)
}

/*
  v-btnの属性のセットを作成し、名前をつける
  セット名はケバブケースで設定(呼び出し元のhtml上で属性値として記述するため)
*/
const ATTRIBUTE_SET: AttributeSet = {
  default: {},
}

export default BaseComponent.extend({
  inheritAttrs: false,
  props: {
    // スタイルセット名(スペース区切りで配列に変換される)
    styleSet: {
      type: String,
      default: '',
    },
  },
  data() {
    return {
      attributeSet: ATTRIBUTE_SET,
    }
  },
  computed: {
    attributes(): Object {
      return this.generateAttributes(
        this.attributeSet,
        this.styleSet,
        this.$attrs
      )
    },
  },
  methods: {
    generateAttributes(
      attrSet: AttributeSet,
      styleSet: string,
      additionalAttrs: object
    ) {
      let rtn = {}

      // デフォルト値を適用
      rtn = Object.assign(rtn, attrSet.default)

      const styles = splitStr(styleSet)
      // スタイルセットを適用
      styles.forEach((styleName) => {
        Object.assign(rtn, attrSet[styleName])
      })

      rtn = Object.assign(rtn, additionalAttrs)

      return rtn
    },
  },
})
