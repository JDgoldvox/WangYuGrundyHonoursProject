using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Stats : MonoBehaviour
{
    public List<int> nums;
    [HideInInspector] public bool isCloned = false; // Flag to check if object is cloned

    void Start()
    {
        if (!isCloned)
        {
            nums.Add(0);

        }
    }
   

    [ContextMenu("Custom Context Button")]
    public void DebugStats()
    {
        foreach (int num in nums)
        {
            Debug.Log(num);
        }
    }
}
