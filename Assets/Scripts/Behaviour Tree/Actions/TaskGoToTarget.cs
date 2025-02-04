using UnityEngine;
using BehaviourTree;
using System.Collections.Generic;

/// <summary>
/// Goes to target and stop at 2m
/// </summary>
public class TaskGoToTarget : Node
{
    private Transform originTransform;
    private float speed;
    private float waitCounter = 0;
    private Animator animator;

    public TaskGoToTarget(Transform transform, float speedIn, Animator animatorIn)
    {
        originTransform = transform;
        speed = speedIn;
        animator = animatorIn;
    }

    public override NODE_STATE Evaluate()
    {

        if (Time.time <= waitCounter)
        {
            return NODE_STATE.FAILURE; // Still in cooldown
        }

        List<Transform> target = (List<Transform>)GetData("targets");

        Vector3 positionToGoTo = target[0].position;
        positionToGoTo.y = originTransform.transform.position.y;

        if (Vector3.Distance(positionToGoTo,originTransform.position) > 3f)
        {
            if(!animator.GetBool("isWalking"))
            {
                animator.SetBool("isWalking", true);
            }

            //Debug.Log("walking to target");
            originTransform.position = Vector3.MoveTowards(
                originTransform.position,
                positionToGoTo,
                speed * Time.deltaTime
                );

            originTransform.LookAt(positionToGoTo);
            originTransform.eulerAngles = new Vector3(0, originTransform.eulerAngles.y, 0);
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
