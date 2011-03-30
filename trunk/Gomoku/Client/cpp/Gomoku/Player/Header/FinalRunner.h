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

#if !defined(AFX_FINALRUNNER_H__16AE9049_D239_4746_96C2_35748E75BC85__INCLUDED_)
#define AFX_FINALRUNNER_H__16AE9049_D239_4746_96C2_35748E75BC85__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#include <windows.h>

#include "..\..\Core\Header\PlayerRunner.h"

using namespace gomoku_core;

namespace gomoku_player {

	class FinalRunner : public PlayerRunner {
	public:
		FinalRunner(HINSTANCE hInst);
		virtual ~FinalRunner();

	protected:
		virtual void createConfig();
		virtual void constructGame();
		virtual string *getTitle();
		virtual void getPlayerNames(bool newGame, list<string> &tag);
		virtual int getPlayerIndex(bool newGame, unsigned char type);
		virtual unsigned char getPlayerType(bool newGame, int index);
		virtual bool isRemotePlayer(int index);

	};

}

#endif // !defined(AFX_FINALRUNNER_H__16AE9049_D239_4746_96C2_35748E75BC85__INCLUDED_)
