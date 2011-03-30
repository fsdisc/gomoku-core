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

#if !defined(AFX_CELL_H__A81F7C72_5281_4CBD_90A2_0D7257283DD2__INCLUDED_)
#define AFX_CELL_H__A81F7C72_5281_4CBD_90A2_0D7257283DD2__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#include <string>

using namespace std;

namespace gomoku_core {

	class Cell {
	public:
		Cell();
		Cell(unsigned char row, unsigned char column);
		virtual ~Cell();
		unsigned char getRow();
		unsigned char getColumn();
		bool equals(Cell cell);
		Cell *clone();
		void clone(Cell cell);
		virtual void clear();
		virtual string *toString();

	private:
		unsigned char row;
		unsigned char column;
	};

}

#endif // !defined(AFX_CELL_H__A81F7C72_5281_4CBD_90A2_0D7257283DD2__INCLUDED_)
