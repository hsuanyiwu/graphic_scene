using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace GraphicScene
{
    public class GraphicTool
    {
        public GraphicTool()
        {
            this.OffsetX = 0;
            this.OffsetY = 0;
            this.Scale = 1.0f;
        }

        public static Pen PenGridSmall = new Pen(Color.FromArgb(70, SystemColors.ControlDark));
        public static Pen PenGridBig = new Pen(Color.FromArgb(100, SystemColors.ControlDark), 2);
        public static Brush BrushBG = new SolidBrush(Color.FromArgb(100, Color.DarkGray));
        public static Pen PenSelected = new Pen(Color.HotPink, 3);
        // ruller
        public static Pen PenRuller = Pens.WhiteSmoke;
        public static Brush BrushRullerBG = new SolidBrush(Color.FromArgb(88, 88, 88));
        public static Font FontRuller = new Font("Arial", 8);

        public Graphics Graphics { get; set; }
        // offset to graphic left-top
        internal int OffsetX { get; set; }
        internal int OffsetY { get; set; }
        internal float Scale { get; set; }

        public Point ToViewPos(int x, int y)
        {
            return new Point(
                (int)(this.OffsetX + x * this.Scale),
                (int)(this.OffsetY + y * this.Scale)
            );
        }

        public Rectangle ToViewRect(int x, int y, int w, int h)
        {
            return new Rectangle(
                (int)(this.OffsetX + x * this.Scale),
                (int)(this.OffsetY + y * this.Scale),
                (int)(w * this.Scale),
                (int)(h * this.Scale)
            );
        }

        public float ToWorldX(int x)
        {
            return (x - this.OffsetX) / this.Scale;
        }

        public float ToWorldY(int y)
        {
            return (y - this.OffsetY) / this.Scale;
        }

        public void DrawTexture(Image image, int x, int y)
        {
            this.Graphics.DrawImage(image, ToViewRect(x, y, image.Width, image.Height));
        }

        public void DrawImage(Image image, int x, int y)
        {
            this.Graphics.DrawImage(image, ToViewPos(x, y));
        }

        /*public void DrawSelected(int x, int y, int w, int h)
        {
            float x_ = this.OffsetX + x * this.Scale;
            float y_ = this.OffsetY + y * this.Scale;

            if (w != 0 && h != 0)
                this.Graphics.DrawRectangle(GraphicTool.PenSelected, x_, y_, w * this.Scale, h * this.Scale);

            var cursor = Properties.Resources.node_cursor;
            this.Graphics.DrawImage(cursor, x_ - cursor.Width / 2, y_ - cursor.Height / 2);
        }*/
    }
}
