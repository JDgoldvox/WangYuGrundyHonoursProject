using UnityEngine;
using BehaviourTreeWang;
using UnityEngine.UIElements;
public class TaskCry : Node
{
    private Transform btTransform;

    private float waitCounter = 0;
    private float maxWaitCounter = 2f;
    private float maxActionTime = 5f;
    private Animator animator;
    private Traits S_Traits;
    public TaskCry(PersonBT bt)
    {
        nodeName = "TaskCry";
        personBT = bt;
        btTransform = personBT.transform;
        animator = personBT.animator;
        S_Traits = personBT.GetComponent<Traits>();
    }

    public override void CloneInit(PersonBT bt)
    {
        nodeName = "TaskCry";
        personBT = bt;
        btTransform = personBT.transform;
        animator = personBT.animator;
        S_Traits = personBT.GetComponent<Traits>();
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

            S_Traits.DecreaseTrait(ref S_Traits.energy);
            S_Traits.IncreaseTrait(ref S_Traits.sadness);
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