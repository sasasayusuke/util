<template>
  <Sheet class="o-karte-information" style-set="information">
    <v-container pa-0>
      <!-- 支援期間 -->
      <v-row class="o-karte-information__unit">
        <v-col cols="auto" class="pb-0">
          <p class="o-karte-information__label">
            {{ $t('karte.pages.list.information.supportDateFromTo') }}
          </p>
        </v-col>
        <v-col class="pb-0">
          {{ project.supportDateFrom }} 〜 {{ project.supportDateTo }}
        </v-col>
        <v-col class="d-flex justify-end pb-0">
          <Button
            v-if="isCustomer === false"
            outlined
            style-set="small-primary"
            :to="forwardToUrl(`/project/${project.id}`)"
          >
            {{ $t('karte.pages.detail.project') }}
          </Button>
        </v-col>
      </v-row>
      <!-- お客様メンバー -->
      <v-row class="o-karte-information__unit">
        <v-col cols="auto">
          <p class="o-karte-information__label">
            {{ $t('karte.pages.list.information.customerUsers') }}
          </p>
        </v-col>
        <v-col class="pt-1">
          <ul class="o-karte-information__list">
            <!-- お客様代表 -->
            <li v-if="project.mainCustomerUserName" class="pt-2">
              <Icon style-set="primary" size="20" class="mr-2">
                icon-org-user-outline
              </Icon>
              {{ project.mainCustomerUserName }}
            </li>
            <!-- お客様 -->
            <li
              v-for="user in project.customerUsers"
              :key="user.id"
              class="pt-2"
            >
              <Icon style-set="primary" size="20" class="mr-2">
                icon-org-user-outline
              </Icon>
              {{ user.name }}
            </li>
          </ul>
        </v-col>
      </v-row>
      <!-- サービス責任者 -->
      <v-row class="o-karte-information__unit">
        <v-col cols="auto">
          <p class="o-karte-information__label">
            {{ $t('karte.pages.list.information.serviceManager') }}
          </p>
        </v-col>
        <v-col class="pt-1">
          <ul class="o-karte-information__list">
            <li v-if="project.serviceManagerName" class="pt-2">
              <Icon style-set="primary" size="20" class="mr-2">
                icon-org-user
              </Icon>
              {{ project.serviceManagerName }}
            </li>
          </ul>
        </v-col>
      </v-row>
      <!-- プロデューサー -->
      <v-row class="o-karte-information__unit">
        <v-col cols="auto">
          <p class="o-karte-information__label">
            {{ $t('karte.pages.list.information.mainSupporter') }}
          </p>
        </v-col>
        <v-col class="pt-1">
          <ul class="o-karte-information__list">
            <li v-if="project.mainSupporterUserName" class="pt-2">
              <Icon style-set="primary" size="20" class="mr-2">
                icon-org-user
              </Icon>
              {{ project.mainSupporterUserName }}
            </li>
          </ul>
        </v-col>
      </v-row>
      <!-- アクセラレーター -->
      <v-row class="o-karte-information__unit">
        <v-col cols="auto">
          <p class="o-karte-information__label">
            {{ $t('karte.pages.list.information.supporters') }}
          </p>
        </v-col>
        <v-col class="pt-1">
          <ul class="o-karte-information__list">
            <li
              v-for="user in project.supporterUsers"
              :key="user.id"
              class="pt-2"
            >
              <Icon style-set="primary" size="20" class="mr-2">
                icon-org-user
              </Icon>
              {{ user.name }}
            </li>
          </ul>
        </v-col>
      </v-row>
      <!-- カスタマーサクセス -->
      <v-row class="o-karte-information__unit">
        <v-col>
          <v-col class="o-karte-information-customer-success">
            <v-col class="o-karte-information-customer-success__title mb-3">
              <Icon style-set="primary" size="25" class="mr-1">
                icon-org-flag-variant
              </Icon>
              <p class="o-karte-information__label">
                {{ $t('karte.pages.list.information.customerSuccess') }}
              </p>
            </v-col>
            <!-- eslint-disable vue/no-v-html -->
            <Sheet
              style-set="text-box"
              v-html="
                $sanitize(
                  typeof project.customerSuccess === 'string'
                    ? project.customerSuccess.replace(/\r?\n/g, '<br />')
                    : project.customerSuccess
                )
              "
            >
            </Sheet>
            <!-- eslint-enable -->
          </v-col>
        </v-col>
      </v-row>
    </v-container>
  </Sheet>
  <!-- <Icon style-set="primary" size="20">icon-org-user</Icon> -->
</template>

<script lang="ts">
import { Icon, Sheet, Button } from '~/components/common/atoms'
import { GetProjectByIdResponse } from '~/models/Project'
import BaseComponent from '~/common/BaseComponent'
import type { PropType } from '~/common/BaseComponent'

export default BaseComponent.extend({
  name: 'KarteInformation',
  components: {
    Icon,
    Sheet,
    Button,
  },
  props: {
    /**
     * プロジェクト情報
     */
    project: {
      type: Object as PropType<GetProjectByIdResponse>,
      required: true,
    },
    /**
     * 顧客ロールか否か
     */
    isCustomer: {
      type: Boolean,
      default: false,
    },
  },
})
</script>

<style lang="scss" scoped>
.o-karte-information {
  font-size: 0.875rem;
}
.o-karte-information__unit {
  .col-auto {
    width: 175px;
  }
}
.o-karte-information__label {
  font-weight: bold;
  margin: 0;
}
.o-karte-information__list {
  padding: 0;
  margin: 0;
  list-style: none;
  display: flex;
  flex-wrap: wrap;
  li {
    margin-right: 16px;
    display: flex;
    align-items: center;
  }
}
.o-karte-information-customer-success {
  background-color: #ebf7ed;
  border-radius: 4px;
  padding: 12px;
  .o-karte-information__label {
    color: #008a19 !important;
  }
  .v-sheet {
    background-color: #ebf7ed !important;
    padding: 0 !important;
  }
}
.o-karte-information-customer-success__title {
  display: flex;
  align-items: center;
  padding: 0 !important;
}
</style>
