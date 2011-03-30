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
import java.util.Timer;
import java.util.TimerTask;

public class Game {

	public final static byte HUMAN_PLAYER = 0;
	public final static byte COMPUTER_PLAYER = 1;
	public final static byte REMOTE_PLAYER = 2;
	
	protected final Config config;
	protected final Board board;
	protected final BoardUI boardUI;
	protected final Client client;
	protected Player firstPlayer;
	protected Player secondPlayer;
	protected boolean disposed;
	protected boolean finished;
	private final List<MoveListener> listeners;
	
	public Game(final Config config, final Board board, final BoardUI boardUI, final Client client) {
		this.config = config;
		this.board = board;
		this.boardUI = boardUI;
		this.client = client;
		this.board.clear();
		this.board.setCurrentPiece(Board.BLACK);
		this.boardUI.clearListeners();
		this.listeners = new ArrayList<MoveListener>();
		this.disposed = false;
		this.finished = false;
	}
	
	public void create() throws Exception {
		if (hasRemotePlayer()) {
			boolean playFirst = (config.getByte(Config.FIRST_TYPE) != REMOTE_PLAYER);
			String gameId = client.createGame(playFirst, config.getInt(Config.BOARD_WIDTH), config.getInt(Config.BOARD_HEIGHT));
			config.setValue(Config.CURRENT_GAME, gameId);
		}
		createPlayers();
		setup();
		checkReady();
	}
	
	public void join(final String gameId) throws Exception {
		if (hasRemotePlayer()) {
			client.joinGame(gameId);
			config.setValue(Config.CURRENT_GAME, gameId);
		}
		createPlayers();
		setup();
		checkReady();
	}
	
	public void list(final List<String> tag) {
		client.listGame(tag);
	}
	
	public void dispose() {
		disposed = true;
		listeners.clear();
		if (firstPlayer != null) {
			firstPlayer.dispose();
		}
		if (secondPlayer != null) {
			secondPlayer.dispose();
		}
		try {
			client.endGame(false, Board.NO_WIN);
		} catch (Exception e) {
		}
	}
	
	private void setup() {
		firstPlayer.addMoveListener(new MoveListener() {
			public void moveMade(final Move move) {
				boardUI.update();
				fireMoveMade(move);
				secondThink();
			}
			public void lastMove(final byte victory) {
				finished = true;
				secondPlayer.setFinished();
				fireLastMove(victory);
				try {
					client.endGame(true, victory);
				} catch (Exception e) {
				}
			}
		});
		secondPlayer.addMoveListener(new MoveListener() {
			public void moveMade(final Move move) {
				boardUI.update();
				fireMoveMade(move);
				firstThink();
			}
			public void lastMove(final byte victory) {
				finished = true;
				firstPlayer.setFinished();
				fireLastMove(victory);
				try {
					client.endGame(true, victory);
				} catch (Exception e) {
				}
			}
		});
	}
	
	protected boolean hasRemotePlayer() {
		boolean remote = false;
		byte firstType = config.getByte(Config.FIRST_TYPE);
		byte secondType = config.getByte(Config.SECOND_TYPE);
		if (firstType == REMOTE_PLAYER && secondType == REMOTE_PLAYER) {
			remote = false;
		} else if (firstType == REMOTE_PLAYER || secondType == REMOTE_PLAYER) {
			remote = true;
		}
		return remote;
	}
	
	protected void createPlayers() {
		boolean remote = false;
		byte firstType = config.getByte(Config.FIRST_TYPE);
		byte secondType = config.getByte(Config.SECOND_TYPE);
		if (firstType == REMOTE_PLAYER && secondType == REMOTE_PLAYER) {
			remote = false;
		} else if (firstType == REMOTE_PLAYER || secondType == REMOTE_PLAYER) {
			remote = true;
		}
		if (remote) {
			if (firstType == REMOTE_PLAYER) {
				firstPlayer = new RemotePlayer(config, client, board, Board.BLACK);
			} else if (firstType == COMPUTER_PLAYER) {
				firstPlayer = new RemotePlayer(config, client, board, new ComputerPlayer(config, board, Board.BLACK));
			} else if (firstType == HUMAN_PLAYER) {
				firstPlayer = new RemotePlayer(config, client, board, new HumanPlayer(config, board, Board.BLACK, boardUI));
			} else {
				firstPlayer = createUnknownPlayer(true, firstType, Board.BLACK);
			}
			if (secondType == REMOTE_PLAYER) {
				secondPlayer = new RemotePlayer(config, client, board, Board.WHITE);
			} else if (secondType == COMPUTER_PLAYER) {
				secondPlayer = new RemotePlayer(config, client, board, new ComputerPlayer(config, board, Board.WHITE));
			} else if (secondType == HUMAN_PLAYER) {
				secondPlayer = new RemotePlayer(config, client, board, new HumanPlayer(config, board, Board.WHITE, boardUI));
			} else {
				secondPlayer = createUnknownPlayer(true, secondType, Board.WHITE);
			}
		} else {
			if (firstType == COMPUTER_PLAYER) {
				firstPlayer = new ComputerPlayer(config, board, Board.BLACK);
			} else if (firstType == HUMAN_PLAYER) {
				firstPlayer = new HumanPlayer(config, board, Board.BLACK, boardUI);
			} else {
				firstPlayer = createUnknownPlayer(false, firstType, Board.BLACK);
			}
			if (secondType == COMPUTER_PLAYER) {
				secondPlayer = new ComputerPlayer(config, board, Board.WHITE);
			} else if (secondType == HUMAN_PLAYER) {
				secondPlayer = new HumanPlayer(config, board, Board.WHITE, boardUI);
			} else {
				secondPlayer = createUnknownPlayer(false, secondType, Board.WHITE);
			}
		}
	}
	
	protected Player createUnknownPlayer(final boolean hasRemote, final byte type, final byte piece) {
		if (hasRemote) {
			return new RemotePlayer(config, client, board, new HumanPlayer(config, board, piece, boardUI));
		} else {
			return new HumanPlayer(config, board, piece, boardUI);
		}
	}
	
	public void addMoveListener(final MoveListener src) {
		listeners.add(src);
	}
	
	public void clearListeners() {
		listeners.clear();
	}
	
	protected void fireMoveMade(final Move move) {
		for (int i = 0; i < listeners.size(); i++) {
			listeners.get(i).moveMade(move);
		}
	}
	
	protected void fireLastMove(final byte victory) {
		for (int i = 0; i < listeners.size(); i++) {
			listeners.get(i).lastMove(victory);
		}
	}
	
	protected void firstThink() {
		Timer timer = new Timer();
		timer.schedule(new FirstThink(), 5);
	}
	
	protected void secondThink() {
		Timer timer = new Timer();
		timer.schedule(new SecondThink(), 5);
	}
	
	protected void checkReady() {
		Timer timer = new Timer();
		timer.schedule(new CheckReady(), 5);
	}
	
	private class FirstThink extends TimerTask {
		public void run() {
			firstPlayer.think(new Move());
		}
	}

	private class SecondThink extends TimerTask {
		public void run() {
			secondPlayer.think(new Move());
		}
	}
	
	private class CheckReady extends TimerTask {
		public void run() {
			boolean stop = false;
			while (!stop && !disposed) {
				stop = firstPlayer.checkReady() && secondPlayer.checkReady();
				if (stop) break;
				try {
					Thread.sleep(1000);
				} catch (Exception e) {
				}
			}
			firstPlayer.setReady();
			secondPlayer.setReady();
			firstThink();
		}
	}
	
}
