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

#include "..\Header\JohnSmith.h"

using namespace gomoku_ai;

JohnSmith::JohnSmith(Config *config, Board *board, unsigned char piece) : ComputerPlayer(config, board, piece) {

}

JohnSmith::~JohnSmith() {

}

int JohnSmith::getMaxWin() {
	return 100000;
}

int JohnSmith::getMinWin() {
	return -100000;
}

int JohnSmith::eval(Board &b) {
	Score *p = new Score(b, this->getPiece());
	Score *o = new Score(b, Board::opponentPiece(this->getPiece()));
	int retVal = 0;

	if (o->uncapped4 > 0)  return this->MINWIN;
	if (p->uncapped4 > 0)  return this->MAXWIN;

	retVal += p->capped2 * 1;
	retVal -= o->capped2 * 5;

	retVal += p->uncapped2 * 10;
	retVal -= o->uncapped2 * 10;

	retVal += p->capped3 * 100;
	retVal -= o->capped3 * 100;

	retVal += p->uncapped3 * 1000;
	retVal -= o->uncapped3 * 1000;

	retVal += p->capped4 * 10000;
	retVal -= o->capped4 * 10000;

	delete p;
	delete o;

	return this->calMax(this->MINWIN, this->calMin(this->MAXWIN, retVal));
}
