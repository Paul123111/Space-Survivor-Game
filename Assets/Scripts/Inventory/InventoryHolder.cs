using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InventoryHolder : MonoBehaviour
{
    //protected ItemStack[] itemStacks;
    protected int numSlots = 0;
    protected UIInventorySlot[] UIInventorySlots;

    // Update is called once per frame
    //void Update()
    //{

    //}

    public GameObject AddItemFromID(int index, ItemObject item) {
        return UIInventorySlots[index].NewItem(item);
    }

    public abstract IEnumerator FillInventory();

    public abstract void AddItemToInventory(ItemObject UIItem);
    public abstract void AddItemToInventory(int id);

    //public abstract void SaveInventory();
    //for (int i = 0; i < 10; i++) {
    //    PlayerPrefs.SetInt("ItemID" + i, Array.IndexOf(itemList, UIInventorySlots[i].GetItemStack().GetItem()));
    //    print(PlayerPrefs.GetInt("ItemID" + i));
    //    PlayerPrefs.SetInt("ItemStack" + i, UIInventorySlots[i].GetItemStack().GetAmount());
    //}
    //}

    public int NumberOfItemsHeld(ItemObject item) {
        int numItems = 0;
        for (int i = 0; i < numSlots; i++) {
            if (UIInventorySlots[i].GetItemStack() == null) continue;
            if (UIInventorySlots[i].GetItemStack().GetItem().Equals(item)) {
                numItems += UIInventorySlots[i].GetItemStack().GetAmount();
            }
        }
        return numItems;
    }

    public void ClearInventory() {
        for (int i = 0; i < numSlots; i++) {
            if (UIInventorySlots[i].GetItemStack() != null) {
                UIInventorySlots[i].GetItemStack().ClearStack();
            }
        }
    }
}
