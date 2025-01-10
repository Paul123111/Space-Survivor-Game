using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DemoEnemy : MonoBehaviour
{
    NavMeshAgent agent;
    [SerializeField] Transform[] waypoints;
    int currentIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(waypoints[currentIndex].position);
    }

    // Update is called once per frame
    void Update()
    {
        Wander();
    }

    void Wander() {
        agent.isStopped = false;

        if (Vector3.Distance(transform.position, waypoints[currentIndex].position) < 3) {

            currentIndex = (currentIndex+1)%4;
            agent.SetDestination(waypoints[currentIndex].position);
        }
    }

}
