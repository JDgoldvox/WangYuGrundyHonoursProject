using UnityEngine;
using BehaviourTree;
using System.Collections.Generic;

public class CheckTargetInRange : Node
{
    private Transform originPosition;
    private int peopleLayerMask = LayerMask.GetMask("People");
    private Animator animator;
    float visionRange;
    public CheckTargetInRange(Transform transformIn, Animator animatorIn, float visionRangeIn)
    {
        originPosition = transformIn;
        animator = animatorIn;
        visionRange = visionRangeIn;
    }

    public override NODE_STATE Evaluate()
    {
       // Debug.Log("checking targets in range");

        object t = GetData("targets");

        if (t == null)
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

                state = NODE_STATE.SUCCESS;
                return state;
            }

            //Debug.Log("target fail");

            //if 0 objects found
            state = NODE_STATE.FAILURE;
            return state;
        }

        // theres already targets?
        state = NODE_STATE.SUCCESS;
        return state;
    }
}
