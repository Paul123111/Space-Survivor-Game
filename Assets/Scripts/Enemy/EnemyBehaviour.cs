using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour
{

    [SerializeField] float fieldOfViewDegrees = 30f;
    [SerializeField] float hearingDistance = 3f;
    [SerializeField] float smellDistance = 10f;
    [SerializeField] float attackRange = 5f;

    Vector3 direction;
    Vector3 leftAngle;
    Vector3 rightAngle;
    bool canSeePlayer = false;
    bool canHearPlayer = false;
    bool canSmellPlayer = false;
    RaycastHit hit;
    Transform player;
    float fieldOfViewCos;
    float fieldOfViewRadians;
    bool isInFieldOfView;

    NavMeshAgent agent;
    bool inAttackRange;
    //State currentState;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        fieldOfViewRadians = fieldOfViewDegrees * ( Mathf.PI / 180.0f );

        fieldOfViewCos = Mathf.Cos(fieldOfViewRadians);
        
        player = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        Look();
        Hear();
        Smell();

        print(canSeePlayer + ", " + canHearPlayer + ", " + canSmellPlayer);

        //if player
        if (canSeePlayer || canHearPlayer || canSmellPlayer) {
            agent.isStopped = false;
            ChasePlayer();
            transform.LookAt(player.position);
        } else {
            agent.isStopped = true;
        }
    }

    void Look() {

        direction = (player.position - transform.position).normalized;
        isInFieldOfView = (Vector3.Dot(transform.forward.normalized, direction) > fieldOfViewCos);

        //Debug.DrawRay(transform.position, direction * 100, Color.green);
        //Debug.DrawRay(transform.position, transform.forward * 100, Color.blue);

        //rightAngle = Quaternion.Euler(0, fieldOfViewDegrees, 0) * transform.forward;
        //Debug.DrawRay(transform.position, rightAngle * 100, Color.red);

        //leftAngle = Quaternion.Euler(0, -fieldOfViewDegrees, 0) * transform.forward;
        //Debug.DrawRay(transform.position, leftAngle * 100, Color.red);

        if (isInFieldOfView && Physics.Raycast(transform.position, direction * 100, out hit)) {
            if (hit.collider.gameObject.tag == "Player") canSeePlayer = true;
            else canSeePlayer = false;
        }

        
    }

    void Hear() {
        if (Vector3.Distance(transform.position, player.position) < hearingDistance) {
            canHearPlayer = true;
        } else {
            canHearPlayer = false;
        }
        //print(Vector3.Distance(transform.position, player.position) + ", " + hearingDistance);
    }

    void Smell() {
        GameObject[] breadCrumbs = GameObject.FindGameObjectsWithTag("BreadCrumb");
        foreach (GameObject bc in breadCrumbs) {
            if (Vector3.Distance(transform.position, bc.transform.position) < smellDistance) {
                canSmellPlayer = true;
                return;
            }
        }
        canSmellPlayer = false;
    }

    void ChasePlayer() {
        agent.destination = player.position;
    }

    void InRange() {
        if (Vector3.Distance(transform.position, player.position) < attackRange) {

        }
    }
}
