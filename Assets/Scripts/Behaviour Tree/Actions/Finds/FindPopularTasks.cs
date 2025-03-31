using UnityEngine;
using BehaviourTreeWang;
using System.Collections.Generic;
using Unity.Mathematics;

public class FindPopularTasks : Node
{
    public FindPopularTasks(PersonBT bt)
    {
        nodeName = "FindPopularTasks";
        personBT = bt;
    }

    public override void CloneInit(PersonBT bt)
    {
        nodeName = "FindPopularTasks";
        personBT = bt;
    }
    public override NODE_STATE Evaluate()
    {
        //get reference to list of poeple
        List<Transform> people = personBT.peopleNear;
        Dictionary<Tasks, int> tasks = new Dictionary<Tasks, int>();

        //go through list of people
        for (int i = 0; i < people.Count; i++) 
        {
            //add what they are currently doing
            Tasks task = people[i].GetComponent<PersonBT>().currentTask;

            if (tasks.TryGetValue(task, out int count))
            {
                if(task != Tasks.None)
                {
                    continue;
                }

                tasks[task] = count + 1;
            }
            else
            {
                tasks.Add(task, 1);
            }
        }

        Tasks mostPopularTask = Tasks.None;
        int largestCount = 0;

        foreach(var kvp in tasks)
        {
            if(kvp.Value > largestCount)
            {
                largestCount = kvp.Value;
                mostPopularTask = kvp.Key;
            }
        }

        if(largestCount != 0)
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
}
