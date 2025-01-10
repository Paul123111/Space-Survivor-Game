using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;

public class ShootAtEnemies : MonoBehaviour
{

    [SerializeField] GameObject laser;
    [SerializeField] float velocity = 3f;
    [SerializeField] float accuracy = 0f;
    [SerializeField] int numProjectiles = 1;
    [SerializeField] int damage = 20;
    [SerializeField] float cooldown = 1f;
    [SerializeField] LayerMask layerMask;
    [SerializeField] LayerMask layerMask2;
    [SerializeField] float detectionRadius;

    Inventory inventory;
    Collider[] nearbyEnemies;

    // Start is called before the first frame update
    void Start()
    {
        inventory = GameObject.FindWithTag("ItemManager").GetComponent<Inventory>();
        StartCoroutine(Attack());
    }

    // Update is called once per frame
    //void Update()
    //{
        
    //}

    Collider FindClosestEnemy() {
        nearbyEnemies = Physics.OverlapSphere(transform.position, detectionRadius, layerMask.value);
        if (nearbyEnemies.Length <= 0) return null;

        float closestDistance = detectionRadius+1;
        int index = -1;
        RaycastHit hit;

        for (int i = 0; i < nearbyEnemies.Length; i++) {
            float dist = Vector3.Distance(nearbyEnemies[i].transform.position, transform.position);
            Vector3 direction = (nearbyEnemies[i].transform.position - transform.position).normalized;
            if (nearbyEnemies[i].gameObject.tag == "Enemy" && dist < closestDistance && Physics.Raycast(transform.position, direction * 100, out hit, 100, layerMask2.value)) {
                //Debug.DrawRay(transform.position, direction * hit.distance, Color.red);
                if (hit.collider.gameObject.tag == "EnemyHurtbox") {
                    //print(hit.collider.gameObject.tag);
                    closestDistance = dist;
                    index = i;
                }
            }
            
        }

        if (index == -1) return null;
        return nearbyEnemies[index];
    }

    void Shoot(Vector3 enemyPos) {
        FlowerItem flowerItem = inventory.GetFlower();

        for (int i = 0; i < numProjectiles; i++) {
            GameObject projectile = Instantiate(laser, transform.position, Quaternion.Euler(-90, transform.eulerAngles.y, 0));
            if (flowerItem != null)
                projectile.GetComponent<LaserTrajectory>().SetDamage((int)Mathf.Round(damage * flowerItem.GetDamage()));
            else {
                projectile.GetComponent<LaserTrajectory>().SetDamage((int)Mathf.Round(damage));
            }
            
            Vector3 inaccurateTargetPos = new Vector3(enemyPos.x + (4 * Random.Range(-accuracy, accuracy)), 0, enemyPos.z + (4 * Random.Range(-accuracy, accuracy)));

            Vector3 direction = (inaccurateTargetPos - transform.position).normalized;
            projectile.transform.rotation = Quaternion.LookRotation(direction);

            Vector3 totalVelocity = projectile.transform.forward * velocity;
            projectile.GetComponent<Rigidbody>().velocity = totalVelocity;

        }
    }

    IEnumerator Attack() {
        for (;;) {
            Collider nearestEnemy = FindClosestEnemy();
            if (nearestEnemy != null) {
                Vector3 faceEnemy = new Vector3(nearestEnemy.transform.position.x, transform.position.y, nearestEnemy.transform.position.z);
                Vector3 dir = (-faceEnemy+transform.position).normalized;
                //print(dir);
                transform.rotation = Quaternion.LookRotation(dir);
                Shoot(nearestEnemy.transform.position);
            }
            yield return new WaitForSeconds(cooldown);
        }
    }
}
