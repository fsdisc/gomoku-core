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

#if !defined(AFX_MOVE_H__E5A347FB_1489_4DEF_93B7_7146BFC0BE58__INCLUDED_)
#define AFX_MOVE_H__E5A347FB_1489_4DEF_93B7_7146BFC0BE58__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#include "Cell.h"

namespace gomoku_core {

	class Move : public Cell {
	public:
		Move();
		Move(unsigned char row, unsigned char column, unsigned char piece);
		virtual ~Move();
		unsigned char getPiece();
		bool equals(Move move);
		Move *clone();
		void clone(Move move);
		virtual void clear();
		virtual string *toString();

	private:
		unsigned char piece;
	};

}

#endif // !defined(AFX_MOVE_H__E5A347FB_1489_4DEF_93B7_7146BFC0BE58__INCLUDED_)
