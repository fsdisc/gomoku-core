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

    Public Class Cell

        Private row As Byte
        Private column As Byte

        Public Sub New()
            Me.row = 0
            Me.column = 0
        End Sub

        Public Sub New(ByVal row As Integer, ByVal column As Integer)
            Me.row = CType(row, Byte)
            Me.column = CType(column, Byte)
        End Sub

        Public Function getRow() As Byte
            Return row
        End Function

        Public Function getColumn() As Byte
            Return column
        End Function

        Public Shadows Function equals(ByVal cell As Cell) As Boolean
            Return row = cell.getRow() And column = cell.getColumn()
        End Function

        Public Function clone() As Cell
            Return New Cell(row, column)
        End Function

        Public Sub clone(ByVal cell As Cell)
            row = cell.getRow()
            column = cell.getColumn()
        End Sub

        Public Overridable Sub clear()
            row = 0
            column = 0
        End Sub

        Public Overrides Function ToString() As String
            Return "(" & row.ToString() & ", " & column.ToString() & ")"
        End Function

    End Class

End Namespace