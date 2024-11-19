using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WanderingState : EnemyState
{
    [SerializeField] float fieldOfViewDegrees = 30f;
    [SerializeField] float hearingDistance = 3f;
    [SerializeField] float smellDistance = 10f;

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

    [SerializeField] NavMeshAgent agent;
    [SerializeField] FaceTarget faceTarget;

    [SerializeField] ChaseState chaseState;
    [SerializeField] LayerMask layerMask;

    [SerializeField] GameObject playerDetectedUI;
    [SerializeField] EnemyHurtbox enemyHurtbox;

    [SerializeField] Animator anim;
    AnimatorStateInfo info;

    Transform[] waypoints;
    int currentIndex = 1;
    [SerializeField] Transform enemyBody;

    // Start is called before the first frame update
    void Start() {
        //agent = GameObject.FindWithTag("Enemy").GetComponent<NavMeshAgent>();
        fieldOfViewRadians = fieldOfViewDegrees * (Mathf.PI / 180.0f);
        fieldOfViewCos = Mathf.Cos(fieldOfViewRadians);
        player = GameObject.FindWithTag("Player").transform;
        waypoints = GameObject.FindWithTag("Waypoints").GetComponentsInChildren<Transform>();
    }

    public override EnemyState RunCurrentState() {

        info = anim.GetCurrentAnimatorStateInfo(0);

        //if (info.IsName("Idle")) {
            Look();
            Hear();
            Smell();
        //}

        //print("Wandering: " + canSeePlayer + ", " + canHearPlayer + ", " + canSmellPlayer + ", " + player.transform.position);

        //if player is detected
        if (IsPlayerDetected()) {
            anim.SetBool("wandering", false);
            playerDetectedUI.SetActive(true);
            return chaseState;
        } else {
            Wander();
            return this;
        };
    }

    void StopMoving() {
        faceTarget.SetLookingAtTarget(false);
        agent.isStopped = true;
        playerDetectedUI.SetActive(false);
    }

    public void Look() {

        direction = (player.position - transform.position).normalized;
        isInFieldOfView = (Vector3.Dot(transform.forward.normalized, direction) > fieldOfViewCos);

        //Debug.DrawRay(transform.position, direction * 100, Color.green);
        //Debug.DrawRay(transform.position, transform.forward * 100, Color.blue);

        //rightAngle = Quaternion.Euler(0, fieldOfViewDegrees, 0) * transform.forward;
        //Debug.DrawRay(transform.position, rightAngle * 100, Color.red);

        //leftAngle = Quaternion.Euler(0, -fieldOfViewDegrees, 0) * transform.forward;
        //Debug.DrawRay(transform.position, leftAngle * 100, Color.red);

        if (isInFieldOfView && Physics.Raycast(transform.position, direction * 100, out hit, 100, layerMask.value)) {
            if (hit.collider.gameObject.tag == "Player") { 
                canSeePlayer = true;
            } else { 
                canSeePlayer = false;
            }
            //print(hit.collider.gameObject.tag);
            return;
        }

        canSeePlayer = false;
    }

    public void Hear() {
        if (Vector3.Distance(transform.position, player.position) < hearingDistance) {
            canHearPlayer = true;
        } else {
            canHearPlayer = false;
        }
        //print(Vector3.Distance(transform.position, player.position) + ", " + hearingDistance);
    }

    public void Smell() {
        GameObject[] breadCrumbs = GameObject.FindGameObjectsWithTag("BreadCrumb");
        foreach (GameObject bc in breadCrumbs) {
            if (Vector3.Distance(transform.position, bc.transform.position) < smellDistance) {
                canSmellPlayer = true;
                return;
            }
        }
        canSmellPlayer = false;
    }

    public bool IsPlayerDetected() {
        
        if (canSeePlayer || canHearPlayer || canSmellPlayer || enemyHurtbox.WasHitRecentlyByPlayer()) {
            anim.SetBool("playerDetected", true);
            return true;
        } else {
            anim.SetBool("playerDetected", false);
            return false;
        }
    }

    void Wander() {
        agent.isStopped = false;
        agent.destination = waypoints[currentIndex].position;
        faceTarget.SetLookingAtTarget(false);
        playerDetectedUI.SetActive(false);
        anim.SetBool("wandering", true);
        Vector3 direction = (waypoints[currentIndex].position - enemyBody.position);
        enemyBody.rotation = Quaternion.Slerp(enemyBody.rotation, Quaternion.LookRotation(new Vector3(direction.x, enemyBody.position.y, direction.z)), 10 * Time.deltaTime);

        if (Vector3.Distance(transform.position, waypoints[currentIndex].position) < 3) {

            int newIndex;
            do {
                newIndex = Random.Range(1, waypoints.Length);
            } while (newIndex == currentIndex);

            currentIndex = newIndex;
        }
        
        
    }

}
