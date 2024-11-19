using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerStatic : MonoBehaviour
{
    [SerializeField] GameObject[] enemy;
    [SerializeField] Grid grid;
    [SerializeField] int x;
    [SerializeField] int z;
    [SerializeField] float time;

    Transform player;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies() {
        for (;;) {
            Vector3 pos = new Vector3(Random.Range(transform.position.x - x, transform.position.x + x), transform.position.y, Random.Range(transform.position.z - z, transform.position.z + z));
            while (Vector3.Distance(pos, player.position) < 10) {
                pos = new Vector3(Random.Range(transform.position.x - x, transform.position.x + x), transform.position.y, Random.Range(transform.position.z - z, transform.position.z + z));
            }

            Instantiate(enemy[Random.Range(0, enemy.Length)], pos, new Quaternion(0, 0, 0, 0));
            yield return new WaitForSeconds(time);
        }
    }

    // Update is called once per frame
    //void Update()
    //{
        
    //}
}
