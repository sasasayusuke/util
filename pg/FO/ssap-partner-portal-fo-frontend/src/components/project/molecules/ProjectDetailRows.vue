<template>
  <v-form
    ref="form"
    :value="isValid"
    class="o-detail-rows"
    @input="handleInput"
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
      :is-editing="isEditing && isNotSurpporterAndSurpporterMgr"
      :value="project.name"
      chip="sf"
    >
      <Sheet width="300">
        <TextField
          v-model="localParam.name"
          role="textbox"
          :max-length="120"
          style-set="outlined"
          required
        />
      </Sheet>
    </CommonDetailRow>
    <!-- 取引先名（お客様名） -->
    <CommonDetailRow
      :label="$t('project.row.customerName.name')"
      required
      :is-editing="isEditing && isNotSurpporterAndSurpporterMgr"
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
    </CommonDetailRow>
    <!-- サービス区分（サービス名） -->
    <CommonDetailRow
      :label="$t('project.row.serviceTypeName.name')"
      required
      :is-editing="isEditing && isNotSurpporterAndSurpporterMgr"
      :value="project.serviceTypeName"
      chip="sf"
    >
      <Sheet width="300">
        <Select
          v-model="localParam.serviceType"
          :is-loading="isLoading.serviceTypes"
          style-set="outlined"
          :items="serviceTypes.serviceTypes"
          item-text="name"
          item-value="id"
          :placeholder="$t('project.row.serviceTypeName.placeholder')"
          required
        />
      </Sheet>
    </CommonDetailRow>
    <!-- 粗利メイン課（アンケート集計課） -->
    <CommonDetailRow
      :label="$t('project.row.supporterOrganizationName.name')"
      :value="project.supporterOrganizationName"
      chip="sf"
    >
    </CommonDetailRow>
    <!-- 新規・更新 -->
    <CommonDetailRow
      :label="$t('project.row.createNew.name')"
      :is-editing="isEditing && isNotSurpporterAndSurpporterMgr"
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
      :is-editing="isEditing && isNotSurpporterAndSurpporterMgr"
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
                :disabled="true"
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
                role="textbox"
                :max-length="120"
                style-set="outlined"
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
                role="textbox"
                :max-length="256"
                :email="isInputEmail()"
                style-set="outlined"
                @change="
                  localParam.salesforceMainCustomer.email = adjustEmail(
                    localParam.salesforceMainCustomer.email
                  )
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
                role="textbox"
                :max-length="80"
                style-set="outlined"
              />
            </Sheet>
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
              :disabled="true"
              @change="onChangeSurveyRespondent"
            />
          </td>
        </tr>
      </table>
    </CommonDetailRow>
    <!-- アンケート送信宛先指定 -->
    <CommonDetailRow
      :label="$t('project.row.anonymousSurveyMailSendSetting.name')"
      :is-editing="isEditing"
      :disabled="true"
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
                :disabled="true"
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
                :disabled="true"
                :email="isInputAnonymousSurveyRespondentEmail"
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
    <!-- 商談所有者（営業担当者） -->
    <CommonDetailRow
      :label="$t('project.row.mainSalesUserName.name')"
      required
      :is-editing="isEditing && isNotSurpporterAndSurpporterMgr"
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
    />
    <!-- カスタマーサクセス -->
    <CommonDetailRow
      :label="$t('project.row.customerSuccess.name')"
      tall
      :is-editing="isEditing && isNotSurpporterAndSurpporterMgr"
      :value="project.customerSuccess"
      :escape-value="false"
      chip="sf"
    >
      <Textarea
        v-model="localParam.customerSuccess"
        :max-length="512"
        style-set="outlined"
      />
    </CommonDetailRow>
    <!-- 支援開始日・支援終了日（支援期間） -->
    <CommonDetailRow
      :label="$t('project.row.supportDateFromTo.name')"
      required
      :is-editing="
        isEditing &&
        isNotSurpporterAndSurpporterMgr &&
        !project.salesforceOpportunityId
      "
      :value="`${project.supportDateFrom} 〜 ${project.supportDateTo}`"
      chip="sf"
    >
      <v-container>
        <v-row>
          <v-col md="auto" class="pa-0" align-self="center">
            <DateSelect
              v-model="localParam.supportDateFrom"
              :date="project.supportDateFrom.replaceAll('/', '-')"
              required
              :max-length="10"
              :allowed-dates="allowedDatesFrom"
            />
          </v-col>
          <v-col md="auto" class="py-0 px-1" align-self="center"> 〜 </v-col>
          <v-col md="auto" class="pa-0" align-self="center">
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
      :is-editing="isEditing && isNotSurpporterAndSurpporterMgr"
      :value="profitYear"
      chip="sf"
    >
      <div class="d-flex align-center">
        <Sheet width="300">
          <TextField
            v-model="localParam.profit.year"
            role="textbox"
            type="number"
            :max-digits="18"
            style-set="outlined"
            positive-number
            number
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
    </CommonDetailRow>
    <!-- アクセラレーター（初期値） -->
    <CommonDetailRow
      :label="$t('project.row.supporterUsers.name')"
      :disabled="!isCreating"
      :is-editing="isCreating"
      chip="sf"
    >
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
      :is-editing="isEditing && isNotSurpporterAndSurpporterMgr"
      :value="`${project.totalContractTime}h`"
      chip="sf"
    >
      <v-container>
        <v-row>
          <v-col md="auto" align-self="center" class="pa-0">
            <Sheet width="80">
              <TextField
                v-model="localParam.totalContractTime"
                role="textbox"
                type="number"
                style-set="outlined"
                :positive-number-digits="16"
                :decimal-number-digits="1"
                number
                required
              />
            </Sheet>
          </v-col>
          <v-col md="auto" align-self="center" class="py-0 pl-1"> h </v-col>
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
          <tr>
            <th>{{ $t('project.row.customerUsers.mainCustomer') }}</th>
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
        :is-disabled-main-customer-user-button="
          isDisabledMainCustomerUserButton
        "
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
      chip="void"
    />
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
      :value="
        project.contractType === $t('common.detail.paid')
          ? $t('common.detail.paid')
          : $t('common.detail.free')
      "
      :is-editing="isEditing && isNotSurpporterAndSurpporterMgr"
      chip="void"
      required
    >
      <RadioGroup
        v-model="localParam.contractType"
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
      :is-editing="isEditing && isSurpporterOrSalesMgrOrProducer"
      chip="void"
    >
      <RadioGroup
        v-model="localParam.isSecret"
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
      chip="void"
    >
    </CommonDetailRow>
  </v-form>
</template>

<script lang="ts">
import CommonDetailRows from '../../common/molecules/CommonDetailRows.vue'
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
  Checkbox,
} from '~/components/common/atoms/index'
import CommonDetailRow from '~/components/common/molecules/CommonDetailRow.vue'
import LastUpdate from '~/components/common/molecules/LastUpdate.vue'
import DateSelect from '~/components/common/molecules/DateSelect.vue'
import CustomerList from '~/components/project/molecules/CustomerList.vue'
import SupporterList from '~/components/project/molecules/SupporterList.vue'

// LocalProject型はGetProjectByIdResponse型と同じ
import {
  GetProjectByIdResponse,
  GetProjectByIdResponse as LocalProject,
} from '~/models/Project'
import { GetServiceTypesResponse } from '~/models/Master'
import { SuggestCustomer } from '~/models/Customer'
import { isLoading } from '~/components/customer/templates/CustomerList.vue'
import { meStore } from '~/store'
import { ENUM_USER_ROLE } from '~/types/User'
export { LocalProject }

export default CommonDetailRows.extend({
  name: 'ProjectDetailRows',
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
  } {
    return {
      /** APIレスポンスの案件情報を編集用にコピーしたもの */
      localParam: Object.assign(new LocalProject(), this.project),
    }
  },
  methods: {
    /**
     * customerIdからcustomerNameを取得する
     */
    getCustomerName() {
      for (const i in this.suggestCustomers) {
        if (this.localParam.customerId === this.suggestCustomers[i].id) {
          this.localParam.customerName = this.suggestCustomers[i].name
          break
        }
      }
    },
    /** localParamを初期値に戻す */
    resetLocalParam() {
      this.localParam = Object.assign(new LocalProject(), this.project)
    },
    saveCustomerEdit(customerInfo: { id: ''; name: '' }, eventType: string) {
      const copiedUsers: object[] = []
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
        // @ts-ignore
        this.localParam.customerUsers = copiedUsers
      }
    },
    /** 顧客新規作成 */
    saveCustomerCreate(customerInfo: { id: ''; name: '' }, eventType: string) {
      if (eventType === 'main') {
        this.localParam.mainCustomerUserId = customerInfo.id
        this.localParam.mainCustomerUserName = customerInfo.name
      } else if (eventType === 'register') {
        this.localParam.customerUsers.push(customerInfo)
      }
    },
    /** 顧客編集保存 */
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
    /** 支援者作成モーダル保存 */
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
    /** 支援者等を一列削除 */
    removeRow(type: string, index: number) {
      if (type === 'customer') {
        this.localParam.customerUsers.splice(index, 1)
      } else if (type === 'supporter') {
        this.localParam.supporterUsers.splice(index, 1)
      }
    },
    /** 支援者責任者、もしくは顧客責任者を空にする */
    resetMain(type: string) {
      if (type === 'customer') {
        this.localParam.mainCustomerUserId = ''
        this.localParam.mainCustomerUserName = ''
      } else if (type === 'supporter') {
        this.localParam.mainSupporterUserId = ''
        this.localParam.mainSupporterUserName = ''
      }
    },
    /** 支援期間 From～To逆転チェック */
    allowedDatesFrom(val: string) {
      if (this.localParam.supportDateTo !== '') {
        const inputDate = new Date(val)
        const toDate = new Date(this.localParam.supportDateTo)
        return inputDate < toDate
      } else {
        return true
      }
    },
    /** 支援期間の有効期間始まり */
    allowedDatesTo(val: string) {
      if (this.localParam.supportDateFrom !== '') {
        const inputDate = new Date(val)
        const fromDate = new Date(this.localParam.supportDateFrom)
        return inputDate > fromDate
      } else {
        return true
      }
    },
    /** emailが存在するか */
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
    /** formのバリデーションを非同期で実施 */
    async handleInput() {
      if (this.$refs.form) {
        const form = this.$refs.form as any
        this.isValid = await form.validate()
      }
    },
  },
  computed: {
    /** FY粗利の数字を文字列に変換 */
    profitYear() {
      if (this.project.profit.year) {
        return this.project.profit.year.toLocaleString()
      }
    },
    /**
     * 支援者または支援者責任者かどうかを判定する
     * @returns 支援者または支援者責任者かどうか
     */
    isSurpporterAndSurpporterMgr(): boolean {
      return (
        meStore.role === ENUM_USER_ROLE.SUPPORTER ||
        meStore.role === ENUM_USER_ROLE.SUPPORTER_MGR
      )
    },
    /**
     * 支援者且つ支援者責任者出ないかどうかを判定する
     * @returns 支援者且つ支援者責任者でないかどうか
     */
    isNotSurpporterAndSurpporterMgr(): boolean {
      return (
        meStore.role !== ENUM_USER_ROLE.SUPPORTER &&
        meStore.role !== ENUM_USER_ROLE.SUPPORTER_MGR
      )
    },
    /**
     * プロデューサーまたは支援者責任者または営業担当者または事業者責任者かどうかを判定する
     * @returns プロデューサーまたは支援者責任者または営業担当者または事業者責任者かどうか
     */
    isSurpporterOrSalesMgrOrProducer(): boolean {
      return (
        meStore.role === ENUM_USER_ROLE.SALES ||
        meStore.role === ENUM_USER_ROLE.SUPPORTER_MGR ||
        meStore.role === ENUM_USER_ROLE.BUSINESS_MGR ||
        meStore.id === this.project.mainSupporterUserId
      )
    },
    /**
     * お客様メンバー一覧 - お客様(代表) のボタンを無効するか判定する
     */
    isDisabledMainCustomerUserButton(): boolean {
      return this.isSurpporterAndSurpporterMgr
    },
    /**
     * 「アンケートの宛先に指定する」のチェックボックスにチェックが入った場合、アンケート送信宛先指定を取引先担当者に連動させる
     */
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
  },
})
</script>
