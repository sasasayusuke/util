export class Notification {
  public id: string = ''
  public userId: string = ''
  public summary: string = ''
  public url: string = ''
  public message: string = ''
  public confirmed: boolean = false
  public noticedAt: string = ''
}

export class NotificationRead {
  public message: string = 'OK'
}
