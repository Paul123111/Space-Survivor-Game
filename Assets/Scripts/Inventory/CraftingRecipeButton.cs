using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftingRecipeButton : MonoBehaviour
{
    CraftingSystem craftingSystem;
    CraftingRecipe craftingRecipe;

    [SerializeField] GameObject UICraftingItem;
    [SerializeField] GameObject equalsSign;
    [SerializeField] GameObject freeItem;

    // Start is called before the first frame update
    void Start()
    {
        craftingSystem = GameObject.Find("CraftingMenu").GetComponent<CraftingSystem>();
        //print(recipeDescription);
    }

    // Update is called once per frame
    //void Update()
    //{
        
    //}

    public void SetRecipe() {
        if (craftingRecipe != null)
            craftingSystem.SetCurrentRecipe(craftingRecipe);
    }

    public void ChangeButtonRecipe(CraftingRecipe craftingRecipe) {
        ClearCraftingRecipeUI();

        this.craftingRecipe = craftingRecipe;
        CraftingRecipe.CraftingRequirement[] craftingRequirements = craftingRecipe.GetRequirements();
        ItemObject result = craftingRecipe.GetResult();

        for (int i = 0; i < craftingRequirements.Length; i++) {
            GameObject newItem = Instantiate(UICraftingItem, transform);
            newItem.GetComponentsInChildren<Image>()[1].sprite = craftingRequirements[i].GetItem().GetIcon();
            TextMeshProUGUI[] newText = newItem.GetComponentsInChildren<TextMeshProUGUI>();
            newText[0].text = "x" + craftingRequirements[i].GetAmount();
            newText[1].text = craftingRequirements[i].GetItem().GetName();
        }
        
        GameObject equalSign = Instantiate(equalsSign, transform);
        if (craftingRequirements.Length == 0) {Destroy(equalSign); Instantiate(freeItem, transform); }

        GameObject newResult = Instantiate(UICraftingItem, transform);
        newResult.GetComponentsInChildren<Image>()[1].sprite = result.GetIcon();
        TextMeshProUGUI[] newResultText = newResult.GetComponentsInChildren<TextMeshProUGUI>();
        newResultText[0].text = "x1";
        newResultText[1].text = result.GetName();


    }

    public void RemoveButtonRecipe() {
        ClearCraftingRecipeUI();
        craftingRecipe = null;
    }

    void ClearCraftingRecipeUI() {
        for (int i = 0; i < transform.childCount; i++) {
            Destroy(transform.GetChild(i).gameObject);
        }
    }
}
