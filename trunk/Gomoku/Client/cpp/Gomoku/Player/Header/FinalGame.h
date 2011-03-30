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

#if !defined(AFX_FINALGAME_H__B13D1A3B_8FC2_4A19_B9BF_000918B3EB26__INCLUDED_)
#define AFX_FINALGAME_H__B13D1A3B_8FC2_4A19_B9BF_000918B3EB26__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#include "..\..\Core\Header\Game.h"

using namespace gomoku_core;

namespace gomoku_player {

	class FinalGame : public Game {
	public:
	    static const unsigned char JOHN_SMITH;
	    static const unsigned char JAMES_COOK;

		FinalGame(Config *config, Board *board, BoardUI *boardUI, Client *client);
		virtual ~FinalGame();

	protected:
		virtual Player *createUnknownPlayer(bool hasRemote, unsigned char type, unsigned char piece);

	};

}

#endif // !defined(AFX_FINALGAME_H__B13D1A3B_8FC2_4A19_B9BF_000918B3EB26__INCLUDED_)
