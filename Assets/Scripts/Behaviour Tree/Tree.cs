using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace BehaviourTree
{
    public abstract class Tree : MonoBehaviour
    {
        private Node root = null;

        protected void Start()
        {
            root = InitTree();
        }

        private void Update()
        {
            //update tree continuously
            if (root != null)
            {
                root.Evaluate();
            }
        }

        protected abstract Node InitTree();

    
    }
}
