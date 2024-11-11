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
            Debug.Log("Creating Tree");
            root = InitTree();

            Debug.Log("note added... ");

            if(root == null)
            {
                Debug.Log("node is null ");
            }
            else
            {
                Debug.Log("node is NOT null ");
            }
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
