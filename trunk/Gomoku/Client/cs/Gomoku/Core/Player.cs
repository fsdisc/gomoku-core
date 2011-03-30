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
    public class Player
    {
	    protected Config config;
	    protected Board board;
	    private byte piece;
	    private List<MoveListener> listeners;
	    private bool finished;
	    private bool ready;
	    protected bool disposed;
	
	    public Player(Config config, Board board, byte piece) 
        {
		    this.config = config;
		    this.board = board;
		    this.piece = piece;
		    this.listeners = new List<MoveListener>();
		    this.finished = false;
		    this.ready = false;
		    this.disposed = false;
	    }
	
	    public byte getPiece() 
        {
		    return piece;
	    }
	
	    public void makeMove(Move move) 
        {
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
		    switch (victory) 
            {
		    case Board.BLACK_WIN:
		    case Board.WHITE_WIN:
		    case Board.DRAW:
			    fireLastMove(victory);
                break;
		    }
	    }
	
	    public virtual void think(Move move) 
        {
	    }
	
	    public bool getReady() 
        {
		    return ready;
	    }
	
	    public virtual void setReady() 
        {
		    ready = true;
	    }
	
	    public bool getFinished() 
        {
		    return finished;
	    }
	
	    public virtual void setFinished() 
        {
		    finished = true;
	    }
	
	    public virtual bool checkReady() 
        {
		    return true;
	    }
	
	    public virtual void dispose() 
        {
		    disposed = true;
		    listeners.Clear();
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
	
	    protected void fireLastMove(byte victory) 
        {
		    finished = true;
		    for (int i = 0; i < listeners.Count; i++) 
            {
			    listeners[i].lastMove(victory);
		    }
	    }

    }
}
