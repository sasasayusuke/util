<template>
  <v-form
    class="o-user-detail-rows pt-4 px-8 pb-0"
    :value="isValid"
    @input="$listeners['input']"
  >
    <!--
      システムロール
      新規作成画面でのみ編集するため、isEditing=isCreatingとする
    -->
    <CommonDetailRow
      :label="$t('user.row.role.name')"
      required
      :is-editing="isCreating"
      :value="rolesToString(user.role)"
    >
      <RadioGroup
        v-model="localParam.role"
        :labels="$t('user.row.role.radio.labels')"
        :values="$t('user.row.role.radio.values')"
        required
        horizontal
        wrap
        hide-details
        @change="onChange"
      />
    </CommonDetailRow>

    <!-- ユーザーネーム -->
    <CommonDetailRow
      :label="$t('user.row.name.name')"
      required
      :is-editing="isEditing"
      :value="user.name"
    >
      <Sheet width="300">
        <TextField
          v-model="localParam.name"
          outlined
          required
          dense
          max-length="120"
          :placeholder="$t('user.row.name.placeholder')"
        />
      </Sheet>
    </CommonDetailRow>

    <!--
      メールアドレス
      新規作成画面でのみ編集するため、isEditing=isCreatingとする
    -->
    <CommonDetailRow
      :label="$t('user.row.email.name')"
      required
      :is-editing="isCreating"
      :value="user.email"
    >
      <Sheet width="300">
        <TextField
          v-model="localParam.email"
          required
          email
          outlined
          dense
          max-length="255"
          :placeholder="$t('user.row.email.placeholder')"
          @change="localParam.email = adjustEmail(localParam.email)"
        />
      </Sheet>
    </CommonDetailRow>

    <!-- 所属会社 -->
    <!-- 顧客の場合 -->
    <template v-if="isCustomer() && isCreating">
      <!-- 新規作成画面 isEditing=isCreating -->
      <CommonDetailRow
        :label="$t('user.row.company.name')"
        required
        :is-editing="isCreating"
        :value="user.customerId"
      >
        <Sheet width="300">
          <AutoComplete
            v-model="localParam.customerId"
            outlined
            dense
            required
            :items="autoCompleteItems()"
            :placeholder="$t('common.placeholder.autoComplete')"
            hide-details
            :loading="isLoading"
            item-text="name"
            item-value="id"
            max-length="255"
            @input="$emit('update', 'customerId', $event)"
          />
        </Sheet>
      </CommonDetailRow>
    </template>

    <!-- 詳細画面 isEditing=!isEditing -->
    <template v-else-if="user.role === 'customer' && !isEditing">
      <CommonDetailRow
        :label="$t('user.row.company.name')"
        required
        :value="user.customerName"
      >
        <Sheet width="300">
          <TextField
            v-model="localParam.customerName"
            outlined
            dense
            required
            max-length="255"
            :placeholder="$t('user.row.company.placeholder')"
          />
        </Sheet>
      </CommonDetailRow>
    </template>

    <!-- 更新画面 isEditing=isEditing -->
    <template v-else-if="user.role === 'customer' && isEditing">
      <CommonDetailRow
        :label="$t('user.row.company.name')"
        required
        :value="user.customerName"
      >
        <Sheet width="300">
          <TextField
            v-model="localParam.customerName"
            outlined
            dense
            required
            max-length="255"
            :placeholder="$t('user.row.company.placeholder')"
          />
        </Sheet>
      </CommonDetailRow>
    </template>

    <!-- 法人ソルバーの場合 -->
    <template v-if="isSolverStaff() && isCreating">
      <!-- 新規作成画面 isEditing=isCreating -->
      <CommonDetailRow
        :label="$t('user.row.company.name')"
        required
        :is-editing="isCreating"
        :value="user.company"
      >
        <Sheet width="300">
          <AutoComplete
            v-model="localParam.company"
            outlined
            dense
            required
            :items="autoSolverCorporationItems()"
            :placeholder="$t('common.placeholder.autoComplete')"
            hide-details
            :loading="isLoading"
            item-text="name"
            item-value="id"
            max-length="255"
            :is-combobox="true"
            @change="setSolverCorporation"
            @input="$emit('update', 'company', $event)"
          />
        </Sheet>
      </CommonDetailRow>
    </template>

    <!-- 詳細画面 isEditing=!isEditing -->
    <template v-else-if="isSolverStaff() && !isEditing">
      <CommonDetailRow
        :label="$t('user.row.company.name')"
        required
        :value="user.company"
      >
        <Sheet width="300">
          <TextField
            v-model="localParam.company"
            outlined
            dense
            required
            max-length="255"
            :placeholder="$t('user.row.company.placeholder')"
          />
        </Sheet>
      </CommonDetailRow>
    </template>

    <!-- 更新画面 isEditing=isEditing -->
    <template v-else-if="isSolverStaff() && isEditing">
      <CommonDetailRow
        :label="$t('user.row.company.name')"
        required
        :is-editing="isEditing"
        :value="user.company"
      >
        <Sheet width="300">
          <AutoComplete
            v-model="localParam.company"
            outlined
            dense
            required
            :items="autoSolverCorporationItems()"
            :placeholder="$t('common.placeholder.autoComplete')"
            hide-details
            :loading="isLoading"
            item-text="name"
            item-value="id"
            max-length="255"
            :is-combobox="true"
            @change="setSolverCorporation"
            @input="$emit('update', 'company', $event)"
          />
        </Sheet>
      </CommonDetailRow>
    </template>

    <!-- 顧客または法人ソルバーではない 編集画面 -->
    <template v-else-if="!isCustomer() && !isSolverStaff()">
      <CommonDetailRow
        :label="$t('user.row.company.name')"
        required
        :is-editing="isEditing"
        :value="user.company"
      >
        <Sheet width="300">
          <TextField
            v-model="localParam.company"
            outlined
            dense
            required
            max-length="255"
            :placeholder="$t('user.row.company.placeholder')"
          />
        </Sheet>
      </CommonDetailRow>
    </template>

    <!-- 部署 -->
    <!-- 新規作成かつ、支援者または支援者責任者または事業者責任者の場合 -->
    <template
      v-if="
        !isEditing &&
        (user.role === 'supporter' ||
          user.role === 'supporter_mgr' ||
          user.role === 'business_mgr' ||
          isSupporter() ||
          isSupporterManager() ||
          isBusinessManager())
      "
    >
      <CommonDetailRow
        :label="$t('user.row.organizationName.name')"
        required
        :is-editing="isCreating"
        :value="toSupporterOrganizationsArray()"
      >
        <Sheet width="300">
          <Select
            v-if="isSupporter() || isBusinessManager()"
            v-model="localParam.supporterOrganizations"
            :items="getSupporterOrganizationsResponse.supporterOrganizations"
            dense
            outlined
            required
            item-text="name"
            item-value="id"
            :placeholder="$t('common.placeholder.select')"
          >
          </Select>
          <Select
            v-else-if="isSupporterManager()"
            v-model="localParam.supporterOrganizations"
            multiple
            size="5"
            :items="getSupporterOrganizationsResponse.supporterOrganizations"
            dense
            outlined
            item-text="name"
            item-value="id"
            :placeholder="$t('common.placeholder.selectMulti')"
          >
          </Select>
        </Sheet>
      </CommonDetailRow>
    </template>

    <!-- 編集かつ支援者または支援者責任者または事業者責任者の場合 -->
    <template
      v-else-if="
        isEditing &&
        (user.role === 'supporter' ||
          user.role === 'supporter_mgr' ||
          user.role === 'business_mgr' ||
          isSupporter() ||
          isSupporterManager() ||
          isBusinessManager())
      "
    >
      <CommonDetailRow
        :label="$t('user.row.organizationName.name')"
        required
        :is-editing="isEditing"
        :value="user.supporterOrganizations"
      >
        <Sheet width="300">
          <!-- 新規作成かつSupporterまたはBusinessManager -->
          <template
            v-if="
              isCreating &&
              (isSupporter() ||
                user.role === 'supporter' ||
                isBusinessManager() ||
                user.role === 'business_mgr')
            "
          >
            <Select
              v-model="localParam.supporterOrganizations"
              dense
              outlined
              required
              :items="getSupporterOrganizationsResponse.supporterOrganizations"
              item-text="name"
              item-value="id"
              :placeholder="$t('common.placeholder.select')"
            />
          </template>
          <template
            v-else-if="
              isEditing &&
              (isSupporter() ||
                user.role === 'supporter' ||
                isBusinessManager() ||
                user.role === 'business_mgr')
            "
          >
            <Select
              v-model="localParam.supporterOrganizations[0]"
              dense
              outlined
              required
              :items="getSupporterOrganizationsResponse.supporterOrganizations"
              item-text="name"
              item-value="id"
              :placeholder="$t('common.placeholder.select')"
            />
          </template>

          <!-- 新規作成かつSupporterMgr -->
          <template
            v-else-if="isSupporterManager() || user.role === 'supporter_mgr'"
          >
            <Select
              v-model="localParam.supporterOrganizations"
              multiple
              size="5"
              dense
              outlined
              required
              :items="getSupporterOrganizationsResponse.supporterOrganizations"
              item-text="name"
              item-value="id"
              :placeholder="$t('common.placeholder.selectMulti')"
            />
          </template>
        </Sheet>
      </CommonDetailRow>
    </template>

    <!-- その他の場合 かつ営業の場合 -->
    <template v-else-if="(isSales() || isSalesManager()) && isEditing">
      <CommonDetailRow
        :label="$t('user.row.organizationName.name')"
        :required="user.role === 'customer' || isCustomer() ? false : true"
        :is-editing="isEditing"
        :value="user.organizationName"
      >
        <Sheet width="300">
          <TextField
            v-model="localParam.organizationName"
            outlined
            dense
            required
            max-length="100"
            :placeholder="$t('user.row.organizationName.placeholder')"
          />
        </Sheet>
      </CommonDetailRow>
    </template>

    <!-- その他の場合 かつ顧客以外の場合 -->
    <template v-else-if="isEditing">
      <CommonDetailRow
        :label="$t('user.row.organizationName.name')"
        :required="user.role === 'customer' || isCustomer() ? false : true"
        :is-editing="isEditing"
        :value="user.organizationName"
      >
        <Sheet width="300">
          <TextField
            v-model="localParam.organizationName"
            :required="user.role === 'customer' || isCustomer() ? false : true"
            outlined
            dense
            max-length="100"
            :placeholder="$t('user.row.organizationName.placeholder')"
          />
        </Sheet>
      </CommonDetailRow>
    </template>

    <!-- その他の場合 新規作成 -->
    <template v-else>
      <CommonDetailRow
        :label="$t('user.row.organizationName.name')"
        required
        :is-editing="isCreating"
        :value="user.organizationName"
      >
        <Sheet width="300">
          <TextField
            v-model="localParam.organizationName"
            outlined
            dense
            required
            :placeholder="$t('user.row.organizationName.placeholder')"
          />
        </Sheet>
      </CommonDetailRow>
    </template>

    <!-- 役職（顧客のみ） -->
    <template v-if="user.role === 'customer' || isCustomer()">
      <CommonDetailRow
        :label="$t('user.row.job.name')"
        :is-editing="isEditing"
        :value="user.job"
      >
        <Sheet width="300">
          <TextField
            v-model="localParam.job"
            outlined
            dense
            max-length="128"
            :placeholder="$t('user.row.job.placeholder')"
          />
        </Sheet>
      </CommonDetailRow>
    </template>

    <!-- 工数調査（支援者または支援者責任者または事業者責任者のみ） -->
    <!-- 詳細画面 -->
    <template
      v-if="
        user.role === 'supporter' ||
        user.role === 'supporter_mgr' ||
        user.role === 'business_mgr' ||
        isSupporter() ||
        isSupporterManager() ||
        isBusinessManager()
      "
    >
      <CommonDetailRow
        :label="$t('user.row.isInputManHour.name')"
        required
        :is-editing="isEditing"
        :value="user.isInputManHour"
      >
        <RadioGroup
          v-model="localParam.isInputManHour"
          :labels="$t('user.row.isInputManHour.radio').labels"
          :values="$t('user.row.isInputManHour.radio').values"
          horizontal
          hide-details
        />

        <template #isNotEditing>
          {{
            user.isInputManHour
              ? $t('user.row.isInputManHour.radio').labels[0]
              : $t('user.row.isInputManHour.radio').labels[1]
          }}
        </template>
      </CommonDetailRow>
    </template>
  </v-form>
</template>

<script lang="ts">
import cloneDeep from 'lodash/cloneDeep'
import {
  TextField,
  RadioGroup,
  Select,
  Sheet,
  Required,
  AutoComplete,
} from '../../common/atoms/index'
import CommonUpdateRow from '../../common/molecules/CommonUpdateRow.vue'
import CommonDetailRow from '../../common/molecules/CommonDetailRow.vue'
import CommonDetailRows from '../../common/molecules/CommonDetailRows.vue'
import type { PropType } from '~/common/BaseComponent'

import { GetUserByIdResponse } from '~/models/User'

import { ISupporterOrganization, IProjectIds } from '~/types/User'

export class LocalUser {
  id?: string
  name?: string
  email?: string
  role?: string
  customerId?: string
  customerName?: string
  job?: string
  company?: string
  solverCorporationId?: string
  supporterOrganizations?: ISupporterOrganization[]
  organizationName?: string
  isInputManHour?: boolean
  projectIds?: IProjectIds[]
  agreed?: boolean
  lastLoginAt?: string
  disabled?: boolean
  createId?: string
  createUserName?: string
  createAt?: string
  updateId?: string
  updateUserName?: string
  updateAt?: string
  version?: number
}

export default CommonDetailRows.extend({
  components: {
    TextField,
    RadioGroup,
    Select,
    Sheet,
    Required,
    CommonUpdateRow,
    CommonDetailRow,
    AutoComplete,
  },
  props: {
    /** CreateUser APIの空ユーザーのレスポンス */
    user: {
      type: Object as PropType<GetUserByIdResponse>,
    },
    /** SuggestCustomers APIの顧客一覧レスポンス */
    suggestCustomers: {
      type: Array,
      required: false,
    },
    /** SuggestSolverCorporations APIの法人一覧レスポンス */
    suggestSolverCorporations: {
      type: Array,
      required: false,
    },
    /** getSupporterOrganizations支援者一覧のレスポンス */
    getSupporterOrganizationsResponse: {
      type: Object,
      required: false,
    },
    isLoading: {
      type: Boolean,
      required: true,
    },
  },
  data() {
    return {
      localParam: cloneDeep(this.user),
    }
  },
  created() {
    // 新規登録時のシステムロールの初期値
    if (this.isCreating) {
      this.localParam.role = ''
    }
  },
  watch: {
    /** 編集モードになると、ユーザー情報をlocalParamにコピーする */
    isEditing() {
      this.localParam = cloneDeep(this.user)
    },
    /** 詳細画面表示時にthis.user.roleをlocalParamに移す（表示判別に使用） */
    user() {
      this.localParam.role = this.user.role
    },
  },
  methods: {
    /**
     * 選択または入力された法人を登録
     * @param solverCorporationInfo 選択または入力された法人情報
     */
    setSolverCorporation(solverCorporationInfo: string | { name: ''; id: '' }) {
      if (solverCorporationInfo) {
        //既存法人の選択時（サジェスト選択）
        if (typeof solverCorporationInfo === 'object') {
          this.localParam.company = solverCorporationInfo.name
          this.localParam.solverCorporationId = solverCorporationInfo.id
          // 新規法人の入力時（サジェスト未選択）
          // solverCorporationInfoには法人名(string)のみ格納される
        } else {
          this.localParam.company = solverCorporationInfo
          this.localParam.solverCorporationId = ''
        }
      }
    },
    toSupporterOrganizationsArray() {
      const temp = []
      for (const key in this.user.supporterOrganizations) {
        const supporterOrganization: ISupporterOrganization =
          this.user.supporterOrganizations[key]
        const id: string = supporterOrganization.id
        const name: string | boolean = this.getSupporterOrganizationsName(id)
        if (name !== false) {
          temp.push(name)
        }
      }
      return temp.join(', ')
    },
    /** ユーザーの支援者組織名を取得 */
    getSupporterOrganizationsName(id: string): string | boolean {
      const supporterOrganizations =
        this.getSupporterOrganizationsResponse.supporterOrganizations
      for (const i in supporterOrganizations) {
        if (supporterOrganizations[i].id === id) {
          return supporterOrganizations[i].name
        }
      }
      return false
    },
    autoCompleteItems() {
      return this.suggestCustomers
    },
    autoSolverCorporationItems() {
      return this.suggestSolverCorporations
    },
    isCustomer() {
      return this.localParam.role === 'customer'
    },
    isSupporter() {
      return this.localParam.role === 'supporter'
    },
    isSupporterManager() {
      return this.localParam.role === 'supporter_mgr'
    },
    isSales() {
      return this.localParam.role === 'sales'
    },
    isSalesManager() {
      return this.localParam.role === 'sales_mgr'
    },
    isBusinessManager() {
      return this.localParam.role === 'business_mgr'
    },
    isSolverStaff() {
      return this.localParam.role === 'solver_staff'
    },

    /**
     * ロール名を日本語に変換
     * @param roleName ログインユーザーのロール
     */
    rolesToString(roleName: string) {
      const rolesLabels = this.$t('user.row.role.radio.labels') as string
      const rolesList = this.$t('user.row.role.radio.values') as string
      for (let i = 0; i < rolesList.length; i++) {
        if (rolesList[i] === roleName) {
          return rolesLabels[i]
        }
      }
    },
    /** システムロールに変更があるたびに、localParamのsupporterOrganizationsを初期化 */
    onChange() {
      //supporterOrganizationsの配列が変わることへの対応
      //メモリリークが発生するのでここでradioButtonが変わるたびに空にしている
      this.localParam.supporterOrganizations = []
    },
  },
})
</script>

<style lang="scss" scoped>
.o-user-detail-rows__select {
  padding: 0;
  margin: 0;
}
.o-user-detail-rows__select__item {
  list-style: none;
  display: inline-block;
  margin-right: 1em;
  &::after {
    content: ',';
  }
  &:last-child {
    &::after {
      display: none;
    }
  }
}
</style>
