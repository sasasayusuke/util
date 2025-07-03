<template>
  <div>
    <!-- その他チェック事項 (体制) -任意- -->
    <MasterKarteDetailSection
      :is-current-program="isCurrentProgram"
      chip
      :title="$t('master-karte.pages.detail.container.others.otherStructure')"
    >
      <!-- 部署のミッション・機能 -->
      <MasterKarteDetailRow
        :subtitle="$t('master-karte.pages.detail.container.others.structure')"
        :label="$t('master-karte.pages.detail.container.others.mission')"
        :text="others.mission"
        :is-current-program="isCurrentProgram"
        :show-current-program="showCurrentProgram"
        :subtext="subtext.mission"
        icon="void"
      >
        <template #editable>
          <Textarea
            v-if="editableRole && !isCurrentProgram"
            v-model="localParam.nextProgram.others.mission"
            style-set="outlined"
            :max-length="100"
            :aria-label="
              $t('master-karte.pages.detail.container.others.labels.mission')
            "
          />
        </template>
        <template #explanation>
          <span v-if="!isCurrentProgram">
            {{
              $t(
                'master-karte.pages.detail.container.others.explanations.mission'
              )
            }}
          </span>
        </template>
      </MasterKarteDetailRow>

      <!-- 人数 -->
      <MasterKarteDetailRow
        :label="$t('master-karte.pages.detail.container.others.numberOfPeople')"
        :text="others.numberOfPeople"
        :is-current-program="isCurrentProgram"
        :show-current-program="showCurrentProgram"
        :subtext="subtext.numberOfPeople"
        icon="void"
      >
        <template #editable>
          <Textarea
            v-if="editableRole && !isCurrentProgram"
            v-model="localParam.nextProgram.others.numberOfPeople"
            style-set="outlined"
            :max-length="100"
            :aria-label="
              $t(
                'master-karte.pages.detail.container.others.labels.numberOfPeople'
              )
            "
          />
        </template>
        <template #explanation>
          <span v-if="!isCurrentProgram">
            {{
              $t(
                'master-karte.pages.detail.container.others.explanations.numberOfPeople'
              )
            }}
          </span>
        </template>
      </MasterKarteDetailRow>

      <!-- 責任者情報 -->
      <MasterKarteDetailRow
        :label="$t('master-karte.pages.detail.container.others.manager')"
        :text="others.manager"
        :is-current-program="isCurrentProgram"
        :show-current-program="showCurrentProgram"
        :subtext="subtext.manager"
        icon="void"
      >
        <template #editable>
          <Textarea
            v-if="editableRole && !isCurrentProgram"
            v-model="localParam.nextProgram.others.manager"
            :max-length="100"
            style-set="outlined"
            :aria-label="
              $t('master-karte.pages.detail.container.others.labels.manager')
            "
          />
        </template>
        <template #explanation>
          <span v-if="!isCurrentProgram">
            {{
              $t(
                'master-karte.pages.detail.container.others.explanations.manager'
              )
            }}
          </span>
        </template>
      </MasterKarteDetailRow>

      <!-- 事業化スキル -->
      <MasterKarteDetailRow
        :label="
          $t(
            'master-karte.pages.detail.container.others.commercializationSkill'
          )
        "
        :text="others.commercializationSkill"
        :is-current-program="isCurrentProgram"
        :show-current-program="showCurrentProgram"
        :subtext="subtext.commercializationSkill"
        icon="void"
      >
        <template #editable>
          <Textarea
            v-if="editableRole && !isCurrentProgram"
            v-model="localParam.nextProgram.others.commercializationSkill"
            style-set="outlined"
            :max-length="100"
            :aria-label="
              $t(
                'master-karte.pages.detail.container.others.labels.commercializationSkill'
              )
            "
          />
        </template>
        <template #explanation>
          <span v-if="!isCurrentProgram">
            {{
              $t(
                'master-karte.pages.detail.container.others.explanations.commercializationSkill'
              )
            }}
          </span>
        </template>
      </MasterKarteDetailRow>

      <!-- パートナー・外注先の有無 -->
      <MasterKarteDetailRow
        :label="$t('master-karte.pages.detail.container.others.existPartners')"
        :text="others.existPartners"
        :is-current-program="isCurrentProgram"
        :show-current-program="showCurrentProgram"
        :subtext="subtext.existPartners"
        icon="void"
      >
        <template #editable>
          <Textarea
            v-if="editableRole && !isCurrentProgram"
            v-model="localParam.nextProgram.others.existPartners"
            style-set="outlined"
            :max-length="100"
            :aria-label="
              $t(
                'master-karte.pages.detail.container.others.labels.existPartners'
              )
            "
          />
        </template>
        <template #explanation>
          <span v-if="!isCurrentProgram">
            {{
              $t(
                'master-karte.pages.detail.container.others.explanations.existPartners'
              )
            }}
          </span>
        </template>
      </MasterKarteDetailRow>
    </MasterKarteDetailSection>

    <!-- その他チェック事項 (組織開発) -任意- -->
    <MasterKarteDetailSection
      :is-current-program="isCurrentProgram"
      chip
      :title="
        $t(
          'master-karte.pages.detail.container.others.otherOrganizationDevelopment'
        )
      "
    >
      <!-- 事務局による支援体制 -->
      <MasterKarteDetailRow
        :label="$t('master-karte.pages.detail.container.others.supportOrder')"
        :text="others.supportOrder"
        :is-current-program="isCurrentProgram"
        :show-current-program="showCurrentProgram"
        :subtext="subtext.supportOrder"
        icon="void"
      >
        <template #editable>
          <Textarea
            v-if="editableRole && !isCurrentProgram"
            v-model="localParam.nextProgram.others.supportOrder"
            style-set="outlined"
            :max-length="100"
            :aria-label="
              $t(
                'master-karte.pages.detail.container.others.labels.supportOrder'
              )
            "
          />
        </template>
        <template #explanation>
          <span v-if="!isCurrentProgram">
            {{
              $t(
                'master-karte.pages.detail.container.others.explanations.supportOrder'
              )
            }}
          </span>
        </template>
      </MasterKarteDetailRow>

      <!-- 事業の提案評価プロセスの有無や課題 -->
      <MasterKarteDetailRow
        :label="
          $t('master-karte.pages.detail.container.others.existEvaluation')
        "
        :text="others.existEvaluation"
        :is-current-program="isCurrentProgram"
        :show-current-program="showCurrentProgram"
        :subtext="subtext.existEvaluation"
        icon="void"
      >
        <template #editable>
          <Textarea
            v-if="editableRole && !isCurrentProgram"
            v-model="localParam.nextProgram.others.existEvaluation"
            style-set="outlined"
            :max-length="100"
            :aria-label="
              $t(
                'master-karte.pages.detail.container.others.labels.existEvaluation'
              )
            "
          />
        </template>
        <template #explanation>
          <span v-if="!isCurrentProgram">
            {{
              $t(
                'master-karte.pages.detail.container.others.explanations.existEvaluation'
              )
            }}
          </span>
        </template>
      </MasterKarteDetailRow>

      <!-- オーディション・社内公募企画の有無や課題 -->
      <MasterKarteDetailRow
        :label="$t('master-karte.pages.detail.container.others.existAudition')"
        :text="others.existAudition"
        :is-current-program="isCurrentProgram"
        :show-current-program="showCurrentProgram"
        :subtext="subtext.existAudition"
        icon="void"
      >
        <template #editable>
          <Textarea
            v-if="editableRole && !isCurrentProgram"
            v-model="localParam.nextProgram.others.existAudition"
            style-set="outlined"
            :max-length="100"
            :aria-label="
              $t(
                'master-karte.pages.detail.container.others.labels.existAudition'
              )
            "
          />
        </template>
        <template #explanation>
          <span v-if="!isCurrentProgram">
            {{
              $t(
                'master-karte.pages.detail.container.others.explanations.existAudition'
              )
            }}
          </span>
        </template>
      </MasterKarteDetailRow>

      <!-- アイディエーション試作の有無や課題 -->
      <MasterKarteDetailRow
        :label="$t('master-karte.pages.detail.container.others.existIdeation')"
        :text="others.existIdeation"
        :is-current-program="isCurrentProgram"
        :show-current-program="showCurrentProgram"
        :subtext="subtext.existIdeation"
        icon="void"
      >
        <template #editable>
          <Textarea
            v-if="editableRole && !isCurrentProgram"
            v-model="localParam.nextProgram.others.existIdeation"
            style-set="outlined"
            :max-length="100"
            :aria-label="
              $t(
                'master-karte.pages.detail.container.others.labels.existIdeation'
              )
            "
          />
        </template>
        <template #explanation>
          <span v-if="!isCurrentProgram">
            {{
              $t(
                'master-karte.pages.detail.container.others.explanations.existIdeation'
              )
            }}
          </span>
        </template>
      </MasterKarteDetailRow>

      <!-- 事業アイデア審査体制の有無や課題 -->
      <MasterKarteDetailRow
        :label="
          $t('master-karte.pages.detail.container.others.existIdeaReview')
        "
        :text="others.existIdeaReview"
        :is-current-program="isCurrentProgram"
        :show-current-program="showCurrentProgram"
        :subtext="subtext.existIdeaReview"
        icon="void"
      >
        <template #editable>
          <Textarea
            v-if="editableRole && !isCurrentProgram"
            v-model="localParam.nextProgram.others.existIdeaReview"
            style-set="outlined"
            :max-length="100"
            :aria-label="
              $t(
                'master-karte.pages.detail.container.others.labels.existIdeaReview'
              )
            "
          />
        </template>
        <template #explanation>
          <span v-if="!isCurrentProgram">
            {{
              $t(
                'master-karte.pages.detail.container.others.explanations.existIdeaReview'
              )
            }}
          </span>
        </template>
      </MasterKarteDetailRow>

      <!-- 予算 -->
      <MasterKarteDetailRow
        :subtitle="
          $t('master-karte.pages.detail.container.others.existSystemAndIssues')
        "
        :label="$t('master-karte.pages.detail.container.others.budget')"
        :text="others.budget"
        :is-current-program="isCurrentProgram"
        :show-current-program="showCurrentProgram"
        :subtext="subtext.budget"
        icon="void"
      >
        <template #editable>
          <Textarea
            v-if="editableRole && !isCurrentProgram"
            v-model="localParam.nextProgram.others.budget"
            style-set="outlined"
            :max-length="100"
            :aria-label="
              $t('master-karte.pages.detail.container.others.labels.budget')
            "
          />
        </template>
        <template #explanation>
          <span v-if="!isCurrentProgram">
            {{
              $t(
                'master-karte.pages.detail.container.others.explanations.budget'
              )
            }}
          </span>
        </template>
      </MasterKarteDetailRow>

      <!-- 人事制度 -->
      <MasterKarteDetailRow
        :label="$t('master-karte.pages.detail.container.others.humanResource')"
        :text="others.humanResource"
        :is-current-program="isCurrentProgram"
        :show-current-program="showCurrentProgram"
        :subtext="subtext.humanResource"
        icon="void"
      >
        <template #editable>
          <Textarea
            v-if="editableRole && !isCurrentProgram"
            v-model="localParam.nextProgram.others.humanResource"
            style-set="outlined"
            :max-length="100"
            :aria-label="
              $t(
                'master-karte.pages.detail.container.others.labels.humanResource'
              )
            "
          />
        </template>
        <template #explanation>
          <span v-if="!isCurrentProgram">
            {{
              $t(
                'master-karte.pages.detail.container.others.explanations.humanResource'
              )
            }}
          </span>
        </template>
      </MasterKarteDetailRow>
    </MasterKarteDetailSection>

    <!-- その他チェック事項 (事業開発) -任意- -->
    <MasterKarteDetailSection
      :is-current-program="isCurrentProgram"
      chip
      :title="
        $t(
          'master-karte.pages.detail.container.others.otherBusinessDevelopment'
        )
      "
    >
      <!-- アイデアはすでにあるか？ -->
      <MasterKarteDetailRow
        :subtitle="
          $t('master-karte.pages.detail.container.others.ideaAndTheme')
        "
        :label="$t('master-karte.pages.detail.container.others.existIdea')"
        :text="others.idea"
        :is-current-program="isCurrentProgram"
        :show-current-program="showCurrentProgram"
        :subtext="subtext.idea"
        icon="void"
      >
        <template #editable>
          <Textarea
            v-if="editableRole && !isCurrentProgram"
            v-model="localParam.nextProgram.others.idea"
            style-set="outlined"
            :max-length="100"
            :aria-label="
              $t('master-karte.pages.detail.container.others.labels.existIdea')
            "
          />
        </template>
      </MasterKarteDetailRow>

      <!-- （アイデアがない場合）テーマはあるか？ -->
      <MasterKarteDetailRow
        :label="$t('master-karte.pages.detail.container.others.existTheme')"
        :text="others.theme"
        :is-current-program="isCurrentProgram"
        :show-current-program="showCurrentProgram"
        :subtext="subtext.theme"
        icon="void"
      >
        <template #editable>
          <Textarea
            v-if="editableRole && !isCurrentProgram"
            v-model="localParam.nextProgram.others.theme"
            style-set="outlined"
            :max-length="100"
            :aria-label="
              $t('master-karte.pages.detail.container.others.labels.existTheme')
            "
          />
        </template>
      </MasterKarteDetailRow>

      <!-- 顧客は誰か？ -->
      <MasterKarteDetailRow
        :subtitle="
          $t('master-karte.pages.detail.container.others.clientAndPainPoint')
        "
        :label="$t('master-karte.pages.detail.container.others.client')"
        :text="others.client"
        :is-current-program="isCurrentProgram"
        :show-current-program="showCurrentProgram"
        :subtext="subtext.client"
        icon="void"
      >
        <template #editable>
          <Textarea
            v-if="editableRole && !isCurrentProgram"
            v-model="localParam.nextProgram.others.client"
            style-set="outlined"
            :max-length="100"
            :aria-label="
              $t('master-karte.pages.detail.container.others.labels.client')
            "
          />
        </template>
      </MasterKarteDetailRow>

      <!-- 顧客の課題は何か？ -->
      <MasterKarteDetailRow
        :label="$t('master-karte.pages.detail.container.others.clientIssue')"
        :text="others.clientIssue"
        :is-current-program="isCurrentProgram"
        :show-current-program="showCurrentProgram"
        :subtext="subtext.clientIssue"
        icon="void"
      >
        <template #editable>
          <Textarea
            v-if="editableRole && !isCurrentProgram"
            v-model="localParam.nextProgram.others.clientIssue"
            style-set="outlined"
            :max-length="100"
            :aria-label="
              $t(
                'master-karte.pages.detail.container.others.labels.clientIssue'
              )
            "
          />
        </template>
      </MasterKarteDetailRow>

      <!-- 顧客に対しどのようなソリューションか？ -->
      <MasterKarteDetailRow
        :subtitle="$t('master-karte.pages.detail.container.others.solution')"
        :label="
          $t('master-karte.pages.detail.container.others.concreteSolution')
        "
        :text="others.solution"
        :is-current-program="isCurrentProgram"
        :show-current-program="showCurrentProgram"
        :subtext="subtext.solution"
        icon="void"
      >
        <template #editable>
          <Textarea
            v-if="editableRole && !isCurrentProgram"
            v-model="localParam.nextProgram.others.solution"
            style-set="outlined"
            :max-length="100"
            :aria-label="
              $t(
                'master-karte.pages.detail.container.others.labels.concreteSolution'
              )
            "
          />
        </template>
      </MasterKarteDetailRow>

      <!-- 独自性は何か？ -->
      <MasterKarteDetailRow
        :label="$t('master-karte.pages.detail.container.others.originality')"
        :text="others.originality"
        :is-current-program="isCurrentProgram"
        :show-current-program="showCurrentProgram"
        :subtext="subtext.originality"
        icon="void"
      >
        <template #editable>
          <Textarea
            v-if="editableRole && !isCurrentProgram"
            v-model="localParam.nextProgram.others.originality"
            style-set="outlined"
            :max-length="100"
            :aria-label="
              $t(
                'master-karte.pages.detail.container.others.labels.originality'
              )
            "
          />
        </template>
      </MasterKarteDetailRow>

      <!-- 必要不可欠な特徴・機能(MVP)は何か？ -->
      <MasterKarteDetailRow
        :label="$t('master-karte.pages.detail.container.others.mvp')"
        :text="others.mvp"
        :is-current-program="isCurrentProgram"
        :show-current-program="showCurrentProgram"
        :subtext="subtext.mvp"
        icon="void"
      >
        <template #editable>
          <Textarea
            v-if="editableRole && !isCurrentProgram"
            v-model="localParam.nextProgram.others.mvp"
            style-set="outlined"
            :max-length="100"
            :aria-label="
              $t('master-karte.pages.detail.container.others.labels.mvp')
            "
          />
        </template>
      </MasterKarteDetailRow>

      <!-- 提供サービスの市場(TAM)の金額規模と将来の伸び率は？ -->
      <MasterKarteDetailRow
        :subtitle="
          $t('master-karte.pages.detail.container.others.marketability')
        "
        :label="$t('master-karte.pages.detail.container.others.tam')"
        :text="others.tam"
        :is-current-program="isCurrentProgram"
        :show-current-program="showCurrentProgram"
        :subtext="subtext.tam"
        icon="void"
      >
        <template #editable>
          <Textarea
            v-if="editableRole && !isCurrentProgram"
            v-model="localParam.nextProgram.others.tam"
            style-set="outlined"
            :max-length="100"
            :aria-label="
              $t('master-karte.pages.detail.container.others.labels.tam')
            "
          />
        </template>
      </MasterKarteDetailRow>

      <!-- ターゲットセグメント(SAM)の金額規模と将来の伸び率は？ -->
      <MasterKarteDetailRow
        :label="$t('master-karte.pages.detail.container.others.sam')"
        :text="others.sam"
        :is-current-program="isCurrentProgram"
        :show-current-program="showCurrentProgram"
        :subtext="subtext.sam"
        icon="void"
      >
        <template #editable>
          <Textarea
            v-if="editableRole && !isCurrentProgram"
            v-model="localParam.nextProgram.others.sam"
            style-set="outlined"
            :max-length="100"
            :aria-label="
              $t('master-karte.pages.detail.container.others.labels.sam')
            "
          />
        </template>
      </MasterKarteDetailRow>

      <!-- 市場投入のタイミングは適切か？ -->
      <MasterKarteDetailRow
        :subtitle="$t('master-karte.pages.detail.container.others.timing')"
        :label="$t('master-karte.pages.detail.container.others.isRightTime')"
        :text="others.isRightTime"
        :is-current-program="isCurrentProgram"
        :show-current-program="showCurrentProgram"
        :subtext="subtext.isRightTime"
        icon="void"
      >
        <template #editable>
          <Textarea
            v-if="editableRole && !isCurrentProgram"
            v-model="localParam.nextProgram.others.isRightTime"
            style-set="outlined"
            :max-length="100"
            :aria-label="
              $t(
                'master-karte.pages.detail.container.others.labels.isRightTime'
              )
            "
          />
        </template>
      </MasterKarteDetailRow>

      <!-- 事業化後の長期戦略やロードマップはあるか？ -->
      <MasterKarteDetailRow
        :label="$t('master-karte.pages.detail.container.others.roadMap')"
        :text="others.roadMap"
        :subtitle="$t('master-karte.pages.detail.container.others.longTerm')"
        :is-current-program="isCurrentProgram"
        :show-current-program="showCurrentProgram"
        :subtext="subtext.roadMap"
        icon="void"
      >
        <template #editable>
          <Textarea
            v-if="editableRole && !isCurrentProgram"
            v-model="localParam.nextProgram.others.roadMap"
            style-set="outlined"
            :max-length="100"
            :aria-label="
              $t('master-karte.pages.detail.container.others.labels.roadMap')
            "
          />
        </template>
      </MasterKarteDetailRow>
    </MasterKarteDetailSection>
  </div>
</template>

<script lang="ts">
import DetailContainer from '~/components/common/organisms/DetailContainer.vue'
import CommonDetailContainer from '~/components/common/organisms/CommonDetailContainer.vue'
import MasterKarteDetailRow from '~/components/master-karte/molecules/MasterKarteDetailRow.vue'
import { GetMasterKarteByIdResponse, Others } from '~/types/MasterKarte'
import type { PropType } from '~/common/BaseComponent'
import {
  OthersClass,
  UpdateMasterKarteResponseClass,
} from '~/models/MasterKarte'
import { currentPageDataStore, meStore } from '~/store'
import { ENUM_USER_ROLE } from '~/types/User'
import { Textarea } from '~/components/common/atoms'

export default CommonDetailContainer.extend({
  components: {
    DetailContainer,
    MasterKarteDetailRow,
    Textarea,
  },
  props: {
    isCurrentProgram: {
      type: Boolean,
      default: true,
    },
    masterKarteProject: {
      type: Object as PropType<GetMasterKarteByIdResponse>,
    },
    /** 当期支援内容のチェックボックスがチェック済か */
    showCurrentProgram: {
      type: Boolean,
      default: true,
    },
    localParam: {
      type: Object as PropType<UpdateMasterKarteResponseClass>,
    },
  },
  computed: {
    others(): Others {
      let program = this.isCurrentProgram
        ? this.masterKarteProject.currentProgram.others
        : this.masterKarteProject.nextProgram.others
      if (program === null) {
        program = new OthersClass()
      }
      return program
    },
    // 次期支援選択時に右に表示される当期支援
    subtext(): Others {
      return this.showCurrentProgram
        ? this.masterKarteProject.currentProgram.others
        : new OthersClass()
    },
    /**
     * 編集可能かどうか
     * 案件を担当している支援者、営業ロールのみ編集可能
     **/
    editableRole(): boolean {
      if (
        meStore.role === ENUM_USER_ROLE.SUPPORTER ||
        meStore.role === ENUM_USER_ROLE.SALES ||
        meStore.role === ENUM_USER_ROLE.SUPPORTER_MGR ||
        meStore.role === ENUM_USER_ROLE.SALES_MGR ||
        meStore.role === ENUM_USER_ROLE.BUSINESS_MGR
      ) {
        // 担当案件
        if (meStore.projectIds?.includes(currentPageDataStore.projectId)) {
          return true
        }
      }
      return false
    },
  },
})
</script>

<style></style>
