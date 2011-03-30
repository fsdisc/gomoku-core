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

#if !defined(AFX_JOHNSMITH_H__E3A8413D_B68B_43B1_9B51_A4B2A933D16B__INCLUDED_)
#define AFX_JOHNSMITH_H__E3A8413D_B68B_43B1_9B51_A4B2A933D16B__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#include "..\..\Core\Header\ComputerPlayer.h"

using namespace gomoku_core;

namespace gomoku_ai {

	class JohnSmith : public ComputerPlayer {
	public:
		JohnSmith(Config *config, Board *board, unsigned char piece);
		virtual ~JohnSmith();

	protected:
		virtual int getMaxWin();
		virtual int getMinWin();
		virtual int eval(Board &b);

	};

}

#endif // !defined(AFX_JOHNSMITH_H__E3A8413D_B68B_43B1_9B51_A4B2A933D16B__INCLUDED_)
