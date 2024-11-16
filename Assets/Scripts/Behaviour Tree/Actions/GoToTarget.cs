using UnityEngine;
using BehaviourTree;
using System.Collections.Generic;

public class GoToTarget : Node
{
    private Transform originTransform;
    private float speed;

    public GoToTarget(Transform transform, float speedIn)
    {
        originTransform = transform;
        speed = speedIn;
    }

    public override NODE_STATE Evaluate()
    {
        Debug.Log("going to target");

        List<Transform> target = (List<Transform>)GetData("targets");

        if (Vector3.Distance(target[0].position, originTransform.position) > 0.01f)
        {
            originTransform.position = Vector3.MoveTowards(
                originTransform.position,
                target[0].position,
                speed * Time.deltaTime
                );

            originTransform.LookAt(target[0].position);
        }

        state = NODE_STATE.RUNNING;
        return state;   
    }

}
