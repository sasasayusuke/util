Option Strict On
Option Explicit On
Imports System.Net.NetworkInformation

''' <summary>
''' VB6 Obsolete Alternative Functions
''' </summary>
Public Module VB6Conv

    ''' <summary>
    ''' PixelsToTwipsX
    ''' </summary>
    ''' <param name="x"></param>
    ''' <returns></returns>
    Public Function PixelsToTwipsX(x As Integer) As Integer
        Using g As Graphics = Graphics.FromHwnd(IntPtr.Zero)
            Dim DpiX As Integer = CInt(g.DpiX)
            Return x * 1440 \ DpiX
        End Using
    End Function

    ''' <summary>
    ''' TwipsToPixelsX
    ''' </summary>
    ''' <param name="x"></param>
    ''' <returns></returns>
    Public Function TwipsToPixelsX(x As Integer) As Integer
        Using g As Graphics = Graphics.FromHwnd(IntPtr.Zero)
            Dim DpiX As Integer = CInt(g.DpiX)
            Return x * DpiX \ 1440
        End Using
    End Function

    ''' <summary>
    ''' PixelsToTwipsY
    ''' </summary>
    ''' <param name="y"></param>
    ''' <returns></returns>
    Public Function PixelsToTwipsY(y As Integer) As Integer
        Using g As Graphics = Graphics.FromHwnd(IntPtr.Zero)
            Dim dpiY As Integer = CInt(g.DpiY)
            Return y * 1440 \ dpiY
        End Using
    End Function

    ''' <summary>
    ''' TwipsToPixelsY
    ''' </summary>
    ''' <param name="y"></param>
    ''' <returns></returns>
    Public Function TwipsToPixelsY(y As Integer) As Integer
        Using g As Graphics = Graphics.FromHwnd(IntPtr.Zero)
            Dim dpiY As Integer = CInt(g.DpiY)
            Return y * dpiY \ 1440
        End Using
    End Function

    ''' <summary>
    ''' LenB代替え関数
    ''' </summary>
    ''' <param name="str"></param>
    ''' <returns></returns>
    Public Function LenB(ByVal str As String) As Integer
        'Shift JISに変換したときに必要なバイト数を返す
        Return System.Text.Encoding.GetEncoding(932).GetByteCount(str)
    End Function

    ''' <summary>
    ''' Left代替え関数
    ''' </summary>
    ''' <param name="str"></param>
    ''' <param name="length"></param>
    ''' <returns></returns>
    Public Function Left(ByVal str As String, ByVal length As Integer) As String
        Return str.Substring(0, length)
    End Function

    ''' <summary>
    ''' Right代替え関数
    ''' </summary>
    ''' <param name="str"></param>
    ''' <param name="length"></param>
    ''' <returns></returns>
    Public Function Right(ByVal str As String, ByVal length As Integer) As String
        Return str.Substring(str.Length - length)
    End Function

    ''' <summary>
    ''' Mid代替え関数
    ''' </summary>
    ''' <param name="str"></param>
    ''' <param name="Pos"></param>
    ''' <param name="length"></param>
    ''' <returns></returns>
    Public Function Mid(ByVal str As String, ByVal Pos As Integer, ByVal length As Integer) As String
        Dim position As Integer = Pos - 1
        Return str.Substring(position, length)
    End Function

    ''' <summary>
    ''' CopyArray代替え関数
    ''' </summary>
    ''' <param name="sourceArray"></param>
    ''' <returns></returns>
    Public Function CopyArray(sourceArray As Array) As Array
        Dim destinationArray As Array = Array.CreateInstance(sourceArray.GetType().GetElementType(), sourceArray.Length)
        Array.Copy(sourceArray, destinationArray, sourceArray.Length)
        Return destinationArray
    End Function

    ''' <summary>
    ''' FontChangeBold代替え関数
    ''' </summary>
    ''' <param name="currentFont"></param>
    ''' <param name="bold"></param>
    ''' <returns></returns>
    Public Function FontChangeBold(currentFont As Font, bold As Boolean) As Font
        Return New Font(currentFont.FontFamily, currentFont.Size, If(bold, FontStyle.Bold, FontStyle.Regular))
    End Function

    ''' <summary>
    ''' TextWidth 関数のVB.NET版
    ''' </summary>
    ''' <param name="text"></param>
    ''' <param name="font"></param>
    ''' <returns></returns>
    Public Function TextWidth(text As String, font As Font) As Integer
        Using g As Graphics = Graphics.FromHwnd(IntPtr.Zero)
            Dim size As SizeF = g.MeasureString(text, font)
            Return CInt(size.Width)
        End Using
    End Function

End Module
