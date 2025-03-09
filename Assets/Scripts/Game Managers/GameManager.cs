using BehaviourTree;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject personPrefab; 
    [SerializeField] private Transform PeopleParent;
    private List<Transform> peopleList;

    [Range(0f, 1f)][SerializeField] private float probabilityOfFilter;
    [Range(0f, 1f)][SerializeField] private float probabilityOfCrossOver;
    [Range(0f, 1f)][SerializeField] private float fitnessThresholdToCull;
    [Range(0, 200)][SerializeField] private int populationSize;

    private float maxX = 100;
    private float minX = -100;
    private float maxY = 2;
    private float maxZ = 100;
    private float minZ = -100;

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
        //prep
        SetPeopleList();
        FilterList();

        //Cross overs (split the tree down the middle)
        CrossoverList();

        //Mutations

        //Prune List or multiply depending on required population size
        PruneOrMultiplyList();
    }

    private void SetPeopleList()
    {
        peopleList = new List<Transform>();

        foreach (Transform t in PeopleParent)
        {
            peopleList.Add(t);
        }
    }
    
    //filters the list by traits
    private void FilterList()
    {
        List<Transform> newPeopleList = new List<Transform>();
        List<Transform> deleteList = new List<Transform>();

        if (peopleList.Count == 0)
        {
            Debug.LogError("List of people is empty - cannot filter");
            return;
        }

        foreach (Transform person in peopleList)
        {
            //Randomise threshold for a filter
            if(Random.Range(0, 1) > probabilityOfFilter)
            {
                continue;
            }

            Traits traits = person.GetComponent<Traits>();

            if(traits.ReturnFitnessFunction() < fitnessThresholdToCull)
            {
                deleteList.Add(person);
            }
            else
            {
                newPeopleList.Add(person);
            }
        }

        //clean up
        foreach (Transform person in deleteList)
        {
            Destroy(person.gameObject);
        }

        //set new filtered list
        peopleList = newPeopleList;

    }

    private void CrossoverList()
    {
        int peopleCount = peopleList.Count;

        //cross over every pair of people
        for (int i = 0; i < peopleCount; i += 2)
        {
            //Randomise threshold for a crossover
            if (Random.Range(0, 1) > probabilityOfCrossOver)
            {
                continue;
            }

            List<Node> newNodes = new List<Node>();

            if (i + 1 < peopleCount)
            {
                newNodes = Crossover(i, i + 1);
            }
            else //last person cross over with first person
            {
                newNodes = Crossover(i, 0);
            }

            //Create new person
            CreatePersonWithChildren(newNodes);
        }
    }

    private void PruneOrMultiplyList()
    {
        //prune
        if(PeopleParent.childCount > populationSize) 
        {
            List<GameObject> removeList = new List<GameObject>();

            //remove children randomly
            int populationToRemove = PeopleParent.childCount - populationSize;

            for (int i = 0; i < populationToRemove; i++)
            {
                int randomChildNum = Random.Range(0, PeopleParent.childCount);
                removeList.Add(PeopleParent.GetChild(randomChildNum).gameObject);
            }

            //delete these people
            foreach(var v in removeList)
            {
                Destroy(v);
            }
        }
        else if(PeopleParent.childCount < populationSize) //mulitiply
        {
            List<GameObject> increaseList = new List<GameObject>();

            //increase children randomly
            int populationToMake = populationSize - PeopleParent.childCount;

            for (int i = 0; i < populationToMake; i++)
            {
                int randomChildNum = Random.Range(0, PeopleParent.childCount);
                increaseList.Add(PeopleParent.GetChild(randomChildNum).gameObject);
            }

            //delete these people
            foreach (var v in increaseList)
            {
                float x = Random.Range(minX, maxX);
                float z = Random.Range(minZ, maxZ);

                Vector3 pos = new Vector3(x, maxY, z);

                Instantiate(v, pos, Quaternion.identity, PeopleParent);
            }
        }
    }
    
    /// <summary>
    /// Crosses over with 2 people from indexes in people list
    /// </summary>
    /// <param name="A"></param>
    /// <param name="indexB"></param>
    /// <returns></returns>
    private List<Node> Crossover(int indexA, int indexB)
    {
        PersonBT personA = peopleList[indexA].GetComponent<PersonBT>();
        List<Node> nodesA = personA.root.children;

        PersonBT personB = peopleList[indexB].GetComponent<PersonBT>();
        List<Node> nodesB = personA.root.children;


        int halfNodesA = nodesA.Count / 2;
        int halfNodesB = nodesB.Count / 2;

        List<Node> newNodes = new List<Node>();

        //add nodes A
        for (int i = 0; i < halfNodesA; i++)
        {
            newNodes.Add(nodesA[i]);
        }

        //add nodes B
        for (int i = 0; i < halfNodesB; i++)
        {
            newNodes.Add(nodesB[i]);
        }

        return newNodes;
    }

    private void CreatePersonWithChildren(List<Node> newNodes)
    {
        float x = Random.Range(minX, maxX);
        float z = Random.Range(minZ, maxZ);

        Vector3 pos = new Vector3(x, maxY, z);

        GameObject p = Instantiate(personPrefab, pos, Quaternion.identity, PeopleParent);

        PersonBT script = p.GetComponent<PersonBT>();

        // Ensure the tree is initialized
        if (script.root == null)
        {
            Debug.LogWarning("Root was null, forcing initialization.");
            script.root = script.InitTree();
        }

        if (script.root == null)
        {
            Debug.LogError("Root is still null after initialization!");
            return;
        }

        script.root.children = newNodes;
    }
}
