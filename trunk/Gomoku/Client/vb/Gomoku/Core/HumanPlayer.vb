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

    Public Class HumanPlayer
        Inherits Player

        Protected boardUI As BoardUI

        Public Sub New(ByVal config As Config, ByVal board As Board, ByVal piece As Byte, ByVal boardUI As BoardUI)
            MyBase.New(config, board, piece)
            Me.boardUI = boardUI
            Me.boardUI.addMoveListener(New MoveAdapter(Me))
        End Sub

        Private Class MoveAdapter
            Implements MoveListener

            Private player As HumanPlayer

            Public Sub New(ByVal player As HumanPlayer)
                Me.player = player
            End Sub

            Public Sub lastMove(ByVal victory As Byte) Implements MoveListener.lastMove

            End Sub

            Public Sub moveMade(ByVal move As Move) Implements MoveListener.moveMade
                If player.getPiece() <> move.getPiece() Then
                    Return
                End If
                player.makeMove(move)
            End Sub
        End Class

    End Class

End Namespace