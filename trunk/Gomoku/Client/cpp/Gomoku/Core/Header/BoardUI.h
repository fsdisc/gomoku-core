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

#if !defined(AFX_BOARDUI_H__68E481D1_716D_4FFE_B3A9_13FD1F14613C__INCLUDED_)
#define AFX_BOARDUI_H__68E481D1_716D_4FFE_B3A9_13FD1F14613C__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#include <list>
#include <string>

#include "Board.h"
#include "MoveListener.h"

using namespace std;

namespace gomoku_core {

	class BoardUI {
	public:
	    static const int LEFT_MARGIN;
	    static const int TOP_MARGIN;
	    static const int CELL_WIDTH;
	    static const int CELL_HEIGHT;
	    static const int PIECE_WIDTH;
	    static const int PIECE_HEIGHT;

		BoardUI(Board *board);
		virtual ~BoardUI();

		virtual void update();
		void addMoveListener(MoveListener *src);
		void clearListeners();

	protected:
		Board *board;

		virtual void draw();
		virtual void drawLine(int x1, int y1, int x2, int y2);
		virtual void drawText(int x, int y, string text);
		virtual void drawOval(int x, int y, int width, int height);
		virtual void fillOval(int x, int y, int width, int height);
		void fireMoveMade(Move move);

	private:
		list<MoveListener *> listeners;

	};

}

#endif // !defined(AFX_BOARDUI_H__68E481D1_716D_4FFE_B3A9_13FD1F14613C__INCLUDED_)
