using UnityEngine;
using BehaviourTree;

public class TaskRandomWalk : Node
{

    private Transform targetTransform;
    private Animator animator;
    private float speed;

    private float walkTime = 5f;
    private float waitCounter = 0f;
    private bool waiting = false;

    private Vector3 destination = Vector3.zero;
    private float maxRange = 10f;

    private PersonBT personBT;
    float xDist, zDist;

    public TaskRandomWalk(PersonBT parentTransform, Animator animatorIn, float speedIn)
    {
        personBT = parentTransform;
        targetTransform = parentTransform.transform;
        animator = animatorIn;
        speed = speedIn;
    }

    public override NODE_STATE Evaluate()
    {
        //Generate position to go to
        if(destination == Vector3.zero)
        {
            Transform personBTTransform = personBT.transform;
            float x = UnityEngine.Random.Range(personBTTransform.position.x - maxRange, personBTTransform.position.x + maxRange);
            float z = UnityEngine.Random.Range(personBTTransform.position.z - maxRange, personBTTransform.position.z + maxRange);
            destination = new Vector3(x, 0, z);
            animator.SetBool("isWalking", true);
            xDist = Mathf.Abs(targetTransform.position.x - destination.x);
            zDist = Mathf.Abs(targetTransform.position.z - destination.z);

            //Debug.Log("Targetting " + destination);
        }

        if (xDist > 0.2f && zDist > 0.2f)
        {
            targetTransform.position = Vector3.MoveTowards(
                targetTransform.position,
                destination,
                speed * Time.deltaTime
            );

            targetTransform.LookAt(destination);
            xDist = Mathf.Abs(targetTransform.position.x - destination.x);
            zDist = Mathf.Abs(targetTransform.position.z - destination.z);

            //Debug.Log("At " + targetTransform.transform.position);
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
