using UnityEngine;
using BehaviourTree;
using Unity.Mathematics;
using System;

public class TaskRandomWalk : Node
{

    private Transform targetTransform;
    private Animator animator;
    private float speed;

    private int currentWaypointIndex = 0;

    private float walkTime = 5f;
    private float waitCounter = 0f;
    private bool waiting = false;

    private Vector3 destination = Vector3.zero;
    private float maxRange = 10f;

    public TaskRandomWalk(Transform parentTransform, Animator animatorIn, float speedIn)
    {
        targetTransform = parentTransform;
        animator = animatorIn;
        speed = speedIn;
    }

    public override NODE_STATE Evaluate()
    {
        //Generate position to go to
        if(destination == Vector3.zero)
        {
            float x = UnityEngine.Random.Range(targetTransform.position.x - maxRange, targetTransform.position.x + maxRange);
            float z = UnityEngine.Random.Range(targetTransform.position.z - maxRange, targetTransform.position.z + maxRange);
            destination = new Vector3(x,targetTransform.position.y,z);
            animator.SetBool("isWalking", true);
        }

        if (Vector3.Distance(destination, targetTransform.position) > 0.2f)
        {
            targetTransform.position = Vector3.MoveTowards(
                targetTransform.position,
                destination,
                speed * Time.deltaTime
            );

            targetTransform.LookAt(destination);
        }
        else
        {
            state = NODE_STATE.SUCCESS;
            destination = Vector3.zero;
            animator.SetBool("isWalking", false);
            return state;
        }

        state = NODE_STATE.RUNNING;
        return state;
    }
}
