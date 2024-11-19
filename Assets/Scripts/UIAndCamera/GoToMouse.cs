using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToMouse : MonoBehaviour {
    [SerializeField] Camera playerCam;
    [SerializeField] Transform player;
    [SerializeField] float radius = 6f;
    [SerializeField] LayerMask layerMask;

    // Start is called before the first frame update
    void Start() {
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update() {
        GetToMouse();
    }

    private void GetToMouse() {
        //Vector3 mousePos = playerCam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
        //transform.position = new Vector3(mousePos.x, 0, mousePos.z);

        RaycastHit hit;
        Ray ray = playerCam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 1000, layerMask.value)) {
            Vector3 objectHit = hit.point;
            Vector3 cursorPos = new Vector3(objectHit.x, 0, objectHit.z);
            if (Vector3.Distance(cursorPos, player.position) < radius) { 
                transform.position = cursorPos;
                //print(cursorPos);
            } else {
                //print(player.position + (cursorPos.normalized * radius));
                transform.position = player.position + ((cursorPos-player.position).normalized * radius);
            }
        }
    }

    public Vector3 GiveMaxRadiusTarget() {
        RaycastHit hit;
        Ray ray = playerCam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 1000, layerMask.value)) {
            Vector3 objectHit = hit.point;
            Vector3 cursorPos = new Vector3(objectHit.x, 0, objectHit.z);
            return player.position + ((cursorPos - player.position).normalized * radius);
        }

        return new Vector3(0, 0, 0);
    }

}

