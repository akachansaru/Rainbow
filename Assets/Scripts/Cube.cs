using UnityEngine;

public class Cube : MonoBehaviour {

    private Touch touch;

    public void OnMouseDown() {

    }

    void Update() {
        if (Input.touchCount > 0) {
            touch = Input.GetTouch(0);
            RaycastHit hitInfo;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hitInfo, 100) && hitInfo.transform.IsChildOf(transform)) {
                print("Hit " + hitInfo.transform.name + " at " + hitInfo.transform.position);
                SelectedPosition(hitInfo);
            }
        }

#if UNITY_EDITOR
        // When a cube on the board is clicked, HandManager is told that a cube from the hand can be chosen to play
        if (Input.GetMouseButtonUp(0)) {
            RaycastHit hitInfo;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hitInfo, 100) && hitInfo.transform.IsChildOf(transform)) {
                // Selected the location to place a cube from hand
                HandManager.handManager.ChooseCube(hitInfo.transform.parent.gameObject.GetComponent<MeshRenderer>().material, 
                    SelectedPosition(hitInfo));
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

    bool IsValidLocation() {
        return true;
    }
}
