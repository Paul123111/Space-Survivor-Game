using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RangedWeaponItem", menuName = "Item/RangedWeaponItem")]
public class RangedWeaponItem : ItemObject
{
    Transform target;
    Transform playerPos;
    [SerializeField] GameObject laser;
    [SerializeField] float velocity = 3f;
    [SerializeField] float accuracy = 0f;
    [SerializeField] int numProjectiles = 1;
    [SerializeField] int damage = 20;
    //LineRenderer lineRenderer;

    //Singleton singleton;
    GoToMouse goToMouse;

    [SerializeField] AudioClip[] laserGunAudioClips;
    AudioSource laserGunAudioSource;
    Inventory inventory;

    Animator anim;
    [SerializeField] LayerMask aimBlocked;

    // Start is called before the first frame update
    public override void SetUp() {
        target = GameObject.FindWithTag("PlayerCursor").transform;
        playerPos = GameObject.FindWithTag("Player").transform;
        lineRenderer = GameObject.FindWithTag("Player").GetComponentInChildren<LineRenderer>();
        singleton = GameObject.FindWithTag("Singleton").GetComponent<Singleton>();
        goToMouse = GameObject.FindWithTag("PlayerCursor").GetComponent<GoToMouse>();
        laserGunAudioSource = GameObject.FindWithTag("LaserGunAudio").GetComponent<AudioSource>();
        anim = GameObject.FindWithTag("PlayerModel").GetComponent<Animator>();
        inventory = GameObject.FindWithTag("ItemManager").GetComponent<Inventory>();
    }

    public override bool UseItem() {
        anim.SetBool("isShooting", true);
        FlowerItem flowerItem = inventory.GetFlower();
        for (int i = 0; i < numProjectiles; i++) {
            GameObject projectile = Instantiate(laser, new Vector3(playerPos.position.x, playerPos.position.y + 1.25f, playerPos.position.z) - playerPos.forward, Quaternion.Euler(-90, playerPos.eulerAngles.y, 0));
            
            if (flowerItem != null)
                projectile.GetComponent<LaserTrajectory>().SetDamage((int)Mathf.Round(damage * flowerItem.GetDamage()));
            else {
                projectile.GetComponent<LaserTrajectory>().SetDamage((int)Mathf.Round(damage));
            }

            Vector3 maxTargetPos = goToMouse.GiveMaxRadiusTarget();
            Vector3 inaccurateTargetPos = new Vector3(maxTargetPos.x + (4 * Random.Range(-accuracy, accuracy)), 0, maxTargetPos.z + (4 * Random.Range(-accuracy, accuracy)));

            Vector3 direction = (inaccurateTargetPos - playerPos.position).normalized;
            projectile.transform.rotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));

            Vector3 totalVelocity = projectile.transform.forward * velocity;
            projectile.GetComponent<Rigidbody>().velocity = totalVelocity;
            
        }
        PlaySound();
        return true;
    }

    public override void WhileSelected() {
        Vector3 direction = (target.position - playerPos.position).normalized;
        lineRenderer.startWidth = 0.2f;
        lineRenderer.endWidth = 0.2f;

        RaycastHit hit;
        if (Physics.Raycast(new Vector3(playerPos.position.x, playerPos.position.y + 1.25f, playerPos.position.z), direction, out hit, 100, aimBlocked)) {
            lineRenderer.SetPositions(new Vector3[] { new Vector3(playerPos.position.x, playerPos.position.y + 1.25f, playerPos.position.z), playerPos.position + new Vector3(direction.x, 0, direction.z) * hit.distance });
        } else {
            lineRenderer.SetPositions(new Vector3[] { new Vector3(playerPos.position.x, playerPos.position.y + 1.25f, playerPos.position.z), playerPos.position + new Vector3(direction.x, 0, direction.z) * 100 });
        }
    }

    public override void OnSwitch() {
        lineRenderer.SetPositions(new Vector3[] { new Vector3(1000, -1000, 1000), new Vector3(1000, -1000, 1000)});
        //singleton.showGrid(false);
    }

    void PlaySound() {
        laserGunAudioSource.clip = laserGunAudioClips[Random.Range(0, laserGunAudioClips.Length)];
        laserGunAudioSource.Play();
    }
}
