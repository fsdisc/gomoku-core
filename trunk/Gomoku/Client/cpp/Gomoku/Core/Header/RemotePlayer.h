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

#if !defined(AFX_REMOTEPLAYER_H__A14A8A3D_3152_486A_801A_571040FF9171__INCLUDED_)
#define AFX_REMOTEPLAYER_H__A14A8A3D_3152_486A_801A_571040FF9171__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#include "Player.h"
#include "Config.h"
#include "Client.h"
#include "Board.h"
#include "MoveListener.h"

namespace gomoku_core {

	class RemotePlayer : public Player {
	public:
		RemotePlayer(Config *config, Client *client, Board *board, unsigned char piece);
		RemotePlayer(Config *config, Client *client, Board *board, Player *player);
		virtual ~RemotePlayer();
		virtual void think(Move &move);
		virtual void setReady();
		virtual void setFinished();
		virtual bool checkReady();
		virtual void dispose();

	protected:
		Player *player;
		Client *client;

	private:
		MoveListener *moveAdapter;

		class MoveAdapter : public MoveListener {
		public:
			MoveAdapter(RemotePlayer *player);
			virtual ~MoveAdapter() { }
			virtual void moveMade(Move move);
			virtual void lastMove(unsigned char victory);
		private:
			RemotePlayer *player;
		};

		friend class MoveAdapter;
	};

}

#endif // !defined(AFX_REMOTEPLAYER_H__A14A8A3D_3152_486A_801A_571040FF9171__INCLUDED_)
