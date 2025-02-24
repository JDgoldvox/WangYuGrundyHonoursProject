using BehaviourTree;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class TreeRandomiser : MonoBehaviour
{
    [SerializeField] private Transform PeopleParent;
    private List<Transform> peopleList;

    /// <summary>
    /// Randomises the trees intially
    /// </summary>
    public void Randomise()
    {
        //Gather people
        SetPeopleList();

        //Randomise the tree
        foreach(Transform person in peopleList)
        {
            PersonBT personBT = person.GetComponent<PersonBT>();

            List<Node> nodes = personBT.root.children;
            int maxChildren = personBT.root.children.Count;

            for (int i = 0; i < maxChildren; i++)
            {
                //find a suitable node to swap with
                int randomNum = Random.Range(0, maxChildren);

                //swap with that node
                Node tempMode = nodes[randomNum];

                //swap nodes
                nodes[randomNum] = nodes[i];
                nodes[i] = tempMode;
            }
        }
    }

    public void NextGeneration()
    {
        //Cross overs (split the tree down the middle)



        //Mutations


    }

    private void SetPeopleList()
    {
        peopleList = new List<Transform>();

        foreach (Transform t in PeopleParent)
        {
            peopleList.Add(t);
        }
    }

}
