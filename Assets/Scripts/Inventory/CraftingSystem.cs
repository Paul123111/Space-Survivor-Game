using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static CraftingRecipe;

public class CraftingSystem : MonoBehaviour
{
    Inventory inventory;
    CraftingRecipe currentRecipe;
    CraftingRecipeButton[] craftingRecipeButtons;

    [SerializeField] CraftingRecipe[] allCraftingRecipes;
    int page = 0;
    int maxPages = 0;

    TextMeshProUGUI itemResultName;

    // Start is called before the first frame update
    void Start()
    {
        inventory = GameObject.FindWithTag("ItemManager").GetComponent<Inventory>();
        GameObject craftingPanel = GameObject.Find("RecipePanel");
        craftingRecipeButtons = craftingPanel.GetComponentsInChildren<CraftingRecipeButton>();
        itemResultName = GameObject.Find("ItemResultName").GetComponent<TextMeshProUGUI>();

        maxPages = (int) Mathf.Floor(allCraftingRecipes.Length / 5.0f);

        SetPage(page);
    }

    // Update is called once per frame
    //void Update()
    //{
        
    //}

    public void CraftItem() {
        if (!CheckItems()) return;
        //print("has items");

        CraftingRecipe.CraftingRequirement[] craftingRequirements = currentRecipe.GetRequirements();
        int itemSlot;

        // improve this code
        for (int i = 0; i < craftingRequirements.Length; i++) {
            itemSlot = inventory.FindItem(craftingRequirements[i].GetItem());
            for (int j = 0; j < craftingRequirements[i].GetAmount(); j++) {
                inventory.RemoveItemFromChosenStack(itemSlot);
                //print(itemSlot);
            }
        }

        inventory.AddItemToInventory(currentRecipe.GetResult());
    }

    public CraftingRecipe GetCurrentRecipe() {
        return currentRecipe;
    }

    public void SetCurrentRecipe(CraftingRecipe craftingRecipe) {
        currentRecipe = craftingRecipe;
        itemResultName.text = craftingRecipe.GetResult().GetName();
    }

    bool CheckItems() {
        if (currentRecipe == null) return false;
        CraftingRecipe.CraftingRequirement[] craftingRequirements = currentRecipe.GetRequirements();

        for (int i = 0; i < craftingRequirements.Length; i++) {
            // check if number of required item in inventory is sufficient
            if (inventory.NumberOfItemsHeld(craftingRequirements[i].GetItem()) < craftingRequirements[i].GetAmount()) {
                return false;
            }
        }
        
        return true;
    }

    void SetPage(int page) {
        for (int i = page*5; i < (page*5)+5; i++) {
            if (i >= allCraftingRecipes.Length) {
                craftingRecipeButtons[i % 5].RemoveButtonRecipe();
                continue;
            }
            craftingRecipeButtons[i%5].ChangeButtonRecipe(allCraftingRecipes[i]);
        }
    }

    public void NextPage() {
        if (page < maxPages) page++;
        SetPage(page);
    }

    public void PrevPage() {
        if (page > 0) page--;
        SetPage(page);
    }
}
