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

package com.bhivef.gomoku.player.application.swing;

import com.bhivef.gomoku.ai.JamesCook;
import com.bhivef.gomoku.ai.JohnSmith;
import com.bhivef.gomoku.core.Board;
import com.bhivef.gomoku.core.BoardUI;
import com.bhivef.gomoku.core.Client;
import com.bhivef.gomoku.core.Config;
import com.bhivef.gomoku.core.Player;
import com.bhivef.gomoku.core.RemotePlayer;

public class Game extends com.bhivef.gomoku.core.Game {

	public final static byte JOHN_SMITH = 3;
	public final static byte JAMES_COOK = 4;
	
	public Game(final Config config, final Board board, final BoardUI boardUI, final Client client) {
		super(config, board, boardUI, client);
	}
	
	protected Player createUnknownPlayer(final boolean hasRemote, final byte type, final byte piece) {
		if (hasRemote) {
			if (type == JOHN_SMITH) {
				return new RemotePlayer(config, client, board, new JohnSmith(config, board, piece));
			} else if (type == JAMES_COOK) {
				return new RemotePlayer(config, client, board, new JamesCook(config, board, piece));
			} else {
				return super.createUnknownPlayer(hasRemote, type, piece);
			}
		} else {
			if (type == JOHN_SMITH) {
				return new JohnSmith(config, board, piece);
			} else if (type == JAMES_COOK) {
				return new JamesCook(config, board, piece);
			} else {
				return super.createUnknownPlayer(hasRemote, type, piece);
			}
		}
	}

}
