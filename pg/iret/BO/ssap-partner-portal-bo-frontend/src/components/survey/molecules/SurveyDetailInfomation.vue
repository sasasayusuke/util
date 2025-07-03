<template>
  <div>
    <Sheet style-set="text-box2">
      <v-container fluid pa-0>
        <v-row>
          <v-col cols="4">
            <Title style-set="xsmall" hx="h3" class="mb-3">
              {{ $t('survey.pages.detail.infomation.company') }}
            </Title>
            <Paragraph style-set="small" class="mb-3">
              {{ survey.company ? survey.company : survey.customerName }}
            </Paragraph>
            <v-divider color="#8F8F8F" />
          </v-col>
          <v-col cols="4">
            <Title style-set="xsmall" hx="h3" class="mb-3">
              {{ $t('survey.pages.detail.infomation.projectName') }}
            </Title>
            <Paragraph style-set="small" class="mb-3">
              <router-link
                class="font-weight-bold"
                :to="`/project/${survey.projectId}`"
                >{{ survey.projectName }}</router-link
              >
            </Paragraph>
            <v-divider color="#8F8F8F" />
          </v-col>
          <v-col cols="4">
            <Title style-set="xsmall" hx="h3" class="mb-3">
              {{ $t('survey.pages.detail.infomation.customerName') }}
            </Title>
            <Paragraph style-set="small" class="mb-3">
              {{ survey.answerUserName }}
            </Paragraph>
            <v-divider color="#8F8F8F" />
          </v-col>
        </v-row>
        <v-row v-if="isShowAll">
          <v-col cols="4">
            <Title style-set="xsmall" hx="h3" class="mb-3">
              {{ $t('survey.pages.detail.infomation.mainSupporterUserName') }}
            </Title>
            <Paragraph style-set="small" class="mb-3">
              {{
                survey.mainSupporterUser ? survey.mainSupporterUser.name : ''
              }}
            </Paragraph>
            <v-divider color="#8F8F8F" />
          </v-col>
          <v-col cols="4">
            <Title style-set="xsmall" hx="h3" class="mb-3">
              {{ $t('survey.pages.detail.infomation.supporters') }}
            </Title>
            <Paragraph style-set="small" class="mb-3">
              {{
                survey.supporterUsers
                  ? survey.supporterUsers.map((elm) => elm.name).join(' ／ ')
                  : ''
              }}
            </Paragraph>
            <v-divider color="#8F8F8F" />
          </v-col>
          <v-col cols="4">
            <Title style-set="xsmall" hx="h3" class="mb-3">
              {{ $t('survey.pages.detail.infomation.salesUserName') }}
            </Title>
            <Paragraph style-set="small" class="mb-3">
              {{ survey.salesUserName }}
            </Paragraph>
            <v-divider color="#8F8F8F" />
          </v-col>
        </v-row>
        <v-row v-if="isShowAll">
          <v-col cols="4">
            <Title style-set="xsmall" hx="h3" class="mb-3">
              {{ $t('survey.pages.detail.infomation.surveyType') }}
            </Title>
            <Paragraph style-set="small" class="mb-3">
              {{ survey.serviceTypeName }}
            </Paragraph>
            <v-divider color="#8F8F8F" />
          </v-col>
          <v-col cols="4">
            <Title style-set="xsmall" hx="h3" class="mb-3">
              {{ $t('survey.pages.detail.infomation.summaryMonth') }}
            </Title>
            <Paragraph style-set="small" class="mb-3">
              {{ surveyPeriod }}
            </Paragraph>
            <v-divider color="#8F8F8F" />
          </v-col>
        </v-row>
        <v-row>
          <v-col>
            <Title style-set="xsmall" hx="h3" class="mb-3">
              {{ $t('survey.pages.detail.infomation.customerSuccess') }}
            </Title>
            <!-- eslint-disable vue/no-v-html -->
            <Paragraph
              style-set="small"
              class="mb-0"
              v-html="
                $sanitize(
                  typeof survey.customerSuccess === 'string'
                    ? survey.customerSuccess.replace(/\r?\n/g, '<br />')
                    : survey.customerSuccess
                )
              "
            >
            </Paragraph>
            <!-- eslint-enable -->
          </v-col>
        </v-row>
      </v-container>
    </Sheet>
    <Button style-set="showAll" class="mt-3" @click="isShowAll = !isShowAll">
      <Icon size="16" class="mr-2" :class="{ 'is-open': isShowAll }"
        >icon-org-arrow-down</Icon
      >
      {{ isShowAll ? $t('common.button.close') : $t('common.button.showAll') }}
    </Button>
  </div>
</template>

<script lang="ts">
import { format } from 'date-fns'
import { ENUM_GET_SURVEY_TYPE } from '~/models/Survey'
import BaseComponent from '~/common/BaseComponent'
import {
  Sheet,
  Title,
  Paragraph,
  Button,
  Icon,
  TextLink,
} from '~/components/common/atoms'

export default BaseComponent.extend({
  components: {
    Sheet,
    Title,
    Paragraph,
    Button,
    Icon,
    TextLink,
  },
  props: {
    /** アンケート情報 */
    survey: {
      type: Object,
      required: true,
    },
  },
  data() {
    return {
      isValid: true,
      isShowAll: false,
      surveyPeriod:
        this.survey.surveyType === ENUM_GET_SURVEY_TYPE.COMPLETION
          ? format(
              new Date(this.survey.supportDateFrom),
              this.$t('common.format.date_ymd2') as string
            ) +
            '～' +
            format(
              new Date(this.survey.supportDateTo),
              this.$t('common.format.date_ymd2') as string
            )
          : format(
              new Date(this.survey.actualSurveyRequestDatetime),
              this.$t('common.format.date_ym2') as string
            ),
    }
  },
})
</script>

<style lang="scss" scoped>
.v-icon {
  &.is-open {
    transform: rotate(180deg);
    transition-duration: 0.3s;
  }
}
.container {
  .row > .col {
    position: relative;
    p {
      min-height: 21px;
    }
    hr {
      position: absolute;
      width: calc(100% - 24px);
      bottom: 12px;
    }
  }
}
</style>
