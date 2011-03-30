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

#if !defined(AFX_JAMESCOOK_H__370655E2_2418_4843_BF2E_7290A0D1491E__INCLUDED_)
#define AFX_JAMESCOOK_H__370655E2_2418_4843_BF2E_7290A0D1491E__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#include <list>

#include "..\..\Core\Header\Player.h"

using namespace std;
using namespace gomoku_core;

namespace gomoku_ai {

	class JamesCook : public Player {
	public:
		JamesCook(Config *config, Board *board, unsigned char piece);
		virtual ~JamesCook();

		virtual void think(Move &move);

		class Row {
		public:
			list<Cell *> Cells;
			list<Cell *> BlankStart;
			list<Cell *> BlankStop;

			Cell *getCell(int idx);
			Cell *getStart(int idx);
			Cell *getStop(int idx);
		};

		class RowList {
		public:
			int size();
			Row *get(int idx);
			void add(Row *row);
			void remove(int idx);

		private:
			list<Row *> rows;
		};

		class Line {
		public:
			int size();
			Cell *get(int idx);
			void add(int row, int column);

		private:
			list<Cell *> cells;

		};

		class LineList {
		public:
			int size();
			Line *get(int idx);
			void add(Line *line);

		private:
			list<Line *> lines;
		};

	protected:
	    static const int WIN_COUNT;
	    LineList lines;

		void getLines();
		Row *getBlanks();
		Cell *findBestMoves(Board &board);
		int findBestMoves(int srcState, Board &board, Row &tag);
		void getRows(int srcState, Board &board, Line &line, RowList &tag);

	};

}

#endif // !defined(AFX_JAMESCOOK_H__370655E2_2418_4843_BF2E_7290A0D1491E__INCLUDED_)
