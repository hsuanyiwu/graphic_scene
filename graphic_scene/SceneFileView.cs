using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GraphicScene
{
    public partial class SceneFileView : UserControl
    {
        private SceneFileMan _fileMan;
        public SceneFileView()
        {
            InitializeComponent();
        }

        private void SceneFileView_Load(object sender, EventArgs e)
        {

        }

        public void SetFileMan(SceneFileMan fileMan)
        {
            _fileMan = fileMan;
            treeView.Nodes.Clear();
            var root = treeView.Nodes.Add("root://");
            
            foreach(var name in _fileMan.SceneNames())
            {
                var node = root.Nodes.Add(name);
                node.ImageIndex = 1;
                node.SelectedImageIndex = 1;
            }
            treeView.ExpandAll();
        }
    }
}
