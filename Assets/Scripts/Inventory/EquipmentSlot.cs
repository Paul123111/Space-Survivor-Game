using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipmentSlot : MonoBehaviour, IDropHandler
{

    [SerializeField] TextMeshProUGUI equipmentStat;
    [SerializeField] string noEquipmentStr;
    [SerializeField] string itemType;

    [SerializeField] GameObject UIItem;

    public void OnDrop(PointerEventData eventData) {
        //if (transform.childCount != 0) return;

        DraggableItem draggableItem = eventData.pointerDrag.GetComponent<DraggableItem>();
        //print(draggableItem.GetItemStack().GetItem().GetType().ToString());
        if (draggableItem != null && draggableItem.GetItemStack().GetItem().GetType().ToString().Equals(itemType)) {
            
            if (GetItemStack() != null) {
                GetItemStack().transform.SetParent(draggableItem.AccountForParentFull());
                draggableItem.SetParentAfterDrag(transform);
            } else {
                draggableItem.SetParentAfterDrag(transform);
            }

            UpdateStats();
        }
    }

    public void EquipItem(DraggableItem draggableItem) {
        if (GetItemStack() != null) {
            GetItemStack().transform.SetParent(draggableItem.transform.parent);
            draggableItem.transform.SetParent(transform);
        } else {
            draggableItem.transform.SetParent(transform);
        }

        UpdateStats();
    }

    public ItemStack GetItemStack() {
        if (transform.childCount == 0) return null;
        return gameObject.GetComponentInChildren<ItemStack>();
    }

    virtual public void UpdateStats() {
        if (GetItemStack() != null && GetItemStack().GetItem() != null && equipmentStat != null) {
            equipmentStat.text = ((EquippableItem)GetItemStack().GetItem()).GetEquippedDescription();
        } else {
            equipmentStat.text = noEquipmentStr;
        }
    }

    virtual public GameObject NewItem(ItemObject item) {
        //print(item.GetName());
        if (GetItemStack() == null) {
            GameObject newItem = Instantiate(UIItem, transform);
            newItem.GetComponent<ItemStack>().SetItem(item);
            newItem.GetComponent<Image>().sprite = item.GetIcon();
            equipmentStat.text = ((EquippableItem) newItem.GetComponent<ItemStack>().GetItem()).GetEquippedDescription();
            return newItem;
        }
        return null;
    }

    private void OnTransformChildrenChanged() {
        UpdateStats();
    }

    protected TextMeshProUGUI GetEquipmentStat() {
        return equipmentStat;
    }

    protected string GetNoEquipment() {
        return noEquipmentStr;
    }

    protected GameObject GetUIItem() {
        return UIItem;
    }
}
