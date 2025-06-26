<!-- v-for="question in surveyMaster.questions"で、設問コンテナを量産する -->
<template>
  <RootTemplate>
    <DetailInPageHeader
      v-if="!isDraft"
      :back-to-detail="`/survey/master/${$route.params.surveyMasterId}`"
      class="o-survey-master-detail-title"
    >
      {{ pageName }}
    </DetailInPageHeader>
    <DetailInPageHeader
      v-else-if="!isEditing"
      :is-valid="isValidDetailWithChanged"
      :is-loading-button="isLoadingButton"
      header-buttons
      class="o-survey-master-detail-title"
      @buttonAction1="saveAfterList(localSurveyMaster)"
      @buttonAction2="backToList"
    >
      {{ pageName }}
    </DetailInPageHeader>
    <CommonUpdateRow
      v-if="!isEditing"
      :name="localSurveyMaster.updateUserName"
      :date="formatDate(new Date(localSurveyMaster.updateAt))"
      class="pt-3"
    />

    <!-- バージョン情報 -->
    <SurveyMasterVersionTable
      v-if="!isEditing"
      v-model="isValidDetail"
      :survey-master="localSurveyMaster"
      :is-editing="isEditing"
      :is-draft="isDraft"
      :index="selectIdx"
      @update="emitSurveyMasterDetail"
    />

    <!-- 設問コンテナ -->
    <template v-for="(question, index) in localSurveyMaster.questions">
      <DetailContainer
        v-if="question.disabled === false && !newQuestion"
        :key="question.id"
        :title="sectionName()"
        :is-editing="isEditing"
        :is-draft="isDraft"
        :is-valid="isValidQuestionWithChanged(index)"
        :is-new="question.isNew"
        :is-loading-button="isLoadingButton"
        is-hide-button1
        is-hide-button2
        is-hide-footer
        class="mt-6 pb-10"
        :class="{
          isHidden: isEditing,
          isEditable: index === selectIdx,
        }"
        @click:positive="
          isEditing ? saveAfterQuestions(localSurveyMaster) : edit(index)
        "
        @click:negative="
          isEditing
            ? backToQuestions()
            : question.isNew
            ? deleted(index)
            : disabled(index)
        "
        @buttonAction3="up(index)"
        @buttonAction4="down(index)"
      >
        <SurveyMasterQuestionTable
          v-model="isValidQuestion[index]"
          :survey-master="localSurveyMaster"
          :snap-survey-master="snapSurveyMaster"
          :is-editing="isEditing"
          :is-creating="isCreating"
          :is-draft="isDraft"
          :index="index"
          :required-choice-count="requiredChoiceCount(index)"
          @update="emitSurveyMasterQuestion"
        />
        <template #footerButton>
          <Sheet class="d-flex justify-center pt-6" width="100%">
            <Button
              style-set="large-tertiary"
              outlined
              width="160"
              class="mr-4"
              @click="isEditing ? backToQuestions() : disabled(index)"
            >
              {{ $t('common.button.cancel') }}
            </Button>
            <Button
              style-set="large-primary"
              width="160"
              :disabled="isValidQuestionWithChanged(index) !== true"
              :loading="isLoadingButton"
              @click="
                isEditing ? saveAfterQuestions(localSurveyMaster) : edit(index)
              "
            >
              {{ $t('common.button.save') }}
            </Button>
          </Sheet>
        </template>
      </DetailContainer>
    </template>

    <!-- 設問追加を押した時に表示する空の設問編集 -->
    <DetailContainer
      v-if="newQuestion"
      :title="sectionName()"
      :is-editing="isEditing"
      :is-draft="isDraft"
      :is-valid="isValidQuestionWithChanged(selectIdx)"
      :is-loading-button="isLoadingButton"
      is-hide-button1
      is-hide-button2
      is-hide-footer
      class="mt-6 pb-10"
      @click:positive="saveAfterQuestions(localSurveyMaster)"
      @click:negative="backToQuestions()"
    >
      <SurveyMasterQuestionTable
        v-model="isValidQuestion[selectIdx]"
        :survey-master="localSurveyMaster"
        :snap-survey-master="snapSurveyMaster"
        :is-editing="isEditing"
        :is-draft="isDraft"
        :index="selectIdx"
        :required-choice-count="requiredChoiceCount(selectIdx)"
        @update="emitSurveyMasterQuestion"
      />
    </DetailContainer>

    <SurveyRevisionFooter
      v-if="!isEditing"
      :is-draft="isDraft"
      :is-valid="isValidDetailWithChanged"
      @click:add="add"
      @click:back="backToList"
      @click:cancel="backToList"
      @click:save="saveAfterList(localSurveyMaster)"
    />

    <!-- 無効の設問 -->
    <template v-if="!isEditing">
      <Heading v-if="!isEditing" :level="3" class="mt-12">
        <template #title>
          {{ $t('survey.pages.revision.table.disabled.name') }}
        </template>
      </Heading>

      <template v-if="localSurveyMaster.questions.length !== 0">
        <!-- 表示ボタン -->
        <v-btn v-if="!expand" block color="white" class="mt-4" @click="open">
          <Icon size="16" class="mr-2">icon-org-arrow-down</Icon
          >{{ $t('common.button.show') }}
        </v-btn>

        <!-- 無効の設問コンテナ -->
        <template v-for="(question, index) in localSurveyMaster.questions">
          <DetailContainer
            v-if="question.disabled === true && expand"
            :key="question.id"
            :title="$t('survey.pages.revision.table.survey.title')"
            :is-editing="isEditing"
            :is-draft="isDraft"
            :is-valid="isValidQuestionWithChanged(index)"
            :is-invalid="isInvalid"
            is-hide-button1
            is-hide-button2
            is-hide-footer
            :is-disabled-button3="isDenyActivation(index)"
            class="o-survey-question-table-invalid pb-10"
            @buttonAction3="enabled(index)"
          >
            <SurveyMasterQuestionTable
              v-model="isValidQuestion[index]"
              :survey-master="localSurveyMaster"
              :snap-survey-master="snapSurveyMaster"
              :is-editing="isEditing"
              :is-creating="isCreating"
              :is-draft="isDraft"
              :index="index"
              :required-choice-count="requiredChoiceCount(index)"
              @update="emitSurveyMasterQuestion"
            />
          </DetailContainer>
        </template>

        <!-- 閉じるボタン -->
        <v-btn
          v-if="expand"
          block
          color="white"
          class="t-button-close mt-6"
          @click="close"
        >
          <Icon size="16" class="mr-2">icon-org-arrow-down</Icon
          >{{ $t('common.button.hidden') }}
        </v-btn>
      </template>
      <Alert v-else style-set="no_data">
        {{ $t('common.alert.noData') }}
      </Alert>
    </template>
  </RootTemplate>
</template>

<script lang="ts">
import cloneDeep from 'lodash/cloneDeep'
import CommonUpdateRow from '../../common/molecules/CommonUpdateRow.vue'
import DetailInPageHeader from '~/components/common/organisms/DetailInPageHeader.vue'
import CommonDetail from '~/components/common/templates/CommonDetailTemplate.vue'
import type { PropType } from '~/common/BaseComponent'
import RootTemplate from '~/components/common/bases/RootTemplate.vue'
import {
  Alert,
  Icon,
  TextField,
  Button,
  Sheet,
} from '~/components/common/atoms/index'
import DetailContainer from '~/components/common/organisms/DetailContainer.vue'
import SurveyRevisionFooter from '~/components/survey/organisms/SurveyRevisionFooter.vue'
import Heading from '~/components/common/molecules/Heading.vue'
import SurveyMasterQuestionTable, {
  LocalSurveyMaster,
} from '~/components/survey/molecules/SurveyMasterQuestionTable.vue'
import SurveyMasterVersionTable from '~/components/survey/molecules/SurveyMasterVersionTable.vue'
import {
  SurveyMasterChoiceGroupItem,
  SurveyMasterChoiceItem,
  SurveyMasterQuestionItem,
  SurveyMasterQuestionFlowItem,
  GetSurveyMasterByIdAndRevisionResponse,
  UpdateSurveyMasterDraftByIdRequest,
  UpdateSurveyMasterDraftById,
} from '~/models/Master'

const { v4: uuidv4 } = require('uuid')

export default CommonDetail.extend({
  components: {
    Icon,
    TextField,
    Button,
    Alert,
    Sheet,
    CommonUpdateRow,
    DetailInPageHeader,
    RootTemplate,
    DetailContainer,
    SurveyRevisionFooter,
    Heading,
    SurveyMasterQuestionTable,
    SurveyMasterVersionTable,
  },
  props: {
    /** アンケートマスター */
    surveyMaster: {
      type: Object as PropType<GetSurveyMasterByIdAndRevisionResponse>,
      required: true,
    },
    /** 各設問のバリデーション状態 */
    questionIndexes: {
      type: Array as PropType<Boolean[]>,
      required: true,
    },
  },
  data() {
    return {
      headerPageName: this.$t('customer.group_info.name'),
      pageName: this.$t('survey.pages.revision.name'),
      isEditing: false,
      isCreating: false,
      isDraft: false,
      isInvalid: false,
      isValidDetail: false,
      isChangedDetail: false,
      isValidQuestion: cloneDeep(this.questionIndexes),
      isChangedQuestion: cloneDeep(this.questionIndexes),
      expand: false,
      localSurveyMaster: this.surveyMaster,
      selectIdx: 0,
      newQuestion: false,
      requiredChoiceCounts: {
        checkbox: 1,
        radio: 2,
        others: 0,
      },
      beforeIndexAttribute: '',
      snapSurveyMaster: cloneDeep(this.surveyMaster),
      isLoadingButton: false,
    }
  },
  methods: {
    /**
     * 表示タイトルを返す
     * @returns 表示タイトル文字列
     */
    sectionName() {
      return this.isEditing
        ? this.$t('survey.pages.revision.table.survey.edit')
        : this.$t('survey.pages.revision.table.survey.title')
    },
    /**
     * 指定した設問を編集する
     * @param index 設問インデックス番号
     */
    edit(index: number) {
      this.clearErrorBar()
      const indexAttributes = this.getIndexAttributes()
      if (this.newQuestion) {
        this.newQuestion = false
      }
      this.isEditing = !this.isEditing
      if (this.isEditing === true) {
        this.isChangedQuestion[index] = false
        // 設問編集画面を開いた時点の状態を保持
        this.snapSurveyMaster = cloneDeep(this.localSurveyMaster)
      }
      this.selectIdx = index
      this.beforeIndexAttribute = indexAttributes[this.selectIdx]
    },
    /** 設問を追加する */
    add() {
      this.clearErrorBar()
      const indexAttributes = this.getIndexAttributes()
      const question = new SurveyMasterQuestionItem()
      const questionId = uuidv4()
      question.id = questionId
      question.isNew = true
      question.required = false
      question.format = 'checkbox'
      question.summaryType = 'normal'
      const choiceItem = new SurveyMasterChoiceItem()
      choiceItem.isNew = true
      const choiceGroupItem = new SurveyMasterChoiceGroupItem()
      choiceGroupItem.id = uuidv4()
      choiceGroupItem.isNew = true
      choiceItem.group.push(choiceGroupItem)
      question.choices.push(choiceItem)
      const questionFlow = new SurveyMasterQuestionFlowItem()
      questionFlow.id = questionId
      this.localSurveyMaster.questions.push(question)
      this.localSurveyMaster.questionFlow.push(questionFlow)
      this.selectIdx = this.localSurveyMaster.questions.length - 1
      this.isEditing = true
      this.newQuestion = true
      this.$set(this.isChangedQuestion, this.selectIdx, false)
      this.beforeIndexAttribute = indexAttributes[this.selectIdx]
    },
    /** アンケートひな形詳細に戻る */
    backToList() {
      this.clearErrorBar()
      const surveyMasterId: string = this.$route.params.surveyMasterId as string
      this.$router.push(`/survey/master/${surveyMasterId}`)
    },
    /** アンケートひな形設問詳細に戻る */
    backToQuestions() {
      this.clearErrorBar()
      if (this.isEditing) {
        this.isEditing = false
      }
      if (this.newQuestion) {
        this.newQuestion = false
        this.localSurveyMaster.questions.splice(this.selectIdx, 1)
        this.localSurveyMaster.questionFlow.splice(this.selectIdx, 1)
      }
      this.refresh()
    },
    /**
     * アンケートひな形設問詳細を保存しアンケートひな形詳細に戻る
     * @param localSurveyMaster 入力中の全てのアンケートマスター情報
     */
    saveAfterList(localSurveyMaster: LocalSurveyMaster) {
      this.isLoadingButton = true
      this.update(localSurveyMaster)
        .then(() => {
          this.backToList()
        })
        .catch(() => {
          this.showErrorBarWithScrollPageTop(this.$t('msg.error.default'))
        })
        .finally(() => {
          this.isLoadingButton = false
        })
    },
    /**
     * アンケートひな形設問詳細を保存しアンケートひな形設問詳細にに戻る
     * @param localSurveyMaster 入力中の全てのアンケートマスター情報
     */
    saveAfterQuestions(localSurveyMaster: LocalSurveyMaster) {
      this.isLoadingButton = true
      //グループ内の子要素が通常の設問に変更された場合は元の所属グループの次に強制移動しグループ外に放出
      const indexAttributes = this.getIndexAttributes()
      const currentIndexAttribute = indexAttributes[this.selectIdx]
      if (
        this.beforeIndexAttribute === 'child' &&
        currentIndexAttribute === 'neutral'
      ) {
        // 自動的にグループ外の下に移動
        let currentIndex = this.selectIdx
        let nextIndex = this.selectIdx + 1
        while (true) {
          if (indexAttributes[nextIndex]) {
            if (indexAttributes[nextIndex] === 'child') {
              this.down(currentIndex)
              nextIndex++
              currentIndex++
            } else {
              break
            }
          } else {
            break
          }
        }
      }
      this.update(localSurveyMaster)
        .then(() => {
          this.newQuestion = false
          this.backToQuestions()
        })
        .catch(() => {
          this.showErrorBarWithScrollPageTop(this.$t('msg.error.default'))
        })
        .finally(() => {
          this.isLoadingButton = false
        })
    },
    open() {
      this.expand = true
    },
    close() {
      this.expand = false
    },
    /**
     * 指定した設問を有効化する
     * @param index 設問インデックス番号
     */
    enabled(index: number) {
      const indexAttributes = this.getIndexAttributes()
      const indexAttribute = indexAttributes[index]
      if (indexAttribute === 'parent') {
        const groupIndexes = this.getIndexGroupByParent(index, indexAttributes)
        for (const i in groupIndexes) {
          const groupIndex = groupIndexes[i]
          this.localSurveyMaster.questions[groupIndex].disabled = false
        }
      } else if (indexAttribute === 'child') {
        const groupIndexes = this.getIndexGroupByChild(index, indexAttributes)
        const groupParentIndex = groupIndexes[0]
        this.localSurveyMaster.questions[groupParentIndex].disabled = false
        this.localSurveyMaster.questions[index].disabled = false
      } else {
        this.localSurveyMaster.questions[index].disabled = false
      }
      this.isInvalid = false
      for (const key in this.localSurveyMaster.questions) {
        if (this.localSurveyMaster.questions[key].disabled === true) {
          this.isInvalid = true
        }
      }
      this.isChangedDetail = true
    },
    /**
     * 指定した設問を削除化する
     * @param index 設問インデックス番号
     */
    deleted(index: number) {
      this.localSurveyMaster.questions =
        this.localSurveyMaster.questions.filter((_, i) => i !== index)
      this.isChangedDetail = true
    },
    /**
     * 指定した設問を無効化する
     * @param index 設問インデックス番号
     */
    disabled(index: number) {
      const indexAttributes = this.getIndexAttributes()
      const indexAttribute = indexAttributes[index]
      if (indexAttribute === 'parent') {
        const groupIndexes = this.getIndexGroupByParent(index, indexAttributes)
        for (const i in groupIndexes) {
          const groupIndex = groupIndexes[i]
          this.localSurveyMaster.questions[groupIndex].disabled = true
        }
      } else {
        this.localSurveyMaster.questions[index].disabled = true
      }
      this.isInvalid = false
      for (const key in this.localSurveyMaster.questions) {
        if (this.localSurveyMaster.questions[key].disabled === true) {
          this.isInvalid = true
        }
      }
      this.isChangedDetail = true
    },
    /**
     * 現在の全設問区分(通常設問/条件元設問/条件設問)を返す
     * @returns 設問区分文字列配列
     */
    getIndexAttributes() {
      const results: any = []
      for (const i in this.localSurveyMaster.questions) {
        const id = String(this.localSurveyMaster.questions[i].id)
        const parentConditionId = String(
          this.localSurveyMaster.questionFlow[i].conditionId
        )
        let isChild = false
        if (parentConditionId !== '') {
          for (const i2 in this.localSurveyMaster.questions) {
            if (this.localSurveyMaster.questions[i2].id === parentConditionId) {
              isChild = true
              break
            }
          }
        }
        let isParent = false
        if (isChild === false) {
          for (const i2 in this.localSurveyMaster.questionFlow) {
            if (
              String(this.localSurveyMaster.questionFlow[i2].conditionId) === id
            ) {
              isParent = true
              break
            }
          }
        }
        if (isChild === false && isParent === false) {
          results[i] = 'neutral'
        } else if (isParent === true) {
          results[i] = 'parent'
        } else if (isChild === true) {
          results[i] = 'child'
        }
      }
      return results
    },
    /**
     * 条件元設問のインデックス番号から条件設問のインデックス番号配列を取得
     * @param parentIndex 条件元設問のインデックス番号
     * @param indexAttributes 設問区分文字列配列
     * @returns 条件設問のインデックス番号配列
     */
    getIndexGroupByParent(parentIndex: number, indexAttributes: any) {
      const indexGroup: number[] = []
      indexGroup.push(parentIndex)
      let nextIndex: number = Number(parentIndex) + 1
      while (true) {
        if (indexAttributes[nextIndex]) {
          if (indexAttributes[nextIndex] === 'child') {
            indexGroup.push(nextIndex)
            nextIndex++
          } else {
            break
          }
        } else {
          break
        }
      }
      return indexGroup
    },
    /**
     * 条件設問のインデックス番号から条件元設問および条件設問のインデックス番号配列を取得
     * @param childIndex 条件設問のインデックス番号
     * @param indexAttributes 設問区分文字列配列
     * @returns 条件元設問および条件設問のインデックス番号配列
     */
    getIndexGroupByChild(childIndex: number, indexAttributes: any) {
      let indexGroup: number[] = []
      let nextIndex: number = Number(childIndex) - 1
      let parentIndex = -1
      while (true) {
        if (indexAttributes[nextIndex]) {
          if (indexAttributes[nextIndex] === 'parent') {
            parentIndex = nextIndex
            break
          } else {
            nextIndex--
          }
        } else {
          break
        }
      }
      if (parentIndex !== -1) {
        indexGroup = this.getIndexGroupByParent(parentIndex, indexAttributes)
      }
      return indexGroup
    },
    /**
     * 条件元設問を上に移動
     * @param parentIndex 条件元設問のインデックス番号
     * @param toIndex 移動先インデックス番号
     * @param indexAttributes 設問区分文字列配列
     */
    upParent(parentIndex: number, toIndex: number, indexAttributes: any) {
      const indexGroup = this.getIndexGroupByParent(
        parentIndex,
        indexAttributes
      ).reverse()
      const lastChildIndex = indexGroup[0]
      const toIndexAttribute = indexAttributes[toIndex]
      if (toIndexAttribute === 'child') {
        const nextGroupIndex = this.getIndexGroupByChild(
          toIndex,
          indexAttributes
        )
        const nextParentToIndex = nextGroupIndex[0]
        indexGroup.forEach(() => {
          this.reorderQuestions(lastChildIndex, nextParentToIndex)
        })
      } else {
        indexGroup.forEach(() => {
          this.reorderQuestions(lastChildIndex, toIndex)
        })
      }
    },
    /**
     * 条件元設問を下に移動
     * @param parentIndex 条件元設問のインデックス番号
     * @param toIndex 移動先インデックス番号
     * @param indexAttributes 設問区分文字列配列
     */
    // eslint-disable-next-line @typescript-eslint/no-unused-vars
    downParent(parentIndex: number, toIndex: number, indexAttributes: any) {
      let indexGroup = this.getIndexGroupByParent(
        parentIndex,
        indexAttributes
      ).reverse()
      const lastChildFromIndex = indexGroup[0]
      let lastChildToIndex = lastChildFromIndex + 1
      const lastChildToIndexAttribute = indexAttributes[lastChildToIndex]
        ? indexAttributes[lastChildToIndex]
        : 'nothing'
      if (lastChildToIndexAttribute === 'parent') {
        const nextGroupIndex = this.getIndexGroupByParent(
          lastChildToIndex,
          indexAttributes
        ).reverse()
        lastChildToIndex = nextGroupIndex[0]
      }
      indexGroup = indexGroup.reverse()
      indexGroup.forEach(() => {
        this.reorderQuestions(parentIndex, lastChildToIndex)
      })
    },
    /**
     * 条件設問を上に移動
     * @param childIndex 条件設問のインデックス番号
     * @param toIndex 移動先インデックス番号
     * @param indexAttributes 設問区分文字列配列
     */
    upChild(childIndex: number, toIndex: number, indexAttributes: any) {
      const indexGroup = this.getIndexGroupByChild(childIndex, indexAttributes)
      indexGroup.shift()
      let boolReorder = false
      for (const i in indexGroup) {
        if (Number(toIndex) === Number(indexGroup[i])) {
          boolReorder = true
        }
      }
      if (boolReorder === true) {
        this.reorderQuestions(childIndex, toIndex)
      }
    },
    /**
     * 条件設問を下に移動
     * @param childIndex 条件設問のインデックス番号
     * @param toIndex 移動先インデックス番号
     * @param indexAttributes 設問区分文字列配列
     */
    downChild(childIndex: number, toIndex: number, indexAttributes: any) {
      const indexGroup = this.getIndexGroupByChild(childIndex, indexAttributes)
      indexGroup.shift()
      let boolReorder = false
      for (const i in indexGroup) {
        if (Number(toIndex) === Number(indexGroup[i])) {
          boolReorder = true
        }
      }
      if (boolReorder === true) {
        this.reorderQuestions(childIndex, toIndex)
      }
    },
    /**
     * 通常設問を上に移動
     * @param neutralIndex 通常設問のインデックス番号
     * @param toIndex 移動先インデックス番号
     * @param indexAttributes 設問区分文字列配列
     */
    upNeutral(neutralIndex: number, toIndex: number, indexAttributes: any) {
      const toIndexAttribute = indexAttributes[toIndex]
      if (toIndexAttribute === 'child') {
        const nextGroupIndex = this.getIndexGroupByChild(
          toIndex,
          indexAttributes
        )
        const nextParentToIndex = nextGroupIndex[0]
        this.reorderQuestions(neutralIndex, nextParentToIndex)
      } else {
        this.reorderQuestions(neutralIndex, toIndex)
      }
    },
    /**
     * 通常設問を下に移動
     * @param neutralIndex 通常設問のインデックス番号
     * @param toIndex 移動先インデックス番号
     * @param indexAttributes 設問区分文字列配列
     */
    downNeutral(neutralIndex: number, toIndex: number, indexAttributes: any) {
      const toIndexAttribute = indexAttributes[toIndex]
      if (toIndexAttribute === 'parent') {
        const nextGroupIndex = this.getIndexGroupByParent(
          toIndex,
          indexAttributes
        ).reverse()
        const lastChildToIndex = nextGroupIndex[0]
        this.reorderQuestions(neutralIndex, lastChildToIndex)
      } else {
        this.reorderQuestions(neutralIndex, toIndex)
      }
    },
    /**
     * インデックス番号から設問区分を割り出し設問区分に応じて上に移動
     * @param fromIndex インデックス番号
     */
    up(fromIndex: number) {
      const toIndex = fromIndex - 1
      const indexAttributes = this.getIndexAttributes()
      const fromIndexAttribute = indexAttributes[fromIndex]
      if (fromIndexAttribute === 'neutral') {
        this.upNeutral(fromIndex, toIndex, indexAttributes)
      } else if (fromIndexAttribute === 'parent') {
        this.upParent(fromIndex, toIndex, indexAttributes)
      } else if (fromIndexAttribute === 'child') {
        this.upChild(fromIndex, toIndex, indexAttributes)
      }
      this.isChangedDetail = true
    },
    /**
     * インデックス番号から設問区分を割り出し設問区分に応じて下に移動
     * @param fromIndex インデックス番号
     */
    down(fromIndex: number) {
      const toIndex = fromIndex + 1
      const indexAttributes = this.getIndexAttributes()
      const fromIndexAttribute = indexAttributes[fromIndex]
      if (fromIndexAttribute === 'neutral') {
        this.downNeutral(fromIndex, toIndex, indexAttributes)
      } else if (fromIndexAttribute === 'parent') {
        this.downParent(fromIndex, toIndex, indexAttributes)
      } else if (fromIndexAttribute === 'child') {
        this.downChild(fromIndex, toIndex, indexAttributes)
      }
      this.isChangedDetail = true
    },
    /**
     * 指定の移動元インデックス番号の設問を指定した移動先に移動
     * @param fromIndex 移動元インデックス番号
     * @param toIndex 移動先インデックス番号
     */
    reorderQuestions(fromIndex: number, toIndex: number) {
      this.localSurveyMaster.questionFlow = this.reorderArray(
        this.localSurveyMaster.questionFlow,
        fromIndex,
        toIndex
      ) as SurveyMasterQuestionFlowItem[]
      this.localSurveyMaster.questions = this.reorderArray(
        this.localSurveyMaster.questions,
        fromIndex,
        toIndex
      ) as SurveyMasterQuestionItem[]
    },
    /**
     * 指定の移動元インデックス番号の配列を指定した移動先に移動
     * @param elements 対象配列
     * @param currentIndex 移動元インデックス番号
     * @param newIndex 移動先インデックス番号
     */
    reorderArray(elements: any, currentIndex: number, newIndex: number) {
      const copyElements = elements.slice()
      const elm = copyElements[currentIndex]
      copyElements.splice(currentIndex, 1)
      copyElements.splice(newIndex, 0, elm)
      return copyElements
    },
    /**
     * 他コンポーネントからアンケートひな形詳細の変更を受け取る
     * @param newSurveyMaster アンケートひな形詳細
     */
    emitSurveyMasterDetail(newSurveyMaster: any) {
      this.$set(this, 'localSurveyMaster', newSurveyMaster)
      this.isChangedDetail = true
    },
    /**
     * 他コンポーネントからアンケートひな形設問詳細の変更を受け取る
     * @param newSurveyMaster アンケートひな形詳細
     * @param index 対象インデックス番号
     */
    emitSurveyMasterQuestion(newSurveyMaster: any, index: number) {
      this.$set(this, 'localSurveyMaster', newSurveyMaster)
      this.isChangedQuestion[index] = true
    },
    /**
     * アンケートひな形設問詳細を保存
     * @param localSurveyMaster 入力中の全てのアンケートマスター情報
     */
    update(localSurveyMaster: LocalSurveyMaster) {
      this.clearErrorBar()
      const request = new UpdateSurveyMasterDraftByIdRequest()
      Object.assign(request, localSurveyMaster)
      const surveyMasterId: string = this.$route.params.surveyMasterId as string
      const version: number = this.surveyMaster.version
      return UpdateSurveyMasterDraftById(surveyMasterId, version, request)
    },
    /** アンケートひな形設問詳細をリフレッシュ */
    refresh() {
      this.clearErrorBar()
      this.$emit('refresh')
    },
    /**
     * 指定したインデックス番号の設問のバリデーション状況および情報変更の有無を返す
     * @param index 対象インデックス番号
     * @returns 判定真偽値
     */
    isValidQuestionWithChanged(index: number): boolean {
      if (
        this.isChangedQuestion[index] &&
        this.isValidQuestion[index] &&
        this.isRequiredChoiceCount(index)
      ) {
        return true
      } else {
        return false
      }
    },
    /**
     * 指定したインデックス番号の設問の回答タイプに応じて回答選択肢の最小数を満たしているか判定
     * @param index 対象インデックス番号
     * @returns 判定真偽値
     */
    isRequiredChoiceCount(index: number): boolean {
      const optionGroups = this.localSurveyMaster.questions[index].choices
      const minRequiredCount = this.requiredChoiceCount(index)
      for (const i in optionGroups) {
        const options = optionGroups[i]
        let currentRequiredCount = 0
        if (options.group && options.group.length && options.group.length > 0) {
          if (!options.isNew) {
            return true
          }

          for (const i2 in options.group) {
            if (options.group[i2].disabled === false) {
              currentRequiredCount++
            }
          }
        }
        if (currentRequiredCount < minRequiredCount) {
          return false
        }
      }
      return true
    },
    /**
     * 指定したインデックス番号の設問の回答タイプに応じて回答選択肢の最小数を返す
     * @param index 対象インデックス番号
     * @returns 回答選択肢の最小数
     */
    requiredChoiceCount(index: number): number {
      if (this.localSurveyMaster.questions[index].format === 'checkbox') {
        return this.requiredChoiceCounts.checkbox
      } else if (this.localSurveyMaster.questions[index].format === 'radio') {
        return this.requiredChoiceCounts.radio
      } else {
        return this.requiredChoiceCounts.others
      }
    },
    /**
     * 指定したインデックス番号の設問が有効化不可か判定する
     * @param index 対象インデックス番号
     * @returns 判定真偽値
     */
    isDenyActivation(index: number): boolean {
      const summaryType = this.localSurveyMaster.questions[index].summaryType
      const format = this.localSurveyMaster.questions[index].format
      const indexAttributes = this.getIndexAttributes()
      const indexAttribute = indexAttributes[index]
      // indexAttributeがneutralの場合は自身のsummaryTypeで判別
      // indexAttributeがparentまたはchildの場合は親のsummaryTypeと全ての子のsummaryTypeで判別
      if (indexAttribute === 'parent') {
        const groupIndexes = this.getIndexGroupByParent(index, indexAttributes)
        for (const i in groupIndexes) {
          const groupIndex = groupIndexes[i]
          if (this.localSurveyMaster.questions[groupIndex].disabled === true) {
            const groupSummaryType =
              this.localSurveyMaster.questions[groupIndex].summaryType
            const groupFormat =
              this.localSurveyMaster.questions[groupIndex].format
            if (
              this.checkDuplicateSummaryType(groupFormat, groupSummaryType) ===
              true
            ) {
              return true
            }
          }
        }
        return false
      } else if (indexAttribute === 'child') {
        const groupIndexes = this.getIndexGroupByChild(index, indexAttributes)
        for (const i in groupIndexes) {
          const groupIndex = groupIndexes[i]
          if (this.localSurveyMaster.questions[groupIndex].disabled === true) {
            const groupSummaryType =
              this.localSurveyMaster.questions[groupIndex].summaryType
            const groupFormat =
              this.localSurveyMaster.questions[groupIndex].format
            if (
              this.checkDuplicateSummaryType(groupFormat, groupSummaryType) ===
              true
            ) {
              return true
            }
          }
        }
        return false
      } else {
        return this.checkDuplicateSummaryType(format, summaryType)
      }
    },
    /**
     * 指定したインデックス番号の設問の集計タイプが他の有効な設問内で重複しているか判定する
     * @param index 対象インデックス番号
     * @returns 判定真偽値
     */
    checkDuplicateSummaryType(format: string, summaryType: string) {
      const type = this.localSurveyMaster.type
      for (const i in this.localSurveyMaster.questions) {
        const question = this.localSurveyMaster.questions[i]
        if (
          question.disabled === false &&
          question.format === 'radio' &&
          format === 'radio'
        ) {
          if (type === 'service') {
            if (
              summaryType === 'satisfaction' &&
              question.summaryType === 'satisfaction'
            ) {
              return true
            }
          } else if (type === 'completion') {
            if (
              (summaryType === 'satisfaction' &&
                question.summaryType === 'satisfaction') ||
              (summaryType === 'continuation' &&
                question.summaryType === 'continuation') ||
              (summaryType === 'recommended' &&
                question.summaryType === 'recommended') ||
              (summaryType === 'sales' && question.summaryType === 'sales')
            ) {
              return true
            }
          } else if (type === 'pp') {
            if (
              (summaryType === 'survey_satisfaction' &&
                question.summaryType === 'survey_satisfaction') ||
              (summaryType === 'man_hour_satisfaction' &&
                question.summaryType === 'man_hour_satisfaction') ||
              (summaryType === 'karte_satisfaction' &&
                question.summaryType === 'karte_satisfaction') ||
              (summaryType === 'master_karte_satisfaction' &&
                question.summaryType === 'master_karte_satisfaction')
            ) {
              return true
            }
          }
        }
      }
      return false
    },
  },
  computed: {
    /**
     * 設問詳細のバリデーション状況および情報変更の有無を返す
     * @returns 判定真偽値
     */
    isValidDetailWithChanged(): boolean {
      if (this.isChangedDetail) {
        return true
      } else {
        return false
      }
    },
  },
  mounted() {
    if (this.localSurveyMaster.revision === 0) {
      this.isDraft = true
    }
    for (const key in this.localSurveyMaster.questions) {
      if (this.localSurveyMaster.questions[key].disabled === true) {
        this.isInvalid = true
      }
    }
  },
})
</script>

<style lang="scss">
.o-survey-question-table-invalid {
  margin-top: 16px;
  + .o-survey-question-table-invalid {
    margin-top: 24px;
  }
}
.o-user-detail-rows {
  &.no-border {
    .o-common-detail-rows__unit {
      &:last-child {
        border-bottom: 0;
      }
    }
  }
}
.t-button-close {
  .v-icon {
    &.theme--light {
      transform: rotate(180deg);
    }
  }
}
.isHidden {
  display: none;
  &.isEditable {
    display: block;
  }
}
</style>
