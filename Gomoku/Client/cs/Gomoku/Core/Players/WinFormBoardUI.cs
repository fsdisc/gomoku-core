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
using System.Drawing;
using System.Windows.Forms;

namespace com.bhivef.gomoku.core.players
{
    public class WinFormBoardUI : BoardUI
    {
	    private Drawer drawer;
	    private Graphics graphics;
        private bool ready;
	
	    public WinFormBoardUI(Board board) : base(board)
        {
		    this.drawer = new Drawer(this);
            this.ready = false;
	    }
	
	    public Panel getUI() 
        {
		    return drawer;
	    }
	
	    public override void update() 
        {
            if (!ready) return;
            MethodInvoker action = delegate { drawer.Refresh(); };
            drawer.BeginInvoke(action);
	    }

        public void setReady()
        {
            ready = true;
        }

	    protected override void drawLine(int x1, int y1, int x2, int y2) 
        {
            graphics.DrawLine(new Pen(Color.Black), x1, y1, x2, y2);
	    }
	
	    protected override void drawText(int x, int y, String text) 
        {
            graphics.DrawString(text, new Font("Arial", 10), new SolidBrush(Color.Black), x, y);
	    }
	
	    protected override void drawOval(int x, int y, int width, int height) 
        {
            graphics.DrawEllipse(new Pen(Color.Black), x, y, width, height);
	    }

	    protected override void fillOval(int x, int y, int width, int height) 
        {
            graphics.FillEllipse(new SolidBrush(Color.Black), x, y, width, height);
	    }

        private class Drawer : Panel
        {
            private WinFormBoardUI boardUI;

            public Drawer(WinFormBoardUI boardUI)
                : base()
            {
                this.boardUI = boardUI;
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                base.OnPaint(e);
                boardUI.graphics = e.Graphics;
                boardUI.draw();
            }

            protected override void OnMouseClick(MouseEventArgs e)
            {
                base.OnMouseClick(e);
                int x = e.X - LEFT_MARGIN;
                int y = e.Y - TOP_MARGIN;
                if (x < 0 || y < 0) return;
                x = x / BoardUI.CELL_WIDTH;
                y = y / BoardUI.CELL_HEIGHT;
                if (x >= boardUI.board.getWidth() || y >= boardUI.board.getHeight()) return;
                boardUI.fireMoveMade(new Move(y, x, boardUI.board.getCurrentPiece()));
            }
        }

    }
}
