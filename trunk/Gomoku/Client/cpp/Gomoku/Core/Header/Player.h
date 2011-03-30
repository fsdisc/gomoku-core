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

#if !defined(AFX_PLAYER_H__62F02C90_9984_4437_B413_4DFA5C74CBA6__INCLUDED_)
#define AFX_PLAYER_H__62F02C90_9984_4437_B413_4DFA5C74CBA6__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#include <list>

#include "Config.h"
#include "Board.h"
#include "MoveListener.h"
#include "Move.h"

using namespace std;

namespace gomoku_core {

	class Player {
	public:
		Player(Config *config, Board *board, unsigned char piece);
		virtual ~Player();

		unsigned char getPiece();
		void makeMove(Move move);
		virtual void think(Move &move);
		bool getReady();
		virtual void setReady();
		bool getFinished();
		virtual void setFinished();
		virtual bool checkReady();
		void addMoveListener(MoveListener *src);
		void clearListeners();
		virtual void dispose();

	protected:
		void fireMoveMade(Move move);
		void fireLastMove(unsigned char victory);

		Config *config;
		Board *board;
	    bool disposed;

	private:
		unsigned char piece;
		list<MoveListener *> listeners;
	    bool finished;
	    bool ready;

	};

}

#endif // !defined(AFX_PLAYER_H__62F02C90_9984_4437_B413_4DFA5C74CBA6__INCLUDED_)
