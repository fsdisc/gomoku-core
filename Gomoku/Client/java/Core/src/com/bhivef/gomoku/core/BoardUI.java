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

public class BoardUI {

	public final static int LEFT_MARGIN = 20;
	public final static int TOP_MARGIN = 20;
	public final static int CELL_WIDTH = 20;
	public final static int CELL_HEIGHT = 20;
	public final static int PIECE_WIDTH = 14;
	public final static int PIECE_HEIGHT = 14;
	
	protected final Board board;
	private final List<MoveListener> listeners;
	
	public BoardUI(final Board board) {
		this.board = board;
		this.listeners = new ArrayList<MoveListener>();
	}
	
	public void update() {
	}
	
	protected void draw() {
		for (int r = 0; r <= board.getHeight(); r++) {
			drawLine(LEFT_MARGIN, TOP_MARGIN + r * CELL_HEIGHT, LEFT_MARGIN + board.getWidth() * CELL_WIDTH, TOP_MARGIN + r * CELL_HEIGHT);
		}
		for (int c = 0; c <= board.getWidth(); c++) {
			drawLine(LEFT_MARGIN + c * CELL_WIDTH, TOP_MARGIN, LEFT_MARGIN + c * CELL_WIDTH, TOP_MARGIN + board.getHeight() * CELL_HEIGHT);
		}
		for (int r = 0; r < board.getHeight(); r++) {
			drawText(0, TOP_MARGIN + r * CELL_HEIGHT, "" + r);
		}
		for (int c = 0; c < board.getWidth(); c++) {
			drawText(LEFT_MARGIN + c * CELL_WIDTH, 0, "" + c);
		}
		for (int r = 0; r < board.getHeight(); r++) {
			for (int c = 0; c < board.getWidth(); c++) {
				byte piece = board.getPiece(r, c);
				if (piece == Board.WHITE) {
					drawOval(LEFT_MARGIN + c * CELL_WIDTH + (CELL_WIDTH - PIECE_WIDTH) / 2, TOP_MARGIN + r * CELL_HEIGHT + (CELL_HEIGHT - PIECE_HEIGHT) / 2, PIECE_WIDTH, PIECE_HEIGHT);
				}
				if (piece == Board.BLACK) {
					drawOval(LEFT_MARGIN + c * CELL_WIDTH + (CELL_WIDTH - PIECE_WIDTH) / 2, TOP_MARGIN + r * CELL_HEIGHT + (CELL_HEIGHT - PIECE_HEIGHT) / 2, PIECE_WIDTH, PIECE_HEIGHT);
					fillOval(LEFT_MARGIN + c * CELL_WIDTH + (CELL_WIDTH - PIECE_WIDTH) / 2, TOP_MARGIN + r * CELL_HEIGHT + (CELL_HEIGHT - PIECE_HEIGHT) / 2, PIECE_WIDTH, PIECE_HEIGHT);
				}
			}
		}
	}
	
	protected void drawLine(final int x1, final int y1, final int x2, final int y2) {
	}

	protected void drawText(final int x, final int y, final String text) {
	}

	protected void drawOval(final int x, final int y, final int width, final int height) {
	}

	protected void fillOval(final int x, final int y, final int width, final int height) {
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
	
}
