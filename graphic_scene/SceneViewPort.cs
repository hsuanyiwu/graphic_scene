using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace GraphicScene
{
    public partial class SceneViewPort : UserControl
    {
        private GraphicTool _graph = new GraphicTool();
        private SceneNode _scene;
        private bool _editMode = true;
        private bool _isDirty;

        //
        private CanvasNode _selectedNode;
        private Rectangle _selectedRect;
        private float _drawX;
        private float _drawY;
        // move view
        private Point _ptMouse = Point.Empty;
        private Point _ptDrag = Point.Empty;
        private bool _mouseDrag = false;

        public SceneViewPort()
        {
            InitializeComponent();
        }

        public SceneNode Scene
        {
            get { return _scene; }
            set
            {
                _selectedNode = null;
                _scene = value;
                _graph.OffsetX = _graph.OffsetY = 200;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            /* 
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBilinear;
             * */
            _graph.Graphics = e.Graphics;
            _graph.Graphics.FillRectangle(Brushes.Black, this.ClientRectangle);

            // background
            if (_editMode)
                BeginDrawEditorBG();

            //
            if (_scene != null)
            {
                _drawX = _graph.OffsetX;
                _drawY = _graph.OffsetY;
                DrawSceneNode(_scene);
            }

            if (_editMode)
                EndDrawEditorBG();

            _isDirty = false;
        }

        private void BeginDrawEditorBG()
        {
            // grid
            DrawGrid(GraphicTool.PenGridSmall, 50);
            DrawGrid(GraphicTool.PenGridBig, 250);
            // x-y
            _graph.Graphics.DrawLine(Pens.Red, _graph.OffsetX, 0, _graph.OffsetX, this.Height);
            _graph.Graphics.DrawLine(Pens.Green, 0, _graph.OffsetY, this.Width, _graph.OffsetY);
        }

        private void EndDrawEditorBG()
        {
            DrawSelected();
            DrawRuler();
        }

        private void DrawGrid(Pen pen, int dist)
        {
            int x0 = _graph.OffsetX % dist;
            int y0 = _graph.OffsetY % dist;

            for (int ix = x0; ix < this.Width; ix += dist)
                _graph.Graphics.DrawLine(pen, ix, 0, ix, this.Height);

            for (int iy = y0; iy < this.Height; iy += dist)
                _graph.Graphics.DrawLine(pen, new Point(0, iy), new Point(this.Width, iy));
        }

        private void DrawRuler()
        {
            const int RULLER_SIZE = 18;

            _graph.Graphics.FillRectangle(GraphicTool.BrushRullerBG, new Rectangle(0, 0, this.Width, RULLER_SIZE));
            _graph.Graphics.FillRectangle(GraphicTool.BrushRullerBG, new Rectangle(0, 0, RULLER_SIZE, this.Height));
            _graph.Graphics.FillRectangle(Brushes.Gray, new Rectangle(0, 0, RULLER_SIZE, RULLER_SIZE));

            int dist = 50;
            int x0 = _graph.OffsetX % dist;
            int y0 = _graph.OffsetY % dist;

            // x
            for (int ix = x0; ix < this.Width; ix += dist)
            {
                for (int jx = ix + 10; jx < ix + dist; jx += 10)
                {
                    if (jx > RULLER_SIZE)
                    {
                        _graph.Graphics.DrawLine(GraphicTool.PenRuller, jx, RULLER_SIZE - 4, jx, RULLER_SIZE - 1);
                    }
                }

                if (ix > RULLER_SIZE)
                {
                    _graph.Graphics.DrawLine(GraphicTool.PenRuller, ix, 2, ix, RULLER_SIZE - 1);
                    string text = ((int)_graph.ToWorldX(ix)).ToString();
                    _graph.Graphics.DrawString(text, GraphicTool.FontRuller, Brushes.White, ix + 1, 0);
                }
            }

            // y
            var fmt = new StringFormat(StringFormatFlags.DirectionVertical);
            for (int iy = y0; iy < this.Height; iy += dist)
            {
                for (int jy = iy + 10; jy < iy + dist; jy += 10)
                {
                    if (jy > RULLER_SIZE)
                    {
                        _graph.Graphics.DrawLine(GraphicTool.PenRuller, RULLER_SIZE - 4, jy, RULLER_SIZE - 1, jy);
                    }
                }

                if (iy > RULLER_SIZE)
                {
                    _graph.Graphics.DrawLine(GraphicTool.PenRuller, 2, iy, RULLER_SIZE - 1, iy);
                    string text = ((int)_graph.ToWorldY(iy)).ToString();
                    _graph.Graphics.DrawString(text, GraphicTool.FontRuller, Brushes.White, 0, iy + 1, fmt);
                }
            }
        }

        private void DrawSelected()
        {
            if (_selectedNode == null)
                return;

            if (_selectedRect.Width != 0 && _selectedRect.Height != 0)
                _graph.Graphics.DrawRectangle(GraphicTool.PenSelected, _selectedRect);

            var img = Properties.Resources.cross;
            _graph.Graphics.DrawImage(img, _selectedRect.X - img.Width / 2, _selectedRect.Y - img.Height / 2);
        }

        private void DrawSceneNode(SceneNode node)
        {
            int oldX = _graph.OffsetX;
            int oldY = _graph.OffsetY;
            float scale = _graph.Scale;

            var temp = node as CanvasNode;
            if (temp != null)
            {
                _drawX += temp.X * _graph.Scale;
                _drawY += temp.Y * _graph.Scale;
                _graph.OffsetX = (int)_drawX;
                _graph.OffsetY = (int)_drawY;
                _graph.Scale *= temp.Scale;

                temp.OnPaint(_graph);

                if (temp.IsSelected)
                {
                    _selectedNode = temp;
                    _selectedRect = _graph.ToViewRect(0, 0, temp.Width, temp.Height);
                }
            }

            foreach (var child in node.Childs)
            {
                DrawSceneNode(child);
            }

            _graph.OffsetX = oldX;
            _graph.OffsetY = oldY;
            _graph.Scale = scale;
        }


        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
            }
            else if (e.Button == MouseButtons.Right)
            {
                _mouseDrag = true;
                _ptDrag = e.Location;
            }

            base.OnMouseDown(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (_mouseDrag)
            {
                var ptMouse = e.Location;
                int dx = ptMouse.X - _ptDrag.X;
                int dy = ptMouse.Y - _ptDrag.Y;

                _ptDrag = ptMouse;
                _graph.OffsetX += dx;
                _graph.OffsetY += dy;

                _isDirty = true;
            }
            base.OnMouseMove(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            _mouseDrag = false;
            base.OnMouseUp(e);
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            var ptMouse = e.Location;

            // view -> world
            float xWorld = _graph.ToWorldX(ptMouse.X);
            float yWorld = _graph.ToWorldY(ptMouse.Y);

            // change scale
            //_graph.Scale = _graph.Scale * (e.Delta > 0 ? 2.0f : 0.5f);
            _graph.Scale = _graph.Scale * (e.Delta > 0 ? 1.25f : 0.8f);

            //world -> view
            float xView = xWorld * _graph.Scale;
            float yView = yWorld * _graph.Scale;
            _graph.OffsetX = ptMouse.X - (int)xView;
            _graph.OffsetY = ptMouse.Y - (int)yView;

            _isDirty = true;
            base.OnMouseWheel(e);
        }

        public void SetPaintDirty()
        {
            _isDirty = true;
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            this.Invalidate();
        }
    }

}
