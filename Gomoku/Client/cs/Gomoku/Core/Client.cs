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
    public class Client
    {
	    protected Config config;
	
	    public Client(Config config) 
        {
		    this.config = config;
	    }
	
	    public virtual void login() 
        {
	    }
	
	    public virtual void logout() 
        {
	    }
	
	    public virtual bool online() 
        {
		    return false;
	    }
	
	    public virtual void clone(String session) 
        {
	    }
	
	    public virtual String createGame(bool playFirst, int width, int height)
        {
		    return "";
	    }
	
	    public virtual void joinGame(String gameId) 
        {
	    }
	
	    public virtual void listGame(List<String> tag) 
        {
	    }
	
	    public virtual Move lastMove() 
        {
		    return new Move();
	    }
	
	    public virtual void makeMove(Move move) 
        {
	    }
	
	    public virtual void gameState(GameState state) 
        {
	    }
	
	    public virtual void endGame(bool finished, byte victory) 
        {
	    }

    }
}
