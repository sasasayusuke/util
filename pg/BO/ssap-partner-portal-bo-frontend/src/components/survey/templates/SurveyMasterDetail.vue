<template>
  <RootTemplate>
    <DetailInPageHeader
      v-if="!isEditing"
      back-to-list="/survey/master/list"
      class="o-survey-master-detail-title"
    >
      {{ pageName }}
    </DetailInPageHeader>
    <SurveyMasterDetailContainer
      v-if="isLoading === false"
      :title="sectionName()"
      :survey-master="latestSurveyMaster(response.masters)"
      :is-editing="isEditing"
      :is-loading="isLoading"
      :is-loading-button="isLoadingButton"
      :class="{ 'o-survey-master-setting-table': !isEditing }"
      @click:positive="onClickPositive"
      @click:negative="onClickNegative"
    />
    <SurveyMasterQuestionContainer
      v-if="isLoading === false"
      :title="$t('survey.pages.masterDetail.version.name')"
      :is-editing="isEditing"
      :is-loading="isLoading"
      :survey-masters="response.masters"
      class="mt-8"
      @click:edit="edit"
      @click:refer="refer"
      @click:draft="patch"
      @click:delete="copy"
    />
  </RootTemplate>
</template>

<script lang="ts">
import SurveyMasterDetailContainer, {
  LocalSurveyMaster,
} from '../organisms/SurveyMasterDetailContainer.vue'
import DetailInPageHeader from '~/components/common/organisms/DetailInPageHeader.vue'
import SurveyMasterQuestionContainer from '~/components/survey/organisms/SurveyMasterQuestionContainer.vue'
import CommonDetail from '~/components/common/templates/CommonDetailTemplate.vue'
import type { PropType } from '~/common/BaseComponent'
import RootTemplate from '~/components/common/bases/RootTemplate.vue'
import { Button, Sheet } from '~/components/common/atoms/index'
import {
  GetSurveyMastersByIdResponse,
  UpdateSurveyMasterLatestByIdRequest,
  UpdateSurveyMasterLatestById,
  GetSurveyMasterByIdAndRevision,
  PatchSurveyMasterRevisionById,
  CopySurveyMasterById,
  SurveyMasterListItem,
} from '~/models/Master'
import { ENUM_GET_SURVEY_MASTERS_IS_LATEST } from '~/types/Master'
export default CommonDetail.extend({
  components: {
    Button,
    Sheet,
    DetailInPageHeader,
    SurveyMasterDetailContainer,
    SurveyMasterQuestionContainer,
    RootTemplate,
  },
  props: {
    /** アンケートマスター */
    response: {
      type: Object as PropType<GetSurveyMastersByIdResponse>,
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
      pageName: this.$t('survey.pages.masterDetail.name'),
      listPagePath: `/survey/master/list`,
      isValid: false,
      isLoadingButton: false,
    }
  },
  methods: {
    /**
     * 表示タイトルを返す
     * @returns 表示タイトル文字列
     */
    sectionName() {
      return this.isEditing
        ? this.$t('survey.pages.masterDetail.setting.edit')
        : this.$t('survey.pages.masterDetail.setting.name')
    },
    /**
     * 入力したアンケートマスターを保存
     * @param localSurveyMaster 入力したアンケートマスター情報
     */
    update(localSurveyMaster: LocalSurveyMaster) {
      this.isLoadingButton = true

      GetSurveyMasterByIdAndRevision(
        localSurveyMaster.id,
        localSurveyMaster.revision
      ).then((res) => {
        const id: string = res.data.id
        const version: number = res.data.version
        const request = new UpdateSurveyMasterLatestByIdRequest()

        // LocalSurveyMasterクラスとプロパティ構成が違うので明示的に代入する。
        request.name = localSurveyMaster.name
        request.type = localSurveyMaster.type
        request.timing = localSurveyMaster.timing
        request.initSendDaySetting = localSurveyMaster.initSendDaySetting
        request.initAnswerLimitDaySetting =
          localSurveyMaster.initAnswerLimitDaySetting
        request.isDisclosure = localSurveyMaster.isDisclosure
        request.memo = localSurveyMaster.memo

        UpdateSurveyMasterLatestById(id, version, request)
          .then(this.updateThen)
          .catch(() => {
            this.showErrorBarWithScrollPageTop(this.$t('msg.error.default'))
          })
          .finally(() => {
            this.isLoadingButton = false
          })
      })
    },
    /**
     * アンケートひな形設問詳細を表示
     * @param id アンケートマスターID
     * @param revision リビジョン番号
     */
    refer(id: String, revision: number) {
      this.$router.push(`/survey/master/${id}/${revision}`)
    },
    /**
     * アンケートひな形設問詳細を編集
     * @param id アンケートマスターID
     * @param revision リビジョン番号
     */
    edit(id: String, revision: number) {
      this.$router.push(`/survey/master/${id}/${revision}`)
    },
    /**
     * 指定したアンケートマスターを運用中に変更
     * @param id アンケートマスターID
     * @param revision リビジョン番号
     */
    patch(id: String, revision: number) {
      GetSurveyMasterByIdAndRevision(id as string, revision as number).then(
        (res) => {
          const version: number = res.data.version
          PatchSurveyMasterRevisionById(id as string, version as number)
            .then(this.updateThen)
            .catch(() => {
              this.showErrorBarWithScrollPageTop(this.$t('msg.error.default'))
            })
        }
      )
    },
    /**
     * 指定したアンケートマスターをコピー作成
     * @param id アンケートマスターID
     * @param originRevision リビジョン番号
     */
    copy(id: String, originRevision: number) {
      CopySurveyMasterById(id as string, originRevision as number)
        .then(this.updateThen)
        .catch(() => {
          this.showErrorBarWithScrollPageTop(this.$t('msg.error.default'))
        })
    },
    /**
     * 最新のアンケートマスターを返却
     * @param {Array} masters GetSurveyMastersByIdApi取得した全てのアンケートマスター
     * @returns 最新のアンケートマスター情報
     */
    latestSurveyMaster(masters: SurveyMasterListItem[]): SurveyMasterListItem {
      return masters.filter(
        (item: SurveyMasterListItem) =>
          item.isLatest === ENUM_GET_SURVEY_MASTERS_IS_LATEST.TRUE
      )[0]
    },
  },
})
</script>

<style lang="scss" scoped>
.o-survey-master-detail-title {
  margin-bottom: 32px;
}
.o-survey-master-setting-table {
  padding-bottom: 27px;
}
</style>
