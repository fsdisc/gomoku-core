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

#if !defined(AFX_PLAYERRUNNER_H__A8A9F142_85E7_4B7A_93BD_F419E2E842F0__INCLUDED_)
#define AFX_PLAYERRUNNER_H__A8A9F142_85E7_4B7A_93BD_F419E2E842F0__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#include <list>
#include <string>
#include <windows.h>

#include "Runner.h"
#include "Board.h"
#include "BoardUI.h"

using namespace std;

LRESULT CALLBACK GenWindowProc(HWND hWnd, UINT uMsg, WPARAM wParam, LPARAM lParam);

class Window {
public:
	HWND hWnd;

	Window(HINSTANCE hInst, HWND parent);
	virtual ~Window();
	virtual LRESULT WindowProc(UINT uMsg, WPARAM wParam, LPARAM lParam);
	virtual void Show();
	virtual void Resize(int width, int height);
	virtual void Wait();
	virtual void Register();
	virtual void Create();

protected:
	HINSTANCE hInst;
	bool closed;
	HWND parent;

	HWND CreateButton(const char *text, int x, int y, int w, int h);
	HWND CreateLabel(const char *text, int x, int y, int w, int h);
	HWND CreateTextBox(const char *text, int x, int y, int w, int h);
	HWND CreateComboBox(int x, int y, int w, int h);
	HWND CreatePassword(const char *text, int x, int y, int w, int h);
	HWND CreateListBox(int x, int y, int w, int h);

	friend LRESULT CALLBACK ::GenWindowProc(HWND hWnd, UINT uMsg, WPARAM wParam, LPARAM lParam);
};

namespace gomoku_core {

	class PlayerRunner : public Runner {
	public:
		PlayerRunner(HINSTANCE hInst);
		virtual ~PlayerRunner();

		virtual void start();

	protected:
		Window *frame;
		HINSTANCE hInst;

		virtual void createBoardUI();
		virtual void createMainUI();
		virtual void blackWin();
		virtual void whiteWin();
		virtual void drawEnd();

		virtual void resizeMain();
		virtual void drawJoinGame();
		virtual void drawSettings();
		virtual void drawNewGame();
		virtual string *getTitle();
		virtual void getPlayerNames(bool newGame, list<string> &tag);
		virtual int getPlayerIndex(bool newGame, unsigned char type);
		virtual unsigned char getPlayerType(bool newGame, int index);
		virtual bool isRemotePlayer(int index);

		class MainWindow : public Window {
		public:
			MainWindow(HINSTANCE hInst, PlayerRunner *runner);
			virtual ~MainWindow();
			LRESULT WindowProc(UINT uMsg, WPARAM wParam, LPARAM lParam);
			virtual void Resize(int width, int height);
			virtual void Create();

		protected:
			PlayerRunner *runner;
			HWND cmdNewGame;
			HWND cmdJoinGame;
			HWND cmdSettings;
		};

		friend class MainWindow;

		class Win32BoardUI : public BoardUI {
		public:
			HWND hWnd;

			Win32BoardUI(Board *board);
			virtual ~Win32BoardUI();

			virtual void update();
			virtual void mouseClicked(int x, int y);

		protected:
			HDC hdc;

			virtual void drawLine(int x1, int y1, int x2, int y2);
			virtual void drawText(int x, int y, string text);
			virtual void drawOval(int x, int y, int width, int height);
			virtual void fillOval(int x, int y, int width, int height);

		};

		class NewGameWindow : public Window {
		public:
			NewGameWindow(HINSTANCE hInst, HWND parent, PlayerRunner *runner);
			virtual ~NewGameWindow();
			LRESULT WindowProc(UINT uMsg, WPARAM wParam, LPARAM lParam);
			virtual void Resize(int width, int height);
			virtual void Create();

		protected:
			PlayerRunner *runner;
			HWND cmdOK;
			HWND cmdCancel;
			HWND txtWidth;
			HWND txtHeight;
			HWND cboFirst;
			HWND cboSecond;

		};

		friend class NewGameWindow;

		class SettingsWindow : public Window {
		public:
			SettingsWindow(HINSTANCE hInst, HWND parent, PlayerRunner *runner);
			virtual ~SettingsWindow();
			LRESULT WindowProc(UINT uMsg, WPARAM wParam, LPARAM lParam);
			virtual void Resize(int width, int height);
			virtual void Create();

		protected:
			PlayerRunner *runner;
			HWND cmdOK;
			HWND cmdCancel;
			HWND txtServerUrl;
			HWND txtUsername;
			HWND txtPassword;

		};

		friend class SettingsWindow;

		class JoinGameWindow : public Window {
		public:
			JoinGameWindow(HINSTANCE hInst, HWND parent, PlayerRunner *runner);
			virtual ~JoinGameWindow();
			LRESULT WindowProc(UINT uMsg, WPARAM wParam, LPARAM lParam);
			virtual void Resize(int width, int height);
			virtual void Create();

		protected:
			PlayerRunner *runner;
			HWND cmdOK;
			HWND cmdCancel;
			HWND lstGame;
			HWND cboPlayer;
			list<string> games;
			void split(string src, string sep, list<string> &tag);

		};

		friend class JoinGameWindow;

	};

}

#endif // !defined(AFX_PLAYERRUNNER_H__A8A9F142_85E7_4B7A_93BD_F419E2E842F0__INCLUDED_)
