using BehaviourTree;

public class CheckAnger : Node
{
    private PersonBT personBT;
    private Traits traits;

    public CheckAnger(PersonBT bt)
    {
        personBT = bt;
        traits = personBT.transform.GetComponent<Traits>();
    }

    public override NODE_STATE Evaluate()
    {
        if (traits.anger > traits.HIGH)
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