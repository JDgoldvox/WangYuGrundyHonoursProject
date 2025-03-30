using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class NextGen : MonoBehaviour
{
    private GameObject prefab;
    public Transform parentF;

    public void next()
    {
        //get prefab to clone
        prefab = parentF.GetChild(parentF.childCount - 1).gameObject;

        //clone this game object
        GameObject d = Instantiate(prefab, parentF);
        //grab the new game object stats
        Stats s = d.GetComponent<Stats>();
        s.isCloned = true;

        //random num
        int random = Random.Range(50, 100);

        //grab original stats list
        List<int> newNums = new List<int>(prefab.GetComponent<Stats>().nums);

        //add random num to list
        newNums.Add(random);

        s.nums = newNums;
    }
}
