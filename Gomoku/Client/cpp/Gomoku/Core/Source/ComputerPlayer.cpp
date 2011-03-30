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

#include "..\Header\ComputerPlayer.h"

using namespace gomoku_core;

ComputerPlayer::Score::Score(Board &board, unsigned char piece) {
	this->uncapped2 = board.findRow(piece, 2, 0);
	this->capped2 = board.findRow(piece, 2, 1);

	this->uncapped3 = board.findRow(piece, 3, 0);
	this->capped3 = board.findRow(piece, 3, 1);

	this->uncapped4 = board.findRow(piece, 4, 0);
	this->capped4 = board.findRow(piece, 4, 1);
}

int ComputerPlayer::calMax(int a, int b) {
	return a > b ? a : b;
}

int ComputerPlayer::calMin(int a, int b) {
	return a < b ? a : b;
}

ComputerPlayer::ComputerPlayer(Config *config, Board *board, unsigned char piece) : Player(config, board, piece) {
	this->searchDepth = this->config->getInt(Config::SEARCH_DEPTH);
	this->moveCount = 0;
	this->MAXWIN = this->getMaxWin();
	this->MINWIN = this->getMinWin();
}

ComputerPlayer::~ComputerPlayer() {

}

void ComputerPlayer::think(Move &move) {
	if (!this->getReady())  return;
	this->think(move, 0, *this->board, this->getPiece(), this->MINWIN - 1, this->MAXWIN + 1);
	if (move.getPiece() == Board::BLANK) {
		if (this->moveCount == 0) {
			Move *nextMove = new Move(this->board->getHeight() / 2, this->board->getWidth() / 2, this->getPiece());
			move.clone(*nextMove);
			delete nextMove;
			this->makeMove(move);
			this->moveCount++;
		}
	} else {
		this->makeMove(move);
		this->moveCount++;
	}
}

int ComputerPlayer::getMaxWin() {
	return 10000;
}

int ComputerPlayer::getMinWin() {
	return -10000;
}

int ComputerPlayer::think(Move &move, int depth, Board &board, unsigned char piece, int alpha, int beta) {
	if (depth == searchDepth) {
		move.clear();
		return this->eval(board);
	}
		
	int max = this->MINWIN - 1;
	int min = this->MAXWIN + 1;
	Move *nextMove = new Move();
	int moveVal = 0;
	while(this->nextPossible(*nextMove, board, piece)) {
		Board *nextBoard = board.clone();
		nextBoard->setPiece(nextMove->getRow(), nextMove->getColumn(), piece);
		unsigned char victory = nextBoard->victory();
		if (victory == Board::BLACK_WIN) {
			if (this->getPiece() == Board::BLACK) {
				moveVal = this->MAXWIN;
			} else {
				moveVal = this->MINWIN;
			}
		} else if (victory == Board::WHITE_WIN) {
			if (this->getPiece() == Board::WHITE) {
				moveVal = this->MAXWIN;
			} else {
				moveVal = this->MINWIN;
			}
		} else {
			Move *lowerMove = new Move();
			moveVal = this->think(*lowerMove, depth + 1, *nextBoard, Board::opponentPiece(piece), alpha, beta);
			delete lowerMove;
		}
		if (piece == this->getPiece()) {
			if (moveVal > max) {
				move.clone(*nextMove);
				max = moveVal;
			}
			alpha = alpha > moveVal ? alpha : moveVal;
			if (alpha >= beta) {
				delete nextBoard;
				delete nextMove;
				return beta;
			}
		} else {
			if (moveVal < min) {
				move.clone(*nextMove);
				min = moveVal;
			}
			beta = beta < moveVal ? beta : moveVal;
			if (beta <= alpha) {
				delete nextBoard;
				delete nextMove;
				return alpha;
			}
		}
		delete nextBoard;
	}
		
	return piece == this->getPiece() ? alpha : beta;
}

bool ComputerPlayer::nextPossible(Move &move, Board &board, unsigned char piece) {
	int row = 0;
	int column = 0;

	if (move.getPiece() != Board::BLANK) {
		row = move.getRow();
		column = move.getColumn() + 1;
		if (column == board.getWidth()) {
			row++;
			column = 0;
		}
	}
		
	while (row < board.getHeight()) {
		while (column < board.getWidth()) {
			if (board.getPiece(row, column) == Board::BLANK && board.hasAdjacentPieces(row, column)) {
				Move *tag = new Move(row, column, piece);
				move.clone(*tag);
				delete tag;
				return true;
			}
			column++;
		}
		column = 0;
		row++;
	}
		
	return false;
}

int ComputerPlayer::eval(Board &b) {
	Score *p = new Score(b, this->getPiece());
	Score *o = new Score(b, Board::opponentPiece(this->getPiece()));
	int retVal = 0;

	if (o->uncapped4 > 0)  return this->MINWIN;
	if (p->uncapped4 > 0)  return this->MAXWIN;

	retVal += p->capped2 * 5;
	retVal -= o->capped2 * 5;

	retVal += p->uncapped2 * 10;
	retVal -= o->uncapped2 * 10;

	retVal += p->capped3 * 20;
	retVal -= o->capped3 * 30;

	retVal += p->uncapped3 * 100;
	retVal -= o->uncapped3 * 120;

	retVal += p->capped4 * 500;
	retVal -= o->capped4 * 500;

	delete p;
	delete o;

	return this->calMax(this->MINWIN, this->calMin(this->MAXWIN, retVal));
}