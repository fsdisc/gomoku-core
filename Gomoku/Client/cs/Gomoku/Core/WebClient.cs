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

using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;

namespace com.bhivef.gomoku.core
{
    public class WebClient : Client
    {
	    private bool ready;
	    private String session;
	
	    public WebClient(Config config) : base(config)
        {
            this.ready = false;
	    }
	
	    public override void login() 
        {
		    if (ready) 
            {
			    logout();
    		}
		
		    String serverUrl = config.getString(Config.SERVER_URL);
		    String extension = config.getString(Config.SERVER_EXTENSION);
		    String username = config.getString(Config.SERVER_USERNAME);
		    String password = config.getPassword(Config.SERVER_PASSWORD);
		    String link = serverUrl + "api/login" + extension + "?u=" + username + "&p=" + password;
		    String response = request(link);
		    String sign = "Success: ";
		    if (response.StartsWith(sign)) 
            {
			    session = response.Substring(sign.Length);
			    ready = true;
			    return;
		    }
		    sign = "Error: ";
		    if (response.StartsWith(sign)) 
            {
			    throw new Exception("Login fail: " + response.Substring(sign.Length));
		    }
		    throw new Exception("Login fail: Invalid api call!");
	    }
	
	    public override void logout() 
        {
		    try 
            {
			    String serverUrl = config.getString(Config.SERVER_URL);
			    String extension = config.getString(Config.SERVER_EXTENSION);
			    String link = serverUrl + "api/logout" + extension + "?s=" + session;
			    request(link);
		    } 
            catch { }
		    ready = false;
		    session = "";
	    }
	
	    public override bool online() 
        {
		    return ready;
	    }
	
	    public override void clone(String session) 
        {
		    this.session = session;
		    this.ready = true;
	    }
	
	    public override String createGame(bool playFirst, int width, int height) 
        {
		    if (!ready) 
            {
			    login();
		    }
		
		    String serverUrl = config.getString(Config.SERVER_URL);
		    String extension = config.getString(Config.SERVER_EXTENSION);
		    String link = serverUrl + "api/create" + extension + "?s=" + session + "&p=" + (playFirst ? "1" : "0") + "&w=" + width + "&h=" + height;
		    String response = request(link);
		    String sign = "Success: ";
		    if (response.StartsWith(sign)) 
            {
			    String id = response.Substring(sign.Length);
			    return id;
		    }
		    sign = "Error: ";
		    if (response.StartsWith(sign)) 
            {
			    throw new Exception("Fail to create game: " + response.Substring(sign.Length));
		    }
		    throw new Exception("Fail to create game: Invalid api call!");
	    }
	
	    public override void joinGame(String gameId) 
        {
		    if (!ready) 
            {
			    login();
		    }
		
		    String serverUrl = config.getString(Config.SERVER_URL);
		    String extension = config.getString(Config.SERVER_EXTENSION);
		    String link = serverUrl + "api/join" + extension + "?s=" + session + "&g=" + gameId;
		    String response = request(link);
		    String sign = "Error: ";
		    if (response.StartsWith(sign)) 
            {
			    throw new Exception("Fail to join game: " + response.Substring(sign.Length));
		    }
		    if (!response.Equals("Success")) 
            {
			    throw new Exception("Fail to join game: Invalid api call!");
		    }
	    }
	
	    public override void listGame(List<String> tag) 
        {
		    try 
            {
			    tag.Clear();
			    if (!ready) 
                {
				    login();
			    }

			    String serverUrl = config.getString(Config.SERVER_URL);
			    String extension = config.getString(Config.SERVER_EXTENSION);
			    String link = serverUrl + "api/list" + extension + "?s=" + session;
			    String response = request(link);
			    String sign = "Success: ";
			    if (response.StartsWith(sign)) 
                {
				    response = response.Substring(sign.Length).Trim();
				    if (response.Length > 0) 
                    {
					    String[] lines = response.Split(new char[] { '\n' });
					    for (int i = 0; i < lines.Length; i++) 
                        {
                            String line = lines[i];
                            if (line.EndsWith("\r")) 
                            {
                                line = line.Substring(0, line.Length - 1);
                            }
						    tag.Add(line);
					    }
				    }
			    }
		    } 
            catch { }
	    }
	
	    public override void makeMove(Move move) 
        {
		    String serverUrl = config.getString(Config.SERVER_URL);
		    String gameId = config.getString(Config.CURRENT_GAME);
		    String extension = config.getString(Config.SERVER_EXTENSION);
		    String link = serverUrl + "api/move" + extension + "?s=" + session + "&g=" + gameId + "&r=" + move.getRow() + "&c=" + move.getColumn() + "&p=" + move.getPiece();
		    String response = request(link);
		    String sign = "Error: ";
		    if (response.StartsWith(sign)) 
            {
			    throw new Exception("Fail to make move: " + response.Substring(sign.Length));
		    }
		    if (!response.Equals("Success")) 
            {
			    throw new Exception("Fail to make move: Invalid api call!");
		    }
	    }
	
	    public override Move lastMove() 
        {
		    try 
            {
			    if (!ready) 
                {
				    login();
			    }

			    String serverUrl = config.getString(Config.SERVER_URL);
			    String extension = config.getString(Config.SERVER_EXTENSION);
			    String gameId = config.getString(Config.CURRENT_GAME);
			    String link = serverUrl + "api/last" + extension + "?s=" + session + "&g=" + gameId;
			    String response = request(link);
			    String sign = "Success: ";
			    if (response.StartsWith(sign)) 
                {
				    response = response.Substring(sign.Length).Trim();
				    if (response.Length > 0) {
					    String[] fields = response.Split(new char[] { '|' });
					    byte piece = Board.BLANK;
                        if (!Byte.TryParse(fields[0], out piece)) 
                        {
                            piece = Board.BLANK;
                        }
					    if (piece != Board.BLACK && piece != Board.WHITE) 
                        {
						    return new Move();
					    }
					    int row = -1;
                        if (!int.TryParse(fields[1], out row)) 
                        {
                            row = -1;
                        }
					    if (row < 0 || row >= config.getInt(Config.BOARD_HEIGHT)) 
                        {
						    return new Move();
					    }
					    int column = -1;
                        if (!int.TryParse(fields[2], out column))
                        {
                            column = -1;
                        }
					    if (column < 0 || column >= config.getInt(Config.BOARD_WIDTH)) 
                        {
						    return new Move();
					    }
					
					    return new Move(row, column, piece);
				    } 
                    else 
                    {
					    return new Move();
				    }
			    } 
                else 
                {
				    return new Move();
			    }
		    } 
            catch 
            {
			    return new Move();
		    }
	    }
	
	    public override void gameState(GameState state) 
        {
		    if (!ready) 
            {
			    login();
		    }
		
		    String serverUrl = config.getString(Config.SERVER_URL);
		    String extension = config.getString(Config.SERVER_EXTENSION);
		    String gameId = config.getString(Config.CURRENT_GAME);
		    String link = serverUrl + "api/state" + extension + "?s=" + session + "&g=" + gameId;
		    String response = request(link);
		    String sign = "Success: ";
		    if (response.StartsWith(sign)) 
            {
			    response = response.Substring(sign.Length);
			    String[] fields = response.Split(new char[] { '|' });
			    state.Joined = fields[0].Equals("1");
			    state.Cancelled = fields[1].Equals("1");
			    state.Finished = fields[2].Equals("1");
                if (!byte.TryParse(fields[3], out state.Victory)) 
                {
				    state.Victory = Board.NO_WIN;
                }
			    return;
		    }
		    sign = "Error: ";
		    if (response.StartsWith(sign)) 
            {
			    throw new Exception("Fail to get game state: " + response.Substring(sign.Length));
		    }
		    throw new Exception("Fail to get game state: Invalid api call!");
	    }
	
	    public override void endGame(bool finished, byte victory) 
        {
            if (!ready)
            {
                login();
            }

		    String serverUrl = config.getString(Config.SERVER_URL);
		    String extension = config.getString(Config.SERVER_EXTENSION);
		    String gameId = config.getString(Config.CURRENT_GAME);
		    String link = serverUrl + "api/end" + extension + "?s=" + session + "&g=" + gameId + "&f=" + (finished ? "1" : "0") + "&v=" + victory;
		    String response = request(link);
		    String sign = "Error: ";
		    if (response.StartsWith(sign)) {
			    throw new Exception("Fail to end game: " + response.Substring(sign.Length));
		    }
		    if (!response.Equals("Success")) {
			    throw new Exception("Fail to end game: Invalid api call!");
		    }
	    }
	
	    protected virtual String request(String link) 
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(link);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream stream = response.GetResponseStream();

            StringBuilder sb  = new StringBuilder();
            byte[] buf = new byte[8192];
		    int count = 0;
		    do
		    {
			    count = stream.Read(buf, 0, buf.Length);

			    if (count != 0)
			    {
				    sb.Append(Encoding.ASCII.GetString(buf, 0, count));
			    }
		    }
		    while (count > 0);

            return sb.ToString();
	    }

    }
}
