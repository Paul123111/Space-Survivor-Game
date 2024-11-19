using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    int activeSlot = 1;
    ItemObject activeItem;
    UIInventorySlot[] UIInventorySlots = new UIInventorySlot[10];
    ItemStack[] itemStacks;

    // All possible item objects the player can hold
    [SerializeField] ItemObject[] itemList;

    Singleton singleton;

    private void Start() {
        GameObject hotbar = GameObject.FindWithTag("Hotbar");
        UIInventorySlots = hotbar.GetComponentsInChildren<UIInventorySlot>();
        itemStacks = hotbar.GetComponentsInChildren<ItemStack>();
        singleton = GameObject.FindWithTag("Singleton").GetComponent<Singleton>();

        for (int i = 1; i < itemList.Length; i++) {
            itemList[i].SetUp();
        }

        StartCoroutine(FillInventory());
    }

    IEnumerator FillInventory() {
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

    public void AddItemFromID(int index, ItemObject item) {
        itemStacks[index].UpdateStack(item);
        if (itemStacks[index].GetItem() != null) {
            UIInventorySlots[index].SetName(itemStacks[index]);
        } else {
            UIInventorySlots[index].SetName(null);
        }
    }

    public void useActiveItem() {
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
        if (activeItem != null) {
            activeItem.WhileSelected();
        }
    }

    public void OnSwitch() {
        if (activeItem != null) {
            activeItem.OnSwitch();
        } else {
            singleton.showGrid(false);
        }
    }

    public int GetActiveSlot() {
        return activeSlot;
    }

    public void AddItemToInventory(int id) {
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
            print("inventory full");
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

    public ItemStack[] GetItemStacks() {
        return itemStacks;
    }

    public void RemoveItemFromStack() {
        itemStacks[GetActiveSlot()].RemoveItem();
    }

    public void UpdateName(int index) {
        if (itemStacks[index].GetItem() != null) {
            UIInventorySlots[index].SetName(itemStacks[index]);
        } else {
            UIInventorySlots[index].SetName(null);
        }
        ChangeHeldItem(index);
    }

    public void ClearInventory() {
        for (int i = 0; i < 10; i++) {
            itemStacks[i].ClearStack();
            UIInventorySlots[i].SetName(null);
        }
    }

    public void SaveInventory() {
        for (int i = 0; i < 10; i++) {
            PlayerPrefs.SetInt("ItemID" + i, Array.IndexOf(itemList, itemStacks[i].GetItem()));
            print(PlayerPrefs.GetInt("ItemID" + i));
            PlayerPrefs.SetInt("ItemStack" + i, itemStacks[i].GetAmount());
        }
    }
}
