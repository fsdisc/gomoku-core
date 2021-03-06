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

public class HumanPlayer extends Player {

	protected final BoardUI boardUI;
	
	public HumanPlayer(final Config config, final Board board, final byte piece, final BoardUI boardUI) {
		super(config, board, piece);
		this.boardUI = boardUI;
		setup();
	}
	
	protected void setup() {
		boardUI.addMoveListener(new MoveListener() {
			public void moveMade(final Move move) {
				if (getPiece() != move.getPiece()) return;
				makeMove(move);
			}
			public void lastMove(final byte victory) {
			}
		});
	}
	
}
