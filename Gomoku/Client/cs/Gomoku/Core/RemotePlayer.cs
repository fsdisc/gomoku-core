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
    public class RemotePlayer : Player
    {
	    protected Player player;
	    protected Client client;

	    public RemotePlayer(Config config, Client client, Board board, byte piece) : base(config, board, piece)
        {
		    this.player = null;
		    this.client = client;
	    }

	    public RemotePlayer(Config config, Client client, Board board, Player player) : base(config, board, player.getPiece())
        {
		    this.player = player;
		    this.client = client;
            this.player.addMoveListener(new MoveAdapter(this));
	    }
	
	    public override void think(Move move) 
        {
		    if (player != null) 
            {
			    player.think(move);
			    return;
		    }
		    while (!disposed) 
            {
			    Move lastMove = client.lastMove();
			    if (lastMove.getPiece() == getPiece()) 
                {
				    makeMove(lastMove);
				    return;
			    }
			    try 
                {
				    System.Threading.Thread.Sleep(1000);
			    } 
                catch { }
		    }
	    }
	
	    public override void dispose() 
        {
		    base.dispose();
		    if (player != null) 
            {
			    player.dispose();
		    }
	    }
	
	    public override void setReady() 
        {
		    base.setReady();
		    if (player != null) 
            {
			    player.setReady();
		    }
	    }
	
	    public override void setFinished() 
        {
		    base.setFinished();
		    if (player != null) 
            {
			    player.setFinished();
		    }
	    }
	
	    public override bool checkReady() 
        {
		    try 
            {
			    GameState state = new GameState();
			    client.gameState(state);
			    return state.Joined && !state.Cancelled && !state.Finished;
		    } 
            catch 
            {
			    return false;
		    }
	    }

        private class MoveAdapter : MoveListener
        {
            private RemotePlayer player;

            public MoveAdapter(RemotePlayer player)
            {
                this.player = player;
            }

            public void moveMade(Move move)
            {
                if (player.getPiece() != move.getPiece()) return;
                try
                {
                    player.client.makeMove(move);
                    player.fireMoveMade(move);
                }
                catch { }
            }

            public void lastMove(byte victory)
            {
                player.fireLastMove(victory);
            }
        }

    }
}
