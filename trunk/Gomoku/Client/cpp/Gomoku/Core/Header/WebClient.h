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

#if !defined(AFX_WEBCLIENT_H__BD09AAFC_E56B_4FE9_97A5_71D3CAB710B7__INCLUDED_)
#define AFX_WEBCLIENT_H__BD09AAFC_E56B_4FE9_97A5_71D3CAB710B7__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#include <string>

#include "Client.h"

using namespace std;

namespace gomoku_core {

	class WebClient : public Client {
	public:
		WebClient(Config *config);
		virtual ~WebClient();

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
		virtual string *request(string link);
		void split(string src, string sep, list<string> &tag);

	private:
		bool ready;
		string session;

	};

}

#endif // !defined(AFX_WEBCLIENT_H__BD09AAFC_E56B_4FE9_97A5_71D3CAB710B7__INCLUDED_)
