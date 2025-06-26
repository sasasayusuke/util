<template>
  <Card class="m-card-project pa-5" :href="href" elevation="3">
    <p
      class="m-card-project__name d-flex justify-space-between text-primary-dark font-size-large font-weight-bold mb-2"
    >
      {{ name }} <Icon size="16px">icon-org-arrow-right</Icon>
    </p>
    <p class="m-card-project__organization font-size-small mb-8">
      {{ organization }}
    </p>
    <p
      class="m-card-project__terms align-center d-flex justify-space-between ma-0"
    >
      <span class="font-size-small">
        <Icon size="20px">mdi-calendar</Icon>
        支援期間：{{ terms }}
      </span>
      <Chip
        v-if="status === 'progress'"
        label
        small
        class="width-70"
        color="primary"
        >実施中</Chip
      >
    </p>
  </Card>
</template>

<script lang="ts">
import { Card, Chip, Icon } from '../atoms/index'
import BaseComponent from '~/common/BaseComponent'

export default BaseComponent.extend({
  components: {
    Card,
    Chip,
    Icon,
  },
  computed: {},
  props: {
    href: {
      type: String,
      default: '#',
    },
    name: {
      type: String,
      default: 'PROJECT NAME',
    },
    organization: {
      type: String,
      default: '法人顧客組織名',
    },
    terms: {
      type: String,
      default: '2021/04/01 〜 2021/12/25',
    },
    status: {
      type: String,
      default: 'progress',
    },
  },
})
</script>
