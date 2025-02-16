using BehaviourTree;
using System.Collections.Generic;
using UnityEngine;

public class FindNearestPlayer : Node
{
    private PersonBT personBT;
    private Transform originTransform;
    private int peopleLayerMask = LayerMask.GetMask("People");
    private float timer = float.MaxValue;
    private float cooldown = 0.5f;

    public FindNearestPlayer(PersonBT bt)
    {
        personBT = bt;
        originTransform = personBT.gameObject.transform;
    }

    public override NODE_STATE Evaluate()
    {
        //Create a cool down
        if(personBT.nearestPlayer != null)
        {
            //if timer not reached, do nothing, as we already have a nearest player
            if(timer >= Time.time)
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
