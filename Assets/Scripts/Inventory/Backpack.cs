using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Backpack : InventoryHolder
{
    //ItemStack[] itemStacks;
    //int numSlots = 40;
    Inventory inventory;

    // Start is called before the first frame update
    void Start()
    {
        GameObject backpack = GameObject.Find("FullInventorySlots");
        inventory = GetComponent<Inventory>();
        itemStacks = backpack.GetComponentsInChildren<ItemStack>();
        UIInventorySlots = backpack.GetComponentsInChildren<UIInventorySlot>();
        numSlots = itemStacks.Length;
    }

    // Update is called once per frame
    //void Update()
    //{

    //}

    public override IEnumerator FillInventory() {
        yield return new WaitForEndOfFrame();

        ClearInventory();
        for (int i = 10; i < numSlots+10; i++) {
            for (int j = 0; j < PlayerPrefs.GetInt("ItemStack" + (i)); j++) {
                AddItemToInventory(PlayerPrefs.GetInt("ItemID" + (i)));
                //print(PlayerPrefs.GetInt("ItemID" + i));
            }
        }
        //OnSwitch();
    }

    public override void AddItemToInventory(ItemObject item) {
        int index = -1;
        //Item item = ItemManager.CreateItem(id);
        ItemObject itemObject = item;
        for (int i = 0; i < numSlots; i++) {
            if (itemStacks[i].GetItem() == null) {
                //print(itemStacks[i % 10].GetItem());
                index = i;
                break;
            }
        }

        for (int i = 0; i < numSlots; i++) {
            if (itemStacks[i].GetItem() == null) continue;
            if (itemStacks[i].GetItem().Equals(itemObject) && itemStacks[i].GetAmount() < itemObject.GetMaxStack()) {
                //print(itemStacks[i % 10].GetItem());
                index = i;
                break;
            }
        }

        if (index == -1) {
            print("inventory full");
            return;
        }
        //print(index);
        AddItemFromID(index, itemObject);
    }

    public override void SaveInventory() {
        for (int i = 10; i < numSlots+10; i++) {
            PlayerPrefs.SetInt("ItemID" + i, Array.IndexOf(inventory.GetItemList(), itemStacks[i].GetItem()));
            print(PlayerPrefs.GetInt("ItemID" + i));
            PlayerPrefs.SetInt("ItemStack" + i, itemStacks[i].GetAmount());
        }
    }

    public override void AddItemToInventory(int id) {
        int index = -1;
        //Item item = ItemManager.CreateItem(id);
        ItemObject itemObject = inventory.GetItemList()[id];
        for (int i = 0; i < numSlots; i++) {
            if (itemStacks[i].GetItem() == null) {
                //print(itemStacks[i % 10].GetItem());
                index = i;
                break;
            }
        }

        for (int i = 0; i < numSlots; i++) {
            if (itemStacks[i].GetItem() == null) continue;
            if (itemStacks[i].GetItem().Equals(itemObject) && itemStacks[i].GetAmount() < itemObject.GetMaxStack()) {
                //print(itemStacks[i % 10].GetItem());
                index = i;
                break;
            }
        }

        if (index == -1) {
            print("inventory full");
            return;
        }
        //print(index);
        AddItemFromID(index, itemObject);
    }

    public void UpdateName(int index) {
        if (itemStacks[index].GetItem() != null) {
            UIInventorySlots[index].SetName(itemStacks[index]);
        } else {
            UIInventorySlots[index].SetName(null);
        }
    }

    public void RemoveItemFromChosenStack(int slot) {
        itemStacks[slot].RemoveItem();
        UpdateName(slot);
    }

    public ItemStack[] GetItemStacks() {
        return itemStacks;
    }

    public bool backpackFull(ItemObject itemObject) {
        int index = -1;
        for (int i = 0; i < numSlots; i++) {
            if (itemStacks[i].GetItem() == null) {
                //print(itemStacks[i % 10].GetItem());
                return false;
            }
        }

        for (int i = 0; i < numSlots; i++) {
            if (itemStacks[i].GetItem() == null) continue;
            if (itemStacks[i].GetItem().Equals(itemObject) && itemStacks[i].GetAmount() < itemObject.GetMaxStack()) {
                //print(itemStacks[i % 10].GetItem());
                return false;
            }
        }

        if (index == -1) {
            //print("inventory full");
            return true;
        }

        return false;
    }
}
