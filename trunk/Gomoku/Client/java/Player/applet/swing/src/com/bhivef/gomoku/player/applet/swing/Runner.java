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

package com.bhivef.gomoku.player.applet.swing;

import com.bhivef.gomoku.core.Config;

public class Runner extends com.bhivef.gomoku.core.player.swing.applet.Runner {

	public Runner(final Config config, final String session) {
		super(config, session);
	}
	
	protected void constructGame() {
		game = new Game(config, board, boardUI, client);
	}
	
	protected String getTitle() {
		return "Gomoku Player";
	}
	
}
