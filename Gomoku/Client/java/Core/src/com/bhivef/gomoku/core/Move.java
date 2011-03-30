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

public class Move extends Cell {

	private byte piece;
	
	public Move() {
		super();
		this.piece = Board.BLANK;
	}
	
	public Move(final int row, final int column, final byte piece) {
		super(row, column);
		this.piece = piece;
	}
	
	public byte getPiece() {
		return piece;
	}
	
	public boolean equals(Move move) {
		return getRow() == move.getRow() && getColumn() == move.getColumn() && piece == move.getPiece();
	}
	
	public Move clone() {
		return new Move(getRow(), getColumn(), piece);
	}
	
	public void clone(Move move) {
		super.clone(move);
		piece = move.getPiece();
	}
	
	public void clear() {
		super.clear();
		piece = Board.BLANK;
	}
	
	public String toString() {
		return Board.playerName(piece) + " " + super.toString();
	}
	
}
