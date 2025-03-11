using BehaviourTree;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class FindPeopleNear : Node
{
    private PersonBT personBT;
    private Transform originTransform;
    private int peopleLayerMask = LayerMask.GetMask("People");
    private float timer = float.MaxValue;
    private float cooldown = 1f;
    public FindPeopleNear(PersonBT bt)
    {
        nodeName = "FindPeopleNear";
        personBT = bt;
        originTransform = personBT.gameObject.transform;
    }

    public override NODE_STATE Evaluate()
    {
        //Create a cool down
        if (personBT.nearestPlayer != null)
        {
            //if timer not reached, do nothing, as we already have a nearest player
            if (timer >= Time.time)
            {
                state = NODE_STATE.SUCCESS;
                return state;
            }
        }

        Collider[] colliders = Physics.OverlapSphere(
            originTransform.position,
            personBT.visionRange,
            peopleLayerMask
            );

        //DO NOT COUNT ITSELF, SO MORE THAN 1 OBJ
        if (colliders.Length > 1)
        {
            List<Transform> otherTargets = new List<Transform>();

            foreach (Collider collider in colliders)
            {
                if (collider.gameObject != originTransform.gameObject)
                {
                    otherTargets.Add(collider.transform);
                }
            }

            personBT.peopleNear = otherTargets;

            //update timer
            timer = Time.time + cooldown;

            state = NODE_STATE.SUCCESS;
            return state;
        }

        //if 0 objects found
        personBT.nearestPlayer = null;
        state = NODE_STATE.FAILURE;
        return state;
    }

}