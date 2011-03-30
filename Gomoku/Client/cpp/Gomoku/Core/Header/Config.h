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

#if !defined(AFX_CONFIG_H__A9EAAD14_1507_4ACF_9568_2292FBAC3558__INCLUDED_)
#define AFX_CONFIG_H__A9EAAD14_1507_4ACF_9568_2292FBAC3558__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#include <map>
#include <string>
#include <time.h>

using namespace std;

namespace gomoku_core {

	class Config {
	public:
		static const bool BOOLEAN_DEFAULT;
		static const double DOUBLE_DEFAULT;
		static const float FLOAT_DEFAULT;
		static const int INT_DEFAULT;
		static const long LONG_DEFAULT;
		static const unsigned char BYTE_DEFAULT;
		static const string STRING_DEFAULT;
		static const string PASSWORD_DEFAULT;

		static const string TRUE_VALUE;
		static const string FALSE_VALUE;

		static const string SERVER_URL;
		static const string SERVER_USERNAME;
		static const string SERVER_PASSWORD;
		static const string SERVER_EXTENSION;
		static const string SEARCH_DEPTH;
		static const string FIRST_TYPE;
		static const string SECOND_TYPE;
		static const string BOARD_WIDTH;
		static const string BOARD_HEIGHT;
		static const string CURRENT_GAME;

		Config();
		virtual ~Config();
		
		string *getString(string name);
		void setValue(string name, string value);

		bool getBoolean(string name);
		void setValue(string name, bool value);

		double getDouble(string name);
		void setValue(string name, double value);

		float getFloat(string name);
		void setValue(string name, float value);

		int getInt(string name);
		void setValue(string name, int value);

		long getLong(string name);
		void setValue(string name, long value);

		unsigned char getByte(string name);
		void setValue(string name, unsigned char value);

		string *getPassword(string name);
		void setPassword(string name, string value);

		tm *getDate(string name);
		void setValue(string name, tm value);

		void save(const char *filename);
		void load(const char *filename);

		static string *encrypt(string value, string key);
		static string *decrypt(string value, string key);
		static tm *current_time();
		static tm *parse_time(const char *value);
		static int compare_time(tm a, tm b);
		static string *format_time(tm value);

	private:
		static const string KEY;

		map<string, string> properties;

		string *getProperty(string name);
		void setProperty(string name, string value);
	};

}

#endif // !defined(AFX_CONFIG_H__A9EAAD14_1507_4ACF_9568_2292FBAC3558__INCLUDED_)
