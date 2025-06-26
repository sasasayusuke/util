// utils/IndexedDB.ts
import Dexie from 'dexie'
import { TableSchema } from '~/tables'
import { Logger } from '~/plugins/logger'

export default class IndexedDB {
  dexie: Dexie
  $logger = new Logger()

  constructor(dbName: string) {
    this.dexie = new Dexie(dbName)
    this.dexie.version(TableSchema.version).stores(TableSchema.schema)

    Object.keys(TableSchema.schema).forEach((tableName) => {
      ;(this as any)[tableName] = this.dexie.table(tableName)
    })
  }

  async add<T>(tableName: string, data: T) {
    try {
      return await this.dexie.table(tableName).add(data)
    } catch (e) {
      this.$logger.error(`[IndexedDB] add: ${tableName}`, e)
    }
  }

  async get<T>(tableName: string, key: string): Promise<T | undefined> {
    try {
      return await this.dexie.table(tableName).get(key)
    } catch (e) {
      this.$logger.error(`[IndexedDB] get: ${tableName}, key=${key}`, e)
      return undefined
    }
  }

  async put(tableName: string, item: any) {
    try {
      return await this.dexie.table(tableName).put(item)
    } catch (e) {
      this.$logger.error(`[IndexedDB] put: ${tableName}`, e)
    }
  }

  async delete(tableName: string, key: string) {
    try {
      await this.dexie.table(tableName).delete(key)
    } catch (e) {
      this.$logger.error(`[IndexedDB] delete: ${tableName}, key=${key}`, e)
    }
  }

  async getAll<T>(tableName: string): Promise<T[]> {
    try {
      return await this.dexie.table(tableName).toArray()
    } catch (e) {
      this.$logger.error(`[IndexedDB] getAll: ${tableName}`, e)
      return []
    }
  }

  async clear(tableName: string) {
    try {
      return await this.dexie.table(tableName).clear()
    } catch (e) {
      this.$logger.error(`[IndexedDB] clear: ${tableName}`, e)
    }
  }

  close() {
    try {
      this.dexie.close()
    } catch (e) {
      this.$logger.error('[IndexedDB] close', e)
    }
  }

  async deleteDatabase() {
    try {
      await this.dexie.delete()
    } catch (e) {
      this.$logger.error('[IndexedDB] deleteDatabase', e)
    }
  }
}
