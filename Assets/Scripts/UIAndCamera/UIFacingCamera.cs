using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFacingCamera : MonoBehaviour
{
    Camera mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward, mainCamera.transform.rotation * Vector3.up);
    }
}
