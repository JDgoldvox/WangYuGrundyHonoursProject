using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace BehaviourTreeWang
{
    public abstract class Tree : MonoBehaviour
    {
        [SerializeReference]
        public Node root = null;
        [HideInInspector] public bool IsCloned = false;

        protected void Start()
        {
            if(IsCloned == false)
            {
                root = InitTree();
            }
            else
            {
                InitClone();
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

        public abstract Node InitTree();
        
        public abstract void InitClone();
    }
}
