using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectGraph
{
    public class Graph : IDisposable
    {
        private HashSet<GraphObject> graphObjects = new HashSet<GraphObject>();
        private HashSet<Node> allNodes = new HashSet<Node>();
        private Stack<Node> activeNodes = new Stack<Node>();
        private Node rootNode;
        private bool disposedValue = false;


        public Graph()
        {
            rootNode = new Node(this, null);
            allNodes.Add(rootNode);
            activeNodes.Push(rootNode);
        }


        public Node CurrentNode
        { get { return activeNodes.Peek(); } }


        public Node RootNode
        { get { return rootNode; } }


        public Node Branch(Node parent)
        {
            if (null == parent)
                throw new ArgumentNullException(nameof(parent));

            var node = new Node(this, parent);
            allNodes.Add(node);
            return node;
        }


        public void RemoveNode(Node removeNode)
        {
            var subNodes = allNodes.Where(n => n.Parent == removeNode).ToList();
            foreach (var subNode in subNodes)
                RemoveNode(subNode);

            foreach (var graphObject in graphObjects)
                graphObject.ClearNode(removeNode);
            allNodes.Remove(removeNode);
        }
        

        [DebuggerStepThrough]
        public void Diddle(Action action)
        {
            Diddle(n => action());
        }


        [DebuggerStepThrough]
        public void Diddle(Action<Node> action)
        {
            using (var node = Branch(activeNodes.Peek()))
            {
                Using(node, action);
            }
        }


        [DebuggerStepThrough]
        public T Diddle<T>(Func<T> func)
        {
            return Diddle(n => func());
        }


        [DebuggerStepThrough]
        public T Diddle<T>(Func<Node, T> func)
        {
            T result;
            using (var node = Branch(activeNodes.Peek()))
            {
                result = Using(node, func);
            }
            return result;
        }


        [DebuggerStepThrough]
        public void Using(Node node, Action action)
        {
            Using(node, n => action());
        }


        [DebuggerStepThrough]
        public void Using(Node node, Action<Node> action)
        {
            activeNodes.Push(node);
            action(node);
            activeNodes.Pop();
        }


        [DebuggerStepThrough]
        public T Using<T>(Node node, Func<T> func)
        {
            return Using(node, n => func());
        }


        [DebuggerStepThrough]
        public T Using<T>(Node node, Func<Node, T> func)
        {
            activeNodes.Push(node);
            T result = func(node);
            activeNodes.Pop();
            return result;
        }


        internal void AttachObject(GraphObject graphObject)
        {
            graphObjects.Add(graphObject);
        }


        internal void DetachObject(GraphObject graphObject)
        {
            graphObjects.Remove(graphObject);
        }

        
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    foreach (var node in allNodes)
                        node.Dispose();
                }

                disposedValue = true;
            }
        }


        public void Dispose()
        {
            Dispose(true);
        }
    }
}
