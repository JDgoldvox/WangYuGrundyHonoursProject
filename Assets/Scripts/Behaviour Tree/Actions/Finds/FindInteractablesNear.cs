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
        nodeName = "FindInteractablesNear";
        personBT = bt;
        originTransform = personBT.gameObject.transform;
    }

    public override NODE_STATE Evaluate()
    {

        Collider[] colliders = Physics.OverlapSphere(
            originTransform.position,
            personBT.visionRange,
            interactableLayerMask
            );

        //DO NOT COUNT ITSELF, SO MORE THAN 1 OBJ
        if (colliders.Length > 0)
        {
            List<Transform> otherTargets = new List<Transform>();

            if (personBT.interactablesNear == null)
            {
                personBT.interactablesNear = new List<Transform>();
            }
            else
            {
                personBT.interactablesNear.Clear(); 
            }

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
