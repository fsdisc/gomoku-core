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

package com.bhivef.gomoku.core;

import java.io.BufferedReader;
import java.io.InputStreamReader;
import java.net.URL;
import java.util.List;

public class WebClient extends Client {

	private boolean online;
	private String session;
	
	public WebClient(final Config config) {
		super(config);
		this.online = false;
	}
	
	public void login() throws Exception {
		if (online) {
			logout();
		}
		
		String serverUrl = config.getString(Config.SERVER_URL);
		String extension = config.getString(Config.SERVER_EXTENSION);
		String username = config.getString(Config.SERVER_USERNAME);
		String password = config.getPassword(Config.SERVER_PASSWORD);
		String link = serverUrl + "api/login" + extension + "?u=" + username + "&p=" + password;
		String response = request(link);
		String sign = "Success: ";
		if (response.startsWith(sign)) {
			session = response.substring(sign.length());
			online = true;
			return;
		}
		sign = "Error: ";
		if (response.startsWith(sign)) {
			throw new Exception("Login fail: " + response.substring(sign.length()));
		}
		throw new Exception("Login fail: Invalid api call!");
	}
	
	public void logout() {
		try {
			String serverUrl = config.getString(Config.SERVER_URL);
			String extension = config.getString(Config.SERVER_EXTENSION);
			String link = serverUrl + "api/logout" + extension + "?s=" + session;
			request(link);
		} catch (Exception e) {
		}
		online = false;
		session = "";
	}
	
	public boolean online() {
		return online;
	}
	
	public void clone(String session) {
		this.session = session;
		this.online = true;
	}
	
	public String createGame(final boolean playFirst, final int width, final int height) throws Exception {
		if (!online) {
			login();
		}
		
		String serverUrl = config.getString(Config.SERVER_URL);
		String extension = config.getString(Config.SERVER_EXTENSION);
		String link = serverUrl + "api/create" + extension + "?s=" + session + "&p=" + (playFirst ? "1" : "0") + "&w=" + width + "&h=" + height;
		String response = request(link);
		String sign = "Success: ";
		if (response.startsWith(sign)) {
			String id = response.substring(sign.length());
			return id;
		}
		sign = "Error: ";
		if (response.startsWith(sign)) {
			throw new Exception("Fail to create game: " + response.substring(sign.length()));
		}
		throw new Exception("Fail to create game: Invalid api call!");
	}
	
	public void joinGame(final String gameId) throws Exception {
		if (!online) {
			login();
		}
		
		String serverUrl = config.getString(Config.SERVER_URL);
		String extension = config.getString(Config.SERVER_EXTENSION);
		String link = serverUrl + "api/join" + extension + "?s=" + session + "&g=" + gameId;
		String response = request(link);
		String sign = "Error: ";
		if (response.startsWith(sign)) {
			throw new Exception("Fail to join game: " + response.substring(sign.length()));
		}
		if (!response.equals("Success")) {
			throw new Exception("Fail to join game: Invalid api call!");
		}
	}
	
	public void listGame(final List<String> tag) {
		try {
			tag.clear();
			if (!online) {
				login();
			}

			String serverUrl = config.getString(Config.SERVER_URL);
			String extension = config.getString(Config.SERVER_EXTENSION);
			String link = serverUrl + "api/list" + extension + "?s=" + session;
			String response = request(link);
			String sign = "Success: ";
			if (response.startsWith(sign)) {
				response = response.substring(sign.length()).trim();
				if (response.length() > 0) {
					String[] lines = response.split("\r\n");
					for (int i = 0; i < lines.length; i++) {
						tag.add(lines[i]);
					}
				}
			}
		} catch (Exception e) {
		}
	}
	
	public void makeMove(final Move move) throws Exception {
		String serverUrl = config.getString(Config.SERVER_URL);
		String gameId = config.getString(Config.CURRENT_GAME);
		String extension = config.getString(Config.SERVER_EXTENSION);
		String link = serverUrl + "api/move" + extension + "?s=" + session + "&g=" + gameId + "&r=" + move.getRow() + "&c=" + move.getColumn() + "&p=" + move.getPiece();
		String response = request(link);
		String sign = "Error: ";
		if (response.startsWith(sign)) {
			throw new Exception("Fail to make move: " + response.substring(sign.length()));
		}
		if (!response.equals("Success")) {
			throw new Exception("Fail to make move: Invalid api call!");
		}
	}
	
	public Move lastMove() {
		try {
			if (!online) {
				login();
			}

			String serverUrl = config.getString(Config.SERVER_URL);
			String extension = config.getString(Config.SERVER_EXTENSION);
			String gameId = config.getString(Config.CURRENT_GAME);
			String link = serverUrl + "api/last" + extension + "?s=" + session + "&g=" + gameId;
			String response = request(link);
			String sign = "Success: ";
			if (response.startsWith(sign)) {
				response = response.substring(sign.length()).trim();
				if (response.length() > 0) {
					String[] fields = response.split("\\|");
					byte piece = Board.BLANK;
					try {
						piece = Byte.parseByte(fields[0]);
					} catch (Exception e) {
						piece = Board.BLANK;
					}
					if (piece != Board.BLACK && piece != Board.WHITE) {
						return new Move();
					}
					int row = -1;
					try {
						row = Integer.parseInt(fields[1]);
					} catch (Exception e) {
						row = -1;
					}
					if (row < 0 || row >= config.getInt(Config.BOARD_HEIGHT)) {
						return new Move();
					}
					int column = -1;
					try {
						column = Integer.parseInt(fields[2]);
					} catch (Exception e) {
						column = -1;
					}
					if (column < 0 || column >= config.getInt(Config.BOARD_WIDTH)) {
						return new Move();
					}
					
					return new Move(row, column, piece);
				} else {
					return new Move();
				}
			} else {
				return new Move();
			}
		} catch (Exception e) {
			return new Move();
		}
	}
	
	public void gameState(GameState state) throws Exception {
		if (!online) {
			login();
		}
		
		String serverUrl = config.getString(Config.SERVER_URL);
		String extension = config.getString(Config.SERVER_EXTENSION);
		String gameId = config.getString(Config.CURRENT_GAME);
		String link = serverUrl + "api/state" + extension + "?s=" + session + "&g=" + gameId;
		String response = request(link);
		String sign = "Success: ";
		if (response.startsWith(sign)) {
			response = response.substring(sign.length());
			String[] fields = response.split("\\|");
			state.Joined = fields[0].equals("1");
			state.Cancelled = fields[1].equals("1");
			state.Finished = fields[2].equals("1");
			try {
				state.Victory = Byte.parseByte(fields[3]);
			} catch (Exception e) {
				state.Victory = Board.NO_WIN;
			}
			return;
		}
		sign = "Error: ";
		if (response.startsWith(sign)) {
			throw new Exception("Fail to get game state: " + response.substring(sign.length()));
		}
		throw new Exception("Fail to get game state: Invalid api call!");
		
	}
	
	public void endGame(final boolean finished, final byte victory) throws Exception {
		if (!online) {
			login();
		}
		
		String serverUrl = config.getString(Config.SERVER_URL);
		String extension = config.getString(Config.SERVER_EXTENSION);
		String gameId = config.getString(Config.CURRENT_GAME);
		String link = serverUrl + "api/end" + extension + "?s=" + session + "&g=" + gameId + "&f=" + (finished ? "1" : "0") + "&v=" + victory;
		String response = request(link);
		String sign = "Error: ";
		if (response.startsWith(sign)) {
			throw new Exception("Fail to end game: " + response.substring(sign.length()));
		}
		if (!response.equals("Success")) {
			throw new Exception("Fail to end game: Invalid api call!");
		}
	}
	
	protected String request(String link) throws Exception {
		URL url = new URL(link);
		BufferedReader in = new BufferedReader(new InputStreamReader(url.openStream()));
		String response = "";
		String line = null;
        while ((line = in.readLine()) != null) {
        	if (response.length() > 0) {
        		response += "\r\n";
        	}
        	response += line;
        }
        in.close();

        return response;
	}
	
}
