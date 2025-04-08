using UnityEngine;

public class StartingDescription : MonoBehaviour
{
    public GameObject starterHelp;
    public bool isStartHelpOpen = false; 

    private void OnTriggerEnter(Collider other)
    { 
        if (other.CompareTag("PlayerObj"))
        {
            isStartHelpOpen = true;
            starterHelp.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PlayerObj"))
        {
            isStartHelpOpen = false;
            starterHelp.SetActive(false);
        }
    }
}
