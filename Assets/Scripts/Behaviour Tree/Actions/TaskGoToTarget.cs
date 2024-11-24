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

    public TaskGoToTarget(Transform transform, float speedIn)
    {
        originTransform = transform;
        speed = speedIn;
    }

    public override NODE_STATE Evaluate()
    {

        if (Time.time <= waitCounter)
        {
            return NODE_STATE.FAILURE; // Still in cooldown
        }

        Debug.Log("going to target");

        List<Transform> target = (List<Transform>)GetData("targets");

        if (Vector3.Distance(target[0].position, originTransform.position) > 2f)
        {
            Debug.Log("walking to target");
            originTransform.position = Vector3.MoveTowards(
                originTransform.position,
                target[0].position,
                speed * Time.deltaTime
                );

            originTransform.LookAt(target[0].position);
        }
        else
        {
            state = NODE_STATE.SUCCESS;
            waitCounter = Time.time + 3f; //Cooldown duration of 3 seconds
            Debug.Log(waitCounter);
            return state;
        }

        state = NODE_STATE.RUNNING;
        return state;   
    }

}
