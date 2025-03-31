using BehaviourTreeWang;

public class CheckAnger : Node
{
    private Traits traits;

    public CheckAnger(PersonBT bt)
    {
        nodeName = "CheckAnger";
        personBT = bt;
        traits = personBT.transform.GetComponent<Traits>();
    }

    public override void CloneInit(PersonBT bt)
    {
        nodeName = "CheckAnger";
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