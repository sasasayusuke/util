/**
 * Pleasanter API ユーティリティクラス（バニラJS版）
 * Python FastAPI の pleasanter/service.py を JavaScript に移植
 */

/**
 * 定数
 */
const EMPTY_DATE = '1899-12-30T00:00:00';
const PAGE_SIZE = 200;

/**
 * Pleasanter API クライアント
 */
class PleasanterAPI {
  /**
   * @param {string} baseUrl - PleasanterのベースURL (例: location.origin)
   * @param {Object} options - オプション
   * @param {string} [options.apiKey] - APIキー（認証が必要な場合）
   * @param {string} [options.apiVersion] - APIバージョン（デフォルト: 1.1）
   * @param {boolean} [options.logging] - ログ出力（デフォルト: false）
   */
  constructor(baseUrl, options = {}) {
    if (!baseUrl) throw new Error('baseUrl は必須です');

    this.baseUrl = baseUrl;
    this.apiKey = options.apiKey || null;
    this.apiVersion = options.apiVersion || 1.1;
    this.logging = options.logging === true;
  }

  /**
   * 基本リクエストボディ
   * @private
   */
  _baseBody() {
    const body = { ApiVersion: this.apiVersion };
    if (this.apiKey) {
      body.ApiKey = this.apiKey;
    }
    return body;
  }

  /**
   * update_dict を 5 つのハッシュに分類
   * @private
   */
  _splitHash(update) {
    const classHash = {};
    const numHash = {};
    const dateHash = {};
    const descriptionHash = {};
    const checkHash = {};
    const other = {};

    for (const [k, v] of Object.entries(update)) {
      if (k.includes('Class')) {
        classHash[k] = v;
      } else if (k.includes('Num')) {
        numHash[k] = v;
      } else if (k.includes('Date')) {
        dateHash[k] = v || EMPTY_DATE;
      } else if (k.includes('Description')) {
        descriptionHash[k] = v;
      } else if (k.includes('Check')) {
        checkHash[k] = v;
      } else {
        other[k] = v;
      }
    }

    return {
      ClassHash: classHash,
      NumHash: numHash,
      DateHash: dateHash,
      DescriptionHash: descriptionHash,
      CheckHash: checkHash,
      ...other,
    };
  }

  /**
   * View セクション構築
   * @private
   */
  _buildView(columns, options = {}) {
    const { setLabelText = true, setDisplayValue = 'DisplayValue' } = options;

    const cols = columns || ['ClassA', 'NumA'];

    return {
      ApiDataType: 'KeyValues',
      ApiColumnKeyDisplayType: setLabelText ? 'LabelText' : 'ColumnName',
      ApiColumnValueDisplayType: setDisplayValue,
      GridColumns: cols,
    };
  }

  /**
   * HTTP POST リクエスト送信
   * @private
   */
  async _request(endpoint, body, options = {}) {
    const { ignoreErrors = false } = options;

    const headers = {
      'Content-Type': 'application/json',
    };

    try {
      const res = await fetch(`${this.baseUrl}${endpoint}`, {
        method: 'POST',
        headers,
        body: JSON.stringify(body),
      });

      if (!res.ok) {
        if (ignoreErrors) {
          console.error(`Pleasanter API Error: ${res.status}`);
          return null;
        }
        throw new Error(`HTTP Error: ${res.status} ${res.statusText}`);
      }

      const data = await res.json();
      return data;
    } catch (error) {
      if (ignoreErrors) {
        console.error('Pleasanter API Request Failed:', error);
        return null;
      }
      throw error;
    }
  }

  /**
   * 単一レコード取得
   * @param {number|string} recordId - レコードID
   * @param {Object} [options] - オプション
   * @param {string[]} [options.columns] - 取得するカラム
   * @param {boolean} [options.setLabelText] - カラムキーをラベル名で表示（デフォルト: true）
   * @param {'DisplayValue'|'Value'|'Both'} [options.setDisplayValue] - 値の表示形式（デフォルト: 'DisplayValue'）
   * @param {boolean} [options.ignoreErrors] - エラーを無視（デフォルト: false）
   * @returns {Promise<Object>} レコードデータ
   */
  async getRecord(recordId, options = {}) {
    if (!recordId) throw new Error('recordId は必須です');

    const {
      columns = null,
      setLabelText = true,
      setDisplayValue = 'DisplayValue',
      ignoreErrors = false,
    } = options;

    if (setDisplayValue === 'Both') {
      // DisplayValueとValueの両方を取得してマージ
      const displayRecord = await this.getRecord(recordId, {
        ...options,
        setDisplayValue: 'DisplayValue',
      });
      const valueRecord = await this.getRecord(recordId, {
        ...options,
        setDisplayValue: 'Value',
      });

      // マージ
      const merged = {};
      for (const key of Object.keys(displayRecord)) {
        merged[key] = {
          Display: displayRecord[key],
          Value: valueRecord[key],
        };
      }
      return merged;
    }

    // 単一形式で取得
    const view = this._buildView(columns, { setLabelText, setDisplayValue });
    const body = { ...this._baseBody(), View: view };

    const resp = await this._request(`/api/items/${recordId}/get`, body, { ignoreErrors });

    if (!resp) {
      return {};
    }

    return resp.Response?.Data?.[0] || {};
  }

  /**
   * レコード一覧取得（ページネーション対応・全件取得）
   * @param {number|string} tableId - テーブル（サイト）ID
   * @param {Object} [options] - オプション
   * @param {string[]} [options.columns] - 取得するカラム
   * @param {boolean} [options.setLabelText] - カラムキーをラベル名で表示（デフォルト: true）
   * @param {'DisplayValue'|'Value'|'Both'} [options.setDisplayValue] - 値の表示形式（デフォルト: 'DisplayValue'）
   * @param {boolean} [options.ignoreErrors] - エラーを無視（デフォルト: false）
   * @returns {Promise<Object[]>} レコード配列
   */
  async getRecords(tableId, options = {}) {
    if (!tableId) throw new Error('tableId は必須です');

    const {
      columns = null,
      setLabelText = true,
      setDisplayValue = 'DisplayValue',
      ignoreErrors = false,
    } = options;

    if (setDisplayValue === 'Both') {
      // DisplayValueとValueの両方を取得してマージ
      const displayRecords = await this.getRecords(tableId, {
        ...options,
        setDisplayValue: 'DisplayValue',
      });
      const valueRecords = await this.getRecords(tableId, {
        ...options,
        setDisplayValue: 'Value',
      });

      // マージ
      const merged = displayRecords.map((displayRec, index) => {
        const valueRec = valueRecords[index];
        const mergedRec = {};
        for (const key of Object.keys(displayRec)) {
          mergedRec[key] = {
            Display: displayRec[key],
            Value: valueRec[key],
          };
        }
        return mergedRec;
      });

      if (this.logging) {
        console.log(`テーブル(${this.baseUrl}/items/${tableId}/index)から ${merged.length} 件取得しました(Both形式)`);
        console.table(merged);
      }
      return merged;
    }

    // 単一形式で取得（ページネーション対応）
    const view = this._buildView(columns, { setLabelText, setDisplayValue });
    const records = [];
    let offset = 0;

    while (true) {
      const body = {
        ...this._baseBody(),
        Offset: offset,
        PageSize: PAGE_SIZE,
        View: view,
      };

      const resp = await this._request(`/api/items/${tableId}/get`, body, { ignoreErrors });

      if (!resp) {
        break;
      }

      const page = resp.Response?.Data || [];
      records.push(...page);

      if (page.length < PAGE_SIZE) {
        break;
      }

      offset += PAGE_SIZE;
    }

    if (this.logging) {
      console.log(`テーブル(${this.baseUrl}/items/${tableId}/index)から ${records.length} 件取得しました`);
      console.table(records);
    }
    return records;
  }

  /**
   * レコード作成
   * @param {number|string} tableId - テーブル（サイト）ID
   * @param {Object} createData - 作成するレコードデータ
   * @returns {Promise<Object>} 作成結果
   */
  async createRecord(tableId, createData = {}) {
    if (!tableId) throw new Error('tableId は必須です');
    if (!createData || Object.keys(createData).length === 0) {
      throw new Error('createData は必須です');
    }

    const payload = {
      ...this._baseBody(),
      ...this._splitHash(createData),
    };

    const data = await this._request(`/api/items/${tableId}/create`, payload);

    if (!data) {
      return {};
    }

    if (this.logging) {
      console.log('登録内容:', data);
    }
    return data;
  }

  /**
   * レコード更新
   * @param {number|string} recordId - 更新するレコードID
   * @param {Object} updateData - 更新データ
   * @returns {Promise<Object>} 更新結果
   */
  async updateRecord(recordId, updateData = {}) {
    if (!recordId) throw new Error('recordId は必須です');
    if (!updateData || Object.keys(updateData).length === 0) {
      throw new Error('updateData は必須です');
    }

    const payload = {
      ...this._baseBody(),
      ...this._splitHash(updateData),
    };

    const data = await this._request(`/api/items/${recordId}/update`, payload);

    if (!data) {
      return {};
    }

    if (this.logging) {
      console.log('更新内容:', data);
    }
    return data;
  }

  /**
   * レコード削除
   * @param {number|string} recordId - 削除するレコードID
   * @returns {Promise<Object>} 削除結果
   */
  async deleteRecord(recordId) {
    if (!recordId) throw new Error('recordId は必須です');

    const payload = this._baseBody();

    const data = await this._request(`/api/items/${recordId}/delete`, payload);

    if (!data) {
      return {};
    }

    if (this.logging) {
      console.log(`削除完了: レコードID ${recordId}`);
    }
    return data;
  }
}
