using BehaviourTreeWang;
using UnityEngine;

/// <summary>
/// Goes to target and stop at 2m
/// </summary>
public class TaskRunAwayFromClosestPerson : Node
{
    PersonBT personBT;
    private Transform btTransform;
    private float speed;
    private Animator animator;
    private Traits S_Traits;

    public TaskRunAwayFromClosestPerson(PersonBT bt)
    {
        nodeName = "TaskRunAwayFromClosestPerson";
        personBT = bt;
        btTransform = personBT.transform;
        speed = personBT.runSpeed;
        animator = personBT.animator;
        S_Traits = personBT.GetComponent<Traits>();
    }

    public override NODE_STATE Evaluate()
    {
        if (personBT.nearestPlayer == null)
        {
            return NODE_STATE.FAILURE;
        }

        Vector3 playerPosition = personBT.nearestPlayer.transform.position;
        Vector3 directionAway = (btTransform.position - playerPosition).normalized; 
        float distance = Vector3.Distance(playerPosition, btTransform.position);
        Vector3 positionToGoTo = btTransform.position + directionAway * (distance + 3);

        //subtract their position
        if (Vector3.Distance(positionToGoTo, btTransform.position) > 0.5f)
        {
            if (!animator.GetBool("isWalking"))
            {
                personBT.ResetAnimations();
                animator.SetBool("isWalking", true);
            }

            btTransform.position = Vector3.MoveTowards(
                btTransform.position,
                positionToGoTo,
                speed * Time.deltaTime
                );

            btTransform.LookAt(positionToGoTo);
            btTransform.eulerAngles = new Vector3(0, btTransform.eulerAngles.y, 0);
            S_Traits.DecreaseTrait(ref S_Traits.energy);
            S_Traits.IncreaseTrait(ref S_Traits.movement);
            S_Traits.IncreaseTrait(ref S_Traits.anger);
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
