<template>
  <RootTemPlate>
    <ListInPageHeader>
      {{ pageName }}
    </ListInPageHeader>
    <MasterSort
      :param="searchParam"
      @sort="search"
      @clear="clear"
      @update="updateParam"
    />
    <MasterListTable
      :is-loading="isLoading"
      :limit="limit"
      :masters="response.masters"
      :total="total"
      :offset-page="offsetPage"
      @click:prev="prevPage"
      @click:next="nextPage"
    />
  </RootTemPlate>
</template>

<script lang="ts">
import ListInPageHeader from '~/components/common/organisms/ListInPageHeader.vue'
import MasterSort, {
  SelectedDataTypeParam,
} from '~/components/master/organisms/MasterSort.vue'
import MasterListTable from '~/components/master/organisms/MasterListTable.vue'
import RootTemPlate from '~/components/common/bases/RootTemplate.vue'
import { GetMastersRequest, GetMastersResponse } from '~/models/Master'
import type { PropType } from '~/common/BaseComponent'
import CommonList from '~/components/common/templates/CommonList'

export default CommonList.extend({
  name: 'MasterListTemplate',
  components: {
    RootTemPlate,
    ListInPageHeader,
    MasterSort,
    MasterListTable,
  },
  props: {
    /**
     * ローディング中か否か
     */
    isLoading: {
      type: Boolean,
      required: true,
    },
    /**
     * GetMasters APIのレスポンス
     */
    response: {
      type: Object as PropType<GetMastersResponse>,
      required: true,
    },
  },
  data() {
    return {
      SearchParamType: SelectedDataTypeParam,
      RequestType: GetMastersRequest,
      apiName: 'getMasters',
      headerPageName: this.$t('master.group_info.name'),
      pageName: this.$t('master.pages.index.name'),
      buttons: [
        { name: this.$t('master.pages.index.name'), link: '/master/list' },
        { name: this.$t('master.pages.create.name'), link: '/master/create' },
      ],
    }
  },
})
</script>
