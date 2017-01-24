using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableCube : MonoBehaviour, IBeginDragHandler, IDragHandler {

    private bool inHand;
    private Vector3 dist;
    private float posX;
    private float posY;

	// Use this for initialization
	void Start () {
        Debug.Log("gameObject initial pos = " + transform.localPosition);
    }
	
	// Update is called once per frame
	void Update () {
        //Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.y);
        //Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        //iTween.MoveUpdate(gameObject, iTween.Hash("position", worldPos, "time", .6));
    }

    bool IsValidLocation() {
        return true;
    }

    public void OnBeginDrag(PointerEventData eventData) {
        dist = Camera.main.ScreenToWorldPoint(transform.position);
        posX = eventData.position.x - dist.x;
        posY = eventData.position.y - dist.y;
    }

    public void OnDrag(PointerEventData eventData) {
        Vector3 currPos = new Vector3(eventData.position.x, eventData.position.y, dist.z);
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(currPos);
        transform.position = new Vector3(-worldPos.x, -worldPos.y, 0);
    }
}
