using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour
{
    CharacterController controller;
    Vector3 playerVelocity;
    Vector3 playerPosPrev;

    [SerializeField] float maxAccelerationGround = 1f;
    [SerializeField] float maxAccelerationWater = 0.5f;
    [SerializeField] float maxSpeedGround = 1f;
    [SerializeField] float maxSpeedWater = 0.5f;

    [SerializeField] float playerSpeed = 1f;
    [SerializeField] float maxAcceleration = 1f;
    [SerializeField] float acceleration = 1f;
    [SerializeField] float deceleration = 1f;
    float currentSpeed;
    Vector3 inputValue;

    //[SerializeField] float rotateSpeed = 1f;
    [SerializeField] Transform target;
    [SerializeField] Camera playerCam;
    Tilemap walls;
    Tilemap ground;
    //[SerializeField] RuleTile tile;
    Grid grid;
    [SerializeField] Inventory inventory;
    [SerializeField] AudioSource collectAudio;
    [SerializeField] GameObject breadCrumb;

    Singleton singleton;
    GameSceneManager gameSceneManager;
    [SerializeField] PlayerStats playerStats;

    bool tilePlaceable = true;

    [SerializeField] float playerInvincibilityTime;
    float invincibleTimer = 0f;

    //[SerializeField] Material defaultMaterial;
    //[SerializeField] Material invincibleMaterial;
    AudioSource playerHurtAudio;
    Animator anim;

    GameObject craftingUI;
    CraftingSystem craftingSystem;

    [SerializeField] PauseGame pauseGame;

    int debugID = 1;

    //[SerializeField] Renderer astronautModel;

    // TODO: this script needs to be modularised, it's becoming difficult to maintain

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        //gameObject.GetComponent<Renderer>().material = defaultMaterial;
        anim = GameObject.FindWithTag("PlayerModel").GetComponent<Animator>();
        grid = GameObject.Find("Grid").GetComponent<Grid>();
        walls = GameObject.Find("Walls").GetComponent<Tilemap>();
        ground = GameObject.Find("Ground").GetComponent<Tilemap>();
        singleton = GameObject.Find("Singleton").GetComponent<Singleton>();
        gameSceneManager = GameObject.Find("Singleton").GetComponent<GameSceneManager>();
        playerHurtAudio = GameObject.Find("PlayerHurtAudio").GetComponent<AudioSource>();
        craftingUI = GameObject.Find("CraftingMenu");
        craftingSystem = craftingUI.GetComponent<CraftingSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        FindGroundTile();

        playerPosPrev = transform.position;

        controller.Move(playerVelocity * playerSpeed * Time.deltaTime);

        if (transform.position.y != 0.375f) {
            transform.position = new Vector3(transform.position.x, 0.375f, transform.position.z);
        }

        //Calculate Rotation
        //if (playerVelocity != new Vector3(0, 0, 0)) {
        //    transform.rotation = Quaternion.Lerp(toRotation, Quaternion.LookRotation(playerVelocity * playerSpeed), Time.fixedDeltaTime * rotateSpeed);
        //} else {
        //transform.rotation = toRotation;
        //transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
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

        Quaternion toRotation = Quaternion.LookRotation(transform.position - new Vector3(target.position.x, 0, target.position.z));
        transform.rotation = toRotation;
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);

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

    void FindGroundTile()
    {
        Vector3Int tilePos = grid.WorldToCell(new Vector3(transform.position.x, 0, transform.position.z));
        if (ground.GetTile(tilePos) == null)
        {
            return;
        }

        //print(ground.GetTile(tilePos).name);
        switch (ground.GetTile(tilePos).name)
        {
            case "BlueBiomeTerrain": maxAcceleration = maxAccelerationGround; playerSpeed = maxSpeedGround; break;
            case "RedBiomeTerrain": maxAcceleration = maxAccelerationGround; playerSpeed = maxSpeedGround; break;
            case "CaveTerrain": maxAcceleration = maxAccelerationGround; playerSpeed = maxSpeedGround; break;
            case "Water": maxAcceleration = maxAccelerationWater; playerSpeed = maxSpeedWater; break;
            default: maxAcceleration = maxAccelerationGround; playerSpeed = maxSpeedGround; break;
        }
    }

    void OnInteract() {
        //print("hi");
        if (pauseGame.GetPaused()) return;

        Vector3Int tilePos = grid.WorldToCell(new Vector3(target.position.x, 0, target.position.z));
        if (walls.GetTile(tilePos) == null) {
            return;
        }
        //print(walls.GetTile(tilePos).name);
        switch (walls.GetTile(tilePos).name) {
            case "CaveEntrance": StartCoroutine(gameSceneManager.SaveAndLoadScene(4)); break;
            case "CaveExit": StartCoroutine(gameSceneManager.SaveAndLoadScene(1)); break;
            case "Spaceship": {
                    if (inventory.NumberOfItemsHeld(10) >= 24 && inventory.NumberOfItemsHeld(4) >= 32 && inventory.NumberOfItemsHeld(5) >= 4 && inventory.NumberOfItemsHeld(19) >= 16) {
                        singleton.GetComponent<GameSceneManager>().LoadSceneByIndex(0);
                    } else {
                        print("not enough materials"); 
                    }
                    break;
                }
            case "CraftingStation":
                {
                    pauseGame.TogglePause();
                    craftingSystem.ViewCrafting();
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
            case "AlienHidePickup": {
                    //print("hi");
                    collectAudio.Play();
                    inventory.AddItemToInventory(18);
                    Destroy(other.gameObject);
                    break;
                }
            case "AlienEyePickup": {
                    //print("hi");
                    collectAudio.Play();
                    inventory.AddItemToInventory(19);
                    Destroy(other.gameObject);
                    break;
                }
            case "EnemyHitbox": {
                    // TODO: move to separate script
                    if (invincibleTimer <= 0 && other.TryGetComponent<EnemyHitbox>(out EnemyHitbox hitbox)) {
                        ArmourItem armourItem = inventory.GetArmour();
                        SetInvinciblityTime();
                        playerStats.ScreenFlash();
                        if (armourItem != null) {
                            playerStats.SetOxygen(playerStats.GetOxygen() - (hitbox.GetDamage() * (1.0f - armourItem.GetDamageReduction())));
                        } else {
                            playerStats.SetOxygen(playerStats.GetOxygen() - hitbox.GetDamage());
                        }
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

    // Debug only
    void OnDebugItem() {
        inventory.AddItemToInventory(debugID % inventory.GetItemList().Length);
        debugID++;
    }

    //void OnDebugSave() {
    //    gameSceneManager.Save();
    //}
}
