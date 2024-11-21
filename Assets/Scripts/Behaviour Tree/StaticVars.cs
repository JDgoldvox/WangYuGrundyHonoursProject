using UnityEngine;
using System;

public class StaticVars : MonoBehaviour
{
    public static System.Random rng;

    private void Awake()
    {
        rng = new System.Random();
    }
}
