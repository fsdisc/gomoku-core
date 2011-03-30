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

#include "..\Header\BoardUI.h"

using namespace gomoku_core;

const int BoardUI::LEFT_MARGIN = 20;
const int BoardUI::TOP_MARGIN = 20;
const int BoardUI::CELL_WIDTH = 20;
const int BoardUI::CELL_HEIGHT = 20;
const int BoardUI::PIECE_WIDTH = 14;
const int BoardUI::PIECE_HEIGHT = 14;

BoardUI::BoardUI(Board *board) {
	this->board = board;
}

BoardUI::~BoardUI() {
	this->listeners.clear();
}

void BoardUI::update() {
	this->draw();
}

void BoardUI::draw() {
	int r, c;
	char buffer[50];
	for (r = 0; r <= this->board->getHeight(); r++) {
		this->drawLine(LEFT_MARGIN, TOP_MARGIN + r * CELL_HEIGHT, LEFT_MARGIN + this->board->getWidth() * CELL_WIDTH, TOP_MARGIN + r * CELL_HEIGHT);
	}
	for (c = 0; c <= this->board->getWidth(); c++) {
		drawLine(LEFT_MARGIN + c * CELL_WIDTH, TOP_MARGIN, LEFT_MARGIN + c * CELL_WIDTH, TOP_MARGIN + this->board->getHeight() * CELL_HEIGHT);
	}
	for (r = 0; r < this->board->getHeight(); r++) {
		sprintf(buffer, "%d", r);
		drawText(0, TOP_MARGIN + r * CELL_HEIGHT, string(buffer));
	}
	for (c = 0; c < this->board->getWidth(); c++) {
		sprintf(buffer, "%d", c);
		drawText(LEFT_MARGIN + c * CELL_WIDTH, 0, string(buffer));
	}
	for (r = 0; r < this->board->getHeight(); r++) {
		for (c = 0; c < this->board->getWidth(); c++) {
			unsigned char piece = this->board->getPiece(r, c);
			if (piece == Board::WHITE) {
				drawOval(LEFT_MARGIN + c * CELL_WIDTH + (CELL_WIDTH - PIECE_WIDTH) / 2, TOP_MARGIN + r * CELL_HEIGHT + (CELL_HEIGHT - PIECE_HEIGHT) / 2, PIECE_WIDTH, PIECE_HEIGHT);
			}
			if (piece == Board::BLACK) {
				drawOval(LEFT_MARGIN + c * CELL_WIDTH + (CELL_WIDTH - PIECE_WIDTH) / 2, TOP_MARGIN + r * CELL_HEIGHT + (CELL_HEIGHT - PIECE_HEIGHT) / 2, PIECE_WIDTH, PIECE_HEIGHT);
				fillOval(LEFT_MARGIN + c * CELL_WIDTH + (CELL_WIDTH - PIECE_WIDTH) / 2, TOP_MARGIN + r * CELL_HEIGHT + (CELL_HEIGHT - PIECE_HEIGHT) / 2, PIECE_WIDTH, PIECE_HEIGHT);
			}
		}
	}
}

void BoardUI::drawLine(int x1, int y1, int x2, int y2) {

}

void BoardUI::drawText(int x, int y, string text) {

}

void BoardUI::drawOval(int x, int y, int width, int height) {

}

void BoardUI::fillOval(int x, int y, int width, int height) {

}

void BoardUI::addMoveListener(MoveListener *src) {
	this->listeners.push_back(src);
}

void BoardUI::clearListeners() {
	this->listeners.clear();
}

void BoardUI::fireMoveMade(Move move) {
	list<MoveListener *>::iterator it;

	for (it = this->listeners.begin(); it != this->listeners.end(); it++) {
		(*it)->moveMade(move);
	}
}
