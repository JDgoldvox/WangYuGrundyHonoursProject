using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Calculations : MonoBehaviour
{
    [SerializeField] private Transform peopleParent;
    private List<float> averageMeanScoreHistory;
    private List<float> averageMedianScoreHistory;

    void Start()
    {
        averageMeanScoreHistory = new List<float>();
        averageMedianScoreHistory = new List<float>();
    }

    public void UpdateAverageScore()
    {
        List<float> allScores = new List<float>();
        float totalScore = 0;

        foreach (Transform person in peopleParent)
        {
            Traits traits = person.GetComponent<Traits>();

            float score = traits.ReturnFitnessFunction();
            allScores.Add(score);
            totalScore += score;
        }

        //calculate the average score
        averageMeanScoreHistory.Add(totalScore / peopleParent.childCount);

        allScores.Sort();
        averageMedianScoreHistory.Add(allScores[allScores.Count / 2]);
    }

    public void WriteScoresToFile()
    {
        var fileName = "SCORES/AVERAGE_SCORES.txt";

        StreamWriter sr;

        if (File.Exists(fileName))
        {
            sr = new StreamWriter(fileName);
        }
        else
        {
            sr = File.CreateText(fileName);
        }

        sr.WriteLine("Mean:");
        foreach (float s in averageMeanScoreHistory)
        {
            sr.WriteLine("{0}", s);
        }
        sr.WriteLine(" ");
        sr.WriteLine(" ");
        sr.WriteLine("Median:");
        foreach (float s in averageMedianScoreHistory)
        {
            sr.WriteLine("{0}", s);
        }
        
        sr.Close();
    }
}
