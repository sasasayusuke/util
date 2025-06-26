<template>
  <div class="m-karter-aside-check-box">
    <Title style-set="aside">
      {{ $t('karte.pages.detail.producer') }}
    </Title>
    <template v-if="!isEditing && producer !== undefined">
      <Checkbox
        :key="index"
        v-model="localIds"
        class="mr-7 mt-2 a-checkbox--gray"
        :label="producer.name"
        :value="producer.id"
        hide-details
        :disabled="!ids.includes(producer.id)"
        type="noEditing"
      />
    </template>
    <template v-else-if="isEditing && producer !== undefined">
      <Checkbox
        :key="index"
        v-model="localIds"
        class="mr-7 mt-2 a-checkbox--black"
        :label="producer.name"
        :value="producer.id"
        hide-details
        @change="onChange($event)"
      />
    </template>
    <Title style-set="aside" class="mt-10">
      {{ $t('karte.pages.detail.accelerator') }}
    </Title>
    <template v-if="!isEditing">
      <Checkbox
        v-for="(user, index) in accelerators"
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
        v-for="(user, index) in accelerators"
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
import { PropType } from '~/common/BaseComponent'
import { SimpleUser } from '~/models/Project'
import KarteAsideCheckBox from '~/components/karte/molecules/KarteAsideCheckBox.vue'

export default KarteAsideCheckBox.extend({
  props: {
    /**
     * ユーザー情報
     */
    producer: {
      type: SimpleUser,
      required: false,
    },
    /**
     * ユーザー情報
     */
    accelerators: {
      type: Array as PropType<SimpleUser[]>,
      required: false,
    },
  },
})
</script>

<style lang="scss" scoped></style>
