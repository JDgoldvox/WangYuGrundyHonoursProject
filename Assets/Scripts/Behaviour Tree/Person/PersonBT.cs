using BehaviourTree;
using System.Collections.Generic;
using UnityEngine;
public class PersonBT : BehaviourTree.Tree
{
    public UnityEngine.Transform[] wayPoints;
    public UnityEngine.Animator animator;
    
    public float speed;
    public float visionRange;

    private Transform modelTransform;

    protected override Node InitTree()
    {
        modelTransform = transform.GetChild(0).transform;

        Node root = new Selector(new List<Node>
        {
            //new Sequence(new List<Node>()
            //{
            //    new CheckTargetInRange(transform, animator, visionRange),
            //    new TaskGoToTarget(transform, speed)
            //}),

            //new TaskPatrol(transform,wayPoints, animator, speed),

            new TaskRandomWalk(transform, animator, speed),
        });

        return root;
    }
}
