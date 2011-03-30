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

#include "..\Header\Client.h"

using namespace gomoku_core;

Client::Client(Config *config) {
	this->config = config;
}

Client::~Client() {

}

void Client::login() {

}

void Client::logout() {

}

bool Client::online() {
	return false;
}

void Client::clone(string session) {

}

string *Client::createGame(bool playFirst, int width, int height) {
	return new string("");
}

void Client::joinGame(string gameId) {

}

void Client::listGame(list<string> &tag) {

}

Move *Client::lastMove() {
	return new Move();
}

void Client::makeMove(Move move) {

}

void Client::gameState(GameState &state) {

}

void Client::endGame(bool finished, unsigned char victory) {

}
