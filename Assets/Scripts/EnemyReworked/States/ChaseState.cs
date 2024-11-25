using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaseState : EnemyState
{
    [SerializeField] WanderingState wanderingState;
    [SerializeField] EnemyState[] attackState;
    [SerializeField] float attackRange = 5f;

    [SerializeField] NavMeshAgent agent;
    [SerializeField] FaceTarget faceTarget;
    Transform player;

    [SerializeField] Animator anim;
    AnimatorStateInfo info;

    // Start is called before the first frame update
    void Start()
    {
        //agent = GetComponent<NavMeshAgent>();
        //fieldOfViewRadians = fieldOfViewDegrees * (Mathf.PI / 180.0f);
        //fieldOfViewCos = Mathf.Cos(fieldOfViewRadians);
        player = GameObject.FindWithTag("Player").transform;
    }

    public override EnemyState RunCurrentState() {
        
        if (InRange()) {
            anim.SetBool("inAttackRange", true);
            return attackState[Random.Range(0, attackState.Length)];
        }
        
        wanderingState.Look();
        wanderingState.Hear();
        wanderingState.Smell();

        //print("Chasing: " + wanderingState.IsPlayerDetected());

        //if player is detected
        if (wanderingState.IsPlayerDetected()) {
            ChasePlayer();
            return this;
        } else {
            return wanderingState;
        };
    }

    void ChasePlayer() {
        faceTarget.SetLookingAtTarget(true);
        agent.isStopped = false;
        agent.destination = player.position;
    }

    public bool InRange() {
        if (Vector3.Distance(transform.position, player.position) < attackRange) {
            return true;
        }
        return false;
    }

    public EnemyState[] GetAttackStates() {
        return attackState;
    }

}
