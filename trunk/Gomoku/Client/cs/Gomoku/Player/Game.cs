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
using com.bhivef.gomoku.ai;

namespace com.bhivef.gomoku.players
{
    public class Game : com.bhivef.gomoku.core.Game
    {
	    public const byte JOHN_SMITH = 3;
	    public const byte JAMES_COOK = 4;
	
	    public Game(Config config, Board board, BoardUI boardUI, Client client) : base(config, board, boardUI, client)
        {
	    }
	
	    protected override Player createUnknownPlayer(bool hasRemote, byte type, byte piece) 
        {
		    if (hasRemote) 
            {
			    if (type == JOHN_SMITH) 
                {
				    return new RemotePlayer(config, client, board, new JohnSmith(config, board, piece));
			    } 
                else if (type == JAMES_COOK) 
                {
				    return new RemotePlayer(config, client, board, new JamesCook(config, board, piece));
			    } 
                else 
                {
				    return base.createUnknownPlayer(hasRemote, type, piece);
			    }
		    } 
            else 
            {
			    if (type == JOHN_SMITH) 
                {
				    return new JohnSmith(config, board, piece);
			    } 
                else if (type == JAMES_COOK) 
                {
				    return new JamesCook(config, board, piece);
			    } 
                else 
                {
				    return base.createUnknownPlayer(hasRemote, type, piece);
			    }
		    }
	    }

    }
}
