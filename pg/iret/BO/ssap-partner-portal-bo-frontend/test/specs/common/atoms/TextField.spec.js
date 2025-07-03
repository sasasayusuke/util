import { mount } from '@vue/test-utils'
import TextField from '@/components/common/atoms/TextField'

describe('TextField', () => {
  // レンダリングの確認
  test('Is it rendered correctly', () => {
    const wrapper = mount(TextField, {
      propsData: {
        placeholder: '日付選択',
      },
    })

    expect(wrapper.element).toMatchSnapshot() // スナップショットの作成
  })
})
