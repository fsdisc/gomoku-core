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

Namespace com.bhivef.gomoku.players

    Public Class Runner
        Inherits com.bhivef.gomoku.core.players.Runner

        Protected Overrides Sub createConfig()
            MyBase.createConfig()
            config.setValue(core.Config.SEARCH_DEPTH, 2)
            config.setValue(core.Config.SERVER_URL, "http://bhivef.com/gomoku/")
            config.setValue(core.Config.SERVER_EXTENSION, ".php")
        End Sub

        Protected Overrides Sub constructGame()
            game = New Game(config, board, boardUI, client)
        End Sub

        Protected Overrides Function getTitle() As String
            Return "Gomoku Player"
        End Function

        Protected Overrides Function getPlayerNames(ByVal newGame As Boolean) As String()
            If newGame Then
                Return New String() {"Human", "Computer", "Remote", "Computer (John Smith)", "Computer (James Cook)"}
            Else
                Return New String() {"Human", "Computer", "Computer (John Smith)", "Computer (James Cook)"}
            End If
        End Function

        Protected Overrides Function getPlayerIndex(ByVal newGame As Boolean, ByVal type As Byte) As Integer
            If newGame Then
                Select Case type
                    Case players.Game.JOHN_SMITH
                        Return 3
                    Case players.Game.JAMES_COOK
                        Return 4
                    Case Else
                        Return MyBase.getPlayerIndex(newGame, type)
                End Select
            Else
                Select Case type
                    Case players.Game.JOHN_SMITH
                        Return 2
                    Case players.Game.JAMES_COOK
                        Return 3
                    Case Else
                        Return MyBase.getPlayerIndex(newGame, type)
                End Select
            End If
        End Function

        Protected Overrides Function getPlayerType(ByVal newGame As Boolean, ByVal index As Integer) As Byte
            If newGame Then
                Dim type As Byte = MyBase.getPlayerType(newGame, index)
                Select Case index
                    Case 3
                        type = players.Game.JOHN_SMITH
                    Case 4
                        type = players.Game.JAMES_COOK
                End Select
                Return type
            Else
                Dim type As Byte = MyBase.getPlayerType(newGame, index)
                Select Case index
                    Case 2
                        type = players.Game.JOHN_SMITH
                    Case 3
                        type = players.Game.JAMES_COOK
                End Select
                Return type
            End If
        End Function

        Protected Overrides Function isRemotePlayer(ByVal index As Integer) As Boolean
            Return index = 2
        End Function

    End Class

End Namespace