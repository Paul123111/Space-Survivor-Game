using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FollowPlayer : MonoBehaviour
{
    Transform player;
    NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(player.position);

        StartCoroutine(CalculateFollow());
    }

    // Update is called once per frame
    //void Update()
    //{
    //    agent.CalculatePath(player.position, path);
    //}

    IEnumerator CalculateFollow()
    {
        for(;;)
        {
            agent.SetDestination(player.position);
            yield return new WaitForSeconds(0.25f);
        }
    }
}
