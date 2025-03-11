using BehaviourTree;
using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InfoPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisplayPersonInfo(GameObject person)
    {
        //Traits
        Traits traits = person.GetComponent<Traits>();

        string newText = "";
        newText += "Happiness: " + traits.happiness.ToString() + "\n";
        newText += "socialness: " + traits.socialness.ToString() + "\n";
        newText += "energy: " + traits.energy.ToString() + "\n";
        newText += "anger: " + traits.anger.ToString() + "\n";
        newText += "movement: " + traits.movement.ToString() + "\n";
        newText += "sadness: " + traits.sadness.ToString() + "\n";

        //Nodes
        newText += "\n\n\n\n";
        PersonBT personBT = person.GetComponent<PersonBT>();
        newText += personBT.GetNodesAsString();

        //set text
        text.text = newText;
    }
}
