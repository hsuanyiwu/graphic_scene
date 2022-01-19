using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace GraphicScene
{
    public class SceneFileMan
    {
        public static readonly string Extension = ".gs";

        private string _rootPath;
        private Dictionary<string, SceneNode> _scenes = new Dictionary<string, SceneNode>();

        public SceneFileMan()
        {

        }

        public void SetRoot(string path)
        {
            _rootPath = path;
            _scenes.Clear();

            var dirInfo = new DirectoryInfo(path);
            foreach (var fileInfo in dirInfo.GetFiles("*.gs"))
            {
                _scenes.Add(fileInfo.Name, null);
            }
        }

        public IEnumerable<string> SceneNames()
        {
            return _scenes.Keys;
        }

        public bool CreateNewScene(string name)
        {
            if (_scenes.ContainsKey(name))
                return false;

            try
            {
                var tmp = new GraphicScene.Node2D();
                var bytes = GraphicScene.SceneNode.Serilize(tmp);
                string filePath = Path.Combine(_rootPath, name);
                File.WriteAllBytes(filePath, bytes);
                _scenes.Add(name, tmp);
               // OnSceneAdded(name);
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        public SceneNode GetScene(string name)
        {
            return _scenes[name];
        }

        public void SaveScene(string name)
        {
            string filePath = Path.Combine(_rootPath, name) + Extension;
            var bytes = SceneNode.Serilize(_scenes[name]);
            File.WriteAllBytes(filePath, bytes);
        }

        private void OnSceneAdded()
        {
            if (SceneAdded != null)
                SceneAdded();
        }

        private void OnSceneRemoved()
        {
            if (SceneRemoved != null)
                SceneRemoved();
        }

        private void OnSceneSelected()
        {
            if (SceneSelected != null)
                SceneSelected();
        }

        public Action SceneAdded;
        public Action SceneRemoved;
        public Action SceneSelected;
    }

    class PackedScene
    {

        public SceneNode CreateInstance()
        {
            return null;
        }
    }
}
