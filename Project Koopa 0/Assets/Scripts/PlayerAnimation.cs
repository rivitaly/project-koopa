using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    //Player stuff, basic
    PlayerMovement playerMovement;
    GameObject playerObj;
    PlayerMovement.PlayerState playerState;

    //I'm not too sure if AnimationClip is what to use, need to update
    Animator animator;
    PlayerMovement.PlayerState currentAnimation = PlayerMovement.PlayerState.Idle;   //Default
  
    void Start()
    {
        playerObj = transform.Find("PlayerObj").gameObject;
        playerMovement = GetComponent<PlayerMovement>();
        animator = playerObj.GetComponent<Animator>();
    }

    void Update()
    {
        //Get current state
        playerState = playerMovement.GetPlayerState();

        //If same state, do nothing
        if (playerState == currentAnimation) return;

        //Set animator's bool state
        animator.SetBool(currentAnimation.ToString(), false);
        animator.SetBool(playerState.ToString(), true);

        //Switch currentAnimation to playerState
        currentAnimation = playerState;

       
        //Switch animations
        //print("Current State: " + currentAnimation);
        //Need to do

    }
}
