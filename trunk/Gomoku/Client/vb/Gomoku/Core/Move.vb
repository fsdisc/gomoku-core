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

    Public Class Move
        Inherits Cell

        Private piece As Byte

        Public Sub New()
            MyBase.New()
            Me.piece = Board.BLANK
        End Sub

        Public Sub New(ByVal row As Integer, ByVal column As Integer, ByVal piece As Byte)
            MyBase.New(row, column)
            Me.piece = piece
        End Sub

        Public Function getPiece() As Byte
            Return piece
        End Function

        Public Overloads Function equals(ByVal move As Move) As Boolean
            Return getRow() = move.getRow() And getColumn() = move.getColumn() And piece = move.getPiece()
        End Function

        Public Overloads Function clone() As Move
            Return New Move(getRow(), getColumn(), piece)
        End Function

        Public Overloads Sub clone(ByVal move As Move)
            MyBase.clone(move)
            piece = move.getPiece()
        End Sub

        Public Overrides Sub clear()
            MyBase.clear()
            piece = Board.BLANK
        End Sub

        Public Overrides Function ToString() As String
            Return Board.playerName(piece) & " " & MyBase.ToString()
        End Function

    End Class

End Namespace