<template>
  <div class="m-karter-aside-links">
    <!-- ファイルダウンロード -->
    <template v-if="isDownload">
      <ul class="m-karter-aside-links__list">
        <template v-for="(file, index) in links">
          <li
            v-if="file.path !== ''"
            :key="index"
            class="m-karter-aside-links__item"
          >
            <Icon size="12">icon-org-arrow</Icon>
            <!--<a :href="file.path" :download="file.fileName">-->
            <a @click="download(file.path, file.fileName)">
              {{ file.fileName }}
            </a>
            <Icon v-if="isEditing" size="18" @click="clearFilesOnScreen(index)"
              >mdi-close</Icon
            >
          </li>
        </template>
      </ul>
      <ul v-if="isEditing" class="m-karter-aside-links__list">
        <template v-if="isDeliverables">
          <li
            v-for="(file, index) in files"
            :key="index"
            class="m-karter-aside-links__item--file-input"
          >
            <FileInput
              class="mb-7"
              accept=".txt, .pdf, .doc, .docx, .xls, .xlsx, .ppt, .pptx, .csv, .zip, .ai, .psd, .jpg, .jpeg, .png, .gif, .mp4, .mov"
              :placeholder="$t('common.placeholder.file_input')"
              :allow-extensions="allowExtensions"
              :max-file-size="maxFileSize"
              @change="onInput($event, index)"
            />
          </li>
        </template>
        <li v-else :key="index" class="m-karter-aside-links__item--file-input">
          <FileInput
            class="mb-7"
            accept=".txt, .pdf, .doc, .docx, .xls, .xlsx, .ppt, .pptx, .csv, .zip, .ai, .psd, .jpg, .jpeg, .png, .gif, .mp4, .mov"
            :placeholder="$t('common.placeholder.file_input')"
            :allow-extensions="allowExtensions"
            :max-file-size="maxFileSize"
          />
        </li>
      </ul>
    </template>
  </div>
</template>

<script lang="ts">
import { format } from 'date-fns'
import { meStore } from '~/store'
import { Icon, FileInput } from '~/components/common/atoms/index'
import BaseComponent from '~/common/BaseComponent'
import { downloadFile } from '~/utils/download'
import { ENUM_MAX_FILE_SIZE } from '~/types/Karte'
import { ENUM_USER_ROLE } from '~/types/User'
import { Documents, GetKarteByIdResponse } from '~/models/Karte'
import type { PropType } from '~/common/BaseComponent'
import { GetProjectByIdResponse } from '~/models/Project'
import { SurveyListItem } from '~/models/Survey'
import { hasRole } from '~/utils/role-authorizer'
import { getDateStr } from '~/utils/upload'

export default BaseComponent.extend({
  name: '',
  components: {
    Icon,
    FileInput,
  },
  props: {
    /**
     * リンク情報
     */
    links: {
      type: Array as PropType<Documents[]>,
      required: true,
    },
    /**
     * ダウンロードしているか否か
     */
    isDownload: {
      type: Boolean,
      default: false,
    },
    /**
     * 編集か否か
     */
    isEditing: {
      type: Boolean,
    },
    /**
     * 成果物か否か
     */
    isDeliverables: {
      type: Boolean,
    },
    /**
     * 閲覧可能なアンケートか否か
     */
    isAllowSurveys: {
      type: Boolean,
    },
    /**
     * emitで親コンポーネントに渡すメソッド名を作るための構成文字列
     */
    item: {
      type: String,
      required: false,
    },
    /**
     * プロジェクト情報
     */
    project: {
      type: Object as PropType<GetProjectByIdResponse>,
      required: false,
    },
    /**
     * カルテ情報
     */
    karte: {
      type: Object as PropType<GetKarteByIdResponse>,
      required: false,
    },
  },
  data() {
    return {
      allowExtensions: [
        'txt',
        'pdf',
        'doc',
        'docx',
        'xls',
        'xlsx',
        'ppt',
        'pptx',
        'csv',
        'zip',
        'ai',
        'psd',
        'jpg',
        'jpeg',
        'png',
        'gif',
        'mp4',
        'mov',
      ],
      maxFileSize: ENUM_MAX_FILE_SIZE,
      files: [null],
      index: '',
      filteredLink: this.links,
    }
  },
  created() {
    // this.linksはpropsで書き換えれないため、別の変数に移す
    this.filteredLink = this.links
  },
  methods: {
    /**
     * カルテの起票月がアンケートの集計月まで且つ回答済み且つ公開OKのアンケートのテキストリンクを表示
     * @param survey
     */
    showsSurvey(survey: any) {
      const karteDate = new Date(this.karte.date)
      // 集計月初め
      const summaryMonth = new Date(survey.summaryMonth)

      if (summaryMonth <= karteDate) {
        // 回答済み且つ公開OK
        return survey.isFinished && survey.isDisclosure
      }
    },
    /**
     * ファイル選択時の動作
     * @param event イベント
     * @param index インデックス番号
     */
    onInput(event: any, index: number): void {
      this.files[index] = event
      let isUsedAllForms = true
      let nextIndex = 0
      for (const i in this.files) {
        if (!this.files[i]) {
          isUsedAllForms = false
        }
        nextIndex++
      }
      this.$set(this.files[index]!, 'uploadDatetime', getDateStr())

      this.$emit('update' + this.item, this.files)
      if (isUsedAllForms === true) {
        this.$set(this.files, nextIndex, null)
      }
    },
    /**
     * ファイルダウンロード時の動作
     * @param key ファイルパス
     * @param fileName ファイル名
     */
    download(key: string, fileName: string): void {
      downloadFile(key).then((res) => {
        const unit8Array = res.Body as Uint8Array
        const blob = new Blob([unit8Array], { type: res.ContentType })
        const url = (window.URL || window.webkitURL).createObjectURL(blob)
        const link = document.createElement('a')
        link.download = fileName
        link.href = url
        document.body.appendChild(link)
        link.click()
        document.body.removeChild(link)
      })
    },
    /**
     * ×を押下したときに画面上からファイルが非表示になる
     * @param index インデックス番号
     */
    clearFilesOnScreen(index: number): void {
      // 削除するファイルのS3上のパスを取得
      const key = this.links[index].path
      this.$emit('clearFilesOnScreen', key)

      // 画面上で選択したファイルを非表示
      this.filteredLink.splice(index, 1)
    },

    /**
     * アンケート名を生成する
     * @param {string} surveyType
     * @param {string} surveyDate
     * @returns 生成したアンケート名
     */
    createSurveyName(surveyType: string, surveyDate: string): string {
      if (surveyType === 'quick') {
        return (
          format(
            new Date(surveyDate),
            this.$t('common.format.date_ym') as string
          ) +
          ' ' +
          this.$t('survey.group_info.surveyNameList.quick')
        )
      } else if (surveyType === 'service') {
        return (
          format(
            new Date(surveyDate),
            this.$t('common.format.date_ym') as string
          ) +
          ' ' +
          this.$t('survey.group_info.surveyNameList.service')
        )
      } else if (surveyType === 'pp') {
        return (
          format(
            new Date(surveyDate),
            this.$t('common.format.date_ym') as string
          ) +
          ' ' +
          this.$t('survey.group_info.surveyNameList.pp')
        )
      } else {
        return (
          format(
            new Date(surveyDate),
            this.$t('common.format.date_ym') as string
          ) +
          ' ' +
          this.$t('survey.group_info.surveyNameList.completion')
        )
      }
    },
    /**
     * アクセス可能なアンケートかどうか確認する
     * @param survey アンケート
     * @returns boolean アクセス可能なアンケートかどうか
     */
    isAccessableSurvey(survey: SurveyListItem) {
      const isSupporterMgr = hasRole([ENUM_USER_ROLE.SUPPORTER_MGR])
      if (!isSupporterMgr) {
        return true
      }

      const organizationIds: string[] = []

      if (survey.mainSupporterUser !== null) {
        organizationIds.push(survey.mainSupporterUser.organizationId)
      }

      if (survey.supporterUsers !== null) {
        survey.supporterUsers.forEach((user) => {
          organizationIds.push(user.organizationId)
        })
      }

      const belongsTo = meStore.supporterOrganizations.find((organization) =>
        organizationIds.includes(organization.id)
      )

      return belongsTo
    },
  },
})
</script>

<style lang="scss" scoped>
.m-karter-aside-links__list {
  padding: 0;
  margin: 0;
}
.m-karter-aside-links__item {
  list-style: none;
  margin-top: 12px;
  padding-left: 15px;
  text-indent: -15px;
  a {
    @include fontSize($size: 'small');
    font-weight: bold;
    color: $c-primary-dark;
  }
}
.m-karter-aside-links__item--file-input {
  @extend .m-karter-aside-links__item;
  padding-left: 0;
  text-indent: 0;
}
</style>
