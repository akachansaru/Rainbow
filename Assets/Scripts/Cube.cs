﻿using System.Collections;
using System.Collections.Generic;
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
        if (Input.GetMouseButtonUp(0)) {
            RaycastHit hitInfo;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hitInfo, 100) && hitInfo.transform.IsChildOf(transform)) {
                // Selected the location to place a cube from hand
                print("Hit " + hitInfo.transform.name + " at " + hitInfo.transform.position);
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
            //Instantiate(CubeBank.cubePrefab,
            //    new Vector3(hitInfo.transform.position.x + 0.5f,
            //    hitInfo.transform.position.y,
            //    hitInfo.transform.position.z),
            //    Quaternion.identity);
            return new Vector3(hitInfo.transform.position.x + 0.5f,
                hitInfo.transform.position.y,
                hitInfo.transform.position.z);
            break;
            case Face.XNeg:
            //Instantiate(CubeBank.cubePrefab,
            //    new Vector3(hitInfo.transform.position.x - 0.5f,
            //    hitInfo.transform.position.y,
            //    hitInfo.transform.position.z),
            //    Quaternion.identity);
            return new Vector3(hitInfo.transform.position.x - 0.5f,
                hitInfo.transform.position.y,
                hitInfo.transform.position.z);
            break;
            case Face.YPos:
            //Instantiate(CubeBank.cubePrefab,
            //    new Vector3(hitInfo.transform.position.x,
            //    hitInfo.transform.position.y + 0.5f,
            //    hitInfo.transform.position.z),
            //    Quaternion.identity);
            return new Vector3(hitInfo.transform.position.x,
                hitInfo.transform.position.y + 0.5f,
                hitInfo.transform.position.z);
            break;
            case Face.YNeg:
            //Instantiate(CubeBank.cubePrefab,
            //    new Vector3(hitInfo.transform.position.x,
            //    hitInfo.transform.position.y - 0.5f,
            //    hitInfo.transform.position.z),
            //    Quaternion.identity);
            return new Vector3(hitInfo.transform.position.x,
                hitInfo.transform.position.y - 0.5f,
                hitInfo.transform.position.z);
            break;
            case Face.ZPos:
            //Instantiate(CubeBank.cubePrefab,
            //    new Vector3(hitInfo.transform.position.x,
            //    hitInfo.transform.position.y,
            //    hitInfo.transform.position.z + 0.5f),
            //    Quaternion.identity);
            return new Vector3(hitInfo.transform.position.x,
                hitInfo.transform.position.y,
                hitInfo.transform.position.z + 0.5f);
            break;
            case Face.Zneg:
            //Instantiate(CubeBank.cubePrefab,
            //    new Vector3(hitInfo.transform.position.x,
            //    hitInfo.transform.position.y,
            //    hitInfo.transform.position.z - 0.5f),
            //    Quaternion.identity);
            return new Vector3(hitInfo.transform.position.x,
                hitInfo.transform.position.y,
                hitInfo.transform.position.z - 0.5f);
            break;
            default:
            Debug.Log("Invalid face");
            return Vector3.zero;
            break;
        }
    }

    bool IsValidLocation() {
        return true;
    }
}
