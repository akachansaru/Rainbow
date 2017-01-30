﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandManager : MonoBehaviour {

    public static HandManager handManager;
    public int score = 0;

    public int handSize = 3;
    public float handSpacing = 0.5f;
    public int drawNum = 1;

    private List<GameObject> hand = new List<GameObject>();
    private bool choosing = false;
    private Vector3 chosenBoardPosition;

    private void Awake() {
        handManager = this;
    }

    private void Start() {
        FillHand(handSize);
    }

    private void Update() {
#if UNITY_EDITOR
        if (choosing && Input.GetMouseButtonUp(0)) {
            Debug.Log("Select cube from hand");
            ChooseCubeToPlay();
        }
#endif
    }

    void ChooseCubeToPlay() {
        RaycastHit hitInfo;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hitInfo, 100) && hitInfo.transform.parent.CompareTag("Valid")) {
            // Selected a cube in the hand
            Transform cube = hitInfo.transform.parent;
            PlaceCube(cube);
            CalculateScore(cube.gameObject, chosenBoardPosition);
            FillHand(drawNum);
        }
    }

    void CalculateScore(GameObject cube, Vector3 finalPosition) {
        List<Ray> rays;
        List<GameObject> neighbors = DetectNeighbors(finalPosition, out rays);
        for (int i = 0; i < rays.Count; i++) {
            score += ScoreChain(rays[i], cube, neighbors[i], 1, 1);
            print("score: " + score);
        }
    }

    int ScoreChain(Ray ray, GameObject firstCube, GameObject secondCube, int chainScore, int chainNumber) {
        ray.origin += ray.direction;
        print("chainNumber: " + chainNumber + " chainScore: " + chainScore);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, 1) && ColorManager.colorManager.InStrictSequence(firstCube, secondCube, hitInfo.transform.parent.gameObject)) {
            chainNumber++;
            chainScore += chainNumber;
            return ScoreChain(ray, secondCube, hitInfo.transform.parent.gameObject, chainScore, chainNumber);
        } else {
            return chainScore;
        }
    }

    void FillHand(int newCubes) {
        List<Color> newColors = CubeBank.cubeBank.DealColors(newCubes);
        DealCubes(newCubes, newColors);
        //List<Color> handColors = new List<Color>();
        //handColors.AddRange(newColors);
        SortHand();
    }

    void SortHand() {
        ColorComparer comparer = new ColorComparer();
        hand.Sort(comparer);
        ReorderCubes();
    }

    void ReorderCubes() {
        float currCubePos = -((float)handSize / 2f - 0.5f) * (CubeBank.cubeSize + handSpacing);
        for (int i = 0; i < handSize; i++) {
            GameObject currCube = hand[i] as GameObject;
            iTween.MoveTo(currCube, iTween.Hash("position", new Vector3(currCubePos, 0f, 0f), "islocal", true, "time", 1f));
            currCube.transform.localRotation = Quaternion.identity;
            currCubePos += (CubeBank.cubeSize + handSpacing);
        }
    }

    void PlaceCube(Transform cube) {
        cube.parent = null;
        cube.rotation = Quaternion.identity;
        iTween.MoveTo(cube.gameObject, iTween.Hash("position", chosenBoardPosition, "time", 1.5f));
        cube.tag = "Cube";
        cube.gameObject.GetComponent<Cube>().enabled = true;
        hand.Remove(cube.gameObject);
        choosing = false;
        Debug.Log("Moving cube from hand");
    }

    /// <summary>
    /// Creates colored cubes on the gameboard as children of Hand giving them the colors 
    /// dealt out by CubeBank.DealColors(int numCubes).
    /// </summary>
    void DealCubes(int numCubes, List<Color> newColors) {
        float currCubePos = -((float)numCubes / 2f - 0.5f) * (CubeBank.cubeSize + handSpacing);
        for (int i = 0; i < numCubes; i++) {
            GameObject newCube = Instantiate(CubeBank.cubePrefab, transform);
            newCube.transform.localPosition = new Vector3(currCubePos, 0f, 0f);
            string newMaterial = ColorManager.colorToMaterial.GetByFirst(newColors[i]);
            newCube.GetComponent<MeshRenderer>().material = Resources.Load(ColorManager.materialPath + newMaterial) as Material;
            newCube.tag = "Hand";
            newCube.GetComponent<Cube>().enabled = false;
            hand.Add(newCube);
            currCubePos += (CubeBank.cubeSize + handSpacing);
        }
    }

    List<GameObject> DetectNeighbors(Vector3 position) {
        // Send out rays in all 6 directions with a distance of 1
        List<GameObject> neighbors = new List<GameObject>();
        RaycastHit hitInfo;
        Ray[] rays = new Ray[] {
        new Ray(position, Vector3.left),
        new Ray(position, Vector3.right),
        new Ray(position, Vector3.up),
        new Ray(position, Vector3.down),
        new Ray(position, Vector3.back),
        new Ray(position, Vector3.forward)
        };

        foreach (Ray ray in rays) {
            if (Physics.Raycast(ray, out hitInfo, CubeBank.cubeSize)) {
                neighbors.Add(hitInfo.transform.parent.gameObject);
            }
        }
        return neighbors;
    }

    List<GameObject> DetectNeighbors(Vector3 position, out List<Ray> hitRays) {
        // Send out rays in all 6 directions with a distance of 1
        List<GameObject> neighbors = new List<GameObject>();
        RaycastHit hitInfo;
        hitRays = new List<Ray>();
        Ray[] rays = new Ray[] {
        new Ray(position, Vector3.left),
        new Ray(position, Vector3.right),
        new Ray(position, Vector3.up),
        new Ray(position, Vector3.down),
        new Ray(position, Vector3.back),
        new Ray(position, Vector3.forward)
        };
        print("position" + position);
        foreach (Ray ray in rays) {
            if (Physics.Raycast(ray, out hitInfo, CubeBank.cubeSize)) {
                neighbors.Add(hitInfo.transform.parent.gameObject);
                hitRays.Add(ray);
            }
        }
        return neighbors;
    }

    void LightUpValidCubes(Vector3 positionToPlace) {
        List<GameObject> neighbors = DetectNeighbors(positionToPlace);

        foreach (GameObject cube in hand) {
            DeactivateCube(cube);
            bool valid = false;
            foreach (GameObject neighbor in neighbors) {
                if (!ColorManager.colorManager.InSequence(cube, neighbor)) {
                    valid = false;
                    break;
                } else {
                    valid = true;
                }
            }
            if (valid) {
                ActivateCube(cube);
            }
        }
    }

    void DeactivateCube(GameObject cube) {
        cube.tag = "Hand";
    }

    void ActivateCube(GameObject cube) {
        cube.tag = "Valid";
        // Light up cube
    }

    public void ChooseCube(Material colorOfNeighbor, Vector3 positionToPlace) {
        LightUpValidCubes(positionToPlace);
        choosing = true;
        chosenBoardPosition = positionToPlace;
        Debug.Log("ChooseCube");
    }
}
