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
using com.bhivef.gomoku.core;

namespace com.bhivef.gomoku.ai
{
    public class JamesCook : Player
    {
	    protected const int WIN_COUNT = 5;
	    protected List<Line> lines;
	
	    public JamesCook(Config config, Board board, byte piece) : base(config, board, piece)
        {
	    }
	
	    public override void think(Move move) 
        {
		    if (!getReady()) return;
		    Cell cell = findBestMoves(board);
		    if (cell != null) 
            {
			    makeMove(new Move(cell.getRow(), cell.getColumn(), getPiece()));
		    }
	    }

	    protected List<Line> getLines() 
        {
		    if (lines != null)  return lines;
		
		    lines = new List<Line>();
		
		    for (int i = 0; i < board.getHeight(); i++) 
            {
			    Line line = new Line();
			    for (int j = 0; j < board.getWidth(); j++) 
                {
				    line.add(i, j);
			    }
			    lines.Add(line);
		    }
		    for (int j = 0; j < board.getWidth(); j++) 
            {
			    Line line = new Line();
			    for (int i = 0; i < board.getHeight(); i++) 
                {
				    line.add(i, j);
			    }
			    lines.Add(line);
		    }
		    for (int n = 0; n < board.getHeight(); n++) 
            {
			    int i = n;
			    int j = 0;
			    Line line = new Line();
			    while (i < board.getHeight() && j < board.getWidth()) 
                {
				    line.add(i, j);
				    i++;
				    j++;
			    }
			    lines.Add(line);
		    }
		    for (int n = 1; n < board.getWidth(); n++) 
            {
			    int i = 0;
			    int j = n;
			    Line line = new Line();
			    while (i < board.getHeight() && j < board.getWidth()) 
                {
				    line.add(i, j);
				    i++;
				    j++;
			    }
			    lines.Add(line);
		    }
		    for (int n = 0; n < board.getHeight(); n++) 
            {
			    int i = n;
			    int j = board.getWidth() - 1;
			    Line line = new Line();
			    while (i < board.getHeight() && j >= 0) 
                {
				    line.add(i, j);
				    i++;
				    j--;
			    }
			    lines.Add(line);
		    }
		    for (int n = board.getWidth() - 2; n >= 0; n--) 
            {
			    int i = 0;
			    int j = n;
			    Line line = new Line();
			    while (i < board.getHeight() && j >= 0) 
                {
				    line.add(i, j);
				    i++;
				    j--;
			    }
			    lines.Add(line);
		    }
		
		    return lines;
	    }
	
	    protected List<Cell> getBlanks() 
        {
		    List<Cell> tag = new List<Cell>();
		
		    for (int i = 0; i < board.getHeight(); i++) 
            {
			    for (int j = 0; j < board.getWidth(); j++) 
                {
				    if (board.getPiece(i, j) == Board.BLANK) 
                    {
					    tag.Add(new Cell(i, j));
				    }
			    }
		    }
		
		    return tag;
	    }
	
	    protected Cell findBestMoves(Board board) 
        {
		    List<Cell> computerMoves = new List<Cell>();
		    List<Cell> humanMoves = new List<Cell>();
		    int computerCounter = findBestMoves(getPiece(), board, computerMoves);
		    int humanCounter = findBestMoves(getPiece() == Board.BLACK ? Board.WHITE : Board.BLACK, board, humanMoves);
		
		    if (humanMoves.Count == 0) 
            {
			    if (computerMoves.Count > 0) 
                {
				    return computerMoves[0].clone();
			    } 
                else 
                {
				    List<Cell> blanks = getBlanks();
				    if (blanks.Count == board.getWidth() * board.getHeight()) 
                    {
					    int row = board.getHeight() / 2;
					    int column = board.getWidth() / 2;
					    return new Cell(row, column);
				    }
                    else if (blanks.Count > 0) 
                    {
					    return blanks[0].clone();
				    }
			    }
		    } 
            else if (computerMoves.Count == 0) 
            {
			    return humanMoves[0].clone();
		    } 
            else 
            {
			    if (humanCounter >= computerCounter) 
                {
				    return humanMoves[0].clone();
			    } 
                else 
                {
				    return computerMoves[0].clone();
			    }
		    }
		
		    return null;
	    }
	
	    protected int findBestMoves(int srcState, Board board, List<Cell> tag) 
        {
		    int counter = 0;
		    List<Row> rows = new List<Row>();
		    List<Line> lines = getLines();
		
		    for (int i = 0; i < lines.Count; i++) 
            {
			    getRows(srcState, board, lines[i], rows);
		    }
		
		    int maxCount = 0;
		    for (int i = rows.Count - 1; i >= 0; i--) 
            {
			    Row row = rows[i];
			    if (row.BlankStart.Count == 0 && row.BlankStop.Count == 0) 
                {
                    rows.RemoveAt(i);
				    continue;
			    }
			    if (row.Cells.Count > WIN_COUNT) 
                {
                    rows.RemoveAt(i);
				    continue;
			    }
			    if (row.BlankStart.Count + row.BlankStop.Count + row.Cells.Count < WIN_COUNT) 
                {
                    rows.RemoveAt(i);
				    continue;
			    }
			    if (maxCount < row.Cells.Count) 
                {
				    maxCount = row.Cells.Count;
			    }
		    }
		    counter = maxCount;
		    for (int i = rows.Count - 1; i >= 0; i--) 
            {
			    Row row = rows[i];
			    if (maxCount > row.Cells.Count) 
                {
                    rows.RemoveAt(i);
	    			continue;
		    	}
		    }
		    for (int i = 0; i < rows.Count; i++) 
            {
			    Row row = rows[i];
			    if (row.BlankStart.Count == 0 || row.BlankStop.Count == 0) continue;
			    if (row.BlankStart.Count > 0) 
                {
				    tag.Add(row.BlankStart[row.BlankStart.Count - 1].clone());
			    }
			    if (row.BlankStop.Count > 0) 
                {
				    tag.Add(row.BlankStop[0].clone());
			    }
		    }
		    if (tag.Count == 0) 
            {
			    for (int i = 0; i < rows.Count; i++) 
                {
				    Row row = rows[i];
				    if (row.BlankStart.Count > 0 && row.BlankStop.Count > 0) continue;
				    if (row.BlankStart.Count > 0) 
                    {
					    tag.Add(row.BlankStart[row.BlankStart.Count - 1].clone());
				    }
				    if (row.BlankStop.Count > 0) 
                    {
					    tag.Add(row.BlankStop[0].clone());
				    }
			    }
		    }
		
		    return counter;
	    }
	
	    protected void getRows(int srcState, Board board, Line line, List<Row> tag) 
        {
		    Row row = new Row();
		    bool start = false;
		
		    for (int i = 0; i < line.size(); i++) 
            {
			    int tagState = board.getPiece(line.get(i).getRow(), line.get(i).getColumn());
			    if (start) 
                {
				    if (tagState == srcState) 
                    {
					    row.Cells.Add(line.get(i).clone());
					    if (i == line.size() - 1) 
                        {
						    tag.Add(row);
						    row = new Row();
					    }
				    } 
                    else 
                    {
					    for (int j = i; j < line.size(); j++) 
                        {
						    if (board.getPiece(line.get(j).getRow(), line.get(j).getColumn()) == Board.BLANK) 
                            {
							    row.BlankStop.Add(line.get(j).clone());
						    } 
                            else 
                            {
							    break;
						    }
					    }
					    tag.Add(row);
					    row = new Row();
					    if (tagState == Board.BLANK) 
                        {
						    row.BlankStart.Add(line.get(i).clone());
					    }
					    start = false;
				    }
			    } 
                else 
                {
				    if (tagState == srcState) 
                    {
					    start = true;
					    row.Cells.Add(line.get(i).clone());
				    } 
                    else if (tagState == Board.BLANK) 
                    {
					    row.BlankStart.Add(line.get(i).clone());
    				} 
                    else 
                    {
	    				row = new Row();
		    		}
			    }
		    }
	    }

	    protected class Row 
        {
		    public List<Cell> Cells = new List<Cell>();
		    public List<Cell> BlankStart = new List<Cell>();
		    public List<Cell> BlankStop = new List<Cell>();
		
		    public override String ToString() 
            {
			    String tag = "";
			    tag += "\r\nCells: ";
			    for (int i = 0; i < Cells.Count; i++) {
				    tag += Cells[i].ToString() + " ";
			    }
			    tag += "\r\nBlankStart: ";
			    for (int i = 0; i < BlankStart.Count; i++) {
				    tag += BlankStart[i].ToString() + " ";
			    }
			    tag += "\r\nBlankStop: ";
			    for (int i = 0; i < BlankStop.Count; i++) {
				    tag += BlankStop[i].ToString() + " ";
			    }
			    return tag;
		    }
	    }
	
	    protected class Line 
        {
		    private List<Cell> cells;
		
		    public Line() 
            {
			    cells = new List<Cell>();
		    }
		
		    public int size() 
            {
			    return cells.Count;
		    }
		
		    public Cell get(int idx) 
            {
			    return cells[idx].clone();
		    }
		
		    public void add(int row, int column) 
            {
			    cells.Add(new Cell(row, column));
		    }
		
		    public void add(Cell src) 
            {
			    cells.Add(src.clone());
		    }
		
		    public void remove(int idx) 
            {
			    cells.RemoveAt(idx);
		    }
		
		    public int indexOf(Cell src) 
            {
			    for (int i = 0; i < cells.Count; i++) 
                {
				    if (cells[i].equals(src)) return i;
			    }
			    return -1;
		    }
		
		    public int indexOf(int row, int column) 
            {
			    return indexOf(new Cell(row, column));
		    }
	    }

    }
}
