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

#if !defined(AFX_GAMESTATE_H__C79CE57C_D0F0_4695_886D_3A60B73BB779__INCLUDED_)
#define AFX_GAMESTATE_H__C79CE57C_D0F0_4695_886D_3A60B73BB779__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

namespace gomoku_core {

	class GameState {
	public:
		GameState();
		virtual ~GameState();

        bool Joined;
        bool Cancelled;
        bool Finished;
        unsigned char Victory;

	};

}

#endif // !defined(AFX_GAMESTATE_H__C79CE57C_D0F0_4695_886D_3A60B73BB779__INCLUDED_)
