<template>
  <Modal v-if="isModalOpen">
    <template #head>{{ $t('karte.pages.tab.modal.title') }}</template>
    <template #default>
      <v-col>
        <!-- お客様 -->
        <v-row class="modal__unit">
          <v-col cols="12">
            <v-row>
              <!-- 公開の場合 -->
              <v-col v-if="isPublic" class="font-weight-bold pt-0 pl-0">
                {{ $t('karte.pages.tab.modal.customer')
                }}{{ $t('karte.pages.tab.modal.public') }}
              </v-col>
              <!-- 非公開の場合 -->
              <v-col
                v-if="!isPublic"
                class="font-weight-bold pt-0 pl-0"
                style="color: #048bff"
                ><span style="color: #000000">{{
                  $t('karte.pages.tab.modal.customer')
                }}</span>
                {{ $t('karte.pages.tab.modal.private') }}</v-col
              >
            </v-row>
            <!-- 公開範囲説明 -->
            <v-row>
              <!-- 個別カルテかつ「非公開」から開いた場合の説明 -->
              <v-col
                v-if="sourceScreen == 'KarteDetail' && !isPublic"
                class="description"
              >
                {{ $t('karte.pages.tab.modal.customerDescriptionPrivate') }}
              </v-col>
              <!-- 個別カルテかつ「公開」から開いた場合の説明 -->
              <v-col
                v-if="sourceScreen == 'KarteDetail' && isPublic"
                class="description"
              >
                <div>
                  {{ $t('karte.pages.tab.modal.customerDescriptionPublic[0]') }}
                </div>
                <div>
                  {{ $t('karte.pages.tab.modal.customerDescriptionPublic[1]') }}
                </div>
              </v-col>
              <!-- マスターカルテ（次期支援）かつ「非公開」から開いた場合の説明 -->
              <v-col
                v-if="sourceScreen == 'MasterKarteNext' && !isPublic"
                class="description"
              >
                <div>
                  {{ $t('karte.pages.tab.modal.customerDescriptionPrivate') }}
                </div>
                <div>
                  {{ $t('karte.pages.tab.modal.customerDescription[0]') }}
                  <svg
                    xmlns="http://www.w3.org/2000/svg"
                    width="17"
                    height="13"
                    viewBox="0 0 17 13"
                  >
                    <path
                      d="M11.409 9.555a4.4 4.4 0 0 0 0-5.044A2.843 2.843 0 0 1 13.05 4a3.034 3.034 0 0 1 0 6.067 2.843 2.843 0 0 1-1.641-.511M4.975 7.033a2.976 2.976 0 1 1 2.975 3.034 3 3 0 0 1-2.975-3.034m1.7 0a1.275 1.275 0 1 0 1.275-1.3 1.288 1.288 0 0 0-1.275 1.3m7.225 8.234V17H2v-1.733S2 11.8 7.95 11.8s5.95 3.467 5.95 3.467m-1.7 0c-.119-.676-1.13-1.733-4.25-1.733s-4.19 1.135-4.25 1.733M13.857 11.8a4.64 4.64 0 0 1 1.743 3.467V17H19v-1.733s0-3.146-5.151-3.467z"
                      transform="translate(-2 -4)"
                      style="fill: #1867c0"
                    />
                  </svg>
                  {{ $t('karte.pages.tab.modal.customerDescription[1]') }}
                </div>
                <div>
                  {{ $t('karte.pages.tab.modal.customerDescription[2]') }}
                </div>
              </v-col>
              <!-- マスターカルテかつ「公開」から開いた場合 -->
              <v-col
                v-if="
                  (sourceScreen == 'MasterKarteCurrent' ||
                    sourceScreen == 'MasterKarteNext') &&
                  isPublic
                "
                class="description"
              >
                <div>
                  {{ $t('karte.pages.tab.modal.customerDescription[3]') }}
                  <svg
                    xmlns="http://www.w3.org/2000/svg"
                    width="17"
                    height="13"
                    viewBox="0 0 17 13"
                  >
                    <path
                      d="M11.409 9.555a4.4 4.4 0 0 0 0-5.044A2.843 2.843 0 0 1 13.05 4a3.034 3.034 0 0 1 0 6.067 2.843 2.843 0 0 1-1.641-.511M4.975 7.033a2.976 2.976 0 1 1 2.975 3.034 3 3 0 0 1-2.975-3.034m1.7 0a1.275 1.275 0 1 0 1.275-1.3 1.288 1.288 0 0 0-1.275 1.3m7.225 8.234V17H2v-1.733S2 11.8 7.95 11.8s5.95 3.467 5.95 3.467m-1.7 0c-.119-.676-1.13-1.733-4.25-1.733s-4.19 1.135-4.25 1.733M13.857 11.8a4.64 4.64 0 0 1 1.743 3.467V17H19v-1.733s0-3.146-5.151-3.467z"
                      transform="translate(-2 -4)"
                      style="fill: #1867c0"
                    />
                  </svg>
                  {{
                    $t('karte.pages.tab.modal.customerDescription[4]')
                  }}&emsp;&emsp;&emsp;
                </div>
                <div>
                  {{ $t('karte.pages.tab.modal.customerDescription[5]') }}
                </div>
              </v-col>
            </v-row>
          </v-col>
        </v-row>
        <!-- Sony Acceleration Platform内部 -->
        <v-row class="modal__unit">
          <v-col cols="12">
            <v-row>
              <!-- 関係者に公開 -->
              <v-col class="font-weight-bold pl-0">
                {{ $t('karte.pages.tab.modal.ssap')
                }}{{ $t('karte.pages.tab.modal.openParties') }}
              </v-col>
            </v-row>
            <!-- 公開範囲説明 -->
            <v-row>
              <v-col class="description">
                <div
                  v-if="sourceScreen != 'MasterKarteCurrent'"
                  style="margin-bottom: 10px"
                >
                  <div>
                    {{
                      $t('karte.pages.tab.modal.ssapDescription[0]')
                    }}&emsp;&emsp;
                  </div>
                  <div>
                    {{ $t('karte.pages.tab.modal.ssapDescription[1]') }}
                  </div>
                </div>
                <ul>
                  <li v-if="sourceScreen == 'KarteDetail'">
                    {{ $t('karte.pages.tab.modal.ssapDescription[2]') }}
                  </li>
                  <li v-if="sourceScreen == 'KarteDetail'">
                    {{ $t('karte.pages.tab.modal.ssapDescription[3]') }}
                  </li>
                  <li v-if="sourceScreen != 'KarteDetail'">
                    {{ $t('karte.pages.tab.modal.ssapDescription[4]') }}
                  </li>
                  <li v-if="sourceScreen != 'KarteDetail'">
                    {{ $t('karte.pages.tab.modal.ssapDescription[5]') }}
                  </li>
                  <li>
                    {{ $t('karte.pages.tab.modal.ssapDescription[6]') }}
                  </li>
                  <li>
                    {{ $t('karte.pages.tab.modal.ssapDescription[7]') }}
                  </li>
                </ul>
              </v-col>
            </v-row>
          </v-col>
        </v-row>
      </v-col>
    </template>
    <template #foot>
      <Button
        outlined
        style-set="large-tertiary"
        width="160"
        @click="$emit('closeModal')"
      >
        {{ $t('common.button.close') }}
      </Button>
    </template>
  </Modal>
</template>

<script lang="ts">
import BaseComponent from '~/common/BaseComponent'
import { Button } from '~/components/common/atoms/index'
import Modal from '~/components/common/molecules/Modal.vue'

export default BaseComponent.extend({
  components: {
    Button,
    Modal,
  },
  props: {
    /**
     * モーダルを開いているか否か
     */
    isModalOpen: {
      type: Boolean,
      default: false,
    },
    /**
     * 遷移元画面
     */
    sourceScreen: {
      type: String,
      default: '',
    },
    /**
     * 公開か非公開か
     */
    isPublic: {
      type: Boolean,
      default: false,
    },
  },
})
</script>

<style lang="scss" scoped>
.modal__unit {
  border-bottom: 1px solid $c-gray-line;
  color: $c-black;
  &:nth-child(n + 1) {
    padding-bottom: 20px;
  }
  &:nth-child(n + 2) {
    margin-top: 20px;
    padding-bottom: 20px;
  }
}
.description {
  background-color: #ebf7ed;
  padding: 12px !important;
  border-radius: 4px;
  white-space: nowrap;
}
</style>
