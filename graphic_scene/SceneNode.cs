using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.ComponentModel;
using System.Reflection;

namespace GraphicScene
{
    public abstract class SceneNode
    {
        private List<SceneNode> _childs = new List<SceneNode>();
        private SceneNode _parent;
        private string _name = "Node";
        private Guid _guid;

        public SceneNode()
        {
            _guid = Guid.NewGuid();
        }

        [Browsable(false)]
        public Guid Guid
        {
            get { return _guid; }
        }

        [Browsable(false)]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        [Browsable(false)]
        public List<SceneNode> Childs
        {
            get { return _childs; }
        }

        [Browsable(false)]
        public SceneNode Parent
        {
            get { return _parent; }
        }

        public void AddNode(SceneNode node)
        {
            node._parent = this;
            _childs.Add(node);
        }

        [Browsable(false)]
        public bool IsSelected
        {
            get;
            set;
        }

        protected virtual void OnSerilize(NodeSerillizer s)
        {
        }

        /*private static Dictionary<string, Type> _typeDict = new Dictionary<string, Type>();

        static public void LoadAssembly(string asmFile)
        {
            var asm = Assembly.LoadFrom(asmFile);
            if (asm == null)
                return;

            var baseType = typeof(SceneNode);
            foreach (var t in asm.GetTypes())
            {
                if (t.IsAbstract || !t.IsSubclassOf(baseType))
                    continue;
                _typeDict.Add(t.FullName, t);
            }
        }*/

        
        public static byte[] Serilize(SceneNode node)
        {
            var bs = new NodeSerillizer();           
            SerillizeNode(node, bs);
            return bs.data.ToArray();
        }

        private static void SerillizeNode(SceneNode node, NodeSerillizer s)
        {
            // node basic
            Type type = node.GetType();
            s.Write("AssemblyName", type.Assembly.GetName().Name);
            s.Write("TypeFullName", type.FullName);
            s.Write("NodeName", node.Name);
            s.Write("GUID", node.Guid.ToString());

            // properties
            var prop = new NodeSerillizer();
            node.OnSerilize(prop);
            s.Write("Property", prop);

            // child node
            var lst = new List<NodeSerillizer>();
            foreach (var child in node._childs)
            {
                var tmp = new NodeSerillizer();
                SerillizeNode(child, tmp);
                lst.Add(tmp);
            }
            s.Write("Child", lst);
        }

        public static SceneNode Deserillize(byte[] bytes)
        {
            var bs = new NodeDeserillizer(bytes);          
            return DeserillizeNode(bs);
        }

        private static SceneNode DeserillizeNode(NodeDeserillizer s)
        {
            // node
            string asmName = s.ReadString("AssemblyName");
            string typeName = s.ReadString("TypeFullName");

            var node = (SceneNode)Activator.CreateInstance(AppDomain.CurrentDomain, asmName, typeName).Unwrap();
            node._name = s.ReadString("NodeName");
            node._guid = Guid.Parse(s.ReadString("GUID"));

            // property
            NodeDeserillizer prop;
            s.Read("Property", out prop);
            node.OnDeserilize(prop);

            // child node
            foreach(var tmp in s.EnumList("Child"))
            {
                var child = DeserillizeNode(tmp);
                node.AddNode(child);
            }

            return node;
        }

        protected virtual void OnDeserilize(NodeDeserillizer s)
        {

        }
    }


}
