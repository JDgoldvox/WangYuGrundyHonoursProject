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
        SetPeopleList();
        PruneOrMultiplyList();

        //reset all traits
        ResetAllTraits();
    }

    private void ResetAllTraits()
    {
        foreach (Transform person in peopleList)
        {
            Traits traits = person.GetComponent<Traits>();
            traits.ResetStats();
        }
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
            //remove children randomly
            int populationToRemove = PeopleParent.childCount - populationSize;

            //Sort people list by ascending order
            peopleList.Sort((a, b) => a.transform.GetComponent<Traits>().ReturnFitnessFunction().CompareTo(b.transform.GetComponent<Traits>().ReturnFitnessFunction()));

            for(int i = 0; i < populationToRemove; i++)
            {
                Destroy(peopleList[i].gameObject);
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
                PersonBT person = v.GetComponent<PersonBT>();
                List<Node> personOriginalNodes = new List<Node>(person.root.children);
                List<Node> newNodes = new List<Node>();

                for (int i = 0; i < personOriginalNodes.Count; i++)
                {
                    Node cloned = DeepCloneNode(personOriginalNodes[i], null);
                    newNodes.Add(cloned);
                }

                CreatePersonWithChildren(newNodes);
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
        List<Node> nodesB = new List<Node>(personB.root.children);

        int halfNodesA = nodesA.Count / 2;
        int halfNodesB = nodesB.Count / 2;

        List<Node> newNodes = new List<Node>();

        //add nodes A
        for (int i = 0; i < halfNodesA; i++)
        {
            Node cloned = DeepCloneNode(nodesA[i], null); 
            newNodes.Add(cloned);
        }

        //add nodes B
        for (int i = 0; i < halfNodesB; i++)
        {
            Node cloned = DeepCloneNode(nodesB[i], null); 
            newNodes.Add(cloned);
        }

        return newNodes;
    }

    private void CreatePersonWithChildren(List<Node> newNodes)
    {
        GameObject newPerson = SpawnPrefabAtRandomLocation(personPrefab);

        PersonBT newPersonBT = newPerson.GetComponent<PersonBT>();
        newPersonBT.IsCloned = true;

        //create a selector node to set as root
        Node newRoot = new Selector();
        newRoot.CloneInit(newPersonBT);

        //set root
        newPersonBT.root = newRoot;

        //Update them
        foreach (Node n in newNodes)
        {
            UpdateCloneInitRecursively(n, newPersonBT); 
            newRoot.Attach(n);
        }
    }

    private GameObject SpawnPrefabAtRandomLocation(GameObject prefab)
    {
        float x = Random.Range(minX, maxX);
        float z = Random.Range(minZ, maxZ);
        Vector3 pos = new Vector3(x, maxY, z);

        GameObject p = Instantiate(prefab, pos, Quaternion.identity, PeopleParent);
        p.GetComponent<Traits>().ResetStats();

        //PersonBT script = p.GetComponent<PersonBT>();
        //foreach (Node n in script.root.children)
        //{
        //    UpdateCloneInitRecursively(n, script);
        //}

        return p;
    }

    private Node DeepCloneNode(Node original, PersonBT bt)
    {
        if (original == null) return null;

        Node clone = original.Clone();
        clone.nodeName = original.nodeName;

        clone.children = new List<Node>();
        foreach (Node child in original.children)
        {
            Node clonedChild = DeepCloneNode(child, bt); 
            clone.Attach(clonedChild);
        }

        return clone;
    }

    private void UpdateCloneInitRecursively(Node node, PersonBT bt)
    {
        node.CloneInit(bt);
        foreach (Node child in node.children)
        {
            UpdateCloneInitRecursively(child, bt);
        }
    }
}
