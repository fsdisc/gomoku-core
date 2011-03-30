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

    Public Class BoardUI

        Public Const LEFT_MARGIN As Integer = 20
        Public Const TOP_MARGIN As Integer = 20
        Public Const CELL_WIDTH As Integer = 20
        Public Const CELL_HEIGHT As Integer = 20
        Public Const PIECE_WIDTH As Integer = 14
        Public Const PIECE_HEIGHT As Integer = 14

        Protected board As Board
        Private listeners As List(Of MoveListener)

        Public Sub New(ByVal board As Board)
            Me.board = board
            Me.listeners = New List(Of MoveListener)
        End Sub

        Public Overridable Sub update()

        End Sub

        Protected Overridable Sub draw()
            For r As Integer = 0 To board.getHeight()
                drawLine(LEFT_MARGIN, TOP_MARGIN + r * CELL_HEIGHT, LEFT_MARGIN + board.getWidth() * CELL_WIDTH, TOP_MARGIN + r * CELL_HEIGHT)
            Next
            For c As Integer = 0 To board.getWidth()
                drawLine(LEFT_MARGIN + c * CELL_WIDTH, TOP_MARGIN, LEFT_MARGIN + c * CELL_WIDTH, TOP_MARGIN + board.getHeight() * CELL_HEIGHT)
            Next
            For r As Integer = 0 To board.getHeight() - 1
                drawText(0, TOP_MARGIN + r * CELL_HEIGHT, r.ToString())
            Next
            For c As Integer = 0 To board.getWidth() - 1
                drawText(LEFT_MARGIN + c * CELL_WIDTH, 0, c.ToString())
            Next
            For r As Integer = 0 To board.getHeight() - 1
                For c As Integer = 0 To board.getWidth() - 1
                    Dim piece As Byte = board.getPiece(r, c)
                    If piece = board.WHITE Then
                        drawOval(CType(LEFT_MARGIN + c * CELL_WIDTH + (CELL_WIDTH - PIECE_WIDTH) / 2, Integer), CType(TOP_MARGIN + r * CELL_HEIGHT + (CELL_HEIGHT - PIECE_HEIGHT) / 2, Integer), PIECE_WIDTH, PIECE_HEIGHT)
                    End If
                    If piece = board.BLACK Then
                        drawOval(CType(LEFT_MARGIN + c * CELL_WIDTH + (CELL_WIDTH - PIECE_WIDTH) / 2, Integer), CType(TOP_MARGIN + r * CELL_HEIGHT + (CELL_HEIGHT - PIECE_HEIGHT) / 2, Integer), PIECE_WIDTH, PIECE_HEIGHT)
                        fillOval(CType(LEFT_MARGIN + c * CELL_WIDTH + (CELL_WIDTH - PIECE_WIDTH) / 2, Integer), CType(TOP_MARGIN + r * CELL_HEIGHT + (CELL_HEIGHT - PIECE_HEIGHT) / 2, Integer), PIECE_WIDTH, PIECE_HEIGHT)
                    End If
                Next
            Next

        End Sub

        Protected Overridable Sub drawLine(ByVal x1 As Integer, ByVal y1 As Integer, ByVal x2 As Integer, ByVal y2 As Integer)

        End Sub

        Protected Overridable Sub drawText(ByVal x As Integer, ByVal y As Integer, ByVal text As String)

        End Sub

        Protected Overridable Sub drawOval(ByVal x As Integer, ByVal y As Integer, ByVal width As Integer, ByVal height As Integer)

        End Sub

        Protected Overridable Sub fillOval(ByVal x As Integer, ByVal y As Integer, ByVal width As Integer, ByVal height As Integer)

        End Sub

        Public Sub addMoveListener(ByVal src As MoveListener)
            listeners.Add(src)
        End Sub

        Public Sub clearListeners()
            listeners.Clear()
        End Sub

        Protected Sub fireMoveMade(ByVal move As Move)
            For i As Integer = 0 To listeners.Count - 1
                listeners(i).moveMade(move)
            Next
        End Sub

    End Class

End Namespace