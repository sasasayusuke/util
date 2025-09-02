Option Strict Off
Option Explicit On

''' <summary>
''' --------------------------------------------------------------------
'''   ユーザー名           株式会社三和商研
'''   業務名               積算データ管理システム
'''   部門名               見積入力部門
'''   プログラム名         見積入力
'''   作成会社             テクノウェア株式会社
'''   作成日               2003/06/03
'''   作成者               oosawa
''' --------------------------------------------------------------------
'''    UPDATE
'''        2003/11/05  oosawa      表示項目設定追加(SnwMT02F10)
'''        2003/11/18  oosawa      原価の掛率で売価を出すように追加
'''                                SnwMT02F11画面追加
'''        2003/11/18  oosawa      OPボタンにF7情報を追加
'''        2003/11/21  oosawa      F7情報時エラー内容項目を消す
'''        2003/11/25  oosawa      員数チェック時もとの場所に戻るように変更
'''        2003/12/02  oosawa      前回単価参照にサイズを追加
'''        2003/12/06  oosawa      前回単価参照を得意先で抽出するように変更
'''        2004/01/22  oosawa      EXCEL取込で桁を切り捨てるように変更
'''        2004/02/05  oosawa      選択画面からだとHOLDしないのをするように修正
'''        2004/03/05  oosawa      テンプレート取り込みで全て直送にする
'''        2004/03/05  oosawa      原価率での売価算出を追加
'''        2004/03/29  oosawa      Excel出力パス名を「現場名」から「見積件名」に変える
'''        2004/04/01  oosawa      売価変更でゼロ割・オーバーフローが出るのを修正
'''        2005/03/26  oosawa      エラー行番号をメッセージに出すように変更
'''        2005/07/05  kawamura    [固定列]設定機能の追加。設定値はiniファイルに保持
'''        2005/07/05  kawamura    選択項目のリスト幅をiniファイルに保持
'''        2005/11/01  kawamura    [U区分]項目の追加。Uを入力行は背景色を黄色にする
'''        2006/11/02  oosawa      前回単価参照に「見積件名」「仕入先」を追加し、100件にした
'''        2006/11/15  kawamura    前回単価参照で単価選択時の項目追加による不具合
'''        2007/02/16  kawamura    見積選択表示で見積日付を[mm/dd]→[yy/mm/dd]
'''        2007/03/09  oosawa      単価参照で仕入単価・売上単価両方０以外は出るように変更
'''        2007/11/17  oosawa      テンプレート選択の明細数を200から300に変更
'''        2008/01/23  oosawa      原価率での制限を設ける。
'''        2008/02/20  oosawa      原価率でのエラーメッセージが実行環境でうまくいかないので
'''                                Msgboxをエラー項目に変更
'''        2008/05/22  oosawa      員数部のクリアを追加
'''        2008/06/10  oosawa      員数部の幅を広がるようにする
'''        2009/02/26  oosawa      2008/05/22に社内在庫～転用までのクリアを追加
'''        2009/07/28  oosawa      単価検索に得意先・仕入先を追加
'''        2009/08/13  oosawa      開始納期は必ず入力するように変更（在庫の関係）
'''        2009/09/26  oosawa      社内在庫参照数・客先在庫参照数の表示
'''                                F08・F09を変更し、
'''                                社内在庫履歴表示(F08)・客先在庫履歴表示(F09)を追加
'''        2009/11/16  oosawa      単価参照でTimeoutが発生
'''        2009/11/26  oosawa      納期Sを未入力対応に変更（在庫関係はでなくなる）
'''        2009/12/02  oosawa      担当者での抽出を追加
'''        2010/04/08  oosawa      区分A・Sの場合の仕入先・配送先クリアが抜けていた。
'''        2010/04/23  oosawa      次画面OPEN時、前画面が消えないことがあるのでおまじない（DOEVENTS）を入れる
'''        2010/05/06  oosawa      処理種別で0～2以外が入力できてしまう
'''        2010/06/16  oosawa      コメント（ACS）はチェック時にクリアするように変更
'''        2011/11/29  oosawa      見積区分での抽出追加
'''                                売上単価ﾏｽﾀとの単価違いをエラーとして表示する
'''                                仕分レベルの時間を未入力時00:00をセットし、入力しなくても良いようにする
'''                                    (F02)時間がスペースならば00:00をセット
'''        2011/12/26  oosawa      UPDATE時、DB接続エラーにて再接続できるように修正
'''        2012/07/02  oosawa      F05、見積NO+行NOでの連結で、売上Dの単価・仕入Dの単価を表示するように修正
'''        2012/08/21  oosawa      物件種別に5:内装を追加
'''        2012/09/06  oosawa      物件種別をスタテッククラスに変更し名称を持ってくる
'''        2012/10/03  oosawa      F05で、日付を新設し、日付以前の100件で検索できるように修正
'''        2013/03/08  oosawa      部署CD・社内伝用得意先項目追加
'''        2013/03/28  oosawa      ドラッグモードボタンの追加
'''        2013/04/09  oosawa      本発注から仮発注になった場合、発注情報を削除するように変更
'''        2013/07/19  oosawa      社内在庫参照・客先在庫参照をやめる
'''                                「社内伝票扱い」追加
'''        2013/08/08  oosawa      売上・仕入更新済みの場合登録不可にする
'''        2013/08/09  oosawa      見積区分を廃止する
'''        2013/09/02  oosawa      仕分レベル設定に日付一括を追加
'''        2013/09/18  oosawa      売上・仕入済みでも変更可能に
'''                                売上日・仕入日を表示
'''        2013/12/26  oosawa      売上・仕入が両方更新済みならば登録不可にする（2013/08/08の修正）
'''        2014/04/22  oosawa      受注区分を初期値１に
'''                                受注日の初期値をオペ日に
'''        2014/04/25  oosawa      納入先に「9999」を入力できないように変更
''' 
'''        2014/07/10  oosawa      中国 切替　バージョン
'''        2014/09/09  oosawa      発注調整欄追加
'''                                仕入済数表示
'''        2014/09/15  oosawa      売上・仕入済みの場合、社内伝に変更できないように変更
'''        2014/11/03  oosawa      売上済CNT表示・ロック
'''        2014/11/11  oosawa      行入替モジュールブロック対応
'''        2014/11/18  oosawa      新売上バージョン
'''
'''        2015/01/09  oosawa      仕入日を未仕入行数に変更
'''        2015/01/13  oosawa      削除メッセージを3回に増やした！！！
'''        2015/01/20  oosawa      更新済を一時解除する。登録できなくなった。
'''        2015/01/24  oosawa      非課税の考えを導入
'''        2015/01/30  oosawa      得意先3055の場合、販売の部分を入力可能にする。
'''        2015/02/04  oosawa      他社伝票番号を追加
'''        2015/05/15  oosawa      現場名を28から40桁に変更
'''        2015/05/28  oosawa      物件種別検索を追加
'''        2015/06/19  oosawa      仕様NOを範囲指定に変更
'''        2015/06/29  oosawa      販売先に3055の入力を不可にする。
'''        2015/07/10  oosawa      物件番号の入力追加
'''        2015/07/20  oosawa      仕入単価マスタの追加（仕入変更時単価参照）
'''        2015/08/28  oosawa      Wel要望により桁指定の税込端数計算を追加
'''        2015/09/28  oosawa      売上仕入単価マスタ追加
'''                                仕入単価マスタの廃止
'''        2015/09/29  oosawa      しまむら用明細納品日付の欄を追加
'''        2015/10/07  oosawa      仕分レベルの桁数を20→40へ変更
'''        2015/10/13  oosawa      しまむら伝票種類に3:FAX消耗品 4:FAX直納を追加
'''        2015/10/16  oosawa      YM関係の項目追加
'''        2015/10/19  oosawa      Wel1%計算を追加（Z???:経費を除く）
'''        2015/10/26  oosawa      名称を在庫管理する製品のみロックする
'''        2015/11/03  oosawa      Wel化粧品メーカー用に在庫管理除外区分を新設
'''        2015/11/13  oosawa      2015/10/26修正分のロックをサイズのみにする
'''        2015/11/19  oosawa      SM内容区分追加（しまむら内の分割　0:なし 1:ﾃﾞｨﾊﾞﾛ 2:台湾しまむら 3:システム開発）
'''        2015/11/26  oosawa      選択画面に物件番号を表示
'''        2015/12/01  oosawa      チェックボックスのチェンジイベントが発生してしまうのを除外
'''        2016/02/09  oosawa      Wel1%を製品マスタの費用区分で計算するように変更
'''        2016/03/07  oosawa      他社部門CDを仕分レベル設定に追加
'''        2016/03/10  oosawa      INIファイルで得意先：三和商研3055が検索できないように制御
'''                                製品NO:拓Z204が検索できないように制御
'''        2016/04/07  oosawa      「得意先に税集計区分3：税対象外を追加」に伴う修正
'''                                税集計区分を変更可能に
'''        2016/06/22  oosawa      クレーム明細区分（1:クレーム）
'''                                作業区分CD（1:施工 2:コール 3:内装）を追加
'''        2016/06/23  oosawa      U区分でRGBでの色付け追加
'''        2016/08/01  oosawa      データセット時、配列セットのミスにより最終行がEmptyで上書きされていた。
'''        2016/08/03  oosawa      F14:しまむら仕様NOに無条件でSを入れる
'''        2016/08/26  oosawa      しまむら伝票種類 FAXをmailに変更
'''        2016/10/26  oosawa      開始納期の範囲指定を追加
'''                                他社伝番・納品日のロック解除は保留
'''        2016/11/11  oosawa      社内在庫欄にマイナスが入るように変更
'''        2016/12/21  oosawa      得意先：9999はエラー（社内伝票扱いで処理）
'''                                社内在庫・発注調整数欄は使用不可。後で非表示にする。
'''        2017/01/06  oosawa      しまむら消耗品取込変更3150用
'''        2017/01/17  oosawa      Welの1%を変動できるように変更
'''        2017/02/06  oosawa      作業区分に4:クレームを追加
'''                                tx_クレーム区分を非表示
'''        2017/02/08  oosawa      見積ｺﾋﾟｰ機能停止（社長命令）
'''        2017/02/10  oosawa      製品でのロックの不具合修正
'''                                社内伝の場合、仕入先3150：三和倉庫は指定できないようにする
'''                                社内在庫数を取り込まないように変更
'''        2017/04/07  oosawa      新規作成時のロック情報生成
'''        2017/07/03  oosawa      R:赤･B:青･H:灰のカラーを追加
'''        2017/10/05  oosawa      員数取込・ＳＭ取込時、行をつぶさないように修正
'''        2017/12/20  oosawa      ENTERKEYでの移動方向変更
'''        2018/03/26  oosawa      ログインIDで制御
'''                                    保存ができないように変更
'''        2018/04/10  oosawa      レビットの集計データ取込作成F15
'''        2018/04/11  oosawa      保存時に再表示するように変更（見積明細連番の為）
'''        2018/06/19  oosawa      コメント行の書き換え防止に仕入済CNTを新設
'''        2018/09/29  oosawa      見積明細連番の桁を9桁へ変更
'''        2018/11/01  oosawa      原価合計を表示
'''        2018/11/03  oosawa      F12員数取込で行指定を追加
'''        2019/12/12  oosawa      レビット用追番Rを追加
'''                                レビット取込変更
'''        2019/12/24  oosawa      消費税の計算がうまくいっていないのでチェック時に再計算するように変更
'''        2020/09/16  oosawa      入出庫日を入庫日と出庫日に分割
'''        2020/10/30  oosawa      ロックできる（開ける）データを３までに制御する
'''        2021/02/25  oosawa      統計用集計CDを追加
'''        2021/04/11  oosawa      見積確定区分を選択画面へ表示
'''        2021/10/08  oosawa      YK物件区分に「3：担当者案件」を追加（稟議書）
'''        2021/11/09  oosawa      YKサプライ区分の名称を「請求管轄」へ
'''                                YKサプライ区分の「2:システム」を「2:情報システム」へ
'''                                YK請求区分の3:ﾁｪｰﾝｽﾄｱの名称に伝票を追加
'''        2022/08/31  oosawa      物件種別に6:委託　追加
'''                                統計集計で委託を集計する為
'''        2022/09/02  oosawa      作業区分3：内装の削除
'''        2022/10/10  oosawa      ベルク請求管轄区分・BtoB番号追加
'''        2022/11/11  oosawa      EXCEL取込時エラー箇所を簡易的に表示するように
'''        2022/11/18  oosawa      担当者・工事担当は仕入・売上計上後変更不可
'''        2022/11/21  oosawa      物件種別5:その他は入力不可にする
'''        2022/11/22  oosawa      物件種別5:その他は入力可にする
'''                                工事担当CD:サポート(98)は物件種別5,6、
'''                                工事担当CD:コールセンター(99)は物件種別2、
'''                                98･99以外は0,1,4のみ入力可能とする。
''' 
'''        2023/04/04  oosawa      作業区分3：内装の削除の復活
'''        2023/04/07  oosawa      コピー時、単価はコピーしない　（保留中）
'''        2023/04/18  oosawa      tx_業種区分の追加
'''                                物件種別から4:内装の削除
'''        2023/05/11  oosawa      コピー時、単価はコピーしない
'''        2023/07/18  oosawa      サイズ変更できてしまう不具合修正
'''        2023/09/26  oosawa      仕入先が非インボイス(番号なし)の場合、行全体の文字を赤くする
'''        2023/10/20  oosawa      2016/04/07追加の税集計区分を非表示に変更（ﾃﾞｻﾞｲﾝ画面で非表示にしただけ）
'''                                    変な修正をしてしまう為
'''        2023/10/31  oosawa      担当者CDを2桁から3桁へ変更（社員番号変更に伴う変更）
'''        2024/01/18  oosawa      統合用見積番号入力欄を追加
'''        2024/01/31  oosawa      工事担当サポート削除
'''        2024/01/31  oosawa      サポートを委託に変更し、6：委託のみ登録可とする
'''                                5：その他はなくす (岩間指示・大道了解)
'''        2024/02/14  oosawa      業種区分も仕入・売上後は変更不可に
'''        2024/02/20  oosawa      コード入力時、労務費の場合は作業区分を1:工事にする。
'''                                物件種別が2：メンテの場合は作業区分を2：コールセンターにする。
'''        2024/03/27  oosawa      入庫日のロックはしない
'''        2024/04/26  oosawa      Col見積区分のロックはしない
'''        2024/06/21  oosawa      見積確定区分を明細側へ移動
'''        2024/06/25  oosawa      業種区分1内装の場合、作業区分が0or1の場合は3に更新する
'''        2024/07/16  oosawa      発注済の区分としてU区分にZを入力できるように変更(色付)
'''        2024/07/19  oosawa      製品取得時に売上仕入単価マスタから単価を取ってきているが小数点以下が落ちていたので修正
'''        2024/07/23  oosawa      完工日付と完了日付を統合（テーブルは残してある）
''' 途中↓
''' --------------------------------------------------------------------
''' </summary>
Friend Class SnwMT01F00
    Inherits System.Windows.Forms.Form

	Dim ReturnF As Boolean 'リターンキー時（確定時）True
	Dim SelectF As Boolean '選択画面制御用
	Dim Loaded As Boolean 'FormLoad成功
	Dim Printed As Boolean 'FormPrint成功
	Dim PreviousControl As System.Windows.Forms.Control 'コントロールの戻りを制御
	Dim ModeF As Integer '処理区分   1：登録 2：修正

	Dim MeWidth, MeHeight As Integer '起動時のフォームサイズHold用

	'ボタン２重起動防止フラグ(cbFunc_Clickで使用)
	Dim CLK2F As Boolean '２重実行制御用

	'日付用クラス
	'UPGRADE_WARNING: 配列を New で宣言することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC9D3AE5-6B95-4B43-91C7-28276302A5E8"' をクリックしてください。
	Dim idc(8) As iDate

	'データHold用ワーク
	Dim HTANTOCD As Object 'HoldID
	Dim HBUSYOCD As Object 'HoldID
	Dim HTOKUCD As Object
	Dim HNTOKUCD As Object
	Dim HNONYUCD As Object
	Dim H_HANTOKUCD As Object
	Dim H_HANNTOKUCD As Object
	Dim H_HANNONYUCD As Object
	Dim H_社内伝票扱い As Object '2014/09/15 ADD
	Dim H_出精値引 As Object '2014/10/28 ADD
	Dim HWELNAICD As Object '2015/06/12 ADD
	Dim HWELBKNCD As Object '2015/06/12 ADD
	Dim HBUKKENNO As Object '2015/07/10 ADD
	'Dim HSUISHINCD  As Variant      '2019/12/12 ADD
	Dim HKANRYOBI As Object '2020/04/11 ADD
	Dim HSYUKEICD As Object '2021/02/25 ADD
	Dim HKOUJITANTOCD As Object '2022/08/08 ADD
	Dim HBUKKENSYU As Object '2022/11/22 ADD
	Dim H統合見積NO As Object '2024/01/16 ADD
	Dim H業種区分 As Object '2024/02/14 ADD

	'別画面より呼び出しの場合
	Dim pParentForm As SnwMT01F00S '呼び出し親フォーム
	Dim pMituNo As Integer '呼び出し見積№
	Dim pEndMsg As Boolean '終了メッセージをだすか？

	'クラス
	Private cTanto As clsTanto
	Private cBusyo As clsBusyo
	Private cTokuisaki As clsTokuisaki
	Private cNonyusaki As clsNonyusaki
	Private cWelBukkenNaiyo As clsWelBukkenNaiyo
	Private cWelBukkenKubun As clsWelBukkenKubun
	Private cBukken As clsBukken '2015/07/10 ADD
	Private cMitumoriH As clsMitumoriH '2024/01/16 ADD

	'Private cSuishin As clsSuishin '2019/12/12 ADD
	Private cKoujiTanto As clsKoujiTanto '2022/08/08 ADD

	Private cSyuukei As clsTokuisaki '2021/02/25 ADD

	'工事担当CD　'2022/11/22 ADD↓
	Private Const SUPPORT_CD As String = "998"
	Private Const CALLC_CD As String = "999"
	'2022/11/22 ADD↑

    'List配列CbFuncのButton
    Private LcbFunc As New List(Of Button)

	'親フォームをセット
	'UPGRADE_NOTE: ParentForm は ParentForm_Renamed にアップグレードされました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"' をクリックしてください。
	WriteOnly Property ParentForm_Renamed() As System.Windows.Forms.Form
		Set(ByVal Value As System.Windows.Forms.Form)
			pParentForm = Value
		End Set
	End Property

	'見積№をセット
	WriteOnly Property MituNo() As Integer
		Set(ByVal Value As Integer)
			pMituNo = Value
		End Set
	End Property

	'終了メッセージの表示抑制用
	WriteOnly Property EndMsg() As Boolean
		Set(ByVal Value As Boolean)
			pEndMsg = Value
		End Set
	End Property

	'製品ｺｰﾄﾞをセット
	'Property Let ResCode(ByRef code As String)
	'    ResultCode = code
	'End Property
	'
	'呼び出されたかフラグをセット
	'Property Let ResCallF(ByRef New_ResCallF As Boolean)
	'    ResultCallF = New_ResCallF
	'End Property

	Private Sub SnwMT01F00_Paint(sender As Object, e As PaintEventArgs) Handles Me.Paint
		Debug.WriteLine("SnwMT01F00 Paint")
		If Loaded = False Then Exit Sub
		'If Printed Then Exit Sub
		Printed = True
		' Graphics オブジェクトを取得
		Dim g As Graphics = e.Graphics

		' DPIを取得
		Dim dpiX As Single = g.DpiX ' 水平方向のDPI
		Dim dpiY As Single = g.DpiY ' 垂直方向のDPI

		' 1ポイントをピクセルに変換
		Dim lineWidth As Single = 1 * (dpiX / 72)

		' 線を描く、見積番号
		Dim pen As New Pen(Color.Black, lineWidth)
		g.DrawLine(pen, 12, 38, 220, 38)

		pen = New Pen(Color.Gray, lineWidth)

		' 四角形を描く、物件情報
		Dim rect As New Rectangle(107, 44, 594, 44)
		g.DrawRectangle(pen, rect)

		' 四角形を描く、見積情報
		rect = New Rectangle(107, 92, 594, 69)
		g.DrawRectangle(pen, rect)

		' 四角形を描く、得意先
		rect = New Rectangle(107, 164, 594, 93)
		g.DrawRectangle(pen, rect)

		' 四角形を描く、納入先
		rect = New Rectangle(107, 260, 594, 89)
		g.DrawRectangle(pen, rect)

		' 四角形を描く、販売先
		rect = New Rectangle(107, 352, 594, 69)
		g.DrawRectangle(pen, rect)

		' 四角形を描く、納期
		rect = New Rectangle(107, 424, 594, 29)
		g.DrawRectangle(pen, rect)

		' 四角形を描く、備考
		rect = New Rectangle(107, 456, 594, 29)
		g.DrawRectangle(pen, rect)

		' 四角形を描く、物件情報
		rect = New Rectangle(107, 488, 594, 149)
		g.DrawRectangle(pen, rect)

		' 四角形を描く、しまむら情報
		rect = New Rectangle(807, 44, 420, 49)
		g.DrawRectangle(pen, rect)

		' 四角形を描く、ウェルシア情報
		rect = New Rectangle(807, 96, 420, 89)
		g.DrawRectangle(pen, rect)

		' 四角形を描く、ヤオコー情報
		rect = New Rectangle(807, 188, 420, 69)
		g.DrawRectangle(pen, rect)

		' 四角形を描く、ベルク情報
		rect = New Rectangle(807, 260, 420, 29)
		g.DrawRectangle(pen, rect)

		' 四角形を描く、見積コントロール
		rect = New Rectangle(807, 292, 420, 209)
		g.DrawRectangle(pen, rect)

		' 四角形を描く、経過情報
		rect = New Rectangle(807, 504, 420, 132)
		g.DrawRectangle(pen, rect)

		' リソースを解放
		pen.Dispose()
	End Sub

	Private Sub SnwMT01F00_Load(sender As Object, e As EventArgs) Handles Me.Load
		Loaded = False
		pEndMsg = True

		Debug.WriteLine("SnwMT01F00 Load")

		'    Set Me.Icon = LoadPicture("snwIcon.ico", , 2) '2021/01/17 ADD

		Dim args As String() = Environment.GetCommandLineArgs()
		If Not args.Any() Then
			Call CriticalAlarm("このプログラムは正しく起動していません。", Me.Text)
			Call Me.Close()
			Exit Sub
		ElseIf Environment.GetCommandLineArgs(0).ToUpper().Contains("DEBUG") Then
			INI.CONNECT = args(1)
		Else
			INI.CONNECT = args(1)
		End If

		'    'ｱﾌﾟﾘｹｰｼｮﾝ初期設定
		'    If ApplicationInit = False Then
		'        Call Unload(Me)
		'        Exit Sub
		'    End If

		'フォームを画面の中央に配置
		Me.Top = VB6Conv.TwipsToPixelsY((VB6Conv.PixelsToTwipsY(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height) - VB6Conv.PixelsToTwipsY(Me.Height)) \ 2)
		Me.Left = VB6Conv.TwipsToPixelsX((VB6Conv.PixelsToTwipsX(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width) - VB6Conv.PixelsToTwipsX(Me.Width)) \ 2)

		'クラス生成
		cTanto = New clsTanto
		cBusyo = New clsBusyo
		cTokuisaki = New clsTokuisaki
		cNonyusaki = New clsNonyusaki
		cWelBukkenNaiyo = New clsWelBukkenNaiyo
		cWelBukkenKubun = New clsWelBukkenKubun
		cBukken = New clsBukken '2015/07/10 ADD

		'    Set cSuishin = New clsSuishin '2019/12/12 ADD
		cKoujiTanto = New clsKoujiTanto '2022/08/08 ADD

		cSyuukei = New clsTokuisaki '2021/02/25 ADD

		cMitumoriH = New clsMitumoriH '2024/01/16 ADD

		'NOTE SS IDC() NEWの代替え
		For i As Integer = 0 To idc.Length - 1
			idc(i) = New iDate()  ' 配列の各要素にインスタンスを作成して代入
		Next

		'日付セット
		idc(0).SetupA(Me, "見積日付", 0)
		idc(1).SetupA(Me, "s納期", 0)
		idc(2).SetupA(Me, "e納期", 0)
		idc(3).SetupA(Me, "OPEN日", 0)
		idc(4).SetupA(Me, "受注日付", 0)

		idc(5).SetupA(Me, "受付日付", 0)
		idc(6).SetupA(Me, "完工日付", 0)

		idc(7).SetupA(Me, "完了日", 0)
		idc(8).SetupA(Me, "請求予定", 0)

		'2014/07/10 ADD
		If COUNTRY_CODE = "CN" Then
			'lblLabels(20).Text = "千元"
			_lblLabels_20.Text = "千元"
			'lblLabels(25).Text = "元"
			_lblLabels_25.Text = "元"

			'UPGRADE_WARNING: オブジェクト tx_出精値引.FormatType の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'[tx_出精値引].FormatType = "#,##0" & KIN_FMT & ";-#,##0" & KIN_FMT & ";"
			[tx_出精値引].FormatType = "#,##0" & KIN_FMT
			[tx_出精値引].FormatTypeNega = "-#,##0" & KIN_FMT
			[tx_出精値引].FormatTypeNull = ""
			[tx_出精値引].FormatTypeZero = ""
			'UPGRADE_WARNING: オブジェクト tx_出精値引.DecimalPlace の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[tx_出精値引].DecimalPlace = KIN_HASU

		End If

		'2003.10.03.DEL--------------
		'    'データセット
		'    If pMituNo = 0 Then         '新規
		'        '空白セット
		'        Call FormInitialize
		'        Call InitialItems
		'        ModeIndicate 0
		'    Else
		'        If Download(pMituNo) = False Then
		'            Call Unload(Me)
		'            Exit Sub
		'        End If
		'    End If
		'2003.10.03.ADD--------------
		'空白セット
		Call FormInitialize()
		'グローバル変数初期化
		Call GlHldBlank()
		'データセット
		If pMituNo = 0 Then '新規
			Call InitialItems()
			ModeIndicate(0)
		Else
			If Download(pMituNo) = False Then
				Call Me.Close()
				Exit Sub
			End If
		End If
		'----------------------------

		'リサイズ用初期値設定（幅・高さ）
		MeWidth = VB6Conv.PixelsToTwipsX(Me.Width)
		MeHeight = VB6Conv.PixelsToTwipsY(Me.Height)

		'製品Noの桁セット-------------------
		'    tx_製品No.MaxLength = SeiIDLength
		'------------------------------------
		Me.Show()

		'2004/11/29 ADD
		If Trim([rf_見積番号].Text) <> vbNullString Then
			'        If Chk締日To見積(Trim$(rf_見積番号)) = False Then    '2015/01/20 DEL
			'            CriticalAlarm "更新済みの為、修正できません。"
			'        Else

			'処理済みのメッセージ
			'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
			If Not IsDbNull(HD_売上日付) Then
				'System.Windows.Forms.Application.DoEvents()
				Me.Hide()
				Inform("売上処理済みのデータです。")
				Me.Show()
				'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
			ElseIf Not IsDbNull(HD_仕入日付) Then
				'System.Windows.Forms.Application.DoEvents()
				Me.Hide()
				Inform("仕入処理済みのデータです。")
				Me.Show()
			End If
			'        End If
		End If

		'原価割れ設定取得
		'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		gGenRituMax = CDec(SpcToNull(GetIni("Limit", "GenRituMax", INIFile), 0)) '2008/01/23 ADD
		If gGenRituMax = 0 Then
			gGenRituMax = 100
		End If
		'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		gGenRituMin = CDec(SpcToNull(GetIni("Limit", "GenRituMin", INIFile), 0)) '2008/01/23 ADD
		If gGenRituMin = 0 Then
			gGenRituMin = 0
		End If

		sb_Msg_Panel2.Text = DateTime.Now.ToString("yyyy/MM/dd")
		sb_Msg_Panel3.Text = DateTime.Now.ToString("HH:mm")
		' タイマーの間隔を設定 (10秒ごとに更新)
		Timer1.Interval = 10000
		Timer1.Start() ' タイマーを開始

		' フォームでキー入力を受け取れるようにする
		Me.KeyPreview = True

		'ロード時にラベル・ボタンを動的に配置する
		AddPicFunction()

		' PicFunction.Controlsのボタン Name と TabIndex を表示（調査）
		'For Each ctrl As Control In PicFunction.Controls
		'	If TypeOf ctrl Is Button AndAlso ctrl.Name.StartsWith("CbFunc") Then
		'		Debug.Print($"Name: {ctrl.Name}, TabIndex: {ctrl.TabIndex}, TabStop: {ctrl.TabStop}")
		'	End If
		'Next

		Loaded = True
	End Sub

	Private Sub AddPicFunction()

		' ラベル・ボタンを配置するPanelを指定
		Dim targetPanel As Panel = PicFunction

		' 生成数を指定
		Dim genLabelCount As Integer = 12
		Dim genLabelWidth As Integer = 65
		Dim genLabelHeight As Integer = 12
		Dim genLabelY As Integer = 1
		Dim genButtonCount As Integer = 12
		Dim genButtonWidth As Integer = 65
		Dim genButtonHeight As Integer = 18
		Dim genButtonY As Integer = 12

		Dim margin As Integer = 7               ' PFキー4つ置きのボタン間の余白

		Dim i As Integer

		For i = 0 To genLabelCount - 1

			' 新しいラベルを作成
			Dim lbl As New Label()

			' ラベルのプロパティを設定
			lbl.Tag = i + 1                     ' IndexをTagプロパティに格納
			lbl.Name = "LbFunc" & i + 1

			' ラベルをPanelに追加
			targetPanel.Controls.Add(lbl)

			lbl.Size = New Size(genLabelWidth, genLabelHeight)
			lbl.Location = New Point((i * genLabelWidth) + 1 + ((i \ 4) * margin), genLabelY) ' ラベルの位置を計算

			lbl.Text = "F" & i + 1
		Next

		For i = 0 To genButtonCount - 1

			' 新しいボタンを作成
			Dim btn As New Button()

			' ボタンのプロパティを設定
			btn.Tag = i + 1                     ' IndexをTagプロパティに格納
			btn.Name = "CbFunc" & i + 1
			btn.Font = New Font("MS Gothic", 7.8, FontStyle.Regular)
			btn.FlatStyle = FlatStyle.Standard

			btn.Size = New Size(genButtonWidth, genButtonHeight)
			btn.Location = New Point((i * genButtonWidth) + 1 + ((i \ 4) * margin), genButtonY) ' ボタンの位置を計算

			'Debug.WriteLine($"Location X:{(i * genButtonWidth) + 1 + ((i \ 4) * margin)}")

			Select Case i
				Case 0
					btn.Text = ""
					btn.TabIndex = i + 91               ' TabIndexをプロパティに格納
				Case 1
					btn.Text = ""
					btn.TabIndex = i + 91               ' TabIndexをプロパティに格納
				Case 2
					btn.Text = ""
					btn.TabIndex = i + 91               ' TabIndexをプロパティに格納
				Case 3
					btn.Text = ""
					btn.TabIndex = i + 91               ' TabIndexをプロパティに格納
				Case 4
					btn.Text = ""
					btn.TabIndex = i + 91               ' TabIndexをプロパティに格納
				Case 5
					btn.Text = ""
					btn.TabIndex = i + 115              ' TabIndexをプロパティに格納
				Case 6
					btn.Text = ""
					btn.TabIndex = i + 113              ' TabIndexをプロパティに格納
				Case 7
					btn.Text = ""
					btn.TabIndex = i + 89               ' TabIndexをプロパティに格納
				Case 8
					btn.Text = "削除"
					btn.TabIndex = i + 89               ' TabIndexをプロパティに格納
				Case 9
					btn.Text = ""
					btn.TabIndex = i + 89               ' TabIndexをプロパティに格納
				Case 10
					btn.Text = "次へ"
					btn.TabIndex = i + 89               ' TabIndexをプロパティに格納
				Case 11
					btn.Text = "戻る"
					btn.TabIndex = i + 89               ' TabIndexをプロパティに格納
				Case Else
					btn.Text = ""
			End Select

			' ボタンにクリックイベントを追加
			AddHandler btn.Click, AddressOf CbFunc_Click

			' ボタンをPanelに追加
			targetPanel.Controls.Add(btn)

			' リストにボタンを追加
			LcbFunc.Add(btn)

		Next

	End Sub

	' 動的に追加したボタンのクリックイベント
	Private Sub CbFunc_Click(sender As Object, e As EventArgs)
		Dim btn As Button = CType(sender, Button)
		Dim Index As Integer = CType(btn.Tag, Integer) - 1
		'Dim Index As Integer = cbFunc.GetIndex(eventSender)
		If LcbFunc(Index).Text = vbNullString Then
			PreviousControl.Focus()
			Exit Sub
		End If
		'ボタンを押されたのが２回目ならば抜ける
		If CLK2F = True Then
			Exit Sub
		End If
		'ボタン２重起動防止フラグのセット
		CLK2F = True
		LcbFunc(Index).TabStop = True
		Select Case Index
			Case 0
			Case 1
			Case 2
			Case 3
			Case 4
			Case 5
			Case 8
				If MsgBoxResult.No = NoYes("削除します。", Me.Text) Then
					PreviousControl.Focus()
				Else
					'2015/01/13 ADD
					If MsgBoxResult.No = NoYes("本当に削除します。", Me.Text) Then
						PreviousControl.Focus()
					Else
						'2015/01/13 ADD
						If MsgBoxResult.No = NoYes("本当に本当に削除します。！！！！", Me.Text) Then
							PreviousControl.Focus()
						Else

							System.Windows.Forms.Application.DoEvents()
							Call Purge()
							pEndMsg = False
							Me.Close()
							CLK2F = False
							Exit Sub
						End If
					End If
				End If
			Case 9
			Case 10
				If Item_Check(LcbFunc(Index).TabIndex) = True Then
					If MsgBoxResult.Yes = YesNo("次へ進みます。", Me.Text) Then
						System.Windows.Forms.Application.DoEvents()
						'cbFunc(Index).Enabled = False

						If Upload() = True Then
							'Call FormInitialize
							[tx_担当者CD].Focus()
							'Set NextStepForm.ResParentForm = Me
							'NextStepForm.MituNo = Trim$(rf_見積番号)
							'NextStepForm.Show
							'員数入力画面呼び出し
							With SnwMT02F00
								.ResParentForm = Me
								'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.MituNo = SpcToNull(Trim(rf_見積番号.Text), 0)
								On Error Resume Next
								.Show()
								If Err.Number <> 0 Then
									'Debug.Print "Err1:" & Err.Number & ":" & Err.Description
									MsgBox("Err1:" & Err.Number & ":" & Err.Description)
								Else
									'Debug.Print "Err2:" & Err.Number & ":" & Err.Description
									System.Windows.Forms.Application.DoEvents() '2010/04/23 ADD
									Me.Hide()
								End If
								Err.Clear()
								On Error GoTo 0
							End With
						End If
					Else
						PreviousControl.Focus()
						'tx_経過備考2.Focus()
					End If
				End If
			Case 11
				Me.Close()
				CLK2F = False
				Exit Sub
		End Select
		LcbFunc(Index).TabStop = False
		'ボタン２重起動防止フラグの初期化
		CLK2F = False
	End Sub

	'UPGRADE_WARNING: イベント SnwMT01F00.Resize は、フォームが初期化されたときに発生します。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"' をクリックしてください。
	Private Sub SnwMT01F00_Resize(sender As Object, e As EventArgs) Handles Me.Resize
		If Me.WindowState <> System.Windows.Forms.FormWindowState.Minimized Then
			'フォーム最小（幅）制御
			If VB6Conv.PixelsToTwipsX(Me.Width) < MeWidth Then
				Me.Width = VB6Conv.TwipsToPixelsX(MeWidth)
			End If
			'フォーム最小（高さ）制御
			If VB6Conv.PixelsToTwipsY(Me.Height) < MeHeight Then
				Me.Height = VB6Conv.TwipsToPixelsY(MeHeight)
			End If
		End If
	End Sub

	Private Sub SnwMT01F00_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
		Dim KeyCode As Integer = e.KeyCode
		Dim Shift As Integer = e.KeyData \ &H10000
		Dim ctl As System.Windows.Forms.Control

		On Error GoTo Form_KeyDown_Err
		Select Case KeyCode
			Case System.Windows.Forms.Keys.Escape
				KeyCode = 0
				Exit Sub
			Case System.Windows.Forms.Keys.F1 To System.Windows.Forms.Keys.F12
				On Error Resume Next
				'ctl = Me.Controls("CbFunc" & (KeyCode - Keys.F1 + 1).ToString())
				'If Err.Number = 0 Then
				'    If ctl IsNot Nothing Then
				'        If ctl.Text <> String.Empty Then
				'            If ctl.Enabled = True Then
				'                ctl.Focus()
				'                If Err.Number = 0 Then
				'                    SendReturnKey()
				'                End If
				'            End If
				'        End If
				'    End If
				'End If
				'UPGRADE_NOTE: オブジェクト ctl をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
				'ctl = Nothing
				Select Case KeyCode
					Case Keys.F1
						LcbFunc_Action(0)
					Case Keys.F2
						LcbFunc_Action(1)
					Case Keys.F3
						LcbFunc_Action(2)
					Case Keys.F4
						LcbFunc_Action(3)
					Case Keys.F5
						LcbFunc_Action(4)
					Case Keys.F6
						LcbFunc_Action(5)
					Case Keys.F7
						LcbFunc_Action(6)
					Case Keys.F8
						LcbFunc_Action(7)
					Case Keys.F9
						LcbFunc_Action(8)
					Case Keys.F10
						LcbFunc_Action(9)
					Case Keys.F11
						LcbFunc_Action(10)
					Case Keys.F12
						LcbFunc_Action(11)
				End Select
				KeyCode = 0
				On Error GoTo 0
				Exit Sub
		End Select

		Exit Sub
Form_KeyDown_Err:
		MsgBox(Err.Number & " " & Err.Description)
	End Sub

	' 指定したインデックスのボタン動作を実行
	Private Sub LcbFunc_Action(index As Integer)
		If index >= 0 AndAlso index < LcbFunc.Count Then
			' ボタンのクリックイベントを呼び出す
			LcbFunc(index).PerformClick()
		End If
	End Sub

	Private Sub SnwMT01F00_KeyPress(sender As Object, e As KeyPressEventArgs) Handles Me.KeyPress
		Dim KeyAscii As Integer = Asc(e.KeyChar)
		'ビープ音消去用
		Select Case KeyAscii
			Case System.Windows.Forms.Keys.Return
				KeyAscii = 0
		End Select
		e.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			e.Handled = True
		End If
	End Sub

	Private Sub SnwMT01F00_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
		Dim Cancel As Boolean = e.Cancel
		Dim UnloadMode As System.Windows.Forms.CloseReason = e.CloseReason
		If Loaded Then
			If pEndMsg Then
				If MsgBoxResult.Yes = NoYes("現在の処理を終了します。") Then
					'UPGRADE_NOTE: オブジェクト idc() をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
					idc(0) = Nothing
					'UPGRADE_NOTE: オブジェクト idc() をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
					idc(1) = Nothing
					'UPGRADE_NOTE: オブジェクト idc() をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
					idc(2) = Nothing
					'UPGRADE_NOTE: オブジェクト idc() をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
					idc(3) = Nothing
					'UPGRADE_NOTE: オブジェクト idc() をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
					idc(4) = Nothing
					'UPGRADE_NOTE: オブジェクト PreviousControl をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
					PreviousControl = Nothing
					'ロック情報を解除する。
					'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					Call UnLockData("見積番号", SpcToNull([rf_見積番号].Text, 0))
					pParentForm.Show()
					'                pParentForm.Download
				Else
					PreviousControl.Focus()
					Cancel = True
				End If
			Else
				'UPGRADE_NOTE: オブジェクト idc() をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
				idc(0) = Nothing
				'UPGRADE_NOTE: オブジェクト idc() をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
				idc(1) = Nothing
				'UPGRADE_NOTE: オブジェクト idc() をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
				idc(2) = Nothing
				'UPGRADE_NOTE: オブジェクト idc() をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
				idc(3) = Nothing
				'UPGRADE_NOTE: オブジェクト idc() をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
				idc(4) = Nothing
				'UPGRADE_NOTE: オブジェクト PreviousControl をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
				PreviousControl = Nothing
				'ロック情報を解除する。
				'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				Call UnLockData("見積番号", SpcToNull([rf_見積番号].Text, 0))
				pParentForm.Show()
				'UPGRADE_ISSUE: Control Download は、汎用名前空間 Form 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
				pParentForm.Download()
				'UPGRADE_ISSUE: Control SetupItems は、汎用名前空間 Form 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
				pParentForm.SetupItems()

			End If
		Else
			pParentForm.Show()
			Exit Sub
		End If
		e.Cancel = Cancel
		'Me.Dispose()
	End Sub

	Private Sub FormInitialize()
		Call SetupBlank()
		Call HldBlank()
		'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
		Call ModeIndicate(System.DBNull.Value)
		[rf_処理区分].BackColor = System.Drawing.ColorTranslator.FromOle(&HE0E0E0)

		EnableHanbai(True)
	End Sub

	Private Sub SetupBlank()
		Try
			ClearControls(Me)
		Catch ex As Exception
		End Try
	End Sub

	Private Sub ClearControls(ByVal parent As Control)
		For Each ctl As Control In parent.Controls
			If TypeOf ctl Is TextBox Then
				DirectCast(ctl, TextBox).Text = ""
			End If
			'UPGRADE_WARNING: TypeOf に新しい動作が指定されています。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"' をクリックしてください。
			If TypeOf ctl Is System.Windows.Forms.Label Then
				If ctl.Name Like "rf_*" Then
					ctl.Text = vbNullString
				End If
			End If
			'UPGRADE_WARNING: TypeOf に新しい動作が指定されています。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"' をクリックしてください。
			If TypeOf ctl Is ExText.ExTextBox Then
				ctl.Text = vbNullString
			End If
			'UPGRADE_WARNING: TypeOf に新しい動作が指定されています。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"' をクリックしてください。
			If TypeOf ctl Is ExNmText.ExNmTextBox Then
				ctl.Text = vbNullString
			End If
			'UPGRADE_WARNING: TypeOf に新しい動作が指定されています。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"' をクリックしてください。
			If TypeOf ctl Is ExDateText.ExDateTextBoxY Then
				ctl.Text = vbNullString
			End If
			'UPGRADE_WARNING: TypeOf に新しい動作が指定されています。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"' をクリックしてください。
			If TypeOf ctl Is ExDateText.ExDateTextBoxM Then
				ctl.Text = vbNullString
			End If
			'UPGRADE_WARNING: TypeOf に新しい動作が指定されています。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"' をクリックしてください。
			If TypeOf ctl Is ExDateText.ExDateTextBoxD Then
				ctl.Text = vbNullString
			End If

			' 子コントロールがある場合、再帰的に処理
			If ctl.HasChildren Then
			    ClearControls(ctl)
			End If
		Next
	End Sub

	Private Sub HldBlank()
		'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト HTANTOCD の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		HTANTOCD = System.DBNull.Value
		'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト HBUSYOCD の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		HBUSYOCD = System.DBNull.Value
		'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト HTOKUCD の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		HTOKUCD = System.DBNull.Value
		'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト HNTOKUCD の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		HNTOKUCD = System.DBNull.Value
		'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト HNONYUCD の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		HNONYUCD = System.DBNull.Value
		'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト H_HANTOKUCD の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		H_HANTOKUCD = System.DBNull.Value
		'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト H_HANNTOKUCD の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		H_HANNTOKUCD = System.DBNull.Value
		'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト H_HANNONYUCD の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		H_HANNONYUCD = System.DBNull.Value

		'2014/09/15 ADD
		'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト H_社内伝票扱い の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		H_社内伝票扱い = System.DBNull.Value

		'2014/10/28 ADD
		'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト H_出精値引 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		H_出精値引 = System.DBNull.Value

		'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト HWELNAICD の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		HWELNAICD = System.DBNull.Value '2015/06/12 ADD
		'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト HWELBKNCD の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		HWELBKNCD = System.DBNull.Value '2015/06/12 ADD

		'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト HBUKKENNO の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		HBUKKENNO = System.DBNull.Value '2015/07/10 ADD

		'    HSUISHINCD = Null   '2019/12/12 ADD

		'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト HKANRYOBI の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		HKANRYOBI = System.DBNull.Value '2020/04/11 ADD

		'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト HSYUKEICD の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		HSYUKEICD = System.DBNull.Value '2021/02/25 ADD

		'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト HKOUJITANTOCD の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		HKOUJITANTOCD = System.DBNull.Value '2022/08/08 ADD

		'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト HBUKKENSYU の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		HBUKKENSYU = System.DBNull.Value '2022/11/22 ADD

		'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト H統合見積NO の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		H統合見積NO = System.DBNull.Value '2024/01/16 ADD

		'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト H業種区分 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		H業種区分 = System.DBNull.Value '2024/02/14 ADD

		HD_見積確定区分 = 0 '2024/06/21 ADD

	End Sub

	Private Sub GlHldBlank()
		'グローバル変数初期化
		'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト HD_見積番号 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		HD_見積番号 = System.DBNull.Value
		HD_担当者CD = 0
		'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト HD_見積日付 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		HD_見積日付 = System.DBNull.Value
		HD_見積件名 = vbNullString
		HD_得意先CD = vbNullString
		HD_得意先名1 = vbNullString
		HD_得意先名2 = vbNullString
		HD_得TEL = vbNullString
		HD_得FAX = vbNullString
		HD_得担当者 = vbNullString
		HD_納得意先CD = vbNullString
		HD_納入先CD = vbNullString
		HD_納入先名1 = vbNullString
		HD_納入先名2 = vbNullString
		HD_納郵便番号 = vbNullString
		HD_納住所1 = vbNullString
		HD_納住所2 = vbNullString
		HD_納TEL = vbNullString
		HD_納FAX = vbNullString
		HD_納担当者 = vbNullString
		'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト HD_納期S の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		HD_納期S = System.DBNull.Value
		'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト HD_納期E の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		HD_納期E = System.DBNull.Value
		HD_備考 = vbNullString
		HD_規模金額 = 0
		'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト HD_OPEN日 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		HD_OPEN日 = System.DBNull.Value
		HD_物件種別 = 0
		HD_現場名 = vbNullString
		HD_支払条件 = 0
		HD_支払条件他 = vbNullString
		HD_納期表示 = 0
		HD_納期表示他 = vbNullString
		HD_見積日出力 = 0
		HD_有効期限 = 0
		HD_受注区分 = 0
		'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト HD_受注日付 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		HD_受注日付 = System.DBNull.Value
		HD_大小口区分 = 0
		HD_出精値引 = 0
		'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト HD_売上日付 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		HD_売上日付 = System.DBNull.Value
		'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト HD_仕入日付 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		HD_仕入日付 = System.DBNull.Value
		HD_合計金額 = 0
		HD_原価合計 = 0
		HD_原価率 = 0
		HD_外税額 = 0
		HD_税集計区分 = 0
		HD_売上端数 = 0
		HD_消費税端数 = 0

		'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト HD_得意先別見積番号 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		HD_得意先別見積番号 = System.DBNull.Value '2005/07/04 ADD

		'    HD_見積区分 = 0                                     '2009/09/26 ADD
		'    HH_見積区分 = 0                                     '2013/04/09 ADD
		HH_受注区分 = 0 '2013/08/08 ADD


		'2013/03/08 ADD↓
		HD_部署CD = 0
		HD_販売先得意先CD = vbNullString
		HD_販売先得意先名1 = vbNullString
		HD_販売先得意先名2 = vbNullString
		HD_販売先納得意先CD = vbNullString
		HD_販売先納入先CD = vbNullString
		HD_販売先納入先名1 = vbNullString
		HD_販売先納入先名2 = vbNullString
		'2013/03/08 ADD↑

		'2013/07/19 ADD↓
		HD_社内伝票扱い = 0
		'2013/07/19 ADD↑

		'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト HD_物件番号 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		HD_物件番号 = System.DBNull.Value '2015/07/10 ADD
		'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト HD_統合見積番号 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		HD_統合見積番号 = System.DBNull.Value '2024/01/16 ADD

		'    HD_営業推進部CD = 0 '2019/12/12 ADD
		HD_工事担当CD = 0 '2022/08/08 ADD
	End Sub

	Private Sub ModeIndicate(ByVal Mode As Object)
		'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
		If IsDBNull(Mode) Then
			[rf_処理区分].Text = vbNullString
			ModeF = 0
			'UPGRADE_WARNING: オブジェクト Mode の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		ElseIf Mode = 0 Then
			[rf_処理区分].Text = "≪登 録≫"
			[rf_処理区分].BackColor = System.Drawing.ColorTranslator.FromOle(&HC0FFFF)
			ModeF = 1
			'UPGRADE_WARNING: オブジェクト Mode の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		ElseIf Mode = 1 Then
			[rf_処理区分].Text = "≪修 正≫"
			[rf_処理区分].BackColor = System.Drawing.ColorTranslator.FromOle(&HC0FFC0)
			ModeF = 2
		Else
			[rf_処理区分].Text = "≪修 正≫"
			[rf_処理区分].BackColor = System.Drawing.ColorTranslator.FromOle(&HC0FFC0)
			ModeF = 2
		End If
	End Sub

	'UPGRADE_NOTE: Name は Name_Renamed にアップグレードされました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"' をクリックしてください。
	Private Sub SetUpFuncs(ByRef Name_Renamed As String)
		If LcbFunc.Count = 0 Then Exit Sub 'Load中のイベント回避
		'ボタン名の変更
		Select Case Name_Renamed
			Case "tx_担当者CD"
				LcbFunc(0).Text = ""
				LcbFunc(1).Text = ""
				LcbFunc(2).Text = ""
				LcbFunc(3).Text = ""
				LcbFunc(4).Text = ""
				LcbFunc(5).Text = ""
				LcbFunc(6).Text = ""
				LcbFunc(7).Text = ""
				If ModeF = 1 Then '追加
					LcbFunc(8).Text = ""
				Else
					LcbFunc(8).Text = "削除"
				End If
				LcbFunc(9).Text = ""
				LcbFunc(10).Text = "次へ"
			Case Else
				LcbFunc(0).Text = ""
				LcbFunc(1).Text = ""
				LcbFunc(2).Text = ""
				LcbFunc(3).Text = ""
				LcbFunc(4).Text = ""
				LcbFunc(5).Text = ""
				LcbFunc(6).Text = ""
				LcbFunc(7).Text = ""
				If ModeF = 1 Then '追加
					LcbFunc(8).Text = ""
				Else
					LcbFunc(8).Text = "削除"
				End If
				LcbFunc(9).Text = ""
				LcbFunc(10).Text = "次へ"
		End Select
	End Sub

	Private Sub CbTabEnd_Enter(sender As Object, e As KeyEventArgs) Handles tx_経過備考2.KeyDown
		' Enterキー、↓キーを検出、Tabキーは取得できない
		If e.KeyCode = Keys.Return Or e.KeyCode = Keys.Down Then
			ReturnF = True
		End If
		If e.KeyCode = Keys.Up Then
			Exit Sub
		End If
		If ReturnF = False Then
			Exit Sub
		End If

		If Item_Check((CbTabEnd.TabIndex)) Then
			LcbFunc(10).Focus()
			LcbFunc(10).PerformClick()
		End If
	End Sub

	'UPGRADE_ISSUE: PictureBox イベント picFunction.GotFocus はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="ABD9AF39-7E24-4AFF-AD8D-3675C1AA3054"' をクリックしてください。
	Private Sub PicFunction_Enter(sender As Object, e As EventArgs) Handles PicFunction.Enter
		PreviousControl.Focus()
	End Sub

	Private Function Item_Check(ByRef ItemNo As Integer) As Boolean
		Dim buf1 As String '名前取得用バッファ
		Dim Chk_ID As String 'チェック用ワーク

		On Error GoTo Item_Check_Err
		Item_Check = False

		'2015/07/10 ADD↓
		'    担当者CDのチェック
		'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト HBUKKENNO の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		'If ItemNo > [tx_物件番号].TabIndex And ([tx_物件番号].Text <> HBUKKENNO.ToString() Or IsDBNull(HBUKKENNO)) Then
		Dim StrHBUKKENNO As String = If(IsDBNull(HBUKKENNO), String.Empty, HBUKKENNO.ToString())
		If (ItemNo > [tx_物件番号].TabIndex) And ([tx_物件番号].Text <> StrHBUKKENNO) Then
			'If ItemNo > [tx_物件番号].TabIndex And ([tx_物件番号].Text <> Convert.ToString(HBUKKENNO) Or IsDBNull(HBUKKENNO)) Then

			If IsCheckText([tx_物件番号]) = True Then
				'--- 入力値をチェック用クラスへ格納
				With cBukken
					.Initialize()
					'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.[物件番号] = NullToZero((tx_物件番号.Text))
					If .GetbyID = False Then
						CriticalAlarm("指定の物件番号は存在しません。")
						'UPGRADE_WARNING: オブジェクト tx_物件番号.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						tx_物件番号.Undo()
						[tx_物件番号].Focus()
						Exit Function

					Else
						[tx_物件番号].Text = CStr(.[物件番号])
						rf_物件略称.Text = .物件略称

						'2022/10/11 ADD↓
						'UPGRADE_WARNING: オブジェクト NullToZero(tx_担当者CD.Text, ) の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						If NullToZero((tx_担当者CD.Text), "") = "" Then
							[tx_担当者CD].Text = CStr(.[担当者CD])
							'--- 入力値をチェック用クラスへ格納
							With cTanto
								.Initialize()
								.[担当者CD] = tx_担当者CD.Text
								If .GetbyID = False Then
									CriticalAlarm("指定の担当者は存在しません。")
									'UPGRADE_WARNING: オブジェクト tx_担当者CD.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
									tx_担当者CD.Undo()
									[tx_担当者CD].Focus()
									[tx_担当者CD].Select()
									'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
									'UPGRADE_WARNING: オブジェクト HTANTOCD の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
									HTANTOCD = System.DBNull.Value
									Exit Function
								Else
									[tx_担当者CD].Text = .[担当者CD]
									rf_担当者名.Text = .担当者名
								End If
							End With
							'--- 入力値をワークへ格納
							'UPGRADE_WARNING: オブジェクト HTANTOCD の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							HTANTOCD = tx_担当者CD.Text
						End If

						'UPGRADE_WARNING: オブジェクト NullToZero(tx_部署CD.Text, ) の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						If NullToZero(([tx_部署CD].Text), "") = "" Then
							[tx_部署CD].Text = CStr(.[部署CD])
							'--- 入力値をチェック用クラスへ格納
							With cBusyo
								.Initialize()
								.[部署CD] = tx_部署CD.Text
								If .GetbyID = False Then
									CriticalAlarm("指定の部署は存在しません。")
									'UPGRADE_WARNING: オブジェクト tx_部署CD.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
									tx_部署CD.Undo()
									[tx_部署CD].Focus()
									[tx_部署CD].Select()
									'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
									'UPGRADE_WARNING: オブジェクト HBUSYOCD の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
									HBUSYOCD = System.DBNull.Value
									Exit Function
								Else
									[tx_部署CD].Text = .[部署CD]
									rf_部署名.Text = .部署名
								End If
							End With
							'--- 入力値をワークへ格納
							'UPGRADE_WARNING: オブジェクト HBUSYOCD の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							HBUSYOCD = tx_部署CD.Text
						End If

						'UPGRADE_WARNING: オブジェクト NullToZero(tx_工事担当CD.Text, ) の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						If NullToZero(([tx_工事担当CD].Text), "") = "" Then
							[tx_工事担当CD].Text = CStr(.[工事担当CD])
							'--- 入力値をチェック用クラスへ格納
							With cKoujiTanto
								.Initialize()
								.[工事担当CD] = tx_工事担当CD.Text
								If .GetbyID = False Then
									CriticalAlarm("指定の工事担当は存在しません。")
									'UPGRADE_WARNING: オブジェクト tx_工事担当CD.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
									tx_工事担当CD.Undo()
									[tx_工事担当CD].Focus()
									[tx_工事担当CD].Select()
									'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
									'UPGRADE_WARNING: オブジェクト HKOUJITANTOCD の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
									HKOUJITANTOCD = System.DBNull.Value
									Exit Function
								Else
									[tx_工事担当CD].Text = .[工事担当CD]
									rf_工事担当名.Text = .工事担当名
								End If
							End With
							'--- 入力値をワークへ格納
							'UPGRADE_WARNING: オブジェクト HKOUJITANTOCD の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							HKOUJITANTOCD = tx_工事担当CD.Text

						End If

						'UPGRADE_WARNING: オブジェクト NullToZero(tx_見積件名.Text, ) の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						If NullToZero(([tx_見積件名].Text), "") = "" Then
							[tx_見積件名].Text = .物件名
						End If


						'UPGRADE_WARNING: オブジェクト NullToZero(idc(0).Text, ) の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						If NullToZero((idc(0).Text), "") = Today.ToString("yyyy/MM/dd") Then
							'初期値ならば変更
							'UPGRADE_WARNING: オブジェクト cBukken.物件登録日付 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							idc(0).Text = .物件登録日付
						End If

						'UPGRADE_WARNING: オブジェクト NullToZero(tx_得意先CD.Text, ) の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						If NullToZero((tx_得意先CD.Text), "") = "" Then
							[tx_得意先CD].Text = .[得意先CD]

							'--- 入力値をチェック用クラスへ格納
							With cTokuisaki
								.Initialize()
								.[得意先CD] = cBukken.得意先CD
								If .GetbyID = False Then
									CriticalAlarm("指定の得意先は存在しません。")
									'UPGRADE_WARNING: オブジェクト tx_得意先CD.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
									tx_得意先CD.Undo()
									[tx_得意先CD].Focus()
									'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
									'UPGRADE_WARNING: オブジェクト HTOKUCD の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
									HTOKUCD = System.DBNull.Value
									Exit Function
								Else
									[rf_売上端数].Text = CStr(.[売上端数])
									rf_消費税端数.Text = CStr(.[消費税端数])
								End If
							End With


							tx_得意先名1.Text = .[得意先名1]
							tx_得意先名2.Text = .[得意先名2]
							tx_得TEL.Text = .[得TEL]
							tx_得FAX.Text = .[得FAX]
							tx_得担当者.Text = .得担当者

							tx_税集計区分.Text = CStr(.[税集計区分])
							rf_税集計区分名.Text = cTokuisaki.Get税集計区分名(CStr(.[税集計区分]))


							'2021/02/25 ADD↓
							tx_集計CD.Text = .[集計CD]
							'--- 入力値をチェック用クラスへ格納
							With cSyuukei
								.Initialize()
								.[得意先CD] = tx_集計CD.Text
								If .GetbyID = False Then
									CriticalAlarm("指定の得意先は存在しません。")
									'UPGRADE_WARNING: オブジェクト tx_集計CD.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
									tx_集計CD.Undo()
									[tx_集計CD].Focus()
									'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
									'UPGRADE_WARNING: オブジェクト HSYUKEICD の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
									HSYUKEICD = System.DBNull.Value
									Exit Function
								End If

								'            [tx_集計CD].Text = .得意先CD
								[rf_集計名].Text = .略称

							End With

							'--- 入力値をワークへ格納
							'UPGRADE_WARNING: オブジェクト HSYUKEICD の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							HSYUKEICD = tx_集計CD.Text
							'2021/02/25 ADD↑

							'--- 入力値をワークへ格納
							'UPGRADE_WARNING: オブジェクト HTOKUCD の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							HTOKUCD = [tx_得意先CD].Text

							'社内伝「9999」ならばロックしない
							'        If [tx_得意先CD].Text = "9999" Then
							If [tx_得意先CD].Text = "9999" Or [tx_得意先CD].Text = "3055" Then '2015/01/30 ADD
								EnableHanbai(True)
							Else
								EnableHanbai(False)
							End If
						End If

						'UPGRADE_WARNING: オブジェクト NullToZero(tx_納得意先CD.Text, ) の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						If NullToZero(([tx_納得意先CD].Text), "") = "" Then
							[tx_納得意先CD].Text = .納得意先CD
							tx_納入先CD.Text = .[納入先CD]

							With cNonyusaki
								.Initialize()
								.[得意先CD] = tx_納得意先CD.Text
								.[納入先CD] = tx_納入先CD.Text
								If .GetbyID = False Then
									CriticalAlarm("指定の納入先は存在しません。")
									'UPGRADE_WARNING: オブジェクト tx_納入先CD.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
									tx_納入先CD.Undo()
									[tx_納入先CD].Focus()
									'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
									'UPGRADE_WARNING: オブジェクト HNONYUCD の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
									HNONYUCD = System.DBNull.Value
									Exit Function
								End If
							End With

							[tx_納入先名1].Text = .[納入先名1]
							tx_納入先名2.Text = .[納入先名2]
							tx_郵便番号.Text = .納郵便番号
							tx_納住所1.Text = .納住所1
							tx_納住所2.Text = .納住所2
							tx_納TEL.Text = .[納TEL]
							tx_納FAX.Text = .[納FAX]
							tx_納担当者.Text = .納担当者

							'--- 入力値をワークへ格納
							'UPGRADE_WARNING: オブジェクト HNTOKUCD の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							HNTOKUCD = tx_納得意先CD.Text
							'UPGRADE_WARNING: オブジェクト HNONYUCD の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							HNONYUCD = [tx_納入先CD].Text

						End If

						If idc(1).Text = "" Then
							'UPGRADE_WARNING: オブジェクト cBukken.予定開始納期 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							idc(1).Text = .予定開始納期
						End If

						If idc(2).Text = "" Then
							'UPGRADE_WARNING: オブジェクト cBukken.予定終了納期 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							idc(2).Text = .予定終了納期
						End If

						If idc(3).Text = "" Then
							'UPGRADE_WARNING: オブジェクト cBukken.予定オープン日 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							idc(3).Text = .予定オープン日
						End If

						'2024/07/23 ADD↓
						If [tx_業種区分].Text = "" Then
							[tx_業種区分].Text = .予定業種区分
						End If
						'2024/07/23 ADD↑

						If tx_物件種別.Text = "0" Then
							[tx_物件種別].Text = CStr(.予定物件種別)
						End If

						If idc(5).Text = "" Then
							'UPGRADE_WARNING: オブジェクト cBukken.予定受付日付 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							idc(5).Text = .予定受付日付
						End If

						If idc(6).Text = "" Then '完工日
							'UPGRADE_WARNING: オブジェクト cBukken.予定完工日付 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							idc(6).Text = .予定完工日付
						End If

						If idc(7).Text = "" Then '完工日
							'UPGRADE_WARNING: オブジェクト cBukken.予定完工日付 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							idc(7).Text = .予定完工日付
						End If

						If idc(8).Text = "" Then
							'UPGRADE_WARNING: オブジェクト cBukken.予定請求予定日付 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							idc(8).Text = .予定請求予定日付
						End If

						SetHanbai()

					End If
				End With
			Else
				rf_物件略称.Text = ""
			End If
			'--- 入力値をワークへ格納
			'UPGRADE_WARNING: オブジェクト HBUKKENNO の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			HBUKKENNO = [tx_物件番号].Text
		End If
		'2015/07/10 ADD↑
		'2024/01/16 ADD↓
		'統合見積番号のチェック
		'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト H統合見積NO の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		'If ItemNo > [tx_統合見積番号].TabIndex And ([tx_統合見積番号].Text <> H統合見積NO Or IsDbNull(H統合見積NO)) Then
		Dim StrH統合見積NO As String = If(IsDBNull(H統合見積NO), String.Empty, H統合見積NO.ToString())
		If (ItemNo > [tx_統合見積番号].TabIndex) And ([tx_統合見積番号].Text <> StrH統合見積NO) Then
			'If ItemNo > [tx_統合見積番号].TabIndex And ([tx_統合見積番号].Text <> Convert.ToString(H統合見積NO) Or IsDbNull(H統合見積NO)) Then

			If IsCheckText([tx_統合見積番号]) = True Then
				'--- 入力値をチェック用クラスへ格納
				With cMitumoriH
					.Initialize()
					'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.見積番号 = NullToZero((tx_統合見積番号.Text))
					If .GetbyID = False Then
						CriticalAlarm("指定の見積番号は存在しません。")
						'UPGRADE_WARNING: オブジェクト tx_統合見積番号.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						tx_統合見積番号.Undo()
						[tx_統合見積番号].Focus()
						Exit Function

					Else
						[tx_統合見積番号].Text = CStr(.見積番号)
						rf_統合見積件名.Text = .[見積件名]
						tx_受注区分.Text = CStr(0) '仮へ変更
					End If
				End With
			Else
				[rf_統合見積件名].Text = ""
				[tx_受注区分].Text = CStr(1) '確定へ変更
			End If
			'--- 入力値をワークへ格納
			'UPGRADE_WARNING: オブジェクト H統合見積NO の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'H統合見積NO = [tx_統合見積番号].Text
			H統合見積NO = CType([tx_統合見積番号].Text, Integer).ToString("#") '2024/07/15 UPD
		End If
		'2024/01/16 ADD↑

		'    担当者CDのチェック
		'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト HTANTOCD の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		'If ItemNo > [tx_担当者CD].TabIndex And ([tx_担当者CD].Text <> HTANTOCD Or IsDBNull(HTANTOCD)) Then
		'Dim StrHTANTOCD As String = If(IsDBNull(HTANTOCD), String.Empty, HTANTOCD.ToString())
		'If ItemNo > [tx_担当者CD].TabIndex And ([tx_担当者CD].Text <> StrHTANTOCD) Then
		'If ItemNo > [tx_担当者CD].TabIndex And ([tx_担当者CD].Text <> Convert.ToString(HTANTOCD) Or IsDBNull(HTANTOCD)) Then
		If ItemNo > [tx_担当者CD].TabIndex Then
			If IsCheckText([tx_担当者CD]) = False Then
				CriticalAlarm("担当者CDが未入力です。")
				'UPGRADE_WARNING: オブジェクト tx_担当者CD.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				[tx_担当者CD].Undo()
				[tx_担当者CD].Focus()
				[tx_担当者CD].Select()
				Exit Function
			End If

			'2022/11/18 ADD↓
			'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
			If Not IsDBNull(HD_売上日付) Then
				Me.Refresh()
				CriticalAlarm("売上処理済みなので変更できません。")
				'UPGRADE_WARNING: オブジェクト tx_担当者CD.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				[tx_担当者CD].Undo()
				[tx_担当者CD].Focus()
				[tx_担当者CD].Select()
				Exit Function
				'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
			ElseIf Not IsDBNull(HD_仕入日付) Then
				Me.Refresh()
				CriticalAlarm("仕入処理済みなので変更できません。")
				'UPGRADE_WARNING: オブジェクト tx_担当者CD.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				[tx_担当者CD].Undo()
				[tx_担当者CD].Focus()
				[tx_担当者CD].Select()
				Exit Function
			End If
			'2022/11/18 ADD↑

			'--- 入力値をチェック用クラスへ格納
			With cTanto
				.Initialize()
				.[担当者CD] = tx_担当者CD.Text
				If .GetbyID = False Then
					CriticalAlarm("指定の担当者は存在しません。")
					'UPGRADE_WARNING: オブジェクト tx_担当者CD.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					[tx_担当者CD].Undo()
					tx_担当者CD.Focus()
					[tx_担当者CD].Select()
					'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
					'UPGRADE_WARNING: オブジェクト HTANTOCD の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					HTANTOCD = System.DBNull.Value
					Exit Function
				Else
					[tx_担当者CD].Text = .[担当者CD]
					rf_担当者名.Text = .担当者名
					tx_部署CD.Text = .部署cls.部署CD
					[rf_部署名].Text = .部署cls.部署名
				End If
			End With
			'--- 入力値をワークへ格納
			'UPGRADE_WARNING: オブジェクト HTANTOCD の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			HTANTOCD = [tx_担当者CD].Text
		End If
		'2022/11/23 ADD↓
		If ItemNo > [tx_担当者CD].TabIndex Then
			With cTanto
				.Initialize()
				.[担当者CD] = tx_担当者CD.Text
				If .GetbyID = False Then
					CriticalAlarm("担当者を入れて下さい。")
					[tx_担当者CD].Focus()
					[tx_担当者CD].Select()
				End If
			End With
		End If
		'2022/11/23 ADD↑

		'    部署CDのチェック
		'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト HBUSYOCD の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		'If ItemNo > tx_部署CD.TabIndex And ([tx_部署CD].Text <> HBUSYOCD Or IsDBNull(HBUSYOCD)) Then
		'Dim StrHBUSYOCD As String = If(IsDBNull(HBUSYOCD), String.Empty, HBUSYOCD.ToString())
		'If ItemNo > tx_部署CD.TabIndex And ([tx_部署CD].Text <> StrHBUSYOCD) Then
		'If ItemNo > tx_部署CD.TabIndex And ([tx_部署CD].Text <> Convert.ToString(HBUSYOCD) Or IsDBNull(HBUSYOCD)) Then
		If ItemNo > tx_部署CD.TabIndex Then
			If IsCheckText([tx_部署CD]) = False Then
				CriticalAlarm("部署CDが未入力です。")
				'UPGRADE_WARNING: オブジェクト tx_部署CD.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				[tx_部署CD].Undo()
				[tx_部署CD].Focus()
				[tx_部署CD].Select()
				Exit Function
			End If

			'--- 入力値をチェック用クラスへ格納
			With cBusyo
				.Initialize()
				.[部署CD] = tx_部署CD.Text
				If .GetbyID = False Then
					CriticalAlarm("指定の部署は存在しません。")
					'UPGRADE_WARNING: オブジェクト tx_部署CD.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					tx_部署CD.Undo()
					[tx_部署CD].Focus()
					[tx_部署CD].Select()
					'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
					'UPGRADE_WARNING: オブジェクト HBUSYOCD の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					HBUSYOCD = System.DBNull.Value
					Exit Function
				Else
					[tx_部署CD].Text = .[部署CD]
					rf_部署名.Text = .部署名
				End If
			End With
			'--- 入力値をワークへ格納
			'UPGRADE_WARNING: オブジェクト HBUSYOCD の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			HBUSYOCD = tx_部署CD.Text
		End If

		'見積日のチェック
		If ItemNo > [tx_見積日付Y].TabIndex Then
			If IsCheckText([tx_見積日付Y]) = False Then
				CriticalAlarm("見積日付が未入力です。")
				'UPGRADE_WARNING: オブジェクト tx_見積日付Y.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				[tx_見積日付Y].Undo()
				[tx_見積日付Y].Focus()
				Exit Function
			End If
		End If
		If ItemNo > [tx_見積日付M].TabIndex Then
			If IsCheckText([tx_見積日付M]) = False Then
				CriticalAlarm("見積日付が未入力です。")
				'UPGRADE_WARNING: オブジェクト tx_見積日付M.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				[tx_見積日付M].Undo()
				[tx_見積日付M].Focus()
				Exit Function
			End If
		End If
		If ItemNo > [tx_見積日付D].TabIndex Then
			If IsCheckText([tx_見積日付D]) = False Then
				CriticalAlarm("見積日付が未入力です。")
				'UPGRADE_WARNING: オブジェクト tx_見積日付D.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				[tx_見積日付D].Undo()
				[tx_見積日付D].Focus()
				Exit Function
			End If
		End If
		If ItemNo > idc(0).TabIndex Then
			If IsDate(idc(0).Text) = False Then
				CriticalAlarm("見積日付が不正です。")
				'UPGRADE_WARNING: オブジェクト idc().ErrorPart.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'NOTE SS idc(0).ErrorPart.Undo()
				idc(0).ErrorPart.Focus()
				Exit Function
			End If
		End If


		'2019/12/12 ADD↓
		'    営業推進部CDのチェック
		'   If ItemNo > [tx_営業推進部CD].TabIndex And ([tx_営業推進部CD] <> HSUISHINCD Or IsNull(HSUISHINCD)) Then
		'       If IsCheckText(tx_営業推進部CD) = False Then
		'           CriticalAlarm "積算担当が未入力です。"
		'           [tx_営業推進部CD].Undo
		'           [tx_営業推進部CD].SetFocus
		'           Exit Function
		'       End If
		'
		'       '--- 入力値をチェック用クラスへ格納
		'       With cSuishin
		'           .Initialize
		'           .営業推進部CD = [tx_営業推進部CD].Text
		'           If .GetbyID = False Then
		'               CriticalAlarm "指定の積算担当は存在しません。"
		'               [tx_営業推進部CD].Undo
		'               [tx_営業推進部CD].SetFocus
		'               HSUISHINCD = Null
		'               Exit Function
		'           Else
		'               [tx_営業推進部CD].Text = .営業推進部CD
		'               [rf_営業推進部名].Caption = .営業推進部名
		'           End If
		'       End With
		'       '--- 入力値をワークへ格納
		'       HSUISHINCD = [tx_営業推進部CD].Text
		'   End If
		'
		'
		'2019/12/12 ADD↑
		'2022/08/08 ADD↓
		'    工事担当CDのチェック
		'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト HKOUJITANTOCD の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		'If ItemNo > [tx_工事担当CD].TabIndex And ([tx_工事担当CD].Text <> HKOUJITANTOCD Or IsDBNull(HKOUJITANTOCD)) Then
		'Dim StrHKOUJITANTOCD As String = If(IsDBNull(HKOUJITANTOCD), String.Empty, HKOUJITANTOCD.ToString())
		'If ItemNo > [tx_工事担当CD].TabIndex And ([tx_工事担当CD].Text <> StrHKOUJITANTOCD) Then
		'If ItemNo > [tx_工事担当CD].TabIndex And ([tx_工事担当CD].Text <> Convert.ToString(HKOUJITANTOCD) Or IsDBNull(HKOUJITANTOCD)) Then
		If ItemNo > [tx_工事担当CD].TabIndex Then
			If IsCheckText([tx_工事担当CD]) = False Then
				CriticalAlarm("工事担当が未入力です。")
				'UPGRADE_WARNING: オブジェクト tx_工事担当CD.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				[tx_工事担当CD].Undo()
				[tx_工事担当CD].Focus()
				[tx_工事担当CD].Select()
				Exit Function
			End If

			'2022/11/18 ADD↓
			'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
			If Not IsDBNull(HD_売上日付) Then
				Me.Refresh()
				CriticalAlarm("売上処理済みなので変更できません。")
				'UPGRADE_WARNING: オブジェクト tx_工事担当CD.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				[tx_工事担当CD].Undo()
				[tx_工事担当CD].Focus()
				[tx_工事担当CD].Select()
				Exit Function
				'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
			ElseIf Not IsDBNull(HD_仕入日付) Then
				Me.Refresh()
				CriticalAlarm("仕入処理済みなので変更できません。")
				'UPGRADE_WARNING: オブジェクト tx_工事担当CD.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				[tx_工事担当CD].Undo()
				[tx_工事担当CD].Focus()
				[tx_工事担当CD].Select()
				Exit Function
			End If
			'2022/11/18 ADD↑

			'--- 入力値をチェック用クラスへ格納
			With cKoujiTanto
				.Initialize()
				.[工事担当CD] = tx_工事担当CD.Text
				If .GetbyID = False Then
					CriticalAlarm("指定の工事担当は存在しません。")
					'UPGRADE_WARNING: オブジェクト tx_工事担当CD.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					tx_工事担当CD.Undo()
					[tx_工事担当CD].Focus()
					[tx_工事担当CD].Select()
					'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
					'UPGRADE_WARNING: オブジェクト HKOUJITANTOCD の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					HKOUJITANTOCD = System.DBNull.Value
					Exit Function
				Else
					[tx_工事担当CD].Text = .[工事担当CD]
					rf_工事担当名.Text = .工事担当名
				End If
			End With
			'--- 入力値をワークへ格納
			'UPGRADE_WARNING: オブジェクト HKOUJITANTOCD の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			HKOUJITANTOCD = tx_工事担当CD.Text
			'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト HBUKKENSYU の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			HBUKKENSYU = System.DBNull.Value
		End If

		'2022/11/23 ADD↓
		If ItemNo > [tx_工事担当CD].TabIndex Then
			With cKoujiTanto
				.Initialize()
				.[工事担当CD] = tx_工事担当CD.Text
				If .GetbyID = False Then
					CriticalAlarm("工事担当を入れて下さい。")
					[tx_工事担当CD].Focus()
					[tx_工事担当CD].Select()
				End If
			End With
		End If
		'2022/11/23 ADD↑

		'2022/08/08 ADD↑


		'キー項目「得意先CD」のチェック
		'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト HTOKUCD の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		'If ItemNo > tx_得意先CD.TabIndex And ([tx_得意先CD].Text <> HTOKUCD Or IsDBNull(HTOKUCD)) Then
		'Dim StrHTOKUCD As String = If(IsDBNull(HTOKUCD), String.Empty, HTOKUCD.ToString())
		'If ItemNo > tx_得意先CD.TabIndex And ([tx_得意先CD].Text <> StrHTOKUCD) Then
		If ItemNo > tx_得意先CD.TabIndex And ([tx_得意先CD].Text <> Convert.ToString(HTOKUCD) Or IsDBNull(HTOKUCD)) Then
			If IsCheckText([tx_得意先CD]) = False Then
				CriticalAlarm("得意先CDが未入力です。")
				'UPGRADE_WARNING: オブジェクト tx_得意先CD.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				[tx_得意先CD].Undo()
				[tx_得意先CD].Focus()
				Exit Function
			End If

			'2014/09/15 ADD↓
			'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
			If Not IsDBNull(HD_売上日付) Then
				Me.Refresh()
				CriticalAlarm("売上処理済みなので変更できません。")
				'UPGRADE_WARNING: オブジェクト tx_得意先CD.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				[tx_得意先CD].Undo()
				[tx_得意先CD].Focus()
				Exit Function
				'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
			ElseIf Not IsDBNull(HD_仕入日付) Then
				Me.Refresh()
				CriticalAlarm("仕入処理済みなので変更できません。")
				'UPGRADE_WARNING: オブジェクト tx_得意先CD.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				[tx_得意先CD].Undo()
				[tx_得意先CD].Focus()
				Exit Function
			End If
			'2014/09/15 ADD↑

			'--- 入力値をチェック用ワークへ格納
			If ISInt(([tx_得意先CD].Text)) Then
				'UPGRADE_WARNING: TextBox プロパティ tx_得意先CD.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
				Chk_ID = CType([tx_得意先CD].Text, Integer).ToString(New String("0"c, [tx_得意先CD].MaxLength))
			Else
				Chk_ID = [tx_得意先CD].Text
			End If

			'2016/12/21 ADD↓
			If Chk_ID = "9999" Then
				CriticalAlarm("指定したコードは入力できません。")
				'UPGRADE_WARNING: オブジェクト tx_得意先CD.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				[tx_得意先CD].Undo()
				[tx_得意先CD].Focus()
				Exit Function
			End If
			'2016/12/21 ADD↑

			'--- 入力値をチェック用クラスへ格納
			With cTokuisaki
				.Initialize()
				.[得意先CD] = Chk_ID
				If .GetbyID = False Then
					CriticalAlarm("指定の得意先は存在しません。")
					'UPGRADE_WARNING: オブジェクト tx_得意先CD.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					tx_得意先CD.Undo()
					[tx_得意先CD].Focus()
					'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
					'UPGRADE_WARNING: オブジェクト HTOKUCD の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					HTOKUCD = System.DBNull.Value
					Exit Function
				Else
					[tx_得意先CD].Text = .[得意先CD]
					tx_得意先名1.Text = .[得意先名1]
					'                [tx_得意先名1].EmptyUndoBuffer
					tx_得意先名2.Text = .[得意先名2]
					tx_得TEL.Text = .電話番号
					tx_得FAX.Text = .FAX番号
					tx_得担当者.Text = .得意先担当者名

					'2016/04/07 ADD↓
					'                [rf_税集計区分].Caption = .税集計区分
					tx_税集計区分.Text = CStr(.[税集計区分])
					rf_税集計区分名.Text = cTokuisaki.Get税集計区分名(CStr(.[税集計区分]))
					'2016/04/07 ADD↑
					rf_売上端数.Text = CStr(.[売上端数])
					rf_消費税端数.Text = CStr(.[消費税端数])


					'2021/02/25 ADD↓
					tx_集計CD.Text = .[集計CD]
					'--- 入力値をチェック用クラスへ格納
					With cSyuukei
						.Initialize()
						.[得意先CD] = tx_集計CD.Text
						If .GetbyID = False Then
							CriticalAlarm("指定の得意先は存在しません。")
							'UPGRADE_WARNING: オブジェクト tx_集計CD.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							tx_集計CD.Undo()
							[tx_集計CD].Focus()
							'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
							'UPGRADE_WARNING: オブジェクト HSYUKEICD の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							HSYUKEICD = System.DBNull.Value
							Exit Function
						End If

						'            [tx_集計CD].Text = .得意先CD
						[rf_集計名].Text = .略称

					End With

					'--- 入力値をワークへ格納
					'UPGRADE_WARNING: オブジェクト HSYUKEICD の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					HSYUKEICD = tx_集計CD.Text
					'2021/02/25 ADD↑

				End If
			End With
			'--- 入力値をワークへ格納
			'UPGRADE_WARNING: オブジェクト HTOKUCD の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			HTOKUCD = [tx_得意先CD].Text


			'社内伝「9999」ならばロックしない
			'        If [tx_得意先CD].Text = "9999" Then
			If [tx_得意先CD].Text = "9999" Or [tx_得意先CD].Text = "3055" Then '2015/01/30 ADD
				EnableHanbai(True)
			Else
				EnableHanbai(False)
				SetHanbai()
			End If

		End If

		'得意先名1のチェック
		If ItemNo > [tx_得意先名1].TabIndex Then
			If IsCheckText([tx_得意先名1]) = False Then
				CriticalAlarm("得意先名1が未入力です。")
				'UPGRADE_WARNING: オブジェクト tx_得意先名1.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				[tx_得意先名1].Undo()
				[tx_得意先名1].Focus()
				Exit Function
			End If
		End If

		'2016/04/07 ADD↓
		'税集計区分のチェック
		If ItemNo > [tx_税集計区分].TabIndex Then
			'税集計区分の表示
			[rf_税集計区分名].Text = cTokuisaki.Get税集計区分名(([tx_税集計区分].Text))
		End If
		'2016/04/07 ADD↑

		'2021/02/25ADD↓
		'キー項目「集計CD」のチェック
		'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト HSYUKEICD の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		'If ItemNo > [tx_集計CD].TabIndex And ([tx_集計CD].Text <> HSYUKEICD Or IsDBNull(HSYUKEICD)) Then
		'Dim StrHSYUKEICD As String = If(IsDBNull(HSYUKEICD), String.Empty, HSYUKEICD.ToString())
		'If ItemNo > [tx_集計CD].TabIndex And ([tx_集計CD].Text <> StrHSYUKEICD) Then
		If ItemNo > [tx_集計CD].TabIndex And ([tx_集計CD].Text <> Convert.ToString(HSYUKEICD) Or IsDBNull(HSYUKEICD)) Then
			If IsCheckText([tx_集計CD]) = False Then
				CriticalAlarm("集計CDが未入力です。")
				'UPGRADE_WARNING: オブジェクト tx_集計CD.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				[tx_集計CD].Undo()
				[tx_集計CD].Focus()
				Exit Function
			End If
			'--- 入力値をチェック用ワークへ格納
			If ISInt(([tx_集計CD].Text)) Then
				'UPGRADE_WARNING: TextBox プロパティ tx_集計CD.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
				Chk_ID = CType([tx_集計CD].Text, Integer).ToString(New String("0"c, [tx_集計CD].MaxLength))
			Else
				Chk_ID = [tx_集計CD].Text
			End If

			'--- 入力値をチェック用クラスへ格納
			With cSyuukei
				.Initialize()
				.[得意先CD] = Chk_ID
				If .GetbyID = False Then
					CriticalAlarm("指定の得意先は存在しません。")
					'UPGRADE_WARNING: オブジェクト tx_集計CD.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					tx_集計CD.Undo()
					[tx_集計CD].Focus()
					'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
					'UPGRADE_WARNING: オブジェクト HSYUKEICD の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					HSYUKEICD = System.DBNull.Value
					Exit Function
				End If

				'            [tx_集計CD].Text = .得意先CD
				[rf_集計名].Text = .略称

			End With

			'--- 入力値をワークへ格納
			'UPGRADE_WARNING: オブジェクト HSYUKEICD の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			HSYUKEICD = tx_集計CD.Text
		End If
		'2021/02/25ADD↑



		'キー項目「納得意先CD」のチェック
		'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト HNTOKUCD の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		'If ItemNo > [tx_納得意先CD].TabIndex And ([tx_納得意先CD].Text <> HNTOKUCD Or IsDBNull(HNTOKUCD)) Then
		'Dim StrHNTOKUCD As String = If(IsDBNull(HNTOKUCD), String.Empty, HNTOKUCD.ToString())
		'If ItemNo > [tx_納得意先CD].TabIndex And ([tx_納得意先CD].Text <> StrHNTOKUCD) Then
		If ItemNo > [tx_納得意先CD].TabIndex And ([tx_納得意先CD].Text <> Convert.ToString(HNTOKUCD) Or IsDBNull(HNTOKUCD)) Then
			If IsCheckText([tx_納得意先CD]) = False Then
				CriticalAlarm("得意先CDが未入力です。")
				'UPGRADE_WARNING: オブジェクト tx_納得意先CD.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				[tx_納得意先CD].Undo()
				[tx_納得意先CD].Focus()
				Exit Function
			End If
			'2014/04/25 ADD↓
			If [tx_納得意先CD].Text = "9999" Then
				CriticalAlarm("指定したコードは入力できません。")
				'UPGRADE_WARNING: オブジェクト tx_納得意先CD.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				[tx_納得意先CD].Undo()
				[tx_納得意先CD].Focus()
				Exit Function
			End If
			'2014/04/25 ADD↑
			'--- 入力値をチェック用ワークへ格納
			If ISInt(([tx_納得意先CD].Text)) Then
				'UPGRADE_WARNING: TextBox プロパティ tx_納得意先CD.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
				Chk_ID = CType([tx_納得意先CD].Text, Integer).ToString(New String("0"c, [tx_納得意先CD].MaxLength))
			Else
				Chk_ID = [tx_納得意先CD].Text
			End If

			'--- 入力値をチェック用クラスへ格納
			With cTokuisaki
				.Initialize()
				.[得意先CD] = Chk_ID
				If .GetbyID = False Then
					CriticalAlarm("指定の得意先は存在しません。")
					'UPGRADE_WARNING: オブジェクト tx_納得意先CD.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					tx_納得意先CD.Undo()
					[tx_納得意先CD].Focus()
					'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
					'UPGRADE_WARNING: オブジェクト HNTOKUCD の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					HNTOKUCD = System.DBNull.Value
					Exit Function
				Else
					[tx_納得意先CD].Text = .[得意先CD]
				End If
			End With
			'--- 入力値をワークへ格納
			'UPGRADE_WARNING: オブジェクト HNTOKUCD の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			HNTOKUCD = tx_納得意先CD.Text
			'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト HNONYUCD の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			HNONYUCD = System.DBNull.Value
		End If

		'キー項目「納入先CD」のチェック
		'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト HNONYUCD の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		'If ItemNo > [tx_納入先CD].TabIndex And ([tx_納入先CD].Text <> HNONYUCD Or IsDBNull(HNONYUCD)) Then
		'Dim StrHNONYUCD As String = If(IsDBNull(HNONYUCD), String.Empty, HNONYUCD.ToString())
		'If ItemNo > [tx_納入先CD].TabIndex And ([tx_納入先CD].Text <> StrHNONYUCD) Then
		If ItemNo > [tx_納入先CD].TabIndex And ([tx_納入先CD].Text <> Convert.ToString(HNONYUCD) Or IsDBNull(HNONYUCD)) Then
			If IsCheckText([tx_納入先CD]) = False Then
				CriticalAlarm("納入先CDが未入力です。")
				'UPGRADE_WARNING: オブジェクト tx_納入先CD.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				[tx_納入先CD].Undo()
				[tx_納入先CD].Focus()
				Exit Function
			End If
			'--- 入力値をチェック用ワークへ格納
			If ISInt(([tx_納入先CD].Text)) Then
				'UPGRADE_WARNING: TextBox プロパティ tx_納入先CD.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
				Chk_ID = CType([tx_納入先CD].Text, Integer).ToString(New String("0"c, [tx_納入先CD].MaxLength))
			Else
				Chk_ID = [tx_納入先CD].Text
			End If

			'--- 入力値をチェック用クラスへ格納
			With cNonyusaki
				.Initialize()
				.[得意先CD] = tx_納得意先CD.Text
				.[納入先CD] = Chk_ID
				If .GetbyID = False Then
					CriticalAlarm("指定の納入先は存在しません。")
					'UPGRADE_WARNING: オブジェクト tx_納入先CD.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					tx_納入先CD.Undo()
					[tx_納入先CD].Focus()
					'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
					'UPGRADE_WARNING: オブジェクト HNONYUCD の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					HNONYUCD = System.DBNull.Value
					Exit Function
				Else
					[tx_納入先CD].Text = .[納入先CD]

					tx_納入先名1.Text = .[納入先名1]
					tx_納入先名2.Text = .[納入先名2]
					tx_郵便番号.Text = .[郵便番号]
					tx_納住所1.Text = .[住所1]
					tx_納住所2.Text = .[住所2]
					tx_納TEL.Text = .電話番号
					tx_納FAX.Text = .FAX番号
					tx_納担当者.Text = .納入先担当者名

				End If
			End With
			'--- 入力値をワークへ格納
			'UPGRADE_WARNING: オブジェクト HNONYUCD の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			HNONYUCD = tx_納入先CD.Text

			'社内伝「9999」ならばロックしない
			If [tx_得意先CD].Text = "9999" Or [tx_得意先CD].Text = "3055" Then '2015/01/30 ADD
			Else
				SetHanbai()
			End If
		End If

		'納入先名1のチェック
		If ItemNo > [tx_納入先名1].TabIndex Then
			If IsCheckText([tx_納入先名1]) = False Then
				CriticalAlarm("納入先名1が未入力です。")
				'UPGRADE_WARNING: オブジェクト tx_納入先名1.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				[tx_納入先名1].Undo()
				[tx_納入先名1].Focus()
				Exit Function
			End If
		End If

		'2014/09/15 ADD↓
		'キー項目「社内伝票扱い」のチェック
		'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト H_社内伝票扱い の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		'If ItemNo > [ck_社内伝票扱い].TabIndex And ([ck_社内伝票扱い].CheckState <> H_社内伝票扱い Or IsDBNull(H_社内伝票扱い)) Then
		Dim StrH_社内伝票扱い As String = If(IsDBNull(H_社内伝票扱い), String.Empty, H_社内伝票扱い.ToString())
		'NOTE SS IsDBNull判定のエラー回避
		If ItemNo > [ck_社内伝票扱い].TabIndex And
		   ([ck_社内伝票扱い].CheckState = CheckState.Unchecked AndAlso StrH_社内伝票扱い <> "Unchecked") Or
		   ([ck_社内伝票扱い].CheckState = CheckState.Checked AndAlso StrH_社内伝票扱い <> "Checked") Then
			'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
			If Not IsDBNull(HD_売上日付) Then
				Me.Refresh()
				CriticalAlarm("売上処理済みなので変更できません。")
				'UPGRADE_WARNING: オブジェクト H_社内伝票扱い の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				[ck_社内伝票扱い].CheckState = H_社内伝票扱い
				[ck_社内伝票扱い].Focus()
				Exit Function
				'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
			ElseIf Not IsDBNull(HD_仕入日付) Then
				Me.Refresh()
				CriticalAlarm("仕入処理済みなので変更できません。")
				'UPGRADE_WARNING: オブジェクト H_社内伝票扱い の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				[ck_社内伝票扱い].CheckState = H_社内伝票扱い
				[ck_社内伝票扱い].Focus()
				Exit Function
			End If

			'--- 入力値をワークへ格納
			'UPGRADE_WARNING: オブジェクト H_社内伝票扱い の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			H_社内伝票扱い = [ck_社内伝票扱い].CheckState

		End If
		'2014/09/15 ADD↑

		'2013/04/17 ADD↓
		'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト H_HANTOKUCD の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		'If ItemNo > [tx_販売先得意先CD].TabIndex And ([tx_販売先得意先CD].Text <> H_HANTOKUCD Or IsDBNull(H_HANTOKUCD)) Then
		'Dim StrH_HANTOKUCD As String = If(IsDBNull(H_HANTOKUCD), String.Empty, H_HANTOKUCD.ToString())
		'If ItemNo > [tx_販売先得意先CD].TabIndex And ([tx_販売先得意先CD].Text <> StrH_HANTOKUCD) Then
		If ItemNo > [tx_販売先得意先CD].TabIndex And ([tx_販売先得意先CD].Text <> Convert.ToString(H_HANTOKUCD) Or IsDBNull(H_HANTOKUCD)) Then
			If IsCheckText([tx_販売先得意先CD]) = False Then
				CriticalAlarm("販売先得意先CDが未入力です。")
				'UPGRADE_WARNING: オブジェクト tx_販売先得意先CD.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				[tx_販売先得意先CD].Undo()
				[tx_販売先得意先CD].Select()
				Exit Function
			End If
		End If
		'2013/04/17 ADD↑

		'2013/03/07 ADD↓
		'If [tx_得意先CD].Text = "9999" Then
		If [tx_得意先CD].Text = "9999" Or [tx_得意先CD].Text = "3055" Then '2015/01/30 ADD
			'キー項目「販売先得意先CD」のチェック
			'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト H_HANTOKUCD の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'If ItemNo > [tx_販売先得意先CD].TabIndex And ([tx_販売先得意先CD].Text <> H_HANTOKUCD Or IsDbNull(H_HANTOKUCD)) Then
			'If ItemNo > [tx_販売先得意先CD].TabIndex And ([tx_販売先得意先CD].Text <> StrH_HANTOKUCD) Then
			If ItemNo > [tx_販売先得意先CD].TabIndex And ([tx_販売先得意先CD].Text <> Convert.ToString(H_HANTOKUCD) Or IsDBNull(H_HANTOKUCD)) Then
				If IsCheckText([tx_販売先得意先CD]) = False Then
					CriticalAlarm("得意先CDが未入力です。")
					'UPGRADE_WARNING: オブジェクト tx_販売先得意先CD.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					[tx_販売先得意先CD].Undo()
					[tx_販売先得意先CD].Focus()
					Exit Function
				End If
				If [tx_販売先得意先CD].Text = "9999" Or [tx_販売先得意先CD].Text = "3055" Then
					CriticalAlarm("指定したコードは入力できません。")
					'UPGRADE_WARNING: オブジェクト tx_販売先得意先CD.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					[tx_販売先得意先CD].Undo()
					[tx_販売先得意先CD].Focus()
					Exit Function
				End If

				'--- 入力値をチェック用ワークへ格納
				If ISInt(([tx_販売先得意先CD].Text)) Then
					'UPGRADE_WARNING: TextBox プロパティ tx_販売先得意先CD.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
					Chk_ID = CType([tx_販売先得意先CD].Text, Integer).ToString(New String("0"c, [tx_販売先得意先CD].MaxLength))
				Else
					Chk_ID = [tx_販売先得意先CD].Text
				End If

				'--- 入力値をチェック用クラスへ格納
				With cTokuisaki
					.Initialize()
					.[得意先CD] = Chk_ID
					If .GetbyID = False Then
						CriticalAlarm("指定の得意先は存在しません。")
						'UPGRADE_WARNING: オブジェクト tx_販売先得意先CD.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						tx_販売先得意先CD.Undo()
						[tx_販売先得意先CD].Focus()
						'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
						'UPGRADE_WARNING: オブジェクト H_HANTOKUCD の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						H_HANTOKUCD = System.DBNull.Value
						Exit Function
					Else
						[tx_販売先得意先CD].Text = .[得意先CD]
						rf_販売先得意先名1.Text = .[得意先名1]
						'                [tx_得意先名1].EmptyUndoBuffer
						rf_販売先得意先名2.Text = .[得意先名2]
					End If
				End With
				'--- 入力値をワークへ格納
				'UPGRADE_WARNING: オブジェクト H_HANTOKUCD の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				H_HANTOKUCD = tx_販売先得意先CD.Text
			End If

			'キー項目「納得意先CD」のチェック
			'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト H_HANNTOKUCD の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'If ItemNo > [tx_販売先納得意先CD].TabIndex And ([tx_販売先納得意先CD].Text <> H_HANNTOKUCD Or IsDbNull(H_HANNTOKUCD)) Then
			'Dim StrH_HANNTOKUCD As String = If(IsDBNull(H_HANNTOKUCD), String.Empty, H_HANNTOKUCD.ToString())
			'If ItemNo > [tx_販売先納得意先CD].TabIndex And ([tx_販売先納得意先CD].Text <> StrH_HANNTOKUCD) Then
			If ItemNo > [tx_販売先納得意先CD].TabIndex And ([tx_販売先納得意先CD].Text <> Convert.ToString(H_HANNTOKUCD) Or IsDBNull(H_HANNTOKUCD)) Then
				If IsCheckText([tx_販売先納得意先CD]) = False Then
					CriticalAlarm("得意先CDが未入力です。")
					'UPGRADE_WARNING: オブジェクト tx_販売先納得意先CD.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					[tx_販売先納得意先CD].Undo()
					[tx_販売先納得意先CD].Focus()
					Exit Function
				End If
				If [tx_販売先納得意先CD].Text = "9999" Or [tx_販売先納得意先CD].Text = "3055" Then
					CriticalAlarm("指定したコードは入力できません。")
					'UPGRADE_WARNING: オブジェクト tx_販売先納得意先CD.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					[tx_販売先納得意先CD].Undo()
					[tx_販売先納得意先CD].Focus()
					Exit Function
				End If
				'--- 入力値をチェック用ワークへ格納
				If ISInt(([tx_販売先納得意先CD].Text)) Then
					'UPGRADE_WARNING: TextBox プロパティ tx_販売先納得意先CD.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
					Chk_ID = CType([tx_販売先納得意先CD].Text, Integer).ToString(New String("0"c, [tx_販売先納得意先CD].MaxLength))
				Else
					Chk_ID = [tx_販売先納得意先CD].Text
				End If

				'--- 入力値をチェック用クラスへ格納
				With cTokuisaki
					.Initialize()
					.[得意先CD] = Chk_ID
					If .GetbyID = False Then
						CriticalAlarm("指定の得意先は存在しません。")
						'UPGRADE_WARNING: オブジェクト tx_販売先納得意先CD.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						tx_販売先納得意先CD.Undo()
						[tx_販売先納得意先CD].Focus()
						'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
						'UPGRADE_WARNING: オブジェクト H_HANNTOKUCD の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						H_HANNTOKUCD = System.DBNull.Value
						Exit Function
					Else
						[tx_販売先納得意先CD].Text = .[得意先CD]
					End If
				End With
				'--- 入力値をワークへ格納
				'UPGRADE_WARNING: オブジェクト H_HANNTOKUCD の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				H_HANNTOKUCD = tx_販売先納得意先CD.Text
				'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト H_HANNONYUCD の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				H_HANNONYUCD = System.DBNull.Value
			End If

			'キー項目「納入先CD」のチェック
			'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト H_HANNONYUCD の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'If ItemNo > [tx_販売先納入先CD].TabIndex And ([tx_販売先納入先CD].Text <> H_HANNONYUCD Or IsDbNull(H_HANNONYUCD)) Then
			'Dim StrH_HANNONYUCD As String = If(IsDBNull(H_HANNONYUCD), String.Empty, H_HANNONYUCD.ToString())
			'If ItemNo > [tx_販売先納入先CD].TabIndex And ([tx_販売先納入先CD].Text <> StrH_HANNONYUCD) Then
			If ItemNo > [tx_販売先納入先CD].TabIndex And ([tx_販売先納入先CD].Text <> Convert.ToString(H_HANNONYUCD) Or IsDBNull(H_HANNONYUCD)) Then
				If IsCheckText([tx_販売先納入先CD]) = False Then
					CriticalAlarm("納入先CDが未入力です。")
					'UPGRADE_WARNING: オブジェクト tx_販売先納入先CD.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					[tx_販売先納入先CD].Undo()
					[tx_販売先納入先CD].Focus()
					Exit Function
				End If
				'--- 入力値をチェック用ワークへ格納
				If ISInt(([tx_販売先納入先CD].Text)) Then
					'UPGRADE_WARNING: TextBox プロパティ tx_販売先納入先CD.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
					Chk_ID = CType([tx_販売先納入先CD].Text, Integer).ToString(New String("0"c, [tx_販売先納入先CD].MaxLength))
				Else
					Chk_ID = [tx_販売先納入先CD].Text
				End If

				'--- 入力値をチェック用クラスへ格納
				With cNonyusaki
					.Initialize()
					.[得意先CD] = tx_販売先納得意先CD.Text
					.[納入先CD] = Chk_ID
					If .GetbyID = False Then
						CriticalAlarm("指定の納入先は存在しません。")
						'UPGRADE_WARNING: オブジェクト tx_販売先納入先CD.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						tx_販売先納入先CD.Undo()
						[tx_販売先納入先CD].Focus()
						'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
						'UPGRADE_WARNING: オブジェクト H_HANNONYUCD の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						H_HANNONYUCD = System.DBNull.Value
						Exit Function
					Else
						[tx_販売先納入先CD].Text = .[納入先CD]

						rf_販売先納入先名1.Text = .[納入先名1]
						rf_販売先納入先名2.Text = .[納入先名2]

					End If
				End With
				'--- 入力値をワークへ格納
				'UPGRADE_WARNING: オブジェクト H_HANNONYUCD の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				H_HANNONYUCD = tx_販売先納入先CD.Text

			End If
		End If
		'2013/03/07 ADD↑

		'2009/08/13 UPD
		'開始納期のチェック
		'    If [tx_見積区分].Text = 1 Then  '2009/11/26 ADD '仮の時はチェックしない
		If ItemNo > [tx_s納期Y].TabIndex Then
			If IsCheckText([tx_s納期Y]) = False Then
				CriticalAlarm("開始納期が未入力です。")
				'UPGRADE_WARNING: オブジェクト tx_s納期Y.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				[tx_s納期Y].Undo()
				[tx_s納期Y].Focus()
				Exit Function
			End If
		End If
		If ItemNo > [tx_s納期M].TabIndex Then
			'If IsCheckText([tx_s納期M]) = False Then
			'	CriticalAlarm("開始納期が未入力です。")
			'	'UPGRADE_WARNING: オブジェクト tx_s納期M.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'	[tx_s納期M].Undo()
			'	[tx_s納期M].Focus()
			'	Exit Function
			'End If
		End If
		If ItemNo > [tx_s納期D].TabIndex Then
			If IsCheckText([tx_s納期D]) = False Then
				CriticalAlarm("開始納期が未入力です。")
				'UPGRADE_WARNING: オブジェクト tx_s納期D.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				[tx_s納期D].Undo()
				[tx_s納期D].Focus()
				Exit Function
			End If
		End If
		If ItemNo > idc(1).TabIndex Then
			If IsDate(idc(1).Text) = False Then
				CriticalAlarm("開始納期が不正です。")
				'UPGRADE_WARNING: オブジェクト idc().ErrorPart.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'NOTE SS idc(1).ErrorPart.Undo()
				idc(1).ErrorPart.Focus()
				'[tx_s納期Y].Focus()
				Exit Function
			End If
		End If
		'   End If
		'    '終了納期のチェック
		'    If ItemNo > [tx_e納期Y].TabIndex Then
		'        If IsCheckText([tx_e納期Y]) = False Then
		'            CriticalAlarm "終了納期が未入力です。"
		'            [tx_e納期Y].Undo
		'            [tx_e納期Y].SetFocus
		'            Exit Function
		'        End If
		'    End If
		'    If ItemNo > [tx_e納期M].TabIndex Then
		'        If IsCheckText([tx_e納期M]) = False Then
		'            CriticalAlarm "終了納期が未入力です。"
		'            [tx_e納期M].Undo
		'            [tx_e納期M].SetFocus
		'            Exit Function
		'        End If
		'    End If
		'    If ItemNo > [tx_e納期D].TabIndex Then
		'        If IsCheckText([tx_e納期D]) = False Then
		'            CriticalAlarm "終了納期が未入力です。"
		'            [tx_e納期D].Undo
		'            [tx_e納期D].SetFocus
		'            Exit Function
		'        End If
		'    End If
		' If ItemNo > idc(2).TabIndex Then
		'     If IsDate(idc(2).Text) = False Then
		'         CriticalAlarm "終了納期が不正です。"
		'         idc(2).ErrorPart.Undo
		'         idc(2).ErrorPart.SetFocus
		'         Exit Function
		'     End If
		' End If
		'
		'    'オープン日のチェック
		'    If ItemNo > [tx_OPEN日Y].TabIndex Then
		'        If IsCheckText([tx_OPEN日Y]) = False Then
		'            CriticalAlarm "オープン日が未入力です。"
		'            [tx_OPEN日Y].Undo
		'            [tx_OPEN日Y].SetFocus
		'            Exit Function
		'        End If
		'    End If
		'    If ItemNo > [tx_OPEN日M].TabIndex Then
		'        If IsCheckText([tx_OPEN日M]) = False Then
		'            CriticalAlarm "オープン日が未入力です。"
		'            [tx_OPEN日M].Undo
		'            [tx_OPEN日M].SetFocus
		'            Exit Function
		'        End If
		'    End If
		'    If ItemNo > [tx_OPEN日D].TabIndex Then
		'        If IsCheckText([tx_OPEN日D]) = False Then
		'            CriticalAlarm "オープン日が未入力です。"
		'            [tx_OPEN日D].Undo
		'            [tx_OPEN日D].SetFocus
		'            Exit Function
		'        End If
		'    End If
		' If ItemNo > idc(3).TabIndex Then
		'     If IsDate(idc(3).Text) = False Then
		'         CriticalAlarm "オープン日が不正です。"
		'         idc(3).ErrorPart.Undo
		'         idc(3).ErrorPart.SetFocus
		'         Exit Function
		'     End If
		' End If

		'2024/02/14 ADD↓
		'業種区分のチェック
		'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト H業種区分 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		'If ItemNo > [tx_業種区分].TabIndex And ([tx_業種区分].Text <> H業種区分 Or IsDBNull(H業種区分)) Then
		'Dim StrH業種区分 As String = If(IsDBNull(H業種区分), String.Empty, H業種区分.ToString())
		'If ItemNo > [tx_業種区分].TabIndex And ([tx_業種区分].Text <> StrH業種区分) Then
		'If ItemNo > [tx_業種区分].TabIndex And ([tx_業種区分].Text <> Convert.ToString(H業種区分) Or IsDBNull(H業種区分)) Then
		If ItemNo > [tx_業種区分].TabIndex Then
			'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
			If Not IsDBNull(HD_売上日付) Then
				'            Me.Refresh
				CriticalAlarm("売上処理済みなので変更できません。")
				'UPGRADE_WARNING: オブジェクト tx_業種区分.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				[tx_業種区分].Undo()
				[tx_業種区分].Focus()
				[tx_業種区分].Select()
				Exit Function
				'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
			ElseIf Not IsDBNull(HD_仕入日付) Then
				'            Me.Refresh
				CriticalAlarm("仕入処理済みなので変更できません。")
				'UPGRADE_WARNING: オブジェクト tx_業種区分.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				[tx_業種区分].Undo()
				[tx_業種区分].Focus()
				[tx_業種区分].Select()
				Exit Function
			End If

			SpcToNull([tx_業種区分].Text, 0)

			'UPGRADE_WARNING: オブジェクト H業種区分 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			H業種区分 = [tx_業種区分].Text
		End If
		'2024/02/14 ADD↑

		'2022/10/26 ADD↓
		'2022/11/22 ADD↓
		'物件種別での工事担当者チェック
		'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト HBUKKENSYU の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		'If ItemNo > [tx_物件種別].TabIndex And ([tx_物件種別].Text <> HBUKKENSYU Or IsDBNull(HBUKKENSYU)) Then
		'Dim StrHBUKKENSYU As String = If(IsDBNull(HBUKKENSYU), String.Empty, HBUKKENSYU.ToString())
		'If ItemNo > [tx_物件種別].TabIndex And ([tx_物件種別].Text <> StrHBUKKENSYU) Then
		'If ItemNo > [tx_物件種別].TabIndex And ([tx_物件種別].Text <> Convert.ToString(HBUKKENSYU) Or IsDBNull(HBUKKENSYU)) Then
		If ItemNo > [tx_物件種別].TabIndex Then
			Select Case [tx_物件種別].Text
				'            Case 5, 6 '2024/01/31 DEL
				Case CStr(6) '2024/01/31 ADD
					If Not ([tx_工事担当CD].Text = SUPPORT_CD) Then 'サポート担当(98)
						CriticalAlarm("指定の工事担当CD(998)を入力して下さい。")
						'UPGRADE_WARNING: オブジェクト tx_物件種別.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						[tx_物件種別].Undo()
						[tx_物件種別].Focus()
						[tx_物件種別].Select()
						Exit Function
					End If
				Case CStr(2)
					If Not ([tx_工事担当CD].Text = CALLC_CD) Then 'コールセンター担当(99)
						CriticalAlarm("指定の工事担当CD(999)を入力して下さい。")
						'UPGRADE_WARNING: オブジェクト tx_物件種別.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						[tx_物件種別].Undo()
						[tx_物件種別].Focus()
						[tx_物件種別].Select()
						Exit Function
					End If
				Case Else
					If ([tx_工事担当CD].Text = SUPPORT_CD) Or ([tx_工事担当CD].Text = CALLC_CD) Then
						'                If ([tx_工事担当CD].Text = CALLC_CD) Then '2024/01/31 ADD '2024/01/31 DEL
						CriticalAlarm("指定の工事担当CDを入力して下さい。")
						'UPGRADE_WARNING: オブジェクト tx_物件種別.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						[tx_物件種別].Undo()
						[tx_物件種別].Focus()
						[tx_物件種別].Select()
						Exit Function
					End If
			End Select
			'--- 入力値をワークへ格納
			'UPGRADE_WARNING: オブジェクト HBUKKENSYU の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			HBUKKENSYU = [tx_物件種別].Text
		End If
		'2022/11/22 ADD↑
		'2022/10/26 ADD↑

		'ウエルシア物件区分のチェック
		'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト HWELBKNCD の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		'If ItemNo > [tx_ウエルシア物件区分].TabIndex And ([tx_ウエルシア物件区分].Text <> HWELBKNCD Or IsDBNull(HWELBKNCD)) Then
		'Dim StrHWELBKNCD As String = If(IsDBNull(HWELBKNCD), String.Empty, HWELBKNCD.ToString())
		'If ItemNo > [tx_ウエルシア物件区分].TabIndex And ([tx_ウエルシア物件区分].Text <> StrHWELBKNCD) Then
		If ItemNo > [tx_ウエルシア物件区分].TabIndex And ([tx_ウエルシア物件区分].Text <> Convert.ToString(HWELBKNCD) Or IsDBNull(HWELBKNCD)) Then
			'        If IsCheckText(tx_ウエルシア物件区分) = False Then
			'            CriticalAlarm "ウエルシア物件区分が未入力です。"
			'            [tx_ウエルシア物件区分].Undo
			'            [tx_ウエルシア物件区分].SetFocus
			'            Exit Function
			'        End If

			'--- 入力値をチェック用クラスへ格納
			If IsCheckText([tx_ウエルシア物件区分]) = True Then
				With cWelBukkenKubun
					.Initialize()
					.[ウエルシア物件区分CD] = tx_ウエルシア物件区分.Text
					If .GetbyID = False Then
						CriticalAlarm("指定の物件区分は存在しません。")
						'UPGRADE_WARNING: オブジェクト tx_ウエルシア物件区分.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						tx_ウエルシア物件区分.Undo()
						[tx_ウエルシア物件区分].Focus()
						'                    HWELBKNCD = Null
						Exit Function
					Else
						[tx_ウエルシア物件区分].Text = .ウエルシア物件区分CD
						[rf_ウエルシア物件区分名].Text = .ウエルシア物件区分名
					End If
				End With
			Else
				rf_ウエルシア物件区分名.Text = ""
			End If
			'--- 入力値をワークへ格納
			'UPGRADE_WARNING: オブジェクト HWELBKNCD の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			HWELBKNCD = [tx_ウエルシア物件区分].Text
		End If
		'2015/06/12 ADD↑

		'2015/06/12 ADD↓
		'ウエルシア物件内容CDのチェック
		'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト HWELNAICD の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		'If ItemNo > [tx_ウエルシア物件内容CD].TabIndex And ([tx_ウエルシア物件内容CD].Text <> HWELNAICD Or IsDBNull(HWELNAICD)) Then
		'Dim StrHWELNAICD As String = If(IsDBNull(HWELNAICD), String.Empty, HWELNAICD.ToString())
		'If ItemNo > [tx_ウエルシア物件内容CD].TabIndex And ([tx_ウエルシア物件内容CD].Text <> StrHWELNAICD) Then
		If ItemNo > [tx_ウエルシア物件内容CD].TabIndex And ([tx_ウエルシア物件内容CD].Text <> Convert.ToString(HWELNAICD) Or IsDBNull(HWELNAICD)) Then
			'        If IsCheckText(tx_ウエルシア物件内容CD) = False Then
			'            CriticalAlarm "ウエルシア物件内容CDが未入力です。"
			'            [tx_ウエルシア物件内容CD].Undo
			'            [tx_ウエルシア物件内容CD].SetFocus
			'            Exit Function
			'        End If

			'--- 入力値をチェック用クラスへ格納
			If IsCheckText([tx_ウエルシア物件内容CD]) = True Then
				With cWelBukkenNaiyo
					.Initialize()
					.[ウエルシア物件内容CD] = tx_ウエルシア物件内容CD.Text
					If .GetbyID = False Then
						CriticalAlarm("指定の物件内容は存在しません。")
						'UPGRADE_WARNING: オブジェクト tx_ウエルシア物件内容CD.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						tx_ウエルシア物件内容CD.Undo()
						[tx_ウエルシア物件内容CD].Focus()
						'                HBUSYOCD = Null
						Exit Function
					Else
						[tx_ウエルシア物件内容CD].Text = .[ウエルシア物件内容CD]
						tx_ウエルシア物件内容名.Text = .[ウエルシア物件内容名]
					End If
				End With
			End If
			'--- 入力値をワークへ格納
			'UPGRADE_WARNING: オブジェクト HWELNAICD の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			HWELNAICD = tx_ウエルシア物件内容CD.Text
		End If


		'    '2013/04/09 ADD↓
		'    If ItemNo > [tx_見積区分].TabIndex Then
		'        If HH_見積区分 = 1 Then '本見積
		'            If [tx_見積区分].Text = 0 Then '仮見積に変わった
		'                '処理済みのメッセージ
		'                If Not IsNull(HD_売上日付) Then
		'                    Me.Refresh
		'                    CriticalAlarm "売上処理済みなので、仮見積に変更できません。"
		'                    [tx_見積区分].Undo
		'                    [tx_見積区分].SetFocus
		'                    Exit Function
		'                ElseIf Not IsNull(HD_仕入日付) Then
		'                    Me.Refresh
		'                    CriticalAlarm "仕入処理済みなので、仮見積に変更できません。"
		'                    [tx_見積区分].Undo
		'                    [tx_見積区分].SetFocus
		'                    Exit Function
		'                End If
		'            End If
		'        End If
		'    End If
		'    '2013/04/09 ADD↑


		If ItemNo > [tx_受注区分].TabIndex Then

			If HH_受注区分 = 1 Then '受注確定
				If CDbl([tx_受注区分].Text) = 0 Then '仮受注に変わった
					'処理済みのメッセージ
					'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
					If Not IsDBNull(HD_売上日付) Then
						Me.Refresh()
						CriticalAlarm("売上処理済みなので、仮受注に変更できません。")
						'UPGRADE_WARNING: オブジェクト tx_受注区分.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						[tx_受注区分].Undo()
						[tx_受注区分].Focus()
						Exit Function
						'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
					ElseIf Not IsDBNull(HD_仕入日付) Then
						Me.Refresh()
						CriticalAlarm("仕入処理済みなので、仮受注に変更できません。")
						'UPGRADE_WARNING: オブジェクト tx_受注区分.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						[tx_受注区分].Undo()
						[tx_受注区分].Focus()
						Exit Function
					End If
				End If
			End If


			If CDbl([tx_受注区分].Text) = 1 Then '確定
				'見積区分のチェック
				'            '2009/09/24 ADD
				'            If [tx_見積区分].Text = 0 Then
				'                CriticalAlarm "見積区分が仮見積の場合、受注区分を確定にはできません。"
				'                [tx_受注区分].Undo
				'                [tx_受注区分].SetFocus
				'                Exit Function
				'            End If

				'受注日のチェック
				If ItemNo > [tx_受注日付Y].TabIndex Then
					If IsCheckText([tx_受注日付Y]) = False Then
						CriticalAlarm("受注日が未入力です。")
						'UPGRADE_WARNING: オブジェクト tx_受注日付Y.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						[tx_受注日付Y].Undo()
						[tx_受注日付Y].Focus()
						Exit Function
					End If
				End If
				If ItemNo > [tx_受注日付M].TabIndex Then
					If IsCheckText([tx_受注日付M]) = False Then
						CriticalAlarm("受注日が未入力です。")
						'UPGRADE_WARNING: オブジェクト tx_受注日付M.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						[tx_受注日付M].Undo()
						[tx_受注日付M].Focus()
						Exit Function
					End If
				End If
				If ItemNo > [tx_受注日付D].TabIndex Then
					If IsCheckText([tx_受注日付D]) = False Then
						CriticalAlarm("受注日が未入力です。")
						'UPGRADE_WARNING: オブジェクト tx_受注日付D.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						[tx_受注日付D].Undo()
						[tx_受注日付D].Focus()
						Exit Function
					End If
				End If
				If ItemNo > idc(4).TabIndex Then
					If IsDate(idc(4).Text) = False Then
						CriticalAlarm("受注日が不正です。")
						'UPGRADE_WARNING: オブジェクト idc().ErrorPart.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						'NOTE SS idc(4).ErrorPart.Undo()
						idc(4).ErrorPart.Focus()
						'[tx_受注日付Y].Focus()
						Exit Function
					End If
				End If

			End If
		End If

		'2014/10/28 ADD↓
		'キー項目「社内伝票扱い」のチェック
		'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト H_出精値引 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		'If ItemNo > [tx_出精値引].TabIndex And ([tx_出精値引].Text <> H_出精値引 Or IsDBNull(H_出精値引)) Then
		'Dim StrH_出精値引 As String = If(IsDBNull(H_出精値引), String.Empty, H_出精値引.ToString())
		'If ItemNo > [tx_出精値引].TabIndex And ([tx_出精値引].Text <> StrH_出精値引) Then
		If ItemNo > [tx_出精値引].TabIndex And ([tx_出精値引].Text <> Convert.ToString(H_出精値引) Or IsDBNull(H_出精値引)) Then
			If Get売上_出精値引() = True Then
				Me.Refresh()
				CriticalAlarm("売上処理済みなので変更できません。")
				'UPGRADE_WARNING: オブジェクト H_出精値引 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				[tx_出精値引].Text = H_出精値引
				[tx_出精値引].Focus()
				Exit Function
			End If

			'--- 入力値をワークへ格納
			'UPGRADE_WARNING: オブジェクト H_出精値引 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			H_出精値引 = [tx_出精値引].Text

		End If
		'2014/10/28 ADD↑

		'完了日付
		If ItemNo > idc(7).TabIndex Then
			If idc(7).IsAllNull = False Then
				If IsDate(idc(7).Text) = False Then
					CriticalAlarm("完了日付が不正です。")
					'UPGRADE_WARNING: オブジェクト idc().ErrorPart.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					'NOTE SS idc(7).ErrorPart.Undo()
					idc(7).ErrorPart.Focus()
					'[tx_完了日Y].Focus()
					Exit Function
				End If

				'入力者取得
				'UPGRADE_WARNING: オブジェクト idc(7).Text の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト NullToZero(HKANRYOBI, ) の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				If NullToZero(HKANRYOBI, "") <> idc(7).Text Then
					[tx_完了者名].Text = GetFullName()
					HD_入力USERID = GetUName()
				End If
			Else
				[tx_完了者名].Text = ""
			End If

			'UPGRADE_WARNING: オブジェクト idc().Text の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト HKANRYOBI の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			HKANRYOBI = idc(7).Text
		End If

		'請求予定日
		If ItemNo > idc(8).TabIndex Then
			If idc(8).IsAllNull = False Then
				If IsDate(idc(8).Text) = False Then
					CriticalAlarm("請求予定が不正です。")
					'UPGRADE_WARNING: オブジェクト idc().ErrorPart.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					'NOTE SS idc(8).ErrorPart.Undo()
					idc(8).ErrorPart.Focus()
					'[tx_請求予定Y].Focus()
					Exit Function
				End If
			End If
		End If

		Item_Check = True

		Exit Function
Item_Check_Err:
		MsgBox(Err.Number & " " & Err.Description)
	End Function

	Private Sub EnableHanbai(ByRef FLG As Boolean)
		[tx_販売先得意先CD].Enabled = FLG
		[tx_販売先納得意先CD].Enabled = FLG
		[tx_販売先納入先CD].Enabled = FLG
	End Sub

	Private Sub SetHanbai()
		[tx_販売先得意先CD].Text = [tx_得意先CD].Text
		[rf_販売先得意先名1].Text = [tx_得意先名1].Text
		[rf_販売先得意先名2].Text = [tx_得意先名2].Text

		[tx_販売先納得意先CD].Text = [tx_納得意先CD].Text
		[tx_販売先納入先CD].Text = [tx_納入先CD].Text
		[rf_販売先納入先名1].Text = [tx_納入先名1].Text
		[rf_販売先納入先名2].Text = [tx_納入先名2].Text
	End Sub

	Private Function Get売上_出精値引() As Boolean
		Dim rs As ADODB.Recordset
		Dim sql As String

		On Error GoTo Get売上_出精値引_Err

		'マウスポインターを砂時計にする
		HourGlass(True)

		'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
		If IsDBNull([rf_見積番号].Text) Or Trim([rf_見積番号].Text) = vbNullString Then
			HourGlass(False)
			Exit Function
		End If


		sql = "SELECT 見積番号 FROM TD売上V2 AS UH"
		sql = sql & " INNER JOIN TD売上明細V2 AS UM"
		sql = sql & " ON UH.売上番号 = UM.売上番号"
		sql = sql & " WHERE 見積番号 = " & [rf_見積番号].Text
		sql = sql & " AND UM.特別区分 = 'ZA'"

		rs = OpenRs(sql, Cn, ADODB.CursorTypeEnum.adOpenForwardOnly, ADODB.LockTypeEnum.adLockReadOnly)

		With rs
			If .EOF Then
				Get売上_出精値引 = False
			Else
				Get売上_出精値引 = True
			End If
		End With
		Call ReleaseRs(rs)

		HourGlass(False)
		Exit Function
Get売上_出精値引_Err:
		MsgBox(Err.Number & " " & Err.Description)
		HourGlass(False)
	End Function

	'Private Function Get担当者DB(ID As String, ByRef IDName As String) As Boolean
	'Dim rs As ADODB.Recordset, sql As String
	'
	'    On Error GoTo Get担当者DB_Err
	'
	'    'マウスポインターを砂時計にする
	'    HourGlass True
	'
	'    If IsNull(ID) Or Trim(ID) = vbNullString Then
	'        IDName1 = vbNullString
	'        IDName = "未設定"
	'        HourGlass False
	'        Exit Function
	'    End If
	'
	'    sql = "SELECT 担当者名 FROM TM担当者 " _
	'        & "WHERE 担当者CD = '" & SQLString(Trim$(ID)) & "'"
	'    Set rs = OpenRs(sql, Cn, adOpenForwardOnly, adLockReadOnly)
	'
	'    With rs
	'        If .EOF Then
	'            IDName = "未登録"
	'            Get担当者DB = False
	'        Else
	'            IDName = NullToZero(![担当者名], vbNullString)
	'            Get担当者DB = True
	'        End If
	'    End With
	'    Call ReleaseRs(rs)
	'
	'    HourGlass False
	'    Exit Function
	'Get担当者DB_Err:
	'    MsgBox Err.Number & " " & Err.Description
	'    HourGlass False
	'End Function

	'Private Function Get得意先DB(ID As String, Optional Existchk As Boolean = False) As Boolean
	'    Dim rs As ADODB.Recordset, sql As String
	'    Dim buf As String
	'
	'    On Error GoTo Get得意先DB_Err
	'
	'    'マウスポインターを砂時計にする
	'    HourGlass True
	'
	'    If IsNull(ID) Or Trim(ID) = vbNullString Then
	'        IDName = "未設定"
	'        Exit Function
	'    End If
	'
	'    sql = "SELECT 得意先名1,得意先名2,郵便番号,電話番号,FAX番号,得意先担当者名," _
	'        & "税集計区分,売上端数,消費税端数" _
	'        & " FROM TM得意先" _
	'        & " WHERE 得意先CD = '" & SQLString(Trim$(ID)) & "'"
	'
	'        Set rs = OpenRs(sql, Cn, adOpenForwardOnly, adLockReadOnly)
	'
	'    With rs
	'        If .EOF Then
	'            IDName = "未登録"
	'            Get得意先DB = False
	'        Else
	'            If Existchk = False Then
	'                [tx_得意先名1].Text = NullToZero(![得意先名1], vbNullString)
	'                [tx_得意先名1].EmptyUndoBuffer
	'                [tx_得意先名2] = NullToZero(![得意先名2], vbNullString)
	'                [tx_得TEL].Text = NullToZero(![電話番号], vbNullString)
	'                [tx_得FAX].Text = NullToZero(![FAX番号], vbNullString)
	'                [tx_得担当者].Text = NullToZero(![得意先担当者名], vbNullString)
	'
	'                [rf_税集計区分].Caption = ![税集計区分]
	'                [rf_売上端数].Caption = ![売上端数]
	'                [rf_消費税端数].Caption = ![消費税端数]
	'            End If
	'            Get得意先DB = True
	'        End If
	'    End With
	'    ReleaseRs rs
	'
	'    HourGlass False
	'    Exit Function
	'Get得意先DB_Err:
	'    MsgBox Err.Number & " " & Err.Description
	'    HourGlass False
	'End Function

	'Private Function Get納入先DB(TOKUID As String, NOUID As String) As Boolean
	'    Dim rs As ADODB.Recordset, sql As String
	'    Dim buf As String
	'
	'    On Error GoTo Get納入先DB_Err
	'
	'    'マウスポインターを砂時計にする
	'    HourGlass True
	'
	'    If IsNull(NOUID) Or Trim(NOUID) = vbNullString Then
	'        IDName = "未設定"
	'        Exit Function
	'    End If
	'
	'    sql = "SELECT 納入先名1,納入先名2,郵便番号,住所1,住所2,電話番号,FAX番号,納入先担当者名" _
	'        & " FROM TM納入先" _
	'        & " WHERE 得意先CD = '" & SQLString(Trim$(TOKUID)) & "'" _
	'        & " AND 納入先CD = '" & SQLString(Trim$(NOUID)) & "'"
	'
	'        Set rs = OpenRs(sql, Cn, adOpenForwardOnly, adLockReadOnly)
	'
	'    With rs
	'        If .EOF Then
	'            IDName = "未登録"
	'            Get納入先DB = False
	'        Else
	'            [tx_納入先名1].Text = NullToZero(![納入先名1], vbNullString)
	'            [tx_納入先名1].EmptyUndoBuffer
	'            [tx_納入先名2] = NullToZero(![納入先名2], vbNullString)
	'            [tx_郵便番号].Text = NullToZero(![郵便番号], vbNullString)
	'            [tx_納住所1].Text = NullToZero(![住所1], vbNullString)
	'            [tx_納住所2].Text = NullToZero(![住所2], vbNullString)
	'            [tx_納TEL].Text = NullToZero(![電話番号], vbNullString)
	'            [tx_納FAX].Text = NullToZero(![FAX番号], vbNullString)
	'            [tx_納担当者].Text = NullToZero(![納入先担当者名], vbNullString)
	'            Get納入先DB = True
	'        End If
	'    End With
	'    ReleaseRs rs
	'
	'    HourGlass False
	'    Exit Function
	'Get納入先DB_Err:
	'    MsgBox Err.Number & " " & Err.Description
	'    HourGlass False
	'End Function

	Private Function Download(ByRef No As Integer) As Boolean
		Dim rs As ADODB.Recordset
		Dim sql As String
		'Dim X As Integer
		'Dim buf As String
		'Dim KANNOOF As Integer

		On Error GoTo Download_Err

		'出荷データ使用チェック
		If LockData("見積番号", No) = False Then
			'        rf_見積番号.Caption = vbNullString
			Exit Function
		End If

		'マウスポインターを砂時計にする
		HourGlass(True)

		'    sql = "SELECT * FROM TD見積" _
		'        & " WHERE 見積番号 = " & No

		sql = "SELECT MH.*,"
		sql = sql & "MHK.見積確定区分,MHK.発注書発行日付,MHK.完了日付,MHK.完了者名,MHK.入力USERID,MHK.請求予定日,MHK.経過備考1,MHK.経過備考2 "
		sql = sql & " FROM TD見積 AS MH"
		sql = sql & " LEFT JOIN TD見積_経過 AS MHK"
		sql = sql & " ON MH.見積番号 = MHK.見積番号"
		sql = sql & " WHERE MH.見積番号 = " & No

		rs = OpenRs(sql, Cn, ADODB.CursorTypeEnum.adOpenKeyset, ADODB.LockTypeEnum.adLockReadOnly)
		With rs
			If .EOF Then
				ReleaseRs(rs)
				GoTo Download_correct
			Else
				rf_見積番号.Text = CStr(No)
				[tx_担当者CD].Text = .Fields("担当者CD").Value
				'            Call Get担当者DB([tx_担当者CD], buf)
				'            rf_担当者名.Caption = buf
				With cTanto
					.Initialize()
					.[担当者CD] = tx_担当者CD.Text
					.GetbyID()
					rf_担当者名.Text = .担当者名
				End With

				idc(0).Text = .Fields("見積日付").Value
				tx_見積件名.Text = .Fields("見積件名").Value
				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				[tx_得意先CD].Text = NullToZero(.Fields("得意先CD").Value, "")
				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				tx_得意先名1.Text = NullToZero(.Fields("得意先名1").Value, "")
				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				tx_得意先名2.Text = NullToZero(.Fields("得意先名2").Value, "")
				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				tx_得TEL.Text = NullToZero(.Fields("得TEL").Value, "")
				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				tx_得FAX.Text = NullToZero(.Fields("得FAX").Value, "")
				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				tx_得担当者.Text = NullToZero(.Fields("得意先担当者").Value, "")
				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				tx_納得意先CD.Text = NullToZero(.Fields("納入得意先CD").Value, "")
				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				tx_納入先CD.Text = NullToZero(.Fields("納入先CD").Value, "")
				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				tx_納入先名1.Text = NullToZero(.Fields("納入先名1").Value, "")
				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				tx_納入先名2.Text = NullToZero(.Fields("納入先名2").Value, "")
				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				tx_郵便番号.Text = NullToZero(.Fields("郵便番号").Value, "")
				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				tx_納住所1.Text = NullToZero(.Fields("住所1").Value, "")
				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				tx_納住所2.Text = NullToZero(.Fields("住所2").Value, "")
				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				tx_納TEL.Text = NullToZero(.Fields("納TEL").Value, "")
				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				tx_納FAX.Text = NullToZero(.Fields("納FAX").Value, "")
				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				tx_納担当者.Text = NullToZero(.Fields("納入先担当者").Value, "")
				idc(1).Text = .Fields("納期S").Value
				idc(2).Text = .Fields("納期E").Value
				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				tx_備考.Text = NullToZero(.Fields("備考").Value, "")
				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'tx_物件金額.Text = NullToZero(.Fields("物件規模金額").Value, "")
				tx_物件金額.Text = CType(NullToZero(.Fields("物件規模金額").Value, ""), Integer).ToString("#,##0")
				idc(3).Text = .Fields("オープン日").Value
				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				tx_物件種別.Text = NullToZero(.Fields("物件種別").Value, "")
				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				tx_伝票種類.Text = NullToZero(.Fields("伝票種類").Value, "") '2015/02/04 ADD
				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				tx_SM内容区分.Text = NullToZero(.Fields("SM内容区分").Value, "") '2015/11/19 ADD

				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				tx_現場名.Text = NullToZero(.Fields("現場名").Value, "")
				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				tx_支払条件.Text = NullToZero(.Fields("支払条件").Value, "")
				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				tx_支払条件他.Text = NullToZero(.Fields("支払条件その他").Value, "")
				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				tx_納期表示.Text = NullToZero(.Fields("納期表示").Value, "")
				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				tx_納期表示他.Text = NullToZero(.Fields("納期その他").Value, "")
				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				tx_出力日.Text = NullToZero(.Fields("見積日出力").Value, "")
				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				tx_有効期限.Text = NullToZero(.Fields("有効期限").Value, "")
				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				tx_受注区分.Text = NullToZero(.Fields("受注区分").Value, "")
				idc(4).Text = .Fields("受注日付").Value
				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				tx_大小口区分.Text = NullToZero(.Fields("大小口区分").Value, "")
				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				tx_出精値引.Text = NullToZero(.Fields("出精値引").Value, "")

				'2016/04/07 ADD↓
				'            rf_税集計区分.Caption = ![税集計区分]
				tx_税集計区分.Text = .Fields("税集計区分").Value
				[rf_税集計区分名].Text = cTokuisaki.Get税集計区分名((.Fields("税集計区分").Value).ToString)
				'2016/04/07 ADD↑
				[rf_売上端数].Text = .Fields("売上端数").Value
				[rf_消費税端数].Text = .Fields("消費税端数").Value

				rf_合計金額.Text = .Fields("合計金額").Value
				rf_原価合計.Text = .Fields("原価合計").Value
				rf_原価率.Text = .Fields("原価率").Value
				rf_外税額.Text = .Fields("外税額").Value

				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				rf_得意先別見積番号.Text = NullToZero(.Fields("得意先別見積番号").Value, "") '2005/07/04 ADD

				'UPGRADE_WARNING: オブジェクト HTANTOCD の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				HTANTOCD = tx_担当者CD.Text
				'UPGRADE_WARNING: オブジェクト HTOKUCD の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				HTOKUCD = [tx_得意先CD].Text
				'            HNTOKUCD = tx_納得意先CD.Text'2014/04/25 DEL
				'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト HNTOKUCD の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				HNTOKUCD = System.DBNull.Value '2014/04/25 ADD
				'UPGRADE_WARNING: オブジェクト HNTOKUCD の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				HNTOKUCD = [tx_納得意先CD].Text '2015/02/09 ADD復活!
				'UPGRADE_WARNING: オブジェクト HNONYUCD の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				HNONYUCD = [tx_納入先CD].Text

				'UPGRADE_WARNING: オブジェクト HD_仕入日付 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				HD_仕入日付 = .Fields("仕入日付").Value
				'UPGRADE_WARNING: オブジェクト HD_売上日付 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				HD_売上日付 = .Fields("売上日付").Value

				'            tx_見積区分.Text = NullToZero(![見積区分], "")      '2009/09/26 ADD
				'            HH_見積区分 = [tx_見積区分].Text                    '2013/04/09 ADD
				HH_受注区分 = CShort([tx_受注区分].Text) '2013/08/08 ADD

				'2013/03/08 ADD↓
				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				[tx_部署CD].Text = NullToZero(.Fields("部署CD").Value)
				With cBusyo
					.Initialize()
					.[部署CD] = tx_部署CD.Text
					.GetbyID()
					rf_部署名.Text = .部署名
				End With

				'            '2019/12/12 ADD↓
				'            [tx_営業推進部CD].Text = NullToZero(![営業推進部CD], "")
				'            With cSuishin
				'                .Initialize
				'                .営業推進部CD = [tx_営業推進部CD].Text
				'                .GetbyID
				'                [rf_営業推進部名].Caption = .営業推進部名
				'            End With
				'2022/08/08 ADD↓
				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				tx_工事担当CD.Text = NullToZero(.Fields("工事担当CD").Value, "")
				With cKoujiTanto
					.Initialize()
					.[工事担当CD] = tx_工事担当CD.Text
					.GetbyID()
					rf_工事担当名.Text = .工事担当名
				End With
				'2022/08/08 ADD↑

				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				tx_販売先得意先CD.Text = NullToZero(.Fields("販売先得意先CD").Value, "")
				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				rf_販売先得意先名1.Text = NullToZero(.Fields("販売先得意先名1").Value, "")
				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				rf_販売先得意先名2.Text = NullToZero(.Fields("販売先得意先名2").Value, "")
				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				tx_販売先納得意先CD.Text = NullToZero(.Fields("販売先納得意先CD").Value, "")
				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				tx_販売先納入先CD.Text = NullToZero(.Fields("販売先納入先CD").Value, "")
				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				rf_販売先納入先名1.Text = NullToZero(.Fields("販売先納入先名1").Value, "")
				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				rf_販売先納入先名2.Text = NullToZero(.Fields("販売先納入先名2").Value, "")

				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				HD_部署CD = NullToZero(.Fields("部署CD").Value)
				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				HD_販売先得意先CD = NullToZero(.Fields("販売先得意先CD").Value, "")
				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				HD_販売先得意先名1 = NullToZero(.Fields("販売先得意先名1").Value, "")
				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				HD_販売先得意先名2 = NullToZero(.Fields("販売先得意先名2").Value, "")
				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				HD_販売先納得意先CD = NullToZero(.Fields("販売先納得意先CD").Value, "")
				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				HD_販売先納入先CD = NullToZero(.Fields("販売先納入先CD").Value, "")
				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				HD_販売先納入先名1 = NullToZero(.Fields("販売先納入先名1").Value, "")
				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				HD_販売先納入先名2 = NullToZero(.Fields("販売先納入先名2").Value, "")

				'UPGRADE_WARNING: オブジェクト HBUSYOCD の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				HBUSYOCD = If(CInt(tx_部署CD.Text) = 0, "", [tx_部署CD].Text)
				'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト H_HANTOKUCD の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				H_HANTOKUCD = SpcToNull(([tx_販売先納得意先CD].Text))
				'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト H_HANNTOKUCD の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				H_HANNTOKUCD = SpcToNull(([tx_販売先納得意先CD].Text))
				'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト H_HANNONYUCD の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				H_HANNONYUCD = SpcToNull(([tx_販売先納入先CD].Text))

				'            HD_営業推進部CD = NullToZero(![営業推進部CD]) '2019/12/12 ADD
				'            HSUISHINCD = NullToZero(![営業推進部CD]) '2019/12/12 ADD

				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				HD_工事担当CD = NullToZero(.Fields("工事担当CD").Value) '2022/08/08 ADD
				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト HKOUJITANTOCD の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				HKOUJITANTOCD = NullToZero(.Fields("工事担当CD").Value) '2022/08/08 ADD

				'2013/03/08 ADD↑

				'2013/07/19 ADD↓
				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				ck_社内伝票扱い.CheckState = NullToZero(.Fields("社内伝票扱い").Value, 0)
				'2013/07/19 ADD↑

				'2014/09/15 ADD
				'UPGRADE_WARNING: オブジェクト H_社内伝票扱い の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				H_社内伝票扱い = ck_社内伝票扱い.CheckState

				'2014/10/28 ADD
				'UPGRADE_WARNING: オブジェクト H_出精値引 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				H_出精値引 = [tx_出精値引].Text

				'2015/06/12 ADD↓
				[tx_ウエルシア物件内容CD].Text = CType((.Fields("ウエルシア物件内容CD").Value), Integer).ToString("#")
				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				[tx_ウエルシア物件内容名].Text = NullToZero(.Fields("ウエルシア物件内容名").Value, "")

				tx_ウエルシア売場面積.Text = CType((.Fields("ウエルシア売場面積").Value), Integer).ToString("#")

				[tx_ウエルシアリース区分].Text = CType((.Fields("ウエルシアリース区分").Value), Integer).ToString("#")

				[tx_ウエルシア物件区分].Text = CType((.Fields("ウエルシア物件区分CD").Value), Integer).ToString("#")
				With cWelBukkenKubun
					.Initialize()
					.[ウエルシア物件区分CD] = tx_ウエルシア物件区分.Text
					.GetbyID()
					rf_ウエルシア物件区分名.Text = .ウエルシア物件区分名
				End With

				'UPGRADE_WARNING: オブジェクト HWELNAICD の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				HWELNAICD = tx_ウエルシア物件内容CD.Text
				'UPGRADE_WARNING: オブジェクト HWELBKNCD の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				HWELBKNCD = [tx_ウエルシア物件区分].Text
				'2015/06/12 ADD↑

				'2015/07/10 ADD↓
				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				[tx_物件番号].Text = CType(NullToZero(.Fields("物件番号").Value), Integer).ToString("#")
				'UPGRADE_WARNING: オブジェクト HBUKKENNO の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				HBUKKENNO = tx_物件番号.Text

				With cBukken
					.Initialize()
					.[物件番号] = NullToZero(tx_物件番号.Text, 0)
					.GetbyID()
					rf_物件略称.Text = .物件略称
				End With

				'2015/07/10 ADD↑
				'2024/01/16 ADD↓
				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'tx_統合見積番号.Text = NullToZero(.Fields("統合見積番号").Value)
				tx_統合見積番号.Text = CType(NullToZero(.Fields("統合見積番号").Value), Integer).ToString("#") '2024/07/15 UPD
				'UPGRADE_WARNING: オブジェクト H統合見積NO の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'H統合見積NO = tx_統合見積番号.Text
				H統合見積NO = CType(NullToZero([tx_統合見積番号].Text), Integer).ToString("#") '2024/07/15 UPD

				With cMitumoriH
					.Initialize()
					'.見積番号 = CInt(tx_統合見積番号.Text)
					.見積番号 = SpcToNull([tx_統合見積番号].Text, 0) '2024/07/15 UPD
					.GetbyID()
					rf_統合見積件名.Text = .[見積件名]
				End With
				'2024/01/16 ADD↑


				'2015/10/16 ADD↓
				idc(5).Text = .Fields("受付日付").Value
				idc(6).Text = .Fields("完工日付").Value
				tx_発注担当者名.Text = .Fields("発注担当者名").Value
				[tx_作業内容].Text = .Fields("作業内容").Value
				[tx_YKサプライ区分].Text = CType((.Fields("YKサプライ区分").Value), Integer).ToString("#")
				[tx_YK物件区分].Text = CType((.Fields("YK物件区分").Value), Integer).ToString("#")
				[tx_YK請求区分].Text = CType((.Fields("YK請求区分").Value), Integer).ToString("#")
				'2015/10/16 ADD↑

				'2015/11/03 ADD↓
				[tx_化粧品メーカー区分].Text = CType((.Fields("化粧品メーカー区分").Value), Integer).ToString("#")
				'2015/11/03 ADD↑

				[tx_クレーム区分].Text = CType((.Fields("クレーム区分").Value), Integer).ToString("#") '2016/06/22 ADD


				'2020/04/11 ADD↓
				If NullToZero((.Fields("発注書発行日付").Value), "").ToString = "" Then
					[tx_発注書発行日付].Text = ""
				Else
					[tx_発注書発行日付].Text = CType((.Fields("発注書発行日付").Value), DateTime).ToString("yyyy年MM月dd日")
				End If

				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'[tx_見積確定区分].Text = NullToZero(.Fields("見積確定区分").Value, "") '2024/06/21 DEL
				HD_見積確定区分 = NullToZero(.Fields("見積確定区分").Value) '2024/06/21 ADD

				idc(7).Text = .Fields("完了日付").Value
				'UPGRADE_WARNING: オブジェクト idc().Text の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト HD_完了日付 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				HD_完了日付 = idc(7).Text
				'UPGRADE_WARNING: オブジェクト idc().Text の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト HKANRYOBI の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				HKANRYOBI = idc(7).Text


				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				[tx_完了者名].Text = NullToZero(.Fields("完了者名").Value, "")

				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				HD_入力USERID = NullToZero(.Fields("入力USERID").Value, "")

				idc(8).Text = .Fields("請求予定日").Value
				'UPGRADE_WARNING: オブジェクト idc().Text の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト HD_請求予定日 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				HD_請求予定日 = idc(8).Text
				'            HKANRYOBI = idc(7).Text

				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				[tx_経過備考1].Text = NullToZero(.Fields("経過備考1").Value, "")
				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				[tx_経過備考2].Text = NullToZero(.Fields("経過備考2").Value, "")

				'2020/04/11 ADD↑

				'2021/02/25 ADD↓
				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				tx_集計CD.Text = NullToZero(.Fields("集計CD").Value, "")
				With cSyuukei
					.Initialize()
					.[得意先CD] = tx_集計CD.Text
					.GetbyID()

					rf_集計名.Text = .略称
				End With
				'UPGRADE_WARNING: オブジェクト HSYUKEICD の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				HSYUKEICD = tx_集計CD.Text
				'2021/02/25 ADD↑

				'2022/10/10 ADD↓
				[tx_B請求管轄区分].Text = CType((.Fields("B請求管轄区分").Value), Integer).ToString("#")
				[tx_BtoB番号].Text = CType((.Fields("BtoB番号").Value), Integer).ToString("#")
				'2022/10/10 ADD↑

				'UPGRADE_WARNING: オブジェクト HBUKKENSYU の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				HBUKKENSYU = [tx_物件種別].Text '2022/11/22 ADD

				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				[tx_業種区分].Text = NullToZero(.Fields("業種区分").Value, "") '2023/04/18 ADD

				'UPGRADE_WARNING: オブジェクト H業種区分 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				H業種区分 = tx_業種区分.Text '2024/02/14 ADD


			End If
		End With

		ReleaseRs(rs)

		ModeIndicate(1)

		'社内伝「9999」ならばロックしない
		'    If [tx_得意先CD].Text = "9999" Then
		If [tx_得意先CD].Text = "9999" Or [tx_得意先CD].Text = "3055" Then '2015/01/30 ADD
			EnableHanbai(True)
		Else
			EnableHanbai(False)
		End If

		'2020/05/26 ADD
		If IsExist請求売上(No) = True Then
			'請求済み
			'変更不可にする
			[tx_完了日Y].Locked = True
			[tx_完了日M].Locked = True
			[tx_完了日D].Locked = True
			[tx_請求予定Y].Locked = True
			[tx_請求予定M].Locked = True
			[tx_請求予定D].Locked = True
			[tx_経過備考1].Locked = True
			[tx_経過備考2].Locked = True
		Else
			'未請求
			[tx_完了日Y].Locked = False
			[tx_完了日M].Locked = False
			[tx_完了日D].Locked = False
			[tx_請求予定Y].Locked = False
			[tx_請求予定M].Locked = False
			[tx_請求予定D].Locked = False
			[tx_経過備考1].Locked = False
			[tx_経過備考2].Locked = False
		End If

		'    '更新のメッセージ
		'    '選択画面で制御しているので完納データはこないはず
		'    If (KANNOOF = 1) Then
		'        DoEvents
		'        Inform "完納データです。"
		'    End If

		Download = True
Download_correct:
		HourGlass(False)
		Exit Function
Download_Err:
		MsgBox(Err.Number & " " & Err.Description)
		HourGlass(False)
	End Function

	Private Sub InitialItems()
		idc(0).Text = Today
		[tx_物件金額].Text = CStr(0)
		[tx_物件種別].Text = CStr(0)
		[tx_支払条件].Text = CStr(0)
		[tx_納期表示].Text = CStr(0)
		'tx_出力日.Text = 0
		[tx_出力日].Text = CStr(1) '2017/06/09 ADD
		[tx_有効期限].Text = CStr(30)
		'    tx_受注区分.Text = 0
		[tx_受注区分].Text = CStr(1) '2014/04/22 ADD
		[tx_大小口区分].Text = CStr(0)
		[tx_出精値引].Text = CStr(0)

		'    tx_見積区分.Text = 0    '2009/09/24 ADD
		'UPGRADE_WARNING: オブジェクト idc().Text の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		idc(4).Text = idc(0).Text '2014/04/22 ADD
	End Sub

	'2015/07/10 ADD↓
	Private Sub tx_物件番号_Enter(sender As Object, e As EventArgs) Handles tx_物件番号.Enter
		If SelectF = False Then
			If Item_Check(([tx_物件番号].TabIndex)) = False Then
				Exit Sub
			End If
		End If
		SelectF = False

		PreviousControl = Me.ActiveControl
		'ボタン名設定
		'UPGRADE_ISSUE: Control Name は、汎用名前空間 ActiveControl 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
		Call SetUpFuncs(Me.ActiveControl.Name)
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = "物件番号を入力して下さい。　選択画面：Space"
	End Sub

	Private Sub tx_物件番号_KeyPress(sender As Object, e As KeyPressEventArgs) Handles tx_物件番号.KeyPress
		Dim KeyAscii As Integer = Asc(e.KeyChar)
		If KeyAscii = Asc(" ") And ([tx_物件番号].SelStart = 0 And [tx_物件番号].SelLength = Len([tx_物件番号].Text)) Then
			KeyAscii = 0
			'---参照画面表示
			SelectF = True
			If cBukken.ShowDialog = True Then
				[tx_物件番号].Text = CStr(cBukken.物件番号)
				ReturnF = True
				[tx_物件番号].NextSetFocus()
			Else
				[tx_物件番号].Focus()
			End If
		End If
	End Sub

	Private Sub tx_物件番号_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_物件番号.KeyDown
		ReturnF = True
	End Sub

	Private Sub tx_物件番号_Leave(sender As Object, e As EventArgs) Handles tx_物件番号.Leave
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = ""
		If ReturnF = False Then
			'UPGRADE_WARNING: オブジェクト tx_物件番号.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[tx_物件番号].Undo()
		End If
		ReturnF = False
	End Sub
	'2015/07/10 ADD↑

	'2024/01/16 ADD↓
	Private Sub tx_統合見積番号_Enter(sender As Object, e As EventArgs) Handles tx_統合見積番号.Enter
		If SelectF = False Then
			If Item_Check(([tx_統合見積番号].TabIndex)) = False Then
				Exit Sub
			End If
		End If
		SelectF = False

		PreviousControl = Me.ActiveControl
		'ボタン名設定
		'UPGRADE_ISSUE: Control Name は、汎用名前空間 ActiveControl 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
		Call SetUpFuncs(Me.ActiveControl.Name)
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = "統合見積番号を入力して下さい。　選択画面：Space"
	End Sub

	Private Sub tx_統合見積番号_KeyPress(sender As Object, e As KeyPressEventArgs) Handles tx_統合見積番号.KeyPress
		Dim KeyAscii As Integer = Asc(e.KeyChar)
		If KeyAscii = Asc(" ") And ([tx_統合見積番号].SelStart = 0 And [tx_統合見積番号].SelLength = Len([tx_統合見積番号].Text)) Then
			KeyAscii = 0
			'---参照画面表示
			SelectF = True
			If cMitumoriH.ShowDialog = True Then
				[tx_統合見積番号].Text = CStr(cMitumoriH.見積番号)
				ReturnF = True
				[tx_統合見積番号].NextSetFocus()
			Else
				[tx_統合見積番号].Focus()
			End If
		End If
	End Sub

	Private Sub tx_統合見積番号_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_統合見積番号.KeyDown
		ReturnF = True
	End Sub

	Private Sub tx_統合見積番号_Leave(sender As Object, e As EventArgs) Handles tx_統合見積番号.Leave
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = ""
		If ReturnF = False Then
			'UPGRADE_WARNING: オブジェクト tx_統合見積番号.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[tx_統合見積番号].Undo()
		End If
		ReturnF = False
	End Sub
	'2024/01/16 ADD↑

	Private Sub tx_担当者CD_Enter(sender As Object, e As EventArgs) Handles tx_担当者CD.Enter
		If SelectF = False Then
			If Item_Check(([tx_担当者CD].TabIndex)) = False Then
				Exit Sub
			End If
		End If
		SelectF = False
		'ボタン名設定
		'UPGRADE_ISSUE: Control Name は、汎用名前空間 ActiveControl 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
		Call SetUpFuncs(Me.ActiveControl.Name)
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = "担当者CDを入力して下さい。　選択画面：Space"
	End Sub

	Private Sub tx_担当者CD_KeyPress(sender As Object, e As KeyPressEventArgs) Handles tx_担当者CD.KeyPress
		Dim KeyAscii As Integer = Asc(e.KeyChar)
		If KeyAscii = Asc(" ") And ([tx_担当者CD].SelStart = 0 And [tx_担当者CD].SelLength = Len([tx_担当者CD].Text)) Then
			KeyAscii = 0
			'---参照画面表示
			SelectF = True
			If cTanto.ShowDialog = True Then
				[tx_担当者CD].Text = cTanto.担当者CD
				ReturnF = True
				SelectF = False
				[tx_担当者CD].NextSetFocus()
			Else
				[tx_担当者CD].Focus()
			End If
			'       '---参照画面表示
			'       SelectF = True
			'       Call Sentak.SelSetup(tx_担当者CD _
			'                , "SELECT 担当者CD,担当者名 FROM TM担当者 " _
			'                , "担当者CD", "担当者名", "", "担当者CD", "担当者選択" _
			'                , False, Null, Null)
			'       Sentak.tx_検索名.IMEMode = 全角ひらがな
			'       Sentak.Show vbModal
			'       If [tx_担当者CD].Tag <> "" Then
			'           ReturnF = True
			'           [tx_担当者CD].NextSetFocus
			'       Else
			'           [tx_担当者CD].SetFocus
			'       End If
		End If
	End Sub

	Private Sub tx_担当者CD_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_担当者CD.KeyDown
		ReturnF = True
	End Sub

	Private Sub tx_担当者CD_Leave(sender As Object, e As EventArgs) Handles tx_担当者CD.Leave
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = ""
		If ReturnF = False Then
			'UPGRADE_WARNING: オブジェクト tx_担当者CD.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[tx_担当者CD].Undo()
		End If
		ReturnF = False
	End Sub

	'2013/03/07 ADD↓
	Private Sub tx_部署CD_Enter(sender As Object, e As EventArgs) Handles tx_部署CD.Enter
		If SelectF = False Then
			If Item_Check(([tx_部署CD].TabIndex)) = False Then
				Exit Sub
			End If
		End If
		SelectF = False
		PreviousControl = Me.ActiveControl
		'ボタン名設定
		'UPGRADE_ISSUE: Control Name は、汎用名前空間 ActiveControl 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
		Call SetUpFuncs(Me.ActiveControl.Name)
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = "部署CDを入力して下さい。　選択画面：Space"
	End Sub

	Private Sub tx_部署CD_KeyPress(sender As Object, e As KeyPressEventArgs) Handles tx_部署CD.KeyPress
		Dim KeyAscii As Integer = Asc(e.KeyChar)
		If KeyAscii = Asc(" ") And ([tx_部署CD].SelStart = 0 And [tx_部署CD].SelLength = Len([tx_部署CD].Text)) Then
			KeyAscii = 0
			'---参照画面表示
			SelectF = True
			If cTanto.部署cls.ShowDialog = True Then
				[tx_部署CD].Text = cTanto.部署cls.部署CD
				ReturnF = True
				[tx_部署CD].NextSetFocus()
			Else
				[tx_部署CD].Focus()
			End If
		End If
	End Sub

	Private Sub tx_部署CD_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_部署CD.KeyDown
		ReturnF = True
	End Sub

	Private Sub tx_部署CD_Leave(sender As Object, e As EventArgs) Handles tx_部署CD.Leave
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = ""
		If ReturnF = False Then
			'UPGRADE_WARNING: オブジェクト tx_部署CD.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[tx_部署CD].Undo()
		End If
		ReturnF = False
	End Sub
	'2013/03/07 ADD↑

	Private Sub tx_見積日付Y_Enter(sender As Object, e As EventArgs) Handles tx_見積日付Y.Enter
		'入力チェック
		If Item_Check(([tx_見積日付Y].TabIndex)) = False Then
			Exit Sub
		End If
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = "見積日付を入力して下さい。"
		PreviousControl = Me.ActiveControl
		'ボタン名設定
		'UPGRADE_ISSUE: Control Name は、汎用名前空間 ActiveControl 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
		Call SetUpFuncs(Me.ActiveControl.Name)
	End Sub

	Private Sub tx_見積日付Y_KeyPress(sender As Object, e As KeyPressEventArgs) Handles tx_見積日付Y.KeyPress
		Dim KeyAscii As Integer = Asc(e.KeyChar)
		If KeyAscii <> 8 Then ' バックスペースは例外
			'UPGRADE_WARNING: TextBox プロパティ tx_見積日付Y.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
			'UPGRADE_ISSUE: 定数 vbFromUnicode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
			'UPGRADE_ISSUE: LenB 関数はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="367764E5-F3F8-4E43-AC3E-7FE0B5E074E2"' をクリックしてください。
			If LenB([tx_見積日付Y].Text & Chr(KeyAscii)) - LenB([tx_見積日付Y].SelText) = [tx_見積日付Y].MaxLength Then
				ReturnF = True
				[tx_見積日付Y].Focus()
			End If
		End If
		e.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			e.Handled = True
		End If
	End Sub

	Private Sub tx_見積日付Y_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_見積日付Y.KeyDown
		ReturnF = True
	End Sub

	Private Sub tx_見積日付Y_Leave(sender As Object, e As EventArgs) Handles tx_見積日付Y.Leave
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = ""
		If ReturnF = False Then
			'UPGRADE_WARNING: オブジェクト tx_見積日付Y.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[tx_見積日付Y].Undo()
		End If
		ReturnF = False
	End Sub

	Private Sub tx_見積日付M_Enter(sender As Object, e As EventArgs) Handles tx_見積日付M.Enter
		'入力チェック
		If Item_Check(([tx_見積日付M].TabIndex)) = False Then
			Exit Sub
		End If
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = "見積日付を入力して下さい。"
		PreviousControl = Me.ActiveControl
		'ボタン名設定
		'UPGRADE_ISSUE: Control Name は、汎用名前空間 ActiveControl 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
		Call SetUpFuncs(Me.ActiveControl.Name)
	End Sub

	Private Sub tx_見積日付M_KeyPress(sender As Object, e As KeyPressEventArgs) Handles tx_見積日付M.KeyPress
		Dim KeyAscii As Integer = Asc(e.KeyChar)
		If KeyAscii <> 8 Then ' バックスペースは例外
			'UPGRADE_WARNING: TextBox プロパティ tx_見積日付M.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
			'UPGRADE_ISSUE: 定数 vbFromUnicode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
			'UPGRADE_ISSUE: LenB 関数はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="367764E5-F3F8-4E43-AC3E-7FE0B5E074E2"' をクリックしてください。
			If LenB([tx_見積日付M].Text & Chr(KeyAscii)) - LenB([tx_見積日付M].SelText) = [tx_見積日付M].MaxLength Then
				If KeyAscii <> 0 Then '入力キーがエラーでないならば
					ReturnF = True
					[tx_見積日付M].Focus()
				End If
			Else
				Select Case Chr(KeyAscii)
					Case CStr(2) To CStr(9)
						ReturnF = True
						[tx_見積日付D].Focus()
				End Select
			End If
		End If
		e.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			e.Handled = True
		End If
	End Sub

	Private Sub tx_見積日付M_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_見積日付M.KeyDown
		ReturnF = True
	End Sub

	Private Sub tx_見積日付M_Leave(sender As Object, e As EventArgs) Handles tx_見積日付M.Leave
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = ""
		If ReturnF = False Then
			'UPGRADE_WARNING: オブジェクト tx_見積日付M.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[tx_見積日付M].Undo()
		End If
		ReturnF = False
	End Sub

	Private Sub tx_見積日付D_Enter(sender As Object, e As EventArgs) Handles tx_見積日付D.Enter
		'入力チェック
		If Item_Check(([tx_見積日付D].TabIndex)) = False Then
			Exit Sub
		End If
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = "見積日付を入力して下さい。"
		PreviousControl = Me.ActiveControl
		'ボタン名設定
		'UPGRADE_ISSUE: Control Name は、汎用名前空間 ActiveControl 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
		Call SetUpFuncs(Me.ActiveControl.Name)
	End Sub

	Private Sub tx_見積日付D_KeyPress(sender As Object, e As KeyPressEventArgs) Handles tx_見積日付D.KeyPress
		Dim KeyAscii As Integer = Asc(e.KeyChar)
		'   If KeyAscii <> 8 Then ' バックスペースは例外
		'       If LenB([tx_見積日付D].Text & Chr$(KeyAscii)) - _
		'            LenB([tx_見積日付D].SelText) = [tx_見積日付D].MaxLength Then
		'           If KeyAscii <> 0 Then       '入力キーがエラーでないならば
		'                   ReturnF = True
		'                   [tx_見積日付D].NextSetFocus
		'           End If
		'       Else
		'           Select Case Chr$(KeyAscii)
		'               Case 4 To 9
		'                   ReturnF = True
		'                   [tx_見積日付D].NextSetFocus
		'           End Select
		'       End If
		'   End If
		e.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			e.Handled = True
		End If
	End Sub

	Private Sub tx_見積日付D_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_見積日付D.KeyDown
		ReturnF = True
	End Sub

	Private Sub tx_見積日付D_Leave(sender As Object, e As EventArgs) Handles tx_見積日付D.Leave
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = ""
		If ReturnF = False Then
			'UPGRADE_WARNING: オブジェクト tx_見積日付D.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[tx_見積日付D].Undo()
		End If
		ReturnF = False
	End Sub

	'2022/08/08 ADD↓
	Private Sub tx_工事担当CD_Enter(sender As Object, e As EventArgs) Handles tx_工事担当CD.Enter
		If SelectF = False Then
			If Item_Check(([tx_工事担当CD].TabIndex)) = False Then
				Exit Sub
			End If
		End If
		SelectF = False
		PreviousControl = Me.ActiveControl
		'ボタン名設定
		'UPGRADE_ISSUE: Control Name は、汎用名前空間 ActiveControl 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
		Call SetUpFuncs(Me.ActiveControl.Name)
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = "工事担当を入力して下さい。　選択画面：Space"
	End Sub

	Private Sub tx_工事担当CD_KeyPress(sender As Object, e As KeyPressEventArgs) Handles tx_工事担当CD.KeyPress
		Dim KeyAscii As Integer = Asc(e.KeyChar)
		If KeyAscii = Asc(" ") And ([tx_工事担当CD].SelStart = 0 And [tx_工事担当CD].SelLength = Len([tx_工事担当CD].Text)) Then
			KeyAscii = 0
			'---参照画面表示
			SelectF = True
			If cKoujiTanto.ShowDialog = True Then
				[tx_工事担当CD].Text = cKoujiTanto.工事担当CD
				ReturnF = True
				[tx_工事担当CD].NextSetFocus()
			Else
				[tx_工事担当CD].Focus()
			End If
		End If
	End Sub

	Private Sub tx_工事担当CD_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_工事担当CD.KeyDown
		ReturnF = True
	End Sub

	Private Sub tx_工事担当CD_Leave(sender As Object, e As EventArgs) Handles tx_工事担当CD.Leave
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = ""
		If ReturnF = False Then
			'UPGRADE_WARNING: オブジェクト tx_工事担当CD.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[tx_工事担当CD].Undo()
		End If
		ReturnF = False
	End Sub
	'2022/08/08 ADD↑

	'2019/12/12 ADD↓
	'Private Sub tx_営業推進部CD_GotFocus()
	'    If SelectF = False Then
	'        If Item_Check([tx_営業推進部CD].TabIndex) = False Then
	'            Exit Sub
	'        End If
	'    End If
	'    SelectF = False
	'    Set PreviousControl = Me.ActiveControl
	'    'ボタン名設定
	'    Call SetUpFuncs(Me.ActiveControl.Name)
	'    [sb_Msg].Items.Item(0).Text = "積算担当を入力して下さい。　選択画面：Space"
	'End Sub
	'
	'Private Sub tx_営業推進部CD_KeyPress(sender As Object, e As KeyPressEventArgs) Handles tx_営業推進部CD.KeyPress
	'    Dim KeyAscii As Integer = Asc(e.KeyChar)
	'    If KeyAscii = Asc(" ") And _
	'        ([tx_営業推進部CD].SelStart = 0 And [tx_営業推進部CD].SelLength = Len([tx_営業推進部CD])) Then
	'        KeyAscii = 0
	'        '---参照画面表示
	'        SelectF = True
	'        If cSuishin.ShowDialog = True Then
	'            [tx_営業推進部CD].Text = cSuishin.営業推進部CD
	'            ReturnF = True
	'            [tx_営業推進部CD].NextSetFocus
	'        Else
	'            [tx_営業推進部CD].SetFocus
	'        End If
	'    End If
	'End Sub
	'
	'Private Sub tx_営業推進部CD_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_営業推進部CD.KeyDown
	'    ReturnF = True
	'End Sub
	'
	'Private Sub tx_営業推進部CD_LostFocus()
	'    [sb_Msg].Items.Item(0).Text = ""
	'    If ReturnF = False Then
	'        [tx_営業推進部CD].Undo
	'    End If
	'    ReturnF = False
	'End Sub
	'2019/12/12 ADD↑

	Private Sub tx_見積件名_Enter(sender As Object, e As EventArgs) Handles tx_見積件名.Enter
		If Item_Check(([tx_見積件名].TabIndex)) = False Then
			Exit Sub
		End If

		PreviousControl = Me.ActiveControl
		'ボタン名設定
		'UPGRADE_ISSUE: Control Name は、汎用名前空間 ActiveControl 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
		Call SetUpFuncs(Me.ActiveControl.Name)
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = "見積件名を入力して下さい。"
	End Sub

	Private Sub tx_見積件名_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_見積件名.KeyDown
		ReturnF = True
	End Sub

	Private Sub tx_見積件名_Leave(sender As Object, e As EventArgs) Handles tx_見積件名.Leave
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = ""
		If ReturnF = False Then
			'UPGRADE_WARNING: オブジェクト tx_見積件名.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[tx_見積件名].Undo()
		End If
		ReturnF = False
	End Sub

	Private Sub tx_得意先CD_Enter(sender As Object, e As EventArgs) Handles tx_得意先CD.Enter
		If SelectF = False Then
			If Item_Check(([tx_得意先CD].TabIndex)) = False Then
				Exit Sub
			End If
			PreviousControl = Me.ActiveControl
		End If
		SelectF = False

		'ボタン名設定
		'UPGRADE_ISSUE: Control Name は、汎用名前空間 ActiveControl 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
		Call SetUpFuncs(Me.ActiveControl.Name)
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = "得意先CDを入力して下さい。　選択画面：Space"
	End Sub

	Private Sub tx_得意先CD_KeyPress(sender As Object, e As KeyPressEventArgs) Handles tx_得意先CD.KeyPress
		Dim KeyAscii As Integer = Asc(e.KeyChar)
		If KeyAscii = Asc(" ") And ([tx_得意先CD].SelStart = 0 And [tx_得意先CD].SelLength = Len([tx_得意先CD].Text)) Then
			KeyAscii = 0
			'---参照画面表示
			SelectF = True
			'---参照画面表示
			If cTokuisaki.ShowDialog = True Then
				[tx_得意先CD].Text = cTokuisaki.得意先CD
				ReturnF = True
				[tx_得意先CD].NextSetFocus()
			Else
				[tx_得意先CD].Focus()
			End If
		End If
		e.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			e.Handled = True
		End If
	End Sub

	Private Sub tx_得意先CD_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_得意先CD.KeyDown
		ReturnF = True
	End Sub

	Private Sub tx_得意先CD_Leave(sender As Object, e As EventArgs) Handles tx_得意先CD.Leave
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = ""
		If ReturnF = False Then
			'UPGRADE_WARNING: オブジェクト tx_得意先CD.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[tx_得意先CD].Undo()
		End If
		ReturnF = False
	End Sub

	Private Sub tx_得意先名1_Enter(sender As Object, e As EventArgs) Handles tx_得意先名1.Enter
		If Item_Check(([tx_得意先名1].TabIndex)) = False Then
			Exit Sub
		End If

		PreviousControl = Me.ActiveControl
		'ボタン名設定
		'UPGRADE_ISSUE: Control Name は、汎用名前空間 ActiveControl 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
		Call SetUpFuncs(Me.ActiveControl.Name)
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = "得意先名1を入力して下さい。"
	End Sub

	Private Sub tx_得意先名1_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_得意先名1.KeyDown
		ReturnF = True
	End Sub

	Private Sub tx_得意先名1_Leave(sender As Object, e As EventArgs) Handles tx_得意先名1.Leave
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = ""
		If ReturnF = False Then
			'UPGRADE_WARNING: オブジェクト tx_得意先名1.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[tx_得意先名1].Undo()
		End If
		ReturnF = False
	End Sub

	Private Sub tx_得意先名2_Enter(sender As Object, e As EventArgs) Handles tx_得意先名2.Enter
		If Item_Check(([tx_得意先名2].TabIndex)) = False Then
			Exit Sub
		End If

		PreviousControl = Me.ActiveControl
		'ボタン名設定
		'UPGRADE_ISSUE: Control Name は、汎用名前空間 ActiveControl 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
		Call SetUpFuncs(Me.ActiveControl.Name)
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = "得意先名2を入力して下さい。"
	End Sub

	Private Sub tx_得意先名2_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_得意先名2.KeyDown
		ReturnF = True
	End Sub

	Private Sub tx_得意先名2_Leave(sender As Object, e As EventArgs) Handles tx_得意先名2.Leave
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = ""
		If ReturnF = False Then
			'UPGRADE_WARNING: オブジェクト tx_得意先名2.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[tx_得意先名2].Undo()
		End If
		ReturnF = False
	End Sub

	Private Sub tx_得TEL_Enter(sender As Object, e As EventArgs) Handles tx_得TEL.Enter
		If Item_Check(([tx_得TEL].TabIndex)) = False Then
			Exit Sub
		End If

		PreviousControl = Me.ActiveControl
		'ボタン名設定
		'UPGRADE_ISSUE: Control Name は、汎用名前空間 ActiveControl 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
		Call SetUpFuncs(Me.ActiveControl.Name)
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = "ＴＥＬを入力して下さい。"
	End Sub

	Private Sub tx_得TEL_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_得TEL.KeyDown
		ReturnF = True
	End Sub

	Private Sub tx_得TEL_Leave(sender As Object, e As EventArgs) Handles tx_得TEL.Leave
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = ""
		If ReturnF = False Then
			'UPGRADE_WARNING: オブジェクト tx_得TEL.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[tx_得TEL].Undo()
		End If
		ReturnF = False
	End Sub

	Private Sub tx_得FAX_Enter(sender As Object, e As EventArgs) Handles tx_得FAX.Enter
		If Item_Check(([tx_得FAX].TabIndex)) = False Then
			Exit Sub
		End If

		PreviousControl = Me.ActiveControl
		'ボタン名設定
		'UPGRADE_ISSUE: Control Name は、汎用名前空間 ActiveControl 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
		Call SetUpFuncs(Me.ActiveControl.Name)
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = "ＦＡＸを入力して下さい。"
	End Sub

	Private Sub tx_得FAX_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_得FAX.KeyDown
		ReturnF = True
	End Sub

	Private Sub tx_得FAX_Leave(sender As Object, e As EventArgs) Handles tx_得FAX.Leave
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = ""
		If ReturnF = False Then
			'UPGRADE_WARNING: オブジェクト tx_得FAX.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[tx_得FAX].Undo()
		End If
		ReturnF = False
	End Sub

	Private Sub tx_得担当者_Enter(sender As Object, e As EventArgs) Handles tx_得担当者.Enter
		If Item_Check(([tx_得担当者].TabIndex)) = False Then
			Exit Sub
		End If

		PreviousControl = Me.ActiveControl
		'ボタン名設定
		'UPGRADE_ISSUE: Control Name は、汎用名前空間 ActiveControl 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
		Call SetUpFuncs(Me.ActiveControl.Name)
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = "得意先担当者名を入力して下さい。"
	End Sub

	Private Sub tx_得担当者_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_得担当者.KeyDown
		ReturnF = True
	End Sub

	Private Sub tx_得担当者_Leave(sender As Object, e As EventArgs) Handles tx_得担当者.Leave
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = ""
		If ReturnF = False Then
			'UPGRADE_WARNING: オブジェクト tx_得担当者.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[tx_得担当者].Undo()
		End If
		ReturnF = False
	End Sub

	'2016/04/07 ADD↓
	Private Sub tx_税集計区分_Enter(sender As Object, e As EventArgs) Handles tx_税集計区分.Enter
		If Item_Check(([tx_税集計区分].TabIndex)) = False Then
			Exit Sub
		End If
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = "税集計区分を入力して下さい。"
		PreviousControl = Me.ActiveControl

		'ボタン名設定
		'UPGRADE_ISSUE: Control Name は、汎用名前空間 ActiveControl 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
		Call SetUpFuncs(Me.ActiveControl.Name)
	End Sub

	Private Sub tx_税集計区分_KeyPress(sender As Object, e As KeyPressEventArgs) Handles tx_税集計区分.KeyPress
		Dim KeyAscii As Integer = Asc(e.KeyChar)
		'MaxLengthは１多く設定する。

		Const Numbers As String = "013" ' 入力許可文字
		'Dim strText As String

		If KeyAscii = System.Windows.Forms.Keys.Return Then KeyAscii = 0

		'数字検査をする
		If KeyAscii <> 8 Then ' バックスペースは例外
			If InStr(Numbers, Chr(KeyAscii)) = 0 Then
				KeyAscii = 0 ' 入力を無効にする
				GoTo EventExitSub
			Else
				'UPGRADE_WARNING: TextBox プロパティ tx_税集計区分.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
				'UPGRADE_ISSUE: 定数 vbFromUnicode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
				'UPGRADE_ISSUE: LenB 関数はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="367764E5-F3F8-4E43-AC3E-7FE0B5E074E2"' をクリックしてください。
				If LenB([tx_税集計区分].Text) = [tx_税集計区分].MaxLength - 1 Then
					Call SelText([tx_税集計区分])
				End If
				'UPGRADE_WARNING: TextBox プロパティ tx_税集計区分.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
				'UPGRADE_ISSUE: 定数 vbFromUnicode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
				'UPGRADE_ISSUE: LenB 関数はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="367764E5-F3F8-4E43-AC3E-7FE0B5E074E2"' をクリックしてください。
				If LenB([tx_税集計区分].Text & Chr(KeyAscii)) - LenB([tx_税集計区分].SelText) = [tx_税集計区分].MaxLength - 1 Then
					ReturnF = True
					[tx_税集計区分].NextSetFocus()
				End If
			End If
		End If
EventExitSub:
		e.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			e.Handled = True
		End If
	End Sub

	Private Sub tx_税集計区分_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_税集計区分.KeyDown
		ReturnF = True
	End Sub

	Private Sub tx_税集計区分_Leave(sender As Object, e As EventArgs) Handles tx_税集計区分.Leave
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = ""
		If ReturnF = False Then
			'UPGRADE_WARNING: オブジェクト tx_税集計区分.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[tx_税集計区分].Undo()
		End If
		ReturnF = False
	End Sub
	'2016/04/07 ADD↑

	'2021/02/25 ADD↓
	Private Sub tx_集計CD_Enter(sender As Object, e As EventArgs) Handles tx_集計CD.Enter
		If SelectF = False Then
			If Item_Check(([tx_集計CD].TabIndex)) = False Then
				Exit Sub
			End If
			PreviousControl = Me.ActiveControl
		End If
		SelectF = False

		PreviousControl = Me.ActiveControl
		'ボタン名設定
		'UPGRADE_ISSUE: Control Name は、汎用名前空間 ActiveControl 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
		Call SetUpFuncs(Me.ActiveControl.Name)
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = "集計CDを入力して下さい。　選択画面：Space"
	End Sub

	Private Sub tx_集計CD_KeyPress(sender As Object, e As KeyPressEventArgs) Handles tx_集計CD.KeyPress
		Dim KeyAscii As Integer = Asc(e.KeyChar)
		If KeyAscii = Asc(" ") And ([tx_集計CD].SelStart = 0 And [tx_集計CD].SelLength = Len([tx_集計CD].Text)) Then
			KeyAscii = 0
			'---参照画面表示
			SelectF = True
			If cSyuukei.ShowDialog = True Then
				[tx_集計CD].Text = cSyuukei.得意先CD
				ReturnF = True
				[tx_集計CD].NextSetFocus()
			Else
				[tx_集計CD].Focus()
			End If
		End If
	End Sub

	Private Sub tx_集計CD_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_集計CD.KeyDown
		ReturnF = True
	End Sub

	Private Sub tx_集計CD_Leave(sender As Object, e As EventArgs) Handles tx_集計CD.Leave
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = ""
		If ReturnF = False Then
			'UPGRADE_WARNING: オブジェクト tx_集計CD.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[tx_集計CD].Undo()
		End If
		ReturnF = False
	End Sub
	'2021/02/25 ADD↑

	Private Sub tx_納得意先CD_Enter(sender As Object, e As EventArgs) Handles tx_納得意先CD.Enter
		If SelectF = False Then
			If Item_Check(([tx_納得意先CD].TabIndex)) = False Then
				Exit Sub
			End If
			PreviousControl = Me.ActiveControl
		End If
		SelectF = False

		'ボタン名設定
		'UPGRADE_ISSUE: Control Name は、汎用名前空間 ActiveControl 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
		Call SetUpFuncs(Me.ActiveControl.Name)
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = "得意先CDを入力して下さい。　選択画面：Space"
	End Sub

	Private Sub tx_納得意先CD_KeyPress(sender As Object, e As KeyPressEventArgs) Handles tx_納得意先CD.KeyPress
		Dim KeyAscii As Integer = Asc(e.KeyChar)
		If KeyAscii = Asc(" ") And ([tx_納得意先CD].SelStart = 0 And [tx_納得意先CD].SelLength = Len([tx_納得意先CD].Text)) Then
			KeyAscii = 0
			'---参照画面表示
			SelectF = True
			If cTokuisaki.ShowDialog = True Then
				[tx_納得意先CD].Text = cTokuisaki.得意先CD
				ReturnF = True
				[tx_納得意先CD].NextSetFocus()
			Else
				[tx_納得意先CD].Focus()
			End If
		End If
		e.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			e.Handled = True
		End If
	End Sub

	Private Sub tx_納得意先CD_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_納得意先CD.KeyDown
		ReturnF = True
	End Sub

	Private Sub tx_納得意先CD_Leave(sender As Object, e As EventArgs) Handles tx_納得意先CD.Leave
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = ""
		If ReturnF = False Then
			'UPGRADE_WARNING: オブジェクト tx_納得意先CD.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[tx_納得意先CD].Undo()
		End If
		ReturnF = False
	End Sub

	Private Sub tx_納入先CD_Enter(sender As Object, e As EventArgs) Handles tx_納入先CD.Enter
		If SelectF = False Then
			If Item_Check(([tx_納入先CD].TabIndex)) = False Then
				Exit Sub
			End If
			PreviousControl = Me.ActiveControl
		End If
		SelectF = False
		'ボタン名設定
		'UPGRADE_ISSUE: Control Name は、汎用名前空間 ActiveControl 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
		Call SetUpFuncs(Me.ActiveControl.Name)
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = "納入先CDを入力して下さい。　選択画面：Space"
	End Sub

	Private Sub tx_納入先CD_KeyPress(sender As Object, e As KeyPressEventArgs) Handles tx_納入先CD.KeyPress
		Dim KeyAscii As Integer = Asc(e.KeyChar)
		If KeyAscii = Asc(" ") And ([tx_納入先CD].SelStart = 0 And [tx_納入先CD].SelLength = Len([tx_納入先CD].Text)) Then
			KeyAscii = 0
			'---参照画面表示
			SelectF = True
			cNonyusaki.得意先CD = [tx_納得意先CD].Text
			If cNonyusaki.ShowDialog = True Then
				[tx_納入先CD].Text = cNonyusaki.納入先CD
				ReturnF = True
				[tx_納入先CD].NextSetFocus()
			Else
				[tx_納入先CD].Focus()
			End If
		End If
		e.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			e.Handled = True
		End If
	End Sub

	Private Sub tx_納入先CD_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_納入先CD.KeyDown
		ReturnF = True
	End Sub

	Private Sub tx_納入先CD_Leave(sender As Object, e As EventArgs) Handles tx_納入先CD.Leave
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = ""
		If ReturnF = False Then
			'UPGRADE_WARNING: オブジェクト tx_納入先CD.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[tx_納入先CD].Undo()
		End If
		ReturnF = False
	End Sub

	Private Sub tx_納入先名1_Enter(sender As Object, e As EventArgs) Handles tx_納入先名1.Enter
		If Item_Check(([tx_納入先名1].TabIndex)) = False Then
			Exit Sub
		End If

		PreviousControl = Me.ActiveControl
		'ボタン名設定
		'UPGRADE_ISSUE: Control Name は、汎用名前空間 ActiveControl 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
		Call SetUpFuncs(Me.ActiveControl.Name)
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = "納入先名１を入力して下さい。"
	End Sub

	Private Sub tx_納入先名1_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_納入先名1.KeyDown
		ReturnF = True
	End Sub

	Private Sub tx_納入先名1_Leave(sender As Object, e As EventArgs) Handles tx_納入先名1.Leave
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = ""
		If ReturnF = False Then
			'UPGRADE_WARNING: オブジェクト tx_納入先名1.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[tx_納入先名1].Undo()
		End If
		ReturnF = False
	End Sub

	Private Sub tx_納入先名2_Enter(sender As Object, e As EventArgs) Handles tx_納入先名2.Enter
		If Item_Check(([tx_納入先名2].TabIndex)) = False Then
			Exit Sub
		End If

		PreviousControl = Me.ActiveControl
		'ボタン名設定
		'UPGRADE_ISSUE: Control Name は、汎用名前空間 ActiveControl 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
		Call SetUpFuncs(Me.ActiveControl.Name)
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = "納入先名２を入力して下さい。"
	End Sub

	Private Sub tx_納入先名2_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_納入先名2.KeyDown
		ReturnF = True
	End Sub

	Private Sub tx_納入先名2_Leave(sender As Object, e As EventArgs) Handles tx_納入先名2.Leave
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = ""
		If ReturnF = False Then
			'UPGRADE_WARNING: オブジェクト tx_納入先名2.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[tx_納入先名2].Undo()
		End If
		ReturnF = False
	End Sub

	Private Sub tx_郵便番号_Enter(sender As Object, e As EventArgs) Handles tx_郵便番号.Enter
		If Item_Check(([tx_郵便番号].TabIndex)) = False Then
			Exit Sub
		End If

		PreviousControl = Me.ActiveControl
		'ボタン名設定
		'UPGRADE_ISSUE: Control Name は、汎用名前空間 ActiveControl 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
		Call SetUpFuncs(Me.ActiveControl.Name)
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = "郵便番号を入力して下さい。"
	End Sub

	Private Sub tx_郵便番号_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_郵便番号.KeyDown
		ReturnF = True
	End Sub

	Private Sub tx_郵便番号_Leave(sender As Object, e As EventArgs) Handles tx_郵便番号.Leave
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = ""
		If ReturnF = False Then
			'UPGRADE_WARNING: オブジェクト tx_郵便番号.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[tx_郵便番号].Undo()
		End If
		ReturnF = False
	End Sub

	Private Sub tx_納住所1_Enter(sender As Object, e As EventArgs) Handles tx_納住所1.Enter
		If Item_Check(([tx_納住所1].TabIndex)) = False Then
			Exit Sub
		End If

		PreviousControl = Me.ActiveControl
		'ボタン名設定
		'UPGRADE_ISSUE: Control Name は、汎用名前空間 ActiveControl 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
		Call SetUpFuncs(Me.ActiveControl.Name)
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = "住所1を入力して下さい。"
	End Sub

	Private Sub tx_納住所1_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_納住所1.KeyDown
		ReturnF = True
	End Sub

	Private Sub tx_納住所1_Leave(sender As Object, e As EventArgs) Handles tx_納住所1.Leave
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = ""
		If ReturnF = False Then
			'UPGRADE_WARNING: オブジェクト tx_納住所1.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[tx_納住所1].Undo()
		End If
		ReturnF = False
	End Sub

	Private Sub tx_納住所2_Enter(sender As Object, e As EventArgs) Handles tx_納住所2.Enter
		If Item_Check(([tx_納住所2].TabIndex)) = False Then
			Exit Sub
		End If

		PreviousControl = Me.ActiveControl
		'ボタン名設定
		'UPGRADE_ISSUE: Control Name は、汎用名前空間 ActiveControl 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
		Call SetUpFuncs(Me.ActiveControl.Name)
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = "住所2を入力して下さい。"
	End Sub

	Private Sub tx_納住所2_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_納住所2.KeyDown
		ReturnF = True
	End Sub

	Private Sub tx_納住所2_Leave(sender As Object, e As EventArgs) Handles tx_納住所2.Leave
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = ""
		If ReturnF = False Then
			'UPGRADE_WARNING: オブジェクト tx_納住所2.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[tx_納住所2].Undo()
		End If
		ReturnF = False
	End Sub

	Private Sub tx_納TEL_Enter(sender As Object, e As EventArgs) Handles tx_納TEL.Enter
		If Item_Check(([tx_納TEL].TabIndex)) = False Then
			Exit Sub
		End If

		PreviousControl = Me.ActiveControl
		'ボタン名設定
		'UPGRADE_ISSUE: Control Name は、汎用名前空間 ActiveControl 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
		Call SetUpFuncs(Me.ActiveControl.Name)
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = "ＴＥＬを入力して下さい。"
	End Sub

	Private Sub tx_納TEL_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_納TEL.KeyDown
		ReturnF = True
	End Sub

	Private Sub tx_納TEL_Leave(sender As Object, e As EventArgs) Handles tx_納TEL.Leave
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = ""
		If ReturnF = False Then
			'UPGRADE_WARNING: オブジェクト tx_納TEL.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[tx_納TEL].Undo()
		End If
		ReturnF = False
	End Sub

	Private Sub tx_納FAX_Enter(sender As Object, e As EventArgs) Handles tx_納FAX.Enter
		If Item_Check(([tx_納FAX].TabIndex)) = False Then
			Exit Sub
		End If

		PreviousControl = Me.ActiveControl
		'ボタン名設定
		'UPGRADE_ISSUE: Control Name は、汎用名前空間 ActiveControl 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
		Call SetUpFuncs(Me.ActiveControl.Name)
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = "ＦＡＸを入力して下さい。"
	End Sub

	Private Sub tx_納FAX_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_納FAX.KeyDown
		ReturnF = True
	End Sub

	Private Sub tx_納FAX_Leave(sender As Object, e As EventArgs) Handles tx_納FAX.Leave
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = ""
		If ReturnF = False Then
			'UPGRADE_WARNING: オブジェクト tx_納FAX.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[tx_納FAX].Undo()
		End If
		ReturnF = False
	End Sub

	Private Sub tx_納担当者_Enter(sender As Object, e As EventArgs) Handles tx_納担当者.Enter
		If Item_Check(([tx_納担当者].TabIndex)) = False Then
			Exit Sub
		End If

		PreviousControl = Me.ActiveControl
		'ボタン名設定
		'UPGRADE_ISSUE: Control Name は、汎用名前空間 ActiveControl 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
		Call SetUpFuncs(Me.ActiveControl.Name)
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = "納入先担当者名を入力して下さい。"
	End Sub

	Private Sub tx_納担当者_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_納担当者.KeyDown
		ReturnF = True
	End Sub

	Private Sub tx_納担当者_Leave(sender As Object, e As EventArgs) Handles tx_納担当者.Leave
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = ""
		If ReturnF = False Then
			'UPGRADE_WARNING: オブジェクト tx_納担当者.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[tx_納担当者].Undo()
		End If
		ReturnF = False
	End Sub
	'2013/07/19 ADD↓

	Private Sub Ck_社内伝票扱い_Enter(sender As Object, e As EventArgs) Handles ck_社内伝票扱い.Enter
		If Item_Check(([ck_社内伝票扱い].TabIndex)) = False Then
			Exit Sub
		End If

		PreviousControl = Me.ActiveControl
		'ボタン名設定
		'UPGRADE_ISSUE: Control Name は、汎用名前空間 ActiveControl 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
		Call SetUpFuncs(Me.ActiveControl.Name)
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = "社内伝扱いの場合はチェックを付けて下さい。"
	End Sub

	Private Sub Ck_社内伝票扱い_KeyDown(sender As Object, e As KeyEventArgs) Handles ck_社内伝票扱い.KeyDown
		Dim KeyCode As Integer = e.KeyCode
		Dim Shift As Integer = e.KeyData \ &H10000
		If KeyCode = System.Windows.Forms.Keys.Return Then
			SendTabKey()
		End If
	End Sub

	Private Sub Ck_社内伝票扱い_Leave(sender As Object, e As EventArgs) Handles ck_社内伝票扱い.Leave
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = ""
		ReturnF = False
	End Sub
	'2013/07/19 ADD↑

	'2013/03/07 ADD↓
	Private Sub tx_販売先得意先CD_Enter(sender As Object, e As EventArgs) Handles tx_販売先得意先CD.Enter
		If SelectF = False Then
			If Item_Check(([tx_販売先得意先CD].TabIndex)) = False Then
				Exit Sub
			End If
			PreviousControl = Me.ActiveControl
		End If
		SelectF = False

		'ボタン名設定
		'UPGRADE_ISSUE: Control Name は、汎用名前空間 ActiveControl 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
		Call SetUpFuncs(Me.ActiveControl.Name)
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = "販売先得意先CDを入力して下さい。　選択画面：Space"
	End Sub

	Private Sub tx_販売先得意先CD_KeyPress(sender As Object, e As KeyPressEventArgs) Handles tx_販売先得意先CD.KeyPress
		Dim KeyAscii As Integer = Asc(e.KeyChar)
		If KeyAscii = Asc(" ") And ([tx_販売先得意先CD].SelStart = 0 And [tx_販売先得意先CD].SelLength = Len([tx_販売先得意先CD].Text)) Then
			KeyAscii = 0
			'---参照画面表示
			SelectF = True
			'---参照画面表示
			If cTokuisaki.ShowDialog = True Then
				[tx_販売先得意先CD].Text = cTokuisaki.得意先CD
				ReturnF = True
				[tx_販売先得意先CD].NextSetFocus()
			Else
				[tx_販売先得意先CD].Focus()
			End If
		End If
		e.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			e.Handled = True
		End If
	End Sub

	Private Sub tx_販売先得意先CD_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_販売先得意先CD.KeyDown
		ReturnF = True
	End Sub

	Private Sub tx_販売先得意先CD_Leave(sender As Object, e As EventArgs) Handles tx_販売先得意先CD.Leave
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = ""
		If ReturnF = False Then
			'UPGRADE_WARNING: オブジェクト tx_販売先得意先CD.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[tx_販売先得意先CD].Undo()
		End If
		ReturnF = False
	End Sub

	Private Sub tx_販売先納得意先CD_Enter(sender As Object, e As EventArgs) Handles tx_販売先納得意先CD.Enter
		If SelectF = False Then
			If Item_Check(([tx_販売先納得意先CD].TabIndex)) = False Then
				Exit Sub
			End If
			PreviousControl = Me.ActiveControl
		End If
		SelectF = False

		'ボタン名設定
		'UPGRADE_ISSUE: Control Name は、汎用名前空間 ActiveControl 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
		Call SetUpFuncs(Me.ActiveControl.Name)
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = "販売先納入得意先CDを入力して下さい。　選択画面：Space"
	End Sub

	Private Sub tx_販売先納得意先CD_KeyPress(sender As Object, e As KeyPressEventArgs) Handles tx_販売先納得意先CD.KeyPress
		Dim KeyAscii As Integer = Asc(e.KeyChar)
		If KeyAscii = Asc(" ") And ([tx_販売先納得意先CD].SelStart = 0 And [tx_販売先納得意先CD].SelLength = Len([tx_販売先納得意先CD].Text)) Then
			KeyAscii = 0
			'---参照画面表示
			SelectF = True
			If cTokuisaki.ShowDialog = True Then
				[tx_販売先納得意先CD].Text = cTokuisaki.得意先CD
				ReturnF = True
				[tx_販売先納得意先CD].NextSetFocus()
			Else
				[tx_販売先納得意先CD].Focus()
			End If
		End If
		e.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			e.Handled = True
		End If
	End Sub

	Private Sub tx_販売先納得意先CD_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_販売先納得意先CD.KeyDown
		ReturnF = True
	End Sub

	Private Sub tx_販売先納得意先CD_Leave(sender As Object, e As EventArgs) Handles tx_販売先納得意先CD.Leave
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = ""
		If ReturnF = False Then
			'UPGRADE_WARNING: オブジェクト tx_販売先納得意先CD.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[tx_販売先納得意先CD].Undo()
		End If
		ReturnF = False
	End Sub

	Private Sub tx_販売先納入先CD_Enter(sender As Object, e As EventArgs) Handles tx_販売先納入先CD.Enter
		If SelectF = False Then
			If Item_Check(([tx_販売先納入先CD].TabIndex)) = False Then
				Exit Sub
			End If
			PreviousControl = Me.ActiveControl
		End If
		SelectF = False
		'ボタン名設定
		'UPGRADE_ISSUE: Control Name は、汎用名前空間 ActiveControl 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
		Call SetUpFuncs(Me.ActiveControl.Name)
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = "納入先CDを入力して下さい。　選択画面：Space"
	End Sub

	Private Sub tx_販売先納入先CD_KeyPress(sender As Object, e As KeyPressEventArgs) Handles tx_販売先納入先CD.KeyPress
		Dim KeyAscii As Integer = Asc(e.KeyChar)
		If KeyAscii = Asc(" ") And ([tx_販売先納入先CD].SelStart = 0 And [tx_販売先納入先CD].SelLength = Len([tx_販売先納入先CD].Text)) Then
			KeyAscii = 0
			'---参照画面表示
			SelectF = True
			cNonyusaki.得意先CD = [tx_販売先納得意先CD].Text
			If cNonyusaki.ShowDialog = True Then
				[tx_販売先納入先CD].Text = cNonyusaki.納入先CD
				ReturnF = True
				[tx_販売先納入先CD].NextSetFocus()
			Else
				[tx_販売先納入先CD].Focus()
			End If
		End If
		e.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			e.Handled = True
		End If
	End Sub

	Private Sub tx_販売先納入先CD_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_販売先納入先CD.KeyDown
		ReturnF = True
	End Sub

	Private Sub tx_販売先納入先CD_Leave(sender As Object, e As EventArgs) Handles tx_販売先納入先CD.Leave
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = ""
		If ReturnF = False Then
			'UPGRADE_WARNING: オブジェクト tx_販売先納入先CD.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[tx_販売先納入先CD].Undo()
		End If
		ReturnF = False
	End Sub
	'2013/03/07 ADD↑

	Private Sub tx_s納期Y_Enter(sender As Object, e As EventArgs) Handles tx_s納期Y.Enter
		'入力チェック
		If Item_Check(([tx_s納期Y].TabIndex)) = False Then
			Exit Sub
		End If
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = "開始納期を入力して下さい。"
		PreviousControl = Me.ActiveControl
		'ボタン名設定
		'UPGRADE_ISSUE: Control Name は、汎用名前空間 ActiveControl 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
		Call SetUpFuncs(Me.ActiveControl.Name)
	End Sub

	Private Sub tx_s納期Y_KeyPress(sender As Object, e As KeyPressEventArgs) Handles tx_s納期Y.KeyPress
		Dim KeyAscii As Integer = Asc(e.KeyChar)
		If KeyAscii <> 8 Then ' バックスペースは例外
			'UPGRADE_WARNING: TextBox プロパティ tx_s納期Y.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
			'UPGRADE_ISSUE: 定数 vbFromUnicode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
			'UPGRADE_ISSUE: LenB 関数はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="367764E5-F3F8-4E43-AC3E-7FE0B5E074E2"' をクリックしてください。
			If LenB([tx_s納期Y].Text & Chr(KeyAscii)) - LenB([tx_s納期Y].SelText) = [tx_s納期Y].MaxLength Then
				ReturnF = True
				[tx_s納期Y].Focus()
			End If
		End If
		e.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			e.Handled = True
		End If
	End Sub

	Private Sub tx_s納期Y_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_s納期Y.KeyDown
		ReturnF = True
	End Sub

	Private Sub tx_s納期Y_Leave(sender As Object, e As EventArgs) Handles tx_s納期Y.Leave
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = ""
		If ReturnF = False Then
			'UPGRADE_WARNING: オブジェクト tx_s納期Y.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[tx_s納期Y].Undo()
		End If
		ReturnF = False
	End Sub

	Private Sub tx_s納期M_Enter(sender As Object, e As EventArgs) Handles tx_s納期M.Enter
		'入力チェック
		If Item_Check(([tx_s納期M].TabIndex)) = False Then
			Exit Sub
		End If
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = "開始納期を入力して下さい。"
		PreviousControl = Me.ActiveControl
		'ボタン名設定
		'UPGRADE_ISSUE: Control Name は、汎用名前空間 ActiveControl 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
		Call SetUpFuncs(Me.ActiveControl.Name)
	End Sub

	Private Sub tx_s納期M_KeyPress(sender As Object, e As KeyPressEventArgs) Handles tx_s納期M.KeyPress
		Dim KeyAscii As Integer = Asc(e.KeyChar)
		If KeyAscii <> 8 Then ' バックスペースは例外
			'UPGRADE_WARNING: TextBox プロパティ tx_s納期M.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
			'UPGRADE_ISSUE: 定数 vbFromUnicode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
			'UPGRADE_ISSUE: LenB 関数はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="367764E5-F3F8-4E43-AC3E-7FE0B5E074E2"' をクリックしてください。
			If LenB([tx_s納期M].Text & Chr(KeyAscii)) - LenB([tx_s納期M].SelText) = [tx_s納期M].MaxLength Then
				If KeyAscii <> 0 Then '入力キーがエラーでないならば
					ReturnF = True
					[tx_s納期M].Focus()
				End If
			Else
				Select Case Chr(KeyAscii)
					Case CStr(2) To CStr(9)
						ReturnF = True
						[tx_s納期D].Focus()
				End Select
			End If
		End If
		e.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			e.Handled = True
		End If
	End Sub

	Private Sub tx_s納期M_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_s納期M.KeyDown
		ReturnF = True
	End Sub

	Private Sub tx_s納期M_Leave(sender As Object, e As EventArgs) Handles tx_s納期M.Leave
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = ""
		If ReturnF = False Then
			'UPGRADE_WARNING: オブジェクト tx_s納期M.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[tx_s納期M].Undo()
		End If
		ReturnF = False
	End Sub

	Private Sub tx_s納期D_Enter(sender As Object, e As EventArgs) Handles tx_s納期D.Enter
		'入力チェック
		If Item_Check(([tx_s納期D].TabIndex)) = False Then
			Exit Sub
		End If
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = "開始納期を入力して下さい。"
		PreviousControl = Me.ActiveControl
		'ボタン名設定
		'UPGRADE_ISSUE: Control Name は、汎用名前空間 ActiveControl 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
		Call SetUpFuncs(Me.ActiveControl.Name)
	End Sub

	Private Sub tx_s納期D_KeyPress(sender As Object, e As KeyPressEventArgs) Handles tx_s納期D.KeyPress
		Dim KeyAscii As Integer = Asc(e.KeyChar)
		'   If KeyAscii <> 8 Then ' バックスペースは例外
		'       If LenB([tx_s納期D].Text & Chr$(KeyAscii)) - _
		'            LenB([tx_s納期D].SelText) = [tx_s納期D].MaxLength Then
		'           If KeyAscii <> 0 Then       '入力キーがエラーでないならば
		'                   ReturnF = True
		'                   [tx_s納期D].NextSetFocus
		'           End If
		'       Else
		'           Select Case Chr$(KeyAscii)
		'               Case 4 To 9
		'                   ReturnF = True
		'                   [tx_s納期D].NextSetFocus
		'           End Select
		'       End If
		'   End If
		e.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			e.Handled = True
		End If
	End Sub

	Private Sub tx_s納期D_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_s納期D.KeyDown
		ReturnF = True
	End Sub

	Private Sub tx_s納期D_Leave(sender As Object, e As EventArgs) Handles tx_s納期D.Leave
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = ""
		If ReturnF = False Then
			'UPGRADE_WARNING: オブジェクト tx_s納期D.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[tx_s納期D].Undo()
		End If
		ReturnF = False
	End Sub

	Private Sub tx_e納期Y_Enter(sender As Object, e As EventArgs) Handles tx_e納期Y.Enter
		'入力チェック
		If Item_Check(([tx_e納期Y].TabIndex)) = False Then
			Exit Sub
		End If
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = "終了納期を入力して下さい。"
		PreviousControl = Me.ActiveControl
		'ボタン名設定
		'UPGRADE_ISSUE: Control Name は、汎用名前空間 ActiveControl 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
		Call SetUpFuncs(Me.ActiveControl.Name)
	End Sub

	Private Sub tx_e納期Y_KeyPress(sender As Object, e As KeyPressEventArgs) Handles tx_e納期Y.KeyPress
		Dim KeyAscii As Integer = Asc(e.KeyChar)
		If KeyAscii <> 8 Then ' バックスペースは例外
			'UPGRADE_WARNING: TextBox プロパティ tx_e納期Y.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
			'UPGRADE_ISSUE: 定数 vbFromUnicode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
			'UPGRADE_ISSUE: LenB 関数はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="367764E5-F3F8-4E43-AC3E-7FE0B5E074E2"' をクリックしてください。
			If LenB([tx_e納期Y].Text & Chr(KeyAscii)) - LenB([tx_e納期Y].SelText) = [tx_e納期Y].MaxLength Then
				ReturnF = True
				[tx_e納期Y].Focus()
			End If
		End If
		e.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			e.Handled = True
		End If
	End Sub

	Private Sub tx_e納期Y_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_e納期Y.KeyDown
		ReturnF = True
	End Sub

	Private Sub tx_e納期Y_Leave(sender As Object, e As EventArgs) Handles tx_e納期Y.Leave
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = ""
		If ReturnF = False Then
			'UPGRADE_WARNING: オブジェクト tx_e納期Y.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[tx_e納期Y].Undo()
		End If
		ReturnF = False
	End Sub

	Private Sub tx_e納期M_Enter(sender As Object, e As EventArgs) Handles tx_e納期M.Enter
		'入力チェック
		If Item_Check(([tx_e納期M].TabIndex)) = False Then
			Exit Sub
		End If
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = "終了納期を入力して下さい。"
		PreviousControl = Me.ActiveControl
		'ボタン名設定
		'UPGRADE_ISSUE: Control Name は、汎用名前空間 ActiveControl 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
		Call SetUpFuncs(Me.ActiveControl.Name)
	End Sub

	Private Sub tx_e納期M_KeyPress(sender As Object, e As KeyPressEventArgs) Handles tx_e納期M.KeyPress
		Dim KeyAscii As Integer = Asc(e.KeyChar)
		If KeyAscii <> 8 Then ' バックスペースは例外
			'UPGRADE_WARNING: TextBox プロパティ tx_e納期M.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
			'UPGRADE_ISSUE: 定数 vbFromUnicode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
			'UPGRADE_ISSUE: LenB 関数はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="367764E5-F3F8-4E43-AC3E-7FE0B5E074E2"' をクリックしてください。
			If LenB([tx_e納期M].Text & Chr(KeyAscii)) - LenB([tx_e納期M].SelText) = [tx_e納期M].MaxLength Then
				If KeyAscii <> 0 Then '入力キーがエラーでないならば
					ReturnF = True
					[tx_e納期M].Focus()
				End If
			Else
				Select Case Chr(KeyAscii)
					Case CStr(2) To CStr(9)
						ReturnF = True
						[tx_e納期D].Focus()
				End Select
			End If
		End If
		e.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			e.Handled = True
		End If
	End Sub

	Private Sub tx_e納期M_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_e納期M.KeyDown
		ReturnF = True
	End Sub

	Private Sub tx_e納期M_Leave(sender As Object, e As EventArgs) Handles tx_e納期M.Leave
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = ""
		If ReturnF = False Then
			'UPGRADE_WARNING: オブジェクト tx_e納期M.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[tx_e納期M].Undo()
		End If
		ReturnF = False
	End Sub

	Private Sub tx_e納期D_Enter(sender As Object, e As EventArgs) Handles tx_e納期D.Enter
		'入力チェック
		If Item_Check(([tx_e納期D].TabIndex)) = False Then
			Exit Sub
		End If
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = "終了納期を入力して下さい。"
		PreviousControl = Me.ActiveControl
		'ボタン名設定
		'UPGRADE_ISSUE: Control Name は、汎用名前空間 ActiveControl 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
		Call SetUpFuncs(Me.ActiveControl.Name)
	End Sub

	Private Sub tx_e納期D_KeyPress(sender As Object, e As KeyPressEventArgs) Handles tx_e納期D.KeyPress
		Dim KeyAscii As Integer = Asc(e.KeyChar)
		'   If KeyAscii <> 8 Then ' バックスペースは例外
		'       If LenB([tx_e納期D].Text & Chr$(KeyAscii)) - _
		'            LenB([tx_e納期D].SelText) = [tx_e納期D].MaxLength Then
		'           If KeyAscii <> 0 Then       '入力キーがエラーでないならば
		'                   ReturnF = True
		'                   [tx_e納期D].NextSetFocus
		'           End If
		'       Else
		'           Select Case Chr$(KeyAscii)
		'               Case 4 To 9
		'                   ReturnF = True
		'                   [tx_e納期D].NextSetFocus
		'           End Select
		'       End If
		'   End If
		e.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			e.Handled = True
		End If
	End Sub

	Private Sub tx_e納期D_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_e納期D.KeyDown
		ReturnF = True
	End Sub

	Private Sub tx_e納期D_Leave(sender As Object, e As EventArgs) Handles tx_e納期D.Leave
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = ""
		If ReturnF = False Then
			'UPGRADE_WARNING: オブジェクト tx_e納期D.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[tx_e納期D].Undo()
		End If
		ReturnF = False
	End Sub
	'
	'Private Sub tx_納期hh_GotFocus()
	'    '入力チェック
	'    If Item_Check([tx_納期hh].TabIndex) = False Then
	'        Exit Sub
	'    End If
	'    sb_Msg.Panels(1).Text = "納期時間を入力して下さい。"
	'    Set PreviousControl = Me.ActiveControl
	'    'ボタン名設定
	'    Call SetUpFuncs(Me.ActiveControl.Name)
	'End Sub
	'
	'Private Sub tx_納期hh_KeyPress(KeyAscii As Integer)
	'    Const Numbers As String = "0123456789" ' 入力許可文字
	'    Dim strText As String
	'
	'    If KeyAscii = vbKeyReturn Then KeyAscii = 0
	'
	'    '数字検査をする
	'    If KeyAscii <> 8 Then ' バックスペースは例外
	'        If InStr(Numbers, Chr(KeyAscii)) = 0 Then
	'            KeyAscii = 0 ' 入力を無効にする
	'            Exit Sub
	'        End If
	'        strText = InsStrToTextBox(tx_納期hh, Chr$(KeyAscii))
	'        If InStr(strText, "0") = 1 Then
	'           KeyAscii = 0
	'        End If
	'        If Len(strText) <= 2 Then
	'            If strText > 23 Then
	'                KeyAscii = 0
	'            End If
	'        Else
	'            KeyAscii = 0
	'        End If
	'    End If
	'End Sub
	'
	'Private Sub tx_納期hh_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_納期hh.KeyDown
	'    ReturnF = True
	'End Sub
	'
	'Private Sub tx_納期hh_LostFocus()
	'    sb_Msg.Panels(1).Text = ""
	'    If ReturnF = False Then
	'        [tx_納期hh].Undo
	'    End If
	'    tx_納期hh = Format$(tx_納期hh, "00")
	'    ReturnF = False
	'End Sub
	'
	'Private Sub tx_納期nn_GotFocus()
	'    '入力チェック
	'    If Item_Check([tx_納期nn].TabIndex) = False Then
	'        Exit Sub
	'    End If
	'    sb_Msg.Panels(1).Text = "納期時間を入力して下さい。"
	'    Set PreviousControl = Me.ActiveControl
	'    'ボタン名設定
	'    Call SetUpFuncs(Me.ActiveControl.Name)
	'End Sub
	'
	'Private Sub tx_納期nn_KeyPress(KeyAscii As Integer)
	'    Const Numbers As String = "0123456789" ' 入力許可文字
	'    Dim strText As String
	'
	'    If KeyAscii = vbKeyReturn Then KeyAscii = 0
	'
	'    '数字検査をする
	'    If KeyAscii <> 8 Then ' バックスペースは例外
	'        If InStr(Numbers, Chr(KeyAscii)) = 0 Then
	'            KeyAscii = 0 ' 入力を無効にする
	'            Exit Sub
	'        End If
	'        strText = InsStrToTextBox(tx_納期nn, Chr$(KeyAscii))
	'        If InStr(strText, "0") = 1 Then
	'            KeyAscii = 0
	'        End If
	'        If Len(strText) <= 2 Then
	'            If strText > 59 Then
	'                KeyAscii = 0
	'            End If
	'        Else
	'            KeyAscii = 0
	'        End If
	'    End If
	'End Sub
	'
	'Private Sub tx_納期nn_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_納期nn.KeyDown
	'    ReturnF = True
	'End Sub
	'
	'Private Sub tx_納期nn_LostFocus()
	'    sb_Msg.Panels(1).Text = ""
	'    If ReturnF = False Then
	'        [tx_納期nn].Undo
	'    End If
	'    tx_納期nn = Format$(tx_納期nn, "00")
	'    ReturnF = False
	'End Sub
	'
	'テキストボックスの現在の位置に文字列を埋め込む
	'Private Function InsStrToTextBox(ByRef TxText As Object, ByVal sInsStr As String) As String
	'    With TxText
	'        If sInsStr = "-" Then
	'            InsStrToTextBox = sInsStr & Left$(.Text, .SelStart) & Right$(.Text, Len(.Text) - .SelStart - .SelLength)
	'        Else
	'            InsStrToTextBox = Left$(.Text, .SelStart) & sInsStr & Right$(.Text, Len(.Text) - .SelStart - .SelLength)
	'        End If
	'    End With
	'End Function

	Private Sub tx_備考_Enter(sender As Object, e As EventArgs) Handles tx_備考.Enter
		If Item_Check(([tx_備考].TabIndex)) = False Then
			Exit Sub
		End If

		PreviousControl = Me.ActiveControl
		'ボタン名設定
		'UPGRADE_ISSUE: Control Name は、汎用名前空間 ActiveControl 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
		Call SetUpFuncs(Me.ActiveControl.Name)
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = "備考を入力して下さい。"
	End Sub

	Private Sub tx_備考_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_備考.KeyDown
		ReturnF = True
	End Sub

	Private Sub tx_備考_Leave(sender As Object, e As EventArgs) Handles tx_備考.Leave
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = ""
		If ReturnF = False Then
			'UPGRADE_WARNING: オブジェクト tx_備考.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[tx_備考].Undo()
		End If
		ReturnF = False
	End Sub

	'2023/04/18 ADD↓
	Private Sub tx_業種区分_Enter(sender As Object, e As EventArgs) Handles tx_業種区分.Enter
		'0:什器 1:内装
		If Item_Check(([tx_業種区分].TabIndex)) = False Then
			Exit Sub
		End If
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = "業種区分を入力して下さい。"
		PreviousControl = Me.ActiveControl

		'ボタン名設定
		'UPGRADE_ISSUE: Control Name は、汎用名前空間 ActiveControl 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
		Call SetUpFuncs(Me.ActiveControl.Name)
	End Sub

	Private Sub tx_業種区分_KeyPress(sender As Object, e As KeyPressEventArgs) Handles tx_業種区分.KeyPress
		Dim KeyAscii As Integer = Asc(e.KeyChar)
		'MaxLengthは１多く設定する。

		Const Numbers As String = "01" ' 入力許可文字       '2023/04/18 ADD
		'Dim strText As String

		If KeyAscii = System.Windows.Forms.Keys.Return Then KeyAscii = 0

		'数字検査をする
		If KeyAscii <> 8 Then ' バックスペースは例外
			If InStr(Numbers, Chr(KeyAscii)) = 0 Then
				KeyAscii = 0 ' 入力を無効にする
				GoTo EventExitSub
			Else
				'UPGRADE_WARNING: TextBox プロパティ tx_業種区分.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
				'UPGRADE_ISSUE: 定数 vbFromUnicode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
				'UPGRADE_ISSUE: LenB 関数はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="367764E5-F3F8-4E43-AC3E-7FE0B5E074E2"' をクリックしてください。
				If LenB([tx_業種区分].Text) = [tx_業種区分].MaxLength - 1 Then
					Call SelText([tx_業種区分])
				End If
				'UPGRADE_WARNING: TextBox プロパティ tx_業種区分.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
				'UPGRADE_ISSUE: 定数 vbFromUnicode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
				'UPGRADE_ISSUE: LenB 関数はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="367764E5-F3F8-4E43-AC3E-7FE0B5E074E2"' をクリックしてください。
				If LenB([tx_業種区分].Text & Chr(KeyAscii)) - LenB([tx_業種区分].SelText) = [tx_業種区分].MaxLength - 1 Then
					ReturnF = True
					[tx_業種区分].NextSetFocus()
				End If
			End If
		End If
EventExitSub:
		e.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			e.Handled = True
		End If
	End Sub

	Private Sub tx_業種区分_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_業種区分.KeyDown
		ReturnF = True
	End Sub

	Private Sub tx_業種区分_Leave(sender As Object, e As EventArgs) Handles tx_業種区分.Leave
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = ""
		If ReturnF = False Then
			'UPGRADE_WARNING: オブジェクト tx_業種区分.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[tx_業種区分].Undo()
		End If
		ReturnF = False
	End Sub
	'2023/04/18 ADD↑

	Private Sub tx_物件金額_Enter(sender As Object, e As EventArgs) Handles tx_物件金額.Enter
		If Item_Check(([tx_物件金額].TabIndex)) = False Then
			Exit Sub
		End If

		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = "物件金額を入力して下さい。"
		PreviousControl = Me.ActiveControl
	End Sub

	Private Sub tx_物件金額_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_物件金額.KeyDown
		ReturnF = True
	End Sub

	Private Sub tx_物件金額_Leave(sender As Object, e As EventArgs) Handles tx_物件金額.Leave
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = ""
		If ReturnF = False Then
			'UPGRADE_WARNING: オブジェクト tx_物件金額.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[tx_物件金額].Undo()
		End If
		ReturnF = False
	End Sub

	Private Sub tx_OPEN日Y_Enter(sender As Object, e As EventArgs) Handles tx_OPEN日Y.Enter
		'入力チェック
		If Item_Check(([tx_OPEN日Y].TabIndex)) = False Then
			Exit Sub
		End If
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = "オープン日を入力して下さい。"
		PreviousControl = Me.ActiveControl
		'ボタン名設定
		'UPGRADE_ISSUE: Control Name は、汎用名前空間 ActiveControl 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
		Call SetUpFuncs(Me.ActiveControl.Name)
	End Sub

	Private Sub tx_OPEN日Y_KeyPress(sender As Object, e As KeyPressEventArgs) Handles tx_OPEN日Y.KeyPress
		Dim KeyAscii As Integer = Asc(e.KeyChar)
		If KeyAscii <> 8 Then ' バックスペースは例外
			'UPGRADE_WARNING: TextBox プロパティ tx_OPEN日Y.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
			'UPGRADE_ISSUE: 定数 vbFromUnicode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
			'UPGRADE_ISSUE: LenB 関数はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="367764E5-F3F8-4E43-AC3E-7FE0B5E074E2"' をクリックしてください。
			If LenB([tx_OPEN日Y].Text & Chr(KeyAscii)) - LenB([tx_OPEN日Y].SelText) = [tx_OPEN日Y].MaxLength Then
				ReturnF = True
				[tx_OPEN日Y].Focus()
			End If
		End If
		e.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			e.Handled = True
		End If
	End Sub

	Private Sub tx_OPEN日Y_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_OPEN日Y.KeyDown
		ReturnF = True
	End Sub

	Private Sub tx_OPEN日Y_Leave(sender As Object, e As EventArgs) Handles tx_OPEN日Y.Leave
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = ""
		If ReturnF = False Then
			'UPGRADE_WARNING: オブジェクト tx_OPEN日Y.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[tx_OPEN日Y].Undo()
		End If
		ReturnF = False
	End Sub

	Private Sub tx_OPEN日M_Enter(sender As Object, e As EventArgs) Handles tx_OPEN日M.Enter
		'入力チェック
		If Item_Check(([tx_OPEN日M].TabIndex)) = False Then
			Exit Sub
		End If
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = "オープン日を入力して下さい。"
		PreviousControl = Me.ActiveControl
		'ボタン名設定
		'UPGRADE_ISSUE: Control Name は、汎用名前空間 ActiveControl 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
		Call SetUpFuncs(Me.ActiveControl.Name)
	End Sub

	Private Sub tx_OPEN日M_KeyPress(sender As Object, e As KeyPressEventArgs) Handles tx_OPEN日M.KeyPress
		Dim KeyAscii As Integer = Asc(e.KeyChar)
		If KeyAscii <> 8 Then ' バックスペースは例外
			'UPGRADE_WARNING: TextBox プロパティ tx_OPEN日M.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
			'UPGRADE_ISSUE: 定数 vbFromUnicode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
			'UPGRADE_ISSUE: LenB 関数はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="367764E5-F3F8-4E43-AC3E-7FE0B5E074E2"' をクリックしてください。
			If LenB([tx_OPEN日M].Text & Chr(KeyAscii)) - LenB([tx_OPEN日M].SelText) = [tx_OPEN日M].MaxLength Then
				If KeyAscii <> 0 Then '入力キーがエラーでないならば
					ReturnF = True
					[tx_OPEN日M].Focus()
				End If
			Else
				Select Case Chr(KeyAscii)
					Case CStr(2) To CStr(9)
						ReturnF = True
						[tx_OPEN日D].Focus()
				End Select
			End If
		End If
		e.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			e.Handled = True
		End If
	End Sub

	Private Sub tx_OPEN日M_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_OPEN日M.KeyDown
		ReturnF = True
	End Sub

	Private Sub tx_OPEN日M_Leave(sender As Object, e As EventArgs) Handles tx_OPEN日M.Leave
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = ""
		If ReturnF = False Then
			'UPGRADE_WARNING: オブジェクト tx_OPEN日M.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[tx_OPEN日M].Undo()
		End If
		ReturnF = False
	End Sub

	Private Sub tx_OPEN日D_Enter(sender As Object, e As EventArgs) Handles tx_OPEN日D.Enter
		'入力チェック
		If Item_Check(([tx_OPEN日D].TabIndex)) = False Then
			Exit Sub
		End If
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = "オープン日を入力して下さい。"
		PreviousControl = Me.ActiveControl
		'ボタン名設定
		'UPGRADE_ISSUE: Control Name は、汎用名前空間 ActiveControl 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
		Call SetUpFuncs(Me.ActiveControl.Name)
	End Sub

	Private Sub tx_OPEN日D_KeyPress(sender As Object, e As KeyPressEventArgs) Handles tx_OPEN日D.KeyPress
		Dim KeyAscii As Integer = Asc(e.KeyChar)
		'   If KeyAscii <> 8 Then ' バックスペースは例外
		'       If LenB([tx_OPEN日D].Text & Chr$(KeyAscii)) - _
		'            LenB([tx_OPEN日D].SelText) = [tx_OPEN日D].MaxLength Then
		'           If KeyAscii <> 0 Then       '入力キーがエラーでないならば
		'                   ReturnF = True
		'                   [tx_OPEN日D].NextSetFocus
		'           End If
		'       Else
		'           Select Case Chr$(KeyAscii)
		'               Case 4 To 9
		'                   ReturnF = True
		'                   [tx_OPEN日D].NextSetFocus
		'           End Select
		'       End If
		'   End If
		e.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			e.Handled = True
		End If
	End Sub

	Private Sub tx_OPEN日D_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_OPEN日D.KeyDown
		ReturnF = True
	End Sub

	Private Sub tx_OPEN日D_Leave(sender As Object, e As EventArgs) Handles tx_OPEN日D.Leave
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = ""
		If ReturnF = False Then
			'UPGRADE_WARNING: オブジェクト tx_OPEN日D.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[tx_OPEN日D].Undo()
		End If
		ReturnF = False
	End Sub

	Private Sub tx_物件種別_Enter(sender As Object, e As EventArgs) Handles tx_物件種別.Enter
		'0:新店 1:改装 2:メンテ 3:補充(しまむら用) 4:内装 5:その他
		'2022/08/31 6:委託　追加
		If Item_Check(([tx_物件種別].TabIndex)) = False Then
			Exit Sub
		End If
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = "物件種別を入力して下さい。"
		PreviousControl = Me.ActiveControl

		'ボタン名設定
		'UPGRADE_ISSUE: Control Name は、汎用名前空間 ActiveControl 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
		Call SetUpFuncs(Me.ActiveControl.Name)
	End Sub

	Private Sub tx_物件種別_KeyPress(sender As Object, e As KeyPressEventArgs) Handles tx_物件種別.KeyPress
		Dim KeyAscii As Integer = Asc(e.KeyChar)
		'MaxLengthは１多く設定する。

		'   Const Numbers As String = "01234" ' 入力許可文字
		'   Const Numbers As String = "012345" ' 入力許可文字       '2012/08/21 ADD
		'   Const Numbers As String = "01245" ' 入力許可文字       '2020/06/18 ADD
		'   Const Numbers As String = "012456" ' 入力許可文字       '2022/08/31 ADD
		'   Const Numbers As String = "01256" ' 入力許可文字       '2023/04/18 ADD
		Const Numbers As String = "0126" ' 入力許可文字       '2024/01/31 ADD
		'Dim strText As String

		If KeyAscii = System.Windows.Forms.Keys.Return Then KeyAscii = 0

		'数字検査をする
		If KeyAscii <> 8 Then ' バックスペースは例外
			If InStr(Numbers, Chr(KeyAscii)) = 0 Then
				KeyAscii = 0 ' 入力を無効にする
				e.Handled = True
				Exit Sub
			Else
				'UPGRADE_WARNING: TextBox プロパティ tx_物件種別.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
				'UPGRADE_ISSUE: 定数 vbFromUnicode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
				'UPGRADE_ISSUE: LenB 関数はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="367764E5-F3F8-4E43-AC3E-7FE0B5E074E2"' をクリックしてください。
				If LenB([tx_物件種別].Text) = [tx_物件種別].MaxLength - 1 Then
					Call SelText([tx_物件種別])
				End If
				'UPGRADE_WARNING: TextBox プロパティ tx_物件種別.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
				'UPGRADE_ISSUE: 定数 vbFromUnicode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
				'UPGRADE_ISSUE: LenB 関数はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="367764E5-F3F8-4E43-AC3E-7FE0B5E074E2"' をクリックしてください。
				If LenB([tx_物件種別].Text & Chr(KeyAscii)) - LenB([tx_物件種別].SelText) = [tx_物件種別].MaxLength - 1 Then
					ReturnF = True
					[tx_物件種別].NextSetFocus()
				End If
			End If
		End If
		e.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			e.Handled = True
		End If
	End Sub

	Private Sub tx_物件種別_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_物件種別.KeyDown
		If e.KeyCode = Keys.Return Then
			ReturnF = True
		End If
	End Sub

	Private Sub tx_物件種別_Leave(sender As Object, e As EventArgs) Handles tx_物件種別.Leave
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = ""
		If ReturnF = False Then
			'UPGRADE_WARNING: オブジェクト tx_物件種別.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[tx_物件種別].Undo()
		End If
		ReturnF = False
	End Sub

	'2015/10/16 ADD↓
	Private Sub tx_受付日付Y_Enter(sender As Object, e As EventArgs) Handles tx_受付日付Y.Enter
		'入力チェック
		If Item_Check(([tx_受付日付Y].TabIndex)) = False Then
			Exit Sub
		End If
		PreviousControl = Me.ActiveControl
		'ボタン名設定
		'UPGRADE_ISSUE: Control Name は、汎用名前空間 ActiveControl 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
		Call SetUpFuncs(Me.ActiveControl.Name)
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = "受付日付を入力して下さい。"
	End Sub

	Private Sub tx_受付日付Y_KeyPress(sender As Object, e As KeyPressEventArgs) Handles tx_受付日付Y.KeyPress
		Dim KeyAscii As Integer = Asc(e.KeyChar)
		If KeyAscii <> 8 Then ' バックスペースは例外
			'UPGRADE_WARNING: TextBox プロパティ tx_受付日付Y.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
			'UPGRADE_ISSUE: 定数 vbFromUnicode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
			'UPGRADE_ISSUE: LenB 関数はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="367764E5-F3F8-4E43-AC3E-7FE0B5E074E2"' をクリックしてください。
			If LenB([tx_受付日付Y].Text & Chr(KeyAscii)) - LenB([tx_受付日付Y].SelText) = [tx_受付日付Y].MaxLength Then
				ReturnF = True
				[tx_受付日付Y].Focus()
			End If
		End If
		e.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			e.Handled = True
		End If
	End Sub

	Private Sub tx_受付日付Y_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_受付日付Y.KeyDown
		ReturnF = True
	End Sub

	Private Sub tx_受付日付Y_Leave(sender As Object, e As EventArgs) Handles tx_受付日付Y.Leave
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = ""
		If ReturnF = False Then
			'UPGRADE_WARNING: オブジェクト tx_受付日付Y.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[tx_受付日付Y].Undo()
		End If
		ReturnF = False
	End Sub

	Private Sub tx_受付日付M_Enter(sender As Object, e As EventArgs) Handles tx_受付日付M.Enter
		'入力チェック
		If Item_Check(([tx_受付日付M].TabIndex)) = False Then
			Exit Sub
		End If
		PreviousControl = Me.ActiveControl
		'ボタン名設定
		'UPGRADE_ISSUE: Control Name は、汎用名前空間 ActiveControl 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
		Call SetUpFuncs(Me.ActiveControl.Name)
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = "受付日付を入力して下さい。"
	End Sub

	Private Sub tx_受付日付M_KeyPress(sender As Object, e As KeyPressEventArgs) Handles tx_受付日付M.KeyPress
		Dim KeyAscii As Integer = Asc(e.KeyChar)
		If KeyAscii <> 8 Then ' バックスペースは例外
			'UPGRADE_WARNING: TextBox プロパティ tx_受付日付M.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
			'UPGRADE_ISSUE: 定数 vbFromUnicode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
			'UPGRADE_ISSUE: LenB 関数はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="367764E5-F3F8-4E43-AC3E-7FE0B5E074E2"' をクリックしてください。
			If LenB([tx_受付日付M].Text & Chr(KeyAscii)) - LenB([tx_受付日付M].SelText) = [tx_受付日付M].MaxLength Then
				If KeyAscii <> 0 Then '入力キーがエラーでないならば
					ReturnF = True
					[tx_受付日付M].Focus()
				End If
			Else
				Select Case Chr(KeyAscii)
					Case CStr(2) To CStr(9)
						ReturnF = True
						[tx_受付日付D].Focus()
				End Select
			End If
		End If
		e.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			e.Handled = True
		End If
	End Sub

	Private Sub tx_受付日付M_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_受付日付M.KeyDown
		ReturnF = True
	End Sub

	Private Sub tx_受付日付M_Leave(sender As Object, e As EventArgs) Handles tx_受付日付M.Leave
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = ""
		If ReturnF = False Then
			'UPGRADE_WARNING: オブジェクト tx_受付日付M.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[tx_受付日付M].Undo()
		End If
		ReturnF = False
	End Sub

	Private Sub tx_受付日付D_Enter(sender As Object, e As EventArgs) Handles tx_受付日付D.Enter
		'入力チェック
		If Item_Check(([tx_受付日付D].TabIndex)) = False Then
			Exit Sub
		End If
		PreviousControl = Me.ActiveControl
		'ボタン名設定
		'UPGRADE_ISSUE: Control Name は、汎用名前空間 ActiveControl 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
		Call SetUpFuncs(Me.ActiveControl.Name)
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = "受付日付を入力して下さい。"
	End Sub

	Private Sub tx_受付日付D_KeyPress(sender As Object, e As KeyPressEventArgs) Handles tx_受付日付D.KeyPress
		Dim KeyAscii As Integer = Asc(e.KeyChar)
		'nop
		e.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			e.Handled = True
		End If
	End Sub

	Private Sub tx_受付日付D_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_受付日付D.KeyDown
		ReturnF = True
	End Sub

	Private Sub tx_受付日付D_Leave(sender As Object, e As EventArgs) Handles tx_受付日付D.Leave
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = ""
		If ReturnF = False Then
			'UPGRADE_WARNING: オブジェクト tx_受付日付D.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[tx_受付日付D].Undo()
		End If
		ReturnF = False
	End Sub

	Private Sub tx_完工日付Y_Enter(sender As Object, e As EventArgs) Handles tx_完工日付Y.Enter
		'入力チェック
		If Item_Check(([tx_完工日付Y].TabIndex)) = False Then
			Exit Sub
		End If
		PreviousControl = Me.ActiveControl
		'ボタン名設定
		'UPGRADE_ISSUE: Control Name は、汎用名前空間 ActiveControl 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
		Call SetUpFuncs(Me.ActiveControl.Name)
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = "完工日付を入力して下さい。"
	End Sub

	Private Sub tx_完工日付Y_KeyPress(sender As Object, e As KeyPressEventArgs) Handles tx_完工日付Y.KeyPress
		Dim KeyAscii As Integer = Asc(e.KeyChar)
		If KeyAscii <> 8 Then ' バックスペースは例外
			'UPGRADE_WARNING: TextBox プロパティ tx_完工日付Y.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
			'UPGRADE_ISSUE: 定数 vbFromUnicode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
			'UPGRADE_ISSUE: LenB 関数はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="367764E5-F3F8-4E43-AC3E-7FE0B5E074E2"' をクリックしてください。
			If LenB([tx_完工日付Y].Text & Chr(KeyAscii)) - LenB([tx_完工日付Y].SelText) = [tx_完工日付Y].MaxLength Then
				ReturnF = True
				[tx_完工日付Y].Focus()
			End If
		End If
		e.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			e.Handled = True
		End If
	End Sub

	Private Sub tx_完工日付Y_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_完工日付Y.KeyDown
		ReturnF = True
	End Sub

	Private Sub tx_完工日付Y_Leave(sender As Object, e As EventArgs) Handles tx_完工日付Y.Leave
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = ""
		If ReturnF = False Then
			'UPGRADE_WARNING: オブジェクト tx_完工日付Y.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[tx_完工日付Y].Undo()
		End If
		ReturnF = False
	End Sub

	Private Sub tx_完工日付M_Enter(sender As Object, e As EventArgs) Handles tx_完工日付M.Enter
		'入力チェック
		If Item_Check(([tx_完工日付M].TabIndex)) = False Then
			Exit Sub
		End If
		PreviousControl = Me.ActiveControl
		'ボタン名設定
		'UPGRADE_ISSUE: Control Name は、汎用名前空間 ActiveControl 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
		Call SetUpFuncs(Me.ActiveControl.Name)
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = "完工日付を入力して下さい。"
	End Sub

	Private Sub tx_完工日付M_KeyPress(sender As Object, e As KeyPressEventArgs) Handles tx_完工日付M.KeyPress
		Dim KeyAscii As Integer = Asc(e.KeyChar)
		If KeyAscii <> 8 Then ' バックスペースは例外
			'UPGRADE_WARNING: TextBox プロパティ tx_完工日付M.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
			'UPGRADE_ISSUE: 定数 vbFromUnicode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
			'UPGRADE_ISSUE: LenB 関数はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="367764E5-F3F8-4E43-AC3E-7FE0B5E074E2"' をクリックしてください。
			If LenB([tx_完工日付M].Text & Chr(KeyAscii)) - LenB([tx_完工日付M].SelText) = [tx_完工日付M].MaxLength Then
				If KeyAscii <> 0 Then '入力キーがエラーでないならば
					ReturnF = True
					[tx_完工日付M].Focus()
				End If
			Else
				Select Case Chr(KeyAscii)
					Case CStr(2) To CStr(9)
						ReturnF = True
						[tx_完工日付D].Focus()
				End Select
			End If
		End If
		e.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			e.Handled = True
		End If
	End Sub

	Private Sub tx_完工日付M_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_完工日付M.KeyDown
		ReturnF = True
	End Sub

	Private Sub tx_完工日付M_Leave(sender As Object, e As EventArgs) Handles tx_完工日付M.Leave
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = ""
		If ReturnF = False Then
			'UPGRADE_WARNING: オブジェクト tx_完工日付M.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[tx_完工日付M].Undo()
		End If
		ReturnF = False
	End Sub

	Private Sub tx_完工日付D_Enter(sender As Object, e As EventArgs) Handles tx_完工日付D.Enter
		'入力チェック
		If Item_Check(([tx_完工日付D].TabIndex)) = False Then
			Exit Sub
		End If
		PreviousControl = Me.ActiveControl
		'ボタン名設定
		'UPGRADE_ISSUE: Control Name は、汎用名前空間 ActiveControl 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
		Call SetUpFuncs(Me.ActiveControl.Name)
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = "完工日付を入力して下さい。"
	End Sub

	Private Sub tx_完工日付D_KeyPress(sender As Object, e As KeyPressEventArgs) Handles tx_完工日付D.KeyPress
		Dim KeyAscii As Integer = Asc(e.KeyChar)
		'nop
		e.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			e.Handled = True
		End If
	End Sub

	Private Sub tx_完工日付D_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_完工日付D.KeyDown
		ReturnF = True
	End Sub

	Private Sub tx_完工日付D_Leave(sender As Object, e As EventArgs) Handles tx_完工日付D.Leave
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = ""
		If ReturnF = False Then
			'UPGRADE_WARNING: オブジェクト tx_完工日付D.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[tx_完工日付D].Undo()
		End If
		ReturnF = False
	End Sub

	Private Sub tx_発注担当者名_Enter(sender As Object, e As EventArgs) Handles tx_発注担当者名.Enter
		If Item_Check(([tx_発注担当者名].TabIndex)) = False Then
			Exit Sub
		End If

		PreviousControl = Me.ActiveControl
		'ボタン名設定
		'UPGRADE_ISSUE: Control Name は、汎用名前空間 ActiveControl 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
		Call SetUpFuncs(Me.ActiveControl.Name)
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = "発注担当者名を入力して下さい。"
	End Sub

	Private Sub tx_発注担当者名_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_発注担当者名.KeyDown
		ReturnF = True
	End Sub

	Private Sub tx_発注担当者名_Leave(sender As Object, e As EventArgs) Handles tx_発注担当者名.Leave
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = ""
		If ReturnF = False Then
			'UPGRADE_WARNING: オブジェクト tx_発注担当者名.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[tx_発注担当者名].Undo()
		End If
		ReturnF = False
	End Sub

	Private Sub tx_作業内容_Enter(sender As Object, e As EventArgs) Handles tx_作業内容.Enter
		If Item_Check(([tx_作業内容].TabIndex)) = False Then
			Exit Sub
		End If

		PreviousControl = Me.ActiveControl
		'ボタン名設定
		'UPGRADE_ISSUE: Control Name は、汎用名前空間 ActiveControl 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
		Call SetUpFuncs(Me.ActiveControl.Name)
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = "作業内容を入力して下さい。"
	End Sub

	Private Sub tx_作業内容_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_作業内容.KeyDown
		ReturnF = True
	End Sub

	Private Sub tx_作業内容_Leave(sender As Object, e As EventArgs) Handles tx_作業内容.Leave
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = ""
		If ReturnF = False Then
			'UPGRADE_WARNING: オブジェクト tx_作業内容.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[tx_作業内容].Undo()
		End If
		ReturnF = False
	End Sub
	'2015/10/16 ADD↑

	'2016/06/09 ADD↓
	Private Sub tx_クレーム区分_Enter(sender As Object, e As EventArgs) Handles tx_クレーム区分.Enter
		If Item_Check(([tx_クレーム区分].TabIndex)) = False Then
			Exit Sub
		End If
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = "クレーム区分を入力して下さい。1:クレーム"
		PreviousControl = Me.ActiveControl

		'ボタン名設定
		'UPGRADE_ISSUE: Control Name は、汎用名前空間 ActiveControl 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
		Call SetUpFuncs(Me.ActiveControl.Name)
	End Sub

	Private Sub tx_クレーム区分_KeyPress(sender As Object, e As KeyPressEventArgs) Handles tx_クレーム区分.KeyPress
		Dim KeyAscii As Integer = Asc(e.KeyChar)
		'MaxLengthは１多く設定する。

		Const Numbers As String = "01" ' 入力許可文字
		'Dim strText As String

		If KeyAscii = System.Windows.Forms.Keys.Return Then KeyAscii = 0

		'数字検査をする
		If KeyAscii <> 8 Then ' バックスペースは例外
			If InStr(Numbers, Chr(KeyAscii)) = 0 Then
				KeyAscii = 0 ' 入力を無効にする
				GoTo EventExitSub
			Else
				'UPGRADE_WARNING: TextBox プロパティ tx_クレーム区分.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
				'UPGRADE_ISSUE: 定数 vbFromUnicode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
				'UPGRADE_ISSUE: LenB 関数はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="367764E5-F3F8-4E43-AC3E-7FE0B5E074E2"' をクリックしてください。
				If LenB([tx_クレーム区分].Text) = [tx_クレーム区分].MaxLength - 1 Then
					Call SelText([tx_クレーム区分])
				End If
				'UPGRADE_WARNING: TextBox プロパティ tx_クレーム区分.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
				'UPGRADE_ISSUE: 定数 vbFromUnicode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
				'UPGRADE_ISSUE: LenB 関数はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="367764E5-F3F8-4E43-AC3E-7FE0B5E074E2"' をクリックしてください。
				If LenB([tx_クレーム区分].Text & Chr(KeyAscii)) - LenB([tx_クレーム区分].SelText) = [tx_クレーム区分].MaxLength - 1 Then
					ReturnF = True
					[tx_クレーム区分].NextSetFocus()
				End If
			End If
		End If
EventExitSub:
		e.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			e.Handled = True
		End If
	End Sub

	Private Sub tx_クレーム区分_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_クレーム区分.KeyDown
		ReturnF = True
	End Sub

	Private Sub tx_クレーム区分_Leave(sender As Object, e As EventArgs) Handles tx_クレーム区分.Leave
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = ""
		If ReturnF = False Then
			'UPGRADE_WARNING: オブジェクト tx_クレーム区分.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[tx_クレーム区分].Undo()
		End If
		ReturnF = False
	End Sub
	'2016/06/09 ADD↑

	'2015/02/04 ADD↓
	Private Sub tx_伝票種類_Enter(sender As Object, e As EventArgs) Handles tx_伝票種類.Enter
		If Item_Check(([tx_伝票種類].TabIndex)) = False Then
			Exit Sub
		End If
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = "伝票種類を入力して下さい。"
		PreviousControl = Me.ActiveControl

		'ボタン名設定
		'UPGRADE_ISSUE: Control Name は、汎用名前空間 ActiveControl 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
		Call SetUpFuncs(Me.ActiveControl.Name)
	End Sub

	Private Sub tx_伝票種類_KeyPress(sender As Object, e As KeyPressEventArgs) Handles tx_伝票種類.KeyPress
		Dim KeyAscii As Integer = Asc(e.KeyChar)
		'MaxLengthは１多く設定する。

		Const Numbers As String = "01234" ' 入力許可文字
		'Dim strText As String

		If KeyAscii = System.Windows.Forms.Keys.Return Then KeyAscii = 0

		'数字検査をする
		If KeyAscii <> 8 Then ' バックスペースは例外
			If InStr(Numbers, Chr(KeyAscii)) = 0 Then
				KeyAscii = 0 ' 入力を無効にする
				GoTo EventExitSub
			Else
				'UPGRADE_WARNING: TextBox プロパティ tx_伝票種類.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
				'UPGRADE_ISSUE: 定数 vbFromUnicode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
				'UPGRADE_ISSUE: LenB 関数はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="367764E5-F3F8-4E43-AC3E-7FE0B5E074E2"' をクリックしてください。
				If LenB([tx_伝票種類].Text) = [tx_伝票種類].MaxLength - 1 Then
					Call SelText([tx_伝票種類])
				End If
				'UPGRADE_WARNING: TextBox プロパティ tx_伝票種類.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
				'UPGRADE_ISSUE: 定数 vbFromUnicode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
				'UPGRADE_ISSUE: LenB 関数はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="367764E5-F3F8-4E43-AC3E-7FE0B5E074E2"' をクリックしてください。
				If LenB([tx_伝票種類].Text & Chr(KeyAscii)) - LenB([tx_伝票種類].SelText) = [tx_伝票種類].MaxLength - 1 Then
					ReturnF = True
					[tx_伝票種類].NextSetFocus()
				End If
			End If
		End If
EventExitSub:
		e.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			e.Handled = True
		End If
	End Sub

	Private Sub tx_伝票種類_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_伝票種類.KeyDown
		ReturnF = True
	End Sub

	Private Sub tx_伝票種類_Leave(sender As Object, e As EventArgs) Handles tx_伝票種類.Leave
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = ""
		If ReturnF = False Then
			'UPGRADE_WARNING: オブジェクト tx_伝票種類.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[tx_伝票種類].Undo()
		End If
		ReturnF = False
	End Sub
	'2015/02/04 ADD↑

	'2015/11/19 ADD↓
	Private Sub tx_SM内容区分_Enter(sender As Object, e As EventArgs) Handles tx_SM内容区分.Enter
		If Item_Check(([tx_SM内容区分].TabIndex)) = False Then
			Exit Sub
		End If
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = "内容区分を入力して下さい。"
		PreviousControl = Me.ActiveControl

		'ボタン名設定
		'UPGRADE_ISSUE: Control Name は、汎用名前空間 ActiveControl 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
		Call SetUpFuncs(Me.ActiveControl.Name)
	End Sub

	Private Sub tx_SM内容区分_KeyPress(sender As Object, e As KeyPressEventArgs) Handles tx_SM内容区分.KeyPress
		Dim KeyAscii As Integer = Asc(e.KeyChar)
		'MaxLengthは１多く設定する。

		Const Numbers As String = "0123" ' 入力許可文字
		'Dim strText As String

		If KeyAscii = System.Windows.Forms.Keys.Return Then KeyAscii = 0

		'数字検査をする
		If KeyAscii <> 8 Then ' バックスペースは例外
			If InStr(Numbers, Chr(KeyAscii)) = 0 Then
				KeyAscii = 0 ' 入力を無効にする
				GoTo EventExitSub
			Else
				'UPGRADE_WARNING: TextBox プロパティ tx_SM内容区分.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
				'UPGRADE_ISSUE: 定数 vbFromUnicode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
				'UPGRADE_ISSUE: LenB 関数はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="367764E5-F3F8-4E43-AC3E-7FE0B5E074E2"' をクリックしてください。
				If LenB([tx_SM内容区分].Text) = [tx_SM内容区分].MaxLength - 1 Then
					Call SelText([tx_SM内容区分])
				End If
				'UPGRADE_WARNING: TextBox プロパティ tx_SM内容区分.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
				'UPGRADE_ISSUE: 定数 vbFromUnicode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
				'UPGRADE_ISSUE: LenB 関数はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="367764E5-F3F8-4E43-AC3E-7FE0B5E074E2"' をクリックしてください。
				If LenB([tx_SM内容区分].Text & Chr(KeyAscii)) - LenB([tx_SM内容区分].SelText) = [tx_SM内容区分].MaxLength - 1 Then
					ReturnF = True
					[tx_SM内容区分].NextSetFocus()
				End If
			End If
		End If
EventExitSub:
		e.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			e.Handled = True
		End If
	End Sub

	Private Sub tx_SM内容区分_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_SM内容区分.KeyDown
		ReturnF = True
	End Sub

	Private Sub tx_SM内容区分_Leave(sender As Object, e As EventArgs) Handles tx_SM内容区分.Leave
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = ""
		If ReturnF = False Then
			'UPGRADE_WARNING: オブジェクト tx_SM内容区分.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[tx_SM内容区分].Undo()
		End If
		ReturnF = False
	End Sub
	'2015/11/19 ADD↑

	'2009/09/24 ADD↓
	'Private Sub tx_見積区分_GotFocus()
	'    If Item_Check([tx_出力日].TabIndex) = False Then
	'        Exit Sub
	'    End If
	'    sb_Msg.Panels(1).Text = "見積区分を入力して下さい。"
	'    Set PreviousControl = Me.ActiveControl
	'
	'    'ボタン名設定
	'    Call SetUpFuncs(Me.ActiveControl.Name)
	'End Sub
	'
	'Private Sub tx_見積区分_KeyPress(KeyAscii As Integer)
	'    'MaxLengthは１多く設定する。
	'
	'    Const Numbers As String = "01" ' 入力許可文字
	'    Dim strText As String
	'
	'    If KeyAscii = vbKeyReturn Then KeyAscii = 0
	'
	'    '数字検査をする
	'    If KeyAscii <> 8 Then ' バックスペースは例外
	'        If InStr(Numbers, Chr(KeyAscii)) = 0 Then
	'            KeyAscii = 0 ' 入力を無効にする
	'            Exit Sub
	'        Else
	'            If LenB([tx_見積区分].Text) = [tx_見積区分].MaxLength - 1 Then
	'                Call SelText([tx_見積区分])
	'            End If
	'            If LenB([tx_見積区分].Text & Chr$(KeyAscii)) - _
	'                LenB([tx_見積区分].SelText) = [tx_見積区分].MaxLength - 1 Then
	'                ReturnF = True
	'                [tx_見積区分].NextSetFocus
	'            End If
	'        End If
	'    End If
	'End Sub
	'
	'Private Sub tx_見積区分_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_見積区分.KeyDown
	'    ReturnF = True
	'End Sub
	'
	'Private Sub tx_見積区分_LostFocus()
	'    sb_Msg.Panels(1).Text = ""
	'    If ReturnF = False Then
	'        tx_見積区分.Undo
	'    End If
	'    ReturnF = False
	'End Sub
	'2009/09/24 ADD↑

	'2015/06/12 ADD↓
	Private Sub tx_ウエルシアリース区分_Enter(sender As Object, e As EventArgs) Handles tx_ウエルシアリース区分.Enter
		If Item_Check(([tx_ウエルシアリース区分].TabIndex)) = False Then
			Exit Sub
		End If
		PreviousControl = Me.ActiveControl

		'ボタン名設定
		'UPGRADE_ISSUE: Control Name は、汎用名前空間 ActiveControl 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
		Call SetUpFuncs(Me.ActiveControl.Name)
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = "ウエルシアリース区分を入力して下さい。"
	End Sub

	Private Sub tx_ウエルシアリース区分_KeyPress(sender As Object, e As KeyPressEventArgs) Handles tx_ウエルシアリース区分.KeyPress
		Dim KeyAscii As Integer = Asc(e.KeyChar)
		'MaxLengthは１多く設定する。

		Const Numbers As String = "012" ' 入力許可文字
		'Dim strText As String

		If KeyAscii = System.Windows.Forms.Keys.Return Then KeyAscii = 0

		'数字検査をする
		If KeyAscii <> 8 Then ' バックスペースは例外
			If InStr(Numbers, Chr(KeyAscii)) = 0 Then
				KeyAscii = 0 ' 入力を無効にする
				GoTo EventExitSub
			Else
				'UPGRADE_WARNING: TextBox プロパティ tx_ウエルシアリース区分.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
				'UPGRADE_ISSUE: 定数 vbFromUnicode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
				'UPGRADE_ISSUE: LenB 関数はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="367764E5-F3F8-4E43-AC3E-7FE0B5E074E2"' をクリックしてください。
				If LenB([tx_ウエルシアリース区分].Text) = [tx_ウエルシアリース区分].MaxLength - 1 Then
					Call SelText([tx_ウエルシアリース区分])
				End If
				'UPGRADE_WARNING: TextBox プロパティ tx_ウエルシアリース区分.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
				'UPGRADE_ISSUE: 定数 vbFromUnicode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
				'UPGRADE_ISSUE: LenB 関数はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="367764E5-F3F8-4E43-AC3E-7FE0B5E074E2"' をクリックしてください。
				If LenB([tx_ウエルシアリース区分].Text & Chr(KeyAscii)) - LenB([tx_ウエルシアリース区分].SelText) = [tx_ウエルシアリース区分].MaxLength - 1 Then
					ReturnF = True
					[tx_ウエルシアリース区分].NextSetFocus()
				End If
			End If
		End If
EventExitSub:
		e.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			e.Handled = True
		End If
	End Sub

	Private Sub tx_ウエルシアリース区分_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_ウエルシアリース区分.KeyDown
		ReturnF = True
	End Sub

	Private Sub tx_ウエルシアリース区分_Leave(sender As Object, e As EventArgs) Handles tx_ウエルシアリース区分.Leave
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = ""
		If ReturnF = False Then
			'UPGRADE_WARNING: オブジェクト tx_ウエルシアリース区分.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[tx_ウエルシアリース区分].Undo()
		End If
		ReturnF = False
	End Sub

	Private Sub tx_ウエルシア物件区分_Enter(sender As Object, e As EventArgs) Handles tx_ウエルシア物件区分.Enter
		If SelectF = False Then
			If Item_Check(([tx_ウエルシア物件区分].TabIndex)) = False Then
				Exit Sub
			End If
		End If
		SelectF = False
		PreviousControl = Me.ActiveControl

		'ボタン名設定
		'UPGRADE_ISSUE: Control Name は、汎用名前空間 ActiveControl 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
		Call SetUpFuncs(Me.ActiveControl.Name)
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = "ウエルシア物件区分を入力して下さい。　選択画面：Space"
	End Sub

	Private Sub tx_ウエルシア物件区分_KeyPress(sender As Object, e As KeyPressEventArgs) Handles tx_ウエルシア物件区分.KeyPress
		Dim KeyAscii As Integer = Asc(e.KeyChar)
		If KeyAscii = Asc(" ") And ([tx_ウエルシア物件区分].SelStart = 0 And [tx_ウエルシア物件区分].SelLength = Len([tx_ウエルシア物件区分].Text)) Then
			KeyAscii = 0
			'---参照画面表示
			SelectF = True

			If cWelBukkenKubun.ShowDialog = True Then
				[tx_ウエルシア物件区分].Text = cWelBukkenKubun.ウエルシア物件区分CD
				ReturnF = True
				[tx_ウエルシア物件区分].NextSetFocus()
			Else
				[tx_ウエルシア物件区分].Focus()
			End If
		End If
	End Sub

	Private Sub tx_ウエルシア物件区分_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_ウエルシア物件区分.KeyDown
		ReturnF = True
	End Sub

	Private Sub tx_ウエルシア物件区分_Leave(sender As Object, e As EventArgs) Handles tx_ウエルシア物件区分.Leave
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = ""
		If ReturnF = False Then
			'UPGRADE_WARNING: オブジェクト tx_ウエルシア物件区分.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[tx_ウエルシア物件区分].Undo()
		End If
		ReturnF = False
	End Sub

	Private Sub tx_ウエルシア物件内容CD_Enter(sender As Object, e As EventArgs) Handles tx_ウエルシア物件内容CD.Enter
		'If SelectF = False Then
		If Item_Check(([tx_ウエルシア物件内容CD].TabIndex)) = False Then
			Exit Sub
		End If
		'End If
		SelectF = False
		PreviousControl = Me.ActiveControl

		'ボタン名設定
		'UPGRADE_ISSUE: Control Name は、汎用名前空間 ActiveControl 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
		Call SetUpFuncs(Me.ActiveControl.Name)
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = "ウエルシア物件内容CDを入力して下さい。　選択画面：Space"
	End Sub

	Private Sub tx_ウエルシア物件内容CD_KeyPress(sender As Object, e As KeyPressEventArgs) Handles tx_ウエルシア物件内容CD.KeyPress
		Dim KeyAscii As Integer = Asc(e.KeyChar)
		If KeyAscii = Asc(" ") And ([tx_ウエルシア物件内容CD].SelStart = 0 And [tx_ウエルシア物件内容CD].SelLength = Len([tx_ウエルシア物件内容CD].Text)) Then
			KeyAscii = 0
			'---参照画面表示
			SelectF = True
			'---参照画面表示

			If cWelBukkenNaiyo.ShowDialog = True Then
				[tx_ウエルシア物件内容CD].Text = cWelBukkenNaiyo.ウエルシア物件内容CD
				ReturnF = True
				[tx_ウエルシア物件内容CD].NextSetFocus()
			Else
				[tx_ウエルシア物件内容CD].Focus()
			End If
		End If
	End Sub

	Private Sub tx_ウエルシア物件内容CD_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_ウエルシア物件内容CD.KeyDown
		ReturnF = True
	End Sub

	Private Sub tx_ウエルシア物件内容CD_Leave(sender As Object, e As EventArgs) Handles tx_ウエルシア物件内容CD.Leave
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = ""
		If ReturnF = False Then
			'UPGRADE_WARNING: オブジェクト tx_ウエルシア物件内容CD.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[tx_ウエルシア物件内容CD].Undo()
		End If
		ReturnF = False
	End Sub

	Private Sub tx_ウエルシア物件内容名_Enter(sender As Object, e As EventArgs) Handles tx_ウエルシア物件内容名.Enter
		If Item_Check(([tx_ウエルシア物件内容名].TabIndex)) = False Then
			Exit Sub
		End If

		PreviousControl = Me.ActiveControl
		'ボタン名設定
		'UPGRADE_ISSUE: Control Name は、汎用名前空間 ActiveControl 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
		Call SetUpFuncs(Me.ActiveControl.Name)
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = "ウエルシア物件内容名を入力して下さい。"
	End Sub

	Private Sub tx_ウエルシア物件内容名_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_ウエルシア物件内容名.KeyDown
		ReturnF = True
	End Sub

	Private Sub tx_ウエルシア物件内容名_Leave(sender As Object, e As EventArgs) Handles tx_ウエルシア物件内容名.Leave
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = ""
		If ReturnF = False Then
			'UPGRADE_WARNING: オブジェクト tx_ウエルシア物件内容名.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[tx_ウエルシア物件内容名].Undo()
		End If
		ReturnF = False
	End Sub

	Private Sub tx_ウエルシア売場面積_Enter(sender As Object, e As EventArgs) Handles tx_ウエルシア売場面積.Enter
		If Item_Check(([tx_ウエルシア売場面積].TabIndex)) = False Then
			Exit Sub
		End If

		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = "ウエルシア売場面積を入力して下さい。"
		PreviousControl = Me.ActiveControl
	End Sub

	Private Sub tx_ウエルシア売場面積_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_ウエルシア売場面積.KeyDown
		ReturnF = True
	End Sub

	Private Sub tx_ウエルシア売場面積_Leave(sender As Object, e As EventArgs) Handles tx_ウエルシア売場面積.Leave
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = ""
		If ReturnF = False Then
			'UPGRADE_WARNING: オブジェクト tx_ウエルシア売場面積.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[tx_ウエルシア売場面積].Undo()
		End If
		ReturnF = False
	End Sub
	'2015/06/12 ADD↑

	'2015/11/03 ADD↓
	Private Sub tx_化粧品メーカー区分_Enter(sender As Object, e As EventArgs) Handles tx_化粧品メーカー区分.Enter
		If Item_Check(([tx_化粧品メーカー区分].TabIndex)) = False Then
			Exit Sub
		End If
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = "化粧品メーカー区分を入力して下さい。（1:在庫計算対象外とする）"
		PreviousControl = Me.ActiveControl

		'ボタン名設定
		'UPGRADE_ISSUE: Control Name は、汎用名前空間 ActiveControl 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
		Call SetUpFuncs(Me.ActiveControl.Name)
	End Sub

	Private Sub tx_化粧品メーカー区分_KeyPress(sender As Object, e As KeyPressEventArgs) Handles tx_化粧品メーカー区分.KeyPress
		Dim KeyAscii As Integer = Asc(e.KeyChar)
		'MaxLengthは１多く設定する。

		Const Numbers As String = "01" ' 入力許可文字
		'Dim strText As String

		If KeyAscii = System.Windows.Forms.Keys.Return Then KeyAscii = 0

		'数字検査をする
		If KeyAscii <> 8 Then ' バックスペースは例外
			If InStr(Numbers, Chr(KeyAscii)) = 0 Then
				KeyAscii = 0 ' 入力を無効にする
				GoTo EventExitSub
			Else
				'UPGRADE_WARNING: TextBox プロパティ tx_化粧品メーカー区分.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
				'UPGRADE_ISSUE: 定数 vbFromUnicode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
				'UPGRADE_ISSUE: LenB 関数はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="367764E5-F3F8-4E43-AC3E-7FE0B5E074E2"' をクリックしてください。
				If LenB([tx_化粧品メーカー区分].Text) = [tx_化粧品メーカー区分].MaxLength - 1 Then
					Call SelText([tx_化粧品メーカー区分])
				End If
				'UPGRADE_WARNING: TextBox プロパティ tx_化粧品メーカー区分.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
				'UPGRADE_ISSUE: 定数 vbFromUnicode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
				'UPGRADE_ISSUE: LenB 関数はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="367764E5-F3F8-4E43-AC3E-7FE0B5E074E2"' をクリックしてください。
				If LenB([tx_化粧品メーカー区分].Text & Chr(KeyAscii)) - LenB([tx_化粧品メーカー区分].SelText) = [tx_化粧品メーカー区分].MaxLength - 1 Then
					ReturnF = True
					[tx_化粧品メーカー区分].NextSetFocus()
				End If
			End If
		End If
EventExitSub:
		e.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			e.Handled = True
		End If
	End Sub

	Private Sub tx_化粧品メーカー区分_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_化粧品メーカー区分.KeyDown
		ReturnF = True
	End Sub

	Private Sub tx_化粧品メーカー区分_Leave(sender As Object, e As EventArgs) Handles tx_化粧品メーカー区分.Leave
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = ""
		If ReturnF = False Then
			'UPGRADE_WARNING: オブジェクト tx_化粧品メーカー区分.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[tx_化粧品メーカー区分].Undo()
		End If
		ReturnF = False
	End Sub
	'2015/11/03 ADD↑

	'2015/10/16 ADD↓
	Private Sub tx_YKサプライ区分_Enter(sender As Object, e As EventArgs) Handles tx_YKサプライ区分.Enter
		If Item_Check(([tx_YKサプライ区分].TabIndex)) = False Then
			Exit Sub
		End If
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = "YKサプライ区分を入力して下さい。"
		PreviousControl = Me.ActiveControl

		'ボタン名設定
		'UPGRADE_ISSUE: Control Name は、汎用名前空間 ActiveControl 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
		Call SetUpFuncs(Me.ActiveControl.Name)
	End Sub

	Private Sub tx_YKサプライ区分_KeyPress(sender As Object, e As KeyPressEventArgs) Handles tx_YKサプライ区分.KeyPress
		Dim KeyAscii As Integer = Asc(e.KeyChar)
		'MaxLengthは１多く設定する。

		Const Numbers As String = " 12" ' 入力許可文字
		'Dim strText As String

		If KeyAscii = System.Windows.Forms.Keys.Return Then KeyAscii = 0

		'数字検査をする
		If KeyAscii <> 8 Then ' バックスペースは例外
			If InStr(Numbers, Chr(KeyAscii)) = 0 Then
				KeyAscii = 0 ' 入力を無効にする
				GoTo EventExitSub
			Else
				'UPGRADE_WARNING: TextBox プロパティ tx_YKサプライ区分.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
				'UPGRADE_ISSUE: 定数 vbFromUnicode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
				'UPGRADE_ISSUE: LenB 関数はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="367764E5-F3F8-4E43-AC3E-7FE0B5E074E2"' をクリックしてください。
				If LenB([tx_YKサプライ区分].Text) = [tx_YKサプライ区分].MaxLength - 1 Then
					Call SelText([tx_YKサプライ区分])
				End If
				'UPGRADE_WARNING: TextBox プロパティ tx_YKサプライ区分.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
				'UPGRADE_ISSUE: 定数 vbFromUnicode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
				'UPGRADE_ISSUE: LenB 関数はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="367764E5-F3F8-4E43-AC3E-7FE0B5E074E2"' をクリックしてください。
				If LenB([tx_YKサプライ区分].Text & Chr(KeyAscii)) - LenB([tx_YKサプライ区分].SelText) = [tx_YKサプライ区分].MaxLength - 1 Then
					ReturnF = True
					[tx_YKサプライ区分].NextSetFocus()
				End If
			End If
		End If
EventExitSub:
		e.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			e.Handled = True
		End If
	End Sub

	Private Sub tx_YKサプライ区分_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_YKサプライ区分.KeyDown
		ReturnF = True
	End Sub

	Private Sub tx_YKサプライ区分_Leave(sender As Object, e As EventArgs) Handles tx_YKサプライ区分.Leave
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = ""
		If ReturnF = False Then
			'UPGRADE_WARNING: オブジェクト tx_YKサプライ区分.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[tx_YKサプライ区分].Undo()
		End If
		ReturnF = False
	End Sub

	Private Sub tx_YK物件区分_Enter(sender As Object, e As EventArgs) Handles tx_YK物件区分.Enter
		If Item_Check(([tx_YK物件区分].TabIndex)) = False Then
			Exit Sub
		End If
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = "YK物件区分を入力して下さい。"
		PreviousControl = Me.ActiveControl

		'ボタン名設定
		'UPGRADE_ISSUE: Control Name は、汎用名前空間 ActiveControl 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
		Call SetUpFuncs(Me.ActiveControl.Name)
	End Sub

	Private Sub tx_YK物件区分_KeyPress(sender As Object, e As KeyPressEventArgs) Handles tx_YK物件区分.KeyPress
		Dim KeyAscii As Integer = Asc(e.KeyChar)
		'MaxLengthは１多く設定する。

		'    Const Numbers As String = " 12" ' 入力許可文字
		Const Numbers As String = " 123" ' 入力許可文字 '2021/10/08 ADD
		'Dim strText As String

		If KeyAscii = System.Windows.Forms.Keys.Return Then KeyAscii = 0

		'数字検査をする
		If KeyAscii <> 8 Then ' バックスペースは例外
			If InStr(Numbers, Chr(KeyAscii)) = 0 Then
				KeyAscii = 0 ' 入力を無効にする
				GoTo EventExitSub
			Else
				'UPGRADE_WARNING: TextBox プロパティ tx_YK物件区分.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
				'UPGRADE_ISSUE: 定数 vbFromUnicode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
				'UPGRADE_ISSUE: LenB 関数はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="367764E5-F3F8-4E43-AC3E-7FE0B5E074E2"' をクリックしてください。
				If LenB([tx_YK物件区分].Text) = [tx_YK物件区分].MaxLength - 1 Then
					Call SelText([tx_YK物件区分])
				End If
				'UPGRADE_WARNING: TextBox プロパティ tx_YK物件区分.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
				'UPGRADE_ISSUE: 定数 vbFromUnicode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
				'UPGRADE_ISSUE: LenB 関数はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="367764E5-F3F8-4E43-AC3E-7FE0B5E074E2"' をクリックしてください。
				If LenB([tx_YK物件区分].Text & Chr(KeyAscii)) - LenB([tx_YK物件区分].SelText) = [tx_YK物件区分].MaxLength - 1 Then
					ReturnF = True
					[tx_YK物件区分].NextSetFocus()
				End If
			End If
		End If
EventExitSub:
		e.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			e.Handled = True
		End If
	End Sub

	Private Sub tx_YK物件区分_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_YK物件区分.KeyDown
		ReturnF = True
	End Sub

	Private Sub tx_YK物件区分_Leave(sender As Object, e As EventArgs) Handles tx_YK物件区分.Leave
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = ""
		If ReturnF = False Then
			'UPGRADE_WARNING: オブジェクト tx_YK物件区分.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[tx_YK物件区分].Undo()
		End If
		ReturnF = False
	End Sub

	Private Sub tx_YK請求区分_Enter(sender As Object, e As EventArgs) Handles tx_YK請求区分.Enter
		If Item_Check(([tx_YK請求区分].TabIndex)) = False Then
			Exit Sub
		End If
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = "YK請求区分を入力して下さい。"
		PreviousControl = Me.ActiveControl

		'ボタン名設定
		'UPGRADE_ISSUE: Control Name は、汎用名前空間 ActiveControl 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
		Call SetUpFuncs(Me.ActiveControl.Name)
	End Sub

	Private Sub tx_YK請求区分_KeyPress(sender As Object, e As KeyPressEventArgs) Handles tx_YK請求区分.KeyPress
		Dim KeyAscii As Integer = Asc(e.KeyChar)
		'MaxLengthは１多く設定する。

		Const Numbers As String = " 123" ' 入力許可文字
		'Dim strText As String

		If KeyAscii = System.Windows.Forms.Keys.Return Then KeyAscii = 0

		'数字検査をする
		If KeyAscii <> 8 Then ' バックスペースは例外
			If InStr(Numbers, Chr(KeyAscii)) = 0 Then
				KeyAscii = 0 ' 入力を無効にする
				GoTo EventExitSub
			Else
				'UPGRADE_WARNING: TextBox プロパティ tx_YK請求区分.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
				'UPGRADE_ISSUE: 定数 vbFromUnicode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
				'UPGRADE_ISSUE: LenB 関数はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="367764E5-F3F8-4E43-AC3E-7FE0B5E074E2"' をクリックしてください。
				If LenB([tx_YK請求区分].Text) = [tx_YK請求区分].MaxLength - 1 Then
					Call SelText([tx_YK請求区分])
				End If
				'UPGRADE_WARNING: TextBox プロパティ tx_YK請求区分.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
				'UPGRADE_ISSUE: 定数 vbFromUnicode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
				'UPGRADE_ISSUE: LenB 関数はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="367764E5-F3F8-4E43-AC3E-7FE0B5E074E2"' をクリックしてください。
				If LenB([tx_YK請求区分].Text & Chr(KeyAscii)) - LenB([tx_YK請求区分].SelText) = [tx_YK請求区分].MaxLength - 1 Then
					ReturnF = True
					[tx_YK請求区分].NextSetFocus()
				End If
			End If
		End If
EventExitSub:
		e.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			e.Handled = True
		End If
	End Sub

	Private Sub tx_YK請求区分_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_YK請求区分.KeyDown
		ReturnF = True
	End Sub

	Private Sub tx_YK請求区分_Leave(sender As Object, e As EventArgs) Handles tx_YK請求区分.Leave
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = ""
		If ReturnF = False Then
			'UPGRADE_WARNING: オブジェクト tx_YK請求区分.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[tx_YK請求区分].Undo()
		End If
		ReturnF = False
	End Sub
	'2015/10/16 ADD↑

	'2022/10/10 ADD↓
	Private Sub tx_B請求管轄区分_Enter(sender As Object, e As EventArgs) Handles tx_B請求管轄区分.Enter
		If Item_Check(([tx_B請求管轄区分].TabIndex)) = False Then
			Exit Sub
		End If
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = "B請求管轄区分を入力して下さい。"
		PreviousControl = Me.ActiveControl

		'ボタン名設定
		'UPGRADE_ISSUE: Control Name は、汎用名前空間 ActiveControl 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
		Call SetUpFuncs(Me.ActiveControl.Name)
	End Sub

	Private Sub tx_B請求管轄区分_KeyPress(sender As Object, e As KeyPressEventArgs) Handles tx_B請求管轄区分.KeyPress
		Dim KeyAscii As Integer = Asc(e.KeyChar)
		'MaxLengthは１多く設定する。

		Const Numbers As String = " 12" ' 入力許可文字
		'Dim strText As String

		If KeyAscii = System.Windows.Forms.Keys.Return Then KeyAscii = 0

		'数字検査をする
		If KeyAscii <> 8 Then ' バックスペースは例外
			If InStr(Numbers, Chr(KeyAscii)) = 0 Then
				KeyAscii = 0 ' 入力を無効にする
				GoTo EventExitSub
			Else
				'UPGRADE_WARNING: TextBox プロパティ tx_B請求管轄区分.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
				'UPGRADE_ISSUE: 定数 vbFromUnicode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
				'UPGRADE_ISSUE: LenB 関数はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="367764E5-F3F8-4E43-AC3E-7FE0B5E074E2"' をクリックしてください。
				If LenB([tx_B請求管轄区分].Text) = [tx_B請求管轄区分].MaxLength - 1 Then
					Call SelText([tx_B請求管轄区分])
				End If
				'UPGRADE_WARNING: TextBox プロパティ tx_B請求管轄区分.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
				'UPGRADE_ISSUE: 定数 vbFromUnicode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
				'UPGRADE_ISSUE: LenB 関数はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="367764E5-F3F8-4E43-AC3E-7FE0B5E074E2"' をクリックしてください。
				If LenB([tx_B請求管轄区分].Text & Chr(KeyAscii)) - LenB([tx_B請求管轄区分].SelText) = [tx_B請求管轄区分].MaxLength - 1 Then
					ReturnF = True
					[tx_B請求管轄区分].NextSetFocus()
				End If
			End If
		End If
EventExitSub:
		e.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			e.Handled = True
		End If
	End Sub

	Private Sub tx_B請求管轄区分_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_B請求管轄区分.KeyDown
		ReturnF = True
	End Sub

	Private Sub tx_B請求管轄区分_Leave(sender As Object, e As EventArgs) Handles tx_B請求管轄区分.Leave
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = ""
		If ReturnF = False Then
			'UPGRADE_WARNING: オブジェクト tx_B請求管轄区分.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[tx_B請求管轄区分].Undo()
		End If
		ReturnF = False
	End Sub

	Private Sub tx_BtoB番号_Enter(sender As Object, e As EventArgs) Handles tx_BtoB番号.Enter
		If Item_Check(([tx_BtoB番号].TabIndex)) = False Then
			Exit Sub
		End If

		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = "BtoB番号を入力して下さい。"
		PreviousControl = Me.ActiveControl
	End Sub

	Private Sub tx_BtoB番号_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_BtoB番号.KeyDown
		ReturnF = True
	End Sub

	Private Sub tx_BtoB番号_Leave(sender As Object, e As EventArgs) Handles tx_BtoB番号.Leave
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = ""
		If ReturnF = False Then
			'UPGRADE_WARNING: オブジェクト tx_BtoB番号.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[tx_BtoB番号].Undo()
		End If
		ReturnF = False
	End Sub
	'2022/10/10 ADD↑

	Private Sub tx_現場名_Enter(sender As Object, e As EventArgs) Handles tx_現場名.Enter
		If Item_Check(([tx_現場名].TabIndex)) = False Then
			Exit Sub
		End If

		PreviousControl = Me.ActiveControl
		'ボタン名設定
		'UPGRADE_ISSUE: Control Name は、汎用名前空間 ActiveControl 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
		Call SetUpFuncs(Me.ActiveControl.Name)
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = "現場名を入力して下さい。"
	End Sub

	Private Sub tx_現場名_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_現場名.KeyDown
		ReturnF = True
	End Sub

	Private Sub tx_現場名_Leave(sender As Object, e As EventArgs) Handles tx_現場名.Leave
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = ""
		If ReturnF = False Then
			'UPGRADE_WARNING: オブジェクト tx_現場名.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[tx_現場名].Undo()
		End If
		ReturnF = False
	End Sub

	Private Sub tx_支払条件_Enter(sender As Object, e As EventArgs) Handles tx_支払条件.Enter
		If Item_Check(([tx_支払条件].TabIndex)) = False Then
			Exit Sub
		End If
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = "御支払条件を入力して下さい。"
		PreviousControl = Me.ActiveControl

		'ボタン名設定
		'UPGRADE_ISSUE: Control Name は、汎用名前空間 ActiveControl 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
		Call SetUpFuncs(Me.ActiveControl.Name)
	End Sub

	Private Sub tx_支払条件_KeyPress(sender As Object, e As KeyPressEventArgs) Handles tx_支払条件.KeyPress
		Dim KeyAscii As Integer = Asc(e.KeyChar)
		'MaxLengthは１多く設定する。

		Const Numbers As String = "01" ' 入力許可文字
		'Dim strText As String

		If KeyAscii = System.Windows.Forms.Keys.Return Then KeyAscii = 0

		'数字検査をする
		If KeyAscii <> 8 Then ' バックスペースは例外
			If InStr(Numbers, Chr(KeyAscii)) = 0 Then
				KeyAscii = 0 ' 入力を無効にする
				GoTo EventExitSub
			Else
				'UPGRADE_WARNING: TextBox プロパティ tx_支払条件.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
				'UPGRADE_ISSUE: 定数 vbFromUnicode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
				'UPGRADE_ISSUE: LenB 関数はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="367764E5-F3F8-4E43-AC3E-7FE0B5E074E2"' をクリックしてください。
				If LenB([tx_支払条件].Text) = [tx_支払条件].MaxLength - 1 Then
					Call SelText([tx_支払条件])
				End If
				'UPGRADE_WARNING: TextBox プロパティ tx_支払条件.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
				'UPGRADE_ISSUE: 定数 vbFromUnicode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
				'UPGRADE_ISSUE: LenB 関数はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="367764E5-F3F8-4E43-AC3E-7FE0B5E074E2"' をクリックしてください。
				If LenB([tx_支払条件].Text & Chr(KeyAscii)) - LenB([tx_支払条件].SelText) = [tx_支払条件].MaxLength - 1 Then
					ReturnF = True
					[tx_支払条件].NextSetFocus()
				End If
			End If
		End If
EventExitSub:
		e.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			e.Handled = True
		End If
	End Sub

	Private Sub tx_支払条件_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_支払条件.KeyDown
		ReturnF = True
	End Sub

	Private Sub tx_支払条件_Leave(sender As Object, e As EventArgs) Handles tx_支払条件.Leave
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = ""
		If ReturnF = False Then
			'UPGRADE_WARNING: オブジェクト tx_支払条件.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[tx_支払条件].Undo()
		End If
		ReturnF = False
	End Sub

	Private Sub tx_支払条件他_Enter(sender As Object, e As EventArgs) Handles tx_支払条件他.Enter
		If Item_Check(([tx_支払条件他].TabIndex)) = False Then
			Exit Sub
		End If

		PreviousControl = Me.ActiveControl
		'ボタン名設定
		'UPGRADE_ISSUE: Control Name は、汎用名前空間 ActiveControl 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
		Call SetUpFuncs(Me.ActiveControl.Name)
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = "支払条件内容を入力して下さい。"
	End Sub

	Private Sub tx_支払条件他_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_支払条件他.KeyDown
		ReturnF = True
	End Sub

	Private Sub tx_支払条件他_Leave(sender As Object, e As EventArgs) Handles tx_支払条件他.Leave
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = ""
		If ReturnF = False Then
			'UPGRADE_WARNING: オブジェクト tx_支払条件他.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[tx_支払条件他].Undo()
		End If
		ReturnF = False
	End Sub

	Private Sub tx_納期表示_Enter(sender As Object, e As EventArgs) Handles tx_納期表示.Enter
		If Item_Check(([tx_納期表示].TabIndex)) = False Then
			Exit Sub
		End If
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = "納期表示を入力して下さい。"
		PreviousControl = Me.ActiveControl

		'ボタン名設定
		'UPGRADE_ISSUE: Control Name は、汎用名前空間 ActiveControl 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
		Call SetUpFuncs(Me.ActiveControl.Name)
	End Sub

	Private Sub tx_納期表示_KeyPress(sender As Object, e As KeyPressEventArgs) Handles tx_納期表示.KeyPress
		Dim KeyAscii As Integer = Asc(e.KeyChar)
		'MaxLengthは１多く設定する。

		Const Numbers As String = "012" ' 入力許可文字
		'Dim strText As String

		If KeyAscii = System.Windows.Forms.Keys.Return Then KeyAscii = 0

		'数字検査をする
		If KeyAscii <> 8 Then ' バックスペースは例外
			If InStr(Numbers, Chr(KeyAscii)) = 0 Then
				KeyAscii = 0 ' 入力を無効にする
				GoTo EventExitSub
			Else
				'UPGRADE_WARNING: TextBox プロパティ tx_納期表示.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
				'UPGRADE_ISSUE: 定数 vbFromUnicode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
				'UPGRADE_ISSUE: LenB 関数はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="367764E5-F3F8-4E43-AC3E-7FE0B5E074E2"' をクリックしてください。
				If LenB([tx_納期表示].Text) = [tx_納期表示].MaxLength - 1 Then
					Call SelText([tx_納期表示])
				End If
				'UPGRADE_WARNING: TextBox プロパティ tx_納期表示.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
				'UPGRADE_ISSUE: 定数 vbFromUnicode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
				'UPGRADE_ISSUE: LenB 関数はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="367764E5-F3F8-4E43-AC3E-7FE0B5E074E2"' をクリックしてください。
				If LenB([tx_納期表示].Text & Chr(KeyAscii)) - LenB([tx_納期表示].SelText) = [tx_納期表示].MaxLength - 1 Then
					ReturnF = True
					[tx_納期表示].NextSetFocus()
				End If
			End If
		End If
EventExitSub:
		e.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			e.Handled = True
		End If
	End Sub

	Private Sub tx_納期表示_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_納期表示.KeyDown
		ReturnF = True
	End Sub

	Private Sub tx_納期表示_Leave(sender As Object, e As EventArgs) Handles tx_納期表示.Leave
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = ""
		If ReturnF = False Then
			'UPGRADE_WARNING: オブジェクト tx_納期表示.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[tx_納期表示].Undo()
		End If
		ReturnF = False
	End Sub

	Private Sub tx_納期表示他_Enter(sender As Object, e As EventArgs) Handles tx_納期表示他.Enter
		If Item_Check(([tx_納期表示他].TabIndex)) = False Then
			Exit Sub
		End If

		PreviousControl = Me.ActiveControl
		'ボタン名設定
		'UPGRADE_ISSUE: Control Name は、汎用名前空間 ActiveControl 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
		Call SetUpFuncs(Me.ActiveControl.Name)
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = "納期内容を入力して下さい。"
	End Sub

	Private Sub tx_納期表示他_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_納期表示他.KeyDown
		ReturnF = True
	End Sub

	Private Sub tx_納期表示他_Leave(sender As Object, e As EventArgs) Handles tx_納期表示他.Leave
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = ""
		If ReturnF = False Then
			'UPGRADE_WARNING: オブジェクト tx_納期表示他.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[tx_納期表示他].Undo()
		End If
		ReturnF = False
	End Sub

	Private Sub tx_出力日_Enter(sender As Object, e As EventArgs) Handles tx_出力日.Enter
		If Item_Check(([tx_出力日].TabIndex)) = False Then
			Exit Sub
		End If
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = "見積日出力を選択して下さい。"
		PreviousControl = Me.ActiveControl

		'ボタン名設定
		'UPGRADE_ISSUE: Control Name は、汎用名前空間 ActiveControl 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
		Call SetUpFuncs(Me.ActiveControl.Name)
	End Sub

	Private Sub tx_出力日_KeyPress(sender As Object, e As KeyPressEventArgs) Handles tx_出力日.KeyPress
		Dim KeyAscii As Integer = Asc(e.KeyChar)
		'MaxLengthは１多く設定する。

		Const Numbers As String = "01" ' 入力許可文字
		'Dim strText As String

		If KeyAscii = System.Windows.Forms.Keys.Return Then KeyAscii = 0

		'数字検査をする
		If KeyAscii <> 8 Then ' バックスペースは例外
			If InStr(Numbers, Chr(KeyAscii)) = 0 Then
				KeyAscii = 0 ' 入力を無効にする
				GoTo EventExitSub
			Else
				'UPGRADE_WARNING: TextBox プロパティ tx_出力日.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
				'UPGRADE_ISSUE: 定数 vbFromUnicode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
				'UPGRADE_ISSUE: LenB 関数はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="367764E5-F3F8-4E43-AC3E-7FE0B5E074E2"' をクリックしてください。
				If LenB([tx_出力日].Text) = [tx_出力日].MaxLength - 1 Then
					Call SelText([tx_出力日])
				End If
				'UPGRADE_WARNING: TextBox プロパティ tx_出力日.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
				'UPGRADE_ISSUE: 定数 vbFromUnicode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
				'UPGRADE_ISSUE: LenB 関数はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="367764E5-F3F8-4E43-AC3E-7FE0B5E074E2"' をクリックしてください。
				If LenB([tx_出力日].Text & Chr(KeyAscii)) - LenB([tx_出力日].SelText) = [tx_出力日].MaxLength - 1 Then
					ReturnF = True
					[tx_出力日].NextSetFocus()
				End If
			End If
		End If
EventExitSub:
		e.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			e.Handled = True
		End If
	End Sub

	Private Sub tx_出力日_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_出力日.KeyDown
		ReturnF = True
	End Sub

	Private Sub tx_出力日_Leave(sender As Object, e As EventArgs) Handles tx_出力日.Leave
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = ""
		If ReturnF = False Then
			'UPGRADE_WARNING: オブジェクト tx_出力日.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[tx_出力日].Undo()
		End If
		ReturnF = False
	End Sub

	Private Sub tx_有効期限_Enter(sender As Object, e As EventArgs) Handles tx_有効期限.Enter
		If Item_Check(([tx_有効期限].TabIndex)) = False Then
			Exit Sub
		End If

		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = "有効期限を入力して下さい。"
		PreviousControl = Me.ActiveControl
	End Sub

	Private Sub tx_有効期限_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_有効期限.KeyDown
		ReturnF = True
	End Sub

	Private Sub tx_有効期限_Leave(sender As Object, e As EventArgs) Handles tx_有効期限.Leave
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = ""
		If ReturnF = False Then
			'UPGRADE_WARNING: オブジェクト tx_有効期限.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[tx_有効期限].Undo()
		End If
		ReturnF = False
	End Sub

	Private Sub tx_受注区分_Enter(sender As Object, e As EventArgs) Handles tx_受注区分.Enter
		If Item_Check(([tx_出力日].TabIndex)) = False Then
			Exit Sub
		End If
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = "受注区分を入力して下さい。"
		PreviousControl = Me.ActiveControl

		'ボタン名設定
		'UPGRADE_ISSUE: Control Name は、汎用名前空間 ActiveControl 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
		Call SetUpFuncs(Me.ActiveControl.Name)
	End Sub

	Private Sub tx_受注区分_KeyPress(sender As Object, e As KeyPressEventArgs) Handles tx_受注区分.KeyPress
		Dim KeyAscii As Integer = Asc(e.KeyChar)
		'MaxLengthは１多く設定する。

		Const Numbers As String = "01" ' 入力許可文字
		'Dim strText As String

		If KeyAscii = System.Windows.Forms.Keys.Return Then KeyAscii = 0

		'数字検査をする
		If KeyAscii <> 8 Then ' バックスペースは例外
			If InStr(Numbers, Chr(KeyAscii)) = 0 Then
				KeyAscii = 0 ' 入力を無効にする
				GoTo EventExitSub
			Else
				'UPGRADE_WARNING: TextBox プロパティ tx_受注区分.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
				'UPGRADE_ISSUE: 定数 vbFromUnicode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
				'UPGRADE_ISSUE: LenB 関数はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="367764E5-F3F8-4E43-AC3E-7FE0B5E074E2"' をクリックしてください。
				If LenB([tx_受注区分].Text) = [tx_受注区分].MaxLength - 1 Then
					Call SelText([tx_受注区分])
				End If
				'UPGRADE_WARNING: TextBox プロパティ tx_受注区分.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
				'UPGRADE_ISSUE: 定数 vbFromUnicode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
				'UPGRADE_ISSUE: LenB 関数はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="367764E5-F3F8-4E43-AC3E-7FE0B5E074E2"' をクリックしてください。
				If LenB([tx_受注区分].Text & Chr(KeyAscii)) - LenB([tx_受注区分].SelText) = [tx_受注区分].MaxLength - 1 Then
					ReturnF = True
					[tx_受注区分].NextSetFocus()
				End If
			End If
		End If
EventExitSub:
		e.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			e.Handled = True
		End If
	End Sub

	Private Sub tx_受注区分_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_受注区分.KeyDown
		ReturnF = True
	End Sub

	Private Sub tx_受注区分_Leave(sender As Object, e As EventArgs) Handles tx_受注区分.Leave
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = ""
		If ReturnF = False Then
			'UPGRADE_WARNING: オブジェクト tx_受注区分.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[tx_受注区分].Undo()
		End If
		ReturnF = False
	End Sub

	Private Sub tx_受注日付Y_Enter(sender As Object, e As EventArgs) Handles tx_受注日付Y.Enter
		'入力チェック
		If Item_Check(([tx_受注日付Y].TabIndex)) = False Then
			Exit Sub
		End If
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = "受注日付を入力して下さい。"
		PreviousControl = Me.ActiveControl
		'ボタン名設定
		'UPGRADE_ISSUE: Control Name は、汎用名前空間 ActiveControl 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
		Call SetUpFuncs(Me.ActiveControl.Name)
	End Sub

	Private Sub tx_受注日付Y_KeyPress(sender As Object, e As KeyPressEventArgs) Handles tx_受注日付Y.KeyPress
		Dim KeyAscii As Integer = Asc(e.KeyChar)
		If KeyAscii <> 8 Then ' バックスペースは例外
			'UPGRADE_WARNING: TextBox プロパティ tx_受注日付Y.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
			'UPGRADE_ISSUE: 定数 vbFromUnicode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
			'UPGRADE_ISSUE: LenB 関数はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="367764E5-F3F8-4E43-AC3E-7FE0B5E074E2"' をクリックしてください。
			If LenB([tx_受注日付Y].Text & Chr(KeyAscii)) - LenB([tx_受注日付Y].SelText) = [tx_受注日付Y].MaxLength Then
				ReturnF = True
				[tx_受注日付Y].Focus()
			End If
		End If
		e.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			e.Handled = True
		End If
	End Sub

	Private Sub tx_受注日付Y_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_受注日付Y.KeyDown
		ReturnF = True
	End Sub

	Private Sub tx_受注日付Y_Leave(sender As Object, e As EventArgs) Handles tx_受注日付Y.Leave
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = ""
		If ReturnF = False Then
			'UPGRADE_WARNING: オブジェクト tx_受注日付Y.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[tx_受注日付Y].Undo()
		End If
		ReturnF = False
	End Sub

	Private Sub tx_受注日付M_Enter(sender As Object, e As EventArgs) Handles tx_受注日付M.Enter
		'入力チェック
		If Item_Check(([tx_受注日付M].TabIndex)) = False Then
			Exit Sub
		End If
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = "受注日付を入力して下さい。"
		PreviousControl = Me.ActiveControl
		'ボタン名設定
		'UPGRADE_ISSUE: Control Name は、汎用名前空間 ActiveControl 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
		Call SetUpFuncs(Me.ActiveControl.Name)
	End Sub

	Private Sub tx_受注日付M_KeyPress(sender As Object, e As KeyPressEventArgs) Handles tx_受注日付M.KeyPress
		Dim KeyAscii As Integer = Asc(e.KeyChar)
		If KeyAscii <> 8 Then ' バックスペースは例外
			'UPGRADE_WARNING: TextBox プロパティ tx_受注日付M.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
			'UPGRADE_ISSUE: 定数 vbFromUnicode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
			'UPGRADE_ISSUE: LenB 関数はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="367764E5-F3F8-4E43-AC3E-7FE0B5E074E2"' をクリックしてください。
			If LenB([tx_受注日付M].Text & Chr(KeyAscii)) - LenB([tx_受注日付M].SelText) = [tx_受注日付M].MaxLength Then
				If KeyAscii <> 0 Then '入力キーがエラーでないならば
					ReturnF = True
					[tx_受注日付M].Focus()
				End If
			Else
				Select Case Chr(KeyAscii)
					Case CStr(2) To CStr(9)
						ReturnF = True
						[tx_受注日付D].Focus()
				End Select
			End If
		End If
		e.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			e.Handled = True
		End If
	End Sub

	Private Sub tx_受注日付M_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_受注日付M.KeyDown
		ReturnF = True
	End Sub

	Private Sub tx_受注日付M_Leave(sender As Object, e As EventArgs) Handles tx_受注日付M.Leave
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = ""
		If ReturnF = False Then
			'UPGRADE_WARNING: オブジェクト tx_受注日付M.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[tx_受注日付M].Undo()
		End If
		ReturnF = False
	End Sub

	Private Sub tx_受注日付D_Enter(sender As Object, e As EventArgs) Handles tx_受注日付D.Enter
		'入力チェック
		If Item_Check(([tx_受注日付D].TabIndex)) = False Then
			Exit Sub
		End If
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = "受注日付を入力して下さい。"
		PreviousControl = Me.ActiveControl
		'ボタン名設定
		'UPGRADE_ISSUE: Control Name は、汎用名前空間 ActiveControl 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
		Call SetUpFuncs(Me.ActiveControl.Name)
	End Sub

	Private Sub tx_受注日付D_KeyPress(sender As Object, e As KeyPressEventArgs) Handles tx_受注日付D.KeyPress
		Dim KeyAscii As Integer = Asc(e.KeyChar)
		'    If KeyAscii <> 8 Then ' バックスペースは例外
		'        If LenB([tx_受注日付D].Text & Chr$(KeyAscii)) - _
		''            LenB([tx_受注日付D].SelText) = [tx_受注日付D].MaxLength Then
		'            If KeyAscii <> 0 Then       '入力キーがエラーでないならば
		'                    ReturnF = True
		'                    [tx_受注日付D].NextSetFocus
		'            End If
		'        Else
		'            Select Case Chr$(KeyAscii)
		'                Case 4 To 9
		'                    ReturnF = True
		'                    [tx_受注日付D].NextSetFocus
		'            End Select
		'        End If
		'    End If
		e.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			e.Handled = True
		End If
	End Sub

	Private Sub tx_受注日付D_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_受注日付D.KeyDown
		ReturnF = True
	End Sub

	Private Sub tx_受注日付D_Leave(sender As Object, e As EventArgs) Handles tx_受注日付D.Leave
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = ""
		If ReturnF = False Then
			'UPGRADE_WARNING: オブジェクト tx_受注日付D.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[tx_受注日付D].Undo()
		End If
		ReturnF = False
	End Sub

	Private Sub tx_大小口区分_Enter(sender As Object, e As EventArgs) Handles tx_大小口区分.Enter
		If Item_Check(([tx_大小口区分].TabIndex)) = False Then
			Exit Sub
		End If
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = "大小口区分を入力して下さい。"
		PreviousControl = Me.ActiveControl

		'ボタン名設定
		'UPGRADE_ISSUE: Control Name は、汎用名前空間 ActiveControl 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
		Call SetUpFuncs(Me.ActiveControl.Name)
	End Sub

	Private Sub tx_大小口区分_KeyPress(sender As Object, e As KeyPressEventArgs) Handles tx_大小口区分.KeyPress
		Dim KeyAscii As Integer = Asc(e.KeyChar)
		'MaxLengthは１多く設定する。

		Const Numbers As String = "01" ' 入力許可文字
		'Dim strText As String

		If KeyAscii = System.Windows.Forms.Keys.Return Then KeyAscii = 0

		'数字検査をする
		If KeyAscii <> 8 Then ' バックスペースは例外
			If InStr(Numbers, Chr(KeyAscii)) = 0 Then
				KeyAscii = 0 ' 入力を無効にする
				GoTo EventExitSub
			Else
				'UPGRADE_WARNING: TextBox プロパティ tx_大小口区分.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
				'UPGRADE_ISSUE: 定数 vbFromUnicode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
				'UPGRADE_ISSUE: LenB 関数はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="367764E5-F3F8-4E43-AC3E-7FE0B5E074E2"' をクリックしてください。
				If LenB([tx_大小口区分].Text) = [tx_大小口区分].MaxLength - 1 Then
					Call SelText([tx_大小口区分])
				End If
				'UPGRADE_WARNING: TextBox プロパティ tx_大小口区分.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
				'UPGRADE_ISSUE: 定数 vbFromUnicode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
				'UPGRADE_ISSUE: LenB 関数はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="367764E5-F3F8-4E43-AC3E-7FE0B5E074E2"' をクリックしてください。
				If LenB([tx_大小口区分].Text & Chr(KeyAscii)) - LenB([tx_大小口区分].SelText) = [tx_大小口区分].MaxLength - 1 Then
					ReturnF = True
					[tx_大小口区分].NextSetFocus()
				End If
			End If
		End If
EventExitSub:
		e.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			e.Handled = True
		End If
	End Sub

	Private Sub tx_大小口区分_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_大小口区分.KeyDown
		ReturnF = True
	End Sub

	Private Sub tx_大小口区分_Leave(sender As Object, e As EventArgs) Handles tx_大小口区分.Leave
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = ""
		If ReturnF = False Then
			'UPGRADE_WARNING: オブジェクト tx_大小口区分.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[tx_大小口区分].Undo()
		End If
		ReturnF = False
	End Sub

	Private Sub tx_出精値引_Enter(sender As Object, e As EventArgs) Handles tx_出精値引.Enter
		If Item_Check(([tx_出精値引].TabIndex)) = False Then
			Exit Sub
		End If

		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = "出精値引(マイナス)を入力して下さい。"
		PreviousControl = Me.ActiveControl
	End Sub

	Private Sub tx_出精値引_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_出精値引.KeyDown
		ReturnF = True
	End Sub

	Private Sub tx_出精値引_Leave(sender As Object, e As EventArgs) Handles tx_出精値引.Leave
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = ""
		If ReturnF = False Then
			'UPGRADE_WARNING: オブジェクト tx_出精値引.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[tx_出精値引].Undo()
		End If
		ReturnF = False
	End Sub

	''	'2020/04/11 ADD↓
	''	Private Sub tx_見積確定区分_Enter(sender As Object, e As EventArgs) Handles tx_見積確定区分.Enter
	''		If Item_Check(([tx_出力日].TabIndex)) = False Then
	''			Exit Sub
	''		End If
	''		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
	''		[sb_Msg].Items.Item(0).Text = "見積確定区分を入力して下さい。"
	''		PreviousControl = Me.ActiveControl
	''
	''		'ボタン名設定
	''		'UPGRADE_ISSUE: Control Name は、汎用名前空間 ActiveControl 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
	''		Call SetUpFuncs(Me.ActiveControl.Name)
	''	End Sub
	''
	''	Private Sub tx_見積確定区分_KeyPress(sender As Object, e As KeyPressEventArgs) Handles tx_見積確定区分.KeyPress
	''		Dim KeyAscii As Integer = Asc(e.KeyChar)
	''		'MaxLengthは１多く設定する。
	''
	''		Const Numbers As String = "01" ' 入力許可文字
	''		'Dim strText As String
	''
	''		If KeyAscii = System.Windows.Forms.Keys.Return Then KeyAscii = 0
	''
	''		'数字検査をする
	''		If KeyAscii <> 8 Then ' バックスペースは例外
	''			If InStr(Numbers, Chr(KeyAscii)) = 0 Then
	''				KeyAscii = 0 ' 入力を無効にする
	''				GoTo EventExitSub
	''			Else
	''				'UPGRADE_WARNING: TextBox プロパティ tx_見積確定区分.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
	''				'UPGRADE_ISSUE: 定数 vbFromUnicode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
	''				'UPGRADE_ISSUE: LenB 関数はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="367764E5-F3F8-4E43-AC3E-7FE0B5E074E2"' をクリックしてください。
	''				If LenB([tx_見積確定区分].Text) = [tx_見積確定区分].MaxLength - 1 Then
	''					Call SelText([tx_見積確定区分])
	''				End If
	''				'UPGRADE_WARNING: TextBox プロパティ tx_見積確定区分.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
	''				'UPGRADE_ISSUE: 定数 vbFromUnicode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
	''				'UPGRADE_ISSUE: LenB 関数はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="367764E5-F3F8-4E43-AC3E-7FE0B5E074E2"' をクリックしてください。
	''				If LenB([tx_見積確定区分].Text & Chr(KeyAscii)) - LenB([tx_見積確定区分].SelectedText) = [tx_見積確定区分].MaxLength - 1 Then
	''					ReturnF = True
	''					[tx_見積確定区分].NextSetFocus()
	''				End If
	''			End If
	''		End If
	''EventExitSub:
	''		e.KeyChar = Chr(KeyAscii)
	''		If KeyAscii = 0 Then
	''			e.Handled = True
	''		End If
	''	End Sub
	''
	''	Private Sub tx_見積確定区分_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_見積確定区分.KeyDown
	''		ReturnF = True
	''	End Sub
	''
	''	Private Sub tx_見積確定区分_Leave(sender As Object, e As EventArgs) Handles tx_見積確定区分.Leave
	''		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
	''		[sb_Msg].Items.Item(0).Text = ""
	''		If ReturnF = False Then
	''			'UPGRADE_WARNING: オブジェクト tx_見積確定区分.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
	''			[tx_見積確定区分].Undo()
	''		End If
	''		ReturnF = False
	''	End Sub

	Private Sub tx_完了日Y_Enter(sender As Object, e As EventArgs) Handles tx_完了日Y.Enter
		'入力チェック
		If Item_Check(([tx_完了日Y].TabIndex)) = False Then
			Exit Sub
		End If
		PreviousControl = Me.ActiveControl
		'ボタン名設定
		'UPGRADE_ISSUE: Control Name は、汎用名前空間 ActiveControl 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
		Call SetUpFuncs(Me.ActiveControl.Name)
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = "完了日を入力して下さい。"
	End Sub

	Private Sub tx_完了日Y_KeyPress(sender As Object, e As KeyPressEventArgs) Handles tx_完了日Y.KeyPress
		Dim KeyAscii As Integer = Asc(e.KeyChar)
		If KeyAscii <> 8 Then ' バックスペースは例外
			'UPGRADE_WARNING: TextBox プロパティ tx_完了日Y.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
			'UPGRADE_ISSUE: 定数 vbFromUnicode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
			'UPGRADE_ISSUE: LenB 関数はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="367764E5-F3F8-4E43-AC3E-7FE0B5E074E2"' をクリックしてください。
			If LenB([tx_完了日Y].Text & Chr(KeyAscii)) - LenB([tx_完了日Y].SelText) = [tx_完了日Y].MaxLength Then
				ReturnF = True
				[tx_完了日Y].Focus()
			End If
		End If
		e.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			e.Handled = True
		End If
	End Sub

	Private Sub tx_完了日Y_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_完了日Y.KeyDown
		ReturnF = True
	End Sub

	Private Sub tx_完了日Y_Leave(sender As Object, e As EventArgs) Handles tx_完了日Y.Leave
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = ""
		If ReturnF = False Then
			'UPGRADE_WARNING: オブジェクト tx_完了日Y.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[tx_完了日Y].Undo()
		End If
		ReturnF = False
	End Sub

	Private Sub tx_完了日M_Enter(sender As Object, e As EventArgs) Handles tx_完了日M.Enter
		'入力チェック
		If Item_Check(([tx_完了日M].TabIndex)) = False Then
			Exit Sub
		End If
		PreviousControl = Me.ActiveControl
		'ボタン名設定
		'UPGRADE_ISSUE: Control Name は、汎用名前空間 ActiveControl 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
		Call SetUpFuncs(Me.ActiveControl.Name)
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = "完了日を入力して下さい。"
	End Sub

	Private Sub tx_完了日M_KeyPress(sender As Object, e As KeyPressEventArgs) Handles tx_完了日M.KeyPress
		Dim KeyAscii As Integer = Asc(e.KeyChar)
		If KeyAscii <> 8 Then ' バックスペースは例外
			'UPGRADE_WARNING: TextBox プロパティ tx_完了日M.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
			'UPGRADE_ISSUE: 定数 vbFromUnicode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
			'UPGRADE_ISSUE: LenB 関数はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="367764E5-F3F8-4E43-AC3E-7FE0B5E074E2"' をクリックしてください。
			If LenB([tx_完了日M].Text & Chr(KeyAscii)) - LenB([tx_完了日M].SelText) = [tx_完了日M].MaxLength Then
				If KeyAscii <> 0 Then '入力キーがエラーでないならば
					ReturnF = True
					[tx_完了日M].Focus()
				End If
			Else
				Select Case Chr(KeyAscii)
					Case CStr(2) To CStr(9)
						ReturnF = True
						[tx_完了日D].Focus()
				End Select
			End If
		End If
		e.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			e.Handled = True
		End If
	End Sub

	Private Sub tx_完了日M_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_完了日M.KeyDown
		ReturnF = True
	End Sub

	Private Sub tx_完了日M_Leave(sender As Object, e As EventArgs) Handles tx_完了日M.Leave
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = ""
		If ReturnF = False Then
			'UPGRADE_WARNING: オブジェクト tx_完了日M.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[tx_完了日M].Undo()
		End If
		ReturnF = False
	End Sub

	Private Sub tx_完了日D_Enter(sender As Object, e As EventArgs) Handles tx_完了日D.Enter
		'入力チェック
		If Item_Check(([tx_完了日D].TabIndex)) = False Then
			Exit Sub
		End If
		PreviousControl = Me.ActiveControl
		'ボタン名設定
		'UPGRADE_ISSUE: Control Name は、汎用名前空間 ActiveControl 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
		Call SetUpFuncs(Me.ActiveControl.Name)
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = "完了日を入力して下さい。"
	End Sub

	Private Sub tx_完了日D_KeyPress(sender As Object, e As KeyPressEventArgs) Handles tx_完了日D.KeyPress
		Dim KeyAscii As Integer = Asc(e.KeyChar)
		'nop
		e.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			e.Handled = True
		End If
	End Sub

	Private Sub tx_完了日D_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_完了日D.KeyDown
		ReturnF = True
	End Sub

	Private Sub tx_完了日D_Leave(sender As Object, e As EventArgs) Handles tx_完了日D.Leave
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = ""
		If ReturnF = False Then
			'UPGRADE_WARNING: オブジェクト tx_完了日D.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[tx_完了日D].Undo()
		End If
		ReturnF = False
	End Sub

	Private Sub tx_完了者名_Enter(sender As Object, e As EventArgs) Handles tx_完了者名.Enter
		If Item_Check(([tx_完了者名].TabIndex)) = False Then
			Exit Sub
		End If

		PreviousControl = Me.ActiveControl
		'ボタン名設定
		'UPGRADE_ISSUE: Control Name は、汎用名前空間 ActiveControl 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
		Call SetUpFuncs(Me.ActiveControl.Name)
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = "完了者名を入力して下さい。"
	End Sub

	Private Sub tx_完了者名_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_完了者名.KeyDown
		ReturnF = True
	End Sub

	Private Sub tx_完了者名_Leave(sender As Object, e As EventArgs) Handles tx_完了者名.Leave
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = ""
		If ReturnF = False Then
			'UPGRADE_WARNING: オブジェクト tx_完了者名.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[tx_完了者名].Undo()
		End If
		ReturnF = False
	End Sub

	Private Sub tx_請求予定Y_Enter(sender As Object, e As EventArgs) Handles tx_請求予定Y.Enter
		'入力チェック
		If Item_Check(([tx_請求予定Y].TabIndex)) = False Then
			Exit Sub
		End If
		PreviousControl = Me.ActiveControl
		'ボタン名設定
		'UPGRADE_ISSUE: Control Name は、汎用名前空間 ActiveControl 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
		Call SetUpFuncs(Me.ActiveControl.Name)
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = "請求予定を入力して下さい。"
	End Sub

	Private Sub tx_請求予定Y_KeyPress(sender As Object, e As KeyPressEventArgs) Handles tx_請求予定Y.KeyPress
		Dim KeyAscii As Integer = Asc(e.KeyChar)
		If KeyAscii <> 8 Then ' バックスペースは例外
			'UPGRADE_WARNING: TextBox プロパティ tx_請求予定Y.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
			'UPGRADE_ISSUE: 定数 vbFromUnicode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
			'UPGRADE_ISSUE: LenB 関数はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="367764E5-F3F8-4E43-AC3E-7FE0B5E074E2"' をクリックしてください。
			If LenB([tx_請求予定Y].Text & Chr(KeyAscii)) - LenB([tx_請求予定Y].SelText) = [tx_請求予定Y].MaxLength Then
				ReturnF = True
				[tx_請求予定Y].Focus()
			End If
		End If
		e.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			e.Handled = True
		End If
	End Sub

	Private Sub tx_請求予定Y_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_請求予定Y.KeyDown
		ReturnF = True
	End Sub

	Private Sub tx_請求予定Y_Leave(sender As Object, e As EventArgs) Handles tx_請求予定Y.Leave
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = ""
		If ReturnF = False Then
			'UPGRADE_WARNING: オブジェクト tx_請求予定Y.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[tx_請求予定Y].Undo()
		End If
		ReturnF = False
	End Sub

	Private Sub tx_請求予定M_Enter(sender As Object, e As EventArgs) Handles tx_請求予定M.Enter
		'入力チェック
		If Item_Check(([tx_請求予定M].TabIndex)) = False Then
			Exit Sub
		End If
		PreviousControl = Me.ActiveControl
		'ボタン名設定
		'UPGRADE_ISSUE: Control Name は、汎用名前空間 ActiveControl 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
		Call SetUpFuncs(Me.ActiveControl.Name)
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = "請求予定を入力して下さい。"
	End Sub

	Private Sub tx_請求予定M_KeyPress(sender As Object, e As KeyPressEventArgs) Handles tx_請求予定M.KeyPress
		Dim KeyAscii As Integer = Asc(e.KeyChar)
		If KeyAscii <> 8 Then ' バックスペースは例外
			'UPGRADE_WARNING: TextBox プロパティ tx_請求予定M.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
			'UPGRADE_ISSUE: 定数 vbFromUnicode はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
			'UPGRADE_ISSUE: LenB 関数はサポートされません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="367764E5-F3F8-4E43-AC3E-7FE0B5E074E2"' をクリックしてください。
			If LenB([tx_請求予定M].Text & Chr(KeyAscii)) - LenB([tx_請求予定M].SelText) = [tx_請求予定M].MaxLength Then
				If KeyAscii <> 0 Then '入力キーがエラーでないならば
					ReturnF = True
					[tx_請求予定M].Focus()
				End If
			Else
				Select Case Chr(KeyAscii)
					Case CStr(2) To CStr(9)
						ReturnF = True
						[tx_請求予定D].Focus()
				End Select
			End If
		End If
		e.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			e.Handled = True
		End If
	End Sub

	Private Sub tx_請求予定M_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_請求予定M.KeyDown
		ReturnF = True
	End Sub

	Private Sub tx_請求予定M_Leave(sender As Object, e As EventArgs) Handles tx_請求予定M.Leave
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = ""
		If ReturnF = False Then
			'UPGRADE_WARNING: オブジェクト tx_請求予定M.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[tx_請求予定M].Undo()
		End If
		ReturnF = False
	End Sub

	Private Sub tx_請求予定D_Enter(sender As Object, e As EventArgs) Handles tx_請求予定D.Enter
		'入力チェック
		If Item_Check(([tx_請求予定D].TabIndex)) = False Then
			Exit Sub
		End If
		PreviousControl = Me.ActiveControl
		'ボタン名設定
		'UPGRADE_ISSUE: Control Name は、汎用名前空間 ActiveControl 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
		Call SetUpFuncs(Me.ActiveControl.Name)
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = "請求予定を入力して下さい。"
	End Sub

	Private Sub tx_請求予定D_KeyPress(sender As Object, e As KeyPressEventArgs) Handles tx_請求予定D.KeyPress
		Dim KeyAscii As Integer = Asc(e.KeyChar)
		'nop
		e.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			e.Handled = True
		End If
	End Sub

	Private Sub tx_請求予定D_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_請求予定D.KeyDown
		ReturnF = True
	End Sub

	Private Sub tx_請求予定D_Leave(sender As Object, e As EventArgs) Handles tx_請求予定D.Leave
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = ""
		If ReturnF = False Then
			'UPGRADE_WARNING: オブジェクト tx_請求予定D.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[tx_請求予定D].Undo()
		End If
		ReturnF = False
	End Sub

	Private Sub tx_経過備考1_Enter(sender As Object, e As EventArgs) Handles tx_経過備考1.Enter
		If Item_Check(([tx_経過備考1].TabIndex)) = False Then
			Exit Sub
		End If

		PreviousControl = Me.ActiveControl
		'ボタン名設定
		'UPGRADE_ISSUE: Control Name は、汎用名前空間 ActiveControl 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
		Call SetUpFuncs(Me.ActiveControl.Name)
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = "備考を入力して下さい。最大全角23文字"
	End Sub

	Private Sub tx_経過備考1_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_経過備考1.KeyDown
		ReturnF = True
	End Sub

	Private Sub tx_経過備考1_Leave(sender As Object, e As EventArgs) Handles tx_経過備考1.Leave
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = ""
		If ReturnF = False Then
			'UPGRADE_WARNING: オブジェクト tx_経過備考1.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[tx_経過備考1].Undo()
		End If
		ReturnF = False
	End Sub

	Private Sub tx_経過備考2_Enter(sender As Object, e As EventArgs) Handles tx_経過備考2.Enter
		If Item_Check(([tx_経過備考2].TabIndex)) = False Then
			Exit Sub
		End If

		PreviousControl = Me.ActiveControl
		'ボタン名設定
		'UPGRADE_ISSUE: Control Name は、汎用名前空間 ActiveControl 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
		Call SetUpFuncs(Me.ActiveControl.Name)
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = "備考を入力して下さい。最大全角23文字"
	End Sub

	Private Sub tx_経過備考2_KeyDown(sender As Object, e As KeyEventArgs) Handles tx_経過備考2.KeyDown
		' ↑キーを検出
		If e.KeyCode = Keys.Up Then
			tx_経過備考1.Focus()
			e.Handled = True
		End If
		' Enterキーを検出
		If e.KeyCode = Keys.Return Then
			ReturnF = True
		End If
	End Sub

	Private Sub tx_経過備考2_Leave(sender As Object, e As EventArgs) Handles tx_経過備考2.Leave
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		[sb_Msg].Items.Item(0).Text = ""
		If ReturnF = False Then
			'UPGRADE_WARNING: オブジェクト tx_経過備考2.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[tx_経過備考2].Undo()
		End If
		ReturnF = False
	End Sub
	'2020/04/11 ADD↑


	'Private Function Upload() As Boolean
	'    Dim cmd As adodb.Command
	'
	'
	'    'マウスポインターを砂時計にする
	'    HourGlass True
	'
	'    Cn.BeginTrans   '---トランザクションの開始
	'    On Error GoTo Upload_err
	'
	'    '見積セット
	'    cmd.ActiveConnection = Cn
	'    cmd.CommandText = "usp_UPD見積"
	'    cmd.CommandType = adCmdStoredProc
	'
	'    With cmd.Parameters
	'        .Item("@i見積番号").Value = SpcToNull(rf_見積番号)
	'        .Item("@i担当者CD").Value = tx_担当者CD.Text
	'        .Item("@i見積日付").Value = idc(0).Text
	'        .Item("@i見積件名").Value = tx_見積件名.Text
	'        .Item("@i得意先CD").Value = tx_得意先CD.Text
	'        .Item("@i得意先名1").Value = tx_得意先名1.Text
	'        .Item("@i得意先名2").Value = tx_得意先名2.Text
	'        .Item("@i得TEL").Value = tx_得TEL.Text
	'        .Item("@i得FAX").Value = tx_得FAX.Text
	'        .Item("@i得意先担当者").Value = tx_得担当者.Text
	'        .Item("@i納入得意先CD").Value = tx_納得意先CD.Text
	'        .Item("@i納入先CD").Value = tx_納入先CD.Text
	'        .Item("@i納入先名1").Value = tx_納入先名1.Text
	'        .Item("@i納入先名2").Value = tx_納入先名2.Text
	'        .Item("@i郵便番号").Value = tx_郵便番号.Text
	'        .Item("@i住所1").Value = tx_納住所1.Text
	'        .Item("@i住所2").Value = tx_納住所2.Text
	'        .Item("@i納TEL").Value = tx_納TEL.Text
	'        .Item("@i納FAX").Value = tx_納FAX.Text
	'        .Item("@i納入先担当者").Value = tx_納担当者.Text
	'        .Item("@i納期S").Value = idc(1).Text
	'        .Item("@i納期E").Value = idc(2).Text
	'        .Item("@i備考").Value = tx_備考.Text
	'        .Item("@i物件規模金額").Value = SpcToNull(tx_物件金額.Text, 0)
	'        .Item("@iオープン日").Value = idc(3).Text
	'        .Item("@i物件種別").Value = SpcToNull(tx_物件種別.Text, 0)
	'        .Item("@i現場名").Value = tx_現場名.Text
	'        .Item("@i支払条件").Value = SpcToNull(tx_支払条件.Text, 0)
	'        .Item("@i支払条件その他").Value = tx_支払条件他.Text
	'        .Item("@i納期表示").Value = SpcToNull(tx_納期表示.Text, 0)
	'        .Item("@i納期その他").Value = tx_納期表示他.Text
	'        .Item("@i見積日出力").Value = SpcToNull(tx_出力日.Text, 0)
	'        .Item("@i有効期限").Value = SpcToNull(tx_有効期限.Text, 0)
	'        .Item("@i受注区分").Value = SpcToNull(tx_受注区分.Text, 0)
	'        .Item("@i受注日付").Value = SpcToNull(idc(4).Text)
	'        .Item("@i大小口区分").Value = SpcToNull(tx_大小口区分.Text, 0)
	'        .Item("@i出精値引").Value = SpcToNull(tx_出精値引.Text, 0)
	'     End With
	'
	'    cmd.Execute
	'    If cmd.State <> 0 Then
	'        If cmd(0) <> 0 Then
	'            CriticalAlarm (cmd("@RetST") & vbCrLf & cmd("@RetMSG"))
	'            Cn.RollbackTrans
	'            GoTo Upload_Correct
	'        End If
	'    Else
	'        If cmd(0) <> 0 Then
	'            CriticalAlarm (cmd("@RetST") & vbCrLf & cmd("@RetMSG"))
	'            Cn.RollbackTrans
	'            GoTo Upload_Correct
	'        End If
	'    End If
	'
	'    Cn.CommitTrans  '---トランザクションをコミットする
	'    Upload = True
	'
	'Upload_Correct:
	'    Set cmd = Nothing
	'    HourGlass False
	'    On Error GoTo 0
	'    Exit Function
	'Upload_err:  '---エラー時
	'    CheckAlarm Err.Number & vbCrLf & Err.Description
	'    Cn.RollbackTrans 'トランザクションを破棄する
	'    Resume Upload_Correct
	'End Function

	Private Function Upload() As Boolean
		Upload = False

		'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト HD_見積番号 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		HD_見積番号 = SpcToNull([rf_見積番号].Text)
		HD_担当者CD = CShort([tx_担当者CD].Text)
		'UPGRADE_WARNING: オブジェクト idc().Text の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト HD_見積日付 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		HD_見積日付 = idc(0).Text
		HD_見積件名 = [tx_見積件名].Text
		HD_得意先CD = [tx_得意先CD].Text
		HD_得意先名1 = [tx_得意先名1].Text
		HD_得意先名2 = [tx_得意先名2].Text
		HD_得TEL = [tx_得TEL].Text
		HD_得FAX = [tx_得FAX].Text
		HD_得担当者 = [tx_得担当者].Text
		HD_納得意先CD = [tx_納得意先CD].Text
		HD_納入先CD = [tx_納入先CD].Text
		HD_納入先名1 = [tx_納入先名1].Text
		HD_納入先名2 = [tx_納入先名2].Text
		HD_納郵便番号 = [tx_郵便番号].Text
		HD_納住所1 = [tx_納住所1].Text
		HD_納住所2 = [tx_納住所2].Text
		HD_納TEL = [tx_納TEL].Text
		HD_納FAX = [tx_納FAX].Text
		HD_納担当者 = [tx_納担当者].Text
		'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト HD_納期S の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		HD_納期S = SpcToNull((idc(1).Text))
		'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト HD_納期E の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		HD_納期E = SpcToNull((idc(2).Text))
		HD_備考 = [tx_備考].Text
		'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		HD_規模金額 = SpcToNull(([tx_物件金額].Text), 0)
		'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト HD_OPEN日 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		HD_OPEN日 = SpcToNull((idc(3).Text))
		'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		HD_物件種別 = SpcToNull(([tx_物件種別].Text), 0)
		HD_現場名 = [tx_現場名].Text
		'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		HD_支払条件 = SpcToNull(([tx_支払条件].Text), 0)
		HD_支払条件他 = [tx_支払条件他].Text
		'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		HD_納期表示 = SpcToNull(([tx_納期表示].Text), 0)
		HD_納期表示他 = [tx_納期表示他].Text
		'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		HD_見積日出力 = SpcToNull(([tx_出力日].Text), 0)
		'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		HD_有効期限 = SpcToNull(([tx_有効期限].Text), 0)
		'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		HD_受注区分 = SpcToNull(([tx_受注区分].Text), 0)
		'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト HD_受注日付 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		HD_受注日付 = SpcToNull((idc(4).Text))
		'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		HD_大小口区分 = SpcToNull(([tx_大小口区分].Text), 0)
		'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		HD_出精値引 = SpcToNull(([tx_出精値引].Text), 0)

		'2016/04/07 ADD↓
		'    HD_税集計区分 = rf_税集計区分.Caption
		HD_税集計区分 = CShort([tx_税集計区分].Text)
		'2016/04/07 ADD↑
		HD_売上端数 = CShort([rf_売上端数].Text)
		HD_消費税端数 = CShort([rf_消費税端数].Text)

		'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		HD_合計金額 = SpcToNull((rf_合計金額.Text), 0)
		'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		HD_原価合計 = SpcToNull((rf_原価合計.Text), 0)
		'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		HD_原価率 = SpcToNull((rf_原価率.Text), 0)
		'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		HD_外税額 = SpcToNull((rf_外税額.Text), 0)

		'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト HD_得意先別見積番号 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		HD_得意先別見積番号 = SpcToNull(rf_得意先別見積番号.Text) '2005/07/04 ADD

		'    HD_見積区分 = SpcToNull(tx_見積区分.Text, 0)                        '2009/09/24 ADD


		'2013/03/08 ADD↓
		'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		HD_部署CD = NullToZero(([tx_部署CD].Text))
		'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		HD_販売先得意先CD = NullToZero(([tx_販売先得意先CD].Text), "")
		'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		HD_販売先得意先名1 = NullToZero(([rf_販売先得意先名1].Text), "")
		'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		HD_販売先得意先名2 = NullToZero(([rf_販売先得意先名2].Text), "")
		'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		HD_販売先納得意先CD = NullToZero(([tx_販売先納得意先CD].Text), "")
		'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		HD_販売先納入先CD = NullToZero(([tx_販売先納入先CD].Text), "")
		'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		HD_販売先納入先名1 = NullToZero(([rf_販売先納入先名1].Text), "")
		'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		HD_販売先納入先名2 = NullToZero(([rf_販売先納入先名2].Text), "")
		'2013/03/08 ADD↑

		'    HD_営業推進部CD = NullToZero([tx_営業推進部CD].Text) '2019/12/12 ADD
		'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		HD_工事担当CD = NullToZero(([tx_工事担当CD].Text)) '2022/08/08 ADD

		'2013/07/19 ADD↓
		'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		HD_社内伝票扱い = NullToZero(([ck_社内伝票扱い].CheckState))
		'2013/07/19 ADD↑

		'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		HD_伝票種類 = SpcToNull(([tx_伝票種類].Text), 0) '2015/02/04 ADD

		'2015/06/12 ADD↓
		'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		HD_ウエルシア物件内容CD = SpcToNull(([tx_ウエルシア物件内容CD].Text), 0)
		HD_ウエルシア物件内容名 = [tx_ウエルシア物件内容名].Text
		'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		HD_ウエルシアリース区分 = SpcToNull(([tx_ウエルシアリース区分].Text), 0)
		'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		HD_ウエルシア物件区分CD = SpcToNull(([tx_ウエルシア物件区分].Text), 0)
		'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		HD_ウエルシア売場面積 = SpcToNull(([tx_ウエルシア売場面積].Text), 0)
		'2015/06/12 ADD↑

		'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト HD_物件番号 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		HD_物件番号 = SpcToNull(([tx_物件番号].Text)) '2015/07/10 ADD
		'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト HD_統合見積番号 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		HD_統合見積番号 = SpcToNull(([tx_統合見積番号].Text)) '2024/01/16 ADD

		'2015/10/16 ADD↓
		'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト HD_受付日付 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		HD_受付日付 = SpcToNull((idc(5).Text))
		'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト HD_完工日付 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		HD_完工日付 = SpcToNull((idc(6).Text))
		'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		HD_発注担当者名 = NullToZero(([tx_発注担当者名].Text), "")
		'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		HD_作業内容 = NullToZero(([tx_作業内容].Text), "")
		'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		HD_YKサプライ区分 = SpcToNull(([tx_YKサプライ区分].Text), 0)
		'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		HD_YK物件区分 = SpcToNull(([tx_YK物件区分].Text), 0)
		'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		HD_YK請求区分 = SpcToNull(([tx_YK請求区分].Text), 0)
		'2015/10/16 ADD↑

		'2015/11/03 ADD↓
		'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		HD_化粧品メーカー区分 = SpcToNull(([tx_化粧品メーカー区分].Text), 0)
		'2015/11/03 ADD↑

		'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		HD_SM内容区分 = SpcToNull(([tx_SM内容区分].Text), 0) '2015/11/19 ADD

		'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		HD_クレーム区分 = SpcToNull(([tx_クレーム区分].Text), 0) '2016/04/09 ADD

		'2020/04/11 ADD↓
		'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		'HD_見積確定区分 = SpcToNull(([tx_見積確定区分].Text), 0) '2024/06/21 DEL

		'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト HD_完了日付 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		HD_完了日付 = SpcToNull((idc(7).Text))
		'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		HD_完了者名 = NullToZero(([tx_完了者名].Text), "")

		HD_完工日付 = HD_完了日付 '2024/07/23 ADD

		'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト HD_請求予定日 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		HD_請求予定日 = SpcToNull((idc(8).Text))

		'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		HD_経過備考1 = NullToZero(([tx_経過備考1].Text), "")
		'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		HD_経過備考2 = NullToZero(([tx_経過備考2].Text), "")

		'2020/04/11 ADD↑

		'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		HD_集計CD = NullToZero(([tx_集計CD].Text)) '2021/02/25 ADD

		'2022/10/10 ADD↓
		'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		HD_B請求管轄区分 = SpcToNull(([tx_B請求管轄区分].Text), 0)
		'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		HD_BtoB番号 = SpcToNull(([tx_BtoB番号].Text), 0)
		'2022/10/10 ADD↑

		'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		HD_業種区分 = SpcToNull(([tx_業種区分].Text), 0) '2023/04/18 ADD


		Upload = True
	End Function

	Private Sub Purge()
		Dim cmd As ADODB.Command

		'マウスポインターを砂時計にする
		HourGlass(True)

		Cn.BeginTrans() '---トランザクションの開始
		On Error GoTo Trans_err

		cmd = New ADODB.Command

		'見積セット
		cmd.let_ActiveConnection(Cn)
		cmd.CommandTimeout = 0 '2007/09/14 ADD
		cmd.CommandText = "usp_MT0100DEL見積"
		cmd.CommandType = ADODB.CommandTypeEnum.adCmdStoredProc

		With cmd.Parameters
			'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Item(1).Value = SpcToNull([rf_見積番号].Text)
		End With

		cmd.Execute()
		If cmd.State <> 0 Then
			If cmd.Parameters(0).Value <> 0 Then
				CriticalAlarm((cmd.Parameters("@RetST").Value & vbCrLf & cmd.Parameters("@RetMSG").Value))
				Cn.RollbackTrans()
				GoTo Trans_Correct
			End If
		Else
			If cmd.Parameters(0).Value <> 0 Then
				CriticalAlarm((cmd.Parameters("@RetST").Value & vbCrLf & cmd.Parameters("@RetMSG").Value))
				Cn.RollbackTrans()
				GoTo Trans_Correct
			End If
		End If

		Cn.CommitTrans() '---トランザクションをコミットする

Trans_Correct:
		'UPGRADE_NOTE: オブジェクト cmd をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
		cmd = Nothing
		On Error GoTo 0
		HourGlass(False)
		Exit Sub

Trans_err: '---エラー時
		MsgBox(Err.Number & " " & Err.Description)
		Cn.RollbackTrans() 'トランザクションを破棄する
		Resume Trans_Correct
	End Sub

	'--2003/11/05.ADD----------------
	Public Sub ReturnLoad(ByRef No As Integer)
		'    Call Download(No)
		Dim rs As ADODB.Recordset
		Dim sql As String
		'    Dim X As Integer
		'    Dim buf As String
		'    Dim KANNOOF As Integer

		On Error GoTo ReturnLoad_Err

		'マウスポインターを砂時計にする
		HourGlass(True)

		sql = "SELECT * FROM TD見積" & " WHERE 見積番号 = " & No

		rs = OpenRs(sql, Cn, ADODB.CursorTypeEnum.adOpenKeyset, ADODB.LockTypeEnum.adLockReadOnly)
		With rs
			If .EOF Then
				ReleaseRs(rs)
				GoTo ReturnLoad_Correct
			Else
				rf_見積番号.Text = CStr(No)

				'2016/04/07 ADD↓
				'            rf_税集計区分.Caption = ![税集計区分]
				'            [tx_税集計区分].Text = ![税集計区分]
				'            [rf_税集計区分名].Caption = cTokuisaki.Get税集計区分名(![税集計区分])
				'2016/04/07 ADD↑
				[rf_売上端数].Text = .Fields("売上端数").Value
				[rf_消費税端数].Text = .Fields("消費税端数").Value

				rf_合計金額.Text = .Fields("合計金額").Value
				rf_原価合計.Text = .Fields("原価合計").Value
				rf_原価率.Text = .Fields("原価率").Value
				rf_外税額.Text = .Fields("外税額").Value

				'UPGRADE_WARNING: オブジェクト HD_仕入日付 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				HD_仕入日付 = .Fields("仕入日付").Value
				'UPGRADE_WARNING: オブジェクト HD_売上日付 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				HD_売上日付 = .Fields("売上日付").Value

				rf_得意先別見積番号.Text = .Fields("得意先別見積番号").Value '2005/07/04 ADD

			End If
		End With

		ReleaseRs(rs)

		ModeIndicate(1)

ReturnLoad_Correct:
		HourGlass(False)
		Exit Sub
ReturnLoad_Err:
		MsgBox(Err.Number & " " & Err.Description)
		HourGlass(False)
	End Sub

	'--------------------------------
	'請求済みか？
	Private Function IsExist請求売上(ByRef ID As Integer) As Boolean
		'存在する場合FALSEで確認メッセージを出す
		Dim rs As ADODB.Recordset
		Dim sql As String

		On Error GoTo IsExist請求売上_Err
		'マウスポインターを砂時計にする
		HourGlass(True)

		sql = "SELECT  USH.請求日付,USH.請求書発行日付"
		sql = sql & "    FROM TD売上請求H AS USH"
		sql = sql & "    WHERE USH.見積番号 = " & Trim(CStr(ID))

		rs = OpenRs(sql, Cn, ADODB.CursorTypeEnum.adOpenForwardOnly, ADODB.LockTypeEnum.adLockReadOnly)

		With rs
			If .EOF Then
				IsExist請求売上 = False
			Else
				IsExist請求売上 = True
			End If
		End With
		ReleaseRs(rs)

		HourGlass(False)
		Exit Function
IsExist請求売上_Err:
		MsgBox(Err.Number & " " & Err.Description)
		HourGlass(False)
	End Function

	Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
		sb_Msg_Panel2.Text = DateTime.Now.ToString("yyyy/MM/dd")
		sb_Msg_Panel3.Text = DateTime.Now.ToString("HH:mm")
	End Sub

End Class