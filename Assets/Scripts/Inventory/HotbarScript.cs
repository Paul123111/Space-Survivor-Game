using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class HotbarScript : MonoBehaviour
{
    //const int NUM_SLOTS = 10;

    [SerializeField] Sprite hotbarSlot;
    [SerializeField] Sprite selected;
    [SerializeField] PauseGame pauseGame;
    [SerializeField] PlayerStats playerStats;

    Inventory inventory;

    Image[] hotbarSlots = new Image[10];
    int selectedSlot = 0;
    bool onCooldown = false;

    //inventory is comprised of ints that give the item ID and the num of items in 1 slot
    //UIInventorySlot[] inventorySlots = new UIInventorySlot[10];

    LineRenderer lineRenderer;

    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        GameObject hotbar = GameObject.FindWithTag("Hotbar");
        anim = GameObject.FindWithTag("PlayerModel").GetComponent<Animator>();

        //print(hotbar.name);
        Image[] hotbarSlotsTemp = new Image[10]; ;
        for (int i = 0; i < hotbar.transform.childCount; ++i) {
            Transform child = hotbar.transform.GetChild(i);

            //execute functionality of child transform here
            hotbarSlotsTemp[i] = child.GetComponent<Image>();
        }

        for (int i = 0; i < hotbarSlots.Length; i++) {
            hotbarSlots[i] = hotbarSlotsTemp[i];
        }
        hotbarSlots[selectedSlot].sprite = selected;

        //inventorySlots = hotbar.GetComponentsInChildren<UIInventorySlot>();
        inventory = GameObject.FindWithTag("ItemManager").GetComponent<Inventory>();
        lineRenderer = GameObject.FindWithTag("Player").GetComponentInChildren<LineRenderer>();

        StartCoroutine(InitialSwitch());
        StartCoroutine(HeldItemEffect());
    }

    //Update is called once per frame
    void Update() {
        if (playerStats.IsDead()) return;
        if (Mouse.current.leftButton.isPressed) {
            if (!onCooldown)
                StartCoroutine(CheckingForUse());
        } else {
            // reset animations when not holding mouse
            anim.SetBool("isShooting", false);
        }
    }

    IEnumerator InitialSwitch() {
        if (playerStats.IsDead()) yield break;
        yield return new WaitForSeconds(0.05f);
        inventory.OnSwitch();
    }

    IEnumerator CheckingForUse() {
        if (playerStats.IsDead()) yield break;
        onCooldown = true;
        inventory.useActiveItem();

        if (inventory.GetActiveItem() == null) {
            yield return new WaitForSeconds(0.05f);
        } else {
            yield return new WaitForSeconds(inventory.GetActiveItem().GetCooldown());
        }

        onCooldown = false;
    }

    IEnumerator HeldItemEffect() {
        for (;;) {
            if (playerStats.IsDead()) yield break;
            if (inventory.GetActiveItem() != null) {
                lineRenderer.SetPositions(new Vector3[] { transform.position, transform.position });
                inventory.HoldActiveItem();
            } else {
                lineRenderer.SetPositions(new Vector3[] { new Vector3(1000, -1000, 1000), new Vector3(1000, -1000, 1000) });
            }
            yield return new WaitForSeconds(0.02f);
        }
    }

    void UseItem() {
        if (playerStats.IsDead()) return;
        inventory.useActiveItem();
    }

    void OnSlot1() {
        SlotX(0);
    }

    void OnSlot2() {
        SlotX(1);
    }

    void OnSlot3() {
        SlotX(2);
    }

    void OnSlot4() {
        SlotX(3);
    }

    void OnSlot5() {
        SlotX(4);
    }

    void OnSlot6() {
        SlotX(5);
    }

    void OnSlot7() {
        SlotX(6);
    }

    void OnSlot8() {
        SlotX(7);
    }

    void OnSlot9() {
        SlotX(8);
    }

    void OnSlot0() {
        SlotX(9);
    }

    void SlotX(int x) {
        if (playerStats.IsDead()) return;

        hotbarSlots[selectedSlot].sprite = hotbarSlot;
        selectedSlot = x;
        hotbarSlots[selectedSlot].sprite = selected;

        inventory.OnSwitch();
        inventory.ChangeHeldItem(x);
        inventory.OnSwitch();
    }

    public void RefreshSlot() {
        SlotX(inventory.GetActiveSlot());
    }

    void OnCycleInventoryLeft() {
        SlotX(negativeMod((selectedSlot - 1), 10));
    }

    void OnCycleInventoryRight() {
        SlotX((selectedSlot + 1) % 10);
    }

    // modulo returns a negative number when used in C# as it returns the remainder instead of the modulo
    int negativeMod(int x, int m) {
        return (x % m + m) % m;
    }
}
