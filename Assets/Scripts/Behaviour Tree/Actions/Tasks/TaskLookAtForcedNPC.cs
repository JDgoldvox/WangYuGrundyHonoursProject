using UnityEngine;
using BehaviourTreeWang;

public class TaskLookAtForcedNPC : Node
{
    private Transform btTransform;
    private float cooldown = 0.5f;
    private float timer = 0;

    public TaskLookAtForcedNPC(PersonBT bt)
    {
        nodeName = "TaskLookAtForcedNPC";
        personBT = bt;
        btTransform = personBT.transform;
    }

    public override void CloneInit(PersonBT bt)
    {
        nodeName = "TaskLookAtForcedNPC";
        personBT = bt;
        btTransform = personBT.transform;
    }

    public override Node Clone()
    {
        return new TaskLookAtForcedNPC(personBT);
    }

    public override NODE_STATE Evaluate()
    {
        if(timer < Time.time)
        {
            timer = Time.time + cooldown;
        }
        else
        {
            state = NODE_STATE.FAILURE;
            return NODE_STATE.FAILURE;
        }

        btTransform.transform.LookAt(personBT.forcedAttentionToPlayer);
        state = NODE_STATE.SUCCESS;
        return state;
    }
}