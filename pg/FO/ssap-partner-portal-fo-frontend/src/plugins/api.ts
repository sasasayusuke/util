import axios, { AxiosInstance, AxiosResponse, AxiosError } from 'axios'
import { Auth } from 'aws-amplify'

import { Logger } from './logger'

const $logger = new Logger() // loggerプラグインを読み込み

export class UnauthorizedError extends Error {} // 401
export class ForbiddenError extends Error {} // 403
export class InternalServerError extends Error {} // 500
export class NotFoundError extends Error {
  response?: AxiosResponse
  constructor(response: AxiosResponse) {
    super()
    this.response = response
  }
} // 404

export class ApiError extends Error {
  constructor(status: number) {
    super()
    this.status = status
  }

  status!: number
}

/**
 * apiへのリクエスト用プラグインクラス.
 */
export class Api {
  // APIインスタンス
  private api: AxiosInstance
  private enableJwt: boolean
  /**
   * コンストラクタ.
   */
  public constructor(enableJwt: boolean = true) {
    this.enableJwt = enableJwt
    this.api = axios.create({
      baseURL: process.env.API_BASE_URL,
      headers: {
        'Content-Type': 'application/json',
      },
      responseType: 'json',
    })

    const createNotFoundError = (response?: AxiosResponse): AxiosResponse => {
      // CloudFrontからエラーページが返却されている場合のレスポンス
      return {
        data: 'Not Found',
        status: 404,
        statusText: 'Not Found',
        headers: response ? response.headers : undefined,
        config: response ? response.config : {},
        request: response ? response.request : undefined,
      }
    }

    this.api.interceptors.response.use(
      function (response) {
        // 成功時の処理
        if (
          response.headers['content-type'] &&
          response.headers['content-type'].includes('text/html')
        ) {
          // CloudFrontからエラーページが返却された場合、404を返却
          return Promise.reject(new NotFoundError(createNotFoundError()))
        }
        // レスポンスデータをURLデコードして返却する
        response.data = decodeResponse(response.data)
        return response
      },
      function (error: AxiosError) {
        let errorToBeThrown: Error = error

        if (error.response) {
          switch (error.response.status) {
            case 401:
              errorToBeThrown = new UnauthorizedError()
              break
            case 403:
              errorToBeThrown = new ForbiddenError()
              break
            case 500:
              errorToBeThrown = new InternalServerError()
          }
        } else {
          errorToBeThrown = new NotFoundError(createNotFoundError())
        }
        $logger.error('api common error', error)
        return Promise.reject(errorToBeThrown)
      }
    )
  }

  /**
   * JWTを取得する.
   */
  public async getJWT(): Promise<string> {
    return await Auth.currentAuthenticatedUser()
      .then((user) => {
        return user.signInUserSession.idToken.jwtToken
      })
      .catch((err) => {
        $logger.error(err)
        if (err === 'The user is not authenticated') {
          throw new UnauthorizedError()
        }
        throw err
      })
  }

  /**
   * getリクエスト
   * @template T 想定されるオブジェクトの型
   * @param url URL
   * @param params リクエストボディの値
   * @returns `axios`のペイロード
   */
  public async get<T, B = { [key: string]: any }>(
    url: string,
    params?: B
  ): Promise<AxiosResponse<T>> {
    const headers = this.enableJwt ? await this.generateHeader() : {}

    const response: AxiosResponse<T> = await this.api.get(url, {
      params,
      headers,
    })

    $logger.info('GET', url, 'request:', params, 'response:', response)
    return response
  }

  /**
   * deleteリクエスト
   * @template T 想定されるオブジェクトの型
   * @param url URL
   * @returns `axios`のペイロード
   */
  public async delete<T>(url: string): Promise<AxiosResponse<T>> {
    const headers = this.enableJwt ? await this.generateHeader() : {}

    const response: AxiosResponse<T> = await this.api.delete(url, {
      headers,
    })

    $logger.info('DELETE', url, 'response:', response)
    return response
  }

  /**
   * postリクエスト
   * @template T 想定されるオブジェクトの型
   * @template B リクエストボディの型
   * @param url URL
   * @param params リクエストボディの値
   * @returns `axios`のペイロード
   */
  public async post<T, B = { [key: string]: any }>(
    url: string,
    params?: B
  ): Promise<AxiosResponse<T>> {
    const headers = this.enableJwt ? await this.generateHeader() : {}

    const response: AxiosResponse<T> = await this.api.post(url, params, {
      headers,
    })

    $logger.info('POST', url, 'request:', params, 'response:', response)
    return response
  }

  /**
   * PUTリクエスト
   * @template T 想定されるオブジェクトの型
   * @template B リクエストボディの型
   * @param url URL
   * @param params リクエストボディの値
   * @returns `axios`のペイロード
   */
  public async put<T, B = { [key: string]: any }>(
    url: string,
    params?: B
  ): Promise<AxiosResponse<T>> {
    const headers = this.enableJwt ? await this.generateHeader() : {}

    const response: AxiosResponse<T> = await this.api.put(url, params, {
      headers,
    })

    $logger.info('PUT', url, 'request:', params, 'response:', response)
    return response
  }

  /**
   * PATCHリクエスト
   * @template T 想定されるオブジェクトの型
   * @template B リクエストボディの型
   * @param url URL
   * @param params リクエストボディの値
   * @returns `axios`のペイロード
   */
  public async patch<T, B = { [key: string]: any }>(
    url: string,
    params?: B
  ): Promise<AxiosResponse<T>> {
    const headers = this.enableJwt ? await this.generateHeader() : {}

    const response: AxiosResponse<T> = await this.api.patch(url, params, {
      headers,
    })

    $logger.info('PATCH', url, 'request:', params, 'response:', response)
    return response
  }

  private async generateHeader() {
    let jwt = ''
    await this.getJWT().then((res) => {
      jwt = res
    })

    return {
      Authorization: `Bearer ${jwt}`,
    }
  }
}

/**
 * URLデコード処理
 * @param data レスポンスデータ
 * @returns URLデコード済のレスポンスデータ
 */
function decodeResponse(data: any): any {
  try {
    if (typeof data === 'string') {
      return decodeURIComponent(data)
    }
    if (Array.isArray(data)) {
      return data.map((item) => decodeResponse(item))
    }
    if (data && typeof data === 'object') {
      Object.keys(data).forEach((key) => {
        data[key] = decodeResponse(data[key])
      })
    }
  } catch (error) {
    // URLデコードに失敗した場合はそのままのデータを返す
    return data
  }
  return data
}

/**
 * インジェクト.
 */
export default (_: any, inject: any) => {
  inject('api', new Api())
}
