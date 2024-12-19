using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
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
    Image itemResultImage;

    // Start is called before the first frame update
    void Start()
    {
        inventory = GameObject.FindWithTag("ItemManager").GetComponent<Inventory>();
        GameObject craftingPanel = GameObject.Find("RecipePanel");
        craftingRecipeButtons = craftingPanel.GetComponentsInChildren<CraftingRecipeButton>();
        itemResultName = GameObject.Find("ItemResultName").GetComponent<TextMeshProUGUI>();
        itemResultImage = GameObject.Find("ItemResultImage").GetComponent<Image>();

        maxPages = (int) Mathf.Floor(allCraftingRecipes.Length / 3.0f);

        
    }

    // Update is called once per frame
    //void Update()
    //{
        
    //}

    public void CraftItem() {
        if (!CheckItems()) return;
        //print("has items");

        CraftingRecipe.CraftingRequirement[] craftingRequirements = currentRecipe.GetRequirements();
        ItemObject item;

        // improve this code
        for (int i = 0; i < craftingRequirements.Length; i++) {
            item = craftingRequirements[i].GetItem();
            for (int j = 0; j < craftingRequirements[i].GetAmount(); j++) {
                inventory.RemoveItemFromInventory(item);
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
        itemResultImage.sprite = craftingRecipe.GetResult().GetIcon();
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
        for (int i = page * 3; i < (page * 3) + 3; i++) {
            if (i >= allCraftingRecipes.Length) {
                craftingRecipeButtons[i % 3].RemoveButtonRecipe();
                continue;
            }
            craftingRecipeButtons[i % 3].ChangeButtonRecipe(allCraftingRecipes[i]);
        }
    }

    public void ViewCrafting() {
        page = 0;
        SetPage(page);
        gameObject.SetActive(true);
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
