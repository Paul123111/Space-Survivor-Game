using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CraftingRecipe", menuName = "Crafting/DefaultRecipe")]
public class CraftingRecipe : ScriptableObject
{
    [SerializeField] CraftingRequirement[] requirements;
    [SerializeField] ItemObject result;
    [SerializeField] int tier;

    public CraftingRequirement[] GetRequirements() {
        return requirements;
    }

    public ItemObject GetResult() {
        return result;
    }

    public int GetTier() {
        return tier;
    }

    [System.Serializable]
    public class CraftingRequirement {
        // requirement for a crafting recipe, eg 4 iron is amount=4, item=ironItem

        [SerializeField] ItemObject item;
        [SerializeField] int amount;

        public ItemObject GetItem() {
            return item;
        }

        public int GetAmount() {
            return amount;
        }
    }
}
