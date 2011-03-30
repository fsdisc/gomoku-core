/*
 *  Gomoku Core
 * 
 *  Copyright (c) 2011 Tran Dinh Thoai <dthoai@yahoo.com>
 *
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU General Public License
 * version 3.
 *
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
 * General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this library; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace com.bhivef.gomoku.core.players
{
    public class Runner : com.bhivef.gomoku.core.Runner
    {
	    protected Form frame;

        public override void start()
        {
            base.start();
            Application.Run(frame);
        }

        protected override void createBoardUI()
        {
            boardUI = new WinFormBoardUI(board);
        }

	    protected override void createMainUI() 
        {
            Panel panel;
            Button button;

            frame = new Form();
            frame.Text = getTitle();
            frame.Size = new Size(2 * BoardUI.LEFT_MARGIN + board.getWidth() * BoardUI.CELL_WIDTH, 5 * BoardUI.TOP_MARGIN + board.getHeight() * BoardUI.CELL_HEIGHT);
            frame.FormBorderStyle = FormBorderStyle.FixedSingle;
            frame.MaximizeBox = false;
            frame.StartPosition = FormStartPosition.CenterScreen;
            frame.FormClosing += new FormClosingEventHandler(frame_FormClosing);
            frame.Load += new EventHandler(frame_Load);

            panel = ((WinFormBoardUI)boardUI).getUI();
            frame.Controls.Add(panel);
            panel.Location = new Point(0, 0);
            panel.Size = new Size(2 * BoardUI.LEFT_MARGIN + board.getWidth() * BoardUI.CELL_WIDTH, BoardUI.TOP_MARGIN + board.getHeight() * BoardUI.CELL_HEIGHT);
            panel.Anchor = AnchorStyles.Bottom | AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

            int top = BoardUI.TOP_MARGIN + board.getHeight() * BoardUI.CELL_HEIGHT + 15;
            int left = BoardUI.LEFT_MARGIN + board.getWidth() * BoardUI.CELL_WIDTH;

            button = new Button();
            frame.Controls.Add(button);
            button.Text = "Settings";
            button.Top = top;
            left -= button.Width;
            button.Left = left;
            button.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            button.Click += new EventHandler(button_Click);
            button.Tag = "Settings";

            button = new Button();
            frame.Controls.Add(button);
            button.Text = "Join Game";
            button.Top = top;
            left -= button.Width + 10;
            button.Left = left;
            button.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            button.Click += new EventHandler(button_Click);
            button.Tag = "JoinGame";

            button = new Button();
            frame.Controls.Add(button);
            button.Text = "New Game";
            button.Top = top;
            left -= button.Width + 10;
            button.Left = left;
            button.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            button.Click += new EventHandler(button_Click);
            button.Tag = "NewGame";

            resizeMain();
        }

        private void frame_Load(object sender, EventArgs e)
        {
            ((WinFormBoardUI)boardUI).setReady();
        }

        private void button_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            String type = (String)button.Tag;
            if ("Settings".Equals(type))
            {
                drawSettings();
            }
            if ("NewGame".Equals(type))
            {
                drawNewGame();
            }
            if ("JoinGame".Equals(type))
            {
                drawJoinGame();
            }
        }

        private void frame_FormClosing(object sender, FormClosingEventArgs e)
        {
            game.dispose();
            client.logout();
        }
	
        private void infoBox(String title, String message) 
        {
            MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void errorBox(String title, String message)
        {
            MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void warningBox(String title, String message)
        {
            MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void placeCentered(Form form) 
        {
            int sw = Screen.PrimaryScreen.Bounds.Width;
            int sh = Screen.PrimaryScreen.Bounds.Height;
            int x = sw - form.Size.Width;
            int y = sh - form.Size.Height;
            form.Location = new Point(x / 2, y / 2);
        }

	    protected override void blackWin() 
        {
		    infoBox("Information", "Game is finished with black win!");
	    }
	
	    protected override void whiteWin() 
        {
		    infoBox("Information", "Game is finished with white win!");
	    }
	
	    protected override void drawEnd() 
        {
		    infoBox("Information", "Game is finished with draw!");
	    }

	    protected virtual void resizeMain() 
        {
		    int width = 2 * BoardUI.LEFT_MARGIN + board.getWidth() * BoardUI.CELL_WIDTH;
		    int height = 5 * BoardUI.TOP_MARGIN + board.getHeight() * BoardUI.CELL_HEIGHT;
            frame.Size = new System.Drawing.Size(width, height);
            placeCentered(frame);
            boardUI.update();
	    }
	
	    protected virtual void drawJoinGame() 
        {
            JoinGameForm dialog = new JoinGameForm(this);
            dialog.ShowDialog(frame);
	    }
	
	    protected virtual void drawSettings() 
        {
            SettingsForm dialog = new SettingsForm(this);
            dialog.ShowDialog(frame);
	    }
	
	    protected virtual void drawNewGame() 
        {
            NewGameForm dialog = new NewGameForm(this);
            dialog.ShowDialog(frame);
	    }

	    protected virtual String getTitle() 
        {
		    return "Gomoku Core";
	    }
	
	    protected virtual String[] getPlayerNames(bool newGame) 
        {
		    if (newGame) 
            {
			    return new String[] { "Human", "Computer", "Remote" };
		    } 
            else 
            {
			    return new String[] { "Human", "Computer" };
		    }
	    }

	    protected virtual int getPlayerIndex(bool newGame, byte type) 
        {
		    if (newGame) 
            {
			    switch (type) 
                {
			    case Game.HUMAN_PLAYER:
				    return 0;
			    case Game.COMPUTER_PLAYER:
				    return 1;
			    case Game.REMOTE_PLAYER:
				    return 2;
			    default:
				    return 0;
			    }
		    } 
            else 
            {
			    switch (type) 
                {
			    case Game.HUMAN_PLAYER:
				    return 0;
			    case Game.COMPUTER_PLAYER:
				    return 1;
			    default:
				    return 0;
    			}
	    	}
	    }
	
	    protected virtual byte getPlayerType(bool newGame, int index) 
        {
		    if (newGame) 
            {
			    byte type = Game.HUMAN_PLAYER;
			    switch (index) 
                {
			    case 0:
				    type = Game.HUMAN_PLAYER;
				    break;
			    case 1:
				    type = Game.COMPUTER_PLAYER;
				    break;
			    case 2:
				    type = Game.REMOTE_PLAYER;
				    break;
			    }
			    return type;
		    } 
            else 
            {
			    byte type = Game.HUMAN_PLAYER;
			    switch (index) 
                {
			    case 0:
				    type = Game.HUMAN_PLAYER;
				    break;
			    case 1:
				    type = Game.COMPUTER_PLAYER;
				    break;
			    }
			    return type;
		    }
	    }
	
	    protected virtual bool isRemotePlayer(int index) 
        {
		    return index == 2;
	    }

        private class JoinGameForm : Form
        {
            private Runner runner;
            private ListBox listGame;
            private ComboBox comboPlayer;
		    private List<String> srcList = new List<String>();

            public JoinGameForm(Runner runner) : base()
            {
                this.runner = runner;
                setup();
            }

            private void setup()
            {
                Label label;
                ComboBox combo;
                ListBox list;
                Button button;

                this.Text = "Join Game";
                this.FormBorderStyle = FormBorderStyle.FixedSingle;
                this.MaximizeBox = false;
                this.Size = new Size(230, 410);
                this.StartPosition = FormStartPosition.CenterScreen;

                list = new ListBox();
                listGame = list;
                this.Controls.Add(list);
                list.Top = 10;
                list.Left = 10;
                list.Width = 200;
                list.Height = 300;
                list.SelectionMode = SelectionMode.One;
		        this.runner.game.list(srcList);
		        for (int i = 0; i < srcList.Count; i++) 
                {
			        String line = srcList[i];
			        String[] fields = line.Split(new char[] { '|' });
			        if (fields[1].Length > 0) 
                    {
                        list.Items.Add(fields[0] + " (" + fields[3] + "x" + fields[4] + ", First: " + fields[1] + ")");
			        } 
                    else 
                    {
                        list.Items.Add(fields[0] + " (" + fields[3] + "x" + fields[4] + ", Second: " + fields[2] + ")");
			        }
		        }

                label = new Label();
                this.Controls.Add(label);
                label.Text = "Player";
                label.AutoSize = true;
                label.Top = 320;
                label.Left = 10;

                combo = new ComboBox();
                comboPlayer = combo;
                this.Controls.Add(combo);
                combo.Width = 160;
                combo.Top = 318;
                combo.Left = 50;
                combo.DropDownStyle = ComboBoxStyle.DropDownList;
                String[] types = this.runner.getPlayerNames(false);
                for (int i = 0; i < types.Length; i++)
                {
                    combo.Items.Add(types[i]);
                }
                combo.SelectedIndex = 0;

                button = new Button();
                this.Controls.Add(button);
                button.Text = "OK";
                button.Top = 350;
                button.Left = 40;
                button.Click += new EventHandler(ok_Click);

                button = new Button();
                this.Controls.Add(button);
                button.Text = "Cancel";
                button.Top = 350;
                button.Left = 115;
                button.Click += new EventHandler(cancel_Click);
            }

            private void cancel_Click(object sender, EventArgs e)
            {
                this.Close();
            }

            private void ok_Click(object sender, EventArgs e)
            {
                int idx = listGame.SelectedIndex;
                if (idx < 0)
                {
                    this.runner.warningBox("Warning", "Game is required to select!");
                    return;
                }
                String line = srcList[idx];
                String[] fields = line.Split(new char[] { '|' });
                bool playFirst = fields[1].Length > 0;
                byte player = this.runner.getPlayerType(false, comboPlayer.SelectedIndex);
                if (playFirst)
                {
                    this.runner.config.setValue(Config.FIRST_TYPE, Game.REMOTE_PLAYER);
                    this.runner.config.setValue(Config.SECOND_TYPE, player);
                }
                else
                {
                    this.runner.config.setValue(Config.FIRST_TYPE, player);
                    this.runner.config.setValue(Config.SECOND_TYPE, Game.REMOTE_PLAYER);
                }
                int width = Board.GO_WIDTH;
                if (!int.TryParse(fields[3], out width))
                {
                    this.runner.errorBox("Error", "Invalid board width!");
                    return;
                }
                int height = Board.GO_HEIGHT;
                if (!int.TryParse(fields[4], out height))
                {
                    this.runner.errorBox("Error", "Invalid board height!");
                    return;
                }
                this.runner.config.setValue(Config.BOARD_WIDTH, width);
                this.runner.config.setValue(Config.BOARD_HEIGHT, height);
                this.runner.createGame();
                this.runner.resizeMain();
                try
                {
                    this.runner.joinGame(fields[0]);
                    this.Close();
                }
                catch (Exception ex)
                {
                    this.runner.errorBox("Error", ex.Message);
                    return;
                }
            }
        }

        private class NewGameForm : Form
        {
            private Runner runner;
            private TextBox textWidth;
            private TextBox textHeight;
            private ComboBox comboFirst;
            private ComboBox comboSecond;

            public NewGameForm(Runner runner) : base()
            {
                this.runner = runner;
                setup();
            }

            private void setup()
            {
                Label label;
                TextBox text;
                ComboBox combo;
                Button button;

                this.Text = "New Game";
                this.FormBorderStyle = FormBorderStyle.FixedSingle;
                this.MaximizeBox = false;
                this.Size = new Size(260, 190);
                this.StartPosition = FormStartPosition.CenterScreen;

                label = new Label();
                this.Controls.Add(label);
                label.Text = "Board width";
                label.AutoSize = true;
                label.Top = 10;
                label.Left = 10;

                text = new TextBox();
                textWidth = text;
                this.Controls.Add(text);
                text.Width = 150;
                text.Top = 8;
                text.Left = 90;
                text.Text = this.runner.config.getByte(Config.BOARD_WIDTH) + "";

                label = new Label();
                this.Controls.Add(label);
                label.Text = "Board height";
                label.AutoSize = true;
                label.Top = 40;
                label.Left = 10;

                text = new TextBox();
                textHeight = text;
                this.Controls.Add(text);
                text.Width = 150;
                text.Top = 38;
                text.Left = 90;
                text.Text = this.runner.config.getByte(Config.BOARD_HEIGHT) + "";

                String[] types = this.runner.getPlayerNames(true);

                label = new Label();
                this.Controls.Add(label);
                label.Text = "First player";
                label.AutoSize = true;
                label.Top = 70;
                label.Left = 10;

                combo = new ComboBox();
                comboFirst = combo;
                this.Controls.Add(combo);
                combo.Width = 150;
                combo.Top = 68;
                combo.Left = 90;
                combo.DropDownStyle = ComboBoxStyle.DropDownList;
                for (int i = 0; i < types.Length; i++)
                {
                    combo.Items.Add(types[i]);
                }
                combo.SelectedIndex = this.runner.getPlayerIndex(true, this.runner.config.getByte(Config.FIRST_TYPE));

                label = new Label();
                this.Controls.Add(label);
                label.Text = "Second player";
                label.AutoSize = true;
                label.Top = 100;
                label.Left = 10;

                combo = new ComboBox();
                comboSecond = combo;
                this.Controls.Add(combo);
                combo.Width = 150;
                combo.Top = 98;
                combo.Left = 90;
                combo.DropDownStyle = ComboBoxStyle.DropDownList;
                for (int i = 0; i < types.Length; i++)
                {
                    combo.Items.Add(types[i]);
                }
                combo.SelectedIndex = this.runner.getPlayerIndex(true, this.runner.config.getByte(Config.SECOND_TYPE));

                button = new Button();
                this.Controls.Add(button);
                button.Text = "OK";
                button.Top = 130;
                button.Left = 50;
                button.Click += new EventHandler(ok_Click);

                button = new Button();
                this.Controls.Add(button);
                button.Text = "Cancel";
                button.Top = 130;
                button.Left = 125;
                button.Click += new EventHandler(cancel_Click);
            }

            private void cancel_Click(object sender, EventArgs e)
            {
                this.Close();
            }

            private void ok_Click(object sender, EventArgs e)
            {
                int width = 0;
                if (!int.TryParse(textWidth.Text, out width))
                {
                    width = 0;
                }
                if (width < 10 || width > 255)
                {
                    this.runner.warningBox("Warning", "Width must be in [10-255] range!");
                    return;
                }
                int height = 0;
                if (!int.TryParse(textHeight.Text, out height))
                {
                    height = 0;
                }
                if (height < 10 || height > 255)
                {
                    this.runner.warningBox("Warning", "Height must be in [10-255] range!");
                    return;
                }
                if (this.runner.isRemotePlayer(comboFirst.SelectedIndex) && this.runner.isRemotePlayer(comboSecond.SelectedIndex))
                {
                    this.runner.warningBox("Warning", "Both first player and second player can not be remote!");
                    return;
                }
                byte firstType = this.runner.getPlayerType(true, comboFirst.SelectedIndex);
                byte secondType = this.runner.getPlayerType(true, comboSecond.SelectedIndex);
                this.runner.config.setValue(Config.BOARD_WIDTH, width);
                this.runner.config.setValue(Config.BOARD_HEIGHT, height);
                this.runner.config.setValue(Config.FIRST_TYPE, firstType);
                this.runner.config.setValue(Config.SECOND_TYPE, secondType);
                this.runner.createGame();
                this.runner.resizeMain();
                try
                {
                    this.runner.newGame();
                }
                catch (Exception ex)
                {
                    this.runner.errorBox("Error", ex.Message);
                }
                this.Close();
            }
        }

        private class SettingsForm : Form
        {
            private Runner runner;
            private TextBox textServerURL;
            private TextBox textUsername;
            private TextBox textPassword;

            public SettingsForm(Runner runner) : base()
            {
                this.runner = runner;
                setup();
            }

            private void setup()
            {
                Label label;
                TextBox text;
                Button button;

                this.Text = "Settings";
                this.FormBorderStyle = FormBorderStyle.FixedSingle;
                this.MaximizeBox = false;
                this.Size = new Size(250, 160);
                this.StartPosition = FormStartPosition.CenterScreen;

                label = new Label();
                this.Controls.Add(label);
                label.Text = "Server URL";
                label.AutoSize = true;
                label.Top = 10;
                label.Left = 10;

                text = new TextBox();
                textServerURL = text;
                this.Controls.Add(text);
                text.Width = 150;
                text.Top = 8;
                text.Left = 75;
                text.Text = this.runner.config.getString(Config.SERVER_URL);

                label = new Label();
                this.Controls.Add(label);
                label.Text = "Username";
                label.AutoSize = true;
                label.Top = 40;
                label.Left = 10;

                text = new TextBox();
                textUsername = text;
                this.Controls.Add(text);
                text.Width = 150;
                text.Top = 38;
                text.Left = 75;
                text.Text = this.runner.config.getString(Config.SERVER_USERNAME);

                label = new Label();
                this.Controls.Add(label);
                label.Text = "Password";
                label.AutoSize = true;
                label.Top = 70;
                label.Left = 10;

                text = new TextBox();
                textPassword = text;
                this.Controls.Add(text);
                text.Width = 150;
                text.Top = 68;
                text.Left = 75;
                text.PasswordChar = '*';
                text.Text = this.runner.config.getPassword(Config.SERVER_PASSWORD);

                button = new Button();
                this.Controls.Add(button);
                button.Text = "OK";
                button.Top = 100;
                button.Left = 50;
                button.Click += new EventHandler(ok_Click);

                button = new Button();
                this.Controls.Add(button);
                button.Text = "Cancel";
                button.Top = 100;
                button.Left = 125;
                button.Click += new EventHandler(cancel_Click);
            }

            private void cancel_Click(object sender, EventArgs e)
            {
                this.Close();
            }

            private void ok_Click(object sender, EventArgs e)
            {
                this.runner.config.setValue(Config.SERVER_URL, textServerURL.Text);
                this.runner.config.setValue(Config.SERVER_USERNAME, textUsername.Text);
                this.runner.config.setPassword(Config.SERVER_PASSWORD, textPassword.Text);
                try
                {
                    this.runner.client.login();
                    this.Close();
                }
                catch (Exception ex)
                {
                    this.runner.errorBox("Error", ex.Message);
                    return;
                }
            }
        }

    }
}
