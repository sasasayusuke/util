<template>
  <TemplateProjectImportDone
    :response="response"
    :is-loading="isLoading"
    :error-status="errorStatus"
    :error-detail="errorDetail"
  />
</template>

<script lang="ts">
import BasePage from '~/common/BasePage'
import TemplateProjectImportDone from '~/components/project/templates/ProjectImportDoneTemplate.vue'
import {
  ImportProjectsRequest,
  ImportProjectsResponse,
  ImportProjects,
  ENUM_IMPORT_PROJECTS_MODE,
} from '~/models/Project'

export default BasePage.extend({
  name: 'ProjectImportDone',
  components: {
    TemplateProjectImportDone,
  },
  middleware: ['roleCheck'],
  head() {
    return {
      title: this.$t('project.pages.import.name') as string,
    }
  },
  data() {
    return {
      response: new ImportProjectsResponse(),
      errorStatus: 0,
      errorDetail: '',
      isLoading: true,
    }
  },
  computed: {
    /**
     * URLパラメーターのobjectKeyを返す
     * @returns URLパラメーターのobjectKey
     */
    objectKey() {
      return this.$route.query.objectKey as string
    },
  },
  methods: {
    /**
     * ImportProjectsAPIを叩いて、案件のCSVデータをエラーチェックまたは取り込みを実行
     */
    async importProjects() {
      this.isLoading = true

      const request = new ImportProjectsRequest()
      request.mode = ENUM_IMPORT_PROJECTS_MODE.EXECUTE
      request.file = this.objectKey

      await ImportProjects(request)
        .then((res) => {
          this.response = res.data
        })
        .catch((error) => {
          if (error.status) {
            this.errorStatus = error.status
          } else {
            // 不明なエラー類は一旦全てこちらに設定 ※番号はとりあえずで999としてます。
            this.errorStatus = 999
          }
          if (error?.response?.data?.detail) {
            this.errorDetail = error?.response?.data?.detail
          }
        })
        .finally(() => {
          this.isLoading = false
        })
    },
  },
  mounted() {
    this.displayLoading([this.importProjects()])
  },
})
</script>
