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

package com.bhivef.gomoku.core.player.swing.applet;

import java.applet.Applet;
import java.awt.BorderLayout;
import java.awt.Button;
import java.awt.FlowLayout;
import java.awt.GridBagConstraints;
import java.awt.GridBagLayout;
import java.awt.SystemColor;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.util.ArrayList;
import java.util.List;

import javax.swing.JButton;
import javax.swing.JComboBox;
import javax.swing.JFrame;
import javax.swing.JLabel;
import javax.swing.JList;
import javax.swing.JOptionPane;
import javax.swing.JPanel;
import javax.swing.JScrollPane;
import javax.swing.JTextField;

import com.bhivef.gomoku.core.Board;
import com.bhivef.gomoku.core.Config;
import com.bhivef.gomoku.core.Game;
import com.bhivef.gomoku.core.player.awt.MsgBox;

public class Loader extends Applet {

	protected Config config;
	protected String session;
	protected Runner runner;
	
	public void init() {
		Button button;
		
		createConfig();
		readParameters();
		createRunner();
		
		setLayout(new FlowLayout());
		
		button = new Button("New Game");
		add(button);
        button.addActionListener(new ActionListener() {
        	public void actionPerformed(ActionEvent ae){
        		drawNewGame();
        	}
        });
		
		button = new Button("Join Game");
		add(button);
        button.addActionListener(new ActionListener() {
        	public void actionPerformed(ActionEvent ae){
        		drawJoinGame();
        	}
        });
	}
	
	protected void createConfig() {
		config = new Config();
		config.setValue(Config.BOARD_WIDTH, Board.GO_WIDTH);
		config.setValue(Config.BOARD_HEIGHT, Board.GO_HEIGHT);
		config.setValue(Config.FIRST_TYPE, Game.HUMAN_PLAYER);
		config.setValue(Config.SECOND_TYPE, Game.HUMAN_PLAYER);
		config.setValue(Config.SEARCH_DEPTH, 2);
		config.setValue(Config.SERVER_URL, "http://bhivef.com/gomoku/");
		config.setValue(Config.SERVER_EXTENSION, ".php");
	}
	
	protected void readParameters() {
		session = getParameter("session");
	}
	
	protected void createRunner() {
		runner = new Runner(config, session);
	}
	
	protected void drawNewGame() {
		JPanel panel;
		JLabel label;
		JTextField text;
		JComboBox choice;
		JButton button;
		GridBagLayout gbl;
		GridBagConstraints gbc;
		
		String[] types = getPlayerNames(true);
		
		final JFrame frame = new JFrame();
		frame.setTitle("New Game");
		frame.setSize(300, 200);
		frame.setResizable(false);
		frame.setBackground(SystemColor.control);

		panel = new JPanel();
		frame.getContentPane().add(panel, BorderLayout.CENTER);
		gbl = new GridBagLayout();
		gbc = new GridBagConstraints();
        panel.setLayout(gbl);
		
        label = new JLabel("Board width  ");
        gbc.gridwidth = 1;
        gbc.gridx = 0;
        gbc.gridy = 0;
        gbc.fill = gbc.NONE;
        gbl.setConstraints(label, gbc);
		panel.add(label);

		text = new JTextField();
		final JTextField textWidth = text;
		text.setText("" + config.getByte(config.BOARD_WIDTH));
        gbc.gridwidth = 3;
        gbc.gridx = 1;
        gbc.gridy = 0;
        gbc.fill = gbc.HORIZONTAL;
        gbl.setConstraints(text, gbc);
		panel.add(text);

        label = new JLabel("Board height  ");
        gbc.gridwidth = 1;
        gbc.gridx = 0;
        gbc.gridy = 1;
        gbc.fill = gbc.NONE;
        gbl.setConstraints(label, gbc);
		panel.add(label);

		text = new JTextField();
		final JTextField textHeight = text;
		text.setText("" + config.getByte(config.BOARD_HEIGHT));
        gbc.gridwidth = 3;
        gbc.gridx = 1;
        gbc.gridy = 1;
        gbc.fill = gbc.HORIZONTAL;
        gbl.setConstraints(text, gbc);
		panel.add(text);

        label = new JLabel("First player  ");
        gbc.gridwidth = 1;
        gbc.gridx = 0;
        gbc.gridy = 2;
        gbc.fill = gbc.NONE;
        gbl.setConstraints(label, gbc);
		panel.add(label);
		
		choice = new JComboBox();
		final JComboBox choiceFirst = choice;
        gbc.gridwidth = 3;
        gbc.gridx = 1;
        gbc.gridy = 2;
        gbc.fill = gbc.HORIZONTAL;
        gbl.setConstraints(choice, gbc);
		panel.add(choice);
		for (int i = 0; i < types.length; i++) {
			choice.addItem(types[i]);
		}
		choice.setSelectedIndex(getPlayerIndex(true, config.getByte(config.FIRST_TYPE)));
		
        label = new JLabel("Second player  ");
        gbc.gridwidth = 1;
        gbc.gridx = 0;
        gbc.gridy = 3;
        gbc.fill = gbc.NONE;
        gbl.setConstraints(label, gbc);
		panel.add(label);

		choice = new JComboBox();
		final JComboBox choiceSecond = choice;
        gbc.gridwidth = 3;
        gbc.gridx = 1;
        gbc.gridy = 3;
        gbc.fill = gbc.HORIZONTAL;
        gbl.setConstraints(choice, gbc);
		panel.add(choice);
		for (int i = 0; i < types.length; i++) {
			choice.addItem(types[i]);
		}
		choice.setSelectedIndex(getPlayerIndex(true, config.getByte(config.SECOND_TYPE)));
		
		panel = new JPanel();
		frame.getContentPane().add(panel, BorderLayout.SOUTH);
        panel.setLayout(new FlowLayout());
        button = new JButton("OK");
        panel.add(button);
        button.addActionListener(new ActionListener() {
        	public void actionPerformed(ActionEvent ae){
        		int width = 0;
        		try {
        			width = Integer.parseInt(textWidth.getText());
        			if (width < 10 || width > 255) {
        				warningBox(frame, "Warning", "Width must be in [10-255] range!");
            			return;
        			}
        		} catch (Exception e) {
        			warningBox(frame, "Warning", "Invalid board width!");
        			return;
        		}
        		int height = 0;
        		try {
        			height = Integer.parseInt(textHeight.getText());
        			if (height < 10 || height > 255) {
        				warningBox(frame, "Warning", "Height must be in [10-255] range!");
            			return;
        			}
        		} catch (Exception e) {
        			warningBox(frame, "Warning", "Invalid board height!");
        			return;
        		}
        		if (isRemotePlayer(choiceFirst.getSelectedIndex()) && isRemotePlayer(choiceSecond.getSelectedIndex())) {
        			warningBox(frame, "Warning", "Both first player and second player can not be remote!");
        			return;
        		}
        		byte firstType = getPlayerType(true, choiceFirst.getSelectedIndex());
        		byte secondType = getPlayerType(true, choiceSecond.getSelectedIndex());
        		config.setValue(Config.BOARD_WIDTH, width);
        		config.setValue(Config.BOARD_HEIGHT, height);
        		config.setValue(Config.FIRST_TYPE, firstType);
        		config.setValue(Config.SECOND_TYPE, secondType);
				frame.setVisible(false);
				frame.dispose();
				runner.start("");
        	}
        });
        button = new JButton("Cancel");
        panel.add(button);
        button.addActionListener(new ActionListener() {
        	public void actionPerformed(ActionEvent ae){
				frame.setVisible(false);
				frame.dispose();
        	}
        });

		MsgBox.placeCentered(frame);
		frame.setVisible(true);
	}

	protected void drawJoinGame() {
		JPanel panel;
		JLabel label;
		JComboBox choice;
		JList list;
		JButton button;
		GridBagLayout gbl;
		GridBagConstraints gbc;
		
		final JFrame frame = new JFrame();
		frame.setSize(300, 250);
		frame.setResizable(false);
		frame.setBackground(SystemColor.control);

		panel = new JPanel();
		frame.getContentPane().add(panel, BorderLayout.CENTER);
		gbl = new GridBagLayout();
		gbc = new GridBagConstraints();
        panel.setLayout(gbl);

		final List<String> srcList = new ArrayList<String>();
		runner.list(srcList);
		String[] items = new String[srcList.size()];
		for (int i = 0; i < srcList.size(); i++) {
			String line = srcList.get(i);
			String[] fields = line.split("\\|");
			if (fields[1].length() > 0) {
				items[i] = fields[0] + " (" + fields[3] + "x" + fields[4] + ", First: " + fields[1] + ")";
			} else {
				items[i] = fields[0] + " (" + fields[3] + "x" + fields[4] + ", Second: " + fields[2] + ")";
			}
		}
        list = new JList(items);
        final JList listGame = list;
        gbc.gridwidth = 4;
        gbc.gridx = 0;
        gbc.gridy = 0;
        gbc.fill = gbc.BOTH;
        JScrollPane scroll = new JScrollPane(list);
        gbl.setConstraints(scroll, gbc);
		panel.add(scroll);
        
		label = new JLabel("Player  ");
        gbc.gridwidth = 1;
        gbc.gridx = 0;
        gbc.gridy = 1;
        gbc.fill = gbc.NONE;
        gbl.setConstraints(label, gbc);
		panel.add(label);

		choice = new JComboBox();
		final JComboBox choicePlayer = choice;
        gbc.gridwidth = 3;
        gbc.gridx = 1;
        gbc.gridy = 1;
        gbc.fill = gbc.HORIZONTAL;
        gbl.setConstraints(choice, gbc);
		panel.add(choice);
		String[] types = getPlayerNames(false);
		for (int i = 0; i < types.length; i++) {
			choice.addItem(types[i]);
		}
		choice.setSelectedIndex(0);
		
		panel = new JPanel();
		frame.getContentPane().add(panel, BorderLayout.SOUTH);
        panel.setLayout(new FlowLayout());
        button = new JButton("OK");
        panel.add(button);
        button.addActionListener(new ActionListener() {
        	public void actionPerformed(ActionEvent ae){
        		int idx = listGame.getSelectedIndex();
        		if (idx < 0) {
        			warningBox(frame, "Warning", "Game is required to select!");
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
        			warningBox(frame, "Error", "Invalid board width!");
        			return;
        		}
        		int height = Board.GO_HEIGHT;
        		try {
        			height = Integer.parseInt(fields[4]);
        		} catch (Exception e) {
        			warningBox(frame, "Error", "Invalid board height!");
        			return;
        		}
        		config.setValue(Config.BOARD_WIDTH, width);
        		config.setValue(Config.BOARD_HEIGHT, height);
				frame.setVisible(false);
				frame.dispose();
				runner.start(fields[0]);
        	}
        });
        button = new JButton("Cancel");
        panel.add(button);
        button.addActionListener(new ActionListener() {
        	public void actionPerformed(ActionEvent ae){
				frame.setVisible(false);
				frame.dispose();
        	}
        });
		
		MsgBox.placeCentered(frame);
		frame.setVisible(true);
	}
	
	protected void warningBox(final JFrame frame, final String title, final String message) {
		JOptionPane.showMessageDialog(frame, message, title, JOptionPane.WARNING_MESSAGE);
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
