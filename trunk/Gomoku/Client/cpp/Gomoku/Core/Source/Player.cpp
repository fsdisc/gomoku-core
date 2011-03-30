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

#include "..\Header\Player.h"

using namespace gomoku_core;

Player::Player(Config *config, Board *board, unsigned char piece) {
	this->config = config;
	this->board = board;
	this->piece = piece;
	this->finished = false;
	this->ready = false;
	this->disposed = false;
}

Player::~Player() {

}

void Player::dispose() {
	this->disposed = true;
	this->listeners.clear();
}

unsigned char Player::getPiece() {
	return this->piece;
}

void Player::makeMove(Move move) {
	if (!this->ready)  return;
	if (this->finished)  return;
	if (this->piece != move.getPiece())  return;
	if (this->piece != this->board->getCurrentPiece())  return;
	if (move.getRow() >= this->board->getHeight() || move.getColumn() >= this->board->getWidth())  return;
	if (this->board->getPiece(move.getRow(), move.getColumn()) != Board::BLANK)  return;
	this->board->setPiece(move.getRow(), move.getColumn(), this->piece);
	this->board->setCurrentPiece(Board::opponentPiece(this->piece));
	this->fireMoveMade(move);
	unsigned char victory = this->board->victory();
	if (victory == Board::BLACK_WIN || victory == Board::WHITE_WIN || victory == Board::DRAW) {
		this->fireLastMove(victory);
	}
}

void Player::think(Move &move) {

}

bool Player::getReady() {
	return this->ready;
}

void Player::setReady() {
	this->ready = true;
}

bool Player::getFinished() {
	return this->finished;
}

void Player::setFinished() {
	this->finished = true;
}

bool Player::checkReady() {
	return true;
}

void Player::addMoveListener(MoveListener *src) {
	this->listeners.push_back(src);
}

void Player::clearListeners() {
	this->listeners.clear();
}

void Player::fireMoveMade(Move move) {
	list<MoveListener *>::iterator it;

	for (it = this->listeners.begin(); it != this->listeners.end(); it++) {
		(*it)->moveMade(move);
	}
}

void Player::fireLastMove(unsigned char victory) {
	list<MoveListener *>::iterator it;

	for (it = this->listeners.begin(); it != this->listeners.end(); it++) {
		(*it)->lastMove(victory);
	}
}
