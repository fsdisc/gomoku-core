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

    Public Class Runner
        Inherits com.bhivef.gomoku.core.Runner

        Protected frame As Form

        Public Overrides Sub start()
            MyBase.start()
            Application.Run(frame)
        End Sub

        Protected Overrides Sub createBoardUI()
            boardUI = New WinFormBoardUI(board)
        End Sub

        Protected Overrides Sub createMainUI()
            Dim panel As Panel
            Dim button As Button

            frame = New Form()
            frame.Text = getTitle()
            frame.Size = New Size(2 * boardUI.LEFT_MARGIN + board.getWidth() * boardUI.CELL_WIDTH, 5 * boardUI.TOP_MARGIN + board.getHeight() * boardUI.CELL_HEIGHT)
            frame.FormBorderStyle = FormBorderStyle.FixedSingle
            frame.MaximizeBox = False
            frame.StartPosition = FormStartPosition.CenterScreen
            AddHandler frame.FormClosing, AddressOf frame_FormClosing
            AddHandler frame.Load, AddressOf frame_Load

            panel = CType(boardUI, WinFormBoardUI).getUI()
            frame.Controls.Add(panel)
            panel.Location = New Point(0, 0)
            panel.Size = New Size(2 * boardUI.LEFT_MARGIN + board.getWidth() * boardUI.CELL_WIDTH, boardUI.TOP_MARGIN + board.getHeight() * boardUI.CELL_HEIGHT)
            panel.Anchor = AnchorStyles.Bottom Or AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right

            Dim top As Integer = boardUI.TOP_MARGIN + board.getHeight() * boardUI.CELL_HEIGHT + 15
            Dim left As Integer = boardUI.LEFT_MARGIN + board.getWidth() * boardUI.CELL_WIDTH

            button = New Button()
            frame.Controls.Add(button)
            button.Text = "Settings"
            button.Top = top
            left -= button.Width
            button.Left = left
            button.Anchor = AnchorStyles.Bottom Or AnchorStyles.Right
            AddHandler button.Click, AddressOf button_Click
            button.Tag = "Settings"

            button = New Button()
            frame.Controls.Add(button)
            button.Text = "Join Game"
            button.Top = top
            left -= button.Width + 10
            button.Left = left
            button.Anchor = AnchorStyles.Bottom Or AnchorStyles.Right
            AddHandler button.Click, AddressOf button_Click
            button.Tag = "JoinGame"

            button = New Button()
            frame.Controls.Add(button)
            button.Text = "New Game"
            button.Top = top
            left -= button.Width + 10
            button.Left = left
            button.Anchor = AnchorStyles.Bottom Or AnchorStyles.Right
            AddHandler button.Click, AddressOf button_Click
            button.Tag = "NewGame"

            resizeMain()
        End Sub

        Private Sub frame_Load(ByVal sender As Object, ByVal e As EventArgs)
            CType(boardUI, WinFormBoardUI).setReady()
        End Sub

        Private Sub button_Click(ByVal sender As Object, ByVal e As EventArgs)
            Dim button As Button = CType(sender, Button)
            Dim type As String = CType(button.Tag, String)
            If "Settings".Equals(type) Then
                drawSettings()
            End If
            If "NewGame".Equals(type) Then
                drawNewGame()
            End If
            If "JoinGame".Equals(type) Then
                drawJoinGame()
            End If
        End Sub

        Private Sub frame_FormClosing(ByVal sender As Object, ByVal e As FormClosingEventArgs)
            game.dispose()
            client.logout()
        End Sub

        Private Sub infoBox(ByVal title As String, ByVal message As String)
            MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Sub

        Private Sub errorBox(ByVal title As String, ByVal message As String)
            MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Sub

        Private Sub warningBox(ByVal title As String, ByVal message As String)
            MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Sub

        Private Sub placeCentered(ByVal form As Form)
            Dim sw As Integer = Screen.PrimaryScreen.Bounds.Width
            Dim sh As Integer = Screen.PrimaryScreen.Bounds.Height
            Dim x As Integer = sw - form.Size.Width
            Dim y As Integer = sh - form.Size.Height
            form.Location = New Point(CType(x / 2, Integer), CType(y / 2, Integer))
        End Sub

        Protected Overrides Sub blackWin()
            infoBox("Information", "Game is finished with black win!")
        End Sub

        Protected Overrides Sub whiteWin()
            infoBox("Information", "Game is finished with white win!")
        End Sub

        Protected Overrides Sub drawEnd()
            infoBox("Information", "Game is finished with draw!")
        End Sub

        Protected Overridable Sub resizeMain()
            Dim width As Integer = 2 * boardUI.LEFT_MARGIN + board.getWidth() * boardUI.CELL_WIDTH
            Dim height As Integer = 5 * boardUI.TOP_MARGIN + board.getHeight() * boardUI.CELL_HEIGHT
            frame.Size = New System.Drawing.Size(width, height)
            placeCentered(frame)
            boardUI.update()
        End Sub

        Protected Overridable Sub drawJoinGame()
            Dim dialog As New JoinGameForm(Me)
            dialog.ShowDialog(frame)
        End Sub

        Protected Overridable Sub drawSettings()
            Dim dialog As New SettingsForm(Me)
            dialog.ShowDialog(frame)
        End Sub

        Protected Overridable Sub drawNewGame()
            Dim dialog As New NewGameForm(Me)
            dialog.ShowDialog(frame)
        End Sub

        Protected Overridable Function getTitle() As String
            Return "Gomoku Core"
        End Function

        Protected Overridable Function getPlayerNames(ByVal newGame As Boolean) As String()
            If newGame Then
                Return New String() {"Human", "Computer", "Remote"}
            Else
                Return New String() {"Human", "Computer"}
            End If
        End Function

        Protected Overridable Function getPlayerIndex(ByVal newGame As Boolean, ByVal type As Byte) As Integer
            If newGame Then
                Select Case type
                    Case game.HUMAN_PLAYER
                        Return 0
                    Case game.COMPUTER_PLAYER
                        Return 1
                    Case game.REMOTE_PLAYER
                        Return 2
                    Case Else
                        Return 0
                End Select
            Else
                Select Case type
                    Case game.HUMAN_PLAYER
                        Return 0
                    Case game.COMPUTER_PLAYER
                        Return 1
                    Case Else
                        Return 0
                End Select
            End If
        End Function

        Protected Overridable Function getPlayerType(ByVal newGame As Boolean, ByVal index As Integer) As Byte
            If newGame Then
                Dim type As Byte = game.HUMAN_PLAYER
                Select Case index
                    Case 0
                        type = game.HUMAN_PLAYER
                    Case 1
                        type = game.COMPUTER_PLAYER
                    Case 2
                        type = game.REMOTE_PLAYER
                End Select
                Return type
            Else
                Dim type As Byte = game.HUMAN_PLAYER
                Select Case index
                    Case 0
                        type = game.HUMAN_PLAYER
                    Case 1
                        type = game.COMPUTER_PLAYER
                End Select
                Return type
            End If
        End Function

        Protected Overridable Function isRemotePlayer(ByVal index As Integer) As Boolean
            Return index = 2
        End Function

        Private Class JoinGameForm
            Inherits Form

            Private runner As Runner
            Private listGame As ListBox
            Private comboPlayer As ComboBox
            Private srcList As New List(Of String)

            Public Sub New(ByVal runner As Runner)
                MyBase.New()
                Me.runner = runner
                setup()
            End Sub

            Private Sub setup()
                Dim label As Label
                Dim combo As ComboBox
                Dim list As ListBox
                Dim button As Button

                Me.Text = "Join Game"
                Me.FormBorderStyle = Windows.Forms.FormBorderStyle.FixedSingle
                Me.MaximizeBox = False
                Me.Size = New Size(230, 410)
                Me.StartPosition = FormStartPosition.CenterScreen

                list = New ListBox()
                listGame = list
                Me.Controls.Add(list)
                list.Top = 10
                list.Left = 10
                list.Width = 200
                list.Height = 300
                list.SelectionMode = SelectionMode.One
                Me.runner.game.list(srcList)
                For i As Integer = 0 To srcList.Count - 1
                    Dim line As String = srcList(i)
                    Dim fields() As String = line.Split(New Char() {"|".Chars(0)})
                    If fields(1).Length > 0 Then
                        list.Items.Add(fields(0) & " (" & fields(3) & "x" & fields(4) & ", First: " & fields(1) & ")")
                    Else
                        list.Items.Add(fields(0) & " (" & fields(3) & "x" & fields(4) & ", Second: " & fields(2) & ")")
                    End If
                Next

                label = New Label()
                Me.Controls.Add(label)
                label.Text = "Player"
                label.AutoSize = True
                label.Top = 320
                label.Left = 10

                combo = New ComboBox()
                comboPlayer = combo
                Me.Controls.Add(combo)
                combo.Width = 160
                combo.Top = 318
                combo.Left = 50
                combo.DropDownStyle = ComboBoxStyle.DropDownList
                Dim types() As String = Me.runner.getPlayerNames(False)
                For i As Integer = 0 To types.Length - 1
                    combo.Items.Add(types(i))
                Next
                combo.SelectedIndex = 0

                button = New Button()
                Me.Controls.Add(button)
                button.Text = "OK"
                button.Top = 350
                button.Left = 40
                AddHandler button.Click, AddressOf ok_Click

                button = New Button()
                Me.Controls.Add(button)
                button.Text = "Cancel"
                button.Top = 350
                button.Left = 115
                AddHandler button.Click, AddressOf cancel_Click
            End Sub

            Private Sub cancel_Click(ByVal sender As Object, ByVal e As EventArgs)
                Me.Close()
            End Sub

            Private Sub ok_Click(ByVal sender As Object, ByVal e As EventArgs)
                Dim idx As Integer = listGame.SelectedIndex
                If idx < 0 Then
                    Me.runner.warningBox("Warning", "Game is required to select!")
                    Return
                End If
                Dim line As String = srcList(idx)
                Dim fields() As String = line.Split(New Char() {"|".Chars(0)})
                Dim playFirst As Boolean = fields(1).Length > 0
                Dim player As Byte = Me.runner.getPlayerType(False, comboPlayer.SelectedIndex)
                If playFirst Then
                    Me.runner.config.setValue(config.FIRST_TYPE, game.REMOTE_PLAYER)
                    Me.runner.config.setValue(config.SECOND_TYPE, player)
                Else
                    Me.runner.config.setValue(config.FIRST_TYPE, player)
                    Me.runner.config.setValue(config.SECOND_TYPE, game.REMOTE_PLAYER)
                End If
                Dim width As Integer = board.GO_WIDTH
                If Not Integer.TryParse(fields(3), width) Then
                    Me.runner.errorBox("Error", "Invalid board width!")
                    Return
                End If
                Dim height As Integer = board.GO_HEIGHT
                If Not Integer.TryParse(fields(4), height) Then
                    Me.runner.errorBox("Error", "Invalid board height!")
                    Return
                End If
                Me.runner.config.setValue(config.BOARD_WIDTH, width)
                Me.runner.config.setValue(config.BOARD_HEIGHT, height)
                Me.runner.createGame()
                Me.runner.resizeMain()
                Try
                    Me.runner.joinGame(fields(0))
                    Me.Close()
                Catch ex As Exception
                    Me.runner.errorBox("Error", ex.Message)
                    Return
                End Try
            End Sub

        End Class

        Private Class NewGameForm
            Inherits Form

            Private runner As Runner
            Private textWidth As TextBox
            Private textHeight As TextBox
            Private comboFirst As ComboBox
            Private comboSecond As ComboBox

            Public Sub New(ByVal runner As Runner)
                MyBase.New()
                Me.runner = runner
                setup()
            End Sub

            Private Sub setup()
                Dim label As Label
                Dim text As TextBox
                Dim combo As ComboBox
                Dim button As Button

                Me.Text = "New Game"
                Me.FormBorderStyle = Windows.Forms.FormBorderStyle.FixedSingle
                Me.MaximizeBox = False
                Me.Size = New Size(260, 190)
                Me.StartPosition = FormStartPosition.CenterScreen

                label = New Label()
                Me.Controls.Add(label)
                label.Text = "Board width"
                label.AutoSize = True
                label.Top = 10
                label.Left = 10

                text = New TextBox()
                textWidth = text
                Me.Controls.Add(text)
                text.Width = 150
                text.Top = 8
                text.Left = 90
                text.Text = Me.runner.config.getByte(config.BOARD_WIDTH).ToString()

                label = New Label()
                Me.Controls.Add(label)
                label.Text = "Board height"
                label.AutoSize = True
                label.Top = 40
                label.Left = 10

                text = New TextBox()
                textHeight = text
                Me.Controls.Add(text)
                text.Width = 150
                text.Top = 38
                text.Left = 90
                text.Text = Me.runner.config.getByte(config.BOARD_HEIGHT).ToString()

                Dim types() As String = Me.runner.getPlayerNames(True)

                label = New Label()
                Me.Controls.Add(label)
                label.Text = "First player"
                label.AutoSize = True
                label.Top = 70
                label.Left = 10

                combo = New ComboBox()
                comboFirst = combo
                Me.Controls.Add(combo)
                combo.Width = 150
                combo.Top = 68
                combo.Left = 90
                combo.DropDownStyle = ComboBoxStyle.DropDownList
                For i As Integer = 0 To types.Length - 1
                    combo.Items.Add(types(i))
                Next
                combo.SelectedIndex = Me.runner.getPlayerIndex(True, Me.runner.config.getByte(config.FIRST_TYPE))

                label = New Label()
                Me.Controls.Add(label)
                label.Text = "Second player"
                label.AutoSize = True
                label.Top = 100
                label.Left = 10

                combo = New ComboBox()
                comboSecond = combo
                Me.Controls.Add(combo)
                combo.Width = 150
                combo.Top = 98
                combo.Left = 90
                combo.DropDownStyle = ComboBoxStyle.DropDownList
                For i As Integer = 0 To types.Length - 1
                    combo.Items.Add(types(i))
                Next
                combo.SelectedIndex = Me.runner.getPlayerIndex(True, Me.runner.config.getByte(config.SECOND_TYPE))

                button = New Button()
                Me.Controls.Add(button)
                button.Text = "OK"
                button.Top = 130
                button.Left = 50
                AddHandler button.Click, AddressOf ok_Click

                button = New Button()
                Me.Controls.Add(button)
                button.Text = "Cancel"
                button.Top = 130
                button.Left = 125
                AddHandler button.Click, AddressOf cancel_Click
            End Sub

            Private Sub cancel_Click(ByVal sender As Object, ByVal e As EventArgs)
                Me.Close()
            End Sub

            Private Sub ok_Click(ByVal sender As Object, ByVal e As EventArgs)
                Dim width As Integer = 0
                If Not Integer.TryParse(textWidth.Text, width) Then
                    width = 0
                End If
                If width < 10 Or width > 255 Then
                    Me.runner.warningBox("Warning", "Width must be in [10-255] range!")
                    Return
                End If
                Dim height As Integer = 0
                If Not Integer.TryParse(textHeight.Text, height) Then
                    height = 0
                End If
                If height < 10 Or height > 255 Then
                    Me.runner.warningBox("Warning", "Height must be in [10-255] range!")
                    Return
                End If
                If Me.runner.isRemotePlayer(comboFirst.SelectedIndex) And Me.runner.isRemotePlayer(comboSecond.SelectedIndex) Then
                    Me.runner.warningBox("Warning", "Both first player and second player can not be remote!")
                    Return
                End If
                Dim firstType As Byte = Me.runner.getPlayerType(True, comboFirst.SelectedIndex)
                Dim secondType As Byte = Me.runner.getPlayerType(True, comboSecond.SelectedIndex)
                Me.runner.config.setValue(config.BOARD_WIDTH, width)
                Me.runner.config.setValue(config.BOARD_HEIGHT, height)
                Me.runner.config.setValue(config.FIRST_TYPE, firstType)
                Me.runner.config.setValue(config.SECOND_TYPE, secondType)
                Me.runner.createGame()
                Me.runner.resizeMain()
                Try
                    Me.runner.newGame()
                Catch ex As Exception
                    Me.runner.errorBox("Error", ex.Message)
                End Try
                Me.Close()
            End Sub

        End Class

        Private Class SettingsForm
            Inherits Form

            Private runner As Runner
            Private textServerURL As TextBox
            Private textUsername As TextBox
            Private textPassword As TextBox

            Public Sub New(ByVal runner As Runner)
                MyBase.New()
                Me.runner = runner
                setup()
            End Sub

            Private Sub setup()
                Dim label As Label
                Dim text As TextBox
                Dim button As Button

                Me.Text = "Settings"
                Me.FormBorderStyle = Windows.Forms.FormBorderStyle.FixedSingle
                Me.MaximizeBox = False
                Me.Size = New Size(250, 160)
                Me.StartPosition = FormStartPosition.CenterScreen

                label = New Label()
                Me.Controls.Add(label)
                label.Text = "Server URL"
                label.AutoSize = True
                label.Top = 10
                label.Left = 10

                text = New TextBox()
                textServerURL = text
                Me.Controls.Add(text)
                text.Width = 150
                text.Top = 8
                text.Left = 75
                text.Text = Me.runner.config.getString(config.SERVER_URL)

                label = New Label()
                Me.Controls.Add(label)
                label.Text = "Username"
                label.AutoSize = True
                label.Top = 40
                label.Left = 10

                text = New TextBox()
                textUsername = text
                Me.Controls.Add(text)
                text.Width = 150
                text.Top = 38
                text.Left = 75
                text.Text = Me.runner.config.getString(config.SERVER_USERNAME)

                label = New Label()
                Me.Controls.Add(label)
                label.Text = "Password"
                label.AutoSize = True
                label.Top = 70
                label.Left = 10

                text = New TextBox()
                textPassword = text
                Me.Controls.Add(text)
                text.Width = 150
                text.Top = 68
                text.Left = 75
                text.PasswordChar = "*".Chars(0)
                text.Text = Me.runner.config.getPassword(config.SERVER_PASSWORD)

                button = New Button()
                Me.Controls.Add(button)
                button.Text = "OK"
                button.Top = 100
                button.Left = 50
                AddHandler button.Click, AddressOf ok_Click

                button = New Button()
                Me.Controls.Add(button)
                button.Text = "Cancel"
                button.Top = 100
                button.Left = 125
                AddHandler button.Click, AddressOf cancel_Click
            End Sub

            Private Sub cancel_Click(ByVal sender As Object, ByVal e As EventArgs)
                Me.Close()
            End Sub

            Private Sub ok_Click(ByVal sender As Object, ByVal e As EventArgs)
                Me.runner.config.setValue(config.SERVER_URL, textServerURL.Text)
                Me.runner.config.setValue(config.SERVER_USERNAME, textUsername.Text)
                Me.runner.config.setPassword(config.SERVER_PASSWORD, textPassword.Text)
                Try
                    Me.runner.client.login()
                    Me.Close()
                Catch ex As Exception
                    Me.runner.errorBox("Error", ex.Message)
                    Return
                End Try
            End Sub

        End Class

    End Class

End Namespace