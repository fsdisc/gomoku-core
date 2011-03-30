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

    Public Class ComputerPlayer
        Inherits Player

        Protected MAXWIN As Integer
        Protected MINWIN As Integer

        Protected searchDepth As Integer
        Protected moveCount As Integer

        Public Sub New(ByVal config As Config, ByVal board As Board, ByVal piece As Byte)
            MyBase.New(config, board, piece)
            Me.searchDepth = config.getInt(config.SEARCH_DEPTH)
            Me.moveCount = 0
            Me.MAXWIN = getMaxWin()
            Me.MINWIN = getMinWin()
        End Sub

        Public Overrides Sub think(ByVal move As Move)
            If Not getReady() Then
                Return
            End If
            think(move, 0, board, getPiece(), MINWIN - 1, MAXWIN + 1)
            If move.getPiece() = core.Board.BLANK Then
                If moveCount = 0 Then
                    move.clone(New Move(CType(board.getHeight() / 2, Integer), CType(board.getWidth() / 2, Integer), getPiece()))
                    makeMove(move)
                    moveCount += 1
                End If
            Else
                makeMove(move)
                moveCount += 1
            End If
        End Sub

        Protected Overridable Function getMaxWin() As Integer
            Return 10000
        End Function

        Protected Overridable Function getMinWin() As Integer
            Return -10000
        End Function

        Protected Overridable Overloads Function think(ByVal move As Move, ByVal depth As Integer, ByVal board As Board, ByVal piece As Byte, ByVal alpha As Integer, ByVal beta As Integer) As Integer
            If depth = searchDepth Then
                move.clear()
                Return eval(board)
            End If

            Dim max As Integer = MINWIN - 1
            Dim min As Integer = MAXWIN + 1
            Dim nextMove As New Move()
            Dim moveVal As Integer = 0
            While nextPossible(nextMove, board, piece)
                Dim nextBoard As Board = board.clone()
                nextBoard.setPiece(nextMove.getRow(), nextMove.getColumn(), piece)
                Dim victory As Byte = nextBoard.victory()
                If victory = core.Board.BLACK_WIN Then
                    If getPiece() = core.Board.BLACK Then
                        moveVal = MAXWIN
                    Else
                        moveVal = MINWIN
                    End If
                ElseIf victory = core.Board.WHITE_WIN Then
                    If getPiece() = core.Board.WHITE Then
                        moveVal = MAXWIN
                    Else
                        moveVal = MINWIN
                    End If
                Else
                    moveVal = think(New Move(), depth + 1, nextBoard, core.Board.opponentPiece(piece), alpha, beta)
                End If
                If piece = getPiece() Then
                    If moveVal > max Then
                        move.clone(nextMove)
                        max = moveVal
                    End If
                    alpha = CType(IIf(alpha > moveVal, alpha, moveVal), Integer)
                    If alpha >= beta Then
                        Return beta
                    End If
                Else
                    If moveVal < min Then
                        move.clone(nextMove)
                        min = moveVal
                    End If
                    beta = CType(IIf(beta < moveVal, beta, moveVal), Integer)
                    If beta <= alpha Then
                        Return alpha
                    End If
                End If
            End While

            Return CType(IIf(piece = getPiece(), alpha, beta), Integer)
        End Function

        Protected Overridable Function nextPossible(ByVal move As Move, ByVal board As Board, ByVal piece As Byte) As Boolean
            Dim row As Integer = 0
            Dim column As Integer = 0

            If move.getPiece() <> core.Board.BLANK Then
                row = move.getRow()
                column = move.getColumn() + 1
                If column = board.getWidth() Then
                    row += 1
                    column = 0
                End If
            End If

            While row < board.getHeight()
                While column < board.getWidth()
                    If board.getPiece(row, column) = core.Board.BLANK And board.hasAdjacentPieces(row, column) Then
                        move.clone(New Move(row, column, piece))
                        Return True
                    End If
                    column += 1
                End While
                column = 0
                row += 1
            End While

            Return False
        End Function

        Protected Overridable Function eval(ByVal b As Board) As Integer
            Dim p As New Score(b, getPiece())
            Dim o As New Score(b, core.Board.opponentPiece(getPiece()))
            Dim retVal As Integer = 0

            If o.uncapped4 > 0 Then
                Return MINWIN
            End If
            If p.uncapped4 > 0 Then
                Return MAXWIN
            End If

            retVal += p.capped2 * 5
            retVal -= o.capped2 * 5

            retVal += p.uncapped2 * 10
            retVal -= o.uncapped2 * 10

            retVal += p.capped3 * 20
            retVal -= o.capped3 * 30

            retVal += p.uncapped3 * 100
            retVal -= o.uncapped3 * 120

            retVal += p.capped4 * 500
            retVal -= o.capped4 * 500

            Return Math.Max(MINWIN, Math.Min(MAXWIN, retVal))
        End Function

        Protected Class Score

            Public Sub New(ByVal board As Board, ByVal piece As Byte)
                uncapped2 = board.findRow(piece, 2, 0)
                capped2 = board.findRow(piece, 2, 1)

                uncapped3 = board.findRow(piece, 3, 0)
                capped3 = board.findRow(piece, 3, 1)

                uncapped4 = board.findRow(piece, 4, 0)
                capped4 = board.findRow(piece, 4, 1)
            End Sub

            Public capped2 As Integer
            Public uncapped2 As Integer

            Public capped3 As Integer
            Public uncapped3 As Integer

            Public capped4 As Integer
            Public uncapped4 As Integer

        End Class

    End Class

End Namespace