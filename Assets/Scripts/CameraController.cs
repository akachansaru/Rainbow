using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    public float rotateSpeed = 2f;

    private bool rotating = false;
    private Vector3 initialClickPosition;

	void Update () {
        if (Input.GetMouseButtonDown(0)) {
            Debug.Log("Starting rotation");
            rotating = true;
            initialClickPosition = Input.mousePosition;
        }
        if (rotating) {
            Vector3 currentClickPosition = Input.mousePosition;
            Vector3 normalizedDelta = new Vector3(initialClickPosition.x - currentClickPosition.x,
                initialClickPosition.y - currentClickPosition.y,
                initialClickPosition.z - currentClickPosition.z);
            normalizedDelta.Normalize();

            transform.LookAt(Vector3.zero);
            transform.Translate(normalizedDelta * Time.deltaTime * rotateSpeed);

            //iTween.LookUpdate(gameObject, Vector3.zero, rotateSpeed);
            //iTween.MoveUpdate(gameObject, normalizedDelta, rotateSpeed);

            if (Input.GetMouseButtonUp(0)) {
                rotating = false;
                Debug.Log("Ending rotation");
            }
        }
    }
}
