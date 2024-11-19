using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHurtbox : MonoBehaviour
{
    [SerializeField] EnemyStatsUI enemyStatsUI;
    
    [SerializeField] float enemyHitAggroTime;
    float aggroTimer = 0f;

    [SerializeField] float enemyInvincibilityTime;
    float invincibleTimer = 0f;

    [SerializeField] GameObject damageNumber;

    [SerializeField] AudioSource enemyHurtAudio;

    [SerializeField] Animator anim;
    AnimatorStateInfo info;

    private void Start() {
        //enemyHurtAudio = GameObject.FindWithTag("EnemyHurtAudio").GetComponent<AudioSource>();
    }

    private void Update() {
        //info = anim.GetNextAnimatorStateInfo(0);
        DepleteTimers();
    }

    private void OnTriggerEnter(Collider other) {
        if ((other.gameObject.tag == "PlayerProjectile") && (invincibleTimer <= 0) && !anim.GetBool("isDead") && (other.TryGetComponent<LaserTrajectory>(out LaserTrajectory projectile))) {
            Hurt(projectile);
            Destroy(other.gameObject);
        }
    }

    // if enemy was hit recently, the enemy will know where player is
    public bool WasHitRecentlyByPlayer() {
        return (aggroTimer > 0);
    }

    void EnemyHitTimer() {
        aggroTimer = enemyHitAggroTime;

        // no invincibility unless melee weapon used
        //invincibleTimer = enemyInvincibilityTime;
    }

    void DepleteTimers() {
        if (aggroTimer > 0) {
            aggroTimer -= Time.deltaTime;
        } 
        
        if (aggroTimer < 0) {
            aggroTimer = 0;
        }

        if (invincibleTimer > 0) {
            invincibleTimer -= Time.deltaTime;
        }

        if (invincibleTimer < 0) {
            invincibleTimer = 0;
        }
    }

    void Hurt(LaserTrajectory projectile) {
        anim.SetTrigger("gotHit");
        float damage = (projectile.GetDamage() * Random.Range(0.85f, 1.15f));
        enemyStatsUI.SetHealth(enemyStatsUI.GetHealth() - damage);
        DamageNumberText damageNumberText = Instantiate(damageNumber, new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), Quaternion.identity).GetComponent<DamageNumberText>();
        damageNumberText.SetDamageText(damage);
        EnemyHitTimer();
        enemyHurtAudio.Play();
    }
}
