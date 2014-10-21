using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace EightQueens {

    public partial class Form1 : Form {


        int queenCount = 0;
        bool hintsOn = false;
        
        private ArrayList queenlist = new ArrayList();
        private ArrayList hintlist = new ArrayList();
        private ArrayList cells = new ArrayList();
        const int WIDTH = 50;
        const int HEIGHT = 50;

        
        public class Cell {
            public Point P;
            public bool hasQueen;
            public int color;
            public Cell(Point C) {
                this.P = C;
                this.hasQueen = false;
            }

            public bool isValidQueen(ArrayList queenlist, int x, int y) {
                foreach (Cell c in queenlist){
                    if (!((x - c.P.X > 0 && x - c.P.X < 50) && (y - c.P.Y > 0 && y - c.P.Y < 50)) && c.hasQueen == true) {
                        if (c.P.X == x || c.P.Y == y || (Math.Abs(c.P.X-x) - Math.Abs(c.P.Y-y)) == 0) {
                            return false;
                        }
                    } 
                }
                return true;
            }

        }

        public Form1() {

            int xCount = 0;
            int yCount = 0;

            for (int i = 0; i < 32; i++) {
                int X = 100 + 50 * xCount;
                int Y = 100 + 50 * yCount;
                Point P = new Point(X, Y);
                Cell c = new Cell(P);
                if (xCount % 2 == 0) {
                    c.color = 0;
                } else {
                    c.color = 1;
                }
                cells.Add(c);
                xCount++;
                if (xCount == 8) {
                    xCount = 0;
                    yCount += 2;
                }
            }
            yCount = 1;
            for (int i = 0; i < 32; i++) {
                int X = 100 + 50 * xCount;
                int Y = 100 + 50 * yCount;
                Point P = new Point(X, Y);
                Cell c = new Cell(P);
                if (xCount % 2 == 0) {
                    c.color = 1;
                } else {
                    c.color = 0;
                }
                cells.Add(c);
                xCount++;
                if (xCount == 8) {
                    xCount = 0;
                    yCount += 2;
                }
            }
            InitializeComponent();
        }
        private void Form1_MouseClick(object sender, MouseEventArgs e) {

            if (e.Button == MouseButtons.Left) {
                //do something for left button
                foreach (Cell c in cells) {
                    if ((e.X - c.P.X > 0 && e.X - c.P.X < 50) && (e.Y - c.P.Y > 0 && e.Y - c.P.Y < 50)) { // first check if clicked within square
                       if (c.isValidQueen(queenlist, c.P.X, c.P.Y) == true) { // then check if a queen can be put there
                            if (queenCount < 8) { // then check if there are less than 8 queens on the board
                                queenCount++;
                                c.hasQueen = true;
                                queenlist.Add(c);
                                if (queenCount == 8) {
                                    DialogResult click = MessageBox.Show("You Win <3", "YEAAHHHHH!!!!!!", MessageBoxButtons.OK);
                                }
                                
                                Invalidate();
                            }
                       } else { // if we can't put a queen there
                           System.Media.SystemSounds.Beep.Play();
                       }
                    }
                }

            }
            if (e.Button == MouseButtons.Right) {
                foreach (Cell c in cells) {
                    if ((e.X - c.P.X > 0 && e.X - c.P.X < 50) && (e.Y - c.P.Y > 0 && e.Y - c.P.Y < 50)) {
                        if (c.hasQueen == true) {
                            queenCount--;
                            c.hasQueen = false;
                            hintlist.Clear();
                            queenlist.Remove(c);
                            Invalidate();
                        }
                    }
                }
            }
        }

        private void Form1_Paint(object sender, PaintEventArgs e) {

            Graphics g = e.Graphics;
            Font font = new Font("Arial", 30, FontStyle.Bold);
            string drawstring = String.Format("You have {0} queens on the board", queenCount);
            g.DrawString(drawstring, Font, Brushes.Black, new Point(200, 23));
            StringFormat sf = new StringFormat();
            sf.LineAlignment = StringAlignment.Center;
               sf.Alignment = StringAlignment.Center;

            foreach (Cell c in cells) {
                if (c.color == 0) {
                    g.FillRectangle(Brushes.White, new Rectangle(c.P.X, c.P.Y, WIDTH, HEIGHT));
                    g.DrawRectangle(Pens.Black, new Rectangle(c.P.X, c.P.Y, 50, 50));
                    if (c.hasQueen == true) {
                        g.DrawString("Q", font, Brushes.Black, new Rectangle(c.P.X, c.P.Y, 50, 50), sf);
                    }
                } else if (c.color == 1) {
                    g.FillRectangle(Brushes.Black, new Rectangle(c.P.X, c.P.Y, WIDTH, HEIGHT));
                    g.DrawRectangle(Pens.Black, new Rectangle(c.P.X, c.P.Y, 50, 50));
                    if (c.hasQueen == true) {
                        g.DrawString("Q", font, Brushes.White, new Rectangle(c.P.X, c.P.Y, 50, 50), sf);
                    }
                }  
            }
            if (hintsOn == true) {
                foreach (Cell c in cells) {
                    if (c.isValidQueen(queenlist, c.P.X, c.P.Y) == false || c.hasQueen == true) {
                        hintlist.Add(c);
                    }
                }
                foreach (Cell h in hintlist) {
                    g.FillRectangle(Brushes.Red, new Rectangle(h.P.X, h.P.Y, WIDTH, HEIGHT));
                    g.DrawRectangle(Pens.Black, new Rectangle(h.P.X, h.P.Y, 50, 50));
                    if (h.hasQueen == true) {
                        g.DrawString("Q", font, Brushes.Black, h.P.X + 2, h.P.Y + 2);
                    }
                }
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e) {
            if (checkBox1.Checked == true) {
                

                hintsOn = true;
            } else {
                hintsOn = false;
                hintlist.Clear();
            }
            Invalidate();
        }

        private void button1_Click(object sender, EventArgs e) {
            queenlist.Clear();
            hintlist.Clear();
            queenCount = 0;
            foreach (Cell c in cells) {
                c.hasQueen = false;
            }
            Invalidate();
        }
    }
}
