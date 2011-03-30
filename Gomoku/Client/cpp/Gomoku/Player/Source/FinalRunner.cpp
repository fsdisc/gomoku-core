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

#include "stdafx.h"

#include "..\Header\FinalRunner.h"
#include "..\Header\FinalGame.h"

using namespace gomoku_player;

FinalRunner::FinalRunner(HINSTANCE hInst) : PlayerRunner(hInst) {

}

FinalRunner::~FinalRunner() {

}

void FinalRunner::createConfig() {
	PlayerRunner::createConfig();
	this->config->setValue(Config::SEARCH_DEPTH, 2);
	this->config->setValue(Config::SERVER_URL, string("http://bhivef.com/gomoku/"));
	this->config->setValue(Config::SERVER_EXTENSION, string(".php"));
}

void FinalRunner::constructGame() {
	this->game = new FinalGame(this->config, this->board, this->boardUI, this->client);
}

string *FinalRunner::getTitle() {
	return new string("Gomoku Player");
}

void FinalRunner::getPlayerNames(bool newGame, list<string> &tag) {
	PlayerRunner::getPlayerNames(newGame, tag);
	tag.push_back("Computer (John Smith)");
	tag.push_back("Computer (James Cook)");
}

int FinalRunner::getPlayerIndex(bool newGame, unsigned char type) {
	if (newGame) {
		if (type == FinalGame::JOHN_SMITH) return 3;
		if (type == FinalGame::JAMES_COOK) return 4;
	} else {
		if (type == FinalGame::JOHN_SMITH) return 2;
		if (type == FinalGame::JAMES_COOK) return 3;
	}
	return PlayerRunner::getPlayerIndex(newGame, type);
}

unsigned char FinalRunner::getPlayerType(bool newGame, int index) {
	unsigned char type = PlayerRunner::getPlayerType(newGame, index);
	if (newGame) {
		if (index == 3) {
			type = FinalGame::JOHN_SMITH;
		}
		if (index == 4) {
			type = FinalGame::JAMES_COOK;
		}
	} else {
		if (index == 2) {
			type = FinalGame::JOHN_SMITH;
		}
		if (index == 3) {
			type = FinalGame::JAMES_COOK;
		}
	}
	return type;
}

bool FinalRunner::isRemotePlayer(int index) {
	return index == 2;
}

int APIENTRY WinMain(HINSTANCE hInstance,
                     HINSTANCE hPrevInstance,
                     LPSTR     lpCmdLine,
                     int       nCmdShow)
{
 	FinalRunner *runner = new FinalRunner(hInstance);
	runner->start();
	delete runner;
	return 0;
}
