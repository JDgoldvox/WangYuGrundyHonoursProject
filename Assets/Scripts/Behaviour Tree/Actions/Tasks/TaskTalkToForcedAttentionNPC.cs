using BehaviourTree;
using UnityEngine;

public class TaskTalkToForcedAttentionNPC : Node
{
    private Transform btTransform;
    private PersonBT personBT;
    private Animator animator;

    public TaskTalkToForcedAttentionNPC(PersonBT bt)
    {
        personBT = bt;
        btTransform = personBT.transform;
        animator = personBT.animator;
    }

    public override NODE_STATE Evaluate()
    {
        if (!animator.GetBool("isTalking"))
        {
            personBT.ResetAnimations();
            animator.SetBool("isTalking", true);
        }

        state = NODE_STATE.RUNNING;
        return state;
    }
}