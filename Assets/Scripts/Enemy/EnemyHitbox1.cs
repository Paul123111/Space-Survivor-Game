using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitbox1 : MonoBehaviour
{
    [SerializeField] GameObject parent;
    EnemyLogic enemyLogic;

    // Start is called before the first frame update
    void Start()
    {
        enemyLogic = parent.GetComponent<EnemyLogic>();
    }

    // Update is called once per frame
    //void Update()
    //{

    //}

    private void OnTriggerEnter(Collider other) {
        //print(other.gameObject.tag);
        if (other.gameObject.tag == "Player") {
            enemyLogic.GetPlayerStats().SetOxygen(enemyLogic.GetPlayerStats().GetOxygen() - 20);
            Destroy(parent);
        }
    }
}
