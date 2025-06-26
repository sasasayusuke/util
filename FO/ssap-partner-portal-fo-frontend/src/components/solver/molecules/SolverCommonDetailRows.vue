<template>
  <v-form
    ref="form"
    v-model="isValid"
    class="o-detail-rows o-common-detail-rows__unit"
    @submit.prevent
  >
    <!-- 応募案件 -->
    <CommonDetailRow
      :label="$t('solver.row.applicationProject.name')"
      :is-editing="isEditing"
      :component="true"
      :tall="true"
      required
    >
      <v-flex v-if="isEditing">
        <v-flex class="d-flex align-center">
          <!-- 案件コード -->
          <span style="margin-right: 10px; display: flex; align-items: center">
            {{ $t('solver.row.applicationProject.projectCode') }}
          </span>
          <Sheet width="455">
            <TextField
              v-model="projectCode"
              :additional-rules="additionalRules"
              :aria-label="$t('solver.row.applicationProject.projectId')"
              style-set="outlined"
              required
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
          <span style="margin-right: 10px; display: flex; align-items: center">
            {{ $t('solver.row.applicationProject.projectId') }}
          </span>
          <Sheet width="160">
            {{ projectId }}
          </Sheet>
          <!-- 案件名 -->
          <span
            style="
              margin-right: 10px;
              margin-left: 25px;
              display: flex;
              align-items: center;
            "
          >
            {{ $t('solver.row.applicationProject.projectName') }}
          </span>
          <Sheet width="500">
            {{ projectName }}
          </Sheet>
        </v-flex>
      </v-flex>
      <v-flex v-if="!isEditing" class="d-flex align-center">
        <Sheet style="max-width: 200px; overflow-wrap: break-word">
          {{ projectId }}
        </Sheet>
        <Sheet
          style="max-width: 590px; overflow-wrap: break-word; margin-left: 36px"
        >
          {{ projectName }}
        </Sheet>
      </v-flex>
    </CommonDetailRow>
    <!-- 法人ソルバー -->
    <CommonDetailRow
      v-if="isDisplaySolverCorporation"
      :label="$t('solver.row.solverCorporation.name')"
      :is-editing="isEditing"
      :value="solverCorporationName"
      :tall="true"
      required
    >
      <Sheet width="520">
        <Select
          v-model="solverCorporationValue"
          :items="solverCorporationItems"
          item-text="label"
          item-value="value"
          :placeholder="$t('common.placeholder.select')"
          style-set="outlined bgWhite bgTransparent"
          :bg-transparent="true"
          required
          @change="findSolverCorporationName"
        />
      </Sheet>
    </CommonDetailRow>
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
import { PropType } from '~/common/BaseVueClass'
import { IGetSolverCorporationsResponse } from '~/types/SolverCorporation'
import { meStore } from '~/store'
import { ENUM_USER_ROLE } from '~/types/User'

export default CommonDetailRows.extend({
  name: 'SolverCommonDetailRows',
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
  },
  props: {
    solverCorporations: {
      type: Object as PropType<IGetSolverCorporationsResponse>,
      required: true,
    },
    isEditing: {
      type: Boolean,
      default: true,
    },
  },
  data() {
    return {
      projectCode: '',
      projectId: '',
      projectName: '',
      solverCorporationName: '',
      solverCorporationValue: '',
      additionalRules: [] as Function[],
    }
  },
  mounted() {
    if (this.$route.query.id) {
      this.projectCode = encodeURIComponent(this.$route.query.id as string)
      this.checkValidateProjectCode()
    }
  },
  computed: {
    // 法人ソルバープルダウンアイテム
    solverCorporationItems() {
      const solverCorporations = this
        .solverCorporations as IGetSolverCorporationsResponse
      return solverCorporations.solverCorporations.map((solverCorporation) => {
        return { label: solverCorporation.name, value: solverCorporation.id }
      })
    },
    // 法人ソルバー表示判定
    isDisplaySolverCorporation() {
      // アライアンス担当で公式サイトから遷移してきた場合（クエリパラメータが設定されている）
      return this.$route.query.id && meStore.role === ENUM_USER_ROLE.APT
    },
  },
  methods: {
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
        this.projectId = splittedProjectCode[0]
        this.projectName = splittedProjectCode[1]
        this.$emit('click:displayProjectName', {
          projectId: this.projectId,
          projectName: this.projectName,
        })
      } else {
        this.projectId = ''
        this.projectName = ''
        this.additionalRules.push(rule)
      }

      this.validationCheck()
    },

    // テキストボックスのバリデーションをリセットする処理
    clearAdditionalRules() {
      this.additionalRules = []
    },

    // リセットボタン押下時の処理
    resetProjectCode() {
      this.projectCode = ''
      this.projectId = ''
      this.projectName = ''
      this.additionalRules = []
      this.validationCheck()
    },

    // 企業名を抽出する処理
    findSolverCorporationName() {
      const solverCorporations = this
        .solverCorporations as IGetSolverCorporationsResponse
      const solverCorporation = solverCorporations.solverCorporations.find(
        (solverCorporation) =>
          solverCorporation.id === this.solverCorporationValue
      )
      this.solverCorporationName = solverCorporation
        ? solverCorporation.name
        : ''
      this.$emit('change:solverCorporation', this.solverCorporationValue)
    },

    // Base64URLをデコードする処理
    decodeBase64Url(base64UrlString: string) {
      const base64String = base64UrlString.replace(/-/g, '+').replace(/_/g, '/')
      return Buffer.from(base64String, 'base64').toString('utf-8')
    },

    // 全ての項目を入力しているかチェック
    validationCheck() {
      let isValidResult = false
      if (this.projectId && this.projectName && this.isValid) {
        isValidResult = true
      } else {
        isValidResult = false
      }
      this.$emit('change:isValid', isValidResult)
    },
  },
})
</script>

<style lang="scss" scoped>
.o-common-detail-rows__unit {
  padding-bottom: 16px;
  &.is-noborder {
    border-bottom: 0;
  }
}
</style>
