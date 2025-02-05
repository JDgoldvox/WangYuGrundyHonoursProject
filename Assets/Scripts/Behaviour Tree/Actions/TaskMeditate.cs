using UnityEngine;
using BehaviourTree;
public class TaskMeditate : Node
{
    private Transform originTransform;
    private float waitCounter = 0;
    private float maxWaitCounter = 2f;
    private float maxMeditateTime = 3f;
    private Animator animator;

    public TaskMeditate(Transform transformIn, Animator animatorIn)
    {
        originTransform = transformIn;
        animator = animatorIn;
    }
    public override NODE_STATE Evaluate()
    {
        if (Time.time <= waitCounter)
        {
            return NODE_STATE.FAILURE; // Still in cooldown
        }

        if (Time.time < waitCounter + maxMeditateTime)
        {
            animator.SetBool("isMeditating", true);
        }
        else
        {
            state = NODE_STATE.SUCCESS;
            animator.SetBool("isMeditating", false);

            //apply cooldown
            waitCounter = Time.time + waitCounter;

            return state;
        }

        state = NODE_STATE.RUNNING;
        return state;
    }

}