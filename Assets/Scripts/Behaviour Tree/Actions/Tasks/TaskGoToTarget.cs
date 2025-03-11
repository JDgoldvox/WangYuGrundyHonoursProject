using UnityEngine;
using BehaviourTree;
using System.Collections.Generic;

/// <summary>
/// Goes to target and stop at 2m
/// </summary>
public class TaskGoToTarget : Node
{
    PersonBT personBT;
    private Transform btTransform;
    private float speed;
    private float waitCounter = 0;
    private Animator animator;
    private Traits S_Traits;

    public TaskGoToTarget(PersonBT bt)
    {
        nodeName = "TaskGoToTarget";
        personBT = bt;
        btTransform = personBT.transform;
        speed = personBT.walkSpeed;
        animator = personBT.animator;
        S_Traits = personBT.GetComponent<Traits>();
    }

    public override NODE_STATE Evaluate()
    {
        if (Time.time <= waitCounter)
        {
            return NODE_STATE.FAILURE; // Still in cooldown
        }

        List<Transform> target = (List<Transform>)GetData("targets");

        if(target[0] == null)
        {
            return NODE_STATE.FAILURE;
        }

        Vector3 positionToGoTo = target[0].position;
        positionToGoTo.y = btTransform.transform.position.y;

        if (Vector3.Distance(positionToGoTo,btTransform.position) > 3f)
        {
            if(!animator.GetBool("isWalking"))
            {
                personBT.ResetAnimations();
                animator.SetBool("isWalking", true);
            }

            //Debug.Log("walking to target");
            btTransform.position = Vector3.MoveTowards(
                btTransform.position,
                positionToGoTo,
                speed * Time.deltaTime
                );

            btTransform.LookAt(positionToGoTo);
            btTransform.eulerAngles = new Vector3(0, btTransform.eulerAngles.y, 0);
            S_Traits.DecreaseTrait(ref S_Traits.energy);
        }
        else
        {
            state = NODE_STATE.SUCCESS;
            waitCounter = Time.time + 3f; //Cooldown duration of 3 seconds
            animator.SetBool("isWalking", false);
            return state;
        }

        state = NODE_STATE.RUNNING;
        return state;   
    }
}
