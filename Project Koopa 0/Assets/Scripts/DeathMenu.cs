using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathMenu : MonoBehaviour
{
    public void Restart()
    {
        SceneManager.LoadScene("MainWorld");
    }
    public void Quit()
    {
        Application.Quit();
    }
}
