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

namespace com.bhivef.gomoku.core
{
    public class BoardUI
    {
	    public const int LEFT_MARGIN = 20;
	    public const int TOP_MARGIN = 20;
	    public const int CELL_WIDTH = 20;
	    public const int CELL_HEIGHT = 20;
	    public const int PIECE_WIDTH = 14;
	    public const int PIECE_HEIGHT = 14;
	
	    protected Board board;
	    private List<MoveListener> listeners;
	
	    public BoardUI(Board board) 
        {
		    this.board = board;
		    this.listeners = new List<MoveListener>();
	    }
	
	    public virtual void update() 
        {
	    }
	
	    protected virtual void draw() 
        {
		    for (int r = 0; r <= board.getHeight(); r++) 
            {
			    drawLine(LEFT_MARGIN, TOP_MARGIN + r * CELL_HEIGHT, LEFT_MARGIN + board.getWidth() * CELL_WIDTH, TOP_MARGIN + r * CELL_HEIGHT);
		    }
		    for (int c = 0; c <= board.getWidth(); c++) 
            {
			    drawLine(LEFT_MARGIN + c * CELL_WIDTH, TOP_MARGIN, LEFT_MARGIN + c * CELL_WIDTH, TOP_MARGIN + board.getHeight() * CELL_HEIGHT);
		    }
		    for (int r = 0; r < board.getHeight(); r++) 
            {
			    drawText(0, TOP_MARGIN + r * CELL_HEIGHT, "" + r);
		    }
		    for (int c = 0; c < board.getWidth(); c++) 
            {
			    drawText(LEFT_MARGIN + c * CELL_WIDTH, 0, "" + c);
		    }
		    for (int r = 0; r < board.getHeight(); r++) 
            {
			    for (int c = 0; c < board.getWidth(); c++) 
                {
				    byte piece = board.getPiece(r, c);
				    if (piece == Board.WHITE) 
                    {
					    drawOval(LEFT_MARGIN + c * CELL_WIDTH + (CELL_WIDTH - PIECE_WIDTH) / 2, TOP_MARGIN + r * CELL_HEIGHT + (CELL_HEIGHT - PIECE_HEIGHT) / 2, PIECE_WIDTH, PIECE_HEIGHT);
				    }
				    if (piece == Board.BLACK) 
                    {
					    drawOval(LEFT_MARGIN + c * CELL_WIDTH + (CELL_WIDTH - PIECE_WIDTH) / 2, TOP_MARGIN + r * CELL_HEIGHT + (CELL_HEIGHT - PIECE_HEIGHT) / 2, PIECE_WIDTH, PIECE_HEIGHT);
					    fillOval(LEFT_MARGIN + c * CELL_WIDTH + (CELL_WIDTH - PIECE_WIDTH) / 2, TOP_MARGIN + r * CELL_HEIGHT + (CELL_HEIGHT - PIECE_HEIGHT) / 2, PIECE_WIDTH, PIECE_HEIGHT);
				    }
			    }
		    }
	    }
	
	    protected virtual void drawLine(int x1, int y1, int x2, int y2) 
        {
	    }

	    protected virtual void drawText(int x, int y, String text) 
        {
	    }

	    protected virtual void drawOval(int x, int y, int width, int height) 
        {
	    }

	    protected virtual void fillOval(int x, int y, int width, int height) 
        {
	    }
	
	    public void addMoveListener(MoveListener src) 
        {
		    listeners.Add(src);
	    }
	
	    public void clearListeners() 
        {
		    listeners.Clear();
	    }
	
	    protected void fireMoveMade(Move move) 
        {
		    for (int i = 0; i < listeners.Count; i++) 
            {
			    listeners[i].moveMade(move);
		    }
	    }

    }
}
