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

#if !defined(AFX_MOVELISTENER_H__2B4D452A_FC16_4745_B191_4B23EC64F59D__INCLUDED_)
#define AFX_MOVELISTENER_H__2B4D452A_FC16_4745_B191_4B23EC64F59D__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#include "Move.h"

namespace gomoku_core {

	class MoveListener {
	public:
		MoveListener();
		virtual ~MoveListener();
		virtual void moveMade(Move move);
		virtual void lastMove(unsigned char victory);

	};

}

#endif // !defined(AFX_MOVELISTENER_H__2B4D452A_FC16_4745_B191_4B23EC64F59D__INCLUDED_)
