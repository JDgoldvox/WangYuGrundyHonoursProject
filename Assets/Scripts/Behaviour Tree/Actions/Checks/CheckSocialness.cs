using BehaviourTree;
using System.Diagnostics;

public class CheckSocialness : Node
{
    private PersonBT personBT;
    private Traits traits;

    public CheckSocialness(PersonBT bt)
    {
        personBT = bt;
        traits = personBT.transform.GetComponent<Traits>();
    }

    public override NODE_STATE Evaluate()
    {
        //Check if Socialness trait is < 0.3
        if (traits.socialness < traits.LOW)
        {
            state = NODE_STATE.SUCCESS;
            return state;
        }
        else
        {
            state = NODE_STATE.FAILURE;
            return state;
        }
    }
}