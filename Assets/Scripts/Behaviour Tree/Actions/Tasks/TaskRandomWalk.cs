using UnityEngine;
using BehaviourTree;

public class TaskRandomWalk : Node
{
    private BehaviourTreeUtility S_BehaviourTreeUtility;
    private Transform targetTransform;
    private Animator animator;
    private float speed;

    private float waitCounter = 0f;

    private Vector3 destination = Vector3.zero;
    private float maxRange = 10f;

    private PersonBT personBT;
    float xDist, zDist;

    public TaskRandomWalk(PersonBT btIn)
    {
        personBT = btIn;
        targetTransform = personBT.transform;
        animator = personBT.animator;
        speed = personBT.walkSpeed;
        S_BehaviourTreeUtility = personBT.transform.GetComponent<BehaviourTreeUtility>();
    }

    public override NODE_STATE Evaluate()
    {
        if (Time.time <= waitCounter)
        {
            return NODE_STATE.FAILURE; // Still in cooldown
        }

        //Generate position to go to
        if (destination == Vector3.zero)
        {
            Transform personBTTransform = personBT.transform;
            float x = UnityEngine.Random.Range(personBTTransform.position.x - maxRange, personBTTransform.position.x + maxRange);
            float z = UnityEngine.Random.Range(personBTTransform.position.z - maxRange, personBTTransform.position.z + maxRange);
            destination = new Vector3(x, personBTTransform.transform.position.y, z);
            xDist = Mathf.Abs(targetTransform.position.x - destination.x);
            zDist = Mathf.Abs(targetTransform.position.z - destination.z);
        }

        if (xDist > 0.2f && zDist > 0.2f)
        {
            if(!animator.GetBool("isWalking"))
            {
                personBT.ResetAnimations();
                animator.SetBool("isWalking", true);
            }

            targetTransform.position = Vector3.MoveTowards(
                targetTransform.position,
                destination,
                speed * Time.deltaTime
            );

            targetTransform.LookAt(destination);
            xDist = Mathf.Abs(targetTransform.position.x - destination.x);
            zDist = Mathf.Abs(targetTransform.position.z - destination.z);
        }
        else
        {
            state = NODE_STATE.SUCCESS;
            destination = Vector3.zero;
            animator.SetBool("isWalking", false);

            //apply cooldown
            waitCounter = Time.time + 2f; //Cooldown duration of 3 seconds

            return state;
        }

        state = NODE_STATE.RUNNING;
        return state;
    }
}
