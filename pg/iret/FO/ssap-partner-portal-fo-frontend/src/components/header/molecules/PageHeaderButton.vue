<template>
  <Button
    v-if="isAccessible(targetLink)"
    :to="targetLink"
    style-set="page-header"
    v-bind="$attrs"
    v-on="$listeners"
  >
    <slot>
      {{ $t(button.label) }}
    </slot>
  </Button>
</template>

<script lang="ts">
import BaseComponent, { PropType } from '~/common/BaseComponent'
import { Button } from '~/components/common/atoms/index'
import { hasRole, getAllowedRoles } from '~/utils/role-authorizer'
import { masterKarteListUrlStore } from '~/store'

export interface IButton {
  label: string
  to: string
  toFunc?: Function
  isCurrent?: boolean
}

export default BaseComponent.extend({
  components: {
    Button,
  },
  props: {
    button: {
      type: Object as PropType<IButton>,
      required: true,
    },
  },
  computed: {
    targetLink(): string {
      if (this.button.isCurrent) {
        return this.$route.path
      }

      let rtn: string = this.button.to

      // routeのparamsにprojectIdが含まれる場合は、なるべくそっちで置換する
      const projectIdParam = this.$route.params.projectId
      if (projectIdParam) {
        rtn = rtn.replace('_projectId', projectIdParam)
      }
      const projectIdStore = this.$store.state['current-page-data'].projectId
      if (projectIdStore) {
        rtn = rtn.replace('_projectId', projectIdStore)
      }
      // routeのparamsにnpfProjectIdが含まれる場合は置換する
      const npfProjectIdStore =
        this.$store.state['current-page-data'].npfProjectId
      if (npfProjectIdStore) {
        rtn = rtn.replace('_npfProjectId', npfProjectIdStore)
      }

      // toが「/project/list」の場合は、vuexに保存されたparamsを付与する
      if (rtn === '/project/list') {
        const masterKarteListParams = masterKarteListUrlStore.params
          ? new URLSearchParams(
              JSON.parse(masterKarteListUrlStore.params)
            ).toString()
          : ''
        rtn = `/project/list?${masterKarteListParams}`
      }
      return rtn
    },
  },
  methods: {
    // TODO: 正規表現を使ってマッチするように書き換える
    // ↑ 現状動作そのものは成立している 本当に正規表現が必要なのか要検討
    isCurrent(path: string): boolean {
      const currentPath = this.$route.path
      let targetPath = path
      targetPath = targetPath.replace('_projectId', '')
      return currentPath.startsWith(targetPath)
    },
    isAccessible(path: string): boolean {
      const allowedRoles = getAllowedRoles(path)
      return hasRole(allowedRoles)
    },
  },
})
</script>
