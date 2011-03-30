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
    public class JohnSmith : ComputerPlayer
    {
    	public JohnSmith(Config config, Board board, byte piece) : base(config, board, piece)
        {
	    }
	
	    protected override int getMaxWin() 
        {
		    return 100000;
	    }

	    protected override int getMinWin() 
        {
		    return -100000;
	    }

	    protected override int eval(Board b) 
        {
		    Score p = new Score(b, getPiece());
		    Score o = new Score(b, Board.opponentPiece(getPiece()));
		    int retVal = 0;

		    if (o.uncapped4 > 0)  return MINWIN;
		    if (p.uncapped4 > 0)  return MAXWIN;

		    retVal += p.capped2 * 1;
		    retVal -= o.capped2 * 5;

		    retVal += p.uncapped2 * 10;
		    retVal -= o.uncapped2 * 10;

		    retVal += p.capped3 * 100;
		    retVal -= o.capped3 * 100;

		    retVal += p.uncapped3 * 1000;
		    retVal -= o.uncapped3 * 1000;

		    retVal += p.capped4 * 10000;
		    retVal -= o.capped4 * 10000;

		    return Math.Max(MINWIN, Math.Min(MAXWIN, retVal));
	    }

    }
}
