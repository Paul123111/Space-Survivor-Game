using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DeathState : EnemyState
{
    [SerializeField] AudioSource enemyDeathAudio;
    [SerializeField] Animator anim;
    AnimatorStateInfo info;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] FaceTarget faceTarget;
    [SerializeField] Collider enemyBody;
    [SerializeField] Collider hurtbox;

    private void Start() {
        //enemyDeathAudio = GameObject.FindWithTag("EnemyDeathAudio").GetComponent<AudioSource>();
    }


    public override EnemyState RunCurrentState() {
        //print("Enemy defeated");
        info = anim.GetCurrentAnimatorStateInfo(0);
        agent.isStopped = true;
        if (!anim.GetBool("isDead")) {
            anim.SetBool("isDead", true);
            enemyBody.enabled = false;
            enemyBody.gameObject.tag = "Untagged";
            hurtbox.enabled = false;
            faceTarget.SetLookingAtTarget(false);
            enemyDeathAudio.Play();
        }
        if (info.IsName("Dying")) {
            anim.SetBool("hasDied", true);
        }
        return this;
    }

}
