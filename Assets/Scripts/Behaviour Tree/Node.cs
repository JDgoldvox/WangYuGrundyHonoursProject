using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTreeWang
{
    public enum NODE_STATE
    {
        RUNNING,
        SUCCESS,
        FAILURE
    }

    public class Node
    {
        protected NODE_STATE state;

        public Node parent;
        public List<Node> children = new List<Node>();
        private Dictionary<string, object> data = new Dictionary<string, object>();
        public string nodeName = "no name";

        public Node()
        {
            parent = null;
        }

        public Node(List<Node> children)
        {
            foreach (Node child in children)
            {
                Attach(child);
            }
        }

        public void Attach(Node node)
        {
            node.parent = this;
            children.Add(node);
        }

        public virtual NODE_STATE Evaluate() => NODE_STATE.FAILURE;

        public void SetData(string key, object value)
        {
            data[key] = value;
        }
        public object GetData(string key)
        {
            //try to get the value from the current nodes data
            if (data.TryGetValue(key, out object value))
            {
                return value;
            }

            //Go up the tree to find the data
            Node node = parent;
            while (node != null)
            {
                //Check each parent nodes data
                if (node.data.TryGetValue(key, out value))
                {
                    return value;
                }
                node = node.parent;
            }

            // No object found
            return null;
        }

        public bool RemoveData(string key)
        {
            if (data.ContainsKey(key))
            {
                data.Remove(key);
                return true;
            }
            else
            {
                if (parent != null)
                {
                    return parent.RemoveData(key);
                }
            }

            return false;
        }
    }
}

