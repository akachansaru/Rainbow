using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour {

    private Touch touch;

	// Use this for initialization
	void Start () {
		
	}

    public void OnMouseDown() {

    }

    // Update is called once per frame
    void Update () {
        if (Input.touchCount > 0) {
            touch = Input.GetTouch(0);
            RaycastHit hitInfo;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hitInfo, 100)) {
                print("Hit " + hitInfo.transform.name);
            }
        }

#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0)) {
            RaycastHit hitInfo;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hitInfo, 100) && hitInfo.transform.IsChildOf(transform)) {
                print("Hit " + hitInfo.transform.name + " at " + hitInfo.transform.position);
                Face hitFace = hitInfo.transform.gameObject.GetComponent<CubeFace>().face;
                GameObject prefab = Resources.Load("Prefabs/CubeWhite") as GameObject;
                #region switch (hitFace)
                switch (hitFace) {
                    case Face.XPos:
                        Instantiate(prefab, 
                            new Vector3(hitInfo.transform.position.x + 0.5f,
                            hitInfo.transform.position.y, 
                            hitInfo.transform.position.z), 
                            Quaternion.identity);
                    break;
                    case Face.XNeg:
                    Instantiate(prefab,
                        new Vector3(hitInfo.transform.position.x - 0.5f,
                        hitInfo.transform.position.y,
                        hitInfo.transform.position.z),
                        Quaternion.identity);
                    break;
                    case Face.YPos:
                    Instantiate(prefab,
                        new Vector3(hitInfo.transform.position.x,
                        hitInfo.transform.position.y + 0.5f,
                        hitInfo.transform.position.z),
                        Quaternion.identity);
                    break;
                    case Face.YNeg:
                    Instantiate(prefab,
                        new Vector3(hitInfo.transform.position.x,
                        hitInfo.transform.position.y - 0.5f,
                        hitInfo.transform.position.z),
                        Quaternion.identity);
                    break;
                    case Face.ZPos:
                    Instantiate(prefab,
                        new Vector3(hitInfo.transform.position.x,
                        hitInfo.transform.position.y,
                        hitInfo.transform.position.z + 0.5f),
                        Quaternion.identity);
                    break;
                    case Face.Zneg:
                    Instantiate(prefab,
                        new Vector3(hitInfo.transform.position.x,
                        hitInfo.transform.position.y,
                        hitInfo.transform.position.z - 0.5f),
                        Quaternion.identity);
                    break;
                    default:
                        Debug.Log("Invalid face");
                    break;
                }
                #endregion
            }
        }
        
#endif
}

    bool IsValidLocation() {
        return true;
    }
}
