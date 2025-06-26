<template>
  <div>
    <DetailContainer
      :title="title"
      hx="h1"
      is-hide-button1
      is-hide-button2
      is-hide-button3
      show-current-program
      :is-current-program="isCurrentProgram"
      :master-karte-project="masterKarteProject"
      :is-editing="false"
    >
      <!-- 最終更新者・更新日時 -->
      <v-row class="last-update">
        <LastUpdate
          :user="
            isCurrentProgram
              ? masterKarteProject.currentProgram.lastUpdateBy
              : masterKarteProject.nextProgram.lastUpdateBy
          "
          :date="
            isCurrentProgram
              ? masterKarteProject.currentProgram.lastUpdateDatetime
              : masterKarteProject.nextProgram.lastUpdateDatetime
          "
          :show-term="false"
          :source-screen="
            isCurrentProgram ? 'MasterKarteCurrent' : 'MasterKarteNext'
          "
          :is-public="
            isCurrentProgram
              ? true
              : localParam.nextProgram.isCustomerPublic
              ? true
              : false
          "
          :is-customer="isCustomer"
        />
      </v-row>
      <!-- 5種のタブ -->
      <MasterKarteDetailTabs
        v-if="!isCustomer"
        :selected="selected"
        :is-current-program="isCurrentProgram"
        @click="onClickTab"
      />
      <MasterKarteDetailContent
        :karten="karten"
        :selected="selected"
        :is-current-program="isCurrentProgram"
        :master-karte-project="masterKarteProject"
        :show-current-program="showCurrentProgram"
        :local-param="localParam"
        @updateResultValue="updateResultValue"
        @updateOthersValue="updateOthersValue"
        @updateFundamentalValue="updateFundamentalValue"
      />

      <template #button>
        <div class="m-heading__button is-karte is-master-karte">
          <!-- 当期支援 -->
          <slot v-if="isCurrentProgram" name="button">
            <!-- 当期支援内容の更新を通知するチェックボックス -->
            <v-checkbox
              v-show="editableRoles"
              v-model="isNotifyUpdateMasterKarteCurrent"
              class="current-checkbox"
            >
              <template #label>
                <span
                  id="checkboxLabel"
                  :class="editableRoles ? 'current-display' : ''"
                  >{{ $t('master-karte.pages.detail.checkbox.current') }}</span
                >
              </template>
            </v-checkbox>

            <!-- 保存するボタン -->
            <Button
              v-show="editableRoles"
              style-set="normal-primary"
              style="height: 30px; width: 96px; font-size: 12px"
              @click="onClickCurrentSave"
            >
              {{ $t('common.button.save2') }}
            </Button>
          </slot>

          <!-- 次期支援 -->
          <slot v-if="!isCurrentProgram" name="button">
            <v-checkbox v-model="showCurrentProgram" class="current-checkbox">
              <template #label>
                <span
                  id="checkboxLabel"
                  :class="editableRoles ? 'current-display' : ''"
                  >{{
                    $t('master-karte.pages.detail.container.showCurrentProgram')
                  }}</span
                >
              </template>
            </v-checkbox>

            <!-- 次期支援内容の更新を通知するチェックボックス -->
            <v-checkbox
              v-show="editableRoles"
              v-model="isNotifyUpdateMasterKarteNext"
              class="current-checkbox"
            >
              <template #label>
                <span
                  id="checkboxLabel"
                  :class="editableRoles ? 'current-display' : ''"
                  >{{ $t('master-karte.pages.detail.checkbox.next') }}</span
                >
              </template>
            </v-checkbox>

            <div v-show="editableRoles" class="public-private-select">
              <span>{{
                $t('master-karte.pages.detail.container.save.customer')
              }}</span>
              <!-- お客様公開 -->
              <Select
                v-model="localParam.nextProgram.isCustomerPublic"
                dense
                outlined
                :aria-label="
                  $t('master-karte.pages.detail.container.save.labels.customer')
                "
                :items="[
                  {
                    text: $t('master-karte.pages.detail.container.save.public'),
                    value: true,
                  },
                  {
                    text: $t(
                      'master-karte.pages.detail.container.save.private'
                    ),
                    value: false,
                  },
                ]"
                :class="{
                  private: !localParam.nextProgram.isCustomerPublic,
                }"
              />
            </div>
            <!-- 保存ボタン -->
            <Button
              v-show="editableRoles"
              style-set="normal-primary"
              style="height: 30px; width: 96px; font-size: 12px"
              @click="onClickNextSave"
            >
              {{ $t('common.button.save2') }}
            </Button>
          </slot>
        </div>
      </template>

      <!-- フッターボタン -->
      <template v-if="editableRoles" #footerButton>
        <!-- 当期支援 -->
        <div v-if="isCurrentProgram" class="inside-footer">
          <!-- 当期支援内容の更新を通知するチェックボックス -->
          <v-checkbox
            v-model="isNotifyUpdateMasterKarteCurrent"
            class="current-checkbox"
          >
            <template #label>
              <span
                id="checkboxLabel"
                :class="editableRoles ? 'current-display' : ''"
                >{{ $t('master-karte.pages.detail.checkbox.current') }}</span
              >
            </template>
          </v-checkbox>
          <Button
            v-if="isCurrentProgram"
            style-set="large-primary"
            style="height: 44px; width: 160px"
            @click="onClickCurrentSave"
          >
            {{ $t('common.button.save2') }}
          </Button>
        </div>
        <!-- 次期支援 -->
        <div v-else class="inside-footer">
          <!-- 次期支援内容の更新を通知するチェックボックス -->
          <v-checkbox
            v-model="isNotifyUpdateMasterKarteNext"
            class="current-checkbox"
          >
            <template #label>
              <span
                id="checkboxLabel"
                :class="editableRoles ? 'current-display' : ''"
                >{{ $t('master-karte.pages.detail.checkbox.next') }}</span
              >
            </template>
          </v-checkbox>
          <div class="public-private-select">
            <span>{{
              $t('master-karte.pages.detail.container.save.customer')
            }}</span>
            <!-- お客様公開 -->
            <Select
              v-model="localParam.nextProgram.isCustomerPublic"
              dense
              outlined
              :aria-label="
                $t('master-karte.pages.detail.container.save.labels.customer')
              "
              :items="[
                {
                  text: $t('master-karte.pages.detail.container.save.public'),
                  value: true,
                },
                {
                  text: $t('master-karte.pages.detail.container.save.private'),
                  value: false,
                },
              ]"
              style="width: 180px; height: 32px"
              :class="{
                private: !localParam.nextProgram.isCustomerPublic,
              }"
            />
          </div>
          <!-- 保存ボタン -->
          <Button
            style-set="large-primary"
            style="height: 44px; width: 160px"
            @click="onClickNextSave"
          >
            {{ $t('common.button.save2') }}
          </Button>
        </div>
      </template>
    </DetailContainer>
    <slot name="snackBar" />
  </div>
</template>

<script lang="ts">
import MasterKarteDetailContent from './MasterKarteDetailContent.vue'
import DetailContainer from '~/components/common/organisms/DetailContainer.vue'
import CommonDetailContainer from '~/components/common/organisms/CommonDetailContainer.vue'
import LastUpdate from '~/components/common/molecules/LastUpdate.vue'
import { GetKartenResponse } from '~/models/Karte'
import {
  GetMasterKarteByIdResponseClass,
  UpdateMasterKarteResponseClass,
} from '~/models/MasterKarte'
import { currentPageDataStore, meStore } from '~/store'
import type { PropType } from '~/common/BaseComponent'
import { ENUM_USER_ROLE } from '~/types/User'
import { Button, Select } from '~/components/common/atoms/index'

export default CommonDetailContainer.extend({
  components: {
    DetailContainer,
    LastUpdate,
    Button,
    Select,
    MasterKarteDetailContent,
  },
  props: {
    karten: {
      type: Array as PropType<GetKartenResponse>,
    },
    isCurrentProgram: {
      type: Boolean,
      default: true,
    },
    localParam: {
      type: Object as PropType<UpdateMasterKarteResponseClass>,
    },
    masterKarteProject: {
      type: Object as PropType<GetMasterKarteByIdResponseClass>,
    },
  },
  data() {
    return {
      selected: 'fundamental',
      showCurrentProgram: true,
      isNotifyUpdateMasterKarteCurrent: false,
      isNotifyUpdateMasterKarteNext: false,
    }
  },
  methods: {
    /**
     * タブクリック時
     * @param tab タブ名
     */
    onClickTab(tab: string) {
      this.selected = tab
    },

    /**
     * 次期支援の更新
     * 当期支援の内容はマスターカルテ詳細取得APIのレスポンス初期値を使用
     */
    onClickNextSave() {
      this.localParam.nextProgram.version = this.masterKarteProject.nextProgram
        .version
        ? this.masterKarteProject.nextProgram.version
        : 0

      // lineupを配列から+繋ぎの文字列に変更
      if (
        this.localParam.nextProgram.fundamentalInformation.lineup.length > 0
      ) {
        this.localParam.nextProgram.fundamentalInformation.lineup =
          this.localParam.nextProgram.fundamentalInformation.lineup.length === 1
            ? this.localParam.nextProgram.fundamentalInformation.lineup[0]
            : // @ts-ignore
              this.localParam.nextProgram.fundamentalInformation.lineup.join(
                '+'
              )
      } else {
        this.localParam.nextProgram.fundamentalInformation.lineup = ''
      }

      const isCurrentProgram = false
      const masterKarteId = this.masterKarteProject.nextProgram.id
        ? this.masterKarteProject.nextProgram.id
        : null

      // 「次期支援内容の更新を通知する」チェックボックスの値をリクエストに代入
      this.localParam.isNotifyUpdateMasterKarte =
        this.isNotifyUpdateMasterKarteNext

      // 次期支援編集保存時は当期支援を送信しない
      // @ts-ignore
      delete this.localParam.currentProgram

      this.$emit(
        'clickSaveButton',
        masterKarteId,
        isCurrentProgram,
        this.localParam
      )
      // 各タブの更新通知のチェックボックスをクリア
      this.isNotifyUpdateMasterKarteCurrent = false
      this.isNotifyUpdateMasterKarteNext = false
    },

    /**
     * 当期支援の更新
     * 次期支援の内容はマスターカルテ詳細取得APIのレスポンス初期値を使用
     */
    onClickCurrentSave() {
      this.localParam.currentProgram.version =
        this.masterKarteProject.currentProgram.version
      const masterKarteId = this.localParam.currentProgram.id
        ? this.localParam.currentProgram.id
        : ''
      const isCurrentProgram = true

      // 「当期支援内容の更新を通知する」チェックボックスの値をリクエストに代入
      this.localParam.isNotifyUpdateMasterKarte =
        this.isNotifyUpdateMasterKarteCurrent

      // 当期支援編集保存時は次期支援を送信しない
      // @ts-ignore
      delete this.localParam.nextProgram

      this.$emit(
        'clickSaveButton',
        masterKarteId,
        isCurrentProgram,
        this.localParam
      )
      // 各タブの更新通知のチェックボックスをクリア
      this.isNotifyUpdateMasterKarteCurrent = false
      this.isNotifyUpdateMasterKarteNext = false
    },

    updateResultValue(localCurrentResult: any) {
      this.localParam.currentProgram.result = localCurrentResult
    },

    updateFundamentalValue(localCurrentFundamental: any) {
      this.localParam.nextProgram.fundamentalInformation =
        localCurrentFundamental
    },

    updateOthersValue(localCurrentOthers: any) {
      this.localParam.nextProgram.others = localCurrentOthers
    },
  },
  watch: {
    /**
     * isCurrentProgramが変更されたら、基本情報を選択状態にする
     * (個別カルテ一覧選択状態で次期支援をクリックした場合などに対応)
     */
    isCurrentProgram: {
      handler(from) {
        this.selected = 'fundamental'

        // 現在プログラムから次期支援に変更した場合にcurrentProgramの値が書きかわっている場合は警告を出す
        if (from === true) {
          if (
            this.masterKarteProject.currentProgram.result !==
            this.localParam.currentProgram.result
          ) {
            this.$logger.warn('currentProgramが変更されています。')
          }
        }

        // 現在プログラムから次期支援に変更した場合にnextProgramの値が書きかわっている場合は警告を出す
        if (from === false) {
          if (
            this.masterKarteProject.nextProgram.fundamentalInformation !==
              this.localParam.nextProgram.fundamentalInformation ||
            this.masterKarteProject.nextProgram.others !==
              this.localParam.nextProgram.others
          ) {
            this.$logger.warn('nextProgramが変更されています。')
          }
        }
      },
      immediate: true,
    },
  },
  computed: {
    title() {
      return this.isCurrentProgram
        ? this.$t('master-karte.pages.detail.titles.current')
        : this.$t('master-karte.pages.detail.titles.next')
    },
    /** お客様ロールかどうか **/
    isCustomer(): boolean {
      return meStore.role === ENUM_USER_ROLE.CUSTOMER
    },
    /**
     * 編集可能かどうか
     * 案件を担当している支援者、営業ロールのみ編集可能
     **/
    editableRoles(): boolean {
      if (
        meStore.role === ENUM_USER_ROLE.SUPPORTER ||
        meStore.role === ENUM_USER_ROLE.SALES ||
        meStore.role === ENUM_USER_ROLE.SUPPORTER_MGR ||
        meStore.role === ENUM_USER_ROLE.SALES_MGR ||
        meStore.role === ENUM_USER_ROLE.BUSINESS_MGR
      ) {
        // 担当案件
        if (meStore.projectIds?.includes(currentPageDataStore.projectId)) {
          return true
        }
      }
      return false
    },
  },
})
</script>

<style lang="scss" scoped>
.last-update {
  margin-right: 5rem;
  margin-top: 1rem;
}

.inside-footer {
  display: flex;
  align-items: center;
  gap: 15px;
}
.private {
  .v-select__selections {
    color: #308eef !important;
  }
}
.public-private-select {
  display: flex;
  align-items: center;
  gap: 5px;
  span {
    @include fontSize('small');
    color: #666666;
    font-weight: bold;
  }
}
#checkboxLabel {
  color: black;
  font-size: 14px;
}
.is-master-karte {
  display: flex;
  gap: 16px;
}
.current-display {
  padding-right: 10px;
  border-right: #e5e5e5 3px solid;
}
.current-checkbox {
  height: 40px;
  display: flex;
  align-items: center;
}
.m-heading__button {
  .a-select {
    width: 180px !important;
    max-width: 100% !important;
  }
}
</style>
