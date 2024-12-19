using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIInventorySlot : MonoBehaviour, IDropHandler
{

    [SerializeField] GameObject UIItem;
    Inventory inventory;

    private void Start() {
        inventory = GameObject.FindWithTag("ItemManager").GetComponent<Inventory>();
    }

    // Credit for OnDrop: https://www.youtube.com/watch?v=kWRyZ3hb1Vc
    public void OnDrop(PointerEventData eventData) {
        //if (transform.childCount != 0) return;

        DraggableItem draggableItem = eventData.pointerDrag.GetComponent<DraggableItem>();
        if (draggableItem != null) {
            if (GetItemStack() != null) {
                if (draggableItem.GetItemStack().GetItem().Equals(GetItemStack().GetItem())) {
                    draggableItem.GetItemStack().DecreaseItem(GetItemStack().IncreaseStack(draggableItem.GetItemStack().GetAmount()));

                } else if (draggableItem.GetParentAfterDrag().GetComponent<UIInventorySlot>() != null) {
                    GetItemStack().transform.SetParent(draggableItem.AccountForParentFull());
                    draggableItem.SetParentAfterDrag(transform);
                }
            } else {
                draggableItem.SetParentAfterDrag(transform);
            }
        }
    }

    public ItemStack GetItemStack() {
        if (transform.childCount == 0) return null;
        //print(gameObject.GetComponentInChildren<DraggableItem>().GetItemStack());
        return gameObject.GetComponentInChildren<ItemStack>();
    }

    public GameObject NewItem(ItemObject item) {
        if (transform.childCount != 0) {
            //print(item.GetName());
            GetComponentInChildren<ItemStack>().IncreaseStack(1);
            return null;
        }
        GameObject newItem = Instantiate(UIItem, transform);
        newItem.GetComponent<ItemStack>().SetItem(item);
        newItem.GetComponent<Image>().sprite = item.GetIcon();
        return newItem;
    }

    //private void OnTransformChildrenChanged() {
    //    if (transform.childCount > 1) {
    //        Transform secondChild = transform.GetChild(1);
    //        secondChild.SetParent(inventory.FirstValidSlotObject(secondChild.GetComponent<ItemStack>().GetItem()).transform);
    //    }
    //}
}
