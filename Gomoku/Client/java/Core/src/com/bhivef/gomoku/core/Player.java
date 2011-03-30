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

public class Player {

	protected final Config config;
	protected final Board board;
	private final byte piece;
	private final List<MoveListener> listeners;
	private boolean finished;
	private boolean ready;
	protected boolean disposed;
	
	public Player(final Config config, final Board board, final byte piece) {
		this.config = config;
		this.board = board;
		this.piece = piece;
		this.listeners = new ArrayList<MoveListener>();
		this.finished = false;
		this.ready = false;
		this.disposed = false;
	}
	
	public byte getPiece() {
		return piece;
	}
	
	public void makeMove(final Move move) {
		if (!ready) return;
		if (finished) return;
		if (piece != move.getPiece()) return;
		if (piece != board.getCurrentPiece()) return;
		if (move.getRow() >= board.getHeight() || move.getColumn() >= board.getWidth()) return;
		if (board.getPiece(move.getRow(), move.getColumn()) != Board.BLANK) return;
		board.setPiece(move.getRow(), move.getColumn(), piece);
		board.setCurrentPiece(Board.opponentPiece(piece));
		fireMoveMade(move);
		byte victory = board.victory();
		switch (victory) {
		case Board.BLACK_WIN:
		case Board.WHITE_WIN:
		case Board.DRAW:
			fireLastMove(victory);
		}
	}
	
	public void think(final Move move) {
	}
	
	public boolean getReady() {
		return ready;
	}
	
	public void setReady() {
		ready = true;
	}
	
	public boolean getFinished() {
		return finished;
	}
	
	public void setFinished() {
		finished = true;
	}
	
	public boolean checkReady() {
		return true;
	}
	
	public void dispose() {
		disposed = true;
		listeners.clear();
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
		finished = true;
		for (int i = 0; i < listeners.size(); i++) {
			listeners.get(i).lastMove(victory);
		}
	}
	
}
