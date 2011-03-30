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

#if !defined(AFX_RUNNER_H__D5D39B3F_FDA2_4514_9D58_4D659C1C08F3__INCLUDED_)
#define AFX_RUNNER_H__D5D39B3F_FDA2_4514_9D58_4D659C1C08F3__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#include <list>
#include <string>

#include "Config.h"
#include "Board.h"
#include "BoardUI.h"
#include "Client.h"
#include "Move.h"
#include "Game.h"
#include "MoveListener.h"

using namespace std;

namespace gomoku_core {

	class Runner {
	public:
		Runner();
		virtual ~Runner();

		virtual void start();

	protected:
		Config *config;
		Board *board;
		BoardUI *boardUI;
		Client *client;
		Game *game;
		list<Move> history;
		bool finished;

		virtual void setup();
		virtual void createConfig();
		virtual void createBoard();
		virtual void createBoardUI();
		virtual void createClient();
		virtual void createGame();
		virtual void constructGame();
		virtual void createMainUI();
		virtual void blackWin();
		virtual void whiteWin();
		virtual void drawEnd();
		virtual void newGame();
		virtual void joinGame(string gameId);

	private:
		MoveListener *moveAdapter;

		class MoveAdapter : public MoveListener {
		public:
			MoveAdapter(Runner *runner);
			virtual ~MoveAdapter() { }

			virtual void moveMade(Move move);
			virtual void lastMove(unsigned char victory);

		private:
			Runner *runner;
		};

		friend class MoveAdapter;
	};

}

#endif // !defined(AFX_RUNNER_H__D5D39B3F_FDA2_4514_9D58_4D659C1C08F3__INCLUDED_)
