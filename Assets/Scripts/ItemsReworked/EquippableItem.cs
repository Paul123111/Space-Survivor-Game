using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquippableItem : ItemObject
{
    [SerializeField] string equippedDescription;
    [SerializeField] Color colour;

    public string GetEquippedDescription() {
        return equippedDescription;
    }

    public Color GetColour() {
        return colour;
    }
}
