using UnityEngine;
using BehaviourTreeWang;
using static UnityEngine.ParticleSystem;

public class CheckForceAttention : Node
{
    public CheckForceAttention(PersonBT bt)
    {
        nodeName = "CheckForceAttention";
        personBT = bt;
    }

    public override Node Clone()
    {
        return new CheckForceAttention(personBT);
    }

    public override void CloneInit(PersonBT bt)
    {
        nodeName = "CheckForceAttention";
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
