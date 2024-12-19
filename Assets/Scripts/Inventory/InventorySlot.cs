using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlot : MonoBehaviour
{

    ItemStack itemStack;

    // Start is called before the first frame update
    void Start()
    {

    }

    //public void AddItem(ItemObject item) {
    //    itemStack.IncrementStack(item);
    //}

    public ItemStack GetItemStack() {
        return itemStack;
    }
}
