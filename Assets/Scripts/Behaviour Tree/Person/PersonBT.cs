using BehaviourTree;
using System.Collections.Generic;
using UnityEngine;
public class PersonBT : BehaviourTree.Tree
{
    public UnityEngine.Transform[] wayPoints;
    public UnityEngine.Animator animator;
    public PersonBT nearestPlayer = null;
    public Transform forcedAttentionToPlayer = null;

    public float walkSpeed;
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
            //TRIGGERS (ACTIONS THAT MUST TAKE PRIORITY) ----------------------------
            
            //Direct interaction 
            new Sequence(new List<Node>
            {
                new CheckForceAttention(this),
                new TaskLookAtForcedNPC(this),
                new TaskTalkToForcedAttentionNPC(this),
            }),

            //Environment (ACTIONS BASED ON CURRENT ENVIRONMENT
            //x
            //y
            //z

            //Traits (ACTIONS BASED ON MUST NEED TO DO TO SURVIVE)

            //Socialness - talk to other people
            new Sequence(new List<Node>()
            {
                new CheckSocialness(this),
                new FindNearestPlayer(this),
                new TaskGoToNearestPerson(this),
                new SetForceAttentionToNPC(this),
                new TaskTalk(this),
            }),

            //Low Energy - meditate
            new Sequence(new List<Node>()
            {
                new CheckEnergy(this),
                new FindNearestPlayer(this),
                new TaskRunAwayFromClosestPerson(this),
                new TaskMeditate(this),
            }),

            //Anger - run away
            new Sequence(new List<Node>()
            {
                new CheckAnger(this),
                new FindNearestPlayer(this),
                new TaskRunAwayFromClosestPerson(this),
            }),

            //Other people (ACTIONS BASED ON OTHER PEOPLE)
            






            //new TaskHappy(this),
            //new TaskMeditate(this),
            //new TaskRandomWalk(this),
            //new TaskCry(this),
            //new TaskLaugh(this),



            //new Sequence(new List<Node>()
            //{
            //    new CheckTargetInRange(this),
            //    new TaskGoToTarget(this),
            //}),
        }); return root;

    }

    public void ResetAnimations()
    {
        foreach (AnimatorControllerParameter parameter in animator.parameters)
        {
            animator.SetBool(parameter.name, false);
        }
    }
}
