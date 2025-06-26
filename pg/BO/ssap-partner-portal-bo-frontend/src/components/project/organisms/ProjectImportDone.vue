<template>
  <CommonContainer
    :title="title"
    :is-editing="isEditing"
    :is-register="isRegister"
    is-hide-button1
    is-hide-header-button
    hx="h1"
    @buttonAction1="buttonAction1"
    @buttonAction2="buttonAction2"
  >
    <Sheet class="pa-8 pb-0">
      <Title hx="h2" style-set="detail">
        <Icon color="btn_primary" size="24">mdi-checkbox-marked-circle</Icon>
        {{ $t('project.pages.import.confirm.completed') }}
      </Title>
      <div class="o-common-data-table o-import-table is-scroll is-import">
        <ProjectImportTable :projects="projects" :is-loading="isLoading" />
      </div>
    </Sheet>
  </CommonContainer>
</template>

<script lang="ts">
import ProjectImportTable from './ProjectImportTable.vue'
import BaseComponent from '~/common/BaseComponent'
import type { PropType } from '~/common/BaseComponent'
import { ImportedProject } from '@/types/Project'

import CommonContainer from '~/components/common/organisms/CommonContainer.vue'

import { Sheet, Title, Icon } from '~/components/common/atoms/index'

export default BaseComponent.extend({
  components: {
    CommonContainer,
    Sheet,
    Title,
    Icon,
    ProjectImportTable,
  },
  props: {
    /** 案件情報 */
    projects: {
      type: Array as PropType<ImportedProject[]>,
      required: true,
    },
    /** 読み込み中か */
    isLoading: {
      type: Boolean,
      required: true,
    },
  },
  data() {
    return {
      title: this.$t('project.pages.import.name'),
      isEditing: false,
      isRegister: false,
    }
  },
  methods: {
    buttonAction1() {},
    /** /project/listに遷移する */
    buttonAction2() {
      this.$router.push('/project/list')
    },
  },
})
</script>

<style lang="scss">
.o-import-table {
  .v-data-table {
    background-color: $c-white;
    .v-data-table__wrapper {
      table {
        tbody {
          tr {
            &:hover {
              background: $c-white !important;
            }

            &:nth-child(even) {
              &:hover {
                background-color: $c-black-table-border !important;
              }
            }
            td {
              font-size: 0.75rem;
            }
          }
        }
      }
    }
  }
}
</style>
