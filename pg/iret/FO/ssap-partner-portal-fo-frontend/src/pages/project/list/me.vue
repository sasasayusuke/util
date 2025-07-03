<template>
  <TemplateProjectListMe
    :response="response"
    :is-loading="isLoading"
    @getProjects="getProjects"
  />
</template>

<script lang="ts">
import BasePage from '~/common/BasePage'
import TemplateProjectListMe from '~/components/project/templates/ProjectListMe.vue'
import {
  GetProjectsYearMonthAllAssignedRequest,
  GetProjectsResponse,
  GetProjects,
} from '~/models/Project'
import { meStore } from '~/store'
import { ENUM_USER_ROLE } from '~/types/User'

export default BasePage.extend({
  // アンケート管理 / 担当案件一覧
  name: 'ProjectMe',
  components: {
    TemplateProjectListMe,
  },
  head() {
    return {
      title: this.$t('project.pages.me.name') as string,
    }
  },
  middleware: ['roleCheck'],
  data() {
    return {
      /** 案件一覧 */
      response: new GetProjectsResponse(),
      isLoading: {
        projects: true,
      },
    }
  },
  mounted() {
    this.displayLoading([this.getProjects()])
  },
  computed: {
    isBusinessMgr() {
      return meStore.role === ENUM_USER_ROLE.BUSINESS_MGR
    },
  },
  methods: {
    /** GetProjects API: 案件の検索・一覧を取得 */
    async getProjects(
      params: GetProjectsYearMonthAllAssignedRequest = new GetProjectsYearMonthAllAssignedRequest()
    ) {
      const paramsCopy = JSON.parse(JSON.stringify(params))

      this.isLoading.projects = true
      if (this.isBusinessMgr) {
        if (params.allAssigned) {
          // 全担当
          paramsCopy.all = true
        } else {
          // 主担当
          paramsCopy.allAssigned = true
        }
      }
      await GetProjects(paramsCopy)
        .then((res) => {
          this.response = res.data
          this.isLoading.projects = false
        })
        .catch((error) => {
          this.apiErrorHandle(error)
        })
    },
  },
})
</script>
