<template>
  <CommonContainer
    :title="title"
    :is-valid="isValid"
    :is-editing="isEditing"
    :is-register="isRegister"
    hx="h1"
    @buttonAction1="buttonAction1"
    @buttonAction2="buttonAction2"
  >
    <Sheet class="pa-8 pb-0">
      <Title hx="h2" style-set="detail">
        {{ $t('project.pages.import.confirm.lead') }}
      </Title>
      <p class="font-size-small">
        {{ $t('project.pages.import.confirm.text') }}
      </p>
      <div class="o-common-data-table is-scroll o-import-table">
        <ProjectImportTable :projects="projects" :is-loading="isLoading" />
      </div>
    </Sheet>
  </CommonContainer>
</template>

<script lang="ts">
import ProjectImportTable from './ProjectImportTable.vue'
import BaseComponent from '~/common/BaseComponent'
import type { PropType } from '~/common/BaseComponent'
import CommonContainer from '~/components/common/organisms/CommonContainer.vue'
import OverflowTooltip from '~/components/common/molecules/OverflowTooltip.vue'
import { ImportedProject } from '@/types/Project'

import {
  Sheet,
  Title,
  Button,
  FileInput,
  Paragraph,
} from '~/components/common/atoms/index'

export default BaseComponent.extend({
  components: {
    CommonContainer,
    Sheet,
    Title,
    Button,
    FileInput,
    Paragraph,
    OverflowTooltip,
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
      isRegister: true,
      isValid: true,
    }
  },
  methods: {
    /** /project/import/doneに遷移する */
    buttonAction1() {
      this.$router.push({
        path: '/project/import/done',
        query: { objectKey: this.objectKey },
      })
    },
    /** /project/importに遷移する */
    buttonAction2() {
      this.$router.push('/project/import')
    },
  },
  computed: {
    /**
     * URLパラメーターのobjectKeyを返す
     * @returns URLパラメーターのobjectKey文字列
     */
    objectKey(): string {
      return this.$route.query.objectKey as string
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
