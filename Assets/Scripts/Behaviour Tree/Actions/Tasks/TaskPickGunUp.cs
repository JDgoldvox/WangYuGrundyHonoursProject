
using UnityEngine;
using BehaviourTree;
public class TaskPickGunUp : Node
{
    private Transform btTransform;
    private PersonBT personBT;

    private float waitCounter = 0;
    private float maxWaitCounter = 2f;
    private float maxActionTime = 1.5f;
    private Animator animator;

    public TaskPickGunUp(PersonBT bt)
    {
        personBT = bt;
        btTransform = personBT.transform;
        animator = personBT.animator;
    }
    public override NODE_STATE Evaluate()
    {
        if (personBT.interactablesNear[0] != null)
        {
            personBT.interactablesNear[0].gameObject.SetActive(false);
            personBT.interactablesNear.Remove(personBT.interactablesNear[0]);
        }

        state = NODE_STATE.RUNNING;
        return state;
    }
}

