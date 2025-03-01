using BehaviourTree;
using System.Diagnostics;
using UnityEngine;

public class CheckEnergy : Node
{
    private PersonBT personBT;
    private Traits traits;
    float targetEnergy = 0;

    public CheckEnergy(PersonBT bt)
    {
        personBT = bt;
        traits = personBT.transform.GetComponent<Traits>();
    }

    public override NODE_STATE Evaluate()
    {

        if (traits.energy < traits.LOW || traits.energy < targetEnergy)
        {
            targetEnergy = 1;
            state = NODE_STATE.SUCCESS;
            return state;
        }
        else
        {
            targetEnergy = 0;
            state = NODE_STATE.FAILURE;
            return state;
        }
    }
}