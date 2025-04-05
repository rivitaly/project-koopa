using UnityEngine;

public class ApplicationQuitHandler : MonoBehaviour
{
    private void OnApplicationQuit()
    {
        if (!Application.isEditor)
        { 
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
    }
}
