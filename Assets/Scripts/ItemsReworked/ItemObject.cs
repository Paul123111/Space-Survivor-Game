using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="ItemExample", menuName="Item/DefaultItem")]
public class ItemObject : ScriptableObject
{
    [SerializeField] bool isStackable = false;
    [SerializeField] int maxStack = 1;
    [SerializeField] string itemName;
    [SerializeField] float cooldown = 0.05f;
    [SerializeField] bool consumable = false;
    [SerializeField] Sprite icon;

    protected Singleton singleton;
    protected LineRenderer lineRenderer;
    protected Transform player;

    //[SerializeField] int id;
    //Will be done in later prototype
    //[SerializeField] Sprite itemSprite;

    public virtual void SetUp() {
        player = GameObject.FindWithTag("Player").transform;
        lineRenderer = player.GetComponentInChildren<LineRenderer>();
        singleton = GameObject.FindWithTag("Singleton").GetComponent<Singleton>();
    }

    // returns true if item was used successfully, false otherwise
    public virtual bool UseItem() {
        //Debug.Log("defaultItem");
        return false;
    }

    public virtual void WhileSelected() {

    }

    public virtual void OnSwitch() {
        if (lineRenderer != null && singleton != null) {
            lineRenderer.SetPositions(new Vector3[] { new Vector3(1000, -1000, 1000), new Vector3(1000, -1000, 1000) });
            //singleton.showGrid(false);
        }
    }

    public string GetName() {
        return itemName;
    }

    public int GetMaxStack() {
        return maxStack;
    }

    public float GetCooldown() {
        return cooldown;
    }

    public bool IsConsumable() {
        return consumable;
    }

    public Sprite GetIcon() {
        return icon;
    }

    //So items can be equal to items of same class
    public override bool Equals(object other) {
        if (other == null)
            return false;

        ItemObject item = other as ItemObject;
        if (item == null)
            return false;

        return (item.isStackable == isStackable) && (item.itemName == itemName) && (item.maxStack == maxStack);
    }

    public override int GetHashCode() {
        return itemName.GetHashCode() ^ maxStack.GetHashCode() ^ isStackable.GetHashCode();
    }
}
