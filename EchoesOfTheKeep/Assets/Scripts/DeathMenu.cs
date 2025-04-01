using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathMenu : MonoBehaviour
{
    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }
    public void Restart()
    {
        SceneManager.LoadScene("MainWorld");
    }
    public void Quit()
    {
        Application.Quit();
    }
}
