using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandManager : MonoBehaviour {
    public int handSize;
    public float handSpacing = 0.5f;

    private Color[] hand;
    private float cubeSize;

    private void Start() {
        hand = CubeBank.cubeBank.DealCubes(handSize);
        cubeSize = CubeBank.cubePrefab.transform.localScale.x;
        CreateColoredCubes();
    }

    void CreateColoredCubes() {
        float currCubePos = -((float)handSize / 2f - 0.5f) * (cubeSize + handSpacing);
        Debug.Log("left" + currCubePos);
        Debug.Log("cubesize " + ((cubeSize + handSpacing)));
        // Creates colored cubes on the gameboard as children of Hand
        for (int i = 0; i < handSize; i++) {
            GameObject newCube = Instantiate(CubeBank.cubePrefab, transform);
            newCube.transform.localPosition = new Vector3(currCubePos, 0f, 0f);
            currCubePos += (cubeSize + handSpacing);
            Material newColor;
            ColorManager.colorToMaterial.TryGetValue(hand[i], out newColor);
            newCube.GetComponent<MeshRenderer>().material = newColor;
        }
    }
}
