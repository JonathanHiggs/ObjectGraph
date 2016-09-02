﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ObjectGraph
{
    public class GraphObject
    {
        private static PropertyRegister propertyRegister = new PropertyRegister();

        private Graph graph;
        private Dictionary<Property, Dictionary<Node, object>> valueStackByProperty = new Dictionary<Property, Dictionary<Node, object>>();


        public GraphObject(Graph graph)
        {
            if (null == graph)
                throw new ArgumentNullException(nameof(graph));

            this.graph = graph;
            this.graph.AttachObject(this);
        }


        public Graph Graph
        { get { return graph; } }


        #region GetValue
        [DebuggerStepThrough]
        public TVal GetValue<TObj, TVal>(StaticProperty<TObj, TVal> property, int minDepth = 1) where TObj : GraphObject
        {
            return GetValue(property, graph.CurrentNode, minDepth);
        }


        public TVal GetValue<TObj, TVal>(StaticProperty<TObj, TVal> property, Node node, int minDepth = 1) where TObj : GraphObject
        {
            if (valueStackByProperty.ContainsKey(property))
            {
                var valueStack = valueStackByProperty[property];

                return (TVal)valueStack[node.Stack.Where(n => n.Depth >= minDepth).First(n => valueStack.ContainsKey(n))];
            }
            return property.Default;
        }
        #endregion


        #region SetValue
        [DebuggerStepThrough]
        public void SetValue<TObj, TVal>(StaticProperty<TObj, TVal> property, TVal value) where TObj : GraphObject
        {
            SetValue(property, graph.CurrentNode, value);
        }


        public void SetValue<TObj, TVal>(StaticProperty<TObj, TVal> property, Node node, TVal value) where TObj : GraphObject
        {
            if (!valueStackByProperty.ContainsKey(property))
                valueStackByProperty.Add(property, new Dictionary<Node, object>());

            var valueStack = valueStackByProperty[property];

            if (!valueStack.ContainsKey(node))
                valueStack.Add(node, value);
            else
                valueStack[node] = value;
        }
        #endregion


        #region ClearValue
        [DebuggerStepThrough]
        public void ClearValue(Property property)
        {
            ClearValue(property, graph.CurrentNode);
        }


        public void ClearValue(Property property, Node node)
        {
            if (!valueStackByProperty.ContainsKey(property))
                return;

            var valueStack = valueStackByProperty[property];

            foreach (var removeNode in valueStack.Keys.Where(n => n == node || n.IsDecendent(node)).ToList())
                valueStack.Remove(removeNode);

            if (valueStack.Count == 0)
                valueStackByProperty.Remove(property);
        }
        #endregion


        #region Register
        protected static StaticProperty<TObj, TVal> RegisterProperty<TObj, TVal>(Expression<Func<TObj, TVal>> property, TVal defaultValue) where TObj : GraphObject
        {
            var graphProperty = new StaticProperty<TObj, TVal>(property, defaultValue);
            propertyRegister.RegisterProperty<TObj>(graphProperty);
            return graphProperty;
        }
        #endregion


        internal void ClearNode(Node node)
        {
            foreach (var valueStack in valueStackByProperty.Values)
                if (valueStack.ContainsKey(node))
                    valueStack.Remove(node);
        }


        public int ValueDepth(Property property, Node node)
        {
            if (valueStackByProperty.ContainsKey(property))
            {
                var valueStack = valueStackByProperty[property];

                return node.Stack.First(n => valueStack.ContainsKey(n)).Depth;
            }
            return 0;
        }
    }
}
