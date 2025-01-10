using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject[] enemy;
    [SerializeField] GameObject[] bossEnemies;
    [SerializeField] Grid grid;
    [SerializeField] int x;
    [SerializeField] int z;
    [SerializeField] DayNightCycle dayNightCycle;
    GameSceneManager gameSceneManager;
    [SerializeField] int maxEnemies = 5;
    [SerializeField] Tilemap walls;

    Transform player;
    bool boss;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        gameSceneManager = GameObject.Find("Singleton").GetComponent<GameSceneManager>();
        if (gameSceneManager != null) {
            maxEnemies += (gameSceneManager.GetDifficulty()*2);
            StartCoroutine(SpawnEnemies());
        }
    }

    // hard difficulty increases enemy cap and spawn rate
    int FindNumEnemies() {
        return GameObject.FindGameObjectsWithTag("EnemySpawned").Length;
    }

    IEnumerator SpawnEnemies() {
        for (;;) {
            if ((dayNightCycle.IsDay() || !gameSceneManager.IsInOverworld()) && FindNumEnemies() < maxEnemies) {
                Vector3 pos = new Vector3(Random.Range(player.position.x - x, player.position.x + x), 0.5f, Random.Range(player.position.z - z, player.position.z + z));

                int counter = 0; // failsafe for infinite loop
                while (Vector3.Distance(pos, player.position) < 10 || pos.x > 63 || pos.z > 63 || pos.x < -63 || pos.z < -63 || counter < 50 || walls.GetTile(grid.WorldToCell(pos)) != null) {
                    pos = new Vector3(Random.Range(player.position.x - x, player.position.x + x), 0.5f, Random.Range(player.position.z - z, player.position.z + z));
                    counter++;
                }

                float dayCountSpawn = (float) (dayNightCycle.GetDayCount() * ((gameSceneManager.GetDifficulty() * 5)+1))+1;
                //print(dayCountSpawn);

                boss = Random.Range(0f, 100f) < 5 * dayNightCycle.GetDayCount() * (gameSceneManager.GetDifficulty()+1);
                if (!boss) { //
                    Instantiate(enemy[Random.Range(0, enemy.Length)], pos, new Quaternion(0, 0, 0, 0));
                    yield return new WaitForSeconds(((8f - dayCountSpawn) > (3f / (gameSceneManager.GetDifficulty() + 1))) ? 8f - dayCountSpawn : (3f / (gameSceneManager.GetDifficulty() + 1)));
                } else {
                    Instantiate(bossEnemies[Random.Range(0, bossEnemies.Length)], pos, new Quaternion(0, 0, 0, 0));
                    yield return new WaitForSeconds(((14f - dayCountSpawn) > (5f / (gameSceneManager.GetDifficulty() + 1))) ? 14f - dayCountSpawn : (5f / (gameSceneManager.GetDifficulty() + 1)));
                }
            }
            yield return new WaitForSeconds(0.01f);
        }
    }

    // Update is called once per frame
    //void Update()
    //{
        
    //}
}
