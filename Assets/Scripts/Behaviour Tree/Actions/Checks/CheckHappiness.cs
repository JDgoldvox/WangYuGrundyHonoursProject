using BehaviourTreeWang;

public class CheckHappiness : Node
{
    private PersonBT personBT;
    private Traits traits;

    public CheckHappiness(PersonBT bt)
    {
        nodeName = "CheckHappiness";
        personBT = bt;
        traits = personBT.transform.GetComponent<Traits>();   
    }

    public override NODE_STATE Evaluate()
    {
        if(traits.happiness < 0.5)
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
