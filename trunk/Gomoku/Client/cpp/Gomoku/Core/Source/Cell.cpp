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

#include "..\Header\Cell.h"

using namespace gomoku_core;

Cell::Cell(unsigned char row, unsigned char column) {
	this->row = row;
	this->column = column;
}

Cell::Cell() {
	this->row = 0;
	this->column = 0;
}

Cell::~Cell() {

}

unsigned char Cell::getRow() {
	return this->row;
}

unsigned char Cell::getColumn() {
	return this->column;
}

bool Cell::equals(Cell cell) {
	return this->row == cell.getRow() && this->column == cell.getColumn();
}

Cell *Cell::clone() {
	return new Cell(this->row, this->column);
}

void Cell::clone(Cell cell) {
	this->row = cell.getRow();
	this->column = cell.getColumn();
}

void Cell::clear() {
	this->row = 0;
	this->column = 0;
}

string *Cell::toString() {
	char buffer[50];
	sprintf(buffer, "(%d, %d)", this->row, this->column);
	return new string(buffer);
}