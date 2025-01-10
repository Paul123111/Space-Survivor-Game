using System;
using System.Collections;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;

public class Inventory : InventoryHolder
{
    int activeSlot = 0;
    ItemObject activeItem;
    //UIInventorySlot[] UIInventorySlots = new UIInventorySlot[10];
    //ItemStack[] itemStacks;
    //Backpack backpack;

    // All possible item objects the player can hold
    [SerializeField] ItemObject[] itemList;
    //[SerializeField] GameObject[] UIItemList;

    Singleton singleton;

    [SerializeField] PauseGame pauseGame;

    EquipmentSlot armourSlot;
    UIInventorySlot UIArmourSlot;
    TextMeshProUGUI armourStat;

    EquipmentSlot[] robotSlots = new EquipmentSlot[4];
    //UIInventorySlot[] UIRobotSlots = new UIInventorySlot[4];
    //int numRobots = 0;

    RobotSystem robotSystem;

    EquipmentSlot flowerSlot;

    AudioSource miningSound;

    TrashSlot trashSlot;

    TextMeshProUGUI activeItemText;
    TextMeshProUGUI activeItemBackground;

    [SerializeField] bool tutorial = false;

    private void Start() {
        GameObject hotbar = GameObject.FindWithTag("Hotbar");
        GameObject fullInventorySlots = GameObject.Find("FullInventorySlots");
        UIInventorySlots = new UIInventorySlot[10];
        UIInventorySlots = hotbar.GetComponentsInChildren<UIInventorySlot>();
        UIInventorySlots = UIInventorySlots.Concat(fullInventorySlots.GetComponentsInChildren<UIInventorySlot>()).ToArray();

        singleton = GameObject.FindWithTag("Singleton").GetComponent<Singleton>();
        numSlots = UIInventorySlots.Length;

        armourSlot = GameObject.Find("ArmourSlot").GetComponent<EquipmentSlot>();

        robotSystem = GameObject.Find("RobotsRoot").GetComponent<RobotSystem>();

        for (int i = 0; i < 4; i++) {
            robotSlots[i] = GameObject.Find("RobotSlot" + (i + 1)).GetComponent<EquipmentSlot>();
            //UIRobotSlots[i] = GameObject.Find("RobotSlot" + (i + 1)).GetComponent<UIInventorySlot>();
        }

        flowerSlot = GameObject.Find("FlowerSlot").GetComponent<EquipmentSlot>();

        miningSound = GameObject.FindWithTag("UseSound").GetComponent<AudioSource>();

        trashSlot = fullInventorySlots.GetComponentInChildren<TrashSlot>();

        activeItemText = GameObject.Find("ActiveItemText").GetComponent<TextMeshProUGUI>();
        activeItemBackground = GameObject.Find("ActiveItemBackground").GetComponent<TextMeshProUGUI>();

        StartCoroutine(FillInventory());
        //ClearInventory();
        //if (!tutorial)
        //    LoadInventory();
        
        //ChangeHeldItem(0);
    }

    public override IEnumerator FillInventory() {
        yield return new WaitForEndOfFrame();

        ClearInventory();
        //for (int i = 1; i <= 10; i++) {
        //    for (int j = 0; j < PlayerPrefs.GetInt("ItemStack" + (i%10)); j++) {
        //        AddItemToInventory(PlayerPrefs.GetInt("ItemID" + (i%10) ));
        //        //print(PlayerPrefs.GetInt("ItemID" + i%10));
        //    }
        //}
        if (!tutorial)
            LoadInventory();


        //SetRobots();
        //SetArmour();
        //SetFlower();

        ChangeHeldItem(0);
        //OnSwitch();
    }

    public void ChangeHeldItem(int index) {
        RemovePlacementTile();
        if (UIInventorySlots[index].GetItemStack() != null && UIInventorySlots[index].GetItemStack().GetAmount() > 0) {
            activeItem = UIInventorySlots[index].GetItemStack().GetItem();
            activeItemBackground.text = "<mark=#00000099 padding=\"50,50,10,10\">> " + activeItem.GetName() + "</mark>";
            activeItemText.text = "> " + activeItem.GetName();
        } else {
            activeItem = null;
            activeItemText.text = "";
            activeItemBackground.text = "";
        }
        activeSlot = index;
        //print(activeItem.name);
    }

    void RemovePlacementTile() {
        //print(GameObject.Find("TileToBePlaced(Clone)"));
        Destroy(GameObject.Find("TileToBePlaced(Clone)"));
        Destroy(GameObject.Find("TileToBePlacedValid(Clone)"));
    }

    //public void AddItemFromID(int index, ItemObject item) {
    //    UIInventorySlots[index].GetItemStack().IncrementStack(item);
    //    if (UIInventorySlots[index].GetItemStack().GetItem() != null) {
    //        UIInventorySlots[index].SetName(UIInventorySlots[index].GetItemStack());
    //    } else {
    //        UIInventorySlots[index].SetName(null);
    //    }
    //}

    public void useActiveItem() {
        if (pauseGame.GetPaused()) return;

        if (activeItem != null) {
            if (activeItem.UseItem() && activeItem.IsConsumable()) {
                RemoveItemFromStack();
                //UpdateName(GetActiveSlot());
            }
        } else {
            //print("no item to use");
        }

        ChangeHeldItem(activeSlot);
    }

    public ItemObject GetActiveItem() {
        return activeItem;
    }

    public void HoldActiveItem() {
        if (pauseGame.GetPaused()) return;

        if (activeItem != null) {
            activeItem.WhileSelected();
        }
    }

    public void OnSwitch() {
        if (pauseGame.GetPaused()) return;

        if (activeItem != null) {
            activeItem.OnSwitch();
        } else {
            //singleton.showGrid(false);
            //miningSound.Stop();
        }
    }

    public int GetActiveSlot() {
        return activeSlot;
    }

    public int FirstValidSlotIndex(ItemObject item) {
        int index = -1;

        for (int i = 0; i < UIInventorySlots.Length; i++) {
            if (UIInventorySlots[i].GetItemStack() == null) {
                index = i;
                break;
            }
        }

        for (int i = 0; i < UIInventorySlots.Length; i++) {
            if (UIInventorySlots[i].GetItemStack() == null) continue;
            if (UIInventorySlots[i].GetItemStack().GetItem().Equals(item) && UIInventorySlots[i].GetItemStack().GetAmount() < item.GetMaxStack()) {
                index = i;
                break;
            }
        }

        return index;
    }

    public int FirstEmptySlotIndex(ItemObject item) {
        int index = -1;

        for (int i = 0; i < UIInventorySlots.Length; i++) {
            if (UIInventorySlots[i].GetItemStack() == null) {
                index = i;
                break;
            }
        }

        return index;
    }

    public GameObject FirstValidSlotObject(ItemObject item) {
        int index = FirstValidSlotIndex(item);
        //print((index != -1) ? UIInventorySlots[index].gameObject : trashSlot.gameObject);
        return (index != -1) ? UIInventorySlots[index].gameObject : trashSlot.gameObject;
    }

    public GameObject FirstEmptySlotObject(ItemObject item) {
        int index = FirstEmptySlotIndex(item);
        //print((index != -1) ? UIInventorySlots[index].gameObject : trashSlot.gameObject);
        return (index != -1) ? UIInventorySlots[index].gameObject : trashSlot.gameObject;
    }

    public override void AddItemToInventory(int id) {
        int index = -1;

        ItemObject item = itemList[id];

        for (int i = 0; i < UIInventorySlots.Length; i++) {
            if (UIInventorySlots[i].GetItemStack() == null) {
                index = i;
                break;
            }
        }

        for (int i = 0; i < UIInventorySlots.Length; i++) {
            if (UIInventorySlots[i].GetItemStack() == null) continue;
            if (UIInventorySlots[i].GetItemStack().GetItem().Equals(item) && UIInventorySlots[i].GetItemStack().GetAmount() < item.GetMaxStack()) {
                index = i;
                break;
            }
        }

        if (index == -1) {
            //print("inventory full");
            return;
        }

        //print(index);
        AddItemFromID(index, item);
    }

    public override void AddItemToInventory(ItemObject item) {
        int index = FirstValidSlotIndex(item);

        //print(index);
        AddItemFromID(index, item);
    }

    public void AddItemToIndex(int index, ItemObject item) {
        AddItemFromID(index, item);
    }

    // gives number of a certain item player is holding
    public int NumberOfItemsHeld(int id) {
        int numItems = 0;
        for (int i = 0; i < numSlots; i++) {
            if (UIInventorySlots[i].GetItemStack() == null) continue;
            if (UIInventorySlots[i].GetItemStack().GetItem().Equals(itemList[id])) {
                numItems += UIInventorySlots[i].GetItemStack().GetAmount();
            }
        }
        return numItems;
    }

    public int NumberOfItemsHeld(string name) {
        int numItems = 0;
        for (int i = 0; i < numSlots; i++) {
            if (UIInventorySlots[i].GetItemStack() == null) continue;
            if (UIInventorySlots[i].GetItemStack().GetItem().name.Equals(name)) {
                numItems += UIInventorySlots[i].GetItemStack().GetAmount();
            }
        }
        return numItems;
    }

    public bool RemoveItemFromInventory(ItemObject itemObject) {
        for (int i = 0; i < numSlots; i++) {
            if (UIInventorySlots[i].GetItemStack() == null) continue;
            if (UIInventorySlots[i].GetItemStack().GetItem().Equals(itemObject) && UIInventorySlots[i].GetItemStack().GetAmount() > 0) {
                //print(i);
                UIInventorySlots[i].GetItemStack().DecreaseItem(1);
                return true;
            }
        }

        //print("item missing");
        return false;
    }

    public void UpdateEquipmentAppearance(EquipmentSlot equipmentSlot) {
        StartCoroutine(UpdateMaterials(equipmentSlot));
    }

    IEnumerator UpdateMaterials(EquipmentSlot equipmentSlot) {
        yield return new WaitForEndOfFrame();
        equipmentSlot.ChangeMaterial();
    }

    //public override int NumberOfItemsHeld(ItemObject item) {
    //    int numItems = 0;
    //    for (int i = 0; i < 10; i++) {
    //        if (UIInventorySlots[i].GetItemStack().GetItem() == null) continue;
    //        if (UIInventorySlots[i].GetItemStack().GetItem().Equals(item)) {
    //            numItems += UIInventorySlots[i].GetItemStack().GetAmount();
    //        }
    //    }
    //    return numItems;
    //}

    public int FindItem(ItemObject item) {
        for (int i = 0; i < numSlots; i++) {
            if (UIInventorySlots[i].GetItemStack() == null) continue;
            if (UIInventorySlots[i].GetItemStack().GetItem().Equals(item)) {
                return i;
            }
        }
        return -1;
    }

    //public ItemStack[] GetItemStacks() {
    //    return itemStacks;
    //}

    public void RemoveItemFromStack() {
        UIInventorySlots[GetActiveSlot()].GetItemStack().DecreaseItem(1);
    }

    public void RemoveItemFromChosenStack(int slot) {
        //print(slot);
        UIInventorySlots[slot].GetItemStack().DecreaseItem(1);
        UpdateName(slot);
    }

    public void UpdateName(int index) {
        if (UIInventorySlots[index].GetItemStack() != null) {
            UIInventorySlots[index].GetItemStack().SetName();
        }
        ChangeHeldItem(index);
    }

    //public void ClearInventory() {
    //    for (int i = 0; i < 10; i++) {
    //        UIInventorySlots[i].GetItemStack().ClearStack();
    //        UIInventorySlots[i].SetName(null);
    //    }
    //}

    public string SaveInventory() {
        string[] inventoryData = new string[numSlots + 6];

        int i = 0;
        for (; i < numSlots; i++) {
            if (UIInventorySlots[i].GetItemStack() != null) {
                inventoryData[i] = FindIdOfItem(UIInventorySlots[i].GetItemStack().GetItem()).ToString() + "|" + UIInventorySlots[i].GetItemStack().GetAmount();
            } else {
                inventoryData[i] = "-1";
            }
        }

        // Saving Equipment
        if (GetArmour() != null) {
            inventoryData[i] = FindIdOfItem(armourSlot.GetItemStack().GetItem()).ToString() + "|" + armourSlot.GetItemStack().GetAmount();
        } else {
            inventoryData[i] = "-1";
        }

        i++;
        if (GetFlower() != null) {
            inventoryData[i] = FindIdOfItem(flowerSlot.GetItemStack().GetItem()).ToString() + "|" + flowerSlot.GetItemStack().GetAmount();
        } else {
            inventoryData[i] = "-1";
        }

        for (int j = 0; j < 4; j++) {
            i++;
            //print(i);
            if (GetRobot(j) != null) {
                inventoryData[i] = FindIdOfItem(robotSlots[j].GetItemStack().GetItem()).ToString() + "|" + robotSlots[j].GetItemStack().GetAmount();
            } else {
                inventoryData[i] = "-1";
            }
        }
        

        string dataToSave = string.Join(",", inventoryData);
        return dataToSave;
        //File.WriteAllText(Application.persistentDataPath + "/gameData.txt", "" + dataToSave);
    }

    public void LoadInventory() {
        string dataRead = File.ReadAllText(Application.persistentDataPath + "/gameData.txt");
        dataRead = dataRead.Split("*")[0];
        string[] inventorySlots = dataRead.Split(",");
        int i = 0;
        for (; i < UIInventorySlots.Length; i++) {
            if (inventorySlots[i] != "-1") {
                string[] itemStack = inventorySlots[i].Split("|");

                GameObject newItem = AddItemFromID(i, itemList[int.Parse(itemStack[0])]);
                if (int.Parse(itemStack[1]) > 1) {
                    newItem.GetComponent<ItemStack>().IncreaseStack(int.Parse(itemStack[1]) - 1);
                }

            }
        }

        // Loading Equipment
        if (inventorySlots[i] != "-1") {
            string[] itemStack = inventorySlots[i].Split("|");
            armourSlot.NewItem(itemList[int.Parse(itemStack[0])]);
        }
        i++;
        if (inventorySlots[i] != "-1") {
            string[] itemStack = inventorySlots[i].Split("|");
            flowerSlot.NewItem(itemList[int.Parse(itemStack[0])]);
        }
        for (int j = 0; j < 4; j++) {
            i++;
            if (inventorySlots[i] != "-1") {
                string[] itemStack = inventorySlots[i].Split("|");
                print(int.Parse(itemStack[0]));
                robotSlots[j].NewItem(itemList[int.Parse(itemStack[0])]);
            }
        }

    }

    public int FindIdOfItem(ItemObject item) {
        for (int i = 0; i < itemList.Length; i++) {
            if (itemList[i].Equals(item)) return i;
        }
        return -1;
    }

    public ItemObject[] GetItemList() {
        return itemList;
    }

    public void EquipArmour(int slot) {
        armourSlot.EquipItem(UIInventorySlots[slot].GetComponentInChildren<DraggableItem>());
    }

    public ArmourItem GetArmour() {
        if (armourSlot.GetItemStack() != null)
            return (ArmourItem) armourSlot.GetItemStack().GetItem();
        return null;
    }

    public void EquipFlower(int slot) {
        flowerSlot.EquipItem(UIInventorySlots[slot].GetComponentInChildren<DraggableItem>());
    }

    public FlowerItem GetFlower() {
        if (flowerSlot.GetItemStack() != null)
            return (FlowerItem) flowerSlot.GetItemStack().GetItem();
        return null;
    }

    // robots
    public void EquipRobot(int slot) {
        for (int i = 0; i < 4; i++) {
            if (robotSlots[i].GetItemStack() == null || i == 3) {
                robotSlots[i].EquipItem(UIInventorySlots[slot].GetComponentInChildren<DraggableItem>());
                robotSystem.UpdateRobots();
                return;
            }
        }
    }

    public RobotItem GetRobot(int index) {
        if (robotSlots[index].GetItemStack() != null)
            return (RobotItem)robotSlots[index].GetItemStack().GetItem();
        return null;
    }
}
