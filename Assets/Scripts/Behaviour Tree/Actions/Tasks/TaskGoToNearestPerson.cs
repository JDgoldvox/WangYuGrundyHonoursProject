using BehaviourTree;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

/// <summary>
/// Goes to target and stop at 2m
/// </summary>
public class TaskGoToNearestPerson : Node
{
    PersonBT personBT;
    private Transform btTransform;
    private float speed;
    private Animator animator;

    public TaskGoToNearestPerson(PersonBT bt)
    {
        personBT = bt;
        btTransform = personBT.transform;
        speed = personBT.walkSpeed;
        animator = personBT.animator;
    }

    public override NODE_STATE Evaluate()
    {
        if (personBT.nearestPlayer == null)
        {
            return NODE_STATE.FAILURE;
        }

        Vector3 positionToGoTo = personBT.nearestPlayer.transform.position;
        positionToGoTo.y = btTransform.transform.position.y;

        if (Vector3.Distance(positionToGoTo, btTransform.position) > 3f)
        {
            if (!animator.GetBool("isWalking"))
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
        }
        else
        {
            state = NODE_STATE.SUCCESS;
            animator.SetBool("isWalking", false);
            return state;
        }

        state = NODE_STATE.RUNNING;
        return state;
    }
}
