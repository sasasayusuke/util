import { Api } from '~/plugins/api'

const $api = new Api()

export class AuthLoginResponse {
  message = ''
}

export async function AuthLogin() {
  return await $api.post<AuthLoginResponse>('/auth/login').catch(() => {
    window.location.href = '/'
  })
}
