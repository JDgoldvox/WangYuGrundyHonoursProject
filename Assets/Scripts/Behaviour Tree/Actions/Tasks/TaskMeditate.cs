using UnityEngine;
using BehaviourTreeWang;
using UnityEngine.UIElements;
public class TaskMeditate : Node
{
    private Transform btTransform;

    private Animator animator;
    private Traits S_Traits;

    public TaskMeditate(PersonBT bt)
    {
        nodeName = "TaskMeditate";
        personBT = bt;
        btTransform = personBT.transform;
        animator = personBT.animator;
        S_Traits = personBT.GetComponent<Traits>();
    }

    public override Node Clone()
    {
        return new TaskMeditate(personBT);
    }
    public override void CloneInit(PersonBT bt)
    {
        nodeName = "TaskMeditate";
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

        S_Traits.IncreaseTraitAtRate(ref S_Traits.energy, 0.1f);

        state = NODE_STATE.SUCCESS;
        return state;
    }

}