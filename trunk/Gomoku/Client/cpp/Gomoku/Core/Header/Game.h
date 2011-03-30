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

#if !defined(AFX_GAME_H__57DBEBD6_FD24_443D_A3B4_FEA16D884A4F__INCLUDED_)
#define AFX_GAME_H__57DBEBD6_FD24_443D_A3B4_FEA16D884A4F__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#include <list>
#include <string>
#include <windows.h>

#include "Config.h"
#include "Board.h"
#include "BoardUI.h"
#include "Client.h"
#include "Player.h"
#include "MoveListener.h"

using namespace std;

namespace gomoku_core {

	class Game {
	public:
	    static const unsigned char HUMAN_PLAYER;
	    static const unsigned char COMPUTER_PLAYER;
	    static const unsigned char REMOTE_PLAYER;

		Game(Config *config, Board *board, BoardUI *boardUI, Client *client);
		virtual ~Game();

		void create();
		void join(string gameId);
		void listGame(list<string> &tag);
		void addMoveListener(MoveListener *src);
		void clearListeners();
		virtual void dispose();

	protected:
	    Config *config;
	    Board *board;
	    BoardUI *boardUI;
	    Client *client;
	    Player *firstPlayer;
	    Player *secondPlayer;
	    bool disposed;
	    bool finished;

		bool hasRemotePlayer();
		void createPlayers();
		virtual Player *createUnknownPlayer(bool hasRemote, unsigned char type, unsigned char piece);
		void fireMoveMade(Move move);
		void fireLastMove(unsigned char victory);
		void firstThink();
		void secondThink();
		void checkReady();

	private:
	    list<MoveListener *> listeners;
		MoveListener *firstMoveAdapter;
		MoveListener *secondMoveAdapter;

		void setup();

		class MoveAdapter : public MoveListener {
		public:
			MoveAdapter(Game *game, bool first);
			virtual ~MoveAdapter() { }
			virtual void moveMade(Move move);
			virtual void lastMove(unsigned char victory);

		private:
			Game *game;
			bool first;

		};

		friend class MoveAdapter;

		static DWORD WINAPI firstThread(LPVOID lpParam);
		static DWORD WINAPI secondThread(LPVOID lpParam);
		static DWORD WINAPI readyThread(LPVOID lpParam);

		friend DWORD WINAPI Game::firstThread(LPVOID lpParam);
		friend DWORD WINAPI Game::secondThread(LPVOID lpParam);
		friend DWORD WINAPI Game::readyThread(LPVOID lpParam);

	};

}

#endif // !defined(AFX_GAME_H__57DBEBD6_FD24_443D_A3B4_FEA16D884A4F__INCLUDED_)
