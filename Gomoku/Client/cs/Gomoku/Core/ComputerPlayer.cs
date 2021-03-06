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
    public class ComputerPlayer : Player
    {
	    protected int MAXWIN;
	    protected int MINWIN;
	
	    protected int searchDepth;
	    protected int moveCount;
	
	    public ComputerPlayer(Config config, Board board, byte piece) : base(config, board, piece)
        {
		    this.searchDepth = config.getInt(Config.SEARCH_DEPTH);
		    this.moveCount = 0;
    		this.MAXWIN = getMaxWin();
	    	this.MINWIN = getMinWin();
	    }

	    public override void think(Move move) 
        {
		    if (!getReady()) return;
		    think(move, 0, board, getPiece(), MINWIN - 1, MAXWIN + 1);
		    if (move.getPiece() == Board.BLANK) 
            {
			    if (moveCount == 0) 
                {
				    move.clone(new Move(board.getHeight() / 2, board.getWidth() / 2, getPiece()));
				    makeMove(move);
				    moveCount++;
			    }
		    } 
            else 
            {
			    makeMove(move);
			    moveCount++;
		    }
	    }
	
	    protected virtual int getMaxWin() 
        {
		    return 10000;
	    }

	    protected virtual int getMinWin() 
        {
		    return -10000;
	    }
	
	    protected virtual int think(Move move, int depth, Board board, byte piece, int alpha, int beta) 
        {
		    if (depth == searchDepth) 
            {
			    move.clear();
			    return eval(board);
		    }
		
	        int max = MINWIN - 1;
	        int min = MAXWIN + 1;
		    Move nextMove = new Move();
	        int moveVal = 0;
		    while(nextPossible(nextMove, board, piece)) 
            {
			    Board nextBoard = board.clone();
			    nextBoard.setPiece(nextMove.getRow(), nextMove.getColumn(), piece);
			    byte victory = nextBoard.victory();
			    if (victory == Board.BLACK_WIN) 
                {
				    if (getPiece() == Board.BLACK) 
                    {
					    moveVal = MAXWIN;
				    } 
                    else 
                    {
					    moveVal = MINWIN;
				    }
			    } 
                else if (victory == Board.WHITE_WIN) 
                {
				    if (getPiece() == Board.WHITE) 
                    {
					    moveVal = MAXWIN;
				    } 
                    else 
                    {
					    moveVal = MINWIN;
				    }
			    } 
                else 
                {
				    moveVal = think(new Move(), depth + 1, nextBoard, Board.opponentPiece(piece), alpha, beta);
			    }
			    if (piece == getPiece()) 
                {
		            if (moveVal > max) 
                    {
					    move.clone(nextMove);
		                max = moveVal;
		            }
				    alpha = alpha > moveVal ? alpha : moveVal;
				    if (alpha >= beta) 
                    {
					    return beta;
				    }
			    } 
                else 
                {
		            if (moveVal < min) 
                    {
					    move.clone(nextMove);
		                min = moveVal;
		            }
				    beta = beta < moveVal ? beta : moveVal;
				    if (beta <= alpha) 
                    {
					    return alpha;
				    }
			    }
		    }
		
		    return piece == getPiece() ? alpha : beta;
	    }
	
	    protected virtual bool nextPossible(Move move, Board board, byte piece) 
        {
		    int row = 0;
		    int column = 0;
		
		    if (move.getPiece() != Board.BLANK) 
            {
			    row = move.getRow();
			    column = move.getColumn() + 1;
			    if (column == board.getWidth()) 
                {
				    row++;
				    column = 0;
			    }
		    }
		
		    while (row < board.getHeight()) 
            {
			    while (column < board.getWidth()) 
                {
				    if (board.getPiece(row, column) == Board.BLANK && board.hasAdjacentPieces(row, column)) 
                    {
					    move.clone(new Move(row, column, piece));
					    return true;
				    }
				    column++;
			    }
			    column = 0;
			    row++;
		    }
		
		    return false;
	    }
	
	    protected virtual int eval(Board b) 
        {
		    Score p = new Score(b, getPiece());
		    Score o = new Score(b, Board.opponentPiece(getPiece()));
		    int retVal = 0;

		    if (o.uncapped4 > 0)  return MINWIN;
		    if (p.uncapped4 > 0)  return MAXWIN;

		    retVal += p.capped2 * 5;
		    retVal -= o.capped2 * 5;

		    retVal += p.uncapped2 * 10;
		    retVal -= o.uncapped2 * 10;

		    retVal += p.capped3 * 20;
		    retVal -= o.capped3 * 30;

		    retVal += p.uncapped3 * 100;
		    retVal -= o.uncapped3 * 120;

		    retVal += p.capped4 * 500;
		    retVal -= o.capped4 * 500;

		    return Math.Max(MINWIN, Math.Min(MAXWIN, retVal));
	    }
	
	    protected class Score 
        {
		    public Score(Board board, byte piece) 
            {
			    uncapped2 = board.findRow(piece, 2, 0);
			    capped2 = board.findRow(piece, 2, 1);

			    uncapped3 = board.findRow(piece, 3, 0);
			    capped3 = board.findRow(piece, 3, 1);

			    uncapped4 = board.findRow(piece, 4, 0);
			    capped4 = board.findRow(piece, 4, 1);
		    }

		    public int capped2;
		    public int uncapped2;

		    public int capped3;
		    public int uncapped3;

		    public int capped4;
		    public int uncapped4;
	    }

    }
}
