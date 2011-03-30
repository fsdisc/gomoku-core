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

#include "stdafx.h"

#include "..\Header\FinalGame.h"
#include "..\..\AI\Header\JohnSmith.h"
#include "..\..\AI\Header\JamesCook.h"
#include "..\..\Core\Header\RemotePlayer.h"

using namespace gomoku_player;

const unsigned char FinalGame::JOHN_SMITH = 3;
const unsigned char FinalGame::JAMES_COOK = 4;

using namespace gomoku_ai;

FinalGame::FinalGame(Config *config, Board *board, BoardUI *boardUI, Client *client) : Game(config, board, boardUI, client) {

}

FinalGame::~FinalGame() {

}

Player *FinalGame::createUnknownPlayer(bool hasRemote, unsigned char type, unsigned char piece) {
	if (hasRemote) {
		if (type == JOHN_SMITH) {
			return new RemotePlayer(this->config, this->client, this->board, new JohnSmith(this->config, this->board, piece));
		} else if (type == JAMES_COOK) {
			return new RemotePlayer(this->config, this->client, this->board, new JamesCook(this->config, this->board, piece));
		} else {
			return Game::createUnknownPlayer(hasRemote, type, piece);
		}
	} else {
		if (type == JOHN_SMITH) {
			return new JohnSmith(this->config, this->board, piece);
		} else if (type == JAMES_COOK) {
			return new JamesCook(this->config, this->board, piece);
		} else {
			return Game::createUnknownPlayer(hasRemote, type, piece);
		}
	}
}
