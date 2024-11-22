using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InventoryHolder : MonoBehaviour
{
    protected ItemStack[] itemStacks;
    protected int numSlots = 0;
    protected UIInventorySlot[] UIInventorySlots;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    //void Update()
    //{

    //}

    public void AddItemFromID(int index, ItemObject item) {
        itemStacks[index].UpdateStack(item);
        if (itemStacks[index].GetItem() != null) {
            UIInventorySlots[index].SetName(itemStacks[index]);
        } else {
            UIInventorySlots[index].SetName(null);
        }
    }

    public abstract IEnumerator FillInventory();

    public abstract void AddItemToInventory(ItemObject item);
    public abstract void AddItemToInventory(int id);

    public abstract void SaveInventory();
    //for (int i = 0; i < 10; i++) {
    //    PlayerPrefs.SetInt("ItemID" + i, Array.IndexOf(itemList, itemStacks[i].GetItem()));
    //    print(PlayerPrefs.GetInt("ItemID" + i));
    //    PlayerPrefs.SetInt("ItemStack" + i, itemStacks[i].GetAmount());
    //}
    //}

    public int NumberOfItemsHeld(ItemObject item) {
        int numItems = 0;
        for (int i = 0; i < numSlots; i++) {
            if (itemStacks[i].GetItem() == null) continue;
            if (itemStacks[i].GetItem().Equals(item)) {
                numItems += itemStacks[i].GetAmount();
            }
        }
        return numItems;
    }

    public void ClearInventory() {
        for (int i = 0; i < numSlots; i++) {
            itemStacks[i].ClearStack();
            UIInventorySlots[i].SetName(null);
        }
    }
}
