import { format as dateFnsFormat, parse } from 'date-fns'
import ja from 'date-fns/locale/ja'
import { Auth } from 'aws-amplify'
import sanitizeHtml from 'sanitize-html'

export function formatDateStr(dateStr: string, format: string): string {
  const rtn = dateFnsFormat(new Date(dateStr), format)
  return rtn
}

const LOCAL_STORAGE_DATE_FORMAT = 'yyyy-MM-dd HH:mm:ss'

export function getCurrentDate(): Date {
  const storageCurrentDateKey = process.env.STORAGE_CURRENT_DATE_KEY
  // デバッグ中でない、または環境変数でlocalStorageの現在日時格納先の指定がなければ、通常通り現在の日時を返す
  if (!process.env.DEBUG || !storageCurrentDateKey) {
    return new Date()
  }

  const specifiedDateString = localStorage.getItem(storageCurrentDateKey)
  // デバッグであっても、localStorageからの指定がなければ、現在日時を返す
  if (!specifiedDateString) {
    return new Date()
  }

  return parse(specifiedDateString, LOCAL_STORAGE_DATE_FORMAT, new Date(), {
    locale: ja,
  })
}

export function setCurrentDate(date: string | Date): void {
  const storageCurrentDateKey = process.env.STORAGE_CURRENT_DATE_KEY!
  if (!process.env.DEBUG || !storageCurrentDateKey) return

  if (typeof date === 'string') {
    localStorage.setItem(storageCurrentDateKey, date)
  } else if (date instanceof Date) {
    localStorage.setItem(
      storageCurrentDateKey,
      dateFnsFormat(date, LOCAL_STORAGE_DATE_FORMAT)
    )
  }
}

/**
 * 日付を年度に変換する
 * @param {Date} date 年度を取得したい日付
 * @returns {number} 年度
 */
export function toFiscalYear(date: Date = getCurrentDate()): number {
  const year = date.getFullYear()
  const fiscalMonth = date.getMonth() - 3 // 3か月前の日付

  // dateに変換することで、月の値がマイナスでも正常に日付に変換する
  // 仮に2000年-2月を指定したなら、1999年11月に変換される
  const threeMonthAgo = new Date(year, fiscalMonth)

  return threeMonthAgo.getFullYear() // 3か月前の年の値(=年度)を返す
}

/**
 * 年度始めの日付を取得する
 * @param {Date} date 年度を取得したい日付
 * @returns {Date} 年度始めのdate
 */
export function getFiscalYearStart(date: Date = getCurrentDate()): Date {
  const fiscalYear: number = toFiscalYear(date)

  return new Date(fiscalYear, 4 - 1)
}

/**
 * 年度終わりの日付を取得する
 * @param {Date} date 年度終わりを取得したい日付
 * @returns {Date} 年度終わり(翌年度開始の1ミリ秒前)のdate
 */
export function getFiscalYearEnd(date: Date = getCurrentDate()): Date {
  const rtn = getFiscalYearStart(date) // 今年度の始まりを取得して
  rtn.setFullYear(rtn.getFullYear() + 1) // 1年後の日付に変換し
  rtn.setMilliseconds(rtn.getMilliseconds() - 1) // その1ミリ秒前を取得する

  return rtn // 例: xxxx/03/31 23:59:59
}

/**
 * date型をyyyyMM形式の整数に変換する
 * @param {Date} date 日付
 * @returns {number}
 */
export function toYearMonthInt(date: Date) {
  const yyyyMM = dateFnsFormat(date, 'yyyyMM')
  return parseInt(yyyyMM)
}

/**
 * メールアドレス文字列を整える
 * @param {string} email メールアドレス
 * @returns {string}
 */
export function adjustEmail(email: string): string {
  // trim関数で文字列の最初と最後をトリミング
  email = email.trim()
  // 文字列中のスペースを全て取り除く
  email = email.replaceAll(/\s+/g, '')
  // 文字列中の水平タブを全て取り除く
  email = email.replaceAll(/\t+/g, '')
  // 文字列中の垂直タブを全て取り除く
  email = email.replaceAll(/\v+/g, '')
  // 文字列中の改行コード(CRLF,LF)を全て取り除く
  email = email.replaceAll(/\n+/g, '')
  // 文字列中の改行コード(CR)を全て取り除く
  email = email.replaceAll(/\r+/g, '')
  // 文字列中の改ページ(0x0c)を全て取り除く
  email = email.replaceAll(/\f+/g, '')
  // スペース、タブ、改行コード以外の目に見えないUnicodeの文字を全て削除
  email = removeInvisibleUnicodeChars(email)
  // ※その他、除去や置き換えを行いたい文字列が発生した場合は以下に追加
  return email
}

/**
 * スペース、タブ、改行コード以外の目に見えないUnicodeの文字を全て削除
 * @param {string} value 文字列
 * @returns {string}
 */
export function removeInvisibleUnicodeChars(value: string): string {
  value = value.replaceAll(/\u0020+/g, '')
  value = value.replaceAll(/\u00A0+/g, '')
  value = value.replaceAll(/\u00AD+/g, '')
  value = value.replaceAll(/\u034F+/g, '')
  value = value.replaceAll(/\u061C+/g, '')
  value = value.replaceAll(/\u115F+/g, '')
  value = value.replaceAll(/\u1160+/g, '')
  value = value.replaceAll(/\u17B4+/g, '')
  value = value.replaceAll(/\u17B5+/g, '')
  value = value.replaceAll(/\u180E+/g, '')
  value = value.replaceAll(/\u2000+/g, '')
  value = value.replaceAll(/\u2001+/g, '')
  value = value.replaceAll(/\u2002+/g, '')
  value = value.replaceAll(/\u2003+/g, '')
  value = value.replaceAll(/\u2004+/g, '')
  value = value.replaceAll(/\u2005+/g, '')
  value = value.replaceAll(/\u2006+/g, '')
  value = value.replaceAll(/\u2007+/g, '')
  value = value.replaceAll(/\u2008+/g, '')
  value = value.replaceAll(/\u2009+/g, '')
  value = value.replaceAll(/\u200A+/g, '')
  value = value.replaceAll(/\u200B+/g, '')
  value = value.replaceAll(/\u200C+/g, '')
  value = value.replaceAll(/\u200D+/g, '')
  value = value.replaceAll(/\u200E+/g, '')
  value = value.replaceAll(/\u200F+/g, '')
  value = value.replaceAll(/\u202F+/g, '')
  value = value.replaceAll(/\u205F+/g, '')
  value = value.replaceAll(/\u2060+/g, '')
  value = value.replaceAll(/\u2061+/g, '')
  value = value.replaceAll(/\u2062+/g, '')
  value = value.replaceAll(/\u2063+/g, '')
  value = value.replaceAll(/\u2064+/g, '')
  value = value.replaceAll(/\u206A+/g, '')
  value = value.replaceAll(/\u206B+/g, '')
  value = value.replaceAll(/\u206C+/g, '')
  value = value.replaceAll(/\u206D+/g, '')
  value = value.replaceAll(/\u206E+/g, '')
  value = value.replaceAll(/\u206F+/g, '')
  value = value.replaceAll(/\u3000+/g, '')
  value = value.replaceAll(/\u2800+/g, '')
  value = value.replaceAll(/\u3164+/g, '')
  value = value.replaceAll(/\uFEFF+/g, '')
  value = value.replaceAll(/\uFFA0+/g, '')
  value = value.replaceAll(/\u1D159+/g, '')
  value = value.replaceAll(/\u1D173+/g, '')
  value = value.replaceAll(/\u1D174+/g, '')
  value = value.replaceAll(/\u1D175+/g, '')
  value = value.replaceAll(/\u1D176+/g, '')
  value = value.replaceAll(/\u1D177+/g, '')
  value = value.replaceAll(/\u1D178+/g, '')
  value = value.replaceAll(/\u1D179+/g, '')
  value = value.replaceAll(/\u1D17A+/g, '')
  return value
}

/**
 * ランダムな文字列を生成
 * @param {number} length 生成する文字列長
 * @param {string[]} targets 生成対象の文字列セット(upper|lower|number|symbol) ※未指定時は全て使用
 * @returns {string}
 */
export function randomString(length: number, targets: string[] = []): string {
  let result: string = ''

  // 大文字英数字記号の定義
  const upperChars = 'ABCDEFGHIJKLMNOPQRSTUVWXYZ'
  const lowerChars = 'abcdefghijklmnopqrstuvwxyz'
  const numberChars = '0123456789'
  const symbolChars = '!?+-@#$%&*_=:;/'

  // targetsで指定されている文字セットを結合
  let chars = ''
  if (targets.length === 0 || targets.includes('upper')) {
    chars += upperChars
  }
  if (targets.length === 0 || targets.includes('lower')) {
    chars += lowerChars
  }
  if (targets.length === 0 || targets.includes('number')) {
    chars += numberChars
  }
  if (targets.length === 0 || targets.includes('symbol')) {
    chars += symbolChars
  }

  // 結合した文字セットを配列化
  const arrChars = chars.split('')

  // 配列化した文字セットをシャッフル
  for (let i = arrChars.length - 1; i > 0; i--) {
    const r = Math.floor(Math.random() * (i + 1))
    const tmp = arrChars[i]
    arrChars[i] = arrChars[r]
    arrChars[r] = tmp
  }

  // シャッフルした文字セットを結合
  let shuffledChars = arrChars.join('')

  // 生成する文字列長よりも文字セットが短い場合は同じ文字列で足りない長さを埋めていく
  if (shuffledChars.length > 0 && shuffledChars.length < length) {
    let tmp = shuffledChars
    while (true) {
      tmp += shuffledChars
      if (tmp.length > length) {
        shuffledChars = tmp
        break
      }
    }
  }

  // 結合したシャッフル済み文字セットから生成する文字列長だけ切り出す
  result = shuffledChars.slice(0, length)
  return result
}

export function signOut() {
  // VueXの削除
  window.localStorage.removeItem(`${process.env.VUEX_STORAGE_KEY}`)
  window.sessionStorage.removeItem(`${process.env.VUEX_STORAGE_KEY}`)
  // サインアウト
  return Auth.signOut()
}

export function dataToCsv(items: any, headers: boolean | any = false) {
  let output = ''
  if (headers !== false && headers !== null && typeof headers === 'object') {
    // ヘッダを指定した場合はヘッダに紐づく情報のみをCSV出力
    for (const i in headers) {
      output += '"' + String(headers[i]) + '",'
    }
    if (output.slice(-1) === ',') {
      output = output.slice(0, -1)
    }
    output += '\r\n'
    for (const i in items) {
      for (const i2 in headers) {
        if (items[i][i2]) {
          output += '"' + String(items[i][i2]) + '",'
        } else {
          output += '"",'
        }
      }
      if (output.slice(-1) === ',') {
        output = output.slice(0, -1)
      }
      output += '\r\n'
    }
  } else {
    // ヘッダを指定しない場合は全ての項目をCSV出力
    for (const i in items) {
      for (const i2 in items[i]) {
        output += '"' + String(items[i][i2]) + '",'
      }
      if (output.slice(-1) === ',') {
        output = output.slice(0, -1)
      }
      output += '\r\n'
    }
  }
  return output
}

export function downloadFile(
  fileName: string,
  content: string,
  mimeType: string,
  addBom: boolean = false
) {
  /* eslint-disable */
  const bom = new Uint8Array([0xef, 0xbb, 0xbf])
  let blob = new Blob()
  if (addBom === true) {
    blob = new Blob([bom, content], { type: mimeType })
  } else {
    blob = new Blob([content], { type: mimeType })
  }
  const url = (window.URL || window.webkitURL).createObjectURL(blob)
  const link = document.createElement('a')
  link.download = fileName
  link.href = url
  document.body.appendChild(link)
  link.click()
  document.body.removeChild(link)
}
