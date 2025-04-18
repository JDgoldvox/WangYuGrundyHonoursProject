using BehaviourTreeWang;
using UnityEngine;

public class TaskTalkToForcedAttentionNPC : Node
{
    private Transform btTransform;
    private Animator animator;
    private Traits S_Traits;

    public TaskTalkToForcedAttentionNPC(PersonBT bt)
    {
        nodeName = "TaskTalkToForcedAttentionNPC";
        personBT = bt;
        btTransform = personBT.transform;
        animator = personBT.animator;
        S_Traits = personBT.GetComponent<Traits>();
    }

    public override Node Clone()
    {
        return new TaskTalkToForcedAttentionNPC(personBT);
    }
    public override void CloneInit(PersonBT bt)
    {
        nodeName = "TaskTalkToForcedAttentionNPC";
        personBT = bt;
        btTransform = personBT.transform;
        animator = personBT.animator;
        S_Traits = personBT.GetComponent<Traits>();
    }
    public override NODE_STATE Evaluate()
    {
        if (!animator.GetBool("isTalking"))
        {
            personBT.ResetAnimations();
            animator.SetBool("isTalking", true);
        }

        S_Traits.DecreaseTrait(ref S_Traits.energy);
        S_Traits.DecreaseTrait(ref S_Traits.anger);

        state = NODE_STATE.SUCCESS;
        return state;
    }
}