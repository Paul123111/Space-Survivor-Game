using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateManager : MonoBehaviour
{
    [SerializeField] EnemyState currentState;
    [SerializeField] EnemyState deathState;
    [SerializeField] EnemyStatsUI enemyStatsUI;
    [SerializeField] Animator anim;
    AnimatorStateInfo info;

    private void Start() {
        
    }

    // Update is called once per frame
    void Update()
    {
        RunStateMachine();
    }

    // Credit to https://www.youtube.com/watch?v=cnpJtheBLLY for Basic State Machine Logic
    void RunStateMachine() {
        info = anim.GetCurrentAnimatorStateInfo(0);

        // dying overrides state
        if (enemyStatsUI.GetHealth() <= 0) {
            deathState.RunCurrentState();
            if (info.IsName("Dead")) {
                Destroy(gameObject, 3f);
            }
            return;
        }

        if (currentState != null) {
            EnemyState nextState = currentState.RunCurrentState();
            //print(currentState);
            
            if (nextState != null)
                SwitchToNextState(nextState);
        }
    }

    void SwitchToNextState(EnemyState nextState) {
        currentState = nextState;
        //Debug.Log(nextState.name);
    }
}
