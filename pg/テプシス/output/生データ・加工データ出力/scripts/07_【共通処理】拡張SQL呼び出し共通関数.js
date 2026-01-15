function undefinedToNull(obj = {}) {
    return Object.fromEntries(
        Object.entries(obj).map(([k, v]) => [k, v === undefined ? null : v])
    );
}

/**
 * 拡張SQLを実行して返す共通関数
 * - 返り値は「オブジェクト配列」か「生JSON文字列」かをコメントアウトで切替
 */
async function sqlTable(name, params = {}) {
    const cleanParams = undefinedToNull(params);

    const body = {
        ApiVersion: API_VERSION,
        Name: name,
        ...(API_KEY ? { ApiKey: API_KEY } : {}),
        ...(Object.keys(cleanParams).length ? { Params: cleanParams } : {}),
    };

    console.log("request body:", body);

    const res = await fetch(ENDPOINT, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        credentials: "include",
        body: JSON.stringify(body),
    });

    if (!res.ok) {
        const text = await res.text().catch(() => "");
        throw new Error(`HTTP ${res.status} ${res.statusText}\n${text}`);
    }

    // =============================
    // 返り値の形式
    // =============================

    // // ---- A) オブジェクト配列で返す（Response.Data.Table）----
    // const json = await res.json();
    // return json?.Response?.Data?.Table ?? [];

    // ---- B) 生のJSON文字列で返す（そのままテキスト）----
    const text = await res.text();
    return text;
}


function sql(name, baseParams = {}) {
    return async (params = {}) => {
        // baseParams（既定） + params（上書き）を合体して送る
        const merged = { ...baseParams, ...params };
        return sqlTable(name, merged);
    };
}