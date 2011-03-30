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

public class RemotePlayer extends Player {

	protected final Player player;
	protected final Client client;
	
	public RemotePlayer(final Config config, final Client client, final Board board, final byte piece) {
		super(config, board, piece);
		this.player = null;
		this.client = client;
	}

	public RemotePlayer(final Config config, final Client client, final Board board, final Player player) {
		super(config, board, player.getPiece());
		this.player = player;
		this.client = client;
		setup();
	}
	
	protected void setup() {
		player.addMoveListener(new MoveListener() {
			public void moveMade(final Move move) {
				if (getPiece() != move.getPiece()) return;
				try {
					client.makeMove(move);
					fireMoveMade(move);
				} catch (Exception e) {
				}
			}
			public void lastMove(final byte victory) {
				fireLastMove(victory);
			}
		});
	}
	
	public void think(final Move move) {
		if (player != null) {
			player.think(move);
			return;
		}
		while (!disposed) {
			Move lastMove = client.lastMove();
			if (lastMove.getPiece() == getPiece()) {
				makeMove(lastMove);
				return;
			}
			try {
				Thread.sleep(1000);
			} catch (Exception e) {
			}
		}
	}
	
	public void dispose() {
		super.dispose();
		if (player != null) {
			player.dispose();
		}
	}
	
	public void setReady() {
		super.setReady();
		if (player != null) {
			player.setReady();
		}
	}
	
	public void setFinished() {
		super.setFinished();
		if (player != null) {
			player.setFinished();
		}
	}
	
	public boolean checkReady() {
		try {
			GameState state = new GameState();
			client.gameState(state);
			return state.Joined && !state.Cancelled && !state.Finished;
		} catch (Exception e) {
			return false;
		}
	}
	
}
