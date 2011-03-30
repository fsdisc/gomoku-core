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
    public class HumanPlayer : Player
    {
	    protected BoardUI boardUI;
	
	    public HumanPlayer(Config config, Board board, byte piece, BoardUI boardUI) : base(config, board, piece)
        {
    		this.boardUI = boardUI;
            this.boardUI.addMoveListener(new MoveAdapter(this));
	    }
	
        private class MoveAdapter : MoveListener
        {
            private HumanPlayer player;

            public MoveAdapter(HumanPlayer player)
            {
                this.player = player;
            }

            public void moveMade(Move move)
            {
				if (player.getPiece() != move.getPiece()) return;
				player.makeMove(move);
            }

            public void lastMove(byte victory)
            {
            }
        }

    }
}
