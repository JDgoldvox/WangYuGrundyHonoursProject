using BehaviourTreeWang;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private bool spawnInitialPopulation;
    [SerializeField] private GameObject personPrefab; 
    [SerializeField] private Transform PeopleParent;
    private List<Transform> peopleList;

    public Slider happinessSlider;
    public Slider sadnessSlider;
    public Slider angerSlider;
    public Slider movementSlider;
    public Slider socialnessSlider;
    public Slider populationSlider;
    public TMP_Text populationText;
    public TMP_Text fitnessText;
    public Slider fitnessThresholdToCull;
    public TMP_Text fitnessThresholdToCullText;


    [Range(0f, 1f)][SerializeField] private float probabilityOfFilter;
    [Range(0f, 1f)][SerializeField] private float probabilityOfCrossOver;
    private int populationSize = 1;

    private float maxX = 20;
    private float minX = -20;
    private float maxY = 2;
    private float maxZ = 20;
    private float minZ = -20;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        //Spawn set amount of population
        if(spawnInitialPopulation)
        {
            //Check how much population is in starting scene to find out amount of people to spawn
            int spawnAmount = populationSize - PeopleParent.childCount;

            for(int i = 0; i < spawnAmount; i++)
            {
                SpawnPrefabAtRandomLocation(personPrefab);

                Randomise();
            }
        }
    }

    private void Update()
    {
        populationText.text = "population: " + (int)populationSlider.value;
        populationSize = (int)populationSlider.value;


        //update avr fitness
        float fitness = 0;
        for(int i = 0; i < PeopleParent.childCount; i++)
        {
            fitness += PeopleParent.GetChild(i).GetComponent<Traits>().ReturnFitnessFunction();
        }

        fitness /= PeopleParent.childCount;
        fitnessText.text = "Average Fitness: " + fitness.ToString("F2");

        //update cull threshold
        fitnessThresholdToCullText.text = "fitness threshold: " + fitnessThresholdToCull.value;
    }

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

            List<Node> nodes = new List<Node>();
            nodes = personBT.root.children;

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
            ////Randomise threshold for a filter
            //if(Random.Range(0, 1) > probabilityOfFilter)
            //{
            //    continue;
            //}

            Traits traits = person.GetComponent<Traits>();

            if(traits.ReturnFitnessFunction() < fitnessThresholdToCull.value)
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
            if (Random.Range(0, 1) < probabilityOfCrossOver)
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

            //Duplicate these people
            foreach (var v in increaseList)
            {
                SpawnPrefabAtRandomLocation(v);
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
        List<Node> nodesA = new List<Node>(personA.root.children);

        PersonBT personB = peopleList[indexB].GetComponent<PersonBT>();
        List<Node> nodesB = new List<Node>(personA.root.children);

        int halfNodesA = nodesA.Count / 2;
        int halfNodesB = nodesB.Count / 2;

        List<Node> newNodes = new List<Node>();

        //add nodes A
        for (int i = 0; i < halfNodesA; i++)
        {
            Node newNodeA = new Node();
            newNodeA.children = new List<Node>(nodesA[i].children);
            newNodeA.nodeName = nodesA[i].nodeName;
            newNodes.Add(newNodeA);   
        }

        //add nodes B
        for (int i = 0; i < halfNodesB; i++)
        {
            Node newNodeB = new Node();
            newNodeB.children = new List<Node>(nodesB[i].children);
            newNodeB.nodeName = nodesB[i].nodeName;
            newNodes.Add(newNodeB);
        }

        return newNodes;
    }

    private void CreatePersonWithChildren(List<Node> newNodes)
    {
        GameObject p = SpawnPrefabAtRandomLocation(personPrefab);

        PersonBT script = p.GetComponent<PersonBT>();
        script.IsCloned = true;

        //create a selector node to set as root

        Node newRoot = new Selector();
        foreach(Node n in newNodes)
        {
            newRoot.Attach(n);
            newRoot.CloneInit(script);

            //set all children to have the same personBT
            n.CloneInit(script);

            //set all children's child to same personBT
            if(n.children.Count > 0)
            {
                foreach (Node child in n.children)
                {
                    child.CloneInit(script);
                }
            }
        }

        script.root = newRoot;
    }

    private GameObject SpawnPrefabAtRandomLocation(GameObject prefab)
    {
        float x = Random.Range(minX, maxX);
        float z = Random.Range(minZ, maxZ);
        Vector3 pos = new Vector3(x, maxY, z);

        GameObject p = Instantiate(prefab, pos, Quaternion.identity, PeopleParent);
        p.GetComponent<Traits>().ResetStats();

        return p;
    }
}
