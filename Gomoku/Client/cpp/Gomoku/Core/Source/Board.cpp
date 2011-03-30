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

#include "..\Header\Board.h"

using namespace gomoku_core;

const unsigned char Board::BLANK = 0;
const unsigned char Board::BLACK = 1;
const unsigned char Board::WHITE = 2;

const unsigned char Board::GO_WIDTH = 19;
const unsigned char Board::GO_HEIGHT = 19;

const unsigned char Board::DRAW = 0;
const unsigned char Board::BLACK_WIN = 1;
const unsigned char Board::WHITE_WIN = 2;
const unsigned char Board::NO_WIN = 3;

Board::Board(unsigned char width, unsigned char height) {
	this->width = width;
	this->height = height;
	this->pieces = new unsigned char[width * height];
	this->clear();
}

Board::~Board() {
	delete [] this->pieces;
}

unsigned char Board::getWidth() {
	return this->width;
}

unsigned char Board::getHeight() {
	return this->height;
}

unsigned char Board::getPiece(int row, int column) {
	return this->pieces[row * this->width + column];
}

void Board::setPiece(int row, int column, unsigned char piece) {
	this->pieces[row * this->width + column] = piece;
}

unsigned char Board::getCurrentPiece() {
	return this->currentPiece;
}

void Board::setCurrentPiece(unsigned char piece) {
	this->currentPiece = piece;
}

void Board::clear() {
	for (int i = 0; i < this->height; i++) {
		for (int j = 0; j < this->width; j++) {
			this->setPiece(i, j, Board::BLANK);
		}
	}
	this->currentPiece = Board::BLACK;
}

void Board::resize(unsigned char width, unsigned char height) {
	this->width = width;
	this->height = height;
	delete [] this->pieces;
	this->pieces = new unsigned char[width * height];
	this->clear();
}

Board *Board::clone() {
	Board *tag = new Board(this->width, this->height);
	for (int i = 0; i < this->height; i++) {
		for (int j = 0; j < this->width; j++) {
			tag->setPiece(i, j, this->getPiece(i, j));
		}
	}
	return tag;
}

unsigned char Board::opponentPiece(unsigned char piece) {
	return piece == Board::BLACK ? Board::WHITE : Board::BLACK;
}

string *Board::playerName(unsigned char piece) {
	switch (piece) {
	case Board::BLACK:
		return new string("Black");
	case Board::WHITE:
		return new string("White");
	}
	return new string("");
}

unsigned char Board::victory() {
	int blankCount = this->width * this->height;
	for (int r = 0; r < this->height; r++) {
		for (int c = 0; c < this->width; c++) {
			if (this->getPiece(r, c) != Board::BLANK) {
				blankCount--;
				switch (victory(r, c)) {
				case Board::BLACK_WIN:
					return Board::BLACK_WIN;
				case Board::WHITE_WIN:
					return Board::WHITE_WIN;
				}
			}
		}
	}
	return blankCount == 0 ? Board::DRAW : Board::NO_WIN;
}

unsigned char Board::victory(int row, int column) {
	unsigned char piece = this->getPiece(row, column);
	if (piece == Board::BLANK) return Board::NO_WIN;
	if (findRow(piece, row, column, 5, 2) > 0) {
		return piece == Board::BLACK ? Board::BLACK_WIN : Board::WHITE_WIN;
	}
	return Board::NO_WIN;
}

bool Board::hasAdjacentPieces(int row, int column) {
	for (int r = row - 1; r <= row + 1 && r >= 0 && r < this->height; r++) {
		for (int c = column - 1; c <= column + 1 && c >= 0 && c < this->width; c++) {
			if (r == row && c == column) continue;
			if (this->getPiece(r, c) != Board::BLANK) return true;
		}
	}
	return false;
}

int Board::findRow(unsigned char piece, int size, int maxcaps) {
	int count = 0;
	for (int r = 0; r < this->height; r++) {
		for (int c = 0; c < this->width; c++) {
			count += findRow(piece, r, c, size, maxcaps);
		}
	}
	return count;
}

int Board::findRow(unsigned char piece, int row, int column, int size, int maxcaps) {
	if (this->getPiece(row, column) != piece) return 0;

	int count = 0;
	unsigned char opponentPiece = Board::opponentPiece(piece);
	bool found = false;
	int c, r, capCount;

	// Check right
	if (column + size <= this->width) {
		found = true;
		for (c = column + 1; c < column + size && found; c++) {
			if (this->getPiece(row, c) != piece) {
				found = false;
				break;
			}
		}
		if (found && column > 0 && this->getPiece(row, column - 1) == piece) {
			found = false;
		}
		if (found && column + size < this->width && this->getPiece(row, column + size) == piece) {
			found = false;
		}
		if (found) {
			capCount = 0;
			if (column == 0 || this->getPiece(row, column - 1) == opponentPiece) {
				capCount++;
			}
			if (column + size == this->width || this->getPiece(row, column + size) == opponentPiece) {
				capCount++;
			}
			if (capCount <= maxcaps) {
				count++;
			}
		}
	}

	// Check down
	if (row + size <= this->height) {
		found = true;
		for (r = row + 1; r < row + size && found; r++) {
			if (this->getPiece(r, column) != piece) {
				found = false;
				break;
			}
		}
		if (found && row > 0 && this->getPiece(row - 1, column) == piece) {
			found = false;
		}
		if (found && row + size < this->height && this->getPiece(row + size, column) == piece) {
			found = false;
		}
		if (found) {
			capCount = 0;
			if (row == 0 || this->getPiece(row - 1, column) == opponentPiece) {
				capCount++;
			}
			if (row + size == this->height || this->getPiece(row + size, column) == opponentPiece) {
				capCount++;
			}
			if (capCount <= maxcaps) {
				count++;
			}
		}
	}

	// Check down-right
	if (row + size <= this->height && column + size <= this->width) {
		found = true;
		for (r = row + 1, c = column + 1; r < row + size && c < column + size && found; r++, c++) {
			if (this->getPiece(r, c) != piece) {
				found = false;
				break;
			}
		}
		if (found && row > 0 && column > 0 && this->getPiece(row - 1, column - 1) == piece) {
			found = false;
		}
		if (found && row + size < this->height && column + size < this->width && this->getPiece(row + size, column + size) == piece) {
			found = false;
		}
		if (found) {
			capCount = 0;
			if (row == 0 || column == 0 || this->getPiece(row - 1, column - 1) == opponentPiece) {
				capCount++;
			}
			if (row + size == this->height || column + size == this->width || this->getPiece(row + size, column + size) == opponentPiece) {
				capCount++;
			}
			if (capCount <= maxcaps) {
				count++;
			}
		}
	}

	// Check down-left
	if (row + size <= this->height && column - size >= -1) {
		found = true;
		for (r = row + 1, c = column - 1; r < row + size && c >= 0 && found; r++, c--) {
			if (this->getPiece(r, c) != piece) {
				found = false;
				break;
			}
		}
		if (found && row > 0 && column < this->width - 1 && this->getPiece(row - 1, column + 1) == piece) {
			found = false;
		}
		if (found && row + size < this->height && column - size >= 0 && this->getPiece(row + size, column - size) == piece) {
			found = false;
		}
		if (found) {
			capCount = 0;
			if (row == 0 || column == this->width - 1 || this->getPiece(row - 1, column + 1) == opponentPiece) {
				capCount++;
			}
			if (row + size == this->height || column - size == -1 || this->getPiece(row + size, column - size) == opponentPiece) {
				capCount++;
			}
			if (capCount <= maxcaps) {
				count++;
			}
		}
	}

	return count;
}
