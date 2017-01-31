using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandManager : MonoBehaviour {

    public static HandManager handManager;

    public int handSize = 3;
    public int drawNum = 1;
    public float percentGray = 0.2f;
    public GameObject startingCube;

    private List<GameObject> hand = new List<GameObject>();
    /// <summary>
    /// Positions in 3D space of the centers of potential cube position.
    /// </summary>
    private List<Vector3> emptySpaces = new List<Vector3>();
    private bool choosing = false;
    private Vector3 chosenBoardPosition;
    private float handSpacing = 0.5f;

    public int remainingHandSize {
        get { return hand.Count; }
    }

    private void Awake() {
        handManager = this;
    }

    private void Start() {
        UpdateEmptySpaces(startingCube.transform.position);
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
            UpdateEmptySpaces(chosenBoardPosition);
            PlaceGrayCubes(percentGray);
            List<Ray> rays;
            List<GameObject> neighbors = DetectNeighbors(chosenBoardPosition, out rays);
            ScoreManager.scoreManager.CalculateScore(cube.gameObject, neighbors, rays);
            FillHand(drawNum);
        }
    }

    /// <summary>
    /// Removes the space that a new cube was just placed in and adds all of the empty spaces around that new cube.
    /// </summary>
    /// <param name="usedSpace"></param>
    void UpdateEmptySpaces(Vector3 usedSpace) {
        emptySpaces.Remove(usedSpace);
        foreach (Ray ray in DirectionalRays(usedSpace)) {
            if (!Physics.Raycast(ray)) {
                emptySpaces.Add(usedSpace + ray.direction);
            }
        }
    }

    void PlaceGrayCubes(float percent) {
        int numGray = Mathf.RoundToInt(percent * emptySpaces.Count);
        for (int i = 0; i < numGray; i++) {
            int rand = Random.Range(0, emptySpaces.Count);
            Vector3 randomPosition = emptySpaces[rand];
            emptySpaces.Remove(randomPosition);
            GameObject grayCube = Instantiate(CubeBank.cubePrefab);
            grayCube.GetComponent<MeshRenderer>().material = Resources.Load(ColorManager.materialPath + "Gray") as Material;
            iTween.MoveTo(grayCube, randomPosition, 1.5f);
        }
    }

    void FillHand(int newCubes) {
        List<Color> newColors;
        if (CubeBank.cubeBank.DealColors(newCubes, out newColors)) {
            DealCubes(newCubes, newColors);
            SortHand();
        } else {
            print("Out of cubes.");
        }
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

    /// <summary>
    /// Returns an array of rays in each 6 directions with a distance of 1 from the specified origin.
    /// </summary>
    /// <param name="origin"></param>
    /// <returns></returns>
    Ray[] DirectionalRays(Vector3 origin) {
        return new Ray[] {
        new Ray(origin, Vector3.left),
        new Ray(origin, Vector3.right),
        new Ray(origin, Vector3.up),
        new Ray(origin, Vector3.down),
        new Ray(origin, Vector3.back),
        new Ray(origin, Vector3.forward)
        };
    }

    List<GameObject> DetectNeighbors(Vector3 position) {
        // Send out rays in all 6 directions with a distance of 1
        List<GameObject> neighbors = new List<GameObject>();
        RaycastHit hitInfo;
        foreach (Ray ray in DirectionalRays(position)) {
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
        foreach (Ray ray in DirectionalRays(position)) {
            if (Physics.Raycast(ray, out hitInfo, CubeBank.cubeSize)) {
                neighbors.Add(hitInfo.transform.parent.gameObject);
                hitRays.Add(ray);
            }
        }
        return neighbors;
    }

    void ActivateValidCubes(Vector3 positionToPlace) {
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
        ActivateValidCubes(positionToPlace);
        choosing = true;
        chosenBoardPosition = positionToPlace;
        Debug.Log("ChooseCube");
    }
}
