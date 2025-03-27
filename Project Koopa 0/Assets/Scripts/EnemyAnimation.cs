using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    Animator animator;
    EnemyBehaviour enemyBehaviour;
    EnemyBehaviour.EnemyState enemyState;
    EnemyBehaviour.EnemyState currentAnimation = EnemyBehaviour.EnemyState.Idle;   //Default

    void Start()
    {
        enemyBehaviour = GetComponent<EnemyBehaviour>();
        animator = gameObject.GetComponent<Animator>();
    }

    void Update()
    {
        //Get current state
        enemyState = enemyBehaviour.GetEnemyState();

        //If same state, do nothing
        if (enemyState == currentAnimation) return;

        //Set animator's bool state
        animator.SetBool(currentAnimation.ToString(), false);
        animator.SetBool(enemyState.ToString(), true);

        //Switch currentAnimation to playerState
        currentAnimation = enemyState;
    }
}
