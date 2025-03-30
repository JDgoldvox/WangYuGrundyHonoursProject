using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadStandardBehaviourTreeScene()
    {
        SceneManager.LoadScene(2);
    }

    public void LoadPlaygroundScene()
    {
        SceneManager.LoadScene(1);
    }

    public void LoadGeneticAlgorithmScene()
    {
        SceneManager.LoadScene(3);
    }

    public void LoadMainMenu()
    {   
        SceneManager.LoadScene(0);
    }

    public void QuitApplication()
    {
        Application.Quit();
    }
}
