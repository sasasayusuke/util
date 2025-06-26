export interface IndexedDbTables {
  version: number
  schema: {
    // ｛table名: 主キー｝
    [tableName: string]: string
  }
}

/*
  IndexedDBのテーブルを定義する
  更新または追加した場合は、versionをインクリメントすること
 */
export const TableSchema: IndexedDbTables = {
  version: 1,
  schema: {
    karte: 'karteId',
  },
}

export const Tables = {
  karte: 'karte',
}
