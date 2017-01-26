using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandManager : MonoBehaviour {

    public static HandManager handManager;
    public int handSize;
    public float handSpacing = 0.5f;

    private Color[] handColors;
    private ArrayList hand;
    private bool choosing = false;
    private Vector3 chosenBoardPosition;

    private void Awake() {
        handManager = this;
    }

    private void Start() {
        handColors = CubeBank.cubeBank.DealCubes(handSize);
        hand = new ArrayList();
        CreateHandOfColoredCubes();
    }

    private void Update() {
#if UNITY_EDITOR
        if (choosing && Input.GetMouseButtonUp(0)) {
            Debug.Log("Select cube from hand");
            RaycastHit hitInfo;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hitInfo, 100) && hitInfo.transform.parent.CompareTag("Valid")) { // Change tag to "Valid"
                // Selected a cube in the hand
                print("Hit " + hitInfo.transform.tag + " at " + hitInfo.transform.position);
                Transform cube = hitInfo.transform.parent;
                PlaceCube(cube);
            }
        }
#endif
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

    void CreateHandOfColoredCubes() {
        float currCubePos = -((float)handSize / 2f - 0.5f) * (CubeBank.cubeSize + handSpacing);
        // Creates colored cubes on the gameboard as children of Hand
        for (int i = 0; i < handSize; i++) {
            GameObject newCube = Instantiate(CubeBank.cubePrefab, transform);
            newCube.transform.localPosition = new Vector3(currCubePos, 0f, 0f);
            UnityEngine.Color newColor = ColorManager.colorToMaterial.GetByFirst(handColors[i]);
            newCube.GetComponent<MeshRenderer>().material.color = newColor;
            newCube.tag = "Hand";
            newCube.GetComponent<Cube>().enabled = false;
            hand.Add(newCube);
            currCubePos += (CubeBank.cubeSize + handSpacing);
        }
    }

    ArrayList DetectNeighbors(Vector3 position) {
        // Send out rays in all 6 directions with a distance of 1
        ArrayList neighbors = new ArrayList();
        RaycastHit hitInfo;
        Ray[] rays = new Ray[] {
        new Ray(position, Vector3.left),
        new Ray(position, Vector3.right),
        new Ray(position, Vector3.up),
        new Ray(position, Vector3.down),
        new Ray(position, Vector3.back),
        new Ray(position, Vector3.forward)
        };

        for (int i = 0; i < 6; i++) {
            if (Physics.Raycast(rays[i], out hitInfo, CubeBank.cubeSize)) {
                neighbors.Add(hitInfo.transform.parent.gameObject);
            }
        }
        return neighbors;
    }

    void LightUpValidCubes(Vector3 positionToPlace) {
        ArrayList neighbors = DetectNeighbors(positionToPlace);

        foreach (GameObject cube in hand) {
            DeactivateCube(cube);
            bool valid = false;
            foreach (GameObject neighbor in neighbors) {
                Debug.Log("Material: " + cube.GetComponent<MeshRenderer>().material.color);
                if (!ColorManager.colorManager.InSequence(ColorManager.colorToMaterial.GetBySecond(cube.GetComponent<MeshRenderer>().material.color),
                    ColorManager.colorToMaterial.GetBySecond(neighbor.GetComponent<MeshRenderer>().material.color))) {
                    valid = false;
                    break;
                } else {
                    valid = true;
                }
            }
            if (valid) {
                ActivateCube(cube);
                Debug.Log("Valid placement");
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
