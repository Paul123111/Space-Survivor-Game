using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

[CreateAssetMenu(fileName = "armour", menuName = "Item/Armour")]
public class ArmourItem : ItemObject
{
    [SerializeField] float damageReduction = 0.4f; // 40% damage reduction
    Inventory inventory;

    public override void SetUp() {
        inventory = GameObject.FindWithTag("ItemManager").GetComponent<Inventory>();
        //Debug.Log(inventory);
    }

    public override bool UseItem() {
        inventory.EquipArmour(inventory.GetActiveSlot());
        return true;
    }

    public float GetDamageReduction() {
        return damageReduction;
    }

}
