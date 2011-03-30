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

    Public Class Board

        Public Const BLANK As Byte = 0
        Public Const BLACK As Byte = 1
        Public Const WHITE As Byte = 2

        Public Const GO_WIDTH As Byte = 19
        Public Const GO_HEIGHT As Byte = 19

        Public Const DRAW As Byte = 0
        Public Const BLACK_WIN As Byte = 1
        Public Const WHITE_WIN As Byte = 2
        Public Const NO_WIN As Byte = 3

        Private width As Byte
        Private height As Byte
        Private pieces(,) As Byte
        Private currentPiece As Byte

        Public Sub New(ByVal width As Integer, ByVal height As Integer)
            Me.width = CType(width, Byte)
            Me.height = CType(height, Byte)
            ReDim pieces(Me.height - 1, Me.width - 1)
            clear()
        End Sub

        Public Function getWidth() As Byte
            Return width
        End Function

        Public Function getHeight() As Byte
            Return height
        End Function

        Public Function getPiece(ByVal row As Integer, ByVal column As Integer) As Byte
            Return pieces(row, column)
        End Function

        Public Sub setPiece(ByVal row As Integer, ByVal column As Integer, ByVal piece As Byte)
            pieces(row, column) = piece
        End Sub

        Public Function getCurrentPiece() As Byte
            Return currentPiece
        End Function

        Public Sub setCurrentPiece(ByVal piece As Byte)
            currentPiece = piece
        End Sub

        Public Sub clear()
            For r As Integer = 0 To height - 1
                For c As Integer = 0 To width - 1
                    pieces(r, c) = BLANK
                Next
            Next
            currentPiece = BLANK
        End Sub

        Public Sub resize(ByVal width As Integer, ByVal height As Integer)
            Me.width = CType(width, Byte)
            Me.height = CType(height, Byte)
            ReDim pieces(Me.height - 1, Me.width - 1)
            clear()
        End Sub

        Public Function clone() As Board
            Dim tag As New Board(width, height)
            For r As Integer = 0 To height - 1
                For c As Integer = 0 To width - 1
                    tag.setPiece(r, c, pieces(r, c))
                Next
            Next
            Return tag
        End Function

        Public Shared Function opponentPiece(ByVal piece As Byte) As Byte
            Return CType(IIf(piece = BLACK, WHITE, BLACK), Byte)
        End Function

        Public Shared Function playerName(ByVal piece As Byte) As String
            Select Case piece
                Case BLACK
                    Return "Black"
                Case WHITE
                    Return "White"
            End Select
            Return ""
        End Function

        Public Function victory() As Byte
            Dim blankCount As Integer = CType(width, Integer) * CType(height, Integer)
            For r As Integer = 0 To height - 1
                For c As Integer = 0 To width - 1
                    If pieces(r, c) <> BLANK Then
                        blankCount -= 1
                        Select Case victory(r, c)
                            Case BLACK_WIN
                                Return BLACK_WIN
                            Case WHITE_WIN
                                Return WHITE_WIN
                        End Select
                    End If
                Next
            Next
            Return CType(IIf(blankCount = 0, DRAW, NO_WIN), Byte)
        End Function

        Public Function victory(ByVal row As Integer, ByVal column As Integer) As Byte
            Dim piece As Byte = pieces(row, column)
            If piece = BLANK Then
                Return NO_WIN
            End If
            If findRow(piece, row, column, 5, 2) > 0 Then
                Return CType(IIf(piece = BLACK, BLACK_WIN, WHITE_WIN), Byte)
            End If
            Return NO_WIN
        End Function

        Public Function hasAdjacentPieces(ByVal row As Integer, ByVal column As Integer) As Boolean
            Dim r As Integer = row - 1
            While r <= row + 1 And r >= 0 And r < height
                Dim c As Integer = column - 1
                While c <= column + 1 And c >= 0 And c < width
                    If r = row And c = column Then
                        c += 1
                        Continue While
                    End If
                    If pieces(r, c) <> BLANK Then
                        Return True
                    End If
                    c += 1
                End While
                r += 1
            End While
            Return False
        End Function

        Public Function findRow(ByVal piece As Byte, ByVal size As Integer, ByVal maxcaps As Integer) As Integer
            Dim count As Integer = 0
            For r As Integer = 0 To height - 1
                For c As Integer = 0 To width - 1
                    count += findRow(piece, r, c, size, maxcaps)
                Next
            Next
            Return count
        End Function

        Public Function findRow(ByVal piece As Byte, ByVal row As Integer, ByVal column As Integer, ByVal size As Integer, ByVal maxcaps As Integer) As Byte
            If pieces(row, column) <> piece Then
                Return 0
            End If

            Dim count As Byte = 0
            Dim opponentPiece As Byte = Board.opponentPiece(piece)
            Dim found As Boolean = False
            Dim c, r, capCount As Integer

            ' Check right
            If column + size <= width Then
                found = True
                c = column + 1
                While c < column + size And found
                    If pieces(row, c) <> piece Then
                        found = False
                    End If
                    c += 1
                End While
                If found AndAlso column > 0 AndAlso pieces(row, column - 1) = piece Then
                    found = False
                End If
                If found AndAlso column + size < width AndAlso pieces(row, column + size) = piece Then
                    found = False
                End If
                If found Then
                    capCount = 0
                    If column = 0 OrElse pieces(row, column - 1) = opponentPiece Then
                        capCount += 1
                    End If
                    If column + size = width OrElse pieces(row, column + size) = opponentPiece Then
                        capCount += 1
                    End If
                    If capCount <= maxcaps Then
                        count += CType(1, Byte)
                    End If
                End If
            End If

            ' Check down
            If row + size <= height Then
                found = True
                r = row + 1
                While r < row + size And found
                    If pieces(r, column) <> piece Then
                        found = False
                    End If
                    r += 1
                End While
                If found AndAlso row > 0 AndAlso pieces(row - 1, column) = piece Then
                    found = False
                End If
                If found AndAlso row + size < height AndAlso pieces(row + size, column) = piece Then
                    found = False
                End If
                If found Then
                    capCount = 0
                    If row = 0 OrElse pieces(row - 1, column) = opponentPiece Then
                        capCount += 1
                    End If
                    If row + size = height OrElse pieces(row + size, column) = opponentPiece Then
                        capCount += 1
                    End If
                    If capCount <= maxcaps Then
                        count += CType(1, Byte)
                    End If
                End If
            End If

            ' Check down-right
            If row + size <= height And column + size <= width Then
                found = True
                r = row + 1
                c = column + 1
                While r < row + size And c < column + size And found
                    If pieces(r, c) <> piece Then
                        found = False
                    End If
                    r += 1
                    c += 1
                End While
                If found AndAlso row > 0 AndAlso column > 0 AndAlso pieces(row - 1, column - 1) = piece Then
                    found = False
                End If
                If found AndAlso row + size < height AndAlso column + size < width AndAlso pieces(row + size, column + size) = piece Then
                    found = False
                End If
                If found Then
                    capCount = 0
                    If row = 0 OrElse column = 0 OrElse pieces(row - 1, column - 1) = opponentPiece Then
                        capCount += 1
                    End If
                    If row + size = height OrElse column + size = width OrElse pieces(row + size, column + size) = opponentPiece Then
                        capCount += 1
                    End If
                    If capCount <= maxcaps Then
                        count += CType(1, Byte)
                    End If
                End If
            End If

            ' Check down-left
            If row + size <= height And column - size >= -1 Then
                found = True
                r = row + 1
                c = column - 1
                While r < row + size And c >= 0 And found
                    If pieces(r, c) <> piece Then
                        found = False
                    End If
                    r += 1
                    c -= 1
                End While
                If found AndAlso row > 0 AndAlso column < width - 1 AndAlso pieces(row - 1, column + 1) = piece Then
                    found = False
                End If
                If found AndAlso row + size < height AndAlso column - size >= 0 AndAlso pieces(row + size, column - size) = piece Then
                    found = False
                End If
                If found Then
                    capCount = 0
                    If row = 0 OrElse column = width - 1 OrElse pieces(row - 1, column + 1) = opponentPiece Then
                        capCount += 1
                    End If
                    If row + size = height OrElse column - size = -1 OrElse pieces(row + size, column - size) = opponentPiece Then
                        capCount += 1
                    End If
                    If capCount <= maxcaps Then
                        count += CType(1, Byte)
                    End If
                End If
            End If

            Return count
        End Function

    End Class

End Namespace