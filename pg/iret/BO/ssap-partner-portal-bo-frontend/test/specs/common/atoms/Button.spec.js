import { mount } from '@vue/test-utils'
import Button from '@/components/common/atoms/Button'

describe('Button', () => {
  // レンダリングの確認
  test('Is it rendered correctly', () => {
    const wrapper = mount(Button, {
      slots: {
        default: 'foo',
      },
      propsData: {
        'style-set': 'normal-primary',
      },
    })

    expect(wrapper.element).toMatchSnapshot() // スナップショットの作成
  })
})
