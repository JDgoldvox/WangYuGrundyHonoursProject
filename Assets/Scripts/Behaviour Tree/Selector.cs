using NUnit.Framework.Interfaces;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Metadata;

namespace BehaviourTree
{
    public class Selector : Node
    {
        //makes sure base is called
        public Selector() : base() { }
        public Selector(List<Node> children) : base(children) { }

        public override NODE_STATE Evaluate()
        {
            foreach (Node node in children)
            {
                switch (node.Evaluate())
                {
                    case NODE_STATE.FAILURE:
                        state = NODE_STATE.FAILURE;
                        return state;
                    case NODE_STATE.RUNNING:
                        state = NODE_STATE.RUNNING;
                        return state;
                    case NODE_STATE.SUCCESS:
                        state = NODE_STATE.SUCCESS;
                        return state;
                }
            }

            state = NODE_STATE.FAILURE;
            return state;
        }
    }
}