using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceTarget : MonoBehaviour
{
    Transform target;
    bool lookingAtTarget;
    [SerializeField] float rotateSpeed;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindWithTag("Player").transform;
        transform.rotation = Quaternion.Euler(new Vector3(0, Random.Range(0f, 360f), 0));
    }

    // Update is called once per frame
    void Update()
    {
        if (lookingAtTarget)
            Rotate();
    }

    void Rotate() {
        Vector3 direction = new Vector3(target.position.x, transform.position.y, target.position.z) - transform.position;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotateSpeed * Time.deltaTime);
    }

    public void SetLookingAtTarget(bool lookingAtTarget) {
        this.lookingAtTarget = lookingAtTarget;
    }
}
