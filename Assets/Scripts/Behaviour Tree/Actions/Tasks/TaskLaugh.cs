using UnityEngine;
using BehaviourTree;
using UnityEngine.UIElements;
public class TaskLaugh : Node
{
    private Transform btTransform;
    private PersonBT personBT;

    private float waitCounter = 0;
    private float maxWaitCounter = 2f;
    private float maxActionTime = 3f;
    private Animator animator;

    public TaskLaugh(PersonBT bt)
    {
        personBT = bt;
        btTransform = personBT.transform;
        animator = personBT.animator;
    }
    public override NODE_STATE Evaluate()
    {
        if (Time.time <= waitCounter)
        {
            return NODE_STATE.FAILURE; // Still in cooldown
        }

        if (Time.time < waitCounter + maxActionTime)
        {
            if (!animator.GetBool("isLaughing"))
            {
                personBT.ResetAnimations();
                animator.SetBool("isLaughing", true);
            }
        }
        else
        {
            state = NODE_STATE.SUCCESS;
            animator.SetBool("isLaughing", false);

            //apply cooldown
            waitCounter = Time.time + maxWaitCounter;

            return state;
        }

        state = NODE_STATE.RUNNING;
        return state;
    }

}