using BehaviourTree;

public class CheckEnergy : Node
{
    private PersonBT personBT;
    private Traits traits;

    public CheckEnergy(PersonBT bt)
    {
        personBT = bt;
        traits = personBT.transform.GetComponent<Traits>();
    }

    public override NODE_STATE Evaluate()
    {
        if (traits.energy < traits.LOW)
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