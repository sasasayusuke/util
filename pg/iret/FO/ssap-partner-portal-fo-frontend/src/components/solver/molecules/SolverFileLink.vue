<template>
  <div>
    <Icon size="12">icon-org-arrow</Icon>
    <a @click="download(file)">
      {{ getFileName(file) }}
    </a>
  </div>
</template>

<script lang="ts">
import { Icon } from '~/components/common/atoms/index'
import BaseComponent from '~/common/BaseComponent'
import { downloadFile } from '~/utils/download'
import { IS3File, ISolverFile } from '~/utils/upload'

export default BaseComponent.extend({
  name: '',
  components: {
    Icon,
  },
  props: {
    file: {
      type: Object as () => ISolverFile,
      required: true,
    },
  },
  data() {
    return {}
  },
  watch: {},
  methods: {
    // S3からファイルダウンロード
    download(fileLink: ISolverFile) {
      if (this.isS3File(fileLink)) {
        downloadFile((fileLink.file as IS3File).path).then((res) => {
          const part = res.Body as Uint8Array
          this.downloadBlob(
            (fileLink.file as IS3File).name,
            part,
            res.ContentType
          )
        })
      } else if (this.isLocalFile(fileLink)) {
        this.downloadBlob(
          (fileLink.file as File).name,
          fileLink.file as File,
          (fileLink.file as File).type
        )
      }
    },
    // ファイルダウンロード
    downloadBlob(
      name: string,
      blobPart?: File | Uint8Array | undefined,
      type?: any
    ) {
      if (!blobPart) {
        return
      }
      const blob = new Blob([blobPart], { type })
      const blobUrl = (window.URL || window.webkitURL).createObjectURL(blob)
      const link = document.createElement('a')
      link.href = blobUrl
      link.download = name
      link.click()
    },
    // ファイル名取得
    getFileName(fileLink: ISolverFile): string {
      if (this.isS3File(fileLink)) {
        return fileLink.file?.name || ''
      } else if (this.isLocalFile(fileLink)) {
        return fileLink.file?.name || ''
      } else {
        return ''
      }
    },

    // 保存済みファイルか判定
    isS3File(fileLink: ISolverFile): boolean {
      if (!fileLink.file) {
        return false
      }
      return fileLink.isSaved && 'path' in fileLink.file
    },

    // ローカルファイルか判定
    isLocalFile(fileLink: ISolverFile): boolean {
      if (!fileLink.file) {
        return false
      }
      return !fileLink.isSaved && 'name' in fileLink.file
    },
  },
})
</script>

<style lang="scss" scoped>
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
</style>
