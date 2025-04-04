
using UnityEngine;
using BehaviourTreeWang;
public class TaskPickGunUp : Node
{
    private Transform btTransform;

    private float waitCounter = 0;
    private float maxWaitCounter = 2f;
    private float maxActionTime = 1.5f;
    private Animator animator;

    public TaskPickGunUp(PersonBT bt)
    {
        nodeName = "TaskPickGunUp";
        personBT = bt;
        btTransform = personBT.transform;
        animator = personBT.animator;
    }

    public override Node Clone()
    {
        return new TaskPickGunUp(personBT);
    }
    public override void CloneInit(PersonBT bt)
    {
        nodeName = "TaskPickGunUp";
        personBT = bt;
        btTransform = personBT.transform;
        animator = personBT.animator;
    }


    public override NODE_STATE Evaluate()
    {
        if (personBT.interactablesNear[0] != null)  
        {
            UnityEngine.GameObject.Destroy(personBT.interactablesNear[0].transform.gameObject);
            personBT.interactablesNear.Remove(personBT.interactablesNear[0]);

            state = NODE_STATE.SUCCESS;
            return state;
        }

        state = NODE_STATE.FAILURE;
        return state;
    }
}

