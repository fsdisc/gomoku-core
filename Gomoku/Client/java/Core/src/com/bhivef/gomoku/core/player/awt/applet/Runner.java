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

package com.bhivef.gomoku.core.player.awt.applet;

import java.awt.Frame;
import java.awt.SystemColor;
import java.awt.event.WindowAdapter;
import java.awt.event.WindowEvent;
import java.util.List;

import com.bhivef.gomoku.core.BoardUI;
import com.bhivef.gomoku.core.Config;
import com.bhivef.gomoku.core.player.awt.MsgBox;
import com.bhivef.gomoku.core.ui.AWTBoardUI;

public class Runner extends com.bhivef.gomoku.core.Runner {

	protected Frame frame;
	protected String session;

	public Runner(final Config config, final String session) {
		this.config = config;
		this.session = session;
		createClient();
	}
	
	public void list(final List<String> tag) {
		client.listGame(tag);
	}
	
	protected void createMainUI() {
		frame = new Frame();
		frame.setTitle(getTitle());
		frame.setSize(10, 10);
		frame.setResizable(false);
		frame.setBackground(SystemColor.control);
		frame.add("Center", ((AWTBoardUI)boardUI).getUI());
		frame.addWindowListener ( new WindowAdapter () {
			public void windowClosing ( WindowEvent evt ) {
				frame.setVisible(false);
				frame.dispose();
				game.dispose();
			}
		});

		int width = 2 * BoardUI.LEFT_MARGIN + board.getWidth() * BoardUI.CELL_WIDTH;
		int height = 3 * BoardUI.TOP_MARGIN + board.getHeight() * BoardUI.CELL_HEIGHT;
		frame.setSize(width, height);
        MsgBox.placeCentered(frame);
        boardUI.update();
		
		frame.setVisible(true);
	}

	protected void createClient() {
		super.createClient();
		client.clone(session);
	}
	
	protected void createConfig() {
	}
	
	protected void blackWin() {
		MsgBox.info(frame, "Information", "Game is finished with black win!");
	}
	
	protected void whiteWin() {
		MsgBox.info(frame, "Information", "Game is finished with white win!");
	}
	
	protected void drawEnd() {
		MsgBox.info(frame, "Information", "Game is finished with draw!");
	}
	
	public void start(final String gameId) {
		try {
			setup();
			if (gameId.length() == 0) {
				newGame();
			} else {
				joinGame(gameId);
			}
		} catch (Exception e) {
			System.out.println(e.toString());
		}
	}

	protected String getTitle() {
		return "Gomoku Core";
	}
	
}
