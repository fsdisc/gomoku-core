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

public class Cell {

	private byte row;
	private byte column;
	
	public Cell() {
		this.row = 0;
		this.column = 0;
	}
	
	public Cell(final int row, final int column) {
		this.row = (byte)row;
		this.column = (byte)column;
	}
	
	public byte getRow() {
		return row;
	}
	
	public byte getColumn() {
		return column;
	}
	
	public boolean equals(Cell cell) {
		return row == cell.getRow() && column == cell.getColumn();
	}
	
	public Cell clone() {
		return new Cell(row, column);
	}
	
	public void clone(Cell cell) {
		row = cell.getRow();
		column = cell.getColumn();
	}
	
	public void clear() {
		row = 0;
		column = 0;
	}
	
	public String toString() {
		return "(" + row + ", " + column + ")";
	}
	
}
