using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.InputSystem;

// Credits: https://www.youtube.com/watch?v=kWRyZ3hb1Vc

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    Transform parentAfterDrag;
    Image image;
    ItemStack itemStack;
    Inventory inventory;

    bool dragActive = false;

    private void Start() {
        image = GetComponent<Image>();
        itemStack = GetComponent<ItemStack>();
        inventory = GameObject.FindWithTag("ItemManager").GetComponent<Inventory>();
    }

    public void OnBeginDrag(PointerEventData eventData) {
        if (!Mouse.current.rightButton.isPressed || Mouse.current.leftButton.isPressed) {
            dragActive = false;
            return;
        }
        
        //print("begin drag");

        dragActive = true;
        parentAfterDrag = transform.parent;

        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        image.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData) {
        //Debug.Log("dragging");
        if (!dragActive) return;
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData) {
        //Debug.Log("end drag");
        if (!dragActive) return;

        if (parentAfterDrag.childCount != 0) {
            //print("original inventory slot full");
            parentAfterDrag = inventory.FirstValidSlotObject(itemStack.GetItem()).transform;
        }

        transform.SetParent(parentAfterDrag);

        image.raycastTarget = true;
        dragActive = false;
    }

    public Transform GetParentAfterDrag() {
        return parentAfterDrag;
    }

    public void SetParentAfterDrag(Transform t) {
        parentAfterDrag = t;
    }

    public Transform AccountForParentFull() {
        if (parentAfterDrag.childCount != 0) {
            print("original inventory slot full");
            parentAfterDrag = inventory.FirstValidSlotObject(itemStack.GetItem()).transform;
        }
        return parentAfterDrag;
    }

    public ItemStack GetItemStack() {
        return itemStack;
    }

    public void SetItem(ItemStack itemStack) {
        this.itemStack = itemStack;
    }
}
