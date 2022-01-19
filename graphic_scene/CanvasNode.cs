using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Drawing;
using System.IO;

namespace GraphicScene
{

    public abstract class CanvasNode : SceneNode
    {
        private int _x = 0;
        private int _y = 0;
        private float _scale = 1.0f;

        public int X { get { return _x; } set { _x = value; } }
        public int Y { get { return _y; } set { _y = value; } }
        public abstract int Width { get; set; }
        public abstract int Height { get; set; }
        public float Scale { get { return _scale; } set { _scale = value; } }

        //public Point LeftTop { get { return new Point(_x, _y); } }

        protected internal virtual void OnPaint(GraphicTool g)
        {
        }

        protected override void OnSerilize(NodeSerillizer s)
        {
            base.OnSerilize(s);

            s.Write("X", X);
            s.Write("Y", Y);
            s.Write("Scale", (int)(Scale * 1000));
            s.Write("Width", Width);
            s.Write("Height", Height);
        }

        protected override void OnDeserilize(NodeDeserillizer s)
        {
            base.OnDeserilize(s);

            this.X = s.ReadInt32("X");
            this.Y = s.ReadInt32("Y");
            this.Scale = s.ReadInt32("Scale") / 1000.0f;
            this.Width = s.ReadInt32("Width");
            this.Height = s.ReadInt32("Height");
        }
    }

    public class Node2D : CanvasNode
    {
        public override int Width { get { return 0; } set { } }
        public override int Height { get { return 0; } set { } }
    }

    public class ControlNode : CanvasNode
    {
        public override int Width { get; set; }
        public override int Height { get; set; }
    }

    public class TextureRect : ControlNode
    {
        private Image _image;

        public Image Image
        {
            get { return _image; }
            set { _image = value; }
        }

        public override int Width
        {
            get { return _image == null ? base.Width : _image.Width; }
            set { if (_image == null) base.Width = value; }
        }

        public override int Height
        {
            get { return _image == null ? base.Height : _image.Height; }
            set { if (_image == null) base.Height = value; }
        }

        protected internal override void OnPaint(GraphicTool g)
        {
            if (_image != null)
            {
                g.DrawTexture(this.Image, 0, 0);
            }
        }

        protected override void OnSerilize(NodeSerillizer s)
        {
            base.OnSerilize(s);

            var cv = new ImageConverter();
            s.Write("Image", (byte[])cv.ConvertTo(_image, typeof(byte[])));
        }

        protected override void OnDeserilize(NodeDeserillizer s)
        {
            base.OnDeserilize(s);

            using (var ms = new MemoryStream(s.ReadBytes("Image")))
            {
                _image = Bitmap.FromStream(ms);
            }
        }
    }
}
