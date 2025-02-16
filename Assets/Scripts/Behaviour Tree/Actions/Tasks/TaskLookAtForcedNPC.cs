using UnityEngine;
using BehaviourTree;
using UnityEngine.UIElements;

public class TaskLookAtForcedNPC : Node
{
    private PersonBT personBT;
    private Transform btTransform;

    public TaskLookAtForcedNPC(PersonBT bt)
    {
        personBT = bt;
        btTransform = personBT.transform;
    }

    public override NODE_STATE Evaluate()
    {
        btTransform.transform.LookAt(personBT.forcedAttentionToPlayer);
        state = NODE_STATE.SUCCESS;
        return state;
    }
}