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

Namespace com.bhivef.gomoku.ai

    Public Class JamesCook
        Inherits Player

        Protected Const WIN_COUNT As Integer = 5
        Protected lines As List(Of Line)

        Public Sub New(ByVal config As Config, ByVal board As Board, ByVal piece As Byte)
            MyBase.New(config, board, piece)
        End Sub

        Public Overrides Sub think(ByVal move As Move)
            If Not getReady() Then
                Return
            End If
            Dim cell As Cell = findBestMoves(board)
            If cell IsNot Nothing Then
                makeMove(New Move(cell.getRow(), cell.getColumn(), getPiece()))
            End If
        End Sub

        Protected Function getLines() As List(Of Line)
            If lines IsNot Nothing Then
                Return lines
            End If

            lines = New List(Of Line)

            For i As Integer = 0 To board.getHeight() - 1
                Dim line As New Line()
                For j As Integer = 0 To board.getWidth() - 1
                    line.add(i, j)
                Next
                lines.Add(line)
            Next
            For j As Integer = 0 To board.getWidth() - 1
                Dim line As New Line()
                For i As Integer = 0 To board.getHeight() - 1
                    line.add(i, j)
                Next
                lines.Add(line)
            Next
            For n As Integer = 0 To board.getHeight() - 1
                Dim i As Integer = n
                Dim j As Integer = 0
                Dim line As New Line()
                While i < board.getHeight() And j < board.getWidth()
                    line.add(i, j)
                    i += 1
                    j += 1
                End While
                lines.Add(line)
            Next
            For n As Integer = 1 To board.getWidth() - 1
                Dim i As Integer = 0
                Dim j As Integer = n
                Dim line As New Line()
                While i < board.getHeight() And j < board.getWidth()
                    line.add(i, j)
                    i += 1
                    j += 1
                End While
                lines.Add(line)
            Next
            For n As Integer = 0 To board.getHeight() - 1
                Dim i As Integer = n
                Dim j As Integer = board.getWidth() - 1
                Dim line As New Line()
                While i < board.getHeight() And j >= 0
                    line.add(i, j)
                    i += 1
                    j -= 1
                End While
                lines.Add(line)
            Next
            For n As Integer = board.getWidth() - 2 To 0 Step -1
                Dim i As Integer = 0
                Dim j As Integer = n
                Dim line As New Line()
                While i < board.getHeight() And j >= 0
                    line.add(i, j)
                    i += 1
                    j -= 1
                End While
                lines.Add(line)
            Next
            Return lines
        End Function

        Protected Function getBlanks() As List(Of Cell)
            Dim tag As New List(Of Cell)
            For i As Integer = 0 To board.getHeight() - 1
                For j As Integer = 0 To board.getWidth() - 1
                    If board.getPiece(i, j) = core.Board.BLANK Then
                        tag.Add(New Cell(i, j))
                    End If
                Next
            Next
            Return tag
        End Function

        Protected Function findBestMoves(ByVal board As Board) As Cell
            Dim computerMoves As New List(Of Cell)
            Dim humanMoves As New List(Of Cell)
            Dim computerCounter As Integer = findBestMoves(getPiece(), board, computerMoves)
            Dim humanCounter As Integer = findBestMoves(core.Board.opponentPiece(getPiece()), board, humanMoves)

            If humanMoves.Count = 0 Then
                If computerMoves.Count > 0 Then
                    Return computerMoves(0).clone()
                Else
                    Dim blanks As List(Of Cell) = getBlanks()
                    If blanks.Count = board.getWidth() * board.getHeight() Then
                        Dim row As Integer = CType(board.getHeight() / 2, Integer)
                        Dim column As Integer = CType(board.getWidth() / 2, Integer)
                        Return New Cell(row, column)
                    ElseIf blanks.Count > 0 Then
                        Return blanks(0).clone()
                    End If
                End If
            ElseIf computerMoves.Count = 0 Then
                Return humanMoves(0).clone()
            Else
                If humanCounter >= computerCounter Then
                    Return humanMoves(0).clone()
                Else
                    Return computerMoves(0).clone()
                End If
            End If

            Return Nothing
        End Function

        Protected Function findBestMoves(ByVal srcState As Integer, ByVal board As Board, ByVal tag As List(Of Cell)) As Integer
            Dim counter As Integer = 0
            Dim rows As New List(Of Row)
            Dim lines As List(Of Line) = getLines()

            For i As Integer = 0 To lines.Count - 1
                getRows(srcState, board, lines(i), rows)
            Next

            Dim maxCount As Integer = 0
            For i As Integer = rows.Count - 1 To 0 Step -1
                Dim row As Row = rows(i)
                If row.BlankStart.Count = 0 And row.BlankStop.Count = 0 Then
                    rows.RemoveAt(i)
                    Continue For
                End If
                If row.Cells.Count > WIN_COUNT Then
                    rows.RemoveAt(i)
                    Continue For
                End If
                If row.BlankStart.Count + row.BlankStop.Count + row.Cells.Count < WIN_COUNT Then
                    rows.RemoveAt(i)
                    Continue For
                End If
                If maxCount < row.Cells.Count Then
                    maxCount = row.Cells.Count
                End If
            Next
            counter = maxCount
            For i As Integer = rows.Count - 1 To 0 Step -1
                Dim row As Row = rows(i)
                If maxCount > row.Cells.Count Then
                    rows.RemoveAt(i)
                    Continue For
                End If
            Next
            For i As Integer = 0 To rows.Count - 1
                Dim row As Row = rows(i)
                If row.BlankStart.Count = 0 Or row.BlankStop.Count = 0 Then
                    Continue For
                End If
                If row.BlankStart.Count > 0 Then
                    tag.Add(row.BlankStart(row.BlankStart.Count - 1).clone())
                End If
                If row.BlankStop.Count > 0 Then
                    tag.Add(row.BlankStop(0).clone())
                End If
            Next
            If tag.Count = 0 Then
                For i As Integer = 0 To rows.Count - 1
                    Dim row As Row = rows(i)
                    If row.BlankStart.Count > 0 And row.BlankStop.Count > 0 Then
                        Continue For
                    End If
                    If row.BlankStart.Count > 0 Then
                        tag.Add(row.BlankStart(row.BlankStart.Count - 1).clone())
                    End If
                    If row.BlankStop.Count > 0 Then
                        tag.Add(row.BlankStop(0).clone())
                    End If
                Next
            End If

            Return counter
        End Function

        Protected Sub getRows(ByVal srcState As Integer, ByVal board As Board, ByVal line As Line, ByVal tag As List(Of Row))
            Dim row As New Row()
            Dim start As Boolean = False

            For i As Integer = 0 To line.size() - 1
                Dim tagState As Integer = board.getPiece(line.get(i).getRow(), line.get(i).getColumn())
                If start Then
                    If tagState = srcState Then
                        row.Cells.Add(line.get(i).clone())
                        If i = line.size() - 1 Then
                            tag.Add(row)
                            row = New Row()
                        End If
                    Else
                        For j As Integer = i To line.size() - 1
                            If board.getPiece(line.get(j).getRow(), line.get(j).getColumn()) = core.Board.BLANK Then
                                row.BlankStop.Add(line.get(j).clone())
                            Else
                                Exit For
                            End If
                        Next
                        tag.Add(row)
                        row = New Row()
                        If tagState = core.Board.BLANK Then
                            row.BlankStart.Add(line.get(i).clone())
                        End If
                        start = False
                    End If
                Else
                    If tagState = srcState Then
                        start = True
                        row.Cells.Add(line.get(i).clone())
                    ElseIf tagState = core.Board.BLANK Then
                        row.BlankStart.Add(line.get(i).clone())
                    Else
                        row = New Row()
                    End If
                End If
            Next
        End Sub

        Protected Class Row
            Public Cells As New List(Of Cell)
            Public BlankStart As New List(Of Cell)
            Public BlankStop As New List(Of Cell)

            Public Overrides Function ToString() As String
                Dim tag As String = ""
                tag &= "\r\nCells: "
                For i As Integer = 0 To Cells.Count - 1
                    tag &= Cells(i).ToString() & " "
                Next
                tag &= "\r\nBlankStart: "
                For i As Integer = 0 To BlankStart.Count - 1
                    tag &= BlankStart(i).ToString() & " "
                Next
                tag &= "\r\nBlankStop: "
                For i As Integer = 0 To BlankStop.Count - 1
                    tag &= BlankStop(i).ToString() & " "
                Next
                Return tag
            End Function
        End Class

        Protected Class Line
            Private cells As List(Of Cell)

            Public Sub New()
                cells = New List(Of Cell)
            End Sub

            Public Function size() As Integer
                Return cells.Count
            End Function

            Public Function [get](ByVal idx As Integer) As Cell
                Return cells(idx).clone()
            End Function

            Public Sub add(ByVal row As Integer, ByVal column As Integer)
                cells.Add(New Cell(row, column))
            End Sub

            Public Sub add(ByVal src As Cell)
                cells.Add(src.clone())
            End Sub

            Public Sub remove(ByVal idx As Integer)
                cells.RemoveAt(idx)
            End Sub

            Public Function indexOf(ByVal src As Cell) As Integer
                For i As Integer = 0 To cells.Count - 1
                    If cells(i).equals(src) Then
                        Return i
                    End If
                Next
                Return -1
            End Function

            Public Function indexOf(ByVal row As Integer, ByVal column As Integer) As Integer
                Return indexOf(New Cell(row, column))
            End Function
        End Class

    End Class

End Namespace