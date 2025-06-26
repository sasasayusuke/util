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
        :user="localParam.updateUserName"
        :date="localParam.updateAt"
        :show-term="false"
      />
    </template>
    <SolverDetailSection
      :is-new="isNew"
      :title="$t('solver.pages.application.detail.section.solverInfo')"
    >
      <!-- 応募案件 -->
      <CommonDetailRow
        v-if="isDisplayApplicationProject"
        :label="$t('solver.row.applicationProject.name')"
        :is-editing="isEditing"
        :component="true"
        :tall="true"
      >
        <!-- ① 変更＋申請の場合 -->
        <!-- ② 変更＋一時保存（本登録）の場合 -->
        <v-flex v-if="isEditing && (isTemporarySaving || isNew)">
          <v-flex class="d-flex align-center">
            <!-- 案件コード -->
            <span
              style="margin-right: 10px; display: flex; align-items: center"
            >
              {{ $t('solver.row.applicationProject.projectCode') }}
            </span>
            <Sheet width="455">
              <TextField
                v-model="projectCode"
                :additional-rules="additionalRules"
                :aria-label="$t('solver.row.applicationProject.projectId')"
                style-set="outlined"
                @input="clearAdditionalRules"
              />
            </Sheet>
            <Button
              style-set="small-primary"
              class="ml-4"
              @click="checkValidateProjectCode"
            >
              {{ $t('solver.row.applicationProject.button.projectName') }}
            </Button>
            <Button
              outlined
              style-set="small-tertiary"
              class="ml-2"
              @click="resetProjectCode"
            >
              {{ $t('common.button.reset') }}
            </Button>
          </v-flex>
          <v-flex class="d-flex mt-5 align-center">
            <!-- 案件ID -->
            <span
              style="margin-right: 10px; display: flex; align-items: center"
            >
              {{ $t('solver.row.applicationProject.projectId') }}
            </span>
            <Sheet width="160">
              {{ localParam.solverApplicationId }}
            </Sheet>
            <!-- 案件名 -->
            <span style="margin-left: 25px; display: flex; align-items: center">
              {{ $t('solver.row.applicationProject.projectName') }}
            </span>
            <Sheet width="470">
              {{ localParam.solverApplicationName }}
            </Sheet>
          </v-flex>
        </v-flex>
        <!-- 詳細＋申請の場合 -->
        <v-flex v-if="!isEditing && isNew" class="d-flex align-center">
          <Sheet style="max-width: 200px; overflow-wrap: break-word">
            {{ localParam.solverApplicationId }}
          </Sheet>
          <Sheet
            style="
              max-width: 590px;
              overflow-wrap: break-word;
              margin-left: 36px;
            "
          >
            {{ localParam.solverApplicationName }}
          </Sheet>
        </v-flex>
        <!-- ① 本登録の変更・詳細の場合 -->
        <!-- ② 詳細＋＋一時保存（本登録）の場合 -->
        <v-flex
          v-if="
            (!isTemporarySaving || (!isEditing && isTemporarySaving)) && !isNew
          "
          class="d-flex align-center"
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
                  localParam.solverApplications &&
                  localParam.solverApplications.length > 0
                "
              >
                <template
                  v-for="(project, index) in localParam.solverApplications"
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
                        index !== localParam.solverApplications.length - 1 &&
                        isEditing
                      "
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
                      {{
                        $t('solver.row.applicationProject.noApplicantsMessage')
                      }}
                    </span>
                  </v-col>
                </v-row>
              </template>
            </v-container>
          </Sheet>
        </v-flex>
      </CommonDetailRow>
      <!-- 個人ソルバー名（一時保存可能の状態の場合） -->
      <CommonDetailRow
        v-if="isEditing && (isTemporarySaving || isNew)"
        :label="$t('solver.row.individualSolverName.name')"
        :notes="$t('solver.row.individualSolverName.notes')"
        :is-editing="isEditing"
        :value="localParam.name"
        required
      >
        <Sheet style="width: 100%">
          <TextField
            v-model="localParam.name"
            :max-length="80"
            :aria-label="$t('solver.row.individualSolverName.name')"
            style-set="outlined"
            required
            @input="onInputForm"
          />
        </Sheet>
      </CommonDetailRow>
      <!-- 個人ソルバー名 -->
      <CommonDetailRow
        v-else
        :label="$t('solver.row.individualSolverName.name')"
        :is-editing="isEditing"
        :value="localParam.name"
        required
      >
        <Sheet style="width: 100%">
          <TextField
            v-model="localParam.name"
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
        :value="localParam.nameKana"
        required
      >
        <Sheet style="width: 100%">
          <TextField
            v-model="localParam.nameKana"
            :max-length="255"
            :aria-label="$t('solver.row.individualSolverKana.name')"
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
          v-model="localParam.sex"
          :labels="genderLabels"
          :values="genderValues"
          horizontal
          @change="findSex"
        />
      </CommonDetailRow>
      <!-- 生年月日 -->
      <CommonDetailRow
        :label="$t('solver.row.dateOfBirth.name')"
        :is-editing="isEditing"
        :value="getDefaultBirthday(localParam.birthDay)"
      >
        <SolverDateSelect
          v-model="localParam.birthDay"
          :placeholder="'yyyy/mm/dd'"
          @change="onInputForm"
        />
      </CommonDetailRow>
      <!-- 連絡先メールアドレス -->
      <CommonDetailRow
        :label="$t('solver.row.mail.name')"
        :is-editing="isEditing"
        :value="localParam.email"
      >
        <Sheet style="width: 100%">
          <TextField
            v-model="localParam.email"
            :max-length="80"
            :email="true"
            :aria-label="$t('solver.row.mail.name')"
            style-set="outlined"
            @input="onInputForm"
          />
        </Sheet>
      </CommonDetailRow>
      <!-- 電話番号 -->
      <CommonDetailRow
        :label="$t('solver.row.phoneNumber.name')"
        :is-editing="isEditing"
        :value="localParam.phone"
      >
        <Sheet width="300">
          <TextField
            v-model="localParam.phone"
            :phone-number="true"
            :max-length="40"
            :aria-label="$t('solver.row.phoneNumber.name')"
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
        required
      >
        <RadioGroup
          v-model="localParam.englishLevel"
          :labels="englishLevelLabels"
          :values="englishLevelValues"
          horizontal
          @change="findEnglishLevel"
        />
      </CommonDetailRow>
      <!-- 学歴 -->
      <CommonDetailRow
        :label="$t('solver.row.education.name')"
        :tall="true"
        :is-editing="isEditing"
        :value="localParam.academicBackground"
        :escape-value="false"
        required
      >
        <Textarea
          v-model="localParam.academicBackground"
          :max-length="255"
          style-set="outlined"
          :placeholder="$t('solver.row.education.placeholder')"
          required
          @input="onInputForm"
        />
      </CommonDetailRow>
      <!-- 専門テーマ -->
      <CommonDetailRow
        :label="$t('solver.row.specialTheme.name')"
        :tall="true"
        :is-editing="isEditing"
        :value="localParam.specializedThemes"
        :escape-value="false"
        required
      >
        <Textarea
          v-model="localParam.specializedThemes"
          :max-length="255"
          style-set="outlined"
          required
          :placeholder="$t('solver.row.specialTheme.placeholder')"
          @input="onInputForm"
        />
      </CommonDetailRow>
      <!-- 職歴 -->
      <CommonDetailRow
        :label="$t('solver.row.workHistory.name')"
        :tall="true"
        :is-editing="isEditing"
        :value="localParam.workHistory"
        :escape-value="false"
        required
      >
        <Textarea
          v-model="localParam.workHistory"
          :max-length="255"
          style-set="outlined"
          required
          :placeholder="$t('solver.row.workHistory.placeholder')"
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
          v-model="localParam.isConsultingFirm"
          :labels="consultingFirmExperienceLabels"
          :values="consultingFirmExperienceValues"
          horizontal
          @change="findConsultingFirmExperience"
        />
      </CommonDetailRow>
      <!-- 役職 -->
      <CommonDetailRow
        :label="$t('solver.row.post.name')"
        :tall="true"
        :is-editing="isEditing"
        :value="localParam.title"
        :escape-value="false"
        required
      >
        <Textarea
          v-model="localParam.title"
          :max-length="255"
          style-set="outlined"
          required
          @input="onInputForm"
        />
      </CommonDetailRow>
      <!-- 主な実績 -->
      <CommonDetailRow
        :label="$t('solver.row.mainAchievements.name')"
        :tall="true"
        :is-editing="isEditing"
        :value="localParam.mainAchievements"
        :escape-value="false"
        required
      >
        <Textarea
          v-model="localParam.mainAchievements"
          :max-length="255"
          style-set="outlined"
          :placeholder="$t('solver.row.mainAchievements.placeholder')"
          required
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
        required
      >
        <SolverImage
          :solver-image="localParam.facePhoto"
          :is-initial-valid="!isDisplayApplicationProject"
          :is-editing="isEditing"
          required
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
            :links="localParam.resume"
            :is-editing="isEditing"
            :solver-corporation-id="solverCorporationIdFile"
            :refresh="fileRefresh"
            @delete="onDeleteFile"
            @change="onInputFile"
          />
        </Sheet>
      </CommonDetailRow>
      <!-- 人月単価 （申請・一時保存変更画面）-->
      <CommonDetailRow
        v-if="isNew || isTemporarySaving || isCandidateSaved"
        :label="$t('solver.row.pricePerPersonMonth.name')"
        :is-editing="isEditing"
        :component="true"
      >
        <span style="margin-right: 5px; display: flex; align-items: center">
          {{ $t('solver.row.pricePerPersonMonth.lower') }}
        </span>
        <Sheet v-if="isEditing" width="160" class="text-right">
          <TextField
            v-model="localParam.pricePerPersonMonthLower"
            :max-diget="120"
            :number="true"
            :positive-number="true"
            :max-digits="8"
            :additional-rules="personMonthRules"
            :aria-label="$t('solver.row.pricePerPersonMonth.name')"
            style-set="outlined"
            @input="onInputPersonMonth"
          />
        </Sheet>
        <Sheet v-if="!isEditing" class="text-right">
          <span>
            {{ getDefaultPrice(localParam.pricePerPersonMonthLower) }}
          </span>
        </Sheet>
        <Sheet
          v-if="isEditing"
          style="margin-left: 8px; display: flex; align-items: center"
        >
          <span>
            {{ $t('solver.row.pricePerPersonMonth.unit') }}
          </span>
        </Sheet>
        <span
          v-else
          style="margin-left: 8px; display: flex; align-items: center"
        >
          {{ getPriceUnit(localParam.pricePerPersonMonthLower) }}
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
            v-model="localParam.pricePerPersonMonth"
            :number="true"
            :positive-number="true"
            :max-digits="8"
            :additional-rules="personMonthRules"
            :aria-label="$t('solver.row.pricePerPersonMonth.name')"
            style-set="outlined"
            @input="onInputPersonMonth"
          />
        </Sheet>
        <Sheet v-if="!isEditing" class="text-right">
          <span>
            {{ getDefaultPrice(localParam.pricePerPersonMonth) }}
          </span>
        </Sheet>
        <span
          v-if="isEditing"
          style="margin-left: 8px; display: flex; align-items: center"
        >
          {{ $t('solver.row.pricePerPersonMonth.unit') }}
        </span>
        <span
          v-else
          style="margin-left: 8px; display: flex; align-items: center"
        >
          {{ getPriceUnit(localParam.pricePerPersonMonth) }}
        </span>
      </CommonDetailRow>
      <!-- 人月単価（個人ソルバー変更画面） -->
      <CommonDetailRow
        v-else
        :label="$t('solver.row.pricePerPersonMonth.name')"
        :is-editing="isEditing"
        :component="true"
      >
        <span style="margin-right: 5px; display: flex; align-items: center">
          {{ $t('solver.row.pricePerPersonMonth.lower') }}
        </span>
        <Sheet class="text-right">
          <span>
            {{ getDefaultPrice(localParam.pricePerPersonMonthLower) }}
          </span>
        </Sheet>
        <span style="margin-left: 8px; display: flex; align-items: center">
          {{ getPriceUnit(localParam.pricePerPersonMonthLower) }}
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
        <Sheet class="text-right">
          <span>
            {{ getDefaultPrice(localParam.pricePerPersonMonth) }}
          </span>
        </Sheet>
        <span style="margin-left: 8px; display: flex; align-items: center">
          {{ getPriceUnit(localParam.pricePerPersonMonth) }}
        </span>
      </CommonDetailRow>
      <!-- 時間単価 （申請・一時保存変更画面）-->
      <CommonDetailRow
        v-if="isNew || isTemporarySaving || isCandidateSaved"
        :label="$t('solver.row.hourlyRate.name')"
        :is-editing="isEditing"
        :component="true"
      >
        <span style="margin-right: 5px; display: flex; align-items: center">
          {{ $t('solver.row.hourlyRate.lower') }}
        </span>
        <Sheet v-if="isEditing" width="160" class="text-right">
          <TextField
            v-model="localParam.hourlyRateLower"
            :number="true"
            :positive-number="true"
            :max-digits="6"
            :additional-rules="hourlyRateRules"
            :aria-label="$t('solver.row.hourlyRate.name')"
            style-set="outlined"
            @input="onInputHourlyRate"
          />
        </Sheet>
        <Sheet v-if="!isEditing" class="text-right">
          <span>
            {{ getDefaultPrice(localParam.hourlyRateLower) }}
          </span>
        </Sheet>
        <span
          v-if="isEditing"
          style="margin-left: 8px; display: flex; align-items: center"
        >
          {{ $t('solver.row.pricePerPersonMonth.unit') }}
        </span>
        <span
          v-else
          style="margin-left: 8px; display: flex; align-items: center"
        >
          {{ getPriceUnit(localParam.hourlyRateLower) }}
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
            v-model="localParam.hourlyRate"
            :number="true"
            :positive-number="true"
            :max-digits="6"
            :additional-rules="hourlyRateRules"
            :aria-label="$t('solver.row.hourlyRate.name')"
            style-set="outlined"
            @input="onInputHourlyRate"
          />
        </Sheet>
        <Sheet v-if="!isEditing" class="text-right">
          <span>
            {{ getDefaultPrice(localParam.hourlyRate) }}
          </span>
        </Sheet>
        <span
          v-if="!isEditing"
          style="margin-left: 8px; display: flex; align-items: center"
        >
          {{ getPriceUnit(localParam.hourlyRate) }}
        </span>
        <span
          v-else
          style="margin-left: 8px; display: flex; align-items: center"
        >
          {{ $t('solver.row.pricePerPersonMonth.unit') }}
        </span>
      </CommonDetailRow>
      <!-- 時間単価（個人ソルバー変更画面） -->
      <CommonDetailRow
        v-else
        :label="$t('solver.row.hourlyRate.name')"
        :is-editing="isEditing"
        :component="true"
      >
        <span style="margin-right: 5px; display: flex; align-items: center">
          {{ $t('solver.row.hourlyRate.lower') }}
        </span>
        <Sheet class="text-right">
          <span>
            {{ getDefaultPrice(localParam.hourlyRateLower) }}
          </span>
        </Sheet>
        <span style="margin-left: 8px; display: flex; align-items: center">
          {{ getPriceUnit(localParam.hourlyRateLower) }}
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
        <Sheet class="text-right">
          <span>
            {{ getDefaultPrice(localParam.hourlyRate) }}
          </span>
        </Sheet>
        <span style="margin-left: 8px; display: flex; align-items: center">
          {{ getPriceUnit(localParam.hourlyRate) }}
        </span>
      </CommonDetailRow>
      <!-- 稼働状況 -->
      <CommonDetailRow
        :label="$t('solver.row.workingStatus.name')"
        :is-editing="isEditing"
        :value="workingStatusValue"
        required
      >
        <RadioGroup
          v-model="localParam.operatingStatus"
          :labels="workingStatusLabels"
          :values="workingStatusValues"
          horizontal
          required
          @change="findWorkingStatus"
        />
      </CommonDetailRow>
      <!-- 提供稼働率（申請・一時保存変更画面） -->
      <CommonDetailRow
        v-if="isNew || isTemporarySaving || isCandidateSaved"
        :label="$t('solver.row.providedUtilizationRate.name')"
        :is-editing="isEditing"
        :component="true"
        required
      >
        <v-flex class="d-flex align-center">
          <span style="margin-right: 10px; display: flex; align-items: center">
            {{ $t('solver.row.providedUtilizationRate.month1') }}
          </span>
          <Sheet v-if="isEditing" width="80" class="text-right">
            <TextField
              v-model="localParam.providedOperatingRate"
              :positive-number="true"
              :range-number-to="100"
              :aria-label="$t('solver.row.providedUtilizationRate.name')"
              style-set="outlined"
              required
              @input="onInputForm"
            />
          </Sheet>
          <Sheet v-if="!isEditing" class="text-right">
            <span>
              {{ localParam.providedOperatingRate }}
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
              v-model="localParam.providedOperatingRateNext"
              :positive-number="true"
              :range-number-to="100"
              :aria-label="$t('solver.row.providedUtilizationRate.name')"
              style-set="outlined"
              required
              @input="onInputForm"
            />
          </Sheet>
          <Sheet v-if="!isEditing" class="text-right">
            <span>
              {{ localParam.providedOperatingRateNext }}
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
              v-model="localParam.operationProspectsMonthAfterNext"
              :max-length="20"
              :aria-label="$t('solver.row.providedUtilizationRate.name')"
              :placeholder="
                $t('solver.row.providedUtilizationRate.placeholder')
              "
              style-set="outlined"
              required
              @input="onInputForm"
            />
          </Sheet>
          <Sheet v-if="!isEditing" class="text-right">
            <span>
              {{ localParam.operationProspectsMonthAfterNext }}
            </span>
          </Sheet>
        </v-flex>
      </CommonDetailRow>
      <!-- 提供稼働率（個人ソルバー変更画面） -->
      <CommonDetailRow
        v-else
        :label="$t('solver.row.providedUtilizationRate.name')"
        :is-editing="isEditing"
        :component="true"
      >
        <v-flex class="d-flex align-center">
          <span style="margin-right: 10px; display: flex; align-items: center">
            {{ $t('solver.row.providedUtilizationRate.month1') }}
          </span>
          <Sheet class="text-right">
            <span>
              {{ localParam.providedOperatingRate }}
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
          <Sheet class="text-right">
            <span>
              {{ localParam.providedOperatingRateNext }}
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
          <Sheet class="text-right">
            <span>
              {{ localParam.operationProspectsMonthAfterNext }}
            </span>
          </Sheet>
        </v-flex>
      </CommonDetailRow>
      <!-- 東証33業種経験/対応可能領域 -->
      <CommonDetailRow
        :label="$t('solver.row.tse33Experience.name')"
        :is-editing="isEditing"
        :value="tse33IndustryExperienceValue"
        required
      >
        <Sheet width="520">
          <Select
            v-model="localParam.tsiAreas"
            :items="tse33IndustryExperienceItems"
            item-text="label"
            item-value="value"
            :placeholder="$t('common.placeholder.selectMulti2')"
            style-set="outlined bgWhite bgTransparent"
            :bg-transparent="true"
            :multiple="true"
            required
            @change="findTse33IndustryExperienceValue"
          />
        </Sheet>
      </CommonDetailRow>
      <!-- 課題マップ50 -->
      <CommonDetailRow
        :label="$t('solver.row.issueMap50.name')"
        :is-editing="isEditing"
        :value="issueMap50Value"
        required
      >
        <Sheet width="520">
          <Select
            v-model="localParam.issueMap50"
            :items="issueMap50Items"
            item-text="label"
            item-value="value"
            :placeholder="$t('common.placeholder.selectMulti2')"
            style-set="outlined bgWhite bgTransparent"
            :bg-transparent="true"
            :multiple="true"
            required
            @change="findIssueMap50Value"
          />
        </Sheet>
      </CommonDetailRow>
    </SolverDetailSection>
    <!-- スクリーニング項目 -->
    <SolverDetailSection
      :is-new="isNew"
      :title="$t('solver.pages.application.detail.section.screening')"
    >
      <!-- 大企業支援実績 -->
      <v-row class="row-subtitle"
        >{{ $t('solver.pages.application.detail.subTitle.enterpriseSupport') }}
      </v-row>
      <!-- Sony Acceleration Platformで新規事業分野における支援実績がある（申請・一時保存変更画面） -->
      <CommonDetailRow
        v-if="isNew || isTemporarySaving || isCandidateSaved"
        :label="
          $t('solver.row.screening.enterpriseSupport.sapSupportAchievement')
        "
        :tall="true"
        :is-editing="isEditing"
        :component="true"
        :escape-value="false"
        required
      >
        <v-flex>
          <RadioGroup
            v-if="isEditing"
            v-model="localParam.screening1.evaluation"
            :labels="screeningLabels"
            :values="screeningValues"
            horizontal
            @change="findScreening($event, 1)"
          />
          <Textarea
            v-if="isEditing"
            v-model="localParam.screening1.evidence"
            :max-length="255"
            style-set="outlined"
            class="mt-3"
            :placeholder="$t('solver.row.screening.placeholder')"
            :disabled="!localParam.screening1.evaluation"
            :required="localParam.screening1.evaluation"
            @input="onInputForm"
          />
          <Sheet v-if="!isEditing" style="overflow-wrap: break-word">
            {{ screening1Value }}
          </Sheet>
          <Sheet
            v-if="!isEditing"
            style="overflow-wrap: break-word; white-space: pre-line"
          >
            {{ localParam.screening1.evidence }}
          </Sheet>
        </v-flex>
      </CommonDetailRow>
      <!-- Sony Acceleration Platformで新規事業分野における支援実績がある（個人ソルバー変更画面） -->
      <CommonDetailRow
        v-else
        :label="
          $t('solver.row.screening.enterpriseSupport.sapSupportAchievement')
        "
        :tall="true"
        :is-editing="isEditing"
        :component="true"
        :escape-value="false"
      >
        <v-flex>
          <Sheet style="overflow-wrap: break-word">
            {{ screening1Value }}
          </Sheet>
          <Sheet style="overflow-wrap: break-word; white-space: pre-line">
            {{ localParam.screening1.evidence }}
          </Sheet>
        </v-flex>
      </CommonDetailRow>
      <!-- 大企業の新規事業分野における支援実績がある（申請・一時保存変更画面） -->
      <CommonDetailRow
        v-if="isNew || isTemporarySaving || isCandidateSaved"
        :label="
          $t('solver.row.screening.enterpriseSupport.newBizSupportAchievement')
        "
        :tall="true"
        :is-editing="isEditing"
        :component="true"
        :escape-value="false"
        required
      >
        <v-flex>
          <RadioGroup
            v-if="isEditing"
            v-model="localParam.screening2.evaluation"
            :labels="screeningLabels"
            :values="screeningValues"
            horizontal
            @change="findScreening($event, 2)"
          />
          <Textarea
            v-if="isEditing"
            v-model="localParam.screening2.evidence"
            :max-length="255"
            style-set="outlined"
            class="mt-3"
            :placeholder="$t('solver.row.screening.placeholder')"
            :disabled="!localParam.screening2.evaluation"
            :required="localParam.screening2.evaluation"
            @input="onInputForm"
          />
          <Sheet v-if="!isEditing" style="overflow-wrap: break-word">
            {{ screening2Value }}
          </Sheet>
          <Sheet
            v-if="!isEditing"
            style="overflow-wrap: break-word; white-space: pre-line"
          >
            {{ localParam.screening2.evidence }}
          </Sheet>
        </v-flex>
      </CommonDetailRow>
      <!-- 大企業の新規事業分野における支援実績がある(個人ソルバー変更画面) -->
      <CommonDetailRow
        v-else
        :label="
          $t('solver.row.screening.enterpriseSupport.newBizSupportAchievement')
        "
        :tall="true"
        :is-editing="isEditing"
        :component="true"
        :escape-value="false"
      >
        <v-flex>
          <Sheet style="overflow-wrap: break-word">
            {{ screening2Value }}
          </Sheet>
          <Sheet style="overflow-wrap: break-word; white-space: pre-line">
            {{ localParam.screening2.evidence }}
          </Sheet>
        </v-flex>
      </CommonDetailRow>
      <!-- 大企業リピート実績 -->
      <v-row class="row-subtitle"
        >{{ $t('solver.pages.application.detail.subTitle.enterpriseRepeat') }}
      </v-row>
      <!-- 大企業の新規事業において同一クライアントとのリピート実績がある（申請・一時保存変更画面） -->
      <CommonDetailRow
        v-if="isNew || isTemporarySaving || isCandidateSaved"
        :label="$t('solver.row.screening.enterpriseRepeat.repeatAchievement')"
        :tall="true"
        :is-editing="isEditing"
        :component="true"
        :escape-value="false"
        required
      >
        <v-flex>
          <RadioGroup
            v-if="isEditing"
            v-model="localParam.screening3.evaluation"
            :labels="screeningLabels"
            :values="screeningValues"
            horizontal
            @change="findScreening($event, 3)"
          />
          <Textarea
            v-if="isEditing"
            v-model="localParam.screening3.evidence"
            :max-length="255"
            style-set="outlined"
            class="mt-3"
            :placeholder="$t('solver.row.screening.placeholder')"
            :disabled="!localParam.screening3.evaluation"
            :required="localParam.screening3.evaluation"
            @input="onInputForm"
          />
          <Sheet v-if="!isEditing" style="overflow-wrap: break-word">
            {{ screening3Value }}
          </Sheet>
          <Sheet
            v-if="!isEditing"
            style="overflow-wrap: break-word; white-space: pre-line"
          >
            {{ localParam.screening3.evidence }}
          </Sheet>
        </v-flex>
      </CommonDetailRow>
      <!-- 大企業の新規事業において同一クライアントとのリピート実績がある（個人ソルバー変更画面） -->
      <CommonDetailRow
        v-else
        :label="$t('solver.row.screening.enterpriseRepeat.repeatAchievement')"
        :tall="true"
        :is-editing="isEditing"
        :component="true"
        :escape-value="false"
      >
        <v-flex>
          <Sheet style="overflow-wrap: break-word">
            {{ screening3Value }}
          </Sheet>
          <Sheet style="overflow-wrap: break-word; white-space: pre-line">
            {{ localParam.screening3.evidence }}
          </Sheet>
        </v-flex>
      </CommonDetailRow>
      <!-- 大企業の新規事業分野において複数の実績がある（申請・一時保存変更画面） -->
      <CommonDetailRow
        v-if="isNew || isTemporarySaving || isCandidateSaved"
        :label="$t('solver.row.screening.enterpriseRepeat.newBizAchievements')"
        :tall="true"
        :is-editing="isEditing"
        :component="true"
        :escape-value="false"
        required
      >
        <v-flex>
          <RadioGroup
            v-if="isEditing"
            v-model="localParam.screening4.evaluation"
            :labels="screeningLabels"
            :values="screeningValues"
            horizontal
            @change="findScreening($event, 4)"
          />
          <Textarea
            v-if="isEditing"
            v-model="localParam.screening4.evidence"
            :max-length="255"
            style-set="outlined"
            class="mt-3"
            :placeholder="$t('solver.row.screening.placeholder')"
            :disabled="!localParam.screening4.evaluation"
            :required="localParam.screening4.evaluation"
            @input="onInputForm"
          />
          <Sheet v-if="!isEditing" style="overflow-wrap: break-word">
            {{ screening4Value }}
          </Sheet>
          <Sheet
            v-if="!isEditing"
            style="overflow-wrap: break-word; white-space: pre-line"
          >
            {{ localParam.screening4.evidence }}
          </Sheet>
        </v-flex>
      </CommonDetailRow>
      <!-- 大企業の新規事業分野において複数の実績がある（個人ソルバー変更画面） -->
      <CommonDetailRow
        v-else
        :label="$t('solver.row.screening.enterpriseRepeat.newBizAchievements')"
        :tall="true"
        :is-editing="isEditing"
        :component="true"
        :escape-value="false"
      >
        <v-flex>
          <Sheet style="overflow-wrap: break-word">
            {{ screening4Value }}
          </Sheet>
          <Sheet style="overflow-wrap: break-word; white-space: pre-line">
            {{ localParam.screening4.evidence }}
          </Sheet>
        </v-flex>
      </CommonDetailRow>
      <!-- 事業化経験 -->
      <v-row class="row-subtitle"
        >{{ $t('solver.pages.application.detail.subTitle.bizExperience') }}
      </v-row>
      <!-- 大企業など新規事業分野における正社員（リーダー）として自ら立ち上げ経験がある（申請・一時保存変更画面） -->
      <CommonDetailRow
        v-if="isNew || isTemporarySaving || isCandidateSaved"
        :label="$t('solver.row.screening.bizExperience.newBizLeaderExperience')"
        :tall="true"
        :is-editing="isEditing"
        :component="true"
        :escape-value="false"
        required
      >
        <v-flex>
          <RadioGroup
            v-if="isEditing"
            v-model="localParam.screening5.evaluation"
            :labels="screeningLabels"
            :values="screeningValues"
            horizontal
            @change="findScreening($event, 5)"
          />
          <Textarea
            v-if="isEditing"
            v-model="localParam.screening5.evidence"
            :max-length="255"
            style-set="outlined"
            class="mt-3"
            :placeholder="$t('solver.row.screening.placeholder')"
            :disabled="!localParam.screening5.evaluation"
            :required="localParam.screening5.evaluation"
            @input="onInputForm"
          />
          <Sheet v-if="!isEditing" style="overflow-wrap: break-word">
            {{ screening5Value }}
          </Sheet>
          <Sheet
            v-if="!isEditing"
            style="overflow-wrap: break-word; white-space: pre-line"
          >
            {{ localParam.screening5.evidence }}
          </Sheet>
        </v-flex>
      </CommonDetailRow>
      <!-- 大企業など新規事業分野における正社員（リーダー）として自ら立ち上げ経験がある（個人ソルバー変更画面） -->
      <CommonDetailRow
        v-else
        :label="$t('solver.row.screening.bizExperience.newBizLeaderExperience')"
        :tall="true"
        :is-editing="isEditing"
        :component="true"
        :escape-value="false"
      >
        <v-flex>
          <Sheet style="overflow-wrap: break-word">
            {{ screening5Value }}
          </Sheet>
          <Sheet style="overflow-wrap: break-word; white-space: pre-line">
            {{ localParam.screening5.evidence }}
          </Sheet>
        </v-flex>
      </CommonDetailRow>
      <!-- 大企業など新規事業分野における正社員（メンバー）としての経験がある（申請・一時保存変更画面） -->
      <CommonDetailRow
        v-if="isNew || isTemporarySaving || isCandidateSaved"
        :label="$t('solver.row.screening.bizExperience.newBizMemberExperience')"
        :tall="true"
        :is-editing="isEditing"
        :component="true"
        :escape-value="false"
        required
      >
        <v-flex>
          <RadioGroup
            v-if="isEditing"
            v-model="localParam.screening6.evaluation"
            :labels="screeningLabels"
            :values="screeningValues"
            horizontal
            @change="findScreening($event, 6)"
          />
          <Textarea
            v-if="isEditing"
            v-model="localParam.screening6.evidence"
            :max-length="255"
            style-set="outlined"
            class="mt-3"
            :placeholder="$t('solver.row.screening.placeholder')"
            :disabled="!localParam.screening6.evaluation"
            :required="localParam.screening6.evaluation"
            @input="onInputForm"
          />
          <Sheet v-if="!isEditing" style="overflow-wrap: break-word">
            {{ screening6Value }}
          </Sheet>
          <Sheet
            v-if="!isEditing"
            style="overflow-wrap: break-word; white-space: pre-line"
          >
            {{ localParam.screening6.evidence }}
          </Sheet>
        </v-flex>
      </CommonDetailRow>
      <!-- 大企業など新規事業分野における正社員（メンバー）としての経験がある（個人ソルバー変更画面） -->
      <CommonDetailRow
        v-else
        :label="$t('solver.row.screening.bizExperience.newBizMemberExperience')"
        :tall="true"
        :is-editing="isEditing"
        :component="true"
        :escape-value="false"
      >
        <v-flex>
          <Sheet style="overflow-wrap: break-word">
            {{ screening6Value }}
          </Sheet>
          <Sheet style="overflow-wrap: break-word; white-space: pre-line">
            {{ localParam.screening6.evidence }}
          </Sheet>
        </v-flex>
      </CommonDetailRow>
      <!-- 黒字化経験 -->
      <v-row class="row-subtitle"
        >{{ $t('solver.pages.application.detail.subTitle.profitExperience') }}
      </v-row>
      <!-- 大企業などでリーダーとしての事業化経験があり、さらに黒字化の経験がある（申請・一時保存変更画面） -->
      <CommonDetailRow
        v-if="isNew || isTemporarySaving || isCandidateSaved"
        :label="
          $t('solver.row.screening.profitExperience.leaderProfitExperience')
        "
        :tall="true"
        :is-editing="isEditing"
        :component="true"
        :escape-value="false"
        required
      >
        <v-flex>
          <RadioGroup
            v-if="isEditing"
            v-model="localParam.screening7.evaluation"
            :labels="screeningLabels"
            :values="screeningValues"
            horizontal
            @change="findScreening($event, 7)"
          />
          <Textarea
            v-if="isEditing"
            v-model="localParam.screening7.evidence"
            :max-length="255"
            style-set="outlined"
            class="mt-3"
            :placeholder="$t('solver.row.screening.placeholder')"
            :disabled="!localParam.screening7.evaluation"
            :required="localParam.screening7.evaluation"
            @input="onInputForm"
          />
          <Sheet v-if="!isEditing" style="overflow-wrap: break-word">
            {{ screening7Value }}
          </Sheet>
          <Sheet
            v-if="!isEditing"
            style="overflow-wrap: break-word; white-space: pre-line"
          >
            {{ localParam.screening7.evidence }}
          </Sheet>
        </v-flex>
      </CommonDetailRow>
      <!-- 大企業などでリーダーとしての事業化経験があり、さらに黒字化の経験がある（個人ソルバー変更画面） -->
      <CommonDetailRow
        v-else
        :label="
          $t('solver.row.screening.profitExperience.leaderProfitExperience')
        "
        :tall="true"
        :is-editing="isEditing"
        :component="true"
        :escape-value="false"
      >
        <v-flex>
          <Sheet style="overflow-wrap: break-word">
            {{ screening7Value }}
          </Sheet>
          <Sheet style="overflow-wrap: break-word; white-space: pre-line">
            {{ localParam.screening7.evidence }}
          </Sheet>
        </v-flex>
      </CommonDetailRow>
      <!-- 大企業などでメンバーとしての事業化経験があり、さらに黒字化の経験がある（申請・一時保存変更画面） -->
      <CommonDetailRow
        v-if="isNew || isTemporarySaving || isCandidateSaved"
        :label="
          $t('solver.row.screening.profitExperience.memberProfitExperience')
        "
        :tall="true"
        :is-editing="isEditing"
        :component="true"
        :escape-value="false"
        required
      >
        <v-flex>
          <RadioGroup
            v-if="isEditing"
            v-model="localParam.screening8.evaluation"
            :labels="screeningLabels"
            :values="screeningValues"
            horizontal
            @change="findScreening($event, 8)"
          />
          <Textarea
            v-if="isEditing"
            v-model="localParam.screening8.evidence"
            :max-length="255"
            style-set="outlined"
            class="mt-3"
            :placeholder="$t('solver.row.screening.placeholder')"
            :disabled="!localParam.screening8.evaluation"
            :required="localParam.screening8.evaluation"
            @input="onInputForm"
          />
          <Sheet v-if="!isEditing" style="overflow-wrap: break-word">
            {{ screening8Value }}
          </Sheet>
          <Sheet
            v-if="!isEditing"
            style="overflow-wrap: break-word; white-space: pre-line"
          >
            {{ localParam.screening8.evidence }}
          </Sheet>
        </v-flex>
      </CommonDetailRow>
      <!-- 大企業などでメンバーとしての事業化経験があり、さらに黒字化の経験がある（個人ソルバー変更画面） -->
      <CommonDetailRow
        v-else
        :label="
          $t('solver.row.screening.profitExperience.memberProfitExperience')
        "
        :tall="true"
        :is-editing="isEditing"
        :component="true"
        :escape-value="false"
      >
        <v-flex>
          <Sheet style="overflow-wrap: break-word">
            {{ screening8Value }}
          </Sheet>
          <Sheet style="overflow-wrap: break-word; white-space: pre-line">
            {{ localParam.screening8.evidence }}
          </Sheet>
        </v-flex>
      </CommonDetailRow>
    </SolverDetailSection>
    <!-- クライテリア項目 -->
    <SolverDetailSection
      :is-new="isNew"
      :title="$t('solver.pages.application.detail.section.criteria')"
    >
      <!-- 1.サービスの現在の提供力 -->
      <v-row class="row-subtitle"
        >{{ $t('solver.pages.application.detail.subTitle.serviceCapacity') }}
      </v-row>
      <!-- 1-1.実践的なノウハウ（申請・一時保存変更画面） -->
      <CommonDetailRow
        v-if="isNew || isTemporarySaving || isCandidateSaved"
        :label="$t('solver.row.criteria.serviceCapacity.practicalKnowhow')"
        :tall="true"
        :is-editing="isEditing"
        :value="localParam.criteria1"
        :escape-value="false"
        required
      >
        <Textarea
          v-model="localParam.criteria1"
          :max-length="255"
          style-set="outlined"
          :placeholder="$t('solver.row.criteria.serviceCapacity.placeholder1')"
          required
          @input="onInputForm"
        />
      </CommonDetailRow>
      <!-- 1-1.実践的なノウハウ（個人ソルバー変更画面） -->
      <CommonDetailRow
        v-else
        :label="$t('solver.row.criteria.serviceCapacity.practicalKnowhow')"
        :tall="true"
        :value="localParam.criteria1"
        :escape-value="false"
      >
        <Sheet style="overflow-wrap: break-word">
          {{ localParam.criteria1 }}
        </Sheet>
      </CommonDetailRow>
      <!-- 1-2.タイムリーな実行力（申請・一時保存変更画面） -->
      <CommonDetailRow
        v-if="isNew || isTemporarySaving || isCandidateSaved"
        :label="$t('solver.row.criteria.serviceCapacity.timelyExecution')"
        :tall="true"
        :is-editing="isEditing"
        :value="localParam.criteria2"
        :escape-value="false"
        required
      >
        <Textarea
          v-model="localParam.criteria2"
          :max-length="255"
          style-set="outlined"
          :placeholder="$t('solver.row.criteria.serviceCapacity.placeholder2')"
          required
          @input="onInputForm"
        />
      </CommonDetailRow>
      <!-- 1-2.タイムリーな実行力（個人ソルバー変更画面） -->
      <CommonDetailRow
        v-else
        :label="$t('solver.row.criteria.serviceCapacity.timelyExecution')"
        :tall="true"
        :value="localParam.criteria2"
        :escape-value="false"
      >
        <Sheet style="overflow-wrap: break-word">
          {{ localParam.criteria2 }}
        </Sheet>
      </CommonDetailRow>
      <!-- 2.顧客対応力 -->
      <v-row class="row-subtitle"
        >{{ $t('solver.pages.application.detail.subTitle.customerSupport') }}
      </v-row>
      <!-- 2-1.顧客ファースト（申請・一時保存変更画面） -->
      <CommonDetailRow
        v-if="isNew || isTemporarySaving || isCandidateSaved"
        :label="$t('solver.row.criteria.customerSupport.customerFirst')"
        :tall="true"
        :is-editing="isEditing"
        :value="localParam.criteria3"
        :escape-value="false"
        required
      >
        <Textarea
          v-model="localParam.criteria3"
          :max-length="255"
          style-set="outlined"
          :placeholder="$t('solver.row.criteria.customerSupport.placeholder1')"
          required
          @input="onInputForm"
        />
      </CommonDetailRow>
      <!-- 2-1.顧客ファースト（個人ソルバー変更画面） -->
      <CommonDetailRow
        v-else
        :label="$t('solver.row.criteria.customerSupport.customerFirst')"
        :tall="true"
        :value="localParam.criteria3"
        :escape-value="false"
      >
        <Sheet style="overflow-wrap: break-word">
          {{ localParam.criteria3 }}
        </Sheet>
      </CommonDetailRow>
      <!-- 2-2.高い品質と顧客からの信頼 （申請・一時保存変更画面）-->
      <CommonDetailRow
        v-if="isNew || isTemporarySaving || isCandidateSaved"
        :label="$t('solver.row.criteria.customerSupport.qualityTrust')"
        :tall="true"
        :is-editing="isEditing"
        :value="localParam.criteria4"
        :escape-value="false"
        required
      >
        <Textarea
          v-model="localParam.criteria4"
          :max-length="255"
          style-set="outlined"
          :placeholder="$t('solver.row.criteria.customerSupport.placeholder2')"
          required
          @input="onInputForm"
        />
      </CommonDetailRow>
      <!-- 2-2.高い品質と顧客からの信頼（個人ソルバー変更画面） -->
      <CommonDetailRow
        v-else
        :label="$t('solver.row.criteria.customerSupport.qualityTrust')"
        :tall="true"
        :value="localParam.criteria4"
        :escape-value="false"
      >
        <Sheet style="overflow-wrap: break-word">
          {{ localParam.criteria4 }}
        </Sheet>
      </CommonDetailRow>
      <!-- 3.将来的な成長性 -->
      <v-row class="row-subtitle"
        >{{ $t('solver.pages.application.detail.subTitle.growthPotential') }}
      </v-row>
      <!-- 3-1.新たな事業機会や成長機会を創出する力（申請・一時保存変更画面） -->
      <CommonDetailRow
        v-if="isNew || isTemporarySaving || isCandidateSaved"
        :label="$t('solver.row.criteria.growthPotential.growthOpportunity')"
        :tall="true"
        :is-editing="isEditing"
        :value="localParam.criteria5"
        :escape-value="false"
        required
      >
        <Textarea
          v-model="localParam.criteria5"
          :max-length="255"
          style-set="outlined"
          :placeholder="$t('solver.row.criteria.growthPotential.placeholder1')"
          required
          @input="onInputForm"
        />
      </CommonDetailRow>
      <!-- 3-1.新たな事業機会や成長機会を創出する力（個人ソルバー変更画面） -->
      <CommonDetailRow
        v-else
        :label="$t('solver.row.criteria.growthPotential.growthOpportunity')"
        :tall="true"
        :value="localParam.criteria5"
        :escape-value="false"
      >
        <Sheet style="overflow-wrap: break-word">
          {{ localParam.criteria5 }}
        </Sheet>
      </CommonDetailRow>
      <!-- 3-2.外部連携推進力（バウンダリースパナー）（申請・一時保存変更画面） -->
      <CommonDetailRow
        v-if="isNew || isTemporarySaving || isCandidateSaved"
        :label="$t('solver.row.criteria.growthPotential.collaborationAbility')"
        :tall="true"
        :is-editing="isEditing"
        :value="localParam.criteria6"
        :escape-value="false"
        required
      >
        <Textarea
          v-model="localParam.criteria6"
          :max-length="255"
          style-set="outlined"
          :placeholder="$t('solver.row.criteria.growthPotential.placeholder2')"
          required
          @input="onInputForm"
        />
      </CommonDetailRow>
      <!-- 3-2.外部連携推進力（バウンダリースパナー）（個人ソルバー変更画面） -->
      <CommonDetailRow
        v-else
        :label="$t('solver.row.criteria.growthPotential.collaborationAbility')"
        :tall="true"
        :value="localParam.criteria6"
        :escape-value="false"
      >
        <Sheet style="overflow-wrap: break-word">
          {{ localParam.criteria6 }}
        </Sheet>
      </CommonDetailRow>
      <!-- 4.Sony Acceleration Platformとの親和性 -->
      <v-row class="row-subtitle"
        >{{ $t('solver.pages.application.detail.subTitle.sapCompatibility') }}
      </v-row>
      <!-- 4-1.ディレクション・ブランドイメージの合致（申請・一時保存変更画面） -->
      <CommonDetailRow
        v-if="isNew || isTemporarySaving || isCandidateSaved"
        :label="$t('solver.row.criteria.sapCompatibility.brandAlignment')"
        :tall="true"
        :is-editing="isEditing"
        :value="localParam.criteria7"
        :escape-value="false"
        required
      >
        <Textarea
          v-model="localParam.criteria7"
          :max-length="255"
          style-set="outlined"
          :placeholder="$t('solver.row.criteria.sapCompatibility.placeholder1')"
          required
          @input="onInputForm"
        />
      </CommonDetailRow>
      <!-- 4-1.ディレクション・ブランドイメージの合致（個人ソルバー変更画面） -->
      <CommonDetailRow
        v-else
        :label="$t('solver.row.criteria.sapCompatibility.brandAlignment')"
        :tall="true"
        :value="localParam.criteria7"
        :escape-value="false"
      >
        <Sheet style="overflow-wrap: break-word">
          {{ localParam.criteria7 }}
        </Sheet>
      </CommonDetailRow>
      <!-- 4-2.長期的思考(持続可能性、自立性)（申請・一時保存変更画面） -->
      <CommonDetailRow
        v-if="isNew || isTemporarySaving || isCandidateSaved"
        :label="$t('solver.row.criteria.sapCompatibility.longTermVision')"
        :tall="true"
        :is-editing="isEditing"
        :value="localParam.criteria8"
        :escape-value="false"
        required
      >
        <Textarea
          v-model="localParam.criteria8"
          :max-length="255"
          style-set="outlined"
          :placeholder="$t('solver.row.criteria.sapCompatibility.placeholder2')"
          required
          @input="onInputForm"
        />
      </CommonDetailRow>
      <!-- 4-2.長期的思考(持続可能性、自立性)（個人ソルバー変更画面） -->
      <CommonDetailRow
        v-else
        :label="$t('solver.row.criteria.sapCompatibility.longTermVision')"
        :tall="true"
        :value="localParam.criteria8"
        :escape-value="false"
      >
        <Sheet style="overflow-wrap: break-word">
          {{ localParam.criteria8 }}
        </Sheet>
      </CommonDetailRow>
    </SolverDetailSection>
    <!-- その他 -->
    <SolverDetailSection
      :is-new="isNew"
      :title="$t('solver.pages.application.detail.section.other')"
    >
      <!-- 備考 -->
      <CommonDetailRow
        :label="$t('solver.row.remarks.name')"
        :tall="true"
        :is-editing="isEditing"
        :value="localParam.notes"
        :escape-value="false"
      >
        <Textarea
          v-model="localParam.notes"
          :max-length="255"
          style-set="outlined"
          @input="onInputForm"
        />
      </CommonDetailRow>
    </SolverDetailSection>
  </v-form>
</template>

<script lang="ts">
import { sha3_256 as sha3 } from 'js-sha3'
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
import { solverCorporationStore } from '~/store'
import { IGetSelectItemsResponse } from '~/types/Master'
import { IFile } from '~/utils/upload'
import { GetSolverByIdResponse } from '~/models/Solver'
import { IGetSolverByIdResponse } from '~/types/Solver'

export default CommonDetailRows.extend({
  name: 'SolverApplicationRows',
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
    // 個人ソルバー情報
    solver: {
      type: Object as PropType<IGetSolverByIdResponse>,
      required: false,
    },
    isNew: {
      type: Boolean,
      default: true,
    },
    isEditing: {
      type: Boolean,
      default: true,
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
    isDisplayApplicationProject: {
      type: Boolean,
      default: true,
    },
    // 一時保存ボタン非活性フラグ
    isTemporaryDisabled: {
      type: Boolean,
      default: true,
    },
    // 添付ファイルのリフレッシュ
    fileRefresh: {
      type: Boolean,
      default: true,
    },
  },
  data() {
    return {
      localParam: {
        ...new GetSolverByIdResponse(),
        solverApplicationId: '',
        solverApplicationName: '',
        deleteSolverApplicationIds: [] as string[],
      },
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
      // スクリーニング項目ラジオボタン
      screeningLabels: this.$t('solver.row.screening.radio.labels'),
      screeningValues: this.$t('solver.row.screening.radio.values'),
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
      // スクリーニング項目1ラジオボタン確認画面表示用
      screening1Value: (
        this.$t('solver.row.screening.radio.labels') as unknown as string[]
      )[0],
      // スクリーニング項目2ラジオボタン確認画面表示用
      screening2Value: (
        this.$t('solver.row.screening.radio.labels') as unknown as string[]
      )[0],
      // スクリーニング項目3ラジオボタン確認画面表示用
      screening3Value: (
        this.$t('solver.row.screening.radio.labels') as unknown as string[]
      )[0],
      // スクリーニング項目4ラジオボタン確認画面表示用
      screening4Value: (
        this.$t('solver.row.screening.radio.labels') as unknown as string[]
      )[0],
      // スクリーニング項目5ラジオボタン確認画面表示用
      screening5Value: (
        this.$t('solver.row.screening.radio.labels') as unknown as string[]
      )[0],
      // スクリーニング項目6ラジオボタン確認画面表示用
      screening6Value: (
        this.$t('solver.row.screening.radio.labels') as unknown as string[]
      )[0],
      // スクリーニング項目7ラジオボタン確認画面表示用
      screening7Value: (
        this.$t('solver.row.screening.radio.labels') as unknown as string[]
      )[0],
      // スクリーニング項目8ラジオボタン確認画面表示用
      screening8Value: (
        this.$t('solver.row.screening.radio.labels') as unknown as string[]
      )[0],
      // 稼働状況確認画面表示用
      workingStatusValue: (
        this.$t('solver.row.workingStatus.radio.labels') as unknown as string[]
      )[0],
      // 東証33業種経験/対応可能領域確認画面表示用
      tse33IndustryExperienceValue: '',
      // 課題マップ50確認画面表示用
      issueMap50Value: '',
      // 個人ソルバー名確認画面表示用
      individualSolverName: '',
      // 案件コード
      projectCode: '',
      // 案件コードバリデーション
      additionalRules: [] as Function[],
      // 人月単価用ルール配列
      personMonthRules: [] as Function[],
      // 時間単価用ルール配列
      hourlyRateRules: [] as Function[],
      // formの値を変更したか否か
      isChanged: false,
      // 削除対象案件IDリスト
      deleteSolverApplicationIds: [] as string[],
      // DOM監視
      observer: null as unknown as MutationObserver,
    }
  },
  async updated() {
    if (!this.isDisplayApplicationProject && !this.isChanged) {
      await this.handleInput()
      if (!this.isValid) {
        this.showErrorBar(
          this.$t('solver.pages.candidate.certification.errorMessage')
        )
      } else {
        this.clearErrorBar()
      }
    }
  },
  watch: {
    // formのバリデーションが変化した時の処理
    isValid() {
      this.$emit('change:isValid', this.changeIsValid(this.isValid))
    },
    // 個人ソルバー名を変更した時の処理
    solver: {
      handler(newValue) {
        this.localParam = {
          ...newValue,
        }
        this.findSex(newValue.sex, false)
        this.findEnglishLevel(newValue.englishLevel, false)
        this.findConsultingFirmExperience(newValue.isConsultingFirm, false)
        // screening1 から screening8 までループで処理
        for (let i = 1; i <= 8; i++) {
          if (!newValue[`screening${i}`]) {
            newValue[`screening${i}`] = {
              evaluation: false,
              evidence: '',
            }
          }
          this.findScreening(newValue[`screening${i}`].evaluation, i, false)
        }
        this.findWorkingStatus(newValue.operatingStatus, false)
        this.findTse33IndustryExperienceValue(newValue.tsiAreas, false)
        this.findIssueMap50Value(newValue.issueMap50, false)
        this.$emit('inputForm', this.localParam)

        if (
          newValue.registrationStatus === 'temporary_saving' &&
          newValue.solverApplications &&
          newValue.solverApplications.length > 0
        ) {
          const solverApplications = JSON.parse(
            JSON.stringify(newValue.solverApplications)
          )
          this.projectCode = solverApplications[0].projectCode
          this.localParam.solverApplicationId = solverApplications[0].id
          this.localParam.solverApplicationName = solverApplications[0].name
        }
        this.localParam.birthDay = this.getEditableBirthday(
          this.localParam.birthDay
        )
      },
    },
    isEditing: {
      handler(newValue) {
        if (newValue) {
          this.localParam.birthDay = this.getEditableBirthday(
            this.localParam.birthDay
          )
        }
      },
      immediate: true,
    },
  },
  computed: {
    // 添付ファイル用法人ソルバーID
    solverCorporationIdFile(): string {
      if (solverCorporationStore.id) {
        return solverCorporationStore.id
      } else {
        return this.solver.corporateId as string
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
    /** 一時保存中の個人ソルバーか */
    isTemporarySaving() {
      if (
        this.solver &&
        this.solver.registrationStatus === 'temporary_saving'
      ) {
        return true
      } else {
        return false
      }
    },
    /** ソルバー候補かつ個人ソルバー登録済みか */
    isCandidateSaved() {
      if (
        this.solver &&
        !this.solver.isSolver &&
        this.solver.registrationStatus === 'saved'
      ) {
        return true
      } else {
        return false
      }
    },
  },
  mounted() {
    // Formの変更を検知し、エラーを監視する
    this.observer = new MutationObserver(this.handleMutation)
    this.observer.observe((this.$refs.form as any).$el, {
      childList: true,
      subtree: true,
    })
  },
  beforeDestroy() {
    // コンポーネント破棄時にオブザーバーを停止
    if (this.observer) {
      this.observer.disconnect()
    }
  },
  methods: {
    handleMutation(mutationsList: any) {
      mutationsList.forEach((mutation: any) => {
        if (mutation.type === 'childList') {
          const form: any = this.$refs.form
          // 表示されているエラーメッセージ表示領域を取得
          const errors: any = form.$el.querySelectorAll('.v-messages__message')
          const errorMessages: any[] = []
          // 各要素のinnerHTMLを取得してエラーメッセージを追加
          errors.forEach((error: any) => {
            errorMessages.push(error.innerHTML)
          })

          // 必須以外のエラーがゼロかつ、個人ソルバー名に値が入っている場合はtrue
          const includesTargetError = () => {
            return (
              errorMessages.filter(
                (msg) => msg !== this.$t('common.rule.required')
              ).length > 0 || this.localParam.name.trim() === ''
            )
          }

          // 必須以外のエラーがゼロかつ、個人ソルバー名に値が入っている場合
          if (
            errorMessages.length === 0 &&
            this.localParam.name.trim() !== ''
          ) {
            this.$emit('updateTemporaryDisabled', false)
          } else {
            // エラーメッセージに必須エラー以外が存在する場合、一時保存ボタンを非活性
            this.$emit('updateTemporaryDisabled', includesTargetError())
          }
        }
      })
    },
    // formの入力値を親コンポーネントに渡す処理
    onInputForm() {
      this.$emit('inputForm', this.localParam)
      this.isChanged = true
      // isValidの値を親コンポーネントに渡す処理
      this.$emit('change:isValid', this.changeIsValid(this.isValid))
    },

    // 人月単価のバリデーション
    onInputPersonMonth() {
      const personMonthLower = Number(this.localParam.pricePerPersonMonthLower)
      const personMonthUpper = Number(this.localParam.pricePerPersonMonth)

      this.personMonthRules = []

      if (
        this.localParam.pricePerPersonMonthLower !== '' &&
        this.localParam.pricePerPersonMonth !== '' &&
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

      this.$emit('inputForm', this.localParam)
      this.isChanged = true
      this.$emit('change:isValid', this.changeIsValid(this.isValid))
    },

    // 時間単価のバリデーション
    onInputHourlyRate() {
      const hourlyRateLower = Number(this.localParam.hourlyRateLower)
      const hourlyRateUpper = Number(this.localParam.hourlyRate)

      this.hourlyRateRules = []

      if (
        this.localParam.hourlyRateLower !== '' &&
        this.localParam.hourlyRate !== '' &&
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

      this.$emit('inputForm', this.localParam)
      this.isChanged = true
      this.$emit('change:isValid', this.changeIsValid(this.isValid))
    },

    // formの入力値を親コンポーネントに渡す処理（登録済添付ファイル）
    onDeleteFile(newValue: IFile) {
      const localParamInputData = {
        registeredFiles: newValue,
        ...this.localParam,
      }
      this.$emit('inputForm', localParamInputData)
      // isValidの値を親コンポーネントに渡す処理
      this.isChanged = true
      this.$emit('change:isValid', this.changeIsValid(this.isValid))
    },

    // formの入力値を親コンポーネントに渡す処理（添付ファイル）
    onInputFile(newValue: IFile) {
      const localParamInputData = {
        files: newValue,
        ...this.localParam,
      }
      this.$emit('inputForm', localParamInputData)
      // isValidの値を親コンポーネントに渡す処理
      this.isChanged = true
      this.$emit('change:isValid', this.changeIsValid(this.isValid))
    },

    // formの入力値を親コンポーネントに渡す処理（個人ソルバー画像）
    onInputImage(newValue: IFile) {
      const solverImageInputData = {
        image: newValue,
        ...this.localParam,
      }
      if (newValue === undefined || newValue.name) {
        this.isChanged = true
        // isValidの値を親コンポーネントに渡す処理
        this.$emit('change:isValid', this.changeIsValid(this.isValid))
      }
      if (newValue === undefined && this.localParam.facePhoto) {
        solverImageInputData.facePhoto.path = ''
        solverImageInputData.facePhoto.fileName = ''
      }
      this.$emit('inputForm', solverImageInputData)
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

    // スクリーニング項目ラジオボタンの表示名を抽出する処理
    findScreening(
      newValue: string,
      screeningNumber: number,
      isEmit: boolean = true
    ) {
      const screeningValues = this.screeningValues as unknown as string[]
      const screeningLabels = this.screeningLabels as unknown as string[]
      const index = screeningValues.indexOf(newValue)
      const screeningProperty = `screening${screeningNumber}Value`
      const screeningKey = `screening${screeningNumber}`
      const thisObject = this as any
      thisObject[screeningProperty] = screeningLabels[index]
      if (!thisObject.localParam[screeningKey]) {
        thisObject.localParam[screeningKey] = {
          evaluation: false,
          evidence: '',
        }
      }
      if (!newValue) {
        thisObject.localParam[screeningKey].evidence = '' // 値をクリア
      }
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
      // 数値部分を抽出し、数値としてソートする関数
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

    // 案件コードを検証し案件IDと案件名を表示する処理
    checkValidateProjectCode() {
      const decodedProjectCode = this.decodeBase64Url(this.projectCode)
      const splittedProjectCode = decodedProjectCode.split(';')
      const reversedProjectId = splittedProjectCode[0]
        .split('')
        .reverse()
        .join('')
      const phrase = sha3(reversedProjectId + splittedProjectCode[1])

      const rule = () => {
        return (
          phrase === splittedProjectCode[2] ||
          `${this.$t('solver.row.applicationProject.errorMessage')}`
        )
      }

      if (phrase === splittedProjectCode[2]) {
        this.localParam.solverApplicationId = splittedProjectCode[0]
        this.localParam.solverApplicationName = splittedProjectCode[1]
        this.additionalRules = []
        this.onInputForm()
      } else {
        this.localParam.solverApplicationId = ''
        this.localParam.solverApplicationName = ''
        this.additionalRules.push(rule)
      }
    },

    // テキストボックスのバリデーションをリセットする処理
    clearAdditionalRules() {
      this.additionalRules = []
    },

    // リセットボタン押下時の処理
    resetProjectCode() {
      this.projectCode = ''
      this.localParam.solverApplicationId = ''
      this.localParam.solverApplicationName = ''
      this.additionalRules = []
    },

    // Base64URLをデコードする処理
    decodeBase64Url(base64UrlString: string) {
      const base64String = base64UrlString.replace(/-/g, '+').replace(/_/g, '/')
      return Buffer.from(base64String, 'base64').toString('utf-8')
    },

    // formのバリデーションを非同期で実施
    async handleInput() {
      if (this.$refs.form) {
        const form = this.$refs.form as any
        this.isValid = await form.validate()
      }
    },

    // 案件を削除
    deleteSolverApplication(solverApplicationId: string) {
      this.localParam.solverApplications =
        this.localParam.solverApplications.filter(
          (app: any) => app.id !== solverApplicationId
        )
      if (!this.deleteSolverApplicationIds.includes(solverApplicationId)) {
        this.deleteSolverApplicationIds.push(solverApplicationId)
      }
      this.localParam.deleteSolverApplicationIds =
        this.deleteSolverApplicationIds
      this.onInputForm()
    },

    changeIsValid(isValid: boolean) {
      return this.isChanged ? isValid : false
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

.row-subtitle {
  font-size: 1rem;
  font-weight: bold;
  margin-top: 1rem;
  margin-left: 0rem;
  margin-bottom: 1rem;
  border-left: 7px solid $c-primary;
  padding-left: 1rem;
}
</style>
