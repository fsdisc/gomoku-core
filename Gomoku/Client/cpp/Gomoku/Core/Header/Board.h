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

#if !defined(AFX_BOARD_H__5D98C166_7C36_4993_B66F_29BE4F5C44C5__INCLUDED_)
#define AFX_BOARD_H__5D98C166_7C36_4993_B66F_29BE4F5C44C5__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#include <string>

using namespace std;

namespace gomoku_core {

	class Board {
	public:
		static const unsigned char BLANK;
		static const unsigned char BLACK;
		static const unsigned char WHITE;

		static const unsigned char GO_WIDTH;
		static const unsigned char GO_HEIGHT;

		static const unsigned char DRAW;
		static const unsigned char BLACK_WIN;
		static const unsigned char WHITE_WIN;
		static const unsigned char NO_WIN;

		Board(unsigned char width, unsigned char height);
		virtual ~Board();

		unsigned char getWidth();
		unsigned char getHeight();
		unsigned char getPiece(int row, int column);
		void setPiece(int row, int column, unsigned char piece);
		unsigned char getCurrentPiece();
		void setCurrentPiece(unsigned char piece);
		void clear();
		void resize(unsigned char width, unsigned char height);
		Board *clone();
		static unsigned char opponentPiece(unsigned char piece);
		static string *playerName(unsigned char piece);
		unsigned char victory();
		unsigned char victory(int row, int column);
		bool hasAdjacentPieces(int row, int column);
		int findRow(unsigned char piece, int size, int maxcaps);
		int findRow(unsigned char piece, int row, int column, int size, int maxcaps);

	private:
		unsigned char width;
		unsigned char height;
		unsigned char *pieces;
		unsigned char currentPiece;
	};

}

#endif // !defined(AFX_BOARD_H__5D98C166_7C36_4993_B66F_29BE4F5C44C5__INCLUDED_)
