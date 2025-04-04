using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTreeWang
{
    /// <summary>
    /// Sequence returns success if all nodes are successful
    /// If 1 node returns failure, they all fail
    /// </summary>
    public class Sequence : Node
    {
        //makes sure base is called
        public Sequence() : base() {}
        public Sequence(string nameOfSequence, List<Node> children) : base(children) 
        {
            nodeName = nameOfSequence;
        }

        public override Node Clone()
        {
            return new Sequence();
        }

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
                        state = NODE_STATE.RUNNING;
                        return state;
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

