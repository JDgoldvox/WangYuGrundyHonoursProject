using UnityEngine;
using BehaviourTree;
using System.Collections.Generic;

public class FindInteractablesNear : Node
{
    private PersonBT personBT;
    private Transform originTransform;
    private int interactableLayerMask = LayerMask.GetMask("interactable");
    private float timer = float.MaxValue;
    private float cooldown = 0.5f;

    public FindInteractablesNear(PersonBT bt)
    {
        personBT = bt;
        originTransform = personBT.gameObject.transform;
    }

    public override NODE_STATE Evaluate()
    {
        ////Create a cool down
        //if (personBT.interactablesNear != null)
        //{
        //    //if timer not reached, do nothing, as we already have a nearest player
        //    if (timer >= Time.time)
        //    {
        //        state = NODE_STATE.SUCCESS;
        //        return state;
        //    }
        //}

        Collider[] colliders = Physics.OverlapSphere(
            originTransform.position,
            personBT.visionRange,
            interactableLayerMask
            );

        //DO NOT COUNT ITSELF, SO MORE THAN 1 OBJ
        if (colliders.Length > 0)
        {
            List<Transform> otherTargets = new List<Transform>();

            foreach (Collider collider in colliders)
            {
                if (collider.gameObject != originTransform.gameObject)
                {
                    personBT.interactablesNear.Add(collider.transform);
                }
            }

            //update timer
            timer = Time.time + cooldown;

            state = NODE_STATE.SUCCESS;
            return state;
        }

        //if 0 objects found
        personBT.interactablesNear = null;
        state = NODE_STATE.FAILURE;
        return state;
    }
}
