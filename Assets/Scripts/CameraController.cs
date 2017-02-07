using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    public static CameraController cameraController;

    public float rotateSpeed = 2f;

    private GameObject selectedCube;
    private Vector3 selectedPosition;
    private bool rotating = false;
    private Vector3 initialClickPosition;

    public GameObject SelectedCube {
        set {
            selectedCube = value;
            MoveCamera();
        }
    }

    public Vector3 SelectedPosition {
        set {
            selectedPosition = value;
        }
    }

    void Awake() {
        cameraController = this;
    }

    void Start() {
        transform.LookAt(selectedCube.transform.position);
        
    }

    void MoveCamera() {
        //print("selected position: " + selectedPosition);
        //print("direction: " + (selectedPosition - selectedCube.transform.position) * 7);
        //print("distance: " + (transform.position - selectedCube.transform.position + (selectedPosition - selectedCube.transform.position)));
        //print("moving camera to: " + (selectedCube.transform.position + (selectedPosition - selectedCube.transform.position) * 7)
        //    + " from: " + transform.position);
        Vector3 moveTo = (selectedCube.transform.position + (selectedPosition - selectedCube.transform.position) * 7);
        iTween.MoveTo(gameObject, iTween.Hash("position", moveTo, "looktarget", selectedCube.transform, "time", 2f, "name", "AutoCamera"));
    }

    void Update () {
        if (!LevelManager.paused) {
            if (Input.GetMouseButtonDown(0)) {
                iTween.StopByName("AutoCamera");
                rotating = true;
                initialClickPosition = Input.mousePosition;
            }
            if (rotating) {
                Vector3 currentClickPosition = Input.mousePosition;
                Vector3 normalizedDelta = initialClickPosition - currentClickPosition;
                normalizedDelta.Normalize();
                transform.Translate(normalizedDelta * Time.deltaTime * rotateSpeed);
                iTween.LookUpdate(gameObject, selectedCube.transform.position, 0.5f);
                if (Input.GetMouseButtonUp(0)) {
                    rotating = false;
                }
            }
        } else if (LevelManager.paused && rotating) {
            rotating = false;
        }
    }
}
