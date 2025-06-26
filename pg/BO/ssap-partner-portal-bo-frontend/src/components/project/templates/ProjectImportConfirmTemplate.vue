<template>
  <RootTemplate>
    <template v-if="!isError">
      <ProjectImportConfirm
        :projects="response.projects"
        :is-loading="isLoading"
      />
    </template>
    <template v-else>
      <ProjectImportFailed
        :projects="response.projects"
        :error-status="errorStatus"
        :error-detail="errorDetail"
        :is-loading="isLoading"
      />
    </template>
  </RootTemplate>
</template>

<script lang="ts">
import BaseComponent, { PropType } from '~/common/BaseComponent'
import RootTemplate from '~/components/common/bases/RootTemplate.vue'
import ProjectImportConfirm from '~/components/project/organisms/ProjectImportConfirm.vue'
import ProjectImportFailed from '~/components/project/organisms/ProjectImportFailed.vue'
import {
  ImportProjectsResponse,
  ENUM_IMPORT_PROJECTS_RESULT,
} from '~/models/Project'

export default BaseComponent.extend({
  created() {
    //一括登録機能は利用不可のため、404画面にリダイレクトする
    this.$router.push('/404')
  },
  components: {
    RootTemplate,
    ProjectImportConfirm,
    ProjectImportFailed,
  },
  props: {
    /** 正常なレスポンス（OK, NG）*/
    response: {
      type: Object as PropType<ImportProjectsResponse>,
    },
    /** 読み込み中か */
    isLoading: {
      type: Boolean,
      required: true,
    },
    /** 200以外のエラー */
    errorStatus: {
      type: Number,
    },
    /** エラー情報詳細 */
    errorDetail: {
      type: String,
      default: '',
    },
  },
  computed: {
    /**
     * エラー判定
     * @returns エラー判定真偽値
     */
    isError(): boolean {
      if (this.response.result === ENUM_IMPORT_PROJECTS_RESULT.NG) {
        return true
      }
      if (this.errorStatus) {
        return true
      }
      return false
    },
  },
})
</script>
