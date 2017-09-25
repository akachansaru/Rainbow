using System.Collections;
using UnityEngine;

public class Cube : MonoBehaviour {

    private Touch touch;
    bool hitCube = false; // TODO: what is hitCube for? Double tapping?

    void Update() {
#if UNITY_ANDROID
        // When a cube on the board is tapped, HandManager is told that a cube from the hand can be chosen to play
        if (HandManager.handManager.PlayerTurn && Input.touchCount == 1) {
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit hitInfo;
            // Got rid of double tapping to select
            if (!CameraController.cameraController.IsMoving() && Physics.Raycast(ray, out hitInfo, 500) && hitInfo.transform.IsChildOf(transform) && Input.GetTouch(0).tapCount == 1 && !hitCube) {
                hitCube = true;
                Debug.DrawRay(ray.origin, ray.direction * 500, UnityEngine.Color.blue, 4f);
                // Selected the location to place a cube from hand
                HandManager.handManager.ChooseCube(GetComponent<MeshRenderer>().material, SelectedPosition(hitInfo));
                //TODO: See if camera can go to "smart" position 
                CameraController.cameraController.MoveCamera(gameObject, SelectedPosition(hitInfo));
            } else if (!Physics.Raycast(ray, out hitInfo, 500)) {
                Debug.DrawRay(ray.origin, ray.direction * 500, UnityEngine.Color.red, 2f);
                hitCube = false;
            }
        }
#endif
    }

    Vector3 SelectedPosition(RaycastHit hitInfo) {
        Face hitFace = hitInfo.transform.gameObject.GetComponent<CubeFace>().face;
        switch (hitFace) {
            case Face.XPos:
            return new Vector3(hitInfo.transform.position.x + 0.5f,
                hitInfo.transform.position.y,
                hitInfo.transform.position.z);
            case Face.XNeg:
            return new Vector3(hitInfo.transform.position.x - 0.5f,
                hitInfo.transform.position.y,
                hitInfo.transform.position.z);
            case Face.YPos:
            return new Vector3(hitInfo.transform.position.x,
                hitInfo.transform.position.y + 0.5f,
                hitInfo.transform.position.z);
            case Face.YNeg:
            return new Vector3(hitInfo.transform.position.x,
                hitInfo.transform.position.y - 0.5f,
                hitInfo.transform.position.z);
            case Face.ZPos:
            return new Vector3(hitInfo.transform.position.x,
                hitInfo.transform.position.y,
                hitInfo.transform.position.z + 0.5f);
            case Face.Zneg:
            return new Vector3(hitInfo.transform.position.x,
                hitInfo.transform.position.y,
                hitInfo.transform.position.z - 0.5f);
            default:
            Debug.Log("Invalid face");
            return Vector3.zero;
        }
    }

    void DoOnPlaced(int cubeScore) {
        GetComponent<AudioSource>().Play();
        ShowCubeScore(cubeScore);
    }

    void ShowCubeScore(int cubeScore) {
        GameObject text = Instantiate(Resources.Load("Prefabs/AmountScoredText"), transform) as GameObject;
        text.transform.LookAt(Camera.main.transform);
        text.transform.localPosition = Vector3.up * 1.5f;
        text.GetComponent<TextMesh>().text = "+" + cubeScore;
        StartCoroutine(HideCubeScore(text));
    }

    IEnumerator HideCubeScore(GameObject text) {
        yield return new WaitForSeconds(1f);
        Destroy(text);
    }
}
