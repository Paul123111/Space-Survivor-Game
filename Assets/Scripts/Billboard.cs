using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Credit - https://www.youtube.com/watch?v=gUeRdzD-e9U
public class Billboard : MonoBehaviour
{

    private Transform cameraTransform;

    // Start is called before the first frame update
    void Start()
    {
        cameraTransform = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(cameraTransform.position, Vector3.up);
        //transform.rotation = Quaternion.Euler(cameraTransform.rotation.eulerAngles.x, 0, cameraTransform.rotation.eulerAngles.z);
    }
}
