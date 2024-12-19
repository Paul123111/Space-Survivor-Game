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

    [SerializeField] float dropChance = 100f; //out of 100
    [SerializeField] GameObject drop;
    Singleton singleton;

    private void Start() {
        //enemyDeathAudio = GameObject.FindWithTag("EnemyDeathAudio").GetComponent<AudioSource>();
        singleton = GameObject.Find("Singleton").GetComponent<Singleton>();
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

            if (Random.Range(0f, 100f) <= dropChance) Instantiate(drop, hurtbox.transform);
            
            enemyDeathAudio.Play();

            singleton.SetKillCount(singleton.GetKillCount() + 1);
        }
        if (info.IsName("Dying")) {
            anim.SetBool("hasDied", true);
        }
        return this;
    }

}
