using UnityEngine;
using BehaviourTree;
using UnityEngine.UIElements;
public class TaskMeditate : Node
{
    private Transform btTransform;
    private PersonBT personBT;

    private Animator animator;

    public TaskMeditate(PersonBT bt)
    {
        personBT = bt;
        btTransform = personBT.transform;
        animator = personBT.animator;
    }
    public override NODE_STATE Evaluate()
    {
        if(!animator.GetBool("isMeditating"))
        {
            personBT.ResetAnimations();
            animator.SetBool("isMeditating", true);
        }

        state = NODE_STATE.RUNNING;
        return state;
    }

}