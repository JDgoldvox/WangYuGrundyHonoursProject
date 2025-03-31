using UnityEngine;
using BehaviourTreeWang;
using UnityEngine.UIElements;
public class TaskHappy : Node
{
    private Transform btTransform;

    private float waitCounter = 0;
    private float maxWaitCounter = 2f;
    private float maxActionTime = 1.5f;
    private Animator animator;
    private Traits S_Traits;

    public TaskHappy(PersonBT bt)
    {
        nodeName = "TaskHappy";
        personBT = bt;
        btTransform = personBT.transform;
        animator = personBT.animator;
        S_Traits = personBT.GetComponent<Traits>();
    }

    public override void CloneInit(PersonBT bt)
    {
        nodeName = "TaskHappy";
        personBT = bt;
        btTransform = personBT.transform;
        animator = personBT.animator;
        S_Traits = personBT.GetComponent<Traits>();
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

            S_Traits.IncreaseTrait(ref S_Traits.happiness);
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