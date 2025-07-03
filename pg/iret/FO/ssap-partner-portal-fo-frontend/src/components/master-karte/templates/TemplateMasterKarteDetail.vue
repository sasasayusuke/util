<template>
  <RootTemPlate style="padding: 0 !important">
    <main class="detail-containers">
      <MasterKarteDetailCurrentTabs
        :is-current-program="isCurrentProgram"
        :master-karte-project="masterKarteProject"
        :next-program-read-roles="nextProgramReadRoles"
        @clickTab="onClickTab"
      />
      <MasterKarteDetailContainer
        :is-current-program="isCurrentProgram"
        :local-param="localParam"
        :master-karte-project="masterKarteProject"
        @clickSaveButton="handleSaveButton"
      >
        <template #snackBar>
          <v-snackbar v-model="snackbar" :vertical="true" timeout="3000">
            {{ $t('master-karte.pages.detail.snack-bar.complete') }}

            <template #action="{ attrs }">
              <v-btn
                color="#008a19"
                text
                v-bind="attrs"
                @click="snackbar = false"
              >
                {{ $t('master-karte.pages.detail.snack-bar.close') }}
              </v-btn>
            </template>
          </v-snackbar>
        </template>
      </MasterKarteDetailContainer>
    </main>
  </RootTemPlate>
</template>

<script lang="ts">
import ListInPageHeader from '~/components/common/organisms/ListInPageHeader.vue'
import BaseComponent from '~/common/BaseComponent'
import RootTemPlate from '~/components/common/bases/RootTemplate.vue'
import PageHeader from '~/components/common/molecules/PageHeader.vue'
import {
  CreateMasterKarte,
  FundamentalInformationClass,
  GetMasterKarteById,
  GetMasterKarteByIdResponseClass,
  OthersClass,
  UpdateMasterKarteById,
  UpdateMasterKarteResponseClass,
} from '~/models/MasterKarte'
import { currentPageDataStore, meStore } from '~/store'
import { ENUM_USER_ROLE } from '~/types/User'

export default BaseComponent.extend({
  name: 'MasterKarteList',
  components: {
    RootTemPlate,
    ListInPageHeader,
    PageHeader,
  },
  data() {
    return {
      pageName: '',
      masterKarteProject: new GetMasterKarteByIdResponseClass(),
      isCurrentProgram: true,
      localParam: new UpdateMasterKarteResponseClass(),
      snackbar: false,
    }
  },
  mounted() {
    const currentProgramParam = this.$route.query.currentProgram
    this.isCurrentProgram = this.isCurrentProgram =
      currentProgramParam === 'true' || currentProgramParam === undefined

    this.displayLoading([this.getMasterKarteById()])
  },
  computed: {
    /**
     * 次期支援の読み取り権限があるか
     *
     */
    nextProgramReadRoles(): boolean {
      if (
        meStore.role === ENUM_USER_ROLE.CUSTOMER &&
        !this.masterKarteProject.nextProgram.isCustomerPublic
      ) {
        // お客様ロールでお客様非公開ステータスの場合
        return false
      }
      return true
    },
  },
  methods: {
    /**
     * localParamにデータをセットする
     */
    setLocalParam() {
      const cpMasterKarteProject = JSON.parse(
        JSON.stringify(this.masterKarteProject)
      )
      this.localParam = {
        ppProjectId: this.masterKarteProject.ppProjectId,
        npfProjectId: this.masterKarteProject.npfProjectId,
        currentProgram: {
          id: cpMasterKarteProject.currentProgram.id,
          version: cpMasterKarteProject.currentProgram.version,
          result: cpMasterKarteProject.currentProgram.result,
        },
        nextProgram: {
          id: cpMasterKarteProject.nextProgram.id,
          isCustomerPublic: cpMasterKarteProject.nextProgram.isCustomerPublic
            ? cpMasterKarteProject.nextProgram.isCustomerPublic
            : false,
          version:
            cpMasterKarteProject.nextProgram.version === null
              ? 0
              : cpMasterKarteProject.nextProgram.version,
          fundamentalInformation:
            cpMasterKarteProject.nextProgram.fundamentalInformation === null
              ? new FundamentalInformationClass()
              : cpMasterKarteProject.nextProgram.fundamentalInformation,
          others:
            cpMasterKarteProject.nextProgram.others === null
              ? new OthersClass()
              : cpMasterKarteProject.nextProgram.others,
        },
        isNotifyUpdateMasterKarte:
          cpMasterKarteProject.isNotifyUpdateMasterKarte !== undefined
            ? cpMasterKarteProject.isNotifyUpdateMasterKarte
            : false,
      }
    },
    /**
     * タブをクリックした際に、当期支援・次期支援のタブを切り替える
     * @param isCurrentProgram
     */
    onClickTab(isCurrentProgram: boolean) {
      this.isCurrentProgram = isCurrentProgram
      this.$router.push({
        query: { currentProgram: isCurrentProgram.toString() },
      })
    },
    /**
     * GetKartenAPIを叩き、案件カルテの一覧を取得
     * @param GetKartenAPIのリクエストパラメータを指定
     */
    async getMasterKarteById() {
      const npfProjectId = this.$route.params.npfProjectId
      // npfProjectIdをstoreに保存
      currentPageDataStore.setNpfProjectId(npfProjectId)

      await GetMasterKarteById(npfProjectId)
        .then((res: any) => {
          this.masterKarteProject = res.data
          currentPageDataStore.setValues({
            projectId: this.masterKarteProject.ppProjectId,
            projectName: this.masterKarteProject.project,
            customerName: this.masterKarteProject.client,
          })
          this.setLocalParam()
          this.splitLineup()
          // 次期支援読み取り権限がない場合、強制的に当期支援を表示する
          if (!this.nextProgramReadRoles && !this.isCurrentProgram) {
            this.isCurrentProgram = true
            this.$router.push({ query: { currentProgram: 'true' } })
          }
        })
        .catch((error) => {
          this.apiErrorHandle(error)
        })
    },

    /* '+'繋ぎになっているラインナップを配列に変換 */
    splitLineup() {
      // @ts-ignore
      this.localParam.nextProgram.fundamentalInformation.lineup =
        this.localParam.nextProgram.fundamentalInformation.lineup === ''
          ? []
          : // @ts-ignore
            this.localParam.nextProgram.fundamentalInformation.lineup.split('+')
    },

    /** 保存ボタンを押した際の処理 */
    async handleSaveButton(
      masterKarteId: string,
      isCurrentProgram: boolean,
      param: UpdateMasterKarteResponseClass
    ) {
      const updatedParam = {
        ...param,
        ppProjectId: this.masterKarteProject.ppProjectId,
        npfProjectId: this.masterKarteProject.npfProjectId,
      }

      // 更新したプログラムにmasterKarteIdが含まれていれば更新APIを実行。なければ作成APIを実行
      if (masterKarteId) {
        await this.displayLoading([
          this.updateMasterKarteById(
            masterKarteId,
            isCurrentProgram,
            updatedParam
          ),
        ])
      } else {
        await this.displayLoading([
          this.createMasterKarte(isCurrentProgram, updatedParam),
        ])
      }
    },
    /**
     * マスターカルテ更新APIを叩く
     * @param param リクエストパラメータ
     */
    async updateMasterKarteById(
      masterKarteId: string,
      isCurrentProgram: boolean,
      updatedParam: UpdateMasterKarteResponseClass
    ) {
      await UpdateMasterKarteById(masterKarteId, isCurrentProgram, updatedParam)
        .then((res: any) => {
          this.masterKarteProject = res.data
          this.localParam = res.data
          this.snackbar = true
          this.setLocalParam()
          this.splitLineup()
        })
        .catch((error) => {
          this.apiErrorHandle(error)
        })
    },
    /**
     * マスターカルテ作成APIを叩く
     * @param param リクエストパラメータ
     */
    async createMasterKarte(isCurrentProgram: boolean, updatedParam: any) {
      await CreateMasterKarte(isCurrentProgram, updatedParam)
        .then((res: any) => {
          this.masterKarteProject = res.data
          this.snackbar = true
          this.setLocalParam()
          this.splitLineup()
        })
        .catch((error) => {
          this.apiErrorHandle(error)
        })
    },
  },
})
</script>

<style>
.detail-containers {
  margin: 32px 0;
}
</style>
