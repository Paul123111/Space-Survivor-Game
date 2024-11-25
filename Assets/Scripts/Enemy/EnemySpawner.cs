using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject[] enemy;
    [SerializeField] GameObject[] bossEnemies;
    [SerializeField] Grid grid;
    [SerializeField] int x;
    [SerializeField] int z;
    [SerializeField] DayNightCycle dayNightCycle;

    Transform player;
    bool boss;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies() {
        for (;;) {
            if (dayNightCycle.IsDay()) {
                Vector3 pos = new Vector3(Random.Range(player.position.x - x, player.position.x + x), 0.5f, Random.Range(player.position.z - z, player.position.z + z));
                while (Vector3.Distance(pos, player.position) < 10) {
                    pos = new Vector3(Random.Range(player.position.x - x, player.position.x + x), 0.5f, Random.Range(player.position.z - z, player.position.z + z));
                }

                boss = Random.Range(0f, 100f) < 20 * dayNightCycle.GetDayCount();
                if (!boss) {
                    Instantiate(enemy[Random.Range(0, enemy.Length)], pos, new Quaternion(0, 0, 0, 0));
                } else {
                    Instantiate(bossEnemies[Random.Range(0, bossEnemies.Length)], pos, new Quaternion(0, 0, 0, 0));
                }
            }
            yield return new WaitForSeconds((6f - dayNightCycle.GetDayCount() > 3f) ? 6f - dayNightCycle.GetDayCount() : 3f);
        }
    }

    // Update is called once per frame
    //void Update()
    //{
        
    //}
}
