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

#include "..\Header\RemotePlayer.h"
#include <windows.h>

using namespace gomoku_core;

RemotePlayer::MoveAdapter::MoveAdapter(RemotePlayer *player) {
	this->player = player;
}

void RemotePlayer::MoveAdapter::moveMade(Move move) {
	if (this->player->getPiece() != move.getPiece())  return;
	try {
		this->player->client->makeMove(move);
		this->player->fireMoveMade(move);
	} catch (...) { }
}

void RemotePlayer::MoveAdapter::lastMove(unsigned char victory) {
	this->player->fireLastMove(victory);
}

RemotePlayer::RemotePlayer(Config *config, Client *client, Board *board, unsigned char piece) : Player(config, board, piece) {
	this->player = NULL;
	this->client = client;
	this->moveAdapter = NULL;
}

RemotePlayer::RemotePlayer(Config *config, Client *client, Board *board, Player *player) : Player(config, board, player->getPiece()) {
	this->player = player;
	this->client = client;
	this->moveAdapter = new RemotePlayer::MoveAdapter(this);
	this->player->addMoveListener(this->moveAdapter);
}

RemotePlayer::~RemotePlayer() {
	if (this->moveAdapter != NULL) {
		delete this->moveAdapter;
	}
	if (this->player != NULL) {
		delete this->player;
	}
}

void RemotePlayer::dispose() {
	Player::dispose();
	if (this->player != NULL) {
		this->player->dispose();
	}
}

void RemotePlayer::think(Move &move) {
	if (this->player != NULL) {
		this->player->think(move);
		return;
	}
	while (!this->disposed) {
		Move *lastMove = this->client->lastMove();
		if (lastMove->getPiece() == this->getPiece()) {
			this->makeMove(*lastMove);
			move.clone(*lastMove);
			delete lastMove;
			return;
		}
		delete lastMove;
		Sleep(1000);
	}
}

void RemotePlayer::setReady() {
	Player::setReady();
	if (this->player != NULL) {
		this->player->setReady();
	}
}

void RemotePlayer::setFinished() {
	Player::setFinished();
	if (this->player != NULL) {
		this->player->setFinished();
	}
}

bool RemotePlayer::checkReady() {
	GameState *state = new GameState();
	try {
		this->client->gameState(*state);
		bool tag = state->Joined && !state->Cancelled && !state->Finished;
		delete state;
		return tag;
	} catch (...) {
		delete state;
		return false;
	}
}
