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

#include "..\Header\HumanPlayer.h"

using namespace gomoku_core;

class MoveAdapter : public MoveListener {
public:
	MoveAdapter(HumanPlayer *player) {
		this->player = player;
	}

	virtual void moveMade(Move move) {
		if (this->player->getPiece() != move.getPiece())  return;
		this->player->makeMove(move);
	}

	virtual void lastMove(unsigned char victory) {
	}

private:
	HumanPlayer *player;

};

HumanPlayer::HumanPlayer(Config *config, Board *board, unsigned char piece, BoardUI *boardUI) : Player(config, board, piece) {
	this->boardUI = boardUI;
	this->moveAdapter = new MoveAdapter(this);
	this->boardUI->addMoveListener(this->moveAdapter);
}

HumanPlayer::~HumanPlayer() {
	delete this->moveAdapter;
}
