import { Api } from '~/plugins/api'
import { Notification, NotificationRead } from '~/types/Notification'

const $api = new Api()

export class GetNotificationsResponse extends Array<Notification> {}

export async function GetNotifications() {
  return await $api.get<GetNotificationsResponse>(`/notifications/me`)
}
export class ConfirmNotificationsRequest {}

export class ConfirmNotificationsResponse extends Array<NotificationRead> {}

export async function ConfirmNotifications() {
  return await $api.patch<
    ConfirmNotificationsResponse,
    ConfirmNotificationsRequest
  >(`/notifications`)
}
