using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToTargetWithoutRotating : MonoBehaviour
{

    [SerializeField] Transform target;
    [SerializeField] float rotateSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = target.position;
        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y + (rotateSpeed * Time.deltaTime), transform.eulerAngles.z);
    }
}
