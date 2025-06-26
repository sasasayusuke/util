<template>
  <v-form
    ref="form"
    :value="isValid"
    class="o-detail-rows"
    @input="$listeners['input']"
  >
    <!-- 最終更新者 -->
    <template v-if="!isCreating">
      <LastUpdate
        :user="project.updateUserName"
        :date="project.updateAt"
        :show-term="false"
      />
    </template>
    <!-- 案件ID -->
    <CommonDetailRow
      :label="$t('project.row.id.name')"
      :is-editing="false"
      :value="project.id"
      chip="void"
    >
    </CommonDetailRow>
    <!-- 取引先識別ID （お客様識別ID） -->
    <CommonDetailRow
      :label="$t('project.row.customerId.name')"
      :is-editing="false"
      :value="project.customerId"
      chip="void"
    >
    </CommonDetailRow>
    <!-- 商談ID -->
    <CommonDetailRow
      :label="$t('project.row.salesforceOpportunityId.name')"
      :is-editing="false"
      :value="project.salesforceOpportunityId"
      chip="sf"
    >
    </CommonDetailRow>
    <!-- SF最終更新日時 -->
    <CommonDetailRow
      :label="$t('project.row.salesforceUpdateAt.name')"
      :is-editing="false"
      :value="project.salesforceUpdateAt"
      chip="sf"
    >
    </CommonDetailRow>
    <!-- 商談名（案件名） -->
    <CommonDetailRow
      :label="$t('project.row.name.name')"
      required
      :is-editing="isEditing"
      :value="project.name"
      chip="sf"
    >
      <Sheet width="300">
        <TextField
          v-model="localParam.name"
          :max-length="120"
          :aria-label="$t('project.row.name.name')"
          style-set="outlined"
          required
        />
      </Sheet>
    </CommonDetailRow>
    <!-- 取引先名（お客様名） -->
    <CommonDetailRow
      :label="$t('project.row.customerName.name')"
      required
      :is-editing="isEditing"
      :value="project.customerName"
      chip="sf"
    >
      <Sheet width="300">
        <AutoComplete
          v-model="localParam.customerId"
          style-set="outlined"
          :items="suggestCustomers"
          item-text="name"
          item-value="id"
          :max-length="255"
          required
          :placeholder="$t('common.placeholder.autoComplete')"
          @change="getCustomerName()"
        />
      </Sheet>
      <Sheet width="300" class="d-flex ml-6">
        <Icon mr-2 size="12" color="text">icon-org-arrow-right</Icon>
        <Button
          style-set="text-button text-underline"
          class="pl-0"
          :to="forwardToUrl(`/customer/create`)"
        >
          {{ $t('project.pages.create.customer.customerCreate') }}
        </Button>
      </Sheet>
    </CommonDetailRow>
    <!-- サービス区分（サービス名） -->
    <CommonDetailRow
      :label="$t('project.row.serviceTypeName.name')"
      required
      :is-editing="isEditing"
      :value="project.serviceTypeName"
      chip="sf"
    >
      <Sheet width="300">
        <Select
          v-model="localParam.serviceType"
          required
          :is-loading="isLoading.serviceTypes"
          style-set="outlined"
          :items="serviceTypes.serviceTypes"
          item-text="name"
          item-value="id"
          :placeholder="$t('project.row.serviceTypeName.placeholder')"
        />
      </Sheet>
    </CommonDetailRow>
    <!-- 粗利メイン課（アンケート集計課） -->
    <CommonDetailRow
      :label="$t('project.row.supporterOrganizationName.name')"
      :value="project.supporterOrganizationName"
      :is-editing="isEditing"
      chip="sf"
    >
      <Sheet width="300">
        <Select
          v-model="localParam.supporterOrganizationId"
          :items="supporterOrganizationsWithNullChoice"
          item-text="name"
          item-value="id"
          style-set="outlined"
          :placeholder="$t('project.row.supporterOrganizationName.placeholder')"
        />
      </Sheet>
    </CommonDetailRow>
    <!-- 新規・更新 -->
    <CommonDetailRow
      :label="$t('project.row.createNew.name')"
      :is-editing="isEditing"
      :value="
        project.createNew
          ? $t('common.detail.new')
          : project.createNew !== null
          ? $t('common.detail.update')
          : null
      "
      chip="sf"
    >
      <RadioGroup
        v-model="localParam.createNew"
        :labels="$t('project.row.createNew.radio').labels"
        :values="$t('project.row.createNew.radio').values"
        horizontal
      />
    </CommonDetailRow>
    <!-- 取引先担当者（お客様担当者） -->
    <CommonDetailRow
      :label="$t('project.row.salesforceMainCustomer.name')"
      :is-editing="isEditing"
      tall
      chip="sf"
    >
      <template #isNotEditing>
        <table class="o-common-detail-rows__table">
          <tr>
            <th>
              {{ $t('project.row.salesforceMainCustomer.detail.name') }}
            </th>
            <td>
              {{ project.salesforceMainCustomer.name }}
            </td>
          </tr>
          <tr>
            <th>
              {{ $t('project.row.salesforceMainCustomer.detail.email') }}
            </th>
            <td>
              {{ project.salesforceMainCustomer.email }}
            </td>
          </tr>
          <tr>
            <th>
              {{
                $t('project.row.salesforceMainCustomer.detail.organizationName')
              }}
            </th>
            <td>
              {{ project.salesforceMainCustomer.organizationName }}
            </td>
          </tr>
          <tr>
            <td colspan="2">
              <Checkbox
                v-model="project.isSurveyEmailToSalesforceMainCustomer"
                class="mr-7 mt-2 mb-1 pt-0"
                :label="
                  $t(
                    'project.row.salesforceMainCustomer.detail.isAnonymousSurveyRespondent'
                  )
                "
                hide-details
                readonly
                :disabled="!project.isSurveyEmailToSalesforceMainCustomer"
              />
            </td>
          </tr>
        </table>
      </template>
      <table class="o-common-detail-rows__table">
        <tr>
          <th>
            {{ $t('project.row.salesforceMainCustomer.detail.name') }}
          </th>
          <td>
            <Sheet width="300">
              <TextField
                v-model="localParam.salesforceMainCustomer.name"
                :max-length="120"
                :aria-label="
                  $t('project.row.salesforceMainCustomer.detail.name')
                "
                style-set="outlined"
                @input="syncDedicatedSurveyUser"
              />
            </Sheet>
          </td>
        </tr>
        <tr>
          <th>
            {{ $t('project.row.salesforceMainCustomer.detail.email') }}
          </th>
          <td>
            <Sheet width="300">
              <TextField
                v-model="localParam.salesforceMainCustomer.email"
                :aria-label="
                  $t('project.row.salesforceMainCustomer.detail.email')
                "
                :max-length="256"
                :email="isInputEmail()"
                style-set="outlined"
                @input="syncDedicatedSurveyUser"
                @change="
                  localParam.salesforceMainCustomer.email = adjustEmail(
                    localParam.salesforceMainCustomer.email
                  )
                  syncDedicatedSurveyUser()
                "
              />
            </Sheet>
          </td>
        </tr>
        <tr>
          <th>
            {{
              $t('project.row.salesforceMainCustomer.detail.organizationName')
            }}
          </th>
          <td>
            <Sheet width="300">
              <TextField
                v-model="localParam.salesforceMainCustomer.organizationName"
                :aria-label="
                  $t(
                    'project.row.salesforceMainCustomer.detail.organizationName'
                  )
                "
                :max-length="80"
                style-set="outlined"
              />
            </Sheet>
          </td>
        </tr>
        <tr>
          <td colspan="2">
            <Checkbox
              v-model="localParam.isSurveyEmailToSalesforceMainCustomer"
              class="mr-7 mt-2 mb-1 pt-0"
              :label="
                $t(
                  'project.row.salesforceMainCustomer.detail.isAnonymousSurveyRespondent'
                )
              "
              hide-details
              :disabled="!isAllowConfigureAnonymous()"
              @change="onChangeSurveyRespondent"
            />
          </td>
        </tr>
      </table>
    </CommonDetailRow>

    <!-- 匿名アンケート送信先指定 -->
    <CommonDetailRow
      :label="$t('project.row.anonymousSurveyMailSendSetting.name')"
      :is-editing="isEditing"
      :disabled="
        isEditing
          ? !isAllowConfigureAnonymous()
            ? true
            : localParam.isSurveyEmailToSalesforceMainCustomer
          : project.isSurveyEmailToSalesforceMainCustomer
      "
      chip="void"
    >
      <template #isNotEditing>
        <table class="o-common-detail-rows__table">
          <tr>
            <th>
              {{ $t('project.row.anonymousSurveyMailSendSetting.detail.name') }}
            </th>
            <td v-if="project.isSurveyEmailToSalesforceMainCustomer">
              {{ project.salesforceMainCustomer.name }}
            </td>
            <td v-else>{{ project.dedicatedSurveyUserName }}</td>
          </tr>
          <tr>
            <th>
              {{
                $t('project.row.anonymousSurveyMailSendSetting.detail.email')
              }}
            </th>
            <td v-if="project.isSurveyEmailToSalesforceMainCustomer">
              {{ project.salesforceMainCustomer.email }}
            </td>
            <td v-else>{{ project.dedicatedSurveyUserEmail }}</td>
          </tr>
        </table>
      </template>
      <table class="o-common-detail-rows__table">
        <tr>
          <th>
            {{ $t('project.row.anonymousSurveyMailSendSetting.detail.name') }}
          </th>
          <td>
            <Sheet width="300">
              <TextField
                v-model="localParam.dedicatedSurveyUserName"
                :max-length="120"
                :aria-label="
                  $t('project.row.anonymousSurveyMailSendSetting.detail.name')
                "
                style-set="outlined"
                :readonly="localParam.isSurveyEmailToSalesforceMainCustomer"
                :disabled="
                  localParam.isSurveyEmailToSalesforceMainCustomer ||
                  !isAllowConfigureAnonymous()
                "
              />
            </Sheet>
          </td>
        </tr>
        <tr>
          <th>
            {{ $t('project.row.anonymousSurveyMailSendSetting.detail.email') }}
          </th>
          <td>
            <Sheet width="300">
              <TextField
                v-model="localParam.dedicatedSurveyUserEmail"
                :aria-label="
                  $t('project.row.anonymousSurveyMailSendSetting.detail.email')
                "
                :max-length="256"
                style-set="outlined"
                :readonly="localParam.isSurveyEmailToSalesforceMainCustomer"
                :disabled="
                  localParam.isSurveyEmailToSalesforceMainCustomer ||
                  !isAllowConfigureAnonymous()
                "
                :email="isInputAnonymousSurveyRespondentEmail()"
                @change="
                  localParam.dedicatedSurveyUserEmail = adjustEmail(
                    localParam.dedicatedSurveyUserEmail
                  )
                "
              />
            </Sheet>
          </td>
        </tr>
      </table>
    </CommonDetailRow>

    <!-- 匿名アンケートパスワード -->
    <CommonDetailRow
      :label="$t('project.row.anonymousSurveyPassword.name')"
      :is-editing="isEditing"
      :value="project.surveyPassword"
      chip="void"
      :disabled="isEditing && !isAllowConfigureAnonymous()"
    >
      <table class="o-common-detail-rows__table">
        <tr>
          <th>
            {{ localParam.surveyPassword }}
          </th>
          <td>
            <Sheet width="300">
              <Button
                style-set="small-primary"
                outlined
                width="96"
                :disabled="!isAllowConfigureAnonymous()"
                @click="changePassword"
                >{{ $t('common.button.changePassword') }}</Button
              >
            </Sheet>
          </td>
        </tr>
      </table>
    </CommonDetailRow>

    <!-- 商談所有者（営業担当者） -->
    <CommonDetailRow
      :label="$t('project.row.mainSalesUserName.name')"
      required
      :is-editing="isEditing"
      :value="project.mainSalesUserName"
      chip="sf"
    >
      <Sheet width="300">
        <AutoComplete
          v-model="localParam.mainSalesUserId"
          style-set="outlined"
          :items="suggestSalesUsers"
          item-text="name"
          item-value="id"
          :placeholder="$t('common.placeholder.autoComplete')"
          :loading="isLoading.suggestSalesUsers"
          :max-length="120"
          required
        />
      </Sheet>
    </CommonDetailRow>
    <!-- フェーズ -->
    <CommonDetailRow
      :label="$t('project.row.phase.name')"
      required
      :is-editing="isCreating"
      :value="project.phase"
      chip="sf"
    >
      <Sheet width="300">
        <Select
          v-model="localParam.phase"
          required
          style-set="outlined"
          :items="$t('project.row.phase.select')"
          :placeholder="$t('project.row.phase.placeholder')"
        />
      </Sheet>
    </CommonDetailRow>
    <!-- カスタマーサクセス -->
    <CommonDetailRow
      :label="$t('project.row.customerSuccess.name')"
      tall
      :is-editing="isEditing"
      :value="customerSuccessValue"
      :escape-value="false"
      chip="sf"
    >
      <Textarea
        v-model="localParam.customerSuccess"
        :max-length="512"
        :aria-label="$t('project.row.customerSuccess.name')"
        style-set="outlined"
      />
    </CommonDetailRow>
    <!-- 支援開始日・支援終了日（支援期間） -->
    <CommonDetailRow
      :label="$t('project.row.supportDateFromTo.name')"
      required
      :is-editing="
        isEditing && (!project.salesforceOpportunityId || isSystemAdmin())
      "
      :value="`${project.supportDateFrom} 〜 ${project.supportDateTo}`"
      chip="sf"
    >
      <v-container>
        <v-row>
          <v-col md="auto" class="pa-0" align-self="start">
            <DateSelect
              v-model="localParam.supportDateFrom"
              :date="project.supportDateFrom.replaceAll('/', '-')"
              required
              :max-length="10"
              :allowed-dates="allowedDatesFrom"
            />
          </v-col>
          <v-col md="auto" class="py-0 px-1" align-self="start">
            <Sheet class="d-flex align-center" height="32"> 〜 </Sheet>
          </v-col>
          <v-col md="auto" class="pa-0" align-self="start">
            <DateSelect
              v-model="localParam.supportDateTo"
              :date="project.supportDateTo.replaceAll('/', '-')"
              required
              :max-length="10"
              :allowed-dates="allowedDatesTo"
              is-no-icon
            />
          </v-col>
        </v-row>
      </v-container>
    </CommonDetailRow>
    <!-- FY粗利 -->
    <CommonDetailRow
      :label="$t('project.row.profitFY.name')"
      :text="$t('project.row.profitFY.currency')"
      required
      :is-editing="isEditing"
      :value="profitYear"
      chip="sf"
    >
      <div class="d-flex align-center">
        <Sheet width="300">
          <TextField
            v-model="localParam.profit.year"
            type="number"
            :max-digits="18"
            style-set="outlined"
            positive-number
            number
            :aria-label="$t('project.row.profitFY.labels')"
            required
          />
        </Sheet>
        &nbsp;{{ $t('project.row.profitFY.currency') }}
      </div>
    </CommonDetailRow>
    <!-- サービス責任者（初期値） -->
    <CommonDetailRow
      :label="$t('project.row.serviceManagerName.name')"
      :disabled="!isCreating"
      :is-editing="isCreating"
      :value="project.serviceManagerName"
      chip="sf"
    >
    </CommonDetailRow>
    <!-- プロデューサー（初期値） -->
    <CommonDetailRow
      :label="$t('project.row.mainSupporterUserName.name')"
      :disabled="!isCreating"
      :is-editing="isCreating"
      :value="project.salesforceMainSupporterUserName"
      chip="sf"
    >
      <Sheet width="300">
        <AutoComplete
          v-model="producerId"
          :items="supporterAutoComplete"
          item-text="name"
          item-value="id"
          style-set="outlined"
          :max-length="120"
          :placeholder="$t('common.placeholder.autoComplete')"
          @input="getProducerName"
        />
      </Sheet>
    </CommonDetailRow>
    <!-- アクセラレーター（初期値） -->
    <CommonDetailRow
      :label="$t('project.row.supporterUsers.name')"
      :disabled="!isCreating"
      :is-editing="isCreating"
      chip="sf"
    >
      <v-container fluid pa-0>
        <v-row
          v-for="(item, index) in localParam.salesforceSupporterUserNames"
          :key="index"
          no-gutters
        >
          <v-col :class="{ 'mt-3': index > 0 }">
            <Sheet
              :width="
                index &&
                index === localParam.salesforceSupporterUserNames.length - 1
                  ? 372
                  : 336
              "
              class="d-flex"
            >
              <AutoComplete
                v-model="acceleratorIds[index]"
                style-set="outlined"
                :items="supporterAutoComplete"
                item-text="name"
                item-value="id"
                :placeholder="$t('common.placeholder.autoComplete')"
                :max-length="120"
                @change="getAcceleratorName(index)"
              />
              <Button
                v-if="
                  index === localParam.salesforceSupporterUserNames.length - 1
                "
                :aria-label="$t('common.button.add')"
                style-set="icon-addable"
                class="ml-2"
                @click="addSalesforceSupporterUserNames"
              >
                <Icon size="28">icon-org-button-plus-outline</Icon>
              </Button>
              <Button
                v-if="localParam.salesforceSupporterUserNames.length > 1"
                :aria-label="$t('common.button.delete')"
                style-set="icon-addable"
                class="ml-2"
                @click="removeSalesforceSupporterUserNames(index)"
              >
                <Icon size="28">icon-org-button-minus-outline</Icon>
              </Button>
            </Sheet>
          </v-col>
        </v-row>
      </v-container>
      <template #isNotEditing>
        <span
          v-for="(user, index) in project.salesforceSupporterUserNames"
          :key="index"
        >
          {{ user }}
          <span v-if="index < project.salesforceSupporterUserNames.length - 1"
            >／</span
          >
        </span>
      </template>
    </CommonDetailRow>
    <!-- 延べ契約時間 -->
    <CommonDetailRow
      :label="$t('project.row.totalContractTime.name')"
      required
      :is-editing="isEditing"
      :value="`${project.totalContractTime}h`"
      chip="sf"
    >
      <v-container>
        <v-row>
          <v-col md="auto" align-self="center" class="pa-0">
            <Sheet width="80">
              <TextField
                v-model="localParam.totalContractTime"
                style-set="outlined"
                :placeholder="$t('project.row.totalContractTime.placeholder')"
                :positive-number-digits="16"
                :decimal-number-digits="1"
                type="number"
                :aria-label="$t('project.row.totalContractTime.labels')"
                number
                required
              />
            </Sheet>
          </v-col>
          <v-col md="auto" class="py-0 pl-1">
            <Sheet class="d-flex align-center" height="32"> h </Sheet>
          </v-col>
        </v-row>
      </v-container>
    </CommonDetailRow>
    <!-- お客様メンバー一覧 -->
    <CommonDetailRow
      :label="$t('project.row.customerUsers.name')"
      :is-editing="isEditing"
      chip="void"
      tall
    >
      <template #isNotEditing>
        <table class="o-common-detail-rows__table">
          <tr class="vertical-top">
            <th>
              {{ $t('project.row.customerUsers.mainCustomer') }}<br />
              <span class="annotation-text">{{
                $t('project.row.customerUsers.annotation')
              }}</span>
            </th>
            <td>{{ project.mainCustomerUserName }}</td>
          </tr>
          <tr>
            <th>{{ $t('project.row.customerUsers.customer') }}</th>
            <td>
              <span v-for="(user, index) in project.customerUsers" :key="index">
                {{ user.name }}
                <span v-if="index < project.customerUsers.length - 1">／</span>
              </span>
            </td>
          </tr>
        </table>
      </template>
      <CustomerList
        :project="project"
        :local-project="localParam"
        :main-customer="{
          id: localParam.mainCustomerUserId,
          name: localParam.mainCustomerUserName,
        }"
        :customers="localParam.customerUsers || []"
        :company="{
          id: localParam.customerId,
          name: localParam.customerName,
        }"
        :is-creating="isCreating"
        :suggest-users="suggestUsers"
        :customer-name="localParam.customerName"
        :selected-customer-id="localParam.customerId"
        :selected-customer-name="localParam.customerName"
        @saveCustomerEdit="saveCustomerEdit"
        @saveCustomerCreate="saveCustomerCreate"
        @removeRow="removeRow"
        @resetMain="resetMain"
      />
    </CommonDetailRow>
    <!-- 支援者一覧 -->
    <CommonDetailRow
      :label="$t('project.row.supporterList.name')"
      :is-editing="isEditing"
      chip="void"
      tall
    >
      <template #isNotEditing>
        <table class="o-common-detail-rows__table">
          <tr>
            <th>{{ $t('project.row.supporterList.mainSupporter') }}</th>
            <td>{{ project.mainSupporterUserName }}</td>
          </tr>
          <tr>
            <th>{{ $t('project.row.supporterList.salesforceSupporter') }}</th>
            <td>
              <span
                v-for="(user, index) in project.supporterUsers"
                :key="index"
              >
                {{ user.name }}
                <span v-if="index < project.supporterUsers.length - 1">／</span>
              </span>
            </td>
          </tr>
        </table>
      </template>
      <SupporterList
        :main-supporter="{
          id: localParam.mainSupporterUserId,
          name: localParam.mainSupporterUserName,
        }"
        :supporters="localParam.supporterUsers || []"
        :is-creating="isCreating"
        :supporter-organizations="supporterOrganizations"
        @saveSupporterCreate="saveSupporterCreate"
        @saveSupporterEdit="saveSupporterEdit"
        @removeRow="removeRow"
        @resetMain="resetMain"
      />
    </CommonDetailRow>
    <!-- 工数調査 -->
    <CommonDetailRow
      :label="$t('project.row.isCountManHour.name')"
      required
      :value="
        project.isCountManHour
          ? $t('common.detail.do')
          : $t('common.detail.not')
      "
      :is-editing="isEditing"
      chip="void"
    >
      <RadioGroup
        v-model="localParam.isCountManHour"
        required
        :labels="$t('project.row.isCountManHour.radio').labels"
        :values="$t('project.row.isCountManHour.radio').values"
        horizontal
        :disabled="!isManHourOps()"
      />
    </CommonDetailRow>
    <!-- 個別カルテ記入リマインド -->
    <CommonDetailRow
      :label="$t('project.row.isKarteRemind.name')"
      required
      :value="
        project.isKarteRemind ? $t('common.detail.do') : $t('common.detail.not')
      "
      :is-editing="isEditing"
      chip="void"
    >
      <RadioGroup
        v-model="localParam.isKarteRemind"
        required
        :labels="$t('project.row.isKarteRemind.radio').labels"
        :values="$t('project.row.isKarteRemind.radio').values"
        horizontal
      />
    </CommonDetailRow>
    <!-- マスターカルテ記入リマインド -->
    <CommonDetailRow
      :label="$t('project.row.isMasterKarteRemind.name')"
      required
      :value="
        project.isMasterKarteRemind
          ? $t('common.detail.do')
          : $t('common.detail.not')
      "
      :is-editing="isEditing"
      chip="void"
    >
      <RadioGroup
        v-model="localParam.isMasterKarteRemind"
        required
        :labels="$t('project.row.isMasterKarteRemind.radio').labels"
        :values="$t('project.row.isMasterKarteRemind.radio').values"
        horizontal
      />
    </CommonDetailRow>
    <!-- 有償／無償 -->
    <CommonDetailRow
      :label="$t('project.row.contractType.name')"
      :value="project.contractType"
      :is-editing="isEditing"
      chip="void"
      required
    >
      <RadioGroup
        v-model="localParam.contractType"
        required
        :labels="$t('project.row.contractType.radio').labels"
        :values="$t('project.row.contractType.radio').values"
        horizontal
      />
    </CommonDetailRow>
    <!-- 公開／非公開 -->
    <CommonDetailRow
      :label="$t('project.row.isSecret.name')"
      required
      :value="
        project.isSecret
          ? $t('project.row.isSecret.radio').labels[1]
          : $t('project.row.isSecret.radio').labels[0]
      "
      :is-editing="isEditing"
      chip="void"
    >
      <RadioGroup
        v-model="localParam.isSecret"
        required
        :labels="$t('project.row.isSecret.radio').labels"
        :values="$t('project.row.isSecret.radio').values"
        horizontal
      />
    </CommonDetailRow>
    <!-- ソルバー担当 -->
    <CommonDetailRow
      :label="$t('project.row.isSolverProject.name')"
      required
      :value="
        project.isSolverProject
          ? $t('common.detail.yes')
          : $t('common.detail.no')
      "
      :is-editing="isEditing"
      chip="void"
    >
      <RadioGroup
        v-model="localParam.isSolverProject"
        required
        :labels="$t('project.row.isSolverProject.radio').labels"
        :values="$t('project.row.isSolverProject.radio').values"
        horizontal
        :disabled="!isSurveyOpsOrSystemAdmin()"
      />
    </CommonDetailRow>
  </v-form>
</template>

<script lang="ts">
import CommonDetailRows from '../../common/molecules/CommonDetailRows.vue'
// import CommonUpdateRow from '../../common/molecules/CommonUpdateRow.vue'
import type { PropType } from '~/common/BaseComponent'
import {
  TextField,
  Select,
  Sheet,
  Chip,
  AutoComplete,
  Icon,
  Button,
  RadioGroup,
  Textarea,
  TextLink,
  Checkbox,
} from '~/components/common/atoms/index'
import CommonDetailRow from '~/components/common/molecules/CommonDetailRow.vue'
import LastUpdate from '~/components/common/molecules/LastUpdate.vue'
import DateSelect from '~/components/common/molecules/DateSelect.vue'
import CustomerList from '~/components/project/molecules/CustomerList.vue'
import SupporterList from '~/components/project/molecules/SupporterList.vue'
import { meStore } from '~/store'
import { randomString } from '~/utils/common-functions'

// LocalProject型はGetProjectByIdResponse型と同じ
import {
  CustomerUser,
  SupporterUser,
  GetProjectByIdResponse,
  GetProjectByIdResponse as LocalProject,
} from '~/models/Project'
import { GetServiceTypesResponse } from '~/models/Master'
import { SuggestCustomer } from '~/models/Customer'
import { isLoading } from '~/components/customer/templates/CustomerList.vue'
export { LocalProject }

export default CommonDetailRows.extend({
  // v-model用
  model: {
    prop: 'isValid',
    event: 'input',
  },
  components: {
    TextField,
    Select,
    Sheet,
    Chip,
    CommonDetailRow,
    LastUpdate,
    AutoComplete,
    Icon,
    Button,
    RadioGroup,
    Textarea,
    DateSelect,
    CustomerList,
    SupporterList,
    TextLink,
    Checkbox,
  },
  props: {
    /** 選択した案件詳細 */
    project: {
      type: Object as PropType<GetProjectByIdResponse>,
      required: true,
    },
    /** サービス種別 */
    serviceTypes: {
      type: Object as PropType<GetServiceTypesResponse>,
      required: true,
    },
    /** 取引先のサジェスト用情報 */
    suggestUsers: {
      type: Array,
      required: true,
    },
    /** 取引先のサジェスト用営業情報 */
    suggestSalesUsers: {
      type: Array,
      required: true,
    },
    /** プロデューサーのサジェスト用情報 */
    suggestSupporterUsers: {
      type: Array,
      required: true,
    },
    /** アクセラレーターのサジェスト用情報 */
    suggestMainSupporterUsers: {
      type: Array,
      required: true,
    },
    /** 取引先のサジェスト用顧客情報 */
    suggestCustomers: {
      type: Array as PropType<SuggestCustomer[]>,
      required: true,
    },
    /** 支援者組織一覧 */
    supporterOrganizations: {
      type: Array,
      required: true,
    },
    /** 各APIが読み込み中か */
    isLoading: {
      type: Object as PropType<isLoading>,
      required: true,
    },
    /** 編集中か */
    isEditing: {
      type: Boolean,
      required: true,
    },
    /** 新規作成中か */
    isCreating: {
      type: Boolean,
      default: false,
    },
  },
  data(): {
    localParam: GetProjectByIdResponse
    acceleratorIds: string[]
    producerId: string
    customerUsers: string
    salesforceSupporterUsers: string
    defaultCustomers: CustomerUser[]
    defaultSupporters: SupporterUser[]
    loadingSupporterSuggest: boolean
    surveyPasswordLength: number
  } {
    return {
      localParam: Object.assign(new LocalProject(), this.project),
      acceleratorIds: [''],
      producerId: '',
      customerUsers: '',
      salesforceSupporterUsers: '',
      defaultCustomers: [{ id: '', name: '' } as CustomerUser],
      defaultSupporters: [{ id: '', name: '' } as SupporterUser],
      loadingSupporterSuggest: true,
      surveyPasswordLength: 12,
    }
  },
  created() {
    this.localParam.salesforceSupporterUserNames.push('')
    // ラジオボタン初期値設定
    // 新規・更新 -- 新規
    this.localParam.createNew = true
    //工数調査 -- する
    this.localParam.isCountManHour = true
    //カルテ記入リマインド -- する
    this.localParam.isKarteRemind = true
    //有償／無償 -- 有償
    const defaultContractType: string = this.$t(
      'project.row.contractType.radio.values[0]'
    ) as string
    this.localParam.contractType = defaultContractType
    //公開／非公開 -- 公開
    this.localParam.isSecret = false
  },
  updated() {
    //匿名アンケートパスワードが空の場合は自動生成する
    if (!this.localParam.surveyPassword) {
      this.changePassword()
    }
  },
  computed: {
    /**
     * カスタマーサクセスのvalueを返す
     * @returns フォーマット済みカスタマーサクセス文字列
     */
    customerSuccessValue() {
      if (this.project.customerSuccess) {
        return this.project.customerSuccess.replace(/\r?\n/g, '<br />')
      }
    },
    /**
     * ユーザーのサジェストを生成
     * @returns ユーザーのサジェスト情報配列
     */
    userAutoComplete() {
      const usersList: string[] = []

      this.suggestUsers.forEach((elm: any) => {
        usersList.push(elm.name)
      })
      return usersList
    },
    /**
     * 粗利メイン課（アンケート集計課）で一度選択した後にも未選択にできる
     * @returns 粗利メイン課（アンケート集計課）配列
     */
    supporterOrganizationsWithNullChoice() {
      if (this.localParam.supporterOrganizationId) {
        const fixedSupporterOrganizations = this.supporterOrganizations
        const noChoice = {
          id: '',
          name: '選択してください',
          shortName: '',
        }

        // 一度選択肢を選んだあとは空の選択肢を追加
        fixedSupporterOrganizations.unshift(noChoice)
        return fixedSupporterOrganizations
      } else {
        return this.supporterOrganizations
      }
    },
    /**
     * プロデューサー・アクセサレーターのサジェストを生成
     * @returns プロデューサー・アクセサレーターのサジェスト情報配列
     */
    supporterAutoComplete() {
      if (this.isCreating) {
        return this.suggestSupporterUsers.concat(this.suggestMainSupporterUsers)
      }
    },
    /**
     * FY粗利の数字を文字列に変換
     * @returns フォーマット済みFY粗利文字列
     */
    profitYear() {
      if (this.project.profit.year) {
        return this.project.profit.year.toLocaleString()
      }
    },
  },
  methods: {
    /**
     * customerIdからcustomerNameを取得する
     */
    getCustomerName() {
      this.suggestCustomers.forEach((suggestedCustomer: any) => {
        if (suggestedCustomer.id === this.localParam.customerId) {
          this.localParam.customerName = suggestedCustomer.name
        }
      })
    },
    /** idを元にプロデューサー名を取得 */
    getProducerName() {
      const allSupporterUsers = this.suggestSupporterUsers.concat(
        this.suggestMainSupporterUsers
      )
      // プロデューサー一致の有無のフラグ
      let isProducerMatch = false

      allSupporterUsers.forEach((supporterUser: any) => {
        if (supporterUser.id === this.producerId) {
          this.localParam.salesforceMainSupporterUserName = supporterUser.name
          isProducerMatch = true
        }
      })

      // 一致しない場合は、値を空文字を返す
      if (!isProducerMatch) {
        this.localParam.salesforceMainSupporterUserName = ''
      }
    },
    /**
     * idを元にアクセラレーター名を取得
     * @param index アクセラレーター情報配列インデックス
     */
    getAcceleratorName(index: number) {
      const allSupporterUsers = this.suggestSupporterUsers.concat(
        this.suggestMainSupporterUsers
      )
      // アクセラレーター一致の有無のフラグ
      let isAcceleratorMatch = false

      allSupporterUsers.forEach((supporterUser: any) => {
        this.acceleratorIds.forEach((acceleratorId) => {
          if (supporterUser.id === acceleratorId) {
            this.localParam.salesforceSupporterUserNames[index] =
              supporterUser.name
            isAcceleratorMatch = true
          }
        })
      })

      // 一致しない場合は、値を空文字を返す
      if (!isAcceleratorMatch) {
        this.localParam.salesforceSupporterUserNames[index] = ''
      }
    },
    /** アクセラレーター（初期値）を追加 */
    addSalesforceSupporterUserNames() {
      this.localParam.salesforceSupporterUserNames.push('')
    },
    /** アクセラレーター（初期値）を削除 */
    removeSalesforceSupporterUserNames(index: number) {
      this.localParam.salesforceSupporterUserNames.splice(index, 1)
      this.acceleratorIds.splice(index, 1)
    },
    resetLocalParam() {
      this.localParam = Object.assign(new LocalProject(), this.project)
    },
    /**
     * お客様編集モーダル保存
     * @param customerInfo 入力されたお客様情報
     * @param eventType 編集対象判定文字列
     */
    saveCustomerEdit(customerInfo: { id: ''; name: '' }, eventType: string) {
      const copiedUsers: CustomerUser[] = []
      if (eventType === 'main') {
        this.localParam.mainCustomerUserId = customerInfo.id
        this.localParam.mainCustomerUserName = customerInfo.name
      } else if (eventType === 'member') {
        this.localParam.customerUsers.forEach(
          (customer: any, index: number) => {
            if (customer.id === customerInfo.id) {
              this.localParam.customerUsers.splice(index, 1, customerInfo)
              copiedUsers.push(customerInfo)
            } else {
              this.localParam.customerUsers.splice(index, 1, customer)
            }
          }
        )
        this.localParam.customerUsers = copiedUsers
      }
    },
    /**
     * お客様作成モーダル保存
     * @param customerInfo 入力されたお客様情報
     * @param eventType 編集対象判定文字列
     */
    saveCustomerCreate(customerInfo: { id: ''; name: '' }, eventType: string) {
      if (eventType === 'main') {
        this.localParam.mainCustomerUserId = customerInfo.id
        this.localParam.mainCustomerUserName = customerInfo.name
      } else if (eventType === 'register') {
        this.localParam.customerUsers.push(customerInfo)
      }
    },
    /**
     * 支援者編集モーダル保存
     * @param supporterInfo 入力された支援者情報
     * @param eventType 編集対象判定文字列
     * @param selectedSupporterId 選択中の支援者ID文字列
     */
    saveSupporterEdit(
      supporterInfo: { id: ''; name: '' },
      eventType: string,
      selectedSupporterId: string
    ) {
      if (eventType === 'edit_producer') {
        this.localParam.mainSupporterUserId = supporterInfo.id
        this.localParam.mainSupporterUserName = supporterInfo.name
      } else if (eventType === 'edit_accelerator') {
        this.localParam.supporterUsers.forEach(
          (supporter: any, index: number) => {
            if (supporter.id === selectedSupporterId) {
              this.localParam.supporterUsers.splice(index, 1, supporterInfo)
            } else {
              this.localParam.supporterUsers.splice(index, 1, supporter)
            }
          }
        )
      }
    },
    /**
     * 支援者作成モーダル保存
     * @param supporterInfo 入力された支援者情報
     * @param eventType 編集対象判定文字列
     */
    saveSupporterCreate(
      supporterInfo: { id: ''; name: '' },
      eventType: string
    ) {
      if (eventType === 'add_producer') {
        this.localParam.mainSupporterUserId = supporterInfo.id
        this.localParam.mainSupporterUserName = supporterInfo.name
      } else if (eventType === 'add_accelerator') {
        this.localParam.supporterUsers.push(supporterInfo)
      }
    },
    /**
     * お客様一覧および支援者一覧の項目削除
     * @param type 削除対象判定文字列
     * @param index 選択された行インデックス番号
     */
    removeRow(type: string, index: number) {
      if (type === 'customer') {
        this.localParam.customerUsers.splice(index, 1)
      } else if (type === 'supporter') {
        this.localParam.supporterUsers.splice(index, 1)
      }
    },
    /**
     * お客様(代表)またはプロデューサーをリセット
     * @param type リセット対象判定文字列
     */
    resetMain(type: string) {
      if (type === 'customer') {
        this.localParam.mainCustomerUserId = ''
        this.localParam.mainCustomerUserName = ''
      } else if (type === 'supporter') {
        this.localParam.mainSupporterUserId = ''
        this.localParam.mainSupporterUserName = ''
      }
    },
    /**
     * 支援開始日 入力可能範囲チェック
     * @param val 入力した支援開始日
     * @returns 入力可能範囲判定真偽値
     */
    allowedDatesFrom(val: string) {
      if (this.localParam.supportDateTo !== '') {
        const inputDate = new Date(val)
        const toDate = new Date(this.localParam.supportDateTo)
        return inputDate < toDate
      } else {
        return true
      }
    },
    /**
     * 支援終了日 入力可能範囲チェック
     * @param val 入力した支援終了日
     * @returns 入力可能範囲判定真偽値
     */
    allowedDatesTo(val: string) {
      if (this.localParam.supportDateFrom !== '') {
        const inputDate = new Date(val)
        const fromDate = new Date(this.localParam.supportDateFrom)
        return inputDate > fromDate
      } else {
        return true
      }
    },
    /**
     * 取引先担当者のメールアドレス欄の入力状態を返す
     * @returns 入力状態判定真偽値
     */
    isInputEmail() {
      if (this.localParam.salesforceMainCustomer.email) {
        return true
      } else {
        return false
      }
    },
    /**
     * 匿名アンケート送信宛先指定のメールアドレス欄の入力状態を返す
     * @returns 入力状態判定真偽値
     */
    isInputAnonymousSurveyRespondentEmail() {
      if (this.localParam.dedicatedSurveyUserEmail) {
        return true
      } else {
        return false
      }
    },
    /**
     * 稼働率調査事務局ロールが含まれているアカウントか判定
     * @returns 稼働率調査事務局ロールが含まれているアカウントかの真偽値
     */
    isManHourOps() {
      return meStore.roles.includes('man_hour_ops')
    },
    /**
     * 営業担当者ロールが含まれているアカウントか判定
     * @returns 営業担当者ロールが含まれているアカウントかの真偽値
     */
    isSales() {
      return meStore.roles.includes('sales')
    },
    /**
     * 営業責任者ロールが含まれているアカウントか判定
     * @returns 営業責任者ロールが含まれているアカウントかの真偽値
     */
    isSalesMgr() {
      return meStore.roles.includes('sales_mgr')
    },
    /**
     * アンケート事務局ロールが含まれているアカウントか判定
     * @returns アンケート事務局ロールが含まれているアカウントかの真偽値
     */
    isSurveyOps() {
      return meStore.roles.includes('survey_ops')
    },
    /**
     * システム管理者ロールが含まれているアカウントか判定
     * @returns システム管理者ロールが含まれているアカウントかの真偽値
     */
    isSystemAdmin() {
      return meStore.roles.includes('system_admin')
    },
    /**
     * 事業者責任者ロールが含まれているアカウントか判定
     * @returns 事業者責任者ロールが含まれているアカウントかの真偽値
     */
    isBusinessMgr() {
      return meStore.roles.includes('business_mgr')
    },
    /**
     * アンケート事務局またはシステム管理者ロールが含まれているアカウントか判定
     * @returns アンケート事務局またはシステム管理者ロールが含まれているアカウントかの真偽値
     */
    isSurveyOpsOrSystemAdmin() {
      return this.isSurveyOps() || this.isSystemAdmin()
    },
    /**
     * 匿名アンケート関連の設定が可能なアカウントか判定
     * @returns 匿名アンケート関連の設定が可能なアカウントかの真偽値
     */
    isAllowConfigureAnonymous() {
      return (
        this.isSales() ||
        this.isSalesMgr() ||
        this.isSurveyOps() ||
        this.isSystemAdmin() ||
        this.isBusinessMgr()
      )
    },
    onChangeSurveyRespondent() {
      // check前後の引き継ぎ
      if (this.localParam.isSurveyEmailToSalesforceMainCustomer) {
        this.localParam.dedicatedSurveyUserName = this.localParam
          .salesforceMainCustomer.name
          ? this.localParam.salesforceMainCustomer.name
          : ''
        this.localParam.dedicatedSurveyUserEmail = this.localParam
          .salesforceMainCustomer.email
          ? this.localParam.salesforceMainCustomer.email
          : ''
      }
    },
    changePassword() {
      // 生成に用いる対象文字列セット配列
      const targetLength: { [key: string]: number } = {
        upper: 0,
        lower: 0,
        number: 0,
        symbol: 0,
      }

      // 記号の含有文字数を3～4文字とする
      targetLength.symbol = Math.floor(Math.random() * (4.9 - 3) + 3)

      // 記号の含有文字数の差分を計算
      let remainLength: number = this.surveyPasswordLength - targetLength.symbol

      // 記号以外の対象文字列セット数を計算
      let otherThanSymbolCount: number = 0
      for (const i in targetLength) {
        if (i !== 'symbol') {
          otherThanSymbolCount++
        }
      }

      // 差分を分割する際の最大文字数
      const maxRemainSplitLength: number = Math.ceil(
        remainLength / otherThanSymbolCount
      )
      // 差分を分割する際の最小文字数
      const minRemainSplitLength: number = Math.floor(
        remainLength / otherThanSymbolCount
      )

      // 設定する文字数を配列化
      const arrLength: number[] = []
      while (true) {
        if (remainLength > maxRemainSplitLength) {
          arrLength.push(maxRemainSplitLength)
          remainLength = remainLength - maxRemainSplitLength
        } else {
          arrLength.push(minRemainSplitLength)
          remainLength = remainLength - minRemainSplitLength
        }
        if (remainLength <= 0) {
          break
        }
      }

      // 設定文字数配列をシャッフル
      for (let i = arrLength.length - 1; i > 0; i--) {
        const r: number = Math.floor(Math.random() * (i + 1))
        const tmp: number = arrLength[i]
        arrLength[i] = arrLength[r]
        arrLength[r] = tmp
      }

      // シャッフルした設定文字数配列の値をそれぞれの対象文字の文字数に設定
      let r = 0
      for (const i in targetLength) {
        if (i !== 'symbol') {
          targetLength[i] = arrLength[r]
          r++
        }
      }

      // ランダム文字列を対象文字毎に生成し結合
      let newPassword: string = ''
      for (const i in targetLength) {
        newPassword += randomString(targetLength[i], [i])
      }

      // ランダム文字列を分割し配列化
      const arrNewPassword: string[] = newPassword.split('')

      // ランダム文字列配列をシャッフル
      for (let i = arrNewPassword.length - 1; i > 0; i--) {
        const r: number = Math.floor(Math.random() * (i + 1))
        const tmp: string = arrNewPassword[i]
        arrNewPassword[i] = arrNewPassword[r]
        arrNewPassword[r] = tmp
      }

      // シャッフルしたランダム文字列配列を結合しパスワードに設定
      this.localParam.surveyPassword = arrNewPassword.join('')
    },
    syncDedicatedSurveyUser() {
      if (this.localParam.isSurveyEmailToSalesforceMainCustomer) {
        this.localParam.dedicatedSurveyUserName = this.localParam
          .salesforceMainCustomer.name
          ? this.localParam.salesforceMainCustomer.name
          : ''
        this.localParam.dedicatedSurveyUserEmail = this.localParam
          .salesforceMainCustomer.email
          ? this.localParam.salesforceMainCustomer.email
          : ''
      }
    },
  },
})
</script>

<style lang="scss">
.o-detail-rows {
  .v-text-field__details {
    overflow: visible;
  }
}
.annotation-text {
  @include fontSize('xsmall');
  font-weight: normal;
  color: $c-black-60;
  white-space: nowrap;
  display: block;
}
</style>
<style lang="scss" scoped>
.vertical-top {
  td,
  th {
    vertical-align: top;
  }
}
</style>
