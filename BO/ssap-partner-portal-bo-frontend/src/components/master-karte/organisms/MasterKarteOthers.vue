<template>
  <div>
    <MasterKarteDetailSection
      :chip="isCurrentProgram"
      :title="$t('master-karte.pages.detail.container.others.otherStructure')"
    >
      <MasterKarteDetailRow
        :subtitle="$t('master-karte.pages.detail.container.others.structure')"
        :label="$t('master-karte.pages.detail.container.others.mission')"
        :text="others.mission"
        :is-current-program="isCurrentProgram"
        :show-current-program="showCurrentProgram"
        :subtext="subtext.mission"
        icon="void"
      >
      </MasterKarteDetailRow>
      <MasterKarteDetailRow
        :label="$t('master-karte.pages.detail.container.others.numberOfPeople')"
        :text="others.numberOfPeople"
        :is-current-program="isCurrentProgram"
        :show-current-program="showCurrentProgram"
        :subtext="subtext.numberOfPeople"
        icon="void"
      >
      </MasterKarteDetailRow>
      <MasterKarteDetailRow
        :label="$t('master-karte.pages.detail.container.others.manager')"
        :text="others.manager"
        :is-current-program="isCurrentProgram"
        :show-current-program="showCurrentProgram"
        :subtext="subtext.manager"
        icon="void"
      >
      </MasterKarteDetailRow>
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
      </MasterKarteDetailRow>
      <MasterKarteDetailRow
        :label="$t('master-karte.pages.detail.container.others.existPartners')"
        :text="others.existPartners"
        :is-current-program="isCurrentProgram"
        :show-current-program="showCurrentProgram"
        :subtext="subtext.existPartners"
        icon="void"
      >
      </MasterKarteDetailRow>
    </MasterKarteDetailSection>
    <MasterKarteDetailSection
      :chip="isCurrentProgram"
      :title="
        $t(
          'master-karte.pages.detail.container.others.otherOrganizationDevelopment'
        )
      "
    >
      <MasterKarteDetailRow
        :label="$t('master-karte.pages.detail.container.others.supportOrder')"
        :text="others.supportOrder"
        :is-current-program="isCurrentProgram"
        :show-current-program="showCurrentProgram"
        :subtext="subtext.supportOrder"
        icon="void"
      >
      </MasterKarteDetailRow>
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
      </MasterKarteDetailRow>
      <MasterKarteDetailRow
        :label="$t('master-karte.pages.detail.container.others.existAudition')"
        :text="others.existAudition"
        :is-current-program="isCurrentProgram"
        :show-current-program="showCurrentProgram"
        :subtext="subtext.existAudition"
        icon="void"
      >
      </MasterKarteDetailRow>
      <MasterKarteDetailRow
        :label="$t('master-karte.pages.detail.container.others.existIdeation')"
        :text="others.existIdeation"
        :is-current-program="isCurrentProgram"
        :show-current-program="showCurrentProgram"
        :subtext="subtext.existIdeation"
        icon="void"
      >
      </MasterKarteDetailRow>
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
      </MasterKarteDetailRow>
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
      </MasterKarteDetailRow>
      <MasterKarteDetailRow
        :label="$t('master-karte.pages.detail.container.others.humanResource')"
        :text="others.humanResource"
        :is-current-program="isCurrentProgram"
        :show-current-program="showCurrentProgram"
        :subtext="subtext.humanResource"
        icon="void"
      >
      </MasterKarteDetailRow>
    </MasterKarteDetailSection>
    <MasterKarteDetailSection
      :chip="isCurrentProgram"
      :title="
        $t(
          'master-karte.pages.detail.container.others.otherBusinessDevelopment'
        )
      "
    >
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
      </MasterKarteDetailRow>
      <MasterKarteDetailRow
        :label="$t('master-karte.pages.detail.container.others.existTheme')"
        :text="others.theme"
        :is-current-program="isCurrentProgram"
        :show-current-program="showCurrentProgram"
        :subtext="subtext.theme"
        icon="void"
      >
      </MasterKarteDetailRow>
      <!-- 顧客・ペインポイント -->
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
      </MasterKarteDetailRow>
      <MasterKarteDetailRow
        :label="$t('master-karte.pages.detail.container.others.clientIssue')"
        :text="others.clientIssue"
        :is-current-program="isCurrentProgram"
        :show-current-program="showCurrentProgram"
        :subtext="subtext.clientIssue"
        icon="void"
      >
      </MasterKarteDetailRow>
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
      </MasterKarteDetailRow>
      <MasterKarteDetailRow
        :label="$t('master-karte.pages.detail.container.others.originality')"
        :text="others.originality"
        :is-current-program="isCurrentProgram"
        :show-current-program="showCurrentProgram"
        :subtext="subtext.originality"
        icon="void"
      >
      </MasterKarteDetailRow>
      <MasterKarteDetailRow
        :label="$t('master-karte.pages.detail.container.others.mvp')"
        :text="others.mvp"
        :is-current-program="isCurrentProgram"
        :show-current-program="showCurrentProgram"
        :subtext="subtext.mvp"
        icon="void"
      >
      </MasterKarteDetailRow>
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
      </MasterKarteDetailRow>
      <MasterKarteDetailRow
        :label="$t('master-karte.pages.detail.container.others.sam')"
        :text="others.sam"
        :is-current-program="isCurrentProgram"
        :show-current-program="showCurrentProgram"
        :subtext="subtext.sam"
        icon="void"
      >
      </MasterKarteDetailRow>
      <MasterKarteDetailRow
        :subtitle="$t('master-karte.pages.detail.container.others.timing')"
        :label="$t('master-karte.pages.detail.container.others.isRightTime')"
        :text="others.isRightTime"
        :is-current-program="isCurrentProgram"
        :show-current-program="showCurrentProgram"
        :subtext="subtext.isRightTime"
        icon="void"
      >
      </MasterKarteDetailRow>
      <MasterKarteDetailRow
        :subtitle="$t('master-karte.pages.detail.container.others.longTerm')"
        :label="$t('master-karte.pages.detail.container.others.roadMap')"
        :text="others.roadMap"
        :is-current-program="isCurrentProgram"
        :show-current-program="showCurrentProgram"
        :subtext="subtext.roadMap"
        icon="void"
      >
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
import { OthersClass } from '~/models/MasterKarte'

export default CommonDetailContainer.extend({
  components: {
    DetailContainer,
    MasterKarteDetailRow,
  },
  props: {
    isCurrentProgram: {
      type: Boolean,
      default: true,
    },
    masterKarteProject: {
      type: Object as PropType<GetMasterKarteByIdResponse>,
    },
    showCurrentProgram: {
      type: Boolean,
      default: true,
    },
    isLoading: {
      type: Boolean,
      default: false,
    },
  },
  data() {
    return {}
  },
  computed: {
    others(): Others {
      return this.isCurrentProgram
        ? this.masterKarteProject.currentProgram.others
        : this.masterKarteProject.nextProgram.others
    },
    // 次期支援選択時に右に表示される当期支援
    subtext(): Others {
      return this.showCurrentProgram
        ? this.masterKarteProject.currentProgram.others
        : new OthersClass()
    },
  },
})
</script>

<style></style>
