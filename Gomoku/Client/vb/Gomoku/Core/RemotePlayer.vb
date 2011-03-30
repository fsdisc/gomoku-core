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

Namespace com.bhivef.gomoku.core

    Public Class RemotePlayer
        Inherits Player

        Protected player As Player
        Protected client As Client

        Public Sub New(ByVal config As Config, ByVal client As Client, ByVal board As Board, ByVal piece As Byte)
            MyBase.New(config, board, piece)
            Me.player = Nothing
            Me.client = client
        End Sub

        Public Sub New(ByVal config As Config, ByVal client As Client, ByVal board As Board, ByVal player As Player)
            MyBase.New(config, board, player.getPiece())
            Me.player = player
            Me.client = client
            Me.player.addMoveListener(New MoveAdapter(Me))
        End Sub

        Public Overrides Sub think(ByVal move As Move)
            If player IsNot Nothing Then
                player.think(move)
                Return
            End If
            While Not disposed
                Dim lastMove As Move = client.lastMove()
                If lastMove.getPiece() = getPiece() Then
                    makeMove(lastMove)
                    Return
                End If
                Try
                    System.Threading.Thread.Sleep(1000)
                Catch
                End Try
            End While
        End Sub

        Public Overrides Sub dispose()
            MyBase.dispose()
            If player IsNot Nothing Then
                player.dispose()
            End If
        End Sub

        Public Overrides Sub setReady()
            MyBase.setReady()
            If player IsNot Nothing Then
                player.setReady()
            End If
        End Sub

        Public Overrides Sub setFinished()
            MyBase.setFinished()
            If player IsNot Nothing Then
                player.setFinished()
            End If
        End Sub

        Public Overrides Function checkReady() As Boolean
            Try
                Dim state As New GameState()
                client.gameState(state)
                Return state.Joined And Not state.Cancelled And Not state.Finished
            Catch
                Return False
            End Try
        End Function

        Private Class MoveAdapter
            Implements MoveListener

            Private player As RemotePlayer

            Public Sub New(ByVal player As RemotePlayer)
                Me.player = player
            End Sub

            Public Sub lastMove(ByVal victory As Byte) Implements MoveListener.lastMove
                player.fireLastMove(victory)
            End Sub

            Public Sub moveMade(ByVal move As Move) Implements MoveListener.moveMade
                If player.getPiece() <> move.getPiece() Then
                    Return
                End If
                Try
                    player.client.makeMove(move)
                    player.fireMoveMade(move)
                Catch
                End Try
            End Sub
        End Class

    End Class

End Namespace