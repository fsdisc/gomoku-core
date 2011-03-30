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

    Public Class Client

        Protected config As Config

        Public Sub New(ByVal config As Config)
            Me.config = config
        End Sub

        Public Overridable Sub login()

        End Sub

        Public Overridable Sub logout()

        End Sub

        Public Overridable Function online() As Boolean
            Return False
        End Function

        Public Overridable Sub clone(ByVal session As String)

        End Sub

        Public Overridable Function createGame(ByVal playFirst As Boolean, ByVal width As Integer, ByVal height As Integer) As String
            Return ""
        End Function

        Public Overridable Sub joinGame(ByVal gameId As String)

        End Sub

        Public Overridable Sub listGame(ByVal tag As List(Of String))

        End Sub

        Public Overridable Function lastMove() As Move
            Return New Move()
        End Function

        Public Overridable Sub makeMove(ByVal move As Move)

        End Sub

        Public Overridable Sub gameState(ByVal state As GameState)

        End Sub

        Public Overridable Sub endGame(ByVal finished As Boolean, ByVal victory As Byte)

        End Sub

    End Class

End Namespace