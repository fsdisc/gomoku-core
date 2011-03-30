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

#include "..\Header\PlayerRunner.h"
#include "..\Header\Game.h"
#include "..\Header\BoardUI.h"

using namespace gomoku_core;

void PlayerRunner::JoinGameWindow::split(string src, string sep, list<string> &tag) {
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

PlayerRunner::JoinGameWindow::JoinGameWindow(HINSTANCE hInst, HWND parent, PlayerRunner *runner) : Window(hInst, parent) {
	this->runner = runner;
}

PlayerRunner::JoinGameWindow::~JoinGameWindow() {
}

LRESULT PlayerRunner::JoinGameWindow::WindowProc(UINT uMsg, WPARAM wParam, LPARAM lParam) {
	LRESULT result;
	switch (uMsg) {
	case WM_CTLCOLORSTATIC :
		SetBkColor((HDC)wParam, RGB(255,255,255));
		return (LONG)GetStockObject(WHITE_BRUSH);
	case WM_COMMAND:
		if (HIWORD(wParam) == BN_CLICKED) {
			HWND hwnd = (HWND)lParam;
			if (this->cmdOK == hwnd) {
				int idx = (int)SendMessage(this->lstGame, LB_GETCURSEL, 0, 0);
				if (idx == LB_ERR) {
					MessageBox(this->hWnd, "Game is required to select!", "Warning", MB_OK);
					return TRUE;
				}
				list<string>::iterator it;
				int no = 0;
				string line = "";
				for (it = this->games.begin(); it != this->games.end(); it++) {
					if (no == idx) {
						line = (*it);
						break;
					}
					no++;
				}
				list<string> fields;
				this->split(line, "|", fields);
				it = fields.begin();
				string id = *it; it++;
				string first = *it; it++;
				string second = *it; it++;
				string width = *it; it++;
				string height = *it; it++;
				idx = (int)SendMessage(this->cboPlayer, CB_GETCURSEL, 0, 0);
				unsigned char player = this->runner->getPlayerType(false, idx);
				if (first.length() > 0) {
                    this->runner->config->setValue(Config::FIRST_TYPE, Game::REMOTE_PLAYER);
                    this->runner->config->setValue(Config::SECOND_TYPE, player);
				} else {
                    this->runner->config->setValue(Config::FIRST_TYPE, player);
                    this->runner->config->setValue(Config::SECOND_TYPE, Game::REMOTE_PLAYER);
				}
                this->runner->config->setValue(Config::BOARD_WIDTH, atoi(width.c_str()));
                this->runner->config->setValue(Config::BOARD_HEIGHT, atoi(height.c_str()));
                this->runner->createGame();
                this->runner->resizeMain();
                try {
                    this->runner->joinGame(id);
                } catch (string s) {
					MessageBox(this->hWnd, s.c_str(), "Error", MB_OK);
					return TRUE;
				} catch (...) {
					MessageBox(this->hWnd, "Fail to join game!", "Error", MB_OK);
					return TRUE;
				}

				this->closed = true;
				SendMessage(this->hWnd, WM_SYSCOMMAND, SC_CLOSE, 0);
				RedrawWindow(this->parent, NULL, NULL, RDW_ERASE | RDW_INVALIDATE | RDW_ERASENOW | RDW_UPDATENOW | RDW_ALLCHILDREN);
			}
			if (this->cmdCancel == hwnd) {
				this->closed = true;
				SendMessage(this->hWnd, WM_SYSCOMMAND, SC_CLOSE, 0);
			}
		}
		result = TRUE;
		break;
	default:
		result = Window::WindowProc(uMsg, wParam, lParam);
		break;
	}
	return result;
}

void PlayerRunner::JoinGameWindow::Resize(int width, int height) {
	Window::Resize(width, height);
	
	int w = 0;
	int h = 0;
	int x = 0;
	int y = 0;
	RECT rcOK, rcCancel;
	GetClientRect(this->cmdOK, &rcOK);
	GetClientRect(this->cmdCancel, &rcCancel);
	w += (rcOK.right - rcOK.left) + (rcCancel.right - rcCancel.left);
	h += rcOK.bottom - rcOK.top;
	y = height - h - 10;
	x = (width - w - 10) / 2;
	w = (rcOK.right - rcOK.left);
	MoveWindow(this->cmdOK, x, y, w, h, TRUE);
	x += w + 10;
	w = (rcCancel.right - rcCancel.left);
	MoveWindow(this->cmdCancel, x, y, w, h, TRUE);
	x += w + 10;
}

void PlayerRunner::JoinGameWindow::Create() {
	Window::Create();
	SetWindowText(this->hWnd, "Join Game");
	this->cmdOK = this->CreateButton("OK", 0, 0, 75, 25);
	this->cmdCancel = this->CreateButton("Cancel", 110, 0, 75, 25);

	list<string> names;
	list<string>::iterator it;
	list<string>::iterator itc;
	char buffer[255];

	this->runner->getPlayerNames(false, names);
	this->runner->game->listGame(this->games);

	this->lstGame = this->CreateListBox(10, 10, 250, 300);
	for (it = this->games.begin(); it != this->games.end(); it++) {
		list<string> fields;
		this->split(*it, "|", fields);
		itc = fields.begin();
		string id = *itc; itc++;
		string first = *itc; itc++;
		string second = *itc; itc++;
		string width = *itc; itc++;
		string height = *itc; itc++;
		if (first.length() > 0) {
			sprintf(buffer, "%s (%sx%s, First: %s)", id.c_str(), width.c_str(), height.c_str(), first.c_str());
		} else {
			sprintf(buffer, "%s (%sx%s, Second: %s)", id.c_str(), width.c_str(), height.c_str(), second.c_str());
		}
		SendMessage(this->lstGame, LB_ADDSTRING, 0, reinterpret_cast<LPARAM>(buffer));
	}

	this->CreateLabel("Player", 10, 310, 60, 20);
	this->cboPlayer = this->CreateComboBox(70, 308, 190, 20 * (names.size() + 1));
	for (it = names.begin(); it != names.end(); it++) {
		sprintf(buffer, "%s", (*it).c_str());
		SendMessage(this->cboPlayer, CB_ADDSTRING, 0, reinterpret_cast<LPARAM>(buffer));
	}
	SendMessage(this->cboPlayer, CB_SETCURSEL, 0, 0);
}

PlayerRunner::SettingsWindow::SettingsWindow(HINSTANCE hInst, HWND parent, PlayerRunner *runner) : Window(hInst, parent) {
	this->runner = runner;
}

PlayerRunner::SettingsWindow::~SettingsWindow() {
}

LRESULT PlayerRunner::SettingsWindow::WindowProc(UINT uMsg, WPARAM wParam, LPARAM lParam) {
	LRESULT result;
	switch (uMsg) {
	case WM_CTLCOLORSTATIC :
		SetBkColor((HDC)wParam, RGB(255,255,255));
		return (LONG)GetStockObject(WHITE_BRUSH);
	case WM_COMMAND:
		if (HIWORD(wParam) == BN_CLICKED) {
			HWND hwnd = (HWND)lParam;
			if (this->cmdOK == hwnd) {
				char buffer[255];

				GetWindowText(this->txtServerUrl, buffer, sizeof(buffer));
				this->runner->config->setValue(Config::SERVER_URL, string(buffer));

				GetWindowText(this->txtUsername, buffer, sizeof(buffer));
				this->runner->config->setValue(Config::SERVER_USERNAME, string(buffer));

				GetWindowText(this->txtPassword, buffer, sizeof(buffer));
				this->runner->config->setPassword(Config::SERVER_PASSWORD, string(buffer));

				try {
					this->runner->client->login();
				} catch (string s) {
					MessageBox(this->hWnd, s.c_str(), "Error", MB_OK);
					return TRUE;
				} catch (...) {
					MessageBox(this->hWnd, "Fail to login using typed account!", "Error", MB_OK);
					return TRUE;
				}

				this->closed = true;
				SendMessage(this->hWnd, WM_SYSCOMMAND, SC_CLOSE, 0);
			}
			if (this->cmdCancel == hwnd) {
				this->closed = true;
				SendMessage(this->hWnd, WM_SYSCOMMAND, SC_CLOSE, 0);
			}
		}
		result = TRUE;
		break;
	default:
		result = Window::WindowProc(uMsg, wParam, lParam);
		break;
	}
	return result;
}

void PlayerRunner::SettingsWindow::Resize(int width, int height) {
	Window::Resize(width, height);
	
	int w = 0;
	int h = 0;
	int x = 0;
	int y = 0;
	RECT rcOK, rcCancel;
	GetClientRect(this->cmdOK, &rcOK);
	GetClientRect(this->cmdCancel, &rcCancel);
	w += (rcOK.right - rcOK.left) + (rcCancel.right - rcCancel.left);
	h += rcOK.bottom - rcOK.top;
	y = height - h - 10;
	x = (width - w - 10) / 2;
	w = (rcOK.right - rcOK.left);
	MoveWindow(this->cmdOK, x, y, w, h, TRUE);
	x += w + 10;
	w = (rcCancel.right - rcCancel.left);
	MoveWindow(this->cmdCancel, x, y, w, h, TRUE);
	x += w + 10;
}

void PlayerRunner::SettingsWindow::Create() {
	Window::Create();
	SetWindowText(this->hWnd, "Settings");
	this->cmdOK = this->CreateButton("OK", 0, 0, 75, 25);
	this->cmdCancel = this->CreateButton("Cancel", 110, 0, 75, 25);
	this->CreateLabel("Server URL", 10, 10, 100, 20);
	this->txtServerUrl = this->CreateTextBox(this->runner->config->getString(Config::SERVER_URL)->c_str(), 100, 8, 285, 20);
	this->CreateLabel("Username", 10, 35, 100, 20);
	this->txtUsername = this->CreateTextBox(this->runner->config->getString(Config::SERVER_USERNAME)->c_str(), 100, 33, 285, 20);
	this->CreateLabel("Password", 10, 60, 100, 20);
	this->txtPassword = this->CreatePassword(this->runner->config->getPassword(Config::SERVER_PASSWORD)->c_str(), 100, 58, 285, 20);
}

PlayerRunner::NewGameWindow::NewGameWindow(HINSTANCE hInst, HWND parent, PlayerRunner *runner) : Window(hInst, parent) {
	this->runner = runner;
}

PlayerRunner::NewGameWindow::~NewGameWindow() {
}

LRESULT PlayerRunner::NewGameWindow::WindowProc(UINT uMsg, WPARAM wParam, LPARAM lParam) {
	LRESULT result;
	switch (uMsg) {
	case WM_CTLCOLORSTATIC :
		SetBkColor((HDC)wParam, RGB(255,255,255));
		return (LONG)GetStockObject(WHITE_BRUSH);
	case WM_COMMAND:
		if (HIWORD(wParam) == BN_CLICKED) {
			HWND hwnd = (HWND)lParam;
			if (this->cmdOK == hwnd) {
				char buffer[50];
				GetWindowText(this->txtWidth, buffer, sizeof(buffer));
				int width = atoi(buffer);
				if (width < 10 || width > 255) {
					MessageBox(this->hWnd, "Width must be in [10-255] range!", "Warning", MB_OK);
					return TRUE;
				}
				GetWindowText(this->txtHeight, buffer, sizeof(buffer));
				int height = atoi(buffer);
				if (height < 10 || height > 255) {
					MessageBox(this->hWnd, "Height must be in [10-255] range!", "Warning", MB_OK);
					return TRUE;
				}
				int idxFirst = (int)SendMessage(this->cboFirst, CB_GETCURSEL, 0, 0);
				int idxSecond = (int)SendMessage(this->cboSecond, CB_GETCURSEL, 0, 0);
				if (this->runner->isRemotePlayer(idxFirst) && this->runner->isRemotePlayer(idxSecond)) {
					MessageBox(this->hWnd, "Both first player and second player can not be remote!", "Warning", MB_OK);
					return TRUE;
				}
				if (this->runner->isRemotePlayer(idxFirst) || this->runner->isRemotePlayer(idxSecond)) {
					if (!this->runner->client->online()) {
						MessageBox(this->hWnd, "Remote player requires valid login account!", "Warning", MB_OK);
						return TRUE;
					}
				}
				unsigned char firstType = this->runner->getPlayerType(true, idxFirst);
				unsigned char secondType = this->runner->getPlayerType(true, idxSecond);
                this->runner->config->setValue(Config::BOARD_WIDTH, width);
                this->runner->config->setValue(Config::BOARD_HEIGHT, height);
                this->runner->config->setValue(Config::FIRST_TYPE, firstType);
                this->runner->config->setValue(Config::SECOND_TYPE, secondType);
                this->runner->createGame();
                this->runner->resizeMain();
                try {
                    this->runner->newGame();
                } catch (string s) {
					MessageBox(this->hWnd, s.c_str(), "Error", MB_OK);
					return TRUE;
				} catch (...) {
					MessageBox(this->hWnd, "Fail to create game!", "Error", MB_OK);
					return TRUE;
				}
				this->closed = true;
				SendMessage(this->hWnd, WM_SYSCOMMAND, SC_CLOSE, 0);
				RedrawWindow(this->parent, NULL, NULL, RDW_ERASE | RDW_INVALIDATE | RDW_ERASENOW | RDW_UPDATENOW | RDW_ALLCHILDREN);
			}
			if (this->cmdCancel == hwnd) {
				this->closed = true;
				SendMessage(this->hWnd, WM_SYSCOMMAND, SC_CLOSE, 0);
			}
		}
		result = TRUE;
		break;
	default:
		result = Window::WindowProc(uMsg, wParam, lParam);
		break;
	}
	return result;
}

void PlayerRunner::NewGameWindow::Resize(int width, int height) {
	Window::Resize(width, height);
	
	int w = 0;
	int h = 0;
	int x = 0;
	int y = 0;
	RECT rcOK, rcCancel;
	GetClientRect(this->cmdOK, &rcOK);
	GetClientRect(this->cmdCancel, &rcCancel);
	w += (rcOK.right - rcOK.left) + (rcCancel.right - rcCancel.left);
	h += rcOK.bottom - rcOK.top;
	y = height - h - 10;
	x = (width - w - 10) / 2;
	w = (rcOK.right - rcOK.left);
	MoveWindow(this->cmdOK, x, y, w, h, TRUE);
	x += w + 10;
	w = (rcCancel.right - rcCancel.left);
	MoveWindow(this->cmdCancel, x, y, w, h, TRUE);
	x += w + 10;
}

void PlayerRunner::NewGameWindow::Create() {
	Window::Create();
	SetWindowText(this->hWnd, "New Game");
	list<string> names;
	this->runner->getPlayerNames(true, names);
	this->cmdOK = this->CreateButton("OK", 0, 0, 75, 25);
	this->cmdCancel = this->CreateButton("Cancel", 110, 0, 75, 25);
	this->CreateLabel("Board width", 10, 10, 100, 20);
	this->txtWidth = this->CreateTextBox(this->runner->config->getString(Config::BOARD_WIDTH)->c_str(), 110, 8, 150, 20);
	this->CreateLabel("Board height", 10, 35, 100, 20);
	this->txtHeight = this->CreateTextBox(this->runner->config->getString(Config::BOARD_HEIGHT)->c_str(), 110, 33, 150, 20);
	this->CreateLabel("First player", 10, 60, 100, 20);
	this->cboFirst = this->CreateComboBox(110, 58, 150, 20 * (names.size() + 1));
	this->CreateLabel("Second player", 10, 85, 100, 20);
	this->cboSecond = this->CreateComboBox(110, 83, 150, 20 * (names.size() + 1));
	list<string>::iterator it;
	for (it = names.begin(); it != names.end(); it++) {
		char buffer[50];
		sprintf(buffer, "%s", (*it).c_str());
		SendMessage(this->cboFirst, CB_ADDSTRING, 0, reinterpret_cast<LPARAM>(buffer));
		SendMessage(this->cboSecond, CB_ADDSTRING, 0, reinterpret_cast<LPARAM>(buffer));
	}
	int idxFirst = this->runner->getPlayerIndex(true, this->runner->config->getByte(Config::FIRST_TYPE));
	int idxSecond = this->runner->getPlayerIndex(true, this->runner->config->getByte(Config::SECOND_TYPE));
	SendMessage(this->cboFirst, CB_SETCURSEL, idxFirst, 0);
	SendMessage(this->cboSecond, CB_SETCURSEL, idxSecond, 0);
}

PlayerRunner::Win32BoardUI::Win32BoardUI(Board *board) : BoardUI(board) {

}

PlayerRunner::Win32BoardUI::~Win32BoardUI() {

}

void PlayerRunner::Win32BoardUI::mouseClicked(int x, int y) {
	x = x - LEFT_MARGIN;
	y = y - TOP_MARGIN;
	if (x < 0 || y < 0) return;
	x = x / BoardUI.CELL_WIDTH;
	y = y / BoardUI.CELL_HEIGHT;
	if (x >= this->board->getWidth() || y >= this->board->getHeight()) return;
	Move *move = new Move(y, x, this->board->getCurrentPiece());
	this->fireMoveMade(*move);
}

void PlayerRunner::Win32BoardUI::update() {
	this->hdc = GetDC(this->hWnd);
	BoardUI::update();
	ReleaseDC(this->hWnd, this->hdc);
}

void PlayerRunner::Win32BoardUI::drawLine(int x1, int y1, int x2, int y2) {
	MoveToEx(this->hdc, x1, y1, NULL);
	LineTo(this->hdc, x2, y2);
}

void PlayerRunner::Win32BoardUI::drawText(int x, int y, string text) {
	char buffer[50];
	sprintf(buffer, "%s", text.c_str());
	TextOut(this->hdc, x, y, buffer, strlen(buffer));
}

void PlayerRunner::Win32BoardUI::drawOval(int x, int y, int width, int height) {
	SelectObject(this->hdc, GetStockObject(WHITE_BRUSH));
	SelectObject(this->hdc, GetStockObject(BLACK_PEN));
	Ellipse(this->hdc, x, y, x + width, y + height);
}

void PlayerRunner::Win32BoardUI::fillOval(int x, int y, int width, int height) {
	SelectObject(this->hdc, GetStockObject(BLACK_BRUSH));
	SelectObject(this->hdc, GetStockObject(BLACK_PEN));
	Ellipse(this->hdc, x, y, x + width, y + height);
}

PlayerRunner::MainWindow::MainWindow(HINSTANCE hInst, PlayerRunner *runner) : Window(hInst, NULL) {
	this->runner = runner;
}

PlayerRunner::MainWindow::~MainWindow() {

}

LRESULT PlayerRunner::MainWindow::WindowProc(UINT uMsg, WPARAM wParam, LPARAM lParam) {
	LRESULT result;
	switch (uMsg) {
	case WM_DESTROY:
		this->runner->game->dispose();
		this->runner->client->logout();
		this->closed = true;
		PostQuitMessage(0);
		result = TRUE;
		break;
	case WM_COMMAND:
		if (HIWORD(wParam) == BN_CLICKED) {
			HWND hwnd = (HWND)lParam;
			if (this->cmdNewGame == hwnd) {
				this->runner->drawNewGame();
			}
			if (this->cmdJoinGame == hwnd) {
				this->runner->drawJoinGame();
			}
			if (this->cmdSettings == hwnd) {
				this->runner->drawSettings();
			}
		}
		result = TRUE;
		break;
	case WM_PAINT:
		this->runner->boardUI->update();
		result = Window::WindowProc(uMsg, wParam, lParam);
		break;
	case WM_LBUTTONUP:
		if (wParam == 0) {
			int x = LOWORD(lParam); 
			int y = HIWORD(lParam); 
			Win32BoardUI *ui;
			ui = (Win32BoardUI *)this->runner->boardUI;
			ui->mouseClicked(x, y);
		}
		break;
	default:
		result = Window::WindowProc(uMsg, wParam, lParam);
		break;
	}
	return result;
}

void PlayerRunner::MainWindow::Create() {
	Window::Create();
	SetWindowText(this->hWnd, this->runner->getTitle()->c_str());
	this->cmdNewGame = this->CreateButton("New Game", 0, 0, 100, 25);
	this->cmdJoinGame = this->CreateButton("Join Game", 110, 0, 100, 25);
	this->cmdSettings = this->CreateButton("Settings", 210, 0, 100, 25);
	Win32BoardUI *ui;
	ui = (Win32BoardUI *)this->runner->boardUI;
	ui->hWnd = this->hWnd;
}

void PlayerRunner::MainWindow::Resize(int width, int height) {
	Window::Resize(width, height);
	
	int w = 0;
	int h = 0;
	int x = 0;
	int y = 0;
	RECT rcNewGame, rcJoinGame, rcSettings;
	GetClientRect(this->cmdNewGame, &rcNewGame);
	GetClientRect(this->cmdJoinGame, &rcJoinGame);
	GetClientRect(this->cmdSettings, &rcSettings);
	w += (rcNewGame.right - rcNewGame.left) + (rcJoinGame.right - rcJoinGame.left) + (rcSettings.right - rcSettings.left);
	h += rcNewGame.bottom - rcNewGame.top;
	y = height - h - 10;
	x = (width - w - 20) / 2;
	w = (rcNewGame.right - rcNewGame.left);
	MoveWindow(this->cmdNewGame, x, y, w, h, TRUE);
	x += w + 10;
	w = (rcJoinGame.right - rcJoinGame.left);
	MoveWindow(this->cmdJoinGame, x, y, w, h, TRUE);
	x += w + 10;
	w = (rcSettings.right - rcSettings.left);
	MoveWindow(this->cmdSettings, x, y, w, h, TRUE);
	x += w + 10;
}

LRESULT CALLBACK ::GenWindowProc(HWND hWnd, UINT uMsg, WPARAM wParam, LPARAM lParam) {
	Window* pWin = (Window*) GetWindowLong(hWnd, GWL_USERDATA);

	switch (uMsg) {
	case WM_NCCREATE:
		// Get the initial creation pointer to the window object
		pWin = (Window *) ((CREATESTRUCT *)lParam)->lpCreateParams;

		pWin->hWnd = hWnd;

		// Set its USERDATA DWORD to point to the window object
		SetWindowLong(hWnd, GWL_USERDATA, (long) pWin);
		break;
	}

	// Call its message proc method.
	if (NULL != pWin)
		return (pWin->WindowProc(uMsg, wParam, lParam));
	else
		return (DefWindowProc(hWnd, uMsg, wParam, lParam));
}

Window::Window(HINSTANCE hInst, HWND parent) { 
	this->hInst = hInst; 
	this->parent = parent;
	this->hWnd = NULL; 
	this->closed = false;
};

Window::~Window() {
 
};

HWND Window::CreateButton(const char *text, int x, int y, int w, int h) {
	HWND hwnd = CreateWindow( 
		"BUTTON",
		text,
		WS_TABSTOP | WS_VISIBLE | WS_CHILD | BS_DEFPUSHBUTTON,  // Styles. 
		x,         // x position. 
		y,         // y position. 
		w,        // Button width.
		h,        // Button height.
		this->hWnd,       // Parent window.
		NULL,       // Menu.
		(HINSTANCE)GetWindowLong(this->hWnd, GWL_HINSTANCE), 
		NULL);      // Pointer not needed.
	return hwnd;
}

HWND Window::CreateLabel(const char *text, int x, int y, int w, int h) {
	HWND hwnd = CreateWindow( 
		"STATIC",
		text,
		SS_LEFT | WS_VISIBLE | WS_CHILD,  // Styles. 
		x,         // x position. 
		y,         // y position. 
		w,        // Width.
		h,        // Height.
		this->hWnd,       // Parent window.
		NULL,       // Menu.
		(HINSTANCE)GetWindowLong(this->hWnd, GWL_HINSTANCE), 
		NULL);      // Pointer not needed.

	return hwnd;
}

HWND Window::CreateTextBox(const char *text, int x, int y, int w, int h) {
	HWND hwnd = CreateWindow( 
		"EDIT",
		text,
		WS_VISIBLE | WS_CHILD | WS_TABSTOP | ES_LEFT | WS_BORDER,  // Styles. 
		x,         // x position. 
		y,         // y position. 
		w,        // Width.
		h,        // Height.
		this->hWnd,       // Parent window.
		NULL,       // Menu.
		(HINSTANCE)GetWindowLong(this->hWnd, GWL_HINSTANCE), 
		NULL);      // Pointer not needed.

	return hwnd;
}

HWND Window::CreatePassword(const char *text, int x, int y, int w, int h) {
	HWND hwnd = CreateWindow( 
		"EDIT",
		text,
		WS_VISIBLE | WS_CHILD | WS_TABSTOP | ES_LEFT | WS_BORDER | ES_PASSWORD,  // Styles. 
		x,         // x position. 
		y,         // y position. 
		w,        // Width.
		h,        // Height.
		this->hWnd,       // Parent window.
		NULL,       // Menu.
		(HINSTANCE)GetWindowLong(this->hWnd, GWL_HINSTANCE), 
		NULL);      // Pointer not needed.
	return hwnd;
}

HWND Window::CreateComboBox(int x, int y, int w, int h) {
	HWND hwnd = CreateWindow( 
		"COMBOBOX",
		"",
		WS_VISIBLE | WS_CHILD | WS_TABSTOP | CBS_DROPDOWNLIST,  // Styles. 
		x,         // x position. 
		y,         // y position. 
		w,        // Width.
		h,        // Height.
		this->hWnd,       // Parent window.
		NULL,       // Menu.
		(HINSTANCE)GetWindowLong(this->hWnd, GWL_HINSTANCE), 
		NULL);      // Pointer not needed.

	return hwnd;
}

HWND Window::CreateListBox(int x, int y, int w, int h) {
	HWND hwnd = CreateWindow( 
		"LISTBOX",
		"",
		WS_VISIBLE | WS_CHILD | WS_TABSTOP | WS_VSCROLL | WS_BORDER,  // Styles. 
		x,         // x position. 
		y,         // y position. 
		w,        // Width.
		h,        // Height.
		this->hWnd,       // Parent window.
		NULL,       // Menu.
		(HINSTANCE)GetWindowLong(this->hWnd, GWL_HINSTANCE), 
		NULL);      // Pointer not needed.

	return hwnd;
}

void Window::Register() {
	WNDCLASSEX wcf;

	wcf.cbSize        = sizeof(WNDCLASSEX);
	wcf.cbClsExtra    = 0;
	wcf.cbWndExtra    = 0;
	wcf.hInstance     = this->hInst;
	wcf.lpfnWndProc   = ::GenWindowProc;
	wcf.hCursor       = LoadCursor(NULL, IDC_ARROW);
	wcf.hIcon         = LoadIcon(NULL, IDI_APPLICATION);
	wcf.hIconSm       = LoadIcon(NULL, IDI_APPLICATION);
	wcf.hbrBackground = (HBRUSH)GetStockObject(WHITE_BRUSH);
	wcf.style         = 0;
	wcf.lpszClassName = "Window";
	wcf.lpszMenuName  = NULL;

	RegisterClassEx(&wcf);
}

void Window::Create() {
	CreateWindowEx(
		WS_EX_APPWINDOW, 
		"Window",
		"",
		WS_CAPTION | WS_SYSMENU | WS_MINIMIZEBOX | WS_BORDER,
		CW_USEDEFAULT, 
		CW_USEDEFAULT,
		1,
		1,
		this->parent,
		NULL,
		this->hInst,
		this);
}

LRESULT Window::WindowProc(UINT uMsg, WPARAM wParam, LPARAM lParam) {
	LRESULT result;
    switch (uMsg) {
	case WM_DESTROY:
		this->closed = true;
		SendMessage(this->hWnd, WM_SYSCOMMAND, SC_CLOSE, 0);
		result = TRUE;
		break;
	default:
		result = DefWindowProc(this->hWnd, uMsg, wParam, lParam);
		break;
	}
	return result;
}

void Window::Show() {
	ShowWindow(this->hWnd, SW_SHOW);
	UpdateWindow(this->hWnd);
}

void Window::Wait() {
	MSG msg;
	while (!this->closed && GetMessage(&msg, (HWND) NULL, 0, 0)) { 
		TranslateMessage(&msg); 
		DispatchMessage(&msg);
	}
}

void Window::Resize(int width, int height) {
	RECT rcClient, rcWindow, rcScreen;
	POINT ptDiff;
	GetClientRect(this->hWnd, &rcClient);
	GetWindowRect(this->hWnd, &rcWindow);
	GetWindowRect(GetDesktopWindow(), &rcScreen);
	ptDiff.x = (rcWindow.right - rcWindow.left) - rcClient.right;
	ptDiff.y = (rcWindow.bottom - rcWindow.top) - rcClient.bottom;
	MoveWindow(this->hWnd, (rcScreen.right - rcScreen.left - width) / 2, (rcScreen.bottom - rcScreen.top - height) / 2, width + ptDiff.x, height + ptDiff.y, TRUE);
}

PlayerRunner::PlayerRunner(HINSTANCE hInst) : Runner() {
	this->hInst = hInst;
}

PlayerRunner::~PlayerRunner() {

}

void PlayerRunner::start() {
	Runner::start();
	this->frame->Register();
	this->frame->Create();
	this->frame->Show();
	this->resizeMain();
	this->frame->Wait();
}

void PlayerRunner::createBoardUI() {
	this->boardUI = new Win32BoardUI(this->board);
}

void PlayerRunner::createMainUI() {
	this->frame = new MainWindow(this->hInst, this);
}

void PlayerRunner::blackWin() {
	MessageBox(this->frame->hWnd, "Game is finished with black win!", "Information", MB_OK);
}

void PlayerRunner::whiteWin() {
	MessageBox(this->frame->hWnd, "Game is finished with white win!", "Information", MB_OK);
}

void PlayerRunner::drawEnd() {
	MessageBox(this->frame->hWnd, "Game is finished with draw!", "Information", MB_OK);
}

void PlayerRunner::resizeMain() {
	int width = 2 * BoardUI::LEFT_MARGIN + this->board->getWidth() * BoardUI::CELL_WIDTH;
	int height = 4 * BoardUI::TOP_MARGIN + this->board->getHeight() * BoardUI::CELL_HEIGHT;
	this->frame->Resize(width, height);
	this->boardUI->update();
}

void PlayerRunner::drawJoinGame() {
	Window *dialog = new JoinGameWindow(this->hInst, this->frame->hWnd, this);
	dialog->Register();
	dialog->Create();
	dialog->Show();
	dialog->Resize(275, 380);
	dialog->Wait();
	SetForegroundWindow(this->frame->hWnd);
	this->boardUI->update();
}

void PlayerRunner::drawSettings() {
	Window *dialog = new SettingsWindow(this->hInst, this->frame->hWnd, this);
	dialog->Register();
	dialog->Create();
	dialog->Show();
	dialog->Resize(400, 130);
	dialog->Wait();
	SetForegroundWindow(this->frame->hWnd);
	this->boardUI->update();
}

void PlayerRunner::drawNewGame() {
	Window *dialog = new NewGameWindow(this->hInst, this->frame->hWnd, this);
	dialog->Register();
	dialog->Create();
	dialog->Show();
	dialog->Resize(275, 150);
	dialog->Wait();
	SetForegroundWindow(this->frame->hWnd);
	this->boardUI->update();
}

string *PlayerRunner::getTitle() {
	return new string("Gomoku Core");
}

void PlayerRunner::getPlayerNames(bool newGame, list<string> &tag) {
	tag.clear();
	tag.push_back("Human");
	tag.push_back("Computer");
	if (newGame) {
		tag.push_back("Remote");
	}
}

int PlayerRunner::getPlayerIndex(bool newGame, unsigned char type) {
	if (newGame) {
		if (type == gomoku_core::Game::HUMAN_PLAYER) {
			return 0;
		} else if (type == gomoku_core::Game::COMPUTER_PLAYER) {
			return 1;
		} else if (type == gomoku_core::Game::REMOTE_PLAYER) {
			return 2;
		} else {
			return 0;
		}
	} else {
		if (type == gomoku_core::Game::HUMAN_PLAYER) {
			return 0;
		} else if (type == gomoku_core::Game::COMPUTER_PLAYER) {
			return 1;
		} else {
			return 0;
		}
	}
}

unsigned char PlayerRunner::getPlayerType(bool newGame, int index) {
	unsigned char type = gomoku_core::Game::HUMAN_PLAYER;
	if (newGame) {
		if (index == 0) {
			type = gomoku_core::Game::HUMAN_PLAYER;
		} else if (index == 1) {
			type = gomoku_core::Game::COMPUTER_PLAYER;
		} else if (index == 2) {
			type = gomoku_core::Game::REMOTE_PLAYER;
		}
	} else {
		if (index == 0) {
			type = gomoku_core::Game::HUMAN_PLAYER;
		} else if (index == 1) {
			type = gomoku_core::Game::COMPUTER_PLAYER;
		}
	}
	return type;
}

bool PlayerRunner::isRemotePlayer(int index) {
	return index == 2;
}
