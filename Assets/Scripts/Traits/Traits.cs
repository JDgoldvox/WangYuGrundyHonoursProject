using System;
using UnityEngine;

public class Traits : MonoBehaviour
{
    [Header("Dynamic traits")]
    public float happiness = 1f; //amount of happiness doing things / being annoyed / tiredness / loudness
    public float socialness = 1f; //talking requirement (0.5 is neutral)
    public float energy = 1f; //speed of which to do activities
    public float anger = 1f; //anger increases when people are being too loud or annoying

    [Header("Static traits")]
    public float attentionSpan = 1f; //speed multiplier for actions
    public float bandwagonEffect = 1f; //chance of joining a group action
    public float Evilness = 1f; //Chance of doing something annoying for other people
    public float shyness = 1f; //chance of joining somebody doing something and talking to them
    public float loudnessTolerance = 1f;

    [HideInInspector] public float LOW;
    [HideInInspector] public float MEDIUM;
    [HideInInspector] public float HIGH;

    private void Start()
    {
        LOW = 0.2f;
        MEDIUM = 0.5f;
        HIGH = 0.8f;

        //dynamic
        happiness = UnityEngine.Random.Range(0.0f, 1.0f);
        socialness = UnityEngine.Random.Range(0.0f, 0.2f);
        energy = UnityEngine.Random.Range(0.0f, 1.0f);
        anger = UnityEngine.Random.Range(0.0f, 1.0f);

        //static
        attentionSpan = UnityEngine.Random.Range(0.0f, 1.0f);
        bandwagonEffect = UnityEngine.Random.Range(0.0f, 1.0f);
        Evilness = UnityEngine.Random.Range(0.0f, 1.0f);
        shyness = UnityEngine.Random.Range(0.0f, 1.0f);
        loudnessTolerance = UnityEngine.Random.Range(0.0f, 1.0f);
    }

    private void Update()
    {
        DecreaseTrait(ref socialness);
        DecreaseTraitAtRate(ref anger, 0.01f);
    }

    public void DecreaseTrait(ref float trait)
    {
        trait -= 0.03f * Time.deltaTime;
        trait = Mathf.Clamp(trait, 0.0f, 1.0f);
    }
    public void IncreaseTrait(ref float trait)
    {
        trait += 0.03f * Time.deltaTime;
        trait = Mathf.Clamp(trait, 0.0f, 1.0f);
    }

    public void DecreaseTraitAtRate(ref float trait, float rate)
    {
        trait -= rate * Time.deltaTime;
        trait = Mathf.Clamp(trait, 0.0f, 1.0f);
    }

    public void IncreaseTraitAtRate(ref float trait, float rate)
    {
        trait += rate * Time.deltaTime;
        trait = Mathf.Clamp(trait, 0.0f, 1.0f);
    }

    public float ReturnFitnessFunction()
    {
        //1/anger

        float tempAnger = 0;

        if(anger == 0)
        {
            tempAnger = 0.01f;
        }
        else
        {
            tempAnger = anger;
        }

            return (happiness + socialness + 1 / tempAnger) / 3;
    }
}
