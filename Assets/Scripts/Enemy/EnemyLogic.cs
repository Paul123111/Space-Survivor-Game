using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyLogic : MonoBehaviour
{

    Transform[] waypoints;
    Transform player;
    int currentIndex = 1;
    NavMeshAgent agent;
    bool seePlayer = false;
    PlayerStats playerStats;

    // Start is called before the first frame update
    void Start()
    {
        waypoints = GameObject.FindWithTag("Waypoints").GetComponentsInChildren<Transform>();
        player = GameObject.FindWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        playerStats = GameObject.FindWithTag("PlayerObject").GetComponent<PlayerStats>();
    }

    // Update is called once per frame
    void Update() {
        //transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z);
        if (seePlayer) {
            ChasePlayer();
        } else {
            Wander();
        }
    }

    void Wander() {
        agent.destination = waypoints[currentIndex].position;

        if (Vector3.Distance(transform.position, waypoints[currentIndex].position) < 3) {

            int newIndex;
            do {
                newIndex = Random.Range(1, waypoints.Length);
            } while (newIndex == currentIndex);

            currentIndex = newIndex;
        }
    }

    void ChasePlayer() {
        agent.destination = player.position;
    }

    public void SetSeePlayer(bool seePlayer) {
        this.seePlayer = seePlayer;
    }

    //private void OnCollisionEnter(Collision collision) {
    //    print(collision.gameObject.tag);
    //    if (collision.gameObject.tag == "Player") {
    //        playerStats.SetOxygen(playerStats.GetOxygen() - 20);
    //        Destroy(gameObject);
    //    }
    //}

    public PlayerStats GetPlayerStats() {
        return playerStats;
    }

}
