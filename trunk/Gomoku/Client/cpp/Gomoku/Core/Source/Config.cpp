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

#include "..\Header\Config.h"

#include <iostream>
#include <fstream>

using namespace gomoku_core;

const bool Config::BOOLEAN_DEFAULT = false;
const double Config::DOUBLE_DEFAULT = 0;
const float Config::FLOAT_DEFAULT = 0;
const int Config::INT_DEFAULT = 0;
const long Config::LONG_DEFAULT = 0;
const unsigned char Config::BYTE_DEFAULT = 0;
const string Config::STRING_DEFAULT = "";
const string Config::PASSWORD_DEFAULT = "";

const string Config::TRUE_VALUE = "true";
const string Config::FALSE_VALUE = "false";

const string Config::SERVER_URL = "server.url";
const string Config::SERVER_USERNAME = "server.username";
const string Config::SERVER_PASSWORD = "server.password";
const string Config::SERVER_EXTENSION = "server.extension";
const string Config::SEARCH_DEPTH = "search.depth";
const string Config::FIRST_TYPE = "type.first";
const string Config::SECOND_TYPE = "type.second";
const string Config::BOARD_WIDTH = "board.width";
const string Config::BOARD_HEIGHT = "board.height";
const string Config::CURRENT_GAME = "game.current";

const string Config::KEY = "968AD25B";

Config::Config() {

}

Config::~Config() {

}

string *Config::getProperty(string name) {
	if (this->properties.find(name) == this->properties.end()) {
		return new string("");
	} else {
		return new string(this->properties[name]);
	}
}

void Config::setProperty(string name, string value) {
	this->properties[name] = string(value);
}

string *Config::getString(string name) {
	string *value = this->getProperty(name);
	if (value->size() == 0) {
		delete value;
		return new string(Config::STRING_DEFAULT);
	} else {
		return value;
	}
}

void Config::setValue(string name, string value) {
	string *oldValue = this->getString(name);
	if (oldValue->compare(value) != 0) {
		this->setProperty(name, value);
	}
}

bool Config::getBoolean(string name) {
	string *value = this->getProperty(name);
	if (value->size() == 0)  return Config::BOOLEAN_DEFAULT;
	if (value->compare(Config::TRUE_VALUE) == 0)  return true;
	return false;
}

void Config::setValue(string name, bool value) {
	bool oldValue = this->getBoolean(name);
	if (oldValue != value) {
		this->setProperty(name, value ? Config::TRUE_VALUE : Config::FALSE_VALUE);
	}
}

double Config::getDouble(string name) {
	string *value = this->getProperty(name);
	if (value->size() == 0)  return Config::DOUBLE_DEFAULT;
	return atof(value->c_str());
}

void Config::setValue(string name, double value) {
	double oldValue = this->getDouble(name);
	if (oldValue != value) {
		char buffer[50];
		sprintf(buffer, "%f", value);
		this->setProperty(name, string(buffer));
	}
}

float Config::getFloat(string name) {
	string *value = this->getProperty(name);
	if (value->size() == 0)  return Config::FLOAT_DEFAULT;
	return float(atof(value->c_str()));
}

void Config::setValue(string name, float value) {
	float oldValue = this->getFloat(name);
	if (oldValue != value) {
		char buffer[50];
		sprintf(buffer, "%f", value);
		this->setProperty(name, string(buffer));
	}
}

int Config::getInt(string name) {
	string *value = this->getProperty(name);
	if (value->size() == 0)  return Config::INT_DEFAULT;
	return atoi(value->c_str());
}

void Config::setValue(string name, int value) {
	int oldValue = this->getInt(name);
	if (oldValue != value) {
		char buffer[50];
		sprintf(buffer, "%d", value);
		this->setProperty(name, string(buffer));
	}
}

long Config::getLong(string name) {
	string *value = this->getProperty(name);
	if (value->size() == 0)  return Config::LONG_DEFAULT;
	return atol(value->c_str());
}

void Config::setValue(string name, long value) {
	long oldValue = this->getLong(name);
	if (oldValue != value) {
		char buffer[50];
		sprintf(buffer, "%ld", value);
		this->setProperty(name, string(buffer));
	}
}

unsigned char Config::getByte(string name) {
	string *value = this->getProperty(name);
	if (value->size() == 0)  return Config::BYTE_DEFAULT;
	return (unsigned char)atoi(value->c_str());
}

void Config::setValue(string name, unsigned char value) {
	unsigned char oldValue = this->getByte(name);
	if (oldValue != value) {
		char buffer[50];
		sprintf(buffer, "%d", value);
		this->setProperty(name, string(buffer));
	}
}

string *Config::getPassword(string name) {
	string *value = this->getProperty(name);
	if (value->size() == 0)  return new string(Config::PASSWORD_DEFAULT);

	try {
		string *tag = this->decrypt(*value, Config::KEY);
		delete value;
		return tag;
	} catch (...) {
		return new string(Config::PASSWORD_DEFAULT);
	}
}

void Config::setPassword(string name, string value) {
	string *oldValue = this->getPassword(name);
	if (oldValue->compare(value) != 0) {
		try {
			string *tag = this->encrypt(value, Config::KEY);
			this->setProperty(name, *tag);
			delete tag;
		} catch (...) {
		}
	}
	delete oldValue;
}

tm *Config::getDate(string name) {
	string *value = this->getProperty(name);
	if (value->size() == 0)  return Config::current_time();
	try {
		return Config::parse_time(value->c_str());
	} catch (...) {
		return Config::current_time();
	}
}

void Config::setValue(string name, tm value) {
	tm *oldValue = this->getDate(name);
	if (Config::compare_time(*oldValue, value) != 0) {
		string *tag = Config::format_time(value);
		this->setProperty(name, *tag);
	}
}

void Config::save(const char *filename) {
	map<string, string>::iterator it;

	ofstream file;
	file.open(filename, ios::out);

	for (it = this->properties.begin(); it != this->properties.end(); it++) {
		file << (*it).first << "=" << (*it).second << endl;
	}

	file.close();
}

void Config::load(const char *filename) {
	this->properties.clear();

	string line;
	ifstream file;
	file.open(filename, ios::in);

	if (file.is_open()) {
		while (file.good()) {
			getline(file, line);
			size_t found = line.find("=");
			if (found != string::npos) {
				int pos = (int)found;
				string key = line.substr(0, pos);
				string value = line.substr(pos + 1);
				this->setProperty(key, value);
			}
		}
		file.close();
	}
}

string *Config::encrypt(string value, string key) {
	string *tag = new string("");

	for (int i = 0; i < value.size(); i++) {
		char c = value.at(i);
		int v = (int)c;
		v += 5;
		if (v >= 256) {
			v -= 256;
		}
		c = (char)v;
		tag->insert(i, 1, c);
	}

	return tag;
}

string *Config::decrypt(string value, string key) {
	string *tag = new string("");

	for (int i = 0; i < value.size(); i++) {
		char c = value.at(i);
		int v = (int)c;
		v -= 5;
		if (v < 0) {
			v += 256;
		}
		c = (char)v;
		tag->insert(i, 1, c);
	}

	return tag;
}

tm *Config::current_time() {
	time_t timer = time(NULL);
	return localtime(&timer);
}

string *Config::format_time(tm value) {
	char daybuf[50];
	strftime(daybuf, sizeof(daybuf), "%m/%d/%Y %H:%M:%S", &value);
	return new string(daybuf);
}

tm *Config::parse_time(const char *value) {
	tm *tag = new tm();
	string src = string(value);
	tag->tm_year = atoi(src.substr(6, 4).c_str()) - 1900;
	tag->tm_mon = atoi(src.substr(0, 2).c_str()) - 1;
	tag->tm_mday = atoi(src.substr(3, 2).c_str());
	tag->tm_hour = atoi(src.substr(11, 2).c_str());
	tag->tm_min = atoi(src.substr(14, 2).c_str());
	tag->tm_sec = atoi(src.substr(17, 2).c_str());
	mktime(tag);
	return tag;
}

int Config::compare_time(tm a, tm b) {
	if (a.tm_year > b.tm_year) {
		return 1;
	} else if (a.tm_year < b.tm_year) {
		return -1;
	}
	if (a.tm_mon > b.tm_mon) {
		return 1;
	} else if (a.tm_mon < b.tm_mon) {
		return -1;
	}
	if (a.tm_mday > b.tm_mday) {
		return 1;
	} else if (a.tm_mday < b.tm_mday) {
		return -1;
	}
	if (a.tm_hour > b.tm_hour) {
		return 1;
	} else if (a.tm_hour < b.tm_hour) {
		return -1;
	}
	if (a.tm_min > b.tm_min) {
		return 1;
	} else if (a.tm_min < b.tm_min) {
		return -1;
	}
	if (a.tm_sec > b.tm_sec) {
		return 1;
	} else if (a.tm_sec < b.tm_sec) {
		return -1;
	}
	return 0;
}