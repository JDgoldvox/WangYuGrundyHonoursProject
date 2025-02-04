using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    /// <summary>
    /// Sequence returns success if all nodes are successful
    /// If 1 node returns failure, they all fail
    /// </summary>
    public class Sequence : Node
    {
        //makes sure base is called
        public Sequence() : base() {}
        public Sequence(List<Node> children) : base(children) {}

        public override NODE_STATE Evaluate() 
        {
            bool isAnyChildRunning = false;

            foreach(Node node in children)
            {
                switch (node.Evaluate()) 
                {
                    case NODE_STATE.FAILURE:
                        state = NODE_STATE.FAILURE;
                        return state;
                    case NODE_STATE.RUNNING:
                        isAnyChildRunning = true;
                        continue;
                    case NODE_STATE.SUCCESS:
                        continue;
                    default:
                        state = NODE_STATE.SUCCESS;
                        return state;
                }
            }

            state = isAnyChildRunning ? NODE_STATE.RUNNING : NODE_STATE.SUCCESS;
            return state;
        }
    }
}

