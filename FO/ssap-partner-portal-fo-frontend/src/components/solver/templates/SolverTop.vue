<!-- ソルバーメニューテンプレートコンポーネント -->

<template>
  <RootTemPlate :class="'fill-height align-start'">
    <v-container>
      <v-layout>
        <v-flex align-self-center>
          <h1 class="text-h5 font-weight-bold text-center">
            {{ $t('solver.pages.menu.title') }}
          </h1>
        </v-flex>
      </v-layout>
      <v-layout class="justify-space-between align-center pt-3 px-3">
        <v-flex align-self-center>
          <div class="font-weight-bold">
            {{ $t('solver.pages.menu.select.title') }}
          </div>
        </v-flex>
        <v-flex align-self-center>
          <Select
            v-if="isSelectDisplay"
            v-model="selectItemValue"
            :items="selectItems"
            style-set="bgWhite"
            class="ml-auto"
            style="width: 240px"
            item-text="name"
            item-value="id"
            dense
            :placeholder="$t('solver.pages.menu.select.placeholder')"
            @change="changeSolverCorporation"
          />
        </v-flex>
      </v-layout>

      <v-layout v-if="isMenuDisplay">
        <v-flex align-self-center>
          <SolverHomeMenu :menu-items="menuItemsCompany" :is-company="true" />
        </v-flex>
      </v-layout>
      <v-layout v-if="isMenuDisplay" class="pt-7">
        <v-flex align-self-center>
          <SolverHomeMenu :menu-items="menuItems" :is-company="false" />
        </v-flex>
      </v-layout>
      <v-layout v-if="!isMenuDisplay" class="pt-3">
        <v-flex class="solver-selection">
          {{ $t('solver.pages.menu.select.selectDescription') }}
        </v-flex>
      </v-layout>
    </v-container>
    <SolverMenuModal :is-modal-open="isModalOpen" @closeModal="closeModal" />
  </RootTemPlate>
</template>

<script lang="ts">
import SolverMenuModal from '../molecules/SolverMenuModal.vue'
import SolverHomeMenu from '~/components/solver/organisms/SolverMenu.vue'
import { Button, Select } from '~/components/common/atoms/index'
import TopCard from '~/components/top/organisms/TopCard.vue'
import BaseComponent, { PropType } from '~/common/BaseComponent'
import RootTemPlate from '~/components/common/bases/RootTemplate.vue'
import { IGetSolverCorporationsResponse } from '~/types/SolverCorporation'
import { meStore, solverCorporationStore } from '~/store'
import { ENUM_USER_ROLE } from '~/types/User'

export default BaseComponent.extend({
  name: 'TemplateSolverTop',
  components: {
    SolverHomeMenu,
    Button,
    TopCard,
    RootTemPlate,
    Select,
    SolverMenuModal,
  },
  props: {
    /** 法人ソルバー一覧 */
    solverCorporations: {
      type: Object as PropType<IGetSolverCorporationsResponse>,
      required: true,
    },
    /** メニューを表示するか */
    isMenuDisplay: {
      type: Boolean,
      required: true,
    },
    /** 法人ソルバー情報入力誘導モーダルを表示するか */
    isModalOpen: {
      type: Boolean,
      required: true,
    },
    /** 企業名 */
    solverCorporationName: {
      type: String,
      required: true,
    },
  },
  data() {
    return {
      selectItemValue: '',
      isSelectDisplay: true,
    }
  },
  mounted() {
    if (meStore.role === ENUM_USER_ROLE.APT) {
      // ログインユーザーがアライアンス担当の場合、法人セレクトボックスを表示
      this.isSelectDisplay = true
      if (solverCorporationStore.id) {
        this.selectItemValue = solverCorporationStore.id
      }
    } else {
      // ログインユーザーが法人ソルバーの場合
      this.isSelectDisplay = false
      this.selectItemValue = meStore.solverCorporationId
    }
  },
  computed: {
    // 法人ソルバー選択プルダウンアイテム
    selectItems(): { name: string; id: string }[] {
      return this.solverCorporations.solverCorporations.map(
        (solverCorporation) => {
          return { name: solverCorporation.name, id: solverCorporation.id }
        }
      )
    },
    // 企業名
    menuItemsCompany(): { title: string; to: string }[] {
      return [
        {
          title: this.solverCorporationName,
          to: `/solver/corporation/${solverCorporationStore.id}`,
        },
      ]
    },
    // メニューアイテム
    menuItems(): {
      category: { title: string; to: string }
      items: { title: string; to: string }[]
    }[] {
      return [
        {
          category: {
            title: this.$t('solver.pages.menu.menuItem.solverList') as string,
            to: ``,
          },
          items: [
            {
              title: this.$t(
                'solver.pages.menu.menuItem.solverCandidate'
              ) as string,
              to: `/solver/candidate/list/${this.selectItemValue}`,
            },
            {
              title: this.$t('solver.pages.menu.menuItem.solver') as string,
              to: `/solver/list/${this.selectItemValue}`,
            },
          ],
        },
        {
          category: {
            title: this.$t(
              'solver.pages.menu.menuItem.newSolverApplication'
            ) as string,
            to: '',
          },
          items: [
            {
              title: this.$t(
                'solver.pages.menu.menuItem.solverCandidate'
              ) as string,
              to: '/solver/candidate/application',
            },
            {
              title: this.$t('solver.pages.menu.menuItem.solver') as string,
              to: '/solver/application',
            },
          ],
        },
        {
          category: {
            title: this.$t(
              'solver.pages.menu.menuItem.monthlyUpdate'
            ) as string,
            to: '',
          },
          items: [
            {
              title: this.$t(
                'solver.pages.menu.menuItem.utilizationRateUpdate'
              ) as string,
              to: `/solver/utilization-rate/${
                this.selectItemValue
              }?source_url=${encodeURIComponent('/solver/menu')}`,
            },
          ],
        },
      ]
    },
  },
  methods: {
    // モーダルを閉じる処理
    closeModal() {
      this.$emit('closeModal')
    },

    // 法人ソルバー選択プルダウンのアイテムを変更した場合の処理
    changeSolverCorporation() {
      this.$emit('changeSolverCorporation', this.selectItemValue)
    },
  },
})
</script>

<style lang="scss">
.solver-selection {
  background-color: #f0f0f0;
  color: #666666;
  text-align: center;
  display: flex;
  align-items: center;
  justify-content: center;
  padding: 10px;
  border-radius: 5px;
  font-weight: bold;
  height: 25rem;
}
</style>
