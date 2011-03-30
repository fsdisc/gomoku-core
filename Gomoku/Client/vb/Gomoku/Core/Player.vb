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

    Public Class Player

        Protected config As Config
        Protected board As Board
        Private piece As Byte
        Private listeners As List(Of MoveListener)
        Private finished As Boolean
        Private ready As Boolean
        Protected disposed As Boolean

        Public Sub New(ByVal config As Config, ByVal board As Board, ByVal piece As Byte)
            Me.config = config
            Me.board = board
            Me.piece = piece
            Me.listeners = New List(Of MoveListener)
            Me.finished = False
            Me.ready = False
            Me.disposed = False
        End Sub

        Public Function getPiece() As Byte
            Return piece
        End Function

        Public Sub makeMove(ByVal move As Move)
            If Not ready Then
                Return
            End If
            If finished Then
                Return
            End If
            If piece <> move.getPiece() Then
                Return
            End If
            If piece <> board.getCurrentPiece() Then
                Return
            End If
            If move.getRow() >= board.getHeight() Or move.getColumn() >= board.getWidth() Then
                Return
            End If
            If board.getPiece(move.getRow(), move.getColumn()) <> board.BLANK Then
                Return
            End If
            board.setPiece(move.getRow(), move.getColumn(), piece)
            board.setCurrentPiece(core.Board.opponentPiece(piece))
            fireMoveMade(move)
            Dim victory As Byte = board.victory()
            Select Case victory
                Case core.Board.BLACK_WIN
                    fireLastMove(victory)
                Case core.Board.WHITE_WIN
                    fireLastMove(victory)
                Case core.Board.DRAW
                    fireLastMove(victory)
            End Select
        End Sub

        Public Overridable Sub think(ByVal move As Move)

        End Sub

        Public Function getReady() As Boolean
            Return ready
        End Function

        Public Overridable Sub setReady()
            ready = True
        End Sub

        Public Function getFinished() As Boolean
            Return finished
        End Function

        Public Overridable Sub setFinished()
            finished = True
        End Sub

        Public Overridable Function checkReady() As Boolean
            Return True
        End Function

        Public Overridable Sub dispose()
            disposed = True
            listeners.Clear()
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

        Protected Sub fireLastMove(ByVal victory As Byte)
            finished = True
            For i As Integer = 0 To listeners.Count - 1
                listeners(i).lastMove(victory)
            Next
        End Sub

    End Class

End Namespace