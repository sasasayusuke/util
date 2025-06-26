<template>
  <div>
    <div class="upload-container">
      <div>
        <div
          v-if="isEditing"
          class="drop-area"
          :class="{
            'drop-active': isDragging && !isDisabled,
            'drop-area-error': isError,
          }"
          @dragover.prevent
          @dragenter.prevent="isDragging = true"
          @dragleave.prevent="isDragging = false"
          @drop.prevent="onDrop"
        >
          <div v-if="(!isDragging && !image) || isDisabled">
            <Icon v-if="!image" size="35" color="#666">icon-org-image</Icon>
            <div v-if="!image" class="pt-4">
              {{ $t('solver.row.photo.inputDescription1') }}<br />or<br />{{
                $t('solver.row.photo.inputDescription2')
              }}
            </div>
          </div>
          <img v-if="image" :src="image" class="image-preview" />
          <div v-if="isDragging && !isDisabled">
            {{ $t('solver.row.photo.inputDescription3') }}
          </div>
        </div>
        <v-input
          ref="inputRef"
          :value="image"
          :rules="validationCheck"
          :class="{ 'd-none': !isError }"
        />
      </div>
      <div v-if="!isEditing && image" class="image-area">
        <img :src="image" class="image-preview" />
      </div>
      <div v-if="isEditing" class="button-column">
        <input ref="fileInput" type="file" hidden @change="onFileChange" />
        <Button
          :disabled="isDisabled"
          style-set="normal-primary"
          @click="selectFile"
        >
          {{ $t('common.button.file_input') }}
        </Button>
        <Button
          outlined
          :disabled="!image || isDisabled"
          style-set="normal-error"
          @click="removeImage"
        >
          {{ $t('common.button.delete') }}
        </Button>
      </div>
      <div v-if="!isNew && !isEditing && image">
        <Button
          style-set="normal-primary"
          @click="downloadImageToLocal(solverImage.path, solverImage.fileName)"
        >
          {{ $t('solver.row.photo.download') }}
        </Button>
      </div>
    </div>
    <div v-if="isEditing && isInputError">
      <div class="v-messages theme--light error--text pt-1">
        <div class="v-messages__message">
          {{ fileErrorMessage }}
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import BaseComponent, { PropType } from '~/common/BaseComponent'
import { Icon } from '~/components/common/atoms/index'
import { Button } from '~/components/common/atoms'
import { getDateStr } from '~/utils/upload'
import { downloadFile } from '~/utils/download'

interface SolverImage {
  fileName: string
  path: string
}

export default BaseComponent.extend({
  name: 'SolverImage',
  components: {
    Icon,
    Button,
  },
  props: {
    solverImage: {
      type: Object as PropType<SolverImage>,
      required: true,
      default: { fileName: '', path: '' },
    },
    isInitialValid: {
      type: Boolean,
      required: false,
    },
    isEditing: {
      type: Boolean,
      default: true,
    },
    isDisabled: {
      type: Boolean,
      default: false,
    },
    isNew: {
      type: Boolean,
      default: true,
    },
    required: {
      type: Boolean,
      default: false,
    },
    selectedImage: {
      default: null,
    },
  },
  data(): {
    image: string | ArrayBuffer | null
    isDragging: boolean
    isError: boolean
    isInputError: boolean
    fileErrorMessage: string
  } {
    return {
      image: null,
      isDragging: false,
      isError: false,
      isInputError: false,
      fileErrorMessage: '',
    }
  },
  watch: {
    // 登録済みの画像をダウンロードする処理
    solverImage(newValue) {
      if (newValue) {
        this.downloadImage(newValue.path)
      }
    },
    // バリデーションエラーメッセージの表示非表示を管理する処理
    image(newValue) {
      if (newValue || !this.required) {
        this.isError = false
      } else {
        this.isError = true
      }
      if (!this.required) {
        this.isError = false
      }
    },
    selectedImage(newValue) {
      if (newValue !== null) {
        const reader = new FileReader()
        reader.onload = (e) => {
          if (e.target) {
            this.image = e.target.result
          }
        }
        reader.readAsDataURL(newValue)
      }
    },
  },
  computed: {
    validationCheck(): Function[] {
      const rules: Function[] = []
      //必須
      if (this.required === true) {
        const rule = (value: any) => !!value || this.$t('common.rule.required')
        rules.push(rule)
      }
      return rules
    },
  },
  mounted() {
    if (this.isInitialValid) {
      const inputRef = this.$refs.inputRef as any
      this.isError = !inputRef.validate()
    }
  },
  methods: {
    // ファイルを選択した時の処理
    onFileChange(event: Event) {
      const input = event.target as HTMLInputElement
      const file = input.files ? input.files[0] : null
      if (file && this.isValidInputFile(file)) {
        this.setImage(file)
      }
    },

    // ドラッグ＆ドロップでファイルをドロップした時の処理
    onDrop(event: DragEvent) {
      this.isDragging = false
      const file = event.dataTransfer?.files[0]
      if (file && this.isValidInputFile(file) && !this.isDisabled) {
        this.setImage(file)
      }
    },

    // インプットファイルを検証する処理
    isValidInputFile(file: File): boolean {
      if (file.type !== 'image/jpeg' && file.type !== 'image/png') {
        this.isInputError = true
        this.fileErrorMessage = this.$t('common.rule.fileExtension', {
          extension: 'jpg/peg, png',
        }) as string
        return false
      } else if (file.size > 2 * 1024 * 1024) {
        this.isInputError = true
        this.fileErrorMessage = this.$t('common.rule.fileSize', {
          size: 2,
          unit: 'MB',
        }) as string
        return false
      } else {
        this.isInputError = false
        this.fileErrorMessage = ''
        return true
      }
    },

    // 画像をセットする処理
    setImage(file: Blob) {
      const reader = new FileReader()
      reader.onload = (e) => {
        if (e.target) {
          this.image = e.target.result
        }
      }
      reader.readAsDataURL(file)
      this.$set(file!, 'uploadDatetime', getDateStr())
      this.$emit('change', file)
    },

    // ファイル選択ボタンをクリックした時の処理
    selectFile(event: MouseEvent) {
      event.preventDefault()
      const fileInput = this.$refs.fileInput as HTMLInputElement | null
      if (fileInput) {
        fileInput.click()
      }
    },

    // 画像を削除する処理
    removeImage() {
      const fileInput = this.$refs.fileInput as HTMLInputElement
      this.image = null
      fileInput.value = ''
      this.isInputError = false
      this.fileErrorMessage = ''
      this.$emit('change')
    },

    // S3から画像をダウンロードする処理
    downloadImage(key: string) {
      if (key) {
        downloadFile(key)
          .then((file: any) => {
            if (file.Body) {
              const downloadedImage = new Blob([file.Body], {
                type: file.ContentType,
              })
              this.setImage(downloadedImage)
            }
          })
          .catch((err) => {
            this.removeImage()
            throw new Error(err)
          })
      }
    },

    // S3から画像をローカルにダウンロードする処理
    downloadImageToLocal(key: string, fileName: string) {
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
  },
})
</script>

<style lang="scss" scoped>
.v-file-input {
  max-width: 300px;
}

.upload-container {
  display: flex;
  align-items: flex-start;
  justify-content: flex-start;
  gap: 20px;
}

.button-column {
  display: flex;
  flex-direction: column;
  align-items: flex-start;
  gap: 10px;
}

.drop-area {
  width: 180px;
  height: 180px;
  border: 2px dashed #ccc;
  display: flex;
  align-items: center;
  justify-content: center;
  text-align: center;
  font-size: 0.75rem;
  font-weight: bold;
  color: #555;
  background-color: #f3f3f3;
  transition: background-color 0.3s;
}

.drop-area-error {
  border: 2px dashed #d53030;
  background-color: #faeded;
}

.drop-active {
  border: 2px dashed #aebfdc;
  background-color: #ebf1fa;
}

.image-preview {
  max-width: 100%;
  max-height: 100%;
}

.image-area {
  width: 180px;
  height: 180px;
  display: flex;
  align-items: center;
  justify-content: center;
  text-align: center;
}
</style>
