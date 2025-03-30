using UnityEngine;
using BehaviourTreeWang;
using System.Collections.Generic;
public static class PreDeterminedTree
{
    public static Node ReturnTree(int treeNum, PersonBT bt)
    {
        switch (treeNum)
        {
            case 0:
                return Tree0(ref bt);
            case 1:
                return Tree1(ref bt);
            case 2:
                return Tree2(ref bt);
            case 3:
                return Tree3(ref bt);
            default:
                return Tree0(ref bt);
        }
    }

    //Very happy
    public static Node Tree0(ref PersonBT bt)
    {
        Node root = new Selector(new List<Node>
        {
            new TaskHappy(bt),

            new Sequence("Walk to target", new List<Node>()
            {
                new CheckTargetInRange(bt),
                new TaskGoToTarget(bt),
            }),

            new Sequence("Direct Interaction", new List<Node>
            {
                new CheckForceAttention(bt),
                new TaskLookAtForcedNPC(bt),
                new TaskTalkToForcedAttentionNPC(bt),
            }),


            new Sequence("Environment", new List<Node>
            {
                //find props
                new FindInteractablesNear(bt),
                new TaskWalkTowardInteractable(bt),
                new TaskPickGunUp(bt),
            }),

            //Socialness - talk to other people
            new Sequence("Socialness",new List<Node>()
            {
                new CheckSocialness(bt),
                new FindNearestPlayer(bt),
                new TaskGoToNearestPerson(bt),
                new SetForceAttentionToNPC(bt),
                new TaskTalk(bt),
            }),

            //Low Energy - meditate 
            new Sequence("Meditate",new List<Node>()
            {
                new CheckEnergy(bt),
                new TaskMeditate(bt),
            }),

            new TaskMeditate(bt),

            new TaskCry(bt),

            //Anger - run away
            new Sequence("anger", new List<Node>()
            {
                new CheckAnger(bt),
                new FindNearestPlayer(bt),
                new TaskRunAwayFromClosestPerson(bt),
            }),

            new TaskRandomWalk(bt),

            new TaskLaugh(bt),

            new Sequence("anger", new List<Node>()
            {
                new CheckAnger(bt),
                new FindNearestPlayer(bt),
                new TaskRunAwayFromClosestPerson(bt),
            }),

        });

        return root;
    }

    //Social
    public static Node Tree1(ref PersonBT bt)
    {
        Node root = new Selector(new List<Node>
        {

            new Sequence("Walk to target", new List<Node>()
            {
                new CheckTargetInRange(bt),
                new TaskGoToTarget(bt),
            }),

            new Sequence("Socialness",new List<Node>()
            {
                new CheckSocialness(bt),
                new FindNearestPlayer(bt),
                new TaskGoToNearestPerson(bt),
                new SetForceAttentionToNPC(bt),
                new TaskTalk(bt),
            }),

            new TaskHappy(bt),

            new TaskCry(bt),

            new TaskLaugh(bt),

            new Sequence("Meditate",new List<Node>()
            {
                new CheckEnergy(bt),
                new TaskMeditate(bt),
            }),

            new Sequence("Environment", new List<Node>
            {
                //find props
                new FindInteractablesNear(bt),
                new TaskWalkTowardInteractable(bt),
                new TaskPickGunUp(bt),
            }),
            
            new TaskMeditate(bt),

             new Sequence("Direct Interaction", new List<Node>
            {
                new CheckForceAttention(bt),
                new TaskLookAtForcedNPC(bt),
                new TaskTalkToForcedAttentionNPC(bt),
            }),

            new Sequence("anger", new List<Node>()
            {
                new CheckAnger(bt),
                new FindNearestPlayer(bt),
                new TaskRunAwayFromClosestPerson(bt),
            }),

            new TaskRandomWalk(bt),

        });

        return root;
    }

    //Angery and sad
    public static Node Tree2(ref PersonBT bt)
    {
        Node root = new Selector(new List<Node>
        {
            new TaskCry(bt),

            new Sequence("Walk to target", new List<Node>()
            {
                new CheckTargetInRange(bt),
                new TaskGoToTarget(bt),
            }),

             new Sequence("Meditate",new List<Node>()
            {
                new CheckEnergy(bt),
                new TaskMeditate(bt),
            }),

            new Sequence("Direct Interaction", new List<Node>
            {
                new CheckForceAttention(bt),
                new TaskLookAtForcedNPC(bt),
                new TaskTalkToForcedAttentionNPC(bt),
            }),

            new Sequence("Socialness",new List<Node>()
            {
                new CheckSocialness(bt),
                new FindNearestPlayer(bt),
                new TaskGoToNearestPerson(bt),
                new SetForceAttentionToNPC(bt),
                new TaskTalk(bt),
            }),

            new TaskHappy(bt),

            new TaskLaugh(bt),

            new Sequence("Environment", new List<Node>
            {
                //find props
                new FindInteractablesNear(bt),
                new TaskWalkTowardInteractable(bt),
                new TaskPickGunUp(bt),
            }),

            new TaskMeditate(bt),
             
            new Sequence("anger", new List<Node>()
            {
                new CheckAnger(bt),
                new FindNearestPlayer(bt),
                new TaskRunAwayFromClosestPerson(bt),
            }),

            new TaskRandomWalk(bt),

        });

        return root;
    }

    //random
    public static Node Tree3(ref PersonBT bt)
    {
        Node root = new Selector(new List<Node>
        {
            new TaskLaugh(bt),

            new Sequence("Environment", new List<Node>
            {
                //find props
                new FindInteractablesNear(bt),
                new TaskWalkTowardInteractable(bt),
                new TaskPickGunUp(bt),
            }),

            new TaskRandomWalk(bt),

            new Sequence("Direct Interaction", new List<Node>
            {
                new CheckForceAttention(bt),
                new TaskLookAtForcedNPC(bt),
                new TaskTalkToForcedAttentionNPC(bt),
            }),

             new Sequence("Meditate",new List<Node>()
            {
                new CheckEnergy(bt),
                new TaskMeditate(bt),
            }),

            new TaskCry(bt),

             new Sequence("Walk to target", new List<Node>()
            {
                new CheckTargetInRange(bt),
                new TaskGoToTarget(bt),
            }),

            new TaskMeditate(bt),

            new TaskHappy(bt),

            new Sequence("Socialness",new List<Node>()
            {
                new CheckSocialness(bt),
                new FindNearestPlayer(bt),
                new TaskGoToNearestPerson(bt),
                new SetForceAttentionToNPC(bt),
                new TaskTalk(bt),
            }),


            new Sequence("anger", new List<Node>()
            {
                new CheckAnger(bt),
                new FindNearestPlayer(bt),
                new TaskRunAwayFromClosestPerson(bt),
            }),

        });

        return root;
    }

}