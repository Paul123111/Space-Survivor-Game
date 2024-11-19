using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour
{
    CharacterController controller;
    Vector3 playerVelocity;
    Vector3 playerPosPrev;

    [SerializeField] float playerSpeed = 1f;
    [SerializeField] float maxAcceleration = 1f;
    [SerializeField] float acceleration = 1f;
    [SerializeField] float deceleration = 1f;
    float currentSpeed;
    Vector3 inputValue;

    //[SerializeField] float rotateSpeed = 1f;
    [SerializeField] Transform target;
    [SerializeField] Camera playerCam;
    [SerializeField] Tilemap tilemap;
    [SerializeField] Tilemap tilemapUnbreakable;
    [SerializeField] RuleTile tile;
    [SerializeField] Grid grid;
    [SerializeField] Inventory inventory;
    [SerializeField] AudioSource collectAudio;
    [SerializeField] GameObject breadCrumb;

    [SerializeField] Singleton singleton;
    [SerializeField] PlayerStats playerStats;

    bool tilePlaceable = true;

    [SerializeField] float playerInvincibilityTime;
    float invincibleTimer = 0f;

    //[SerializeField] Material defaultMaterial;
    //[SerializeField] Material invincibleMaterial;
    [SerializeField] AudioSource playerHurtAudio;
    Animator anim;

    //[SerializeField] Renderer astronautModel;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        //gameObject.GetComponent<Renderer>().material = defaultMaterial;
        anim = GameObject.FindWithTag("PlayerModel").GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        playerPosPrev = transform.position;

        controller.Move(playerVelocity * playerSpeed * Time.deltaTime);
        Quaternion toRotation = Quaternion.LookRotation(transform.position - new Vector3(target.position.x, 0, target.position.z));

        if (transform.position.y != 0.375f) {
            transform.position = new Vector3(transform.position.x, 0.375f, transform.position.z);
        }

        //Calculate Rotation
        //if (playerVelocity != new Vector3(0, 0, 0)) {
        //    transform.rotation = Quaternion.Lerp(toRotation, Quaternion.LookRotation(playerVelocity * playerSpeed), Time.fixedDeltaTime * rotateSpeed);
        //} else {
        transform.rotation = toRotation;
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
        //tilePlaceable = true;
        //}

        if (invincibleTimer > 0) {
            invincibleTimer -= Time.deltaTime;
        } else if (invincibleTimer == 0) {
            //gameObject.GetComponent<Renderer>().material = defaultMaterial;
        } else if (invincibleTimer < 0) {
            invincibleTimer = 0;
        }
        
    }

    private void FixedUpdate() {
        if (playerVelocity.magnitude > playerSpeed) {
            playerVelocity = playerVelocity.normalized * playerSpeed;
        }

        Acceleration();
        IsWalkingBackwards();
    }

    void Acceleration() {
        if (inputValue != Vector3.zero) {
            if (currentSpeed + acceleration * Time.fixedDeltaTime < maxAcceleration) {
                currentSpeed += acceleration * Time.fixedDeltaTime;
            } else {
                currentSpeed = maxAcceleration;
            }
            playerVelocity += inputValue * (currentSpeed * Time.fixedDeltaTime);
        } else if (playerVelocity != Vector3.zero) {
            if (currentSpeed > 0) {
                currentSpeed -= deceleration * Time.fixedDeltaTime;
            } else if (currentSpeed < 0) {
                currentSpeed = 0;
            }
            playerVelocity -= playerVelocity.normalized * (deceleration * Time.fixedDeltaTime);
        }
    }

    void IsWalkingBackwards() {
        // if the dot product of transform forward and the difference between the player's position from this frame and the last is below 0, player is moving backwards
        if (Vector3.Dot(transform.forward, playerPosPrev - transform.position) < 0) {
            anim.SetFloat("speed", -playerVelocity.magnitude);
        } else {
            anim.SetFloat("speed", playerVelocity.magnitude);
        }
    }

    void OnMove(InputValue value) {
        Vector3 moveValue = value.Get<Vector2>();
        inputValue = new Vector3(moveValue.x, 0, moveValue.y);
    }

    void OnInteract() {
        //print("hi");
        Vector3Int tilePos = grid.WorldToCell(new Vector3(target.position.x, 0, target.position.z));
        if (tilemapUnbreakable.GetTile(tilePos) == null) {
            return;
        }
        //print(tilemapUnbreakable.GetTile(tilePos).name);
        switch (tilemapUnbreakable.GetTile(tilePos).name) {
            case "CaveEntrance": singleton.GetComponent<GameSceneManager>().LoadSceneByIndex(5); break;
            case "CaveExit": singleton.GetComponent<GameSceneManager>().LoadSceneByIndex(1); break;
            case "Spaceship": {
                    if (inventory.NumberOfItemsHeld(10) >= 24 && inventory.NumberOfItemsHeld(4) >= 32 && inventory.NumberOfItemsHeld(5) >= 4) {
                        singleton.GetComponent<GameSceneManager>().LoadSceneByIndex(0);
                    } else {
                        print("not enough materials"); 
                    }
                    break;
                }
            default: break;
        }
    }

    private void OnTriggerEnter(Collider other) {
        //print(other.gameObject.tag);
        switch (other.gameObject.tag) {
            case "Pickup": {
                    collectAudio.Play();
                    inventory.AddItemToInventory(2);
                    Destroy(other.gameObject);
                    break;
                }
             case "WoodPickup": {
                        //print("hi");
                    collectAudio.Play();
                    inventory.AddItemToInventory(4);
                    Destroy(other.gameObject);
                    break;
                }
            case "FlowerPickup": {
                    collectAudio.Play();
                    inventory.AddItemToInventory(5);
                    Destroy(other.gameObject);
                    break;
                }
            case "BoulderPickup": {
                    //print("hi");
                    collectAudio.Play();
                    inventory.AddItemToInventory(9);
                    Destroy(other.gameObject);
                    break;
                }
            case "IronPickup": {
                    //print("hi");
                    collectAudio.Play();
                    inventory.AddItemToInventory(10);
                    Destroy(other.gameObject);
                    break;
                }
            case "EnemyHitbox": {
                    if (invincibleTimer <= 0 && other.TryGetComponent<EnemyHitbox>(out EnemyHitbox hitbox)) {
                        SetInvinciblityTime();
                        playerStats.SetOxygen(playerStats.GetOxygen() - hitbox.GetDamage());
                        playerHurtAudio.Play();
                    }
                    break;
                }
            default: break;
        }
    }

    public bool GetTilePlaceable() {
        return tilePlaceable;
    }

    public void SetInvinciblityTime() {
        //astronautModel.materials[1] = invincibleMaterial;
        invincibleTimer = playerInvincibilityTime;
    }
}
