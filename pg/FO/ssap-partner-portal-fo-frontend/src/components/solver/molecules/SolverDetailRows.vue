<template>
  <v-form
    ref="form"
    v-model="isValid"
    class="o-detail-rows"
    @input="$listeners['input']"
  >
    <!-- 最終更新者 -->
    <template v-if="!isNew">
      <LastUpdate
        :user="solverCandidate.updateUserName"
        :date="solverCandidate.updateAt"
        :show-term="false"
      />
    </template>
    <!-- 応募案件 -->
    <CommonDetailRow
      v-if="!isNew"
      :label="$t('solver.row.applicationProject.name')"
      :is-editing="isEditing"
      :value="solverCandidate.name"
      :component="true"
      :tall="true"
    >
      <Sheet style="width: 100%">
        <v-container class="p-3 pb-4" style="background-color: #f0f0f0">
          <v-row>
            <v-col class="pb-0 font-weight-bold">
              {{ $t('solver.row.applicationProject.appliedProject') }}
            </v-col>
          </v-row>
          <template
            v-if="
              solverCandidate.solverApplications &&
              solverCandidate.solverApplications.length > 0
            "
          >
            <template
              v-for="(project, index) in solverCandidate.solverApplications"
            >
              <div :key="index">
                <v-row class="py-2">
                  <v-col class="d-flex pt-2 pb-0">
                    <div class="pr-7 pt-2">{{ project.id }}</div>
                    <div class="pt-2">{{ project.name }}</div>
                  </v-col>
                  <v-col
                    v-if="isEditing"
                    cols="auto"
                    class="justify-end pt-3 pb-0"
                  >
                    <Button
                      outlined
                      style-set="small-tertiary"
                      width="100"
                      @click="deleteSolverApplication(project.id)"
                    >
                      {{
                        $t(
                          'solver.row.applicationProject.button.deleteAplication'
                        )
                      }}
                    </Button>
                  </v-col>
                </v-row>
                <v-row
                  v-if="
                    index !== solverCandidate.solverApplications.length - 1 &&
                    isEditing
                  "
                  :key="index"
                  style="border-top: solid 1px #cbcbcb"
                  class="mx-0"
                ></v-row>
              </div>
            </template>
          </template>
          <template v-else>
            <v-row>
              <v-col class="pb-0">
                <span style="color: #666666">
                  {{ $t('solver.row.applicationProject.noApplicantsMessage') }}
                </span>
              </v-col>
            </v-row>
          </template>
        </v-container>
      </Sheet>
    </CommonDetailRow>
    <!-- 個人ソルバー名 -->
    <CommonDetailRow
      v-if="isNew"
      :label="$t('solver.row.individualSolverName.name')"
      :is-editing="isEditing"
      :value="solverCandidate.name"
      required
    >
      <Sheet style="width: 100%">
        <AutoComplete
          v-model="solverCandidate.name"
          :items="suggestIndividualSolverName"
          item-text="text"
          item-value="value"
          :max-length="80"
          :is-combobox="true"
          :aria-label="$t('solver.row.individualSolverName.name')"
          :disabled="isDisabled"
          style-set="outlined"
          required
          @change="findSolverIndividualSolverName"
        />
      </Sheet>
    </CommonDetailRow>
    <CommonDetailRow
      v-if="!isNew"
      :label="$t('solver.row.individualSolverName.name')"
      :is-editing="isEditing"
      :value="solverCandidate.name"
      required
    >
      <Sheet style="width: 100%">
        <TextField
          v-model="solverCandidate.name"
          :max-length="80"
          :aria-label="$t('solver.row.individualSolverName.name')"
          style-set="outlined"
          required
          @input="onInputForm"
        />
      </Sheet>
    </CommonDetailRow>
    <!-- 個人ソルバーかな -->
    <CommonDetailRow
      :label="$t('solver.row.individualSolverKana.name')"
      :is-editing="isEditing"
      :value="solverCandidate.nameKana"
      required
    >
      <Sheet style="width: 100%">
        <TextField
          v-model="solverCandidate.nameKana"
          :max-length="255"
          :aria-label="$t('solver.row.individualSolverKana.name')"
          :disabled="isDisabled"
          style-set="outlined"
          required
          @input="onInputForm"
        />
      </Sheet>
    </CommonDetailRow>
    <!-- 性別 -->
    <CommonDetailRow
      :label="$t('solver.row.gender.name')"
      :is-editing="isEditing"
      :value="sexValue"
      required
    >
      <RadioGroup
        v-model="solverCandidate.sex"
        :labels="genderLabels"
        :values="genderValues"
        :disabled="isDisabled"
        horizontal
        @change="findSex"
      />
    </CommonDetailRow>
    <!-- 生年月日 -->
    <CommonDetailRow
      :label="$t('solver.row.dateOfBirth.name')"
      :is-editing="isEditing"
      :value="getDefaultBirthday(solverCandidate.birthDay)"
    >
      <SolverDateSelect
        v-model="solverCandidate.birthDay"
        :placeholder="'yyyy/mm/dd'"
        :disabled="isDisabled"
        @change="onInputForm"
      />
    </CommonDetailRow>
    <!-- 連絡先メールアドレス -->
    <CommonDetailRow
      :label="$t('solver.row.mail.name')"
      :is-editing="isEditing"
      :value="solverCandidate.email"
    >
      <Sheet style="width: 100%">
        <TextField
          v-model="solverCandidate.email"
          :max-length="80"
          :email="true"
          :aria-label="$t('solver.row.mail.name')"
          :disabled="isDisabled"
          style-set="outlined"
          @input="onInputForm"
        />
      </Sheet>
    </CommonDetailRow>
    <!-- 電話番号 -->
    <CommonDetailRow
      :label="$t('solver.row.phoneNumber.name')"
      :is-editing="isEditing"
      :value="solverCandidate.phone"
    >
      <Sheet width="300">
        <TextField
          v-model="solverCandidate.phone"
          :phone-number="true"
          :max-length="40"
          :aria-label="$t('solver.row.phoneNumber.name')"
          :disabled="isDisabled"
          style-set="outlined"
          @input="onInputForm"
        />
      </Sheet>
    </CommonDetailRow>
    <!-- 英語レベル -->
    <CommonDetailRow
      :label="$t('solver.row.englishLevel.name')"
      :is-editing="isEditing"
      :value="englishLevelValue"
    >
      <RadioGroup
        v-model="solverCandidate.englishLevel"
        :labels="englishLevelLabels"
        :values="englishLevelValues"
        :disabled="isDisabled"
        horizontal
        @change="findEnglishLevel"
      />
    </CommonDetailRow>
    <!-- 学歴 -->
    <CommonDetailRow
      :label="$t('solver.row.education.name')"
      :tall="true"
      :is-editing="isEditing"
      :value="solverCandidate.academicBackground"
      :escape-value="false"
    >
      <Textarea
        v-model="solverCandidate.academicBackground"
        :max-length="255"
        style-set="outlined"
        :placeholder="$t('solver.row.education.placeholder')"
        :disabled="isDisabled"
        @input="onInputForm"
      />
    </CommonDetailRow>
    <!-- 専門テーマ -->
    <CommonDetailRow
      :label="$t('solver.row.specialTheme.name')"
      :tall="true"
      :is-editing="isEditing"
      :value="solverCandidate.specializedThemes"
      :escape-value="false"
      required
    >
      <Textarea
        v-model="solverCandidate.specializedThemes"
        :max-length="255"
        style-set="outlined"
        required
        :placeholder="$t('solver.row.specialTheme.placeholder')"
        :disabled="isDisabled"
        @input="onInputForm"
      />
    </CommonDetailRow>
    <!-- 職歴 -->
    <CommonDetailRow
      :label="$t('solver.row.workHistory.name')"
      :tall="true"
      :is-editing="isEditing"
      :value="solverCandidate.workHistory"
      :escape-value="false"
      required
    >
      <Textarea
        v-model="solverCandidate.workHistory"
        :max-length="255"
        style-set="outlined"
        required
        :placeholder="$t('solver.row.workHistory.placeholder')"
        :disabled="isDisabled"
        @input="onInputForm"
      />
    </CommonDetailRow>
    <!-- コンサルティングファーム経験有無 -->
    <CommonDetailRow
      :label="$t('solver.row.consultingFirmExperience.name')"
      :is-editing="isEditing"
      :value="consultingFirmExperienceValue"
      required
    >
      <RadioGroup
        v-model="solverCandidate.isConsultingFirm"
        :labels="consultingFirmExperienceLabels"
        :values="consultingFirmExperienceValues"
        :disabled="isDisabled"
        horizontal
        @change="findConsultingFirmExperience"
      />
    </CommonDetailRow>
    <!-- 役職 -->
    <CommonDetailRow
      :label="$t('solver.row.post.name')"
      :tall="true"
      :is-editing="isEditing"
      :value="solverCandidate.title"
      :escape-value="false"
    >
      <Textarea
        v-model="solverCandidate.title"
        :max-length="255"
        :disabled="isDisabled"
        style-set="outlined"
        @input="onInputForm"
      />
    </CommonDetailRow>
    <!-- 主な実績 -->
    <CommonDetailRow
      :label="$t('solver.row.mainAchievements.name')"
      :tall="true"
      :is-editing="isEditing"
      :value="solverCandidate.mainAchievements"
      :escape-value="false"
      required
    >
      <Textarea
        v-model="solverCandidate.mainAchievements"
        :max-length="255"
        style-set="outlined"
        required
        :placeholder="$t('solver.row.mainAchievements.placeholder')"
        :disabled="isDisabled"
        @input="onInputForm"
      />
    </CommonDetailRow>
    <!-- 個人ソルバー画像 -->
    <CommonDetailRow
      :label="$t('solver.row.photo.name')"
      :notes="`${$t('solver.row.photo.notes1')}\n${$t(
        'solver.row.photo.notes2'
      )}`"
      :is-editing="isEditing"
      :component="true"
      :tall="true"
    >
      <SolverImage
        :solver-image="solverCandidate.facePhoto"
        :is-editing="isEditing"
        :is-disabled="isDisabled"
        :is-new="isNew"
        :selected-image="solverCandidate.image"
        @change="onInputImage"
      />
    </CommonDetailRow>
    <!-- 添付ファイル -->
    <CommonDetailRow
      :label="$t('solver.row.file.name')"
      :notes="`${$t('solver.row.file.notes1')}\n${$t(
        'solver.row.file.notes2'
      )}`"
      :is-editing="isEditing"
      :component="true"
      :tall="true"
    >
      <Sheet style="width: 100%">
        <SolverFile
          :links="solverCandidate.resume"
          :is-editing="isEditing"
          :solver-corporation-id="solverCorporationIdFile"
          :solver-id="solverId"
          :is-disabled="isDisabled"
          :selected-files="solverCandidate.files"
          @change="onInputFile"
        />
      </Sheet>
    </CommonDetailRow>
    <!-- 人月単価 -->
    <CommonDetailRow
      :label="$t('solver.row.pricePerPersonMonth.name')"
      :is-editing="isEditing"
      :component="true"
    >
      <span style="margin-right: 5px; display: flex; align-items: center">
        {{ $t('solver.row.pricePerPersonMonth.lower') }}
      </span>
      <Sheet v-if="isEditing" width="160" class="text-right">
        <TextField
          v-model="solverCandidate.pricePerPersonMonthLower"
          :number="true"
          :max-diget="120"
          :positive-number="true"
          :max-digits="8"
          :additional-rules="personMonthRules"
          :aria-label="$t('solver.row.pricePerPersonMonth.name')"
          :disabled="isDisabled || isRegisteredSolverCandidate"
          style-set="outlined"
          @input="onInputPersonMonth"
        />
      </Sheet>
      <Sheet v-if="!isEditing" class="text-right">
        <span>
          {{ getDefaultPrice(solverCandidate.pricePerPersonMonthLower) }}
        </span>
      </Sheet>
      <span
        v-if="!isEditing"
        style="margin-left: 8px; display: flex; align-items: center"
      >
        {{ getPriceUnit(solverCandidate.pricePerPersonMonthLower) }}
      </span>
      <span v-else style="margin-left: 8px; display: flex; align-items: center">
        {{ $t('solver.row.pricePerPersonMonth.unit') }}
      </span>
      <span
        style="
          margin-left: 3px;
          margin-right: 3px;
          display: flex;
          align-items: center;
        "
      >
        〜
      </span>
      <span style="margin-right: 5px; display: flex; align-items: center">
        {{ $t('solver.row.pricePerPersonMonth.upper') }}
      </span>
      <Sheet v-if="isEditing" width="160" class="text-right">
        <TextField
          v-model="solverCandidate.pricePerPersonMonth"
          :number="true"
          :positive-number="true"
          :max-digits="8"
          :additional-rules="personMonthRules"
          :aria-label="$t('solver.row.pricePerPersonMonth.name')"
          :disabled="isDisabled || isRegisteredSolverCandidate"
          style-set="outlined"
          @input="onInputPersonMonth"
        />
      </Sheet>
      <Sheet v-if="!isEditing" class="text-right">
        <span>
          {{ getDefaultPrice(solverCandidate.pricePerPersonMonth) }}
        </span>
      </Sheet>
      <span
        v-if="!isEditing"
        style="margin-left: 8px; display: flex; align-items: center"
      >
        {{ getPriceUnit(solverCandidate.pricePerPersonMonth) }}
      </span>
      <span v-else style="margin-left: 8px; display: flex; align-items: center">
        {{ $t('solver.row.pricePerPersonMonth.unit') }}
      </span>
    </CommonDetailRow>
    <!-- 時間単価 -->
    <CommonDetailRow
      :label="$t('solver.row.hourlyRate.name')"
      :is-editing="isEditing"
      :component="true"
    >
      <span style="margin-right: 5px; display: flex; align-items: center">
        {{ $t('solver.row.hourlyRate.lower') }}
      </span>
      <Sheet v-if="isEditing" width="160" class="text-right">
        <TextField
          v-model="solverCandidate.hourlyRateLower"
          :number="true"
          :positive-number="true"
          :max-digits="6"
          :additional-rules="hourlyRateRules"
          :aria-label="$t('solver.row.hourlyRate.name')"
          :disabled="isDisabled || isRegisteredSolverCandidate"
          style-set="outlined"
          @input="onInputHourlyRate"
        />
      </Sheet>
      <Sheet v-if="!isEditing" class="text-right">
        <span>
          {{ getDefaultPrice(solverCandidate.hourlyRateLower) }}
        </span>
      </Sheet>
      <span
        v-if="!isEditing"
        style="margin-left: 8px; display: flex; align-items: center"
      >
        {{ getPriceUnit(solverCandidate.hourlyRateLower) }}
      </span>
      <span v-else style="margin-left: 8px; display: flex; align-items: center">
        {{ $t('solver.row.pricePerPersonMonth.unit') }}
      </span>
      <span
        style="
          margin-left: 3px;
          margin-right: 3px;
          display: flex;
          align-items: center;
        "
      >
        〜
      </span>
      <span style="margin-right: 5px; display: flex; align-items: center">
        {{ $t('solver.row.hourlyRate.upper') }}
      </span>
      <Sheet v-if="isEditing" width="160" class="text-right">
        <TextField
          v-model="solverCandidate.hourlyRate"
          :number="true"
          :positive-number="true"
          :max-digits="6"
          :additional-rules="hourlyRateRules"
          :aria-label="$t('solver.row.hourlyRate.name')"
          :disabled="isDisabled || isRegisteredSolverCandidate"
          style-set="outlined"
          @input="onInputHourlyRate"
        />
      </Sheet>
      <Sheet v-if="!isEditing" class="text-right">
        <span>
          {{ getDefaultPrice(solverCandidate.hourlyRate) }}
        </span>
      </Sheet>
      <span
        v-if="!isEditing"
        style="margin-left: 8px; display: flex; align-items: center"
      >
        {{ getPriceUnit(solverCandidate.hourlyRate) }}
      </span>
      <span v-else style="margin-left: 8px; display: flex; align-items: center">
        {{ $t('solver.row.pricePerPersonMonth.unit') }}
      </span>
    </CommonDetailRow>
    <!-- 稼働状況 -->
    <CommonDetailRow
      :label="$t('solver.row.workingStatus.name')"
      :is-editing="isEditing"
      :value="workingStatusValue"
    >
      <RadioGroup
        v-model="solverCandidate.operatingStatus"
        :labels="workingStatusLabels"
        :values="workingStatusValues"
        :disabled="isDisabled"
        horizontal
        @change="findWorkingStatus"
      />
    </CommonDetailRow>
    <!-- 提供稼働率 -->
    <CommonDetailRow
      :label="$t('solver.row.providedUtilizationRate.name')"
      :is-editing="isEditing"
      :component="true"
    >
      <v-flex class="d-flex align-center">
        <span style="margin-right: 10px; display: flex; align-items: center">
          {{ $t('solver.row.providedUtilizationRate.month1') }}
        </span>
        <Sheet v-if="isEditing" width="80" class="text-right">
          <TextField
            v-model="solverCandidate.providedOperatingRate"
            :positive-number="true"
            :range-number-to="100"
            :aria-label="$t('solver.row.providedUtilizationRate.name')"
            :disabled="isDisabled || isRegisteredSolverCandidate"
            style-set="outlined"
            @input="onInputForm"
          />
        </Sheet>
        <Sheet v-if="!isEditing" class="text-right">
          <span>
            {{ solverCandidate.providedOperatingRate }}
          </span>
        </Sheet>
        <span style="margin-left: 10px; display: flex; align-items: center">
          {{ $t('solver.row.providedUtilizationRate.unit') }}
        </span>
        <span
          style="
            margin-right: 10px;
            margin-left: 30px;
            display: flex;
            align-items: center;
          "
        >
          {{ $t('solver.row.providedUtilizationRate.month2') }}
        </span>
        <Sheet v-if="isEditing" width="80" class="text-right">
          <TextField
            v-model="solverCandidate.providedOperatingRateNext"
            :positive-number="true"
            :range-number-to="100"
            :aria-label="$t('solver.row.providedUtilizationRate.name')"
            :disabled="isDisabled || isRegisteredSolverCandidate"
            style-set="outlined"
            @input="onInputForm"
          />
        </Sheet>
        <Sheet v-if="!isEditing" class="text-right">
          <span>
            {{ solverCandidate.providedOperatingRateNext }}
          </span>
        </Sheet>
        <span style="margin-left: 10px; display: flex; align-items: center">
          {{ $t('solver.row.providedUtilizationRate.unit') }}
        </span>
      </v-flex>
      <!-- 再来月以降の稼働見込み -->
      <v-flex class="d-flex mt-3 align-center" style="width: 100%">
        <span style="margin-right: 10px; display: flex; align-items: center">
          {{ $t('solver.row.providedUtilizationRate.monthAfterNext') }}
        </span>
        <Sheet v-if="isEditing" width="426">
          <TextField
            v-model="solverCandidate.operationProspectsMonthAfterNext"
            :max-length="20"
            :aria-label="$t('solver.row.providedUtilizationRate.name')"
            :placeholder="$t('solver.row.providedUtilizationRate.placeholder')"
            :disabled="isDisabled || isRegisteredSolverCandidate"
            style-set="outlined"
            @input="onInputForm"
          />
        </Sheet>
        <!-- 再来月以降の稼働見込みの値がない場合は非表示 -->
        <Sheet v-if="!isEditing" class="text-right">
          <span>
            {{ solverCandidate.operationProspectsMonthAfterNext }}
          </span>
        </Sheet>
      </v-flex>
    </CommonDetailRow>
    <!-- 東証33業種経験/対応可能領域 -->
    <CommonDetailRow
      :label="$t('solver.row.tse33Experience.name')"
      :is-editing="isEditing"
      :value="tse33IndustryExperienceValue"
    >
      <Sheet width="520">
        <Select
          v-model="solverCandidate.tsiAreas"
          :items="tse33IndustryExperienceItems"
          item-text="label"
          item-value="value"
          :placeholder="$t('common.placeholder.selectMulti2')"
          style-set="outlined bgWhite bgTransparent"
          :bg-transparent="true"
          :multiple="true"
          :disabled="isDisabled"
          @change="findTse33IndustryExperienceValue"
        />
      </Sheet>
    </CommonDetailRow>
    <!-- 課題マップ50 -->
    <CommonDetailRow
      :label="$t('solver.row.issueMap50.name')"
      :is-editing="isEditing"
      :value="issueMap50Value"
    >
      <Sheet width="520">
        <Select
          v-model="solverCandidate.issueMap50"
          :items="issueMap50Items"
          item-text="label"
          item-value="value"
          :placeholder="$t('common.placeholder.selectMulti2')"
          style-set="outlined bgWhite bgTransparent"
          :bg-transparent="true"
          :multiple="true"
          :disabled="isDisabled"
          @change="findIssueMap50Value"
        />
      </Sheet>
    </CommonDetailRow>
    <!-- 備考 -->
    <CommonDetailRow
      :label="$t('solver.row.remarks.name')"
      :tall="true"
      :is-editing="isEditing"
      :value="solverCandidate.notes"
      :escape-value="false"
    >
      <Textarea
        v-model="solverCandidate.notes"
        :max-length="255"
        :disabled="isDisabled"
        style-set="outlined"
        @input="onInputForm"
      />
    </CommonDetailRow>
  </v-form>
</template>

<script lang="ts">
import CommonDetailRows from '../../common/molecules/CommonDetailRows.vue'
import {
  TextField,
  Select,
  Sheet,
  AutoComplete,
  Icon,
  Button,
  RadioGroup,
  Textarea,
  Checkbox,
} from '~/components/common/atoms/index'
import CommonDetailRow from '~/components/common/molecules/CommonDetailRow.vue'
import LastUpdate from '~/components/common/molecules/LastUpdate.vue'
import SolverDateSelect from '~/components/common/molecules/SolverDateSelect.vue'
import CustomerList from '~/components/project/molecules/CustomerList.vue'
import SupporterList from '~/components/project/molecules/SupporterList.vue'
import SolverImage from '~/components/solver/molecules/SolverImage.vue'
import SolverFile from '~/components/solver/molecules/SolverFile.vue'
import { PropType } from '~/common/BaseVueClass'
import { IGetSolverByIdResponse, IGetSolversResponse } from '~/types/Solver'
import { meStore } from '~/store'
import { ENUM_USER_ROLE } from '~/types/User'
import { IGetSelectItemsResponse } from '~/types/Master'
import { IFile } from '~/utils/upload'

export default CommonDetailRows.extend({
  name: 'SolverDetailRows',
  // v-model用
  model: {
    prop: 'isValid',
    event: 'input',
  },
  components: {
    TextField,
    Select,
    Sheet,
    CommonDetailRow,
    LastUpdate,
    AutoComplete,
    Icon,
    Button,
    RadioGroup,
    Textarea,
    SolverDateSelect,
    CustomerList,
    SupporterList,
    Checkbox,
    SolverImage,
    SolverFile,
  },
  props: {
    isEditing: {
      type: Boolean,
      default: true,
    },
    isDisabled: {
      type: Boolean,
      default: true,
    },
    isRegisteredSolverCandidate: {
      type: Boolean,
      default: false,
    },
    solverCandidate: {
      type: Object as PropType<any>,
      required: true,
    },
    solvers: {
      type: Object as PropType<IGetSolversResponse>,
      required: true,
    },
    solver: {
      type: Object as PropType<IGetSolverByIdResponse>,
      required: true,
    },
    issueMap50: {
      type: Object as PropType<IGetSelectItemsResponse>,
      required: true,
    },
    tsiAreas: {
      type: Object as PropType<IGetSelectItemsResponse>,
      required: true,
    },
    solverCorporationId: {
      type: String,
      required: true,
    },
    isNew: {
      type: Boolean,
      default: true,
    },
  },
  data() {
    return {
      // 性別ラジオボタン
      genderLabels: this.$t('solver.row.gender.radio.labels'),
      genderValues: this.$t('solver.row.gender.radio.values'),
      // 英語レベルラジオボタン
      englishLevelLabels: this.$t('solver.row.englishLevel.radio.labels'),
      englishLevelValues: this.$t('solver.row.englishLevel.radio.values'),
      // コンサルティングファーム経験有無ラジオボタン
      consultingFirmExperienceLabels: this.$t(
        'solver.row.consultingFirmExperience.radio.labels'
      ),
      consultingFirmExperienceValues: this.$t(
        'solver.row.consultingFirmExperience.radio.values'
      ),
      // 稼働状況ラジオボタン
      workingStatusLabels: this.$t('solver.row.workingStatus.radio.labels'),
      workingStatusValues: this.$t('solver.row.workingStatus.radio.values'),
      // 性別確認画面表示用
      sexValue: (
        this.$t('solver.row.gender.radio.labels') as unknown as string[]
      )[0],
      // 英語レベル確認画面表示用
      englishLevelValue: (
        this.$t('solver.row.englishLevel.radio.labels') as unknown as string[]
      )[0],
      // コンサルティングファーム経験有無確認画面表示用
      consultingFirmExperienceValue: (
        this.$t(
          'solver.row.consultingFirmExperience.radio.labels'
        ) as unknown as string[]
      )[0],
      // 稼働状況確認画面表示用
      workingStatusValue: (
        this.$t('solver.row.workingStatus.radio.labels') as unknown as string[]
      )[0],
      // 東証33業種経験/対応可能領域確認画面表示用
      tse33IndustryExperienceValue: '',
      // 課題マップ50確認画面表示用
      issueMap50Value: '',
      // 削除対象案件IDリスト
      deleteSolverApplicationIds: [] as string[],
      // 人月単価用ルール配列
      personMonthRules: [] as Function[],
      // 時間単価用ルール配列
      hourlyRateRules: [] as Function[],
    }
  },
  computed: {
    // 個人ソルバー名サジェストアイテム
    suggestIndividualSolverName() {
      const solvers = this.solvers as IGetSolversResponse
      return solvers.solvers.map((solver) => {
        let solverName = solver.name
        if (solver.name && solver.birthDay) {
          const solverType = solver.isSolver ? '個人ソルバー' : 'ソルバー候補'
          solverName = `【${solverType}】${solver.name}（${solver.birthDay}）`
        }
        return { text: solverName, value: solver.id }
      })
    },

    // 添付ファイル用法人ソルバーID
    solverCorporationIdFile(): string {
      if (meStore.role === ENUM_USER_ROLE.APT) {
        return this.solverCorporationId
      } else {
        return meStore.solverCorporationId
      }
    },

    // 課題マップ50プルダウンアイテム
    issueMap50Items(): (
      | {
          header: string
        }
      | {
          label: string
          value: string
        }
    )[] {
      const formattedIssueMap50 = this.generateSelectItems(
        this.issueMap50.masters
      )

      return formattedIssueMap50
    },

    // 東証33業種経験/対応可能領域プルダウンアイテム
    tse33IndustryExperienceItems(): (
      | {
          header: string
        }
      | {
          label: string
          value: string
        }
    )[] {
      const formattedTse33IndustryExperience = this.generateSelectItems(
        this.tsiAreas.masters
      )

      return formattedTse33IndustryExperience
    },

    // 添付ファイル用個人ソルバーID
    solverId(): string | undefined {
      const filteredSolver = this.solvers.solvers.find(
        (solver) => solver.id === this.solverCandidate.id
      )
      if (filteredSolver) {
        return this.solverCandidate.id
      } else {
        return undefined
      }
    },
  },
  watch: {
    // formのバリデーションが変化した時の処理
    isValid() {
      this.$emit('change:isValid', this.isValid)
    },
    // 個人ソルバー名を変更した時の処理
    solver: {
      handler(newValue) {
        this.findSolverIndividualSolverName(newValue.id, false)
        this.findSex(newValue.sex, false)
        this.findEnglishLevel(newValue.englishLevel, false)
        this.findConsultingFirmExperience(newValue.isConsultingFirm, false)
        this.findWorkingStatus(newValue.operatingStatus, false)
        this.findTse33IndustryExperienceValue(newValue.tsiAreas, false)
        this.findIssueMap50Value(newValue.issueMap50, false)
      },
      deep: true,
    },
    isEditing: {
      handler(newValue) {
        if (newValue) {
          this.solverCandidate.birthDay = this.getEditableBirthday(
            this.solverCandidate.birthDay
          )
        }
      },
      immediate: true,
    },
  },
  methods: {
    // formの入力値を親コンポーネントに渡す処理
    onInputForm() {
      this.$emit('inputForm', this.solverCandidate)
      // isValidの値を親コンポーネントに渡す処理
      this.$emit('change:isValid', this.isValid)
    },

    // 人月単価のバリデーション
    onInputPersonMonth() {
      const personMonthLower = Number(
        this.solverCandidate.pricePerPersonMonthLower
      )
      const personMonthUpper = Number(this.solverCandidate.pricePerPersonMonth)

      this.personMonthRules = []

      if (
        this.solverCandidate.pricePerPersonMonthLower !== '' &&
        this.solverCandidate.pricePerPersonMonth !== '' &&
        personMonthLower !== 0 &&
        personMonthUpper !== 0 &&
        personMonthLower !== null &&
        personMonthLower !== undefined &&
        personMonthUpper !== null &&
        personMonthUpper !== undefined &&
        personMonthLower > personMonthUpper
      ) {
        this.personMonthRules.push(() =>
          this.$t('common.rule.additionalRuleRangeNumber')
        )
      }

      this.$emit('inputForm', this.solverCandidate)
      this.$emit('change:isValid', this.isValid)
    },

    // 時間単価のバリデーション
    onInputHourlyRate() {
      const hourlyRateLower = Number(this.solverCandidate.hourlyRateLower)
      const hourlyRateUpper = Number(this.solverCandidate.hourlyRate)

      this.hourlyRateRules = []

      if (
        this.solverCandidate.hourlyRateLower !== '' &&
        this.solverCandidate.hourlyRate !== '' &&
        hourlyRateLower !== 0 &&
        hourlyRateUpper !== 0 &&
        hourlyRateLower !== null &&
        hourlyRateLower !== undefined &&
        hourlyRateUpper !== null &&
        hourlyRateUpper !== undefined &&
        hourlyRateLower > hourlyRateUpper
      ) {
        this.hourlyRateRules.push(() =>
          this.$t('common.rule.additionalRuleRangeNumber')
        )
      }

      this.$emit('inputForm', this.solverCandidate)
      this.$emit('change:isValid', this.isValid)
    },

    // formの入力値を親コンポーネントに渡す処理（添付ファイル）
    onInputFile(newValue: IFile) {
      const solverCandidateInputData = {
        ...this.solverCandidate,
        files: newValue,
      }
      this.$emit('inputForm', solverCandidateInputData)
      // isValidの値を親コンポーネントに渡す処理
      this.$emit('change:isValid', this.isValid)
    },

    // formの入力値を親コンポーネントに渡す処理（個人ソルバー画像）
    onInputImage(newValue: IFile) {
      const solverImageInputData = {
        ...this.solverCandidate,
        image: newValue,
      }
      if (newValue === undefined && this.solverCandidate.facePhoto) {
        solverImageInputData.facePhoto.path = undefined
        solverImageInputData.facePhoto.fileName = undefined
      }
      if (newValue === undefined || newValue.name) {
        this.$emit('inputForm', solverImageInputData)
        // isValidの値を親コンポーネントに渡す処理
        this.$emit('change:isValid', this.isValid)
      }
    },

    // 個人ソルバー名を抽出する処理
    findSolverIndividualSolverName(newValue: string, isEmit: boolean = true) {
      let solverId: string | undefined = newValue
      let solverName = ''
      const individualSolverName = this.suggestIndividualSolverName.find(
        (IndividualSolverName) => IndividualSolverName.value === newValue
      )
      if (!individualSolverName) {
        solverId = undefined
        solverName = newValue
      }

      const solverInfo = {
        solverId,
        solverName,
      }
      if (isEmit) {
        this.$emit('change:solverName', solverInfo)
      }
    },

    // 性別の表示名を抽出する処理
    findSex(newValue: string, isEmit: boolean = true) {
      const genderValues = this.genderValues as unknown as string[]
      const genderLabels = this.genderLabels as unknown as string[]
      const index = genderValues.indexOf(newValue)
      this.sexValue = genderLabels[index]
      if (isEmit) {
        this.onInputForm()
      }
    },

    // 英語レベルの表示名を抽出する処理
    findEnglishLevel(newValue: string, isEmit: boolean = true) {
      const englishLevelValue = this.englishLevelValues as unknown as string[]
      const englishLevelLabels = this.englishLevelLabels as unknown as string[]
      const index = englishLevelValue.indexOf(newValue)
      this.englishLevelValue = englishLevelLabels[index]
      if (isEmit) {
        this.onInputForm()
      }
    },

    // コンサルティングファーム経験有無の表示名を抽出する処理
    findConsultingFirmExperience(newValue: string, isEmit: boolean = true) {
      const consultingFirmExperienceValues = this
        .consultingFirmExperienceValues as unknown as string[]
      const consultingFirmExperienceLabels = this
        .consultingFirmExperienceLabels as unknown as string[]
      const index = consultingFirmExperienceValues.indexOf(newValue)
      this.consultingFirmExperienceValue = consultingFirmExperienceLabels[index]
      if (isEmit) {
        this.onInputForm()
      }
    },

    // 稼働状況の表示名を抽出する処理
    findWorkingStatus(newValue: string, isEmit: boolean = true) {
      const workingStatusValues = this
        .workingStatusValues as unknown as string[]
      const workingStatusLabels = this
        .workingStatusLabels as unknown as string[]
      const index = workingStatusValues.indexOf(newValue)
      this.workingStatusValue = workingStatusLabels[index]
      if (isEmit) {
        this.onInputForm()
      }
    },

    // 東証33業種経験/対応可能領域の表示名を抽出する処理
    findTse33IndustryExperienceValue(
      newValue: string[],
      isEmit: boolean = true
    ) {
      const tse33IndustryExperienceValues = newValue.map(
        (filteredTse33IndustryExperienceValue) => {
          const targetItem = this.tse33IndustryExperienceItems.find(
            (tse33IndustryExperienceItem) =>
              'value' in tse33IndustryExperienceItem &&
              tse33IndustryExperienceItem.value ===
                filteredTse33IndustryExperienceValue
          )
          return (targetItem as { label: string; value: string })?.label
        }
      )
      this.tse33IndustryExperienceValue =
        tse33IndustryExperienceValues.join(', ')
      if (isEmit) {
        this.onInputForm()
      }
    },

    // 課題マップ50の表示名を抽出する処理
    findIssueMap50Value(newValue: any[], isEmit: boolean = true) {
      // 数値部分を抽出して数値として比較するための関数
      const extractNumber = (str: string) => {
        // 文字列の先頭から始まる連続する数字を格納
        const match = str.match(/^\d+/)
        return match ? parseInt(match[0], 10) : 0
      }

      const issueMap50Values = newValue
        .map((filteredIssueMap50Value) => {
          const targetItem = this.issueMap50Items.find(
            (issueMap50Item) =>
              'value' in issueMap50Item &&
              issueMap50Item.value === filteredIssueMap50Value
          )
          return (targetItem as { label: string; value: string })?.label
        })
        // 数値部分でソート
        .sort((a, b) => {
          return extractNumber(a) - extractNumber(b)
        })

      this.issueMap50Value = issueMap50Values.join(', ')
      if (isEmit) {
        this.onInputForm()
      }
    },

    // プルダウンアイテム（複数選択可）を生成する処理
    generateSelectItems(
      targetArray: { category: string; id: string; name: string }[]
    ) {
      const groupedTargetArray = targetArray.reduce(
        (acc: Record<string, { label: string; value: string }[]>, item) => {
          if (!acc[item.category]) {
            acc[item.category] = []
          }
          acc[item.category].push({ label: item.name, value: item.id })
          return acc
        },
        {}
      )
      const formattedTargetArray = Object.entries(groupedTargetArray).reduce(
        (
          acc: ({ header: string } | { label: string; value: string })[],
          [category, items]
        ) => {
          if (category && category !== 'null') {
            acc.push({ header: category })
          }
          acc.push(...items)
          return acc
        },
        [] as Array<{ header: string } | { label: string; value: string }>
      )

      return formattedTargetArray
    },

    // 案件を削除
    deleteSolverApplication(solverApplicationId: string) {
      this.solverCandidate.solverApplications =
        this.solverCandidate.solverApplications.filter(
          (app: any) => app.id !== solverApplicationId
        )
      if (!this.deleteSolverApplicationIds.includes(solverApplicationId)) {
        this.deleteSolverApplicationIds.push(solverApplicationId)
      }
      const solverCandidateInputData = {
        deleteSolverApplicationIds: this.deleteSolverApplicationIds,
        ...this.solverCandidate,
      }
      this.$emit('inputForm', solverCandidateInputData)
      this.$emit('change:isValid', this.isValid)
    },
  },
})
</script>

<style lang="scss" scoped>
.text-right {
  display: flex;
  align-items: center;
  justify-content: right;
  input {
    text-align: right;
  }
}
</style>
<style lang="scss">
.o-common-detail-rows__data {
  overflow-wrap: break-word;
  word-break: break-all;
}
</style>
