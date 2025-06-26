/**
 * App constant class
 */
export namespace AppConst {
  export namespace Config {
    export const DEFAULT_PAGINATION_LIMIT: Number = 30
    export const UPLOAD_ALLOW_IMAGE_EXT: Array<String> = [
      'jpeg',
      'jpg',
      'png',
      'gif',
      'svg',
    ]
    export const UTILIZATION_FLAG: Object = {
      0: 'enabled',
      1: 'disabled',
    }
  }
}
