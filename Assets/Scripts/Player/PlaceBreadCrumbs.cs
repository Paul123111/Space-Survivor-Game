using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceBreadCrumbs : MonoBehaviour
{
    Vector3 previousPosition;
    float counter = 0;
    public GameObject BC;

    // Start is called before the first frame update
    void Start() {
        previousPosition = gameObject.transform.position;
    }

    // Update is called once per frame
    void Update() {
        Vector3 currentLocation = gameObject.transform.position;
        float distance = Vector3.Distance(previousPosition, currentLocation);
        if (distance > 1.0f) {
            previousPosition = currentLocation;
            GameObject g = Instantiate(BC, currentLocation, Quaternion.identity);
            g.name = "BC" + counter;
            counter++;
        }
    }
}
