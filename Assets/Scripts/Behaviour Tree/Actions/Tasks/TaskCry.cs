using UnityEngine;
using BehaviourTree;
using UnityEngine.UIElements;
public class TaskCry : Node
{
    private Transform btTransform;
    private PersonBT personBT;

    private float waitCounter = 0;
    private float maxWaitCounter = 2f;
    private float maxActionTime = 5f;
    private Animator animator;

    public TaskCry(PersonBT bt)
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
            if (!animator.GetBool("isCrying"))
            {
                personBT.ResetAnimations();
                animator.SetBool("isCrying", true);
            }
        }
        else
        {
            state = NODE_STATE.SUCCESS;
            animator.SetBool("isCrying", false);

            //apply cooldown
            waitCounter = Time.time + maxWaitCounter;

            return state;
        }

        state = NODE_STATE.RUNNING;
        return state;
    }

}