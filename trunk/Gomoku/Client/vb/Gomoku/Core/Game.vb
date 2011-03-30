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

    Public Class Game

        Public Const HUMAN_PLAYER As Byte = 0
        Public Const COMPUTER_PLAYER As Byte = 1
        Public Const REMOTE_PLAYER As Byte = 2

        Protected config As Config
        Protected board As Board
        Protected boardUI As BoardUI
        Protected client As Client
        Protected firstPlayer As Player
        Protected secondPlayer As Player
        Protected disposed As Boolean
        Protected finished As Boolean
        Private listeners As List(Of MoveListener)

        Public Sub New(ByVal config As Config, ByVal board As Board, ByVal boardUI As BoardUI, ByVal client As Client)
            Me.config = config
            Me.board = board
            Me.boardUI = boardUI
            Me.client = client
            Me.board.clear()
            Me.board.setCurrentPiece(core.Board.BLACK)
            Me.boardUI.clearListeners()
            Me.listeners = New List(Of MoveListener)
            Me.disposed = False
            Me.finished = False
        End Sub

        Public Sub create()
            If hasRemotePlayer() Then
                Dim playFirst As Boolean = (config.getByte(config.FIRST_TYPE) <> REMOTE_PLAYER)
                Dim gameId As String = client.createGame(playFirst, config.getInt(config.BOARD_WIDTH), config.getInt(config.BOARD_HEIGHT))
                config.setValue(config.CURRENT_GAME, gameId)
            End If
            createPlayers()
            setup()
            checkReady()
        End Sub

        Public Sub join(ByVal gameId As String)
            If hasRemotePlayer() Then
                client.joinGame(gameId)
                config.setValue(config.CURRENT_GAME, gameId)
            End If
            createPlayers()
            setup()
            checkReady()
        End Sub

        Public Sub list(ByVal tag As List(Of String))
            client.listGame(tag)
        End Sub

        Public Sub dispose()
            disposed = True
            listeners.Clear()
            If firstPlayer IsNot Nothing Then
                firstPlayer.dispose()
            End If
            If secondPlayer IsNot Nothing Then
                secondPlayer.dispose()
            End If
            Try
                client.endGame(False, board.NO_WIN)
            Catch
            End Try
        End Sub

        Private Sub setup()
            firstPlayer.addMoveListener(New MoveAdapter(Me, True))
            secondPlayer.addMoveListener(New MoveAdapter(Me, False))
        End Sub

        Protected Function hasRemotePlayer() As Boolean
            Dim remote As Boolean = False
            Dim firstType As Byte = config.getByte(config.FIRST_TYPE)
            Dim secondType As Byte = config.getByte(config.SECOND_TYPE)
            If firstType = REMOTE_PLAYER And secondType = REMOTE_PLAYER Then
                remote = False
            ElseIf firstType = REMOTE_PLAYER Or secondType = REMOTE_PLAYER Then
                remote = True
            End If
            Return remote
        End Function

        Protected Sub createPlayers()
            Dim remote As Boolean = False
            Dim firstType As Byte = config.getByte(config.FIRST_TYPE)
            Dim secondType As Byte = config.getByte(config.SECOND_TYPE)
            If firstType = REMOTE_PLAYER And secondType = REMOTE_PLAYER Then
                remote = False
            ElseIf firstType = REMOTE_PLAYER Or secondType = REMOTE_PLAYER Then
                remote = True
            End If

            If remote Then
                If firstType = REMOTE_PLAYER Then
                    firstPlayer = New RemotePlayer(config, client, board, board.BLACK)
                ElseIf firstType = COMPUTER_PLAYER Then
                    firstPlayer = New RemotePlayer(config, client, board, New ComputerPlayer(config, board, board.BLACK))
                ElseIf firstType = HUMAN_PLAYER Then
                    firstPlayer = New RemotePlayer(config, client, board, New HumanPlayer(config, board, board.BLACK, boardUI))
                Else
                    firstPlayer = createUnknownPlayer(True, firstType, board.BLACK)
                End If
                If secondType = REMOTE_PLAYER Then
                    secondPlayer = New RemotePlayer(config, client, board, board.WHITE)
                ElseIf secondType = COMPUTER_PLAYER Then
                    secondPlayer = New RemotePlayer(config, client, board, New ComputerPlayer(config, board, board.WHITE))
                ElseIf secondType = HUMAN_PLAYER Then
                    secondPlayer = New RemotePlayer(config, client, board, New HumanPlayer(config, board, board.WHITE, boardUI))
                Else
                    secondPlayer = createUnknownPlayer(True, secondType, board.WHITE)
                End If
            Else
                If firstType = COMPUTER_PLAYER Then
                    firstPlayer = New ComputerPlayer(config, board, board.BLACK)
                ElseIf firstType = HUMAN_PLAYER Then
                    firstPlayer = New HumanPlayer(config, board, board.BLACK, boardUI)
                Else
                    firstPlayer = createUnknownPlayer(False, firstType, board.BLACK)
                End If
                If secondType = COMPUTER_PLAYER Then
                    secondPlayer = New ComputerPlayer(config, board, board.WHITE)
                ElseIf secondType = HUMAN_PLAYER Then
                    secondPlayer = New HumanPlayer(config, board, board.WHITE, boardUI)
                Else
                    secondPlayer = createUnknownPlayer(False, secondType, board.WHITE)
                End If
            End If
        End Sub

        Protected Overridable Function createUnknownPlayer(ByVal hasRemote As Boolean, ByVal type As Byte, ByVal piece As Byte) As Player
            If hasRemote Then
                Return New RemotePlayer(config, client, board, New HumanPlayer(config, board, piece, boardUI))
            Else
                Return New HumanPlayer(config, board, piece, boardUI)
            End If
        End Function

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
            For i As Integer = 0 To listeners.Count - 1
                listeners(i).lastMove(victory)
            Next
        End Sub

        Protected Sub firstThink()
            Dim runner As New FirstThread(Me)
            Dim thread As New System.Threading.Thread(New System.Threading.ThreadStart(AddressOf runner.run))
            thread.Start()
        End Sub

        Protected Sub secondThink()
            Dim runner As New SecondThread(Me)
            Dim thread As New System.Threading.Thread(New System.Threading.ThreadStart(AddressOf runner.run))
            thread.Start()
        End Sub

        Protected Sub checkReady()
            Dim runner As New ReadyThread(Me)
            Dim thread As New System.Threading.Thread(New System.Threading.ThreadStart(AddressOf runner.run))
            thread.Start()
        End Sub

        Private Class FirstThread

            Private game As Game

            Public Sub New(ByVal game As Game)
                Me.game = game
            End Sub

            Public Sub run()
                game.firstPlayer.think(New Move())
            End Sub

        End Class

        Private Class SecondThread

            Private game As Game

            Public Sub New(ByVal game As Game)
                Me.game = game
            End Sub

            Public Sub run()
                game.secondPlayer.think(New Move())
            End Sub

        End Class

        Private Class ReadyThread

            Private game As Game

            Public Sub New(ByVal game As Game)
                Me.game = game
            End Sub

            Public Sub run()
                Dim stopIt As Boolean = False
                While Not stopIt And Not game.disposed
                    stopIt = game.firstPlayer.checkReady() And game.secondPlayer.checkReady()
                    If stopIt Then
                        Exit While
                    End If
                    Try
                        System.Threading.Thread.Sleep(1000)
                    Catch
                    End Try
                End While
                game.firstPlayer.setReady()
                game.secondPlayer.setReady()
                game.firstThink()
            End Sub

        End Class

        Private Class MoveAdapter
            Implements MoveListener

            Private game As Game
            Private first As Boolean

            Public Sub New(ByVal game As Game, ByVal first As Boolean)
                Me.game = game
                Me.first = first
            End Sub

            Public Sub lastMove(ByVal victory As Byte) Implements MoveListener.lastMove
                game.finished = True
                If first Then
                    game.secondPlayer.setFinished()
                Else
                    game.firstPlayer.setFinished()
                End If
                game.fireLastMove(victory)
                Try
                    game.client.endGame(True, victory)
                Catch
                End Try
            End Sub

            Public Sub moveMade(ByVal move As Move) Implements MoveListener.moveMade
                game.boardUI.update()
                game.fireMoveMade(move)
                If first Then
                    game.secondThink()
                Else
                    game.firstThink()
                End If
            End Sub

        End Class

    End Class

End Namespace