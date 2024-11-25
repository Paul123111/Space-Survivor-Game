using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIInventorySlot : MonoBehaviour
{
    //[SerializeField] Item item;
    TextMeshProUGUI nameDisplay;
    //[SerializeField] int amount;

    // Start is called before the first frame update
    void Awake()
    {
        nameDisplay = GetComponentInChildren<TextMeshProUGUI>();
        //print(nameDisplay.text);
        //SetItem(item);
    }

    public void SetName(ItemStack itemStack) {
        if (itemStack != null && itemStack.GetItem() != null) {
            nameDisplay.text = itemStack.GetItem().GetName() + " x" + itemStack.GetAmount();
        } else {
            nameDisplay.text = "no item";
        } 
    }

    public void SetStringName(string name) {
        nameDisplay.text = name;
    }

    // Update is called once per frame
    //void Update()
    //{

    //}

    //public Item GetItem() {
    //    if (item != null)
    //        return item;
    //    return null;
    //}

    //public void SetItem(Item item) {
    //    this.item = item;
    //    if (this.item != null) {
    //        nameDisplay.text = this.item.GetName();
    //    } else {
    //        nameDisplay.text = "no item";
    //    }
    //}

    //public void AddItem(Item item) {
    //}
}
