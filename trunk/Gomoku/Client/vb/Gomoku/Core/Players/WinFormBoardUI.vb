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

Namespace com.bhivef.gomoku.core.players

    Public Class WinFormBoardUI
        Inherits BoardUI

        Private drawer As DrawerPanel
        Private graphics As Graphics
        Private ready As Boolean

        Public Sub New(ByVal board As Board)
            MyBase.New(board)
            Me.drawer = New DrawerPanel(Me)
            Me.ready = False
        End Sub

        Public Function getUI() As Panel
            Return drawer
        End Function

        Public Overrides Sub update()
            If Not ready Then
                Return
            End If
            Dim action As New MethodInvoker(AddressOf refreshDrawer)
            drawer.BeginInvoke(action)
        End Sub

        Private Sub refreshDrawer()
            drawer.Refresh()
        End Sub

        Public Sub setReady()
            ready = True
        End Sub

        Protected Overrides Sub drawLine(ByVal x1 As Integer, ByVal y1 As Integer, ByVal x2 As Integer, ByVal y2 As Integer)
            graphics.DrawLine(New Pen(Color.Black), x1, y1, x2, y2)
        End Sub

        Protected Overrides Sub drawText(ByVal x As Integer, ByVal y As Integer, ByVal text As String)
            graphics.DrawString(text, New Font("Arial", 10), New SolidBrush(Color.Black), x, y)
        End Sub

        Protected Overrides Sub drawOval(ByVal x As Integer, ByVal y As Integer, ByVal width As Integer, ByVal height As Integer)
            graphics.DrawEllipse(New Pen(Color.Black), x, y, width, height)
        End Sub

        Protected Overrides Sub fillOval(ByVal x As Integer, ByVal y As Integer, ByVal width As Integer, ByVal height As Integer)
            graphics.FillEllipse(New SolidBrush(Color.Black), x, y, width, height)
        End Sub

        Private Class DrawerPanel
            Inherits Panel

            Private boardUI As WinFormBoardUI

            Public Sub New(ByVal boardUI As WinFormBoardUI)
                Me.boardUI = boardUI
            End Sub

            Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
                MyBase.OnPaint(e)
                boardUI.graphics = e.Graphics
                boardUI.draw()
            End Sub

            Protected Overrides Sub OnMouseClick(ByVal e As System.Windows.Forms.MouseEventArgs)
                MyBase.OnMouseClick(e)
                Dim x As Integer = e.X - LEFT_MARGIN
                Dim y As Integer = e.Y - TOP_MARGIN
                If x < 0 Or y < 0 Then
                    Return
                End If
                x = CType(Math.Floor(x / core.BoardUI.CELL_WIDTH), Integer)
                y = CType(Math.Floor(y / core.BoardUI.CELL_HEIGHT), Integer)
                If x >= boardUI.board.getWidth() Or y >= boardUI.board.getHeight() Then
                    Return
                End If
                boardUI.fireMoveMade(New Move(y, x, boardUI.board.getCurrentPiece()))
            End Sub

        End Class

    End Class

End Namespace