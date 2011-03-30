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

package com.bhivef.gomoku.core;

import java.util.List;

public class Client {

	protected final Config config;
	
	public Client(final Config config) {
		this.config = config;
	}
	
	public void login() throws Exception {
	}
	
	public void logout() {
	}
	
	public boolean online() {
		return false;
	}
	
	public void clone(String session) {
	}
	
	public String createGame(final boolean playFirst, final int width, final int height) throws Exception {
		return "";
	}
	
	public void joinGame(final String gameId) throws Exception {
	}
	
	public void listGame(final List<String> tag) {
	}
	
	public Move lastMove() {
		return new Move();
	}
	
	public void makeMove(final Move move) throws Exception {
	}
	
	public void gameState(GameState state) throws Exception {
	}
	
	public void endGame(final boolean finished, final byte victory) throws Exception {
	}
	
}
