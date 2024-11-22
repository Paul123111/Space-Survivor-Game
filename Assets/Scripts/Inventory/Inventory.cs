using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class Inventory : InventoryHolder
{
    int activeSlot = 1;
    ItemObject activeItem;
    //UIInventorySlot[] UIInventorySlots = new UIInventorySlot[10];
    //ItemStack[] itemStacks;
    Backpack backpack;

    // All possible item objects the player can hold
    [SerializeField] ItemObject[] itemList;

    Singleton singleton;

    [SerializeField] PauseGame pauseGame;

    private void Start() {
        GameObject hotbar = GameObject.FindWithTag("Hotbar");
        UIInventorySlots = new UIInventorySlot[10];
        UIInventorySlots = hotbar.GetComponentsInChildren<UIInventorySlot>();
        itemStacks = hotbar.GetComponentsInChildren<ItemStack>();
        singleton = GameObject.FindWithTag("Singleton").GetComponent<Singleton>();
        backpack = GetComponent<Backpack>();
        numSlots = 10;

        for (int i = 1; i < itemList.Length; i++) {
            itemList[i].SetUp();
        }

        StartCoroutine(FillInventory());
    }

    public override IEnumerator FillInventory() {
        yield return new WaitForEndOfFrame();

        ClearInventory();
        for (int i = 1; i <= 10; i++) {
            for (int j = 0; j < PlayerPrefs.GetInt("ItemStack" + (i%10)); j++) {
                AddItemToInventory(PlayerPrefs.GetInt("ItemID" + (i%10) ));
                //print(PlayerPrefs.GetInt("ItemID" + i));
            }
        }

        ChangeHeldItem(1);
        //OnSwitch();
    }

    public void ChangeHeldItem(int index) {
        activeItem = itemStacks[index].GetItem();
        activeSlot = index;
    }

    //public void AddItemFromID(int index, ItemObject item) {
    //    itemStacks[index].UpdateStack(item);
    //    if (itemStacks[index].GetItem() != null) {
    //        UIInventorySlots[index].SetName(itemStacks[index]);
    //    } else {
    //        UIInventorySlots[index].SetName(null);
    //    }
    //}

    public void useActiveItem() {
        if (pauseGame.GetPaused()) return;

        if (activeItem != null) {
            if (activeItem.UseItem() && activeItem.IsConsumable()) {
                RemoveItemFromStack();
                UpdateName(GetActiveSlot());
            }
        } else {
            print("no item to use");
        }
    }

    public ItemObject GetActiveItem() {
        return activeItem;
    }

    public void HoldActiveItem() {
        if (pauseGame.GetPaused()) return;

        if (activeItem != null) {
            activeItem.WhileSelected();
        }
    }

    public void OnSwitch() {
        if (pauseGame.GetPaused()) return;

        if (activeItem != null) {
            activeItem.OnSwitch();
        } else {
            singleton.showGrid(false);
        }
    }

    public int GetActiveSlot() {
        return activeSlot;
    }

    public override void AddItemToInventory(int id) {
        int index = -1;
        //Item item = ItemManager.CreateItem(id);
        ItemObject itemObject = itemList[id];
        for (int i = 1; i <= 10; i++) {
            if (itemStacks[i % 10].GetItem() == null) {
                //print(itemStacks[i % 10].GetItem());
                index = i % 10;
                break;
            }
        }

        for (int i = 1; i <= 10; i++) {
            if (itemStacks[i % 10].GetItem() == null) continue;
            if (itemStacks[i % 10].GetItem().Equals(itemObject) && itemStacks[i % 10].GetAmount() < itemObject.GetMaxStack()) {
                //print(itemStacks[i % 10].GetItem());
                index = i % 10;
                break;
            }
        }

        if (index == -1) {
            //print("inventory full");
            backpack.AddItemToInventory(id);
            return;
        }
        //print(index);
        AddItemFromID(index, itemObject);
    }

    public override void AddItemToInventory(ItemObject item) {
        int index = -1;
        //Item item = ItemManager.CreateItem(id);
        ItemObject itemObject = item;
        for (int i = 1; i <= 10; i++) {
            if (itemStacks[i % 10].GetItem() == null) {
                //print(itemStacks[i % 10].GetItem());
                index = i % 10;
                break;
            }
        }

        for (int i = 1; i <= 10; i++) {
            if (itemStacks[i % 10].GetItem() == null) continue;
            if (itemStacks[i % 10].GetItem().Equals(itemObject) && itemStacks[i % 10].GetAmount() < itemObject.GetMaxStack()) {
                //print(itemStacks[i % 10].GetItem());
                index = i % 10;
                break;
            }
        }

        if (index == -1) {
            //print("inventory full");
            backpack.AddItemToInventory(item);
            return;
        }
        //print(index);
        AddItemFromID(index, itemObject);
    }

    // gives number of a certain item player is holding
    public int NumberOfItemsHeld(int id) {
        int numItems = 0;
        for (int i = 0; i < 10; i++) {
            if (itemStacks[i].GetItem() == null) continue;
            if (itemStacks[i].GetItem().Equals(itemList[id])) {
                numItems += itemStacks[i].GetAmount();
            }
        }
        return numItems;
    }

    public int NumberOfItemsHeld(string name) {
        int numItems = 0;
        for (int i = 0; i < 10; i++) {
            if (itemStacks[i].GetItem() == null) continue;
            if (itemStacks[i].GetItem().name.Equals(name)) {
                numItems += itemStacks[i].GetAmount();
            }
        }
        return numItems;
    }

    //public override int NumberOfItemsHeld(ItemObject item) {
    //    int numItems = 0;
    //    for (int i = 0; i < 10; i++) {
    //        if (itemStacks[i].GetItem() == null) continue;
    //        if (itemStacks[i].GetItem().Equals(item)) {
    //            numItems += itemStacks[i].GetAmount();
    //        }
    //    }
    //    return numItems;
    //}

    public int FindItem(ItemObject item) {
        for (int i = 1; i <= 10; i++) {
            if (itemStacks[i % 10].GetItem() == null) continue;
            if (itemStacks[i % 10].GetItem().Equals(item)) {
                return i % 10;
            }
        }
        return -1;
    }

    public ItemStack[] GetItemStacks() {
        return itemStacks;
    }

    public void RemoveItemFromStack() {
        itemStacks[GetActiveSlot()].RemoveItem();
    }

    public void RemoveItemFromChosenStack(int slot) {
        itemStacks[slot].RemoveItem();
        UpdateName(slot);
    }

    public void UpdateName(int index) {
        if (itemStacks[index].GetItem() != null) {
            UIInventorySlots[index].SetName(itemStacks[index]);
        } else {
            UIInventorySlots[index].SetName(null);
        }
        ChangeHeldItem(index);
    }

    //public void ClearInventory() {
    //    for (int i = 0; i < 10; i++) {
    //        itemStacks[i].ClearStack();
    //        UIInventorySlots[i].SetName(null);
    //    }
    //}

    public override void SaveInventory() {
        for (int i = 0; i < 10; i++) {
            PlayerPrefs.SetInt("ItemID" + i, Array.IndexOf(itemList, itemStacks[i].GetItem()));
            print(PlayerPrefs.GetInt("ItemID" + i));
            PlayerPrefs.SetInt("ItemStack" + i, itemStacks[i].GetAmount());
        }
    }

    public ItemObject[] GetItemList() {
        return itemList;
    }

    public void TranferToBackpack(int slot) {
        if (!pauseGame.GetPaused() || itemStacks[slot] == null) return;
        ItemObject item = itemStacks[slot].GetItem();
        if (backpack.backpackFull(item)) return;

        RemoveItemFromChosenStack(slot);
        UpdateName(slot);
        backpack.AddItemToInventory(item);
        ChangeHeldItem(1);
    }

    public void TranferToInventory(int slot) {
        if (!pauseGame.GetPaused() || backpack.GetItemStacks()[slot] == null) return;
        ItemObject item = backpack.GetItemStacks()[slot].GetItem();
        backpack.RemoveItemFromChosenStack(slot);
        backpack.UpdateName(slot);
        AddItemToInventory(item);
        ChangeHeldItem(1);
    }
}
