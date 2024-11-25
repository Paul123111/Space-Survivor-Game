using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotSystem : MonoBehaviour
{
    Inventory inventory;
    [SerializeField] Transform[] robotSpawns;
    GameObject[] robots = new GameObject[4];

    // Start is called before the first frame update
    void Start()
    {
        inventory = GameObject.FindWithTag("ItemManager").GetComponent<Inventory>();
    }

    // Update is called once per frame
    //void Update()
    //{
        
    //}

    public void UpdateRobots() {
        for (int i = 0; i < 4; i++) {
            if (robots[i] != null) {
                Destroy(robots[i]);
            }
            RobotItem robotItem = inventory.GetRobot(i);
            print(robotItem);
            if (robotItem != null) {
                robots[i] = Instantiate(robotItem.GetRobot(), robotSpawns[i]);
            }
        }
    }
}
