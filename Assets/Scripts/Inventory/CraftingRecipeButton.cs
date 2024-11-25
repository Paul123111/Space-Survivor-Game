using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CraftingRecipeButton : MonoBehaviour
{
    CraftingSystem craftingSystem;
    CraftingRecipe craftingRecipe;
    TextMeshProUGUI recipeDescription;
    

    // Start is called before the first frame update
    void Start()
    {
        craftingSystem = GameObject.Find("CraftingMenu").GetComponent<CraftingSystem>();
        recipeDescription = GetComponentInChildren<TextMeshProUGUI>();
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
        this.craftingRecipe = craftingRecipe;
        CraftingRecipe.CraftingRequirement[] craftingRequirements = craftingRecipe.GetRequirements();
        string str = "";
        for (int i = 0; i < craftingRequirements.Length; i++) {
            str += craftingRequirements[i].GetAmount();
            str += "x ";
            str += craftingRequirements[i].GetItem().GetName();

            if (i < craftingRequirements.Length - 1) str += " + ";
        }
        str += " = " + craftingRecipe.GetResult().GetName();

        if (recipeDescription != null)
            recipeDescription.text = str;
    }

    public void RemoveButtonRecipe() {
        craftingRecipe = null;
        recipeDescription.text = "";
    }
}
