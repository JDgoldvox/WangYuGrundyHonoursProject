using UnityEngine;
using BehaviourTreeWang;
using UnityEngine.UIElements;
public class SetForceAttentionToNPC : Node
{
    public SetForceAttentionToNPC(PersonBT bt)
    {
        personBT = bt;
    }

    public override Node Clone()
    {
        return new SetForceAttentionToNPC(personBT);
    }
    public override void CloneInit(PersonBT bt)
    {
        personBT = bt;
    }
    public override NODE_STATE Evaluate()
    {
        //set this forced attention NPC
        personBT.forcedAttentionToPlayer = personBT.nearestPlayer.transform;

        //set the forced attention NPC to this
        personBT.forcedAttentionToPlayer.GetComponent<PersonBT>().forcedAttentionToPlayer = personBT.transform;

        state = NODE_STATE.SUCCESS;
        return state;
    }
}

