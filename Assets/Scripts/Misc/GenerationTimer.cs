using Unity.VisualScripting;
using UnityEngine;
using TMPro;

public class GenerationTimer : MonoBehaviour
{
    [SerializeField] private bool enableConstantGenerationTime;
    [SerializeField] private float secondsPerGeneration;
    [SerializeField] private TMP_InputField timeInput;

    private float timer;
    private GameManager gameManager;
    private Calculations calculations;

    private void Awake()
    {
        gameManager = GetComponent<GameManager>();
        calculations = GetComponent<Calculations>();
    }

    private void Start()
    {
        //Initially generate a time to get next generation
        timer = Time.time + secondsPerGeneration;
    }

    private void Update()
    {
        if(!enableConstantGenerationTime)
        {
            return;
        }

        //Check if surpassed timer
        if(Time.time >= timer)
        {
            timer = Time.time + secondsPerGeneration;
        }
        else
        {
            return;
        }

        //Create new generation
        calculations.UpdateAverageScore();
        gameManager.NextGeneration();
    }

    public void EnableAutoGenerationTimer()
    {
        enableConstantGenerationTime = !enableConstantGenerationTime;
    }

    public void ChangeInterval()
    {
        if (float.TryParse(timeInput.text.ToString(), out float number))
        {
            secondsPerGeneration = number;
        }
        else
        {
            Debug.Log("Couldn't parse input");
        }
    }
}
