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

package com.bhivef.gomoku.core.player.awt.application;

import java.awt.Button;
import java.awt.Choice;
import java.awt.Dialog;
import java.awt.FlowLayout;
import java.awt.Frame;
import java.awt.GridBagConstraints;
import java.awt.GridBagLayout;
import java.awt.Label;
import java.awt.Panel;
import java.awt.SystemColor;
import java.awt.TextField;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.awt.event.WindowAdapter;
import java.awt.event.WindowEvent;
import java.util.ArrayList;
import java.util.List;

import com.bhivef.gomoku.core.Board;
import com.bhivef.gomoku.core.BoardUI;
import com.bhivef.gomoku.core.Config;
import com.bhivef.gomoku.core.Game;
import com.bhivef.gomoku.core.player.awt.MsgBox;
import com.bhivef.gomoku.core.ui.AWTBoardUI;

public class Runner extends com.bhivef.gomoku.core.Runner {

	protected Frame frame;
	
	public static void main(String[] args) {
		Runner runner = new Runner();
		runner.start();
	}
	
	protected void createMainUI() {
		frame = new Frame();
		frame.setTitle(getTitle());
		frame.setSize(10, 10);
		frame.setResizable(false);
		frame.setBackground(SystemColor.control);
		frame.add("Center", ((AWTBoardUI)boardUI).getUI());
		frame.addWindowListener ( new WindowAdapter () {
			public void windowClosing ( WindowEvent evt ) {
				client.logout();
				System.exit(0);
			}
		});
		
		Panel p = new Panel();
        p.setLayout(new FlowLayout());
        Button button = new Button("New Game");
        p.add(button);
        button.addActionListener(new ActionListener() {
        	public void actionPerformed(ActionEvent ae){
        		drawNewGame();
        	}
        });
        button = new Button("Join Game");
        p.add(button);
        button.addActionListener(new ActionListener() {
        	public void actionPerformed(ActionEvent ae){
        		drawJoinGame();
        	}
        });
        button = new Button("Settings");
        p.add(button);
        button.addActionListener(new ActionListener() {
        	public void actionPerformed(ActionEvent ae){
        		drawSettings();
        	}
        });
        
        frame.add("South", p);
        
        resizeMain();
		frame.setVisible(true);
	}
	
	protected void blackWin() {
		MsgBox.info(frame, "Information", "Game is finished with black win!");
	}
	
	protected void whiteWin() {
		MsgBox.info(frame, "Information", "Game is finished with white win!");
	}
	
	protected void drawEnd() {
		MsgBox.info(frame, "Information", "Game is finished with draw!");
	}

	protected void resizeMain() {
		int width = 2 * BoardUI.LEFT_MARGIN + board.getWidth() * BoardUI.CELL_WIDTH;
		int height = 5 * BoardUI.TOP_MARGIN + board.getHeight() * BoardUI.CELL_HEIGHT;
		frame.setSize(width, height);
        MsgBox.placeCentered(frame);
        boardUI.update();
	}
	
	protected void drawJoinGame() {
		Button button;
		Panel panel;
		java.awt.List list;
		Label label;
		Choice choice;
		GridBagLayout gbl;
		GridBagConstraints gbc;
		
		final Dialog dialog = new Dialog(frame);
		dialog.setTitle("Join Game");
		dialog.setSize(300, 300);
		dialog.addWindowListener ( new WindowAdapter () {
			public void windowClosing ( WindowEvent evt ) {
				dialog.setVisible(false);
				dialog.dispose();
			}
		});
		
		panel = new Panel();
		dialog.add("Center", panel);
		gbl = new GridBagLayout();
		gbc = new GridBagConstraints();
        panel.setLayout(gbl);
		
		list = new java.awt.List(10, false);
		final java.awt.List listGame = list;
        gbc.gridwidth = 4;
        gbc.gridx = 0;
        gbc.gridy = 0;
        gbc.fill = gbc.BOTH;
        gbl.setConstraints(list, gbc);
		panel.add(list);
		final List<String> srcList = new ArrayList<String>();
		game.list(srcList);
		for (int i = 0; i < srcList.size(); i++) {
			String line = srcList.get(i);
			String[] fields = line.split("\\|");
			if (fields[1].length() > 0) {
				list.add(fields[0] + " (" + fields[3] + "x" + fields[4] + ", First: " + fields[1] + ")");
			} else {
				list.add(fields[0] + " (" + fields[3] + "x" + fields[4] + ", Second: " + fields[2] + ")");
			}
		}
		
		label = new Label("Player");
        gbc.gridwidth = 1;
        gbc.gridx = 0;
        gbc.gridy = 1;
        gbc.fill = gbc.NONE;
        gbl.setConstraints(label, gbc);
		panel.add(label);
		
		choice = new Choice();
		final Choice choicePlayer = choice;
        gbc.gridwidth = 3;
        gbc.gridx = 1;
        gbc.gridy = 1;
        gbc.fill = gbc.HORIZONTAL;
        gbl.setConstraints(choice, gbc);
		panel.add(choice);
		String[] types = getPlayerNames(false);
		for (int i = 0; i < types.length; i++) {
			choice.add(types[i]);
		}
		choice.select(0);
		
		panel = new Panel();
		dialog.add("South", panel);
        panel.setLayout(new FlowLayout());
        button = new Button("OK");
        panel.add(button);
        button.addActionListener(new ActionListener() {
        	public void actionPerformed(ActionEvent ae){
        		int idx = listGame.getSelectedIndex();
        		if (idx < 0) {
        			MsgBox.info(frame, "Warning", "Game is required to select!");
        			return;
        		}
        		String line = srcList.get(idx);
        		String[] fields = line.split("\\|");
        		boolean playFirst = fields[1].length() > 0;
        		byte player = getPlayerType(false, choicePlayer.getSelectedIndex());
        		if (playFirst) {
        			config.setValue(Config.FIRST_TYPE, Game.REMOTE_PLAYER);
        			config.setValue(Config.SECOND_TYPE, player);
        		} else {
        			config.setValue(Config.FIRST_TYPE, player);
        			config.setValue(Config.SECOND_TYPE, Game.REMOTE_PLAYER);
        		}
        		int width = Board.GO_WIDTH;
        		try {
        			width = Integer.parseInt(fields[3]);
        		} catch (Exception e) {
        			MsgBox.info(frame, "Error", "Invalid board width!");
        			return;
        		}
        		int height = Board.GO_HEIGHT;
        		try {
        			height = Integer.parseInt(fields[4]);
        		} catch (Exception e) {
        			MsgBox.info(frame, "Error", "Invalid board height!");
        			return;
        		}
        		config.setValue(Config.BOARD_WIDTH, width);
        		config.setValue(Config.BOARD_HEIGHT, height);
        		createGame();
        		resizeMain();
        		try {
            		joinGame(fields[0]);
        		} catch (Exception e) {
        			MsgBox.info(frame, "Error", e.toString());
        			return;
        		}
        		
				dialog.setVisible(false);
				dialog.dispose();
        	}
        });
        button = new Button("Cancel");
        panel.add(button);
        button.addActionListener(new ActionListener() {
        	public void actionPerformed(ActionEvent ae){
				dialog.setVisible(false);
				dialog.dispose();
        	}
        });
		
		MsgBox.placeCentered(dialog);
		dialog.setVisible(true);
	}
	
	protected void drawSettings() {
		Button button;
		Panel panel;
		Label label;
		TextField text;
		GridBagLayout gbl;
		GridBagConstraints gbc;
		
		final Dialog dialog = new Dialog(frame);
		dialog.setTitle("Settings");
		dialog.setSize(300, 160);
		dialog.addWindowListener ( new WindowAdapter () {
			public void windowClosing ( WindowEvent evt ) {
				dialog.setVisible(false);
				dialog.dispose();
			}
		});
		
		panel = new Panel();
		dialog.add("Center", panel);
		gbl = new GridBagLayout();
		gbc = new GridBagConstraints();
        panel.setLayout(gbl);

        label = new Label("Server URL");
        gbc.gridwidth = 1;
        gbc.gridx = 0;
        gbc.gridy = 0;
        gbc.fill = gbc.NONE;
        gbl.setConstraints(label, gbc);
		panel.add(label);
		
		text = new TextField();
		final TextField textServerURL = text;
		text.setText("" + config.getString(config.SERVER_URL));
        gbc.gridwidth = 3;
        gbc.gridx = 1;
        gbc.gridy = 0;
        gbc.fill = gbc.HORIZONTAL;
        gbl.setConstraints(text, gbc);
		panel.add(text);
		
		label = new Label("Username");
        gbc.gridwidth = 1;
        gbc.gridx = 0;
        gbc.gridy = 1;
        gbc.fill = gbc.NONE;
        gbl.setConstraints(label, gbc);
		panel.add(label);
		
		text = new TextField();
		final TextField textUsername = text;
		text.setText("" + config.getString(config.SERVER_USERNAME));
        gbc.gridwidth = 3;
        gbc.gridx = 1;
        gbc.gridy = 1;
        gbc.fill = gbc.HORIZONTAL;
        gbl.setConstraints(text, gbc);
		panel.add(text);
		
		label = new Label("Password");
        gbc.gridwidth = 1;
        gbc.gridx = 0;
        gbc.gridy = 2;
        gbc.fill = gbc.NONE;
        gbl.setConstraints(label, gbc);
		panel.add(label);
		
		text = new TextField();
		final TextField textPassword = text;
		text.setEchoChar('*');
		text.setText("" + config.getPassword(config.SERVER_PASSWORD));
        gbc.gridwidth = 3;
        gbc.gridx = 1;
        gbc.gridy = 2;
        gbc.fill = gbc.HORIZONTAL;
        gbl.setConstraints(text, gbc);
		panel.add(text);
		
		panel = new Panel();
		dialog.add("South", panel);
        panel.setLayout(new FlowLayout());
        button = new Button("OK");
        panel.add(button);
        button.addActionListener(new ActionListener() {
        	public void actionPerformed(ActionEvent ae){
        		config.setValue(Config.SERVER_URL, textServerURL.getText());
        		config.setValue(Config.SERVER_USERNAME, textUsername.getText());
        		config.setPassword(Config.SERVER_PASSWORD, textPassword.getText());
        		try {
            		client.login();
        		} catch (Exception e) {
        			MsgBox.info(frame, "Error", e.toString());
        			return;
        		}
				dialog.setVisible(false);
				dialog.dispose();
        	}
        });
        button = new Button("Cancel");
        panel.add(button);
        button.addActionListener(new ActionListener() {
        	public void actionPerformed(ActionEvent ae){
				dialog.setVisible(false);
				dialog.dispose();
        	}
        });
		
		MsgBox.placeCentered(dialog);
		dialog.setVisible(true);
	}
	
	protected void drawNewGame() {
		Button button;
		Panel panel;
		Label label;
		TextField text;
		Choice choice;
		GridBagLayout gbl;
		GridBagConstraints gbc;
		
		String[] types = getPlayerNames(true);
		
		final Dialog dialog = new Dialog(frame);
		dialog.setTitle("New Game");
		dialog.setSize(300, 200);
		dialog.addWindowListener ( new WindowAdapter () {
			public void windowClosing ( WindowEvent evt ) {
				dialog.setVisible(false);
				dialog.dispose();
			}
		});
		
		panel = new Panel();
		dialog.add("Center", panel);
		gbl = new GridBagLayout();
		gbc = new GridBagConstraints();
        panel.setLayout(gbl);

        label = new Label("Board width");
        gbc.gridwidth = 1;
        gbc.gridx = 0;
        gbc.gridy = 0;
        gbc.fill = gbc.NONE;
        gbl.setConstraints(label, gbc);
		panel.add(label);
		
		text = new TextField();
		final TextField textWidth = text;
		text.setText("" + config.getByte(config.BOARD_WIDTH));
        gbc.gridwidth = 3;
        gbc.gridx = 1;
        gbc.gridy = 0;
        gbc.fill = gbc.HORIZONTAL;
        gbl.setConstraints(text, gbc);
		panel.add(text);
		
		label = new Label("Board height");
        gbc.gridwidth = 1;
        gbc.gridx = 0;
        gbc.gridy = 1;
        gbc.fill = gbc.NONE;
        gbl.setConstraints(label, gbc);
		panel.add(label);
		
		text = new TextField();
		final TextField textHeight = text;
		text.setText("" + config.getByte(config.BOARD_HEIGHT));
        gbc.gridwidth = 3;
        gbc.gridx = 1;
        gbc.gridy = 1;
        gbc.fill = gbc.HORIZONTAL;
        gbl.setConstraints(text, gbc);
		panel.add(text);
		
		label = new Label("First player");
        gbc.gridwidth = 1;
        gbc.gridx = 0;
        gbc.gridy = 2;
        gbc.fill = gbc.NONE;
        gbl.setConstraints(label, gbc);
		panel.add(label);
		
		choice = new Choice();
		final Choice choiceFirst = choice;
        gbc.gridwidth = 3;
        gbc.gridx = 1;
        gbc.gridy = 2;
        gbc.fill = gbc.HORIZONTAL;
        gbl.setConstraints(choice, gbc);
		panel.add(choice);
		for (int i = 0; i < types.length; i++) {
			choice.add(types[i]);
		}
		choice.select(getPlayerIndex(true, config.getByte(config.FIRST_TYPE)));
		
		label = new Label("Second player");
        gbc.gridwidth = 1;
        gbc.gridx = 0;
        gbc.gridy = 3;
        gbc.fill = gbc.NONE;
        gbl.setConstraints(label, gbc);
		panel.add(label);
		
		choice = new Choice();
		final Choice choiceSecond = choice;
        gbc.gridwidth = 3;
        gbc.gridx = 1;
        gbc.gridy = 3;
        gbc.fill = gbc.HORIZONTAL;
        gbl.setConstraints(choice, gbc);
		panel.add(choice);
		for (int i = 0; i < types.length; i++) {
			choice.add(types[i]);
		}
		choice.select(getPlayerIndex(true, config.getByte(config.SECOND_TYPE)));
		
		panel = new Panel();
		dialog.add("South", panel);
        panel.setLayout(new FlowLayout());
        button = new Button("OK");
        panel.add(button);
        button.addActionListener(new ActionListener() {
        	public void actionPerformed(ActionEvent ae){
        		int width = 0;
        		try {
        			width = Integer.parseInt(textWidth.getText());
        			if (width < 10 || width > 255) {
            			MsgBox.info(frame, "Warning", "Width must be in [10-255] range!");
            			return;
        			}
        		} catch (Exception e) {
        			MsgBox.info(frame, "Warning", "Invalid board width!");
        			return;
        		}
        		int height = 0;
        		try {
        			height = Integer.parseInt(textHeight.getText());
        			if (height < 10 || height > 255) {
        				MsgBox.info(frame, "Warning", "Height must be in [10-255] range!");
            			return;
        			}
        		} catch (Exception e) {
        			MsgBox.info(frame, "Warning", "Invalid board height!");
        			return;
        		}
        		if (isRemotePlayer(choiceFirst.getSelectedIndex()) && isRemotePlayer(choiceSecond.getSelectedIndex())) {
        			MsgBox.info(frame, "Warning", "Both first player and second player can not be remote!");
        			return;
        		}
        		byte firstType = getPlayerType(true, choiceFirst.getSelectedIndex());
        		byte secondType = getPlayerType(true, choiceSecond.getSelectedIndex());
        		config.setValue(Config.BOARD_WIDTH, width);
        		config.setValue(Config.BOARD_HEIGHT, height);
        		config.setValue(Config.FIRST_TYPE, firstType);
        		config.setValue(Config.SECOND_TYPE, secondType);
        		createGame();
        		resizeMain();
        		try {
            		newGame();
        		} catch (Exception e) {
        			MsgBox.info(frame, "Error", e.toString());
        		}
				dialog.setVisible(false);
				dialog.dispose();
        	}
        });
        button = new Button("Cancel");
        panel.add(button);
        button.addActionListener(new ActionListener() {
        	public void actionPerformed(ActionEvent ae){
				dialog.setVisible(false);
				dialog.dispose();
        	}
        });
		
		MsgBox.placeCentered(dialog);
		dialog.setVisible(true);
	}

	protected String getTitle() {
		return "Gomoku Core";
	}
	
	protected String[] getPlayerNames(final boolean newGame) {
		if (newGame) {
			return new String[] { "Human", "Computer", "Remote" };
		} else {
			return new String[] { "Human", "Computer" };
		}
	}

	protected int getPlayerIndex(final boolean newGame, final byte type) {
		if (newGame) {
			switch (type) {
			case Game.HUMAN_PLAYER:
				return 0;
			case Game.COMPUTER_PLAYER:
				return 1;
			case Game.REMOTE_PLAYER:
				return 2;
			default:
				return 0;
			}
		} else {
			switch (type) {
			case Game.HUMAN_PLAYER:
				return 0;
			case Game.COMPUTER_PLAYER:
				return 1;
			default:
				return 0;
			}
		}
	}
	
	protected byte getPlayerType(final boolean newGame, final int index) {
		if (newGame) {
			byte type = Game.HUMAN_PLAYER;
			switch (index) {
			case 0:
				type = Game.HUMAN_PLAYER;
				break;
			case 1:
				type = Game.COMPUTER_PLAYER;
				break;
			case 2:
				type = Game.REMOTE_PLAYER;
				break;
			}
			return type;
		} else {
			byte type = Game.HUMAN_PLAYER;
			switch (index) {
			case 0:
				type = Game.HUMAN_PLAYER;
				break;
			case 1:
				type = Game.COMPUTER_PLAYER;
				break;
			}
			return type;
		}
	}
	
	protected boolean isRemotePlayer(final int index) {
		return index == 2;
	}
	
}
