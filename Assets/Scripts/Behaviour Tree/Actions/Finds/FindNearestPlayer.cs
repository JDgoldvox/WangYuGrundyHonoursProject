using BehaviourTree;
using System.Collections.Generic;
using UnityEngine;

public class FindNearestPlayer : Node
{
    private PersonBT personBT;
    private Transform originTransform;
    private int peopleLayerMask = LayerMask.GetMask("People");
    private float timer = 0;
    private float cooldown = 0.5f;

    public FindNearestPlayer(PersonBT bt)
    {
        nodeName = "FindNearestPlayer";
        personBT = bt;
        originTransform = personBT.gameObject.transform;
    }

    public override NODE_STATE Evaluate()
    {
        //Create a cool down

        //if timer not reached, do nothing, as we already have a nearest player
        if (Time.time >= timer || personBT.nearestPlayer == null)
        {
            timer = Time.time + cooldown;
        }
        else
        {
            if(personBT.nearestPlayer != null)
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

        personBT.nearestPlayer = null;

        Collider[] colliders = Physics.OverlapSphere(
            originTransform.position,
            personBT.visionRange,
            peopleLayerMask
            );

        //DO NOT COUNT ITSELF, SO MORE THAN 1 OBJ
        if (colliders.Length > 1)
        {
            List<Transform> otherTargets = new List<Transform>();
            float currentClosestDistance = float.MaxValue;
            PersonBT closestPerson = null;

            foreach (Collider collider in colliders)
            {
                if (collider.gameObject != originTransform.gameObject)
                {
                    float dist = (collider.transform.position - originTransform.position).sqrMagnitude;

                    if(dist < currentClosestDistance)
                    {
                        currentClosestDistance = dist;
                        closestPerson = collider.transform.GetComponent<PersonBT>();
                    }
                }
            }

            personBT.nearestPlayer = closestPerson;

            state = NODE_STATE.SUCCESS;
            return state;
        }

        //if 0 objects found
        personBT.nearestPlayer = null;
        state = NODE_STATE.FAILURE;
        return state;
    }
}
