using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemStack : MonoBehaviour
{
    ItemObject item;
    
    //Item item;
    int amount;

    private void Start() {
        item = null;
    }

    public ItemStack() {
        item = null;
        amount = 0;
    }

    //return true if item is new
    public bool UpdateStack(ItemObject item) {
        if (this.item != null) {
            if (this.item.Equals(item) && amount < this.item.GetMaxStack()) {
                amount++;
                return false;
            } else if (!this.item.Equals(item)) {
                this.item = item;
                amount = 1;
                return true;
            }
        } else {
            this.item = item;
            if (item != null) {
                amount = 1;
            } else {
                amount = 0;
            }
        }
        return true;
    }

    public void RemoveItem() {
        amount--;
        if (amount <= 0) {
            amount = 0;
            item = null;
        }
    }

    public ItemObject GetItem() {
        return item;
    }

    public int GetAmount() {
        return amount;
    }

    public void ClearStack() {
        item = null;
        amount = 0;
    }
}
