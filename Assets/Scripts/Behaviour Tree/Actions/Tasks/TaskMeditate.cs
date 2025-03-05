using UnityEngine;
using BehaviourTree;
using UnityEngine.UIElements;
public class TaskMeditate : Node
{
    private Transform btTransform;
    private PersonBT personBT;

    private Animator animator;
    private Traits S_Traits;

    public TaskMeditate(PersonBT bt)
    {
        personBT = bt;
        btTransform = personBT.transform;
        animator = personBT.animator;
        S_Traits = personBT.GetComponent<Traits>();
    }
    public override NODE_STATE Evaluate()
    {
        if(!animator.GetBool("isMeditating"))
        {
            personBT.ResetAnimations();
            animator.SetBool("isMeditating", true);
        }

        S_Traits.IncreaseTraitAtRate(ref S_Traits.energy, 10);

        state = NODE_STATE.SUCCESS;
        return state;
    }

}