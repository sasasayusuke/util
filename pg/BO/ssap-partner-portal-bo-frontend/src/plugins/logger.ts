/* eslint-disable no-console */

/**
 * ロガークラス.
 */
export class Logger {
  /**
   * デバッグ
   * @param messages メッセージ
   */
  debug(...messages: any[]) {
    const arrowLogLevel: string[] = ['debug']
    const logLevel: any = process.env.CONSOLE_LOG_LEVEL
    if (process.env.CONSOLE_LOGGING && arrowLogLevel.includes(logLevel)) {
      console.log('[debug]', ...messages)
    }
  }

  /**
   * 情報
   * @param messages メッセージ
   */
  info(...messages: any[]) {
    const arrowLogLevel: string[] = ['debug', 'info']
    const logLevel: any = process.env.CONSOLE_LOG_LEVEL
    if (process.env.CONSOLE_LOGGING && arrowLogLevel.includes(logLevel)) {
      console.info('[info]', ...messages)
    }
  }

  /**
   * 警告
   * @param messages メッセージ
   */
  warn(...messages: any[]) {
    const arrowLogLevel: string[] = ['debug', 'info', 'warn']
    const logLevel: any = process.env.CONSOLE_LOG_LEVEL
    if (process.env.CONSOLE_LOGGING && arrowLogLevel.includes(logLevel)) {
      console.warn('[warn]', ...messages)
    }
  }

  /**
   * エラー
   * @param messages メッセージ
   */
  error(...messages: any[]) {
    const arrowLogLevel: string[] = ['debug', 'info', 'warn', 'error']
    const logLevel: any = process.env.CONSOLE_LOG_LEVEL
    if (process.env.CONSOLE_LOGGING && arrowLogLevel.includes(logLevel)) {
      console.error('[error]', ...messages)
    }
  }
}
/* eslint-enable */

/**
 * インジェクト.
 */
export default (_: any, inject: any) => {
  inject('logger', new Logger())
}
