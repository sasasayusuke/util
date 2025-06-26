<template>
  <div class="m-karter-aside-check-box">
    <Title style-set="aside">
      {{ title }}
    </Title>
    <template v-if="!isEditing">
      <Checkbox
        v-for="(user, index) in users"
        :key="index"
        v-model="localIds"
        class="mr-7 mt-2 a-checkbox--gray"
        :label="user.name"
        :value="user.id"
        hide-details
        :disabled="!ids.includes(user.id)"
        type="noEditing"
      />
    </template>
    <template v-else>
      <Checkbox
        v-for="(user, index) in users"
        :key="index"
        v-model="localIds"
        class="mr-7 mt-2 a-checkbox--black"
        :label="user.name"
        :value="user.id"
        hide-details
        @change="onChange($event)"
      />
    </template>
  </div>
</template>

<script lang="ts">
import { Title, Checkbox } from '~/components/common/atoms/index'
import BaseComponent, { PropType } from '~/common/BaseComponent'
import { SimpleUser } from '~/models/Project'

export default BaseComponent.extend({
  name: '',
  components: {
    Title,
    Checkbox,
  },
  props: {
    /**
     * タイトル
     */
    title: {
      type: String,
      required: true,
    },
    /**
     * ユーザー情報
     */
    users: {
      type: Array as PropType<SimpleUser[]>,
      required: false,
    },
    /**
     * ユーザーのID情報
     */
    ids: {
      type: Array,
      required: true,
    },
    /**
     * 編集か否か
     */
    isEditing: {
      type: Boolean,
    },
    /**
     * 顧客ロールか否か
     */
    isCustomer: {
      type: Boolean,
      default: false,
    },
    /**
     * emitで親コンポーネントに渡すメソッド名を作るための構成文字列
     */
    item: {
      type: String,
      required: false,
    },
  },
  data() {
    return {
      localIds: this.ids,
    }
  },
  methods: {
    /**
     * チェックボックスを変更した時の動作
     * @param イベント
     */
    onChange(event: any) {
      this.$emit('update' + this.item, event)
    },
  },
})
</script>

<style lang="scss" scoped></style>
