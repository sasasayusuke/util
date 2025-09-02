# フォルダ構造とコンテンツを取得するPowerShellスクリプト (UTF-8対応)

# PowerShellのエンコーディングをUTF-8に設定
$OutputEncoding = [System.Text.Encoding]::UTF8
[Console]::OutputEncoding = [System.Text.Encoding]::UTF8

# 最大ファイルサイズ (5MB)
$MAX_FILE_SIZE = 5 * 1024 * 1024

# 出力形式: 'json' または 'markdown'
$OUTPUT_FORMAT = 'markdown'

# ここに対象フォルダを指定
$TARGET_PATH = "C:\Users\NT-210174\Desktop\SQLserver\Scripts_バラ\SP"
$TARGET_PATH = "C:\Users\NT-210174\Desktop\SQLserver\SnwMT01見積入力_施工_倉庫_仕入元_Revit_meisai"
# $TARGET_PATH = "C:\svn\trunk\src"
# $TARGET_PATH = "C:\Users\NT-210174\Desktop\work\tool\gif-creator"



# 対象フォルダ (空の配列はすべてのフォルダが対象)
$TARGET_FOLDERS = @(
    # "backend",
    # "frontend",
    # "gitlab"
)


# 除外設定
$EXCLUDED_FOLDERS = @(
    '.history',
    '.zip',
    '.git',
    '.svn',
    '.next',
    '__pycache__',
    'node_modules',
    'gitlab',
    'log',
    'history',
    'dist',
    'Htmls',
    'Scripts',
    'Styles'
)

$EXCLUDED_EXTENSIONS = @(
    ".png",
    ".jpg",
    ".mp3",
    ".mp4",
    ".LICENSE.txt",
    ".map",
    ".exe",
    ".vbw",
    ".frx"
)

$EXCLUDED_FILES = @(
    "package-lock.json",
    "bundle.js"
)

function Get-FolderStructure {
    $structure = @{}

    Get-ChildItem -Path $TARGET_PATH -Recurse | ForEach-Object {
        $relativePath = $_.FullName.Substring($TARGET_PATH.Length + 1)
        $pathParts = $relativePath -split '\\'

        # 現在のフォルダが対象フォルダかどうかを確認
        if ($TARGET_FOLDERS.Count -eq 0 -or $TARGET_FOLDERS -contains $pathParts[0]) {
            $current = $structure

            foreach ($part in $pathParts) {
                if (-not $current.ContainsKey($part)) {
                    $current[$part] = @{}
                }
                $current = $current[$part]
            }

            if ($_.PSIsContainer) {
                # フォルダの処理
                if ($EXCLUDED_FOLDERS -contains $_.Name) {
                    $current["$($_.Name)/ (除外)"] = "除外されたフォルダ"
                }
            }
            else {
                # ファイルの処理
                if ($EXCLUDED_FILES -contains $_.Name) {
                    $current["$($_.Name) (除外)"] = "除外されたファイル"
                }
                elseif ($EXCLUDED_EXTENSIONS -contains $_.Extension) {
                    $current["$($_.Name) (除外)"] = "除外された拡張子"
                }
                elseif ($_.Length -gt $MAX_FILE_SIZE) {
                    $current[$_.Name] = "ファイルサイズが制限を超えています: $($_.Length) バイト"
                }
                else {
                    try {
                        # UTF-8エンコーディングを明示的に指定
                        $content = Get-Content -Path $_.FullName -Raw -Encoding UTF8 -ErrorAction Stop
                        $current[$_.Name] = $content
                    }
                    catch {
                        $current[$_.Name] = "ファイル読み込みエラー: $_"
                    }
                }
            }
        }
    }

    return $structure
}

function ConvertTo-MarkdownStructure {
    param (
        [Parameter(Mandatory=$true)]
        [hashtable]$Structure,

        [int]$Depth = 0
    )

    $markdown = ""
    foreach ($key in $Structure.Keys) {
        $value = $Structure[$key]
        $indent = "  " * $Depth

        if ($value -is [hashtable]) {
            $markdown += "$indent- $key`n"
            $markdown += ConvertTo-MarkdownStructure -Structure $value -Depth ($Depth + 1)
        }
        else {
            $markdown += "$indent- $key`n"
            if ($value -eq "除外されたフォルダ" -or $value -eq "除外されたファイル" -or $value -eq "除外された拡張子") {
                $markdown += "$indent  $value`n"
            }
            elseif ($value -is [string] -and -not $value.StartsWith("ファイルサイズが制限を超えています") -and -not $value.StartsWith("ファイル読み込みエラー")) {
                $markdown += "=================================================================================================`n"
                $markdown += "$value`n"
                $markdown += "=================================================================================================`n"
            }
            else {
                $markdown += "$indent  $value`n"
            }
        }
    }

    return $markdown
}

function Export-Structure {
    param (
        [Parameter(Mandatory=$true)]
        [hashtable]$Structure,

        [string]$OutputFormat = 'json'
    )

    if ($OutputFormat -eq 'json') {
        $output = $Structure | ConvertTo-Json -Depth 100 -Encoding UTF8
    }
    elseif ($OutputFormat -eq 'markdown') {
        $output = ConvertTo-MarkdownStructure -Structure $Structure
    }
    else {
        throw "無効な出力形式です。'json' または 'markdown' を選択してください。"
    }

    if ($output.Length -gt $MAX_FILE_SIZE) {
        Write-Warning "出力サイズ ($($output.Length) バイト) が最大許容サイズ ($MAX_FILE_SIZE バイト) を超えています。"
        Write-Warning "出力は切り捨てられます。"
        $output = $output.Substring(0, $MAX_FILE_SIZE) + "`n...(切り捨てられました)"
    }

    Write-Host "`nフォルダ構造とコンテンツ ($OutputFormat 形式):"
    Write-Host $output

    try {
        Set-Clipboard -Value $output
        Write-Host "`nフォルダ構造とコンテンツ (サイズ: $($output.Length) バイト) をクリップボードにコピーしました。"
    }
    catch {
        Write-Host "`nクリップボードへのコピー中にエラーが発生しました: $_"
        Write-Host "出力はコンソールにのみ表示されます。"
    }
}

# メイン実行
$result = Get-FolderStructure
Export-Structure -Structure $result -OutputFormat $OUTPUT_FORMAT