using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public void Play()
    {
        SceneManager.LoadScene("MainWorld");
    }

    public void Quit()
    { 
        Application.Quit();
    }
}
