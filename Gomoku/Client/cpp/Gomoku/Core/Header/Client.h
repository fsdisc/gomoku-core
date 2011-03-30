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

#if !defined(AFX_CLIENT_H__BFA713AB_CFA1_4076_8D3F_2B128CD30550__INCLUDED_)
#define AFX_CLIENT_H__BFA713AB_CFA1_4076_8D3F_2B128CD30550__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#include <string>
#include <list>

#include "Config.h"
#include "Move.h"
#include "GameState.h"

using namespace std;

namespace gomoku_core {

	class Client {
	public:
		Client(Config *config);
		virtual ~Client();

		virtual void login();
		virtual void logout();
		virtual bool online();
		virtual void clone(string session);
		virtual string *createGame(bool playFirst, int width, int height);
		virtual void joinGame(string gameId);
		virtual void listGame(list<string> &tag);
		virtual Move *lastMove();
		virtual void makeMove(Move move);
		virtual void gameState(GameState &state);
		virtual void endGame(bool finished, unsigned char victory);

	protected:
		Config *config;
	};

}

#endif // !defined(AFX_CLIENT_H__BFA713AB_CFA1_4076_8D3F_2B128CD30550__INCLUDED_)
