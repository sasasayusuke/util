// API設定をシステム設定から取得
const API_CONFIG = {
    BASE_URL: window.location.origin,
    API_VERSION: SYSTEM_CONFIG.API.VERSION,
    TIMEOUT: SYSTEM_CONFIG.API.TIMEOUT,
    EMPTY_DATE: SYSTEM_CONFIG.API.EMPTY_DATE
};

// ベースボディ
function baseBody() {
    return { ApiVersion: API_CONFIG.API_VERSION };
}

// エラーチェック用ヘルパー関数
function isAPIError(result) {
    return result && result._error === true;
}

// エラー詳細を取得
function getErrorInfo(result) {
    if (!isAPIError(result)) {
        return null;
    }
    
    return {
        type: result._errorType,
        status: result._status,
        message: result._message,
        statusText: result._statusText,
        data: result._data,
        originalError: result._originalError
    };
}

// 更新データを5つのハッシュに分類
function splitHash(updateData) {
    const classHash = {};
    const numHash = {};
    const dateHash = {};
    const descriptionHash = {};
    const checkHash = {};
    const other = {};

    for (const [key, value] of Object.entries(updateData)) {
        if (key.includes('Class')) {
            classHash[key] = value;
        } else if (key.includes('Num')) {
            numHash[key] = value;
        } else if (key.includes('Date')) {
            dateHash[key] = value || API_CONFIG.EMPTY_DATE;
        } else if (key.includes('Description')) {
            descriptionHash[key] = value;
        } else if (key.includes('Check')) {
            checkHash[key] = value;
        } else {
            other[key] = value;
        }
    }

    return {
        ClassHash: classHash,
        NumHash: numHash,
        DateHash: dateHash,
        DescriptionHash: descriptionHash,
        CheckHash: checkHash,
        ...other
    };
}

// View設定を構築
function buildView(columns = null, setLabelText = true, setDisplayValue = true) {
    if (!columns || columns.length === 0) {
        columns = ['ClassA', 'NumA'];
    }

    return {
        ApiDataType: 'KeyValues',
        ApiColumnKeyDisplayType: setLabelText ? 'LabelText' : 'ColumnName',
        ApiColumnValueDisplayType: setDisplayValue ? 'DisplayValue' : 'Value',
        GridColumns: columns
    };
}

// 単一レコード取得
async function commonGetRecord(recordId, options = {}) {
    const {
        columns = null,
        setLabelText = true,
        setDisplayValue = true,
        ignoreErrors = false
    } = options;

    const view = buildView(columns, setLabelText, setDisplayValue);

    try {
        const body = {
            ...baseBody(),
            View: view
        };

        const response = await fetch(`${API_CONFIG.BASE_URL}/api/items/${recordId}/get`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(body)
        });

        if (!response.ok) {
            if (!ignoreErrors) {
                console.error('API Error:', response.status, response.statusText);
            }
            return {
                _error: true,
                _errorType: 'HTTP_ERROR',
                _status: response.status,
                _statusText: response.statusText,
                _message: `HTTP Error: ${response.status} ${response.statusText}`
            };
        }

        const data = await response.json();

        if (data.StatusCode !== 200 || !data.Response || !data.Response.Data) {
            if (!ignoreErrors) {
                console.error('Response Error:', data);
            }
            return {
                _error: true,
                _errorType: 'API_ERROR',
                _status: data.StatusCode,
                _message: data.Message || 'API response error',
                _data: data
            };
        }

        const result = data.Response.Data[0] || {};
        // 正常な場合はエラーフラグを追加しない
        return result;

    } catch (error) {
        if (!ignoreErrors) {
            console.error('commonGetRecord Error:', error);
        }
        return {
            _error: true,
            _errorType: 'NETWORK_ERROR',
            _message: error.message || 'Network or parsing error',
            _originalError: error
        };
    }
}

// レコード更新API
async function commonUpdateRecord(recordId, updateDict = {}, options = {}) {
    const { ignoreErrors = false } = options;

    try {
        const body = {
            ...baseBody(),
            ...splitHash(updateDict)
        };

        const response = await fetch(`${API_CONFIG.BASE_URL}/api/items/${recordId}/update`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(body)
        });

        if (!response.ok) {
            if (!ignoreErrors) {
                console.error('API Error:', response.status, response.statusText);
            }
            return null;
        }

        const result = await response.json();

        if (result.StatusCode !== 200) {
            if (!ignoreErrors) {
                console.error('Update Error:', result);
            }
            return null;
        }

        return result;

    } catch (error) {
        if (!ignoreErrors) {
            console.error('commonUpdateRecord Error:', error);
        }
        return null;
    }
}

// レコード作成API
async function commonCreateRecord(siteId, createDict = {}, options = {}) {
    const { ignoreErrors = false } = options;

    try {
        const body = {
            ...baseBody(),
            ...splitHash(createDict)
        };

        const response = await fetch(`${API_CONFIG.BASE_URL}/api/items/${siteId}/create`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(body)
        });

        if (!response.ok) {
            if (!ignoreErrors) {
                console.error('API Error:', response.status, response.statusText);
            }
            return null;
        }

        const result = await response.json();

        if (result.StatusCode !== 200) {
            if (!ignoreErrors) {
                console.error('Create Error:', result);
            }
            return null;
        }

        return result;

    } catch (error) {
        if (!ignoreErrors) {
            console.error('commonCreateRecord Error:', error);
        }
        return null;
    }
}

// レコード削除API
async function commonDeleteRecord(recordId, options = {}) {
    const {
        physical = false,
        ignoreErrors = false
    } = options;

    try {
        const body = {
            ...baseBody(),
            Physical: physical
        };

        const response = await fetch(`${API_CONFIG.BASE_URL}/api/items/${recordId}/delete`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(body)
        });

        if (!response.ok) {
            if (!ignoreErrors) {
                console.error('API Error:', response.status, response.statusText);
            }
            return null;
        }

        const result = await response.json();

        if (result.StatusCode !== 200) {
            if (!ignoreErrors) {
                console.error('Delete Error:', result);
            }
            return null;
        }

        return result;

    } catch (error) {
        if (!ignoreErrors) {
            console.error('commonDeleteRecord Error:', error);
        }
        return null;
    }
}