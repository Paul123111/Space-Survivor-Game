using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ShootingState : EnemyState
{
    [SerializeField] GameObject laser;
    [SerializeField] float velocity = 3f;
    [SerializeField] float accuracy = 0f;
    [SerializeField] int numProjectiles = 1;
    [SerializeField] int damage = 20;
    [SerializeField] float cooldown = 1f;
    [SerializeField] int numRounds = 1;
    [SerializeField] float timeBetweenRounds = 1f;

    [SerializeField] NavMeshAgent agent;
    [SerializeField] FaceTarget faceTarget;
    [SerializeField] ChaseState chaseState;

    bool canAttack = true;
    bool tooFar = false;
    Transform playerPos;

    [SerializeField] Animator anim;
    AnimatorStateInfo info;

    bool newAttackReady = false;

    // Start is called before the first frame update
    void Start()
    {
        playerPos = GameObject.FindWithTag("Player").transform;
    }

    public override EnemyState RunCurrentState() {
        //print(canAttack);
        info = anim.GetCurrentAnimatorStateInfo(0);
        faceTarget.SetLookingAtTarget(true);
        agent.isStopped = true;

        if (canAttack && (info.IsName("PreparingToAttack") || info.IsName("AttackFinished"))) {
            if (newAttackReady) {
                newAttackReady = false;
                return chaseState.GetAttackStates()[Random.Range(0, chaseState.GetAttackStates().Length)];
            }

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
        for (int i = 0; i < numRounds; i++) {
            for (int j = 0; j < numProjectiles; j++) {
                GameObject projectile = Instantiate(laser, transform.position, Quaternion.identity);
                projectile.GetComponent<EnemyHitbox>().SetDamage(damage);
                Vector3 inaccurateTargetPos = new Vector3(playerPos.position.x + (4 * Random.Range(-accuracy, accuracy)), 0, playerPos.position.z + (4 * Random.Range(-accuracy, accuracy)));

                Vector3 direction = (inaccurateTargetPos - transform.position).normalized;
                projectile.transform.rotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));

                Vector3 totalVelocity = projectile.transform.forward * velocity;
                projectile.GetComponent<Rigidbody>().velocity = totalVelocity;

            }
            yield return new WaitForSeconds(timeBetweenRounds);
        }
        //PlaySound();

        yield return new WaitForSeconds(cooldown);

        if (chaseState.InRange()) {
            canAttack = true;
            newAttackReady = true;
        } else {
            anim.SetBool("inAttackRange", false);
            tooFar = true;
        }
    }
}
