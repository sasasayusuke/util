<template>
  <div>
    <ul v-if="!isEditing" class="solver-file__list">
      <!-- 登録済み添付ファイルリンク -->
      <li
        v-for="(file, index) in files"
        :key="index"
        :class="`solver-file__item ${index === 0 ? 'mt-0;' : 'mt-3'}`"
      >
        <div v-if="file.file">
          <SolverFileLink :file="file" />
        </div>
      </li>
    </ul>
    <ul v-if="isEditing" class="solver-file__list mt-3">
      <!-- 添付ファイルインプットフォーム -->
      <li
        v-for="(file, index) in files"
        :key="index"
        class="solver-file__item--file-input"
      >
        <FileInput
          :class="inputFileError ? 'error-input-solver-file' : ''"
          :value="file.file"
          accept=".txt, .pdf, .doc, .docx, .xls, .xlsx, .ppt, .pptx"
          :placeholder="$t('common.placeholder.file_input')"
          :allow-extensions="allowExtensions"
          :disabled="isDisabled"
          :additional-rules="additionalRules"
          truncate-length="50"
          @change="onChangeFile($event, index)"
          @click:clear="onChangeFile($event, index)"
        />
        <div class="mb-7" style="padding: 0 42px">
          <div
            v-if="inputFileError"
            class="v-messages theme--light error--text pt-1"
          >
            <div class="v-messages__message">
              {{ errorMessage }}
            </div>
          </div>
        </div>
      </li>
    </ul>
  </div>
</template>

<script lang="ts">
import { Icon, FileInput } from '~/components/common/atoms/index'
import BaseComponent from '~/common/BaseComponent'
import type { PropType } from '~/common/BaseComponent'
import { getDateStr, IS3File, ISolverFile } from '~/utils/upload'
import { FileContent } from '~/models/Solver'

export default BaseComponent.extend({
  name: '',
  components: {
    Icon,
    FileInput,
  },
  props: {
    // 登録済みファイル
    links: {
      type: Array as PropType<FileContent[]>,
      required: true,
    },
    isEditing: {
      type: Boolean,
    },
    isDisabled: {
      type: Boolean,
      default: false,
    },
    required: {
      type: Boolean,
      default: false,
    },
    refresh: {
      type: Boolean,
      default: false,
    },
    selectedFiles: {
      type: Array,
      default: () => [],
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
      ],
      additionalRules: [] as (string | boolean)[],
      inputFileError: false,
      errorMessage: '',
      files: [] as ISolverFile[],
    }
  },
  watch: {
    // 登録済みファイルが変更されたときの処理
    links: {
      handler() {
        const savedFiles = this.links.map((link, index) => {
          return {
            file: { name: link.fileName, path: link.path },
            isSaved: true,
            index,
          }
        })

        for (const file of savedFiles) {
          const index = this.files.findIndex(
            (f) =>
              f.file &&
              f.isSaved &&
              (file.file as IS3File).path === file.file.path
          )

          if (index >= 0) {
            file.index = this.files[index].index
          }
        }

        this.files = this.files
          .filter((file) => file && !file.isSaved)
          .concat(savedFiles)
        this.files.sort((a, b) => a.index - b.index)
        this.addEmpty()
        this.resetIndex()
      },
      immediate: true,
    },
    refresh(newValue) {
      if (newValue) {
        this.files = this.links.map((link, index) => {
          return {
            file: { name: link.fileName, path: link.path },
            isSaved: true,
            index,
          }
        })
        this.addEmpty()
      }
    },
    selectedFiles(newValue) {
      if (newValue !== null) {
        this.files = newValue.map((f: File, index: any) => {
          return {
            file: f,
            isSaved: false,
            index,
          }
        })
        this.addEmpty()
        this.resetIndex()
      }
    },
  },
  methods: {
    // ファイル選択時の処理
    onChangeFile(event: any, index: number): void {
      const targetIndex = this.files.findIndex(
        (f) => f.file && f.index === index
      )
      const targetFile = this.files[targetIndex]
      if (event && event instanceof File) {
        // ファイル追加
        const index = targetIndex >= 0 ? targetIndex : this.files.length - 1
        this.files[index] = {
          file: event,
          isSaved: false,
          index,
        }
        this.$set(this.files[index].file!, 'uploadDatetime', getDateStr())
      } else if (event == null && targetFile) {
        // ファイル削除
        this.files[index].file = null
      }

      // 対象ファイルが存在かつ、保存済みの場合は削除
      if (targetFile && targetFile.isSaved && targetFile.file) {
        const saveFileIndex = this.links.findIndex(
          (l) => l.path === (targetFile.file as IS3File).path
        )
        if (saveFileIndex >= 0) {
          this.links.splice(saveFileIndex, 1)
          // this.$emit('update:links', this.links)
        }
      }

      // 入力チェック
      this.verifyInputFiles(event)
      if (!this.inputFileError) {
        // 値を更新
        this.updateFiles()
        // 空行を追加
        this.addEmpty()
        this.resetIndex()
      }
    },
    updateFiles(): void {
      this.$emit(
        'change',
        this.files.filter((f) => f.file && !f.isSaved).map((f) => f.file)
      )
      this.resetIndex()
    },
    // インプットファイルチェック
    verifyInputFiles(event: any): boolean {
      // 合計ファイルサイズチェック
      const newFiles = this.files
        .filter((file) => !file.isSaved)
        .map((file) => file.file)
      const totalSize = newFiles.reduce(
        (sum, file: any) => sum + (file ? file.size : 0),
        0
      )
      if (event && totalSize > 20 * 1024 * 1024) {
        this.inputFileError = true
        this.errorMessage = this.$t('solver.row.file.errorMessage') as string
      } else {
        this.inputFileError = false
        this.errorMessage = ''
      }

      // 必須チェック
      if (this.required === true && !this.inputFileError) {
        if (this.files.filter((f) => f.file).length === 0) {
          this.inputFileError = true
          this.errorMessage = this.$t('common.rule.required') as string
        } else {
          this.inputFileError = false
          this.errorMessage = ''
        }
      }

      if (this.inputFileError) {
        this.additionalRules.push(false)
      } else {
        this.$children.forEach((child: any) => (child.rules = []))
        this.additionalRules = []
      }
      return this.inputFileError
    },

    // 空行追加
    addEmpty() {
      this.files = this.files.filter((f) => f.file)
      if (this.inputFileError && this.files.length > 0) {
        return
      }
      this.files.push({
        file: null,
        isSaved: false,
        index: this.files.length,
      })
    },

    // 保持データのインデックスをリセット
    resetIndex() {
      let index = 0
      for (const file of this.files) {
        file.index = index
        index += 1
      }
    },
  },
})
</script>

<style lang="scss" scoped>
.solver-file__list {
  padding: 0;
  margin: 0;
}
.solver-file__item {
  list-style: none;
  padding-left: 15px;
  text-indent: -15px;
  a {
    @include fontSize($size: 'small');
    font-weight: bold;
    color: $c-primary-dark;
  }
}
.solver-file__item--file-input {
  @extend .solver-file__item;
  padding-left: 0;
  text-indent: 0;
}
</style>
<style lang="scss">
.error-input-solver-file {
  fieldset {
    border-color: #d53030 !important;
  }

  .mdi-paperclip {
    color: #d53030 !important;
  }
}
</style>
