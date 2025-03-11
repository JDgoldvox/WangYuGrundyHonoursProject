using System;
using UnityEngine;

public class Traits : MonoBehaviour
{
    [Header("Dynamic traits")]
    public float happiness; //Actions that make you happy
    public float socialness; //talking requirement (0.5 is neutral)
    public float energy; //speed of which to do activities
    public float anger; //anger increases when people are being too loud or annoying
    public float sadness; //Actions that make you sad
    public float movement; //any physical translational movement

    [Header("Static traits")]
    public float attentionSpan = 1f; //speed multiplier for actions
    public float bandwagonEffect = 1f; //chance of joining a group action
    public float Evilness = 1f; //Chance of doing something annoying for other people
    public float shyness = 1f; //chance of joining somebody doing something and talking to them
    public float loudnessTolerance = 1f;

    [HideInInspector] public float LOW;
    [HideInInspector] public float MEDIUM;
    [HideInInspector] public float HIGH;

    [HideInInspector] public float happinessInfluence;
    [HideInInspector] public float socialnessInfluence;
    [HideInInspector] public float energyInfluence;
    [HideInInspector] public float angerInfluence;
    [HideInInspector] public float sadnessInfluence;
    [HideInInspector] public float movementInfluence;

    private void Start()
    {
        LOW = 0.2f;
        MEDIUM = 0.5f;
        HIGH = 0.8f;

        happinessInfluence = 1f;
        socialnessInfluence = 1f;
        energyInfluence = 1f;
        angerInfluence = 1f;
        sadnessInfluence = 1f;
        movementInfluence = 1f;

        //dynamic
        ResetStats();

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
        DecreaseTraitAtRate(ref movement, 0.01f);
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
        float tempAnger = 0;
        SetInfluence();

        if(anger == 0)
        {
            tempAnger = 0.01f;
        }
        else
        {
            tempAnger = anger;
        }

        float score =
            (happiness * happinessInfluence)
            + (socialness * socialnessInfluence)
            + (anger * angerInfluence) 
            + (sadness * sadnessInfluence)
            + (movement * movementInfluence);

        return score / 5;
    }

    public void ResetStats()
    {
        happiness = 0.5f;
        socialness = 0.5f;
        energy = 0.5f;
        anger = 0.5f;
        sadness = 0.5f;
        movement = 0.5f;
    }

    private void SetInfluence()
    {
        happinessInfluence = GameManager.Instance.happinessSlider.value;
        sadnessInfluence = GameManager.Instance.sadnessSlider.value;
        angerInfluence = GameManager.Instance.angerSlider.value;
        socialnessInfluence = GameManager.Instance.socialnessSlider.value;
        movementInfluence = GameManager.Instance.movementSlider.value;
    }
}
