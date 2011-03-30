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

    Public Class JohnSmith
        Inherits ComputerPlayer

        Public Sub New(ByVal config As Config, ByVal board As Board, ByVal piece As Byte)
            MyBase.New(config, board, piece)
        End Sub


        Protected Overrides Function getMaxWin() As Integer
            Return 100000
        End Function

        Protected Overrides Function getMinWin() As Integer
            Return -100000
        End Function


        Protected Overrides Function eval(ByVal b As Board) As Integer
            Dim p As New Score(b, getPiece())
            Dim o As New Score(b, core.Board.opponentPiece(getPiece()))
            Dim retVal As Integer = 0

            If o.uncapped4 > 0 Then
                Return MINWIN
            End If
            If p.uncapped4 > 0 Then
                Return MAXWIN
            End If

            retVal += p.capped2 * 1
            retVal -= o.capped2 * 5

            retVal += p.uncapped2 * 10
            retVal -= o.uncapped2 * 10

            retVal += p.capped3 * 100
            retVal -= o.capped3 * 100

            retVal += p.uncapped3 * 1000
            retVal -= o.uncapped3 * 1000

            retVal += p.capped4 * 10000
            retVal -= o.capped4 * 10000

            Return Math.Max(MINWIN, Math.Min(MAXWIN, retVal))
        End Function

    End Class

End Namespace