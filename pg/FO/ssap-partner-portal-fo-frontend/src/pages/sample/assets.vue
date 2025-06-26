<template>
  <RootTemplate :class="'p-assets fill-height align-start'">
    <v-container class="p-assets__container" py-5>
      <h1 class="text-h2 pb-10 font-weight-bold">アセット</h1>
      <section>
        <h2 class="text-h4 mb-5 font-weight-bold">ボタン</h2>
        <!-- x-large -->
        <div>
          <h3 class="text-h5 mb-1 font-weight-bold">x-large</h3>
          <Button style-set="xlarge-primary">
            {{ $t('common.button.yes') }}
          </Button>
          <Button style-set="xlarge-secondary">
            {{ $t('common.button.yes') }}
          </Button>
          <Button style-set="xlarge-tertiary">
            {{ $t('common.button.yes') }}
          </Button>
          <Button style-set="xlarge-disabled">
            {{ $t('common.button.yes') }}
          </Button>
        </div>
        <!-- large -->
        <div>
          <h3 class="text-h5 mb-1 font-weight-bold">large</h3>
          <Button style-set="large-primary">
            {{ $t('common.button.yes') }}
          </Button>
          <Button style-set="large-secondary">
            {{ $t('common.button.yes') }}
          </Button>
          <Button style-set="large-tertiary">
            {{ $t('common.button.yes') }}
          </Button>
          <Button style-set="large-disabled">
            {{ $t('common.button.yes') }}
          </Button>
        </div>
        <!-- normal -->
        <div>
          <h3 class="text-h5 mb-1 font-weight-bold">normal</h3>
          <Button style-set="normal-primary">
            {{ $t('common.button.yes') }}
          </Button>
          <Button style-set="normal-secondary">
            {{ $t('common.button.yes') }}
          </Button>
          <Button style-set="normal-tertiary">
            {{ $t('common.button.yes') }}
          </Button>
          <Button style-set="normal-disabled">
            {{ $t('common.button.yes') }}
          </Button>
        </div>
        <!-- small -->
        <div>
          <h3 class="text-h5 mb-1 font-weight-bold">small</h3>
          <Button style-set="small-primary">
            {{ $t('common.button.yes') }}
          </Button>
          <Button style-set="small-secondary">
            {{ $t('common.button.yes') }}
          </Button>
          <Button style-set="small-tertiary">
            {{ $t('common.button.yes') }}
          </Button>
          <Button style-set="small-disabled">
            {{ $t('common.button.yes') }}
          </Button>
        </div>
        <!-- x-small -->
        <div>
          <h3 class="text-h5 mb-1 font-weight-bold">x-small</h3>
          <Button style-set="xsmall-primary">
            {{ $t('common.button.yes') }}
          </Button>
          <Button style-set="xsmall-secondary">
            {{ $t('common.button.yes') }}
          </Button>
          <Button style-set="xsmall-tertiary">
            {{ $t('common.button.yes') }}
          </Button>
          <Button style-set="xsmall-disabled">
            {{ $t('common.button.yes') }}
          </Button>
        </div>
      </section>
      <section>
        <h2 class="text-h4 mb-5 font-weight-bold">ボタン（アウトライン）</h2>
        <!-- x-large -->
        <div>
          <h3 class="text-h5 mb-1 font-weight-bold">x-large</h3>
          <Button outlined style-set="xlarge-primary">
            {{ $t('common.button.yes') }}
          </Button>
          <Button outlined style-set="xlarge-secondary">
            {{ $t('common.button.yes') }}
          </Button>
          <Button outlined style-set="xlarge-tertiary">
            {{ $t('common.button.yes') }}
          </Button>
          <Button outlined style-set="xlarge-disabled">
            {{ $t('common.button.yes') }}
          </Button>
        </div>
        <!-- large -->
        <div>
          <h3 class="text-h5 mb-1 font-weight-bold">large</h3>
          <Button outlined style-set="large-primary">
            {{ $t('common.button.yes') }}
          </Button>
          <Button outlined style-set="large-secondary">
            {{ $t('common.button.yes') }}
          </Button>
          <Button outlined style-set="large-tertiary">
            {{ $t('common.button.yes') }}
          </Button>
          <Button outlined style-set="large-disabled">
            {{ $t('common.button.yes') }}
          </Button>
        </div>
        <!-- normal -->
        <div>
          <h3 class="text-h5 mb-1 font-weight-bold">normal</h3>
          <Button outlined style-set="normal-primary">
            {{ $t('common.button.yes') }}
          </Button>
          <Button outlined style-set="normal-secondary">
            {{ $t('common.button.yes') }}
          </Button>
          <Button outlined style-set="normal-tertiary">
            {{ $t('common.button.yes') }}
          </Button>
          <Button outlined style-set="normal-disabled">
            {{ $t('common.button.yes') }}
          </Button>
        </div>
        <!-- small -->
        <div>
          <h3 class="text-h5 mb-1 font-weight-bold">small</h3>
          <Button outlined style-set="small-primary">
            {{ $t('common.button.yes') }}
          </Button>
          <Button outlined style-set="small-secondary">
            {{ $t('common.button.yes') }}
          </Button>
          <Button outlined style-set="small-tertiary">
            {{ $t('common.button.yes') }}
          </Button>
          <Button outlined style-set="small-disabled">
            {{ $t('common.button.yes') }}
          </Button>
        </div>
        <!-- x-small -->
        <div>
          <h3 class="text-h5 mb-1 font-weight-bold">x-small</h3>
          <Button outlined style-set="xsmall-primary">
            {{ $t('common.button.yes') }}
          </Button>
          <Button outlined style-set="xsmall-secondary">
            {{ $t('common.button.yes') }}
          </Button>
          <Button outlined style-set="xsmall-tertiary">
            {{ $t('common.button.yes') }}
          </Button>
          <Button outlined style-set="xsmall-disabled">
            {{ $t('common.button.yes') }}
          </Button>
        </div>
      </section>
      <section>
        <h2 class="text-h4 mb-5 font-weight-bold">ボタン（戻る）</h2>
        <!-- x-large -->
        <div>
          <h3 class="text-h5 mb-1 font-weight-bold">x-large</h3>
          <Button style-set="large-primary">
            <v-layout justify-space-between>
              <Icon class="mr-2" size="14" color="white"
                >icon-org-arrow-left</Icon
              >
              {{ $t('common.button.prev') }}
              <Icon class="ml-2" size="14" color="transparent"
                >icon-org-arrow-right</Icon
              >
            </v-layout>
          </Button>
          <Button outlined style-set="large-primary">
            <v-layout justify-space-between>
              <Icon class="mr-2" size="14" color="btn_primary"
                >icon-org-arrow-left</Icon
              >
              {{ $t('common.button.prev') }}
              <Icon class="ml-2" size="14" color="transparent"
                >icon-org-arrow-right</Icon
              >
            </v-layout>
          </Button>
          <Button outlined style-set="large-tertiary">
            <v-layout justify-space-between>
              <Icon class="mr-2" size="14" color="btn_tertiary"
                >icon-org-arrow-left</Icon
              >
              {{ $t('common.button.prev') }}
              <Icon class="ml-2" size="14" color="transparent"
                >icon-org-arrow-right</Icon
              >
            </v-layout>
          </Button>
          <Button style-set="large-primary" class="px-2" width="auto">
            <v-layout justify-space-between>
              <Icon class="mr-2" size="14" color="white"
                >icon-org-arrow-left</Icon
              >
              {{ $t('common.button.lastMonth') }}
              <Icon class="ml-2" size="14" color="transparent"
                >icon-org-arrow-right</Icon
              >
            </v-layout>
          </Button>
        </div>
      </section>
      <section>
        <h2 class="text-h4 mb-5 font-weight-bold">ボタン（次へ）</h2>
        <!-- x-large -->
        <div>
          <h3 class="text-h5 mb-1 font-weight-bold">x-large</h3>
          <Button style-set="large-primary">
            <v-layout justify-space-between>
              <Icon class="mr-2" size="14" color="transparent"
                >icon-org-arrow-left</Icon
              >
              {{ $t('common.button.next') }}
              <Icon class="ml-2" size="14" color="white"
                >icon-org-arrow-right</Icon
              >
            </v-layout>
          </Button>
          <Button outlined style-set="large-primary">
            <v-layout justify-space-between>
              <Icon class="mr-2" size="14" color="transparent"
                >icon-org-arrow-left</Icon
              >
              {{ $t('common.button.next') }}
              <Icon size="14" color="btn_primary">icon-org-arrow-right</Icon>
            </v-layout>
          </Button>
          <Button outlined style-set="large-tertiary">
            <v-layout justify-space-between>
              <Icon class="mr-2" size="14" color="transparent"
                >icon-org-arrow-left</Icon
              >
              {{ $t('common.button.next') }}
              <Icon class="ml-2" size="14" color="btn_tertiary"
                >icon-org-arrow-right</Icon
              >
            </v-layout>
          </Button>
          <Button style-set="large-primary" class="px-2" width="auto">
            <v-layout justify-space-between>
              <Icon class="mr-2" size="14" color="transparent"
                >icon-org-arrow-left</Icon
              >
              {{ $t('common.button.nextMonth') }}
              <Icon class="ml-2" size="14" color="white"
                >icon-org-arrow-right</Icon
              >
            </v-layout>
          </Button>
        </div>
      </section>
      <section>
        <h2 class="text-h4 mb-5 font-weight-bold">追加・削除</h2>
        <div>
          <Button style-set="add">
            <Icon class="mr-1" size="20" color="btn_primary">mdi-plus</Icon>
            {{ $t('common.button.addQuestion') }}
          </Button>
          <Button style-set="add">
            <Icon class="mr-1" size="20" color="btn_primary">mdi-plus</Icon>
            {{ $t('common.button.addGroup') }}
          </Button>
          <Button style-set="add">
            <Icon class="mr-1" size="20" color="btn_primary">mdi-minus</Icon>
            {{ $t('common.button.delGroup') }}
          </Button>
        </div>
      </section>
      <section>
        <h2 class="text-h4 mb-5 font-weight-bold">その他</h2>
        <div>
          <Button style-set="sort">
            {{ $t('common.button.sort') }}
          </Button>
          <Button style-set="submit"> 確定して提出する </Button>
          <Button style-set="count">
            <Icon size="20">mdi-chevron-up</Icon>
          </Button>
          <Button style-set="count">
            <Icon size="20">mdi-chevron-down</Icon>
          </Button>
        </div>
      </section>
      <section>
        <h2 class="text-h4 mb-5 font-weight-bold">ページネーション</h2>
        <div>
          <v-pagination
            v-model="page"
            :length="6"
            color="btn_primary"
          ></v-pagination>
        </div>
      </section>
      <section>
        <h2 class="text-h4 mb-5 font-weight-bold">グループボタン</h2>
        <div>
          <GroupButton>
            <Button style-set="group-xlarge"> サービスアンケート </Button>
            <Button outlined style-set="group-xlarge">
              サービスアンケート<br />修了アンケート合算
            </Button>
          </GroupButton>
        </div>
        <div>
          <GroupButton>
            <Button disabled style-set="group-xlarge">
              サービスアンケート
            </Button>
            <Button disabled style-set="group-xlarge">
              サービスアンケート<br />修了アンケート合算
            </Button>
          </GroupButton>
        </div>
        <div>
          <GroupButton>
            <Button style-set="group-small"> 組織開発 </Button>
            <Button outlined style-set="group-small"> アイデア可視化 </Button>
            <Button outlined style-set="group-small"> 事業育成＆検証 </Button>
          </GroupButton>
        </div>
      </section>
      <section>
        <h2 class="text-h4 mb-5 font-weight-bold">開閉メニュー</h2>
        <div>
          <Menu
            bottom
            :offset-y="true"
            name="その他"
            :list-values="[
              'メニュー１',
              'メニュー２',
              'メニュー３',
              'メニュー４',
            ]"
            :list-links="['/', '/', '/', '/']"
          >
          </Menu>
        </div>
      </section>
      <section>
        <h2 class="text-h4 mb-5 font-weight-bold">アイコン付きリンク</h2>
        <div>
          <TextLink :to="'/'">hoge</TextLink>
          <br />
          <TextLink :to="'/'" disabled>hoge</TextLink>
        </div>
      </section>
      <section>
        <h2 class="text-h4 mb-5 font-weight-bold">チェックボックス</h2>
        <div>
          <Checkbox
            v-model="checkbox"
            label="チェックボックス1"
            value="チェックボックス1"
            hide-details
          />
          <Checkbox
            v-model="checkbox"
            label="チェックボックス2"
            value="チェックボックス2"
            hide-details
          />
          <Checkbox
            v-model="checkbox"
            label="チェックボックス3"
            value="チェックボックス3"
            hide-details
            disabled
          />
          <Checkbox
            v-model="checkbox"
            class="is-error"
            label="チェックボックス4"
            value="チェックボックス4"
            hide-details
          />
          <ErrorText class="mt-2"
            >エラーテキストエラーテキストエラーテキスト</ErrorText
          >
        </div>
      </section>
      <section>
        <h2 class="text-h4 mb-5 font-weight-bold">ラジオボタン</h2>
        <div>
          <RadioGroup
            v-model="checkbox"
            :labels="['ラジオボタン1']"
            :values="['ラジオボタン1']"
            hide-details
          />
          <RadioGroup
            v-model="checkbox"
            :labels="['ラジオボタン2']"
            :values="['ラジオボタン2']"
            hide-details
            disabled
          />
          <RadioGroup
            v-model="checkbox"
            :labels="['ラジオボタン3']"
            :values="['ラジオボタン3']"
            error-text="エラーエラーエラーエラーエラー"
            class="is-error"
            hide-details
          />
          <ErrorText class="mt-2"
            >エラーテキストエラーテキストエラーテキスト</ErrorText
          >
        </div>
      </section>
      <section>
        <h2 class="text-h4 mb-5 font-weight-bold">テキストボックス</h2>
        <div>
          <TextField role="textbox" dense outlined />
          <br />
          <TextField role="textbox" class="is-error" dense outlined />
          <ErrorText class="mt-2"
            >エラーテキストエラーテキストエラーテキスト</ErrorText
          >
        </div>
      </section>
      <section>
        <h2 class="text-h4 mb-5 font-weight-bold">セレクトボックス</h2>
        <div>
          <Select
            dense
            outlined
            :items="['アイテム1', 'アイテム2', 'アイテム3', 'アイテム4']"
          />
          <br />
          <Select
            class="is-error"
            dense
            outlined
            :items="['アイテム1', 'アイテム2', 'アイテム3', 'アイテム4']"
          />
          <ErrorText class="mt-2"
            >エラーテキストエラーテキストエラーテキスト</ErrorText
          >
        </div>
      </section>
      <section>
        <h2 class="text-h4 mb-5 font-weight-bold">日付選択</h2>
        <div>
          <DateSelect />
          <br />
          <DateSelect :disabled="true" />
          <br />
          <DateSelect class="is-error" />
          <ErrorText class="mt-2 ml-8"
            >エラーテキストエラーテキストエラーテキスト</ErrorText
          >
        </div>
      </section>
      <section>
        <h2 class="text-h4 mb-5 font-weight-bold">時間選択</h2>
        <div>
          <TimeSelect />
          <br />
          <TimeSelect :disabled="true" />
          <br />
          <TimeSelect class="is-error" />
          <ErrorText class="mt-2 ml-8"
            >エラーテキストエラーテキストエラーテキスト</ErrorText
          >
        </div>
      </section>
      <section>
        <h2 class="text-h4 mb-5 font-weight-bold">テキストエリア</h2>
        <div>
          <Textarea outlined placeholder="placeholder" value="14pxの場合" />
          <br />
          <Textarea
            class="font-small"
            outlined
            placeholder="placeholder"
            value="12pxの場合"
          />
          <br />
          <Textarea outlined disabled placeholder="placeholder" />
          <br />
          <Textarea class="is-error" outlined placeholder="placeholder" />
          <ErrorText class="mt-2"
            >エラーテキストエラーテキストエラーテキスト</ErrorText
          >
        </div>
      </section>
      <section>
        <h2 class="text-h4 mb-5 font-weight-bold">ファイルインプット</h2>
        <div>
          <FileInput dense outlined />
          <br />
          <FileInput class="is-error" dense outlined />
          <ErrorText class="mt-2 ml-8"
            >エラーテキストエラーテキストエラーテキスト</ErrorText
          >
        </div>
      </section>
      <section>
        <h2 class="text-h4 mb-5 font-weight-bold">ラベル</h2>
        <div>
          <Chip x-small outlined style-set="primary">sf</Chip>
          <Chip x-small style-set="required">必須</Chip>
          <Chip x-small style-set="any">任意</Chip>
          <Chip outlined style-set="primary">sf</Chip>
          <Chip style-set="required">必須</Chip>
          <Chip style-set="any">任意</Chip>
          <br /><br />
          <Chip large outlined style-set="secondary-70"> 予定 </Chip>
          <Chip large style-set="primary-70"> 実施中 </Chip>
          <Chip large style-set="secondary-70"> 実施済み </Chip>
          <br />
          <br />
          <Chip small outlined style-set="secondary-70"> 未回答 </Chip>
          <Chip small style-set="secondary-70"> 済み </Chip>
          <Chip small style-set="tertiary-70"> アーカイブ </Chip>
        </div>
      </section>
      <section>
        <h2 class="text-h4 mb-5 font-weight-bold">タイトル</h2>
        <div>
          <Heading :level="3">
            <template #title> 担当案件集計 </template>
          </Heading>
          <Heading :level="3" class="type2">
            <template #title> 担当案件集計 </template>
            <template #date>最終集計日時： 2021/08/02 17:21</template>
            <template #button>
              <Button outlined style-set="small-primary">案件一覧</Button>
            </template>
          </Heading>
          <Heading :level="3" class="type3">
            <template #title> 担当案件集計 </template>
            <template #date>最終集計日時： 2021/08/02 17:21</template>
            <template #button>
              <Button outlined style-set="small-primary">案件一覧</Button>
            </template>
          </Heading>
          <Heading :level="3" class="type4">
            <template #title> 担当案件集計 </template>
            <template #date>最終集計日時： 2021/08/02 17:21</template>
            <template #button>
              <Button outlined style-set="small-primary">案件一覧</Button>
            </template>
          </Heading>
          <Heading :level="3" class="type5">
            <template #title> 担当案件集計 </template>
            <template #date>最終集計日時： 2021/08/02 17:21</template>
            <template #button>
              <Button outlined style-set="small-primary">案件一覧</Button>
            </template>
          </Heading>
        </div>
      </section>
    </v-container>
  </RootTemplate>
</template>

<script lang="ts">
import {
  Button,
  Icon,
  TextLink,
  Checkbox,
  RadioGroup,
  TextField,
  Select,
  DatePicker,
  ErrorText,
  Textarea,
  FileInput,
  Chip,
} from '~/components/common/atoms/index'
import BaseComponent from '~/common/BaseComponent'
import RootTemplate from '~/components/common/bases/RootTemplate.vue'

import GroupButton from '~/components/common/molecules/GroupButton.vue'
import Menu from '~/components/common/molecules/Menu.vue'
import DateSelect from '~/components/common/molecules/DateSelect.vue'
import TimeSelect from '~/components/common/molecules/TimeSelect.vue'
import Heading from '~/components/common/molecules/Heading.vue'

export default BaseComponent.extend({
  components: {
    RootTemplate,
    Menu,
    DateSelect,
    GroupButton,
    Button,
    Icon,
    TextLink,
    Checkbox,
    RadioGroup,
    TextField,
    Select,
    DatePicker,
    ErrorText,
    TimeSelect,
    Textarea,
    FileInput,
    Chip,
    Heading,
  },
  data() {
    return {
      page: 1,
      checkbox: '',
      menu2: '',
      date: '',
    }
  },
})
</script>
<style lang="scss" scoped>
.p-assets {
  section {
    &:nth-of-type(n + 2) {
      padding-top: 40px;
    }
    > div {
      &:nth-of-type(n + 2) {
        padding-top: 20px;
      }
    }
  }
}
.p-assets__container {
  > section {
    // display: none;
    &:last-of-type {
      display: block;
    }
  }
}
</style>
