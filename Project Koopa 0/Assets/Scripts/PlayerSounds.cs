using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] private AudioClip[] sounds = new AudioClip[3];
    private AudioSource source;

    PlayerMovement playerMovement;
    PlayerMovement.PlayerState playerState;
    PlayerMovement.PlayerState currentState = PlayerMovement.PlayerState.Idle;

    void Start()
    {
        source = GetComponent<AudioSource>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        //Get current state
        playerState = playerMovement.GetPlayerState();

        //If same state, do nothing
        if (playerState == currentState) return;

        //Set sound
        if(playerState == PlayerMovement.PlayerState.Walk || playerState == PlayerMovement.PlayerState.Run)
        {
            source.clip = sounds[0];
            source.loop = true;
        }
        else if(playerState == PlayerMovement.PlayerState.Jump)
        {
            source.clip = sounds[1];
            source.loop = false;
        }
        else if(playerState == PlayerMovement.PlayerState.Attack)
        {
            source.clip = sounds[2];
            source.loop = false;
        }
        else
            source.clip = null;

        if (source.clip != null)
            source.Play();

        //Switch currentState to playerState
        currentState = playerState;
    }
}
