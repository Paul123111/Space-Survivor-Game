using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TrashSlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData) {
        DraggableItem draggableItem = eventData.pointerDrag.GetComponent<DraggableItem>();
        
        if (draggableItem != null ) Destroy(draggableItem.gameObject);
    }

    private void OnTransformChildrenChanged() {
        if (transform.childCount > 0)
            Destroy(transform.GetChild(0).gameObject);
    }
}
