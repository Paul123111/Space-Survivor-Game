using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeAttackState : EnemyState
{

    [SerializeField] GameObject hitBox;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] FaceTarget faceTarget;
    [SerializeField] ChaseState chaseState;

    [SerializeField] float initalDelay = 1f;
    [SerializeField] float activeTime = 0.5f;
    [SerializeField] float attackCooldown = 1f;
    bool canAttack = true;
    bool tooFar = false;

    [SerializeField] Animator anim;
    AnimatorStateInfo info;

    public override EnemyState RunCurrentState() {
        info = anim.GetCurrentAnimatorStateInfo(0);
        faceTarget.SetLookingAtTarget(true);
        agent.isStopped = true;

        if (canAttack && (info.IsName("PreparingToAttack") || info.IsName("AttackFinished"))) {
            tooFar = false;
            anim.ResetTrigger("attack");
            anim.SetTrigger("attack");
            StartCoroutine(Attack());
        }

        if (tooFar) {
            canAttack = true;
            return chaseState;
        }

        return this;
    }

    IEnumerator Attack() {
        //Debug.Log("I will attack");
        canAttack = false;

        yield return new WaitForSeconds(initalDelay);
        hitBox.SetActive(true);
        yield return new WaitForSeconds(activeTime);
        hitBox.SetActive(false);
        yield return new WaitForSeconds(attackCooldown);
        
        if (chaseState.InRange()) {
            canAttack = true;
        } else {
            anim.SetBool("inAttackRange", false);
            tooFar = true;
        }
    }
}
