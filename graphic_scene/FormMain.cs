using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Diagnostics.Eventing.Reader;
using System.Threading;
using GraphicScene;
using System.IO;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace GraphicScene
{
    public partial class FormMain : Form
    {
        private SceneFileMan _fileMan = new SceneFileMan();
        //private SceneItem _root = new SceneItem();
        private SceneNode _scene;

        public FormMain()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            sceneTreeView.SelectedNodeChanged += () =>
            {
                propertyGrid1.SelectedObject = sceneTreeView.SelectedNode;
            };

            //this.WindowState = FormWindowState.Maximized;
            this.Activated += AfterFormLoad;
        }

        private void AfterFormLoad(object sender, EventArgs e)
        {
            /*this.Activated -= AfterFormLoad;

            var dlg = new CommonOpenFileDialog();
            dlg.InitialDirectory = Properties.Settings.Default.ProjectPath;
            dlg.IsFolderPicker = true;
            if (dlg.ShowDialog() != CommonFileDialogResult.Ok)
            {
                this.Close();
                return;*
            }

            _fileMan.SetRoot(dlg.FileName);
            sceneFileView1.SetFileMan(_fileMan);*/

            _scene = GetScene();
            if (_scene == null)
                return;

            sceneViewPort.Scene = _scene;

            sceneTreeView.SetScene(_scene);

        }

        private SceneNode GetScene()
        {
            if (!File.Exists("strip.gs"))
            {
                /*var stripPNP = new TextureRect();
                var image = Image.FromFile(@".\path55.png");// @".\Asset\strip_pnp.png");
//                var image = Bitmap.FromFile();
                //stripPNP.Image = image;

                return stripPNP;*/

                var node = new TextureRect();
                node.Name = "Strip";
                node.Image = Image.FromFile(@"Asset\strip_pnp.png");

                var p1 = new TextureRect();
                p1.Name = "ArmL";
                p1.Image = Image.FromFile(@"Asset\sucker.png");

                var p2 = new TextureRect();
                p2.Image = Image.FromFile(@"Asset\sucker.png");
                p2.Name = "ArmR";
                p2.X = 200;

                node.AddNode(p1);
                node.AddNode(p2);

                var bytes = SceneNode.Serilize(node);
                File.WriteAllBytes("strip.gs", bytes);
                return node;
            }
            else
            {
                var bytes = File.ReadAllBytes("strip.gs");
                return SceneNode.Deserillize(bytes);
            }
        }
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {

        }
    }
}