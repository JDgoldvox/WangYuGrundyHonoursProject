using BehaviourTree;
using System.Collections.Generic;
using UnityEngine;
public class PersonBT : BehaviourTree.Tree
{
    public UnityEngine.Transform[] wayPoints;
    public UnityEngine.Animator animator;
    
    public float speed;
    public float visionRange;
    protected override Node InitTree()
    {
        Node root = new Selector(new List<Node>
        {
            new Sequence(new List<Node>()
            {
                new CheckTargetInRange(transform, animator, visionRange),
                new GoToTarget(transform, speed)
            }),

            new Patrol(transform,wayPoints, animator, speed)
        });

        return root;
    }
}
