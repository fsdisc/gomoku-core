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

#include "..\Header\Move.h"
#include "..\Header\Board.h"

using namespace gomoku_core;

Move::Move() : Cell() {
	this->piece = Board::BLANK;
}

Move::Move(unsigned char row, unsigned char column, unsigned char piece) : Cell(row, column) {
	this->piece = piece;
}

Move::~Move() {

}

unsigned char Move::getPiece() {
	return this->piece;
}

bool Move::equals(Move move) {
	return this->getRow() == move.getRow() && this->getColumn() == move.getColumn() && this->piece == move.getPiece();
}

Move *Move::clone() {
	return new Move(this->getRow(), this->getColumn(), this->piece);
}

void Move::clone(Move move) {
	Cell::clone(move);
	this->piece = move.getPiece();
}

void Move::clear() {
	Cell::clear();
	this->piece = Board::BLANK;
}

string *Move::toString() {
	string *tag = Board::playerName(this->piece);
	tag->append(" ");
	string *temp = Cell::toString();
	tag->append(*temp);
	delete temp;
	return tag;
}
