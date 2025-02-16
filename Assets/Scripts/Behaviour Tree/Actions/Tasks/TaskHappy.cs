using UnityEngine;
using BehaviourTree;
using UnityEngine.UIElements;
public class TaskHappy : Node
{
    private Transform btTransform;
    private PersonBT personBT;

    private float waitCounter = 0;
    private float maxWaitCounter = 2f;
    private float maxActionTime = 1.5f;
    private Animator animator;

    public TaskHappy(PersonBT bt)
    {
        personBT = bt;
        btTransform = personBT.transform;
        animator = personBT.animator;
    }
    public override NODE_STATE Evaluate()
    {
        foreach (AnimatorControllerParameter parameter in animator.parameters)
        {
            animator.SetBool(parameter.name, false);
        }

        if (Time.time <= waitCounter)
        {
            return NODE_STATE.FAILURE; // Still in cooldown
        }

        if (Time.time < waitCounter + maxActionTime)
        {
            if(!animator.GetBool("isHappy"))
            {
                personBT.ResetAnimations();
                animator.SetBool("isHappy", true);
            }
        }
        else
        {
            state = NODE_STATE.SUCCESS;
            animator.SetBool("isHappy", false);

            //apply cooldown
            waitCounter = Time.time + maxWaitCounter;

            return state;
        }

        state = NODE_STATE.RUNNING;
        return state;
    }
}