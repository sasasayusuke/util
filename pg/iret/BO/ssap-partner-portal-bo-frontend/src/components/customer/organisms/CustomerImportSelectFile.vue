<template>
  <CommonContainer
    :title="$t('customer.pages.import.name')"
    :is-editing="false"
    is-hide-button1
    is-hide-button2
    is-hide-footer
    note-head="false"
    hx="h1"
  >
    <Sheet class="pa-8" width="830">
      <Title hx="h2" style-set="detail">
        {{ $t('customer.pages.import.lead') }}
        <ToolTips>{{ $t('customer.pages.import.help') }}</ToolTips>
      </Title>
      <FileInput
        v-model="file"
        class="mb-7"
        :placeholder="$t('common.placeholder.file_input')"
      />
      <Button :disabled="!file" style-set="large-primary" @click="upload">
        {{ $t('customer.pages.import.importButton') }}
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
    /**
     * 顧客情報をアップロード
     */
    async upload() {
      if (this.file) {
        const s3UploadResponse = await uploadFile(
          this.file,
          S3_KEY_TEMPLATE.IMPORT_CUSTOMER
        )

        const s3ObjectKey = s3UploadResponse.Key

        this.$router.push({
          path: '/customer/import/confirm',
          query: { objectKey: s3ObjectKey },
        })
      }
    },
    /**
     * ファイルの拡張子を確認
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
