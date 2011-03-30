'
'  Gomoku Core
' 
'  Copyright (c) 2011 Tran Dinh Thoai <dthoai@yahoo.com>
'
' This library is free software; you can redistribute it and/or
' modify it under the terms of the GNU General Public License
' version 3.
'
' This library is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY; without even the implied warranty of
' MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
' General Public License for more details.
'
' You should have received a copy of the GNU General Public License
' along with this library; if not, write to the Free Software
' Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
'

Option Explicit On
Option Strict On
Imports System.Security.Cryptography
Imports System.IO
Imports System.Text

Namespace com.bhivef.gomoku.core

    Public Class Config

        Public Const BOOLEAN_DEFAULT As Boolean = False
        Public Const DOUBLE_DEFAULT As Double = 0
        Public Const FLOAT_DEFAULT As Single = 0
        Public Const INT_DEFAULT As Integer = 0
        Public Const LONG_DEFAULT As Long = 0
        Public Const BYTE_DEFAULT As Byte = 0
        Public Const STRING_DEFAULT As String = ""
        Public Const PASSWORD_DEFAULT As String = ""

        Public Const [TRUE] As String = "true"
        Public Const [FALSE] As String = "false"

        Public Const SERVER_URL As String = "server.url"
        Public Const SERVER_USERNAME As String = "server.username"
        Public Const SERVER_PASSWORD As String = "server.password"
        Public Const SERVER_EXTENSION As String = "server.extension"
        Public Const SEARCH_DEPTH As String = "search.depth"
        Public Const FIRST_TYPE As String = "type.first"
        Public Const SECOND_TYPE As String = "type.second"
        Public Const BOARD_WIDTH As String = "board.width"
        Public Const BOARD_HEIGHT As String = "board.height"
        Public Const CURRENT_GAME As String = "game.current"

        Private Const KEY As String = "968AD25B"

        Private properties As Dictionary(Of String, String)

        Public Sub New()
            Me.properties = New Dictionary(Of String, String)
        End Sub

        Private Function getProperty(ByVal name As String) As String
            Dim value As String = ""
            If Not properties.TryGetValue(name, value) Then
                value = ""
            End If
            Return value
        End Function

        Private Sub setProperty(ByVal name As String, ByVal value As String)
            If properties.ContainsKey(name) Then
                properties(name) = value
            Else
                properties.Add(name, value)
            End If
        End Sub

        Public Function getBoolean(ByVal name As String) As Boolean
            Dim value As String = getProperty(name)
            If value.Length = 0 Then
                Return BOOLEAN_DEFAULT
            End If
            If value.Equals(Config.TRUE) Then
                Return True
            End If
            Return False
        End Function

        Public Function getDouble(ByVal name As String) As Double
            Dim value As String = getProperty(name)
            If value.Length = 0 Then
                Return DOUBLE_DEFAULT
            End If
            Dim ival As Double = DOUBLE_DEFAULT
            If Not Double.TryParse(value, ival) Then
                ival = DOUBLE_DEFAULT
            End If
            Return ival
        End Function

        Public Function getFloat(ByVal name As String) As Single
            Dim value As String = getProperty(name)
            If value.Length = 0 Then
                Return FLOAT_DEFAULT
            End If
            Dim ival As Single = FLOAT_DEFAULT
            If Not Single.TryParse(value, ival) Then
                ival = FLOAT_DEFAULT
            End If
            Return ival
        End Function

        Public Function getInt(ByVal name As String) As Integer
            Dim value As String = getProperty(name)
            If value.Length = 0 Then
                Return INT_DEFAULT
            End If
            Dim ival As Integer = INT_DEFAULT
            If Not Integer.TryParse(value, ival) Then
                ival = INT_DEFAULT
            End If
            Return ival
        End Function

        Public Function getByte(ByVal name As String) As Byte
            Dim value As String = getProperty(name)
            If value.Length = 0 Then
                Return BYTE_DEFAULT
            End If
            Dim ival As Byte = BYTE_DEFAULT
            If Not Byte.TryParse(value, ival) Then
                ival = BYTE_DEFAULT
            End If
            Return ival
        End Function

        Public Function getLong(ByVal name As String) As Long
            Dim value As String = getProperty(name)
            If value.Length = 0 Then
                Return LONG_DEFAULT
            End If
            Dim ival As Long = LONG_DEFAULT
            If Not Long.TryParse(value, ival) Then
                ival = LONG_DEFAULT
            End If
            Return ival
        End Function

        Public Function getString(ByVal name As String) As String
            Dim value As String = getProperty(name)
            If value.Length = 0 Then
                Return STRING_DEFAULT
            End If
            Return value
        End Function

        Public Function getPassword(ByVal name As String) As String
            Dim src As String = getString(name)
            Dim tag As String = PASSWORD_DEFAULT
            Try
                tag = decrypt(src, KEY)
            Catch
                tag = PASSWORD_DEFAULT
            End Try
            Return tag
        End Function

        Public Function getDate(ByVal name As String) As Date
            Dim ival As Date = Date.Now
            Dim value As String = getProperty(name)
            If value.Length = 0 Then
                Return ival
            End If
            If Not Date.TryParse(value, ival) Then
                ival = Date.Now
            End If
            Return ival
        End Function

        Public Sub setValue(ByVal name As String, ByVal value As Double)
            Dim oldValue As Double = getDouble(name)
            If (oldValue <> value) Then
                setProperty(name, value.ToString())
            End If
        End Sub

        Public Sub setValue(ByVal name As String, ByVal value As Single)
            Dim oldValue As Single = getFloat(name)
            If oldValue <> value Then
                setProperty(name, value.ToString())
            End If
        End Sub

        Public Sub setValue(ByVal name As String, ByVal value As Integer)
            Dim oldValue As Integer = getInt(name)
            If oldValue <> value Then
                setProperty(name, value.ToString())
            End If
        End Sub

        Public Sub setValue(ByVal name As String, ByVal value As Byte)
            Dim oldValue As Byte = getByte(name)
            If oldValue <> value Then
                setProperty(name, value.ToString())
            End If
        End Sub

        Public Sub setValue(ByVal name As String, ByVal value As Long)
            Dim oldValue As Long = getLong(name)
            If oldValue <> value Then
                setProperty(name, value.ToString())
            End If
        End Sub

        Public Sub setValue(ByVal name As String, ByVal value As String)
            Dim oldValue As String = getString(name)
            If Not oldValue.Equals(value) Then
                If value IsNot Nothing Then
                    setProperty(name, value)
                End If
            End If
        End Sub

        Public Sub setPassword(ByVal name As String, ByVal value As String)
            Dim oldValue As String = getPassword(name)
            If Not oldValue.Equals(value) Then
                If value IsNot Nothing Then
                    Try
                        setProperty(name, encrypt(value, KEY))
                    Catch
                    End Try
                End If
            End If
        End Sub

        Public Sub setValue(ByVal name As String, ByVal value As Boolean)
            Dim oldValue As Boolean = getBoolean(name)
            If oldValue <> value Then
                setProperty(name, CType(IIf(value, Config.TRUE, Config.FALSE), String))
            End If
        End Sub

        Public Sub setValue(ByVal name As String, ByVal value As Date)
            Dim oldValue As Date = getDate(name)
            If Not oldValue.Equals(value) Then
                setProperty(name, value.ToString())
            End If
        End Sub

        Public Sub save(ByVal filename As String)
            Try
                Dim lines(properties.Count - 1) As String
                Dim idx As Integer = 0
                For Each key As String In properties.Keys
                    lines(idx) = key & "=" & getProperty(key)
                Next
                File.WriteAllLines(filename, lines)
            Catch
            End Try
        End Sub

        Public Sub load(ByVal filename As String)
            Try
                properties = New Dictionary(Of String, String)
                Dim lines() As String = File.ReadAllLines(filename)
                For idx As Integer = 0 To lines.Length - 1
                    Dim pos As Integer = lines(idx).IndexOf("=")
                    If pos >= 0 Then
                        Dim key As String = lines(idx).Substring(0, pos)
                        Dim value As String = lines(idx).Substring(pos + 1)
                        setProperty(key, value)
                    End If
                Next
            Catch
            End Try
        End Sub

        Public Shared Function encrypt(ByVal value As String, ByVal key As String) As String
            Dim bytes() As Byte = ASCIIEncoding.ASCII.GetBytes(key)
            Dim provider As New DESCryptoServiceProvider()
            Dim memory As New MemoryStream()
            Dim crypto As New CryptoStream(memory, provider.CreateEncryptor(bytes, bytes), CryptoStreamMode.Write)
            Dim writer As New StreamWriter(crypto)
            writer.Write(value)
            writer.Flush()
            crypto.FlushFinalBlock()
            writer.Flush()
            Return Convert.ToBase64String(memory.GetBuffer(), 0, CType(memory.Length, Integer))
        End Function

        Public Shared Function decrypt(ByVal value As String, ByVal key As String) As String
            Dim bytes() As Byte = ASCIIEncoding.ASCII.GetBytes(key)
            Dim provider As New DESCryptoServiceProvider()
            Dim memory As New MemoryStream(Convert.FromBase64String(value))
            Dim crypto As New CryptoStream(memory, provider.CreateDecryptor(bytes, bytes), CryptoStreamMode.Read)
            Dim reader As New StreamReader(crypto)
            Return reader.ReadToEnd()
        End Function

    End Class

End Namespace