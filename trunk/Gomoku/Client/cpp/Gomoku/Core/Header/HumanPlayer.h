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

#if !defined(AFX_HUMANPLAYER_H__3B261712_D340_412B_817A_192D0AE3FA5E__INCLUDED_)
#define AFX_HUMANPLAYER_H__3B261712_D340_412B_817A_192D0AE3FA5E__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#include "Config.h"
#include "Board.h"
#include "BoardUI.h"
#include "MoveListener.h"
#include "Player.h"

namespace gomoku_core {

	class HumanPlayer : public Player {
	public:
		HumanPlayer(Config *config, Board *board, unsigned char piece, BoardUI *boardUI);
		virtual ~HumanPlayer();

	protected:
		BoardUI *boardUI;
		MoveListener *moveAdapter;

	};

}

#endif // !defined(AFX_HUMANPLAYER_H__3B261712_D340_412B_817A_192D0AE3FA5E__INCLUDED_)
