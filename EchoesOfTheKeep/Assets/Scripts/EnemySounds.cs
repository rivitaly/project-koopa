using UnityEngine;

public class EnemySounds : MonoBehaviour
{
    [SerializeField] private AudioClip[] sounds = new AudioClip[2]; //Hardcoded amount of sounds for enemy
    AudioSource source;

    EnemyBehaviour enemyBehaviour;
    EnemyBehaviour.EnemyState enemyState;
    EnemyBehaviour.EnemyState currentState = EnemyBehaviour.EnemyState.Idle;   //Default

    void Start()
    {
        source = GetComponent<AudioSource>();
        enemyBehaviour = GetComponent<EnemyBehaviour>();
    }

    void Update()
    {
        //Get current state
        enemyState = enemyBehaviour.GetEnemyState();

        //If same state, do nothing
        if (enemyState == currentState) return;

        //Set sound to appropriate state
        if(enemyState == EnemyBehaviour.EnemyState.Chase)
        {
            source.clip = sounds[0];
            source.loop = true;
        }
        else if(enemyState == EnemyBehaviour.EnemyState.Attack)
        {
            source.clip = sounds[1];
            source.loop = false;
        }
        else
        {
            source.clip = null;
        }

        source.Play();

        //Switch currentState to enemyState
        currentState = enemyState;
    }
}
