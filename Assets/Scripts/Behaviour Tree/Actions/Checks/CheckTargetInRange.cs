using UnityEngine;
using BehaviourTree;
using System.Collections.Generic;

public class CheckTargetInRange : Node
{
    private PersonBT personBT;
    private Transform originPosition;
    private int peopleLayerMask = LayerMask.GetMask("People");
    private Animator animator;
    float visionRange;
    float cooldown = 2;
    float timer = float.MaxValue;
    public CheckTargetInRange(PersonBT bt)
    {
        personBT = bt;
        originPosition = personBT.transform;
        animator = personBT.animator;
        visionRange = personBT.visionRange;
    }

    public override NODE_STATE Evaluate()
    {
        object t = GetData("targets");

        if (Time.time < timer)
        {
            Collider[] colliders = Physics.OverlapSphere(
                originPosition.position,
                visionRange,
                peopleLayerMask
                );

            //DO NOT COUNT ITSELF, SO MORE THAN 1 OBJ
            if (colliders.Length > 1)
            {
                //Debug.Log("found more than 1 object");
                
                List<Transform> otherTargets = new List <Transform>();

                foreach (Collider collider in colliders)
                {
                    //Debug.Log($"{collider.gameObject.name}");
                    if(collider.gameObject != originPosition.gameObject)
                    {
                        otherTargets.Add(collider.transform);
                    }
                }

                parent.parent.SetData("targets", otherTargets);
                animator.SetBool("isWalking", true);


                timer = Time.time + cooldown;

                state = NODE_STATE.SUCCESS;
                return state;
            }

            //if 0 objects found
            state = NODE_STATE.FAILURE;
            return state;
        }


        // theres already targets?
        if(t != null)
        {
            state = NODE_STATE.SUCCESS;
            return state;
        }
        else
        {
            state = NODE_STATE.FAILURE;
            return state;
        }
    }
}
