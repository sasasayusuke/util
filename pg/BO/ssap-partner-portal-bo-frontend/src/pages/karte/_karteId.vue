<template>
  <TemplateKarteDetail
    :project="project"
    :karte="karte"
    :surveys="getSurveysResponse.surveys"
  />
</template>

<script lang="ts">
import BasePage from '~/common/BasePage'
import TemplateKarteDetail, {
  isLoading,
} from '~/components/karte/templates/KarteDetail.vue'
import { GetProjectByIdResponse, GetProjectById } from '~/models/Project'
import { GetKarteByIdResponse, GetKarteById } from '~/models/Karte'
import { GetSurveysResponse, GetSurveys } from '~/models/Survey'
import { currentPageDataStore } from '~/store'
import { GetNpfProjectId } from '~/models/MasterKarte'

export default BasePage.extend({
  name: 'KarteDetail',
  components: {
    TemplateKarteDetail,
  },
  middleware: ['roleCheck'],
  head() {
    return {
      title: this.$t('karte.pages.detail.name') as string,
    }
  },
  data(): {
    project: GetProjectByIdResponse
    karte: GetKarteByIdResponse
    getSurveysResponse: GetSurveysResponse
    isLoading: isLoading
  } {
    return {
      project: new GetProjectByIdResponse(),
      karte: new GetKarteByIdResponse(),
      getSurveysResponse: new GetSurveysResponse(),
      isLoading: {
        project: true,
        karte: true,
        surveys: true,
      },
    }
  },
  mounted() {
    this.displayLoading([
      this.getKarteById(() => {
        this.getProjectById()
        this.getSurveys()
      }),
    ])
  },
  methods: {
    /**
     * GetKartenAPIを叩き、案件カルテをkarteIdで取得
     * @param _callback getProjectById, getSurveys methodsを指定
     */
    async getKarteById(_callback: any) {
      const callback: any = _callback || function () {}
      this.isLoading.karte = true
      const id: string = this.$route.params.karteId as string

      await GetKarteById(id)
        .then((res) => {
          this.karte = res.data
          this.isLoading.karte = false
          callback()
        })
        .catch((error) => {
          if (error.response && error.response.status === 404) {
            this.$router.push('/404')
          } else {
            this.apiErrorHandle(error)
          }
        })
    },
    /**
     * GetProjectByIdを叩き、案件をprojectIdで一意に取得。ストアのcurrentPageDataStoreにデータを格納
     */
    async getProjectById() {
      this.isLoading.project = true
      const id: string = this.karte.projectId as string

      await GetProjectById(id).then((res) => {
        this.project = res.data
        if (!this.project.updateUserName) {
          this.project.updateUserName = ''
        }
        this.isLoading.project = false

        currentPageDataStore.setValues({
          projectName: this.project.name,
          customerName: this.project.customerName,
        })
        currentPageDataStore.setPpProjectId(this.project.id)
      })
      await GetNpfProjectId(id).then((res) => {
        currentPageDataStore.setNpfProjectId(res.data.npfProjectId)
      })
    },
    /**
     * GetSurveyAPIを叩き、案件アンケートの一覧を取得
     */
    async getSurveys() {
      this.isLoading.surveys = true
      const params = { projectId: '' }
      params.projectId = this.karte.projectId as string
      await GetSurveys(params).then((res) => {
        this.getSurveysResponse = res.data
        this.isLoading.surveys = false
      })
    },
  },
})
</script>
