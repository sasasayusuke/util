import BaseVueClass, { PropType } from './BaseVueClass'

/**
 * コンポーネントの基底クラス
 */
export default BaseVueClass.extend({
  data() {
    return {}
  },
  methods: {},
})

export type { PropType }
export type { FormModel } from '~/types/Form'
