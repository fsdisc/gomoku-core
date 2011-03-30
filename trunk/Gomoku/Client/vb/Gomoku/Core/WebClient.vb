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
Imports System.Text
Imports System.Net
Imports System.IO

Namespace com.bhivef.gomoku.core

    Public Class WebClient
        Inherits Client

        Private ready As Boolean
        Private session As String

        Public Sub New(ByVal config As Config)
            MyBase.New(config)
            Me.ready = False
        End Sub

        Public Overrides Sub login()
            If ready Then
                logout()
            End If

            Dim serverUrl As String = config.getString(config.SERVER_URL)
            Dim extension As String = config.getString(config.SERVER_EXTENSION)
            Dim username As String = config.getString(config.SERVER_USERNAME)
            Dim password As String = config.getPassword(config.SERVER_PASSWORD)
            Dim link As String = serverUrl & "api/login" & extension & "?u=" & username & "&p=" & password
            Dim response As String = request(link)
            Dim sign As String = "Success: "
            If response.StartsWith(sign) Then
                session = response.Substring(sign.Length)
                ready = True
                Return
            End If
            sign = "Error: "
            If response.StartsWith(sign) Then
                Throw New Exception("Login fail: " & response.Substring(sign.Length))
            End If
            Throw New Exception("Login fail: Invalid api call!")
        End Sub

        Public Overrides Sub logout()
            Try
                Dim serverUrl As String = config.getString(config.SERVER_URL)
                Dim extension As String = config.getString(config.SERVER_EXTENSION)
                Dim link As String = serverUrl & "api/logout" & extension & "?s=" & session
                request(link)
            Catch
            End Try
            ready = False
            session = ""
        End Sub

        Public Overrides Function online() As Boolean
            Return ready
        End Function

        Public Overrides Sub clone(ByVal session As String)
            Me.session = session
            Me.ready = True
        End Sub

        Public Overrides Function createGame(ByVal playFirst As Boolean, ByVal width As Integer, ByVal height As Integer) As String
            If Not ready Then
                login()
            End If

            Dim serverUrl As String = config.getString(config.SERVER_URL)
            Dim extension As String = config.getString(config.SERVER_EXTENSION)
            Dim link As String = serverUrl & "api/create" & extension & "?s=" & session & "&p=" & CType(IIf(playFirst, "1", "0"), String) & "&w=" & width & "&h=" & height
            Dim response As String = request(link)
            Dim sign As String = "Success: "
            If response.StartsWith(sign) Then
                Dim id As String = response.Substring(sign.Length)
                Return id
            End If
            sign = "Error: "
            If response.StartsWith(sign) Then
                Throw New Exception("Fail to create game: " & response.Substring(sign.Length))
            End If
            Throw New Exception("Fail to create game: Invalid api call!")
        End Function

        Public Overrides Sub joinGame(ByVal gameId As String)
            If Not ready Then
                login()
            End If

            Dim serverUrl As String = config.getString(config.SERVER_URL)
            Dim extension As String = config.getString(config.SERVER_EXTENSION)
            Dim link As String = serverUrl & "api/join" & extension & "?s=" & session & "&g=" & gameId
            Dim response As String = request(link)
            Dim sign As String = "Error: "
            If response.StartsWith(sign) Then
                Throw New Exception("Fail to join game: " & response.Substring(sign.Length))
            End If
            If Not response.Equals("Success") Then
                Throw New Exception("Fail to join game: Invalid api call!")
            End If
        End Sub

        Public Overrides Sub listGame(ByVal tag As System.Collections.Generic.List(Of String))
            Try
                tag.Clear()
                If Not ready Then
                    login()
                End If

                Dim serverUrl As String = config.getString(config.SERVER_URL)
                Dim extension As String = config.getString(config.SERVER_EXTENSION)
                Dim link As String = serverUrl & "api/list" & extension & "?s=" & session
                Dim response As String = request(link)
                Dim sign As String = "Success: "
                If response.StartsWith(sign) Then
                    response = response.Substring(sign.Length).Trim()
                    If response.Length > 0 Then
                        Dim lines() As String = response.Split(New Char() {"\n".Chars(0)})
                        For i As Integer = 0 To lines.Length - 1
                            Dim line As String = lines(i)
                            If line.EndsWith("\r") Then
                                line = line.Substring(0, line.Length - 1)
                            End If
                            tag.Add(line)
                        Next
                    End If
                End If
            Catch
            End Try
        End Sub

        Public Overrides Sub makeMove(ByVal move As Move)
            Dim serverUrl As String = config.getString(config.SERVER_URL)
            Dim gameId As String = config.getString(config.CURRENT_GAME)
            Dim extension As String = config.getString(config.SERVER_EXTENSION)
            Dim link As String = serverUrl & "api/move" & extension & "?s=" & session & "&g=" & gameId & "&r=" & move.getRow() & "&c=" & move.getColumn() & "&p=" & move.getPiece()
            Dim response As String = request(link)
            Dim sign As String = "Error: "
            If response.StartsWith(sign) Then
                Throw New Exception("Fail to make move: " & response.Substring(sign.Length))
            End If
            If Not response.Equals("Success") Then
                Throw New Exception("Fail to make move: Invalid api call!")
            End If
        End Sub

        Public Overrides Function lastMove() As Move
            Try
                If Not ready Then
                    login()
                End If

                Dim serverUrl As String = config.getString(config.SERVER_URL)
                Dim extension As String = config.getString(config.SERVER_EXTENSION)
                Dim gameId As String = config.getString(config.CURRENT_GAME)
                Dim link As String = serverUrl & "api/last" & extension & "?s=" & session & "&g=" & gameId
                Dim response As String = request(link)
                Dim sign As String = "Success: "
                If response.StartsWith(sign) Then
                    response = response.Substring(sign.Length).Trim()
                    If response.Length > 0 Then
                        Dim fields() As String = response.Split(New Char() {"|".Chars(0)})
                        Dim piece As Byte = Board.BLANK
                        If Not Byte.TryParse(fields(0), piece) Then
                            piece = Board.BLANK
                        End If
                        If piece <> Board.BLACK And piece <> Board.WHITE Then
                            Return New Move()
                        End If
                        Dim row As Integer = -1
                        If Not Integer.TryParse(fields(1), row) Then
                            row = -1
                        End If
                        If row < 0 Or row >= config.getInt(config.BOARD_HEIGHT) Then
                            Return New Move()
                        End If
                        Dim column As Integer = -1
                        If Not Integer.TryParse(fields(2), column) Then
                            column = -1
                        End If
                        If column < 0 Or column >= config.getInt(config.BOARD_WIDTH) Then
                            Return New Move()
                        End If
                        Return New Move(row, column, piece)
                    Else
                        Return New Move()
                    End If
                Else
                    Return New Move()
                End If
            Catch
                Return New Move()
            End Try
        End Function

        Public Overrides Sub gameState(ByVal state As GameState)
            If Not ready Then
                login()
            End If

            Dim serverUrl As String = config.getString(config.SERVER_URL)
            Dim extension As String = config.getString(config.SERVER_EXTENSION)
            Dim gameId As String = config.getString(config.CURRENT_GAME)
            Dim link As String = serverUrl & "api/state" & extension & "?s=" & session & "&g=" & gameId
            Dim response As String = request(link)
            Dim sign As String = "Success: "
            If response.StartsWith(sign) Then
                response = response.Substring(sign.Length)
                Dim fields() As String = response.Split(New Char() {"|".Chars(0)})
                state.Joined = fields(0).Equals("1")
                state.Cancelled = fields(1).Equals("1")
                state.Finished = fields(2).Equals("1")
                If Not Byte.TryParse(fields(3), state.Victory) Then
                    state.Victory = Board.NO_WIN
                End If
                Return
            End If
            sign = "Error: "
            If response.StartsWith(sign) Then
                Throw New Exception("Fail to get game state: " & response.Substring(sign.Length))
            End If
            Throw New Exception("Fail to get game state: Invalid api call!")
        End Sub

        Public Overrides Sub endGame(ByVal finished As Boolean, ByVal victory As Byte)
            If Not ready Then
                login()
            End If

            Dim serverUrl As String = config.getString(config.SERVER_URL)
            Dim extension As String = config.getString(config.SERVER_EXTENSION)
            Dim gameId As String = config.getString(config.CURRENT_GAME)
            Dim link As String = serverUrl & "api/end" & extension & "?s=" & session & "&g=" & gameId & "&f=" & CType(IIf(finished, "1", "0"), String) & "&v=" & victory.ToString()
            Dim response As String = request(link)
            Dim sign As String = "Error: "
            If response.StartsWith(sign) Then
                Throw New Exception("Fail to end game: " & response.Substring(sign.Length))
            End If
            If Not response.Equals("Success") Then
                Throw New Exception("Fail to end game: Invalid api call!")
            End If
        End Sub

        Protected Overridable Function request(ByVal link As String) As String
            Dim req As HttpWebRequest = CType(WebRequest.Create(link), HttpWebRequest)
            Dim response As HttpWebResponse = CType(req.GetResponse(), HttpWebResponse)
            Dim stream As Stream = response.GetResponseStream()

            Dim sb As New StringBuilder()
            Dim buf(8191) As Byte
            Dim count As Integer = 0
            Do
                count = stream.Read(buf, 0, buf.Length)
                If count <> 0 Then
                    sb.Append(Encoding.ASCII.GetString(buf, 0, count))
                End If
            Loop While count > 0
            Return sb.ToString()
        End Function

    End Class

End Namespace