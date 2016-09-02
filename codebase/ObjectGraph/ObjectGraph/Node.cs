using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectGraph
{
    public class Node : IDisposable
    {
        private Graph graph;
        private Node parent;
        private bool disposedValue = false;


        public Node(Graph graph, Node parent)
        {
            if (null == graph)
                throw new ArgumentNullException(nameof(graph));

            this.graph = graph;
            this.parent = parent;
        }


        public int Depth
        {
            get
            {
                if (null == parent)
                    return 1;
                return parent.Depth + 1;
            }
        }


        public Graph Graph
        { get { return graph; } }


        public Node Parent
        { get { return parent; } }


        public bool IsRoot
        { get { return null == parent; } }


        public IEnumerable<Node> Stack
        {
            get
            {
                var node = this;
                while (null != node)
                {
                    yield return node;
                    node = node.parent;
                }
            }
        }


        public bool IsDecendent(Node node)
        {
            return node.Stack.Contains(this);
        }


        public override string ToString()
        {
            return $"<Node({Depth})>";
        }


        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    graph.RemoveNode(this);
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
