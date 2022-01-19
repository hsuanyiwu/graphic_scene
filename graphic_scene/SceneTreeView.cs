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
    public partial class SceneTreeView : UserControl
    {
        private SceneNode _scene;
        private SceneNode _seleced;
        public SceneTreeView()
        {
            InitializeComponent();
        }

        public void SetScene(SceneNode scene)
        {
            if (_seleced != null)
                _seleced.IsSelected = false;
            _seleced = null;
            treeView.Nodes.Clear();
            AddSceneNode(scene, treeView.Nodes);
            treeView.ExpandAll();
        }

        private void AddSceneNode(SceneNode scene, TreeNodeCollection root)
        {
            var tree = new TreeNode(scene.Name);
            tree.ImageIndex = 0;
            tree.Tag = scene;

            foreach (var child in scene.Childs)
            {
                AddSceneNode(child, tree.Nodes);
            }

            root.Add(tree);
        }

        private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (_seleced != null)
                _seleced.IsSelected = false;

            _seleced = (SceneNode)e.Node.Tag;
            _seleced.IsSelected = true;

            if (SelectedNodeChanged != null)
                SelectedNodeChanged();
        }
        public SceneNode SelectedNode
        {
            get { return _seleced; }
        }

        public Action SelectedNodeChanged;
    }
}
