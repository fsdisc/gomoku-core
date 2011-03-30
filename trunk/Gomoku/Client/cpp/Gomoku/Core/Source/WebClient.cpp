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

#include "..\Header\WebClient.h"
#include "..\Private\Socket.h" // Requires Ws2_32.lib, see: http://www.adp-gmbh.ch/win/misc/sockets.html
#include "..\Header\Board.h"

using namespace gomoku_core;

WebClient::WebClient(Config *config) : Client(config) {
	this->ready = false;
}

WebClient::~WebClient() {

}

string *WebClient::request(string link) {
	string host;
	string path;
	size_t pos = link.find("//");
	if (pos == string::npos) {
		host = link.substr(0);
	} else {
		host = link.substr(pos + 2);
	}
	pos = host.find("/");
	if (pos == string::npos) {
		path = "";
	} else {
		path = host.substr(pos);
		host = host.substr(0, pos);
	}
	string *tag = new string("");
	try {
		SocketClient s(host, 80);
		s.SendLine("GET " + path + " HTTP/1.0");
		s.SendLine("Host: " + host);
		s.SendLine("");

		bool start = false;
		while (true) {
			string line = s.ReceiveLine();
			if (line.empty()) break;
			if (start) {
				tag->append(line);
			} else {
				if (line == "\r\n") {
					start = true;
					continue;
				}
			}
		}
	} catch (const char* s) {
		*tag = "Error: ";
		tag->append(s);
	} catch (string s) {
		*tag = "Error: ";
		tag->append(s);
	} catch (...) {
		*tag = "Error: Unhandled exception";
	}

	return tag;
}

void WebClient::login() {
	if (this->ready) {
		this->logout();
	}
		
	string *serverUrl = this->config->getString(Config::SERVER_URL);
	string *extension = this->config->getString(Config::SERVER_EXTENSION);
	string *username = this->config->getString(Config::SERVER_USERNAME);
	string *password = this->config->getPassword(Config::SERVER_PASSWORD);
	char buffer[255];
	sprintf(buffer, "%sapi/login%s?u=%s&p=%s", serverUrl->c_str(), extension->c_str(), username->c_str(), password->c_str());
	string *link = new string(buffer);
	string *response = request(*link);
	delete serverUrl;
	delete extension;
	delete username;
	delete password;
	delete link;
	string sign = "Success: ";
	if (response->find(sign) == 0) {
		this->session = response->substr(sign.length());
		this->ready = true;
		delete response;
		return;
	}
	sign = "Error: ";
	if (response->find(sign) == 0) {
		string message = "Login fail: " + response->substr(sign.length());
		delete response;
		throw message;
	}
	delete response;
	throw string("Login fail: Invalid api call!");
}

void WebClient::logout() {
	try {
		string *serverUrl = this->config->getString(Config::SERVER_URL);
		string *extension = this->config->getString(Config::SERVER_EXTENSION);
		char buffer[255];
		sprintf(buffer, "%sapi/logout%s?s=%s", serverUrl->c_str(), extension->c_str(), this->session.c_str());
		string *link = new string(buffer);
		string *response = request(*link);
		delete serverUrl;
		delete extension;
		delete link;
		delete response;
	} catch (...) { }
	this->ready = false;
	this->session = "";
}

bool WebClient::online() {
	return this->ready;
}

void WebClient::clone(string session) {
	this->session = session;
	this->ready = true;
}

string *WebClient::createGame(bool playFirst, int width, int height) {
	if (!this->ready) {
		this->login();
	}

	string *serverUrl = this->config->getString(Config::SERVER_URL);
	string *extension = this->config->getString(Config::SERVER_EXTENSION);
	char buffer[255];
	sprintf(buffer, "%sapi/create%s?s=%s&p=%s&w=%d&h=%d", serverUrl->c_str(), extension->c_str(), this->session.c_str(), playFirst ? "1" : "0", width, height);
	string *link = new string(buffer);
	string *response = request(*link);
	delete serverUrl;
	delete extension;
	delete link;
	string sign = "Success: ";
	if (response->find(sign) == 0) {
		string *tag = new string(response->substr(sign.length()));
		delete response;
		return tag;
	}
	sign = "Error: ";
	if (response->find(sign) == 0) {
		string message = "Fail to create game: " + response->substr(sign.length());
		delete response;
		throw message;
	}
	delete response;
	throw string("Fail to create game: Invalid api call!");
}

void WebClient::joinGame(string gameId) {
	if (!this->ready) {
		this->login();
	}

	string *serverUrl = this->config->getString(Config::SERVER_URL);
	string *extension = this->config->getString(Config::SERVER_EXTENSION);
	char buffer[255];
	sprintf(buffer, "%sapi/join%s?s=%s&g=%s", serverUrl->c_str(), extension->c_str(), this->session.c_str(), gameId.c_str());
	string *link = new string(buffer);
	string *response = request(*link);
	delete serverUrl;
	delete extension;
	delete link;
	string sign = "Error: ";
	if (response->find(sign) == 0) {
		string message = "Fail to join game: " + response->substr(sign.length());
		delete response;
		throw message;
	}
	if (*response != "Success") {
		delete response;
		throw string("Fail to join game: Invalid api call!");
	}
	delete response;
}

void WebClient::listGame(list<string> &tag) {
	try {
		tag.clear();

		if (!this->ready) {
			this->login();
		}

		string *serverUrl = this->config->getString(Config::SERVER_URL);
		string *extension = this->config->getString(Config::SERVER_EXTENSION);
		char buffer[255];
		sprintf(buffer, "%sapi/list%s?s=%s", serverUrl->c_str(), extension->c_str(), this->session.c_str());
		string *link = new string(buffer);
		string *response = request(*link);
		delete serverUrl;
		delete extension;
		delete link;
		string sign = "Success: ";
		if (response->find(sign) == 0) {
			string data = response->substr(sign.length());
			if (data.length() > 0) {
				this->split(data, "\r\n", tag);
			}
		}
		delete response;
	} catch (...) { }
}

Move *WebClient::lastMove() {
	try {
		if (!this->ready) {
			this->login();
		}

		string *serverUrl = this->config->getString(Config::SERVER_URL);
		string *extension = this->config->getString(Config::SERVER_EXTENSION);
		string *gameId = this->config->getString(Config::CURRENT_GAME);
		char buffer[255];
		sprintf(buffer, "%sapi/last%s?s=%s&g=%s", serverUrl->c_str(), extension->c_str(), this->session.c_str(), gameId->c_str());
		string *link = new string(buffer);
		string *response = request(*link);
		delete serverUrl;
		delete extension;
		delete gameId;
		delete link;
		string sign = "Success: ";
		if (response->find(sign) == 0) {
			string data = response->substr(sign.length());
			delete response;
			if (data.size() == 0) {
				return new Move();
			} else {
				list<string> fields;
				this->split(data, "|", fields);
				list<string>::iterator it;
				unsigned char piece = Board::BLANK;
				unsigned char row = 0;
				unsigned char column = 0;
				unsigned char no = 0;
				for (it = fields.begin(); it != fields.end(); it++, no++) {
					if (no == 0) {
						piece = unsigned char(atoi((*it).c_str()));
						if (piece != Board::BLACK && piece != Board::WHITE) {
							return new Move();
						}
					}
					if (no == 1) {
						row = unsigned char(atoi((*it).c_str()));
						if (row >= this->config->getByte(Config::BOARD_HEIGHT)) {
							return new Move();
						}
					}
					if (no == 2) {
						column = unsigned char(atoi((*it).c_str()));
						if (column >= this->config->getByte(Config::BOARD_WIDTH)) {
							return new Move();
						}
					}
				}
				return new Move(row, column, piece);
			}
		} else {
			delete response;
			return new Move();
		}
	} catch (...) { 
		return new Move();
	}
}

void WebClient::makeMove(Move move) {
	if (!this->ready) {
		this->login();
	}

	string *serverUrl = this->config->getString(Config::SERVER_URL);
	string *extension = this->config->getString(Config::SERVER_EXTENSION);
	string *gameId = this->config->getString(Config::CURRENT_GAME);
	char buffer[255];
	sprintf(buffer, "%sapi/move%s?s=%s&g=%s&r=%d&c=%d&p=%d", serverUrl->c_str(), extension->c_str(), this->session.c_str(), gameId->c_str(), move.getRow(), move.getColumn(), move.getPiece());
	string *link = new string(buffer);
	string *response = request(*link);
	delete serverUrl;
	delete extension;
	delete gameId;
	delete link;
	string sign = "Error: ";
	if (response->find(sign) == 0) {
		string message = "Fail to make move: " + response->substr(sign.length());
		delete response;
		throw message;
	}
	if (*response != "Success") {
		delete response;
		throw string("Fail to make move: Invalid api call!");
	}
	delete response;
}

void WebClient::gameState(GameState &state) {
	if (!this->ready) {
		this->login();
	}

	string *serverUrl = this->config->getString(Config::SERVER_URL);
	string *extension = this->config->getString(Config::SERVER_EXTENSION);
	string *gameId = this->config->getString(Config::CURRENT_GAME);
	char buffer[255];
	sprintf(buffer, "%sapi/state%s?s=%s&g=%s", serverUrl->c_str(), extension->c_str(), this->session.c_str(), gameId->c_str());
	string *link = new string(buffer);
	string *response = request(*link);
	delete serverUrl;
	delete extension;
	delete gameId;
	delete link;
	string sign = "Success: ";
	if (response->find(sign) == 0) {
		string data = response->substr(sign.length());
		delete response;
		list<string> fields;
		this->split(data, "|", fields);
		list<string>::iterator it;
		unsigned char no = 0;
		for (it = fields.begin(); it != fields.end(); it++, no++) {
			if (no == 0) {
				state.Joined = (*it) == "1";
			}
			if (no == 1) {
				state.Cancelled = (*it) == "1";
			}
			if (no == 2) {
				state.Finished = (*it) == "1";
			}
			if (no == 3) {
				state.Victory = unsigned char(atoi((*it).c_str()));
			}
		}
		return;
	}
	sign = "Error: ";
	if (response->find(sign) == 0) {
		string message = "Fail to get game state: " + response->substr(sign.length());
		delete response;
		throw message;
	}
	delete response;
	throw string("Fail to get game state: Invalid api call!");
}

void WebClient::endGame(bool finished, unsigned char victory) {
	if (!this->ready) {
		this->login();
	}

	string *serverUrl = this->config->getString(Config::SERVER_URL);
	string *extension = this->config->getString(Config::SERVER_EXTENSION);
	string *gameId = this->config->getString(Config::CURRENT_GAME);
	char buffer[255];
	sprintf(buffer, "%sapi/end%s?s=%s&g=%s&f=%s&v=%d", serverUrl->c_str(), extension->c_str(), this->session.c_str(), gameId->c_str(), finished ? "1" : "0", victory);
	string *link = new string(buffer);
	string *response = request(*link);
	delete serverUrl;
	delete extension;
	delete gameId;
	delete link;
	string sign = "Error: ";
	if (response->find(sign) == 0) {
		string message = "Fail to end game: " + response->substr(sign.length());
		delete response;
		throw message;
	}
	if (*response != "Success") {
		delete response;
		throw string("Fail to end game: Invalid api call!");
	}
	delete response;
}

void WebClient::split(string src, string sep, list<string> &tag) {
	tag.clear();
	size_t curPos = 0;
	size_t nextPos = src.find(sep);
	while (nextPos != string::npos) {
		string line = src.substr(curPos, nextPos - curPos);
		tag.push_back(line);
		curPos = nextPos + sep.length();
		nextPos = src.find(sep, curPos);
	}
	tag.push_back(src.substr(curPos));
}
