using UnityEngine;
using BehaviourTreeWang;
public class TaskTalk : Node
{
    private Transform btTransform;
    private PersonBT personBT;

    private float maxActionTime = 3f;
    private float actionTimer = float.MaxValue;
    private Animator animator;
    private Traits S_Traits;

    public TaskTalk(PersonBT bt)
    {
        nodeName = "TaskTalk";
        personBT = bt;
        btTransform = personBT.transform;
        animator = personBT.animator;
        S_Traits = personBT.GetComponent<Traits>();
    }
    public override NODE_STATE Evaluate()
    {
        if (Time.time < actionTimer)
        {
            if (!animator.GetBool("isTalking"))
            {
                personBT.ResetAnimations();
                animator.SetBool("isTalking", true);
                actionTimer = Time.time + maxActionTime;
            }

            S_Traits.DecreaseTrait(ref S_Traits.energy);
            S_Traits.IncreaseTrait(ref S_Traits.socialness);
        }
        else
        {
            state = NODE_STATE.SUCCESS;
            animator.SetBool("isTalking", false);
            actionTimer = float.MaxValue;
            return state;
        }

        state = NODE_STATE.RUNNING;
        return state;
    }

}