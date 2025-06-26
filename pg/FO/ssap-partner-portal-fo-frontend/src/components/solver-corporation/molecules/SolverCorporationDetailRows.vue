<template>
  <v-form
    ref="form"
    v-model="isValid"
    class="o-detail-rows"
    @input="$listeners['input']"
  >
    <!-- 最終更新者 -->
    <template v-if="!isCreating">
      <LastUpdate
        :user="solverCorporation.updateUserName"
        :date="solverCorporation.updateAt"
        :show-term="false"
      />
    </template>
    <SolverCorporationDetailSection
      :title="$t('solver-corporation.pages.detail.section.fundamental')"
    >
      <!-- 企業名 -->
      <CommonDetailRow
        :label="$t('solver-corporation.row.solverCorporationName.name')"
        :is-editing="isEditing"
        :value="solverCorporation.name"
        required
      >
        <Sheet style="width: 100%">
          <TextField
            v-model="localParam.name"
            role="textbox"
            :max-length="40 * 2"
            :aria-label="
              $t('solver-corporation.row.solverCorporationName.name')
            "
            style-set="outlined"
            required
            :half-width-char-as-half="true"
            @input="onInputForm"
          />
        </Sheet>
      </CommonDetailRow>
      <!-- 企業名略称 -->
      <CommonDetailRow
        :label="$t('solver-corporation.row.companyAbbreviation.name')"
        :is-editing="isEditing"
        :value="solverCorporation.companyAbbreviation"
        required
      >
        <Sheet style="width: 100%">
          <TextField
            v-model="localParam.companyAbbreviation"
            role="textbox"
            :max-length="14"
            :aria-label="$t('solver-corporation.row.companyAbbreviation.name')"
            style-set="outlined"
            required
            @input="onInputForm"
          />
        </Sheet>
      </CommonDetailRow>
      <!-- 業種 -->
      <CommonDetailRow
        :label="$t('solver-corporation.row.industry.name')"
        :is-editing="isEditing"
        :value="solverCorporation.industry"
        required
      >
        <Sheet style="width: 100%">
          <TextField
            v-model="localParam.industry"
            role="textbox"
            :max-length="100"
            :aria-label="$t('solver-corporation.row.industry.name')"
            style-set="outlined"
            required
            @input="onInputForm"
          />
        </Sheet>
      </CommonDetailRow>
      <!-- 住所 -->
      <CommonDetailRow
        :label="$t('solver-corporation.row.address.name')"
        :tall="true"
        :is-editing="isEditing"
        :value="solverCorporation.address"
        required
      >
        <template #isNotEditing>
          <table class="o-common-detail-rows__table">
            <tr>
              <th>
                {{ $t('solver-corporation.row.address.postalCode') }}
              </th>
              <td>
                {{ solverCorporation.address.postalCode }}
              </td>
            </tr>
            <tr>
              <th>
                {{ $t('solver-corporation.row.address.state') }}
              </th>
              <td>
                {{ solverCorporation.address.state }}
              </td>
            </tr>
            <tr>
              <th>
                {{ $t('solver-corporation.row.address.city') }}
              </th>
              <td>
                {{ solverCorporation.address.city }}
              </td>
            </tr>
            <tr>
              <th>
                {{ $t('solver-corporation.row.address.street') }}
              </th>
              <td>
                {{ solverCorporation.address.street }}
              </td>
            </tr>
            <tr>
              <th>
                {{ $t('solver-corporation.row.address.building') }}
              </th>
              <td>
                {{ solverCorporation.address.building }}
              </td>
            </tr>
          </table>
        </template>
        <table class="o-common-detail-rows__table">
          <!-- 郵便番号 -->
          <tr>
            <th>
              {{ $t('solver-corporation.row.address.postalCode') }}
            </th>
            <td>
              <Sheet width="150">
                <TextField
                  v-model="localParam.address.postalCode"
                  role="textbox"
                  :number="true"
                  :max-length="7"
                  :postal-code="true"
                  :aria-label="$t('solver-corporation.row.address.postalCode')"
                  style-set="outlined"
                  required
                  @input="onInputForm"
                />
              </Sheet>
            </td>
          </tr>
          <!-- 都道府県 -->
          <tr>
            <th>
              {{ $t('solver-corporation.row.address.state') }}
            </th>
            <td>
              <Sheet width="300">
                <Select
                  v-model="localParam.address.state"
                  :items="stateItems"
                  item-text="label"
                  item-value="value"
                  :placeholder="$t('common.placeholder.select')"
                  style-set="outlined"
                  :bg-transparent="true"
                  required
                  @input="onInputForm"
                />
              </Sheet>
            </td>
          </tr>
          <!-- 市区郡 -->
          <tr>
            <th>
              {{ $t('solver-corporation.row.address.city') }}
            </th>
            <td>
              <Sheet style="width: 100%">
                <TextField
                  v-model="localParam.address.city"
                  role="textbox"
                  :max-length="20"
                  :aria-label="$t('solver-corporation.row.address.city')"
                  style-set="outlined"
                  required
                  @input="onInputForm"
                />
              </Sheet>
            </td>
          </tr>
          <!-- 町名・番地 -->
          <tr>
            <th>
              {{ $t('solver-corporation.row.address.street') }}
            </th>
            <td>
              <Sheet style="width: 100%">
                <TextField
                  v-model="localParam.address.street"
                  role="textbox"
                  :max-length="20"
                  :aria-label="$t('solver-corporation.row.address.street')"
                  style-set="outlined"
                  required
                  @input="onInputForm"
                />
              </Sheet>
            </td>
          </tr>
          <!-- 建物名 -->
          <tr>
            <th>
              {{ $t('solver-corporation.row.address.building') }}
            </th>
            <td>
              <Sheet style="width: 100%">
                <TextField
                  v-model="localParam.address.building"
                  role="textbox"
                  :max-length="40"
                  :aria-label="$t('solver-corporation.row.address.building')"
                  style-set="outlined"
                  @input="onInputForm"
                />
              </Sheet>
            </td>
          </tr>
        </table>
      </CommonDetailRow>
      <!-- 事業内容 -->
      <CommonDetailRow
        :label="$t('solver-corporation.row.businessContent.name')"
        :tall="true"
        :is-editing="isEditing"
        :value="solverCorporation.businessContent"
        :escape-value="false"
        required
      >
        <Textarea
          v-model="localParam.businessContent"
          :max-length="50 * 2"
          style-set="outlined"
          required
          :half-width-char-as-half="true"
          @input="onInputForm"
        />
      </CommonDetailRow>
      <!-- 経営陣 -->
      <CommonDetailRow
        :label="$t('solver-corporation.row.managementTeam.name')"
        :tall="true"
        :is-editing="isEditing"
        :value="solverCorporation.managementTeam"
        :escape-value="false"
        required
      >
        <Textarea
          v-model="localParam.managementTeam"
          :max-length="50 * 2"
          style-set="outlined"
          required
          :half-width-char-as-half="true"
          @input="onInputForm"
        />
      </CommonDetailRow>
      <!-- 資本金（変更） -->
      <CommonDetailRow
        v-if="isEditing"
        :label="$t('solver-corporation.row.capital.name')"
        :is-editing="isEditing"
        :component="true"
        :noborder="true"
        required
      >
        <Sheet width="160">
          <TextField
            v-model="localParam.capital.value"
            :positive-number="true"
            role="textbox"
            type="number"
            :max-digits="16"
            :aria-label="$t('solver-corporation.row.capital.value')"
            style-set="outlined"
            required
            @input="onInputForm"
            @keydown="blockInput"
          />
        </Sheet>
        <span style="margin-left: 10px; display: flex; align-items: center">
          {{ $t('solver-corporation.row.capital.value') }}
        </span>
      </CommonDetailRow>
      <!-- 資本金メモ（変更） -->
      <CommonDetailRow
        v-if="isEditing"
        :label="$t('solver-corporation.row.capital.memo')"
        :is-editing="isEditing"
        :component="true"
      >
        <Sheet style="width: calc(100% - 294px)">
          <TextField
            v-model="localParam.capital.memo"
            :placeholder="$t('solver-corporation.row.capital.placeholder')"
            role="textbox"
            :max-length="18 * 2"
            :aria-label="$t('solver-corporation.row.capital.memo')"
            style-set="outlined"
            :half-width-char-as-half="true"
            @input="onInputForm"
          />
        </Sheet>
      </CommonDetailRow>
      <!-- 資本金（詳細） -->
      <CommonDetailRow
        v-if="!isEditing"
        :label="$t('solver-corporation.row.capital.name')"
        :is-editing="!isEditing"
        :component="true"
        :noborder="true"
      >
        <Sheet>
          {{ solverCorporation.capital.value }}
        </Sheet>
        <span style="margin-left: 10px; display: flex; align-items: center">
          {{ $t('solver-corporation.row.capital.value') }}
        </span>
      </CommonDetailRow>
      <!-- 資本金メモ（詳細） -->
      <CommonDetailRow
        v-if="!isEditing"
        :label="$t('solver-corporation.row.capital.memo')"
        :is-editing="!isEditing"
        :component="true"
      >
        <Sheet>
          {{ solverCorporation.capital.memo }}
        </Sheet>
      </CommonDetailRow>
      <!-- 設立 -->
      <CommonDetailRow
        :label="$t('solver-corporation.row.established.name')"
        :is-editing="isEditing"
        :value="solverCorporation.established"
        required
      >
        <SolverDateSelect
          v-model="localParam.established"
          :placeholder="$t('solver-corporation.row.established.placeholder')"
          :date="
            localParam.established
              ? localParam.established.replaceAll('/', '-')
              : ''
          "
          :min-date="calculatedMinDate"
          :max-date="calculatedMaxDate"
          required
          :is-read-only="false"
          @input="onInputForm"
        />
      </CommonDetailRow>
      <!-- 上場取引所 -->
      <CommonDetailRow
        :label="$t('solver-corporation.row.listingExchange.name')"
        :tall="true"
        :is-editing="isEditing"
        :value="solverCorporation.listingExchange"
        :escape-value="false"
        required
      >
        <Textarea
          v-model="localParam.listingExchange"
          :max-length="50 * 2"
          style-set="outlined"
          required
          :half-width-char-as-half="true"
          @input="onInputForm"
        />
      </CommonDetailRow>
      <!-- 従業員数（変更） -->
      <CommonDetailRow
        v-if="isEditing"
        :label="$t('solver-corporation.row.employee.name')"
        :is-editing="isEditing"
        :component="true"
        :noborder="true"
        required
      >
        <Sheet width="160">
          <TextField
            v-model="localParam.employee.value"
            :positive-number="true"
            role="textbox"
            type="number"
            :max-digits="16"
            :aria-label="$t('solver-corporation.row.employee.value')"
            style-set="outlined"
            required
            @input="onInputForm"
            @keydown="blockInput"
          />
        </Sheet>
        <span style="margin-left: 10px; display: flex; align-items: center">
          {{ $t('solver-corporation.row.employee.value') }}
        </span>
      </CommonDetailRow>
      <!-- 従業員数メモ（変更） -->
      <CommonDetailRow
        v-if="isEditing"
        :label="$t('solver-corporation.row.employee.memo')"
        :is-editing="isEditing"
        :component="true"
      >
        <Sheet style="width: calc(100% - 294px)">
          <TextField
            v-model="localParam.employee.memo"
            :placeholder="$t('solver-corporation.row.employee.placeholder')"
            role="textbox"
            :max-length="20 * 2"
            :aria-label="$t('solver-corporation.row.employee.memo')"
            style-set="outlined"
            :half-width-char-as-half="true"
            @input="onInputForm"
          />
        </Sheet>
      </CommonDetailRow>
      <!-- 従業員数（詳細） -->
      <CommonDetailRow
        v-if="!isEditing"
        :label="$t('solver-corporation.row.employee.name')"
        :is-editing="!isEditing"
        :component="true"
        :noborder="true"
      >
        <Sheet>
          {{ solverCorporation.employee.value }}
        </Sheet>
        <span style="margin-left: 10px; display: flex; align-items: center">
          {{ $t('solver-corporation.row.employee.value') }}
        </span>
      </CommonDetailRow>
      <!-- 従業員数メモ（詳細） -->
      <CommonDetailRow
        v-if="!isEditing"
        :label="$t('solver-corporation.row.employee.memo')"
        :is-editing="!isEditing"
        :component="true"
      >
        <Sheet>
          {{ solverCorporation.employee.memo }}
        </Sheet>
      </CommonDetailRow>
      <!-- 売上（変更） -->
      <CommonDetailRow
        v-if="isEditing"
        :label="$t('solver-corporation.row.earnings.name')"
        :is-editing="isEditing"
        :component="true"
        :noborder="true"
        required
      >
        <Sheet width="160">
          <TextField
            v-model="localParam.earnings.value"
            :positive-number="true"
            role="textbox"
            type="number"
            :max-digits="16"
            :aria-label="$t('solver-corporation.row.earnings.name')"
            style-set="outlined"
            required
            @input="onInputForm"
            @keydown="blockInput"
          />
        </Sheet>
        <span style="margin-left: 10px; display: flex; align-items: center">
          {{ $t('solver-corporation.row.earnings.value') }}
        </span>
      </CommonDetailRow>
      <!-- 売上メモ（変更） -->
      <CommonDetailRow
        v-if="isEditing"
        :label="$t('solver-corporation.row.earnings.memo')"
        :is-editing="isEditing"
        :component="true"
      >
        <Sheet style="width: calc(100% - 294px)">
          <TextField
            v-model="localParam.earnings.memo"
            :placeholder="$t('solver-corporation.row.earnings.placeholder')"
            role="textbox"
            :max-length="19 * 2"
            :aria-label="$t('solver.row.pricePerPersonMonth.name')"
            style-set="outlined"
            :half-width-char-as-half="true"
            @input="onInputForm"
          />
        </Sheet>
      </CommonDetailRow>
      <!-- 売上（詳細） -->
      <CommonDetailRow
        v-if="!isEditing"
        :label="$t('solver-corporation.row.earnings.name')"
        :is-editing="!isEditing"
        :component="true"
        :noborder="true"
      >
        <Sheet>
          {{ solverCorporation.earnings.value }}
        </Sheet>
        <span style="margin-left: 10px; display: flex; align-items: center">
          {{ $t('solver-corporation.row.earnings.value') }}
        </span>
      </CommonDetailRow>
      <!-- 売上メモ（詳細） -->
      <CommonDetailRow
        v-if="!isEditing"
        :label="$t('solver-corporation.row.earnings.memo')"
        :is-editing="!isEditing"
        :component="true"
      >
        <Sheet>
          {{ solverCorporation.earnings.memo }}
        </Sheet>
      </CommonDetailRow>
      <!-- ミッション -->
      <CommonDetailRow
        :label="$t('solver-corporation.row.mission.name')"
        :tall="true"
        :is-editing="isEditing"
        :value="solverCorporation.mission"
        :escape-value="false"
        required
      >
        <Textarea
          v-model="localParam.mission"
          :max-length="255"
          style-set="outlined"
          required
          @input="onInputForm"
        />
      </CommonDetailRow>
      <!-- ビジョン -->
      <CommonDetailRow
        :label="$t('solver-corporation.row.vision.name')"
        :tall="true"
        :is-editing="isEditing"
        :value="solverCorporation.vision"
        :escape-value="false"
        required
      >
        <Textarea
          v-model="localParam.vision"
          :max-length="255"
          style-set="outlined"
          required
          @input="onInputForm"
        />
      </CommonDetailRow>
      <!-- 課題マップ50 -->
      <CommonDetailRow
        :label="$t('solver-corporation.row.issueMap50.name')"
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
            @input="onInputForm"
          />
        </Sheet>
      </CommonDetailRow>
      <!-- 法人ソルバー画像 -->
      <CommonDetailRow
        :label="$t('solver-corporation.row.corporatePhoto.name')"
        :notes="`${$t('solver-corporation.row.corporatePhoto.notes1')}\n${$t(
          'solver-corporation.row.corporatePhoto.notes2'
        )}`"
        :is-editing="isEditing"
        :component="true"
        :tall="true"
      >
        <SolverCorporationImage
          :solver-corporation-image="localParam.corporatePhoto"
          :is-editing="isEditing"
          :is-new="false"
          @change="onInputImage"
        />
      </CommonDetailRow>
      <!-- 会社案内資料 -->
      <CommonDetailRow
        :label="$t('solver-corporation.row.corporateInfoDocument.name')"
        :notes="`${$t(
          'solver-corporation.row.corporateInfoDocument.notes1'
        )}\n${$t('solver-corporation.row.corporateInfoDocument.notes2')}`"
        :is-editing="isEditing"
        :component="true"
        :tall="true"
      >
        <Sheet style="width: 100%">
          <SolverFile
            :links="localParam.corporateInfoDocument"
            :is-editing="isEditing"
            :is-solver-corporation="true"
            :solver-corporation-id="solverCorporationIdFile"
            @change="onInputFile"
          />
        </Sheet>
      </CommonDetailRow>
    </SolverCorporationDetailSection>
    <SolverCorporationDetailSection
      :title="$t('solver-corporation.pages.detail.section.charge')"
    >
      <!-- 主担当 -->
      <v-row class="row-subtitle"
        >{{ $t('solver-corporation.row.mainCharge.mainTitle') }}
      </v-row>
      <!-- ふりがな -->
      <CommonDetailRow
        :label="$t('solver-corporation.row.mainCharge.kana')"
        :is-editing="isEditing"
        :value="solverCorporation.mainCharge.kana"
        required
      >
        <Sheet style="width: 100%">
          <TextField
            v-model="localParam.mainCharge.kana"
            role="textbox"
            :max-length="100"
            :aria-label="$t('solver-corporation.row.mainCharge.kana')"
            style-set="outlined"
            required
            @input="onInputForm"
          />
        </Sheet>
      </CommonDetailRow>
      <!-- 氏名 -->
      <CommonDetailRow
        :label="$t('solver-corporation.row.mainCharge.name')"
        :is-editing="isEditing"
        :value="solverCorporation.mainCharge.name"
        required
      >
        <Sheet style="width: 100%">
          <TextField
            v-model="localParam.mainCharge.name"
            role="textbox"
            :max-length="100"
            :aria-label="$t('solver-corporation.row.mainCharge.name')"
            style-set="outlined"
            required
            @input="onInputForm"
          />
        </Sheet>
      </CommonDetailRow>
      <!-- 部署 -->
      <CommonDetailRow
        :label="$t('solver-corporation.row.mainCharge.department')"
        :is-editing="isEditing"
        :value="solverCorporation.mainCharge.department"
        required
      >
        <Sheet style="width: 100%">
          <TextField
            v-model="localParam.mainCharge.department"
            role="textbox"
            :max-length="100"
            :aria-label="$t('solver-corporation.row.mainCharge.department')"
            style-set="outlined"
            required
            @input="onInputForm"
          />
        </Sheet>
      </CommonDetailRow>
      <!-- 役職 -->
      <CommonDetailRow
        :label="$t('solver-corporation.row.mainCharge.title')"
        :is-editing="isEditing"
        :value="solverCorporation.mainCharge.title"
        required
      >
        <Sheet style="width: 100%">
          <TextField
            v-model="localParam.mainCharge.title"
            role="textbox"
            :max-length="100"
            :aria-label="$t('solver-corporation.row.mainCharge.title')"
            style-set="outlined"
            required
            @input="onInputForm"
          />
        </Sheet>
      </CommonDetailRow>
      <!-- 電話番号（ハイフンあり） -->
      <CommonDetailRow
        :label="$t('solver-corporation.row.mainCharge.phone')"
        :is-editing="isEditing"
        :value="solverCorporation.mainCharge.phone"
        required
      >
        <Sheet style="width: 100%">
          <TextField
            v-model="localParam.mainCharge.phone"
            role="textbox"
            :phone-number="true"
            :max-length="40"
            :aria-label="$t('solver-corporation.row.mainCharge.phone')"
            style-set="outlined"
            required
            @input="onInputForm"
          />
        </Sheet>
      </CommonDetailRow>
      <!-- メールアドレス -->
      <CommonDetailRow
        :label="$t('solver-corporation.row.mainCharge.email')"
        :is-editing="isEditing"
        :value="solverCorporation.mainCharge.email"
        required
      >
        <Sheet style="width: 100%">
          <TextField
            v-model="localParam.mainCharge.email"
            role="textbox"
            :email="true"
            :max-length="80"
            :aria-label="$t('solver-corporation.row.mainCharge.email')"
            style-set="outlined"
            required
            @input="onInputForm"
          />
        </Sheet>
      </CommonDetailRow>
      <!-- 副担当 -->
      <v-row class="row-subtitle">{{
        $t('solver-corporation.row.deputyCharge.mainTitle')
      }}</v-row>
      <!-- ふりがな -->
      <CommonDetailRow
        :label="$t('solver-corporation.row.deputyCharge.kana')"
        :is-editing="isEditing"
        :value="solverCorporation.deputyCharge.kana"
      >
        <Sheet style="width: 100%">
          <TextField
            v-model="localParam.deputyCharge.kana"
            role="textbox"
            :max-length="100"
            :aria-label="$t('solver-corporation.row.deputyCharge.kana')"
            style-set="outlined"
            @input="onInputForm"
          />
        </Sheet>
      </CommonDetailRow>
      <!-- 氏名 -->
      <CommonDetailRow
        :label="$t('solver-corporation.row.deputyCharge.name')"
        :is-editing="isEditing"
        :value="solverCorporation.deputyCharge.name"
      >
        <Sheet style="width: 100%">
          <TextField
            v-model="localParam.deputyCharge.name"
            role="textbox"
            :max-length="100"
            :aria-label="$t('solver-corporation.row.deputyCharge.name')"
            style-set="outlined"
            @input="onInputForm"
          />
        </Sheet>
      </CommonDetailRow>
      <!-- 部署 -->
      <CommonDetailRow
        :label="$t('solver-corporation.row.deputyCharge.department')"
        :is-editing="isEditing"
        :value="solverCorporation.deputyCharge.department"
      >
        <Sheet style="width: 100%">
          <TextField
            v-model="localParam.deputyCharge.department"
            role="textbox"
            :max-length="100"
            :aria-label="$t('solver-corporation.row.deputyCharge.department')"
            style-set="outlined"
            @input="onInputForm"
          />
        </Sheet>
      </CommonDetailRow>
      <!-- 役職 -->
      <CommonDetailRow
        :label="$t('solver-corporation.row.deputyCharge.title')"
        :is-editing="isEditing"
        :value="solverCorporation.deputyCharge.title"
      >
        <Sheet style="width: 100%">
          <TextField
            v-model="localParam.deputyCharge.title"
            role="textbox"
            :max-length="100"
            :aria-label="$t('solver-corporation.row.deputyCharge.title')"
            style-set="outlined"
            @input="onInputForm"
          />
        </Sheet>
      </CommonDetailRow>
      <!-- 電話番号（ハイフンあり） -->
      <CommonDetailRow
        :label="$t('solver-corporation.row.deputyCharge.phone')"
        :is-editing="isEditing"
        :value="solverCorporation.deputyCharge.phone"
      >
        <Sheet style="width: 100%">
          <TextField
            v-model="localParam.deputyCharge.phone"
            role="textbox"
            :phone-number="true"
            :max-length="40"
            :aria-label="$t('solver-corporation.row.deputyCharge.phone')"
            style-set="outlined"
            @input="onInputForm"
          />
        </Sheet>
      </CommonDetailRow>
      <!-- メールアドレス -->
      <CommonDetailRow
        :label="$t('solver-corporation.row.deputyCharge.email')"
        :is-editing="isEditing"
        :value="solverCorporation.deputyCharge.email"
      >
        <Sheet style="width: 100%">
          <TextField
            v-model="localParam.deputyCharge.email"
            role="textbox"
            :email="localParam.deputyCharge.email ? true : false"
            :max-length="80"
            :aria-label="$t('solver-corporation.row.deputyCharge.email')"
            style-set="outlined"
            @input="onInputForm"
          />
        </Sheet>
      </CommonDetailRow>
      <!-- その他担当 -->
      <v-row class="row-subtitle">{{
        $t('solver-corporation.row.otherCharge.mainTitle')
      }}</v-row>
      <!-- 氏名 -->
      <CommonDetailRow
        :label="$t('solver-corporation.row.otherCharge.name')"
        :is-editing="isEditing"
        :value="solverCorporation.otherCharge.name"
        :escape-value="false"
        :tall="true"
      >
        <Textarea
          v-model="localParam.otherCharge.name"
          :max-length="255"
          style-set="outlined"
          @input="onInputForm"
        />
      </CommonDetailRow>
      <!-- 部署 -->
      <CommonDetailRow
        :label="$t('solver-corporation.row.otherCharge.department')"
        :is-editing="isEditing"
        :value="solverCorporation.otherCharge.department"
        :escape-value="false"
        :tall="true"
      >
        <Textarea
          v-model="localParam.otherCharge.department"
          :max-length="255"
          style-set="outlined"
          @input="onInputForm"
        />
      </CommonDetailRow>
      <!-- 役職 -->
      <CommonDetailRow
        :label="$t('solver-corporation.row.otherCharge.title')"
        :is-editing="isEditing"
        :value="solverCorporation.otherCharge.title"
        :escape-value="false"
        :tall="true"
      >
        <Textarea
          v-model="localParam.otherCharge.title"
          :max-length="255"
          style-set="outlined"
          @input="onInputForm"
        />
      </CommonDetailRow>
      <!-- 電話番号（ハイフンあり） -->
      <CommonDetailRow
        :label="$t('solver-corporation.row.otherCharge.phone')"
        :is-editing="isEditing"
        :value="solverCorporation.otherCharge.phone"
        :escape-value="false"
        :tall="true"
      >
        <Textarea
          v-model="localParam.otherCharge.phone"
          :max-length="255"
          style-set="outlined"
          @input="onInputForm"
        />
      </CommonDetailRow>
      <!-- メールアドレス -->
      <CommonDetailRow
        :label="$t('solver-corporation.row.otherCharge.email')"
        :is-editing="isEditing"
        :value="solverCorporation.otherCharge.email"
        :escape-value="false"
        :tall="true"
      >
        <Textarea
          v-model="localParam.otherCharge.email"
          :email="true"
          :max-length="255"
          style-set="outlined"
          @input="onInputForm"
        />
      </CommonDetailRow>
    </SolverCorporationDetailSection>
    <SolverCorporationDetailSection
      :title="$t('solver-corporation.pages.detail.section.other')"
    >
      <!-- 備考 -->
      <CommonDetailRow
        :label="$t('solver.row.remarks.name')"
        :tall="true"
        :is-editing="isEditing"
        :value="solverCorporation.notes"
        :escape-value="false"
      >
        <Textarea
          v-model="localParam.notes"
          :max-length="255"
          style-set="outlined"
          @input="onInputForm"
        />
      </CommonDetailRow>
    </SolverCorporationDetailSection>
  </v-form>
</template>

<script lang="ts">
import { cloneDeep } from 'lodash'
import { format } from 'date-fns'
import CommonDetailRows from '../../common/molecules/CommonDetailRows.vue'
import type { PropType } from '~/common/BaseComponent'
import {
  TextField,
  Select,
  Sheet,
  Textarea,
} from '~/components/common/atoms/index'
import CommonDetailRow from '~/components/common/molecules/CommonDetailRow.vue'
import LastUpdate from '~/components/common/molecules/LastUpdate.vue'
import SolverDateSelect from '~/components/common/molecules/SolverDateSelect.vue'
import SolverFile from '~/components/solver/molecules/SolverFile.vue'
import SolverCorporationDetailSection from '~/components/solver-corporation/molecules/SolverCorporationDetailSection.vue'
import SolverCorporationImage from '~/components/solver-corporation/molecules/SolverCorporationImage.vue'
import { solverCorporationStore } from '~/store'
// LocalSolverCorporation（更新データ）型はGetSolverCorporationByIdResponse型と同じ
import {
  GetSolverCorporationByIdResponse,
  GetSolverCorporationByIdResponse as LocalSolverCorporation,
} from '~/models/SolverCorporation'
import { IGetSelectItemsResponse } from '~/types/Master'
import { IFile } from '~/utils/upload'
export { LocalSolverCorporation }

export interface isLoading {
  selectItem: boolean
  solverCorporation: boolean
}

class LocalCorporateInfoDocument {
  path: string = ''
  fileName: string = ''
}

export default CommonDetailRows.extend({
  name: 'SolverCorporationDetailRows',
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
    Textarea,
    SolverDateSelect,
    SolverCorporationDetailSection,
    SolverCorporationImage,
    SolverFile,
  },
  props: {
    /** 選択した法人ソルバー情報 */
    solverCorporation: {
      type: Object as PropType<GetSolverCorporationByIdResponse>,
      required: true,
    },
    issueMap50: {
      type: Object as PropType<IGetSelectItemsResponse>,
      required: true,
    },
    isLoading: {
      type: Object as PropType<isLoading>,
      required: true,
    },
    isEditing: {
      type: Boolean,
      required: true,
    },
    isDisabled: {
      type: Boolean,
      default: true,
    },
    isNew: {
      type: Boolean,
      default: true,
    },
  },
  data() {
    const currentDate = new Date()
    return {
      /** APIレスポンスの案件情報を編集用にコピー */
      localParam: Object.assign(
        new LocalSolverCorporation(),
        /** propsの値を直接変更をするとエラーになるのでコピー */
        cloneDeep(this.solverCorporation)
      ),

      // 課題マップ50確認画面表示用
      issueMap50Value: '',

      // 都道府県を定義
      stateItems: [
        { label: '北海道', value: '北海道' },
        { label: '青森県', value: '青森県' },
        { label: '岩手県', value: '岩手県' },
        { label: '宮城県', value: '宮城県' },
        { label: '秋田県', value: '秋田県' },
        { label: '山形県', value: '山形県' },
        { label: '福島県', value: '福島県' },
        { label: '茨城県', value: '茨城県' },
        { label: '栃木県', value: '栃木県' },
        { label: '群馬県', value: '群馬県' },
        { label: '埼玉県', value: '埼玉県' },
        { label: '千葉県', value: '千葉県' },
        { label: '東京都', value: '東京都' },
        { label: '神奈川県', value: '神奈川県' },
        { label: '新潟県', value: '新潟県' },
        { label: '富山県', value: '富山県' },
        { label: '石川県', value: '石川県' },
        { label: '福井県', value: '福井県' },
        { label: '山梨県', value: '山梨県' },
        { label: '長野県', value: '長野県' },
        { label: '岐阜県', value: '岐阜県' },
        { label: '静岡県', value: '静岡県' },
        { label: '愛知県', value: '愛知県' },
        { label: '三重県', value: '三重県' },
        { label: '滋賀県', value: '滋賀県' },
        { label: '京都府', value: '京都府' },
        { label: '大阪府', value: '大阪府' },
        { label: '兵庫県', value: '兵庫県' },
        { label: '奈良県', value: '奈良県' },
        { label: '和歌山県', value: '和歌山県' },
        { label: '鳥取県', value: '鳥取県' },
        { label: '島根県', value: '島根県' },
        { label: '岡山県', value: '岡山県' },
        { label: '広島県', value: '広島県' },
        { label: '山口県', value: '山口県' },
        { label: '徳島県', value: '徳島県' },
        { label: '香川県', value: '香川県' },
        { label: '愛媛県', value: '愛媛県' },
        { label: '高知県', value: '高知県' },
        { label: '福岡県', value: '福岡県' },
        { label: '佐賀県', value: '佐賀県' },
        { label: '長崎県', value: '長崎県' },
        { label: '熊本県', value: '熊本県' },
        { label: '大分県', value: '大分県' },
        { label: '宮崎県', value: '宮崎県' },
        { label: '鹿児島県', value: '鹿児島県' },
        { label: '沖縄県', value: '沖縄県' },
      ],
      keysToDeleteFiles: [] as string[],
      isChanged: false,
      localCorporateInfoDocument: new LocalCorporateInfoDocument(),
      // 現在の年月日を基準に計算
      calculatedMinDate: this.$t('solver-corporation.row.established.minDate'),
      calculatedMaxDate: format(
        new Date(currentDate.getFullYear() + 100, 11, 31),
        'yyyy-MM-dd'
      ), // 現在の年から100年後の12月31日
    }
  },
  mounted() {
    // 住所の都道府県が存在する場合setStateFromExistingDataを実行
    if (this.solverCorporation.address.state) {
      this.setStateFromExistingData(this.solverCorporation.address.state)
    }
  },
  watch: {
    // formのバリデーションが変化した時の処理
    isValid() {
      this.$emit('change:isValid', this.isValid)
    },
    solverCorporation() {
      /** 案件情報をコピーする関数を実行 */
      this.resetLocalParam()
      if (this.solverCorporation.issueMap50) {
        this.findIssueMap50Value(this.solverCorporation.issueMap50)
      }
    },
  },
  methods: {
    /** APIレスポンスの案件情報を編集用にコピー */
    resetLocalParam() {
      this.localParam = Object.assign(
        new LocalSolverCorporation(),
        /** propsの値を直接変更をするとエラーになるのでコピー */
        cloneDeep(this.solverCorporation)
      )
    },
    // isValidの値を親コンポーネントに渡す処理
    onInputForm() {
      this.$emit('change:isValid', this.isValid)
    },
    // formの入力値を親コンポーネントに渡す処理（法人ソルバー画像）
    onInputImage(newValue: IFile) {
      const solverCorporationImageInputData = {
        image: newValue,
        ...this.localParam,
      }
      if (newValue === undefined || newValue.name) {
        this.$emit('inputForm', solverCorporationImageInputData)
        // isValidの値を親コンポーネントに渡す処理
        this.$emit('change:isValid', this.isValid)
      }
    },
    // formの入力値を親コンポーネントに渡す処理（会社案内資料）
    onInputFile(newValue: IFile) {
      const solverCorporationDocumentInputData = {
        files: newValue,
        ...this.localParam,
      }
      this.$emit('inputForm', solverCorporationDocumentInputData)
      // isValidの値を親コンポーネントに渡す処理
      this.$emit('change:isValid', this.isValid)
    },
    // 課題マップ50の表示名を抽出する処理
    findIssueMap50Value(newValue: string[]) {
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
          if (category !== 'null') {
            acc.push({ header: category })
          }
          acc.push(...items)
          return acc
        },
        [] as Array<{ header: string } | { label: string; value: string }>
      )

      return formattedTargetArray
    },
    // 都道府県を設定
    setStateFromExistingData(state: string) {
      const existingState = this.stateItems.find((item) => item.value === state)
      if (existingState) {
        this.solverCorporation.address.state = existingState.value
      }
    },
    // フォームに「e、E、+、-、.」の入力を不可
    blockInput(event: any) {
      if (
        event.key === 'e' ||
        event.key === 'E' ||
        event.key === '+' ||
        event.key === '-' ||
        event.key === '.'
      ) {
        event.preventDefault()
      }
    },
  },
  computed: {
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
    // 添付ファイル用法人ソルバーID
    solverCorporationIdFile(): string {
      if (solverCorporationStore.id) {
        return solverCorporationStore.id
      } else {
        return this.solverCorporation.id as string
      }
    },
  },
})
</script>

<style lang="scss" scoped>
.row-subtitle {
  font-size: 1.125rem;
  font-weight: bold;
  margin-top: 1.125rem;
  margin-left: 0rem;
  margin-bottom: 0rem;
  border-left: 7px solid $c-primary;
  padding-left: 1.125rem;
}

.o-common-detail-rows__table {
  width: 100%;
  th {
    width: 100px !important;
  }
}
</style>
