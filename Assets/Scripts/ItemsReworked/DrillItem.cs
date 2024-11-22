using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.InputSystem;
using TMPro;

[CreateAssetMenu(fileName = "drill", menuName = "Item/Drill")]
public class DrillItem : ItemObject {
    Grid grid;
    Tilemap tilemap;
    Transform target;
    Transform player;
    LineRenderer lineRenderer;
    Singleton singleton;
    AudioSource miningSound;
    AudioSource blockBreakSound;

    Animator anim;

    // Start is called before the first frame update
    public override void SetUp() {
        grid = GameObject.FindWithTag("Grid").GetComponent<Grid>();
        tilemap = GameObject.FindWithTag("Tilemap").GetComponent<Tilemap>();
        target = GameObject.FindWithTag("PlayerCursor").transform;
        player = GameObject.FindWithTag("Player").transform;
        anim = GameObject.FindWithTag("PlayerModel").GetComponent<Animator>();
        lineRenderer = player.GetComponentInChildren<LineRenderer>();
        singleton = GameObject.FindWithTag("Singleton").GetComponent<Singleton>();
        miningSound = GameObject.FindWithTag("UseSound").GetComponent<AudioSource>();
        blockBreakSound = GameObject.FindWithTag("BlockBreakSound").GetComponent<AudioSource>();
    }

    public override bool UseItem() {
        //TODO mining durability for blocks (+ hold left mouse with drill to mine)
        //lineRenderer.SetPositions(new Vector3[]{player.position, target.position});
        anim.SetBool("isShooting", true);

        Vector3Int tilePos = grid.WorldToCell(new Vector3(target.position.x, 0, target.position.z));
        if (tilemap.GetTile(tilePos) == null)
            return false;
        //print(tilemap.GetTile(tilePos).name);

        switch (tilemap.GetTile(tilePos).name) {
            case "BlueBiomeWall": ItemManager.CreateItemPickups(1, new Vector3(target.position.x, 0, target.position.z)); break;
            case "TreeTile": for (int i = 0; i < 6; i++) { ItemManager.CreateItemPickups(2, new Vector3(target.position.x, 0, target.position.z)); } break;
            case "WoodTile": ItemManager.CreateItemPickups(2, new Vector3(target.position.x, 0, target.position.z)); break;
            case "FlowerTile": ItemManager.CreateItemPickups(3, new Vector3(target.position.x, 0, target.position.z)); break;
            case "BoulderTile": ItemManager.CreateItemPickups(4, new Vector3(target.position.x, 0, target.position.z)); ItemManager.CreateItemPickups(5, new Vector3(target.position.x, 0, target.position.z)); break;
            default: return false;
        }
        tilemap.SetTile(tilePos, null);
        singleton.SetScore(singleton.GetScore()+1);
        blockBreakSound.Play();
        return true;
    }

    public override void WhileSelected() {
        lineRenderer.startWidth = 0.5f;
        lineRenderer.endWidth = 0.5f;
        lineRenderer.SetPositions(new Vector3[] { new Vector3(player.position.x, player.position.y+1.25f, player.position.z), target.position });
        if (Mouse.current.leftButton.isPressed) {
            if (!miningSound.isPlaying)
                miningSound.Play();
        } else {
            miningSound.Stop();
        }
    }

    public override void OnSwitch() {
        lineRenderer.SetPositions(new Vector3[] { new Vector3(1000, -1000, 1000), new Vector3(1000, -1000, 1000) });
        singleton.showGrid(true);
    }

}
