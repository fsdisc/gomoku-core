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

#include "..\Header\JamesCook.h"

using namespace gomoku_ai;

const int JamesCook::WIN_COUNT = 5;

JamesCook::JamesCook(Config *config, Board *board, unsigned char piece) : Player(config, board, piece) {

}

JamesCook::~JamesCook() {

}

void JamesCook::think(Move &move) {
	if (!this->getReady()) return;
	Cell *cell = this->findBestMoves(*this->board);
	if (cell != NULL) {
		Move *bestMove = new Move(cell->getRow(), cell->getColumn(), this->getPiece());
		move.clone(*bestMove);
		this->makeMove(move);
	}
}

void JamesCook::getLines() {
	if (this->lines.size() > 0) return;

	int i, j, n;

	for (i = 0; i < this->board->getHeight(); i++) {
		Line *line = new Line();
		for (j = 0; j < this->board->getWidth(); j++) {
			line->add(i, j);
		}
		this->lines.add(line);
	}
	for (j = 0; j < this->board->getWidth(); j++) {
		Line *line = new Line();
		for (i = 0; i < this->board->getHeight(); i++) {
			line->add(i, j);
		}
		this->lines.add(line);
	}
	for (n = 0; n < this->board->getHeight(); n++) {
		i = n;
		j = 0;
		Line *line = new Line();
		while (i < this->board->getHeight() && j < this->board->getWidth()) {
			line->add(i, j);
			i++;
			j++;
		}
		this->lines.add(line);
	}
	for (n = 1; n < this->board->getWidth(); n++) {
		i = 0;
		j = n;
		Line *line = new Line();
		while (i < this->board->getHeight() && j < this->board->getWidth()) {
			line->add(i, j);
			i++;
			j++;
		}
		this->lines.add(line);
	}
	for (n = 0; n < this->board->getHeight(); n++) {
		i = n;
		j = this->board->getWidth() - 1;
		Line *line = new Line();
		while (i < this->board->getHeight() && j >= 0) {
			line->add(i, j);
			i++;
			j--;
		}
		this->lines.add(line);
	}
	for (n = this->board->getWidth() - 2; n >= 0; n--) {
		i = 0;
		j = n;
		Line *line = new Line();
		while (i < this->board->getHeight() && j >= 0) {
			line->add(i, j);
			i++;
			j--;
		}
		this->lines.add(line);
	}
}

JamesCook::Row *JamesCook::getBlanks() {
	Row *tag = new Row();
		
	for (int i = 0; i < this->board->getHeight(); i++) {
		for (int j = 0; j < this->board->getWidth(); j++) {
			if (this->board->getPiece(i, j) == Board.BLANK) {
				tag->Cells.push_back(new Cell(i, j));
			}
		}
	}
		
	return tag;
}

Cell *JamesCook::findBestMoves(Board &board) {
	Row *computerMoves = new Row();
	Row *humanMoves = new Row();
	int computerCounter = this->findBestMoves(this->getPiece(), board, *computerMoves);
	int humanCounter = this->findBestMoves(Board::opponentPiece(this->getPiece()), board, *humanMoves);
		
	if (humanMoves->Cells.size() == 0) {
		if (computerMoves->Cells.size() > 0) {
			return computerMoves->getCell(0);
		} else {
			Row *blanks = this->getBlanks();
			if (blanks->Cells.size() == this->board->getWidth() * this->board->getHeight()) {
				int row = this->board->getHeight() / 2;
				int column = this->board->getWidth() / 2;
				return new Cell(row, column);
			} else if (blanks->Cells.size() > 0) {
					    return blanks->getCell(0);
			}
		}
	} else if (computerMoves->Cells.size() == 0) {
		return humanMoves->getCell(0);
	} else {
		if (humanCounter >= computerCounter) {
			return humanMoves->getCell(0);
		} else {
			return computerMoves->getCell(0);
		}
	}

	return NULL;
}

int JamesCook::findBestMoves(int srcState, Board &board, Row &tag) {
	int counter = 0;
	RowList rows;
	Row *row;
	int i;

	this->getLines();

	for (i = 0; i < this->lines.size(); i++) {
		Line *line = this->lines.get(i);
		getRows(srcState, board, *line, rows);
	}
		
	int maxCount = 0;
	for (i = rows.size() - 1; i >= 0; i--) {
		row = rows.get(i);
		if (row->BlankStart.size() == 0 && row->BlankStop.size() == 0) {
			rows.remove(i);
			continue;
		}
		if (row->Cells.size() > WIN_COUNT) {
			rows.remove(i);
			continue;
		}
		if (row->BlankStart.size() + row->BlankStop.size() + row->Cells.size() < WIN_COUNT) {
			rows.remove(i);
			continue;
		}
		if (maxCount < row->Cells.size()) {
			maxCount = row->Cells.size();
		}
	}
	counter = maxCount;
	for (i = rows.size() - 1; i >= 0; i--) {
		row = rows.get(i);
		if (maxCount > row->Cells.size()) {
			rows.remove(i);
			continue;
		}
	}
	for (i = 0; i < rows.size(); i++) {
		row = rows.get(i);
		if (row->BlankStart.size() == 0 || row->BlankStop.size() == 0) continue;
		if (row->BlankStart.size() > 0) {
			tag.Cells.push_back(row->getStart(row->BlankStart.size() - 1));
		}
		if (row->BlankStop.size() > 0) {
			tag.Cells.push_back(row->getStop(0));
		}
	}
	if (tag.Cells.size() == 0) {
		for (i = 0; i < rows.size(); i++) {
			row = rows.get(i);
			if (row->BlankStart.size() > 0 && row->BlankStop.size() > 0) continue;
			if (row->BlankStart.size() > 0) {
				tag.Cells.push_back(row->getStart(row->BlankStart.size() - 1));
			}
			if (row->BlankStop.size() > 0) {
				tag.Cells.push_back(row->getStop(0));
			}
		}
	}
		
	return counter;
}

void JamesCook::getRows(int srcState, Board &board, Line &line, RowList &tag) {
	Row *row = new Row();
	bool start = false;
	int i, j;
		
	for (i = 0; i < line.size(); i++) {
		int tagState = this->board->getPiece(line.get(i)->getRow(), line.get(i)->getColumn());
		if (start) {
			if (tagState == srcState) {
				row->Cells.push_back(line.get(i));
				if (i == line.size() - 1) {
					tag.add(row);
					row = new Row();
				}
			} else {
				for (j = i; j < line.size(); j++) {
					if (this->board->getPiece(line.get(j)->getRow(), line.get(j)->getColumn()) == Board::BLANK) {
						row->BlankStop.push_back(line.get(j));
					} else {
						break;
					}
				}
				tag.add(row);
				row = new Row();
				if (tagState == Board::BLANK) {
					row->BlankStart.push_back(line.get(i));
				}
				start = false;
			}
		} else {
			if (tagState == srcState) {
				start = true;
				row->Cells.push_back(line.get(i));
			} else if (tagState == Board::BLANK) {
				row->BlankStart.push_back(line.get(i));
			} else {
				row = new Row();
			}
		}
	}
}

Cell *JamesCook::Row::getCell(int idx) {
	list<Cell *>::iterator it;
	int no = 0;
	for (it = this->Cells.begin(); it != this->Cells.end(); it++) {
		if (no == idx) {
			return (*it);
		}
		no++;
	}
	return NULL;
}

Cell *JamesCook::Row::getStart(int idx) {
	list<Cell *>::iterator it;
	int no = 0;
	for (it = this->BlankStart.begin(); it != this->BlankStart.end(); it++) {
		if (no == idx) {
			return (*it);
		}
		no++;
	}
	return NULL;
}

Cell *JamesCook::Row::getStop(int idx) {
	list<Cell *>::iterator it;
	int no = 0;
	for (it = this->BlankStop.begin(); it != this->BlankStop.end(); it++) {
		if (no == idx) {
			return (*it);
		}
		no++;
	}
	return NULL;
}

int JamesCook::RowList::size() {
	return this->rows.size();
}

JamesCook::Row *JamesCook::RowList::get(int idx) {
	list<Row *>::iterator it;
	int no = 0;
	for (it = this->rows.begin(); it != this->rows.end(); it++) {
		if (no == idx) {
			return (*it);
		}
		no++;
	}
	return NULL;
}

void JamesCook::RowList::add(Row *row) {
	this->rows.push_back(row);
}

void JamesCook::RowList::remove(int idx) {
	list<Row *>::iterator it;
	int no = 0;
	for (it = this->rows.begin(); it != this->rows.end(); it++) {
		if (no == idx) {
			this->rows.remove(*it);
			return;
		}
		no++;
	}
}

int JamesCook::Line::size() {
	return this->cells.size();
}

Cell *JamesCook::Line::get(int idx) {
	list<Cell *>::iterator it;
	int no = 0;
	for (it = this->cells.begin(); it != this->cells.end(); it++) {
		if (no == idx) {
			return (*it);
		}
		no++;
	}
	return NULL;
}

void JamesCook::Line::add(int row, int column) {
	Cell *cell = new Cell(row, column);
	this->cells.push_back(cell);
}

int JamesCook::LineList::size() {
	return this->lines.size();
}

JamesCook::Line *JamesCook::LineList::get(int idx) {
	list<Line *>::iterator it;
	int no = 0;
	for (it = this->lines.begin(); it != this->lines.end(); it++) {
		if (no == idx) {
			return (*it);
		}
		no++;
	}
	return NULL;
}

void JamesCook::LineList::add(Line *line) {
	this->lines.push_back(line);
}

