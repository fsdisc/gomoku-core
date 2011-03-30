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

    Public Class Runner

        Protected config As Config
        Protected board As Board
        Protected boardUI As BoardUI
        Protected client As Client
        Protected game As Game
        Protected history As List(Of Move)
        Protected finished As Boolean

        Public Overridable Sub start()
            setup()
            Try
                newGame()
            Catch
            End Try
        End Sub

        Protected Overridable Sub setup()
            createConfig()
            createBoard()
            createBoardUI()
            createClient()
            createGame()
            createMainUI()
        End Sub

        Protected Overridable Sub createConfig()
            config = New Config()
            config.setValue(config.SEARCH_DEPTH, 2)
            config.setValue(config.BOARD_WIDTH, board.GO_WIDTH)
            config.setValue(config.BOARD_HEIGHT, board.GO_HEIGHT)
            config.setValue(config.FIRST_TYPE, game.HUMAN_PLAYER)
            config.setValue(config.SECOND_TYPE, game.HUMAN_PLAYER)
            config.setValue(config.SERVER_URL, "http://bhivef.com/gomoku/")
            config.setValue(config.SERVER_EXTENSION, ".php")
        End Sub

        Protected Overridable Sub createBoard()
            board = New Board(config.getByte(config.BOARD_WIDTH), config.getByte(config.BOARD_HEIGHT))
        End Sub

        Protected Overridable Sub createBoardUI()

        End Sub

        Protected Overridable Sub createClient()
            client = New WebClient(config)
        End Sub

        Protected Overridable Sub createGame()
            finished = False
            history = New List(Of Move)
            board.resize(config.getByte(config.BOARD_WIDTH), config.getByte(config.BOARD_HEIGHT))
            If game IsNot Nothing Then
                game.dispose()
            End If
            constructGame()
            game.addMoveListener(New MoveAdapter(Me))
        End Sub

        Protected Overridable Sub constructGame()
            game = New Game(config, board, boardUI, client)
        End Sub

        Protected Overridable Sub createMainUI()

        End Sub

        Protected Overridable Sub blackWin()

        End Sub

        Protected Overridable Sub whiteWin()

        End Sub

        Protected Overridable Sub drawEnd()

        End Sub

        Protected Overridable Sub newGame()
            game.create()
        End Sub

        Protected Overridable Sub joinGame(ByVal gameId As String)
            game.join(gameId)
        End Sub

        Private Class MoveAdapter
            Implements MoveListener

            Private runner As Runner

            Public Sub New(ByVal runner As Runner)
                Me.runner = runner
            End Sub

            Public Sub lastMove(ByVal victory As Byte) Implements MoveListener.lastMove
                If runner.finished Then
                    Return
                End If
                runner.finished = True
                Select Case victory
                    Case core.Board.DRAW
                        runner.drawEnd()
                    Case core.Board.BLACK_WIN
                        runner.blackWin()
                    Case core.Board.WHITE_WIN
                        runner.whiteWin()
                End Select
            End Sub

            Public Sub moveMade(ByVal move As Move) Implements MoveListener.moveMade
                runner.history.Add(move.clone())
            End Sub

        End Class

    End Class

End Namespace