using UnityEngine;
using BehaviourTree;
public class TaskTalk : Node
{
    private Transform btTransform;
    private PersonBT personBT;

    private float maxActionTime = 3f;
    private float actionTimer = float.MaxValue;
    private Animator animator;

    public TaskTalk(PersonBT bt)
    {
        personBT = bt;
        btTransform = personBT.transform;
        animator = personBT.animator;
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