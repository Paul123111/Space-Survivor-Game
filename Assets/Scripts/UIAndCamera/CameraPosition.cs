using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraMouse : MonoBehaviour {
    [SerializeField] CinemachineVirtualCamera playerCam;
    [SerializeField] Transform targetGroup;
    [SerializeField] Transform player;
    [SerializeField] float speed = 3f;
    Quaternion fromRotation;
    //Vector2 cameraPos;
    PauseGame pauseGame;
    bool startCamera = false;

    // Start is called before the first frame update
    void Start() {
        fromRotation = Quaternion.Euler(new Vector3(75f, 0f, 0f));
        pauseGame = GameObject.FindWithTag("Player").GetComponent<PauseGame>();
    }

    // Update is called once per frame
    void LateUpdate() {
        if (pauseGame.GetPaused()) return;

        float interpolation = speed * Time.fixedDeltaTime;
        Vector3 position = playerCam.transform.position;
        Vector3 behindTarget = new Vector3(player.transform.position.x, 0, player.transform.position.z - 10); //- new Vector3(2 * player.transform.position.z);

        position.x = Mathf.Lerp(transform.position.x, targetGroup.position.x, interpolation);
        position.z = Mathf.Lerp(behindTarget.z, targetGroup.position.z, interpolation);

        playerCam.transform.position = position;

        
        //Quaternion toRotation = Quaternion.LookRotation(targetGroup.position);
        playerCam.transform.LookAt(targetGroup.position);
        //playerCam.transform.rotation = Quaternion.Euler(new Vector3(Mathf.Lerp(fromRotation.x, toRotation.x, interpolation), 0, 0));
    }

    //IEnumerator WaitForSpawn() {
    //    yield return new WaitForEndOfFrame();
    //    startCamera = true;
    //}

    //private void CameraPosition() {
    //    Vector3 playerPos = new Vector3(player.transform.position.x, 0, player.transform.position.z);
    //    Vector3 mousePos = playerCam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Camera.main.nearClipPlane, Input.mousePosition.z));
    //    float distance = Vector3.Distance(playerPos, mousePos);
    //    //Debug.Log(mousePos);

    //    if (distance > radius) {
    //        Vector3 distanceFromPlayer = new Vector3(mousePos.x - playerPos.x, 0, mousePos.y - playerPos.y);
    //        distanceFromPlayer *= radius / distance;
    //        //Debug.Log(distanceFromPlayer);
    //        cameraPos = new Vector3(distanceFromPlayer.x + playerPos.x, 0, distanceFromPlayer.y + playerPos.y);
    //    } else {
    //        cameraPos = new Vector3(mousePos.x, 0, mousePos.y);
    //    }

    //    transform.position = cameraPos;
    //}
}
