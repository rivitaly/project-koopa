using UnityEngine;
using UnityEngine.InputSystem;

public class Secret : MonoBehaviour
{
    string[] secret = new string[8] { "Up", "Up", "Down", "Down", "Left", "Right", "Left", "Right" };
    public AudioClip secretSound;
    int pos = 0;
    bool isActivated = false;

    private void OnUp()
    {
        if (isActivated) { return; }
        if (secret[pos] == "Up")
            pos++;
        else
            pos = 0;

        if (pos == secret.Length)
            SecretMethod();
    }

    private void OnDown()
    {
        if (isActivated) { return; }
        if (secret[pos] == "Down")
            pos++;
        else
            pos = 0;

        if (pos == secret.Length)
            SecretMethod();
    }

    private void OnPrevious()
    {
        if (isActivated) { return; }
        if (secret[pos] == "Left")
            pos++;
        else
            pos = 0;

        if (pos == secret.Length)
            SecretMethod();
    }

    private void OnNext()
    {
        if (isActivated) { return; }
        if (secret[pos] == "Right")
            pos++;
        else
            pos = 0;

        if (pos == secret.Length)
            SecretMethod();
    }

    private void SecretMethod()
    {
        print("Secret");
        GetComponent<AudioSource>().clip = secretSound;
        GetComponent<AudioSource>().Play();
        isActivated = true;
    }
}
