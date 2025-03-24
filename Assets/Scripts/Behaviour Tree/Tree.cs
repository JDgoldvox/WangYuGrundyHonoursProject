using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace BehaviourTree
{
    public abstract class Tree : MonoBehaviour
    {
        [SerializeReference]
        public Node root = null;
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

        public abstract Node InitTree();
    
    }
}
