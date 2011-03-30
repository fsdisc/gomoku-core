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

package com.bhivef.gomoku.core.player.awt;

import java.awt.BorderLayout;
import java.awt.Button;
import java.awt.Dialog;
import java.awt.Dimension;
import java.awt.FlowLayout;
import java.awt.Frame;
import java.awt.Label;
import java.awt.Panel;
import java.awt.Window;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;

public class MsgBox extends Dialog implements ActionListener {

	private Button ok;
	
	public MsgBox(Frame parent, String title, String message) {
		super(parent, title, true);
		setLayout(new BorderLayout());
		add("Center", new Label(message));
        
		Panel p = new Panel();
        p.setLayout(new FlowLayout());
        p.add(ok = new Button("OK"));
        ok.addActionListener(this); 
        add("South",p);

        Dimension d = getToolkit().getScreenSize();
        setLocation(d.width / 3, d.height / 3);

        pack();
        setVisible(true);
	}

    public void actionPerformed(ActionEvent ae){
        if(ae.getSource() == ok) {
            setVisible(false);
        }
    }
	
	public static void placeCentered(Window window) {
        Dimension d1 = window.getToolkit().getScreenSize();
		Dimension d2 = window.getSize();
		window.setLocation((int)(d1.getWidth() - d2.getWidth()) / 2, (int)(d1.getHeight() - d2.getHeight()) / 2); 
	}
	
	public static void info(Frame parent, String title, String message) {
		MsgBox box = new MsgBox(parent, title, message);
		box.dispose();
	}

}
