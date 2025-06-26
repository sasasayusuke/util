<template>
  <v-form
    ref="form"
    v-model="formValid"
    :value="isValid"
    class="o-admin-detail-rows pt-4 px-8 pb-0"
    @input="$listeners['input']"
  >
    <CommonDetailRow
      :label="$t('admin.row.name.name')"
      required
      :is-editing="isEditing"
      :value="admin.name"
    >
      <!-- ユーザーネーム -->
      <v-row class="o-admin-detail-rows__unit">
        <v-col class="o-admin-detail-rows__data">
          <v-row v-if="isEditing" class="px-3 pt-3 pb-3">
            <Sheet width="300">
              <TextField
                v-model="localParam.name"
                outlined
                dense
                required
                max-length="120"
                :placeholder="$t('admin.row.name.placeholder')"
              />
            </Sheet>
          </v-row>
        </v-col>
      </v-row>
    </CommonDetailRow>
    <!--
      メールアドレス
      新規作成画面でのみ編集するため、isEditing=isCreatingとする
    -->
    <CommonDetailRow
      :label="$t('admin.row.email.name')"
      required
      :is-editing="isCreating"
      :value="admin.email"
    >
      <v-row class="o-admin-detail-rows__unit">
        <v-col class="o-admin-detail-rows__data">
          <v-row v-if="isCreating" class="px-3 pt-3 pb-3">
            <Sheet width="300">
              <TextField
                v-model="localParam.email"
                outlined
                dense
                email
                required
                max-length="256"
                :placeholder="$t('admin.row.email.placeholder')"
                @change="localParam.email = adjustEmail(localParam.email)"
              />
            </Sheet>
          </v-row>
        </v-col>
      </v-row>
    </CommonDetailRow>
    <!-- 所属会社 -->
    <CommonDetailRow
      :label="$t('admin.row.company.name')"
      required
      :is-editing="isEditing"
      :value="admin.company"
    >
      <v-row class="o-admin-detail-rows__unit">
        <v-col class="o-admin-detail-rows__data">
          <v-row v-if="isEditing || isCreating" class="px-3 pt-3 pb-3">
            <Sheet width="300">
              <TextField
                v-model="localParam.company"
                outlined
                dense
                required
                max-length="256"
                :placeholder="$t('admin.row.company.placeholder')"
              />
            </Sheet>
          </v-row>
        </v-col>
      </v-row>
    </CommonDetailRow>
    <!-- 部署 -->
    <!-- 支援者責任者新規作成 -->
    <template v-if="!isEditing && showsChoices">
      <CommonDetailRow
        :label="$t('admin.row.organizationName.name')"
        required
        :is-editing="isCreating"
        :value="organizationName"
      >
        <Sheet width="300">
          <Select
            v-model="localParam.supporterOrganizations"
            dense
            outlined
            :items="supporterOrganizations"
            item-text="name"
            item-value="id"
            multiple
            required
            :placeholder="$t('common.placeholder.selectMulti')"
          >
          </Select>
        </Sheet>
      </CommonDetailRow>
    </template>
    <!-- 支援者責任者編集 -->
    <template v-else-if="isEditing && showsChoices">
      <CommonDetailRow
        :label="$t('admin.row.organizationName.name')"
        required
        :is-editing="isEditing"
        :value="admin.supporterOrganizations"
      >
        <Sheet width="300">
          <!-- 支援者責任者新規作成 -->
          <template v-if="showsChoices">
            <Select
              v-model="localParam.supporterOrganizations"
              dense
              outlined
              :items="supporterOrganizations"
              item-text="name"
              item-value="id"
              multiple
              required
              :placeholder="$t('common.placeholder.selectMulti')"
            />
          </template>
        </Sheet>
      </CommonDetailRow>
    </template>

    <!-- その他の場合 -->
    <template v-else-if="isEditing">
      <CommonDetailRow
        :label="$t('admin.row.organizationName.name')"
        :is-editing="isEditing"
        required
        :value="admin.organizationName"
      >
        <Sheet width="300">
          <TextField
            v-model="localParam.organizationName"
            outlined
            dense
            required
            max-length="80"
            :placeholder="$t('admin.row.organizationName.placeholder')"
          />
        </Sheet>
      </CommonDetailRow>
    </template>

    <!-- 新規作成その他の場合  -->
    <template v-else>
      <CommonDetailRow
        :label="$t('admin.row.organizationName.name')"
        required
        :is-editing="isCreating"
        :value="admin.organizationName"
      >
        <Sheet width="300">
          <TextField
            v-model="localParam.organizationName"
            outlined
            dense
            required
            max-length="80"
            :placeholder="$t('admin.row.organizationName.placeholder')"
          />
        </Sheet>
      </CommonDetailRow>
    </template>

    <!-- 役職 -->
    <CommonDetailRow
      :label="$t('admin.row.job.name')"
      :is-editing="isEditing"
      :value="admin.job"
    >
      <v-row class="o-admin-detail-rows__unit">
        <v-col class="o-admin-detail-rows__data">
          <v-row v-if="isEditing || isCreating" class="px-3 pt-3 pb-3">
            <Sheet width="300">
              <TextField
                v-model="localParam.job"
                outlined
                dense
                max-length="128"
                :placeholder="$t('admin.row.job.placeholder')"
              />
            </Sheet>
          </v-row>
        </v-col>
      </v-row>
    </CommonDetailRow>
    <!-- システムロール -->
    <CommonDetailRow
      :label="$t('admin.row.roles.name')"
      required
      :is-editing="isEditing"
      :value="rolesToString()"
    >
      <v-row class="o-admin-detail-rows__unit">
        <v-col class="o-admin-detail-rows__data">
          <v-row v-if="isEditing || isCreating" class="px-3 pt-3 pb-3">
            <Checkbox
              v-for="(role, roleIndex) in $t('admin.row.roles.list')"
              :key="roleIndex"
              v-model="localParam.roles"
              class="mr-7 mt-2 mb-1 pt-0"
              :label="role.name"
              :value="role.value"
              hide-details
              required
            />
          </v-row>
        </v-col>
      </v-row>
    </CommonDetailRow>
  </v-form>
</template>

<script lang="ts">
import {
  TextField,
  Checkbox,
  Select,
  Sheet,
  Required,
  ToolTips,
} from '../../common/atoms/index'
import CommonDetailRow from '../../common/molecules/CommonDetailRow.vue'
import CommonDetailRows from '../../common/molecules/CommonDetailRows.vue'
import {
  ISupporterOrganization,
  IGetSupporterOrganizations,
} from '@/types/Admin'
import {
  GetAdminByIdResponse,
  GetAdminByIdResponse as LocalAdmin,
} from '@/models/Admin'
import type { PropType } from '~/common/BaseComponent'

export { LocalAdmin }

export default CommonDetailRows.extend({
  name: 'AdminDetailRows',
  components: {
    TextField,
    Checkbox,
    Select,
    Sheet,
    Required,
    ToolTips,
    CommonDetailRow,
  },
  props: {
    /**
     * Back Officeにログイン可能な選択中の管理ユーザー情報
     */
    admin: {
      type: Object as PropType<GetAdminByIdResponse>,
      required: true,
    },
  },
  data() {
    return {
      /**
       * 部署を複数選択可能
       */
      showsChoices: false,
      supporterOrganizations: [] as ISupporterOrganization[],
      formValid: false,
      localParam: Object.assign(new LocalAdmin(), this.admin),
      selectRequired: false,
    }
  },
  watch: {
    /**
     * 入力中の管理者ユーザーのロールを検知し、支援者責任者の場合は部署が複数選択可能になる
     */
    localParam: {
      deep: true,
      handler(newValue: any) {
        if (newValue.roles && newValue.roles.includes('supporter_mgr')) {
          this.showsChoices = true
        } else {
          this.showsChoices = false
        }
      },
    },
    /**
     * 表示されている管理者ユーザーが支援者責任者の場合は部署が複数選択可能になる。createdではadminの最新情報を取得できないため、watchを利用。
     */
    admin: {
      deep: true,
      handler(newValue: any) {
        if (newValue.roles.includes('supporter_mgr')) {
          this.showsChoices = true
        } else {
          this.showsChoices = false
        }
      },
    },
  },
  mounted() {
    this.getSupporterOrganizations()

    if (this.admin.roles.includes('supporter_mgr')) {
      this.showsChoices = true
    }
  },
  methods: {
    /**
     * 必須項目が入力されているかチェック
     * @params el 管理者ユーザーの各項目
     */
    validationCheck(el: string[]) {
      if (!el.length) {
        this.selectRequired = true
      } else {
        this.selectRequired = false
      }
    },
    /**
     * getSupporterOrganizationsAPIを叩いて、支援者組織の一覧を取得
     */
    async getSupporterOrganizations() {
      await this.$api
        .get<IGetSupporterOrganizations>(`/masters/supporter-organizations`)
        .then((res) => {
          this.$logger.info(res.data)
          this.supporterOrganizations = res.data.supporterOrganizations
        })
        .catch((error) => {
          if (error.response && error.response.status === 404) {
            this.$router.push('/404')
          } else {
            this.apiErrorHandle(error)
          }
        })
    },
    /**
     * 管理者ユーザーが選択しているロールを日本語に変換
     */
    rolesToString() {
      const rolesList = this.$t('admin.row.roles.list')
      if (!this.admin.roles) return
      let rolesString = ''
      this.admin.roles.forEach((elm: any) => {
        // @ts-ignore
        const roleConvertJa = rolesList.filter(
          (item: any) => item.value === elm
        )[0].name

        rolesString = rolesString
          ? `${rolesString}/${roleConvertJa}`
          : roleConvertJa
      })
      return rolesString
    },
    /**
     * 選択中のユーザー情報をリセット
     */
    resetLocalParam() {
      this.localParam = Object.assign(new LocalAdmin(), this.admin)
    },
  },
  computed: {
    /**
     * 管理者ユーザーの部署を表示。ロールによって表示部署が異なる。
     * @returns 部署名 もしくは 複数部署名を","で結合したもの
     */
    organizationName(): string {
      if (this.admin.supporterOrganizations) {
        return this.admin.supporterOrganizations
          .map((elm) => {
            return elm.name
          })
          .join(', ')
      } else {
        // 支援者責任者以外の場合にはorganizationNameを表示
        return this.admin.organizationName
      }
    },
  },
})
</script>

<style lang="scss" scoped>
.o-admin-detail-rows__title {
  font-size: 0.875rem;
  font-weight: bold;
  align-items: center;
  padding-top: 18px;
}
.o-admin-detail-rows__data {
  align-items: center;
}
.o-admin-detail-rows__select {
  padding: 0;
  margin: 0;
}
.o-admin-detail-rows__select__item {
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
