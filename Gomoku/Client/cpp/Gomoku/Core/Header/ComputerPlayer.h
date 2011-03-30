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

#if !defined(AFX_COMPUTERPLAYER_H__F6B12753_EA32_48D4_928F_AF8B421143D7__INCLUDED_)
#define AFX_COMPUTERPLAYER_H__F6B12753_EA32_48D4_928F_AF8B421143D7__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#include "Player.h"
#include "Config.h"
#include "Board.h"

namespace gomoku_core {

	class ComputerPlayer : public Player {
	public:
		ComputerPlayer(Config *config, Board *board, unsigned char piece);
		virtual ~ComputerPlayer();

		virtual void think(Move &move);

	protected:
		virtual int getMaxWin();
		virtual int getMinWin();
		virtual int think(Move &move, int depth, Board &board, unsigned char piece, int alpha, int beta);
		virtual bool nextPossible(Move &move, Board &board, unsigned char piece);
		virtual int eval(Board &b);
		int calMax(int a, int b);
		int calMin(int a, int b);

	    int MAXWIN;
	    int MINWIN;
	    int searchDepth;
	    int moveCount;

		class Score {
		public:
			int capped2;
			int uncapped2;

			int capped3;
			int uncapped3;

			int capped4;
			int uncapped4;

			Score(Board &board, unsigned char piece);
			virtual ~Score() { }
		};

	};

}

#endif // !defined(AFX_COMPUTERPLAYER_H__F6B12753_EA32_48D4_928F_AF8B421143D7__INCLUDED_)
