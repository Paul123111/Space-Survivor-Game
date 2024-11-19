using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class HotbarScript : MonoBehaviour
{
    //const int NUM_SLOTS = 10;

    [SerializeField] Sprite hotbarSlot;
    [SerializeField] Sprite selected;

    Inventory inventory;

    Image[] hotbarSlots = new Image[10];
    int selectedSlot = 1;
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
        Image[] hotbarSlotsTemp = hotbar.GetComponentsInChildren<Image>();
        for (int i = 0; i < hotbarSlots.Length; i++) {
            hotbarSlots[i] = hotbarSlotsTemp[i + 1];
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
        if (Mouse.current.leftButton.isPressed) {
            if (!onCooldown)
                StartCoroutine(CheckingForUse());
        } else {
            // reset animations when not holding mouse
            anim.SetBool("isShooting", false);
        }
    }

    IEnumerator InitialSwitch() {
        yield return new WaitForSeconds(0.05f);
        inventory.OnSwitch();
    }

    IEnumerator CheckingForUse() {
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
            if (inventory.GetActiveItem() != null) {
                lineRenderer.SetPositions(new Vector3[] { transform.position, transform.position });
                inventory.HoldActiveItem();
            }
            yield return new WaitForSeconds(0.02f);
        }
    }

    void UseItem() {
        inventory.useActiveItem();
    }

    void OnSlot1() {
        SlotX(1);
    }

    void OnSlot2() {
        SlotX(2);
    }

    void OnSlot3() {
        SlotX(3);
    }

    void OnSlot4() {
        SlotX(4);
    }

    void OnSlot5() {
        SlotX(5);
    }

    void OnSlot6() {
        SlotX(6);
    }

    void OnSlot7() {
        SlotX(7);
    }

    void OnSlot8() {
        SlotX(8);
    }

    void OnSlot9() {
        SlotX(9);
    }

    void OnSlot0() {
        SlotX(0);
    }

    void SlotX(int x) {
        hotbarSlots[selectedSlot].sprite = hotbarSlot;
        selectedSlot = x;
        hotbarSlots[selectedSlot].sprite = selected;

        inventory.OnSwitch();
        inventory.ChangeHeldItem(x);
        inventory.OnSwitch();
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
