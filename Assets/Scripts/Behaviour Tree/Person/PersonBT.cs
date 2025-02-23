using BehaviourTree;
using System.Collections.Generic;
using UnityEngine;

public enum Tasks{
    None,
    Happy,
    Dance,
    Walk,
    Talk,
    Meditate
}
public class PersonBT : BehaviourTree.Tree
{
    public UnityEngine.Animator animator;

    public PersonBT nearestPlayer = null;
    public Transform forcedAttentionToPlayer = null;
    public List<Transform> peopleNear = null;
    public List<Transform> interactablesNear = null;

    public Tasks popularTask = Tasks.None;
    public Tasks currentTask = Tasks.None;

    public float walkSpeed;
    public float visionRange;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    protected override Node InitTree()
    {
        peopleNear = new List<Transform>();
        interactablesNear = new List<Transform>();

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

            //Environment (ACTIONS BASED ON CURRENT ENVIRONMENT  --------------------------------------------
            new Sequence(new List<Node>
            {
                //find props
                new FindInteractablesNear(this),
                new TaskWalkTowardInteractable(this),
                new TaskPickGunUp(this),
            }),


            //Traits (ACTIONS BASED ON MUST NEED TO DO TO SURVIVE)  --------------------------------------------

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

            //Other people (ACTIONS BASED ON OTHER PEOPLE) --------------------------------------------
            //new Sequence(new List<Node>()
            //{
            //    new FindPeopleNear(this),
            //    new FindPopularTasks(this),
            //    //new TaskDoMostPopularTask(this), //How do I do this without copying code?
            //}),

            //Random  --------------------------------------------

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
            new TaskRandomWalk(this),
        }); 
        
        return root;

    }

    public void ResetAnimations()
    {
        foreach (AnimatorControllerParameter parameter in animator.parameters)
        {
            animator.SetBool(parameter.name, false);
        }
    }
}
