<template>
  <MonthlyProjectsDataTable
    v-if="!isLoading"
    :headers="formattedHeader()"
    :items="formattedManHours"
    :total="manHours.length"
    :is-loading="isLoading"
    is-show-total
    @csvOutput="$emit('csvOutput')"
  >
  </MonthlyProjectsDataTable>
</template>
<script lang="ts">
import MonthlyProjectsDataTable from '~/components/man-hour/molecules/MonthlyProjectsDataTable.vue'
import CommonDataTable from '~/components/common/organisms/CommonDataTable.vue'
import BaseComponent from '~/common/BaseComponent'

export default BaseComponent.extend({
  name: 'SummaryManHourListTable',
  components: {
    MonthlyProjectsDataTable,
    CommonDataTable,
  },
  props: {
    /** 月次工数分類別工数一覧 */
    manHours: {
      type: Array,
      required: true,
    },
    /** ヘッダ情報 */
    header: {
      type: Array,
      required: true,
    },
    /** 読み込み中判定 */
    isLoading: {
      type: Boolean,
    },
  },
  mounted() {},
  methods: {
    /** ヘッダ情報の整形
     * @returns 整形されたヘッダ情報
     */
    formattedHeader() {
      const formattedManHourHeader: any = [
        [
          {
            text: this.$t('man-hour.pages.report.index.header.division1'),
            value: 'manHourTypeName',
            sortable: false,
            rowspan: 2,
            width: '100px',
          },
          {
            text: this.$t('man-hour.pages.report.index.header.division2'),
            value: 'subName',
            sortable: false,
            maxLength: 29,
            rowspan: 2,
          },
          {
            text: this.$t('man-hour.pages.report.index.header.category'),
            value: 'serviceTypeName',
            sortable: false,
            rowspan: 2,
          },
          {
            text: this.$t('man-hour.pages.report.index.header.role'),
            value: 'roleName',
            sortable: false,
            rowspan: 2,
          },
        ],
        [],
      ]
      const supporterOrganizationManHours =
        //@ts-ignore
        this.header.supporterOrganizationManHours
      //@ts-ignore
      const supporterOrganizationTotal = this.header.supporterOrganizationTotal
      //合計の描画
      const totalCount = supporterOrganizationTotal.length
      const headerTotal: any = {
        text: this.$t('man-hour.pages.report.index.header.total'),
        value: '',
        sortable: false,
        colspan: totalCount + 1,
      }
      formattedManHourHeader[0].push(headerTotal)
      //合計の子要素supporterOrganization
      supporterOrganizationTotal.map((elm: any) => {
        const totalChild: any = {
          text: elm.supporterOrganizationName,
          align: 'start',
          value: elm.supporterOrganizationId + 'Total',
          sortable: false,
        }
        return formattedManHourHeader[1].push(totalChild)
      })
      //合計の子要素の合計
      const totalChidrenTotal: any = {
        text: this.$t('man-hour.pages.report.index.header.total'),
        align: 'start',
        value: 'totalAmount',
        sortable: false,
      }
      formattedManHourHeader[1].push(totalChidrenTotal)
      //各supporterOrganization
      supporterOrganizationManHours.map((elm: any) => {
        const supportersCount = elm.supporters.length
        const supOrgs: any = {
          text: elm.supporterOrganizationName,
          value: elm.supporterOrganizationId,
          sortable: false,
          colspan: supportersCount,
        }
        elm.supporters.map((e: any) => {
          const totalChild: any = {
            text: e.name,
            align: 'start',
            value: elm.supporterOrganizationId + e.id + 'Member',
            sortable: false,
          }
          return formattedManHourHeader[1].push(totalChild)
        })
        return formattedManHourHeader[0].push(supOrgs)
      })
      return formattedManHourHeader
    },
  },
  computed: {
    // 表の表示形式を揃えるための変換は、ListTable organismで行う
    // (上流で元データを加工しないように)
    formattedManHours() {
      return this.manHours.map((manHour: any) => {
        let totalAmount = 0
        //子要素supporterOrganizationのvalue
        manHour.supporterOrganizationTotal.map((elm: any) => {
          totalAmount = totalAmount + elm.manHour
          return (manHour[elm.supporterOrganizationId + 'Total'] = elm.manHour)
        })
        manHour.totalAmount = totalAmount
        //子要素supportersのvalue
        manHour.supporterOrganizationManHours.map((elm: any) => {
          elm.supporters.map((e: any) => {
            return (manHour[elm.supporterOrganizationId + e.id + 'Member'] =
              e.manHour)
          })
          return manHour
        })
        return manHour
      })
    },
  },
})
</script>

<style lang="scss"></style>
