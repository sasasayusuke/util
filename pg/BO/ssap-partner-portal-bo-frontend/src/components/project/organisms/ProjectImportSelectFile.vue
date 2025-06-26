<template>
  <CommonContainer
    :title="$t('project.pages.import.name')"
    :is-editing="false"
    is-hide-button1
    is-hide-button2
    is-hide-footer
    hx="h1"
  >
    <Sheet class="pa-8" width="830">
      <Title hx="h2" style-set="detail">
        {{ $t('project.pages.import.lead') }}
        <ToolTips>{{ $t('customer.pages.import.help') }}</ToolTips>
      </Title>
      <FileInput
        v-model="file"
        class="mb-7"
        :placeholder="$t('common.placeholder.file_input')"
      />
      <Button :disabled="!file" style-set="large-primary" @click="upload">
        {{ $t('project.pages.import.importButton') }}
      </Button>
    </Sheet>
  </CommonContainer>
</template>

<script lang="ts">
import BaseComponent from '~/common/BaseComponent'
import CommonContainer from '~/components/common/organisms/CommonContainer.vue'
import {
  Sheet,
  Title,
  Button,
  FileInput,
  ToolTips,
} from '~/components/common/atoms/index'

import { S3_KEY_TEMPLATE, uploadFile } from '~/utils/upload'

/**
 * ファイル名から拡張子を返す
 * @param filename ファイル名
 * @returns ファイル拡張子
 */
function getExt(filename: string) {
  const pos = filename.lastIndexOf('.')
  if (pos === -1) return ''
  return filename.slice(pos + 1)
}

export default BaseComponent.extend({
  components: {
    CommonContainer,
    Sheet,
    Title,
    Button,
    FileInput,
    ToolTips,
  },
  data(): { file: File | undefined } {
    return {
      file: undefined,
    }
  },
  watch: {
    file(newFile) {
      this.checkFileExt(newFile)
    },
  },
  methods: {
    /** S3にファイルをアップロードし、確認画面に遷移 */
    async upload() {
      if (this.file) {
        const s3UploadResponse = await uploadFile(
          this.file,
          S3_KEY_TEMPLATE.IMPORT_CUSTOMER
        )
        const s3ObjectKey = s3UploadResponse.Key
        this.$router.push({
          path: '/project/import/confirm',
          query: { objectKey: s3ObjectKey },
        })
      }
    },
    /**
     * ファイルオブジェクトからCSVファイルが設定されているか確認
     * @param file 選択したファイルオブジェクト
     */
    checkFileExt(file: any) {
      if (file) {
        const ext = 'csv'
        if (String(getExt(file.name)).toLowerCase() !== ext) {
          this.file = undefined
        }
      }
    },
  },
})
</script>
