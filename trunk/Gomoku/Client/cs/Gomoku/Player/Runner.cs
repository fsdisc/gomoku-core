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

namespace com.bhivef.gomoku.players
{
    public class Runner : com.bhivef.gomoku.core.players.Runner
    {

        protected override void createConfig()
        {
            base.createConfig();
            config.setValue(core.Config.SEARCH_DEPTH, 2);
            config.setValue(core.Config.SERVER_URL, "http://bhivef.com/gomoku/");
            config.setValue(core.Config.SERVER_EXTENSION, ".php");
        }

	    protected override void constructGame() 
        {
		    game = new Game(config, board, boardUI, client);
	    }
	
	    protected override String getTitle() 
        {
		    return "Gomoku Player";
	    }
	
	    protected override String[] getPlayerNames(bool newGame) 
        {
		    if (newGame) 
            {
			    return new String[] { "Human", "Computer", "Remote", "Computer (John Smith)", "Computer (James Cook)" };
		    } 
            else 
            {
			    return new String[] { "Human", "Computer", "Computer (John Smith)", "Computer (James Cook)" };
		    }
	    }

	    protected override int getPlayerIndex(bool newGame, byte type) 
        {
		    if (newGame) 
            {
			    switch (type) 
                {
			    case Game.JOHN_SMITH:
				    return 3;
			    case Game.JAMES_COOK:
				    return 4;
			    default:
				    return base.getPlayerIndex(newGame, type);
			    }
		    } 
            else 
            {
			    switch (type) 
                {
			    case Game.JOHN_SMITH:
    				return 2;
	    		case Game.JAMES_COOK:
		    		return 3;
			    default:
				    return base.getPlayerIndex(newGame, type);
			    }
		    }
	    }
	
	    protected override byte getPlayerType(bool newGame, int index) 
        {
		    if (newGame) 
            {
			    byte type = base.getPlayerType(newGame, index);
			    switch (index) 
                {
			    case 3:
				    type = Game.JOHN_SMITH;
				    break;
			    case 4:
				    type = Game.JAMES_COOK;
				    break;
			    }
			    return type;
		    } 
            else 
            {
			    byte type = base.getPlayerType(newGame, index);
			    switch (index) 
                {
			    case 2:
				    type = Game.JOHN_SMITH;
				    break;
			    case 3:
				    type = Game.JAMES_COOK;
				    break;
			    }
			    return type;
		    }
	    }
	
	    protected override bool isRemotePlayer(int index) 
        {
		    return index == 2;
	    }

    }
}
