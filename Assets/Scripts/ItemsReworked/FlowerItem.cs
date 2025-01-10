using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "flower", menuName = "Item/Flower")]
public class FlowerItem : EquippableItem
{

    [SerializeField] float oxygenRate;
    [SerializeField] float powerRate;
    Inventory inventory;

    [SerializeField] float damage = 1;

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

    public float GetDamage() {
        return damage;
    }
}
