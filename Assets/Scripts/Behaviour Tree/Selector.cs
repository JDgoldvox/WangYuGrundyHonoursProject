using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTreeWang
{
    /// <summary>
    /// Selector node returns 1 state
    /// If atleast 1 node is successful, return success
    /// If no nodes are successful, return failure
    /// </summary>
    public class Selector : Node
    {
        //makes sure base is called
        public Selector() : base() { }
        public Selector(List<Node> children) : base(children) { }

        public override Node Clone()
        {
            return new Selector();
        }

        public override NODE_STATE Evaluate()
        { 
            foreach (Node node in children)
            {
                switch (node.Evaluate())
                {
                    case NODE_STATE.FAILURE:
                        continue;
                    case NODE_STATE.RUNNING:
                        state = NODE_STATE.RUNNING;
                        return state;
                    case NODE_STATE.SUCCESS:
                        state = NODE_STATE.SUCCESS;
                        return state;
                    default:
                        continue;
                }
            }

            state = NODE_STATE.FAILURE;
            return state;
        }
    }
}