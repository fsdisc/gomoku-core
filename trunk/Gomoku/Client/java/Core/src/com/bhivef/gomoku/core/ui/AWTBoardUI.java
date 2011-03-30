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

package com.bhivef.gomoku.core.ui;

import java.awt.Graphics;
import java.awt.Panel;
import java.awt.event.MouseEvent;
import java.awt.event.MouseListener;

import com.bhivef.gomoku.core.Board;
import com.bhivef.gomoku.core.Move;

public final class AWTBoardUI extends com.bhivef.gomoku.core.BoardUI {

	private final Drawer drawer;
	private Graphics graphics;
	
	public AWTBoardUI(final Board board) {
		super(board);
		this.drawer = new Drawer();
		setup();
	}
	
	private void setup() {
		drawer.addMouseListener(new MouseListener() {
		    public void mousePressed(MouseEvent e) {
		    }
		    public void mouseReleased(MouseEvent e) {
		    }
		    public void mouseEntered(MouseEvent e) {
		    }
		    public void mouseExited(MouseEvent e) {
		    }
		    public void mouseClicked(MouseEvent e) {
		    	int x = e.getX() - LEFT_MARGIN;
		    	int y = e.getY() - TOP_MARGIN;
		    	if (x < 0 || y < 0) return;
		    	x = x / CELL_WIDTH;
		    	y = y / CELL_HEIGHT;
		    	if (x >= board.getWidth() || y >= board.getHeight()) return;
		    	fireMoveMade(new Move(y, x, board.getCurrentPiece()));
		    }
		});
	}
	
	public Panel getUI() {
		return drawer;
	}
	
	public void update() {
		drawer.repaint();
	}
	
	protected void drawLine(final int x1, final int y1, final int x2, final int y2) {
		graphics.drawLine(x1, y1, x2, y2);
	}
	
	protected void drawText(final int x, final int y, final String text) {
		graphics.drawString(text, x, y + graphics.getFontMetrics().getHeight());
	}
	
	protected void drawOval(final int x, final int y, final int width, final int height) {
		graphics.drawOval(x, y, width, height);
	}

	protected void fillOval(final int x, final int y, final int width, final int height) {
		graphics.fillOval(x, y, width, height);
	}
	
	private class Drawer extends Panel {
		public void paint(Graphics g) {
			graphics = g;
			draw();
		}
	}
	
}
