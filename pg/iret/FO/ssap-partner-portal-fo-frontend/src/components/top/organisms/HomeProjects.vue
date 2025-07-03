<template>
  <section>
    <v-container class="pa-0 mb-4">
      <v-flex class="d-flex justify-space-between align-center">
        <h2 class="font-size-large font-weight-bold">
          {{ $t('top.pages.home.header.projects') }}
        </h2>
        <Button outlined color="primary" small :to="toProjectList">
          {{ $t('common.button.viewAll') }}
        </Button>
      </v-flex>
    </v-container>
    <v-container v-if="projects.length" class="pa-0">
      <v-row>
        <v-col
          v-for="(project, index) in projects"
          :key="index"
          cols="6"
          md="4"
          :class="displayStyle(index)"
        >
          <HomeProjectsCard :project="project" />
        </v-col>
        <v-col
          v-for="index in blank"
          :key="index"
          cols="6"
          md="4"
          :class="displayStyle(index + projects.length - 1)"
        >
          <HomeProjectsCard :blank="true" />
        </v-col>
      </v-row>
      <v-row v-if="projects.length > 3">
        <v-col>
          <Button
            style-set="showContinuation"
            :class="{ 'is-open': showMore }"
            @click="toggleShowProjects"
          >
            <Icon left> icon-org-arrow-down </Icon>
            {{
              showMore
                ? $t('common.button.close')
                : $t('common.button.showContinuation')
            }}
          </Button>
        </v-col>
      </v-row>
    </v-container>
    <Alert v-else style-set="no_data">
      {{ $t('common.alert.no_data') }}
    </Alert>
  </section>
</template>

<script lang="ts">
import HomeProjectsCard from '../molecules/HomeProjectsCard.vue'
import BaseComponent from '~/common/BaseComponent'
import type { PropType } from '~/common/BaseComponent'
import { Button, Icon, Alert } from '~/components/common/atoms/index'
import { ProjectListItem } from '~/models/Project'
import { masterKarteListUrlStore } from '~/store'

export default BaseComponent.extend({
  components: {
    HomeProjectsCard,
    Button,
    Icon,
    Alert,
  },
  data() {
    return {
      showMore: false,
    }
  },
  computed: {
    /**
     * 1列に3案件のカードを表示
     */
    blank() {
      const projects = this.projects as any
      return Math.ceil(projects.length / 3) * 3 - projects.length
    },
    /**
     * 案件一覧へのリンク
     */
    toProjectList() {
      const masterKarteListParams = masterKarteListUrlStore.params
        ? new URLSearchParams(
            JSON.parse(masterKarteListUrlStore.params)
          ).toString()
        : ''
      return `/project/list?${masterKarteListParams}`
    },
  },
  methods: {
    /**
     * 「続きを表示する」ボタンを押下
     */
    toggleShowProjects() {
      if (this.showMore === true) {
        this.showMore = false
      } else {
        this.showMore = true
      }
    },
    displayStyle(index: Number) {
      if (!this.showMore) {
        return index > 2 ? 'd-none' : 'd-inline-block'
      } else {
        return index > 8 ? 'd-none' : 'd-inline-block'
      }
    },
  },
  props: {
    /**
     * 案件情報
     */
    projects: {
      type: Array as PropType<ProjectListItem[]>,
    },
  },
})
</script>
