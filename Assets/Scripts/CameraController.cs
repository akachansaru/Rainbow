using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // TODO: add recenter button incase the view gets totally wacky
    public static CameraController cameraController;

    public float rotateSpeed = 2f;
    public float zoomSpeed = 0.5f;
    public int cameraMoveDistance = 7;

    // For iTween camera movement
    public iTween.EaseType easeType;
    public float lookTime = 1f;
    public float time = 1f;

    public static bool rotating = false;

    private GameObject _selectedCube;
    private Vector2 initialClickPosition;
    private float maxZoom = 20f;
    private float minZoom = 6f;

    void Awake()
    {
        cameraController = this;
    }

    void Start()
    {
        transform.LookAt(_selectedCube.transform.position);
    }

    public void MoveCamera(GameObject selectedCube, Vector3 selectedPosition)
    {
        // TODO: make the selected cube appear in the middle of the screen when the camera movement is done
        Vector3 moveTo = (selectedCube.transform.position + (selectedPosition - selectedCube.transform.position) * cameraMoveDistance);
        iTween.MoveTo(gameObject, iTween.Hash("position", moveTo, "looktarget", selectedCube.transform, "looktime", lookTime,
            "time", time, "easetype", easeType, "name", "AutoCamera"));
        _selectedCube = selectedCube;
    }

    /// <summary>
    /// For touch devices. Follows normal touch zoom: touches moving towards each other zooms out, touches moving away zooms in. 
    /// Camera must be in perspective projection.
    /// </summary>
    void PinchToZoom(Touch touchZero, Touch touchOne)
    {
        Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
        Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

        float touchDeltaMagPrev = (touchZeroPrevPos - touchOnePrevPos).magnitude;
        float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;
        float deltaMagnitudeDiff = touchDeltaMag - touchDeltaMagPrev;

        // Restrict the camera's zoom so it can't go too far or too close.
        RaycastHit hitInfo;
        Ray ray = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(ray, out hitInfo, 100))
        {
            if (hitInfo.distance > minZoom && hitInfo.distance < maxZoom)
            {
                print("Zooming");
                transform.localPosition += transform.forward * deltaMagnitudeDiff * zoomSpeed * Time.deltaTime;
            }
            else if (hitInfo.distance <= minZoom)
            {
                // Move the camera a little if it went over the bounds of the zoom
                transform.localPosition -= transform.forward * zoomSpeed * Time.deltaTime;
            }
            else if (hitInfo.distance >= maxZoom)
            {
                // Move the camera a little if it went over the bounds of the zoom
                transform.localPosition += transform.forward * zoomSpeed * Time.deltaTime;
            }
        }
    }

    /// <summary>
    /// For touch devices. One finger panning. Rotates around the current center.
    /// </summary>
    void PanView(Touch touch)
    {
        if (touch.phase == TouchPhase.Began)
        {
            //iTween.StopByName("AutoCamera");
            // TODO: when autoCamera is interupted there is a jump in the camera when panning starts
            initialClickPosition = touch.position;
        }
        if (touch.phase == TouchPhase.Moved)
        {
            rotating = true;
            Vector2 currentClickPosition = touch.position;
            Vector2 normalizedDelta = currentClickPosition - initialClickPosition;
            normalizedDelta.Normalize();

            // Translate the camera in the opposite direction to the swipe while looking at the selected cube.
            transform.Translate(-normalizedDelta * Time.deltaTime * rotateSpeed);
            transform.LookAt(_selectedCube.transform, transform.up); // Need transform.up otherwise it tries to orient the camera to world up.
        }
        if (touch.phase == TouchPhase.Ended)
        {
            rotating = false;
        }
    }

    void Update()
    {
#if UNITY_ANDROID
        if (!LevelManager.paused)
        {
            if (Input.touchCount == 1)
            {
                PanView(Input.GetTouch(0));
            }
            if (Input.touchCount == 2)
            {
                PinchToZoom(Input.GetTouch(0), Input.GetTouch(1));
            }
        }
        else if (LevelManager.paused && rotating)
        {
            rotating = false;
        }
#endif
    }
}
