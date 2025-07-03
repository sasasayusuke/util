<template>
  <v-form
    class="o-user-detail-rows pt-4 px-8 pb-0"
    :value="value"
    @input="$listeners['input']"
  >
    <SurveyEditRow
      :label="$t('user.row.role.name')"
      required
      :is-editing="true"
      :tooltip="$t('user.row.role.tooltips')"
      :value="'hoge'"
    >
      <DateSelect />
      <!-- <RadioGroup
        v-model="localSurvey.id"
        :labels="$t('user.row.role.radio').labels"
        :values="$t('user.row.role.radio').values"
        horizontal
        hide-details
      /> -->
    </SurveyEditRow>
  </v-form>
</template>

<script lang="ts">
import {
  TextField,
  RadioGroup,
  Select,
  ToolTips,
  Sheet,
  Required,
} from '~/components/common/atoms'
import BaseComponent from '~/common/BaseComponent'
import DateSelect from '~/components/common/molecules/DateSelect.vue'
import SurveyDetailRow from '~/components/survey/molecules/SurveyDetailRow.vue'
import SurveyEditRow from '~/components/survey/molecules/SurveyEditRow.vue'

export default BaseComponent.extend({
  components: {
    TextField,
    RadioGroup,
    Select,
    ToolTips,
    Sheet,
    Required,
    SurveyDetailRow,
    SurveyEditRow,
    DateSelect,
  },
  props: {
    isCreating: {
      type: Boolean,
      default: false,
    },
    survey: {
      type: Object,
    },
    isEditing: {
      type: Boolean,
      required: true,
    },
  },
  data() {
    return {
      headButtons: [],
      footButtons: [],
      value: '',
      localSurvey: this.survey,
    }
  },
  computed: {
    isCustomer() {
      // @ts-ignore
      return this.localUser.role === 'customer'
    },
    isSupporter() {
      // @ts-ignore
      return this.localUser.role === 'supporter'
    },
    isSupporterManager() {
      // @ts-ignore
      return this.localUser.role === 'supporter_mgr'
    },
  },
  methods: {},
})
</script>

<style lang="scss" scoped>
.o-user-detail-rows__select {
  padding: 0;
  margin: 0;
}
.o-user-detail-rows__select__item {
  list-style: none;
  display: inline-block;
  margin-right: 1em;
  &::after {
    content: ',';
  }
  &:last-child {
    &::after {
      display: none;
    }
  }
}
</style>
