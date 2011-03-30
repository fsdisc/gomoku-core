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
using com.bhivef.gomoku.core.players;

namespace com.bhivef.gomoku.core
{
    public class Runner
    {
	    protected Config config;
	    protected Board board;
	    protected BoardUI boardUI;
	    protected Client client;
	    protected Game game;
	    protected List<Move> history;
	    protected bool finished;

	    public virtual void start() 
        {
		    setup();
		    try 
            {
			    newGame();
		    } catch { }
	    }

	    protected virtual void setup() 
        {
		    createConfig();
		    createBoard();
		    createBoardUI();
		    createClient();
		    createGame();
		    createMainUI();
	    }
	
	    protected virtual void createConfig() 
        {
		    config = new Config();
		    config.setValue(Config.SEARCH_DEPTH, 2);
		    config.setValue(Config.BOARD_WIDTH, Board.GO_WIDTH);
		    config.setValue(Config.BOARD_HEIGHT, Board.GO_HEIGHT);
		    config.setValue(Config.FIRST_TYPE, Game.HUMAN_PLAYER);
		    config.setValue(Config.SECOND_TYPE, Game.HUMAN_PLAYER);
		    config.setValue(Config.SERVER_URL, "http://bhivef.com/gomoku/");
		    config.setValue(Config.SERVER_EXTENSION, ".php");
	    }
	
	    protected virtual void createBoard() 
        {
		    board = new Board(config.getByte(Config.BOARD_WIDTH), config.getByte(Config.BOARD_HEIGHT));
	    }

	    protected virtual void createBoardUI() 
        {
	    }
	
	    protected virtual void createClient() 
        {
		    client = new WebClient(config);
	    }
	
	    protected virtual void createGame() 
        {
		    finished = false;
		    history = new List<Move>();
		    board.resize(config.getByte(Config.BOARD_WIDTH), config.getByte(Config.BOARD_HEIGHT));
		    if (game != null) {
			    game.dispose();
		    }
		    constructGame();
            game.addMoveListener(new MoveAdapter(this));
	    }

	    protected virtual void constructGame() 
        {
		    game = new Game(config, board, boardUI, client);
	    }
	
	    protected virtual void createMainUI() 
        {
	    }
	
	    protected virtual void blackWin() 
        {
	    }
	
	    protected virtual void whiteWin() 
        {
	    }
	
	    protected virtual void drawEnd() 
        {
	    }
	
	    protected virtual void newGame() 
        {
		    game.create();
	    }
	
	    protected virtual void joinGame(String gameId) 
        {
		    game.join(gameId);
	    }

        private class MoveAdapter : MoveListener
        {
            private Runner runner;

            public MoveAdapter(Runner runner)
            {
                this.runner = runner;
            }

            public void moveMade(Move move)
            {
                runner.history.Add(move.clone());
            }

            public void lastMove(byte victory)
            {
                if (runner.finished) return;
                runner.finished = true;
                switch (victory)
                {
                    case Board.DRAW:
                        runner.drawEnd();
                        break;
                    case Board.BLACK_WIN:
                        runner.blackWin();
                        break;
                    case Board.WHITE_WIN:
                        runner.whiteWin();
                        break;
                    default:
                        return;
                }
            }
        }

    }
}
