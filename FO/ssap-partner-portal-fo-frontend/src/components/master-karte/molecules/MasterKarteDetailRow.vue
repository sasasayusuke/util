<template>
  <div>
    <v-row v-show="subtitle" class="row-subtitle">{{ subtitle }} </v-row>
    <v-row
      class="o-common-detail-rows__unit"
      :class="[disabled ? 'is-disabled' : '']"
    >
      <v-col md="auto" class="icon">
        <div v-if="icon === 'void' || isCustomer" style="width: 16px"></div>
        <svg
          v-else
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
      </v-col>
      <v-col
        :cols="cols"
        align-self="start"
        class="o-common-detail-rows__title pr-4"
      >
        <slot name="label">
          {{ label }}
        </slot>
        <div v-if="$slots.explanation" class="explanation-text">
          <slot name="explanation" />
        </div>
      </v-col>
      <v-col align-self="start" class="o-common-detail-rows__data" cols="8">
        <div class="o-common-detail-rows__data__text">
          <!-- 次期支援タブ選択 -->
          <div v-show="!isCurrentProgram" style="display: flex">
            <!-- 次期支援 -->
            <div
              style="margin-right: 28px"
              :style="showCurrentProgram ? 'width: 58%' : 'width: 100%'"
            >
              <slot name="editable" />
              <!-- 編集不可モード -->
              <span v-if="!editableRole">{{ text }}</span>
            </div>
            <!-- 次期支援内当期支援 -->
            <div
              v-show="showCurrentProgram"
              style="width: 38%; color: #666; font-size: 12px"
            >
              {{ $t('master-karte.pages.detail.container.currentProgramTitle')
              }}<br />
              {{ currentText }}
            </div>
          </div>

          <!-- 当期支援タブ選択 -->
          <div v-show="isCurrentProgram">
            <!-- 編集モード -->
            <span v-if="$slots.editable" class="master-karte-text">
              <div class="o-karte-detail-rows__data">
                <v-row class="px-3 pt-3 pb-3">
                  <slot name="editable" />
                </v-row>
              </div>
            </span>
            <!-- 編集不可モード -->
            <span
              v-else
              style="white-space: pre-wrap"
              class="master-karte-text"
              v-text="text"
            />
          </div>

          <!-- URLなど編集不可項目 -->
          <slot name="uneditable" />
        </div>
      </v-col>
    </v-row>
  </div>
</template>

<script lang="ts">
import { Chip, Textarea } from '../../common/atoms/index'
import BaseComponent from '~/common/BaseComponent'
import { currentPageDataStore, meStore } from '~/store'
import { ENUM_USER_ROLE } from '~/types/User'

export default BaseComponent.extend({
  components: {
    Chip,
    Textarea,
  },
  props: {
    label: { type: String },
    cols: { type: String, default: '3' },
    text: { type: [Number, String] },
    disabled: { type: Boolean, default: false },
    icon: { type: String, default: '' },
    chipWidth: { type: Number, required: false, default: 33 },
    subtitle: { type: String, default: '', required: false },
    isCurrentProgram: {
      type: Boolean,
      default: true,
    },
    subtext: {
      type: [String, Number],
    },
    showCurrentProgram: {
      type: Boolean,
      default: true,
    },
  },
  computed: {
    chipSize() {
      return {
        width: `${this.chipWidth}px`,
      }
    },
    /** お客様ロールかどうか **/
    isCustomer(): boolean {
      return meStore.role === ENUM_USER_ROLE.CUSTOMER
    },
    /**
     * 編集可能かどうか
     * 案件を担当している支援者、営業ロールのみ編集可能
     **/
    editableRole(): boolean {
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
    /**
     * 次期支援表示時に右側に表示される当期支援
     */
    currentText(): string {
      if (typeof this.subtext === 'string') {
        // @ts-ignore
        return this.subtext?.includes('+')
          ? // @ts-ignore
            this.subtext.split('+').join(' / ')
          : this.subtext
      }
      return this.subtext?.toString()
    },
  },
})
</script>

<style lang="scss" scoped>
.o-common-detail-rows__unit {
  border-bottom: 1px solid $c-gray-line;
  margin: 0 !important;
  padding-top: 20px;
  &.is-disabled {
    .o-common-detail-rows__title,
    .o-common-detail-rows__data {
      color: #8f8f8f;
    }
  }
  &.is-noborder {
    border-bottom: 0;
  }
}
.o-common-detail-rows__label {
  padding-top: 0;
}
.o-common-detail-rows__title {
  padding-top: 0;
  font-size: 0.875rem;
  font-weight: bold;
  align-items: center;
  padding-right: 0;
}
.o-common-detail-rows__data {
  padding-top: 0;
  align-items: center;
  font-size: 0.875rem;
  padding-right: 0;
  table {
    th,
    td {
      padding-bottom: 12px;
    }
    tr {
      &:last-child {
        th,
        td {
          padding-bottom: 0;
        }
      }
    }
    th {
      text-align: left;
      padding-right: 24px;
      padding-left: 0;
      width: 150px;
    }
  }
  .o-common-detail-rows__table__edit {
    padding-left: 24px;
    text-align: left;
  }
}
.o-common-detail-rows__data__text {
  word-break: break-all;
  white-space: pre-line;
}
.explanation-text {
  font-size: 10px;
  color: #666;
  margin-top: 8px;
}
.row-subtitle {
  font-size: 1.125rem;
  font-weight: bold;
  margin-top: 1.125rem;
  margin-left: 0rem;
  margin-bottom: 0rem;
  border-left: 7px solid $c-primary;
  padding-left: 1.125rem;
}
.master-karte-text {
  font-weight: normal;
  font-size: 14px;
}

.master-karte-text__link {
  a {
    color: #008a19;
    font-weight: bold;
    text-decoration: underline;
  }
}

.satisfaction__radio::v-deep label {
  font-size: 14px;
  color: #8f8f8f;
}

.satisfaction__radio::v-deep .v-icon {
  color: #8f8f8f;
}

.is-active::v-deep label,
.is-active::v-deep .v-input--selection-controls__input .v-icon {
  color: #1867c0 !important;
  font-weight: bold;
}

.icon {
  padding-top: 0;
}
</style>
