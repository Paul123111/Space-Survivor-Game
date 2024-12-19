using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RobotSlot : EquipmentSlot
{

    RobotSystem robotSystem;

    private void Start() {
        robotSystem = GameObject.Find("RobotsRoot").GetComponent<RobotSystem>();
    }

    public override void UpdateStats() {
        GetEquipmentStat().text = robotSystem.RobotList();
    }

    private void OnTransformChildrenChanged() {
        robotSystem.UpdateRobots();
        UpdateStats();
    }

    public override GameObject NewItem(ItemObject item) {
        if (GetItemStack() == null) {
            GameObject newItem = Instantiate(GetUIItem(), transform);
            newItem.GetComponent<ItemStack>().SetItem(item);
            newItem.GetComponent<Image>().sprite = item.GetIcon();

            robotSystem.UpdateRobots();
            UpdateStats();
            return newItem;
        }
        return null;
    }

}
