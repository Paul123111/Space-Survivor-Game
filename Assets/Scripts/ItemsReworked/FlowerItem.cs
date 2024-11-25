using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "flower", menuName = "Item/Flower")]
public class FlowerItem : ItemObject
{

    [SerializeField] float oxygenRate;
    [SerializeField] float powerRate;
    Inventory inventory;

    public override void SetUp() {
        inventory = GameObject.FindWithTag("ItemManager").GetComponent<Inventory>();
        //Debug.Log(inventory);
    }

    public override bool UseItem() {
        inventory.EquipFlower(inventory.GetActiveSlot());
        return true;
    }

    public float GetOxygenRate() {
        return oxygenRate;
    }

    public float GetPowerRate() {
        return powerRate;
    }
}
