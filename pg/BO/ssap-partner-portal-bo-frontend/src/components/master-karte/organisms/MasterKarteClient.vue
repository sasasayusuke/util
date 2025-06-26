<template>
  <div>
    <MasterKarteDetailSection
      chip
      :title="
        $t(
          'master-karte.pages.detail.container.companyDepartment.clientInformation'
        )
      "
    >
      <!-- お客様名 -->
      <MasterKarteDetailRow
        :label="
          $t(
            'master-karte.pages.detail.container.companyDepartment.customerName'
          )
        "
        :text="companyDepartment.customerName"
        icon="void"
      >
      </MasterKarteDetailRow>

      <!-- URL -->
      <MasterKarteDetailRow
        :label="
          $t(
            'master-karte.pages.detail.container.companyDepartment.customerUrl'
          )
        "
        :text="companyDepartment.customerUrl"
        icon="void"
      >
      </MasterKarteDetailRow>

      <!-- 顧客セグメント -->
      <MasterKarteDetailRow
        :label="
          $t(
            'master-karte.pages.detail.container.companyDepartment.customerSegment'
          )
        "
        :text="companyDepartment.category"
        icon="void"
      >
      </MasterKarteDetailRow>

      <!-- 設立 -->
      <MasterKarteDetailRow
        :label="
          $t(
            'master-karte.pages.detail.container.companyDepartment.establishment'
          )
        "
        :text="companyDepartment.establishment"
        icon="void"
      >
      </MasterKarteDetailRow>

      <!-- 従業員 -->
      <MasterKarteDetailRow
        :label="
          $t('master-karte.pages.detail.container.companyDepartment.employee')
        "
        :text="companyDepartment.employee ? companyDepartment.employee : ''"
        icon="void"
      >
      </MasterKarteDetailRow>

      <!-- 資本金 -->
      <MasterKarteDetailRow
        :label="
          $t(
            'master-karte.pages.detail.container.companyDepartment.capitalStock'
          )
        "
        :text="
          companyDepartment.capitalStock ? companyDepartment.capitalStock : ''
        "
        icon="void"
      >
      </MasterKarteDetailRow>
      <MasterKarteDetailRow
        :label="
          $t(
            'master-karte.pages.detail.container.companyDepartment.businessSummary'
          )
        "
        :text="companyDepartment.businessSummary"
        icon="void"
      >
      </MasterKarteDetailRow>

      <!-- 業界セグメント -->
      <MasterKarteDetailRow
        :label="
          $t(
            'master-karte.pages.detail.container.companyDepartment.industrySegment'
          )
        "
        :text="companyDepartment.industrySegment"
        icon="void"
      >
      </MasterKarteDetailRow>
    </MasterKarteDetailSection>

    <MasterKarteDetailSection
      chip
      :title="
        $t(
          'master-karte.pages.detail.container.companyDepartment.departmentInformation'
        )
      "
    >
      <!-- 部署名 -->
      <MasterKarteDetailRow
        :label="
          $t(
            'master-karte.pages.detail.container.companyDepartment.departmentName'
          )
        "
        :text="companyDepartment.departmentName"
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
import { CompanyDepartment } from '~/types/MasterKarte'

export default CommonDetailContainer.extend({
  components: {
    DetailContainer,
    MasterKarteDetailRow,
  },
  props: {
    masterKarteProject: {
      type: Object,
      required: true,
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
    companyDepartment(): CompanyDepartment {
      return this.masterKarteProject.currentProgram.companyDepartment
    },
  },
})
</script>

<style></style>
