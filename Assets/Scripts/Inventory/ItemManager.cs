using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class instantiates items based on ID - 0 is empty
public class ItemManager : MonoBehaviour
{
    [SerializeField] GameObject[] itemList;
    static GameObject[] items;

    [SerializeField] GameObject[] itemPickupList;
    static GameObject[] itemPickups;


    private void Start() {
        items = itemList;
        itemPickups = itemPickupList;
    }

    //public static Item CreateItem(int id) {
    //    return Instantiate(items[id]).GetComponent<Item>();
    //}

    //public static int GetID(Item item) {
    //    for (int i = 1; i < items.Length; i++) {
    //        if (items[i] == null) continue;
    //        if (items[i].GetComponent<Item>().Equals(item)) {
    //            return i;
    //        }
    //    }
    //    return 0;
    //}

    public static void CreateItemPickups(int id, Vector3 pos) {
        Instantiate(itemPickups[id], pos, new Quaternion(0, 0, 0, 0));
    }
}
