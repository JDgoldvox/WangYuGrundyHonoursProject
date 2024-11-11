using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
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
                        state = NODE_STATE.SUCCESS;
                        return state;
                }
            }

            state = isAnyChildRunning ? NODE_STATE.RUNNING : NODE_STATE.SUCCESS;
            return state;
        }
        
    }
}

