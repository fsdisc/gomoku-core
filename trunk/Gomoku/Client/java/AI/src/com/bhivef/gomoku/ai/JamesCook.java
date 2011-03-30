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

package com.bhivef.gomoku.ai;

import java.util.ArrayList;
import java.util.List;

import com.bhivef.gomoku.core.Board;
import com.bhivef.gomoku.core.Cell;
import com.bhivef.gomoku.core.Config;
import com.bhivef.gomoku.core.Move;
import com.bhivef.gomoku.core.Player;

public class JamesCook extends Player {

	protected final static int WIN_COUNT = 5;
	protected List<Line> lines;
	
	public JamesCook(final Config config, final Board board, final byte piece) {
		super(config, board, piece);
	}
	
	public void think(final Move move) {
		if (!getReady()) return;
		Cell cell = findBestMoves(board);
		if (cell != null) {
			makeMove(new Move(cell.getRow(), cell.getColumn(), getPiece()));
		}
	}

	public List<Line> getLines() {
		if (lines != null)  return lines;
		
		lines = new ArrayList<Line>();
		
		for (int i = 0; i < board.getHeight(); i++) {
			Line line = new Line();
			for (int j = 0; j < board.getWidth(); j++) {
				line.add(i, j);
			}
			lines.add(line);
		}
		for (int j = 0; j < board.getWidth(); j++) {
			Line line = new Line();
			for (int i = 0; i < board.getHeight(); i++) {
				line.add(i, j);
			}
			lines.add(line);
		}
		for (int n = 0; n < board.getHeight(); n++) {
			int i = n;
			int j = 0;
			Line line = new Line();
			while (i < board.getHeight() && j < board.getWidth()) {
				line.add(i, j);
				i++;
				j++;
			}
			lines.add(line);
		}
		for (int n = 1; n < board.getWidth(); n++) {
			int i = 0;
			int j = n;
			Line line = new Line();
			while (i < board.getHeight() && j < board.getWidth()) {
				line.add(i, j);
				i++;
				j++;
			}
			lines.add(line);
		}
		for (int n = 0; n < board.getHeight(); n++) {
			int i = n;
			int j = board.getWidth() - 1;
			Line line = new Line();
			while (i < board.getHeight() && j >= 0) {
				line.add(i, j);
				i++;
				j--;
			}
			lines.add(line);
		}
		for (int n = board.getWidth() - 2; n >= 0; n--) {
			int i = 0;
			int j = n;
			Line line = new Line();
			while (i < board.getHeight() && j >= 0) {
				line.add(i, j);
				i++;
				j--;
			}
			lines.add(line);
		}
		
		return lines;
	}
	
	protected List<Cell> getBlanks() {
		List<Cell> tag = new ArrayList<Cell>();
		
		for (int i = 0; i < board.getHeight(); i++) {
			for (int j = 0; j < board.getWidth(); j++) {
				if (board.getPiece(i, j) == Board.BLANK) {
					tag.add(new Cell(i, j));
				}
			}
		}
		
		return tag;
	}
	
	protected Cell findBestMoves(Board board) {
		List<Cell> computerMoves = new ArrayList<Cell>();
		List<Cell> humanMoves = new ArrayList<Cell>();
		int computerCounter = findBestMoves(getPiece(), board, computerMoves);
		int humanCounter = findBestMoves(getPiece() == Board.BLACK ? Board.WHITE : Board.BLACK, board, humanMoves);
		
		if (humanMoves.size() == 0) {
			if (computerMoves.size() > 0) {
				return computerMoves.get(0).clone();
			} else {
				List<Cell> blanks = getBlanks();
				if (blanks.size() == board.getWidth() * board.getHeight()) {
					int row = board.getHeight() / 2;
					int column = board.getWidth() / 2;
					return new Cell(row, column);
				} else if (blanks.size() > 0) {
					return blanks.get(0).clone();
				}
			}
		} else if (computerMoves.size() == 0) {
			return humanMoves.get(0).clone();
		} else {
			if (humanCounter >= computerCounter) {
				return humanMoves.get(0).clone();
			} else {
				return computerMoves.get(0).clone();
			}
		}
		
		return null;
	}
	
	protected int findBestMoves(int srcState, Board board, List<Cell> tag) {
		int counter = 0;
		List<Row> rows = new ArrayList<Row>();
		List<Line> lines = getLines();
		
		for (int i = 0; i < lines.size(); i++) {
			getRows(srcState, board, lines.get(i), rows);
		}
		
		int maxCount = 0;
		for (int i = rows.size() - 1; i >= 0; i--) {
			Row row = rows.get(i);
			if (row.BlankStart.size() == 0 && row.BlankStop.size() == 0) {
				rows.remove(i);
				continue;
			}
			if (row.Cells.size() > WIN_COUNT) {
				rows.remove(i);
				continue;
			}
			if (row.BlankStart.size() + row.BlankStop.size() + row.Cells.size() < WIN_COUNT) {
				rows.remove(i);
				continue;
			}
			if (maxCount < row.Cells.size()) {
				maxCount = row.Cells.size();
			}
		}
		counter = maxCount;
		for (int i = rows.size() - 1; i >= 0; i--) {
			Row row = rows.get(i);
			if (maxCount > row.Cells.size()) {
				rows.remove(i);
				continue;
			}
		}
		for (int i = 0; i < rows.size(); i++) {
			Row row = rows.get(i);
			if (row.BlankStart.size() == 0 || row.BlankStop.size() == 0) continue;
			if (row.BlankStart.size() > 0) {
				tag.add(row.BlankStart.get(row.BlankStart.size() - 1).clone());
			}
			if (row.BlankStop.size() > 0) {
				tag.add(row.BlankStop.get(0).clone());
			}
		}
		if (tag.size() == 0) {
			for (int i = 0; i < rows.size(); i++) {
				Row row = rows.get(i);
				if (row.BlankStart.size() > 0 && row.BlankStop.size() > 0) continue;
				if (row.BlankStart.size() > 0) {
					tag.add(row.BlankStart.get(row.BlankStart.size() - 1).clone());
				}
				if (row.BlankStop.size() > 0) {
					tag.add(row.BlankStop.get(0).clone());
				}
			}
		}
		
		return counter;
	}
	
	protected void getRows(int srcState, Board board, Line line, List<Row> tag) {
		Row row = new Row();
		boolean start = false;
		
		for (int i = 0; i < line.size(); i++) {
			int tagState = board.getPiece(line.get(i).getRow(), line.get(i).getColumn());
			if (start) {
				if (tagState == srcState) {
					row.Cells.add(line.get(i).clone());
					if (i == line.size() - 1) {
						tag.add(row);
						row = new Row();
					}
				} else {
					for (int j = i; j < line.size(); j++) {
						if (board.getPiece(line.get(j).getRow(), line.get(j).getColumn()) == Board.BLANK) {
							row.BlankStop.add(line.get(j).clone());
						} else {
							break;
						}
					}
					tag.add(row);
					row = new Row();
					if (tagState == Board.BLANK) {
						row.BlankStart.add(line.get(i).clone());
					}
					start = false;
				}
			} else {
				if (tagState == srcState) {
					start = true;
					row.Cells.add(line.get(i).clone());
				} else if (tagState == Board.BLANK) {
					row.BlankStart.add(line.get(i).clone());
				} else {
					row = new Row();
				}
			}
		}
	}

	protected static class Row {

		public List<Cell> Cells = new ArrayList<Cell>();
		public List<Cell> BlankStart = new ArrayList<Cell>();
		public List<Cell> BlankStop = new ArrayList<Cell>();
		
		public String toString() {
			String tag = "";
			tag += "\r\nCells: ";
			for (int i = 0; i < Cells.size(); i++) {
				tag += Cells.get(i).toString() + " ";
			}
			tag += "\r\nBlankStart: ";
			for (int i = 0; i < BlankStart.size(); i++) {
				tag += BlankStart.get(i).toString() + " ";
			}
			tag += "\r\nBlankStop: ";
			for (int i = 0; i < BlankStop.size(); i++) {
				tag += BlankStop.get(i).toString() + " ";
			}
			return tag;
		}
		
	}
	
	protected static class Line {

		private List<Cell> cells;
		
		public Line() {
			cells = new ArrayList<Cell>();
		}
		
		public int size() {
			return cells.size();
		}
		
		public Cell get(int idx) {
			return cells.get(idx).clone();
		}
		
		public void add(int row, int column) {
			cells.add(new Cell(row, column));
		}
		
		public void add(Cell src) {
			cells.add(src.clone());
		}
		
		public void remove(int idx) {
			cells.remove(idx);
		}
		
		public int indexOf(Cell src) {
			for (int i = 0; i < cells.size(); i++) {
				if (cells.get(i).equals(src)) return i;
			}
			return -1;
		}
		
		public int indexOf(int row, int column) {
			return indexOf(new Cell(row, column));
		}
		
	}
	
}
