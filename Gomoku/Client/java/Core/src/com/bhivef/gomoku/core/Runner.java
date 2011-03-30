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

package com.bhivef.gomoku.core;

import java.util.ArrayList;
import java.util.List;

import com.bhivef.gomoku.core.ui.AWTBoardUI;

public class Runner {

	protected Config config;
	protected Board board;
	protected BoardUI boardUI;
	protected Client client;
	protected Game game;
	protected List<Move> history;
	protected boolean finished;
	
	public void start() {
		setup();
		try {
			newGame();
		} catch (Exception e) {
		}
	}

	protected void setup() {
		createConfig();
		createBoard();
		createBoardUI();
		createClient();
		createGame();
		createMainUI();
	}
	
	protected void createConfig() {
		config = new Config();
		config.setValue(Config.SEARCH_DEPTH, 2);
		config.setValue(Config.BOARD_WIDTH, Board.GO_WIDTH);
		config.setValue(Config.BOARD_HEIGHT, Board.GO_HEIGHT);
		config.setValue(Config.FIRST_TYPE, Game.HUMAN_PLAYER);
		config.setValue(Config.SECOND_TYPE, Game.HUMAN_PLAYER);
		config.setValue(Config.SERVER_URL, "http://bhivef.com/gomoku/");
		config.setValue(Config.SERVER_EXTENSION, ".php");
	}
	
	protected void createBoard() {
		board = new Board(config.getByte(Config.BOARD_WIDTH), config.getByte(Config.BOARD_HEIGHT));
	}

	protected void createBoardUI() {
		boardUI = new AWTBoardUI(board);
	}
	
	protected void createClient() {
		client = new WebClient(config);
	}
	
	protected void createGame() {
		finished = false;
		history = new ArrayList<Move>();
		board.resize(config.getByte(Config.BOARD_WIDTH), config.getByte(Config.BOARD_HEIGHT));
		if (game != null) {
			game.dispose();
		}
		constructGame();
		game.addMoveListener(new MoveListener() {
			public void moveMade(final Move move) {
				history.add(move.clone());
			}
			public void lastMove(final byte victory) {
				if (finished) return;
				switch (victory) {
				case Board.DRAW:
					drawEnd();
					break;
				case Board.BLACK_WIN:
					blackWin();
					break;
				case Board.WHITE_WIN:
					whiteWin();
					break;
				default:
					return;
				}
				finished = true;
			}
		});
	}

	protected void constructGame() {
		game = new Game(config, board, boardUI, client);
	}
	
	protected void createMainUI() {
	}
	
	protected void blackWin() {
	}
	
	protected void whiteWin() {
	}
	
	protected void drawEnd() {
	}
	
	protected void newGame() throws Exception {
		game.create();
	}
	
	protected void joinGame(String gameId) throws Exception {
		game.join(gameId);
	}
	
}
