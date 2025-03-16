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
    public float runSpeed;
    public float visionRange;

    public int preDeterminedTree = -1;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public override Node InitTree()
    {
        peopleNear = new List<Transform>();
        interactablesNear = new List<Transform>();

        if(preDeterminedTree != -1)
        {
            Node d = PreDeterminedTree.ReturnTree(preDeterminedTree, this);
            return d;
        }

        Node root = new Selector(new List<Node>
        {
            //TRIGGERS (ACTIONS THAT MUST TAKE PRIORITY) ----------------------------
            
            //Direct interaction 
            new Sequence("Direct Interaction", new List<Node>
            {
                new CheckForceAttention(this),
                new TaskLookAtForcedNPC(this),
                new TaskTalkToForcedAttentionNPC(this),
            }),

            //Environment (ACTIONS BASED ON CURRENT ENVIRONMENT  --------------------------------------------
            new Sequence("Environment", new List<Node>
            {
                //find props
                new FindInteractablesNear(this),
                new TaskWalkTowardInteractable(this),
                new TaskPickGunUp(this),
            }),

            //Traits (ACTIONS BASED ON MUST NEED TO DO TO SURVIVE)  --------------------------------------------

            //Low Energy - meditate 
            new Sequence("Meditate",new List<Node>()
            {
                new CheckEnergy(this),
                new TaskMeditate(this),
            }),

            //Socialness - talk to other people
            new Sequence("Socialness",new List<Node>()
            {
                new CheckSocialness(this),
                new FindNearestPlayer(this),
                new TaskGoToNearestPerson(this),
                new SetForceAttentionToNPC(this),
                new TaskTalk(this),
            }),

            //Anger - run away
            new Sequence("anger", new List<Node>()
            {
                new CheckAnger(this),
                new FindNearestPlayer(this),
                new TaskRunAwayFromClosestPerson(this),
            }),

            ////Other people (ACTIONS BASED ON OTHER PEOPLE) --------------------------------------------
            //new Sequence("Actions based on other people", new List<Node>()
            //{
            //    new FindPeopleNear(this),
            //    new FindPopularTasks(this),
            //    //new TaskDoMostPopularTask(this), //How do I do this without copying code?
            //}),

            //Random  --------------------------------------------

            new TaskHappy(this),
            new TaskMeditate(this),
            new TaskCry(this),
            new TaskLaugh(this),

            new Sequence("Walk to target", new List<Node>()
            {
                new CheckTargetInRange(this),
                new TaskGoToTarget(this),
            }),

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

    public string GetNodesAsString()
    {
        string s = "";

        //add children node names;
        foreach (var child in root.children)
        {
            string childrenAsString = child.nodeName + "\n";

            //add children's children node names
            if (child.children.Count != 0)
            {
                foreach (var c in child.children)
                {
                    childrenAsString += "       " + c.nodeName + "\n";
                }
            }

            s += childrenAsString + "\n";
        }

        return s;
    }
}
