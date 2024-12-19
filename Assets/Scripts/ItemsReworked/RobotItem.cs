using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "armour", menuName = "Item/Robot")]
public class RobotItem : EquippableItem
{
    [SerializeField] GameObject robot;
    Inventory inventory;

    public override void SetUp() {
        inventory = GameObject.FindWithTag("ItemManager").GetComponent<Inventory>();
    }

    public override bool UseItem() {
        inventory.EquipRobot(inventory.GetActiveSlot());
        return true;
    }

    public GameObject GetRobot() {
        return robot;
    }
}
