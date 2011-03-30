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

#include "..\Header\Game.h"
#include "..\Header\RemotePlayer.h"
#include "..\Header\ComputerPlayer.h"
#include "..\Header\HumanPlayer.h"

using namespace gomoku_core;

const unsigned char Game::HUMAN_PLAYER = 0;
const unsigned char Game::COMPUTER_PLAYER = 1;
const unsigned char Game::REMOTE_PLAYER = 2;

DWORD WINAPI Game::firstThread(LPVOID lpParam) {
	Game *game = (Game *)lpParam; 
	Move *move = new Move();
	game->firstPlayer->think(*move);
	return 0;
}

DWORD WINAPI Game::secondThread(LPVOID lpParam) {
	Game *game = (Game *)lpParam; 
	Move *move = new Move();
	game->secondPlayer->think(*move);
	return 0;
}

DWORD WINAPI Game::readyThread(LPVOID lpParam) {
	Game *game = (Game *)lpParam; 
	bool stop = false;
	while (!stop && !game->disposed) {
		stop = game->firstPlayer->checkReady() && game->secondPlayer->checkReady();
		if (stop) break;
		Sleep(1000);
	}
	game->firstPlayer->setReady();
	game->secondPlayer->setReady();
	game->firstThink();
	return 0;
}

Game::MoveAdapter::MoveAdapter(Game *game, bool first) {
	this->game = game;
	this->first = first;
}

void Game::MoveAdapter::moveMade(Move move) {
	this->game->boardUI->update();
	this->game->fireMoveMade(move);
	if (first) {
		this->game->secondThink();
	} else {
		this->game->firstThink();
	}
}

void Game::MoveAdapter::lastMove(unsigned char victory) {
	this->game->finished = true;
	if (first) {
		this->game->secondPlayer->setFinished();
	} else {
		this->game->firstPlayer->setFinished();
	}
	this->game->fireLastMove(victory);
	try {
		this->game->client->endGame(true, victory);
	} catch (...) { }
}

Game::Game(Config *config, Board *board, BoardUI *boardUI, Client *client) {
	this->config = config;
	this->board = board;
	this->boardUI = boardUI;
	this->client = client;
	this->board->clear();
	this->board->setCurrentPiece(Board::BLACK);
	this->boardUI->clearListeners();
	this->disposed = false;
	this->finished = false;
}

Game::~Game() {
	if (this->firstMoveAdapter != NULL) {
		delete this->firstMoveAdapter;
	}
	if (this->secondMoveAdapter != NULL) {
		delete this->secondMoveAdapter;
	}
	if (this->firstPlayer != NULL) {
		delete this->firstPlayer;
	}
	if (this->secondPlayer != NULL) {
		delete this->secondPlayer;
	}
}

void Game::dispose() {
	this->disposed = true;
	this->listeners.clear();
	if (this->firstPlayer != NULL) {
		this->firstPlayer->dispose();
	}
	if (this->secondPlayer != NULL) {
		this->secondPlayer->dispose();
	}
	try {
		this->client->endGame(false, Board::NO_WIN);
	} catch (...) { }
}

void Game::create() {
	if (this->hasRemotePlayer()) {
		bool playFirst = (this->config->getByte(Config::FIRST_TYPE) != Game::REMOTE_PLAYER);
		string *gameId = this->client->createGame(playFirst, this->config->getInt(Config::BOARD_WIDTH), this->config->getInt(Config::BOARD_HEIGHT));
		this->config->setValue(Config::CURRENT_GAME, *gameId);
	}
	this->createPlayers();
	this->setup();
	this->checkReady();
}

void Game::join(string gameId) {
	if (this->hasRemotePlayer()) {
		this->client->joinGame(gameId);
		this->config->setValue(Config::CURRENT_GAME, gameId);
	}
	this->createPlayers();
	this->setup();
	this->checkReady();
}

void Game::listGame(list<string> &tag) {
	this->client->listGame(tag);
}

void Game::setup() {
	this->firstMoveAdapter = new MoveAdapter(this, true);
	this->firstPlayer->addMoveListener(this->firstMoveAdapter);
	this->secondMoveAdapter = new MoveAdapter(this, false);
	this->secondPlayer->addMoveListener(this->secondMoveAdapter);
}

bool Game::hasRemotePlayer() {
	bool remote = false;
	unsigned char firstType = this->config->getByte(Config::FIRST_TYPE);
	unsigned char secondType = this->config->getByte(Config::SECOND_TYPE);
	if (firstType == Game::REMOTE_PLAYER && secondType == Game::REMOTE_PLAYER) {
		remote = false;
	} else if (firstType == Game::REMOTE_PLAYER || secondType == Game::REMOTE_PLAYER) {
		remote = true;
	}
	return remote;
}

void Game::createPlayers() {
	bool remote = false;
	unsigned char firstType = this->config->getByte(Config::FIRST_TYPE);
	unsigned char secondType = this->config->getByte(Config::SECOND_TYPE);
	if (firstType == Game::REMOTE_PLAYER && secondType == Game::REMOTE_PLAYER) {
		remote = false;
	} else if (firstType == Game::REMOTE_PLAYER || secondType == Game::REMOTE_PLAYER) {
		remote = true;
	}
	if (remote) {
		if (firstType == Game::REMOTE_PLAYER) {
			this->firstPlayer = new RemotePlayer(this->config, this->client, this->board, Board::BLACK);
		} else if (firstType == Game::COMPUTER_PLAYER) {
			this->firstPlayer = new RemotePlayer(this->config, this->client, this->board, new ComputerPlayer(this->config, this->board, Board::BLACK));
		} else if (firstType == Game::HUMAN_PLAYER) {
			this->firstPlayer = new RemotePlayer(this->config, this->client, this->board, new HumanPlayer(this->config, this->board, Board::BLACK, this->boardUI));
		} else {
			this->firstPlayer = this->createUnknownPlayer(true, firstType, Board::BLACK);
		}
		if (secondType == Game::REMOTE_PLAYER) {
			this->secondPlayer = new RemotePlayer(this->config, this->client, this->board, Board::WHITE);
		} else if (secondType == Game::COMPUTER_PLAYER) {
			this->secondPlayer = new RemotePlayer(this->config, this->client, this->board, new ComputerPlayer(this->config, this->board, Board::WHITE));
		} else if (secondType == Game::HUMAN_PLAYER) {
			this->secondPlayer = new RemotePlayer(this->config, this->client, this->board, new HumanPlayer(this->config, this->board, Board::WHITE, this->boardUI));
		} else {
			this->secondPlayer = this->createUnknownPlayer(true, secondType, Board::WHITE);
		}
	} else {
		if (firstType == Game::COMPUTER_PLAYER) {
			this->firstPlayer = new ComputerPlayer(this->config, this->board, Board::BLACK);
		} else if (firstType == Game::HUMAN_PLAYER) {
			this->firstPlayer = new HumanPlayer(this->config, this->board, Board::BLACK, this->boardUI);
		} else {
			this->firstPlayer = this->createUnknownPlayer(false, firstType, Board::BLACK);
		}
		if (secondType == Game::COMPUTER_PLAYER) {
			this->secondPlayer = new ComputerPlayer(this->config, this->board, Board::WHITE);
		} else if (secondType == Game::HUMAN_PLAYER) {
			this->secondPlayer = new HumanPlayer(this->config, this->board, Board::WHITE, this->boardUI);
		} else {
			this->secondPlayer = this->createUnknownPlayer(false, secondType, Board::WHITE);
		}
	}
}

Player *Game::createUnknownPlayer(bool hasRemote, unsigned char type, unsigned char piece) {
	if (hasRemote) {
		return new RemotePlayer(this->config, this->client, this->board, new HumanPlayer(this->config, this->board, piece, this->boardUI));
	} else {
		return new HumanPlayer(this->config, this->board, piece, this->boardUI);
	}
}

void Game::addMoveListener(MoveListener *src) {
	this->listeners.push_back(src);
}

void Game::clearListeners() {
	this->listeners.clear();
}

void Game::fireMoveMade(Move move) {
	list<MoveListener *>::iterator it;

	for (it = this->listeners.begin(); it != this->listeners.end(); it++) {
		(*it)->moveMade(move);
	}
}

void Game::fireLastMove(unsigned char victory) {
	list<MoveListener *>::iterator it;

	for (it = this->listeners.begin(); it != this->listeners.end(); it++) {
		(*it)->lastMove(victory);
	}
}

void Game::firstThink() {
	HANDLE handle = CreateThread(NULL, 0, Game::firstThread, this, 0, NULL);
}

void Game::secondThink() {
	HANDLE handle = CreateThread(NULL, 0, Game::secondThread, this, 0, NULL);
}

void Game::checkReady() {
	HANDLE handle = CreateThread(NULL, 0, Game::readyThread, this, 0, NULL);
}

