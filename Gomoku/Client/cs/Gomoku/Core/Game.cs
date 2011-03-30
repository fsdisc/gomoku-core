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

namespace com.bhivef.gomoku.core
{
    public class Game
    {
	    public const byte HUMAN_PLAYER = 0;
	    public const byte COMPUTER_PLAYER = 1;
	    public const byte REMOTE_PLAYER = 2;
	
	    protected Config config;
	    protected Board board;
	    protected BoardUI boardUI;
	    protected Client client;
	    protected Player firstPlayer;
	    protected Player secondPlayer;
	    protected bool disposed;
	    protected bool finished;
	    private List<MoveListener> listeners;

	    public Game(Config config, Board board, BoardUI boardUI, Client client) 
        {
		    this.config = config;
		    this.board = board;
		    this.boardUI = boardUI;
		    this.client = client;
		    this.board.clear();
		    this.board.setCurrentPiece(Board.BLACK);
		    this.boardUI.clearListeners();
		    this.listeners = new List<MoveListener>();
		    this.disposed = false;
		    this.finished = false;
	    }
	
	    public void create() 
        {
		    if (hasRemotePlayer()) 
            {
			    bool playFirst = (config.getByte(Config.FIRST_TYPE) != REMOTE_PLAYER);
			    String gameId = client.createGame(playFirst, config.getInt(Config.BOARD_WIDTH), config.getInt(Config.BOARD_HEIGHT));
			    config.setValue(Config.CURRENT_GAME, gameId);
		    }
		    createPlayers();
		    setup();
		    checkReady();
	    }
	
	    public void join(String gameId) 
        {
		    if (hasRemotePlayer()) 
            {
			    client.joinGame(gameId);
			    config.setValue(Config.CURRENT_GAME, gameId);
		    }
		    createPlayers();
		    setup();
		    checkReady();
	    }
	
	    public void list(List<String> tag) 
        {
		    client.listGame(tag);
	    }
	
	    public void dispose() 
        {
		    disposed = true;
		    listeners.Clear();
		    if (firstPlayer != null) 
            {
			    firstPlayer.dispose();
		    }
		    if (secondPlayer != null) 
            {
			    secondPlayer.dispose();
		    }
		    try 
            {
			    client.endGame(false, Board.NO_WIN);
		    } 
            catch { }
	    }
	
	    private void setup() 
        {
            firstPlayer.addMoveListener(new MoveAdapter(this, true));
            secondPlayer.addMoveListener(new MoveAdapter(this, false));
	    }
	
	    protected bool hasRemotePlayer() {
		    bool remote = false;
		    byte firstType = config.getByte(Config.FIRST_TYPE);
		    byte secondType = config.getByte(Config.SECOND_TYPE);
		    if (firstType == REMOTE_PLAYER && secondType == REMOTE_PLAYER) 
            {
			    remote = false;
		    }
            else if (firstType == REMOTE_PLAYER || secondType == REMOTE_PLAYER) 
            {
			    remote = true;
		    }
		    return remote;
	    }
	
	    protected void createPlayers() 
        {
		    bool remote = false;
		    byte firstType = config.getByte(Config.FIRST_TYPE);
		    byte secondType = config.getByte(Config.SECOND_TYPE);
		    if (firstType == REMOTE_PLAYER && secondType == REMOTE_PLAYER) 
            {
			    remote = false;
		    } 
            else if (firstType == REMOTE_PLAYER || secondType == REMOTE_PLAYER) 
            {
			    remote = true;
		    }
		    if (remote) 
            {
			    if (firstType == REMOTE_PLAYER) 
                {
				    firstPlayer = new RemotePlayer(config, client, board, Board.BLACK);
			    } 
                else if (firstType == COMPUTER_PLAYER) 
                {
				    firstPlayer = new RemotePlayer(config, client, board, new ComputerPlayer(config, board, Board.BLACK));
			    } 
                else if (firstType == HUMAN_PLAYER) 
                {
				    firstPlayer = new RemotePlayer(config, client, board, new HumanPlayer(config, board, Board.BLACK, boardUI));
			    } 
                else 
                {
				    firstPlayer = createUnknownPlayer(true, firstType, Board.BLACK);
			    }
			    if (secondType == REMOTE_PLAYER) 
                {
				    secondPlayer = new RemotePlayer(config, client, board, Board.WHITE);
			    } 
                else if (secondType == COMPUTER_PLAYER) 
                {
				    secondPlayer = new RemotePlayer(config, client, board, new ComputerPlayer(config, board, Board.WHITE));
			    } 
                else if (secondType == HUMAN_PLAYER) 
                {
				    secondPlayer = new RemotePlayer(config, client, board, new HumanPlayer(config, board, Board.WHITE, boardUI));
			    } 
                else 
                {
				    secondPlayer = createUnknownPlayer(true, secondType, Board.WHITE);
			    }
		    } 
            else 
            {
			    if (firstType == COMPUTER_PLAYER) 
                {
				    firstPlayer = new ComputerPlayer(config, board, Board.BLACK);
			    } 
                else if (firstType == HUMAN_PLAYER) 
                {
				    firstPlayer = new HumanPlayer(config, board, Board.BLACK, boardUI);
			    } 
                else 
                {
				    firstPlayer = createUnknownPlayer(false, firstType, Board.BLACK);
			    }
			    if (secondType == COMPUTER_PLAYER) 
                {
				    secondPlayer = new ComputerPlayer(config, board, Board.WHITE);
			    } 
                else if (secondType == HUMAN_PLAYER) 
                {
				    secondPlayer = new HumanPlayer(config, board, Board.WHITE, boardUI);
			    } 
                else 
                {
				    secondPlayer = createUnknownPlayer(false, secondType, Board.WHITE);
			    }
		    }
	    }
	
	    protected virtual Player createUnknownPlayer(bool hasRemote, byte type, byte piece) 
        {
		    if (hasRemote) 
            {
			    return new RemotePlayer(config, client, board, new HumanPlayer(config, board, piece, boardUI));
		    } 
            else 
            {
			    return new HumanPlayer(config, board, piece, boardUI);
		    }
	    }
	
	    public void addMoveListener(MoveListener src) 
        {
		    listeners.Add(src);
	    }
	
	    public void clearListeners() 
        {
		    listeners.Clear();
	    }
	
	    protected void fireMoveMade(Move move) 
        {
		    for (int i = 0; i < listeners.Count; i++) {
			    listeners[i].moveMade(move);
		    }
	    }
	
	    protected void fireLastMove(byte victory) 
        {
		    for (int i = 0; i < listeners.Count; i++) 
            {
			    listeners[i].lastMove(victory);
		    }
	    }
	
	    protected void firstThink() 
        {
            FirstThread runner = new FirstThread(this);
            System.Threading.Thread thread = new System.Threading.Thread(new System.Threading.ThreadStart(runner.run));
            thread.Start();
	    }

	    protected void secondThink() 
        {
            SecondThread runner = new SecondThread(this);
            System.Threading.Thread thread = new System.Threading.Thread(new System.Threading.ThreadStart(runner.run));
            thread.Start();
        }

	    protected void checkReady() {
            ReadyThread runner = new ReadyThread(this);
            System.Threading.Thread thread = new System.Threading.Thread(new System.Threading.ThreadStart(runner.run));
            thread.Start();
        }

        private class FirstThread
        {
            private Game game;

            public FirstThread(Game game)
            {
                this.game = game;
            }

            public void run()
            {
                game.firstPlayer.think(new Move());
            }
        }

        private class SecondThread
        {
            private Game game;

            public SecondThread(Game game)
            {
                this.game = game;
            }

            public void run()
            {
                game.secondPlayer.think(new Move());
            }
        }

        private class ReadyThread
        {
            private Game game;

            public ReadyThread(Game game)
            {
                this.game = game;
            }

            public void run()
            {
                bool stop = false;
                while (!stop && !game.disposed)
                {
                    stop = game.firstPlayer.checkReady() && game.secondPlayer.checkReady();
                    if (stop) break;
                    try
                    {
                        System.Threading.Thread.Sleep(1000);
                    }
                    catch { }
                }
                game.firstPlayer.setReady();
                game.secondPlayer.setReady();
                game.firstThink();
            }
        }

        private class MoveAdapter : MoveListener
        {
            private Game game;
            private bool first;

            public MoveAdapter(Game game, bool first)
            {
                this.game = game;
                this.first = first;
            }

            public void moveMade(Move move)
            {
                game.boardUI.update();
                game.fireMoveMade(move);
                if (first)
                {
                    game.secondThink();
                }
                else
                {
                    game.firstThink();
                }
            }

            public void lastMove(byte victory)
            {
                game.finished = true;
                if (first)
                {
                    game.secondPlayer.setFinished();
                }
                else
                {
                    game.firstPlayer.setFinished();
                }
                game.fireLastMove(victory);
                try
                {
                    game.client.endGame(true, victory);
                }
                catch { }
            }
        }

    }
}
