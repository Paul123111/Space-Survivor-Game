using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


// V1.0 - reworked to not exist without an item
public class ItemStack : MonoBehaviour
{
    [SerializeField] ItemObject item; // cannot be null anymore
    TextMeshProUGUI nameDisplay;

    //Item item;
    int amount;

    private void Awake() {
        amount = 1;
        nameDisplay = GetComponentInChildren<TextMeshProUGUI>();    
    }

    private void Start() {
        item.SetUp();
    }

    // returns overflow from full stack
    public int IncreaseStack(int amount) {
        //print(amount);
        if (this.amount <= item.GetMaxStack()) {
            int prevAmount = this.amount;
            this.amount += amount;

            if (this.amount > item.GetMaxStack()) {
                this.amount = item.GetMaxStack();
                SetName();
                //print(item.GetMaxStack() - prevAmount);
                return item.GetMaxStack() - prevAmount;
            }

            SetName();
        }
        return amount;
    }

    public void DecreaseItem(int amount) {
        this.amount -= amount;
        SetName();
        if (this.amount <= 0) {
            ClearStack();
        }
    }

    public ItemObject GetItem() {
        return item;
    }

    public void SetItem(ItemObject item) {
        this.item = item;
    }

    public int GetAmount() {
        return amount;
    }

    public void ClearStack() {
        //print("deleted item");
        Destroy(gameObject);
    }

    public void SetName() {
        //print(amount + ", " + nameDisplay);
        if (nameDisplay != null)
            nameDisplay.text = " x" + amount;
    }
}
