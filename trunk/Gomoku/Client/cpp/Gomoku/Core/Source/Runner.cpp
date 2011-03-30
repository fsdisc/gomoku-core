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

#include "..\Header\Runner.h"
#include "..\Header\WebClient.h"

using namespace gomoku_core;

Runner::MoveAdapter::MoveAdapter(Runner *runner) {
	this->runner = runner;
}

void Runner::MoveAdapter::moveMade(Move move) {
	Move *nextMove = move.clone();
	this->runner->history.push_back(*nextMove);
}

void Runner::MoveAdapter::lastMove(unsigned char victory) {
	if (this->runner->finished) return;
	this->runner->finished = true;
	if (victory == Board::DRAW) {
		this->runner->drawEnd();
	} else if (victory == Board::BLACK_WIN) {
		this->runner->blackWin();
	} else if (victory == Board::WHITE_WIN) {
		this->runner->whiteWin();
	}
}

Runner::Runner() {
	this->moveAdapter = NULL;
	this->config = NULL;
	this->board = NULL;
	this->boardUI = NULL;
	this->client = NULL;
	this->game = NULL;
}

Runner::~Runner() {
	if (this->moveAdapter != NULL) {
		delete this->moveAdapter;
	}
	if (this->config != NULL) {
		delete this->config;
	}
	if (this->board != NULL) {
		delete this->board;
	}
	if (this->boardUI != NULL) {
		delete this->boardUI;
	}
	if (this->client != NULL) {
		delete this->client;
	}
	if (this->game != NULL) {
		delete this->game;
	}
	this->history.clear();
}

void Runner::start() {
	this->setup();
	try {
		this->newGame();
	} catch (...) { }
}

void Runner::setup() {
	this->createConfig();
	this->createBoard();
	this->createBoardUI();
	this->createClient();
	this->createGame();
	this->createMainUI();
}

void Runner::createConfig() {
	this->config = new Config();
	this->config->setValue(Config::SEARCH_DEPTH, 2);
	this->config->setValue(Config::BOARD_WIDTH, Board::GO_WIDTH);
	this->config->setValue(Config::BOARD_HEIGHT, Board::GO_HEIGHT);
	this->config->setValue(Config::FIRST_TYPE, Game::HUMAN_PLAYER);
	this->config->setValue(Config::SECOND_TYPE, Game::HUMAN_PLAYER);
	this->config->setValue(Config::SERVER_URL, string("http://bhivef.com/gomoku/"));
	this->config->setValue(Config::SERVER_EXTENSION, string(".php"));
}

void Runner::createBoard() {
	this->board = new Board(this->config->getByte(Config::BOARD_WIDTH), this->config->getByte(Config::BOARD_HEIGHT));
}

void Runner::createBoardUI() {
	
}

void Runner::createClient() {
	this->client = new WebClient(this->config);
}

void Runner::createGame() {
	this->finished = false;
	this->history.clear();
	this->board->resize(this->config->getByte(Config::BOARD_WIDTH), this->config->getByte(Config::BOARD_HEIGHT));
	if (this->game != NULL) {
		this->game->dispose();
	}
	this->constructGame();
	this->moveAdapter = new MoveAdapter(this);
	this->game->addMoveListener(this->moveAdapter);
}

void Runner::constructGame() {
	this->game = new Game(this->config, this->board, this->boardUI, this->client);
}

void Runner::createMainUI() {

}

void Runner::blackWin() {

}

void Runner::whiteWin() {

}

void Runner::drawEnd() {

}

void Runner::newGame() {
	this->game->create();
}

void Runner::joinGame(string gameId) {
	this->game->join(gameId);
}


