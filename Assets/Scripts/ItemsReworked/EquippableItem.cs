using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquippableItem : ItemObject
{
    [SerializeField] string equippedDescription;

    public string GetEquippedDescription() {
        return equippedDescription;
    }
}
