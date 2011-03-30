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

public class Loader extends com.bhivef.gomoku.core.player.swing.applet.Loader {

	protected void createConfig() {
		super.createConfig();
		config.setValue(Config.SEARCH_DEPTH, 2);
		config.setValue(Config.SERVER_URL, "http://bhivef.com/gomoku/");
		config.setValue(Config.SERVER_EXTENSION, ".php");
	}
	
	protected void createRunner() {
		runner = new Runner(config, session);
	}

	protected String[] getPlayerNames(final boolean newGame) {
		if (newGame) {
			return new String[] { "Human", "Computer", "Remote", "Computer (John Smith)", "Computer (James Cook)" };
		} else {
			return new String[] { "Human", "Computer", "Computer (John Smith)", "Computer (James Cook)" };
		}
	}

	protected int getPlayerIndex(final boolean newGame, final byte type) {
		if (newGame) {
			switch (type) {
			case Game.JOHN_SMITH:
				return 3;
			case Game.JAMES_COOK:
				return 4;
			default:
				return super.getPlayerIndex(newGame, type);
			}
		} else {
			switch (type) {
			case Game.JOHN_SMITH:
				return 2;
			case Game.JAMES_COOK:
				return 3;
			default:
				return super.getPlayerIndex(newGame, type);
			}
		}
	}
	
	protected byte getPlayerType(final boolean newGame, final int index) {
		if (newGame) {
			byte type = super.getPlayerType(newGame, index);
			switch (index) {
			case 3:
				type = Game.JOHN_SMITH;
				break;
			case 4:
				type = Game.JAMES_COOK;
				break;
			}
			return type;
		} else {
			byte type = super.getPlayerType(newGame, index);
			switch (index) {
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
	
	protected boolean isRemotePlayer(final int index) {
		return index == 2;
	}

}
