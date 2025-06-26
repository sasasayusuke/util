<template>
  <CommonNoticeCard class="o-UnderMaintenanceCard">
    <v-card-title class="o-UnderMaintenanceCard__header">
      <h1 class="o-UnderMaintenanceCard__title">
        {{ $t('top.pages.notice.maintenance.title') }}
      </h1>
    </v-card-title>
    <div class="o-UnderMaintenanceCard__body">
      <p class="o-UnderMaintenanceCard__description">
        <span>{{ $t('top.pages.notice.maintenance.description') }}</span>
      </p>
      <p class="o-UnderMaintenanceCard__maintenance-period">
        <span>{{ $t('top.pages.notice.maintenance.maintenance_period') }}</span>
      </p>
      <p class="o-UnderMaintenanceCard__caution">
        <span>{{ $t('top.pages.notice.maintenance.caution') }}</span>
      </p>
    </div>
  </CommonNoticeCard>
</template>

<script lang="ts">
import CommonNoticeCard from '../../common/organisms/CommonNoticeCard.vue'
import BaseComponent from '~/common/BaseComponent'

export default BaseComponent.extend({
  components: {
    CommonNoticeCard,
  },
})
</script>

<style lang="scss" scoped>
.o-UnderMaintenanceCard {
  padding: 60px 0;
  &.v-card {
    border-color: $c-primary-dark;
    border-width: 2px;
  }
}
.o-UnderMaintenanceCard__header {
  padding: 0;
}
.o-UnderMaintenanceCard__title {
  font-size: 1.425rem;
  font-weight: bold;
  color: $c-primary-dark;
  text-align: center;
  width: 100%;
  justify-content: center;
  padding: 0;
}
.o-UnderMaintenanceCard__body {
  text-align: center;
  p {
    margin-bottom: 0;
  }
  span {
    white-space: pre-wrap;
  }
}
.o-UnderMaintenanceCard__description {
  @include fontSize('small');
  margin-top: 24px;
}
.o-UnderMaintenanceCard__maintenance-period {
  @include fontSize('xlarge');
  font-weight: bold;
  color: $c-primary-dark;
  margin-top: 32px;
}
.o-UnderMaintenanceCard__caution {
  @include fontSize('xsmall');
  margin-top: 8px;
}
</style>
