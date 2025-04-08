using UnityEngine;

public class ApplicationQuitHandler : MonoBehaviour
{
    //All this does is makes sure the process is killed if we aren't running the application
    private void OnApplicationQuit()
    {
        if (!Application.isEditor)
        { 
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
    }
}
