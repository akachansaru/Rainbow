using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandManager : MonoBehaviour {

    public static HandManager handManager;

    public int handSize = 3;
    public int drawNum = 1;
    public float percentGray = 0.2f;
    public GameObject overlayCanvas;

    private List<GameObject> hand = new List<GameObject>();
    /// <summary>
    /// Positions in 3D space of the centers of potential cube position.
    /// </summary>
    private List<Vector3> emptySpaces = new List<Vector3>();
    private bool choosing = false;
    private Vector3 chosenBoardPosition;
    private float handSpacing = 0.5f;
    private float handCubeMoveSpeed = 1f;
    private float grayCubeMoveSpeed = 0.8f;
    private bool playerTurn = true;
    private GameObject placeHolder;

    public bool PlayerTurn {
        get { return playerTurn; }
    }

    public int remainingHandSize {
        get { return hand.Count; }
    }

    void Awake() {
        handManager = this;
        placeHolder = Instantiate(Resources.Load("Prefabs/PlaceHolder")) as GameObject;
        placeHolder.SetActive(false);
    }

    void Start() {
        NewGame();
    }
    
    public void NewGame() {
        CubeBank.cubeBank.PopulateNewGameColors();
        ScoreManager.score = 0;
        GameObject startingCube = Instantiate(CubeBank.cubePrefab);
        startingCube.tag = "Cube";
        startingCube.GetComponent<Cube>().enabled = true;
        UpdateEmptySpaces(startingCube.transform.position);
        LevelManager.paused = false;
        FillHand(handSize);
    }

    void Update() {
#if UNITY_EDITOR
        if (!LevelManager.paused && choosing && playerTurn && Input.GetMouseButtonUp(0)) {
            Debug.Log("Select cube from hand");
            RaycastHit hitInfo;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hitInfo, 100) && hitInfo.transform.parent.CompareTag("Valid")) {
                StartCoroutine(UpdateBoard(hitInfo.transform.parent.gameObject));
            }
        }
#endif
    }

    IEnumerator UpdateBoard(GameObject cube) {
        playerTurn = false;
        PlaceCube(cube);
        UpdateEmptySpaces(chosenBoardPosition);
        PlaceGrayCubes(percentGray);
        List<Ray> rays;
        int cubeScore = ScoreManager.scoreManager.CalculateScore(cube, DetectNeighbors(chosenBoardPosition, out rays), rays);
        StartCoroutine(ShowCubeScore(cube, cubeScore));
        yield return new WaitForSeconds(handCubeMoveSpeed + grayCubeMoveSpeed);
        FillHand(drawNum);
        if (OutOfMoves()) {
            ScoreManager.scoreManager.OutOfMoves = true;
        } else {
            playerTurn = true;
        }
    }

    bool OutOfMoves() {
        foreach (Vector3 space in emptySpaces) {
            bool canBePlaced = false;
            foreach (GameObject cube in hand) {
                canBePlaced = false;
                print("# neighbors to empty space " + space + ": " + DetectNeighbors(space).Count);
                foreach (GameObject neighbor in DetectNeighbors(space)) {
                    if (!ColorManager.colorManager.InSequence(cube, neighbor)) {
                        canBePlaced = false;
                        print("Cube " + cube.GetComponent<MeshRenderer>().material.name + " doesn't work next to " + neighbor.GetComponent<MeshRenderer>().material.name);
                        break;
                    } else {
                        canBePlaced = true;
                        print("Cube " + cube.GetComponent<MeshRenderer>().material.name + " works next to " + neighbor.GetComponent<MeshRenderer>().material.name);
                    }
                }
                // If even one cube from the hand can be placed the player is not out of moves.
                // If the cube in hand that was just checked can't be placed continue with the loop and go onto the next cube.
                // If I need the cube and the place it can be played, I can get it here.
                if (canBePlaced) {
                    print("There's still a move at " + space + " for " + cube.GetComponent<MeshRenderer>().material.name);
                    return false;
                } else {
                    print("No place at " + space + " for " + cube.GetComponent<MeshRenderer>().material.name);
                }
            }
        }
        // If all of the loops complete there are no valid places and the player is out of moves.
        print("Out of moves.");
        return true;
    }

    /// <summary>
    /// Removes the space that a new cube was just placed in and adds all of the empty spaces around that new cube.
    /// </summary>
    /// <param name="usedSpace"></param>
    void UpdateEmptySpaces(Vector3 usedSpace) {
        emptySpaces.Remove(usedSpace);
        foreach (Ray ray in DirectionalRays(usedSpace)) {
            if (!Physics.Raycast(ray) && !emptySpaces.Contains(usedSpace + ray.direction)) {
                emptySpaces.Add(usedSpace + ray.direction);
            }
        }
    }

    void PlaceGrayCubes(float percent) {
        int numGray = Mathf.RoundToInt(percent * emptySpaces.Count);
        for (int i = 0; i < numGray; i++) {
            int rand = Random.Range(0, emptySpaces.Count);
            Vector3 randomPosition = emptySpaces[rand];
            UpdateEmptySpaces(randomPosition);
            GameObject grayCube = Instantiate(CubeBank.cubePrefab);
            grayCube.GetComponent<MeshRenderer>().material = CubeBank.grayMaterial;
            iTween.MoveTo(grayCube, iTween.Hash("position", randomPosition, "time", grayCubeMoveSpeed, "delay", handCubeMoveSpeed));
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

    void PlaceCube(GameObject cube) {
        placeHolder.SetActive(false);
        cube.transform.parent = null;
        cube.transform.rotation = Quaternion.identity;
        iTween.MoveTo(cube.gameObject, iTween.Hash("position", chosenBoardPosition, "time", handCubeMoveSpeed));
        cube.tag = "Cube";
        cube.GetComponent<Cube>().enabled = true;
        hand.Remove(cube);
        choosing = false;
    }

    IEnumerator ShowCubeScore(GameObject cube, int cubeScore) {
        yield return new WaitForSeconds(handCubeMoveSpeed - handCubeMoveSpeed * 0.2f);
        GameObject text = Instantiate(Resources.Load("Prefabs/AmountScoredText"), overlayCanvas.transform) as GameObject;
        text.transform.localPosition = Camera.main.WorldToScreenPoint(cube.transform.position);
        text.GetComponent<Text>().text = "+" + cubeScore;
        StartCoroutine(HideCubeScore(text));
    }

    IEnumerator HideCubeScore(GameObject text) {
        yield return new WaitForSeconds(1f);
        Destroy(text);
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
    /// Returns an array of rays in each 6 directions from the specified origin.
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

    /// <summary>
    /// Send out rays in all 6 directions from position with a distance of 1
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    List<GameObject> DetectNeighbors(Vector3 position) {
        List<Ray> hitRays;
        return DetectNeighbors(position, out hitRays);
    }

    /// <summary>
    /// Send out rays in all 6 directions from position with a distance of 1
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    List<GameObject> DetectNeighbors(Vector3 position, out List<Ray> hitRays) {
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

    /// <summary>
    /// Checks each cube in hand against the selected board cube to see if any can be placed there. 
    /// Activates all cubes in the hand that in color sequence with the board cube.
    /// </summary>
    /// <param name="positionToPlace"></param>
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
        placeHolder.transform.position = positionToPlace;
        placeHolder.SetActive(true);
        ActivateValidCubes(positionToPlace);
        choosing = true;
        chosenBoardPosition = positionToPlace;
        Debug.Log("ChooseCube");
    }
}
