using UnityEngine;

public class Traits : MonoBehaviour
{
    //dynamic traits
    public float happiness = 1f; //amount of happiness doing things / being annoyed / tiredness / loudness
    public float socialness = 1f; //talking requirement (0.5 is neutral)
    public float energy = 1f; //speed of which to do activities
    public float anger = 1f; //anger increases when people are being too loud or annoying

    //static traits
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
        LOW = 0.1f;
        MEDIUM = 0.5f;
        HIGH = 0.9f;

        //dynamic
        happiness = Random.Range(0.0f, 1.0f);
        socialness = Random.Range(0.0f, 0.2f);
        energy = Random.Range(0.0f, 1.0f);
        anger = Random.Range(0.0f, 1.0f);

        //static
        attentionSpan = Random.Range(0.0f, 1.0f);
        bandwagonEffect = Random.Range(0.0f, 1.0f);
        Evilness = Random.Range(0.0f, 1.0f);
        shyness = Random.Range(0.0f, 1.0f);
        loudnessTolerance = Random.Range(0.0f, 1.0f);
    }
    public void DecreaseTrait(ref float trait)
    {
        trait -= 0.01f * Time.deltaTime;
        trait = Mathf.Clamp(trait, 0.0f, 1.0f);
    }
    public void IncreaseTrait(ref float trait)
    {
        trait += 0.01f * Time.deltaTime;
        trait = Mathf.Clamp(trait, 0.0f, 1.0f);
    }
}
