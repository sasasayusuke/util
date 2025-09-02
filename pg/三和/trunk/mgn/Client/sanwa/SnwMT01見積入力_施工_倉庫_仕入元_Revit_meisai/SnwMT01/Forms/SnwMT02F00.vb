Option Strict Off
Option Explicit On
Imports VB = Microsoft.VisualBasic
Imports Microsoft.VisualBasic.PowerPacks
Friend Class SnwMT02F00
	Inherits System.Windows.Forms.Form
	'
	'--------------------------------------------------------------------
	'  ユーザー名           株式会社 三和商研
	'  業務名               販売管理システム
	'  部門名               見積部門
	'  プログラム名         員数入力処理
	'  作成会社             テクノウェア株式会社
	'  作成日               2003/04/22
	'  作成者               oosawa
	'  引継日               2003/06/12
	'  引継者               kawamura
	'--------------------------------------------------------------------
	'   UPDATE
	'       2005/07/05  kawamura    [固定列]設定機能の追加。設定値はiniファイルに保持
	'       2005/10/14  kawamura    [U区分]項目の追加。Uを入力行は背景色を黄色にする
	'       2006/06/22  oosawa      取り込み時のHOLDを追加
	'       2006/06/22  oosawa      明示的インスタンスに変更
	'       2009/08/26  oosawa      EXCEL取り込みエラー時　行番号をだすように変更
	'       2010/01/05  oosawa      見積区分がコメントでない行のみ合計対象とする
	'       2010/01/06  oosawa      上記修正
	'       2010/06/16  oosawa      コメント（ACS）はチェック時にクリアするように変更
	'       2013/03/28  oosawa      ドラッグモードボタンの追加
	'       2013/04/09  oosawa      本発注から仮発注になった場合、発注情報を削除するように変更
	'       2013/07/19  oosawa      社内在庫参照・客先在庫参照をやめる
	'       2013/08/08  oosawa      売上・仕入更新済みの場合登録不可にする
	'       2013/12/26  oosawa      売上・仕入が両方更新済みならば登録不可にする（2013/08/08の修正）
	'       2015/02/04  oosawa      明細に伝票番号の欄を追加
	'       2015/08/28  oosawa      Wel要望により桁指定の税込端数計算を追加
	'       2015/09/29  oosawa      しまむら用明細納品日付の欄を追加
	'       2015/10/26  oosawa      名称を在庫管理する製品のみロックする
	'       2015/11/03  oosawa      Wel化粧品メーカー用に在庫管理除外区分を新設
	'       2015/11/13  oosawa      2015/10/26修正分のロックをサイズのみにする
	'       2015/12/01  oosawa      チェックボックスのチェンジイベントが発生してしまうのを除外
	'       2016/06/23  oosawa      U区分でRGBでの色付け追加
	'       2016/06/22  oosawa      クレーム明細区分（1:クレーム）
	'                               作業区分CD（1:施工 2:コール 3:内装）を追加
	'       2017/01/17  oosawa      Welの1%を変動できるように変更
	'       2017/02/10  oosawa      製品でのロックの不具合修正
	'                               社内伝の場合、仕入先3150：三和倉庫は指定できないようにする
	'                               社内在庫数を取り込まないように変更
	'       2017/03/10  oosawa      仕入業者を追加
	'                               仕入先を出荷元として処理する
	'                               表示を「仕入先」から「出荷元」に変更
	'       2017/04/07  oosawa      新規作成時のロック情報生成
	'       2017/06/27  oosawa      数量表示で小数点カットの不具合
	'       2017/07/03  oosawa      R:赤･B:青･H:灰のカラーを追加
	'       2017/10/05  oosawa      員数取込・ＳＭ取込時、行をつぶさないように修正
	'       2017/12/20  oosawa      ENTERKEYでの移動方向変更
	'       2018/04/10  oosawa      レビットの集計データ取込作成F15
	'       2018/04/11  oosawa      保存時に再表示するように変更（見積明細連番の為）
	'       2018/06/19  oosawa      コメント行の書き換え防止に仕入済CNTを新設
	'       2018/07/31  oosawa      社内伝に3150のチェックをストアドでも行なう
	'       2019/12/12  oosawa      レビット用追番Rを追加
	'                               レビット取込変更
	'       2019/12/24  oosawa      消費税の計算がうまくいっていないのでチェック時に再計算するように変更
	'       2020/03/31  oosawa      ↑それでも税がZEROになる。ResumuNextはずす
	'
	'途中↓
	'--------------------------------------------------------------------
	'
	'--Formで使用する変数--
	'コントロールの戻りを制御
	Dim PreviousControl As System.Windows.Forms.Control
	'各項目でEnterKeyが押されたかのﾁｪｯｸﾌﾗｸﾞ
	Dim ReturnF As Boolean
	'FormLoadフラグ
	Dim Loaded As Boolean
	'UPGRADE_NOTE: Closed は Closed_Renamed にアップグレードされました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"' をクリックしてください。
	Dim Closed_Renamed As Boolean
	Dim SpreadErr As Boolean
	'ボタン２重起動防止フラグ
	Dim CLK2F As Boolean '２重実行制御用
	
	'起動時のフォームサイズHold用
	Dim MeWidth As Integer
	Dim MeHeight As Integer
	Dim MeHeightLimit As Integer
	Dim LvHeightLimit As Integer
	Dim WkHeight As Integer 'リストビューからファンクションキー間の高さHold用
	'処理モード用   1:登録 2:修正
	Dim ModeF As Short
	
	'2015/09/29 DEL↓publicへ移動
	'''''''ｽﾌﾟﾚｯﾄﾞｼｰﾄの定数
	''''''Private Const Col展開 As Integer = 1
	''''''Private Const Col見積区分 As Integer = 2
	''''''Private Const Col他社伝票番号 As Integer = 3
	''''''Private Const Col他社納品日付 As Integer = 4
	''''''Private Const ColSP区分 As Integer = 5
	''''''Private Const ColPC区分 As Integer = 6
	''''''Private Const Col製品NO As Integer = 7
	''''''Private Const Col仕様NO As Integer = 8
	''''''Private Const Colベース色 As Integer = 9
	''''''Private Const Col名称 As Integer = 10
	''''''Private Const ColW As Integer = 11
	''''''Private Const ColD As Integer = 12
	''''''Private Const ColH As Integer = 13
	''''''Private Const ColD1 As Integer = 14
	''''''Private Const ColD2 As Integer = 15
	''''''Private Const ColH1 As Integer = 16
	''''''Private Const ColH2 As Integer = 17
	''''''Private Const Colエラー内容 As Integer = 18
	''''''Private Const Col定価 As Integer = 19
	''''''Private Const ColU区分 As Integer = 20          '2005/10/14.ADD  以降の項目を１プラス
	''''''Private Const Col原価 As Integer = 21
	''''''Private Const Col仕入率 As Integer = 22
	''''''Private Const Col売価 As Integer = 23
	''''''Private Const Col売上率 As Integer = 24
	''''''Private Const ColM As Integer = 25
	''''''Private Const Col見積数量 As Integer = 26
	''''''Private Const Col単位 As Integer = 27
	''''''Private Const Col金額 As Integer = 28
	''''''Private Const Col仕入金額 As Integer = 29
	''''''Private Const Col売上税区分 As Integer = 30
	''''''Private Const Col消費税額 As Integer = 31
	''''''Private Const Col仕入先CD As Integer = 32
	''''''Private Const Col仕入先名 As Integer = 33
	''''''Private Const Col送り先CD As Integer = 34
	''''''Private Const Col送り先名 As Integer = 35
	''''''Private Const Col社内在庫 As Integer = 36
	''''''Private Const Col社内在庫参照 As Integer = 37
	''''''Private Const Col客先在庫 As Integer = 38
	''''''Private Const Col客先在庫参照 As Integer = 39
	''''''Private Const Col転用 As Integer = 40
	''''''Private Const Col発注調整数 As Integer = 41 '2014/09/09 ADD
	''''''Private Const Col発注数 As Integer = 42
	''''''Private Const Col数量合計 As Integer = 43
	'''''''F03にも持つので修正時はそちらも修正！
	''''''Private Const Col仕分数1 As Integer = 44
	''''''Private Const Col製品区分 As Integer = 74
	''''''Private Const Col見積明細連番 As Integer = 75
	''''''Private Const Col仕入済数 As Integer = 76 '2014/09/09 ADD
	''''''Private Const Col売上済CNT As Integer = 77 '2014/09/09 ADD
	''''''
	'''''''''Private Const SiwakeCol As Integer = 33      '仕分数１のCol
	'''''''''Private Const TeikaCol As Integer = 14       '定価のCol
	'''''''''Private Const SiireCol As Integer = 26       '仕入先のCol
	
	'ｽﾌﾟﾚｯﾄﾞのクラス
	'UPGRADE_NOTE: clsSPD は clsSPD_Renamed にアップグレードされました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"' をクリックしてください。
	Dim clsSPD_Renamed As clsSPD
	'保存ﾁｪｯｸ用ワーク
	Dim WkTBLS() As Object
	
	'変数
	Dim HoldPc As Object
	Dim HoldSei As Object
	Dim HoldSiy As Object
	Dim HoldCD As Object
	Dim HCol As Short
	Dim HRow As Short
	
	'製品情報選択画面チェックデータの受け取り
	Public SelData As Object
	'見積選択画面情報
	Dim pParentForm As System.Windows.Forms.Form
	Dim pMituNo As Integer
	'Excel取込データの受け取り
	Public SiwaNo As Short
	Public SiwaGyo As Integer
	Public TempData As Object 'F04で取り込んだEXCEL情報
	Public InXLSPath As String '取込のエクセルパス     '2004/02/5  ADD
	Public REVIT_SIWANO As Integer 'REVIT取込時指定の仕分番号  '2019/08/03 ADD
	
	'初期登録日HOLD
	Dim InitDate As Date
	Dim gZEI As Decimal
	
	
	'''HOLD
	''Dim HTEMPNM As String
	
	Dim wNUMBER As Integer
	Dim TMPTABLE As String
	Dim TMPTABLE_Check As String
	
	'エラー行番号
	Dim gErrNo As String '2005/03/26 ADD
	
	'クラス
	Private cSiiresaki As clsSiiresaki
	Private cHaisosaki As clsHaisosaki
	'Private cDspSyaZaiko As clsDspSyaZaiko  '社内在庫クラス '2013/07/19 DEL
	'Private cDspKyakuZaiko As clsDspKyakuZaiko  '客先在庫クラス '2013/07/19 DEL
	Private cSirTanka As clsSirTanka '2015/07/20 ADD
	Private cUriSirTanka As clsUriSirTanka '2015/09/28 ADD
	Private cLoginControl As clsLoginControl '2018/03/26 ADD
	Private cMitsumoriM As clsMitsumoriM '2018/05/03 ADD
	
	'仕入済数が全てゼロの場合False
	'一つでもあればTrue
	Private shiirezumiF As Boolean '2014/09/09 ADD '売上と共用する
	
	Private Sub cb員数取込_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cb員数取込.Click
		Dim fpSpd As Object
		Dim F12 As SnwMT02F12
		
		F12 = New SnwMT02F12
		'UPGRADE_WARNING: オブジェクト fpSpd.SetFocus の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		fpSpd.SetFocus()
		
		'Excel入力処理
		With F12
			.ResParentForm = Me
			'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.ActRow = fpSpd.ActiveRow
			
			Me.Enabled = False
			VB6.ShowForm(F12, VB6.FormShowConstants.Modeless, Me)
		End With
		'UPGRADE_NOTE: オブジェクト F12 をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
		F12 = Nothing
	End Sub
	
	'2015/11/10 ADD↓
	Private Sub cbしまむら消耗品取込_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cbしまむら消耗品取込.Click
		Dim fpSpd As Object
		Dim F14 As SnwMT02F14
		
		F14 = New SnwMT02F14
		'UPGRADE_WARNING: オブジェクト fpSpd.SetFocus の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		fpSpd.SetFocus()
		
		'Excel入力処理
		With F14
			.ResParentForm = Me
			'UPGRADE_WARNING: オブジェクト Me.fpSpd の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.fpSpd = Me.fpSpd
			'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.ActRow = fpSpd.ActiveRow
			
			Me.Enabled = False
			VB6.ShowForm(F14, VB6.FormShowConstants.Modeless, Me)
		End With
		'UPGRADE_NOTE: オブジェクト F14 をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
		F14 = Nothing
	End Sub
	'2015/11/10 ADD↑
	
	'2013/03/28 ADD↓
	'UPGRADE_WARNING: イベント ck_DragMode.CheckStateChanged は、フォームが初期化されたときに発生します。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"' をクリックしてください。
	Private Sub ck_DragMode_CheckStateChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles ck_DragMode.CheckStateChanged
		Dim fpSpd As Object
		'ドラッグアンドドロップができるモードに切り替える
		With fpSpd
			If [ck_DragMode].CheckState = 0 Then
				'UPGRADE_WARNING: オブジェクト fpSpd.EditModePermanent の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.EditModePermanent = True
				'UPGRADE_WARNING: オブジェクト fpSpd.SelectBlockOptions の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.SelectBlockOptions = 0
				''''            .OperationMode = 0
			Else
				'UPGRADE_WARNING: オブジェクト fpSpd.EditModePermanent の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.EditModePermanent = False
				'UPGRADE_WARNING: オブジェクト fpSpd.SelectBlockOptions の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.SelectBlockOptions = 2 '行選択モード
				''''            .OperationMode = 2
				
			End If
			'UPGRADE_WARNING: オブジェクト fpSpd.SetFocus の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.SetFocus()
			'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト fpSpd.ActiveCol の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Col = .ActiveCol
			'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Row = .ActiveRow
			'UPGRADE_WARNING: オブジェクト fpSpd.Action の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Action = FPSpreadADO.ActionConstants.ActionActiveCell
		End With
	End Sub
	'2013/03/28 ADD↑
	
	'2017/12/20 ADD↓
	'UPGRADE_WARNING: イベント ck_MoveMode.CheckStateChanged は、フォームが初期化されたときに発生します。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"' をクリックしてください。
	Private Sub ck_MoveMode_CheckStateChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles ck_MoveMode.CheckStateChanged
		Dim fpSpd As Object
		'ENTERKEYで右移動・下移動を切り替える
		With fpSpd
			If [ck_MoveMode].CheckState = 0 Then
				'UPGRADE_WARNING: オブジェクト fpSpd.EditEnterAction の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.EditEnterAction = FPSpreadADO.EditEnterActionConstants.EditEnterActionNext
				[ck_MoveMode].Text = "→"
			Else
				'UPGRADE_WARNING: オブジェクト fpSpd.EditEnterAction の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.EditEnterAction = FPSpreadADO.EditEnterActionConstants.EditEnterActionDown
				[ck_MoveMode].Text = "↓"
			End If
			'UPGRADE_WARNING: オブジェクト fpSpd.SetFocus の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.SetFocus()
			'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト fpSpd.ActiveCol の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Col = .ActiveCol
			'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Row = .ActiveRow
			'UPGRADE_WARNING: オブジェクト fpSpd.Action の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Action = FPSpreadADO.ActionConstants.ActionActiveCell
		End With
	End Sub
	'2017/12/20 ADD↑
	
	'2018/04/10 ADD↓
	Private Sub cbレビット取込_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cbレビット取込.Click
		Dim fpSpd As Object
		Dim F15 As SnwMT02F15
		
		F15 = New SnwMT02F15
		'UPGRADE_WARNING: オブジェクト fpSpd.SetFocus の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		fpSpd.SetFocus()
		
		'Excel入力処理
		With F15
			.ResParentForm = Me
			'        Set .fpSpd = Me.fpSpd
			
			Me.Enabled = False
			VB6.ShowForm(F15, VB6.FormShowConstants.Modeless, Me)
		End With
		'UPGRADE_NOTE: オブジェクト F15 をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
		F15 = Nothing
	End Sub
	'2018/04/10 ADD↑
	
	Private Sub Command1_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles Command1.Click
		Dim fpSpd As Object
		
		With fpSpd
			'UPGRADE_WARNING: オブジェクト fpSpd.EditModePermanent の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.EditModePermanent = False
			
			'UPGRADE_WARNING: オブジェクト fpSpd.BlockMode の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.BlockMode = True
			'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト fpSpd.SelBlockCol の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Col = .SelBlockCol
			'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト fpSpd.SelBlockRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Row = .SelBlockRow
			'UPGRADE_WARNING: オブジェクト fpSpd.Col2 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト fpSpd.SelBlockCol2 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Col2 = .SelBlockCol2
			'UPGRADE_WARNING: オブジェクト fpSpd.Row2 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト fpSpd.SelBlockRow2 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Row2 = .SelBlockRow2
			'UPGRADE_WARNING: オブジェクト fpSpd.Action の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Action = FPSpreadADO.ActionConstants.ActionClearText
			'UPGRADE_WARNING: オブジェクト fpSpd.BlockMode の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.BlockMode = False
		End With
	End Sub
	
	Private Sub dispFormDate()
		Dim fpSpd As Object
		Dim F16 As SnwMT02F16
		Dim wMaxRow As Integer
		
		'UPGRADE_WARNING: オブジェクト fpSpd.DataRowCnt の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		If fpSpd.DataRowCnt = 0 Then
			CriticalAlarm("明細がありません。")
			'UPGRADE_WARNING: オブジェクト fpSpd.SetFocus の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			fpSpd.SetFocus()
			Exit Sub
		End If
		
		Dim WkTBLS() As Object
		With fpSpd
			'UPGRADE_WARNING: オブジェクト fpSpd.DataRowCnt の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			If .DataRowCnt = 0 Then
				CriticalAlarm("明細がありません。")
				Exit Sub
			End If
			
			'UPGRADE_WARNING: 配列 WkTBLS の下限が 1,1 から 0,0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="0F1C9BE1-AF9D-476E-83B1-17D43BECFF20"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト fpSpd.MaxCols の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト fpSpd.DataRowCnt の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			ReDim WkTBLS(.DataRowCnt, .MaxCols)
			'        Dim WkTBLSs(1 To .DataRowCnt, 1 To .MaxCols)
			'スプレッド上のデータを配列に
			'UPGRADE_WARNING: オブジェクト fpSpd.GetArray の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			Call .GetArray(1, 1, WkTBLS)
			
			'未入力最終明細サーチ
			For wMaxRow = UBound(WkTBLS, 1) To LBound(WkTBLS, 1) Step -1
				'UPGRADE_WARNING: オブジェクト SpcToNull(WkTBLS(wMaxRow, Col発注数), 0) の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				If Not ((IsCheckNull(WkTBLS(wMaxRow, ColPC区分)) = True Or IsCheckNull(WkTBLS(wMaxRow, Col製品NO)) = True Or IsCheckNull(WkTBLS(wMaxRow, Col仕様NO)) = True Or IsCheckNull(WkTBLS(wMaxRow, Col名称)) = True) And SpcToNull(WkTBLS(wMaxRow, Col発注数), 0) = 0) Then Exit For '発注数
				''''        If IsCheckNull(WkTBLS(wMaxRow, 24)) = False Then Exit For               '仕入先CD
				''''        If IsCheckNull(WkTBLS(wMaxRow, 25)) = False Then Exit For               '配送先CD
			Next 
		End With
		
		If wMaxRow = 0 Then
			CriticalAlarm("明細がありません。")
			'UPGRADE_WARNING: オブジェクト fpSpd.SetFocus の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			fpSpd.SetFocus()
			Exit Sub
		End If
		
		F16 = New SnwMT02F16
		
		'入出庫日入力処理
		With F16
			.ResParentForm = Me
			
			'UPGRADE_WARNING: オブジェクト fpSpd.ActiveCol の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			If fpSpd.ActiveCol = Col入庫日 Then '2020/09/16 ADD
				.Col入出庫日 = "入庫日"
			Else
				.Col入出庫日 = "出庫日"
			End If
			
			VB6.ShowForm(F16, VB6.FormShowConstants.Modal, Me)
			'UPGRADE_WARNING: オブジェクト fpSpd.SetFocus の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			fpSpd.SetFocus()
			
		End With
		'UPGRADE_NOTE: オブジェクト F16 をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
		F16 = Nothing
		
	End Sub
	
	''''Dim clsWheel As clsWheel
	
	Private Sub SnwMT02F00_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
		Dim fpSpd As Object
		Loaded = False
		
		'SPREAD設定
		clsSPD_Renamed = New clsSPD
		clsSPD_Renamed.CtlSpd = fpSpd
		
		'クラス生成
		cSiiresaki = New clsSiiresaki
		cHaisosaki = New clsHaisosaki
		'    Set cDspSyaZaiko = New clsDspSyaZaiko  '2009/09/24 ADD '2013/07/19 DEL
		'    Set cDspKyakuZaiko = New clsDspKyakuZaiko  '2009/09/24 ADD '2013/07/19 DEL
		cSirTanka = New clsSirTanka '2015/07/20 ADD
		cUriSirTanka = New clsUriSirTanka '2015/09/28 ADD
		'保存制御
		'2018/03/26 ADD↓
		cLoginControl = New clsLoginControl
		cLoginControl.LoginName = GetUName()
		cLoginControl.AppTitle = My.Application.Info.Title
		cLoginControl.GetbyID()
		'2018/03/26 ADD↑
		cMitsumoriM = New clsMitsumoriM '2018/05/03 ADD
		
		'リサイズ用初期値設定（幅・高さ）
		MeWidth = VB6.PixelsToTwipsX(Me.Width)
		'UPGRADE_WARNING: オブジェクト fpSpd.Height の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		MeHeight = VB6.PixelsToTwipsY(Me.Height) - fpSpd.Height 'ｽﾌﾟﾚｯﾄﾞ分を抜いた高さ
		''    LvHeightLimit = fpSpd.Font.Size * (34 + 24)     '一行分?
		'UPGRADE_WARNING: オブジェクト fpSpd.Font の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		LvHeightLimit = fpSpd.Font.Size * ((34 + 24) * 2) '一行分?
		MeHeightLimit = MeHeight + LvHeightLimit '最小の高さ
		
		'2014/07/10 ADD
		'中国用に金額を小数点2桁にする。
		If COUNTRY_CODE = "CN" Then
			With fpSpd
				'UPGRADE_WARNING: オブジェクト fpSpd.BlockMode の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.BlockMode = True
				'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.Col = Col金額
				'UPGRADE_WARNING: オブジェクト fpSpd.Col2 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.Col2 = Col仕入金額
				'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.Row = -1
				
				'UPGRADE_WARNING: オブジェクト fpSpd.TypeFloatMin の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.TypeFloatMin = "-999999999.99"
				'UPGRADE_WARNING: オブジェクト fpSpd.TypeFloatMax の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.TypeFloatMax = "999999999.99"
				'UPGRADE_WARNING: オブジェクト fpSpd.TypeFloatDecimalPlaces の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.TypeFloatDecimalPlaces = 2
				
				'UPGRADE_WARNING: オブジェクト fpSpd.BlockMode の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.BlockMode = False
			End With
		End If
		
		
		'選択情報を表示
		Call FormInitialize()
		'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト SelData の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		SelData = System.DBNull.Value
		Call ValueInitialize()
		Call Download(pMituNo)
		
		'仕分レベル読み込み
		Call SiwakeDL(pMituNo)
		
		With fpSpd
			'初期位置
			'''        .Col = 3
			'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Col = Col見積区分 '2003/10/29
			'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Row = 1
			'UPGRADE_WARNING: オブジェクト fpSpd.Action の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Action = FPSpreadADO.ActionConstants.ActionActiveCell
		End With
		
		'見積情報を初期表示
		op_Disp(5).Checked = True
		''    Call op_Disp_Click(5)
		''    Call SpdRowDisp
		
		'Wel%情報セット
		'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		[tx_Welパー算出].Text = NullToZero(GetIni("Disp", "WelPercent", INIFile)) '2017/01/17 ADD
		
		
		If HD_見積確定区分 = 0 Then
			'見積未確定
			[rf_売価確定].Visible = False
		Else
			'見積確定
			[rf_売価確定].Visible = True
		End If
		
		Loaded = True
		Closed_Renamed = False
	End Sub
	
	'UPGRADE_WARNING: イベント SnwMT02F00.Resize は、フォームが初期化されたときに発生します。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"' をクリックしてください。
	Private Sub SnwMT02F00_Resize(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Resize
		Dim fpSpd As Object
		If Me.WindowState <> System.Windows.Forms.FormWindowState.Minimized Then
			'見出しライン制御
			'''        ln_over(0).X2 = Me.Width
			'''        ln_over(1).X2 = Me.Width
			'フォーム最小（幅）制御
			If VB6.PixelsToTwipsX(Me.Width) < MeWidth Then
				Me.Width = VB6.TwipsToPixelsX(MeWidth)
			End If
			''        fpSpd.Width = Me.ScaleWidth - (fpSpd.Left * 2)
			'フォーム最小（高さ）制御・リストビューの高さをフォームの高さに比例
			If VB6.PixelsToTwipsY(Me.Height) < MeHeight Then
				Me.Height = VB6.TwipsToPixelsY(MeHeightLimit)
			End If
			'UPGRADE_WARNING: オブジェクト fpSpd.Height の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			fpSpd.Height = VB6.PixelsToTwipsY(Me.Height) - MeHeight
			'UPGRADE_WARNING: オブジェクト fpSpd.Width の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト fpSpd.Left の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			fpSpd.Width = VB6.PixelsToTwipsX(Me.ClientRectangle.Width) - (fpSpd.Left * 2) '2008/06/10 ADD
			
			'検索項目移動
			'UPGRADE_WARNING: オブジェクト fpSpd.Height の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト fpSpd.Top の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			lb_項目(4).Top = VB6.TwipsToPixelsY(fpSpd.Top + fpSpd.Height)
			'UPGRADE_WARNING: オブジェクト fpSpd.Height の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト fpSpd.Top の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[tx_製品NO].Top = VB6.TwipsToPixelsY(fpSpd.Top + fpSpd.Height)
			'UPGRADE_WARNING: オブジェクト fpSpd.Height の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト fpSpd.Top の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			[cb検索].Top = VB6.TwipsToPixelsY(fpSpd.Top + fpSpd.Height)
		End If
		
		'表示切替
		'UPGRADE_WARNING: オブジェクト fpSpd.Height の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト fpSpd.Top の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		fr_Disp.Top = VB6.TwipsToPixelsY(fpSpd.Top + fpSpd.Height)
		
		
	End Sub
	
	Private Sub SnwMT02F00_KeyDown(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
		Dim KeyCode As Short = eventArgs.KeyCode
		Dim Shift As Short = eventArgs.KeyData \ &H10000
		Dim ctl As System.Windows.Forms.Control
		
		On Error GoTo Form_KeyDown_Err
		Select Case KeyCode
			Case System.Windows.Forms.Keys.Escape
				KeyCode = 0
				Exit Sub
			Case System.Windows.Forms.Keys.F1 To System.Windows.Forms.Keys.F12
				On Error Resume Next
				ctl = CType(Me.Controls("cbFunc"), Object)(KeyCode - System.Windows.Forms.Keys.F1 + 1)
				If Err.Number = 0 Then
					If ctl.Text <> vbNullString Then
						If ctl.Enabled = True Then
							ctl.Focus()
							If Err.Number = 0 Then
								SendReturnKey()
							End If
						End If
					End If
				End If
				'UPGRADE_NOTE: オブジェクト ctl をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
				ctl = Nothing
				KeyCode = 0
				On Error GoTo 0
				Exit Sub
		End Select
		
		Exit Sub
Form_KeyDown_Err: 
		MsgBox(Err.Number & " " & Err.Description)
	End Sub
	
	Private Sub SnwMT02F00_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress
		Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
		'ビープ音消去用
		Select Case KeyAscii
			Case System.Windows.Forms.Keys.Return
				KeyAscii = 0
		End Select
		eventArgs.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			eventArgs.Handled = True
		End If
	End Sub
	
	Private Sub SnwMT02F00_FormClosing(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
		Dim Cancel As Boolean = eventArgs.Cancel
		Dim UnloadMode As System.Windows.Forms.CloseReason = eventArgs.CloseReason
		'''''    If Loaded Then
		'''''        If Closed Then
		''''''            pParentForm.ClearMode (True)
		'''''            pParentForm.EndMsg = False
		'''''            Unload pParentForm
		''''''''            pParentForm.Visible = True
		'''''
		'''''            Exit Sub
		'''''        End If
		'''''        If vbYes = NoYes("現在の処理を終了します。") Then
		''''''            pParentForm.ClearMode (True)
		'''''            pParentForm.Visible = True
		'''''        Else
		'''''            PreviousControl.SetFocus
		'''''            Cancel = True
		'''''        End If
		'''''    End If
		If Loaded Then
			If Closed_Renamed Then '閉じるボタンの場合
				If MsgBoxResult.Yes = NoYes("現在の処理を終了します。" & vbCrLf & "保存していないデータは破棄されます。") Then
					'UPGRADE_ISSUE: Control EndMsg は、汎用名前空間 Form 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
					pParentForm.EndMsg = False
					pParentForm.Close()
					'                Set cDspSyaZaiko = Nothing  '2009/09/24 ADD '2013/07/19 DEL
					'                Set cDspKyakuZaiko = Nothing  '2009/09/24 ADD '2013/07/19 DEL
					'UPGRADE_NOTE: オブジェクト clsSPD_Renamed をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
					clsSPD_Renamed = Nothing
					
					''''''test---------------------
					'''''                pParentForm.Visible = True
					'''''                If rf_見積番号.Caption <> vbNullString Then
					'''''                    Call pParentForm.ReturnLoad(CLng(rf_見積番号.Caption))  '2003/11/05.ADD
					'''''                End If
					''''''                Set cDspSyaZaiko = Nothing  '2009/09/24 ADD '2013/07/19 DEL
					''''''                Set cDspKyakuZaiko = Nothing  '2009/09/24 ADD '2013/07/19 DEL
					'''''                Set clsSPD = Nothing
					''''''test---------------------
					
					
					
					Exit Sub
				Else
					Closed_Renamed = False
					PreviousControl.Focus()
					Cancel = True
				End If
			Else
				If MsgBoxResult.Yes = NoYes("前画面に戻ります。" & vbCrLf & "保存していないデータは破棄されます。") Then
					pParentForm.Visible = True
					If rf_見積番号.Text <> vbNullString Then
						'UPGRADE_ISSUE: Control ReturnLoad は、汎用名前空間 Form 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
						Call pParentForm.ReturnLoad(CInt(rf_見積番号.Text)) '2003/11/05.ADD
					End If
					'                Set cDspSyaZaiko = Nothing  '2009/09/24 ADD '2013/07/19 DEL
					'                Set cDspKyakuZaiko = Nothing  '2009/09/24 ADD '2013/07/19 DEL
					'UPGRADE_NOTE: オブジェクト clsSPD_Renamed をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
					clsSPD_Renamed = Nothing
				Else
					PreviousControl.Focus()
					Cancel = True
				End If
			End If
		End If
		eventArgs.Cancel = Cancel
	End Sub
	
	Private Sub FormInitialize()
		'    Call SetupBlank
		'    Call HldBlank
		'スプレッドの設定をする。
		Call fpSpd_Initialize()
		ModeIndicate(0)
	End Sub
	
	Private Sub ValueInitialize()
		SiwaNo = 0
		SiwaGyo = 0
		'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト TempData の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		TempData = System.DBNull.Value
		
		shiirezumiF = False '2014/10/9 ADD
	End Sub
	
	Private Sub ModeIndicate(ByVal Mode As Object)
		'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
		If IsDbNull(Mode) Then
			[rf_処理区分].Text = vbNullString
			[rf_処理区分].BackColor = System.Drawing.ColorTranslator.FromOle(&HE0E0E0)
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
		End If
	End Sub
	
	'UPGRADE_NOTE: Name は Name_Renamed にアップグレードされました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"' をクリックしてください。
	Private Sub SetUpFuncs(ByRef Name_Renamed As String)
		'ボタン名の変更
		Select Case Name_Renamed
			'''        Case "tx_シート名称"
			'''            cbFunc(1).Caption = ""
			'''            cbFunc(2).Caption = ""
			'''            cbFunc(3).Caption = ""
			'''            cbFunc(4).Caption = ""
			'''            cbFunc(5).Caption = ""
			'''            cbFunc(6).Caption = ""
			'''            cbFunc(7).Caption = ""
			'''            cbFunc(8).Caption = ""
			'''            cbFunc(9).Caption = ""
			'''''''''            cbFunc(10).Caption = ""   '2003/11/05.DEL
			Case "展開", "PC区分", "製品NO", "見積区分" '', "fpSpd"
				[cbFunc](1).Text = "行挿入"
				[cbFunc](2).Text = "行削除"
				[cbFunc](3).Text = "行ｺﾋﾟｰ"
				[cbFunc](4).Text = "行複写"
				[cbFunc](5).Text = "上へ"
				[cbFunc](6).Text = "下へ"
				[cbFunc](7).Text = "表示項"
				'''            cbFunc(8).Caption = ""
				If shiirezumiF = True Then '2014/09/09 ADD
					'仕入済数が全てゼロでなければ
					[cbFunc](8).Text = ""
				Else
					'仕入済数が全てゼロ
					[cbFunc](8).Text = "数削除" '2008/05/22 ADD
				End If
				''            If ModeF = 1 Then       '追加
				''                cbFunc(9).Caption = ""
				''            Else
				''                cbFunc(9).Caption = "削除"
				''            End If
				[cbFunc](9).Text = ""
				'''''            cbFunc(10).Caption = ""    '2003/11/05.DEL
			Case "原価"
				[cbFunc](1).Text = ""
				[cbFunc](2).Text = ""
				[cbFunc](3).Text = ""
				[cbFunc](4).Text = ""
				[cbFunc](5).Text = "原割設" '2008/01/23 ADD
				[cbFunc](6).Text = ""
				[cbFunc](7).Text = "表示項"
				[cbFunc](8).Text = "単価参"
				[cbFunc](9).Text = ""
				'''''            cbFunc(10).Caption = ""    '2003/11/05.DEL
			Case "売価"
				[cbFunc](1).Text = ""
				[cbFunc](2).Text = ""
				[cbFunc](3).Text = ""
				[cbFunc](4).Text = ""
				[cbFunc](5).Text = "原割設" '2008/01/23 ADD
				[cbFunc](6).Text = "掛率"
				[cbFunc](7).Text = "表示項"
				[cbFunc](8).Text = "単価参"
				[cbFunc](9).Text = ""
				'''''            cbFunc(10).Caption = ""    '2003/11/05.DEL
			Case "社内在庫数"
				[cbFunc](1).Text = ""
				[cbFunc](2).Text = ""
				[cbFunc](3).Text = ""
				[cbFunc](4).Text = ""
				[cbFunc](5).Text = ""
				[cbFunc](6).Text = ""
				[cbFunc](7).Text = "表示項"
				[cbFunc](8).Text = "社在参"
				[cbFunc](9).Text = ""
				'''''            cbFunc(10).Caption = ""    '2003/11/05.DEL
			Case "客先在庫数"
				[cbFunc](1).Text = ""
				[cbFunc](2).Text = ""
				[cbFunc](3).Text = ""
				[cbFunc](4).Text = ""
				[cbFunc](5).Text = ""
				[cbFunc](6).Text = ""
				[cbFunc](7).Text = "表示項"
				[cbFunc](8).Text = "客在参"
				[cbFunc](9).Text = ""
				'''''            cbFunc(10).Caption = ""    '2003/11/05.DEL
				'2017/03/30 ADD↓
			Case "仕分数"
				[cbFunc](1).Text = ""
				[cbFunc](2).Text = ""
				[cbFunc](3).Text = ""
				[cbFunc](4).Text = ""
				'            cbFunc(5).Caption = "左へ"
				'            cbFunc(6).Caption = "右へ"
				[cbFunc](7).Text = "表示項"
				[cbFunc](8).Text = ""
				[cbFunc](9).Text = ""
				'2017/03/30 ADD↑
				'2018/05/03 ADD↓
			Case "名称"
				[cbFunc](1).Text = ""
				[cbFunc](2).Text = ""
				[cbFunc](3).Text = ""
				[cbFunc](4).Text = ""
				[cbFunc](5).Text = "明細検"
				[cbFunc](6).Text = ""
				[cbFunc](7).Text = "表示項"
				[cbFunc](8).Text = ""
				''            If ModeF = 1 Then       '追加
				''                cbFunc(9).Caption = ""
				''            Else
				''                cbFunc(9).Caption = "削除"
				''            End If
				[cbFunc](9).Text = ""
				'''''            cbFunc(10).Caption = ""    '2003/11/05.DEL
				'2018/05/03 ADD↑
				'2018/05/03 ADD↓
			Case "入庫日", "出庫日" '2020/09/16 ADD
				[cbFunc](1).Text = ""
				[cbFunc](2).Text = ""
				[cbFunc](3).Text = ""
				[cbFunc](4).Text = ""
				[cbFunc](5).Text = "日設定"
				[cbFunc](6).Text = ""
				[cbFunc](7).Text = "表示項"
				[cbFunc](8).Text = ""
				''            If ModeF = 1 Then       '追加
				''                cbFunc(9).Caption = ""
				''            Else
				''                cbFunc(9).Caption = "削除"
				''            End If
				[cbFunc](9).Text = ""
				'''''            cbFunc(10).Caption = ""    '2003/11/05.DEL
				'2018/05/03 ADD↑
			Case Else
				[cbFunc](1).Text = ""
				[cbFunc](2).Text = ""
				[cbFunc](3).Text = ""
				[cbFunc](4).Text = ""
				[cbFunc](5).Text = ""
				[cbFunc](6).Text = ""
				[cbFunc](7).Text = "表示項"
				[cbFunc](8).Text = ""
				''            If ModeF = 1 Then       '追加
				''                cbFunc(9).Caption = ""
				''            Else
				''                cbFunc(9).Caption = "削除"
				''            End If
				[cbFunc](9).Text = ""
				'''''            cbFunc(10).Caption = ""    '2003/11/05.DEL
		End Select
	End Sub
	
	Private Sub cbFunc_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cbFunc.Click
		Dim Index As Short = cbFunc.GetIndex(eventSender)
		Dim fpSpd As Object
		Dim getdata As Object
		Dim check As Boolean
		Dim wCol As Short
		Dim wRow As Short
		Dim wGetKBN As Short
		Dim wPCKUBN As String
		Dim wSEIHNO As String
		Dim wSIYONO As String
		Dim wSEIHNM As String
		Dim wSYAZAI As Decimal
		Dim wKYAZAI As Decimal
		
		Dim wSirCD As String '2009/07/28 ADD
		
		If [cbFunc](Index).Text = vbNullString Then
			PreviousControl.Focus()
			Exit Sub
		End If
		'ボタンを押されたのが２回目ならば抜ける
		If CLK2F = True Then
			Exit Sub
		End If
		'ボタン２重起動防止フラグのセット
		CLK2F = True
		[cbFunc](Index).TabStop = True
		Dim frm As SnwMT02F13
		Dim F05 As SnwMT02F05
		Dim F08 As SnwMT02F08
		Dim F09 As SnwMT02F09 '2009/08/10 ADD
		Select Case Index
			Case 1
				'行の挿入
				If clsSPD_Renamed.LineInsert = -1 Then
					System.Windows.Forms.Application.DoEvents()
					Inform("これ以上、挿入できません。")
				Else
					[cbFunc](11).Text = "ﾁｪｯｸ"
				End If
			Case 2
				''            If CCur(NullToZero(clsSPD.GetTextEX(Col仕入済数, fpSpd.ActiveRow))) <> 0 Then '2014/09/09 ADD
				'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				If CDec(NullToZero(clsSPD_Renamed.GetTextEX(Col仕入済CNT, fpSpd.ActiveRow))) <> 0 Then '2014/09/09 ADD'2018/06/19 ADD
					CriticalAlarm("仕入済みです。")
					'UPGRADE_WARNING: オブジェクト fpSpd.SetFocus の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					fpSpd.SetFocus()
				Else
					'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					If CDec(NullToZero(clsSPD_Renamed.GetTextEX(Col売上済CNT, fpSpd.ActiveRow))) <> 0 Then '2014/09/09 ADD
						CriticalAlarm("売上済みです。")
						'UPGRADE_WARNING: オブジェクト fpSpd.SetFocus の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						fpSpd.SetFocus()
					Else
						'行の削除
						Call clsSPD_Renamed.LineDelete()
						With fpSpd
							'UPGRADE_WARNING: オブジェクト fpSpd.SetFocus の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.SetFocus()
							'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							'UPGRADE_WARNING: オブジェクト fpSpd.ActiveCol の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.Col = .ActiveCol
							'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.Row = .ActiveRow
							'UPGRADE_WARNING: オブジェクト fpSpd.Action の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.Action = FPSpreadADO.ActionConstants.ActionActiveCell
							'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							'UPGRADE_WARNING: オブジェクト fpSpd.GetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.GetText(ColPC区分, .ActiveRow, HoldPc)
							'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							'UPGRADE_WARNING: オブジェクト fpSpd.GetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.GetText(Col製品NO, .ActiveRow, HoldSei)
							'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							'UPGRADE_WARNING: オブジェクト fpSpd.GetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.GetText(Col仕様NO, .ActiveRow, HoldSiy)
						End With
						Call Calc合計()
						cbFunc(11).Text = "ﾁｪｯｸ"
					End If
				End If
			Case 3
				'複写する為の行情報の保持
				Call clsSPD_Renamed.LineCopyHold()
			Case 4
				'''            If CCur(NullToZero(clsSPD.GetTextEX(Col仕入済数, fpSpd.ActiveRow))) <> 0 Then '2014/09/09 ADD
				'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				If CDec(NullToZero(clsSPD_Renamed.GetTextEX(Col仕入済CNT, fpSpd.ActiveRow))) <> 0 Then '2014/09/09 ADD'2018/06/19 ADD
					CriticalAlarm("仕入済みです。")
					'UPGRADE_WARNING: オブジェクト fpSpd.SetFocus の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					fpSpd.SetFocus()
				Else
					'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					If CDec(NullToZero(clsSPD_Renamed.GetTextEX(Col売上済CNT, fpSpd.ActiveRow))) <> 0 Then '2014/09/09 ADD
						CriticalAlarm("売上済みです。")
						'UPGRADE_WARNING: オブジェクト fpSpd.SetFocus の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						fpSpd.SetFocus()
					Else
						'コピーした行の複写
						Call clsSPD_Renamed.LinePaste()
						'明細連番の項目のクリア
						With fpSpd
							'2014/09/09 ADD↓
							'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.Row = .ActiveRow
							'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.Col = Col見積明細連番
							'UPGRADE_WARNING: オブジェクト fpSpd.Value の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.Value = vbNullString
							'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.Row = .ActiveRow
							'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.Col = Col仕入済数
							'UPGRADE_WARNING: オブジェクト fpSpd.Value の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.Value = vbNullString
							'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.Col = Col仕入済CNT '2018/06/19 ADD
							'UPGRADE_WARNING: オブジェクト fpSpd.Value の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.Value = vbNullString '2018/06/19 ADD
							'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.Col = Col売上済CNT
							'UPGRADE_WARNING: オブジェクト fpSpd.Value の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.Value = vbNullString
							'2014/09/09 ADD↑
							
							'ロック解除する
							Call fpspd_colUnLock()
							
							'製品によりロックする
							'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							Call Lock_Seihin(fpSpd.ActiveRow, clsSPD_Renamed.GetTextEX(Col製品NO, fpSpd.ActiveRow), clsSPD_Renamed.GetTextEX(Col仕様NO, fpSpd.ActiveRow)) '2017/02/10 ADD
							
							
							'''                .Row = .ActiveRow
							'''                .Col = .MaxCols
							'''                .Value = vbNullString
							'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							'UPGRADE_WARNING: オブジェクト fpSpd.GetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.GetText(ColPC区分, .ActiveRow, HoldPc)
							'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							'UPGRADE_WARNING: オブジェクト fpSpd.GetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.GetText(Col製品NO, .ActiveRow, HoldSei)
							'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							'UPGRADE_WARNING: オブジェクト fpSpd.GetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.GetText(Col仕様NO, .ActiveRow, HoldSiy)
						End With
						Call Calc合計()
						cbFunc(11).Text = "ﾁｪｯｸ"
					End If
				End If
			Case 5
				Select Case [cbFunc](Index).Text
					Case "上へ"
						'一行前の行と入れ替え
						'''                    Call clsSPD.LineSwap(-1)
						Call clsSPD_Renamed.LineSwap2(-1) '2014/11/11 ADD
						[cbFunc](11).Text = "ﾁｪｯｸ"
					Case "原割設"
						frm = New SnwMT02F13
						
						With frm
							VB6.ShowForm(frm, VB6.FormShowConstants.Modal, Me)
						End With
						'UPGRADE_NOTE: オブジェクト frm をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
						frm = Nothing
						'2017/03/30 ADD↓
					Case "左へ"
						Call clsSPD_Renamed.colSwap(Col仕分数1, Col仕分数1 + 29, -1)
						[cbFunc](11).Text = "ﾁｪｯｸ"
						'2017/03/30 ADD↑
						'2018/05/03 ADD↓
					Case "日設定"
						Call dispFormDate()
						[cbFunc](11).Text = "ﾁｪｯｸ"
						
					Case "明細検"
						''                    If CCur(NullToZero(clsSPD.GetTextEX(Col仕入済数, fpSpd.ActiveRow))) <> 0 Then '2014/09/09 ADD
						'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						If CDec(NullToZero(clsSPD_Renamed.GetTextEX(Col仕入済CNT, fpSpd.ActiveRow))) <> 0 Then '2014/09/09 ADD'2018/06/19 ADD
							CriticalAlarm("仕入済みです。")
							'UPGRADE_WARNING: オブジェクト fpSpd.SetFocus の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							fpSpd.SetFocus()
							'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						ElseIf CDec(NullToZero(clsSPD_Renamed.GetTextEX(Col売上済CNT, fpSpd.ActiveRow))) <> 0 Then  '2014/09/09 ADD
							CriticalAlarm("売上済みです。")
							'UPGRADE_WARNING: オブジェクト fpSpd.SetFocus の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							fpSpd.SetFocus()
							'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						ElseIf Trim(clsSPD_Renamed.GetTextEX(Col名称, fpSpd.ActiveRow)) <> "" Then  '2014/09/09 ADD
							CriticalAlarm("入力済み行です。")
							'UPGRADE_WARNING: オブジェクト fpSpd.SetFocus の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							fpSpd.SetFocus()
						Else
							'見積明細検索画面
							If doMeisaiKensaku() = True Then
								ReturnF = True
								[cbFunc](11).Text = "ﾁｪｯｸ"
							End If
						End If
						PreviousControl.Focus()
						'2018/05/03 ADD↑
				End Select
				'''            '一行前の行と入れ替え
				'''            Call clsSPD.LineSwap(-1)
				'''            cbFunc(11).Caption = "ﾁｪｯｸ"
			Case 6
				Select Case [cbFunc](Index).Text
					Case "下へ"
						'一行後ろの行と入れ替え
						'''                    Call clsSPD.LineSwap
						Call clsSPD_Renamed.LineSwap2() '2014/11/11 ADD
						[cbFunc](11).Text = "ﾁｪｯｸ"
					Case "掛率"
						With SnwMT02F11
							.ResCodeCTL = fpSpd
							'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							'UPGRADE_WARNING: オブジェクト fpSpd.ActiveCol の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							fpSpd.Col = fpSpd.ActiveCol
							'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							fpSpd.Row = fpSpd.ActiveRow
							'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							'UPGRADE_WARNING: オブジェクト fpSpd.GetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							check = fpSpd.GetText(Col原価, fpSpd.ActiveRow, getdata) '原価
							If check Then
								'UPGRADE_WARNING: オブジェクト getdata の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.ResCodeGenka = getdata
							Else
								.ResCodeGenka = 0
							End If
							'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							'UPGRADE_WARNING: オブジェクト fpSpd.GetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							check = fpSpd.GetText(Col売価, fpSpd.ActiveRow, getdata) '原価
							If check Then
								'UPGRADE_WARNING: オブジェクト getdata の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.ResCodeBaika = getdata
							Else
								.ResCodeBaika = 0
							End If
							VB6.ShowForm(SnwMT02F11, VB6.FormShowConstants.Modal, Me)
						End With
						'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						Call Get金額(fpSpd.ActiveRow)
						Call Calc合計()
						[cbFunc](11).Text = "ﾁｪｯｸ"
						PreviousControl.Focus()
						'2017/03/30 ADD↓
					Case "右へ"
						Call clsSPD_Renamed.colSwap(Col仕分数1, Col仕分数1 + 29, 1)
						[cbFunc](11).Text = "ﾁｪｯｸ"
						'2017/03/30 ADD↑
				End Select
			Case 7
				'表示項目設定
				With SnwMT02F10
					''                Set .ResCodeCTL = fpSpd
					VB6.ShowForm(SnwMT02F10, VB6.FormShowConstants.Modal, Me)
				End With
				Call SpdRowDisp()
				PreviousControl.Focus()
			Case 8
				Select Case [cbFunc](Index).Text
					Case "単価参"
						'仕入単価・売上単価を入力時に過去５日分(製品毎)の単価履歴を表示
						With fpSpd
							'UPGRADE_WARNING: オブジェクト fpSpd.ActiveCol の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							wCol = .ActiveCol
							'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							wRow = .ActiveRow
							'UPGRADE_WARNING: オブジェクト fpSpd.GetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							check = .GetText(ColPC区分, wRow, getdata) 'PC区分
							If check Then
								'UPGRADE_WARNING: オブジェクト getdata の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								wPCKUBN = getdata
							End If
							'UPGRADE_WARNING: オブジェクト fpSpd.GetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							check = .GetText(Col製品NO, wRow, getdata) '製品NO
							If check Then
								'UPGRADE_WARNING: オブジェクト getdata の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								wSEIHNO = getdata
							End If
							'UPGRADE_WARNING: オブジェクト fpSpd.GetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							check = .GetText(Col仕様NO, wRow, getdata) '仕様NO
							If check Then
								'UPGRADE_WARNING: オブジェクト getdata の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								wSIYONO = getdata
							End If
							'UPGRADE_WARNING: オブジェクト fpSpd.ActiveCol の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							If .ActiveCol = Col原価 Then
								wGetKBN = 1
								'UPGRADE_WARNING: オブジェクト fpSpd.ActiveCol の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							ElseIf .ActiveCol = Col売価 Then 
								wGetKBN = 2
							End If
							'UPGRADE_WARNING: オブジェクト fpSpd.GetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							check = .GetText(Col仕入先CD, wRow, getdata) '仕様NO
							If check Then
								'UPGRADE_WARNING: オブジェクト getdata の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								wSirCD = getdata
							End If
							F05 = New SnwMT02F05 '2009/08/10 ADD
							
							'UPGRADE_WARNING: TextBox プロパティ tx_Dummy1.MaxLength には新しい動作が含まれます。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"' をクリックしてください。
							tx_Dummy1.Maxlength = 11
							'''                        Call SnwMT02F05.SelSetup(wGetKBN, tx_Dummy1, wPCKUBN, wSEIHNO, wSIYONO) '2009/07/28 DEL
							Call F05.SelSetup(wGetKBN, tx_Dummy1, wPCKUBN, wSEIHNO, wSIYONO, ([rf_得意先CD].Text), wSirCD)
							VB6.ShowForm(F05, VB6.FormShowConstants.Modal, Me)
							'UPGRADE_WARNING: オブジェクト fpSpd.SetFocus の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							fpSpd.SetFocus()
							If tx_Dummy1.Tag <> "" Then
								'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								'UPGRADE_WARNING: オブジェクト fpSpd.ActiveCol の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.SetText(.ActiveCol, .ActiveRow, tx_Dummy1)
								'原価・売価
								'UPGRADE_WARNING: オブジェクト fpSpd.ActiveCol の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								Call Get率(.ActiveRow, .ActiveCol)
								'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								Call Get金額(.ActiveRow)
								Call Calc合計()
								SendReturnKey()
								cbFunc(11).Text = "ﾁｪｯｸ"
							Else
								'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								'UPGRADE_WARNING: オブジェクト fpSpd.ActiveCol の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.Col = .ActiveCol
								'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.Row = .ActiveRow
								'UPGRADE_WARNING: オブジェクト fpSpd.Action の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.Action = FPSpreadADO.ActionConstants.ActionActiveCell
							End If
							
							'UPGRADE_NOTE: オブジェクト F05 をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
							F05 = Nothing '2009/08/10 ADD
						End With
					Case "社在参"
						With fpSpd
							'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							'UPGRADE_WARNING: オブジェクト fpSpd.ActiveCol の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.Col = .ActiveCol
							'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.Row = .ActiveRow
							'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							'UPGRADE_WARNING: オブジェクト fpSpd.GetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							check = .GetText(Col製品NO, .ActiveRow, getdata) '製品NO
							If check Then
								'UPGRADE_WARNING: オブジェクト getdata の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								wSEIHNO = getdata
							End If
							'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							'UPGRADE_WARNING: オブジェクト fpSpd.GetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							check = .GetText(Col仕様NO, .ActiveRow, getdata) '仕様NO
							If check Then
								'UPGRADE_WARNING: オブジェクト getdata の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								wSIYONO = getdata
							End If
							'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							'UPGRADE_WARNING: オブジェクト fpSpd.GetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							check = .GetText(Col名称, .ActiveRow, getdata) '製品名
							If check Then
								'UPGRADE_WARNING: オブジェクト getdata の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								wSEIHNM = getdata
							End If
							'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							'UPGRADE_WARNING: オブジェクト fpSpd.GetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							check = .GetText(Col社内在庫, .ActiveRow, getdata) '社内在庫数
							If check Then
								'UPGRADE_WARNING: オブジェクト getdata の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								wSYAZAI = getdata
							End If
						End With
						
						If Not (wSEIHNO = "" And wSIYONO = "") Then
							'UPGRADE_WARNING: オブジェクト NullToZero(HD_納期S, ) の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							If NullToZero(HD_納期S, "") <> "" Then '2009/11/26 ADD
								
								F08 = New SnwMT02F08
								
								With F08
									'UPGRADE_WARNING: オブジェクト HD_納期S の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
									.指定日付 = HD_納期S
									.担当者CD = HD_担当者CD
									.[製品NO] = wSEIHNO
									.[仕様NO] = wSIYONO
									.製品名 = wSEIHNM
									
									VB6.ShowForm(F08, VB6.FormShowConstants.Modal, Me)
								End With
								'UPGRADE_NOTE: オブジェクト F08 をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
								F08 = Nothing
							End If '2009/11/26 ADD
						End If
						PreviousControl.Focus()
					Case "客在参"
						With fpSpd
							'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							'UPGRADE_WARNING: オブジェクト fpSpd.ActiveCol の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.Col = .ActiveCol
							'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.Row = .ActiveRow
							'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							'UPGRADE_WARNING: オブジェクト fpSpd.GetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							check = .GetText(Col製品NO, .ActiveRow, getdata) '製品NO
							If check Then
								'UPGRADE_WARNING: オブジェクト getdata の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								wSEIHNO = getdata
							End If
							'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							'UPGRADE_WARNING: オブジェクト fpSpd.GetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							check = .GetText(Col仕様NO, .ActiveRow, getdata) '仕様NO
							If check Then
								'UPGRADE_WARNING: オブジェクト getdata の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								wSIYONO = getdata
							End If
							'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							'UPGRADE_WARNING: オブジェクト fpSpd.GetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							check = .GetText(Col名称, .ActiveRow, getdata) '製品名
							If check Then
								'UPGRADE_WARNING: オブジェクト getdata の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								wSEIHNM = getdata
							End If
							'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							'UPGRADE_WARNING: オブジェクト fpSpd.GetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							check = .GetText(Col客先在庫, .ActiveRow, getdata) '客先在庫数
							If check Then
								'UPGRADE_WARNING: オブジェクト getdata の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								wKYAZAI = getdata
							End If
						End With
						
						If Not (wSEIHNO = "" And wSIYONO = "") Then
							'UPGRADE_WARNING: オブジェクト NullToZero(HD_納期S, ) の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							If NullToZero(HD_納期S, "") <> "" Then '2009/11/26 ADD
								F09 = New SnwMT02F09
								
								With F09
									'UPGRADE_WARNING: オブジェクト HD_納期S の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
									.指定日付 = HD_納期S
									.担当者CD = HD_担当者CD
									.得意先CD = HD_得意先CD
									.[製品NO] = wSEIHNO
									.[仕様NO] = wSIYONO
									.製品名 = wSEIHNM
									
									VB6.ShowForm(F09, VB6.FormShowConstants.Modal, Me)
								End With
								'UPGRADE_NOTE: オブジェクト F09 をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
								F09 = Nothing
							End If '2009/11/26 ADD
						End If
						PreviousControl.Focus()
						'2008/05/22 ADD↓
					Case "数削除"
						If MsgBoxResult.Yes = YesNo("員数部をクリアします。", Me.Text) Then
							
							With fpSpd
								'UPGRADE_WARNING: オブジェクト fpSpd.ReDraw の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.ReDraw = False
								'UPGRADE_WARNING: オブジェクト fpSpd.AutoCalc の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.AutoCalc = False
								'数量欄のクリア
								'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.Col = Col仕分数1
								'UPGRADE_WARNING: オブジェクト fpSpd.Col2 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.Col2 = Col仕分数1 + 29
								'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.Row = 1
								'UPGRADE_WARNING: オブジェクト fpSpd.Row2 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								'UPGRADE_WARNING: オブジェクト fpSpd.MaxRows の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.Row2 = .MaxRows
								
								'UPGRADE_WARNING: オブジェクト fpSpd.BlockMode の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.BlockMode = True
								'UPGRADE_WARNING: オブジェクト fpSpd.Action の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.Action = FPSpreadADO.ActionConstants.ActionClear
								'UPGRADE_WARNING: オブジェクト fpSpd.BlockMode の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.BlockMode = False
								
								'2009/02/26 ADD↓
								'客先在庫関係のクリア
								'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.Col = Col社内在庫
								'UPGRADE_WARNING: オブジェクト fpSpd.Col2 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.Col2 = Col社内在庫
								'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.Row = 1
								'UPGRADE_WARNING: オブジェクト fpSpd.Row2 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								'UPGRADE_WARNING: オブジェクト fpSpd.MaxRows の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.Row2 = .MaxRows
								
								'UPGRADE_WARNING: オブジェクト fpSpd.BlockMode の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.BlockMode = True
								'UPGRADE_WARNING: オブジェクト fpSpd.Action の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.Action = FPSpreadADO.ActionConstants.ActionClear
								'UPGRADE_WARNING: オブジェクト fpSpd.BlockMode の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.BlockMode = False
								
								'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.Col = Col客先在庫
								'UPGRADE_WARNING: オブジェクト fpSpd.Col2 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.Col2 = Col客先在庫
								'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.Row = 1
								'UPGRADE_WARNING: オブジェクト fpSpd.Row2 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								'UPGRADE_WARNING: オブジェクト fpSpd.MaxRows の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.Row2 = .MaxRows
								
								'UPGRADE_WARNING: オブジェクト fpSpd.BlockMode の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.BlockMode = True
								'UPGRADE_WARNING: オブジェクト fpSpd.Action の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.Action = FPSpreadADO.ActionConstants.ActionClear
								'UPGRADE_WARNING: オブジェクト fpSpd.BlockMode の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.BlockMode = False
								
								'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.Col = Col転用
								'UPGRADE_WARNING: オブジェクト fpSpd.Col2 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.Col2 = Col転用
								'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.Row = 1
								'UPGRADE_WARNING: オブジェクト fpSpd.Row2 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								'UPGRADE_WARNING: オブジェクト fpSpd.MaxRows の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.Row2 = .MaxRows
								
								'UPGRADE_WARNING: オブジェクト fpSpd.BlockMode の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.BlockMode = True
								'UPGRADE_WARNING: オブジェクト fpSpd.Action の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.Action = FPSpreadADO.ActionConstants.ActionClear
								'UPGRADE_WARNING: オブジェクト fpSpd.BlockMode の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.BlockMode = False
								'2009/02/26 ADD↑
								
								'金額欄のクリア
								'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.Col = Col金額
								'UPGRADE_WARNING: オブジェクト fpSpd.Col2 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.Col2 = Col仕入金額
								'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.Row = 1
								'UPGRADE_WARNING: オブジェクト fpSpd.Row2 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								'UPGRADE_WARNING: オブジェクト fpSpd.MaxRows の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.Row2 = .MaxRows
								
								'UPGRADE_WARNING: オブジェクト fpSpd.BlockMode の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.BlockMode = True
								'UPGRADE_WARNING: オブジェクト fpSpd.Action の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.Action = FPSpreadADO.ActionConstants.ActionClear
								'UPGRADE_WARNING: オブジェクト fpSpd.BlockMode の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.BlockMode = False
								'消費税欄のクリア
								'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.Col = Col消費税額
								'UPGRADE_WARNING: オブジェクト fpSpd.Col2 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.Col2 = Col消費税額
								'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.Row = 1
								'UPGRADE_WARNING: オブジェクト fpSpd.Row2 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								'UPGRADE_WARNING: オブジェクト fpSpd.MaxRows の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.Row2 = .MaxRows
								
								'UPGRADE_WARNING: オブジェクト fpSpd.BlockMode の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.BlockMode = True
								'UPGRADE_WARNING: オブジェクト fpSpd.Action の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.Action = FPSpreadADO.ActionConstants.ActionClear
								'UPGRADE_WARNING: オブジェクト fpSpd.BlockMode の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.BlockMode = False
								'合計計算
								Call Calc合計()
								
								'UPGRADE_WARNING: オブジェクト fpSpd.AutoCalc の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.AutoCalc = True
								'UPGRADE_WARNING: オブジェクト fpSpd.ReDraw の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.ReDraw = True
							End With
						End If
						'UPGRADE_WARNING: オブジェクト fpSpd.SetFocus の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						fpSpd.SetFocus()
						'2008/05/22 ADD↑
				End Select
				'''''--2003/11/05.DEL----------------------
				'''''        Case 10
				'''''            If vbYes = NoYes("現在の編集内容を破棄します。", Me.Caption) Then
				'''''                DoEvents
				'''''''                Call FormInitialize
				'''''                Call InitialItems
				'''''                With fpSpd
				'''''                    .SetFocus
				'''''                    '初期位置
				'''''                    .Col = 3
				'''''                    .Row = 1
				'''''                    .Action = ActionActiveCell
				'''''                End With
				'''''            Else
				'''''                PreviousControl.SetFocus
				'''''            End If
				'''''--2003/11/05.ADD-----------------------
			Case 9
				''            fpSpd.EditModePermanent = False '常時入力モードを維持するかどうかを設定します。
			Case 10
				Me.Close()
				CLK2F = False
				Exit Sub
				'''''---------------------------------------
			Case 11
				If [cbFunc](Index).Text = "ﾁｪｯｸ" Then
					'2004/11/29 ADD
					If Trim(rf_見積番号.Text) <> vbNullString Then
						If Chk締日To見積(CInt(Trim(rf_見積番号.Text))) = False Then
							'''''''''''                        CriticalAlarm "更新済みの為、修正できません。"'2015/01/20 DEL
							'''''''''''                        cbFunc(Index).TabStop = False
							'''''''''''                        'ボタン２重起動防止フラグの初期化
							'''''''''''                        CLK2F = False
							'''''''''''                        Exit Sub
							'2013/09/18 DEL↓
							''''                    '2013/08/08 ADD↓
							''''                    Else
							''''                        '処理済みのメッセージ
							''''                        If Not IsNull(HD_売上日付) Then
							''''                            CriticalAlarm "売上処理済みの為、修正できません。"
							''''                            cbFunc(Index).TabStop = False
							''''                            'ボタン２重起動防止フラグの初期化
							''''                            CLK2F = False
							''''                            PreviousControl.SetFocus
							''''                            Exit Sub
							''''                        ElseIf Not IsNull(HD_仕入日付) Then
							''''                            CriticalAlarm "仕入処理済みの為、修正できません。"
							''''                            cbFunc(Index).TabStop = False
							''''                            'ボタン２重起動防止フラグの初期化
							''''                            CLK2F = False
							''''                            PreviousControl.SetFocus
							''''                            Exit Sub
							''''                        End If
							''''                    '2013/08/08 ADD↑
							'2013/09/18 DEL↑
						End If
					End If
					
					Call Calc合計() '2019/12/24 ADD
					
					If Item_Check([cbFunc](Index).TabIndex) = True Then
						Select Case Upload_Chk
							Case 0
								[cbFunc](Index).Text = "登録" '2018/03/26 DEL
								'''                            '2018/03/26 ADD↓
								'''                            If cLoginControl.AppUpDel = True Then
								'''                                cbFunc(Index).Caption = "登録"
								'''                            Else
								'''                                cbFunc(Index).Caption = ""
								'''                            End If
								'''                            '2018/03/26 ADD↑
								
							Case -1
								'位置付け
								'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								fpSpd.Col = 3
								'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								fpSpd.Row = 1
								'UPGRADE_WARNING: オブジェクト fpSpd.Action の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								fpSpd.Action = FPSpreadADO.ActionConstants.ActionActiveCell
							Case Else
								'位置付け
								'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								fpSpd.Col = 3
								'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								fpSpd.Row = 1
								'UPGRADE_WARNING: オブジェクト fpSpd.Action の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								fpSpd.Action = FPSpreadADO.ActionConstants.ActionActiveCell
								CriticalAlarm("エラー内容を確認して下さい。" & Replace(gErrNo, ",", vbCrLf & "行番号"))
						End Select
						'''''*5
						'''''                    If Upload_Chk = 0 Then
						'''''''                        fpSpd.Row = -1
						'''''''                        fpSpd.Col = 13
						'''''''                        fpSpd.ForeColor = RGB(0, 0, 0)
						'''''                        cbFunc(Index).Caption = "登録"
						'''''                    Else
						'''''''                        fpSpd.Row = -1
						'''''''                        fpSpd.Col = 13
						'''''''                        fpSpd.ForeColor = RGB(255, 0, 0)
						'''''                        '行の位置付け
						'''''                        fpSpd.Col = 3
						'''''                        fpSpd.Row = 1
						'''''                        fpSpd.Action = ActionActiveCell
						'''''                        CriticalAlarm "エラー内容を確認して下さい。"
						'''''                    End If
						PreviousControl.Focus()
					End If
				Else
					If MsgBoxResult.Yes = YesNo("保存します。", Me.Text) Then
						System.Windows.Forms.Application.DoEvents()
						
						If Upload Then
							''                    Call FormInitialize
							''                    [tx_親製品NO].SetFocus
							''--2003/11/05.DEL-------------------------------
							''                        Closed = True
							''                        Unload Me
							''                        CLK2F = False
							''                        Exit Sub
							''--2003/11/05.ADD-------------------------------
							[cbFunc](Index).Text = "登録済"
							PreviousControl.Focus()
							''-----------------------------------------------
						Else
							PreviousControl.Focus()
						End If
					Else
						PreviousControl.Focus()
					End If
				End If
				''--2003/11/05.DEL-----------
				''        Case 12
				''            Unload Me
				''            CLK2F = False
				''            Exit Sub
				''--2003/11/05.ADD-----------
			Case 12
				Closed_Renamed = True
				Me.Close()
				CLK2F = False
				Exit Sub
				''---------------------------
		End Select
		[cbFunc](Index).TabStop = False
		'ボタン２重起動防止フラグの初期化
		CLK2F = False
	End Sub
	
	Private Function doMeisaiKensaku() As Boolean
		Dim fpSpd As Object
		'見積の明細検索画面を表示、セットする
		
		If cMitsumoriM.ShowDialog = True Then
			doMeisaiKensaku = True
			With fpSpd
				'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.SetText(ColSP区分, .ActiveRow, cMitsumoriM.SP区分)
				'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.SetText(ColPC区分, .ActiveRow, cMitsumoriM.PC区分)
				
				'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.SetText(Col製品NO, .ActiveRow, cMitsumoriM.製品NO)
				'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.SetText(Col仕様NO, .ActiveRow, cMitsumoriM.仕様NO)
				'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.SetText(Colベース色, .ActiveRow, cMitsumoriM.ベース色)
				'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.SetText(Col名称, .ActiveRow, cMitsumoriM.漢字名称)
				
				'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.SetText(ColW, .ActiveRow, cMitsumoriM.W)
				'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.SetText(ColD, .ActiveRow, cMitsumoriM.D)
				'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.SetText(ColH, .ActiveRow, cMitsumoriM.H)
				'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.SetText(ColD1, .ActiveRow, cMitsumoriM.D1)
				'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.SetText(ColD2, .ActiveRow, cMitsumoriM.D2)
				'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.SetText(ColH1, .ActiveRow, cMitsumoriM.H1)
				'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.SetText(ColH2, .ActiveRow, cMitsumoriM.H2)
				'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.SetText(Col単位, .ActiveRow, cMitsumoriM.単位名)
				'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.SetText(Col定価, .ActiveRow, cMitsumoriM.定価)
				'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.SetText(Col原価, .ActiveRow, cMitsumoriM.仕入単価)
				'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.SetText(Col売価, .ActiveRow, cMitsumoriM.売上単価)
				'            .SetText ColU区分, .ActiveRow, cMitsumoriM.U区分
				'            .SetText ColM, .ActiveRow, cMitsumoriM.M区分
				'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.SetText(Col仕入業者CD, .ActiveRow, cMitsumoriM.仕入業者CD)
				'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.SetText(Col仕入業者名, .ActiveRow, cMitsumoriM.仕入業者名)
				'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.SetText(Col仕入先CD, .ActiveRow, cMitsumoriM.仕入先CD)
				'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.SetText(Col仕入先名, .ActiveRow, cMitsumoriM.仕入先名)
				'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.SetText(Col送り先CD, .ActiveRow, cMitsumoriM.配送先CD)
				'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.SetText(Col送り先名, .ActiveRow, cMitsumoriM.配送先名)
				'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.SetText(Col作業区分CD, .ActiveRow, cMitsumoriM.作業区分CD)
				
				'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.SetText(Col売上税区分, .ActiveRow, cMitsumoriM.売上税区分)
				'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.SetText(Col製品区分, .ActiveRow, cMitsumoriM.製品区分)
				'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.SetText(Col明細備考, .ActiveRow, cMitsumoriM.明細備考) '2018/11/03 ADD
				
				
				'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				Call Lock_Seihin(.ActiveRow, clsSPD_Renamed.GetTextEX(Col製品NO, .ActiveRow), clsSPD_Renamed.GetTextEX(Col仕様NO, .ActiveRow)) '2020/10/10 ADD
				
				'2020/10/10 DEL↓
				'''''                '2015/10/26 ADD
				'''''                Dim cSeihin As clsSeihin
				'''''                Set cSeihin = New clsSeihin
				'''''                '2015/10/26 ADD↓
				'''''                '在庫管理する場合、名称をロックする。
				'''''                cSeihin.Initialize
				'''''                cSeihin.製品NO = clsSPD.GetTextEX(Col製品NO, .ActiveRow)
				'''''                cSeihin.仕様NO = clsSPD.GetTextEX(Col仕様NO, .ActiveRow)
				'''''                If cSeihin.GetbyID = True Then
				''''''                    If cSeihin.在庫区分 = 0 Then
				'''''                    If cSeihin.サイズ変更区分 = 0 Then '2020/10/10 ADD
				'''''                        .BlockMode = True
				'''''                        .Row = .ActiveRow
				'''''                        .Row2 = .ActiveRow
				'''''                        '.Col = Col名称
				'''''                        .Col = ColW '2015/11/13 ADD
				'''''                        .Col2 = ColH2
				'''''                        .Lock = True
				'''''                        .BlockMode = False
				'''''                    Else
				'''''                        .BlockMode = True
				'''''                        .Row = .ActiveRow
				'''''                        .Row2 = .ActiveRow
				'''''                        '.Col = Col名称
				'''''                        .Col = ColW '2015/11/13 ADD
				'''''                        .Col2 = ColH2
				'''''                        .Lock = False
				'''''                        .BlockMode = False
				'''''                    End If
				'''''                Else
				'''''                    .BlockMode = True
				'''''                    .Row = .ActiveRow
				'''''                    .Row2 = .ActiveRow
				'''''                    '.Col = Col名称
				'''''                    .Col = ColW '2015/11/13 ADD
				'''''                    .Col2 = ColH2
				'''''                    .Lock = False
				'''''                    .BlockMode = False
				'''''                End If
				'''''                '2015/10/26 ADD↑
				'2020/10/10 DEL↑
				
			End With
		End If
		
	End Function
	
	
	
	Private Sub cbFunc_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cbFunc.Enter
		Dim Index As Short = cbFunc.GetIndex(eventSender)
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		sb_Msg.Items.Item(1).Text = ""
	End Sub
	
	Private Sub cbFuncSub_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cbFuncSub.Click
		Dim Index As Short = cbFuncSub.GetIndex(eventSender)
		Dim fpSpd As Object
		Dim wRow As Integer
		
		'ボタンを押されたのが２回目ならば抜ける
		If CLK2F = True Then
			Exit Sub
		End If
		'ボタン２重起動防止フラグのセット
		CLK2F = True
		cbFuncSub(Index).TabStop = True
		Dim F06 As SnwMT02F06 '2009/08/10 ADD
		Select Case Index
			Case 1
				''DoEvents
				'価格設定画面処理
				op_Disp(2).Checked = True
				SnwMT02F01.ResParentForm = Me
				'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				SnwMT02F01.MituNo = SpcToNull(rf_見積番号, 0)
				''''            SnwMT02F01.SetCol = TeikaCol
				SnwMT02F01.SetCol = Col定価
				VB6.ShowForm(SnwMT02F01, VB6.FormShowConstants.Modal, Me)
				PreviousControl.Focus()
				[cbFunc](11).Text = "ﾁｪｯｸ"
			Case 2
				'仕分レベル設定画面処理
				SnwMT02F02.ResParentForm = Me
				'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				SnwMT02F02.MituNo = SpcToNull(rf_見積番号, 0)
				SnwMT02F02.SetCol = Col仕分数1
				VB6.ShowForm(SnwMT02F02, VB6.FormShowConstants.Modal, Me)
				PreviousControl.Focus()
			Case 3
				'Excel出力処理
				SnwMT02F03.ResParentForm = Me
				'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				SnwMT02F03.MituNo = SpcToNull(rf_見積番号, 0)
				SnwMT02F03.MituNM = rf_見積名称.Text
				'UPGRADE_WARNING: オブジェクト Me.fpSpd の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				SnwMT02F03.OwnerSpd = Me.fpSpd
				''            SnwMT02F03.Show vbModal, Me
				Me.Enabled = False
				VB6.ShowForm(SnwMT02F03, VB6.FormShowConstants.Modeless, Me)
				''            PreviousControl.SetFocus
			Case 4
				'Excel入力処理
				SnwMT02F04.ResParentForm = Me
				'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				SnwMT02F04.MituNo = SpcToNull(rf_見積番号, 0)
				''            SnwMT02F04.Show vbModal, Me
				Me.Enabled = False
				VB6.ShowForm(SnwMT02F04, VB6.FormShowConstants.Modeless, Me)
				''            PreviousControl.SetFocus
			Case 5
				'顧客テンプレート貼付処理
				''            Call SelKokTmp.SelSetup(tx_Dummy1, rf_得意先CD, rf_得意先名)
				''            SelKokTmp.Show vbModal, Me
				''            tx_Dummy1.MaxLength = 40
				''            If tx_Dummy1.Tag <> "" Then
				''                Call DspTokTemp(rf_得意先CD, tx_Dummy1)
				''                cbFunc(11).Caption = "ﾁｪｯｸ"
				''                fpSpd.Col = 1
				''                fpSpd.Row = 1
				''                fpSpd.Action = ActionActiveCell
				''            Else
				''                PreviousControl.SetFocus
				''            End If
				F06 = New SnwMT02F06 '2009/08/10 ADD
				
				'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				wRow = fpSpd.ActiveRow
				F06.ResParentForm = Me
				Call F06.SelSetup([rf_得意先CD].Text, rf_得意先名.Text)
				VB6.ShowForm(F06, VB6.FormShowConstants.Modal, Me)
				''            PreviousControl.SetFocus
				''            cbFunc(11).Caption = "ﾁｪｯｸ"
				
				System.Windows.Forms.Application.DoEvents()
				'UPGRADE_WARNING: オブジェクト fpSpd.SetFocus の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				fpSpd.SetFocus()
				'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				fpSpd.Col = 3
				'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				fpSpd.Row = wRow
				'UPGRADE_WARNING: オブジェクト fpSpd.Action の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				fpSpd.Action = FPSpreadADO.ActionConstants.ActionActiveCell
				If DspKokData(wRow) Then
					[cbFunc](11).Text = "ﾁｪｯｸ"
				End If
				
				'UPGRADE_NOTE: オブジェクト F06 をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
				F06 = Nothing '2009/08/10 ADD
		End Select
		cbFuncSub(Index).TabStop = False
		'ボタン２重起動防止フラグの初期化
		CLK2F = False
	End Sub
	
	Private Sub fpSpd_DragDropBlock(ByVal Col As Integer, ByVal Row As Integer, ByVal Col2 As Integer, ByVal Row2 As Integer, ByVal NewCol As Integer, ByVal NewRow As Integer, ByVal NewCol2 As Integer, ByVal NewRow2 As Integer, ByVal Overwrite As Boolean, ByRef Action As Short, ByRef DataOnly As Boolean, ByRef Cancel As Boolean)
		If Overwrite = False Then
			Cancel = True
		End If
	End Sub
	
	'UPGRADE_ISSUE: PictureBox イベント picFunction.GotFocus はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="ABD9AF39-7E24-4AFF-AD8D-3675C1AA3054"' をクリックしてください。
	Private Sub picFunction_GotFocus()
		PreviousControl.Focus()
	End Sub
	
	Private Sub cbFuncSub_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cbFuncSub.Enter
		Dim Index As Short = cbFuncSub.GetIndex(eventSender)
		'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
		sb_Msg.Items.Item(1).Text = ""
	End Sub
	
	Private Sub cbTabEnd_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cbTabEnd.Enter
		''    If Item_Check(cbTabEnd.TabIndex) Then
		[cbFunc](11).Focus()
		[cbFunc](11).PerformClick()
		''    End If
	End Sub
	
	Private Sub cb固定列_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cb固定列.Click
		Dim fpSpd As Object
		''---2005/07/05.ADD
		Dim HCol As Short
		Dim HRow As Short
		Dim wColsFrozen As Short
		
		cb固定列.TabStop = True
		
		'colのHOLD
		'UPGRADE_WARNING: オブジェクト fpSpd.ActiveCol の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		HCol = fpSpd.ActiveCol
		'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		HRow = fpSpd.ActiveRow
		
		' 設定を SPREADに反映します。
		With fpSpd
			'UPGRADE_WARNING: オブジェクト fpSpd.ReDraw の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.ReDraw = False ' 再描画 禁止
			
			'UPGRADE_WARNING: オブジェクト fpSpd.ActiveCol の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			If .ActiveCol = 1 Then
				' 固定ｾﾙを設定します。
				wColsFrozen = 0 ' 列
				'UPGRADE_WARNING: オブジェクト fpSpd.ColsFrozen の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.ColsFrozen = 0 ' 列
			Else
				' 固定ｾﾙを設定します。
				'UPGRADE_WARNING: オブジェクト fpSpd.ActiveCol の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				wColsFrozen = .ActiveCol - 1
				'UPGRADE_WARNING: オブジェクト fpSpd.ColsFrozen の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト fpSpd.ActiveCol の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.ColsFrozen = .ActiveCol - 1
			End If
			
			'UPGRADE_WARNING: オブジェクト fpSpd.ReDraw の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.ReDraw = True ' 再描画 許可
			
			'ActiveCellの設定
			'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Col = HCol
			'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Row = HRow
			'UPGRADE_WARNING: オブジェクト fpSpd.Action の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Action = FPSpreadADO.ActionConstants.ActionActiveCell
			'UPGRADE_WARNING: オブジェクト fpSpd.SetFocus の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.SetFocus()
		End With
		
		'固定列情報の保持
		WriteIni("ColsFrozen", "MT02F00_SPD", CStr(wColsFrozen), INIFile)
		
		cb固定列.TabStop = False
	End Sub
	
	'''Private Sub cb展開_Click()
	'''    cb展開.TabStop = True
	'''
	'''    If vbYes = YesNo("展開します。", Me.Caption) Then
	'''        DoEvents
	'''
	'''        If Tenkai_Chk Then
	'''            cb展開.TabStop = False
	'''            fpSpd.SetFocus
	'''            '位置付け
	'''            fpSpd.Col = 3
	'''            fpSpd.Row = 1
	'''            fpSpd.Action = ActionActiveCell
	'''            Exit Sub
	'''        Else
	'''            PreviousControl.SetFocus
	'''        End If
	'''    Else
	'''        PreviousControl.SetFocus
	'''    End If
	'''    cb展開.TabStop = False
	'''End Sub
	'''
	'''Private Sub cmdChkOn_Click()
	'''    Dim i As Integer, j As Integer
	'''
	'''    If fpSpd.DataRowCnt = 0 Then
	'''        Inform "対象データがありません。"
	'''        Exit Sub
	'''    End If
	'''
	'''    j = GetSerchMax
	'''
	'''    For i = 1 To j
	'''        fpSpd.Col = 1
	'''        fpSpd.Row = i
	'''        fpSpd.Text = 1
	'''    Next
	'''End Sub
	'''
	'''Private Sub cmdChkOff_Click()
	'''    Dim i As Integer
	'''
	'''    If fpSpd.DataRowCnt = 0 Then
	'''        Exit Sub
	'''    End If
	'''
	'''    For i = 1 To fpSpd.DataRowCnt
	'''        fpSpd.Col = 1
	'''        fpSpd.Row = i
	'''        fpSpd.Text = 0
	'''    Next
	'''End Sub
	''
	''Private Sub tx_シート名称_GotFocus()
	''''    If Item_Check([tx_シート名称].TabIndex) = False Then
	''''        Exit Sub
	''''    End If
	''
	''    Set PreviousControl = Me.ActiveControl
	''    'ボタン名設定
	''    Call SetUpFuncs(Me.ActiveControl.Name)
	''    [sb_Msg].Panels(1).Text = "シート名称を入力して下さい。"
	''End Sub
	''
	''Private Sub tx_シート名称_RtnKeyDown(KeyCode As Integer, Shift As Integer, Cancel As Boolean)
	''    ReturnF = True
	''End Sub
	''
	''Private Sub tx_シート名称_LostFocus()
	''    [sb_Msg].Panels(1).Text = ""
	''    If ReturnF = False Then
	''        [tx_シート名称].Undo
	''    End If
	''    ReturnF = False
	''End Sub
	
	Private Function Item_Check(ByRef ItemNo As Short) As Short
		Dim Chk_ID As String 'ﾁｪｯｸ用ワーク
		
		On Error GoTo Item_Check_Err
		Item_Check = False
		
		''    'キー項目「シート名称」のﾁｪｯｸ
		''    If ItemNo > [tx_シート名称].TabIndex And ([tx_シート名称] <> HTEMPNM Or IsNull(HTEMPNM)) Then
		''        If IsCheckText([tx_シート名称]) = False Then
		''            CriticalAlarm "シート名称が未入力です。"
		''            [tx_シート名称].Undo
		''            [tx_シート名称].SetFocus
		''            Exit Function
		''        End If
		''
		''        '--- 入力値をワークへ格納
		''        HTEMPNM = [tx_シート名称]
		''        cbFunc(11).Caption = "ﾁｪｯｸ"
		''    End If
		Item_Check = True
		
		Exit Function
Item_Check_Err: 
		MsgBox(Err.Number & " " & Err.Description)
	End Function
	
	Private Sub fpSpd_Initialize()
		Dim fpSpd As Object
		Dim wColsFrozen As String
		
		With fpSpd
			'UPGRADE_WARNING: オブジェクト fpSpd.ReDraw の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.ReDraw = False
			''''        .MaxCols = 76
			'UPGRADE_WARNING: オブジェクト fpSpd.MaxRows の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.MaxRows = SPMaxRow
			'UPGRADE_WARNING: オブジェクト fpSpd.EditModePermanent の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.EditModePermanent = True '常時入力モードを維持するかどうかを設定します。
			'セル
			'UPGRADE_WARNING: オブジェクト fpSpd.UnitType の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.UnitType = FPSpreadADO.UnitTypeConstants.UnitTypeTwips
			'列の幅を設定する。
			'UPGRADE_WARNING: オブジェクト fpSpd.RowHeight の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.RowHeight(-1) = 250
			'グリッド線の表示形式を設定します。
			'UPGRADE_WARNING: オブジェクト fpSpd.GridSolid の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.GridSolid = True
			'セルの内容を消去します。
			'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Col = -1
			'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Row = -1
			'UPGRADE_WARNING: オブジェクト fpSpd.Action の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Action = FPSpreadADO.ActionConstants.ActionClearText
			'セルの背景色をグリッド線の下に表示します。
			'UPGRADE_WARNING: オブジェクト fpSpd.BackColorStyle の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.BackColorStyle = FPSpreadADO.BackColorStyleConstants.BackColorStyleUnderGrid
			'仕分の合計計算式を設定します。（仕分１＝SiwakeCol＝33）
			''''        .Col = SiwakeCol - 1
			'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Col = Col数量合計
			'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Row = -1
			'UPGRADE_WARNING: オブジェクト fpSpd.Formula の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Formula = "sum(BA#:CD#)" '2020/09/16 ADD
			'        .Formula = "sum(AZ#:CC#)" '2019/12/12 ADD
			''        .Formula = "sum(AY#:CB#)" '2018/05/03 ADD
			'''''        .Formula = "sum(AW#:BZ#)" '2017/05/14 ADD
			''''        .Formula = "sum(AU#:BX#)" '2016/06/22 ADD
			''        .Formula = "sum(AR#:BU#)" '2015/09/29 ADD
			'''        .Formula = "sum(AQ#:BT#)" '2015/02/04 ADD
			''''''''        .Formula = "sum(Ak#:BN#)"   2003/10/29
			''''''''        .Formula = "sum(AL#:BO#)"   2005/10/14
			''        .Formula = "sum(AM#:BP#)"
			'''        .Formula = "sum(AO#:BR#)"
			''''        .Formula = "sum(AP#:BS#)" '2014/09/09 ADD
			'見積数量の計算式を設定します。
			''''        .Col = SiwakeCol - 13
			'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Col = Col見積数量
			'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Row = -1
			'UPGRADE_WARNING: オブジェクト fpSpd.Formula の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Formula = "AZ#-AW#-AU#" '2020/09/16 ADD
			'        .Formula = "AY#-AV#-AT#" '2018/12/12 ADD
			''        .Formula = "AX#-AU#-AS#" '2018/05/03 ADD
			'''''        .Formula = "AV#-AS#-AQ#" '2017/05/14 ADD
			''''        .Formula = "AT#-AQ#-AO#" '2016/06/22 ADD
			''        .Formula = "AQ#-AN#-AL#" '2015/09/29 ADD
			'''        .Formula = "AP#-AM#-AK#" '2015/02/04 ADD
			''''''''        .Formula = "AJ#-AH#-AG#"    2003/10/29
			''''''''        .Formula = "AK#-AI#-AH#"    2005/10/14
			'''        .Formula = "AL#-AJ#-AI#"
			'''        .Formula = "AN#-AL#-AJ#"
			''''        .Formula = "AO#-AL#-AJ#" '2014/09/09 ADD
			'発注数の計算式を設定します。
			''''        .Col = SiwakeCol - 2
			'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Col = Col発注数
			'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Row = -1
			'UPGRADE_WARNING: オブジェクト fpSpd.Formula の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Formula = "AZ#-AW#-AU#-AS#+AX#" '2020/09/16 ADD
			'        .Formula = "AY#-AV#-AT#-AR#+AW# '2019/12/12 ADD"
			''        .Formula = "AX#-AU#-AS#-AQ#+AV# '2018/05/03 ADD"
			'''''        .Formula = "AV#-AS#-AQ#-AO#+AT#" '2017/05/14 ADD
			''''        .Formula = "AT#-AQ#-AO#-AM#+AR#" '2016/06/22 ADD
			''        .Formula = "AQ#-AN#-AL#-AJ#+AO#" '2015/09/29 ADD
			'''        .Formula = "AP#-AM#-AK#-AI#+AN#" '2015/02/04 ADD
			''''''''        .Formula = "AJ#-AH#-AG#-AF#"
			''''''''        .Formula = "AK#-AI#-AH#-AG#"    2005/10/14
			''        .Formula = "AL#-AJ#-AI#-AH#"
			'''        .Formula = "AN#-AL#-AJ#-AH#"
			''''        .Formula = "AO#-AL#-AJ#-AH#+AM#" '2014/09/09 ADD
			'発注数(AN) = Σ(AO)-転用数(AL)-社内在庫数(AH)-客先在庫数(AJ)+発注調整数(AM)
			
			'2023/09/26 ADD↓
			'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Col = 1
			'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Row = 1
			'UPGRADE_WARNING: オブジェクト fpSpd.Col2 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト fpSpd.MaxCols の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Col2 = .MaxCols
			'UPGRADE_WARNING: オブジェクト fpSpd.Row2 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト fpSpd.MaxRows の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Row2 = .MaxRows
			'UPGRADE_WARNING: オブジェクト fpSpd.BlockMode の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.BlockMode = True
			'UPGRADE_WARNING: オブジェクト fpSpd.ForeColor の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.ForeColor = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black)
			'UPGRADE_WARNING: オブジェクト fpSpd.BlockMode の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.BlockMode = False
			'2023/09/26 ADD↑
			
			
			'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Col = Col原価
			'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Row = -1
			'UPGRADE_WARNING: オブジェクト fpSpd.ForeColor の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.ForeColor = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black)
			'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Col = Col売価
			'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Row = -1
			'UPGRADE_WARNING: オブジェクト fpSpd.ForeColor の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.ForeColor = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black)
			
			'2009/09/26 ADD↓
			'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Col = Col社内在庫
			'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Row = -1
			'UPGRADE_WARNING: オブジェクト fpSpd.ForeColor の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.ForeColor = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black)
			'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Col = Col客先在庫参照
			'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Row = -1
			'UPGRADE_WARNING: オブジェクト fpSpd.ForeColor の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.ForeColor = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black)
			'2009/09/26 ADD↑
			
			'2016/12/21 ADD↓
			'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Col = Col社内在庫
			'.ColHidden = True
			'UPGRADE_WARNING: オブジェクト fpSpd.Lock の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Lock = True
			'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Col = Col発注調整数
			'.ColHidden = True
			'UPGRADE_WARNING: オブジェクト fpSpd.Lock の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Lock = True
			'2016/12/21 ADD↑
			
			If VB.Command() = "DEBUG" Then
			Else
				'製品区分を非表示にします。
				'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.Col = Col製品区分
				'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.ColHidden = True
				'製品区分を非表示にします。
				'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.Col = Col見積明細連番
				'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.ColHidden = True
			End If
			'初期位置
			'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Col = Col見積区分
			'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Row = 1
			'UPGRADE_WARNING: オブジェクト fpSpd.Action の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Action = FPSpreadADO.ActionConstants.ActionActiveCell
			
			''---2005/07/05.DEL
			''        '列固定
			''        .ColsFrozen = Col名称
			''---2005/07/05.ADD
			'表示項目設定取得
			'UPGRADE_WARNING: オブジェクト GetIni() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			wColsFrozen = GetIni("ColsFrozen", "MT02F00_SPD", INIFile)
			If wColsFrozen = vbNullString Then
				WriteIni("ColsFrozen", "MT02F00_SPD", CStr(Col名称), INIFile)
				'UPGRADE_WARNING: オブジェクト fpSpd.ColsFrozen の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.ColsFrozen = Col名称
			Else
				'UPGRADE_WARNING: オブジェクト fpSpd.ColsFrozen の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.ColsFrozen = CShort(wColsFrozen)
			End If
			''-----------------
		End With
	End Sub
	
	Private Sub fpspd_colUnLock()
		Dim fpSpd As Object
		With fpSpd
			'UPGRADE_WARNING: オブジェクト fpSpd.BlockMode の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.BlockMode = True
			'ロック解除
			'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Col = Col展開
			'UPGRADE_WARNING: オブジェクト fpSpd.Col2 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Col2 = Col製品区分 - 1
			'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Row = .ActiveRow
			'UPGRADE_WARNING: オブジェクト fpSpd.Row2 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Row2 = .ActiveRow
			'UPGRADE_WARNING: オブジェクト fpSpd.Lock の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Lock = False
			
			'ロック
			'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Col = ColSP区分
			'UPGRADE_WARNING: オブジェクト fpSpd.Col2 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Col2 = ColSP区分
			'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Row = .ActiveRow
			'UPGRADE_WARNING: オブジェクト fpSpd.Row2 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Row2 = .ActiveRow
			'UPGRADE_WARNING: オブジェクト fpSpd.Lock の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Lock = True
			
			'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Col = Colエラー内容
			'UPGRADE_WARNING: オブジェクト fpSpd.Col2 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Col2 = Colエラー内容
			'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Row = .ActiveRow
			'UPGRADE_WARNING: オブジェクト fpSpd.Row2 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Row2 = .ActiveRow
			'UPGRADE_WARNING: オブジェクト fpSpd.Lock の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Lock = True
			
			'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Col = Col見積数量
			'UPGRADE_WARNING: オブジェクト fpSpd.Col2 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Col2 = Col見積数量
			'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Row = .ActiveRow
			'UPGRADE_WARNING: オブジェクト fpSpd.Row2 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Row2 = .ActiveRow
			'UPGRADE_WARNING: オブジェクト fpSpd.Lock の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Lock = True
			
			'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Col = Col金額
			'UPGRADE_WARNING: オブジェクト fpSpd.Col2 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Col2 = Col消費税額
			'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Row = .ActiveRow
			'UPGRADE_WARNING: オブジェクト fpSpd.Row2 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Row2 = .ActiveRow
			'UPGRADE_WARNING: オブジェクト fpSpd.Lock の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Lock = True
			
			'2017/03/10 ADD↓
			'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Col = Col仕入業者名
			'UPGRADE_WARNING: オブジェクト fpSpd.Col2 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Col2 = Col仕入業者名
			'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Row = .ActiveRow
			'UPGRADE_WARNING: オブジェクト fpSpd.Row2 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Row2 = .ActiveRow
			'UPGRADE_WARNING: オブジェクト fpSpd.Lock の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Lock = True
			'2017/03/10 ADD↑
			
			'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Col = Col仕入先名
			'UPGRADE_WARNING: オブジェクト fpSpd.Col2 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Col2 = Col仕入先名
			'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Row = .ActiveRow
			'UPGRADE_WARNING: オブジェクト fpSpd.Row2 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Row2 = .ActiveRow
			'UPGRADE_WARNING: オブジェクト fpSpd.Lock の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Lock = True
			
			'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Col = Col送り先名
			'UPGRADE_WARNING: オブジェクト fpSpd.Col2 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Col2 = Col送り先名
			'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Row = .ActiveRow
			'UPGRADE_WARNING: オブジェクト fpSpd.Row2 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Row2 = .ActiveRow
			'UPGRADE_WARNING: オブジェクト fpSpd.Lock の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Lock = True
			
			'            .Col = Col社内在庫参照
			'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Col = Col社内在庫 '2016/12/21 ADD
			'UPGRADE_WARNING: オブジェクト fpSpd.Col2 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Col2 = Col社内在庫参照
			'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Row = .ActiveRow
			'UPGRADE_WARNING: オブジェクト fpSpd.Row2 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Row2 = .ActiveRow
			'UPGRADE_WARNING: オブジェクト fpSpd.Lock の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Lock = True
			
			'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Col = Col客先在庫参照
			'UPGRADE_WARNING: オブジェクト fpSpd.Col2 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Col2 = Col客先在庫参照
			'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Row = .ActiveRow
			'UPGRADE_WARNING: オブジェクト fpSpd.Row2 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Row2 = .ActiveRow
			'UPGRADE_WARNING: オブジェクト fpSpd.Lock の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Lock = True
			
			'            .Col = Col発注数
			'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Col = Col発注調整数 '2016/12/21 ADD
			'UPGRADE_WARNING: オブジェクト fpSpd.Col2 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Col2 = Col数量合計
			'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Row = .ActiveRow
			'UPGRADE_WARNING: オブジェクト fpSpd.Row2 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Row2 = .ActiveRow
			'UPGRADE_WARNING: オブジェクト fpSpd.Lock の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Lock = True
			
			'UPGRADE_WARNING: オブジェクト fpSpd.BlockMode の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.BlockMode = False
		End With
		
	End Sub
	
	Private Sub fpSpd_Advance(ByVal AdvanceNext As Boolean)
		Dim fpSpd As Object
		With fpSpd
			'UPGRADE_WARNING: オブジェクト fpSpd.MaxRows の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			If .ActiveRow = .MaxRows Then
				If AdvanceNext = True Then
					''''                If .ActiveCol <> (SiwakeCol + 30) Then
					'UPGRADE_WARNING: オブジェクト fpSpd.ActiveCol の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					If .ActiveCol <> (Col仕分数1 + 29) Then
						'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						'UPGRADE_WARNING: オブジェクト fpSpd.ActiveCol の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Col = .ActiveCol + 1
						'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Row = 1
						'UPGRADE_WARNING: オブジェクト fpSpd.Action の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Action = FPSpreadADO.ActionConstants.ActionActiveCell
						'UPGRADE_WARNING: オブジェクト fpSpd.TopRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.TopRow = 1
					Else
						cbTabEnd.Focus()
					End If
				End If
				'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			ElseIf fpSpd.ActiveRow = 1 Then 
				If AdvanceNext = False Then
					'UPGRADE_WARNING: オブジェクト fpSpd.ActiveCol の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					If .ActiveCol <> 1 Then
						''                        .Col = .ActiveCol
						''                        .Row = .MaxRows
						''                        .Action = ActionActiveCell
						''                    Else
						''                        tx_シート名称.SetFocus
					End If
				End If
			End If
		End With
		'スプレッドコメント表示
		'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト fpSpd.ActiveCol の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Call Comment_spd(fpSpd.ActiveCol, fpSpd.ActiveRow)
	End Sub
	
	Private Sub fpSpd_Change(ByVal Col As Integer, ByVal Row As Integer)
		If Col <> Col展開 Then '2015/12/01 ADD
			'チェックボックス型はデータセットでチェンジイベントが発生してしまうので除外！
			[cbFunc](11).Text = "ﾁｪｯｸ"
			''    Debug.Print "fpSpd_Change"
		End If
	End Sub
	
	Private Sub fpSpd_EditMode(ByVal Col As Integer, ByVal Row As Integer, ByVal Mode As Short, ByVal ChangeMade As Boolean)
		Dim fpSpd As Object
		'    Debug.Print "fpSpd_EditMode = " & Col & ":" & Row & ":" & Mode & ":" & ChangeMade
		
		Dim check As Boolean
		
		If Mode = 0 Then Exit Sub 'フォーカスがないならば
		If ChangeMade = True Then Exit Sub
		
		'行の色を変える
		With fpSpd
			'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Col = -1
			'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Row = .ActiveRow
			'UPGRADE_WARNING: オブジェクト fpSpd.BackColor の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			If .BackColor <> &HFFFFC0 Then
				'UPGRADE_WARNING: オブジェクト fpSpd.BackColor の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.BackColor = &HFFFFC0
			End If
		End With
		''''ACSコメントの関係でクリアするのでHOLDは無条件で行なう
		''''    If HCol = Col And HRow = Row Then Exit Sub  '2010/06/16 DEL
		
		'        Debug.Print "hold:" & " col:" & Col & " row:" & Row
		Select Case Col
			Case ColPC区分
				'UPGRADE_WARNING: オブジェクト fpSpd.GetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				check = fpSpd.GetText(ColPC区分, Row, HoldPc)
			Case Col製品NO
				'UPGRADE_WARNING: オブジェクト fpSpd.GetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				check = fpSpd.GetText(ColPC区分, Row, HoldPc)
				'UPGRADE_WARNING: オブジェクト fpSpd.GetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				check = fpSpd.GetText(Col製品NO, Row, HoldSei)
			Case Col仕様NO
				'UPGRADE_WARNING: オブジェクト fpSpd.GetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				check = fpSpd.GetText(ColPC区分, Row, HoldPc)
				'UPGRADE_WARNING: オブジェクト fpSpd.GetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				check = fpSpd.GetText(Col製品NO, Row, HoldSei)
				'UPGRADE_WARNING: オブジェクト fpSpd.GetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				check = fpSpd.GetText(Col仕様NO, Row, HoldSiy)
				''        Case 6, 7
				''             'IMEモード[使用不可]
				''''            ImmAssociateContext Me.hwnd, 0&
				''        Case 14 To 18
				''             'IMEモード[使用不可]
				''            ImmAssociateContext Me.hwnd, 0&
				''            Debug.Print "IME:" & Col
		End Select
		
		'UPGRADE_WARNING: オブジェクト fpSpd.GetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		check = fpSpd.GetText(Col, Row, HoldCD)
		
		HCol = Col
		HRow = Row
		''    Debug.Print "EditMode col & Row & HoldCD = " & Col & ":" & Row & ":" & HoldCD
	End Sub
	
	Private Sub fpSpd_GotFocus()
		Dim fpSpd As Object
		'''    Debug.Print "SiwaNo:" & SiwaNo
		''''''    If SiwaNo <> 0 Then
		''''''        'EXCEL取込データ表示
		''''''        If SetImportData Then
		''''''            cbFunc(11).Caption = "ﾁｪｯｸ"
		''''''''            fpSpd.SetFocus
		''''''            Exit Sub
		''''''        End If
		''''''        SiwaNo = 0
		''''''    End If
		'入力チェック
		'UPGRADE_WARNING: オブジェクト fpSpd.TabIndex の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		If Item_Check(fpSpd.TabIndex) = False Then
			Exit Sub
		End If
		PreviousControl = Me.ActiveControl
		'ボタン名設定
		'UPGRADE_ISSUE: Control Name は、汎用名前空間 ActiveControl 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
		Call SetUpFuncs(Me.ActiveControl.Name)
		'スプレッドコメント表示
		'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト fpSpd.ActiveCol の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Call Comment_spd(fpSpd.ActiveCol, fpSpd.ActiveRow)
		'ホイールコントロール制御開始
		'UPGRADE_WARNING: オブジェクト fpSpd の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Call StartWheel(fpSpd)
	End Sub
	
	Private Sub fpSpd_LostFocus()
		'ホイールコントロール制御解除
		Call EndWheel()
	End Sub
	
	Private Sub fpSpd_KeyDown(ByRef KeyCode As Short, ByRef Shift As Short)
		Dim fpSpd As Object
		'うまくいかない
		''''    Select Case KeyCode
		''''        Case vbKeyDelete, vbKeyBack
		''''            With fpSpd
		''''                .Col = .ActiveCol
		''''                .Row = .ActiveRow
		''''                If (.SelLength = Len(.SelText)) And (.Lock = False) _
		'''''                    And (.CellType = CellTypeInteger _
		'''''                    Or .CellType = CellTypeFloat) Then
		''''                    'セル内のデータのみをクリアします。
		''''                    .Action = ActionClearText
		''''                End If
		''''            End With
		''''    End Select
		'SHIFT+左矢印で先頭に戻る
		If KeyCode = System.Windows.Forms.Keys.Left And Shift = VB6.ShiftConstants.ShiftMask Then
			With fpSpd
				'初期位置
				'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.Col = Col見積区分
				'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.Row = .ActiveRow
				'UPGRADE_WARNING: オブジェクト fpSpd.Action の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.Action = FPSpreadADO.ActionConstants.ActionActiveCell
				KeyCode = 0
				'UPGRADE_WARNING: オブジェクト fpSpd.LeftCol の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.LeftCol = Col見積区分
			End With
		End If
		'SHIFT+右矢印で仕分数1に進む
		If KeyCode = System.Windows.Forms.Keys.Right And Shift = VB6.ShiftConstants.ShiftMask Then
			With fpSpd
				'初期位置
				'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.Col = Col仕分数1
				'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.Row = .ActiveRow
				'UPGRADE_WARNING: オブジェクト fpSpd.Action の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.Action = FPSpreadADO.ActionConstants.ActionActiveCell
				KeyCode = 0
			End With
		End If
		'SHIFT+上矢印で先頭に戻る
		If KeyCode = System.Windows.Forms.Keys.Up And Shift = VB6.ShiftConstants.ShiftMask Then
			With fpSpd
				'UPGRADE_WARNING: オブジェクト fpSpd.DataRowCnt の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト fpSpd.ActiveCol の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				Call fpSpd_LeaveCell(.ActiveCol, .ActiveRow, Col見積区分, .DataRowCnt, False)
				'初期位置
				'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.Col = Col見積区分
				'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.Row = 1
				'UPGRADE_WARNING: オブジェクト fpSpd.Action の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.Action = FPSpreadADO.ActionConstants.ActionActiveCell
				KeyCode = 0
			End With
		End If
		'SHIFT+下矢印で最終行に戻る
		If KeyCode = System.Windows.Forms.Keys.Down And Shift = VB6.ShiftConstants.ShiftMask Then
			With fpSpd
				'UPGRADE_WARNING: オブジェクト fpSpd.DataRowCnt の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト fpSpd.ActiveCol の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				Call fpSpd_LeaveCell(.ActiveCol, .ActiveRow, Col見積区分, .DataRowCnt, False)
				'初期位置
				'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.Col = Col見積区分
				'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト fpSpd.DataRowCnt の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.Row = .DataRowCnt
				'UPGRADE_WARNING: オブジェクト fpSpd.Action の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.Action = FPSpreadADO.ActionConstants.ActionActiveCell
				KeyCode = 0
			End With
		End If
		
	End Sub
	
	Private Sub fpSpd_KeyPress(ByRef KeyAscii As Short)
		Dim fpSpd As Object
		Dim check As Boolean
		Dim getdata As Object
		Const Numbers As String = "0123456789@" ' 入力許可文字
		
		If KeyAscii = System.Windows.Forms.Keys.Return Then KeyAscii = 0
		
		With fpSpd
			'UPGRADE_WARNING: オブジェクト fpSpd.ActiveCol の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			Select Case .ActiveCol
				Case 1
					'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.Col = 1
					'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.Row = 1
					cbFunc(11).Text = "ﾁｪｯｸ"
				Case Col見積区分
					'コメント「C」のみ入力可とする
					If KeyAscii <> System.Windows.Forms.Keys.Back Then ' バックスペースは例外
						'''                    If InStr("ACS", Chr(KeyAscii)) = 0 Then                 '2004/12/01
						If InStr("ACS123456789", Chr(KeyAscii)) = 0 Then '2005/07/04 UPD
							KeyAscii = 0 ' 入力を無効にする
							Exit Sub
						End If
					End If
					'2016/06/22 ADD↓
				Case Colクレーム区分
					If KeyAscii <> System.Windows.Forms.Keys.Back Then ' バックスペースは例外
						If InStr("1", Chr(KeyAscii)) = 0 Then
							KeyAscii = 0 ' 入力を無効にする
							Exit Sub
						End If
					End If
					'2016/06/22 ADD↑
				Case ColPC区分, Col製品NO
					'製品情報選択
					'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					'UPGRADE_WARNING: オブジェクト fpSpd.ActiveCol の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					'UPGRADE_WARNING: オブジェクト fpSpd.GetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					check = .GetText(.ActiveCol, .ActiveRow, getdata)
					If check Then
						'UPGRADE_WARNING: オブジェクト fpSpd.SelLength の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						'UPGRADE_WARNING: オブジェクト fpSpd.SelStart の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						If KeyAscii = Asc(" ") And (.SelStart = 0 And .SelLength = Len(getdata)) Then
							KeyAscii = 0
							SelSeiInfChk.ResParentForm = Me
							VB6.ShowForm(SelSeiInfChk, VB6.FormShowConstants.Modal, Me)
							
							System.Windows.Forms.Application.DoEvents()
							'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							'UPGRADE_WARNING: オブジェクト fpSpd.ActiveCol の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.Col = .ActiveCol
							'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.Row = .ActiveRow
							'UPGRADE_WARNING: オブジェクト fpSpd.Action の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.Action = FPSpreadADO.ActionConstants.ActionActiveCell
							If DspSelData Then
								[cbFunc](11).Text = "ﾁｪｯｸ"
							End If
							'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							'UPGRADE_WARNING: オブジェクト fpSpd.GetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							check = fpSpd.GetText(ColPC区分, .Row, HoldPc) '2004/02/05 ADD
							'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							'UPGRADE_WARNING: オブジェクト fpSpd.GetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							check = fpSpd.GetText(Col製品NO, .Row, HoldSei) '2004/02/05 ADD
						End If
					End If
				Case ColW To ColH2 'サイズ
					'数字検査をする
					If KeyAscii <> System.Windows.Forms.Keys.Back Then ' バックスペースは例外
						If InStr(Numbers, Chr(KeyAscii)) = 0 Then
							KeyAscii = 0 ' 入力を無効にする
							Exit Sub
						End If
						'＠の制御
						'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						'UPGRADE_WARNING: オブジェクト fpSpd.ActiveCol の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						fpSpd.Col = fpSpd.ActiveCol
						'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						fpSpd.Row = fpSpd.ActiveRow
						'UPGRADE_WARNING: オブジェクト fpSpd.Text の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						'UPGRADE_WARNING: オブジェクト fpSpd.SelText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						If fpSpd.SelText = fpSpd.Text Then Exit Sub
						'UPGRADE_WARNING: オブジェクト fpSpd.Text の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						If fpSpd.Text <> "" Then '何かが入っている
							If Chr(KeyAscii) = "@" Then '＠が入力された
								KeyAscii = 0
								Exit Sub
							Else '数字が入力された
								'UPGRADE_WARNING: オブジェクト fpSpd.Text の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								If InStr(fpSpd.Text, "@") <> 0 Then '＠が入っている
									KeyAscii = 0
									Exit Sub
								End If
							End If
						End If
					End If
					'2017/03/10 ADD↓
				Case Col仕入業者CD
					'仕入先（仕入先選択画面）
					'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					'UPGRADE_WARNING: オブジェクト fpSpd.ActiveCol の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					'UPGRADE_WARNING: オブジェクト fpSpd.GetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					check = .GetText(.ActiveCol, .ActiveRow, getdata)
					If check Then
						'UPGRADE_WARNING: オブジェクト fpSpd.SelLength の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						'UPGRADE_WARNING: オブジェクト fpSpd.SelStart の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						If KeyAscii = Asc(" ") And (.SelStart = 0 And .SelLength = Len(getdata)) Then
							KeyAscii = 0
							
							If cSiiresaki.ShowDialog = True Then
								'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								'UPGRADE_WARNING: オブジェクト fpSpd.ActiveCol の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.SetText(.ActiveCol, .ActiveRow, cSiiresaki.仕入先CD)
								SendReturnKey()
								[cbFunc](11).Text = "ﾁｪｯｸ"
							Else
								'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								'UPGRADE_WARNING: オブジェクト fpSpd.ActiveCol の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.Col = .ActiveCol
								'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.Row = .ActiveRow
								'UPGRADE_WARNING: オブジェクト fpSpd.Action の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.Action = FPSpreadADO.ActionConstants.ActionActiveCell
							End If
						End If
					End If
					'2017/03/10 ADD↑
					
				Case Col仕入先CD
					'仕入先（仕入先選択画面）
					'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					'UPGRADE_WARNING: オブジェクト fpSpd.ActiveCol の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					'UPGRADE_WARNING: オブジェクト fpSpd.GetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					check = .GetText(.ActiveCol, .ActiveRow, getdata)
					If check Then
						'UPGRADE_WARNING: オブジェクト fpSpd.SelLength の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						'UPGRADE_WARNING: オブジェクト fpSpd.SelStart の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						If KeyAscii = Asc(" ") And (.SelStart = 0 And .SelLength = Len(getdata)) Then
							KeyAscii = 0
							
							If cSiiresaki.ShowDialog = True Then
								'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								'UPGRADE_WARNING: オブジェクト fpSpd.ActiveCol の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.SetText(.ActiveCol, .ActiveRow, cSiiresaki.仕入先CD)
								SendReturnKey()
								[cbFunc](11).Text = "ﾁｪｯｸ"
							Else
								'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								'UPGRADE_WARNING: オブジェクト fpSpd.ActiveCol の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.Col = .ActiveCol
								'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.Row = .ActiveRow
								'UPGRADE_WARNING: オブジェクト fpSpd.Action の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.Action = FPSpreadADO.ActionConstants.ActionActiveCell
							End If
							
							'''                        tx_Dummy1.MaxLength = TokuIDLength
							'''                        Set SirSen.ResCodeCTL = tx_Dummy1
							'''                        SirSen.Show vbModal, Me
							'''                        If tx_Dummy1.Tag <> "" Then
							'''                            .SetText .ActiveCol, .ActiveRow, tx_Dummy1
							'''                            SendReturnKey
							'''                            cbFunc(11).Caption = "ﾁｪｯｸ"
							'''                        Else
							'''                            .Col = .ActiveCol
							'''                            .Row = .ActiveRow
							'''                            .Action = ActionActiveCell
							'''                        End If
						End If
					End If
				Case Col送り先CD
					'送り先（配送先選択画面）
					'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					'UPGRADE_WARNING: オブジェクト fpSpd.ActiveCol の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					'UPGRADE_WARNING: オブジェクト fpSpd.GetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					check = .GetText(.ActiveCol, .ActiveRow, getdata)
					If check Then
						'UPGRADE_WARNING: オブジェクト fpSpd.SelLength の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						'UPGRADE_WARNING: オブジェクト fpSpd.SelStart の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						If KeyAscii = Asc(" ") And (.SelStart = 0 And .SelLength = Len(getdata)) Then
							KeyAscii = 0
							
							If cHaisosaki.ShowDialog = True Then
								'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								'UPGRADE_WARNING: オブジェクト fpSpd.ActiveCol の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.SetText(.ActiveCol, .ActiveRow, cHaisosaki.配送先CD)
								SendReturnKey()
								[cbFunc](11).Text = "ﾁｪｯｸ"
							Else
								'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								'UPGRADE_WARNING: オブジェクト fpSpd.ActiveCol の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.Col = .ActiveCol
								'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.Row = .ActiveRow
								'UPGRADE_WARNING: オブジェクト fpSpd.Action の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.Action = FPSpreadADO.ActionConstants.ActionActiveCell
							End If
							
							'''                        KeyAscii = 0
							'''                        tx_Dummy1.MaxLength = 2
							'''                        Set HaiSen.ResCodeCTL = tx_Dummy1
							'''                        HaiSen.Show vbModal, Me
							'''                        If tx_Dummy1.Tag <> "" Then
							'''                            .SetText .ActiveCol, .ActiveRow, tx_Dummy1
							'''                            SendReturnKey
							'''                            cbFunc(11).Caption = "ﾁｪｯｸ"
							'''                        Else
							'''                            .Col = .ActiveCol
							'''                            .Row = .ActiveRow
							'''                            .Action = ActionActiveCell
							'''                        End If
						End If
					End If
					'---2005/10/14.ADD
				Case ColU区分
					'                If InStr("U", Chr$(KeyAscii)) = 0 Then
					If InStr("URBH", Chr(KeyAscii)) = 0 Then '2017/07/03 ADD
						KeyAscii = 0
					End If
					'2016/06/22 ADD↓
				Case Col作業区分CD
					''                If InStr("1234", Chr$(KeyAscii)) = 0 Then '2016/06/21 ADD '2017/02/06 ADD
					'''                If InStr("124", Chr$(KeyAscii)) = 0 Then '2022/09/02 ADD
					If InStr("1234", Chr(KeyAscii)) = 0 Then '2023/04/04 ADD
						KeyAscii = 0
					End If
					'2016/06/22 ADD↑
			End Select
		End With
	End Sub
	
	Private Sub fpSpd_LeaveCell(ByVal Col As Integer, ByVal Row As Integer, ByVal NewCol As Integer, ByVal NewRow As Integer, ByRef Cancel As Boolean)
		Dim fpSpd As Object
		Dim check As Boolean 'Cell取り出しチェックフラグ
		Dim getdata As Object 'Cell取り出し用
		Dim chkPc, getPc As Object
		Dim chkSei, getSei As Object
		Dim chkSiy, getSiy As Object
		
		Dim bufName As String '名前取得用バッファ
		Dim bufBase As String
		Dim bufW As Short
		Dim bufD As Short
		Dim bufH As Short
		Dim bufD1 As Short
		Dim bufD2 As Short
		Dim bufTANI As String
		
		'UPGRADE_ISSUE: Control Name は、汎用名前空間 ActiveControl 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
		If Me.ActiveControl.Name = "cbClose" Then Exit Sub
		
		With fpSpd
			'入力された情報を取得
			'UPGRADE_WARNING: オブジェクト fpSpd.GetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			check = .GetText(Col, Row, getdata)
			
			Select Case Col
				'2010/01/05 ADD↓
				Case Col見積区分
					Call Calc合計()
					'2010/01/05 ADD↑
				Case ColPC区分 '3
					If check = False Then
						'UPGRADE_WARNING: オブジェクト HoldPc の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						'UPGRADE_WARNING: オブジェクト getdata の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						If getdata <> HoldPc Then
							'UPGRADE_WARNING: オブジェクト fpSpd.GetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							'UPGRADE_WARNING: オブジェクト chkSei の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							chkSei = .GetText(Col製品NO, Row, getSei)
							'UPGRADE_WARNING: オブジェクト chkSei の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							If chkSei <> False Then
								''                            .SetText 4, Row, ""
								''                            .SetText 5, Row, ""
								''                            .SetText 6, Row, ""
								''                            .SetText 7, Row, ""
								''                            .SetText 8, Row, ""
								''                            .SetText 9, Row, ""
								''                            .SetText 10, Row, ""
								''                            .SetText 11, Row, ""
								''                            .SetText 12, Row, ""
								''                            .SetText TeikaCol, Row, ""
								''                            .SetText TeikaCol + 1, Row, ""
								''                            .SetText TeikaCol + 3, Row, ""
								''                            .SetText 21, Row, ""
								''                            .SetText SiireCol, Row, ""
								''                            '原価・売価
								''                            Call Get率(Row, 15)
								''                            Call Get率(Row, 17)
								''                            Call Get金額(Row)
								''                            Call Calc合計
								'UPGRADE_WARNING: オブジェクト fpSpd.GetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								'UPGRADE_WARNING: オブジェクト chkSiy の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								chkSiy = .GetText(Col仕様NO, Row, getSiy)
								'UPGRADE_WARNING: オブジェクト getSiy の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								'UPGRADE_WARNING: オブジェクト getSei の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								'UPGRADE_WARNING: オブジェクト getdata の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								Call Entry_Chk(0, CShort(Row), CStr(getdata), CStr(getSei), CStr(getSiy))
								'原価・売価
								Call Get率(Row, Col原価)
								Call Get率(Row, Col売価)
								Call Get金額(Row)
								Call Calc合計()
							Else
							End If
						End If
					Else
						'UPGRADE_WARNING: オブジェクト HoldPc の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						'UPGRADE_WARNING: オブジェクト getdata の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						If getdata = HoldPc Then
							'UPGRADE_WARNING: オブジェクト getdata の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.SetText(Col, Row, "" & getdata)
						Else
							'UPGRADE_WARNING: オブジェクト fpSpd.GetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							'UPGRADE_WARNING: オブジェクト chkSei の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							chkSei = .GetText(Col製品NO, Row, getSei)
							'UPGRADE_WARNING: オブジェクト chkSei の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							If chkSei = False Then
							Else
								'UPGRADE_WARNING: オブジェクト fpSpd.GetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								'UPGRADE_WARNING: オブジェクト chkSiy の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								chkSiy = .GetText(Col仕様NO, Row, getSiy)
								'UPGRADE_WARNING: オブジェクト getSiy の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								'UPGRADE_WARNING: オブジェクト getSei の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								'UPGRADE_WARNING: オブジェクト getdata の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								Call Entry_Chk(0, CShort(Row), CStr(getdata), CStr(getSei), CStr(getSiy))
								'原価・売価
								Call Get率(Row, Col原価)
								Call Get率(Row, Col売価)
								Call Get金額(Row)
								Call Calc合計()
							End If
						End If
					End If
				Case Col製品NO '4
					'値が変わったら
					If check = False Then
						'UPGRADE_WARNING: オブジェクト HoldSei の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						'UPGRADE_WARNING: オブジェクト getdata の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						If getdata <> HoldSei Then
							'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.SetText(Col製品NO, Row, "")
							'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.SetText(Col仕様NO, Row, "")
							'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.SetText(Colベース色, Row, "")
							'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.SetText(Col名称, Row, "")
							'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.SetText(ColW, Row, "")
							'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.SetText(ColD, Row, "")
							'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.SetText(ColH, Row, "")
							'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.SetText(ColD1, Row, "")
							'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.SetText(ColD2, Row, "")
							'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.SetText(ColH1, Row, "")
							'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.SetText(ColH2, Row, "")
							'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.SetText(Colエラー内容, Row, "")
							'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.SetText(Col定価, Row, "")
							'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.SetText(ColU区分, Row, "") '2005/10/14
							'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.SetText(Col原価, Row, "")
							'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.SetText(Col売価, Row, "")
							'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.SetText(Col単位, Row, "")
							'2017/03/10 ADD↓
							'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.SetText(Col仕入業者CD, Row, "")
							'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.SetText(Col仕入業者名, Row, "")
							'2017/03/10 ADD↑
							'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.SetText(Col仕入先CD, Row, "")
							'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.SetText(Col仕入先名, Row, "")
							
							'原価・売価
							Call Get率(Row, Col原価)
							Call Get率(Row, Col売価)
							Call Get金額(Row)
							Call Calc合計()
							'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.SetText(Col製品区分, Row, "") '2003/11/05
							'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.SetText(Col社内在庫参照, Row, "") '2010/01/27 ADD
							'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.SetText(Col客先在庫参照, Row, "") '2010/01/27 ADD
							'2015/10/26 ADD↓
							'名称をロック解除する。
							'UPGRADE_WARNING: オブジェクト fpSpd.BlockMode の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.BlockMode = True
							'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.Row = Row
							'UPGRADE_WARNING: オブジェクト fpSpd.Row2 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.Row2 = Row
							'                        .Col = Col名称 '2015/11/13 DEL
							'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.Col = ColW '2015/11/13 ADD
							'UPGRADE_WARNING: オブジェクト fpSpd.Col2 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.Col2 = ColH2
							'UPGRADE_WARNING: オブジェクト fpSpd.Lock の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.Lock = False
							'UPGRADE_WARNING: オブジェクト fpSpd.BlockMode の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.BlockMode = False
							'2015/10/26 ADD↑
							
						End If
					Else
						'UPGRADE_WARNING: オブジェクト HoldSei の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						'UPGRADE_WARNING: オブジェクト getdata の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						If getdata = HoldSei Then
							'UPGRADE_WARNING: オブジェクト getdata の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.SetText(Col, Row, "" & getdata)
						Else
							'UPGRADE_WARNING: オブジェクト fpSpd.GetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							'UPGRADE_WARNING: オブジェクト chkPc の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							chkPc = .GetText(ColPC区分, Row, getPc)
							'UPGRADE_WARNING: オブジェクト fpSpd.GetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							'UPGRADE_WARNING: オブジェクト chkSiy の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							chkSiy = .GetText(Col仕様NO, Row, getSiy)
							'UPGRADE_WARNING: オブジェクト getSiy の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							'UPGRADE_WARNING: オブジェクト getdata の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							'UPGRADE_WARNING: オブジェクト getPc の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							Call Entry_Chk(0, CShort(Row), CStr(getPc), CStr(getdata), CStr(getSiy))
							'原価・売価
							Call Get率(Row, Col原価)
							Call Get率(Row, Col売価)
							Call Get金額(Row)
							Call Calc合計()
						End If
					End If
				Case Col仕様NO '5
					'UPGRADE_WARNING: オブジェクト fpSpd.GetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					'UPGRADE_WARNING: オブジェクト chkPc の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					chkPc = .GetText(ColPC区分, Row, getPc)
					'UPGRADE_WARNING: オブジェクト fpSpd.GetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					'UPGRADE_WARNING: オブジェクト chkSei の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					chkSei = .GetText(Col製品NO, Row, getSei)
					'UPGRADE_WARNING: オブジェクト chkPc の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					'UPGRADE_WARNING: オブジェクト chkSei の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					If check = False And chkSei = False And chkPc = False Then
						'2015/10/26 ADD↓
						'名称をロック解除する。
						'UPGRADE_WARNING: オブジェクト fpSpd.BlockMode の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.BlockMode = True
						'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Row = Row
						'UPGRADE_WARNING: オブジェクト fpSpd.Row2 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Row2 = Row
						'.Col = Col名称
						'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Col = ColW '2015/11/13 ADD
						'UPGRADE_WARNING: オブジェクト fpSpd.Col2 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Col2 = ColH2
						'UPGRADE_WARNING: オブジェクト fpSpd.Lock の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Lock = False
						'UPGRADE_WARNING: オブジェクト fpSpd.BlockMode の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.BlockMode = False
						'2015/10/26 ADD↑
					Else
						'UPGRADE_WARNING: オブジェクト HoldSiy の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						'UPGRADE_WARNING: オブジェクト getdata の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						'UPGRADE_WARNING: オブジェクト HoldSei の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						'UPGRADE_WARNING: オブジェクト getSei の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						'UPGRADE_WARNING: オブジェクト HoldPc の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						'UPGRADE_WARNING: オブジェクト getPc の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						If getPc = HoldPc And getSei = HoldSei And getdata = HoldSiy Then
						Else
							'UPGRADE_WARNING: オブジェクト getdata の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							'UPGRADE_WARNING: オブジェクト getSei の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							'UPGRADE_WARNING: オブジェクト getPc の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							Call Entry_Chk(0, CShort(Row), CStr(getPc), CStr(getSei), CStr(getdata))
							'原価・売価
							Call Get率(Row, Col原価)
							Call Get率(Row, Col売価)
							Call Get金額(Row)
							Call Calc合計()
						End If
					End If
				Case Col定価
					'UPGRADE_WARNING: オブジェクト HoldCD の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					'UPGRADE_WARNING: オブジェクト getdata の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					If getdata <> HoldCD Then
						'原価・売価
						Call Get単価(Row, Col仕入率)
						Call Get単価(Row, Col売上率)
						Call Get金額(Row)
						Call Calc合計()
					End If
					'''            Case Col原価, Col売価
					'''                If getdata <> HoldCD Then
					'''                    '原価・売価
					'''                    Call Get率(Row, Col)
					'''                    Call Get金額(Row)
					'''                    Call Calc合計
					'''                End If
					'2008/01/23 ADD↓
				Case Col原価, Col売価
					'UPGRADE_WARNING: オブジェクト HoldCD の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					'UPGRADE_WARNING: オブジェクト getdata の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					If getdata <> HoldCD Then
						Select Case chk原価率(Row)
							Case False
								'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.Col = Col原価
								'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.Row = Row
								'UPGRADE_WARNING: オブジェクト fpSpd.ForeColor の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.ForeColor = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red)
								'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.Col = Col売価
								'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.Row = Row
								'UPGRADE_WARNING: オブジェクト fpSpd.ForeColor の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.ForeColor = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red)
								'''                            .SetFocus
								'''                            .SetText col, Row, "" & HoldCD        '元に戻す
								'''                            .col = .ActiveCol
								'''                            .Row = .ActiveRow
								'''                            .Action = ActionActiveCell
								'''                            Cancel = True
								'''                            Exit Sub
							Case True
								'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.Col = Col原価
								'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.Row = Row
								'UPGRADE_WARNING: オブジェクト fpSpd.ForeColor の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.ForeColor = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black)
								'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.Col = Col売価
								'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.Row = Row
								'UPGRADE_WARNING: オブジェクト fpSpd.ForeColor の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.ForeColor = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black)
						End Select
						'原価・売価
						Call Get率(Row, Col)
						Call Get金額(Row)
						Call Calc合計()
					End If
					'2008/01/23 ADD↑
				Case Col仕入率
					'UPGRADE_WARNING: オブジェクト HoldCD の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					'UPGRADE_WARNING: オブジェクト getdata の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					If getdata <> HoldCD Then
						'仕入％・売上％
						'ここでスプレッドにセットしてしまうとUNDOができない。
						'                    Call Get単価(Row, Col, False)
						'2008/01/23 ADD↓
						Select Case chk原価率(Row, Get単価(Row, Col仕入率, False))
							Case False
								'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.Col = Col原価
								'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.Row = Row
								'UPGRADE_WARNING: オブジェクト fpSpd.ForeColor の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.ForeColor = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red)
								'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.Col = Col売価
								'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.Row = Row
								'UPGRADE_WARNING: オブジェクト fpSpd.ForeColor の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.ForeColor = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red)
								'''                            .SetFocus
								'''                            .SetText col, Row, "" & HoldCD        '元に戻す
								'''                            .col = .ActiveCol
								'''                            .Row = .ActiveRow
								'''                            .Action = ActionActiveCell
								'''                            Cancel = True
								'''                            Exit Sub
							Case True
								'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.Col = Col原価
								'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.Row = Row
								'UPGRADE_WARNING: オブジェクト fpSpd.ForeColor の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.ForeColor = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black)
								'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.Col = Col売価
								'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.Row = Row
								'UPGRADE_WARNING: オブジェクト fpSpd.ForeColor の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.ForeColor = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black)
						End Select
						'2008/01/23 ADD↑
						
						Call Get単価(Row, Col)
						Call Get金額(Row)
						Call Calc合計()
					End If
				Case Col売上率
					'UPGRADE_WARNING: オブジェクト HoldCD の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					'UPGRADE_WARNING: オブジェクト getdata の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					If getdata <> HoldCD Then
						'仕入％・売上％
						'ここでスプレッドにセットしてしまうとUNDOができない。
						'                    Call Get単価(Row, Col, False)
						'2008/01/23 ADD↓
						Select Case chk原価率(Row,  , Get単価(Row, Col売上率, False))
							Case False
								'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.Col = Col原価
								'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.Row = Row
								'UPGRADE_WARNING: オブジェクト fpSpd.ForeColor の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.ForeColor = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red)
								'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.Col = Col売価
								'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.Row = Row
								'UPGRADE_WARNING: オブジェクト fpSpd.ForeColor の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.ForeColor = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red)
								'''                            .SetFocus
								'''                            .SetText col, Row, "" & HoldCD        '元に戻す
								'''                            .col = .ActiveCol
								'''                            .Row = .ActiveRow
								'''                            .Action = ActionActiveCell
								'''                            Cancel = True
								'''                            Exit Sub
							Case True
								'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.Col = Col原価
								'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.Row = Row
								'UPGRADE_WARNING: オブジェクト fpSpd.ForeColor の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.ForeColor = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black)
								'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.Col = Col売価
								'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.Row = Row
								'UPGRADE_WARNING: オブジェクト fpSpd.ForeColor の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.ForeColor = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black)
						End Select
						'2008/01/23 ADD↑
						
						Call Get単価(Row, Col)
						Call Get金額(Row)
						Call Calc合計()
					End If
					'2014/09/09 ADD↓
				Case Col社内在庫
					'UPGRADE_WARNING: オブジェクト HoldCD の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					'UPGRADE_WARNING: オブジェクト getdata の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					If getdata <> HoldCD Then
						''                    If CCur(NullToZero(clsSPD.GetTextEX(Col仕入済数, Row))) <> 0 Then
						'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						If CDec(NullToZero(clsSPD_Renamed.GetTextEX(Col仕入済CNT, Row))) <> 0 Then '2018/06/19 ADD
							'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							If CDec(NullToZero(clsSPD_Renamed.GetTextEX(Col発注数, Row))) < CDec(NullToZero(clsSPD_Renamed.GetTextEX(Col仕入済数, Row))) Then
								'UPGRADE_WARNING: オブジェクト fpSpd.SetFocus の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.SetFocus()
								CriticalAlarm("仕入済みです。")
								'UPGRADE_WARNING: オブジェクト HoldCD の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.SetText(Col, Row, "" & HoldCD) '元に戻す
								'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								'UPGRADE_WARNING: オブジェクト fpSpd.ActiveCol の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.Col = .ActiveCol
								'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.Row = .ActiveRow
								'UPGRADE_WARNING: オブジェクト fpSpd.Action の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.Action = FPSpreadADO.ActionConstants.ActionActiveCell
								Cancel = True
								SpreadErr = True
							End If
						End If
					End If
					'2014/09/09 ADD↑
				Case Col客先在庫, Col転用
					'UPGRADE_WARNING: オブジェクト HoldCD の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					'UPGRADE_WARNING: オブジェクト getdata の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					If getdata <> HoldCD Then
						'2014/09/09 ADD↓
						''                    If CCur(NullToZero(clsSPD.GetTextEX(Col仕入済数, Row))) <> 0 Then
						'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						If CDec(NullToZero(clsSPD_Renamed.GetTextEX(Col仕入済CNT, Row))) <> 0 Then '2018/06/19 ADD
							'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							If CDec(NullToZero(clsSPD_Renamed.GetTextEX(Col発注数, Row))) < CDec(NullToZero(clsSPD_Renamed.GetTextEX(Col仕入済数, Row))) Then
								'UPGRADE_WARNING: オブジェクト fpSpd.SetFocus の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.SetFocus()
								CriticalAlarm("仕入済みです。")
								'UPGRADE_WARNING: オブジェクト HoldCD の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.SetText(Col, Row, "" & HoldCD) '元に戻す
								'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								'UPGRADE_WARNING: オブジェクト fpSpd.ActiveCol の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.Col = .ActiveCol
								'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.Row = .ActiveRow
								'UPGRADE_WARNING: オブジェクト fpSpd.Action の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.Action = FPSpreadADO.ActionConstants.ActionActiveCell
								Cancel = True
								SpreadErr = True
							End If
						End If
						'2014/09/09 ADD↑
						
						'客先在庫・転用
						Call Get金額(Row)
						Call Calc合計()
					End If
					'2014/09/09 ADD↓
				Case Col発注調整数
					'UPGRADE_WARNING: オブジェクト HoldCD の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					'UPGRADE_WARNING: オブジェクト getdata の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					If getdata <> HoldCD Then
						'                If Row <> NewRow Then
						''                    If CCur(NullToZero(clsSPD.GetTextEX(Col仕入済数, Row))) <> 0 Then
						'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						If CDec(NullToZero(clsSPD_Renamed.GetTextEX(Col仕入済CNT, Row))) <> 0 Then '2018/06/19 ADD
							'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							If CDec(NullToZero(clsSPD_Renamed.GetTextEX(Col発注数, Row))) < CDec(NullToZero(clsSPD_Renamed.GetTextEX(Col仕入済数, Row))) Then
								'UPGRADE_WARNING: オブジェクト fpSpd.SetFocus の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.SetFocus()
								CriticalAlarm("仕入済みです。")
								'UPGRADE_WARNING: オブジェクト HoldCD の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.SetText(Col, Row, "" & HoldCD) '元に戻す
								'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								'UPGRADE_WARNING: オブジェクト fpSpd.ActiveCol の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.Col = .ActiveCol
								'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.Row = .ActiveRow
								'UPGRADE_WARNING: オブジェクト fpSpd.Action の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.Action = FPSpreadADO.ActionConstants.ActionActiveCell
								Cancel = True
								SpreadErr = True
							End If
						End If
						'                End If
						'2014/09/09 ADD↑
						
					End If
					
					'2014/09/09 ADD↓
				Case Col仕分数1 To Col仕分数1 + 29
					If check = False Then
					Else
						'UPGRADE_WARNING: オブジェクト HoldCD の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						'UPGRADE_WARNING: オブジェクト getdata の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						If getdata = HoldCD Then
						Else
							''                        If CCur(NullToZero(clsSPD.GetTextEX(Col仕入済数, Row))) <> 0 Then
							'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							If CDec(NullToZero(clsSPD_Renamed.GetTextEX(Col仕入済CNT, Row))) <> 0 Then '2018/06/19 ADD
								'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								If CDec(NullToZero(clsSPD_Renamed.GetTextEX(Col発注数, Row))) < CDec(NullToZero(clsSPD_Renamed.GetTextEX(Col仕入済数, Row))) Then
									'UPGRADE_WARNING: オブジェクト fpSpd.SetFocus の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
									.SetFocus()
									CriticalAlarm("仕入済みです。")
									'UPGRADE_WARNING: オブジェクト HoldCD の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
									'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
									.SetText(Col, Row, "" & HoldCD) '元に戻す
									'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
									'UPGRADE_WARNING: オブジェクト fpSpd.ActiveCol の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
									.Col = .ActiveCol
									'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
									'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
									.Row = .ActiveRow
									'UPGRADE_WARNING: オブジェクト fpSpd.Action の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
									.Action = FPSpreadADO.ActionConstants.ActionActiveCell
									Cancel = True
									SpreadErr = True
								End If
							End If
						End If
					End If
					'仕分数
					Call Get金額(Row)
					Call Calc合計()
					'2014/09/09 ADD↑
					
					'''''''            Case SiwakeCol To SiwakeCol + 30
					'''            Case Col仕分数1 To Col仕分数1 + 29
					'''                If getdata <> HoldCD Then
					'''                    '仕分数
					'''                    Call Get金額(Row)
					'''                    Call Calc合計
					'''                End If
					''''            Case SiireCol
					
					'2017/03/10 ADD↓
				Case Col仕入業者CD
					'値が変わったら
					If check = False Then
						'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.SetText(Col仕入業者名, Row, Get仕入先略称(getdata))
					Else
						'仕入先CD
						'UPGRADE_WARNING: オブジェクト HoldCD の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						'UPGRADE_WARNING: オブジェクト getdata の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						If getdata <> HoldCD Then
							
							'2017/02/10 ADD↓
							'社内伝で仕入先が三和倉庫はありえない。
							'UPGRADE_WARNING: オブジェクト getdata の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							If getdata = "3150" Then '
								'UPGRADE_WARNING: オブジェクト fpSpd.SetFocus の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.SetFocus()
								CriticalAlarm("指定できません。")
								'UPGRADE_WARNING: オブジェクト HoldCD の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.SetText(Col, Row, "" & HoldCD) '元に戻す
								'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								'UPGRADE_WARNING: オブジェクト fpSpd.ActiveCol の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.Col = .ActiveCol
								'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.Row = .ActiveRow
								'UPGRADE_WARNING: オブジェクト fpSpd.Action の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.Action = FPSpreadADO.ActionConstants.ActionActiveCell
								Cancel = True
								SpreadErr = True
							Else
								'2017/02/10 ADD↑
								
								If ISInt(getdata) Then
									'UPGRADE_WARNING: オブジェクト getdata の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
									getdata = VB6.Format(getdata, New String("0", 4))
									'UPGRADE_WARNING: オブジェクト getdata の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
									'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
									.SetText(Col仕入業者CD, Row, "" & getdata)
									'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
									.SetText(Col仕入業者名, Row, Get仕入先略称(getdata))
								End If
								'''''                            '2015/09/28 ADD↓
								'''''                            cUriSirTanka.Initialize
								'''''                            cUriSirTanka.得意先CD = [rf_得意先CD].Caption
								'''''                            cUriSirTanka.仕入先CD = getdata
								'''''                            cUriSirTanka.製品NO = clsSPD.GetTextEX(Col製品NO, Row)
								'''''                            cUriSirTanka.仕様NO = clsSPD.GetTextEX(Col仕様NO, Row)
								'''''
								'''''                            If cUriSirTanka.GetbyID = True Then
								'''''                                If [tx_大小口区分].Text = 0 Then
								'''''                                    .SetText Col原価, Row, Format$(NullToZero(cUriSirTanka.大口仕入単価, 0), "#")
								'''''                                    .SetText Col売価, Row, Format$(NullToZero(cUriSirTanka.大口売上単価, 0), "#")
								'''''                                Else
								'''''                                    .SetText Col原価, Row, Format$(NullToZero(cUriSirTanka.小口仕入単価, 0), "#")
								'''''                                    .SetText Col売価, Row, Format$(NullToZero(cUriSirTanka.小口売上単価, 0), "#")
								'''''                                End If
								'''''                            End If
								'''''                            '2015/09/28 ADD↑
								
							End If
						End If
						'2015/07/20 ADD↓
						'原価・売価
						Call Get率(Row, Col原価)
						Call Get金額(Row)
						Call Calc合計()
						'2015/07/20 ADD↑
					End If
					'2017/03/10 ADD↑
				Case Col仕入先CD
					'値が変わったら
					If check = False Then
						'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.SetText(Col仕入先名, Row, Get仕入先略称(getdata))
					Else
						'仕入先CD
						'UPGRADE_WARNING: オブジェクト HoldCD の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						'UPGRADE_WARNING: オブジェクト getdata の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						If getdata <> HoldCD Then
							
							'2017/02/10 ADD↓
							'社内伝で仕入先が三和倉庫はありえない。
							'UPGRADE_WARNING: オブジェクト getdata の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							If getdata = "3150" And HD_社内伝票扱い <> 0 Then '
								'UPGRADE_WARNING: オブジェクト fpSpd.SetFocus の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.SetFocus()
								CriticalAlarm("指定できません。")
								'UPGRADE_WARNING: オブジェクト HoldCD の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.SetText(Col, Row, "" & HoldCD) '元に戻す
								'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								'UPGRADE_WARNING: オブジェクト fpSpd.ActiveCol の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.Col = .ActiveCol
								'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.Row = .ActiveRow
								'UPGRADE_WARNING: オブジェクト fpSpd.Action の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.Action = FPSpreadADO.ActionConstants.ActionActiveCell
								Cancel = True
								SpreadErr = True
							Else
								'2017/02/10 ADD↑
								
								If ISInt(getdata) Then
									'UPGRADE_WARNING: オブジェクト getdata の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
									getdata = VB6.Format(getdata, New String("0", 4))
									'UPGRADE_WARNING: オブジェクト getdata の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
									'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
									.SetText(Col仕入先CD, Row, "" & getdata)
									'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
									.SetText(Col仕入先名, Row, Get仕入先略称(getdata))
									
									'2023/09/26 ADD↓
									'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
									.Col = 1
									'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
									.Row = Row
									'UPGRADE_WARNING: オブジェクト fpSpd.Col2 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
									'UPGRADE_WARNING: オブジェクト fpSpd.MaxCols の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
									.Col2 = .MaxCols
									'UPGRADE_WARNING: オブジェクト fpSpd.Row2 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
									.Row2 = Row
									'UPGRADE_WARNING: オブジェクト fpSpd.BlockMode の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
									.BlockMode = True
									'UPGRADE_WARNING: オブジェクト fpSpd.ForeColor の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
									.ForeColor = Getインボイス番号NGColor(getdata)
									'UPGRADE_WARNING: オブジェクト fpSpd.BlockMode の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
									.BlockMode = False
									'2023/09/26 ADD↑
								End If
								
								
								'2015/09/28 ADD↓
								cUriSirTanka.Initialize()
								cUriSirTanka.得意先CD = [rf_得意先CD].Text
								'UPGRADE_WARNING: オブジェクト getdata の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								cUriSirTanka.仕入先CD = getdata
								cUriSirTanka.製品NO = clsSPD_Renamed.GetTextEX(Col製品NO, Row)
								cUriSirTanka.仕様NO = clsSPD_Renamed.GetTextEX(Col仕様NO, Row)
								
								If cUriSirTanka.GetbyID = True Then
									If CDbl([tx_大小口区分].Text) = 0 Then
										'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
										'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
										.SetText(Col原価, Row, VB6.Format(NullToZero((cUriSirTanka.大口仕入単価), 0), "#"))
										'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
										'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
										.SetText(Col売価, Row, VB6.Format(NullToZero((cUriSirTanka.大口売上単価), 0), "#"))
									Else
										'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
										'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
										.SetText(Col原価, Row, VB6.Format(NullToZero((cUriSirTanka.小口仕入単価), 0), "#"))
										'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
										'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
										.SetText(Col売価, Row, VB6.Format(NullToZero((cUriSirTanka.小口売上単価), 0), "#"))
									End If
								End If
								'2015/09/28 ADD↑
								
								''''                        '2015/07/20 ADD↓
								''''                        cSirTanka.Initialize
								''''                        cSirTanka.仕入先CD = getdata
								''''                        cSirTanka.製品NO = clsSPD.GetTextEX(Col製品NO, Row)
								''''                        cSirTanka.仕様NO = clsSPD.GetTextEX(Col仕様NO, Row)
								''''
								''''                        If cSirTanka.GetbyID = True Then
								''''                            .SetText Col原価, Row, cSirTanka.仕入単価
								''''                        End If
								''''                        '2015/07/20 ADD↑
							End If
						End If
						'2015/07/20 ADD↓
						'原価・売価
						Call Get率(Row, Col原価)
						Call Get金額(Row)
						Call Calc合計()
						'2015/07/20 ADD↑
					End If
					'''            Case SiireCol + 1
				Case Col送り先CD
					'値が変わったら
					If check = False Then
						'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.SetText(Col送り先名, Row, Get配送先略称(getdata))
					Else
						'配送先CD
						'UPGRADE_WARNING: オブジェクト HoldCD の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						'UPGRADE_WARNING: オブジェクト getdata の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						If getdata <> HoldCD Then
							If ISInt(getdata) Then
								'UPGRADE_WARNING: オブジェクト getdata の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								getdata = VB6.Format(getdata, New String("0", 2))
								'UPGRADE_WARNING: オブジェクト getdata の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.SetText(Col送り先CD, Row, "" & getdata)
								'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.SetText(Col送り先名, Row, Get配送先略称(getdata))
							End If
						End If
					End If
					'-------------------
					'-----2005/10/14.ADD
				Case ColU区分
					'値が変わったら
					If check = False Then
					Else
						'''                    'U区分
						'''                    If getdata = "U" Then
						'''                        '行の背景色セット
						'''                        Call RowBackColorSet(1, Row)
						'''                    Else
						'''                        '行の背景色クリア
						'''                        Call RowBackColorSet(0, Row)
						'''                    End If
						'                    Call RowBackColorSet2(CStr(getdata), Row)
					End If
				Case Col作業区分CD
					'値が変わったら
					'                If check = False Then
					'UPGRADE_WARNING: オブジェクト HoldCD の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					'UPGRADE_WARNING: オブジェクト getdata の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					If getdata <> HoldCD Then
						'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.SetText(Col作業区分名, Row, ModKubuns.Get作業区分名(NullToZero(getdata, "")))
					End If
			End Select
		End With
		
		'UPGRADE_WARNING: オブジェクト fpSpd.GetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Call fpSpd.GetText(ColU区分, Row, getdata)
		'    Debug.Print "row:" & Row & "   newrow:" & NewRow
		'行の色を変える
		If Row <> NewRow Then
			With fpSpd
				'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.Col = -1
				'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.Row = Row
				'UPGRADE_WARNING: オブジェクト fpSpd.BackColor の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.BackColor = &HFFFFFFFF
				If NewRow > 0 Then
					''            If getdata = "U" Then   '2005/10/14.ADD
					''                '行の背景色セット
					''                Call RowBackColorSet(1, Row)
					''            Else
					''                .Col = -1
					''                .Row = NewRow
					''                .BackColor = &HFFFFC0
					''            End If
					'2017/07/03 ADD
					Call RowBackColorSet2(clsSPD_Renamed.GetTextEX(ColU区分, NewRow), NewRow)
				End If
				'2017/07/03 ADD
				Call RowBackColorSet2(clsSPD_Renamed.GetTextEX(ColU区分, Row), Row)
			End With
		End If
		
		'スプレッドコメント表示
		Call Comment_spd(NewCol, NewRow)
	End Sub
	
	'Private Sub Entry_Chk(KIRIWAKE As Integer, Row As Integer, PCKBN As String, SEIHNO As String, SIYONO As String)
	Private Sub Entry_Chk(ByRef KIRIWAKE As Short, ByRef Row As Integer, ByRef PCKBN As String, ByRef SEIHNO As String, ByRef SIYONO As String)
		Dim fpSpd As Object '2020/10/10 ADD
		'付加情報のセット
		
		'sw=0：通常(各マスタ存在時、項目セット)
		'sw=1：顧客テンプレート貼付(各サイズと仕入先を再セットしない)
		Dim check As Boolean 'Cell取り出しチェックフラグ
		Dim getdata As Object 'Cell取り出し用
		Dim chkPc, getPc As Object
		Dim chkSei, getSei As Object
		Dim chkSiy, getSiy As Object
		Dim chkW, getW As Object
		Dim chkD, getD As Object
		Dim chkH, getH As Object
		Dim chkD1, getD1 As Object
		Dim chkD2, getD2 As Object
		Dim chkGenka, getGenka As Object
		Dim chkBaika, getBaika As Object
		Dim chkSiire, getSiire As Object
		Dim chkHaiso, getHaiso As Object
		
		Dim SeihKB As Short
		Dim bufName As String '名前取得用バッファ
		Dim bufBase As String
		Dim bufW As Short
		Dim bufD As Short
		Dim bufH As Short
		Dim bufD1 As Short
		Dim bufD2 As Short
		Dim bufH1 As Short
		Dim bufH2 As Short
		Dim bufTeika As Decimal
		Dim bufGenka As Decimal
		Dim bufBaika As Decimal
		Dim bufTANI As String
		Dim bufSIRECD As String
		Dim bufUZeiKBN As Short
		Dim bufSZeiKBN As Short
		Dim bufHIYOUKBN As Short '2024/02/20 ADD
		
		With fpSpd
			SeihKB = Get製品区分(PCKBN, SEIHNO, SIYONO)
			Select Case SeihKB
				Case -1
					'エラーの時
					'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.SetText(Col製品区分, Row, "")
					''---2004/03/05 ADD
					If KIRIWAKE = 1 Then
						'配送先
						'UPGRADE_WARNING: オブジェクト fpSpd.GetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						'UPGRADE_WARNING: オブジェクト chkHaiso の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						chkHaiso = .GetText(Col送り先CD, Row, getHaiso)
						'UPGRADE_WARNING: オブジェクト chkHaiso の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						If chkHaiso = False Then
							'UPGRADE_WARNING: オブジェクト getHaiso の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							getHaiso = "01"
						End If
						'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.SetText(Col送り先CD, Row, getHaiso)
						'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.SetText(Col送り先名, Row, Get配送先略称(getHaiso))
					End If
					
					'2015/10/26 ADD↓
					'名称をロック解除する。
					'UPGRADE_WARNING: オブジェクト fpSpd.BlockMode の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.BlockMode = True
					'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.Row = Row
					'UPGRADE_WARNING: オブジェクト fpSpd.Row2 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.Row2 = Row
					'.Col = Col名称
					'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.Col = ColW '2015/11/13 ADD
					'UPGRADE_WARNING: オブジェクト fpSpd.Col2 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.Col2 = ColH2
					'UPGRADE_WARNING: オブジェクト fpSpd.Lock の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.Lock = False
					'UPGRADE_WARNING: オブジェクト fpSpd.BlockMode の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.BlockMode = False
					'2015/10/26 ADD↑
					
					''-----------------
				Case 0, 3
					'--製品Ｍ・PCＭ
					Select Case Get製品情報VDB(SeihKB, PCKBN, SEIHNO, SIYONO, bufName, bufBase, bufW, bufD, bufH, bufD1, bufD2, bufH1, bufH2, bufTeika, bufGenka, bufBaika, bufTANI, bufSIRECD, bufUZeiKBN, bufSZeiKBN, bufHIYOUKBN)
						
						Case 0
							'OKのとき
							'定価
							'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.SetText(Col定価, Row, IIf(bufTeika = 0, "", bufTeika))
							'原価
							'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.SetText(Col原価, Row, IIf(bufGenka = 0, "", bufGenka))
							'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.SetText(ColU区分, Row, "") '2005/10/14
							'売価
							'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.SetText(Col売価, Row, IIf(bufBaika = 0, "", bufBaika))
							'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.SetText(ColM, Row, "")
							'売上税区分
							'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.SetText(Col売上税区分, Row, bufUZeiKBN)
							'配送先
							'UPGRADE_WARNING: オブジェクト fpSpd.GetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							'UPGRADE_WARNING: オブジェクト chkHaiso の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							chkHaiso = .GetText(Col送り先CD, Row, getHaiso)
							'UPGRADE_WARNING: オブジェクト chkHaiso の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							If chkHaiso = False Then
								'UPGRADE_WARNING: オブジェクト getHaiso の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								getHaiso = "01"
							End If
							'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.SetText(Col送り先CD, Row, getHaiso)
							'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.SetText(Col送り先名, Row, Get配送先略称(getHaiso))
							'製品区分
							'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.SetText(Col製品区分, Row, SeihKB)
							'仕入先略称
							'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.SetText(Col仕入先名, Row, Get仕入先略称(bufSIRECD))
							
							If KIRIWAKE = 0 Then
								'選択画面、手入力の場合
								'ベース色
								'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.SetText(Colベース色, Row, bufBase)
								'名称
								'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.SetText(Col名称, Row, bufName)
								'単位
								'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.SetText(Col単位, Row, bufTANI)
								
								'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.SetText(ColW, Row, IIf(bufW = 0, "", bufW)) 'W
								'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.SetText(ColD, Row, IIf(bufD = 0, "", bufD)) 'D
								'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.SetText(ColH, Row, IIf(bufH = 0, "", bufH)) 'H
								'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.SetText(ColD1, Row, IIf(bufD1 = 0, "", bufD1)) 'D1
								'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.SetText(ColD2, Row, IIf(bufD2 = 0, "", bufD2)) 'D2
								'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.SetText(ColH1, Row, IIf(bufH1 = 0, "", bufH1)) 'H1
								'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.SetText(ColH2, Row, IIf(bufH2 = 0, "", bufH2)) 'H2
								'2017/03/10 ADD↓
								'仕入業者
								'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.SetText(Col仕入業者CD, Row, bufSIRECD)
								'仕入先略称
								'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.SetText(Col仕入業者名, Row, Get仕入先略称(bufSIRECD))
								'2017/03/10 ADD↑
								'仕入先
								'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.SetText(Col仕入先CD, Row, bufSIRECD)
							End If
							
							'単価マスタ
							Select Case Get単価DB(SeihKB, rf_得意先CD.Text, CShort([tx_大小口区分].Text), PCKBN, SEIHNO, SIYONO, bufGenka, bufBaika)
								Case 0
									'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
									.SetText(Col原価, Row, IIf(bufGenka = 0, "", bufGenka))
									'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
									.SetText(Col売価, Row, IIf(bufBaika = 0, "", bufBaika))
							End Select
							
							'2015/09/28 ADD↓
							cUriSirTanka.Initialize()
							cUriSirTanka.得意先CD = [rf_得意先CD].Text
							cUriSirTanka.仕入先CD = clsSPD_Renamed.GetTextEX(Col仕入先CD, Row) '2024/01/19 ADD
							'''''                        cUriSirTanka.仕入先CD = clsSPD.GetTextEX(Col仕入業者CD, Row) '2017/05/14 ADD '2024/01/19 DEL
							cUriSirTanka.製品NO = SEIHNO
							cUriSirTanka.仕様NO = SIYONO
							
							If cUriSirTanka.GetbyID = True Then
								If CDbl([tx_大小口区分].Text) = 0 Then
									'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
									'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
									.SetText(Col原価, Row, VB6.Format(NullToZero((cUriSirTanka.大口仕入単価), 0), "#"))
									'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
									'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
									.SetText(Col売価, Row, VB6.Format(NullToZero((cUriSirTanka.大口売上単価), 0), "#"))
								Else
									'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
									'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
									.SetText(Col原価, Row, VB6.Format(NullToZero((cUriSirTanka.小口仕入単価), 0), "#"))
									'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
									'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
									.SetText(Col売価, Row, VB6.Format(NullToZero((cUriSirTanka.小口売上単価), 0), "#"))
								End If
							End If
							'2015/09/28 ADD↑
							
							'2024/02/20 ADD↓
							'コード入力時、労務費の場合は作業区分を1:工事にする。
							'物件種別が2：メンテの場合は作業区分を2：コールセンターにする。
							If (bufHIYOUKBN = 1) Then
								'通常
								'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.SetText(Col作業区分CD, Row, 1)
								'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.SetText(Col作業区分名, Row, ModKubuns.Get作業区分名(CStr(1)))
							End If
							If (HD_業種区分 = 1) Then
								'内装
								'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.SetText(Col作業区分CD, Row, 3)
								'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.SetText(Col作業区分名, Row, ModKubuns.Get作業区分名(CStr(3)))
							End If
							If (HD_物件種別 = 2) Then
								'コールセンター
								'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.SetText(Col作業区分CD, Row, 2)
								'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.SetText(Col作業区分名, Row, ModKubuns.Get作業区分名(CStr(2)))
							End If
							'2024/02/20 ADD↑
							
							
							Call Lock_Seihin(Row, SEIHNO, SIYONO) '2017/02/10 ADD
							''''                        '2015/10/26 ADD↓
							''''                        '在庫管理する場合、名称をロックする。
							''''                        Dim cSeihin As clsSeihin
							''''                        Set cSeihin = New clsSeihin
							''''                        cSeihin.Initialize
							''''                        cSeihin.製品NO = SEIHNO
							''''                        cSeihin.仕様NO = SIYONO
							''''                        If cSeihin.GetbyID = True Then
							''''                            If cSeihin.在庫区分 = 0 Then
							''''                                .BlockMode = True
							''''                                .Row = Row
							''''                                .Row2 = Row
							''''                                '.Col = Col名称
							''''                                .Col = ColW '2015/11/13 ADD
							''''                                .Col2 = ColH2
							''''                                .Lock = True
							''''                                .BlockMode = False
							''''                            Else
							''''                                .BlockMode = True
							''''                                .Row = Row
							''''                                .Row2 = Row
							''''                                '.Col = Col名称
							''''                                .Col = ColW '2015/11/13 ADD
							''''                                .Col2 = ColH2
							''''                                .Lock = False
							''''                                .BlockMode = False
							''''                            End If
							''''                        Else
							''''                            .BlockMode = True
							''''                            .Row = Row
							''''                            .Row2 = Row
							''''                            '.Col = Col名称
							''''                            .Col = ColW '2015/11/13 ADD
							''''                            .Col2 = ColH2
							''''                            .Lock = False
							''''                            .BlockMode = False
							''''                        End If
							''''                        '2015/10/26 ADD↑
							
							
							'''                        '2015/07/20 ADD↓
							'''                        cSirTanka.Initialize
							'''                        cSirTanka.仕入先CD = clsSPD.GetTextEX(Col仕入先CD, Row)
							'''                        cSirTanka.製品NO = SEIHNO
							'''                        cSirTanka.仕様NO = SIYONO
							'''
							'''                        If cSirTanka.GetbyID = True Then
							'''                            .SetText Col原価, Row, cSirTanka.仕入単価
							'''                        End If
							'''                        '2015/07/20 ADD↑
							
							
							'''                        '2009/09/26 ADD↓ '2013/07/19 DEL
							'''                        If SeihKB = 0 Then  '製品時
							'''                            If cDspSyaZaiko.GetZaikoInfo(HD_納期S, HD_担当者CD, SEIHNO, SIYONO, CLng(NullToZero(HD_見積番号))) = False Then
							'''                                .SetText Col社内在庫参照, Row, ""
							'''                            Else
							'''                                .SetText Col社内在庫参照, Row, cDspSyaZaiko.合計在庫数
							'''                            End If
							'''                            'チェック時でよい？？？
							''''''                            If cDspSyaZaiko.合計在庫数 < cDspSyaZaiko.適正在庫数 Then
							''''''                                '適正在庫数を下回った場合赤くする。
							''''''                                .Col = Col社内在庫参照
							''''''                                .Row = Row
							''''''                                .ForeColor = vbRed
							''''''                            Else
							''''''                                .Col = Col社内在庫参照
							''''''                                .Row = Row
							''''''                                .ForeColor = vbBlack
							''''''                            End If
							'''
							'''                            If cDspKyakuZaiko.GetZaikoInfo(HD_納期S, HD_担当者CD, rf_得意先CD, SEIHNO, SIYONO, CLng(NullToZero(HD_見積番号))) = False Then
							'''                                .SetText Col客先在庫参照, Row, ""
							'''                            Else
							'''                                .SetText Col客先在庫参照, Row, cDspKyakuZaiko.合計在庫数
							'''                            End If
							'''                        End If
							'''                        '2009/09/26 ADD↑ '2013/07/19 DEL
						Case -1
							'Get製品情報VDBで見つからなかった場合
							'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.SetText(Colベース色, Row, "")
							'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.SetText(Col名称, Row, "")
							'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.SetText(ColW, Row, "")
							'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.SetText(ColD, Row, "")
							'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.SetText(ColH, Row, "")
							'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.SetText(ColD1, Row, "")
							'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.SetText(ColD2, Row, "")
							'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.SetText(ColH1, Row, "")
							'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.SetText(ColH2, Row, "")
							'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.SetText(Col定価, Row, "")
							'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.SetText(ColU区分, Row, "") '2005/10/14
							'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.SetText(Col原価, Row, "")
							'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.SetText(Col売価, Row, "")
							'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.SetText(ColM, Row, "")
							'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.SetText(Col単位, Row, "")
							'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.SetText(Col売上税区分, Row, "")
							'2017/03/10 ADD↓
							'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.SetText(Col仕入業者CD, Row, "")
							'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.SetText(Col仕入業者名, Row, "")
							'2017/03/10 ADD↑
							'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.SetText(Col仕入先CD, Row, "")
							'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.SetText(Col仕入先名, Row, "")
							''---2004/03/05.DEL
							''                        .SetText Col送り先CD, Row, ""
							''                        .SetText Col送り先名, Row, ""
							''---2004/03/05.ADD
							If KIRIWAKE = 1 Then
								'配送先
								'UPGRADE_WARNING: オブジェクト fpSpd.GetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								'UPGRADE_WARNING: オブジェクト chkHaiso の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								chkHaiso = .GetText(Col送り先CD, Row, getHaiso)
								'UPGRADE_WARNING: オブジェクト chkHaiso の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								If chkHaiso = False Then
									'UPGRADE_WARNING: オブジェクト getHaiso の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
									getHaiso = "01"
								End If
								'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.SetText(Col送り先CD, Row, getHaiso)
								'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.SetText(Col送り先名, Row, Get配送先略称(getHaiso))
							End If
							''-----------------
							'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.SetText(Col製品区分, Row, "")
							
							'2009/09/26 ADD↓
							'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.SetText(Col社内在庫参照, Row, "")
							'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.SetText(Col客先在庫参照, Row, "")
							'2009/09/26 ADD↑
							
							'2015/10/26 ADD↓
							'名称をロック解除する。
							'UPGRADE_WARNING: オブジェクト fpSpd.BlockMode の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.BlockMode = True
							'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.Row = Row
							'UPGRADE_WARNING: オブジェクト fpSpd.Row2 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.Row2 = Row
							'.Col = Col名称
							'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.Col = ColW '2015/11/13 ADD
							'UPGRADE_WARNING: オブジェクト fpSpd.Col2 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.Col2 = ColH2
							'UPGRADE_WARNING: オブジェクト fpSpd.Lock の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.Lock = False
							'UPGRADE_WARNING: オブジェクト fpSpd.BlockMode の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.BlockMode = False
							'2015/10/26 ADD↑
					End Select
				Case 1, 2
					'--品群Ｍ・ユニットＭ
					Select Case Get製品情報VDB(SeihKB, PCKBN, SEIHNO, SIYONO, bufName, bufBase, bufW, bufD, bufH, bufD1, bufD2, bufH1, bufH2, bufTeika, bufGenka, bufBaika, bufTANI, bufSIRECD, bufUZeiKBN, bufSZeiKBN, bufHIYOUKBN)
						
						Case 0
							''                        'ベース色
							''                        .SetText 6, Row, ""
							'漢字名称
							'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.SetText(Col名称, Row, bufName)
							'定価
							'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.SetText(Col定価, Row, "")
							'U区分
							'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.SetText(ColU区分, Row, "") '2005/10/14
							'原価
							'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.SetText(Col原価, Row, "")
							'売価
							'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.SetText(Col売価, Row, "")
							'M区分
							'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.SetText(ColM, Row, "")
							''                        '単位名
							''                        .SetText 21, Row, ""
							'売上税区分
							'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.SetText(Col売上税区分, Row, "")
							'配送先
							'UPGRADE_WARNING: オブジェクト fpSpd.GetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							'UPGRADE_WARNING: オブジェクト chkHaiso の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							chkHaiso = .GetText(Col送り先CD, Row, getHaiso)
							'UPGRADE_WARNING: オブジェクト chkHaiso の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							If chkHaiso = False Then
								'UPGRADE_WARNING: オブジェクト getHaiso の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								getHaiso = "01"
							End If
							'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.SetText(Col送り先CD, Row, getHaiso)
							'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.SetText(Col送り先名, Row, Get配送先略称(getHaiso))
							'製品区分
							'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.SetText(Col製品区分, Row, SeihKB)
							
							If KIRIWAKE = 0 Then
								'選択画面、手入力の場合
								'名称
								'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.SetText(Col名称, Row, bufName)
							End If
							'2009/09/26 ADD↓
							'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.SetText(Col社内在庫参照, Row, "")
							'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.SetText(Col客先在庫参照, Row, "")
							'2009/09/26 ADD↑
						Case -1
							'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.SetText(Colベース色, Row, "")
							'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.SetText(Col名称, Row, "")
							'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.SetText(ColW, Row, "")
							'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.SetText(ColD, Row, "")
							'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.SetText(ColH, Row, "")
							'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.SetText(ColD1, Row, "")
							'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.SetText(ColD2, Row, "")
							'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.SetText(ColH1, Row, "")
							'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.SetText(ColH2, Row, "")
							'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.SetText(Col定価, Row, "")
							'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.SetText(ColU区分, Row, "") '2005/10/14
							'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.SetText(Col原価, Row, "")
							'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.SetText(Col売価, Row, "")
							'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.SetText(ColM, Row, "")
							'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.SetText(Col単位, Row, "")
							'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.SetText(Col売上税区分, Row, "")
							'2017/03/10 ADD↓
							'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.SetText(Col仕入業者CD, Row, "")
							'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.SetText(Col仕入業者名, Row, "")
							'2017/03/10 ADD↑
							'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.SetText(Col仕入先CD, Row, "")
							'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.SetText(Col仕入先名, Row, "")
							''---2004/03/05.DEL
							''                        .SetText Col送り先CD, Row, ""
							''                        .SetText Col送り先名, Row, ""
							''---2004/03/05.ADD
							If KIRIWAKE = 1 Then
								'配送先
								'UPGRADE_WARNING: オブジェクト fpSpd.GetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								'UPGRADE_WARNING: オブジェクト chkHaiso の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								chkHaiso = .GetText(Col送り先CD, Row, getHaiso)
								'UPGRADE_WARNING: オブジェクト chkHaiso の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								If chkHaiso = False Then
									'UPGRADE_WARNING: オブジェクト getHaiso の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
									getHaiso = "01"
								End If
								'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.SetText(Col送り先CD, Row, getHaiso)
								'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
								.SetText(Col送り先名, Row, Get配送先略称(getHaiso))
							End If
							''-----------------
							'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.SetText(Col製品区分, Row, "")
							
							'2009/09/26 ADD↓
							'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.SetText(Col社内在庫参照, Row, "")
							'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.SetText(Col客先在庫参照, Row, "")
							'2009/09/26 ADD↑
					End Select
			End Select
		End With
	End Sub
	
	'Private Sub Lock_Seihin(Row As Integer, SEIHNO As String, SIYONO As String) '2017/02/10 ADD
	Private Sub Lock_Seihin(ByRef Row As Integer, ByRef SEIHNO As String, ByRef SIYONO As String)
		Dim fpSpd As Object '2020/10/10 ADD
		Dim cSeihin As clsSeihin
		With fpSpd
			'2015/10/26 ADD↓
			'在庫管理する場合、名称をロックする。
			cSeihin = New clsSeihin
			cSeihin.Initialize()
			cSeihin.製品NO = SEIHNO
			cSeihin.仕様NO = SIYONO
			cSeihin.isDo在庫管理 = clsSeihin.Type在庫管理.全て '2023/07/18 ADD
			If cSeihin.GetbyID = True Then
				'            If cSeihin.在庫区分 = 0 Then
				If cSeihin.サイズ変更区分 = 0 Then '2020/10/10 ADD
					'UPGRADE_WARNING: オブジェクト fpSpd.BlockMode の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.BlockMode = True
					'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.Row = Row
					'UPGRADE_WARNING: オブジェクト fpSpd.Row2 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.Row2 = Row
					'.Col = Col名称
					'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.Col = ColW '2015/11/13 ADD
					'UPGRADE_WARNING: オブジェクト fpSpd.Col2 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.Col2 = ColH2
					'UPGRADE_WARNING: オブジェクト fpSpd.Lock の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.Lock = True
					'UPGRADE_WARNING: オブジェクト fpSpd.BlockMode の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.BlockMode = False
				Else
					'UPGRADE_WARNING: オブジェクト fpSpd.BlockMode の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.BlockMode = True
					'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.Row = Row
					'UPGRADE_WARNING: オブジェクト fpSpd.Row2 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.Row2 = Row
					'.Col = Col名称
					'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.Col = ColW '2015/11/13 ADD
					'UPGRADE_WARNING: オブジェクト fpSpd.Col2 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.Col2 = ColH2
					'UPGRADE_WARNING: オブジェクト fpSpd.Lock の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.Lock = False
					'UPGRADE_WARNING: オブジェクト fpSpd.BlockMode の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.BlockMode = False
				End If
			Else
				'UPGRADE_WARNING: オブジェクト fpSpd.BlockMode の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.BlockMode = True
				'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.Row = Row
				'UPGRADE_WARNING: オブジェクト fpSpd.Row2 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.Row2 = Row
				'.Col = Col名称
				'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.Col = ColW '2015/11/13 ADD
				'UPGRADE_WARNING: オブジェクト fpSpd.Col2 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.Col2 = ColH2
				'UPGRADE_WARNING: オブジェクト fpSpd.Lock の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.Lock = False
				'UPGRADE_WARNING: オブジェクト fpSpd.BlockMode の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.BlockMode = False
			End If
			'2015/10/26 ADD↑
		End With
	End Sub
	
	Private Sub Comment_spd(ByRef Col As Integer, ByRef Row As Integer)
		Dim fpSpd As Object
		'IMEモードを「オフ」にする
		Call ImmOpenModeSet(Me.Handle.ToInt32)
		
		'スプレッドのコメントをだす
		Dim buf As String
		
		With fpSpd
			Select Case Col
				Case -1
				Case 1
					'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
					sb_Msg.Items.Item(1).Text = "チェック：展開する"
					'ボタン名設定
					Call SetUpFuncs("展開")
				Case Col見積区分
					'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
					sb_Msg.Items.Item(1).Text = "見積区分を入力して下さい。  コメント：A･C 空白：S"
					'ボタン名設定
					Call SetUpFuncs("見積区分")
				Case ColSP区分
					'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
					sb_Msg.Items.Item(1).Text = "ＳＰ区分を入力して下さい。"
					'ボタン名設定
					Call SetUpFuncs("SP区分")
				Case ColPC区分
					'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
					sb_Msg.Items.Item(1).Text = "ＰＣ区分を入力して下さい。　選択画面：Space"
					'ボタン名設定
					Call SetUpFuncs("PC区分")
				Case Col製品NO
					'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
					sb_Msg.Items.Item(1).Text = "製品NOを入力して下さい。　選択画面：Space"
					'ボタン名設定
					Call SetUpFuncs("製品NO")
				Case Col仕様NO
					'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
					sb_Msg.Items.Item(1).Text = "仕様NOを入力して下さい。"
					'ボタン名設定
					Call SetUpFuncs("仕様NO")
					'IMEモードを「半角カタカナ」にする
					Call ImmOpenModeSet(Me.Handle.ToInt32, Win32API_IME.ConversionMODE.HANKANA)
				Case Colベース色
					'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
					sb_Msg.Items.Item(1).Text = "ベース色を入力して下さい。"
					'ボタン名設定
					Call SetUpFuncs("ベース色")
					'IMEモードを「半角カタカナ」にする
					Call ImmOpenModeSet(Me.Handle.ToInt32, Win32API_IME.ConversionMODE.HANKANA)
				Case Col名称
					'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
					sb_Msg.Items.Item(1).Text = "名称を入力して下さい。"
					'ボタン名設定
					Call SetUpFuncs("名称")
					'IMEモードを「全角ひらがな」にする
					Call ImmOpenModeSet(Me.Handle.ToInt32, Win32API_IME.ConversionMODE.ZENHIRA)
					'''''            Case (TeikaCol + 1), (TeikaCol + 4)
					'''''                .Col = Col
					'''''                .Row = 0
					'''''                buf = .Text
					'''''                sb_Msg.Panels(1).Text = buf & "を入力して下さい。"
					'''''                'ボタン名設定
					'''''                Call SetUpFuncs(buf)
				Case Col単位
					'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
					sb_Msg.Items.Item(1).Text = "単位を入力して下さい。"
					'ボタン名設定
					Call SetUpFuncs("単位")
					'IMEモードを「全角ひらがな」にする
					Call ImmOpenModeSet(Me.Handle.ToInt32, Win32API_IME.ConversionMODE.ZENHIRA)
					'2017/03/10 ADD↓
				Case Col仕入業者CD
					'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
					sb_Msg.Items.Item(1).Text = "仕入業者を入力して下さい。　選択画面：Space"
					'ボタン名設定
					Call SetUpFuncs("仕入業者")
					'2017/03/10 ADD↑
				Case Col仕入先CD
					'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
					sb_Msg.Items.Item(1).Text = "仕入先を入力して下さい。　選択画面：Space"
					'ボタン名設定
					Call SetUpFuncs("仕入先")
				Case Col送り先CD
					'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
					sb_Msg.Items.Item(1).Text = "送り先を入力して下さい。　選択画面：Space"
					'ボタン名設定
					Call SetUpFuncs("送り先")
					'2018/05/03 ADD↓
				Case Col入庫日
					'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
					sb_Msg.Items.Item(1).Text = "入庫日を入力して下さい。　選択画面：Space"
					'ボタン名設定
					Call SetUpFuncs("入庫日")
					'2017/05/03 ADD↑
					'2020/09/16 ADD↓
				Case Col出庫日
					'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
					sb_Msg.Items.Item(1).Text = "出庫日を入力して下さい。　選択画面：Space"
					'ボタン名設定
					Call SetUpFuncs("出庫日")
					'2020/09/16 ADD↑
				Case Col社内在庫
					'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
					sb_Msg.Items.Item(1).Text = "社内在庫数を入力して下さい。　選択画面：Space"
					'ボタン名設定
					Call SetUpFuncs("社内在庫数")
				Case Col客先在庫
					'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
					sb_Msg.Items.Item(1).Text = "客先在庫数を入力して下さい。　選択画面：Space"
					'ボタン名設定
					Call SetUpFuncs("客先在庫数")
					''''            Case SiwakeCol To SiwakeCol + 30
				Case ColU区分
					'sb_Msg.Panels(1).Text = "原価が未確定の場合、Uを入力して下さい。"
					'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
					sb_Msg.Items.Item(1).Text = "原価未確定:U 売価未確定:B 注意書き:R 発注保留:H" '2017/07/03 ADD
					'ボタン名設定
					Call SetUpFuncs("U")
				Case ColM
					'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
					sb_Msg.Items.Item(1).Text = "単価を固定したい場合、Mを入力して下さい。"
					'ボタン名設定
					Call SetUpFuncs("M")
				Case Col作業区分CD
					'''                sb_Msg.Panels(1).Text = "作業区分を入力して下さい。（1:施工 2:コール 3:内装 4:ｸﾚｰﾑ）"
					''''                sb_Msg.Panels(1).Text = "作業区分を入力して下さい。（1:工事 2:コール 4:ｸﾚｰﾑ）" '2022/09/02 ADD
					'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
					sb_Msg.Items.Item(1).Text = "作業区分を入力して下さい。（1:工事 2:コール 3:内装 4:ｸﾚｰﾑ）" '2023/04/04 ADD
					'ボタン名設定
					Call SetUpFuncs(buf)
					'2019/12/12 ADD↓
				Case Col追番R
					'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
					sb_Msg.Items.Item(1).Text = "レビット取込用の場合、Rを入力して下さい。" '2017/12/12 ADD
					'ボタン名設定
					Call SetUpFuncs("追番R")
					'2019/12/12 ADD↑
				Case Col仕分数1 To Col仕分数1 + 29
					'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					fpSpd.Col = Col
					'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					fpSpd.Row = 0
					'UPGRADE_WARNING: オブジェクト fpSpd.Text の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					buf = fpSpd.Text
					'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
					sb_Msg.Items.Item(1).Text = buf & "を入力して下さい。"
					'ボタン名設定
					'                Call SetUpFuncs(Buf)
					Call SetUpFuncs("仕分数") '2017/03/30 ADD
				Case Else
					'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					fpSpd.Col = Col
					'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					fpSpd.Row = 0
					'UPGRADE_WARNING: オブジェクト fpSpd.Text の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					buf = fpSpd.Text
					'UPGRADE_WARNING: コレクション sb_Msg.Panels の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"' をクリックしてください。
					sb_Msg.Items.Item(1).Text = buf & "を入力して下さい。"
					'ボタン名設定
					Call SetUpFuncs(buf)
			End Select
		End With
	End Sub
	
	'UPGRADE_WARNING: イベント op_Disp.CheckedChanged は、フォームが初期化されたときに発生します。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"' をクリックしてください。
	Private Sub op_Disp_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles op_Disp.CheckedChanged
		If eventSender.Checked Then
			Dim Index As Short = op_Disp.GetIndex(eventSender)
			Dim fpSpd As Object
			'スプレッドの表示項目の変更
			'------------------------------------------
			'   UPDATE      2005/10/14  [U区分]項目追加
			'------------------------------------------
			'初期位置
			'UPGRADE_WARNING: オブジェクト fpSpd.ReDraw の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			fpSpd.ReDraw = False
			Select Case Index
				Case 0 '見積情報
					With fpSpd
						'UPGRADE_WARNING: オブジェクト fpSpd.LeftCol の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.LeftCol = 1
						'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Col = Colベース色
						'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.ColHidden = True
						'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Col = ColD1
						'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.ColHidden = True
						'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Col = ColD2
						'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.ColHidden = True
						'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Col = ColH1
						'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.ColHidden = True
						'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Col = ColH2
						'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.ColHidden = True
						'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Col = ColU区分
						'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.ColHidden = True
						'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Col = Col原価
						'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.ColHidden = True
						'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Col = Col仕入率
						'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.ColHidden = True
						'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Col = Col金額
						'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.ColHidden = True
						'2017/03/10 ADD↓
						'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Col = Col仕入業者CD
						'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.ColHidden = True
						'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Col = Col仕入業者名
						'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.ColHidden = True
						'2017/03/10 ADD↑
						'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Col = Col仕入先CD
						'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.ColHidden = True
						'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Col = Col仕入先名
						'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.ColHidden = True
						'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Col = Col送り先CD
						'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.ColHidden = True
						'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Col = Col送り先名
						'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.ColHidden = True
						'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Col = Colエラー内容
						'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.ColHidden = True
					End With
				Case 1 '製品情報
					With fpSpd
						'UPGRADE_WARNING: オブジェクト fpSpd.LeftCol の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.LeftCol = 1
						'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Col = Colベース色
						'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.ColHidden = False
						'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Col = ColD1
						'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.ColHidden = False
						'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Col = ColD2
						'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.ColHidden = False
						'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Col = ColH1
						'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.ColHidden = False
						'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Col = ColH2
						'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.ColHidden = False
						
						'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Col = ColU区分
						'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.ColHidden = True
						'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Col = Col原価
						'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.ColHidden = True
						'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Col = Col仕入率
						'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.ColHidden = True
						'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Col = Col金額
						'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.ColHidden = True
						'2017/03/10 ADD↓
						'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Col = Col仕入業者CD
						'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.ColHidden = True
						'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Col = Col仕入業者名
						'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.ColHidden = True
						'2017/03/10 ADD↑
						'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Col = Col仕入先CD
						'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.ColHidden = True
						'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Col = Col仕入先名
						'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.ColHidden = True
						'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Col = Col送り先CD
						'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.ColHidden = True
						'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Col = Col送り先名
						'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.ColHidden = True
						'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Col = Colエラー内容
						'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.ColHidden = True
					End With
				Case 2 '価格情報
					With fpSpd
						'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Col = Colベース色
						'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.ColHidden = True
						'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Col = ColD1
						'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.ColHidden = True
						'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Col = ColD2
						'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.ColHidden = True
						'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Col = ColH1
						'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.ColHidden = True
						'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Col = ColH2
						'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.ColHidden = True
						'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Col = ColU区分
						'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.ColHidden = False
						'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Col = Col原価
						'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.ColHidden = False
						'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Col = Col仕入率
						'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.ColHidden = False
						'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Col = Col金額
						'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.ColHidden = False
						'2017/03/10 ADD↓
						'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Col = Col仕入業者CD
						'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.ColHidden = True
						'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Col = Col仕入業者名
						'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.ColHidden = True
						'2017/03/10 ADD↑
						'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Col = Col仕入先CD
						'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.ColHidden = True
						'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Col = Col仕入先名
						'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.ColHidden = True
						'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Col = Col送り先CD
						'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.ColHidden = True
						'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Col = Col送り先名
						'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.ColHidden = True
						'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Col = Colエラー内容
						'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.ColHidden = True
					End With
				Case 3 '取引先情報
					With fpSpd
						'UPGRADE_WARNING: オブジェクト fpSpd.LeftCol の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.LeftCol = Col仕入先CD
						'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Col = Colベース色
						'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.ColHidden = True
						'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Col = ColD1
						'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.ColHidden = True
						'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Col = ColD2
						'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.ColHidden = True
						'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Col = ColH1
						'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.ColHidden = True
						'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Col = ColH2
						'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.ColHidden = True
						'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Col = ColU区分
						'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.ColHidden = True
						'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Col = Col原価
						'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.ColHidden = True
						'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Col = Col仕入率
						'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.ColHidden = True
						'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Col = Col金額
						'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.ColHidden = True
						'2017/03/10 ADD↓
						'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Col = Col仕入業者CD
						'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.ColHidden = False
						'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Col = Col仕入業者名
						'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.ColHidden = False
						'2017/03/10 ADD↑
						'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Col = Col仕入先CD
						'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.ColHidden = False
						'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Col = Col仕入先名
						'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.ColHidden = False
						'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Col = Col送り先CD
						'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.ColHidden = False
						'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Col = Col送り先名
						'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.ColHidden = False
						'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Col = Colエラー内容
						'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.ColHidden = True
					End With
				Case 4 'エラー情報
					With fpSpd
						'UPGRADE_WARNING: オブジェクト fpSpd.LeftCol の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.LeftCol = 1
						'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Col = Colベース色
						'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.ColHidden = False
						'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Col = ColD1
						'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.ColHidden = False
						'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Col = ColD2
						'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.ColHidden = False
						'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Col = ColH1
						'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.ColHidden = False
						'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Col = ColH2
						'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.ColHidden = False
						'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Col = ColU区分
						'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.ColHidden = True
						'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Col = Col原価
						'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.ColHidden = True
						'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Col = Col仕入率
						'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.ColHidden = True
						'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Col = Col金額
						'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.ColHidden = True
						'2017/03/10 ADD↓
						'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Col = Col仕入業者CD
						'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.ColHidden = False
						'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Col = Col仕入業者名
						'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.ColHidden = False
						'2017/03/10 ADD↑
						'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Col = Col仕入先CD
						'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.ColHidden = False
						'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Col = Col仕入先名
						'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.ColHidden = False
						'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Col = Col送り先CD
						'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.ColHidden = False
						'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Col = Col送り先名
						'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.ColHidden = False
						'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Col = Colエラー内容
						'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.ColHidden = False
						'''                '2008/01/31 ADD↓
						'''                .SetFocus
						'''                .Col = .ActiveCol
						'''                .Row = .ActiveRow
						'''                .Action = ActionActiveCell
						'''                '2008/01/31 ADD↑
					End With
				Case 5
					Call SpdRowDisp()
					
			End Select
			'UPGRADE_WARNING: オブジェクト fpSpd.ReDraw の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			fpSpd.ReDraw = True
			'元の位置に戻る
			''    PreviousControl.SetFocus
		End If
	End Sub
	
	Private Sub op_Disp_MouseUp(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.MouseEventArgs) Handles op_Disp.MouseUp
		Dim Button As Short = eventArgs.Button \ &H100000
		Dim Shift As Short = System.Windows.Forms.Control.ModifierKeys \ &H10000
		Dim X As Single = VB6.PixelsToTwipsX(eventArgs.X)
		Dim Y As Single = VB6.PixelsToTwipsY(eventArgs.Y)
		Dim Index As Short = op_Disp.GetIndex(eventSender)
		'元の位置に戻る
		PreviousControl.Focus()
	End Sub
	
	Private Function Get製品情報DB(ByVal PCKB As String, ByVal SEIHNO As String, ByVal SIYONO As String, ByRef SeihKBN As Short) As Short
		Dim rs As ADODB.Recordset
		Dim sql As String
		
		On Error GoTo Get製品情報DB_Err
		'マウスポインターを砂時計にする
		HourGlass(True)
		
		'---製品情報存在チェック
		sql = "SELECT 製品区分 " & "FROM TM製品情報 " & "WHERE PC区分 = '" & VB.Left(PCKB & Space(1), 1) & "' " & "AND 製品No = '" & VB.Left(SQLString(SEIHNO) & Space(7), 7) & "' " & "AND 仕様No = '" & VB.Left(SQLString(SIYONO) & Space(7), 7) & "'"
		
		rs = OpenRs(sql, Cn, ADODB.CursorTypeEnum.adOpenForwardOnly, ADODB.LockTypeEnum.adLockReadOnly)
		
		With rs
			If .EOF Then
				Get製品情報DB = -1
			Else
				SeihKBN = .Fields("製品区分").Value
				Get製品情報DB = .Fields("製品区分").Value
			End If
			''            Select Case ![製品区分]
			''                '製品
			''                Case 0
			''                    Get製品情報DB = -1
			''                '品群
			''                Case 1
			''                    Get製品情報DB = -2
			''                'ユニット
			''                Case 2
			''                    Get製品情報DB = -3
			''                'ＰＣ
			''                Case 3
			''                    Get製品情報DB = -4
			''            End Select
		End With
		ReleaseRs(rs)
		
		HourGlass(False)
		Exit Function
Get製品情報DB_Err: 
		MsgBox(Err.Number & " " & Err.Description)
		HourGlass(False)
	End Function
	
	''''''''''''Private Function Get製品DB(ByVal SEIHNO As String, ByVal SIYONO As String, _
	'''''''''''''            IDName As String, Base As String, SizeW As Integer, SizeD As Integer, _
	'''''''''''''            SizeH As Integer, SizeD1 As Integer, SizeD2 As Integer, TANIName As String) As Integer
	''''''''''''
	''''''''''''    Dim rs As adodb.Recordset, SQL As String
	''''''''''''
	''''''''''''    On Error GoTo Get製品DB_Err
	''''''''''''    'マウスポインターを砂時計にする
	''''''''''''    HourGlass True
	''''''''''''
	''''''''''''    SQL = "SELECT 漢字名称, ベース色, W, D, H, D1, D2, H1, H2, 単位名 " _
	'''''''''''''        & "FROM TM製品 " _
	'''''''''''''        & "WHERE 製品NO = '" & SQLString(Trim$(SEIHNO)) & "'" _
	'''''''''''''        & "AND 仕様NO = '" & SQLString(Trim$(SIYONO)) & "'" _
	'''''''''''''
	''''''''''''    Set rs = OpenRs(SQL, Cn, adOpenForwardOnly, adLockReadOnly)
	''''''''''''
	''''''''''''    With rs
	''''''''''''        If .EOF Then
	''''''''''''            Get製品DB = -1
	''''''''''''        Else
	''''''''''''            IDName = NullToZero(![漢字名称], vbNullString)
	''''''''''''            Base = NullToZero(![ベース色], vbNullString)
	''''''''''''            SizeW = NullToZero(![w], vbNullString)
	''''''''''''            SizeD = NullToZero(![d], vbNullString)
	''''''''''''            SizeH = NullToZero(![H], vbNullString)
	''''''''''''            SizeD1 = NullToZero(![D1], vbNullString)
	''''''''''''            SizeD2 = NullToZero(![D2], vbNullString)
	''''''''''''            SizeH1 = NullToZero(![H1], vbNullString)
	''''''''''''            SizeH2 = NullToZero(![H2], vbNullString)
	''''''''''''            TANIName = NullToZero(![単位名], vbNullString)
	''''''''''''            Get製品DB = 0
	''''''''''''        End If
	''''''''''''    End With
	''''''''''''    ReleaseRs rs
	''''''''''''
	''''''''''''    HourGlass False
	''''''''''''    Exit Function
	''''''''''''Get製品DB_Err:
	''''''''''''    MsgBox Err.Number & " " & Err.Description
	''''''''''''    HourGlass False
	''''''''''''End Function
	'''''''''
	'''''''''Private Function Get品群DB(ByVal HINGNO As String, IDName As String) As Integer
	'''''''''    Dim rs As adodb.Recordset, SQL As String
	'''''''''
	'''''''''    On Error GoTo Get品群DB_Err
	'''''''''    'マウスポインターを砂時計にする
	'''''''''    HourGlass True
	'''''''''
	'''''''''    SQL = "SELECT 品群名称 " _
	''''''''''        & "FROM TM品群 " _
	''''''''''        & "WHERE 品群NO = '" & SQLString(Trim$(HINGNO)) & "'" _
	''''''''''
	'''''''''    Set rs = OpenRs(SQL, Cn, adOpenForwardOnly, adLockReadOnly)
	'''''''''
	'''''''''    With rs
	'''''''''        If .EOF Then
	'''''''''            Get品群DB = -1
	'''''''''        Else
	'''''''''            IDName = NullToZero(![品群名称], vbNullString)
	'''''''''            Get品群DB = 0
	'''''''''        End If
	'''''''''    End With
	'''''''''    ReleaseRs rs
	'''''''''
	'''''''''    HourGlass False
	'''''''''    Exit Function
	'''''''''Get品群DB_Err:
	'''''''''    MsgBox Err.Number & " " & Err.Description
	'''''''''    HourGlass False
	'''''''''End Function
	'''''''''
	'''''''''Private Function GetユニットDB(ByVal UNITNO As String, IDName As String) As Integer
	'''''''''    Dim rs As adodb.Recordset, SQL As String
	'''''''''
	'''''''''    On Error GoTo GetユニットDB_Err
	'''''''''    'マウスポインターを砂時計にする
	'''''''''    HourGlass True
	'''''''''
	'''''''''    SQL = "SELECT ユニット名 " _
	''''''''''        & "FROM TMユニット " _
	''''''''''        & "WHERE ユニットNO = '" & SQLString(Trim$(UNITNO)) & "'" _
	''''''''''
	'''''''''    Set rs = OpenRs(SQL, Cn, adOpenForwardOnly, adLockReadOnly)
	'''''''''
	'''''''''    With rs
	'''''''''        If .EOF Then
	'''''''''            GetユニットDB = -1
	'''''''''        Else
	'''''''''            IDName = NullToZero(![ユニット名], vbNullString)
	'''''''''            GetユニットDB = 0
	'''''''''        End If
	'''''''''    End With
	'''''''''    ReleaseRs rs
	'''''''''
	'''''''''    HourGlass False
	'''''''''    Exit Function
	'''''''''GetユニットDB_Err:
	'''''''''    MsgBox Err.Number & " " & Err.Description
	'''''''''    HourGlass False
	'''''''''End Function
	'''''''''
	'''''''''Private Function GetPCDB(ByVal PCKUBN As String, ByVal SEIHNO As String, ByVal SIYONO As String, _
	''''''''''            IDName As String, SizeW As Integer, SizeD As Integer, SizeH As Integer, TANIName) As Integer
	'''''''''    Dim rs As adodb.Recordset, SQL As String
	'''''''''
	'''''''''    On Error GoTo GetPCDB_Err
	'''''''''    'マウスポインターを砂時計にする
	'''''''''    HourGlass True
	'''''''''
	'''''''''    SQL = "SELECT 漢字名称, W, D, H, 単位名 " _
	''''''''''        & "FROM TMPC " _
	''''''''''        & "WHERE PC区分 = '" & SQLString(Trim$(PCKUBN)) & "'" _
	''''''''''        & "AND 製品NO = '" & SQLString(Trim$(SEIHNO)) & "'"
	'''''''''
	'''''''''    Set rs = OpenRs(SQL, Cn, adOpenForwardOnly, adLockReadOnly)
	'''''''''
	'''''''''    With rs
	'''''''''        If .EOF Then
	'''''''''            GetPCDB = -1
	'''''''''        Else
	'''''''''            IDName = NullToZero(![漢字名称], vbNullString)
	'''''''''            SizeW = NullToZero(![w], vbNullString)
	'''''''''            SizeD = NullToZero(![d], vbNullString)
	'''''''''            SizeH = NullToZero(![H], vbNullString)
	'''''''''            TANIName = NullToZero(![単位名], vbNullString)
	'''''''''            GetPCDB = 0
	'''''''''        End If
	'''''''''    End With
	'''''''''    ReleaseRs rs
	'''''''''
	'''''''''    HourGlass False
	'''''''''    Exit Function
	'''''''''GetPCDB_Err:
	'''''''''    MsgBox Err.Number & " " & Err.Description
	'''''''''    HourGlass False
	'''''''''End Function
	
	Private Function Get単価DB(ByVal SeihKB As Short, ByVal TOKUCD As String, ByVal OKKubn As Short, ByVal PCKUBN As String, ByVal SEIHNO As String, ByVal SIYONO As String, ByRef Genka As Decimal, ByRef Baika As Decimal) As Short
		Dim rs As ADODB.Recordset
		Dim sql As String
		Dim whr As String
		
		On Error GoTo Get単価DB_Err
		'マウスポインターを砂時計にする
		HourGlass(True)
		
		Select Case SeihKB
			Case 0
				whr = " 製品区分 = 0" & " AND 製品NO = '" & SQLString(Trim(SEIHNO)) & "'" & " AND 仕様NO = '" & SQLString(Trim(SIYONO)) & "'"
			Case 1
			Case 2
			Case 3
				whr = " 製品区分 = 3" & " AND PC区分 = '" & SQLString(Trim(PCKUBN)) & "'" & " AND 製品NO = '" & SQLString(Trim(SEIHNO)) & "'"
		End Select
		
		sql = "SELECT 大口売上単価, 小口売上単価, 大口仕入単価, 小口仕入単価 " & "FROM TM単価 " & "WHERE 得意先CD = '" & SQLString(Trim(TOKUCD)) & "'" & " AND" & whr
		
		rs = OpenRs(sql, Cn, ADODB.CursorTypeEnum.adOpenForwardOnly, ADODB.LockTypeEnum.adLockReadOnly)
		
		With rs
			If .EOF Then
				Get単価DB = -1
			Else
				If OKKubn = 0 Then
					'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					Genka = NullToZero(.Fields("大口仕入単価"), 0)
					'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					Baika = NullToZero(.Fields("大口売上単価"), 0)
				Else
					'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					Genka = NullToZero(.Fields("小口仕入単価"), 0)
					'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					Baika = NullToZero(.Fields("小口売上単価"), 0)
				End If
				Get単価DB = 0
			End If
		End With
		ReleaseRs(rs)
		
		HourGlass(False)
		Exit Function
Get単価DB_Err: 
		MsgBox(Err.Number & " " & Err.Description)
		HourGlass(False)
	End Function
	
	Private Function Get製品情報VDB(ByVal SKUBN As Short, ByRef PCKUBN As String, ByVal SEIHNO As String, ByVal SIYONO As String, ByRef IDName As String, ByRef BaseCol As String, ByRef SizeW As Short, ByRef SizeD As Short, ByRef SizeH As Short, ByRef SizeD1 As Short, ByRef SizeD2 As Short, ByRef SizeH1 As Short, ByRef SizeH2 As Short, ByRef Teika As Decimal, ByRef Genka As Decimal, ByRef Baika As Decimal, ByRef TANIName As String, ByRef SIRECD As String, ByRef UZEIKBN As Short, ByRef SZEIKBN As Short, ByRef HIYOUKBN As Short) As Short
		Dim rs As ADODB.Recordset
		Dim sql As String
		Dim whr As String
		
		On Error GoTo Get製品情報VDB_Err
		'マウスポインターを砂時計にする
		HourGlass(True)
		
		Select Case SKUBN
			Case 0
				whr = " 製品区分 = 0" & " AND 製品NO = '" & SQLString(Trim(SEIHNO)) & "'" & " AND 仕様NO = '" & SQLString(Trim(SIYONO)) & "'" & " AND 廃盤FLG = 0" '2015/09/11 ADD
			Case 1
				whr = " 製品区分 = 1" & " AND 製品NO = '" & SQLString(Trim(SEIHNO)) & "'"
			Case 2
				whr = " 製品区分 = 2" & " AND 製品NO = '" & SQLString(Trim(SEIHNO)) & "'"
			Case 3
				whr = " 製品区分 = 3" & " AND PC区分 = '" & SQLString(Trim(PCKUBN)) & "'" & " AND 製品NO = '" & SQLString(Trim(SEIHNO)) & "'"
		End Select
		
		'''''''''    '2016/03/10 ADD↓
		'''''''''    '製品Z204拓の制御
		'''''''''    If GetIni("Disp", "VisibleTAKU", INIFile) = "FALSE" Then
		'''''''''        If whr <> "" Then
		'''''''''            whr = whr & " and "
		'''''''''        End If
		'''''''''        whr = whr & "NOT(製品NO = 'Z204')"
		'''''''''    End If
		'''''''''    '2016/03/10 ADD↑
		
		''    SQL = "SELECT ベース色, 名称, W, D, H, D1, D2," _
		'''        & " 定価, 仕入単価, 売上単価, 単位名, 仕入先 " _
		'''        & "FROM VM製品情報 " _
		'''        & "WHERE PC区分 = '" & SQLString(Trim$(PCKUBN)) & "'" _
		'''        & "AND 製品NO = '" & SQLString(Trim$(SEIHNO)) & "'" _
		'''        & "AND 仕様NO = '" & SQLString(Trim$(SIYONO)) & "'" _
		'''        & "AND 製品区分 = '" & SQLString(Trim$(SKUBN)) & "'"
		''''''    sql = "SELECT ベース色, 名称, W, D, H, D1, D2, H1, H2," _
		'''''''        & " 定価, 仕入単価, 売上単価, 単位名, 仕入先, 売上税区分, 仕入税区分 " _
		'''''''        & "FROM VM製品情報 " _
		'''''''        & "WHERE" & whr
		sql = "SELECT ベース色, 名称, W, D, H, D1, D2, H1, H2,"
		sql = sql & " 定価, 仕入単価, 売上単価, 単位名, 仕入先, 売上税区分, 仕入税区分 "
		sql = sql & ",費用区分" '2024/02/20 ADD
		sql = sql & " FROM VM製品情報 "
		sql = sql & " WHERE" & whr
		
		rs = OpenRs(sql, Cn, ADODB.CursorTypeEnum.adOpenForwardOnly, ADODB.LockTypeEnum.adLockReadOnly)
		
		With rs
			If .EOF Then
				Get製品情報VDB = -1
			Else
				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				IDName = NullToZero(.Fields("名称"), vbNullString)
				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				BaseCol = NullToZero(.Fields("ベース色"), vbNullString)
				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				SizeW = NullToZero(.Fields("W"), 0)
				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				SizeD = NullToZero(.Fields("D"), 0)
				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				SizeH = NullToZero(.Fields("H"), 0)
				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				SizeD1 = NullToZero(.Fields("D1"), 0)
				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				SizeD2 = NullToZero(.Fields("D2"), 0)
				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				SizeH1 = NullToZero(.Fields("H1"), 0)
				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				SizeH2 = NullToZero(.Fields("H2"), 0)
				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				Teika = NullToZero(.Fields("定価"), 0)
				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				Genka = NullToZero(.Fields("仕入単価"), 0)
				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				Baika = NullToZero(.Fields("売上単価"), 0)
				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				TANIName = NullToZero(.Fields("単位名"), vbNullString)
				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				SIRECD = NullToZero(.Fields("仕入先"), vbNullString)
				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				UZEIKBN = NullToZero(.Fields("売上税区分"), 0)
				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				SZEIKBN = NullToZero(.Fields("仕入税区分"), 0)
				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				HIYOUKBN = NullToZero(.Fields("費用区分"), 0) '2024/02/20 ADD
				Get製品情報VDB = 0
			End If
		End With
		ReleaseRs(rs)
		
		HourGlass(False)
		Exit Function
Get製品情報VDB_Err: 
		MsgBox(Err.Number & " " & Err.Description)
		HourGlass(False)
	End Function
	
	Private Function Upload_Chk() As Short
		Dim fpSpd As Object
		Dim rs As ADODB.Recordset
		'仕分名称チェック用
		Dim buf As String
		
		Dim w仕分数入力MAX列 As Integer '2017/05/10 ADD
		
		Upload_Chk = -1
		
		'UPGRADE_WARNING: オブジェクト fpSpd.DataRowCnt の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		If fpSpd.DataRowCnt = 0 Then
			CriticalAlarm("明細がありません。")
			'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			fpSpd.Col = fpSpd.ActiveRow
			'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			fpSpd.Row = fpSpd.ActiveRow
			'UPGRADE_WARNING: オブジェクト fpSpd.Action の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			fpSpd.Action = FPSpreadADO.ActionConstants.ActionActiveCell
			'UPGRADE_WARNING: オブジェクト fpSpd.SetFocus の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			fpSpd.SetFocus()
			Exit Function
		End If
		
		'2011/12/26 ADD↓
		If Cn Is Nothing Then
			'        If ApplicationInit = False Then
			If ApplicationInit(True) = False Then '2015/10/21 ADD 複数画面の場合抜けている？
				'UPGRADE_NOTE: オブジェクト Cn をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
				Cn = Nothing
				CLK2F = False '2015/07/17 ADD
				Exit Function
			End If
		End If
		'2011/12/26 ADD↑
		
		'マウスポインターを砂時計にする
		HourGlass(True)
		On Error GoTo Upload_Chk_err
		
		'Tmpテーブル作成
		''''    TMPTABLE = OutputTmp
		''    OutputTmp
		CreateTmp見積明細() '2013/03/13 ADD
		'Tmpテーブル書き込み
		If InsDataTmp = False Then
			GoTo Upload_Chk_Correct
			'Exit Function
		End If
		
		'--見積明細
		Dim cmd As New ADODB.Command
		Dim grs As ADODB.Recordset
		
		Cn.CommandTimeout = 0
		cmd.CommandTimeout = 0
		cmd.let_ActiveConnection(Cn)
		cmd.CommandText = "usp_MT0100員数UploadCheck" '2013/03/13 ADD
		cmd.CommandType = ADODB.CommandTypeEnum.adCmdStoredProc
		cmd.Parameters.Refresh()
		
		' それぞれのパラメータの値を指定する
		With cmd.Parameters
			.Item(0).Direction = ADODB.ParameterDirectionEnum.adParamReturnValue
			'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Item("@i見積番号").Value = SpcToNull(rf_見積番号, 0)
			.Item("@i見積日付").Value = VB6.Format(tx_見積日付.Text, "yyyy/mm/dd")
			.Item("@i得意先CD").Value = [rf_得意先CD].Text
			.Item("@i売上端数").Value = tx_売上端数.Text
			.Item("@i消費税端数").Value = tx_消費税端数.Text
			.Item("@i受注区分").Value = tx_受注区分.Text
			.Item("@i大小口区分").Value = [tx_大小口区分].Text
			.Item("@CompName").Value = GetPCName
			.Item("@iRITUMAX").Value = gGenRituMax '2008/01/23 ADD
			.Item("@iRITUMIN").Value = gGenRituMin '2008/01/23 ADD
			
			.Item("@i売上納期").Value = (HD_納期S)
			.Item("@i担当者CD").Value = HD_担当者CD
			
			.Item("@i社内伝票扱い").Value = HD_社内伝票扱い '2018/07/31 ADD
			
			.Item("@i見積確定区分").Value = HD_見積確定区分 '2020/06/18 ADD
			'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Item("@i合計金額").Value = SpcToNull(tx_合計金額, 0) '2020/06/18 ADD
			
			
		End With
		Cn.CursorLocation = ADODB.CursorLocationEnum.adUseClient 'MoveLastを使用する場合
		
		''    Set Grs = New adodb.Recordset
		'---コマンド実行
		''    cmd.Execute
		grs = cmd.Execute
		'''    Dim a As Integer
		'''    Dim b As String
		'''    Set Grs = OpenRs("{ ? = call usp_MT0100員数UploadCheck('" & Format$(tx_見積日付, "yyyy/mm/dd") & "','" & rf_得意先CD & "'," & tx_売上端数 & "," & tx_消費税端数 & "," & tx_受注区分 & "," & tx_大小口区分 & ",'" & GetPCName & "'," & a & ",'" & b & "') }", Cn, adOpenKeyset, adLockOptimistic)
		''set rs = cmd.Execute
		Cn.CursorLocation = ADODB.CursorLocationEnum.adUseServer
		Dim a As Object
		For	Each a In Cn.Errors
			'UPGRADE_WARNING: オブジェクト a の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			Debug.Print("err:" & a)
		Next a
		If grs.State <> 0 Then
			If grs.EOF Then
				Upload_Chk = -1
				Inform("該当データ無し")
				GoTo Upload_Chk_Correct
			Else
				rs = grs
				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				Upload_Chk = NullToZero((rs.Fields("ErrCnt").Value), 0)
				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				gErrNo = NullToZero(cmd.Parameters("@wErr行番号"), "") '2005/03/26 ADD
				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				w仕分数入力MAX列 = NullToZero(cmd.Parameters("@w仕分MAX列"), 0) '2017/05/10 ADD
			End If
		Else
			Upload_Chk = -1
			CriticalAlarm((cmd.Parameters("@RetST").Value & " : " & cmd.Parameters("@RetMsg").Value))
			GoTo Upload_Chk_Correct
		End If
		
		'UPGRADE_NOTE: オブジェクト cmd をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
		cmd = Nothing
		
		'チェック後データ再表示
		Call SetCheckTempD(rs)
		'''    If SetCheckTempD(rs) = False Then
		'''        Upload_Chk = -1
		'''    End If
		
		'仕分名称チェック(受注区分が確定の場合)
		If Upload_Chk = 0 Then
			If HD_受注区分 = 1 Then
				'''        If HD_見積区分 = 1 Then             '2009/09/26 ADD
				'            Select Case Upload_SiwakeChk
				Select Case Upload_SiwakeChk(w仕分数入力MAX列) '2017/05/10 ADD
					Case 0
					Case -1
						Upload_Chk = -1
						Inform("仕分名称が設定されていません。")
					Case -2
						Upload_Chk = -1
						Inform("仕分内訳の納期が設定されていません。")
					Case -3
						Upload_Chk = -1
						Inform("仕分内訳の時間が設定されていません。")
						'2016/05/10 ADD↓
					Case -4
						Upload_Chk = -1
						Inform("仕分名称の数が入力と合っていません。")
						'2016/05/10 ADD↑
				End Select
			End If
		End If
		
		HourGlass(False)
		
Upload_Chk_Correct: 
		On Error GoTo 0
		
		'作業テーブル削除
		DropTmp見積明細() '2013/03/13 ADD
		
		HourGlass(False)
		Exit Function
		
Upload_Chk_err: '---エラー時
		MsgBox(Err.Number & " " & Err.Description)
		'作業テーブル削除
		DropTmp見積明細() '2013/03/13 ADD
		'UPGRADE_NOTE: オブジェクト Cn をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
		Cn = Nothing '2011/12/26 ADD
		Resume Upload_Chk_Correct
	End Function
	
	'Private Function Upload_SiwakeChk() As Integer
	Private Function Upload_SiwakeChk(ByRef w仕分数入力MAX列 As Integer) As Short
		Dim i As Short
		Dim j As Short
		
		Upload_SiwakeChk = 0
		
		'UPGRADE_WARNING: IsEmpty は、IsNothing にアップグレードされ、新しい動作が指定されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"' をクリックしてください。
		If IsNothing(gSiwakeTBL) Then
			Upload_SiwakeChk = -1
			Exit Function
		End If
		
		For j = 0 To UBound(gSiwakeTBL, 2)
			'納期
			If IsDate(gSiwakeTBL(2, j)) = False Then
				Upload_SiwakeChk = -2
				Exit Function
			End If
			'時間
			If IsDate(gSiwakeTBL(3, j)) = False Then
				Upload_SiwakeChk = -3
				Exit Function
			End If
		Next 
		
		If UBound(gSiwakeTBL, 2) + 1 < w仕分数入力MAX列 Then
			'仕分名称MAXのが少ないときエラー
			Upload_SiwakeChk = -4
			Exit Function
		End If
	End Function
	
	'''Private Sub SetCheckTempD(rs As adodb.Recordset)
	Private Function SetCheckTempD(ByRef rs As ADODB.Recordset) As Boolean
		Dim fpSpd As Object '2009/09/26 ADD
		'取り込んだシートと画面上のデータをマージしたデータを再表示する
		'チェックしたデータを再表示する。
		'------------------------------------------
		'   UPDATE      2005/10/14  [U区分]項目追加
		'                           U設定の行は黄色表示する
		'------------------------------------------
		Dim RecArry() As Object
		'    Dim i As Long, j As Integer
		Dim wLine As Integer
		Dim X As Integer
		
		'2015/10/26 ADD
		Dim cSeihin As clsSeihin
		cSeihin = New clsSeihin
		
		'初期値をTRUEにする。
		SetCheckTempD = True
		
		'シートのクリア
		Call clsSPD_Renamed.sprClearText()
		'行の色を初期化する
		With fpSpd
			'UPGRADE_WARNING: オブジェクト fpSpd.BlockMode の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.BlockMode = True
			'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Col = 1
			'UPGRADE_WARNING: オブジェクト fpSpd.Col2 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト fpSpd.MaxCols の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Col2 = .MaxCols
			'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Row = 1
			'UPGRADE_WARNING: オブジェクト fpSpd.Row2 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト fpSpd.MaxRows の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Row2 = .MaxRows
			'UPGRADE_WARNING: オブジェクト fpSpd.BackColor の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.BackColor = &HFFFFFF
			'UPGRADE_WARNING: オブジェクト fpSpd.BlockMode の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.BlockMode = False
		End With
		
		'UPGRADE_WARNING: オブジェクト fpSpd.AutoCalc の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		fpSpd.AutoCalc = False
		
		'表示行セット
		wLine = 1
		
		''    '2017/05/14 ADD
		''    RecArry = rs.GetRows(fpSpd.MaxRows, , _
		'''                Array("展開CHK", "見積区分", "クレーム明細区分", "他社納品日付", "他社伝票番号", "SP区分", "PC区分", "製品NO", "仕様NO", _
		'''                        "ベース色", "漢字名称", "W", "D", "H", "D1", "D2", "H1", "H2", "エラー内容", _
		'''                        "定価", "U区分", "仕入単価", "仕入率", "売上単価", "売上率", "M区分", "見積数量", "単位名", _
		'''                        "売上金額", "仕入金額", "売上税区分", "消費税額", "仕入業者CD", "仕入業者名", _
		'''                        "仕入先CD", "仕入先名", _
		'''                        "作業区分CD", "作業区分CD", "配送先CD", "配送先名", _
		'''                        "社内在庫数", "社内在庫数", "客先在庫数", "客先在庫数", "転用数", "発注調整数", "発注数", "総数量", _
		'''                        "仕分数1", "仕分数2", "仕分数3", "仕分数4", "仕分数5", _
		'''                        "仕分数6", "仕分数7", "仕分数8", "仕分数9", "仕分数10", _
		'''                        "仕分数11", "仕分数12", "仕分数13", "仕分数14", "仕分数15", _
		'''                        "仕分数16", "仕分数17", "仕分数18", "仕分数19", "仕分数20", _
		'''                        "仕分数21", "仕分数22", "仕分数23", "仕分数24", "仕分数25", _
		'''                        "仕分数26", "仕分数27", "仕分数28", "仕分数29", "仕分数30", _
		'''                        "製品区分", "見積明細連番", "仕入済数", "売上済CNT" _
		'''                        ))
		With fpSpd
			'UPGRADE_WARNING: オブジェクト fpSpd.ReDraw の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.ReDraw = False
			
			Do Until rs.EOF
				
				'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				fpSpd.SetText(Col展開, wLine, Trim(rs.Fields("展開CHK").Value & ""))
				
				'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				fpSpd.SetText(Col見積区分, wLine, Trim(rs.Fields("見積区分").Value & ""))
				'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				fpSpd.SetText(Colクレーム区分, wLine, VB6.Format(Trim(rs.Fields("クレーム明細区分").Value & ""), "#")) '2016/06/22 ADD
				
				'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				fpSpd.SetText(Col他社伝票番号, wLine, Trim(rs.Fields("他社伝票番号").Value & ""))
				'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				fpSpd.SetText(Col他社納品日付, wLine, Trim(rs.Fields("他社納品日付").Value & ""))
				
				'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				fpSpd.SetText(ColSP区分, wLine, Trim(rs.Fields("SP区分").Value & ""))
				'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				fpSpd.SetText(ColPC区分, wLine, Trim(rs.Fields("PC区分").Value & ""))
				
				'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				fpSpd.SetText(Col製品NO, wLine, Trim(rs.Fields("製品NO").Value & ""))
				'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				fpSpd.SetText(Col仕様NO, wLine, Trim(rs.Fields("仕様NO").Value & ""))
				'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				fpSpd.SetText(Colベース色, wLine, Trim(rs.Fields("ベース色").Value & ""))
				'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				fpSpd.SetText(Col名称, wLine, Trim(rs.Fields("漢字名称").Value & ""))
				
				'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				fpSpd.SetText(ColW, wLine, VB6.Format(rs.Fields("W").Value & "", "#"))
				'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				fpSpd.SetText(ColD, wLine, VB6.Format(rs.Fields("D").Value & "", "#"))
				'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				fpSpd.SetText(ColH, wLine, VB6.Format(rs.Fields("H").Value & "", "#"))
				
				'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				fpSpd.SetText(ColD1, wLine, VB6.Format(rs.Fields("D1").Value & "", "#"))
				'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				fpSpd.SetText(ColD2, wLine, VB6.Format(rs.Fields("D2").Value & "", "#"))
				'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				fpSpd.SetText(ColH1, wLine, VB6.Format(rs.Fields("H1").Value & "", "#"))
				'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				fpSpd.SetText(ColH2, wLine, VB6.Format(rs.Fields("H2").Value & "", "#"))
				
				'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				fpSpd.SetText(Colエラー内容, wLine, Trim(rs.Fields("エラー内容").Value & ""))
				'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				fpSpd.SetText(Col定価, wLine, IIf(Trim(rs.Fields("定価").Value & "") = "0", "", Trim(rs.Fields("定価").Value & "")))
				'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				fpSpd.SetText(ColU区分, wLine, Trim(rs.Fields("U区分").Value & ""))
				
				'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				fpSpd.SetText(Col原価, wLine, IIf(Trim(rs.Fields("仕入単価").Value & "") = "0", "", Trim(rs.Fields("仕入単価").Value & "")))
				'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				fpSpd.SetText(Col仕入率, wLine, IIf(Trim(rs.Fields("仕入率").Value & "") = "0", "", Trim(rs.Fields("仕入率").Value & "")))
				'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				fpSpd.SetText(Col売価, wLine, IIf(Trim(rs.Fields("売上単価").Value & "") = "0", "", Trim(rs.Fields("売上単価").Value & "")))
				'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				fpSpd.SetText(Col売上率, wLine, IIf(Trim(rs.Fields("売上率").Value & "") = "0", "", Trim(rs.Fields("売上率").Value & "")))
				
				'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				fpSpd.SetText(ColM, wLine, Trim(rs.Fields("M区分").Value & ""))
				
				'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				fpSpd.SetText(Col見積数量, wLine, IIf(Trim(rs.Fields("見積数量").Value & "") = "0", "", Trim(rs.Fields("見積数量").Value & "")))
				'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				fpSpd.SetText(Col単位, wLine, Trim(rs.Fields("単位名").Value & ""))
				
				'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				fpSpd.SetText(Col金額, wLine, IIf(Trim(rs.Fields("売上金額").Value & "") = "0", "", Trim(rs.Fields("売上金額").Value & "")))
				'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				fpSpd.SetText(Col仕入金額, wLine, IIf(Trim(rs.Fields("仕入金額").Value & "") = "0", "", Trim(rs.Fields("仕入金額").Value & "")))
				
				'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				fpSpd.SetText(Col売上税区分, wLine, Trim(rs.Fields("売上税区分").Value & ""))
				'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				fpSpd.SetText(Col消費税額, wLine, IIf(Trim(rs.Fields("消費税額").Value & "") = "0", "", Trim(rs.Fields("消費税額").Value & "")))
				
				'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				fpSpd.SetText(Col明細備考, wLine, Trim(rs.Fields("明細備考").Value & "")) '2018/05/03 ADD
				'            fpSpd.SetText Col入出庫日, wLine, Trim$(rs.Fields("入出庫日").Value & "") '2018/05/03 ADD
				'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				fpSpd.SetText(Col入庫日, wLine, VB6.Format(Trim(rs.Fields("入庫日").Value & ""), "yy/mm/dd")) '2018/05/03 ADD
				
				'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				fpSpd.SetText(Col出庫日, wLine, VB6.Format(Trim(rs.Fields("出庫日").Value & ""), "yy/mm/dd")) '2020/09/16 ADD
				
				'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				fpSpd.SetText(Col追番R, wLine, Trim(rs.Fields("追番R").Value & "")) '2019/12/12 ADD
				
				Select Case Trim(rs.Fields("見積区分").Value & "")
					Case "A", "C", "S"
						'2017/05/14 ADD↓
						'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						fpSpd.SetText(Col仕入業者CD, wLine, "")
						'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						fpSpd.SetText(Col仕入業者名, wLine, "")
						'2017/05/14 ADD↑
						'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						fpSpd.SetText(Col仕入先CD, wLine, "")
						'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						fpSpd.SetText(Col仕入先名, wLine, "")
						'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						fpSpd.SetText(Col送り先CD, wLine, "")
						'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						fpSpd.SetText(Col送り先名, wLine, "")
					Case Else
						'2017/05/14 ADD↓
						'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						fpSpd.SetText(Col仕入業者CD, wLine, Trim(rs.Fields("仕入業者CD").Value & ""))
						'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						fpSpd.SetText(Col仕入業者名, wLine, Trim(rs.Fields("仕入業者名").Value & ""))
						'2017/05/14 ADD↑
						'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						fpSpd.SetText(Col仕入先CD, wLine, Trim(rs.Fields("仕入先CD").Value & ""))
						'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						fpSpd.SetText(Col仕入先名, wLine, Trim(rs.Fields("仕入先名").Value & ""))
						'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						fpSpd.SetText(Col送り先CD, wLine, Trim(rs.Fields("配送先CD").Value & ""))
						'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						fpSpd.SetText(Col送り先名, wLine, Trim(rs.Fields("配送先名").Value & ""))
				End Select
				'2016/06/22 ADD↓
				'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				fpSpd.SetText(Col作業区分CD, wLine, VB6.Format(Trim(rs.Fields("作業区分CD").Value & ""), "#"))
				''        fpSpd.SetText Col作業区分名, wLine, Trim$(rs.Fields("作業区分名").Value & "")
				'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				fpSpd.SetText(Col作業区分名, wLine, ModKubuns.Get作業区分名(VB6.Format(Trim(rs.Fields("作業区分CD").Value & ""), "#")))
				'2016/06/22 ADD↑
				
				
				'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				fpSpd.SetText(Col社内在庫, wLine, IIf(Trim(rs.Fields("社内在庫数").Value & "") = "0", "", Trim(rs.Fields("社内在庫数").Value & "")))
				'        fpSpd.SetText Col社内在庫参照, wLine, IIf(Trim$(rs.Fields("社内在庫数").Value & "") = "0", "", Trim$(rs.Fields("社内在庫数").Value & ""))
				
				'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				fpSpd.SetText(Col客先在庫, wLine, IIf(Trim(rs.Fields("客先在庫数").Value & "") = "0", "", Trim(rs.Fields("客先在庫数").Value & "")))
				'        fpSpd.SetText Col客先在庫参照, wLine, IIf(Trim$(rs.Fields("客先在庫数").Value & "") = "0", "", Trim$(rs.Fields("客先在庫数").Value & ""))
				'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				fpSpd.SetText(Col転用, wLine, IIf(Trim(rs.Fields("転用数").Value & "") = "0", "", Trim(rs.Fields("転用数").Value & "")))
				
				'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				fpSpd.SetText(Col発注調整数, wLine, IIf(Trim(rs.Fields("発注調整数").Value & "") = "0", "", Trim(rs.Fields("発注調整数").Value & "")))
				'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				fpSpd.SetText(Col発注数, wLine, IIf(Trim(rs.Fields("発注数").Value & "") = "0", "", Trim(rs.Fields("発注数").Value & "")))
				'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				fpSpd.SetText(Col数量合計, wLine, IIf(Trim(rs.Fields("総数量").Value & "") = "0", "", Trim(rs.Fields("総数量").Value & "")))
				
				
				'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				fpSpd.SetText(Col製品区分, wLine, Trim(rs.Fields("製品区分").Value & ""))
				'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				fpSpd.SetText(Col見積明細連番, wLine, Trim(rs.Fields("見積明細連番").Value & ""))
				'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				fpSpd.SetText(Col仕入済数, wLine, Trim(rs.Fields("仕入済数").Value & ""))
				
				'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				fpSpd.SetText(Col仕入済CNT, wLine, Trim(rs.Fields("仕入済CNT").Value & "")) '2018/06/19 ADD
				'''            If Trim$(rs.Fields("仕入済数").Value & "") <> "0" Then
				If Trim(rs.Fields("仕入済CNT").Value & "") <> "0" Then '2018/06/19 ADD
					'仕入発生してたらロックして変更不可にする
					With fpSpd
						'UPGRADE_WARNING: オブジェクト fpSpd.BlockMode の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.BlockMode = True
						'ロック
						'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Col = Col展開
						'UPGRADE_WARNING: オブジェクト fpSpd.Col2 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Col2 = Col展開
						'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Row = wLine
						'UPGRADE_WARNING: オブジェクト fpSpd.Row2 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Row2 = wLine
						'UPGRADE_WARNING: オブジェクト fpSpd.Lock の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Lock = True
						
						'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Col = ColSP区分
						'UPGRADE_WARNING: オブジェクト fpSpd.Col2 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Col2 = Col定価
						'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Row = wLine
						'UPGRADE_WARNING: オブジェクト fpSpd.Row2 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Row2 = wLine
						'UPGRADE_WARNING: オブジェクト fpSpd.Lock の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Lock = True
						
						'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Col = Col原価
						'UPGRADE_WARNING: オブジェクト fpSpd.Col2 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Col2 = Col仕入率
						'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Row = wLine
						'UPGRADE_WARNING: オブジェクト fpSpd.Row2 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Row2 = wLine
						'UPGRADE_WARNING: オブジェクト fpSpd.Lock の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Lock = True
						
						'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Col = Col仕入業者CD '2017/03/10 ADD
						'.Col = Col仕入先CD
						'UPGRADE_WARNING: オブジェクト fpSpd.Col2 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Col2 = Col送り先CD
						'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Row = wLine
						'UPGRADE_WARNING: オブジェクト fpSpd.Row2 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Row2 = wLine
						'UPGRADE_WARNING: オブジェクト fpSpd.Lock の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Lock = True
						
						'明細備考はロック解除 2022/08/08 ADD
						'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Col = Col明細備考
						'UPGRADE_WARNING: オブジェクト fpSpd.Col2 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Col2 = Col明細備考
						'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Row = wLine
						'UPGRADE_WARNING: オブジェクト fpSpd.Row2 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Row2 = wLine
						'UPGRADE_WARNING: オブジェクト fpSpd.Lock の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Lock = False
						
						'UPGRADE_WARNING: オブジェクト fpSpd.BlockMode の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.BlockMode = False
					End With
					shiirezumiF = True
				End If
				'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				fpSpd.SetText(Col売上済CNT, wLine, Trim(rs.Fields("売上済CNT").Value & ""))
				If Trim(rs.Fields("売上済CNT").Value & "") <> "0" Then
					'仕入発生してたらロックして変更不可にする
					With fpSpd
						'UPGRADE_WARNING: オブジェクト fpSpd.BlockMode の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.BlockMode = True
						'全ての明細項目をロック
						'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Col = Col展開
						'UPGRADE_WARNING: オブジェクト fpSpd.Col2 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Col2 = Col売上済CNT
						'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Row = wLine
						'UPGRADE_WARNING: オブジェクト fpSpd.Row2 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Row2 = wLine
						'UPGRADE_WARNING: オブジェクト fpSpd.Lock の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Lock = True
						
						'明細備考はロック解除 2022/08/08 ADD
						'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Col = Col明細備考
						'UPGRADE_WARNING: オブジェクト fpSpd.Col2 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Col2 = Col明細備考
						'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Row = wLine
						'UPGRADE_WARNING: オブジェクト fpSpd.Row2 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Row2 = wLine
						'UPGRADE_WARNING: オブジェクト fpSpd.Lock の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Lock = False
						
						''''                    '2016/10/26 ADD↓
						''''                    .Col = Col他社納品日付
						''''                    .Col2 = Col他社伝票番号
						''''                    .Row = wLine
						''''                    .Row2 = wLine
						''''                    .Lock = False
						''''                    '2016/10/26 ADD↑
						
						'UPGRADE_WARNING: オブジェクト fpSpd.BlockMode の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.BlockMode = False
					End With
				End If
				
				'            fpSpd.SetText Col仕分数1, wLine, Format$(Trim$(rs.Fields("仕分数1").Value & ""), "#")
				For X = 1 To 30
					'rs.Fields("仕分数" & j) = NullToZero(WkTBLS(wLine, Col仕分数1 + j - 1))
					''                fpSpd.SetText Col仕分数1 + X - 1, wLine, Format$(Trim$(rs.Fields("仕分数" & X).Value & ""), "#")
					'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					fpSpd.SetText(Col仕分数1 + X - 1, wLine, IIf(Trim(rs.Fields("仕分数" & X).Value & "") = "0", "", Trim(rs.Fields("仕分数" & X).Value & ""))) '2017/06/27 ADD
					
				Next 
				
				Call Get率(wLine, Col原価)
				Call Get率(wLine, Col売価)
				Call Get金額(wLine)
				
				'2017/07/03 ADD
				Call RowBackColorSet2(clsSPD_Renamed.GetTextEX(ColU区分, CInt(wLine)), CInt(wLine))
				
				Call Lock_Seihin(wLine, clsSPD_Renamed.GetTextEX(Col製品NO, CInt(wLine)), clsSPD_Renamed.GetTextEX(Col仕様NO, CInt(wLine))) '2020/10/10 ADD
				'2020/10/10 DEL↓
				'''''            '2015/10/26 ADD↓
				'''''            '在庫管理する場合、名称をロックする。
				'''''            cSeihin.Initialize
				'''''            cSeihin.製品NO = clsSPD.GetTextEX(Col製品NO, CLng(wLine))
				'''''            cSeihin.仕様NO = clsSPD.GetTextEX(Col仕様NO, CLng(wLine))
				'''''            If cSeihin.GetbyID = True Then
				'''''                If cSeihin.在庫区分 = 0 Then
				'''''                    .BlockMode = True
				'''''                    .Row = wLine
				'''''                    .Row2 = wLine
				'''''                    '.Col = Col名称
				'''''                    .Col = ColW '2015/11/13 ADD
				'''''                    .Col2 = ColH2
				'''''                    .Lock = True
				'''''                    .BlockMode = False
				'''''                Else
				'''''                    .BlockMode = True
				'''''                    .Row = wLine
				'''''                    .Row2 = wLine
				'''''                    '.Col = Col名称
				'''''                    .Col = ColW '2015/11/13 ADD
				'''''                    .Col2 = ColH2
				'''''                    .Lock = False
				'''''                    .BlockMode = False
				'''''                End If
				'''''            Else
				'''''                .BlockMode = True
				'''''                .Row = wLine
				'''''                .Row2 = wLine
				'''''                '.Col = Col名称
				'''''                .Col = ColW '2015/11/13 ADD
				'''''                .Col2 = ColH2
				'''''                .Lock = False
				'''''                .BlockMode = False
				'''''            End If
				'''''            '2015/10/26 ADD↑
				'2020/10/10 DEL↑
				
				If clsSPD_Renamed.GetTextEX(Colエラー内容, CInt(wLine)) Like "原価率*" Then '2015/03/10 ADD
					'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.Col = Col原価
					'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.Row = wLine
					'UPGRADE_WARNING: オブジェクト fpSpd.ForeColor の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.ForeColor = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red)
					'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.Col = Col売価
					'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.Row = wLine
					'UPGRADE_WARNING: オブジェクト fpSpd.ForeColor の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.ForeColor = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red)
				Else
					'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.Col = Col原価
					'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.Row = wLine
					'UPGRADE_WARNING: オブジェクト fpSpd.ForeColor の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.ForeColor = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black)
					'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.Col = Col売価
					'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.Row = wLine
					'UPGRADE_WARNING: オブジェクト fpSpd.ForeColor の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.ForeColor = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black)
				End If
				
				'2023/09/26 ADD↓
				'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.Col = 1
				'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.Row = wLine
				'UPGRADE_WARNING: オブジェクト fpSpd.Col2 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト fpSpd.MaxCols の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.Col2 = .MaxCols
				'UPGRADE_WARNING: オブジェクト fpSpd.Row2 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.Row2 = wLine
				'UPGRADE_WARNING: オブジェクト fpSpd.BlockMode の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.BlockMode = True
				'UPGRADE_WARNING: オブジェクト fpSpd.ForeColor の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.ForeColor = Getインボイス番号NGColor(Trim(rs.Fields("仕入先CD").Value & ""))
				'UPGRADE_WARNING: オブジェクト fpSpd.BlockMode の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.BlockMode = False
				'2023/09/26 ADD↑
				
				
				'表示行カウント
				wLine = wLine + 1
				
				rs.MoveNext()
				'            i = i + 1
			Loop 
			'UPGRADE_WARNING: オブジェクト fpSpd.ReDraw の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.ReDraw = True
		End With
		
		Call Calc合計()
		
		'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト fpSpd.ActiveCol の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		fpSpd.Col = fpSpd.ActiveCol '2003/11/25 ADD
		'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		fpSpd.Row = fpSpd.ActiveRow
		'''    fpSpd.Col = 1
		'''    fpSpd.Row = 1
		'UPGRADE_WARNING: オブジェクト fpSpd.Action の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		fpSpd.Action = FPSpreadADO.ActionConstants.ActionActiveCell
		'UPGRADE_WARNING: オブジェクト fpSpd.AutoCalc の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		fpSpd.AutoCalc = True
	End Function
	''''''''
	'''''''''''Private Sub SetCheckTempD(rs As adodb.Recordset)
	''''''''Private Function SetCheckTempD(rs As ADODB.Recordset) As Boolean    '2009/09/26 ADD
	'''''''''取り込んだシートと画面上のデータをマージしたデータを再表示する
	'''''''''チェックしたデータを再表示する。
	'''''''''------------------------------------------
	'''''''''   UPDATE      2005/10/14  [U区分]項目追加
	'''''''''                           U設定の行は黄色表示する
	'''''''''------------------------------------------
	''''''''    Dim RecArry() As Variant
	''''''''    Dim i As Long, j As Integer
	''''''''
	''''''''    '初期値をTRUEにする。
	''''''''    SetCheckTempD = True
	''''''''
	''''''''    'シートのクリア
	''''''''    Call clsSPD.sprClearText
	''''''''    '行の色を初期化する
	''''''''    With fpSpd
	''''''''        .BlockMode = True
	''''''''            .Col = 1
	''''''''            .Col2 = .MaxCols
	''''''''            .Row = 1
	''''''''            .Row2 = .MaxRows
	''''''''            .BackColor = &HFFFFFF
	''''''''        .BlockMode = False
	''''''''    End With
	''''''''
	''''''''    fpSpd.AutoCalc = False
	'''''''''''    RecArry = rs.GetRows(fpSpd.MaxRows, , _
	'''''''''                Array("展開CHK", "見積区分", "SP区分", "PC区分", "製品NO", "仕様NO", _
	'''''''''                        "ベース色", "漢字名称", "W", "D", "H", "D1", "D2", "H1", "H2", "エラー内容", _
	'''''''''                        "定価", "U区分", "仕入単価", "仕入率", "売上単価", "売上率", "M区分", "見積数量", "単位名", _
	'''''''''                        "売上金額", "仕入金額", "売上税区分", "消費税額", "仕入先CD", "仕入先名", "配送先CD", "配送先名", _
	'''''''''                        "社内在庫数", "社内在庫数", "客先在庫数", "客先在庫数", "転用数", "発注数", "総数量", _
	'''''''''                        "仕分数1", "仕分数2", "仕分数3", "仕分数4", "仕分数5", _
	'''''''''                        "仕分数6", "仕分数7", "仕分数8", "仕分数9", "仕分数10", _
	'''''''''                        "仕分数11", "仕分数12", "仕分数13", "仕分数14", "仕分数15", _
	'''''''''                        "仕分数16", "仕分数17", "仕分数18", "仕分数19", "仕分数20", _
	'''''''''                        "仕分数21", "仕分数22", "仕分数23", "仕分数24", "仕分数25", _
	'''''''''                        "仕分数26", "仕分数27", "仕分数28", "仕分数29", "仕分数30", _
	'''''''''                        "製品区分", "見積明細連番" _
	'''''''''                        ))
	''''''''
	'''''''''''    RecArry = rs.GetRows(fpSpd.MaxRows, , _
	'''''''''                Array("展開CHK", "見積区分", "SP区分", "PC区分", "製品NO", "仕様NO", _
	'''''''''                        "ベース色", "漢字名称", "W", "D", "H", "D1", "D2", "H1", "H2", "エラー内容", _
	'''''''''                        "定価", "U区分", "仕入単価", "仕入率", "売上単価", "売上率", "M区分", "見積数量", "単位名", _
	'''''''''                        "売上金額", "仕入金額", "売上税区分", "消費税額", "仕入先CD", "仕入先名", "配送先CD", "配送先名", _
	'''''''''                        "社内在庫数", "社内在庫数", "客先在庫数", "客先在庫数", "転用数", "発注数", "総数量", _
	'''''''''                        "仕分数1", "仕分数2", "仕分数3", "仕分数4", "仕分数5", _
	'''''''''                        "仕分数6", "仕分数7", "仕分数8", "仕分数9", "仕分数10", _
	'''''''''                        "仕分数11", "仕分数12", "仕分数13", "仕分数14", "仕分数15", _
	'''''''''                        "仕分数16", "仕分数17", "仕分数18", "仕分数19", "仕分数20", _
	'''''''''                        "仕分数21", "仕分数22", "仕分数23", "仕分数24", "仕分数25", _
	'''''''''                        "仕分数26", "仕分数27", "仕分数28", "仕分数29", "仕分数30", _
	'''''''''                        "製品区分", "見積明細連番", _
	'''''''''                        "社内在庫数FLG", "社内適正在庫数FLG", "客先在庫数FLG" _
	'''''''''                        ))
	''''''''    '2014/09/09 ADD
	'''''''''''    RecArry = rs.GetRows(fpSpd.MaxRows, , _
	'''''''''                Array("展開CHK", "見積区分", "SP区分", "PC区分", "製品NO", "仕様NO", _
	'''''''''                        "ベース色", "漢字名称", "W", "D", "H", "D1", "D2", "H1", "H2", "エラー内容", _
	'''''''''                        "定価", "U区分", "仕入単価", "仕入率", "売上単価", "売上率", "M区分", "見積数量", "単位名", _
	'''''''''                        "売上金額", "仕入金額", "売上税区分", "消費税額", "仕入先CD", "仕入先名", "配送先CD", "配送先名", _
	'''''''''                        "社内在庫数", "社内在庫数", "客先在庫数", "客先在庫数", "転用数", "発注調整数", "発注数", "総数量", _
	'''''''''                        "仕分数1", "仕分数2", "仕分数3", "仕分数4", "仕分数5", _
	'''''''''                        "仕分数6", "仕分数7", "仕分数8", "仕分数9", "仕分数10", _
	'''''''''                        "仕分数11", "仕分数12", "仕分数13", "仕分数14", "仕分数15", _
	'''''''''                        "仕分数16", "仕分数17", "仕分数18", "仕分数19", "仕分数20", _
	'''''''''                        "仕分数21", "仕分数22", "仕分数23", "仕分数24", "仕分数25", _
	'''''''''                        "仕分数26", "仕分数27", "仕分数28", "仕分数29", "仕分数30", _
	'''''''''                        "製品区分", "見積明細連番", "仕入済数", "売上済CNT" _
	'''''''''                        ))
	''''''''    '2015/02/04 ADD
	'''''''''    RecArry = rs.GetRows(fpSpd.MaxRows, , _
	''''''''''                Array("展開CHK", "見積区分", "SP区分", "PC区分", "製品NO", "仕様NO", _
	''''''''''                        "ベース色", "他社伝票番号", "漢字名称", "W", "D", "H", "D1", "D2", "H1", "H2", "エラー内容", _
	''''''''''                        "定価", "U区分", "仕入単価", "仕入率", "売上単価", "売上率", "M区分", "見積数量", "単位名", _
	''''''''''                        "売上金額", "仕入金額", "売上税区分", "消費税額", "仕入先CD", "仕入先名", "配送先CD", "配送先名", _
	''''''''''                        "社内在庫数", "社内在庫数", "客先在庫数", "客先在庫数", "転用数", "発注調整数", "発注数", "総数量", _
	''''''''''                        "仕分数1", "仕分数2", "仕分数3", "仕分数4", "仕分数5", _
	''''''''''                        "仕分数6", "仕分数7", "仕分数8", "仕分数9", "仕分数10", _
	''''''''''                        "仕分数11", "仕分数12", "仕分数13", "仕分数14", "仕分数15", _
	''''''''''                        "仕分数16", "仕分数17", "仕分数18", "仕分数19", "仕分数20", _
	''''''''''                        "仕分数21", "仕分数22", "仕分数23", "仕分数24", "仕分数25", _
	''''''''''                        "仕分数26", "仕分数27", "仕分数28", "仕分数29", "仕分数30", _
	''''''''''                        "製品区分", "見積明細連番", "仕入済数", "売上済CNT" _
	''''''''''                        ))
	''''''''''    '2015/09/29 ADD
	''''''''''    RecArry = rs.GetRows(fpSpd.MaxRows, , _
	'''''''''''                Array("展開CHK", "見積区分", "他社納品日付", "他社伝票番号", "SP区分", "PC区分", "製品NO", "仕様NO", _
	'''''''''''                        "ベース色", "漢字名称", "W", "D", "H", "D1", "D2", "H1", "H2", "エラー内容", _
	'''''''''''                        "定価", "U区分", "仕入単価", "仕入率", "売上単価", "売上率", "M区分", "見積数量", "単位名", _
	'''''''''''                        "売上金額", "仕入金額", "売上税区分", "消費税額", "仕入先CD", "仕入先名", "配送先CD", "配送先名", _
	'''''''''''                        "社内在庫数", "社内在庫数", "客先在庫数", "客先在庫数", "転用数", "発注調整数", "発注数", "総数量", _
	'''''''''''                        "仕分数1", "仕分数2", "仕分数3", "仕分数4", "仕分数5", _
	'''''''''''                        "仕分数6", "仕分数7", "仕分数8", "仕分数9", "仕分数10", _
	'''''''''''                        "仕分数11", "仕分数12", "仕分数13", "仕分数14", "仕分数15", _
	'''''''''''                        "仕分数16", "仕分数17", "仕分数18", "仕分数19", "仕分数20", _
	'''''''''''                        "仕分数21", "仕分数22", "仕分数23", "仕分数24", "仕分数25", _
	'''''''''''                        "仕分数26", "仕分数27", "仕分数28", "仕分数29", "仕分数30", _
	'''''''''''                        "製品区分", "見積明細連番", "仕入済数", "売上済CNT" _
	'''''''''''                        ))
	''''''''
	'''''''''''    '2016/06/22 ADD
	'''''''''''    RecArry = rs.GetRows(fpSpd.MaxRows, , _
	''''''''''''                Array("展開CHK", "見積区分", "クレーム明細区分", "他社納品日付", "他社伝票番号", "SP区分", "PC区分", "製品NO", "仕様NO", _
	''''''''''''                        "ベース色", "漢字名称", "W", "D", "H", "D1", "D2", "H1", "H2", "エラー内容", _
	''''''''''''                        "定価", "U区分", "仕入単価", "仕入率", "売上単価", "売上率", "M区分", "見積数量", "単位名", _
	''''''''''''                        "売上金額", "仕入金額", "売上税区分", "消費税額", "仕入先CD", "仕入先名", _
	''''''''''''                        "作業区分CD", "作業区分CD", "配送先CD", "配送先名", _
	''''''''''''                        "社内在庫数", "社内在庫数", "客先在庫数", "客先在庫数", "転用数", "発注調整数", "発注数", "総数量", _
	''''''''''''                        "仕分数1", "仕分数2", "仕分数3", "仕分数4", "仕分数5", _
	''''''''''''                        "仕分数6", "仕分数7", "仕分数8", "仕分数9", "仕分数10", _
	''''''''''''                        "仕分数11", "仕分数12", "仕分数13", "仕分数14", "仕分数15", _
	''''''''''''                        "仕分数16", "仕分数17", "仕分数18", "仕分数19", "仕分数20", _
	''''''''''''                        "仕分数21", "仕分数22", "仕分数23", "仕分数24", "仕分数25", _
	''''''''''''                        "仕分数26", "仕分数27", "仕分数28", "仕分数29", "仕分数30", _
	''''''''''''                        "製品区分", "見積明細連番", "仕入済数", "売上済CNT" _
	''''''''''''                        ))
	''''''''
	''''''''    '2017/05/14 ADD
	''''''''    RecArry = rs.GetRows(fpSpd.MaxRows, , _
	'''''''''                Array("展開CHK", "見積区分", "クレーム明細区分", "他社納品日付", "他社伝票番号", "SP区分", "PC区分", "製品NO", "仕様NO", _
	'''''''''                        "ベース色", "漢字名称", "W", "D", "H", "D1", "D2", "H1", "H2", "エラー内容", _
	'''''''''                        "定価", "U区分", "仕入単価", "仕入率", "売上単価", "売上率", "M区分", "見積数量", "単位名", _
	'''''''''                        "売上金額", "仕入金額", "売上税区分", "消費税額", "仕入業者CD", "仕入業者名", _
	'''''''''                        "仕入先CD", "仕入先名", _
	'''''''''                        "作業区分CD", "作業区分CD", "配送先CD", "配送先名", _
	'''''''''                        "社内在庫数", "社内在庫数", "客先在庫数", "客先在庫数", "転用数", "発注調整数", "発注数", "総数量", _
	'''''''''                        "仕分数1", "仕分数2", "仕分数3", "仕分数4", "仕分数5", _
	'''''''''                        "仕分数6", "仕分数7", "仕分数8", "仕分数9", "仕分数10", _
	'''''''''                        "仕分数11", "仕分数12", "仕分数13", "仕分数14", "仕分数15", _
	'''''''''                        "仕分数16", "仕分数17", "仕分数18", "仕分数19", "仕分数20", _
	'''''''''                        "仕分数21", "仕分数22", "仕分数23", "仕分数24", "仕分数25", _
	'''''''''                        "仕分数26", "仕分数27", "仕分数28", "仕分数29", "仕分数30", _
	'''''''''                        "製品区分", "見積明細連番", "仕入済数", "売上済CNT" _
	'''''''''                        ))
	''''''''
	''''''''    With fpSpd
	''''''''        .ReDraw = False
	''''''''        For i = 0 To UBound(RecArry, 2)
	''''''''            For j = 0 To UBound(RecArry)
	''''''''                Select Case j + 1
	''''''''                    Case ColW To ColH2
	''''''''                        'サイズ
	''''''''                        .SetText j + 1, i + 1, IIf(Trim$("" & RecArry(j, i)) = "0", "", Trim$("" & RecArry(j, i)))
	''''''''                    Case Col定価, Col原価, Col仕入率, Col売価, Col売上率, Col見積数量, Col金額, Col仕入金額, _
	'''''''''                           Col社内在庫, Col客先在庫, Col転用, Col発注数, Col数量合計 _
	'''''''''                          , Col発注調整数 '2014/09/09 ADD
	''''''''                        '定価・原価・仕入％・売価・売上％
	''''''''                        .SetText j + 1, i + 1, IIf(Trim$("" & RecArry(j, i)) = "0", "", Trim$("" & RecArry(j, i)))
	''''''''                    Case Col仕分数1 To Col仕分数1 + 29
	''''''''                        '仕分数
	''''''''                        .SetText j + 1, i + 1, IIf(Trim$("" & RecArry(j, i)) = "0", "", Trim$("" & RecArry(j, i)))
	''''''''                    Case Col製品区分
	''''''''                        '製品区分
	''''''''                        .SetText Col製品区分, i + 1, "" & RecArry(j, i)
	''''''''                    Case Col見積明細連番
	''''''''                        '見積明細連番
	''''''''                        .SetText Col見積明細連番, i + 1, Trim$("" & RecArry(j, i))
	''''''''                    Case Col社内在庫参照, Col客先在庫参照               '2009/09/26 ADD
	''''''''                        '仮に社内在庫数と客先在庫数が入るので無視する
	''''''''
	'''''''''                    Case 73 '社内在庫数FLG
	'''''''''
	'''''''''                    Case 74 '社内適正在庫数FLG
	'''''''''
	'''''''''                    Case 75 '客先在庫数FLG
	''''''''                    Case Col仕入済数
	''''''''                        '仕入済数
	''''''''                        .SetText Col仕入済数, i + 1, Trim$("" & RecArry(j, i))
	''''''''                    Case Col売上済CNT '2014/11/3 ADD
	''''''''                        .SetText Col売上済CNT, i + 1, Trim$("" & RecArry(j, i))
	''''''''
	''''''''                    '2016/06/22 ADD↓
	''''''''                    Case Colクレーム区分
	''''''''                        .SetText Colクレーム区分, i + 1, Format$("" & RecArry(j, i), "#")
	''''''''                    Case Col作業区分CD
	''''''''                        .SetText Col作業区分CD, i + 1, Format$("" & RecArry(j, i), "#")
	''''''''                        .SetText Col作業区分名, i + 1, ModKubuns.Get作業区分名(Format$("" & RecArry(j, i), "#"))
	''''''''                    Case Col作業区分名
	''''''''                        'no
	''''''''                    '2016/06/22 ADD↑
	''''''''                    Case Else
	''''''''                        .SetText j + 1, i + 1, Trim$("" & RecArry(j, i))
	''''''''                End Select
	''''''''            Next
	''''''''
	''''''''            Call Get率(i + 1, Col原価)
	''''''''            Call Get率(i + 1, Col売価)
	''''''''            Call Get金額(i + 1)
	''''''''            '----------------------
	''''''''            '---   2005/10/14.ADD
	'''''''''''            If RecArry(17, i) = "U" Then
	''''''''''''            If clsSPD.GetTextEX(ColU区分, CLng(i + 1)) = "U" Then '2015/03/04 ADD
	''''''''''''                Call RowBackColorSet(1, CLng(i + 1))
	''''''''''''            End If
	''''''''            '2016/06/21 ADD
	''''''''            Call RowBackColorSet2(clsSPD.GetTextEX(ColU区分, CLng(i + 1)), CLng(i + 1))
	''''''''            '2008/01/29 ADD
	'''''''''''            If RecArry(15, i) Like "原価率*" Then
	''''''''            If clsSPD.GetTextEX(Colエラー内容, CLng(i + 1)) Like "原価率*" Then '2015/03/10 ADD
	''''''''                .Col = Col原価
	''''''''                .Row = i + 1
	''''''''                .ForeColor = vbRed
	''''''''                .Col = Col売価
	''''''''                .Row = i + 1
	''''''''                .ForeColor = vbRed
	''''''''            Else
	''''''''                .Col = Col原価
	''''''''                .Row = i + 1
	''''''''                .ForeColor = vbBlack
	''''''''                .Col = Col売価
	''''''''                .Row = i + 1
	''''''''                .ForeColor = vbBlack
	''''''''            End If
	''''''''
	'''''''''''            '2009/09/26 ADD↓ '2013/07/19 DEL
	'''''''''''            '社内・客先在庫取得
	'''''''''''            If RecArry(68, i) = 0 Then '製品時
	'''''''''''                If cDspSyaZaiko.GetZaikoInfo(HD_納期S, HD_担当者CD, CStr(RecArry(4, i)), CStr(RecArry(5, i)), CLng(NullToZero(HD_見積番号))) = False Then
	'''''''''''                    '管理していないか､管理外
	'''''''''''                    fpSpd.SetText Col社内在庫参照, i + 1, ""
	'''''''''''                Else
	'''''''''''                    fpSpd.SetText Col社内在庫参照, i + 1, cDspSyaZaiko.合計在庫数
	'''''''''''
	'''''''''''                    If RecArry(72, i) = 1 Then  '社内在庫数割れの場合
	'''''''''''                        .Col = Col社内在庫
	'''''''''''                        .Row = i + 1
	'''''''''''                        .ForeColor = vbRed
	'''''''''''                    Else
	'''''''''''                        .Col = Col社内在庫
	'''''''''''                        .Row = i + 1
	'''''''''''                        .ForeColor = vbBlack
	'''''''''''                    End If
	'''''''''''
	'''''''''''                    If RecArry(73, i) = 1 Then  '社内適正在庫数割れの場合
	'''''''''''                        .Col = Col社内在庫参照
	'''''''''''                        .Row = i + 1
	'''''''''''                        .ForeColor = vbRed
	'''''''''''                    Else
	'''''''''''                        .Col = Col社内在庫参照
	'''''''''''                        .Row = i + 1
	'''''''''''                        .ForeColor = vbBlack
	'''''''''''                    End If
	'''''''''''
	'''''''''''''                    If cDspSyaZaiko.合計在庫数 + RecArry(33, i) < cDspSyaZaiko.適正在庫数 Then
	'''''''''''''                        '適正在庫数を下回った場合赤くする。
	'''''''''''''                        .Col = Col社内在庫参照
	'''''''''''''                        .Row = i + 1
	'''''''''''''                        .ForeColor = vbRed
	'''''''''''''                    Else
	'''''''''''''                        .Col = Col社内在庫参照
	'''''''''''''                        .Row = i + 1
	'''''''''''''                        .ForeColor = vbBlack
	'''''''''''''                    End If
	'''''''''''''                    '在庫チェック
	'''''''''''''                    If RecArry(33, i) > cDspSyaZaiko.合計在庫数 Then
	'''''''''''''                        .Col = Col社内在庫
	'''''''''''''                        .Row = i + 1
	'''''''''''''                        .ForeColor = vbRed
	'''''''''''''                        .SetText Colエラー内容, i + 1, "社内在庫エラー"
	'''''''''''''                        SetCheckTempD = False
	'''''''''''''                    Else
	'''''''''''''                        .Col = Col社内在庫
	'''''''''''''                        .Row = i + 1
	'''''''''''''                        .ForeColor = vbBlack
	'''''''''''''                    End If
	'''''''''''                End If
	'''''''''''
	'''''''''''                If cDspKyakuZaiko.GetZaikoInfo(HD_納期S, HD_担当者CD, rf_得意先CD, CStr(RecArry(4, i)), CStr(RecArry(5, i)), CLng(NullToZero(HD_見積番号))) = False Then
	'''''''''''                    '管理していないか､管理外
	'''''''''''                    fpSpd.SetText Col客先在庫参照, i + 1, ""
	'''''''''''                Else
	'''''''''''                    fpSpd.SetText Col客先在庫参照, i + 1, cDspKyakuZaiko.合計在庫数
	'''''''''''
	'''''''''''                    If RecArry(74, i) = 1 Then  '客先在庫数割れの場合
	'''''''''''                        .Col = Col客先在庫
	'''''''''''                        .Row = i + 1
	'''''''''''                        .ForeColor = vbRed
	'''''''''''                    Else
	'''''''''''                        .Col = Col客先在庫
	'''''''''''                        .Row = i + 1
	'''''''''''                        .ForeColor = vbBlack
	'''''''''''                    End If
	'''''''''''''                    '在庫チェック
	'''''''''''''                    If RecArry(35, i) > cDspKyakuZaiko.合計在庫数 Then
	'''''''''''''                        .Col = Col客先在庫
	'''''''''''''                        .Row = i + 1
	'''''''''''''                        .ForeColor = vbRed
	'''''''''''''                        .SetText Colエラー内容, i + 1, "客先在庫エラー"
	'''''''''''''                        SetCheckTempD = False
	'''''''''''''                    Else
	'''''''''''''                        .Col = Col客先在庫
	'''''''''''''                        .Row = i + 1
	'''''''''''''                        .ForeColor = vbBlack
	'''''''''''''                    End If
	'''''''''''                End If
	'''''''''''            End If
	'''''''''''
	'''''''''''            '2009/09/26 ADD↑ '2013/07/19 DEL
	''''''''
	''''''''        Next
	''''''''        .ReDraw = True
	''''''''    End With
	''''''''
	''''''''    Call Calc合計
	''''''''
	''''''''    fpSpd.Col = fpSpd.ActiveCol             '2003/11/25 ADD
	''''''''    fpSpd.Row = fpSpd.ActiveRow
	'''''''''''    fpSpd.Col = 1
	'''''''''''    fpSpd.Row = 1
	''''''''    fpSpd.Action = ActionActiveCell
	''''''''    fpSpd.AutoCalc = True
	''''''''End Function
	
	Public Function Get単価(ByRef Row As Integer, ByRef Col As Integer, Optional ByRef SetSpd As Boolean = True) As Decimal
		Dim fpSpd As Object
		'''Public Function Get単価(Row As Long, Col As Long) As Currency    '2008/01/23 DEL
		Dim check As Boolean
		Dim getdata As Object
		Dim i As Integer
		Dim wTeika As Object
		Dim wRitu As Object
		Dim wTanka As Decimal
		
		Get単価 = 0
		
		With fpSpd
			
			'UPGRADE_WARNING: オブジェクト fpSpd.ReDraw の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.ReDraw = False
			
			'UPGRADE_WARNING: オブジェクト fpSpd.GetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			check = .GetText(Col定価, Row, wTeika) '定価
			If check Then
				'UPGRADE_WARNING: オブジェクト fpSpd.GetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				check = .GetText(Col, Row, wRitu) '％
				If check Then
					'UPGRADE_WARNING: オブジェクト wRitu の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					'UPGRADE_WARNING: オブジェクト wTeika の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					wTanka = (CDec(wTeika) * wRitu) / 100
					If SetSpd = True Then '2008/01/23 ADD
						'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.SetText(Col - 1, Row, VB6.Format(wTanka, "#.##")) '2014/08/06 UPD
					End If
				Else
					wTanka = 0
				End If
			Else
				wTanka = 0
			End If
			
			'UPGRADE_WARNING: オブジェクト fpSpd.ReDraw の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.ReDraw = True
			
		End With
		Get単価 = wTanka
		
	End Function
	
	Public Sub Get率(ByRef Row As Integer, ByRef Col As Integer)
		Dim fpSpd As Object
		Dim check As Boolean
		Dim getdata As Object
		Dim i As Integer
		Dim wTeika As Object
		Dim wTanka As Object
		Dim wRitu As Decimal
		
		With fpSpd
			
			''        .ReDraw = False
			''
			'UPGRADE_WARNING: オブジェクト fpSpd.GetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			check = .GetText(Col定価, Row, wTeika) '定価
			If check Then
				'UPGRADE_WARNING: オブジェクト fpSpd.GetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				check = .GetText(Col, Row, wTanka) '単価
				If check Then
					'UPGRADE_WARNING: オブジェクト wTeika の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					If wTeika <> "0" Then 'ZERO割抑制 2003/11/20  ADD
						'UPGRADE_WARNING: オブジェクト wTeika の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						'UPGRADE_WARNING: オブジェクト wTanka の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						wRitu = (CDec(wTanka) / CDec(wTeika)) * 100
						'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.SetText(Col + 1, Row, VB6.Format(wRitu, "#.##"))
					Else
						wRitu = 0
						'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.SetText(Col + 1, Row, "")
					End If
					
				Else
					wRitu = 0
					'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.SetText(Col + 1, Row, "")
				End If
			Else
				wRitu = 0
				'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.SetText(Col + 1, Row, "")
			End If
			
			''        .ReDraw = True
			''
		End With
	End Sub
	'2008/01/23 ADD↓
	''Private Function chk原価率(Row As Long) As Boolean
	Private Function chk原価率(ByRef Row As Integer, Optional ByRef wGenka As Object = Nothing, Optional ByRef wBaika As Object = Nothing) As Boolean
		Dim fpSpd As Object
		Dim check As Boolean
		Dim getdata As Object
		'    Dim wBaika As Currency
		'    Dim wGenka As Currency
		Dim wRitu As Decimal
		
		
		
		With fpSpd
			'UPGRADE_NOTE: IsMissing() は IsNothing() に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="8AE1CB93-37AB-439A-A4FF-BE3B6760BB23"' をクリックしてください。
			If IsNothing(wGenka) Then
				'UPGRADE_WARNING: オブジェクト fpSpd.GetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				check = .GetText(Col原価, Row, getdata) '仕入単価
				If check Then
					'UPGRADE_WARNING: オブジェクト getdata の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					'UPGRADE_WARNING: オブジェクト wGenka の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					wGenka = getdata
				Else
					'UPGRADE_WARNING: オブジェクト wGenka の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					wGenka = 0
				End If
			End If
			
			'UPGRADE_NOTE: IsMissing() は IsNothing() に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="8AE1CB93-37AB-439A-A4FF-BE3B6760BB23"' をクリックしてください。
			If IsNothing(wBaika) Then
				'UPGRADE_WARNING: オブジェクト fpSpd.GetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				check = .GetText(Col売価, Row, getdata) '売上単価
				If check Then
					'UPGRADE_WARNING: オブジェクト getdata の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					'UPGRADE_WARNING: オブジェクト wBaika の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					wBaika = getdata
				Else
					'UPGRADE_WARNING: オブジェクト wBaika の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					wBaika = 0
				End If
			End If
		End With
		
		'UPGRADE_WARNING: オブジェクト wGenka の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		If wGenka = 0 Then
			chk原価率 = True
			Exit Function
		End If
		'UPGRADE_WARNING: オブジェクト wBaika の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		If wBaika = 0 Then
			chk原価率 = True
			Exit Function
		End If
		
		'UPGRADE_WARNING: オブジェクト wBaika の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト wGenka の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		wRitu = (wGenka / wBaika) * 100
		
		If wRitu > gGenRituMax Then
			''''        CriticalAlarm "原価率上限オーバー"                  '2008/02/20 DEL
			'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			fpSpd.SetText(Colエラー内容, Row, "原価率上限オーバー") '2008/02/20 ADD
			
			chk原価率 = False
			Exit Function
		End If
		If wRitu < gGenRituMin Then
			''''        CriticalAlarm "原価率下限オーバー"                  '2008/02/20 DEL
			'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			fpSpd.SetText(Colエラー内容, Row, "原価率下限オーバー") '2008/02/20 ADD
			chk原価率 = False
			Exit Function
		End If
		
		chk原価率 = True
		
	End Function
	'2008/01/23 ADD↑
	
	Private Sub Get金額(ByRef Row As Integer)
		Dim fpSpd As Object
		''    Debug.Print "Get金額"
		Dim check As Boolean
		Dim getdata As Object
		Dim wSuryo As Decimal
		Dim wBaika As Decimal
		Dim wGenka As Decimal
		Dim wUriKin As Decimal
		Dim wGenKin As Decimal
		
		On Error GoTo keisan_err
		With fpSpd
			''        .ReDraw = False
			
			'UPGRADE_WARNING: オブジェクト fpSpd.GetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			check = .GetText(Col見積数量, Row, getdata) '数量
			If check Then
				'UPGRADE_WARNING: オブジェクト getdata の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				wSuryo = getdata
			Else
				wSuryo = 0
			End If
			'UPGRADE_WARNING: オブジェクト fpSpd.GetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			check = .GetText(Col原価, Row, getdata) '仕入単価
			If check Then
				'UPGRADE_WARNING: オブジェクト getdata の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				wGenka = getdata
			Else
				wGenka = 0
			End If
			'UPGRADE_WARNING: オブジェクト fpSpd.GetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			check = .GetText(Col売価, Row, getdata) '売上単価
			If check Then
				'UPGRADE_WARNING: オブジェクト getdata の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				wBaika = getdata
			Else
				wBaika = 0
			End If
			
			'UPGRADE_WARNING: オブジェクト NullToZero(wBaika) の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			wUriKin = ISHasuu_rtn(CShort(tx_売上端数.Text), NullToZero(wSuryo) * NullToZero(wBaika), KIN_HASU)
			'UPGRADE_WARNING: オブジェクト NullToZero(wGenka) の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			wGenKin = ISHasuu_rtn(CShort(tx_売上端数.Text), NullToZero(wSuryo) * NullToZero(wGenka), KIN_HASU)
			
			'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.SetText(Col金額, Row, wUriKin)
			'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.SetText(Col仕入金額, Row, wGenKin)
			
			Call Get消費税(Row)
			
			''        .ReDraw = True
		End With
		On Error GoTo 0
		
		Exit Sub
		
keisan_err: 
		If Err.Number = 6 Then
			Beep()
			Inform("オーバーフローしました。")
			Err.Clear()
			'UPGRADE_WARNING: オブジェクト PreviousControl.Undo の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			PreviousControl.Undo()
			PreviousControl.Focus()
			''''    Else
			''''        Beep
			''''        MsgBox "Error code = " & Err.Number & vbCrLf & Err.Description, vbCritical, Me.Caption
		End If
		
	End Sub
	
	Private Sub Get消費税(ByRef Row As Integer)
		Dim fpSpd As Object
		Dim check As Boolean
		Dim getdata As Object
		Dim wUriKin As Decimal
		Dim wZeiKin As Decimal
		Dim wZeiKb As Short
		
		With fpSpd
			
			''        .ReDraw = False
			
			'UPGRADE_WARNING: オブジェクト fpSpd.GetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			check = .GetText(Col売上税区分, Row, getdata) '売上税区分
			If check Then
				'UPGRADE_WARNING: オブジェクト getdata の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				wZeiKb = getdata
			Else
				wZeiKb = 0
			End If
			'UPGRADE_WARNING: オブジェクト fpSpd.GetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			check = .GetText(Col金額, Row, getdata) '売上金額
			If check Then
				'UPGRADE_WARNING: オブジェクト getdata の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				wUriKin = getdata
			Else
				wUriKin = 0
			End If
			
			Select Case wZeiKb
				Case 0 '外税
					wZeiKin = 0
				Case 1 '内税
					'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					wZeiKin = ISHasuu_rtn(CShort(tx_消費税端数.Text), NullToZero(wUriKin, 0) / (100 + gZEI) * gZEI, KIN_HASU)
				Case 2 '非課税
					wZeiKin = 0 '2015/01/24 ADD
			End Select
			
			'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.SetText(Col消費税額, Row, wZeiKin)
			
			''        .ReDraw = True
			
		End With
		On Error GoTo 0
	End Sub
	
	Private Function Get税率(ByRef SDate As Date) As Decimal
		Dim rs As ADODB.Recordset
		Dim sql As String
		Dim ZEI As Decimal
		
		Get税率 = 0
		
		sql = "select * from TM税率 where 税率ID = 'ZR'"
		rs = OpenRs(sql, Cn, ADODB.CursorTypeEnum.adOpenForwardOnly, ADODB.LockTypeEnum.adLockReadOnly)
		
		With rs
			If Not .EOF Then
				Select Case .Fields("切替日付").Value
					Case Is <= SDate
						ZEI = .Fields("税率").Value
					Case Is > SDate
						ZEI = .Fields("旧税率").Value
				End Select
			End If
		End With
		ReleaseRs(rs)
		
		Get税率 = ZEI
	End Function
	
	'パブリック
	'SnwMT02F04[員数シート取込]より呼ばれる。
	Public Function SetImportData() As Short
		Dim fpSpd As Object
		'   画面上のデータとエクセルより取り込んだデータを
		'   マージして画面上に表示する。
		'
		Dim rs As ADODB.Recordset
		
		SetImportData = -1
		
		HourGlass(True)
		On Error GoTo SetImportData_err
		
		'Tmpテーブル作成
		''    OutputTmp
		CreateTmp見積明細() '2013/03/13 ADD
		''    OutputTmpCheck
		CreateTmp見積明細CSV() '2013/03/13 ADD
		
		'Tmpテーブル書き込み
		'UPGRADE_WARNING: オブジェクト fpSpd.DataRowCnt の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		If fpSpd.DataRowCnt <> 0 Then
			If InsDataTmp = False Then
				'''            Exit Function
				GoTo SetImportData_Correct
			End If
		End If
		
		'取込分テーブル書き込み
		If InsDataTmpCheck = False Then
			HourGlass(False)
			GoTo SetImportData_Correct
			'''        Exit Function
		End If
		
		''    SetImportData = False
		''    HOURGLASS FALSE
		''    Exit Function
		
		'--見積明細
		Dim cmd As New ADODB.Command
		Dim grs As ADODB.Recordset
		
		cmd.let_ActiveConnection(Cn)
		cmd.CommandTimeout = 0 '2007/09/14 ADD
		cmd.CommandText = "usp_MT0100員数ImportCheck" '2013/03/13 ADD
		cmd.CommandType = ADODB.CommandTypeEnum.adCmdStoredProc
		cmd.Parameters.Refresh()
		
		' それぞれのパラメータの値を指定する
		With cmd.Parameters
			.Item(0).Direction = ADODB.ParameterDirectionEnum.adParamReturnValue
			.Item("@i見積番号").Value = pMituNo
			.Item("@i仕分番号").Value = SiwaNo
			.Item("@i得意先CD").Value = [rf_得意先CD].Text
			.Item("@i大小口区分").Value = [tx_大小口区分].Text
			.Item("@CompName").Value = GetPCName
		End With
		
		grs = New ADODB.Recordset
		'---コマンド実行
		grs = cmd.Execute
		
		If grs.State <> 0 Then
			If grs.EOF Then
				SetImportData = -1
				Inform("該当データ無し")
			Else
				rs = grs
				SetImportData = 1
			End If
		Else
			SetImportData = -1
			CriticalAlarm((cmd.Parameters("@RetST").Value & " : " & cmd.Parameters("@RetMsg").Value))
		End If
		
		'UPGRADE_NOTE: オブジェクト cmd をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
		cmd = Nothing
		
		'チェック後データ再表示
		Call SetCheckTempD(rs)
		
		'''    SiwaNo = 0
		
		HourGlass(False)
		
SetImportData_Correct: 
		On Error GoTo 0
		'UPGRADE_NOTE: オブジェクト cmd をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
		cmd = Nothing
		
		'作業テーブル削除
		DropTmp見積明細() '2013/03/13 ADD
		DropTmp見積明細CSV() '2013/03/13 ADD
		
		HourGlass(False)
		Exit Function
		
SetImportData_err: '---エラー時
		MsgBox(Err.Number & " " & Err.Description)
		Resume SetImportData_Correct
	End Function
	
	'2018/04/11 ADD↓
	'パブリック
	'SnwMT02F15[レビット取込]より呼ばれる。
	Public Function SetImportData_Revit() As Short
		Dim fpSpd As Object
		'   画面上のデータとエクセルより取り込んだデータを
		'   マージして画面上に表示する。
		'
		Dim rs As ADODB.Recordset
		
		SetImportData_Revit = -1
		
		HourGlass(True)
		On Error GoTo SetImportData_err
		
		'Tmpテーブル作成
		''    OutputTmp
		CreateTmp見積明細() '2013/03/13 ADD
		''    OutputTmpCheck
		CreateTmp見積明細CSV() '2013/03/13 ADD
		
		'Tmpテーブル書き込み
		'UPGRADE_WARNING: オブジェクト fpSpd.DataRowCnt の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		If fpSpd.DataRowCnt <> 0 Then
			If InsDataTmp = False Then
				'''            Exit Function
				GoTo SetImportData_Correct
			End If
		End If
		
		'取込分テーブル書き込み
		If InsDataTmpCheck_Revit = False Then
			HourGlass(False)
			GoTo SetImportData_Correct
			'''        Exit Function
		End If
		
		''    SetImportData = False
		''    HOURGLASS FALSE
		''    Exit Function
		
		'--見積明細
		Dim cmd As New ADODB.Command
		Dim grs As ADODB.Recordset
		
		cmd.let_ActiveConnection(Cn)
		cmd.CommandTimeout = 0 '2007/09/14 ADD
		cmd.CommandText = "usp_MT0100員数ImportCheck_Revit" '2013/03/13 ADD
		cmd.CommandType = ADODB.CommandTypeEnum.adCmdStoredProc
		cmd.Parameters.Refresh()
		
		' それぞれのパラメータの値を指定する
		With cmd.Parameters
			.Item(0).Direction = ADODB.ParameterDirectionEnum.adParamReturnValue
			.Item("@i見積番号").Value = pMituNo
			'        .Item("@i仕分番号").Value = SiwaNo
			.Item("@i得意先CD").Value = [rf_得意先CD].Text
			.Item("@i大小口区分").Value = [tx_大小口区分].Text
			.Item("@i仕分番号").Value = REVIT_SIWANO
			'        .Item("@CompName").Value = GetPCName
		End With
		
		grs = New ADODB.Recordset
		'---コマンド実行
		grs = cmd.Execute
		
		If grs.State <> 0 Then
			If grs.EOF Then
				SetImportData_Revit = -1
				Inform("該当データ無し")
			Else
				rs = grs
				SetImportData_Revit = 1
			End If
		Else
			SetImportData_Revit = -1
			CriticalAlarm((cmd.Parameters("@RetST").Value & " : " & cmd.Parameters("@RetMsg").Value))
		End If
		
		'UPGRADE_NOTE: オブジェクト cmd をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
		cmd = Nothing
		
		'チェック後データ再表示
		Call SetCheckTempD(rs)
		
		'''    SiwaNo = 0
		
		HourGlass(False)
		
SetImportData_Correct: 
		On Error GoTo 0
		'UPGRADE_NOTE: オブジェクト cmd をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
		cmd = Nothing
		
		'作業テーブル削除
		DropTmp見積明細() '2013/03/13 ADD
		DropTmp見積明細CSV() '2013/03/13 ADD
		
		HourGlass(False)
		Exit Function
		
SetImportData_err: '---エラー時
		MsgBox(Err.Number & " " & Err.Description)
		Resume SetImportData_Correct
	End Function
	'2018/04/11 ADD↑
	
	Private Function Upload() As Boolean
		Dim fpSpd As Object
		Dim rs As ADODB.Recordset
		Dim sql As String
		Dim cmd As ADODB.Command
		
		cmd = New ADODB.Command
		
		Upload = False
		
		'UPGRADE_WARNING: オブジェクト fpSpd.DataRowCnt の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		If fpSpd.DataRowCnt = 0 Then
			CriticalAlarm("明細がありません。")
			'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			fpSpd.Col = fpSpd.ActiveRow
			'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			fpSpd.Row = fpSpd.ActiveRow
			'UPGRADE_WARNING: オブジェクト fpSpd.Action の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			fpSpd.Action = FPSpreadADO.ActionConstants.ActionActiveCell
			'UPGRADE_WARNING: オブジェクト fpSpd.SetFocus の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			fpSpd.SetFocus()
			Exit Function
		End If
		
		'2011/12/26 ADD↓
		If Cn Is Nothing Then
			If ApplicationInit(True) = False Then
				'UPGRADE_NOTE: オブジェクト Cn をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
				Cn = Nothing
				Exit Function
			End If
		End If
		'2011/12/26 ADD↑
		
		'マウスポインターを砂時計にする
		HourGlass(True)
		
		'''    ReDim WkTBLS(1 To fpSpd.DataRowCnt, 1 To fpSpd.MaxCols)
		'''
		'''    Call fpSpd.GetArray(1, 1, WkTBLS)
		
		Cn.BeginTrans() '---トランザクションの開始
		On Error GoTo Upload_Err
		
		'''    '仕分レベル設定保存
		'''    If SiwakeUpload = False Then
		'''        Cn.RollbackTrans
		'''        Exit Function
		'''    End If
		
		'Tmpテーブル作成
		''    OutputTmp
		CreateTmp見積明細() '2013/03/13 ADD
		'Tmpテーブル書き込み
		If InsDataTmp = False Then
			Cn.RollbackTrans()
			Exit Function
		End If
		
		'見積セット
		Cn.CommandTimeout = 0
		cmd.CommandTimeout = 0
		cmd.let_ActiveConnection(Cn)
		cmd.CommandText = "usp_MT0100UPD見積" '2013/03/13 ADD
		cmd.CommandType = ADODB.CommandTypeEnum.adCmdStoredProc
		
		With cmd.Parameters
			'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Item("@i見積番号").Value = SpcToNull(rf_見積番号, 0)
			.Item("@i担当者CD").Value = HD_担当者CD
			'UPGRADE_WARNING: オブジェクト HD_見積日付 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Item("@i見積日付").Value = VB6.Format(HD_見積日付, "yyyy/mm/dd")
			.Item("@i見積件名").Value = HD_見積件名
			.Item("@i得意先CD").Value = HD_得意先CD
			.Item("@i得意先名1").Value = HD_得意先名1
			.Item("@i得意先名2").Value = HD_得意先名2
			.Item("@i得TEL").Value = HD_得TEL
			.Item("@i得FAX").Value = HD_得FAX
			.Item("@i得意先担当者").Value = HD_得担当者
			.Item("@i納入得意先CD").Value = HD_納得意先CD
			.Item("@i納入先CD").Value = HD_納入先CD
			.Item("@i納入先名1").Value = HD_納入先名1
			.Item("@i納入先名2").Value = HD_納入先名2
			.Item("@i郵便番号").Value = HD_納郵便番号
			.Item("@i住所1").Value = HD_納住所1
			.Item("@i住所2").Value = HD_納住所2
			.Item("@i納TEL").Value = HD_納TEL
			.Item("@i納FAX").Value = HD_納FAX
			.Item("@i納入先担当者").Value = HD_納担当者
			'UPGRADE_WARNING: オブジェクト HD_納期S の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
			.Item("@i納期S").Value = IIf(IsDbNull(HD_納期S), System.DBNull.Value, VB6.Format(HD_納期S, "yyyy/mm/dd"))
			'UPGRADE_WARNING: オブジェクト HD_納期E の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
			.Item("@i納期E").Value = IIf(IsDbNull(HD_納期E), System.DBNull.Value, VB6.Format(HD_納期E, "yyyy/mm/dd"))
			.Item("@i備考").Value = HD_備考
			'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Item("@i物件規模金額").Value = SpcToNull(HD_規模金額, 0)
			'UPGRADE_WARNING: オブジェクト HD_OPEN日 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
			.Item("@iオープン日").Value = IIf(IsDbNull(HD_OPEN日), System.DBNull.Value, VB6.Format(HD_OPEN日, "yyyy/mm/dd"))
			'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Item("@i物件種別").Value = SpcToNull(HD_物件種別, 0)
			.Item("@i現場名").Value = HD_現場名
			'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Item("@i支払条件").Value = SpcToNull(HD_支払条件, 0)
			.Item("@i支払条件その他").Value = HD_支払条件他
			'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Item("@i納期表示").Value = SpcToNull(HD_納期表示, 0)
			.Item("@i納期その他").Value = HD_納期表示他
			'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Item("@i見積日出力").Value = SpcToNull(HD_見積日出力, 0)
			'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Item("@i有効期限").Value = SpcToNull(HD_有効期限, 0)
			'UPGRADE_WARNING: オブジェクト HD_受注日付 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
			.Item("@i受注日付").Value = IIf(IsDbNull(HD_受注日付), System.DBNull.Value, VB6.Format(HD_受注日付, "yyyy/mm/dd"))
			'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Item("@i大小口区分").Value = SpcToNull(HD_大小口区分, 0)
			'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Item("@i出精値引").Value = SpcToNull(HD_出精値引, 0)
			
			.Item("@i税集計区分").Value = HD_税集計区分
			.Item("@i売上端数").Value = HD_売上端数
			.Item("@i消費税端数").Value = HD_消費税端数
			
			'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Item("@i合計金額").Value = SpcToNull(tx_合計金額, 0)
			'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Item("@i原価合計").Value = SpcToNull(tx_原価合計, 0)
			'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Item("@i原価率").Value = SpcToNull((rf_原価率.Text), 0)
			'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Item("@i外税額").Value = SpcToNull(tx_外税額, 0)
			.Item("@CompName").Value = GetPCName
			
			'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Item("@i得意先別見積番号").Value = SpcToNull(rf_得意先別見積番号, 0) '2005/07/04 ADD
			
			.Item("@i見積区分").Value = 1 'SpcToNull(HD_見積区分, 0)                               '2009/09/26 ADD
			
			'2013/03/08 ADD↓
			'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Item("@i部署CD").Value = NullToZero(HD_部署CD)
			'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Item("@i販売先得意先CD").Value = NullToZero(HD_販売先得意先CD, "")
			'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Item("@i販売先得意先名1").Value = NullToZero(HD_販売先得意先名1, "")
			'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Item("@i販売先得意先名2").Value = NullToZero(HD_販売先得意先名2, "")
			'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Item("@i販売先納得意先CD").Value = NullToZero(HD_販売先納得意先CD, "")
			'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Item("@i販売先納入先CD").Value = NullToZero(HD_販売先納入先CD, "")
			'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Item("@i販売先納入先名1").Value = NullToZero(HD_販売先納入先名1, "")
			'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Item("@i販売先納入先名2").Value = NullToZero(HD_販売先納入先名2, "")
			'2013/03/08 ADD↑
			
			'''         .Item("@i営業推進部CD").Value = NullToZero(HD_営業推進部CD, 0) '2019/12/12 ADD
			
			'2013/07/19 ADD↓
			'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Item("@i社内伝票扱い").Value = NullToZero(HD_社内伝票扱い, 0)
			'2013/07/19 ADD↑
			
			'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Item("@i伝票種類").Value = SpcToNull(HD_伝票種類, 0) '2015/02/04 ADD
			
			'2015/06/12 ADD↓
			'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Item("@iウエルシア物件内容CD").Value = SpcToNull(HD_ウエルシア物件内容CD, 0)
			'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Item("@iウエルシア物件内容名").Value = NullToZero(HD_ウエルシア物件内容名, "")
			'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Item("@iウエルシアリース区分").Value = SpcToNull(HD_ウエルシアリース区分, 0)
			'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Item("@iウエルシア物件区分CD").Value = SpcToNull(HD_ウエルシア物件区分CD, 0)
			'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Item("@iウエルシア売場面積").Value = SpcToNull(HD_ウエルシア売場面積, 0)
			'2015/06/12 ADD↑
			
			'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Item("@i物件番号").Value = SpcToNull(HD_物件番号, 0) '2015/07/10 ADD
			'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Item("@i統合見積番号").Value = SpcToNull(HD_統合見積番号, 0) '2024/01/16 ADD
			
			'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Item("@i受注区分").Value = SpcToNull(HD_受注区分, 0)
			
			'2015/10/16 ADD↓
			'UPGRADE_WARNING: オブジェクト HD_受付日付 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
			.Item("@i受付日付").Value = IIf(IsDbNull(HD_受付日付), System.DBNull.Value, VB6.Format(HD_受付日付, "yyyy/mm/dd"))
			'UPGRADE_WARNING: オブジェクト HD_完工日付 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
			.Item("@i完工日付").Value = IIf(IsDbNull(HD_完工日付), System.DBNull.Value, VB6.Format(HD_完工日付, "yyyy/mm/dd"))
			'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Item("@i発注担当者名").Value = NullToZero(HD_発注担当者名, "")
			'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Item("@i作業内容").Value = NullToZero(HD_作業内容, "")
			'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Item("@iYKサプライ区分").Value = SpcToNull(HD_YKサプライ区分, 0)
			'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Item("@iYK物件区分").Value = SpcToNull(HD_YK物件区分, 0)
			'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Item("@iYK請求区分").Value = SpcToNull(HD_YK請求区分, 0)
			'2015/10/16 ADD↑
			
			'2015/11/03 ADD↓
			'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Item("@i化粧品メーカー区分").Value = SpcToNull(HD_化粧品メーカー区分, 0)
			'2015/11/03 ADD↑
			
			'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Item("@iSM内容区分").Value = SpcToNull(HD_SM内容区分, 0) '2015/11/19 ADD
			
			'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Item("@iクレーム区分").Value = SpcToNull(HD_クレーム区分, 0) '2016/06/09 ADD
			
			'''        .Item("@i営業推進部CD").Value = SpcToNull(HD_営業推進部CD, 0) '2019/12/12 ADD
			'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Item("@i工事担当CD").Value = SpcToNull(HD_工事担当CD, 0) '2022/08/08 ADD
			
			
			'2020/04/11 ADD↓
			'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Item("@i見積確定区分").Value = SpcToNull(HD_見積確定区分, 0)
			'UPGRADE_WARNING: オブジェクト HD_完了日付 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
			.Item("@i完了日付").Value = IIf(IsDbNull(HD_完了日付), System.DBNull.Value, VB6.Format(HD_完了日付, "yyyy/mm/dd"))
			'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Item("@i完了者名").Value = NullToZero(HD_完了者名, "")
			'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Item("@i入力USERID").Value = NullToZero(HD_入力USERID, "")
			
			'UPGRADE_WARNING: オブジェクト HD_請求予定日 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
			.Item("@i請求予定日").Value = IIf(IsDbNull(HD_請求予定日), System.DBNull.Value, VB6.Format(HD_請求予定日, "yyyy/mm/dd"))
			
			'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Item("@i経過備考1").Value = NullToZero(HD_経過備考1, "")
			'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Item("@i経過備考2").Value = NullToZero(HD_経過備考2, "")
			'2020/04/11 ADD↑
			
			'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Item("@i集計CD").Value = NullToZero(HD_集計CD, "") '2021/02/25 ADD
			
			
			'2022/10/10 ADD↓
			'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Item("@iB請求管轄区分").Value = NullToZero(HD_B請求管轄区分, 0)
			'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Item("@iBtoB番号").Value = NullToZero(HD_BtoB番号, 0)
			'2022/10/10 ADD↑
			
			'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Item("@i業種区分").Value = SpcToNull(HD_業種区分, 0) '2023/04/18 ADD
			
		End With
		
		Cn.CursorLocation = ADODB.CursorLocationEnum.adUseClient 'MoveLast・OUTPUT変数を使用する場合
		
		rs = cmd.Execute '2018/04/11 ADD
		
		Cn.CursorLocation = ADODB.CursorLocationEnum.adUseServer
		
		''''2018/04/11 DEL↓
		'''    cmd.Execute
		'''
		''''''    Dim a As Object
		''''''    For Each a In Cn.Errors
		''''''        Debug.Print "err:" & a
		''''''    Next
		'''
		'''    If cmd.State <> 0 Then
		'''        If cmd(0) <> 0 Then
		'''            CriticalAlarm (cmd("@RetST") & vbCrLf & cmd("@RetMSG"))
		'''            Cn.RollbackTrans
		'''            GoTo Upload_Correct
		'''        End If
		'''    Else
		'''        If cmd(0) <> 0 Then
		'''            CriticalAlarm (cmd("@RetST") & vbCrLf & cmd("@RetMSG"))
		'''            Cn.RollbackTrans
		'''            GoTo Upload_Correct
		'''        End If
		'''    End If
		''''2018/04/11 DEL↑
		
		'2018/04/11 ADD↓
		If rs.State <> 0 Then
			If rs.EOF Then
				Inform("該当データ無し")
				Cn.RollbackTrans()
				GoTo Upload_Correct
			End If
		Else
			CriticalAlarm((cmd.Parameters("@RetST").Value & vbCrLf & cmd.Parameters("@RetMSG").Value))
			Cn.RollbackTrans()
			GoTo Upload_Correct
		End If
		
		
		'データ再表示
		Call SetCheckTempD(rs)
		'2018/04/11 ADD↑
		
		'新番号
		wNUMBER = cmd.Parameters("@RetMNO").Value
		
		rf_得意先別見積番号.Text = cmd.Parameters("@RetTNO").Value '2005/07/04 ADD
		'UPGRADE_WARNING: オブジェクト HD_得意先別見積番号 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		HD_得意先別見積番号 = cmd.Parameters("@RetTNO").Value '2005/07/04 ADD
		
		'仕分レベル設定保存
		If SiwakeUpload = False Then
			Cn.RollbackTrans()
			Exit Function
		End If
		
		'--2003/11/05.ADD---------------
		If rf_見積番号.Text = "" Then
			rf_見積番号.Text = CStr(wNUMBER)
			'UPGRADE_ISSUE: Control rf_見積番号 は、汎用名前空間 Form 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
			pParentForm.rf_見積番号 = wNUMBER '2017/04/07 ADD
			'2017/04/07 ADD↓
			'ロック情報生成
			If LockData("見積番号", wNUMBER) = False Then
				Cn.RollbackTrans()
				GoTo Upload_Correct
			End If
			'2017/04/07 ADD↑
			
			Call ModeIndicate(1)
		End If
		'-------------------------------
		
		'''    '--見積ヘッダー
		'''    SQL = "UPDATE TD見積 " _
		''''        & "SET 合計金額 = " & tx_合計金額 _
		''''        & ", 原価合計 = " & tx_原価合計 _
		''''        & ", 外税額 = " & tx_外税額 _
		''''        & ", 原価率 = " & CInt(rf_原価率.Caption) _
		''''        & " WHERE 見積番号 = " & pMituNo
		'''
		'''    Cn.Execute (SQL)
		'''
		'''    Cn.CommitTrans  '---トランザクションをコミットする
		'''
		'''    '--見積明細
		'''    Dim cmd As New adodb.Command
		
		''''    cmd.ActiveConnection = Cn
		''''    cmd.CommandText = "usp_MT0100UPD見積シートM"
		''''    cmd.CommandType = adCmdStoredProc
		''''    cmd.Parameters.Refresh
		''''
		''''    ' それぞれのパラメータの値を指定する
		''''    cmd.Parameters(0).Direction = adParamReturnValue
		''''''    cmd.Parameters(1).Value = 1
		''''    cmd.Parameters(1).Value = tx_受注区分
		''''    cmd.Parameters(2).Value = pMituNo
		''''    cmd.Parameters(3).Value = GetPCName
		''''    Call cmd.Execute
		
		'''    '2009/09/26 ADD↓
		'''    If SpcToNull(HD_見積区分, 0) = 1 Then   '本見積の場合
		'''        If Upload発注情報(wNUMBER) = False Then
		'''            Cn.RollbackTrans
		'''            HourGlass False
		'''            Exit Function
		'''        End If
		'''    '2013/04/09 ADD↓
		'''    Else
		'''        If HH_見積区分 = 1 Then
		'''            '本見積から仮見積に変わった場合
		'''            If IsNull(HD_売上日付) And IsNull(HD_仕入日付) Then '処理されていない
		'''                If Delete発注情報(wNUMBER) = False Then
		'''                    Cn.RollbackTrans
		'''                    Exit Function
		'''                End If
		'''            End If
		'''        End If
		'''    '2013/04/09 ADD↑
		'''    End If
		'''    '2009/09/26 ADD↑
		
		'2013/08/08 ADD↓
		'UPGRADE_WARNING: オブジェクト SpcToNull(HD_受注区分, 0) の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		If SpcToNull(HD_受注区分, 0) = 1 Then '受注確定の場合
			'        If Upload発注情報(wNUMBER) = False Then
			'            Cn.RollbackTrans
			'            HourGlass False
			'            Exit Function
			'        End If
		Else
			If HH_受注区分 = 1 Then
				'受注確定から仮受注に変わった場合
				'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
				If IsDbNull(HD_売上日付) And IsDbNull(HD_仕入日付) Then '処理されていない
					If Delete発注情報(wNUMBER) = False Then
						Cn.RollbackTrans()
						HourGlass(False)
						Exit Function
					End If
				End If
			End If
		End If
		'2013/08/08 ADD↑
		
		
		
		'作業テーブル削除
		DropTmp見積明細() '2013/03/13 ADD
		
		Cn.CommitTrans() '---トランザクションをコミットする
		Upload = True
		
Upload_Correct: 
		'UPGRADE_NOTE: オブジェクト cmd をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
		cmd = Nothing
		HourGlass(False)
		On Error GoTo 0
		Exit Function
Upload_Err: '---エラー時
		CheckAlarm(Err.Number & vbCrLf & Err.Description)
		Cn.RollbackTrans() 'トランザクションを破棄する
		'UPGRADE_NOTE: オブジェクト Cn をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
		Cn = Nothing '2011/12/26 ADD
		Resume Upload_Correct
	End Function
	
	Private Function SiwakeUpload() As Boolean
		Dim rs As ADODB.Recordset
		Dim sql As String
		Dim i As Short
		Dim j As Short
		
		SiwakeUpload = False
		'マウスポインターを砂時計にする
		HourGlass(True)
		
		On Error GoTo SiwakeUpload_err
		
		''    SQL = "DELETE FROM TD見積シート内訳名称 " _
		'''        & "WHERE 見積番号 = " & pMituNo
		sql = "DELETE FROM TD見積シート内訳名称 " & "WHERE 見積番号 = " & wNUMBER
		Cn.Execute(sql)
		
		'UPGRADE_WARNING: IsEmpty は、IsNothing にアップグレードされ、新しい動作が指定されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"' をクリックしてください。
		If Not IsNothing(gSiwakeTBL) Then
			For i = 0 To UBound(gSiwakeTBL, 2)
				'''            sql = "INSERT INTO TD見積シート内訳名称 " _
				''''                & "(見積番号, 仕分番号, 名称, 略称, 納期, 時間) " _
				''''                & "VALUES " _
				''''                & "(" _
				''''                & wNUMBER & "," _
				''''                & i + 1 & "," _
				''''                & "'" & SQLString(gSiwakeTBL(0, i)) & "'," _
				''''                & "'" & SQLString(gSiwakeTBL(1, i)) & "'," _
				''''                & IIf(gSiwakeTBL(2, i) = vbNullString, "Null", SQLDate(gSiwakeTBL(2, i), DBType)) & "," _
				''''                & IIf(gSiwakeTBL(3, i) = vbNullString, "Null", "'" & SQLString(gSiwakeTBL(3, i)) & "'") _
				''''                & ")"
				'2016/03/07 ADD↓
				sql = "INSERT INTO TD見積シート内訳名称 "
				sql = sql & "(見積番号, 仕分番号, 名称, 略称, 納期, 時間,他社部門CD) "
				sql = sql & "VALUES "
				sql = sql & "("
				sql = sql & wNUMBER & ","
				sql = sql & i + 1 & ","
				sql = sql & "'" & SQLString(gSiwakeTBL(0, i)) & "',"
				sql = sql & "'" & SQLString(gSiwakeTBL(1, i)) & "',"
				'UPGRADE_WARNING: オブジェクト gSiwakeTBL() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				sql = sql & IIf(gSiwakeTBL(2, i) = vbNullString, "Null", SQLDate(gSiwakeTBL(2, i), DBType)) & ","
				'UPGRADE_WARNING: オブジェクト gSiwakeTBL() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				sql = sql & IIf(gSiwakeTBL(3, i) = vbNullString, "Null", "'" & SQLString(gSiwakeTBL(3, i)) & "'") & ","
				sql = sql & "'" & SQLString(gSiwakeTBL(4, i)) & "'"
				sql = sql & ")"
				'2016/03/07 ADD↑
				
				Cn.Execute(sql)
			Next 
		End If
		
		SiwakeUpload = True
		
SiwakeUpload_Correct: 
		On Error GoTo 0
		
		HourGlass(False)
		Exit Function
		
SiwakeUpload_err: '---エラー時
		MsgBox(Err.Number & " " & Err.Description)
		Resume SiwakeUpload_Correct
	End Function
	
	Private Sub CreateTmp見積明細()
		'#Tmp見積明細テーブルを生成する
		
		'2013/03/13 ADD↓
		Dim cmd As New ADODB.Command
		Dim sql As String
		
		
		sql = "SELECT TOP 0 * INTO #Tmp見積明細 FROM Tmp見積明細"
		'''''    sql = "SELECT TOP 0 * INTO zTmp見積明細 FROM Tmp見積明細"
		
		cmd.let_ActiveConnection(Cn)
		cmd.CommandTimeout = 0
		cmd.CommandText = sql
		cmd.CommandType = ADODB.CommandTypeEnum.adCmdText
		
		cmd.Execute()
		
		'''    Cn.Execute sql
		
		'UPGRADE_NOTE: オブジェクト cmd をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
		cmd = Nothing
		'2013/03/13 ADD↑
		
	End Sub
	
	Private Sub DropTmp見積明細()
		'#Tmp見積明細テーブルを削除する
		
		'2013/03/13 ADD↓
		Dim cmd As New ADODB.Command
		Dim sql As String
		
		On Error Resume Next
		
		sql = "DROP TABLE #Tmp見積明細"
		
		cmd.let_ActiveConnection(Cn)
		cmd.CommandTimeout = 0
		cmd.CommandText = sql
		cmd.CommandType = ADODB.CommandTypeEnum.adCmdText
		
		cmd.Execute(sql)
		
		'UPGRADE_NOTE: オブジェクト cmd をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
		cmd = Nothing
		
		On Error GoTo 0
		'2013/03/13 ADD↑
		
	End Sub
	'''
	'''Private Sub OutputTmp()
	'''    Dim cmd As New ADODB.Command
	'''
	'''    cmd.ActiveConnection = Cn
	'''    cmd.CommandTimeout = 0                      '2007/09/14 ADD
	'''    cmd.CommandText = "usp_CreateTmp見積明細"
	'''    cmd.CommandType = adCmdStoredProc
	'''    cmd.Parameters.Refresh
	'''
	'''    ' それぞれのパラメータの値を指定する
	'''    cmd.Parameters(0).Direction = adParamReturnValue
	'''    cmd.Parameters(1).Value = GetPCName
	'''    Call cmd.Execute
	'''
	'''    TMPTABLE = cmd(2)
	'''
	'''    Set cmd = Nothing
	'''
	'''End Sub
	
	Private Sub CreateTmp見積明細CSV()
		'取込用#Tmp見積明細CSVテーブルを生成する
		
		'2013/03/13 ADD↓
		Dim cmd As New ADODB.Command
		Dim sql As String
		
		sql = "SELECT TOP 0 * INTO #Tmp見積明細CSV FROM Tmp見積明細"
		''''    sql = "SELECT TOP 0 * INTO zTmp見積明細CSV FROM Tmp見積明細"
		
		cmd.let_ActiveConnection(Cn)
		cmd.CommandTimeout = 0
		cmd.CommandText = sql
		cmd.CommandType = ADODB.CommandTypeEnum.adCmdText
		
		cmd.Execute(sql)
		
		'UPGRADE_NOTE: オブジェクト cmd をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
		cmd = Nothing
		'2013/03/13 ADD↑
		
	End Sub
	
	Private Sub DropTmp見積明細CSV()
		'#Tmp見積明細CSVテーブルを削除する
		
		'2013/03/13 ADD↓
		Dim cmd As New ADODB.Command
		Dim sql As String
		
		On Error Resume Next
		
		sql = "DROP TABLE #Tmp見積明細CSV"
		
		cmd.let_ActiveConnection(Cn)
		cmd.CommandTimeout = 0
		cmd.CommandText = sql
		cmd.CommandType = ADODB.CommandTypeEnum.adCmdText
		
		cmd.Execute(sql)
		
		'UPGRADE_NOTE: オブジェクト cmd をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
		cmd = Nothing
		
		On Error GoTo 0
		'2013/03/13 ADD↑
		
	End Sub
	''''
	''''Private Sub OutputTmpCheck()
	''''''    Dim rs As adodb.Recordset, SQL As String
	''''    Dim cmd As New ADODB.Command
	''''''    Dim Grs(0) As adodb.Recordset
	''''
	''''    cmd.ActiveConnection = Cn
	''''    cmd.CommandTimeout = 0                      '2007/09/14 ADD
	''''''    cmd.CommandText = "sp_CreateTmp見積"
	''''    cmd.CommandText = "usp_CreateTmp見積明細Check"
	''''    cmd.CommandType = adCmdStoredProc
	''''    cmd.Parameters.Refresh
	''''
	''''    ' それぞれのパラメータの値を指定する
	''''    cmd.Parameters(0).Direction = adParamReturnValue
	''''    cmd.Parameters(1).Value = GetPCName
	''''    Call cmd.Execute
	''''
	''''    TMPTABLE_Check = cmd(2)
	''''
	''''    Set cmd = Nothing
	''''End Sub
	
	Private Function InsDataTmp() As Boolean
		Dim fpSpd As Object
		'チェック用に画面のデータをワークテーブルに書き込むモジュール
		Dim i, j As Short
		Dim wMaxRow As Short
		Dim getdata As Object
		Dim check As Boolean
		Dim sql As String
		Dim rs As ADODB.Recordset
		
		'サイズ変換用
		Dim H_W As Integer
		Dim H_D As Integer
		Dim H_H As Integer
		Dim H_D1 As Integer
		Dim H_D2 As Integer
		Dim H_H1 As Integer
		Dim H_H2 As Integer
		
		InsDataTmp = False
		
		'UPGRADE_WARNING: オブジェクト fpSpd.DataRowCnt の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		If fpSpd.DataRowCnt = 0 Then
			CriticalAlarm("明細がありません。")
			'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			fpSpd.Col = fpSpd.ActiveRow
			'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			fpSpd.Row = fpSpd.ActiveRow
			'UPGRADE_WARNING: オブジェクト fpSpd.Action の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			fpSpd.Action = FPSpreadADO.ActionConstants.ActionActiveCell
			'UPGRADE_WARNING: オブジェクト fpSpd.SetFocus の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			fpSpd.SetFocus()
			Exit Function
		End If
		
		'UPGRADE_WARNING: 配列 WkTBLS の下限が 1,1 から 0,0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="0F1C9BE1-AF9D-476E-83B1-17D43BECFF20"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト fpSpd.MaxCols の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト fpSpd.DataRowCnt の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		ReDim WkTBLS(fpSpd.DataRowCnt, fpSpd.MaxCols)
		'スプレッド上のデータを配列に
		'UPGRADE_WARNING: オブジェクト fpSpd.GetArray の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Call fpSpd.GetArray(1, 1, WkTBLS)
		
		'UPGRADE_WARNING: オブジェクト WkTBLS() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		If Trim(WkTBLS(1, ColW)) = "@" Or Trim(WkTBLS(1, ColD)) = "@" Or Trim(WkTBLS(1, ColH)) = "@" Or Trim(WkTBLS(1, ColD1)) = "@" Or Trim(WkTBLS(1, ColD2)) = "@" Or Trim(WkTBLS(1, ColH1)) = "@" Or Trim(WkTBLS(1, ColH2)) = "@" Then
			CriticalAlarm("一行目に＠は使用できません。")
			Exit Function
		End If
		
		'未入力最終明細サーチ
		For wMaxRow = UBound(WkTBLS, 1) To LBound(WkTBLS, 1) Step -1
			If IsCheckNull(WkTBLS(wMaxRow, ColPC区分)) = False Then Exit For 'PC区分
			If IsCheckNull(WkTBLS(wMaxRow, Col製品NO)) = False Then Exit For '製品NO
			If IsCheckNull(WkTBLS(wMaxRow, Col仕様NO)) = False Then Exit For '仕様NO
			If IsCheckNull(WkTBLS(wMaxRow, Col名称)) = False Then Exit For '名称
			''''        If IsCheckNull(WkTBLS(wMaxRow, 24)) = False Then Exit For               '仕入先CD
			''''        If IsCheckNull(WkTBLS(wMaxRow, 25)) = False Then Exit For               '配送先CD
		Next 
		
		'''    Set rs = OpenRs("[" & TMPTABLE & "]", Cn, adOpenKeyset, adLockPessimistic)
		rs = OpenRs("#Tmp見積明細", Cn, ADODB.CursorTypeEnum.adOpenKeyset, ADODB.LockTypeEnum.adLockPessimistic) '2013/03/13 ADD
		
		''    For i = 1 To UBound(WkTBLS)
		For i = 1 To wMaxRow
			rs.AddNew()
			'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			rs.Fields("見積明細連番").Value = SpcToNull(WkTBLS(i, Col見積明細連番))
			rs.Fields("見積番号").Value = pMituNo
			rs.Fields("行番号").Value = i
			rs.Fields("追番").Value = 0
			
			'UPGRADE_WARNING: オブジェクト WkTBLS(i, Col見積区分) の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			rs.Fields("見積区分").Value = WkTBLS(i, Col見積区分) '2003/10/29 ADD
			
			Select Case WkTBLS(i, Col見積区分) '2010/06/16 ADD
				Case "A", "C", "S" 'ｺﾒﾝﾄは除外する
					'2010/06/16 ADD↓
					rs.Fields("展開chk").Value = 0
					rs.Fields("SP区分").Value = ""
					rs.Fields("PC区分").Value = ""
					rs.Fields("製品NO").Value = ""
					rs.Fields("仕様NO").Value = ""
					rs.Fields("ベース色").Value = ""
					'UPGRADE_WARNING: オブジェクト WkTBLS(i, Col見積区分) の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					If WkTBLS(i, Col見積区分) = "S" Then
						rs.Fields("漢字名称").Value = ""
					Else
						'UPGRADE_WARNING: オブジェクト WkTBLS(i, Col名称) の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						rs.Fields("漢字名称").Value = WkTBLS(i, Col名称)
					End If
					rs.Fields("W").Value = 0
					rs.Fields("D").Value = 0
					rs.Fields("H").Value = 0
					rs.Fields("D1").Value = 0
					rs.Fields("D2").Value = 0
					rs.Fields("H1").Value = 0
					rs.Fields("H2").Value = 0
					rs.Fields("エラー内容").Value = ""
					rs.Fields("定価").Value = 0
					rs.Fields("仕入単価").Value = 0
					rs.Fields("仕入率").Value = 0
					rs.Fields("売上単価").Value = 0
					rs.Fields("売上率").Value = 0
					'rs![U区分] = ""
					'UPGRADE_WARNING: オブジェクト WkTBLS(i, ColU区分) の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					rs.Fields("U区分").Value = WkTBLS(i, ColU区分) '2017/07/03 ADD
					rs.Fields("M区分").Value = ""
					rs.Fields("見積数量").Value = 0
					rs.Fields("単位名").Value = ""
					rs.Fields("売上金額").Value = 0
					rs.Fields("仕入金額").Value = 0
					rs.Fields("売上税区分").Value = 0
					rs.Fields("消費税額").Value = 0
					'2017/03/10 ADD↓
					rs.Fields("仕入業者CD").Value = ""
					rs.Fields("仕入業者名").Value = ""
					'2017/03/10 ADD↑
					rs.Fields("仕入先CD").Value = ""
					rs.Fields("仕入先名").Value = ""
					rs.Fields("配送先CD").Value = ""
					rs.Fields("配送先名").Value = ""
					rs.Fields("総数量").Value = 0
					rs.Fields("発注数").Value = 0
					rs.Fields("転用数").Value = 0
					rs.Fields("客先在庫数").Value = 0
					rs.Fields("社内在庫数").Value = 0
					
					'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
					rs.Fields("製品区分").Value = System.DBNull.Value
					
					For j = 1 To 30
						rs.Fields("仕分数" & j).Value = 0
					Next 
					
					rs.Fields("社内在庫数FLG").Value = 0
					rs.Fields("社内適正在庫数FLG").Value = 0
					rs.Fields("客先在庫数FLG").Value = 0
					
					rs.Fields("発注調整数").Value = 0 '2014/09/09 ADD
					
					rs.Fields("他社伝票番号").Value = "" '2014/02/24 ADD
					'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
					rs.Fields("他社納品日付").Value = System.DBNull.Value '2015/09/29 ADD
					
					'2016/06/22 ADD↓
					rs.Fields("クレーム明細区分").Value = 0
					rs.Fields("作業区分CD").Value = 0
					'2016/06/22 ADD↑
					
					'2010/06/16 ADD↑
					rs.Fields("明細備考").Value = "" '2018/05/03 ADD
					'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
					rs.Fields("入庫日").Value = System.DBNull.Value '2018/05/03 ADD
					'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
					rs.Fields("出庫日").Value = System.DBNull.Value '2020/09/16 ADD
					
					
					rs.Fields("追番R").Value = "" '2019/12/12 ADD
				Case Else
					
					'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					rs.Fields("展開chk").Value = SpcToNull(WkTBLS(i, 1), 0)
					'UPGRADE_WARNING: オブジェクト WkTBLS(i, ColSP区分) の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					rs.Fields("SP区分").Value = WkTBLS(i, ColSP区分)
					'UPGRADE_WARNING: オブジェクト WkTBLS(i, ColPC区分) の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					rs.Fields("PC区分").Value = WkTBLS(i, ColPC区分)
					'UPGRADE_WARNING: オブジェクト WkTBLS(i, Col製品NO) の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					rs.Fields("製品NO").Value = WkTBLS(i, Col製品NO)
					'UPGRADE_WARNING: オブジェクト WkTBLS(i, Col仕様NO) の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					rs.Fields("仕様NO").Value = WkTBLS(i, Col仕様NO)
					'UPGRADE_WARNING: オブジェクト WkTBLS(i, Colベース色) の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					rs.Fields("ベース色").Value = WkTBLS(i, Colベース色)
					'UPGRADE_WARNING: オブジェクト WkTBLS(i, Col名称) の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					rs.Fields("漢字名称").Value = WkTBLS(i, Col名称)
					'UPGRADE_WARNING: オブジェクト WkTBLS() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					If InStr(WkTBLS(i, ColW), "@") <> 0 Then
						rs.Fields("W").Value = H_W
					Else
						'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						rs.Fields("W").Value = SpcToNull(WkTBLS(i, ColW), 0)
						'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						H_W = SpcToNull(WkTBLS(i, ColW), 0)
					End If
					'UPGRADE_WARNING: オブジェクト SpcToNull(WkTBLS(i, ColD), 0) の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					If SpcToNull(WkTBLS(i, ColD), 0) = "@" Then
						rs.Fields("D").Value = H_D
					Else
						'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						rs.Fields("D").Value = SpcToNull(WkTBLS(i, ColD), 0)
						'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						H_D = SpcToNull(WkTBLS(i, ColD), 0)
					End If
					'UPGRADE_WARNING: オブジェクト SpcToNull(WkTBLS(i, ColH), 0) の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					If SpcToNull(WkTBLS(i, ColH), 0) = "@" Then
						rs.Fields("H").Value = H_H
					Else
						'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						rs.Fields("H").Value = SpcToNull(WkTBLS(i, ColH), 0)
						'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						H_H = SpcToNull(WkTBLS(i, ColH), 0)
					End If
					'UPGRADE_WARNING: オブジェクト SpcToNull(WkTBLS(i, ColD1), 0) の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					If SpcToNull(WkTBLS(i, ColD1), 0) = "@" Then
						rs.Fields("D1").Value = H_D1
					Else
						'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						rs.Fields("D1").Value = SpcToNull(WkTBLS(i, ColD1), 0)
						'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						H_D1 = SpcToNull(WkTBLS(i, ColD1), 0)
					End If
					'UPGRADE_WARNING: オブジェクト SpcToNull(WkTBLS(i, ColD2), 0) の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					If SpcToNull(WkTBLS(i, ColD2), 0) = "@" Then
						rs.Fields("D2").Value = H_D2
					Else
						'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						rs.Fields("D2").Value = SpcToNull(WkTBLS(i, ColD2), 0)
						'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						H_D2 = SpcToNull(WkTBLS(i, ColD2), 0)
					End If
					'UPGRADE_WARNING: オブジェクト SpcToNull(WkTBLS(i, ColH1), 0) の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					If SpcToNull(WkTBLS(i, ColH1), 0) = "@" Then
						rs.Fields("H1").Value = H_H1
					Else
						'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						rs.Fields("H1").Value = SpcToNull(WkTBLS(i, ColH1), 0)
						'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						H_H1 = SpcToNull(WkTBLS(i, ColH1), 0)
					End If
					'UPGRADE_WARNING: オブジェクト SpcToNull(WkTBLS(i, ColH2), 0) の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					If SpcToNull(WkTBLS(i, ColH2), 0) = "@" Then
						rs.Fields("H2").Value = H_H2
					Else
						'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						rs.Fields("H2").Value = SpcToNull(WkTBLS(i, ColH2), 0)
						'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						H_H2 = SpcToNull(WkTBLS(i, ColH2), 0)
					End If
					'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					rs.Fields("エラー内容").Value = NullToZero(WkTBLS(i, Colエラー内容), vbNullString)
					'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					rs.Fields("定価").Value = NullToZero(WkTBLS(i, Col定価), 0)
					'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					rs.Fields("仕入単価").Value = NullToZero(WkTBLS(i, Col原価), 0)
					'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					rs.Fields("仕入率").Value = NullToZero(WkTBLS(i, Col仕入率), 0)
					'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					rs.Fields("売上単価").Value = NullToZero(WkTBLS(i, Col売価), 0)
					'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					rs.Fields("売上率").Value = NullToZero(WkTBLS(i, Col売上率), 0)
					'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					rs.Fields("U区分").Value = NullToZero(WkTBLS(i, ColU区分), vbNullString) '2005/10/14.ADD
					'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					rs.Fields("M区分").Value = NullToZero(WkTBLS(i, ColM), vbNullString)
					'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					rs.Fields("見積数量").Value = NullToZero(WkTBLS(i, Col見積数量), 0)
					'UPGRADE_WARNING: オブジェクト WkTBLS(i, Col単位) の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					rs.Fields("単位名").Value = WkTBLS(i, Col単位)
					'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					rs.Fields("売上金額").Value = NullToZero(WkTBLS(i, Col金額), 0)
					'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					rs.Fields("仕入金額").Value = NullToZero(WkTBLS(i, Col仕入金額), 0)
					'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					rs.Fields("売上税区分").Value = NullToZero(WkTBLS(i, Col売上税区分), 0)
					'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					rs.Fields("消費税額").Value = NullToZero(WkTBLS(i, Col消費税額), 0)
					
					'2017/03/10 ADD↓
					'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					rs.Fields("仕入業者CD").Value = NullToZero(WkTBLS(i, Col仕入業者CD), vbNullString)
					'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					rs.Fields("仕入業者名").Value = NullToZero(WkTBLS(i, Col仕入業者名), vbNullString)
					'2017/03/10 ADD↑
					
					'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					rs.Fields("仕入先CD").Value = NullToZero(WkTBLS(i, Col仕入先CD), vbNullString)
					'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					rs.Fields("仕入先名").Value = NullToZero(WkTBLS(i, Col仕入先名), vbNullString)
					'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					rs.Fields("配送先CD").Value = NullToZero(WkTBLS(i, Col送り先CD), vbNullString)
					'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					rs.Fields("配送先名").Value = NullToZero(WkTBLS(i, Col送り先名), vbNullString)
					''''        rs![総数量] = NullToZero(WkTBLS(i, SiwakeCol - 1), 0)
					'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					rs.Fields("総数量").Value = NullToZero(WkTBLS(i, Col数量合計), 0)
					''''        rs![発注数] = NullToZero(WkTBLS(i, SiwakeCol - 2), 0)
					'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					rs.Fields("発注数").Value = NullToZero(WkTBLS(i, Col発注数), 0)
					''''        rs![転用数] = NullToZero(WkTBLS(i, SiwakeCol - 3), 0)
					'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					rs.Fields("転用数").Value = NullToZero(WkTBLS(i, Col転用), 0)
					''''        rs![客先在庫数] = NullToZero(WkTBLS(i, SiwakeCol - 4), 0)
					'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					rs.Fields("客先在庫数").Value = NullToZero(WkTBLS(i, Col客先在庫), 0)
					''''        rs![社内在庫数] = NullToZero(WkTBLS(i, SiwakeCol - 5), 0)
					'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					rs.Fields("社内在庫数").Value = NullToZero(WkTBLS(i, Col社内在庫), 0)
					
					'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					rs.Fields("発注調整数").Value = NullToZero(WkTBLS(i, Col発注調整数), 0) '2014/09/09 ADD
					
					'UPGRADE_WARNING: IsEmpty は、IsNothing にアップグレードされ、新しい動作が指定されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"' をクリックしてください。
					If IsNothing(WkTBLS(i, Col製品区分)) Then
						'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
						rs.Fields("製品区分").Value = System.DBNull.Value
						''            rs![製品区分] = 0
					Else
						'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						rs.Fields("製品区分").Value = SpcToNull(WkTBLS(i, Col製品区分))
					End If
					
					For j = 1 To 30
						''''            rs.Fields("仕分数" & j) = NullToZero(WkTBLS(i, SiwakeCol + j - 1))
						'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						rs.Fields("仕分数" & j).Value = NullToZero(WkTBLS(i, Col仕分数1 + j - 1))
					Next 
					
					'2009/10/05 ADD↓
					rs.Fields("社内在庫数FLG").Value = 0
					rs.Fields("社内適正在庫数FLG").Value = 0
					rs.Fields("客先在庫数FLG").Value = 0
					'2009/10/05 ADD↑
					
					'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					rs.Fields("他社伝票番号").Value = NullToZero(WkTBLS(i, Col他社伝票番号), vbNullString) '2014/02/24 ADD
					'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
					'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					rs.Fields("他社納品日付").Value = NullToZero(WkTBLS(i, Col他社納品日付), System.DBNull.Value) '2015/09/29 ADD
					
					'2016/06/22 ADD↓
					'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					rs.Fields("クレーム明細区分").Value = SpcToNull(WkTBLS(i, Colクレーム区分), 0)
					'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					rs.Fields("作業区分CD").Value = SpcToNull(WkTBLS(i, Col作業区分CD), 0)
					'2016/06/22 ADD↑
					
					'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					rs.Fields("明細備考").Value = NullToZero(WkTBLS(i, Col明細備考), vbNullString) '2018/05/03 ADD
					'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
					'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					rs.Fields("入庫日").Value = NullToZero(WkTBLS(i, Col入庫日), System.DBNull.Value) '2018/05/03 ADD
					'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
					'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					rs.Fields("出庫日").Value = NullToZero(WkTBLS(i, Col出庫日), System.DBNull.Value) '2020/09/16 ADD
					
					'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					rs.Fields("追番R").Value = NullToZero(WkTBLS(i, Col追番R), vbNullString) '2019/12/12 ADD
					
			End Select '2010/06/16 ADD
			
			rs.Update()
		Next 
		
		ReleaseRs(rs)
		
		InsDataTmp = True
	End Function
	
	Private Function InsDataTmpCheck() As Boolean
		'   エクセル取込で使用
		
		Dim i, j As Short
		Dim wMaxRow As Short
		Dim getdata As Object
		Dim check As Boolean
		Dim sql As String
		Dim rs As ADODB.Recordset
		Dim SeiKB As Short
		
		'サイズ変換用
		Dim H_W As Integer
		Dim H_D As Integer
		Dim H_H As Integer
		Dim H_D1 As Integer
		Dim H_D2 As Integer
		Dim H_H1 As Integer
		Dim H_H2 As Integer
		
		InsDataTmpCheck = False
		On Error GoTo InsDataTmpCheck_err
		
		'''    Set rs = OpenRs("[" & TMPTABLE_Check & "]", Cn, adOpenKeyset, adLockPessimistic)
		rs = OpenRs("#Tmp見積明細CSV", Cn, ADODB.CursorTypeEnum.adOpenKeyset, ADODB.LockTypeEnum.adLockPessimistic) '2013/03/13 ADD
		
		For i = 1 To UBound(TempData)
			''    For i = 1 To wMaxRow
			rs.AddNew()
			'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
			rs.Fields("見積明細連番").Value = System.DBNull.Value
			rs.Fields("見積番号").Value = pMituNo
			rs.Fields("行番号").Value = i
			rs.Fields("追番").Value = 0
			
			rs.Fields("展開chk").Value = 0
			'UPGRADE_WARNING: オブジェクト TempData() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			rs.Fields("見積区分").Value = TempData(i, 1)
			'UPGRADE_WARNING: オブジェクト TempData() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			rs.Fields("SP区分").Value = TempData(i, 2)
			'UPGRADE_WARNING: オブジェクト TempData() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			rs.Fields("PC区分").Value = TempData(i, 3)
			'UPGRADE_WARNING: オブジェクト TempData() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			rs.Fields("製品NO").Value = TempData(i, 4)
			'UPGRADE_WARNING: オブジェクト TempData() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			rs.Fields("仕様NO").Value = TempData(i, 5)
			'UPGRADE_WARNING: オブジェクト TempData() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			rs.Fields("ベース色").Value = TempData(i, 6)
			rs.Fields("他社伝票番号").Value = "" 'TempData(i, 7)  '2015/02/04 ADD 下は1カウント+
			'UPGRADE_WARNING: オブジェクト TempData() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			rs.Fields("漢字名称").Value = TempData(i, 8)
			'UPGRADE_WARNING: オブジェクト TempData() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			If InStr(TempData(i, 9), "@") <> 0 Then
				rs.Fields("W").Value = H_W
			Else
				'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				rs.Fields("W").Value = SpcToNull(TempData(i, 9), 0)
				'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				H_W = SpcToNull(TempData(i, 9), 0)
			End If
			'UPGRADE_WARNING: オブジェクト SpcToNull(TempData(i, 10), 0) の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			If SpcToNull(TempData(i, 10), 0) = "@" Then
				rs.Fields("D").Value = H_D
			Else
				'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				rs.Fields("D").Value = SpcToNull(TempData(i, 10), 0)
				'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				H_D = SpcToNull(TempData(i, 10), 0)
			End If
			'UPGRADE_WARNING: オブジェクト SpcToNull(TempData(i, 11), 0) の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			If SpcToNull(TempData(i, 11), 0) = "@" Then
				rs.Fields("H").Value = H_H
			Else
				'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				rs.Fields("H").Value = SpcToNull(TempData(i, 11), 0)
				'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				H_H = SpcToNull(TempData(i, 11), 0)
			End If
			'UPGRADE_WARNING: オブジェクト SpcToNull(TempData(i, 12), 0) の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			If SpcToNull(TempData(i, 12), 0) = "@" Then
				rs.Fields("D1").Value = H_D1
			Else
				'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				rs.Fields("D1").Value = SpcToNull(TempData(i, 12), 0)
				'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				H_D1 = SpcToNull(TempData(i, 12), 0)
			End If
			'UPGRADE_WARNING: オブジェクト SpcToNull(TempData(i, 13), 0) の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			If SpcToNull(TempData(i, 13), 0) = "@" Then
				rs.Fields("D2").Value = H_D2
			Else
				'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				rs.Fields("D2").Value = SpcToNull(TempData(i, 13), 0)
				'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				H_D2 = SpcToNull(TempData(i, 13), 0)
			End If
			'UPGRADE_WARNING: オブジェクト SpcToNull(TempData(i, 14), 0) の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			If SpcToNull(TempData(i, 14), 0) = "@" Then
				rs.Fields("H1").Value = H_H1
			Else
				'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				rs.Fields("H1").Value = SpcToNull(TempData(i, 14), 0)
				'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				H_H1 = SpcToNull(TempData(i, 14), 0)
			End If
			'UPGRADE_WARNING: オブジェクト SpcToNull(TempData(i, 15), 0) の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			If SpcToNull(TempData(i, 15), 0) = "@" Then
				rs.Fields("H2").Value = H_H2
			Else
				'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				rs.Fields("H2").Value = SpcToNull(TempData(i, 15), 0)
				'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				H_H2 = SpcToNull(TempData(i, 15), 0)
			End If
			rs.Fields("エラー内容").Value = vbNullString
			rs.Fields("定価").Value = 0
			rs.Fields("仕入単価").Value = 0
			rs.Fields("仕入率").Value = 0
			rs.Fields("売上単価").Value = 0
			rs.Fields("売上率").Value = 0
			rs.Fields("U区分").Value = vbNullString '2005/10/14.ADD
			rs.Fields("M区分").Value = vbNullString
			rs.Fields("見積数量").Value = 0
			'UPGRADE_WARNING: オブジェクト TempData() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			rs.Fields("単位名").Value = TempData(i, 19)
			rs.Fields("売上金額").Value = 0
			rs.Fields("仕入金額").Value = 0
			rs.Fields("売上税区分").Value = 0
			rs.Fields("消費税額").Value = 0
			rs.Fields("仕入先CD").Value = vbNullString
			rs.Fields("仕入先名").Value = vbNullString
			rs.Fields("配送先CD").Value = vbNullString
			rs.Fields("配送先名").Value = vbNullString
			'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			rs.Fields("総数量").Value = NullToZero(TempData(i, 28), 0)
			rs.Fields("発注数").Value = 0
			rs.Fields("社内在庫数").Value = 0
			rs.Fields("客先在庫数").Value = 0
			rs.Fields("転用数").Value = 0
			
			'UPGRADE_WARNING: オブジェクト TempData() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			SeiKB = Get製品区分(CStr(TempData(i, 3)), CStr(TempData(i, 4)), CStr(TempData(i, 5)))
			Select Case SeiKB
				Case -1
					'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
					rs.Fields("製品区分").Value = System.DBNull.Value
				Case 0, 1, 2, 3
					rs.Fields("製品区分").Value = SeiKB
			End Select
			
			'エクセル側で新規の製品行が存在した場合、新しい行を作成する。
			For j = 1 To 30
				If SiwaNo = j Then
					'指定の仕分番目にも総数量(26)をセットしておく。(インサートの時に使用する)
					'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					rs.Fields("仕分数" & j).Value = NullToZero(TempData(i, 28), 0)
				Else
					rs.Fields("仕分数" & j).Value = 0
				End If
			Next 
			
			'2009/10/05 ADD↓
			rs.Fields("社内在庫数FLG").Value = 0
			rs.Fields("社内適正在庫数FLG").Value = 0
			rs.Fields("客先在庫数FLG").Value = 0
			'2009/10/05 ADD↑
			
			rs.Fields("発注調整数").Value = 0 '2014/09/09 ADD
			
			'2016/06/22 ADD↓
			rs.Fields("クレーム明細区分").Value = 0
			rs.Fields("作業区分CD").Value = 0
			'2016/06/22 ADD↑
			
			'2017/03/10 ADD↓
			rs.Fields("仕入業者CD").Value = ""
			rs.Fields("仕入業者名").Value = ""
			'2017/03/10 ADD↑
			
			rs.Fields("明細備考").Value = "" '2018/05/03 ADD
			'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
			rs.Fields("入庫日").Value = System.DBNull.Value '2018/05/03 ADD
			'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
			rs.Fields("出庫日").Value = System.DBNull.Value '2020/09/16 ADD
			
			rs.Fields("追番R").Value = "" '2019/12/12 ADD
			
			rs.Update()
		Next 
		
		ReleaseRs(rs)
		
		InsDataTmpCheck = True
		'2009/08/26 ADD ↓
		Exit Function
		
InsDataTmpCheck_err: '---エラー時
		MsgBox("明細" & i & "行目" & vbCrLf & Err.Number & " " & Err.Description)
		'    MsgBox Err.Number & " " & Err.Description
	End Function
	
	'2018/04/11 ADD↓
	Private Function InsDataTmpCheck_Revit() As Boolean
		'   レビット取込で使用
		
		Dim i, j As Short
		Dim wMaxRow As Short
		Dim getdata As Object
		Dim check As Boolean
		Dim sql As String
		Dim rs As ADODB.Recordset
		Dim SeiKB As Short
		
		'サイズ変換用
		Dim H_W As Integer
		Dim H_D As Integer
		Dim H_H As Integer
		Dim H_D1 As Integer
		Dim H_D2 As Integer
		Dim H_H1 As Integer
		Dim H_H2 As Integer
		
		InsDataTmpCheck_Revit = False
		On Error GoTo InsDataTmpCheck_err
		
		'''    Set rs = OpenRs("[" & TMPTABLE_Check & "]", Cn, adOpenKeyset, adLockPessimistic)
		rs = OpenRs("#Tmp見積明細CSV", Cn, ADODB.CursorTypeEnum.adOpenKeyset, ADODB.LockTypeEnum.adLockPessimistic) '2013/03/13 ADD
		
		For i = 1 To UBound(TempData)
			''    For i = 1 To wMaxRow
			rs.AddNew()
			'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
			rs.Fields("見積明細連番").Value = System.DBNull.Value
			rs.Fields("見積番号").Value = pMituNo
			rs.Fields("行番号").Value = i
			rs.Fields("追番").Value = 0
			
			rs.Fields("展開chk").Value = 0
			'UPGRADE_WARNING: オブジェクト TempData() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			rs.Fields("見積区分").Value = TempData(i, 1)
			'UPGRADE_WARNING: オブジェクト TempData() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			rs.Fields("SP区分").Value = TempData(i, 2)
			'UPGRADE_WARNING: オブジェクト TempData() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			rs.Fields("PC区分").Value = TempData(i, 3)
			'UPGRADE_WARNING: オブジェクト TempData() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			rs.Fields("製品NO").Value = TempData(i, 4)
			'UPGRADE_WARNING: オブジェクト TempData() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			rs.Fields("仕様NO").Value = TempData(i, 5)
			'UPGRADE_WARNING: オブジェクト TempData() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			rs.Fields("ベース色").Value = TempData(i, 6)
			rs.Fields("他社伝票番号").Value = "" 'TempData(i, 7)  '2015/02/04 ADD 下は1カウント+
			'UPGRADE_WARNING: オブジェクト TempData() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			rs.Fields("漢字名称").Value = TempData(i, 8)
			'UPGRADE_WARNING: オブジェクト TempData() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			If InStr(TempData(i, 9), "@") <> 0 Then
				rs.Fields("W").Value = H_W
			Else
				'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				rs.Fields("W").Value = SpcToNull(TempData(i, 9), 0)
				'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				H_W = SpcToNull(TempData(i, 9), 0)
			End If
			'UPGRADE_WARNING: オブジェクト SpcToNull(TempData(i, 10), 0) の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			If SpcToNull(TempData(i, 10), 0) = "@" Then
				rs.Fields("D").Value = H_D
			Else
				'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				rs.Fields("D").Value = SpcToNull(TempData(i, 10), 0)
				'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				H_D = SpcToNull(TempData(i, 10), 0)
			End If
			'UPGRADE_WARNING: オブジェクト SpcToNull(TempData(i, 11), 0) の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			If SpcToNull(TempData(i, 11), 0) = "@" Then
				rs.Fields("H").Value = H_H
			Else
				'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				rs.Fields("H").Value = SpcToNull(TempData(i, 11), 0)
				'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				H_H = SpcToNull(TempData(i, 11), 0)
			End If
			'UPGRADE_WARNING: オブジェクト SpcToNull(TempData(i, 12), 0) の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			If SpcToNull(TempData(i, 12), 0) = "@" Then
				rs.Fields("D1").Value = H_D1
			Else
				'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				rs.Fields("D1").Value = SpcToNull(TempData(i, 12), 0)
				'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				H_D1 = SpcToNull(TempData(i, 12), 0)
			End If
			'UPGRADE_WARNING: オブジェクト SpcToNull(TempData(i, 13), 0) の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			If SpcToNull(TempData(i, 13), 0) = "@" Then
				rs.Fields("D2").Value = H_D2
			Else
				'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				rs.Fields("D2").Value = SpcToNull(TempData(i, 13), 0)
				'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				H_D2 = SpcToNull(TempData(i, 13), 0)
			End If
			'UPGRADE_WARNING: オブジェクト SpcToNull(TempData(i, 14), 0) の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			If SpcToNull(TempData(i, 14), 0) = "@" Then
				rs.Fields("H1").Value = H_H1
			Else
				'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				rs.Fields("H1").Value = SpcToNull(TempData(i, 14), 0)
				'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				H_H1 = SpcToNull(TempData(i, 14), 0)
			End If
			'UPGRADE_WARNING: オブジェクト SpcToNull(TempData(i, 15), 0) の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			If SpcToNull(TempData(i, 15), 0) = "@" Then
				rs.Fields("H2").Value = H_H2
			Else
				'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				rs.Fields("H2").Value = SpcToNull(TempData(i, 15), 0)
				'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				H_H2 = SpcToNull(TempData(i, 15), 0)
			End If
			rs.Fields("エラー内容").Value = vbNullString
			rs.Fields("定価").Value = 0
			rs.Fields("仕入単価").Value = 0
			rs.Fields("仕入率").Value = 0
			rs.Fields("売上単価").Value = 0
			rs.Fields("売上率").Value = 0
			rs.Fields("U区分").Value = vbNullString '2005/10/14.ADD
			rs.Fields("M区分").Value = vbNullString
			rs.Fields("見積数量").Value = 0
			'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			rs.Fields("単位名").Value = NullToZero(TempData(i, 19), "")
			rs.Fields("売上金額").Value = 0
			rs.Fields("仕入金額").Value = 0
			rs.Fields("売上税区分").Value = 0
			rs.Fields("消費税額").Value = 0
			rs.Fields("仕入先CD").Value = vbNullString
			rs.Fields("仕入先名").Value = vbNullString
			rs.Fields("配送先CD").Value = vbNullString
			rs.Fields("配送先名").Value = vbNullString
			'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			rs.Fields("総数量").Value = NullToZero(TempData(i, 28), 0)
			rs.Fields("発注数").Value = 0
			rs.Fields("社内在庫数").Value = 0
			rs.Fields("客先在庫数").Value = 0
			'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			rs.Fields("転用数").Value = NullToZero(TempData(i, 26), 0)
			
			'UPGRADE_WARNING: オブジェクト TempData() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			SeiKB = Get製品区分(CStr(TempData(i, 3)), CStr(TempData(i, 4)), CStr(TempData(i, 5)))
			Select Case SeiKB
				Case -1
					'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
					rs.Fields("製品区分").Value = System.DBNull.Value
				Case 0, 1, 2, 3
					rs.Fields("製品区分").Value = SeiKB
			End Select
			
			'エクセル側で新規の製品行が存在した場合、新しい行を作成する。
			For j = 1 To 30
				'明細の仕分レベルの値を元にセット
				'UPGRADE_WARNING: オブジェクト TempData(i, 29) の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				If TempData(i, 29) = j Then '仕分レベル
					'指定の仕分番目にも総数量(26)をセットしておく。(インサートの時に使用する)
					'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					rs.Fields("仕分数" & j).Value = NullToZero(TempData(i, 28), 0)
				Else
					rs.Fields("仕分数" & j).Value = 0
				End If
			Next 
			
			'2009/10/05 ADD↓
			rs.Fields("社内在庫数FLG").Value = 0
			rs.Fields("社内適正在庫数FLG").Value = 0
			rs.Fields("客先在庫数FLG").Value = 0
			'2009/10/05 ADD↑
			
			rs.Fields("発注調整数").Value = 0 '2014/09/09 ADD
			
			'2016/06/22 ADD↓
			rs.Fields("クレーム明細区分").Value = 0
			rs.Fields("作業区分CD").Value = 0
			'2016/06/22 ADD↑
			
			'2017/03/10 ADD↓
			rs.Fields("仕入業者CD").Value = ""
			rs.Fields("仕入業者名").Value = ""
			'2017/03/10 ADD↑
			
			rs.Fields("明細備考").Value = "" '2018/05/03 ADD
			'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
			rs.Fields("入庫日").Value = System.DBNull.Value '2018/05/03 ADD
			'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
			rs.Fields("出庫日").Value = System.DBNull.Value '2020/09/16 ADD
			
			'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			rs.Fields("追番R").Value = NullToZero(TempData(i, 30), "") '2019/12/12 ADD
			
			rs.Update()
		Next 
		
		ReleaseRs(rs)
		
		InsDataTmpCheck_Revit = True
		'2009/08/26 ADD ↓
		Exit Function
		
InsDataTmpCheck_err: '---エラー時
		MsgBox("明細" & i & "行目" & vbCrLf & Err.Number & " " & Err.Description)
		'    MsgBox Err.Number & " " & Err.Description
	End Function
	'2018/04/11 ADD↑
	
	Private Function Download(ByRef MituNo As Integer) As Short
		Dim fpSpd As Object
		Dim rs As ADODB.Recordset
		Dim sql As String
		Dim wMeisai As Object
		Dim wLine As Short
		Dim i As Short
		Dim cnTimeOut As Integer '2004/12/07
		
		On Error GoTo Download_Err
		'マウスポインターを砂時計にする
		HourGlass(True)
		
		Download = 0
		
		'''    cnTimeOut = Cn.ConnectionTimeout
		'''    Cn.ConnectionTimeout = 0
		
		'''''    '---見積ヘッダーセット
		'''''    SQL = "SELECT M.受注区分, M.見積日付, M.見積件名, M.見積件名, M.得意先CD, M.得意先名1, " _
		''''''        & "M.合計金額, M.原価率, M.大小口区分, TM.売上端数, TM.消費税端数 " _
		''''''        & "FROM TD見積 AS M " _
		''''''        & "LEFT JOIN TM得意先 AS TM ON M.得意先CD = TM.得意先CD " _
		''''''        & "WHERE M.見積番号 = " & MituNo
		'''''
		'''''    Set rs = OpenRs(SQL, Cn, adOpenForwardOnly, adLockReadOnly)
		'''''
		'''''    With rs
		'''''        If Not .EOF Then
		'''''            rf_見積番号 = MituNo
		'''''            rf_見積名称 = ![見積件名]
		'''''            rf_得意先CD = ![得意先CD]
		'''''            rf_得意先名 = ![得意先名1]
		'''''            rf_合計金額 = NullToZero(Format$(![合計金額], "#,##0"), 0) & " (円)"
		'''''            rf_原価率 = NullToZero(![原価率], 0)
		'''''            tx_売上端数 = ![売上端数]
		'''''            tx_消費税端数 = ![消費税端数]
		'''''            tx_見積日付 = ![見積日付]
		'''''            tx_受注区分 = ![受注区分]
		'''''            tx_大小口区分 = ![大小口区分]
		'''''        End If
		'''''    End With
		rf_見積番号.Text = VB6.Format(MituNo, "#")
		rf_見積名称.Text = HD_見積件名
		[rf_得意先CD].Text = HD_得意先CD
		rf_得意先名.Text = HD_得意先名1 & " " & HD_得意先名2
		'            rf_合計金額 = NullToZero(Format$(HD_合計金額, "#,##0"), 0) & " (円)"
		'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		rf_合計金額.Text = NullToZero(VB6.Format(HD_合計金額, "#,##0" & KIN_FMT), 0) '2014/07/10 ADD
		'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		rf_原価率.Text = NullToZero(HD_原価率, 0)
		tx_売上端数.Text = CStr(HD_売上端数)
		tx_消費税端数.Text = CStr(HD_消費税端数)
		'UPGRADE_WARNING: オブジェクト HD_見積日付 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		tx_見積日付.Text = HD_見積日付
		tx_受注区分.Text = CStr(HD_受注区分)
		[tx_大小口区分].Text = CStr(HD_大小口区分)
		'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		rf_得意先別見積番号.Text = NullToZero(HD_得意先別見積番号, "") '2005/07/04 ADD
		
		'    On Error Resume Next'2020/03/31 DEL
		''    ZEI = Get税率(idc(1).Text)
		gZEI = Get税率(CDate(tx_見積日付.Text))
		On Error GoTo 0
		
		'---見積明細セット
		'''    sql = "SELECT * " _
		''''        & "FROM TD見積シートM AS MM " _
		''''        & "WHERE MM.見積番号 = " & MituNo _
		''''        & " ORDER BY 行番号"
		'''''    '2014/09/09 ADD
		'''''    sql = "SELECT  MM.*,ISNULL(HCM.仕入済数,0) AS 仕入済数"
		'''''    sql = sql & "    FROM    TD見積シートM AS MM"
		'''''    sql = sql & "    LEFT JOIN   ("
		'''''    sql = sql & "                 SELECT 見積明細連番, 仕入済数 = SUM(仕入数量)"
		'''''    sql = sql & "                    From TD仕入明細内訳"
		'''''    sql = sql & "                    GROUP BY 見積明細連番"
		'''''    sql = sql & "                 )AS HCM"
		'''''    sql = sql & "                 ON MM.見積明細連番 = HCM.見積明細連番"
		'''''    sql = sql & "    WHERE MM.見積番号 = " & MituNo
		'''''    sql = sql & "    ORDER BY 行番号"
		
		'2014/10/28 ADD
		'''    sql = "SELECT  MM.*,ISNULL(HCM.仕入済数,0) AS 仕入済数,ISNULL(UM.売上済CNT,0) AS 売上済CNT"
		sql = "SELECT  MM.*,ISNULL(HCM.仕入済数,0) AS 仕入済数,ISNULL(HCM.仕入済CNT,0) AS 仕入済CNT,"
		sql = sql & "ISNULL(UM.売上済CNT,0) AS 売上済CNT"
		'    sql = sql & "  ,在庫区分 = ISNULL(SE.在庫区分,1)" '2015/10/26 ADD
		sql = sql & "  ,サイズ変更区分 = ISNULL(SE.サイズ変更区分,1)" '2015/10/26 ADD
		sql = sql & "        FROM    TD見積シートM AS MM"
		sql = sql & "        LEFT JOIN   ("
		sql = sql & "                     SELECT 見積明細連番, 仕入済数 = SUM(仕入数量),仕入済CNT = COUNT(見積明細連番)"
		sql = sql & "                        FROM TD仕入明細内訳"
		sql = sql & "                        GROUP BY 見積明細連番"
		sql = sql & "                     )AS HCM"
		sql = sql & "                     ON MM.見積明細連番 = HCM.見積明細連番"
		sql = sql & "        LEFT JOIN   ("
		sql = sql & "                     SELECT 見積明細連番, 売上済CNT = COUNT(見積明細連番)"
		sql = sql & "                        FROM TD売上明細V2"
		sql = sql & "                        GROUP BY 見積明細連番"
		sql = sql & "                     )AS UM"
		sql = sql & "                     ON MM.見積明細連番 = UM.見積明細連番"
		'2015/10/26↓
		sql = sql & "        LEFT JOIN   ("
		'sql = sql & "                     SELECT 製品NO,仕様NO,在庫区分"
		sql = sql & "                     SELECT 製品NO,仕様NO,在庫区分,サイズ変更区分" '2020/10/10 ADd
		sql = sql & "                        FROM TM製品"
		sql = sql & "                     )AS SE"
		sql = sql & "                     ON MM.製品NO = SE.製品NO"
		sql = sql & "                     AND MM.仕様NO = SE.仕様NO"
		'2015/10/26↑
		sql = sql & "    WHERE MM.見積番号 = " & MituNo
		sql = sql & "    ORDER BY 行番号"
		
		
		rs = OpenRs(sql, Cn, ADODB.CursorTypeEnum.adOpenForwardOnly, ADODB.LockTypeEnum.adLockReadOnly)
		
		With rs
			If .EOF Then
				Call ModeIndicate(0)
				InitDate = Today
				Call InitialItems()
				wLine = 0
			Else
				Call ModeIndicate(1)
				InitDate = .Fields("初期登録日").Value
				Call SetupItems(rs, wLine)
			End If
		End With
		
		If wLine <> 0 Then
			
			'---見積明細内訳セット
			sql = "SELECT * FROM TD見積シート内訳 " & "WHERE 見積番号 = " & MituNo & " ORDER BY 行番号"
			
			rs = OpenRs(sql, Cn, ADODB.CursorTypeEnum.adOpenForwardOnly, ADODB.LockTypeEnum.adLockReadOnly)
			
			With rs
				If Not .EOF Then
					'UPGRADE_WARNING: 配列 wMeisai の下限が 1,1 から 0,0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="0F1C9BE1-AF9D-476E-83B1-17D43BECFF20"' をクリックしてください。
					ReDim wMeisai(wLine, 30)
					
					Do Until .EOF
						''                wMeisai(![行番号], ![仕分番号]) = NullToZero(![数量], 0)
						'UPGRADE_WARNING: オブジェクト wMeisai() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						wMeisai(.Fields("行番号"), .Fields("仕分番号")) = IIf(.Fields("数量").Value = 0, "", .Fields("数量").Value)
						.MoveNext()
					Loop 
					
					'UPGRADE_WARNING: オブジェクト fpSpd.AutoCalc の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					fpSpd.AutoCalc = False
					''''                Call fpSpd.SetArray(SiwakeCol, 1, wMeisai)
					'UPGRADE_WARNING: オブジェクト fpSpd.SetArray の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					Call fpSpd.SetArray(Col仕分数1, 1, wMeisai)
					'UPGRADE_WARNING: オブジェクト fpSpd.AutoCalc の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					fpSpd.AutoCalc = True
				End If
			End With
		End If
		
		'---見積明細内訳名称セット
		sql = "SELECT * FROM TD見積シート内訳名称 " & "WHERE 見積番号 = " & MituNo
		
		rs = OpenRs(sql, Cn, ADODB.CursorTypeEnum.adOpenForwardOnly, ADODB.LockTypeEnum.adLockReadOnly)
		
		With rs
			If Not .EOF Then
				'''            i = SiwakeCol
				i = Col仕分数1
				Do Until .EOF
					With fpSpd
						If rs.Fields("略称").Value = vbNullString Then
							'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.Row = 0
							'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.Col = i
							''''                        .Text = i - SiwakeCol + 1
							'UPGRADE_WARNING: オブジェクト fpSpd.Text の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.Text = i - Col仕分数1 + 1
						Else
							'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.Row = 0
							'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.Col = i
							'UPGRADE_WARNING: オブジェクト fpSpd.Text の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							.Text = rs.Fields("略称").Value
						End If
					End With
					i = i + 1
					.MoveNext()
				Loop 
			End If
		End With
		
		Call ReleaseRs(rs)
		
		Calc合計()
		
		HourGlass(False)
		'''    Cn.ConnectionTimeout = cnTimeOut
		Exit Function
Download_Err: 
		Call ReleaseRs(rs)
		MsgBox(Err.Number & " " & Err.Description)
		HourGlass(False)
	End Function
	
	Private Sub InitialItems()
		Call clsSPD_Renamed.sprClearText()
	End Sub
	
	'2015/09/29 DEL↓
	''''Private Sub SetupItems(rs As ADODB.Recordset, ByRef wMaxRow As Integer)
	''''    Dim RecArry() As Variant
	''''    Dim i As Integer, j As Integer
	'''''------------------------------------------
	'''''   UPDATE      2005/10/14  [U区分]項目追加
	'''''------------------------------------------
	''''
	''''    'シートのクリア
	''''    Call clsSPD.sprClearText
	''''
	'''''''    RecArry = rs.GetRows(fpSpd.MaxRows, , _
	'''''                Array("見積区分", "SP区分", "PC区分", "製品NO", "仕様NO", "ベース色", "漢字名称", _
	'''''                    "W", "D", "H", "D1", "D2", "H1", "H2", _
	'''''                    "エラー内容", "定価", "U区分", "仕入単価", "仕入率", "売上単価", "売上率", _
	'''''                    "M区分", "見積数量", "単位名", "売上金額", "仕入金額", "売上税区分", "消費税額", _
	'''''                    "仕入先CD", "仕入先名", "配送先CD", "配送先名", _
	'''''                    "社内在庫数", "社内在庫数", "客先在庫数", "客先在庫数", "転用数", "発注数", "総数量", _
	'''''                    "製品区分", "見積明細連番"))
	''''    '2014/09/09 ADD
	''''''    RecArry = rs.GetRows(fpSpd.MaxRows, , _
	'''''                Array("見積区分", "SP区分", "PC区分", "製品NO", "仕様NO", "ベース色", "漢字名称", _
	'''''                    "W", "D", "H", "D1", "D2", "H1", "H2", _
	'''''                    "エラー内容", "定価", "U区分", "仕入単価", "仕入率", "売上単価", "売上率", _
	'''''                    "M区分", "見積数量", "単位名", "売上金額", "仕入金額", "売上税区分", "消費税額", _
	'''''                    "仕入先CD", "仕入先名", "配送先CD", "配送先名", _
	'''''                    "社内在庫数", "社内在庫数", "客先在庫数", "客先在庫数", "転用数", "発注調整数", "発注数", "総数量", _
	'''''                    "製品区分", "見積明細連番", "仕入済数", "売上済CNT"))
	''''    '2015/02/04 ADD
	''''    RecArry = rs.GetRows(fpSpd.MaxRows, , _
	'''''                Array("見積区分", "SP区分", "PC区分", "製品NO", "仕様NO", "ベース色", "他社伝票番号", "漢字名称", _
	'''''                    "W", "D", "H", "D1", "D2", "H1", "H2", _
	'''''                    "エラー内容", "定価", "U区分", "仕入単価", "仕入率", "売上単価", "売上率", _
	'''''                    "M区分", "見積数量", "単位名", "売上金額", "仕入金額", "売上税区分", "消費税額", _
	'''''                    "仕入先CD", "仕入先名", "配送先CD", "配送先名", _
	'''''                    "社内在庫数", "社内在庫数", "客先在庫数", "客先在庫数", "転用数", "発注調整数", "発注数", "総数量", _
	'''''                    "製品区分", "見積明細連番", "仕入済数", "売上済CNT"))
	''''
	''''    For i = 0 To UBound(RecArry, 2)
	''''        For j = 0 To UBound(RecArry)
	''''
	''''
	''''''            Debug.Print "RecArry:" & RecArry(j, i) & " j:" & j
	''''            Select Case j + 2       'ZEROオリジンで１列あとからセットのため+2
	''''                Case ColW To ColH2
	''''                    'サイズ
	''''                    fpSpd.SetText j + 2, i + 1, IIf(Trim$("" & RecArry(j, i)) = "0", "", Trim$("" & RecArry(j, i)))
	''''                Case Col定価, Col原価, Col仕入率, Col売価, Col売上率, Col見積数量
	''''                    '定価・原価・仕入率・売価・売上率・見積数量
	''''                    fpSpd.SetText j + 2, i + 1, IIf(Trim$("" & RecArry(j, i)) = "0", "", Trim$("" & RecArry(j, i)))
	''''                Case Col金額, Col仕入金額
	''''                    '売上金額・仕入金額・×(売上税区分・消費税額)
	''''                    fpSpd.SetText j + 2, i + 1, IIf(Trim$("" & RecArry(j, i)) = "0", "", Trim$("" & RecArry(j, i)))
	''''                Case Col社内在庫, Col客先在庫, Col転用, Col発注数, Col数量合計, Col発注調整数
	''''                    '社内在庫・客先在庫・転用・発注数・総数量
	''''                    fpSpd.SetText j + 2, i + 1, IIf(Trim$("" & RecArry(j, i)) = "0", "", Trim$("" & RecArry(j, i)))
	''''                'Case 42 'Col数量合計 + 1 '38
	''''                Case 43 'Col数量合計 + 1 '38    '2015/02/04 ADD
	''''                    '製品区分
	''''                    fpSpd.SetText Col製品区分, i + 1, Trim$("" & RecArry(j, i))
	''''                'Case 43 'Col数量合計 + 2 '39
	''''                Case 44 'Col数量合計 + 2 '39    '2015/02/04 ADD
	''''                    '見積明細連番
	''''                    fpSpd.SetText Col見積明細連番, i + 1, Trim$("" & RecArry(j, i))
	''''                Case Col仕入先CD, Col仕入先名, Col送り先CD, Col送り先名
	'''''''                    If RecArry(0, i) = "C" Then                                              '2010/04/08 DEL
	''''                    If RecArry(0, i) = "C" Or RecArry(0, i) = "A" Or RecArry(0, i) = "S" Then   '2010/04/08 ADD
	''''                        fpSpd.SetText j + 2, i + 1, ""
	''''                    Else
	''''                        fpSpd.SetText j + 2, i + 1, Trim$("" & RecArry(j, i))
	''''                    End If
	''''                Case Col社内在庫参照, Col客先在庫参照               '2009/09/26 ADD
	''''                    '仮に社内在庫数と客先在庫数が入るので無視する
	''''                'Case 44 '2013/11/26 ADD
	''''                Case 45 '2015/02/04 ADD
	''''                    fpSpd.SetText Col仕入済数, i + 1, Trim$("" & RecArry(j, i))
	''''                    If Trim$("" & RecArry(j, i)) <> "0" Then
	''''                        '仕入発生してたらロックして変更不可にする
	''''                        With fpSpd
	''''                            .BlockMode = True
	''''                                'ロック
	''''                                .Col = Col展開
	''''                                .Col2 = Col展開
	''''                                .Row = i + 1
	''''                                .Row2 = i + 1
	''''                                .Lock = True
	''''
	''''                                .Col = ColSP区分
	''''                                .Col2 = Col定価
	''''                                .Row = i + 1
	''''                                .Row2 = i + 1
	''''                                .Lock = True
	''''
	''''                                .Col = Col原価
	''''                                .Col2 = Col仕入率
	''''                                .Row = i + 1
	''''                                .Row2 = i + 1
	''''                                .Lock = True
	''''
	''''                                .Col = Col仕入先CD
	''''                                .Col2 = Col送り先CD
	''''                                .Row = i + 1
	''''                                .Row2 = i + 1
	''''                                .Lock = True
	''''
	''''                            .BlockMode = False
	''''                        End With
	''''                        shiirezumiF = True
	''''                    End If
	''''                'Case 45 '2014/10/28 ADD
	''''                Case 46 '2015/02/04 ADD
	''''                    fpSpd.SetText Col売上済CNT, i + 1, Trim$("" & RecArry(j, i))
	''''                    If Trim$("" & RecArry(j, i)) <> "0" Then
	''''                        '仕入発生してたらロックして変更不可にする
	''''                        With fpSpd
	''''                            .BlockMode = True
	''''                                '全ての明細項目をロック
	''''                                .Col = Col展開
	'''''                                .Col2 = Col展開
	'''''                                .Row = i + 1
	'''''                                .Row2 = i + 1
	'''''                                .Lock = True
	'''''
	'''''                                .Col = ColSP区分
	'''''                                .Col2 = ColU区分
	'''''                                .Row = i + 1
	'''''                                .Row2 = i + 1
	'''''                                .Lock = True
	'''''
	'''''                                .Col = Col売価
	'''''                                .Col2 = Col売上率
	'''''                                .Row = i + 1
	'''''                                .Row2 = i + 1
	'''''                                .Lock = True
	'''''
	'''''                                .Col = Col仕入先CD
	''''                                .Col2 = Col売上済CNT
	''''                                .Row = i + 1
	''''                                .Row2 = i + 1
	''''                                .Lock = True
	''''
	''''                            .BlockMode = False
	''''                        End With
	''''                        shiirezumiF = True
	''''                    End If
	''''                Case Else
	''''                    fpSpd.SetText j + 2, i + 1, Trim$("" & RecArry(j, i))
	''''            End Select
	''''        Next
	''''        '----------------------
	''''        '---   2005/10/14.ADD
	'''''''        If RecArry(16, i) = "U" Then
	''''        If clsSPD.GetTextEX(ColU区分, CLng(i + 1)) = "U" Then '2015/03/04 ADD
	''''            Call RowBackColorSet(1, CLng(i + 1))
	''''        End If
	''''
	'''''''        '2009/09/26 ADD↓ '2013/07/19 DEL
	'''''''        If RecArry(39, i) = 0 Then '製品時
	'''''''            If cDspSyaZaiko.GetZaikoInfo(HD_納期S, HD_担当者CD, CStr(RecArry(3, i)), CStr(RecArry(4, i)), CLng(NullToZero(HD_見積番号))) = False Then
	'''''''                fpSpd.SetText Col社内在庫参照, i + 1, ""
	'''''''            Else
	'''''''                fpSpd.SetText Col社内在庫参照, i + 1, cDspSyaZaiko.合計在庫数
	'''''''            End If
	'''''''            If cDspKyakuZaiko.GetZaikoInfo(HD_納期S, HD_担当者CD, rf_得意先CD, CStr(RecArry(3, i)), CStr(RecArry(4, i)), CLng(NullToZero(HD_見積番号))) = False Then
	'''''''                fpSpd.SetText Col客先在庫参照, i + 1, ""
	'''''''            Else
	'''''''                fpSpd.SetText Col客先在庫参照, i + 1, cDspKyakuZaiko.合計在庫数
	'''''''            End If
	'''''''        End If
	'''''''        '2009/09/26 ADD↑ '2013/07/19 DEL
	''''
	''''    Next
	''''
	''''
	''''    wMaxRow = UBound(RecArry, 2) + 1
	''''
	''''    fpSpd.Col = 1
	''''    fpSpd.Row = 1
	''''    fpSpd.Action = ActionActiveCell
	''''End Sub
	
	'2015/09/29 ADD↓
	Private Sub SetupItems(ByRef rs As ADODB.Recordset, ByRef wMaxRow As Short)
		Dim fpSpd As Object
		Dim i, j As Short
		
		'シートのクリア
		Call clsSPD_Renamed.sprClearText()
		
		i = 1
		'2015/10/26 ADD↓
		''                        Dim cSeihin As clsSeihin
		''                        Set cSeihin = New clsSeihin
		'2015/10/26 ADD↑
		
		'UPGRADE_WARNING: オブジェクト fpSpd.ReDraw の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		fpSpd.ReDraw = False
		Do Until rs.EOF
			
			'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			fpSpd.SetText(Col見積区分, i, Trim(rs.Fields("見積区分").Value & ""))
			
			'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			fpSpd.SetText(Colクレーム区分, i, VB6.Format(Trim(rs.Fields("クレーム明細区分").Value & ""), "#")) '2016/06/22 ADD
			
			'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			fpSpd.SetText(Col他社伝票番号, i, Trim(rs.Fields("他社伝票番号").Value & ""))
			'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			fpSpd.SetText(Col他社納品日付, i, Trim(rs.Fields("他社納品日付").Value & ""))
			
			'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			fpSpd.SetText(ColSP区分, i, Trim(rs.Fields("SP区分").Value & ""))
			'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			fpSpd.SetText(ColPC区分, i, Trim(rs.Fields("PC区分").Value & ""))
			
			'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			fpSpd.SetText(Col製品NO, i, Trim(rs.Fields("製品NO").Value & ""))
			'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			fpSpd.SetText(Col仕様NO, i, Trim(rs.Fields("仕様NO").Value & ""))
			'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			fpSpd.SetText(Colベース色, i, Trim(rs.Fields("ベース色").Value & ""))
			'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			fpSpd.SetText(Col名称, i, Trim(rs.Fields("漢字名称").Value & ""))
			
			'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			fpSpd.SetText(ColW, i, VB6.Format(rs.Fields("W").Value & "", "#"))
			'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			fpSpd.SetText(ColD, i, VB6.Format(rs.Fields("D").Value & "", "#"))
			'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			fpSpd.SetText(ColH, i, VB6.Format(rs.Fields("H").Value & "", "#"))
			
			'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			fpSpd.SetText(ColD1, i, VB6.Format(rs.Fields("D1").Value & "", "#"))
			'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			fpSpd.SetText(ColD2, i, VB6.Format(rs.Fields("D2").Value & "", "#"))
			'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			fpSpd.SetText(ColH1, i, VB6.Format(rs.Fields("H1").Value & "", "#"))
			'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			fpSpd.SetText(ColH2, i, VB6.Format(rs.Fields("H2").Value & "", "#"))
			'2015/10/26 ADD↓
			With fpSpd
				'在庫管理する場合、名称をロックする。
				''                        If rs.Fields("在庫区分").Value = 0 Then
				If rs.Fields("サイズ変更区分").Value = 0 Then '2020/10/10 ADD
					'在庫管理する
					'UPGRADE_WARNING: オブジェクト fpSpd.BlockMode の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.BlockMode = True
					'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.Row = i
					'UPGRADE_WARNING: オブジェクト fpSpd.Row2 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.Row2 = i
					'.Col = Col名称
					'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.Col = ColW '2015/11/13 ADD
					'UPGRADE_WARNING: オブジェクト fpSpd.Col2 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.Col2 = ColH2
					'UPGRADE_WARNING: オブジェクト fpSpd.Lock の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.Lock = True
					'UPGRADE_WARNING: オブジェクト fpSpd.BlockMode の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.BlockMode = False
				Else
					'在庫管理しない
					'UPGRADE_WARNING: オブジェクト fpSpd.BlockMode の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.BlockMode = True
					'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.Row = i
					'UPGRADE_WARNING: オブジェクト fpSpd.Row2 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.Row2 = i
					'.Col = Col名称
					'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.Col = ColW '2015/11/13 ADD
					'UPGRADE_WARNING: オブジェクト fpSpd.Col2 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.Col2 = ColH2
					'UPGRADE_WARNING: オブジェクト fpSpd.Lock の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.Lock = False
					'UPGRADE_WARNING: オブジェクト fpSpd.BlockMode の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.BlockMode = False
				End If
			End With
			'2015/10/26 ADD↑
			
			
			'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			fpSpd.SetText(Colエラー内容, i, Trim(rs.Fields("エラー内容").Value & ""))
			'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			fpSpd.SetText(Col定価, i, IIf(Trim(rs.Fields("定価").Value & "") = "0", "", Trim(rs.Fields("定価").Value & "")))
			'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			fpSpd.SetText(ColU区分, i, Trim(rs.Fields("U区分").Value & ""))
			
			'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			fpSpd.SetText(Col原価, i, IIf(Trim(rs.Fields("仕入単価").Value & "") = "0", "", Trim(rs.Fields("仕入単価").Value & "")))
			'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			fpSpd.SetText(Col仕入率, i, IIf(Trim(rs.Fields("仕入率").Value & "") = "0", "", Trim(rs.Fields("仕入率").Value & "")))
			'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			fpSpd.SetText(Col売価, i, IIf(Trim(rs.Fields("売上単価").Value & "") = "0", "", Trim(rs.Fields("売上単価").Value & "")))
			'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			fpSpd.SetText(Col売上率, i, IIf(Trim(rs.Fields("売上率").Value & "") = "0", "", Trim(rs.Fields("売上率").Value & "")))
			
			'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			fpSpd.SetText(ColM, i, Trim(rs.Fields("M区分").Value & ""))
			
			'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			fpSpd.SetText(Col見積数量, i, IIf(Trim(rs.Fields("見積数量").Value & "") = "0", "", Trim(rs.Fields("見積数量").Value & "")))
			'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			fpSpd.SetText(Col単位, i, Trim(rs.Fields("単位名").Value & ""))
			'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			fpSpd.SetText(Col金額, i, IIf(Trim(rs.Fields("売上金額").Value & "") = "0", "", Trim(rs.Fields("売上金額").Value & "")))
			'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			fpSpd.SetText(Col仕入金額, i, IIf(Trim(rs.Fields("仕入金額").Value & "") = "0", "", Trim(rs.Fields("仕入金額").Value & "")))
			
			'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			fpSpd.SetText(Col売上税区分, i, Trim(rs.Fields("売上税区分").Value & ""))
			'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			fpSpd.SetText(Col消費税額, i, IIf(Trim(rs.Fields("消費税額").Value & "") = "0", "", Trim(rs.Fields("消費税額").Value & "")))
			
			'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			fpSpd.SetText(Col明細備考, i, Trim(rs.Fields("明細備考").Value & "")) '2018/05/03 ADD
			'        fpSpd.SetText Col入出庫日, i, Trim$(rs.Fields("入出庫日").Value & "") '2018/05/03 ADD
			'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			fpSpd.SetText(Col入庫日, i, VB6.Format(Trim(rs.Fields("入庫日").Value & ""), "yy/mm/dd")) '2018/05/03 ADD
			'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			fpSpd.SetText(Col出庫日, i, VB6.Format(Trim(rs.Fields("出庫日").Value & ""), "yy/mm/dd")) '2020/09/16 ADD
			
			'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			fpSpd.SetText(Col追番R, i, Trim(rs.Fields("追番R").Value & "")) '2019/12/12 ADD
			
			Select Case Trim(rs.Fields("見積区分").Value & "")
				Case "A", "C", "S"
					'2017/03/10 ADD↓
					'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					fpSpd.SetText(Col仕入業者CD, i, "")
					'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					fpSpd.SetText(Col仕入業者名, i, "")
					'2017/03/10 ADD↑
					'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					fpSpd.SetText(Col仕入先CD, i, "")
					'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					fpSpd.SetText(Col仕入先名, i, "")
					'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					fpSpd.SetText(Col送り先CD, i, "")
					'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					fpSpd.SetText(Col送り先名, i, "")
				Case Else
					'2017/03/10 ADD↓
					'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					fpSpd.SetText(Col仕入業者CD, i, Trim(rs.Fields("仕入業者CD").Value & ""))
					'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					fpSpd.SetText(Col仕入業者名, i, Trim(rs.Fields("仕入業者名").Value & ""))
					'2017/03/10 ADD↑
					'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					fpSpd.SetText(Col仕入先CD, i, Trim(rs.Fields("仕入先CD").Value & ""))
					'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					fpSpd.SetText(Col仕入先名, i, Trim(rs.Fields("仕入先名").Value & ""))
					'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					fpSpd.SetText(Col送り先CD, i, Trim(rs.Fields("配送先CD").Value & ""))
					'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					fpSpd.SetText(Col送り先名, i, Trim(rs.Fields("配送先名").Value & ""))
			End Select
			'2016/06/22 ADD↓
			'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			fpSpd.SetText(Col作業区分CD, i, VB6.Format(Trim(rs.Fields("作業区分CD").Value & ""), "#"))
			''        fpSpd.SetText Col作業区分名, i, Trim$(rs.Fields("作業区分名").Value & "")
			'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			fpSpd.SetText(Col作業区分名, i, ModKubuns.Get作業区分名(VB6.Format(Trim(rs.Fields("作業区分CD").Value & ""), "#")))
			'2016/06/22 ADD↑
			
			
			'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			fpSpd.SetText(Col社内在庫, i, IIf(Trim(rs.Fields("社内在庫数").Value & "") = "0", "", Trim(rs.Fields("社内在庫数").Value & "")))
			'        fpSpd.SetText Col社内在庫参照, i, IIf(Trim$(rs.Fields("社内在庫数").Value & "") = "0", "", Trim$(rs.Fields("社内在庫数").Value & ""))
			
			'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			fpSpd.SetText(Col客先在庫, i, IIf(Trim(rs.Fields("客先在庫数").Value & "") = "0", "", Trim(rs.Fields("客先在庫数").Value & "")))
			'        fpSpd.SetText Col客先在庫参照, i, IIf(Trim$(rs.Fields("客先在庫数").Value & "") = "0", "", Trim$(rs.Fields("客先在庫数").Value & ""))
			'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			fpSpd.SetText(Col転用, i, IIf(Trim(rs.Fields("転用数").Value & "") = "0", "", Trim(rs.Fields("転用数").Value & "")))
			
			'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			fpSpd.SetText(Col発注調整数, i, IIf(Trim(rs.Fields("発注調整数").Value & "") = "0", "", Trim(rs.Fields("発注調整数").Value & "")))
			'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			fpSpd.SetText(Col発注数, i, IIf(Trim(rs.Fields("発注数").Value & "") = "0", "", Trim(rs.Fields("発注数").Value & "")))
			'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			fpSpd.SetText(Col数量合計, i, IIf(Trim(rs.Fields("総数量").Value & "") = "0", "", Trim(rs.Fields("総数量").Value & "")))
			
			
			'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			fpSpd.SetText(Col製品区分, i, Trim(rs.Fields("製品区分").Value & ""))
			'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			fpSpd.SetText(Col見積明細連番, i, Trim(rs.Fields("見積明細連番").Value & ""))
			'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			fpSpd.SetText(Col仕入済数, i, Trim(rs.Fields("仕入済数").Value & ""))
			'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			fpSpd.SetText(Col仕入済CNT, i, Trim(rs.Fields("仕入済CNT").Value & "")) '2018/06/19 ADD
			
			'        If Trim$(rs.Fields("仕入済数").Value & "") <> "0" Then
			If Trim(rs.Fields("仕入済CNT").Value & "") <> "0" Then '2018/06/19 ADD
				'仕入発生してたらロックして変更不可にする
				With fpSpd
					'UPGRADE_WARNING: オブジェクト fpSpd.BlockMode の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.BlockMode = True
					'ロック
					'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.Col = Col展開
					'UPGRADE_WARNING: オブジェクト fpSpd.Col2 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.Col2 = Col展開
					'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.Row = i
					'UPGRADE_WARNING: オブジェクト fpSpd.Row2 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.Row2 = i
					'UPGRADE_WARNING: オブジェクト fpSpd.Lock の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.Lock = True
					
					'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.Col = ColSP区分
					'UPGRADE_WARNING: オブジェクト fpSpd.Col2 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.Col2 = Col定価
					'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.Row = i
					'UPGRADE_WARNING: オブジェクト fpSpd.Row2 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.Row2 = i
					'UPGRADE_WARNING: オブジェクト fpSpd.Lock の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.Lock = True
					
					'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.Col = Col原価
					'UPGRADE_WARNING: オブジェクト fpSpd.Col2 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.Col2 = Col仕入率
					'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.Row = i
					'UPGRADE_WARNING: オブジェクト fpSpd.Row2 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.Row2 = i
					'UPGRADE_WARNING: オブジェクト fpSpd.Lock の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.Lock = True
					
					'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.Col = Col仕入業者CD '2017/03/10 ADD
					'.Col = Col仕入先CD
					'UPGRADE_WARNING: オブジェクト fpSpd.Col2 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.Col2 = Col送り先CD
					'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.Row = i
					'UPGRADE_WARNING: オブジェクト fpSpd.Row2 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.Row2 = i
					'UPGRADE_WARNING: オブジェクト fpSpd.Lock の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.Lock = True
					
					'2018/05/30 ADD↓
					'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.Col = Col入庫日
					'UPGRADE_WARNING: オブジェクト fpSpd.Col2 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.Col2 = Col入庫日
					'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.Row = i
					'UPGRADE_WARNING: オブジェクト fpSpd.Row2 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.Row2 = i
					'UPGRADE_WARNING: オブジェクト fpSpd.Lock の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.Lock = True
					'2018/05/30 ADD↑
					
					'2020/09/16 ADD↓
					'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.Col = Col出庫日
					'UPGRADE_WARNING: オブジェクト fpSpd.Col2 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.Col2 = Col出庫日
					'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.Row = i
					'UPGRADE_WARNING: オブジェクト fpSpd.Row2 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.Row2 = i
					'UPGRADE_WARNING: オブジェクト fpSpd.Lock の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.Lock = True
					'2020/09/16 ADD↑
					
					'明細備考はロック解除 2022/08/08 ADD
					'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.Col = Col明細備考
					'UPGRADE_WARNING: オブジェクト fpSpd.Col2 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.Col2 = Col明細備考
					'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.Row = i
					'UPGRADE_WARNING: オブジェクト fpSpd.Row2 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.Row2 = i
					'UPGRADE_WARNING: オブジェクト fpSpd.Lock の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.Lock = False
					
					'UPGRADE_WARNING: オブジェクト fpSpd.BlockMode の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.BlockMode = False
				End With
				shiirezumiF = True
			End If
			'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			fpSpd.SetText(Col売上済CNT, i, Trim(rs.Fields("売上済CNT").Value & ""))
			If Trim(rs.Fields("売上済CNT").Value & "") <> "0" Then
				'仕入発生してたらロックして変更不可にする
				With fpSpd
					'UPGRADE_WARNING: オブジェクト fpSpd.BlockMode の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.BlockMode = True
					'全ての明細項目をロック
					'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.Col = Col展開
					'UPGRADE_WARNING: オブジェクト fpSpd.Col2 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.Col2 = Col売上済CNT
					'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.Row = i
					'UPGRADE_WARNING: オブジェクト fpSpd.Row2 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.Row2 = i
					'UPGRADE_WARNING: オブジェクト fpSpd.Lock の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.Lock = True
					
					'明細備考はロック解除 2022/08/08 ADD
					'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.Col = Col明細備考
					'UPGRADE_WARNING: オブジェクト fpSpd.Col2 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.Col2 = Col明細備考
					'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.Row = i
					'UPGRADE_WARNING: オブジェクト fpSpd.Row2 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.Row2 = i
					'UPGRADE_WARNING: オブジェクト fpSpd.Lock の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.Lock = False
					
					''''                    '2016/10/26 ADD↓
					''''                    .Col = Col他社納品日付
					''''                    .Col2 = Col他社伝票番号
					''''                    .Row = i
					''''                    .Row2 = i
					''''                    .Lock = False
					''''                    '2016/10/26 ADD↑
					
					'UPGRADE_WARNING: オブジェクト fpSpd.BlockMode の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.BlockMode = False
				End With
				shiirezumiF = True
			End If
			
			'''        If clsSPD.GetTextEX(ColU区分, CLng(i)) = "U" Then '2015/03/04 ADD
			'''            Call RowBackColorSet(1, CLng(i))
			'''        End If
			'2017/07/03 ADD
			Call RowBackColorSet2(clsSPD_Renamed.GetTextEX(ColU区分, CInt(i)), CInt(i))
			
			
			''        '金額再計算
			''        ret = fpSpd.GetText(Col仕入数量, i, buf)
			''        If buf <> 0 Then
			''            Call Get金額(i)
			''        End If
			
			'2023/09/26 ADD↓
			With fpSpd
				'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.Col = 1
				'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.Row = i
				'UPGRADE_WARNING: オブジェクト fpSpd.Col2 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト fpSpd.MaxCols の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.Col2 = .MaxCols
				'UPGRADE_WARNING: オブジェクト fpSpd.Row2 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.Row2 = i
				'UPGRADE_WARNING: オブジェクト fpSpd.BlockMode の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.BlockMode = True
				'UPGRADE_WARNING: オブジェクト fpSpd.ForeColor の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.ForeColor = Getインボイス番号NGColor(Trim(rs.Fields("仕入先CD").Value & ""))
				'UPGRADE_WARNING: オブジェクト fpSpd.BlockMode の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.BlockMode = False
			End With
			'2023/09/26 ADD↑
			
			rs.MoveNext()
			i = i + 1
		Loop 
		'UPGRADE_WARNING: オブジェクト fpSpd.ReDraw の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		fpSpd.ReDraw = True
		
		'''    wMaxRow = i
		wMaxRow = i - 1 '2016/08/01 ADD  最後に＋１しているので引いておく
		
		'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		fpSpd.Col = Col展開
		'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		fpSpd.Row = 1
		'UPGRADE_WARNING: オブジェクト fpSpd.Action の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		fpSpd.Action = FPSpreadADO.ActionConstants.ActionActiveCell
		
	End Sub
	
	
	Private Function DspSelData() As Boolean
		Dim fpSpd As Object
		'製品情報選択から選択したデータを表示
		Dim wRow As Short
		Dim i As Short
		
		DspSelData = False
		
		'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		wRow = fpSpd.ActiveRow
		
		'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
		If IsDbNull(SelData) Then
			Exit Function
		Else
			With fpSpd
				'UPGRADE_WARNING: オブジェクト fpSpd.ReDraw の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.ReDraw = False
				For i = LBound(SelData, 1) To UBound(SelData, 1)
					'UPGRADE_WARNING: オブジェクト fpSpd.MaxRows の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					If wRow + i > .MaxRows Then
						Exit For
					End If
					
					'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.SetText(ColPC区分, wRow + i, SelData(i, 0))
					'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.SetText(Col製品NO, wRow + i, SelData(i, 1))
					'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.SetText(Col仕様NO, wRow + i, SelData(i, 2))
					'UPGRADE_WARNING: オブジェクト SelData() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					Call Entry_Chk(0, wRow + i, CStr(SelData(i, 0)), CStr(SelData(i, 1)), CStr(SelData(i, 2)))
					'原価・売価
					Call Get率(wRow + i, Col原価)
					Call Get率(wRow + i, Col売価)
					Call Get金額(wRow + i)
				Next 
				'UPGRADE_WARNING: オブジェクト fpSpd.ReDraw の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.ReDraw = True
			End With
		End If
		Call Calc合計()
		
		'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト SelData の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		SelData = System.DBNull.Value
		DspSelData = True
	End Function
	
	Private Function DspKokData(ByRef wRow As Integer) As Boolean
		Dim fpSpd As Object
		'顧客テンプレート選択から選択したデータを表示
		Dim i As Short
		
		DspKokData = False
		
		'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
		If IsDbNull(SelData) Then
			Exit Function
		Else
			With fpSpd
				'UPGRADE_WARNING: オブジェクト fpSpd.AutoCalc の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.AutoCalc = False
				'UPGRADE_WARNING: オブジェクト fpSpd.ReDraw の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.ReDraw = False
				For i = LBound(SelData, 1) To UBound(SelData, 1)
					'UPGRADE_WARNING: オブジェクト fpSpd.MaxRows の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					If wRow + i > fpSpd.MaxRows Then
						Exit For
					End If
					'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.SetText(ColPC区分, wRow + i, SelData(i, 0)) 'PC区分
					'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.SetText(Col製品NO, wRow + i, SelData(i, 1)) '製品NO
					'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.SetText(Col仕様NO, wRow + i, SelData(i, 2)) '仕様NO
					'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.SetText(Colベース色, wRow + i, SelData(i, 3)) 'ベース色
					'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.SetText(Col名称, wRow + i, SelData(i, 4)) '名称
					'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.SetText(ColW, wRow + i, SelData(i, 5)) 'W
					'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.SetText(ColD, wRow + i, SelData(i, 6)) 'D
					'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.SetText(ColH, wRow + i, SelData(i, 7)) 'H
					'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.SetText(ColD1, wRow + i, SelData(i, 8)) 'D1
					'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.SetText(ColD2, wRow + i, SelData(i, 9)) 'D2
					'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.SetText(ColH1, wRow + i, SelData(i, 10)) 'H1
					'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.SetText(ColH2, wRow + i, SelData(i, 11)) 'H2
					'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.SetText(Col単位, wRow + i, SelData(i, 12)) '単位
					'2017/03/10 ADD↓
					'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.SetText(Col仕入業者CD, wRow + i, SelData(i, 13)) '仕入先
					'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.SetText(Col仕入業者名, wRow + i, Get仕入先略称(SelData(i, 13)))
					'2017/03/10 ADD↑
					'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.SetText(Col仕入先CD, wRow + i, SelData(i, 13)) '仕入先
					'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.SetText(Col仕入先名, wRow + i, Get仕入先略称(SelData(i, 13)))
					'各マスタより項目セット
					'UPGRADE_WARNING: オブジェクト SelData() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					Call Entry_Chk(1, wRow + i, CStr(SelData(i, 0)), CStr(SelData(i, 1)), CStr(SelData(i, 2)))
					'原価・売価
					Call Get率(wRow + i, Col原価)
					Call Get率(wRow + i, Col売価)
					Call Get金額(wRow + i)
				Next 
				'UPGRADE_WARNING: オブジェクト fpSpd.AutoCalc の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.AutoCalc = True
				'UPGRADE_WARNING: オブジェクト fpSpd.ReDraw の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.ReDraw = True
			End With
		End If
		Call Calc合計()
		
		'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト SelData の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		SelData = System.DBNull.Value
		DspKokData = True
	End Function
	
	Public Sub DspGenka(ByRef Ritu As Decimal)
		Dim fpSpd As Object
		'価格設定より呼び出されるモジュール（原価計算）
		Dim check As Boolean
		Dim getdata As Object
		Dim i As Integer
		Dim wBaika As Decimal
		Dim wGenka As Decimal
		Dim wUriKei As Decimal
		Dim wGenKei As Decimal
		
		With fpSpd
			'UPGRADE_WARNING: オブジェクト fpSpd.ReDraw の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.ReDraw = False
			'UPGRADE_WARNING: オブジェクト fpSpd.AutoCalc の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.AutoCalc = False
			'UPGRADE_WARNING: オブジェクト fpSpd.DataRowCnt の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			For i = 1 To .DataRowCnt
				'UPGRADE_WARNING: オブジェクト fpSpd.GetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				check = .GetText(Col定価, i, getdata) '定価
				If check Then
					'UPGRADE_WARNING: オブジェクト getdata の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					wGenka = (getdata * Ritu) / 100
					'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.SetText(Col原価, i, wGenka)
					'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.SetText(Col仕入率, i, VB6.Format(Ritu, "#.##"))
				Else
					wBaika = 0
				End If
				
				Call Get金額(i)
			Next 
			'UPGRADE_WARNING: オブジェクト fpSpd.AutoCalc の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.AutoCalc = True
			'UPGRADE_WARNING: オブジェクト fpSpd.ReDraw の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.ReDraw = True
		End With
		
		Call Calc合計()
	End Sub
	
	Public Sub DspBaika(ByRef Ritu As Decimal)
		Dim fpSpd As Object
		'価格設定より呼び出されるモジュール(売価計算）
		Dim check As Boolean
		Dim getdata As Object
		Dim i As Integer
		Dim wBaika As Decimal
		Dim wGenka As Decimal
		Dim wUriKei As Decimal
		Dim wGenKei As Decimal
		
		With fpSpd
			'UPGRADE_WARNING: オブジェクト fpSpd.ReDraw の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.ReDraw = False
			'UPGRADE_WARNING: オブジェクト fpSpd.AutoCalc の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.AutoCalc = False
			'UPGRADE_WARNING: オブジェクト fpSpd.DataRowCnt の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			For i = 1 To .DataRowCnt
				'UPGRADE_WARNING: オブジェクト fpSpd.GetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				check = .GetText(Col定価, i, getdata) '定価
				If check Then
					'UPGRADE_WARNING: オブジェクト getdata の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					wBaika = (getdata * Ritu) / 100
					'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.SetText(Col売価, i, wBaika)
					'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.SetText(Col売上率, i, VB6.Format(Ritu, "#.##"))
				Else
					wBaika = 0
				End If
				
				Call Get金額(i)
			Next 
			'UPGRADE_WARNING: オブジェクト fpSpd.AutoCalc の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.AutoCalc = True
			'UPGRADE_WARNING: オブジェクト fpSpd.ReDraw の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.ReDraw = True
			
		End With
		
		Call Calc合計()
	End Sub
	
	Public Sub Calc合計()
		Dim fpSpd As Object
		Dim check As Boolean
		Dim getdata As Object
		Dim i As Short
		Dim wSuryo As Decimal
		Dim wBaika As Decimal
		Dim wGenka As Decimal
		Dim wUriKin As Decimal
		Dim wGenKin As Decimal
		Dim wUriKei As Decimal
		Dim wGenKei As Decimal
		Dim wKyakuSu As Decimal
		Dim wTenyo As Decimal
		Dim wGokeiSu As Decimal
		Dim wUZeiKBN As Short '売上税区分
		Dim KAZEITotal As Decimal '課税対象額(TAX計算用)
		
		Dim wMituKbn As Object '2010/01/05 ADD
		
		Dim ZEI As Decimal
		Dim wZeiKb As Short
		
		On Error GoTo Calc合計_err '2020/03/31 ADD
		''    ZEI = Get税率(idc(1).Text)
		ZEI = Get税率(CDate(tx_見積日付.Text))
		'On Error GoTo 0
		
		'---------------------------
		'DEBUG
		If ZEI = 0 Then
			MsgBox("税率ZERO!!")
		End If
		'---------------------------
		
		With fpSpd
			'UPGRADE_WARNING: オブジェクト fpSpd.ReDraw の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.ReDraw = False
			'UPGRADE_WARNING: オブジェクト fpSpd.AutoCalc の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.AutoCalc = False
			'UPGRADE_WARNING: オブジェクト fpSpd.MaxRows の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			For i = 1 To .MaxRows
				'UPGRADE_WARNING: オブジェクト fpSpd.GetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				check = .GetText(Col見積数量, i, getdata) '見積数量
				If check Then
					'UPGRADE_WARNING: オブジェクト getdata の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					wGokeiSu = getdata
				Else
					wGokeiSu = 0
				End If
				''''''            check = .GetText(SiwakeCol - 4, i, getdata)      '客先在庫数
				''            check = .GetText(Col客先在庫, i, getdata)      '客先在庫数
				''            If check Then
				''                wKyakuSu = getdata
				''            Else
				''                wKyakuSu = 0
				''            End If
				''''''            check = .GetText(SiwakeCol - 3, i, getdata)      '転用数
				''            check = .GetText(Col転用, i, getdata)      '転用数
				''            If check Then
				''                wTenyo = getdata
				''            Else
				''                wTenyo = 0
				''            End If
				
				'合計数量が見積数量より小さい場合(転用が多い場合)計算しない
				'値引き・返品は計算する
				'''''''''            If wKyakuSu + wTenyo > 0 And wGokeiSu < 0 Then
				'''''''''            Else
				'2010/01/05 ADD↓
				'UPGRADE_WARNING: オブジェクト fpSpd.GetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				check = .GetText(Col見積区分, i, getdata) '見積区分
				If check Then
					'UPGRADE_WARNING: オブジェクト getdata の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					'UPGRADE_WARNING: オブジェクト wMituKbn の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					wMituKbn = getdata
				Else
					'UPGRADE_WARNING: オブジェクト wMituKbn の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					wMituKbn = ""
				End If
				'2010/01/05 ADD↑
				'''            If wMituKbn = "" Then   '2010/01/05 ADD
				
				'            If InStr("ACS", wMituKbn) = 0 Then  '2010/01/05 ADD
				Select Case wMituKbn '2010/01/06 ADD
					Case "A", "C", "S" 'ｺﾒﾝﾄは除外する
					Case Else
						'UPGRADE_WARNING: オブジェクト fpSpd.GetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						check = .GetText(Col金額, i, getdata) '売上金額
						If check Then
							'UPGRADE_WARNING: オブジェクト getdata の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							wUriKin = getdata
						Else
							wUriKin = 0
						End If
						'UPGRADE_WARNING: オブジェクト fpSpd.GetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						check = .GetText(Col仕入金額, i, getdata) '仕入金額
						If check Then
							'UPGRADE_WARNING: オブジェクト getdata の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							wGenKin = getdata
						Else
							wGenKin = 0
						End If
						'UPGRADE_WARNING: オブジェクト fpSpd.GetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						check = .GetText(Col売上税区分, i, getdata) '売上税区分
						If check Then
							'UPGRADE_WARNING: オブジェクト getdata の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							wUZeiKBN = getdata
						Else
							wUZeiKBN = 0
						End If
						
						wUriKei = wUriKei + wUriKin
						wGenKei = wGenKei + wGenKin
						
						If wUZeiKBN = 0 Then '外税のみ集計
							KAZEITotal = KAZEITotal + wUriKin
						End If
				End Select '2010/01/06 ADD
				'''''''''            End If
				'            End If  '2010/01/05 ADD
			Next 
			'UPGRADE_WARNING: オブジェクト fpSpd.AutoCalc の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.AutoCalc = True
			'UPGRADE_WARNING: オブジェクト fpSpd.ReDraw の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.ReDraw = True
		End With
		
		tx_合計金額.Text = CStr(wUriKei)
		tx_原価合計.Text = CStr(wGenKei)
		If wUriKei <> 0 Then
			rf_原価率.Text = VB6.Format((wGenKei / wUriKei) * 100, "0.00")
		Else
			rf_原価率.Text = VB6.Format(0, "0.00")
		End If
		
		rf_原価合計.Text = VB6.Format(wGenKei, "#,##0" & KIN_FMT) '2018/11/01 ADD
		'    rf_合計金額.Caption = Format$(wUriKei, "#,##0") & " (円)"
		rf_合計金額.Text = VB6.Format(wUriKei, "#,##0" & KIN_FMT) '2014/07/10 ADD
		
		'消費税セット
		Select Case HD_税集計区分
			Case 0 '伝票単位
				tx_外税額.Text = CStr(ISHasuu_rtn(HD_消費税端数, (KAZEITotal + HD_出精値引) / 100 * ZEI, KIN_HASU)) '2014/07/10 ADD
			Case 1 '請求単位
				tx_外税額.Text = CStr(0)
				'2016/04/07 ADD↓
			Case Else '税対象外（免税）
				tx_外税額.Text = CStr(0)
				'2016/04/07 ADD↑
		End Select
		
		'2015/08/28 ADD
		[rf_税込金額].Text = VB6.Format(CDec(rf_合計金額.Text) + CDec(HD_出精値引) + CDec(tx_外税額.Text), "#,##0" & KIN_FMT)
		
		Exit Sub
Calc合計_err: '---エラー時
		MsgBox(Err.Number & " " & Err.Description)
		
	End Sub
	''''''
	''''''Private Sub DspTokTemp(TOKUCD As String, TEMPNM As String)
	''''''    Dim rs As adodb.Recordset, SQL As String
	''''''
	''''''    On Error GoTo DspTokTemp_Err
	''''''    'マウスポインターを砂時計にする
	''''''    HourGlass True
	''''''
	''''''    '---顧客テンプレートＭ存在ﾁｪｯｸ
	''''''    SQL = "SELECT KT.行NO, KT.PC区分, KT.製品NO, KT.仕様NO, " _
	'''''''            & "名称=(COALESCE(SE.漢字名称,HI.品群名称,UN.ユニット名,PC.漢字名称)), " _
	'''''''            & "SE.ベース色, " _
	'''''''            & "KT.W, KT.D, KT.H, KT.D1, KT.D2, " _
	'''''''            & "単位=(COALESCE(SE.単位名,PC.単位名)) " _
	'''''''        & "FROM TM顧客テンプレート AS KT " _
	'''''''            & "LEFT JOIN TM製品 AS SE " _
	'''''''                & "ON KT.製品区分 = 0 AND KT.製品NO = SE.製品NO AND KT.仕様NO = SE.仕様NO " _
	'''''''            & "LEFT JOIN TM品群 AS HI " _
	'''''''                & "ON KT.製品区分 = 1 AND KT.製品NO = HI.品群NO " _
	'''''''            & "LEFT JOIN TMユニット AS UN " _
	'''''''                & "ON KT.製品区分 = 2 AND KT.製品NO = UN.ユニットNO " _
	'''''''            & "LEFT JOIN TMPC AS PC " _
	'''''''                & "ON KT.製品区分 = 3 AND KT.PC区分 = PC.PC区分 AND KT.製品NO = PC.製品NO " _
	'''''''        & "WHERE 得意先CD = '" & SQLString(Trim$(TOKUCD)) & "'" _
	'''''''            & "AND テンプレート名 = '" & SQLString(Trim$(TEMPNM)) & "'" _
	'''''''        & "ORDER BY 得意先CD, テンプレート名, 行NO"
	''''''
	''''''    Set rs = OpenRs(SQL, Cn, adOpenForwardOnly, adLockReadOnly)
	''''''
	''''''    If Not rs.EOF Then
	''''''        Call SetTokTemp(rs)
	''''''    End If
	''''''    Call ReleaseRs(rs)
	''''''
	''''''    HourGlass False
	''''''    Exit Sub
	''''''DspTokTemp_Err:
	''''''    MsgBox Err.Number & " " & Err.Description
	''''''    HourGlass False
	''''''End Sub
	''''''
	''''''Private Sub SetTokTemp(rs As adodb.Recordset)
	''''''    Dim RecArry() As Variant
	''''''    Dim i As Integer, j As Integer
	''''''
	''''''    'シートのクリア
	''''''    Call clsSPD.sprClearText
	''''''
	''''''    RecArry = rs.GetRows(fpSpd.MaxRows, , _
	'''''''                Array("PC区分", "製品NO", "仕様NO", "ベース色", "名称", "W", "D", "H", "D1", "D2", _
	'''''''                        "単位"))
	''''''
	''''''    For i = 0 To UBound(RecArry, 2)
	''''''        For j = 0 To UBound(RecArry)
	''''''            Select Case j
	''''''                Case 5 To 9
	''''''                    fpSpd.SetText j + 3, i + 1, IIf(Trim$("" & RecArry(j, i)) = "0", "", Trim$("" & RecArry(j, i)))
	''''''                Case 10
	''''''                    fpSpd.SetText 21, i + 1, Trim$("" & RecArry(j, i))
	''''''                Case Else
	''''''                    fpSpd.SetText j + 3, i + 1, Trim$("" & RecArry(j, i))
	''''''            End Select
	''''''        Next
	''''''    Next
	''''''
	''''''    fpSpd.Col = 1
	''''''    fpSpd.Row = 1
	''''''    fpSpd.Action = ActionActiveCell
	''''''End Sub
	
	Private Function SiwakeDL(ByRef MituNo As Integer) As Short
		Dim rs As ADODB.Recordset
		Dim sql As String
		Dim wMeisai As Object
		Dim wLine As Short
		Dim i As Short
		
		On Error GoTo Download_Err
		'マウスポインターを砂時計にする
		HourGlass(True)
		
		SiwakeDL = 0
		
		'---見積明細内訳名称セット
		sql = "SELECT * FROM TD見積シート内訳名称 " & "WHERE 見積番号 = " & MituNo
		
		rs = OpenRs(sql, Cn, ADODB.CursorTypeEnum.adOpenForwardOnly, ADODB.LockTypeEnum.adLockReadOnly)
		
		With rs
			If .EOF Then
				If IsArray(gSiwakeTBL) Then
					Erase gSiwakeTBL
				End If
				'UPGRADE_WARNING: オブジェクト gSiwakeTBL の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				gSiwakeTBL = Nothing
			Else
				'''            gSiwakeTBL = .GetRows(, , Array("名称", "略称", "納期", "時間"))
				'UPGRADE_WARNING: Array に新しい動作が指定されています。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト rs.GetRows() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト gSiwakeTBL の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				gSiwakeTBL = .GetRows( ,  , New Object(){"名称", "略称", "納期", "時間", "他社部門CD"}) '2016/03/07 ADD
			End If
		End With
		
		Call ReleaseRs(rs)
		
		HourGlass(False)
		Exit Function
Download_Err: 
		Call ReleaseRs(rs)
		MsgBox(Err.Number & " " & Err.Description)
		HourGlass(False)
	End Function
	
	Private Function GetSerchMax() As Short
		Dim fpSpd As Object
		Dim i As Short
		
		GetSerchMax = 0
		
		'UPGRADE_WARNING: オブジェクト fpSpd.DataRowCnt の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		If fpSpd.DataRowCnt = 0 Then
			CriticalAlarm("明細がありません。")
			'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			fpSpd.Col = fpSpd.ActiveRow
			'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			fpSpd.Row = fpSpd.ActiveRow
			'UPGRADE_WARNING: オブジェクト fpSpd.Action の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			fpSpd.Action = FPSpreadADO.ActionConstants.ActionActiveCell
			'UPGRADE_WARNING: オブジェクト fpSpd.SetFocus の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			fpSpd.SetFocus()
			Exit Function
		End If
		
		'UPGRADE_WARNING: 配列 WkTBLS の下限が 1,1 から 0,0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="0F1C9BE1-AF9D-476E-83B1-17D43BECFF20"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト fpSpd.MaxCols の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト fpSpd.DataRowCnt の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		ReDim WkTBLS(fpSpd.DataRowCnt, fpSpd.MaxCols)
		
		'UPGRADE_WARNING: オブジェクト fpSpd.GetArray の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Call fpSpd.GetArray(1, 1, WkTBLS)
		
		'未入力最終明細サーチ
		For i = UBound(WkTBLS, 1) To LBound(WkTBLS, 1) Step -1
			If IsCheckNull(WkTBLS(i, ColPC区分)) = False Then Exit For 'PC区分
			If IsCheckNull(WkTBLS(i, Col製品NO)) = False Then Exit For '製品NO
			If IsCheckNull(WkTBLS(i, Col仕様NO)) = False Then Exit For '仕様NO
			If IsCheckNull(WkTBLS(i, Col名称)) = False Then Exit For '名称
			''''        If IsCheckNull(WkTBLS(wMaxRow, 24)) = False Then Exit For               '仕入先CD
			''''        If IsCheckNull(WkTBLS(wMaxRow, 25)) = False Then Exit For               '配送先CD
		Next 
		
		GetSerchMax = i
		
	End Function
	'
	'Private Function Tenkai_Chk() As Integer
	'    Dim rs As adodb.Recordset
	'
	'    Tenkai_Chk = -1
	'
	'    If fpSpd.DataRowCnt = 0 Then
	'        CriticalAlarm "明細がありません。"
	'        fpSpd.Col = fpSpd.ActiveRow
	'        fpSpd.Row = fpSpd.ActiveRow
	'        fpSpd.Action = ActionActiveCell
	'        fpSpd.SetFocus
	'        Exit Function
	'    End If
	'
	'    'マウスポインターを砂時計にする
	'    HourGlass True
	'    On Error GoTo Tenkai_Chk_err
	'
	'    'Tmpテーブル作成
	'''''    TMPTABLE = OutputTmp
	'''    OutputTmp
	'    CreateTmp見積明細   '2013/03/13 ADD
	'    'Tmpテーブル書き込み
	'    If InsDataTmp = False Then
	'        GoTo Tenkai_Chk_Correct
	'''        Exit Function
	'    End If
	'
	'    '--見積明細
	'    Dim cmd As New adodb.Command
	'    Dim grs As adodb.Recordset
	'
	'    Cn.CommandTimeout = 0
	'    cmd.CommandTimeout = 0
	'    cmd.ActiveConnection = Cn
	'    cmd.CommandText = "usp_MT0100員数展開" '2013/03/13 ADD
	'    cmd.CommandType = adCmdStoredProc
	'    cmd.Parameters.Refresh
	'
	'    ' それぞれのパラメータの値を指定する
	'    With cmd.Parameters
	'        .Item(0).Direction = adParamReturnValue
	'        .Item("@i見積日付").Value = Format$(tx_見積日付, "yyyy/mm/dd")
	'        .Item("@i得意先CD").Value = rf_得意先CD
	'        .Item("@i売上端数").Value = tx_売上端数
	'        .Item("@i消費税端数").Value = tx_消費税端数
	'        .Item("@i受注区分").Value = tx_受注区分
	'        .Item("@i大小口区分").Value = tx_大小口区分
	'        .Item("@CompName").Value = GetPCName
	'    End With
	'    '---コマンド実行
	'    Set grs = cmd.Execute
	'    Dim a As Object
	'    For Each a In Cn.Errors
	'        Debug.Print "err:" & a
	'    Next
	'    If grs.State <> 0 Then
	'        If grs.EOF Then
	'            Tenkai_Chk = -1
	'            Inform ("該当データ無し")
	'            GoTo Tenkai_Chk_Correct
	'        Else
	'            Set rs = grs
	''''            Tenkai_Chk = NullToZero(rs.Fields("ErrCnt").Value, 0)
	'        End If
	'    Else
	'        Tenkai_Chk = -1
	'        CriticalAlarm (cmd("@RetST") & " : " & cmd("@RetMsg"))
	'        GoTo Tenkai_Chk_Correct
	'    End If
	'
	'    Set cmd = Nothing
	'
	'    'チェック後データ再表示
	'    Call SetCheckTempD(rs)
	'
	'    HourGlass False
	'
	'Tenkai_Chk_Correct:
	'    On Error GoTo 0
	'
	'    '作業テーブル削除
	'    DropTmp見積明細 '2013/03/13 ADD
	'
	'    HourGlass False
	'    Exit Function
	'
	'Tenkai_Chk_err:  '---エラー時
	'    MsgBox Err.Number & " " & Err.Description
	'
	'    Resume Tenkai_Chk_Correct
	'End Function
	
	'選択したコードを送るコントロールをセット
	WriteOnly Property ResParentForm() As System.Windows.Forms.Form
		Set(ByVal Value As System.Windows.Forms.Form)
			pParentForm = Value
		End Set
	End Property
	
	'表示項目をセット
	WriteOnly Property MituNo() As Integer
		Set(ByVal Value As Integer)
			pMituNo = Value
		End Set
	End Property
	
	Private Function Get仕入先略称(ByRef ID As Object) As String
		Dim rs As ADODB.Recordset
		Dim sql As String
		On Error GoTo Get仕入先略称_Err
		
		'マウスポインターを砂時計にする
		HourGlass(True)
		
		'UPGRADE_WARNING: オブジェクト ID の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
		If IsDbNull(ID) Or Trim(ID) = vbNullString Then
			HourGlass(False)
			Get仕入先略称 = vbNullString
			Exit Function
		End If
		
		'UPGRADE_WARNING: オブジェクト ID の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		sql = "SELECT 略称 FROM TM仕入先 " & "WHERE 仕入先CD = '" & SQLString(Trim(ID)) & "'"
		rs = OpenRs(sql, Cn, ADODB.CursorTypeEnum.adOpenForwardOnly, ADODB.LockTypeEnum.adLockReadOnly)
		
		With rs
			If .EOF Then
				Get仕入先略称 = vbNullString
			Else
				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				Get仕入先略称 = NullToZero(.Fields("略称"), vbNullString)
			End If
		End With
		ReleaseRs(rs)
		
		HourGlass(False)
		Exit Function
Get仕入先略称_Err: 
		MsgBox(Err.Number & " " & Err.Description)
		HourGlass(False)
	End Function
	
	Private Function Get配送先略称(ByRef ID As Object) As String
		Dim rs As ADODB.Recordset
		Dim sql As String
		On Error GoTo Get配送先略称_Err
		
		'マウスポインターを砂時計にする
		HourGlass(True)
		
		'UPGRADE_WARNING: オブジェクト ID の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		'UPGRADE_WARNING: Null/IsNull() の使用が見つかりました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"' をクリックしてください。
		If IsDbNull(ID) Or Trim(ID) = vbNullString Then
			HourGlass(False)
			Get配送先略称 = vbNullString
			Exit Function
		End If
		
		'UPGRADE_WARNING: オブジェクト ID の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		sql = "SELECT 略称 FROM TM配送先 " & "WHERE 配送先CD = '" & SQLString(Trim(ID)) & "'"
		rs = OpenRs(sql, Cn, ADODB.CursorTypeEnum.adOpenForwardOnly, ADODB.LockTypeEnum.adLockReadOnly)
		
		With rs
			If .EOF Then
				Get配送先略称 = vbNullString
			Else
				'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				Get配送先略称 = NullToZero(.Fields("略称"), vbNullString)
			End If
		End With
		ReleaseRs(rs)
		
		HourGlass(False)
		Exit Function
Get配送先略称_Err: 
		MsgBox(Err.Number & " " & Err.Description)
		HourGlass(False)
	End Function
	
	Private Function SpdRowDisp() As Object
		Dim fpSpd As Object
		Dim Disp_Spd As String
		Dim vDisp_Spd As Object
		
		'表示項目設定取得
		'UPGRADE_WARNING: オブジェクト GetIni() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Disp_Spd = GetIni("Disp", "MT02F00_SPD", INIFile)
		
		'UPGRADE_WARNING: オブジェクト vDisp_Spd の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		vDisp_Spd = Split(Disp_Spd, ",")
		
		With fpSpd
			'UPGRADE_WARNING: オブジェクト fpSpd.ReDraw の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.ReDraw = False
			'UPGRADE_WARNING: オブジェクト fpSpd.LeftCol の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.LeftCol = 1
			'SP区分
			'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Col = ColSP区分
			'UPGRADE_WARNING: オブジェクト vDisp_Spd(0) の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			If vDisp_Spd(0) = 0 Then
				'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.ColHidden = True
			Else
				'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.ColHidden = False
			End If
			'PC区分
			'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Col = ColPC区分
			'UPGRADE_WARNING: オブジェクト vDisp_Spd(1) の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			If vDisp_Spd(1) = 0 Then
				'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.ColHidden = True
			Else
				'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.ColHidden = False
			End If
			'ベース色
			'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Col = Colベース色
			'UPGRADE_WARNING: オブジェクト vDisp_Spd(2) の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			If vDisp_Spd(2) = 0 Then
				'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.ColHidden = True
			Else
				'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.ColHidden = False
			End If
			'名称
			'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Col = Col名称
			'UPGRADE_WARNING: オブジェクト vDisp_Spd(3) の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			If vDisp_Spd(3) = 0 Then
				'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.ColHidden = True
			Else
				'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.ColHidden = False
			End If
			'D1～H2
			'UPGRADE_WARNING: オブジェクト vDisp_Spd(4) の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			If vDisp_Spd(4) = 0 Then
				'''            .Col = 9
				'''            .ColHidden = True
				'''            .Col = 10
				'''            .ColHidden = True
				'''            .Col = 11
				'''            .ColHidden = True
				'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.Col = ColD1
				'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.ColHidden = True
				'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.Col = ColD2
				'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.ColHidden = True
				'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.Col = ColH1
				'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.ColHidden = True
				'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.Col = ColH2
				'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.ColHidden = True
			Else
				'''            .Col = 9
				'''            .ColHidden = False
				'''            .Col = 10
				'''            .ColHidden = False
				'''            .Col = 11
				'''            .ColHidden = False
				'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.Col = ColD1
				'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.ColHidden = False
				'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.Col = ColD2
				'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.ColHidden = False
				'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.Col = ColH1
				'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.ColHidden = False
				'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.Col = ColH2
				'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.ColHidden = False
			End If
			'エラー内容
			'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Col = Colエラー内容
			'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.ColHidden = True
			
			'定価
			'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Col = Col定価
			'UPGRADE_WARNING: オブジェクト vDisp_Spd(5) の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			If vDisp_Spd(5) = 0 Then
				'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.ColHidden = True
			Else
				'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.ColHidden = False
			End If
			'U区分                      '---2005/10/14.ADD  U区分追加   ※以下はプラス１
			'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Col = ColU区分
			'UPGRADE_WARNING: オブジェクト vDisp_Spd(6) の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			If vDisp_Spd(6) = 0 Then
				'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.ColHidden = True
			Else
				'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.ColHidden = False
			End If
			'原価
			'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Col = Col原価
			'UPGRADE_WARNING: オブジェクト vDisp_Spd(7) の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			If vDisp_Spd(7) = 0 Then
				'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.ColHidden = True
			Else
				'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.ColHidden = False
			End If
			'仕入率
			'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Col = Col仕入率
			'UPGRADE_WARNING: オブジェクト vDisp_Spd(8) の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			If vDisp_Spd(8) = 0 Then
				'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.ColHidden = True
			Else
				'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.ColHidden = False
			End If
			'売価
			'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Col = Col売価
			'UPGRADE_WARNING: オブジェクト vDisp_Spd(9) の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			If vDisp_Spd(9) = 0 Then
				'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.ColHidden = True
			Else
				'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.ColHidden = False
			End If
			'売上率
			'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Col = Col売上率
			'UPGRADE_WARNING: オブジェクト vDisp_Spd(10) の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			If vDisp_Spd(10) = 0 Then
				'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.ColHidden = True
			Else
				'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.ColHidden = False
			End If
			'Ｍ
			'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Col = ColM
			'UPGRADE_WARNING: オブジェクト vDisp_Spd(11) の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			If vDisp_Spd(11) = 0 Then
				'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.ColHidden = True
			Else
				'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.ColHidden = False
			End If
			'見積数量
			'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Col = Col見積数量
			'UPGRADE_WARNING: オブジェクト vDisp_Spd(12) の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			If vDisp_Spd(12) = 0 Then
				'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.ColHidden = True
			Else
				'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.ColHidden = False
			End If
			'単位
			'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Col = Col単位
			'UPGRADE_WARNING: オブジェクト vDisp_Spd(13) の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			If vDisp_Spd(13) = 0 Then
				'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.ColHidden = True
			Else
				'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.ColHidden = False
			End If
			'金額
			'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Col = Col金額
			'UPGRADE_WARNING: オブジェクト vDisp_Spd(14) の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			If vDisp_Spd(14) = 0 Then
				'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.ColHidden = True
			Else
				'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.ColHidden = False
			End If
			'仕入先CD
			'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Col = Col仕入先CD
			'UPGRADE_WARNING: オブジェクト vDisp_Spd(15) の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			If vDisp_Spd(15) = 0 Then
				'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.ColHidden = True
			Else
				'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.ColHidden = False
			End If
			'仕入先名
			'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Col = Col仕入先名
			'UPGRADE_WARNING: オブジェクト vDisp_Spd(16) の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			If vDisp_Spd(16) = 0 Then
				'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.ColHidden = True
			Else
				'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.ColHidden = False
			End If
			'送り先CD
			'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Col = Col送り先CD
			'UPGRADE_WARNING: オブジェクト vDisp_Spd(17) の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			If vDisp_Spd(17) = 0 Then
				'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.ColHidden = True
			Else
				'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.ColHidden = False
			End If
			'送り先名
			'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Col = Col送り先名
			'UPGRADE_WARNING: オブジェクト vDisp_Spd(18) の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			If vDisp_Spd(18) = 0 Then
				'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.ColHidden = True
			Else
				'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.ColHidden = False
			End If
			'社内在庫
			'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Col = Col社内在庫
			'UPGRADE_WARNING: オブジェクト fpSpd.Col2 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Col2 = Col社内在庫参照 '2009/09/26 ADD
			'UPGRADE_WARNING: オブジェクト fpSpd.BlockMode の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.BlockMode = True
			'UPGRADE_WARNING: オブジェクト vDisp_Spd(19) の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			If vDisp_Spd(19) = 0 Then
				'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.ColHidden = True
			Else
				'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.ColHidden = False
			End If
			'UPGRADE_WARNING: オブジェクト fpSpd.BlockMode の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.BlockMode = False
			'客先在庫
			'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Col = Col客先在庫
			'UPGRADE_WARNING: オブジェクト fpSpd.Col2 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Col2 = Col客先在庫参照 '2009/09/26 ADD
			'UPGRADE_WARNING: オブジェクト fpSpd.BlockMode の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.BlockMode = True
			'UPGRADE_WARNING: オブジェクト vDisp_Spd(20) の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			If vDisp_Spd(20) = 0 Then
				'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.ColHidden = True
			Else
				'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.ColHidden = False
			End If
			'UPGRADE_WARNING: オブジェクト fpSpd.BlockMode の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.BlockMode = False
			'転用
			'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Col = Col転用
			'UPGRADE_WARNING: オブジェクト vDisp_Spd(21) の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			If vDisp_Spd(21) = 0 Then
				'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.ColHidden = True
			Else
				'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.ColHidden = False
			End If
			'発注数
			'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Col = Col発注数
			'UPGRADE_WARNING: オブジェクト vDisp_Spd(22) の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			If vDisp_Spd(22) = 0 Then
				'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.ColHidden = True
			Else
				'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.ColHidden = False
			End If
			'2015/02/24 ADD↓
			'他社伝票番号
			On Error Resume Next
			'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Col = Col他社伝票番号
			'UPGRADE_WARNING: オブジェクト vDisp_Spd(23) の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			If vDisp_Spd(23) = 0 Then
				'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.ColHidden = True
			Else
				'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.ColHidden = False
			End If
			On Error GoTo 0
			'2015/02/24 ADD↑
			'2015/09/29 ADD↓
			'他社納品日付
			On Error Resume Next
			'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Col = Col他社納品日付
			'UPGRADE_WARNING: オブジェクト vDisp_Spd(24) の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			If vDisp_Spd(24) = 0 Then
				'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.ColHidden = True
			Else
				'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.ColHidden = False
			End If
			On Error GoTo 0
			'2015/09/29 ADD↑
			'2018/05/03 ADD↓
			'明細備考
			On Error Resume Next
			'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.Col = Col明細備考
			'UPGRADE_WARNING: オブジェクト vDisp_Spd(25) の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			If vDisp_Spd(25) = 0 Then
				'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.ColHidden = True
			Else
				'UPGRADE_WARNING: オブジェクト fpSpd.ColHidden の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.ColHidden = False
			End If
			On Error GoTo 0
			'2018/05/03 ADD↑
			'2009/09/18 ADD↓
			'''        On Error Resume Next
			'''        '社内在庫参照
			'''        .Col = Col社内在庫参照
			'''        If vDisp_Spd(23) = 0 Then
			'''            .ColHidden = True
			'''        Else
			'''            .ColHidden = False
			'''        End If
			'''        '客先在庫参照
			'''        .Col = Col客先在庫参照
			'''        If vDisp_Spd(24) = 0 Then
			'''            .ColHidden = True
			'''        Else
			'''            .ColHidden = False
			'''        End If
			'''        On Error GoTo 0
			'2009/09/18 ADD↑
			
			'UPGRADE_WARNING: オブジェクト fpSpd.ReDraw の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.ReDraw = True
		End With
		
	End Function
	
	'パブリック
	'SnwMT02F12[員数取込]より呼ばれる。
	Public Sub DspGetData(ByRef rs As ADODB.Recordset, ByRef DspRow As Integer)
		Dim fpSpd As Object
		
		'※　後で、SetCheckTempDと統合する？
		'途中からの挿入の処理なので、統合はちょっと？？（AllClearとか・項目の統一ができていない）
		
		'指定の行に指定の見積番号のシート情報を表示する
		'------------------------------------------
		'   UPDATE      2005/10/14  [U区分]項目追加
		'                           U設定の行は黄色表示する
		'------------------------------------------
		'    Dim RecArry() As Variant
		'    Dim i As Long, j As Integer
		Dim X As Integer
		Dim wLine As Integer
		
		'2015/10/26 ADD
		Dim cSeihin As clsSeihin
		cSeihin = New clsSeihin
		
		
		HourGlass(True)
		
		'表示行セット
		wLine = DspRow
		'    i = 1
		
		'UPGRADE_WARNING: オブジェクト fpSpd.AutoCalc の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		fpSpd.AutoCalc = False
		
		'''    '2016/06/22 ADD
		'''    RecArry = rs.GetRows(fpSpd.MaxRows, , _
		''''                Array("見積区分", "クレーム明細区分", "他社納品日付", "他社伝票番号", "SP区分", "PC区分", "製品NO", "仕様NO", _
		''''                        "ベース色", "漢字名称", "W", "D", "H", "D1", "D2", "H1", "H2", "エラー内容", _
		''''                        "定価", "U区分", "仕入単価", "仕入率", "売上単価", "売上率", "M区分", "見積数量", "単位名", _
		''''                        "売上金額", "仕入金額", "売上税区分", "消費税額", "仕入先CD", "仕入先名", _
		''''                        "作業区分CD", "作業区分CD", "配送先CD", "配送先名", _
		''''                        "社内在庫数", "社内在庫数", "客先在庫数", "客先在庫数", "転用数", "発注調整数", "発注数", "総数量", _
		''''                        "仕分数1", "仕分数2", "仕分数3", "仕分数4", "仕分数5", _
		''''                        "仕分数6", "仕分数7", "仕分数8", "仕分数9", "仕分数10", _
		''''                        "仕分数11", "仕分数12", "仕分数13", "仕分数14", "仕分数15", _
		''''                        "仕分数16", "仕分数17", "仕分数18", "仕分数19", "仕分数20", _
		''''                        "仕分数21", "仕分数22", "仕分数23", "仕分数24", "仕分数25", _
		''''                        "仕分数26", "仕分数27", "仕分数28", "仕分数29", "仕分数30", _
		''''                        "製品区分" _
		''''                        ))
		
		With fpSpd
			'UPGRADE_WARNING: オブジェクト fpSpd.ReDraw の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.ReDraw = False
			
			Do Until rs.EOF
				'2017/10/05 ADD↓
				If clsSPD_Renamed.GetTextEX(Col見積区分, wLine) = "" And clsSPD_Renamed.GetTextEX(Col製品NO, wLine) = "" And clsSPD_Renamed.GetTextEX(Col仕様NO, wLine) = "" And clsSPD_Renamed.GetTextEX(Col名称, wLine) = "" Then
					'見積区分・製品NO・仕様NO・名称が空白の場合、行に書き込む
					'空白以外は1行下げて確認する。
					'2017/10/05 ADD↑
					
					'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					fpSpd.SetText(Col見積区分, wLine, Trim(rs.Fields("見積区分").Value & ""))
					'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					fpSpd.SetText(Colクレーム区分, wLine, VB6.Format(Trim(rs.Fields("クレーム明細区分").Value & ""), "#")) '2016/06/22 ADD
					
					'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					fpSpd.SetText(Col他社伝票番号, wLine, Trim(rs.Fields("他社伝票番号").Value & ""))
					'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					fpSpd.SetText(Col他社納品日付, wLine, Trim(rs.Fields("他社納品日付").Value & ""))
					
					'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					fpSpd.SetText(ColSP区分, wLine, Trim(rs.Fields("SP区分").Value & ""))
					'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					fpSpd.SetText(ColPC区分, wLine, Trim(rs.Fields("PC区分").Value & ""))
					
					'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					fpSpd.SetText(Col製品NO, wLine, Trim(rs.Fields("製品NO").Value & ""))
					'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					fpSpd.SetText(Col仕様NO, wLine, Trim(rs.Fields("仕様NO").Value & ""))
					'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					fpSpd.SetText(Colベース色, wLine, Trim(rs.Fields("ベース色").Value & ""))
					'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					fpSpd.SetText(Col名称, wLine, Trim(rs.Fields("漢字名称").Value & ""))
					
					'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					fpSpd.SetText(ColW, wLine, VB6.Format(rs.Fields("W").Value & "", "#"))
					'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					fpSpd.SetText(ColD, wLine, VB6.Format(rs.Fields("D").Value & "", "#"))
					'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					fpSpd.SetText(ColH, wLine, VB6.Format(rs.Fields("H").Value & "", "#"))
					
					'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					fpSpd.SetText(ColD1, wLine, VB6.Format(rs.Fields("D1").Value & "", "#"))
					'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					fpSpd.SetText(ColD2, wLine, VB6.Format(rs.Fields("D2").Value & "", "#"))
					'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					fpSpd.SetText(ColH1, wLine, VB6.Format(rs.Fields("H1").Value & "", "#"))
					'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					fpSpd.SetText(ColH2, wLine, VB6.Format(rs.Fields("H2").Value & "", "#"))
					
					'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					fpSpd.SetText(Col明細備考, wLine, Trim(rs.Fields("明細備考").Value & "")) '2018/05/03 ADD
					
					'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					fpSpd.SetText(Colエラー内容, wLine, Trim(rs.Fields("エラー内容").Value & ""))
					'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					fpSpd.SetText(Col定価, wLine, IIf(Trim(rs.Fields("定価").Value & "") = "0", "", Trim(rs.Fields("定価").Value & "")))
					'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					fpSpd.SetText(ColU区分, wLine, Trim(rs.Fields("U区分").Value & ""))
					
					'2023/05/11 DEL↓ 単価は参照しない
					'                fpSpd.SetText Col原価, wLine, IIf(Trim$(rs.Fields("仕入単価").Value & "") = "0", "", Trim$(rs.Fields("仕入単価").Value & ""))
					'                fpSpd.SetText Col仕入率, wLine, IIf(Trim$(rs.Fields("仕入率").Value & "") = "0", "", Trim$(rs.Fields("仕入率").Value & ""))
					'                fpSpd.SetText Col売価, wLine, IIf(Trim$(rs.Fields("売上単価").Value & "") = "0", "", Trim$(rs.Fields("売上単価").Value & ""))
					'                fpSpd.SetText Col売上率, wLine, IIf(Trim$(rs.Fields("売上率").Value & "") = "0", "", Trim$(rs.Fields("売上率").Value & ""))
					'2023/04/07 DEL↑
					
					'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					fpSpd.SetText(ColM, wLine, Trim(rs.Fields("M区分").Value & ""))
					
					'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					fpSpd.SetText(Col見積数量, wLine, IIf(Trim(rs.Fields("見積数量").Value & "") = "0", "", Trim(rs.Fields("見積数量").Value & "")))
					'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					fpSpd.SetText(Col単位, wLine, Trim(rs.Fields("単位名").Value & ""))
					
					'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					fpSpd.SetText(Col金額, wLine, IIf(Trim(rs.Fields("売上金額").Value & "") = "0", "", Trim(rs.Fields("売上金額").Value & "")))
					'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					fpSpd.SetText(Col仕入金額, wLine, IIf(Trim(rs.Fields("仕入金額").Value & "") = "0", "", Trim(rs.Fields("仕入金額").Value & "")))
					
					'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					fpSpd.SetText(Col売上税区分, wLine, Trim(rs.Fields("売上税区分").Value & ""))
					'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					fpSpd.SetText(Col消費税額, wLine, IIf(Trim(rs.Fields("消費税額").Value & "") = "0", "", Trim(rs.Fields("消費税額").Value & "")))
					
					Select Case Trim(rs.Fields("見積区分").Value & "")
						Case "A", "C", "S"
							'2017/05/14 ADD↓
							'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							fpSpd.SetText(Col仕入業者CD, wLine, "")
							'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							fpSpd.SetText(Col仕入業者名, wLine, "")
							'2017/05/14 ADD↑
							'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							fpSpd.SetText(Col仕入先CD, wLine, "")
							'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							fpSpd.SetText(Col仕入先名, wLine, "")
							'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							fpSpd.SetText(Col送り先CD, wLine, "")
							'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							fpSpd.SetText(Col送り先名, wLine, "")
						Case Else
							'2017/05/14 ADD↓
							'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							fpSpd.SetText(Col仕入業者CD, wLine, Trim(rs.Fields("仕入業者CD").Value & ""))
							'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							fpSpd.SetText(Col仕入業者名, wLine, Trim(rs.Fields("仕入業者名").Value & ""))
							'2017/05/14 ADD↑
							'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							fpSpd.SetText(Col仕入先CD, wLine, Trim(rs.Fields("仕入先CD").Value & ""))
							'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							fpSpd.SetText(Col仕入先名, wLine, Trim(rs.Fields("仕入先名").Value & ""))
							'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							fpSpd.SetText(Col送り先CD, wLine, Trim(rs.Fields("配送先CD").Value & ""))
							'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
							fpSpd.SetText(Col送り先名, wLine, Trim(rs.Fields("配送先名").Value & ""))
					End Select
					'2016/06/22 ADD↓
					'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					fpSpd.SetText(Col作業区分CD, wLine, VB6.Format(Trim(rs.Fields("作業区分CD").Value & ""), "#"))
					''        fpSpd.SetText Col作業区分名, wLine, Trim$(rs.Fields("作業区分名").Value & "")
					'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					fpSpd.SetText(Col作業区分名, wLine, ModKubuns.Get作業区分名(VB6.Format(Trim(rs.Fields("作業区分CD").Value & ""), "#")))
					'2016/06/22 ADD↑
					
					
					'''            fpSpd.SetText Col社内在庫, wLine, IIf(Trim$(rs.Fields("社内在庫数").Value & "") = "0", "", Trim$(rs.Fields("社内在庫数").Value & ""))
					'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					fpSpd.SetText(Col社内在庫, wLine, "")
					'        fpSpd.SetText Col社内在庫参照, wLine, IIf(Trim$(rs.Fields("社内在庫数").Value & "") = "0", "", Trim$(rs.Fields("社内在庫数").Value & ""))
					
					'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					fpSpd.SetText(Col客先在庫, wLine, IIf(Trim(rs.Fields("客先在庫数").Value & "") = "0", "", Trim(rs.Fields("客先在庫数").Value & "")))
					'        fpSpd.SetText Col客先在庫参照, wLine, IIf(Trim$(rs.Fields("客先在庫数").Value & "") = "0", "", Trim$(rs.Fields("客先在庫数").Value & ""))
					'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					fpSpd.SetText(Col転用, wLine, IIf(Trim(rs.Fields("転用数").Value & "") = "0", "", Trim(rs.Fields("転用数").Value & "")))
					
					'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					fpSpd.SetText(Col発注調整数, wLine, IIf(Trim(rs.Fields("発注調整数").Value & "") = "0", "", Trim(rs.Fields("発注調整数").Value & "")))
					'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					fpSpd.SetText(Col発注数, wLine, IIf(Trim(rs.Fields("発注数").Value & "") = "0", "", Trim(rs.Fields("発注数").Value & "")))
					'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					fpSpd.SetText(Col数量合計, wLine, IIf(Trim(rs.Fields("総数量").Value & "") = "0", "", Trim(rs.Fields("総数量").Value & "")))
					
					
					'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					fpSpd.SetText(Col製品区分, wLine, Trim(rs.Fields("製品区分").Value & ""))
					
					'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					fpSpd.SetText(Col入庫日, wLine, VB6.Format(Trim(rs.Fields("入庫日").Value & ""), "yy/mm/dd")) '2018/08/06 ADD
					
					'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					fpSpd.SetText(Col出庫日, wLine, VB6.Format(Trim(rs.Fields("出庫日").Value & ""), "yy/mm/dd")) '2020/09/16 ADD
					
					'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					fpSpd.SetText(Col追番R, wLine, Trim(rs.Fields("追番R").Value & "")) '2019/12/12 ADD
					
					
					'            fpSpd.SetText Col仕分数1, wLine, Format$(Trim$(rs.Fields("仕分数1").Value & ""), "#")
					For X = 1 To 30
						'rs.Fields("仕分数" & j) = NullToZero(WkTBLS(wLine, Col仕分数1 + j - 1))
						''                fpSpd.SetText Col仕分数1 + X - 1, wLine, Format$(Trim$(rs.Fields("仕分数" & X).Value & ""), "#")
						'UPGRADE_WARNING: オブジェクト fpSpd.SetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						fpSpd.SetText(Col仕分数1 + X - 1, wLine, IIf(Trim(rs.Fields("仕分数" & X).Value & "") = "0", "", Trim(rs.Fields("仕分数" & X).Value & ""))) '2017/06/27 ADD
					Next 
					
					Call Get率(wLine, Col原価)
					Call Get率(wLine, Col売価)
					Call Get金額(wLine)
					
					'2017/07/03 ADD
					Call RowBackColorSet2(clsSPD_Renamed.GetTextEX(ColU区分, CInt(wLine)), CInt(wLine))
					
					
					Call Lock_Seihin(wLine, clsSPD_Renamed.GetTextEX(Col製品NO, CInt(wLine)), clsSPD_Renamed.GetTextEX(Col仕様NO, CInt(wLine))) '2020/10/10 ADD
					
					'2020/10/10 DEL↓
					'''''                '2015/10/26 ADD↓
					'''''                '在庫管理する場合、名称をロックする。
					'''''                cSeihin.Initialize
					'''''                cSeihin.製品NO = clsSPD.GetTextEX(Col製品NO, CLng(wLine))
					'''''                cSeihin.仕様NO = clsSPD.GetTextEX(Col仕様NO, CLng(wLine))
					'''''                If cSeihin.GetbyID = True Then
					'''''                    If cSeihin.在庫区分 = 0 Then
					'''''                        .BlockMode = True
					'''''                        .Row = wLine
					'''''                        .Row2 = wLine
					'''''                        '.Col = Col名称
					'''''                        .Col = ColW '2015/11/13 ADD
					'''''                        .Col2 = ColH2
					'''''                        .Lock = True
					'''''                        .BlockMode = False
					'''''                    Else
					'''''                        .BlockMode = True
					'''''                        .Row = wLine
					'''''                        .Row2 = wLine
					'''''                        '.Col = Col名称
					'''''                        .Col = ColW '2015/11/13 ADD
					'''''                        .Col2 = ColH2
					'''''                        .Lock = False
					'''''                        .BlockMode = False
					'''''                    End If
					'''''                Else
					'''''                    .BlockMode = True
					'''''                    .Row = wLine
					'''''                    .Row2 = wLine
					'''''                    '.Col = Col名称
					'''''                    .Col = ColW '2015/11/13 ADD
					'''''                    .Col2 = ColH2
					'''''                    .Lock = False
					'''''                    .BlockMode = False
					'''''                End If
					'''''                '2015/10/26 ADD↑
					'2020/10/10 DEL↑
					
					If clsSPD_Renamed.GetTextEX(Colエラー内容, CInt(wLine)) Like "原価率*" Then '2015/03/10 ADD
						'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Col = Col原価
						'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Row = wLine
						'UPGRADE_WARNING: オブジェクト fpSpd.ForeColor の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.ForeColor = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red)
						'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Col = Col売価
						'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Row = wLine
						'UPGRADE_WARNING: オブジェクト fpSpd.ForeColor の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.ForeColor = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red)
					Else
						'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Col = Col原価
						'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Row = wLine
						'UPGRADE_WARNING: オブジェクト fpSpd.ForeColor の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.ForeColor = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black)
						'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Col = Col売価
						'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.Row = wLine
						'UPGRADE_WARNING: オブジェクト fpSpd.ForeColor の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						.ForeColor = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black)
					End If
					
					'2023/09/26 ADD↓
					'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.Col = 1
					'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.Row = wLine
					'UPGRADE_WARNING: オブジェクト fpSpd.Col2 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					'UPGRADE_WARNING: オブジェクト fpSpd.MaxCols の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.Col2 = .MaxCols
					'UPGRADE_WARNING: オブジェクト fpSpd.Row2 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.Row2 = wLine
					'UPGRADE_WARNING: オブジェクト fpSpd.BlockMode の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.BlockMode = True
					'UPGRADE_WARNING: オブジェクト fpSpd.ForeColor の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.ForeColor = Getインボイス番号NGColor(Trim(rs.Fields("仕入先CD").Value & ""))
					'UPGRADE_WARNING: オブジェクト fpSpd.BlockMode の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.BlockMode = False
					'2023/09/26 ADD↑
					
					rs.MoveNext()
					'            i = i + 1
				End If
				'表示行カウント
				wLine = wLine + 1
			Loop 
			'UPGRADE_WARNING: オブジェクト fpSpd.ReDraw の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			.ReDraw = True
		End With
		
		Call Calc合計()
		
		'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト fpSpd.ActiveCol の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		fpSpd.Col = fpSpd.ActiveCol '2003/11/25 ADD
		'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		fpSpd.Row = fpSpd.ActiveRow
		'''    fpSpd.Col = 1
		'''    fpSpd.Row = 1
		'UPGRADE_WARNING: オブジェクト fpSpd.Action の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		fpSpd.Action = FPSpreadADO.ActionConstants.ActionActiveCell
		'UPGRADE_WARNING: オブジェクト fpSpd.AutoCalc の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		fpSpd.AutoCalc = True
		
		'2006/06/22 ADD-----------------
		Dim check As Boolean
		'UPGRADE_WARNING: オブジェクト fpSpd.ActiveCol の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Select Case fpSpd.ActiveCol
			Case ColPC区分
				'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト fpSpd.GetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				check = fpSpd.GetText(ColPC区分, fpSpd.ActiveRow, HoldPc)
			Case Col製品NO
				'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト fpSpd.GetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				check = fpSpd.GetText(ColPC区分, fpSpd.ActiveRow, HoldPc)
				'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト fpSpd.GetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				check = fpSpd.GetText(Col製品NO, fpSpd.ActiveRow, HoldSei)
			Case Col仕様NO
				'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト fpSpd.GetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				check = fpSpd.GetText(ColPC区分, fpSpd.ActiveRow, HoldPc)
				'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト fpSpd.GetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				check = fpSpd.GetText(Col製品NO, fpSpd.ActiveRow, HoldSei)
				'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				'UPGRADE_WARNING: オブジェクト fpSpd.GetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				check = fpSpd.GetText(Col仕様NO, fpSpd.ActiveRow, HoldSiy)
				''        Case 6, 7
				''             'IMEモード[使用不可]
				''''            ImmAssociateContext Me.hwnd, 0&
				''        Case 14 To 18
				''             'IMEモード[使用不可]
				''            ImmAssociateContext Me.hwnd, 0&
				''            Debug.Print "IME:" & Col
		End Select
		
		'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト fpSpd.ActiveCol の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト fpSpd.GetText の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		check = fpSpd.GetText(fpSpd.ActiveCol, fpSpd.ActiveRow, HoldCD)
		
		'UPGRADE_WARNING: オブジェクト fpSpd.ActiveCol の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		HCol = fpSpd.ActiveCol
		'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		HRow = fpSpd.ActiveRow
		
		HourGlass(False)
	End Sub
	
	'''''''''
	''''''''''パブリック
	''''''''''SnwMT02F12[員数取込]より呼ばれる。
	'''''''''Public Sub DspGetData(rs As ADODB.Recordset, DspRow As Long)
	'''''''''
	''''''''''※　後で、SetCheckTempDと統合する？
	'''''''''
	''''''''''指定の行に指定の見積番号のシート情報を表示する
	''''''''''------------------------------------------
	''''''''''   UPDATE      2005/10/14  [U区分]項目追加
	''''''''''                           U設定の行は黄色表示する
	''''''''''------------------------------------------
	'''''''''    Dim RecArry() As Variant
	'''''''''    Dim i As Long, j As Integer
	'''''''''    Dim wLine As Long
	'''''''''
	'''''''''    '2015/10/26 ADD
	'''''''''    Dim cSeihin As clsSeihin
	'''''''''    Set cSeihin = New clsSeihin
	'''''''''
	'''''''''
	'''''''''    HourGlass True
	'''''''''
	'''''''''    '表示行セット
	'''''''''    wLine = DspRow
	'''''''''
	'''''''''    fpSpd.AutoCalc = False
	''''''''''''    RecArry = rs.GetRows(fpSpd.MaxRows, , _
	''''''''''                Array("見積区分", "SP区分", "PC区分", "製品NO", "仕様NO", _
	''''''''''                        "ベース色", "漢字名称", "W", "D", "H", "D1", "D2", "H1", "H2", "エラー内容", _
	''''''''''                        "定価", "U区分", "仕入単価", "仕入率", "売上単価", "売上率", "M区分", "見積数量", "単位名", _
	''''''''''                        "売上金額", "仕入金額", "売上税区分", "消費税額", "仕入先CD", "仕入先名", "配送先CD", "配送先名", _
	''''''''''                        "社内在庫数", "社内在庫数", "客先在庫数", "客先在庫数", "転用数", "発注数", "総数量", _
	''''''''''                        "仕分数1", "仕分数2", "仕分数3", "仕分数4", "仕分数5", _
	''''''''''                        "仕分数6", "仕分数7", "仕分数8", "仕分数9", "仕分数10", _
	''''''''''                        "仕分数11", "仕分数12", "仕分数13", "仕分数14", "仕分数15", _
	''''''''''                        "仕分数16", "仕分数17", "仕分数18", "仕分数19", "仕分数20", _
	''''''''''                        "仕分数21", "仕分数22", "仕分数23", "仕分数24", "仕分数25", _
	''''''''''                        "仕分数26", "仕分数27", "仕分数28", "仕分数29", "仕分数30", _
	''''''''''                        "製品区分" _
	''''''''''                        ))
	'''''''''    '2014/09/09 ADD
	'''''''''''    RecArry = rs.GetRows(fpSpd.MaxRows, , _
	''''''''''''                Array("見積区分", "SP区分", "PC区分", "製品NO", "仕様NO", _
	''''''''''''                        "ベース色", "漢字名称", "W", "D", "H", "D1", "D2", "H1", "H2", "エラー内容", _
	''''''''''''                        "定価", "U区分", "仕入単価", "仕入率", "売上単価", "売上率", "M区分", "見積数量", "単位名", _
	''''''''''''                        "売上金額", "仕入金額", "売上税区分", "消費税額", "仕入先CD", "仕入先名", "配送先CD", "配送先名", _
	''''''''''''                        "社内在庫数", "社内在庫数", "客先在庫数", "客先在庫数", "転用数", "発注調整数", "発注数", "総数量", _
	''''''''''''                        "仕分数1", "仕分数2", "仕分数3", "仕分数4", "仕分数5", _
	''''''''''''                        "仕分数6", "仕分数7", "仕分数8", "仕分数9", "仕分数10", _
	''''''''''''                        "仕分数11", "仕分数12", "仕分数13", "仕分数14", "仕分数15", _
	''''''''''''                        "仕分数16", "仕分数17", "仕分数18", "仕分数19", "仕分数20", _
	''''''''''''                        "仕分数21", "仕分数22", "仕分数23", "仕分数24", "仕分数25", _
	''''''''''''                        "仕分数26", "仕分数27", "仕分数28", "仕分数29", "仕分数30", _
	''''''''''''                        "製品区分" _
	''''''''''''                        ))
	'''''''''    '2015/02/03 ADD
	''''''''''''    RecArry = rs.GetRows(fpSpd.MaxRows, , _
	'''''''''''''                Array("見積区分", "SP区分", "PC区分", "製品NO", "仕様NO", _
	'''''''''''''                        "ベース色", "他社伝票番号", "漢字名称", "W", "D", "H", "D1", "D2", "H1", "H2", "エラー内容", _
	'''''''''''''                        "定価", "U区分", "仕入単価", "仕入率", "売上単価", "売上率", "M区分", "見積数量", "単位名", _
	'''''''''''''                        "売上金額", "仕入金額", "売上税区分", "消費税額", "仕入先CD", "仕入先名", "配送先CD", "配送先名", _
	'''''''''''''                        "社内在庫数", "社内在庫数", "客先在庫数", "客先在庫数", "転用数", "発注調整数", "発注数", "総数量", _
	'''''''''''''                        "仕分数1", "仕分数2", "仕分数3", "仕分数4", "仕分数5", _
	'''''''''''''                        "仕分数6", "仕分数7", "仕分数8", "仕分数9", "仕分数10", _
	'''''''''''''                        "仕分数11", "仕分数12", "仕分数13", "仕分数14", "仕分数15", _
	'''''''''''''                        "仕分数16", "仕分数17", "仕分数18", "仕分数19", "仕分数20", _
	'''''''''''''                        "仕分数21", "仕分数22", "仕分数23", "仕分数24", "仕分数25", _
	'''''''''''''                        "仕分数26", "仕分数27", "仕分数28", "仕分数29", "仕分数30", _
	'''''''''''''                        "製品区分" _
	'''''''''''''                        ))
	'''''''''''    '2015/09/29 ADD
	'''''''''''    RecArry = rs.GetRows(fpSpd.MaxRows, , _
	''''''''''''                Array("見積区分", "他社納品日付", "他社伝票番号", "SP区分", "PC区分", "製品NO", "仕様NO", _
	''''''''''''                        "ベース色", "漢字名称", "W", "D", "H", "D1", "D2", "H1", "H2", "エラー内容", _
	''''''''''''                        "定価", "U区分", "仕入単価", "仕入率", "売上単価", "売上率", "M区分", "見積数量", "単位名", _
	''''''''''''                        "売上金額", "仕入金額", "売上税区分", "消費税額", "仕入先CD", "仕入先名", "配送先CD", "配送先名", _
	''''''''''''                        "社内在庫数", "社内在庫数", "客先在庫数", "客先在庫数", "転用数", "発注調整数", "発注数", "総数量", _
	''''''''''''                        "仕分数1", "仕分数2", "仕分数3", "仕分数4", "仕分数5", _
	''''''''''''                        "仕分数6", "仕分数7", "仕分数8", "仕分数9", "仕分数10", _
	''''''''''''                        "仕分数11", "仕分数12", "仕分数13", "仕分数14", "仕分数15", _
	''''''''''''                        "仕分数16", "仕分数17", "仕分数18", "仕分数19", "仕分数20", _
	''''''''''''                        "仕分数21", "仕分数22", "仕分数23", "仕分数24", "仕分数25", _
	''''''''''''                        "仕分数26", "仕分数27", "仕分数28", "仕分数29", "仕分数30", _
	''''''''''''                        "製品区分" _
	''''''''''''                        ))
	'''''''''
	'''''''''    '2016/06/22 ADD
	'''''''''    RecArry = rs.GetRows(fpSpd.MaxRows, , _
	''''''''''                Array("見積区分", "クレーム明細区分", "他社納品日付", "他社伝票番号", "SP区分", "PC区分", "製品NO", "仕様NO", _
	''''''''''                        "ベース色", "漢字名称", "W", "D", "H", "D1", "D2", "H1", "H2", "エラー内容", _
	''''''''''                        "定価", "U区分", "仕入単価", "仕入率", "売上単価", "売上率", "M区分", "見積数量", "単位名", _
	''''''''''                        "売上金額", "仕入金額", "売上税区分", "消費税額", "仕入先CD", "仕入先名", _
	''''''''''                        "作業区分CD", "作業区分CD", "配送先CD", "配送先名", _
	''''''''''                        "社内在庫数", "社内在庫数", "客先在庫数", "客先在庫数", "転用数", "発注調整数", "発注数", "総数量", _
	''''''''''                        "仕分数1", "仕分数2", "仕分数3", "仕分数4", "仕分数5", _
	''''''''''                        "仕分数6", "仕分数7", "仕分数8", "仕分数9", "仕分数10", _
	''''''''''                        "仕分数11", "仕分数12", "仕分数13", "仕分数14", "仕分数15", _
	''''''''''                        "仕分数16", "仕分数17", "仕分数18", "仕分数19", "仕分数20", _
	''''''''''                        "仕分数21", "仕分数22", "仕分数23", "仕分数24", "仕分数25", _
	''''''''''                        "仕分数26", "仕分数27", "仕分数28", "仕分数29", "仕分数30", _
	''''''''''                        "製品区分" _
	''''''''''                        ))
	'''''''''
	'''''''''    With fpSpd
	'''''''''        .ReDraw = False
	'''''''''        For i = 0 To UBound(RecArry, 2)
	'''''''''            For j = 0 To UBound(RecArry)
	'''''''''                Select Case j + 2
	'''''''''                    Case ColW To ColH2
	'''''''''                        'サイズ
	'''''''''                        .SetText j + 2, wLine, IIf(Trim$("" & RecArry(j, i)) = "0", "", Trim$("" & RecArry(j, i)))
	''''''''''''                    Case Col定価, Col原価, Col仕入率, Col売価, Col売上率, Col見積数量, Col金額, Col仕入金額, _
	'''''''''''''                           Col社内在庫, Col客先在庫, Col転用, Col発注数, Col数量合計, _
	'''''''''''''                           Col発注調整数 '2014/09/09 ADD
	''''''''''''                        '定価・原価・仕入％・売価・売上％
	''''''''''''                        .SetText j + 2, wLine, IIf(Trim$("" & RecArry(j, i)) = "0", "", Trim$("" & RecArry(j, i)))
	'''''''''                    Case Col定価, Col原価, Col仕入率, Col売価, Col売上率, Col見積数量, Col金額, Col仕入金額, _
	''''''''''                           Col客先在庫, Col転用, Col発注数, Col数量合計, _
	''''''''''                           Col発注調整数 '2014/09/09 ADD
	'''''''''                        '定価・原価・仕入％・売価・売上％
	'''''''''                        .SetText j + 2, wLine, IIf(Trim$("" & RecArry(j, i)) = "0", "", Trim$("" & RecArry(j, i)))
	'''''''''                    '2017/02/10 ADD↓
	'''''''''                    Case Col社内在庫
	'''''''''                        .SetText j + 2, wLine, ""
	'''''''''                    '2017/02/10 ADD↑
	'''''''''                    Case Col仕分数1 To Col仕分数1 + 29
	'''''''''                        '仕分数
	'''''''''                        .SetText j + 2, wLine, IIf(Trim$("" & RecArry(j, i)) = "0", "", Trim$("" & RecArry(j, i)))
	'''''''''                    Case Col製品区分
	'''''''''                        '製品区分
	'''''''''                        .SetText j + 2, wLine, "" & RecArry(j, i)
	'''''''''''''                    Case Col見積明細連番
	'''''''''''''                        '見積明細連番
	'''''''''''''                        .SetText Col見積明細連番 + 2, wLine, ""
	'''''''''                    Case Col社内在庫参照, Col客先在庫参照               '2009/09/26 ADD
	'''''''''                        '仮に社内在庫数と客先在庫数が入るので無視する
	'''''''''                    '2016/06/22 ADD↓
	'''''''''                    Case Colクレーム区分
	'''''''''                        .SetText Colクレーム区分, wLine, Format$("" & RecArry(j, i), "#")
	'''''''''                    Case Col作業区分CD
	'''''''''                        .SetText Col作業区分CD, wLine, Format$("" & RecArry(j, i), "#")
	'''''''''                        .SetText Col作業区分名, wLine, ModKubuns.Get作業区分名(Format$("" & RecArry(j, i), "#"))
	'''''''''                    Case Col作業区分名
	'''''''''                        'no
	'''''''''                    '2016/06/22 ADD↑
	'''''''''                    Case Else
	'''''''''                        .SetText j + 2, wLine, Trim$("" & RecArry(j, i))
	'''''''''                End Select
	'''''''''            Next
	'''''''''
	'''''''''            Call Get率(wLine, Col原価)
	'''''''''            Call Get率(wLine, Col売価)
	'''''''''            Call Get金額(wLine)
	'''''''''            '----------------------
	'''''''''            '---   2005/10/14.ADD
	'''''''''''            If RecArry(16, i) = "U" Then
	'''''''''''''            If clsSPD.GetTextEX(ColU区分, CLng(wLine)) = "U" Then '2015/03/04 ADD
	'''''''''''''                Call RowBackColorSet(1, CLng(wLine))
	'''''''''''''            End If
	'''''''''            '2016/06/21 ADD
	'''''''''            Call RowBackColorSet2(clsSPD.GetTextEX(ColU区分, CLng(wLine)), CLng(wLine))
	'''''''''
	''''''''''''            '2009/09/26 ADD↓ '2013/07/19 DEL
	''''''''''''            If RecArry(39, i) = 0 Then '製品時
	''''''''''''                If cDspSyaZaiko.GetZaikoInfo(HD_納期S, HD_担当者CD, CStr(RecArry(3, i)), CStr(RecArry(4, i)), CLng(NullToZero(HD_見積番号))) = False Then
	''''''''''''                    fpSpd.SetText Col社内在庫参照, wLine, ""
	''''''''''''                Else
	''''''''''''                    fpSpd.SetText Col社内在庫参照, wLine, cDspSyaZaiko.合計在庫数
	''''''''''''                End If
	''''''''''''                If cDspKyakuZaiko.GetZaikoInfo(HD_納期S, HD_担当者CD, rf_得意先CD, CStr(RecArry(3, i)), CStr(RecArry(4, i)), CLng(NullToZero(HD_見積番号))) = False Then
	''''''''''''                    fpSpd.SetText Col客先在庫参照, wLine, ""
	''''''''''''                Else
	''''''''''''                    fpSpd.SetText Col客先在庫参照, wLine, cDspKyakuZaiko.合計在庫数
	''''''''''''                End If
	''''''''''''            End If
	''''''''''''            '2009/09/26 ADD↑ '2013/07/19 DEL
	'''''''''
	'''''''''            '2015/10/26 ADD↓
	'''''''''            '在庫管理する場合、名称をロックする。
	'''''''''            cSeihin.Initialize
	'''''''''            cSeihin.製品NO = clsSPD.GetTextEX(Col製品NO, CLng(wLine))
	'''''''''            cSeihin.仕様NO = clsSPD.GetTextEX(Col仕様NO, CLng(wLine))
	'''''''''            If cSeihin.GetbyID = True Then
	'''''''''                If cSeihin.在庫区分 = 0 Then
	'''''''''                    .BlockMode = True
	'''''''''                    .Row = wLine
	'''''''''                    .Row2 = wLine
	'''''''''                    '.Col = Col名称
	'''''''''                    .Col = ColW '2015/11/13 ADD
	'''''''''                    .Col2 = ColH2
	'''''''''                    .Lock = True
	'''''''''                    .BlockMode = False
	'''''''''                Else
	'''''''''                    .BlockMode = True
	'''''''''                    .Row = wLine
	'''''''''                    .Row2 = wLine
	'''''''''                    '.Col = Col名称
	'''''''''                    .Col = ColW '2015/11/13 ADD
	'''''''''                    .Col2 = ColH2
	'''''''''                    .Lock = False
	'''''''''                    .BlockMode = False
	'''''''''                End If
	'''''''''            Else
	'''''''''                .BlockMode = True
	'''''''''                .Row = wLine
	'''''''''                .Row2 = wLine
	'''''''''                '.Col = Col名称
	'''''''''                .Col = ColW '2015/11/13 ADD
	'''''''''                .Col2 = ColH2
	'''''''''                .Lock = False
	'''''''''                .BlockMode = False
	'''''''''            End If
	'''''''''            '2015/10/26 ADD↑
	'''''''''
	'''''''''
	'''''''''            '表示行カウント
	'''''''''            wLine = wLine + 1
	'''''''''        Next
	'''''''''        .ReDraw = True
	'''''''''    End With
	'''''''''
	'''''''''    Call Calc合計
	'''''''''
	'''''''''    fpSpd.Col = fpSpd.ActiveCol             '2003/11/25 ADD
	'''''''''    fpSpd.Row = fpSpd.ActiveRow
	''''''''''''    fpSpd.Col = 1
	''''''''''''    fpSpd.Row = 1
	'''''''''    fpSpd.Action = ActionActiveCell
	'''''''''    fpSpd.AutoCalc = True
	'''''''''
	'''''''''    '2006/06/22 ADD-----------------
	'''''''''    Dim check As Boolean
	'''''''''    Select Case fpSpd.ActiveCol
	'''''''''        Case ColPC区分
	'''''''''            check = fpSpd.GetText(ColPC区分, fpSpd.ActiveRow, HoldPc)
	'''''''''        Case Col製品NO
	'''''''''            check = fpSpd.GetText(ColPC区分, fpSpd.ActiveRow, HoldPc)
	'''''''''            check = fpSpd.GetText(Col製品NO, fpSpd.ActiveRow, HoldSei)
	'''''''''        Case Col仕様NO
	'''''''''           check = fpSpd.GetText(ColPC区分, fpSpd.ActiveRow, HoldPc)
	'''''''''            check = fpSpd.GetText(Col製品NO, fpSpd.ActiveRow, HoldSei)
	'''''''''            check = fpSpd.GetText(Col仕様NO, fpSpd.ActiveRow, HoldSiy)
	'''''''''''        Case 6, 7
	'''''''''''             'IMEモード[使用不可]
	'''''''''''''            ImmAssociateContext Me.hwnd, 0&
	'''''''''''        Case 14 To 18
	'''''''''''             'IMEモード[使用不可]
	'''''''''''            ImmAssociateContext Me.hwnd, 0&
	'''''''''''            Debug.Print "IME:" & Col
	'''''''''    End Select
	'''''''''
	'''''''''    check = fpSpd.GetText(fpSpd.ActiveCol, fpSpd.ActiveRow, HoldCD)
	'''''''''
	'''''''''    HCol = fpSpd.ActiveCol
	'''''''''    HRow = fpSpd.ActiveRow
	'''''''''    '2006/06/22 ADD END--------------------
	'''''''''
	'''''''''    HourGlass False
	'''''''''End Sub
	
	Private Sub RowBackColorSet(ByRef Mode As Short, ByRef wRow As Integer)
		Dim fpSpd As Object
		'----------------------------------
		'   UPDATE      2005/10/14  新設
		'----------------------------------
		With fpSpd
			Select Case Mode
				Case 0
					'行の背景色を初期値にする
					'UPGRADE_WARNING: オブジェクト fpSpd.BlockMode の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.BlockMode = True
					'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.Col = 1
					'UPGRADE_WARNING: オブジェクト fpSpd.Col2 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					'UPGRADE_WARNING: オブジェクト fpSpd.MaxCols の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.Col2 = .MaxCols
					'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.Row = wRow
					'UPGRADE_WARNING: オブジェクト fpSpd.Row2 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.Row2 = wRow
					'UPGRADE_WARNING: オブジェクト fpSpd.BackColor の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.BackColor = &HFFFFFF
					'UPGRADE_WARNING: オブジェクト fpSpd.BlockMode の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.BlockMode = False
				Case 1
					'行の背景色を黄色にする
					'UPGRADE_WARNING: オブジェクト fpSpd.BlockMode の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.BlockMode = True
					'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.Col = 1
					'UPGRADE_WARNING: オブジェクト fpSpd.Col2 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					'UPGRADE_WARNING: オブジェクト fpSpd.MaxCols の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.Col2 = .MaxCols
					'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.Row = wRow
					'UPGRADE_WARNING: オブジェクト fpSpd.Row2 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.Row2 = wRow
					'UPGRADE_WARNING: オブジェクト fpSpd.BackColor の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.BackColor = &H80FFFF
					'UPGRADE_WARNING: オブジェクト fpSpd.BlockMode の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.BlockMode = False
			End Select
		End With
	End Sub
	
	Private Sub RowBackColorSet2(ByRef Mode As String, ByRef wRow As Integer)
		Dim fpSpd As Object
		'----------------------------------
		'   UPDATE      2005/10/14  新設
		'----------------------------------
		With fpSpd
			Select Case Mode
				Case ""
					'行の背景色を初期値にする
					'                .BlockMode = True
					'                    .Col = 1
					'                    .Col2 = .MaxCols
					'                    .Row = wRow
					'                    .Row2 = wRow
					'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.Col = -1
					'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.Row = wRow
					'UPGRADE_WARNING: オブジェクト fpSpd.BackColor の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.BackColor = &HFFFFFF
					'                .BlockMode = False
				Case "U"
					'行の背景色を黄色にする
					'UPGRADE_WARNING: オブジェクト fpSpd.BlockMode の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.BlockMode = True
					'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.Col = 1
					'UPGRADE_WARNING: オブジェクト fpSpd.Col2 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					'UPGRADE_WARNING: オブジェクト fpSpd.MaxCols の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.Col2 = .MaxCols
					'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.Row = wRow
					'UPGRADE_WARNING: オブジェクト fpSpd.Row2 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.Row2 = wRow
					'UPGRADE_WARNING: オブジェクト fpSpd.BackColor の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.BackColor = &H80FFFF
					'UPGRADE_WARNING: オブジェクト fpSpd.BlockMode の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.BlockMode = False
				Case "R"
					'行の背景色を赤色にする
					'UPGRADE_WARNING: オブジェクト fpSpd.BlockMode の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.BlockMode = True
					'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.Col = 1
					'UPGRADE_WARNING: オブジェクト fpSpd.Col2 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					'UPGRADE_WARNING: オブジェクト fpSpd.MaxCols の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.Col2 = .MaxCols
					'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.Row = wRow
					'UPGRADE_WARNING: オブジェクト fpSpd.Row2 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.Row2 = wRow
					'                    .BackColor = RGB(255, 150, 150)
					'                    .BackColor = RGB(230, 184, 183)
					'UPGRADE_WARNING: オブジェクト fpSpd.BackColor の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.BackColor = RGB(242, 220, 219)
					'UPGRADE_WARNING: オブジェクト fpSpd.BlockMode の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.BlockMode = False
				Case "H"
					'行の背景色を灰色にする
					'UPGRADE_WARNING: オブジェクト fpSpd.BlockMode の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.BlockMode = True
					'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.Col = 1
					'UPGRADE_WARNING: オブジェクト fpSpd.Col2 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					'UPGRADE_WARNING: オブジェクト fpSpd.MaxCols の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.Col2 = .MaxCols
					'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.Row = wRow
					'UPGRADE_WARNING: オブジェクト fpSpd.Row2 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.Row2 = wRow
					'UPGRADE_WARNING: オブジェクト fpSpd.BackColor の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.BackColor = RGB(217, 217, 217)
					'UPGRADE_WARNING: オブジェクト fpSpd.BlockMode の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.BlockMode = False
				Case "B"
					'行の背景色を青色にする
					'UPGRADE_WARNING: オブジェクト fpSpd.BlockMode の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.BlockMode = True
					'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.Col = 1
					'UPGRADE_WARNING: オブジェクト fpSpd.Col2 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					'UPGRADE_WARNING: オブジェクト fpSpd.MaxCols の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.Col2 = .MaxCols
					'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.Row = wRow
					'UPGRADE_WARNING: オブジェクト fpSpd.Row2 の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.Row2 = wRow
					'                    .BackColor = RGB(150, 150, 255)
					'                    .BackColor = RGB(184, 204, 228)
					'UPGRADE_WARNING: オブジェクト fpSpd.BackColor の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.BackColor = RGB(220, 230, 241)
					'UPGRADE_WARNING: オブジェクト fpSpd.BlockMode の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					.BlockMode = False
			End Select
		End With
	End Sub
	''''''2009/09/26 ADD
	'''''Private Function Upload発注情報(m見積番号 As Long) As Boolean
	'''''    Dim cmd As New adodb.Command
	'''''    Dim prm As adodb.Parameter
	'''''    Dim GrsHead As adodb.Recordset            'グローバルレコードセット(明細用)
	'''''
	'''''    Upload発注情報 = False
	'''''    HourGlass True
	'''''
	'''''    On Error GoTo Upload発注情報_Err
	'''''    '---コマンドパラメータ設定
	'''''    ' コマンドを実行する接続先を指定する
	'''''    cmd.ActiveConnection = Cn
	'''''    cmd.CommandTimeout = 0
	'''''    cmd.CommandText = "usp_HC0100発注作成"
	'''''    cmd.CommandType = adCmdStoredProc
	'''''
	'''''    ' それぞれのパラメータの値を指定する
	'''''    With cmd.Parameters
	'''''        .Item(0).Direction = adParamReturnValue
	'''''
	'''''        '-----adParamInput
	'''''        .Item(1).Value = m見積番号
	'''''
	'''''    End With
	'''''
	'''''    cmd.Execute
	'''''
	'''''    If cmd(0) <> 0 Then
	'''''        CriticalAlarm (cmd("@RetST") & " : " & cmd("@RetMsg"))
	'''''        Upload発注情報 = False
	'''''    Else
	'''''        Upload発注情報 = True
	'''''    End If
	'''''
	'''''
	'''''    Set cmd = Nothing
	'''''
	'''''
	'''''    On Error GoTo 0
	'''''    HourGlass False
	'''''    Exit Function
	'''''
	'''''Upload発注情報_Err:
	'''''    CriticalAlarm Err.Number & " " & Err.Description
	'''''    Set cmd = Nothing
	'''''    HourGlass False
	'''''End Function
	
	Private Sub cb検索_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cb検索.Click
		Dim fpSpd As Object
		Dim ret As Short
		Dim ActiveRow As Integer '2013/02/12 ADD
		'UPGRADE_WARNING: オブジェクト fpSpd.SetFocus の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		fpSpd.SetFocus()
		'カレント列から"0120"を検索
		'''    ret = clsSPD.SearchCol(Col製品NO, 1, -1, [tx_製品NO].Text)   '2013/02/12 DEL
		'2013/02/12 ADD↓
		'    If fpSpd.ActiveRow <= 1 Then
		'        ActiveRow = 1
		'    Else
		'        ActiveRow = fpSpd.ActiveRow + 1
		'    End If
		'    ret = clsSPD.SearchCol(Col製品NO, ActiveRow, -1, [tx_製品NO].Text)
		'UPGRADE_WARNING: オブジェクト fpSpd.ActiveCol の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト fpSpd.ActiveRow の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		ret = clsSPD_Renamed.Search(fpSpd.ActiveRow, fpSpd.ActiveCol, [tx_製品NO].Text) '2014/11/13 ADD
		'2013/02/12 ADD↑
		If ret = -1 Then
			CheckAlarm("該当データは存在しません。")
			'2013/02/12 ADD↓
			'        Call fpSpd_GotFocus 'ホイール制御が効かなくなるので呼び出す
			[cb検索].Focus()
			'UPGRADE_WARNING: オブジェクト fpSpd.SetFocus の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			fpSpd.SetFocus()
			'UPGRADE_WARNING: オブジェクト fpSpd.Col の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			fpSpd.Col = 1
			'UPGRADE_WARNING: オブジェクト fpSpd.Row の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			fpSpd.Row = 1
			'UPGRADE_WARNING: オブジェクト fpSpd.Action の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			fpSpd.Action = FPSpreadADO.ActionConstants.ActionActiveCell
			'2013/02/12 ADD↑
		Else
			'UPGRADE_WARNING: オブジェクト fpSpd.SetFocus の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			fpSpd.SetFocus()
			'        fpSpd.Col = Col製品NO
			'        fpSpd.Row = ret
			'''        If fpSpd.Lock = True Then
			'''            fpSpd.Lock = False
			'''            fpSpd.Action = ActionActiveCell
			'''            fpSpd.Lock = True
			'''        Else
			'UPGRADE_WARNING: オブジェクト fpSpd.Action の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			fpSpd.Action = FPSpreadADO.ActionConstants.ActionGotoCell
			'UPGRADE_WARNING: オブジェクト fpSpd.Action の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
			fpSpd.Action = FPSpreadADO.ActionConstants.ActionActiveCell
			'''        End If
			'        fpSpd.SetActiveCell Col実棚卸数, ret
		End If
	End Sub
	
	'2013/04/09 ADD↓
	Private Function Delete発注情報(ByRef m見積番号 As Integer) As Boolean
		Dim cmd As New ADODB.Command
		Dim prm As ADODB.Parameter
		Dim sql As String
		
		Delete発注情報 = False
		HourGlass(True)
		
		On Error GoTo Delete発注情報_Err
		'---コマンドパラメータ設定
		' コマンドを実行する接続先を指定する
		cmd.let_ActiveConnection(Cn)
		cmd.CommandTimeout = 0
		
		sql = "DELETE FROM TD発注明細 WHERE 見積番号 = " & m見積番号
		
		cmd.CommandText = sql
		cmd.CommandType = ADODB.CommandTypeEnum.adCmdText
		
		cmd.Execute()
		
		sql = "DELETE FROM TD発注 WHERE 見積番号 = " & m見積番号
		
		cmd.CommandText = sql
		cmd.CommandType = ADODB.CommandTypeEnum.adCmdText
		
		cmd.Execute()
		Delete発注情報 = True
		
		'UPGRADE_NOTE: オブジェクト cmd をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
		cmd = Nothing
		
		On Error GoTo 0
		HourGlass(False)
		Exit Function
		
Delete発注情報_Err: 
		CriticalAlarm(Err.Number & " " & Err.Description)
		'UPGRADE_NOTE: オブジェクト cmd をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
		cmd = Nothing
		HourGlass(False)
	End Function
	'2013/04/09 ADD↑
	
	Private Sub cb端数値引計算_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cb端数値引計算.Click
		Dim w税込金額 As Decimal
		Dim w端数抜金額 As Decimal
		Dim w端数値引金額 As Decimal
		
		On Error GoTo keisan_err
		
		w税込金額 = CDec(rf_合計金額.Text) + CDec(HD_出精値引) + CDec(tx_外税額.Text)
		'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		w端数抜金額 = ISRoundDown(w税込金額, NullToZero(tx_端数値引桁数) * -1)
		
		w端数値引金額 = (w税込金額 - w端数抜金額) - (ISHasuu_rtn(HD_消費税端数, (w税込金額 - w端数抜金額) / (100 + gZEI) * gZEI, KIN_HASU))
		
		'マイナス表示
		[rf_端数値引金額].Text = VB6.Format(w端数値引金額 * -1, "#,##0" & KIN_FMT)
		
		On Error GoTo 0
		
		Exit Sub
		
keisan_err: 
		If Err.Number = 6 Then
			Beep()
			Inform("オーバーフローしました。")
			Err.Clear()
			''        PreviousControl.Undo
			PreviousControl.Focus()
			''''    Else
			''''        Beep
			''''        MsgBox "Error code = " & Err.Number & vbCrLf & Err.Description, vbCritical, Me.Caption
		End If
	End Sub
	
	'2015/10/19 ADD↓
	Private Sub cbWel1パー算出_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cbWel1パー算出.Click
		Dim fpSpd As Object
		Dim WkTBLS() As Object
		Dim i As Integer
		Dim Kei As Decimal
		
		Dim cSeihin As clsSeihin
		cSeihin = New clsSeihin
		cSeihin.isDo在庫管理 = clsSeihin.Type在庫管理.全て
		cSeihin.isDo廃盤表示 = clsSeihin.Type廃盤表示.廃盤表示しない
		
		'UPGRADE_WARNING: オブジェクト fpSpd.DataRowCnt の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		If fpSpd.DataRowCnt = 0 Then
			Exit Sub
		End If
		
		'UPGRADE_WARNING: 配列 WkTBLS の下限が 1,1 から 0,0 に変更されました。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="0F1C9BE1-AF9D-476E-83B1-17D43BECFF20"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト fpSpd.MaxCols の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		'UPGRADE_WARNING: オブジェクト fpSpd.DataRowCnt の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		ReDim WkTBLS(fpSpd.DataRowCnt, fpSpd.MaxCols) '大きさ変えて
		
		'UPGRADE_WARNING: オブジェクト fpSpd.GetArray の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		Call fpSpd.GetArray(1, 1, WkTBLS) '値をセット
		
		Kei = 0
		'Loop
		For i = 1 To UBound(WkTBLS)
			'2016/02/09 ADD↓
			With cSeihin
				.Initialize()
				'UPGRADE_WARNING: オブジェクト WkTBLS(i, Col製品NO) の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.[製品NO] = WkTBLS(i, Col製品NO)
				'UPGRADE_WARNING: オブジェクト WkTBLS(i, Col仕様NO) の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
				.[仕様NO] = WkTBLS(i, Col仕様NO)
				If .GetbyID = True Then
					If .[費用区分] = 0 Then '材料費
						'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
						Kei = Kei + SpcToNull(WkTBLS(i, Col金額), 0)
					End If
				Else
					'コードがない場合は材料費とする
					'UPGRADE_WARNING: オブジェクト SpcToNull() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
					Kei = Kei + SpcToNull(WkTBLS(i, Col金額), 0)
				End If
			End With
			'2016/02/09 ADD↑
			'''        If Not (WkTBLS(i, Col製品NO) Like "Z???") Then
			'''            Kei = Kei + SpcToNull(WkTBLS(i, Col金額), 0)
			'''        End If
		Next 
		
		'出精値引追加
		Kei = Kei + CDec(HD_出精値引)
		
		'''    [rf_1パー金額].Caption = Format$(Round(Kei * 0.01, 0), "#,##0")
		'UPGRADE_WARNING: オブジェクト NullToZero() の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		rf_1パー金額.Text = VB6.Format(System.Math.Round(Kei * NullToZero(([tx_Welパー算出].Text)) / 100, 0), "#,##0") '2017/01/17 ADD
		WriteIni("Disp", "WelPercent", ([tx_Welパー算出].Text), INIFile) '2017/01/17 ADD
	End Sub
	'2015/10/19 ADD↑
	
	'2023/09/26 ADD↓
	Private Function Getインボイス番号NGColor(ByRef siicd As Object) As String
		
		'UPGRADE_WARNING: オブジェクト siicd の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
		cSiiresaki.仕入先CD = siicd
		If cSiiresaki.GetbyID Then
			If cSiiresaki.インボイス登録番号 = "" Then
				Getインボイス番号NGColor = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red).ToString
			Else
				Getインボイス番号NGColor = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black).ToString
			End If
		Else
			Getインボイス番号NGColor = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red).ToString
		End If
	End Function
	'2023/09/26 ADD↑
End Class