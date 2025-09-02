
Imports ADODB

''' <summary>
'''///////////////////////////
'''ログイン制御クラス
'''///////////////////////////
'''Ver.1.00           '2018.03.26     新設 
''' </summary>
Public Class clsLoginControl

    '変数
    Private m_LoginName As String
    Private m_AppTitle As String

    Private m_AppEnabled As Boolean
    Private m_AppUpDel As Boolean

    Private m_MaxLength As Integer '桁数
    Private m_EOF As Boolean '存在確認

    'Private m_Error As Integer 'エラー

    '//////////////////////////////////////
    '   LoginName
    '//////////////////////////////////////
    Public Property LoginName As String
        Get
            Return m_LoginName
        End Get
        Set(ByVal value As String)
            m_LoginName = value
        End Set
    End Property

    '//////////////////////////////////////
    '   AppTitle
    '//////////////////////////////////////
    Public Property AppTitle As String
        Get
            Return m_AppTitle
        End Get
        Set(ByVal value As String)
            m_AppTitle = value
        End Set
    End Property

    '//////////////////////////////////////
    '   AppEnabled
    '//////////////////////////////////////
    Public Property AppEnabled As Boolean
        Get
            Return m_AppEnabled
        End Get
        Set(ByVal value As Boolean)
            m_AppEnabled = value
        End Set
    End Property

    '//////////////////////////////////////
    '   AppUpDel
    '//////////////////////////////////////
    Public Property AppUpDel As Boolean
        Get
            Return m_AppUpDel
        End Get
        Set(ByVal value As Boolean)
            m_AppUpDel = value
        End Set
    End Property

    '//////////////////////////////////////
    '   MaxLength
    '//////////////////////////////////////
    Public Property MaxLength As Integer
        Get
            Return m_MaxLength
        End Get
        Set(ByVal value As Integer)
            m_MaxLength = value
        End Set
    End Property

    '//////////////////////////////////////
    '   存在確認
    '//////////////////////////////////////
    Public ReadOnly Property EOF As Boolean
        Get
            Return m_EOF
        End Get
    End Property

    '//////////////////////////////////////
    '   変数の初期化
    '//////////////////////////////////////
    Public Sub New()
        '初期化
        m_MaxLength = 10
        '初期化
        Initialize()
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    '//////////////////////////////////////
    '   クリアメソッド
    '//////////////////////////////////////
    Public Sub Initialize()
        '初期化
        '初期値を1桁多く設定する
        m_LoginName = New String(" "c, m_MaxLength + 1)
        m_AppTitle = New String(" "c, m_MaxLength + 1)
        m_AppEnabled = False
    End Sub

    '//////////////////////////////////////
    '   データを読み込むメソッド
    '//////////////////////////////////////
    Public Function GetbyID() As Boolean
        Dim rs As ADODB.Recordset = Nothing
        Dim sql As String

        Try
            ' SQL生成
            sql = "SELECT * FROM TMログイン制御" &
              " WHERE LoginName = '" & SQLString(Me.LoginName) & "'" &
              " AND AppTitle = '" & SQLString(Me.AppTitle) & "'"

            ' SQL実行
            rs = OpenRs(sql, Cn, ADODB.CursorTypeEnum.adOpenForwardOnly, ADODB.LockTypeEnum.adLockReadOnly)

            If rs.EOF Then
                GetbyID = False
                m_EOF = True
                Me.AppEnabled = False
                Me.AppUpDel = False
            Else
                GetbyID = True
                m_EOF = False

                Me.LoginName = rs.Fields("LoginName").Value.ToString()
                Me.AppTitle = rs.Fields("AppTitle").Value.ToString()
                Me.AppEnabled = Convert.ToBoolean(rs.Fields("AppEnabled").Value)
                Me.AppUpDel = Convert.ToBoolean(rs.Fields("AppUpDel").Value)
            End If
        Catch ex As Exception
            ' エラー時の処理
            Throw New ApplicationException(ex.Message)
        Finally
            ' リソースの解放
            If rs IsNot Nothing Then ReleaseRs(rs)
        End Try
    End Function

    '//////////////////////////////////////
    '   Upload
    '//////////////////////////////////////
    Public Function Upload() As Boolean
        Dim rs As ADODB.Recordset = Nothing
        Dim sql As String

        Try
            ' SQL生成
            sql = "SELECT * FROM TMログイン制御" &
              " WHERE LoginName = '" & SQLString(Me.LoginName) & "'" &
              " AND AppTitle = '" & SQLString(Me.AppTitle) & "'"

            ' SQL実行
            rs = OpenRs(sql, Cn, ADODB.CursorTypeEnum.adOpenKeyset, ADODB.LockTypeEnum.adLockOptimistic)

            If rs.EOF Then
                rs.AddNew()
                rs.Fields("LoginName").Value = Me.LoginName
                rs.Fields("AppTitle").Value = Me.AppTitle
                rs.Fields("初期登録日").Value = DateTime.Now
            End If

            rs.Fields("AppEnabled").Value = Me.AppEnabled
            rs.Fields("AppUpDel").Value = Me.AppUpDel
            rs.Fields("登録変更日").Value = DateTime.Now

            rs.Update()
            Upload = True
        Catch ex As Exception
            ' エラー時の処理
            Throw New ApplicationException(ex.Message)
        Finally
            ' リソースの解放
            If rs IsNot Nothing Then ReleaseRs(rs)
        End Try
    End Function

End Class
