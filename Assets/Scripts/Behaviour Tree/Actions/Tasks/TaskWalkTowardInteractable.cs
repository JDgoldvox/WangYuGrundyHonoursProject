using UnityEngine;
using BehaviourTreeWang;
using UnityEngine.UIElements;

public class TaskWalkTowardInteractable : Node
{
    private Traits S_Traits;

    public TaskWalkTowardInteractable(PersonBT bt)
    {
        nodeName = "TaskWalkTowardInteractable";
        personBT = bt;
        S_Traits = personBT.GetComponent<Traits>();
    }

    public override Node Clone()
    {
        return new TaskWalkTowardInteractable(personBT);
    }
    public override void CloneInit(PersonBT bt)
    {
        nodeName = "TaskWalkTowardInteractable";
        personBT = bt;
        S_Traits = personBT.GetComponent<Traits>();
    }
    public override NODE_STATE Evaluate()
    {
        if (personBT.interactablesNear[0] == null)
        {
            state = NODE_STATE.FAILURE;
            return state;
        }

        if (personBT.interactablesNear.Count != 0)
        {
            personBT.transform.position = Vector3.MoveTowards(
                personBT.transform.position,
                personBT.interactablesNear[0].transform.position,
                personBT.walkSpeed * Time.deltaTime
            );

            S_Traits.DecreaseTrait(ref S_Traits.energy);
            S_Traits.IncreaseTrait(ref S_Traits.movement);

            if (Vector3.Distance(personBT.interactablesNear[0].transform.position , personBT.transform.position) < 0.5)
            {
                state = NODE_STATE.SUCCESS;
                return state;
            }

            state = NODE_STATE.RUNNING;
            return state;
        }
        else
        {
            state = NODE_STATE.FAILURE;
            return state;
        }
    }
}