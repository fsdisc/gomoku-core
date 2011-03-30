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

public class Board {

	public final static byte BLANK = 0;
	public final static byte BLACK = 1;
	public final static byte WHITE = 2;
	
	public final static byte GO_WIDTH = 19;
	public final static byte GO_HEIGHT = 19;

	public final static byte DRAW = 0;
	public final static byte BLACK_WIN = 1;
	public final static byte WHITE_WIN = 2;
	public final static byte NO_WIN = 3;
	
	private byte width;
	private byte height;
	private byte[][] pieces;
	private byte currentPiece;
	
	public Board() {
		this.width = GO_WIDTH;
		this.height = GO_HEIGHT;
		this.pieces = new byte[this.height][this.width];
		clear();
	}
	
	public Board(final int width, final int height) {
		this.width = (byte)width;
		this.height = (byte)height;
		this.pieces = new byte[this.height][this.width];
		clear();
	}
	
	public byte getWidth() {
		return width;
	}
	
	public byte getHeight() {
		return height;
	}
	
	public byte getPiece(final int row, final int column) {
		return pieces[row][column];
	}
	
	public void setPiece(final int row, final int column, final byte piece) {
		pieces[row][column] = piece;
	}
	
	public byte getCurrentPiece() {
		return currentPiece;
	}
	
	public void setCurrentPiece(final byte piece) {
		currentPiece = piece;
	}
	
	public void clear() {
		for (int r = 0; r < height; r++) {
			for (int c = 0; c < width; c++) {
				pieces[r][c] = BLANK;
			}
		}
		currentPiece = BLACK;
	}
	
	public void resize(final int width, final int height) {
		this.width = (byte)width;
		this.height = (byte)height;
		this.pieces = new byte[this.height][this.width];
		clear();
	}
	
	public Board clone() {
		Board tag = new Board(width, height);
		for (int r = 0; r < height; r++) {
			for (int c = 0; c < width; c++) {
				tag.setPiece(r, c, pieces[r][c]);
			}
		}
		return tag;
	}
	
	public static byte opponentPiece(final byte piece) {
		return piece == BLACK ? WHITE : BLACK;
	}
	
	public static String playerName(final byte piece) {
		switch (piece) {
		case BLACK:
			return "Black";
		case WHITE:
			return "White";
		}
		return "";
	}
	
	public byte victory() {
		int blankCount = width * height;
		for (int r = 0; r < height; r++) {
			for (int c = 0; c < width; c++) {
				if (pieces[r][c] != BLANK) {
					blankCount--;
					switch (victory(r, c)) {
					case BLACK_WIN:
						return BLACK_WIN;
					case WHITE_WIN:
						return WHITE_WIN;
					}
				}
			}
		}
		return blankCount == 0 ? DRAW : NO_WIN;
	}
	
	public byte victory(final int row, final int column) {
		byte piece = pieces[row][column];
		if (piece == BLANK)  return NO_WIN;
		if (findRow(piece, row, column, 5, 2) > 0) {
			return piece == BLACK ? BLACK_WIN : WHITE_WIN;
		}
		return NO_WIN;
	}
	
	public boolean hasAdjacentPieces(final int row, final int column) {
		for (int r = row - 1; r <= row + 1 && r >= 0 && r < height; r++) {
			for (int c = column - 1; c <= column + 1 && c >= 0 && c < width; c++) {
				if (r == row && c == column) continue;
				if (pieces[r][c] != BLANK) return true;
			}
		}
		return false;
	}
	
	public int findRow(final byte piece, final int size, final int maxcaps) {
		int count = 0;
		for (int r = 0; r < height; r++) {
			for (int c = 0; c < width; c++) {
				count += findRow(piece, r, c, size, maxcaps);
			}
		}
		return count;
	}
	
	public byte findRow(final byte piece, final int row, final int column, final int size, final int maxcaps) {
		if (pieces[row][column] != piece) return 0;
		
		byte count = 0;
		byte opponentPiece = opponentPiece(piece);
		boolean found = false;
		int c, r, capCount;
		
		// Check right
		if (column + size <= width) {
			found = true;
			for (c = column + 1; c < column + size && found; c++) {
				if (pieces[row][c] != piece)  found = false;
			}
			if (found && column > 0 && pieces[row][column - 1] == piece) {
				found = false;
			}
			if (found && column + size < width && pieces[row][column + size] == piece) {
				found = false;
			}
			if (found) {
				capCount = 0;
				if (column == 0 || pieces[row][column - 1] == opponentPiece) {
					capCount++;
				}
				if (column + size == width || pieces[row][column + size] == opponentPiece) {
					capCount++;
				}
				if (capCount <= maxcaps) {
					count++;
				}
			}
		}
		
		// Check down
		if (row + size <= height) {
			found = true;
			for (r = row + 1; r < row + size && found; r++) {
				if (pieces[r][column] != piece)  found = false;
			}
			if (found && row > 0 && pieces[row - 1][column] == piece) {
				found = false;
			}
			if (found && row + size < height && pieces[row + size][column] == piece) {
				found = false;
			}
			if (found) {
				capCount = 0;
				if (row == 0 || pieces[row - 1][column] == opponentPiece) {
					capCount++;
				}
				if (row + size == height || pieces[row + size][column] == opponentPiece) {
					capCount++;
				}
				if (capCount <= maxcaps) {
					count++;
				}
			}
		}

		// Check down-right
		if (row + size <= height && column + size <= width) {
			found = true;
			for (r = row + 1, c = column + 1; r < row + size && c < column + size && found; r++, c++) {
				if (pieces[r][c] != piece)  found = false;
			}
			if (found && row > 0 && column > 0 && pieces[row - 1][column - 1] == piece) {
				found = false;
			}
			if (found && row + size < height && column + size < width && pieces[row + size][column + size] == piece) {
				found = false;
			}
			if (found) {
				capCount = 0;
				if (row == 0 || column == 0 || pieces[row - 1][column - 1] == opponentPiece) {
					capCount++;
				}
				if (row + size == height || column + size  == width || pieces[row + size][column + size] == opponentPiece) {
					capCount++;
				}
				if (capCount <= maxcaps) {
					count++;
				}
			}
		}
		
		// Check down-left
		if (row + size <= height && column - size >= -1) {
			found = true;
			for (r = row + 1, c = column - 1; r < row + size && c >= 0 && found; r++, c--) {
				if (pieces[r][c] != piece)  found = false;
			}
			if (found && row > 0 && column < width - 1 && pieces[row - 1][column + 1] == piece) {
				found = false;
			}
			if (found && row + size < height && column - size >= 0 && pieces[row + size][column - size] == piece) {
				found = false;
			}
			if (found) {
				capCount = 0;
				if (row == 0 || column == width - 1 || pieces[row - 1][column + 1] == opponentPiece) {
					capCount++;
				}
				if (row + size == height || column - size == -1 || pieces[row + size][column - size] == opponentPiece) {
					capCount++;
				}
				if (capCount <= maxcaps) {
					count++;
				}
			}
		}
		
		return count;
	}
	
}
