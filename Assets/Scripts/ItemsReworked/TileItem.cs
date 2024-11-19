using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "TileItem", menuName = "Item/TileItem")]
public class TileItem : ItemObject
{
    Grid grid;
    Tilemap tilemap;
    Transform target;
    [SerializeField] RuleTile tile;
    [SerializeField] RuleTile tileToBePlaced;
    [SerializeField] RuleTile tileToBePlacedValid;
    Inventory inventory;
    Vector3Int tilePosPrev;
    Transform playerPos;
    bool tilePlaceable;
    AudioSource blockPlaceSound;

    Singleton singleton;

    // Start is called before the first frame update
    public override void SetUp() {
        grid = GameObject.FindWithTag("Grid").GetComponent<Grid>();
        tilemap = GameObject.FindWithTag("Tilemap").GetComponent<Tilemap>();
        target = GameObject.FindWithTag("PlayerCursor").transform;
        playerPos = GameObject.FindWithTag("Player").transform;
        inventory = GameObject.FindWithTag("ItemManager").GetComponent<Inventory>();
        singleton = GameObject.FindWithTag("Singleton").GetComponent<Singleton>();
        blockPlaceSound = GameObject.FindWithTag("BlockPlaceSound").GetComponent<AudioSource>();
    }

    public override bool UseItem() {
        //TODO mining durability for blocks (+ hold left mouse with drill to mine)
        Vector3Int tilePos = grid.WorldToCell(new Vector3(target.position.x, 0, target.position.z));
        //Vector3Int tilePlayerPos = grid.WorldToCell(new Vector3(playerPos.position.x, 0, playerPos.position.z));
        if (tilePlaceable && (tilemap.GetTile(tilePos) == null || tilemap.GetTile(tilePosPrev) == tileToBePlacedValid)) {
            blockPlaceSound.Play();
            tilemap.SetTile(tilePos, tile);
            return true;
        }

        return false;
    }

    public override void WhileSelected() {
        Vector3Int tilePos = grid.WorldToCell(new Vector3(target.position.x, 0, target.position.z));
        Vector3Int tilePlayerPos = grid.WorldToCell(new Vector3(playerPos.position.x, 0, playerPos.position.z));
        if (tilemap.GetTile(tilePosPrev) == null || tilemap.GetTile(tilePosPrev) == tileToBePlaced || tilemap.GetTile(tilePosPrev) == tileToBePlacedValid) {
            if (tilePos != tilePosPrev)
                tilemap.SetTile(tilePosPrev, null);
        }

        tilePlaceable = Vector3.Distance(tilePlayerPos, tilePos) >= 1;

        if (tilemap.GetTile(tilePos) == null || tilemap.GetTile(tilePos) == tileToBePlaced || tilemap.GetTile(tilePos) == tileToBePlacedValid)
            if (tilePlaceable) {
                tilemap.SetTile(tilePos, tileToBePlacedValid);
            } else {
                tilemap.SetTile(tilePos, tileToBePlaced);
            }

        tilePosPrev = new Vector3Int(tilePos.x, tilePos.y, tilePos.z);
    }

    public override void OnSwitch() {
        singleton.showGrid(true);
        Vector3Int tilePos = grid.WorldToCell(new Vector3(target.position.x, 0, target.position.z));
        if (tilemap.GetTile(tilePos) == null || tilemap.GetTile(tilePos) == tileToBePlaced || tilemap.GetTile(tilePos) == tileToBePlacedValid)
            tilemap.SetTile(tilePos, null);
    }

}
