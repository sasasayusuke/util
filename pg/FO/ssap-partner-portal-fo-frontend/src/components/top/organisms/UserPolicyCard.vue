<template>
  <v-row justify="center">
    <v-col cols="auto">
      <Sheet
        class="o-UserPolicyCard py-12"
        width="664"
        elevation="3"
        rounded
        color="white"
      >
        <h1 class="o-UserPolicyCard__title">
          {{ $t('top.pages.user_policy.title') }}
        </h1>
        <p class="o-UserPolicyCard__text">
          <span>{{ $t('top.pages.user_policy.text') }}</span>
        </p>
        <UserPolicyButtons
          :user="user"
          :is-editing="isEditing"
          :is-loading="isLoading"
          @update="localUser = $event"
          @click:positive="$emit('click:positive', localUser)"
        />
      </Sheet>
    </v-col>
  </v-row>
</template>

<script lang="ts">
import { Checkbox, Button, Sheet } from '../../common/atoms/index'
import BaseComponent, { PropType } from '~/common/BaseComponent'
import UserPolicyButtons, {
  LocalUser,
} from '~/components/top/molecules/UserPolicyButtons.vue'
import { PatchUserByMineRequest } from '~/models/User'

export { LocalUser }

export default BaseComponent.extend({
  components: {
    Checkbox,
    Button,
    Sheet,
    UserPolicyButtons,
  },
  props: {
    /**
     * 編集モードか否か
     */
    isEditing: {
      type: Boolean,
    },
    /**
     * ローディング中か否か
     */
    isLoading: {
      type: Boolean,
      required: true,
    },
    /**
     * ユーザーの利用規約同意情報
     */
    user: {
      type: Object as PropType<PatchUserByMineRequest>,
    },
  },
  data() {
    return {
      localUser: new LocalUser(),
    }
  },
})
</script>

<style lang="scss" scoped>
.o-UserPolicyCard {
}
.o-UserPolicyCard__title {
  text-align: center;
  @include fontSize('xxxlarge');
  margin-bottom: 36px;
}
.o-UserPolicyCard__text {
  text-align: center;
  @include fontSize('large');
  line-height: 1.5;
  font-weight: bold;
  margin-bottom: 36px;
  span {
    white-space: pre-line;
  }
}
</style>
