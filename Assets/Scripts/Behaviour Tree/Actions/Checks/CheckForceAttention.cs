using UnityEngine;
using BehaviourTree;

public class CheckForceAttention : Node
{
    private PersonBT personBT;

    public CheckForceAttention(PersonBT bt)
    {
        personBT = bt;
    }
    public override NODE_STATE Evaluate()
    {
        if(personBT.forcedAttentionToPlayer != null)
        {
            state = NODE_STATE.SUCCESS;
            return state;
        }

        state = NODE_STATE.FAILURE;
        return state;
    }
}
