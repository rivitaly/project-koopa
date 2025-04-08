using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    //loads Main Scene
    public void Play()
    {
        SceneManager.LoadScene("MainWorld");
    }

    //Quits the application
    public void Quit()
    { 
        Application.Quit();
    }
}
