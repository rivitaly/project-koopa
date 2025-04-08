using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathMenu : MonoBehaviour
{
    void Start()
    {
        //confines cursor to window and sets the visibility to true
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }
    //loads Main Scene
    public void Restart()
    {
        SceneManager.LoadScene("MainWorld");
    }
    //quits the application
    public void Quit()
    {
        Application.Quit();
    }
}
