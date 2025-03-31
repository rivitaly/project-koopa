using UnityEngine;

public class PlayerSounds : MonoBehaviour
{

    [SerializeField] private AudioClip[] sounds = new AudioClip[4]; //Hardcoded amount of sounds the player will use
    private AudioSource source;

    PlayerMovement playerMovement;
    PlayerMovement.PlayerState playerState;
    PlayerMovement.PlayerState currentState = PlayerMovement.PlayerState.Idle;  //Default

    void Start()
    {
        source = GetComponent<AudioSource>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        //Get current state
        playerState = playerMovement.GetPlayerState();

        //If same state, do nothing
        if (playerState == currentState) return;

        //Set sound
        if(playerState == PlayerMovement.PlayerState.Damaged)
        {
            source.clip = sounds[3];
            source.loop = false;
        }
        else if(playerState == PlayerMovement.PlayerState.Walk || playerState == PlayerMovement.PlayerState.Run)    //Walk and Run will use same sound
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

        //If theres an actual sound to play
        if (source.clip != null)
            source.Play();

        //Switch currentState to playerState
        currentState = playerState;
    }
}
