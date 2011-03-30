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

Imports com.bhivef.gomoku.core
Imports com.bhivef.gomoku.ai

Namespace com.bhivef.gomoku.players

    Public Class Game
        Inherits com.bhivef.gomoku.core.Game

        Public Const JOHN_SMITH As Byte = 3
        Public Const JAMES_COOK As Byte = 4

        Public Sub New(ByVal config As Config, ByVal board As Board, ByVal boardUI As BoardUI, ByVal client As Client)
            MyBase.New(config, board, boardUI, client)
        End Sub

        Protected Overrides Function createUnknownPlayer(ByVal hasRemote As Boolean, ByVal type As Byte, ByVal piece As Byte) As Player
            If hasRemote Then
                If type = JOHN_SMITH Then
                    Return New RemotePlayer(config, client, board, New JohnSmith(config, board, piece))
                ElseIf type = JAMES_COOK Then
                    Return New RemotePlayer(config, client, board, New JamesCook(config, board, piece))
                Else
                    Return MyBase.createUnknownPlayer(hasRemote, type, piece)
                End If
            Else
                If type = JOHN_SMITH Then
                    Return New JohnSmith(config, board, piece)
                ElseIf type = JAMES_COOK Then
                    Return New JamesCook(config, board, piece)
                Else
                    Return MyBase.createUnknownPlayer(hasRemote, type, piece)
                End If
            End If
        End Function

    End Class

End Namespace