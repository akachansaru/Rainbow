using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    public static CameraController cameraController;

    public float rotateSpeed = 2f;
    public float zoomSpeed = 0.5f;
    public int cameraMoveDistance = 7;

    private GameObject _selectedCube;
    //private Vector3 selectedPosition;
    public static bool rotating = false;
    private Vector2 initialClickPosition;
    private float maxZoom = 20f;
    private float minZoom = 6f;

    void Awake() {
        cameraController = this;
    }

    void Start() {
        transform.LookAt(_selectedCube.transform.position);
    }

    // TODO: See if camera can go to "smart" position 
    public void MoveCamera(GameObject selectedCube, Vector3 selectedPosition) {
        Debug.Log("Old cam pos: " + transform.position);
        Debug.Log("Camera up: " + transform.up);
        Vector3 moveTo = (selectedCube.transform.position + (selectedPosition - selectedCube.transform.position) * cameraMoveDistance);
        Debug.Log("New cam pos: " + moveTo);
        Debug.Log("Camera up: " + transform.up);
        iTween.MoveTo(gameObject, iTween.Hash("position", moveTo, "looktarget", selectedCube.transform, "looktime", 1.25f,
            "time", 1.25f, "easetype", "linear", "name", "AutoCamera"));
        _selectedCube = selectedCube;
    }

    /// <summary>
    /// For touch devices. Follows normal touch zoom: touches moving towards each other zooms out, touches moving away zooms in. 
    /// Camera must be in perspective projection.
    /// </summary>
    void PinchToZoom(Touch touchZero, Touch touchOne) {
        Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
        Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

        float touchDeltaMagPrev = (touchZeroPrevPos - touchOnePrevPos).magnitude;
        float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;
        float deltaMagnitudeDiff = touchDeltaMag - touchDeltaMagPrev;

        //GetComponent<Camera>().fieldOfView += deltaMagnitudeDiff * zoomSpeed;
        //GetComponent<Camera>().fieldOfView = Mathf.Clamp(GetComponent<Camera>().fieldOfView, 40f, 120f);

        // Restrict the camera's zoom so it can't go too far or too close.
        RaycastHit hitInfo;
        Ray ray = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(ray, out hitInfo, 100)) {
            if (hitInfo.distance > minZoom && hitInfo.distance < maxZoom) {
                print("Zooming");
                transform.localPosition += transform.forward * deltaMagnitudeDiff * zoomSpeed * Time.deltaTime;
            } else if (hitInfo.distance <= minZoom) {
                // Move the camera a little if it went over the bounds of the zoom
                transform.localPosition -= transform.forward * zoomSpeed * Time.deltaTime;
            } else if (hitInfo.distance >= maxZoom) {
                // Move the camera a little if it went over the bounds of the zoom
                transform.localPosition += transform.forward * zoomSpeed * Time.deltaTime;
            }
        }
    }

    //private float oldAngle = 0f;

    void TwistToRotateZ() {
        //Touch touchZero = Input.GetTouch(0);
        //Touch touchOne = Input.GetTouch(1);

        //float deltaMagnitudeDiff = touchDeltaMagPrev - touchDeltaMag;
        //transform.rotation.z += deltaMagnitudeDiff * zoomSpeed;

        //Vector2 v2 = Input.GetTouch(0).position - Input.GetTouch(1).position;
        //float newAngle = Mathf.Atan2(v2.y, v2.x);
        //float deltaAngle = Mathf.DeltaAngle(newAngle, oldAngle);
        //oldAngle = newAngle;
        //transform.Rotate(transform.forward, deltaAngle);
    }

    //void LateUpdate() {
    //    float pinchAmount = 0;
    //    Quaternion desiredRotation = transform.rotation;

    //    DetectTouchMovement.Calculate();

    //    if (Mathf.Abs(DetectTouchMovement.pinchDistanceDelta) > 0) { // zoom
    //        pinchAmount = DetectTouchMovement.pinchDistanceDelta;
    //    }

    //    if (Mathf.Abs(DetectTouchMovement.turnAngleDelta) > 0) { // rotate
    //        Vector3 rotationDeg = Vector3.zero;
    //        rotationDeg.z = -DetectTouchMovement.turnAngleDelta;
    //        desiredRotation *= Quaternion.Euler(rotationDeg);
    //    }


    //    // not so sure those will work:
    //    transform.rotation = desiredRotation;
    //    transform.position += Vector3.forward * pinchAmount;
    //}

    void PanView(Touch touch) {
        if (touch.phase == TouchPhase.Began) {
            iTween.StopByName("AutoCamera");
            initialClickPosition = touch.position;
        }
        if (touch.phase == TouchPhase.Moved) {
            rotating = true;
            Vector2 currentClickPosition = touch.position;
            Vector2 normalizedDelta = currentClickPosition - initialClickPosition;
            normalizedDelta.Normalize();

            // Translate the camera in the opposite direction to the swipe.
            transform.Translate(-normalizedDelta * Time.deltaTime * rotateSpeed);
            transform.LookAt(_selectedCube.transform, transform.up); // Need transform.up otherwise it tries to orient the camera to world up.
        }
        if (touch.phase == TouchPhase.Ended) {
            rotating = false;
        }
    }

    //void PanViewTwoFinger(Touch touch0, Touch touch1) {
    //    if (touch0.phase == TouchPhase.Began || touch0.phase == TouchPhase.Began) {
    //        //iTween.StopByName("AutoCamera");
    //        //rotating = true;
    //        initialClickPosition = touch.position;
    //    }
    //    if (touch.phase == TouchPhase.Moved) {
    //        Vector2 currentClickPosition = touch.position;
    //        Vector2 normalizedDelta = currentClickPosition - initialClickPosition;
    //        normalizedDelta.Normalize();

    //        // Translate the camera in the opposite direction to the swipe.
    //        transform.Translate(-normalizedDelta * Time.deltaTime * rotateSpeed);
    //        transform.LookAt(selectedCube.transform, transform.up); // Need transform.up otherwise it tries to orient the camera to world up.
    //        //iTween.LookUpdate(gameObject, selectedCube.transform.position, 0.5f);
    //    }
    //}

    void Update() {
#if UNITY_ANDROID
        if (!LevelManager.paused) {
            if (Input.touchCount == 1) {
                PanView(Input.GetTouch(0));
            }
            if (Input.touchCount == 2) {
                PinchToZoom(Input.GetTouch(0), Input.GetTouch(1));
                TwistToRotateZ();
            }
        } else if (LevelManager.paused && rotating) {
            rotating = false;
        }
    }
#endif
}
