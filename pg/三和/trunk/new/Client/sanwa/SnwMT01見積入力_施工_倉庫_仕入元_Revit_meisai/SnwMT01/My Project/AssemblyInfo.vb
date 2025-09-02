Imports System
Imports System.Reflection
Imports System.Runtime.InteropServices

' アセンブリに関する一般情報は以下を通して制御されます
' アセンブリに関連付けられている情報を変更するには、
' これらの属性値を変更してください。

' アセンブリ属性の値を確認します

<Assembly: AssemblyTitle("積算・販売管理システム 2024")>
<Assembly: AssemblyDescription("見積入力")>
<Assembly: AssemblyCompany("三和商研株式会社")>
<Assembly: AssemblyProduct("SnwMT01")>
<Assembly: AssemblyCopyright("Copyright © Sanwa Co.,Ltd.")>
<Assembly: AssemblyTrademark("")>
<Assembly: AssemblyCulture("")>

<Assembly: ComVisible(False)>

'このプロジェクトが COM に公開される場合、次の GUID が typelib の ID になります
<Assembly: Guid("c20fa8a7-a147-44f1-a305-6d57d419af10")>

' アセンブリのバージョン情報は次の 4 つの値で構成されています:
'
'      メジャー バージョン
'      マイナー バージョン
'      ビルド番号
'      Revision
'
' すべての値を指定するか、次を使用してビルド番号とリビジョン番号を既定に設定できます
' 既定値にすることができます:
' <Assembly: AssemblyVersion("1.0.*")>

<Assembly: AssemblyVersion("1.0.0.0")>
<Assembly: AssemblyFileVersion("1.0.0.0")>

' 現在は使われていない
#If DEBUG Then
<Assembly: AssemblyConfiguration("Debug")>
#Else
<Assembly: AssemblyConfiguration("Release")>
#End If
