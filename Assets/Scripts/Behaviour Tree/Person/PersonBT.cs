using BehaviourTree;
using System.Collections.Generic;
using UnityEngine;
public class PersonBT : BehaviourTree.Tree
{
    public UnityEngine.Transform[] wayPoints;
    public UnityEngine.Animator animator;
    
    public float speed;
    public float visionRange;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    //new TaskPatrol(transform,wayPoints, animator, speed),
    protected override Node InitTree()
    {
        Node root = new Selector(new List<Node>
        {
            new Sequence(new List<Node>()
            {
                new CheckTargetInRange(transform, animator, visionRange),
                new TaskGoToTarget(transform, speed, animator)
            }),
            new TaskMeditate(transform, animator),
            new TaskRandomWalk(this, animator, speed),
        });

        return root;
    }
}
