using UnityEngine;

using BehaviourTree;

public class Patrol : Node
{
    private Transform transform;
    private Transform[] wayPoints;
    public Animator animator;

    private int currentWaypointIndex = 0;

    private float waitTime = 1f;
    private float waitCounter = 0f;
    private bool waiting = false;

    public Patrol(Transform transformIn, Transform[] wayPointsIn, Animator animatorIn)
    {
        transform = transformIn;
        wayPoints = wayPointsIn;
        animator = animatorIn;
    }

    public override NODE_STATE Evaluate()
    {
        if (waiting)
        {
            waitCounter += Time.deltaTime;

            if (waitCounter >= waitTime)
            {
                waiting = false;
                animator.SetBool("isWalking", true);
            }
           
        }
        else
        {
            Transform wp = wayPoints[currentWaypointIndex];

            if (Vector3.Distance(transform.position, wp.position) < 0.01f)
            {
                transform.position = wp.position;
                waitCounter = 0f;
                waiting = true;

                currentWaypointIndex = (currentWaypointIndex + 1) % wayPoints.Length;
                animator.SetBool("isWalking", false);
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, wp.position, PersonBT.speed * Time.deltaTime);
                transform.LookAt(wp.position);
            }
        }

        state = NODE_STATE.RUNNING;
        return state;
    }
}
